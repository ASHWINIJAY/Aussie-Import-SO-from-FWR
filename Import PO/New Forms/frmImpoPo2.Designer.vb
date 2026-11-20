<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmImportPo2
    Inherits DevExpress.XtraEditors.XtraForm

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.btnValidate = New DevExpress.XtraEditors.SimpleButton()
        Me.btnPost = New DevExpress.XtraEditors.SimpleButton()
        Me.GroupControl2 = New DevExpress.XtraEditors.GroupControl()
        Me.txtCompPass = New DevExpress.XtraEditors.TextEdit()
        Me.LabelControl4 = New DevExpress.XtraEditors.LabelControl()
        Me.LabelControl2 = New DevExpress.XtraEditors.LabelControl()
        Me.txtComp = New DevExpress.XtraEditors.TextEdit()
        Me.GroupControl3 = New DevExpress.XtraEditors.GroupControl()
        Me.LabelControl3 = New DevExpress.XtraEditors.LabelControl()
        Me.LabelControl5 = New DevExpress.XtraEditors.LabelControl()
        Me.txtUser = New DevExpress.XtraEditors.TextEdit()
        Me.txtPass = New DevExpress.XtraEditors.TextEdit()
        Me.lblVersion = New DevExpress.XtraEditors.LabelControl()
        Me.GroupControl4 = New DevExpress.XtraEditors.GroupControl()
        Me.btnDuplicate = New DevExpress.XtraEditors.SimpleButton()
        Me.SimpleButton2 = New DevExpress.XtraEditors.SimpleButton()
        Me.btnChangeStatus = New DevExpress.XtraEditors.SimpleButton()
        Me.btnCallAPI = New DevExpress.XtraEditors.SimpleButton()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.SimplebtnButton1 = New DevExpress.XtraEditors.SimpleButton()
        CType(Me.GroupControl2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupControl2.SuspendLayout()
        CType(Me.txtCompPass.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtComp.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GroupControl3, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupControl3.SuspendLayout()
        CType(Me.txtUser.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtPass.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GroupControl4, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupControl4.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'btnValidate
        '
        Me.btnValidate.Enabled = False
        Me.btnValidate.Location = New System.Drawing.Point(13, 536)
        Me.btnValidate.Margin = New System.Windows.Forms.Padding(4)
        Me.btnValidate.Name = "btnValidate"
        Me.btnValidate.Size = New System.Drawing.Size(255, 34)
        Me.btnValidate.TabIndex = 3
        Me.btnValidate.Text = "Validate"
        '
        'btnPost
        '
        Me.btnPost.Location = New System.Drawing.Point(276, 536)
        Me.btnPost.Margin = New System.Windows.Forms.Padding(4)
        Me.btnPost.Name = "btnPost"
        Me.btnPost.Size = New System.Drawing.Size(250, 34)
        Me.btnPost.TabIndex = 4
        Me.btnPost.Text = "Post"
        '
        'GroupControl2
        '
        Me.GroupControl2.Controls.Add(Me.txtCompPass)
        Me.GroupControl2.Controls.Add(Me.LabelControl4)
        Me.GroupControl2.Controls.Add(Me.LabelControl2)
        Me.GroupControl2.Controls.Add(Me.txtComp)
        Me.GroupControl2.Location = New System.Drawing.Point(8, 290)
        Me.GroupControl2.Margin = New System.Windows.Forms.Padding(4)
        Me.GroupControl2.Name = "GroupControl2"
        Me.GroupControl2.Size = New System.Drawing.Size(536, 150)
        Me.GroupControl2.TabIndex = 10
        Me.GroupControl2.Text = "Company details"
        '
        'txtCompPass
        '
        Me.txtCompPass.Location = New System.Drawing.Point(183, 93)
        Me.txtCompPass.Margin = New System.Windows.Forms.Padding(4)
        Me.txtCompPass.Name = "txtCompPass"
        Me.txtCompPass.Properties.UseSystemPasswordChar = True
        Me.txtCompPass.Size = New System.Drawing.Size(183, 26)
        Me.txtCompPass.TabIndex = 4
        '
        'LabelControl4
        '
        Me.LabelControl4.Location = New System.Drawing.Point(16, 98)
        Me.LabelControl4.Margin = New System.Windows.Forms.Padding(4)
        Me.LabelControl4.Name = "LabelControl4"
        Me.LabelControl4.Size = New System.Drawing.Size(94, 13)
        Me.LabelControl4.TabIndex = 3
        Me.LabelControl4.Text = "Company Password"
        '
        'LabelControl2
        '
        Me.LabelControl2.Location = New System.Drawing.Point(16, 58)
        Me.LabelControl2.Margin = New System.Windows.Forms.Padding(4)
        Me.LabelControl2.Name = "LabelControl2"
        Me.LabelControl2.Size = New System.Drawing.Size(81, 13)
        Me.LabelControl2.TabIndex = 3
        Me.LabelControl2.Text = "Syspro Company"
        '
        'txtComp
        '
        Me.txtComp.Location = New System.Drawing.Point(183, 54)
        Me.txtComp.Margin = New System.Windows.Forms.Padding(4)
        Me.txtComp.Name = "txtComp"
        Me.txtComp.Size = New System.Drawing.Size(45, 26)
        Me.txtComp.TabIndex = 0
        '
        'GroupControl3
        '
        Me.GroupControl3.Controls.Add(Me.LabelControl3)
        Me.GroupControl3.Controls.Add(Me.LabelControl5)
        Me.GroupControl3.Controls.Add(Me.txtUser)
        Me.GroupControl3.Controls.Add(Me.txtPass)
        Me.GroupControl3.Location = New System.Drawing.Point(8, 134)
        Me.GroupControl3.Margin = New System.Windows.Forms.Padding(4)
        Me.GroupControl3.Name = "GroupControl3"
        Me.GroupControl3.Size = New System.Drawing.Size(536, 147)
        Me.GroupControl3.TabIndex = 9
        Me.GroupControl3.Text = "User details"
        '
        'LabelControl3
        '
        Me.LabelControl3.Location = New System.Drawing.Point(16, 93)
        Me.LabelControl3.Margin = New System.Windows.Forms.Padding(4)
        Me.LabelControl3.Name = "LabelControl3"
        Me.LabelControl3.Size = New System.Drawing.Size(82, 13)
        Me.LabelControl3.TabIndex = 3
        Me.LabelControl3.Text = "Syspro Password"
        '
        'LabelControl5
        '
        Me.LabelControl5.Location = New System.Drawing.Point(16, 54)
        Me.LabelControl5.Margin = New System.Windows.Forms.Padding(4)
        Me.LabelControl5.Name = "LabelControl5"
        Me.LabelControl5.Size = New System.Drawing.Size(84, 13)
        Me.LabelControl5.TabIndex = 3
        Me.LabelControl5.Text = "Syspro Username"
        '
        'txtUser
        '
        Me.txtUser.Location = New System.Drawing.Point(183, 50)
        Me.txtUser.Margin = New System.Windows.Forms.Padding(4)
        Me.txtUser.Name = "txtUser"
        Me.txtUser.Size = New System.Drawing.Size(183, 26)
        Me.txtUser.TabIndex = 1
        '
        'txtPass
        '
        Me.txtPass.Location = New System.Drawing.Point(183, 88)
        Me.txtPass.Margin = New System.Windows.Forms.Padding(4)
        Me.txtPass.Name = "txtPass"
        Me.txtPass.Properties.UseSystemPasswordChar = True
        Me.txtPass.Size = New System.Drawing.Size(183, 26)
        Me.txtPass.TabIndex = 2
        '
        'lblVersion
        '
        Me.lblVersion.Appearance.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
        Me.lblVersion.Appearance.Options.UseFont = True
        Me.lblVersion.Location = New System.Drawing.Point(3, 918)
        Me.lblVersion.Margin = New System.Windows.Forms.Padding(4)
        Me.lblVersion.Name = "lblVersion"
        Me.lblVersion.Size = New System.Drawing.Size(98, 21)
        Me.lblVersion.TabIndex = 11
        Me.lblVersion.Text = "AppVersion"
        '
        'GroupControl4
        '
        Me.GroupControl4.Controls.Add(Me.btnDuplicate)
        Me.GroupControl4.Controls.Add(Me.SimpleButton2)
        Me.GroupControl4.Controls.Add(Me.btnChangeStatus)
        Me.GroupControl4.Controls.Add(Me.btnCallAPI)
        Me.GroupControl4.Controls.Add(Me.PictureBox1)
        Me.GroupControl4.Controls.Add(Me.GroupControl3)
        Me.GroupControl4.Controls.Add(Me.lblVersion)
        Me.GroupControl4.Controls.Add(Me.GroupControl2)
        Me.GroupControl4.Controls.Add(Me.btnValidate)
        Me.GroupControl4.Controls.Add(Me.btnPost)
        Me.GroupControl4.Controls.Add(Me.SimplebtnButton1)
        Me.GroupControl4.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GroupControl4.Location = New System.Drawing.Point(0, 0)
        Me.GroupControl4.Margin = New System.Windows.Forms.Padding(4)
        Me.GroupControl4.Name = "GroupControl4"
        Me.GroupControl4.Size = New System.Drawing.Size(538, 663)
        Me.GroupControl4.TabIndex = 12
        '
        'btnDuplicate
        '
        Me.btnDuplicate.Enabled = False
        Me.btnDuplicate.Location = New System.Drawing.Point(276, 482)
        Me.btnDuplicate.Margin = New System.Windows.Forms.Padding(4)
        Me.btnDuplicate.Name = "btnDuplicate"
        Me.btnDuplicate.Size = New System.Drawing.Size(249, 34)
        Me.btnDuplicate.TabIndex = 17
        Me.btnDuplicate.Text = "Check Duplicate PO(s)"
        '
        'SimpleButton2
        '
        Me.SimpleButton2.Enabled = False
        Me.SimpleButton2.Location = New System.Drawing.Point(276, 578)
        Me.SimpleButton2.Margin = New System.Windows.Forms.Padding(4)
        Me.SimpleButton2.Name = "SimpleButton2"
        Me.SimpleButton2.Size = New System.Drawing.Size(249, 34)
        Me.SimpleButton2.TabIndex = 16
        Me.SimpleButton2.Text = "Email Confirmation To Customer"
        '
        'btnChangeStatus
        '
        Me.btnChangeStatus.Enabled = False
        Me.btnChangeStatus.Location = New System.Drawing.Point(13, 578)
        Me.btnChangeStatus.Margin = New System.Windows.Forms.Padding(4)
        Me.btnChangeStatus.Name = "btnChangeStatus"
        Me.btnChangeStatus.Size = New System.Drawing.Size(255, 34)
        Me.btnChangeStatus.TabIndex = 15
        Me.btnChangeStatus.Text = "Change API status to Exported"
        '
        'btnCallAPI
        '
        Me.btnCallAPI.Location = New System.Drawing.Point(13, 482)
        Me.btnCallAPI.Margin = New System.Windows.Forms.Padding(4)
        Me.btnCallAPI.Name = "btnCallAPI"
        Me.btnCallAPI.Size = New System.Drawing.Size(255, 34)
        Me.btnCallAPI.TabIndex = 14
        Me.btnCallAPI.Text = "Get Orders For FWR Customer"
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = Global.Import_PO.My.Resources.Resources.AT_logo
        Me.PictureBox1.Location = New System.Drawing.Point(4, 34)
        Me.PictureBox1.Margin = New System.Windows.Forms.Padding(4)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(538, 90)
        Me.PictureBox1.TabIndex = 13
        Me.PictureBox1.TabStop = False
        '
        'SimplebtnButton1
        '
        Me.SimplebtnButton1.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.SimplebtnButton1.Location = New System.Drawing.Point(454, 623)
        Me.SimplebtnButton1.Margin = New System.Windows.Forms.Padding(4)
        Me.SimplebtnButton1.Name = "SimplebtnButton1"
        Me.SimplebtnButton1.Size = New System.Drawing.Size(71, 34)
        Me.SimplebtnButton1.TabIndex = 6
        Me.SimplebtnButton1.Text = "Close"
        '
        'frmImportPo2
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(144.0!, 144.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.CancelButton = Me.SimplebtnButton1
        Me.ClientSize = New System.Drawing.Size(538, 663)
        Me.ControlBox = False
        Me.Controls.Add(Me.GroupControl4)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
        Me.IsMdiContainer = True
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.MaximumSize = New System.Drawing.Size(548, 972)
        Me.MinimumSize = New System.Drawing.Size(369, 650)
        Me.Name = "frmImportPo2"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Import FWR"
        CType(Me.GroupControl2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupControl2.ResumeLayout(False)
        Me.GroupControl2.PerformLayout()
        CType(Me.txtCompPass.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtComp.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GroupControl3, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupControl3.ResumeLayout(False)
        Me.GroupControl3.PerformLayout()
        CType(Me.txtUser.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtPass.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GroupControl4, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupControl4.ResumeLayout(False)
        Me.GroupControl4.PerformLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents btnValidate As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents btnPost As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents GroupControl2 As DevExpress.XtraEditors.GroupControl
    Friend WithEvents txtCompPass As DevExpress.XtraEditors.TextEdit
    Friend WithEvents LabelControl4 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents LabelControl2 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents txtComp As DevExpress.XtraEditors.TextEdit
    Friend WithEvents GroupControl3 As DevExpress.XtraEditors.GroupControl
    Friend WithEvents LabelControl3 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents LabelControl5 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents txtUser As DevExpress.XtraEditors.TextEdit
    Friend WithEvents txtPass As DevExpress.XtraEditors.TextEdit
    Friend WithEvents lblVersion As DevExpress.XtraEditors.LabelControl
    Friend WithEvents GroupControl4 As DevExpress.XtraEditors.GroupControl
    Friend WithEvents PictureBox1 As PictureBox
    Friend WithEvents btnCallAPI As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents SimpleButton2 As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents btnChangeStatus As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents SimplebtnButton1 As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents btnDuplicate As DevExpress.XtraEditors.SimpleButton
End Class
