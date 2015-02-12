Imports SolidEdgeFramework
Imports System.Runtime.InteropServices
Imports SolidEdgeFrameworkSupport
Imports SolidEdgeConstants.UnitTypeConstants
Imports SolidEdgeConstants.ObjectType
Imports System.Runtime.InteropServices.ComTypes

Public Class PathPointList
    Public PathPoints As New List(Of Double)
End Class

Public Class MsgEventArg
    Inherits EventArgs
    Sub New(m As String)
        Me.Msg = m
    End Sub
    Property Msg As String
End Class

Public Class PathProcessor
    Implements ISECommandEvents

    Private mSolidApp As SolidEdgeFramework.Application
    Private mDraftDoc As SolidEdgeDraft.DraftDocument
    Private mSheet As SolidEdgeDraft.Sheet
    Private mPoints As Points2d
    Private mBsplines As BSplineCurves2d
    Private mBspline As BSplineCurve2d
    Private mUOM As UnitsOfMeasure = Nothing
    Private mVar As Object
    Private mVars As Variables
    Private mVarList As VariableList
    Private mUnitType As SolidEdgeFramework.UnitTypeConstants
    Private mPathPointLists As New List(Of PathPointList)
    Private mCommand As Command = Nothing

    Public Event OnMsg(sender As Object, e As MsgEventArg)
    Public Event OnVarOrDim(sender As Object, e As MsgEventArg)
    Public Event OnVarChanged(sender As Object, e As MsgEventArg)

    Public Function InitEdge() As Boolean
        Try
            Try
                mSolidApp = TryCast(Marshal.GetActiveObject("SolidEdge.Application"), SolidEdgeFramework.Application)
            Catch ex As Exception
                ReportStatus("Solid Edge is not running!")
                Return False
            End Try

            If mSolidApp.ActiveDocumentType <> DocumentTypeConstants.igDraftDocument Then
                ReportStatus("Drafting only!")
                Return False
            Else
                OleMessageFilter.Register()

                'Just so we get an undo mark
                mCommand = mSolidApp.CreateCommand(SolidEdgeConstants.seCmdFlag.seNoDeactivate)
                mCommand.Start()

                HookUnHookCommandEventHandlers(True)

                mDraftDoc = mSolidApp.ActiveDocument
                mUOM = mDraftDoc.UnitsOfMeasure
                mVars = mDraftDoc.Variables

                'Populate with variables and or dimensions
                mVarList = mVars.Query("*")
                For r As Integer = 1 To mVarList.Count
                    mVar = mVarList.Item(r)
                    If (mVar.Type = igDimension AndAlso mVar.constraint) Then 'Driving dimension
                        If mVar.Formula.ToString.Length = 0 Then
                            RaiseEvent OnVarOrDim(Me, New MsgEventArg(mVar.VariableTableName))
                        End If
                    ElseIf mVar.Type = igVariable Then
                        If mVar.Formula.ToString.Length = 0 Then
                            RaiseEvent OnVarOrDim(Me, New MsgEventArg(mVar.VariableTableName))
                        End If
                    End If
                    ReleaseRCW(mVar)
                Next
            End If
        Catch ex As Exception
            ReportStatus(ex.Message)
            Return False
        End Try
        Return True
    End Function

    Private mInitialValue As Double
    Public ReadOnly Property InitialValue() As Double
        Get
            Return mInitialValue
        End Get
    End Property

    Public Function CreateTrace(incStr As String, maxStr As String) As Boolean
        Dim inc As Double
        Dim max As Double
        Dim start As Double
        Dim cur As Double = start
        Dim tmr As New Stopwatch
        Dim pt As SolidEdgeFrameworkSupport.Point2d = Nothing
        Try

            mSheet = mDraftDoc.ActiveSheet
            mPathPointLists.Clear()
            mBsplines = mSheet.BsplineCurves2d
            mPoints = mSheet.Points2d
            inc = CDbl(mUOM.ParseUnit(mUnitType, incStr))
            max = CDbl(mUOM.ParseUnit(mUnitType, maxStr))
            start = mVar.value

            If mPoints.Count = 0 Then
                ReportStatus("No points found to track")
                Return False
            End If

            'A path list for each existing point
            For r As Integer = 1 To mPoints.Count
                pt = mPoints.Item(r)
                Dim path As New PathPointList
                path.PathPoints.Add(pt.x)
                path.PathPoints.Add(pt.y)
                mPathPointLists.Add(path)
                ReleaseRCW(pt)
            Next

            tmr.Start()
            Do
                If tmr.ElapsedMilliseconds > 10000 Then '10 seconds max
                    ReportStatus("Too many iterations")
                    Return False
                End If
                If max > start Then
                    cur += inc
                    If cur >= max Then Exit Do
                    mVar.Value = cur 'adjust var val by increment
                    AddPathPoint()
                Else
                    cur -= inc
                    If cur <= max Then Exit Do
                    mVar.Value = cur
                    AddPathPoint()
                End If
            Loop
            mVar.Value = max

            AddPathPoint()
            CreateCurves()
            Return True
        Catch ex As Exception
            ReportStatus(ex.Message)
            Return False
        Finally
            tmr.Stop()
        End Try
    End Function

    Private Sub AddPathPoint()
        Dim pt As SolidEdgeFrameworkSupport.Point2d = Nothing
        For r As Integer = 1 To mPoints.Count
            pt = mPoints.Item(r)
            Dim trace = mPathPointLists(r - 1)
            Dim tpc = trace.PathPoints.Count
            If tpc > 1 Then
                'Only if the new point is not close to the last point
                If Math.Abs(trace.PathPoints(tpc - 1) - pt.y) > 0.001 Then
                    If Math.Abs(trace.PathPoints(tpc - 2) - pt.x) > 0.001 Then
                        trace.PathPoints.Add(pt.x)
                        trace.PathPoints.Add(pt.y)
                    End If
                End If
            End If
            ReleaseRCW(pt)
        Next
    End Sub

    Private Sub CreateCurves()
        Dim pointStyle As SolidEdgeFrameworkSupport.GeometryStyle2d = Nothing
        Dim curveStyle As SolidEdgeFrameworkSupport.GeometryStyle2d = Nothing

        Dim ct As Integer
        For r As Integer = 1 To mPoints.Count
            Dim pt As SolidEdgeFrameworkSupport.Point2d = mPoints.Item(r)
            Dim trace = mPathPointLists(r - 1)
            ct = CInt(trace.PathPoints.Count / 2)
            mBspline = mBsplines.AddByPoints(4, ct, trace.PathPoints.ToArray())
            pointStyle = pt.Style
            curveStyle = mBspline.Style
            curveStyle.LinearColor = pointStyle.LinearColor
            ReleaseRCW(pt)
            ReleaseRCW(pointStyle)
            ReleaseRCW(curveStyle)
        Next

    End Sub

    Public Sub SetVarValFromString(ByVal stringValue As String)
        Try
            mVar.Value = mUOM.ParseUnit(mUnitType, stringValue)
            RaiseEvent OnVarChanged(Me, New MsgEventArg(mUOM.FormatUnit(mUnitType, mVar.value)))
        Catch ex As Exception
            ReportStatus(ex.Message)
        End Try
    End Sub

    Public Function InitVarOrDimToDrive(ByVal name As String) As String
        Try
            mVar = mVarList.Item(name)
            GetUnitType()
            mInitialValue = Val(mUOM.FormatUnit(mUnitType, mVar.Value))
            Return mUOM.FormatUnit(mUnitType, mVar.Value)
        Catch ex As Exception
            ReportStatus(ex.Message)
            Return String.Empty
        End Try

    End Function

    Private Sub GetUnitType()
        Dim objType As SolidEdgeConstants.ObjectType
        objType = mVar.Type
        If objType = igVariable Then
            mUnitType = mVar.UnitsType
        Else ' igVariable
            Select Case CType(mVar, Dimension).DimensionType
                Case DimTypeConstants.igDimTypeAngular, DimTypeConstants.igDimTypeAngularCoordinate, DimTypeConstants.igDimTypeArcAngle
                    mUnitType = UnitTypeConstants.igUnitAngle
                Case Else
                    mUnitType = UnitTypeConstants.igUnitDistance
            End Select
        End If
    End Sub

    Private Sub ReportStatus(msg As String)
        RaiseEvent OnMsg(Me, New MsgEventArg(msg))
    End Sub

    Public Sub FinalCleanup()
        HookUnHookCommandEventHandlers(False)
        ReleaseRCW(mVar)
        ReleaseRCW(mVars)
        ReleaseRCW(mVarList)
        ReleaseRCW(mUOM)
        ReleaseRCW(mDraftDoc)
        ReleaseRCW(mSolidApp)
        OleMessageFilter.Unregister()
    End Sub

    Private Sub ReleaseRCW(ByRef o As Object)
        If o IsNot Nothing Then
            Debug.Assert(0 = Marshal.ReleaseComObject(o))
            o = Nothing
        End If
    End Sub
