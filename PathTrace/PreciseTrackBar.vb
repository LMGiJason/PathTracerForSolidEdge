
Public Class PreciseTrackBar
    Inherits TrackBar
    Public Event FloatValueChanged(sender As Object, e As FloatValuesEventArg)
    Private mFloatVal As Double = 0
    Private mFloatRange As Double = 0
    Private mFloatValue As Double
    Private mInitValue As Double
    Private mMinValue As Double
    Private mIncValue As Double
    Private mSuspendValueChanged As Boolean

    Public Sub Initialize(initVal As Double, limit As Double)
        mSuspendValueChanged = True
        'TrackBar min must be < max
        mInitValue = initVal
        If limit > initVal Then
            mFloatRange = limit - initVal
            mMinValue = initVal
            Me.Value = Me.Minimum
        Else
            mFloatRange = initVal - limit
            mMinValue = limit
            Me.Value = Me.Maximum
        End If
        mIncValue = mFloatRange / Me.Maximum
        mSuspendValueChanged = False
    End Sub

    Public Sub SetToMax()
        mSuspendValueChanged = True
        Me.Value = Me.Maximum
        mSuspendValueChanged = False
    End Sub

    Private Sub PreciseTrackBar_ValueChanged(sender As Object, e As EventArgs) Handles Me.ValueChanged
        If mSuspendValueChanged Then Return

        mFloatVal = mMinValue + (Me.Value * mIncValue)
        RaiseEvent FloatValueChanged(Me, New FloatValuesEventArg(mFloatVal, Me.Value))
    End Sub
End Class

Public Class FloatValuesEventArg
    Inherits EventArgs
    Sub New(fval As Double, ival As Integer)
        Me.FloatVal = fval
        Me.IntVal = ival
    End Sub
    Property FloatVal As Double
    Property IntVal As Integer
End Class