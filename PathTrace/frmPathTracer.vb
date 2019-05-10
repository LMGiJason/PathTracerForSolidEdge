'Written by Jason Titcomb while working for LMGi www.tlmgi.com
'This code is provided AS IS and is not meant to be a reference of any kind.
Public Class frmPathTrace
    Private WithEvents mProcessor As New PathProcessor
    Private mInitialStringValue As String
    Private mInitialValue As Double

    Private Sub frmPathTrace_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        My.Settings.LastVar = cboVarsOrDims.SelectedItem
        mProcessor.FinalCleanup()
    End Sub

    Private Sub frmPathTrace_Load(sender As Object, e As EventArgs) Handles Me.Load
        Me.cboVarsOrDims.Items.Clear()
        If mProcessor.InitEdge() Then
            If cboVarsOrDims.Items.Count > 0 Then
                If My.Settings.LastFile = mProcessor.FileName Then
                    Dim idx = cboVarsOrDims.FindStringExact(My.Settings.LastVar)
                    If idx >= 0 Then
                        cboVarsOrDims.SelectedIndex = idx
                    End If
                End If
            End If
            My.Settings.LastFile = mProcessor.FileName
        Else
            btnProcess.Enabled = False
        End If
    End Sub

    Private Sub mProcessor_OnMsg(sender As Object, e As MsgEventArg) Handles mProcessor.OnMsg
        lblStatus.Text = e.Msg
    End Sub

    Private Sub mProcessor_OnVar(sender As Object, e As MsgEventArg) Handles mProcessor.OnVarOrDim
        cboVarsOrDims.Items.Add(e.Msg)
    End Sub

    Private Sub cboVars_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboVarsOrDims.SelectedIndexChanged
        mInitialStringValue = mProcessor.InitVarOrDimToDrive(Me.cboVarsOrDims.SelectedItem)
        mInitialValue = mProcessor.InitialValue
        SetSliderRange()
    End Sub

    Private Sub txtInputs_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtLimit.KeyPress, txtIncrement.KeyPress, txtMinDist.KeyPress
        If (Not e.KeyChar = ChrW(Keys.Back) And ("-0123456789.").IndexOf(e.KeyChar) = -1) Then
            e.Handled = True
        End If
    End Sub

    Private Sub txtLimit_TextChanged(sender As Object, e As EventArgs) Handles txtLimit.TextChanged, txtMinDist.TextChanged
        SetSliderRange()
    End Sub

    Private Sub SetSliderRange()
        Try
            Dim precision As Integer = 1
            Dim limit, range, inc As Double
            btnProcess.Enabled = False
            preciseTrackBar.Enabled = False

            If txtLimit.TextLength > 0 Then
                limit = Double.Parse(txtLimit.Text)
            Else
                lblStatus.Text = "Limit is required."
                Exit Sub
            End If

            If limit = mInitialValue Then
                lblStatus.Text = "Limit cannot = initial value."
                Exit Sub
            End If

            If txtIncrement.TextLength > 0 Then
                inc = Double.Parse(txtIncrement.Text)
            Else
                lblStatus.Text = "Increment is required."
                Exit Sub
            End If

            'TrackBar min must be < max
            If limit > mInitialValue Then
                range = limit - mInitialValue
                If Math.Sign(inc) <= 0 Then
                    lblStatus.Text = "Increment must be positive if max > initial."
                    Exit Sub
                End If
            Else
                range = mInitialValue - limit
                If Math.Sign(inc) >= 0 Then
                    lblStatus.Text = "Increment must be negative if max < initial."
                    Exit Sub
                End If
            End If
            btnProcess.Enabled = True
            preciseTrackBar.Enabled = True
            preciseTrackBar.Initialize(mInitialValue, limit)
            lblStatus.Text = mInitialStringValue & " to " & txtLimit.Text

        Catch ex As Exception
            lblStatus.Text = ex.Message
        End Try
    End Sub

    Private Sub txtIncrement_TextChanged(sender As Object, e As EventArgs) Handles txtIncrement.TextChanged
        SetSliderRange()
    End Sub

    Private Sub btnProcess_Click(sender As Object, e As EventArgs) Handles btnProcess.Click
        If mProcessor.CreateTrace(txtIncrement.Text, txtLimit.Text, txtMinDist.Text) Then
            preciseTrackBar.SetToMax()
        End If
    End Sub

    Private Sub mProcessor_OnVarChanged(sender As Object, e As MsgEventArg) Handles mProcessor.OnVarChanged
        lblStatus.Text = e.Msg
    End Sub

    Private Sub preciseTrackBar_FloatValueChanged(sender As Object, e As FloatValuesEventArg) Handles preciseTrackBar.FloatValueChanged
        mProcessor.SetVarValFromString(e.FloatVal)
    End Sub

    Private Sub frmPathTrace_KeyUp(sender As Object, e As KeyEventArgs) Handles Me.KeyUp
        If e.KeyCode = Keys.Escape Then
            Me.Close()
        End If
    End Sub
End Class
