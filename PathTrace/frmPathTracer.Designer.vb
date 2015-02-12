<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmPathTrace
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmPathTrace))
        Me.statusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.lblStatus = New System.Windows.Forms.ToolStripStatusLabel()
        Me.btnProcess = New System.Windows.Forms.Button()
        Me.label3 = New System.Windows.Forms.Label()
        Me.label2 = New System.Windows.Forms.Label()
        Me.label1 = New System.Windows.Forms.Label()
        Me.cboVarsOrDims = New System.Windows.Forms.ComboBox()
        Me.txtLimit = New System.Windows.Forms.TextBox()
        Me.txtIncrement = New System.Windows.Forms.TextBox()
        Me.preciseTrackBar = New PathTrace.PreciseTrackBar()
        Me.statusStrip1.SuspendLayout()
        CType(Me.preciseTrackBar, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'statusStrip1
        '
        Me.statusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.lblStatus})
        Me.statusStrip1.Location = New System.Drawing.Point(0, 177)
        Me.statusStrip1.Name = "statusStrip1"
        Me.statusStrip1.Size = New System.Drawing.Size(453, 25)
        Me.statusStrip1.SizingGrip = False
        Me.statusStrip1.TabIndex = 15
        Me.statusStrip1.Text = "statusStrip1"
        '
        'lblStatus
        '
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.Size = New System.Drawing.Size(50, 20)
        Me.lblStatus.Text = "Ready"
        '
        'btnProcess
        '
        Me.btnProcess.Location = New System.Drawing.Point(342, 134)
        Me.btnProcess.Name = "btnProcess"
        Me.btnProcess.Size = New System.Drawing.Size(83, 29)
        Me.btnProcess.TabIndex = 14
        Me.btnProcess.Text = "Process"
        Me.btnProcess.UseVisualStyleBackColor = True
        '
        'label3
        '
        Me.label3.AutoSize = True
        Me.label3.Location = New System.Drawing.Point(325, 10)
        Me.label3.Name = "label3"
        Me.label3.Size = New System.Drawing.Size(37, 17)
        Me.label3.TabIndex = 13
        Me.label3.Text = "Limit"
        '
        'label2
        '
        Me.label2.AutoSize = True
        Me.label2.Location = New System.Drawing.Point(216, 10)
        Me.label2.Name = "label2"
        Me.label2.Size = New System.Drawing.Size(70, 17)
        Me.label2.TabIndex = 12
        Me.label2.Text = "Increment"
        '
        'label1
        '
        Me.label1.AutoSize = True
        Me.label1.Location = New System.Drawing.Point(9, 10)
        Me.label1.Name = "label1"
        Me.label1.Size = New System.Drawing.Size(138, 17)
        Me.label1.TabIndex = 11
        Me.label1.Text = "Variable / Dimension"
        '
        'cboVarsOrDims
        '
        Me.cboVarsOrDims.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboVarsOrDims.FormattingEnabled = True
        Me.cboVarsOrDims.Location = New System.Drawing.Point(12, 32)
        Me.cboVarsOrDims.Name = "cboVarsOrDims"
        Me.cboVarsOrDims.Size = New System.Drawing.Size(167, 24)
        Me.cboVarsOrDims.TabIndex = 8
        '
        'txtLimit
        '
        Me.txtLimit.DataBindings.Add(New System.Windows.Forms.Binding("Text", Global.PathTrace.My.MySettings.Default, "Limit", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.txtLimit.Location = New System.Drawing.Point(325, 33)
        Me.txtLimit.Name = "txtLimit"
        Me.txtLimit.Size = New System.Drawing.Size(100, 22)
        Me.txtLimit.TabIndex = 10
        Me.txtLimit.Text = Global.PathTrace.My.MySettings.Default.Limit
        '
        'txtIncrement
        '
        Me.txtIncrement.DataBindings.Add(New System.Windows.Forms.Binding("Text", Global.PathTrace.My.MySettings.Default, "Increment", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.txtIncrement.Location = New System.Drawing.Point(219, 33)
        Me.txtIncrement.Name = "txtIncrement"
        Me.txtIncrement.Size = New System.Drawing.Size(100, 22)
        Me.txtIncrement.TabIndex = 9
        Me.txtIncrement.Text = Global.PathTrace.My.MySettings.Default.Increment
        '
        'preciseTrackBar
        '
        Me.preciseTrackBar.LargeChange = 1
        Me.preciseTrackBar.Location = New System.Drawing.Point(11, 72)
        Me.preciseTrackBar.Maximum = 100
        Me.preciseTrackBar.Name = "preciseTrackBar"
        Me.preciseTrackBar.Size = New System.Drawing.Size(414, 56)
        Me.preciseTrackBar.TabIndex = 17
        Me.preciseTrackBar.TickFrequency = 10
        '
        'frmPathTrace
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(453, 202)
        Me.Controls.Add(Me.preciseTrackBar)
        Me.Controls.Add(Me.statusStrip1)
        Me.Controls.Add(Me.btnProcess)
        Me.Controls.Add(Me.label3)
        Me.Controls.Add(Me.label2)
        Me.Controls.Add(Me.label1)
        Me.Controls.Add(Me.txtLimit)
        Me.Controls.Add(Me.txtIncrement)
        Me.Controls.Add(Me.cboVarsOrDims)
        Me.DataBindings.Add(New System.Windows.Forms.Binding("Location", Global.PathTrace.My.MySettings.Default, "FormPosition", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.Location = Global.PathTrace.My.MySettings.Default.FormPosition
        Me.Name = "frmPathTrace"
        Me.Text = "Path Tracer for Solid Edge "
        Me.TopMost = True
        Me.statusStrip1.ResumeLayout(False)
        Me.statusStrip1.PerformLayout()
        CType(Me.preciseTrackBar, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Private WithEvents statusStrip1 As System.Windows.Forms.StatusStrip
    Private WithEvents lblStatus As System.Windows.Forms.ToolStripStatusLabel
    Private WithEvents btnProcess As System.Windows.Forms.Button
    Private WithEvents label3 As System.Windows.Forms.Label
    Private WithEvents label2 As System.Windows.Forms.Label
    Private WithEvents label1 As System.Windows.Forms.Label
    Private WithEvents txtLimit As System.Windows.Forms.TextBox
    Private WithEvents txtIncrement As System.Windows.Forms.TextBox
    Private WithEvents cboVarsOrDims As System.Windows.Forms.ComboBox
    Friend WithEvents preciseTrackBar As PreciseTrackBar

End Class