#Region "Command Events"

    Public Sub Activate() Implements ISECommandEvents.Activate
    End Sub

    Public Sub Deactivate() Implements ISECommandEvents.Deactivate
    End Sub

    Public Sub Idle(lCount As Integer, ByRef pbMore As Boolean) Implements ISECommandEvents.Idle
    End Sub

    Public Sub KeyDown(ByRef KeyCode As Short, Shift As Short) Implements ISECommandEvents.KeyDown
    End Sub

    Public Sub KeyPress(ByRef KeyAscii As Short) Implements ISECommandEvents.KeyPress
    End Sub

    Public Sub KeyUp(ByRef KeyCode As Short, Shift As Short) Implements ISECommandEvents.KeyUp
    End Sub

    'This is all I really want from the command
    Public Sub Terminate() Implements ISECommandEvents.Terminate
        System.Windows.Forms.Application.Exit()
    End Sub


    Private Command_CP As IConnectionPoint
    Private Command_CP_Cookie As Integer

    Private Sub HookUnHookCommandEventHandlers(hook As Boolean)
        Dim i As Type = GetType(SolidEdgeFramework.ISECommandEvents)
        Dim EventGuid As Guid = i.GUID
        Command_CP_Cookie = -1
        HookUnhookEvents(Command_CP, hook, mCommand, Command_CP_Cookie, EventGuid)
    End Sub

    Private Sub HookUnhookEvents(ByRef CP As IConnectionPoint, ByVal Add As Boolean, ByVal obj As Object, ByRef Cookie As Integer, ByVal EventGuid As Guid)
        Dim CPC As IConnectionPointContainer

        CPC = obj

        If Not CPC Is Nothing Then

            If Add Then
                CPC.FindConnectionPoint(EventGuid, CP)
                If Not CP Is Nothing Then
                    CP.Advise(Me, Cookie)
                End If
            Else
                If Not CP Is Nothing Then
                    If Not Cookie = -1 Then
                        Try
                            CP.Unadvise(Cookie)
                            Marshal.ReleaseComObject(CP)
                        Catch ex As Exception
                            'swallow all exceptions on unadvise
                            'the host may not be available at this point
                            ' What kind of host leaves the party before the guests?
                        Finally
                            CP = Nothing
                            Cookie = -1
                        End Try
                    End If
                End If

            End If
        End If
    End Sub
#End Region

End Class
