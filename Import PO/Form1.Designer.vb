<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmImport
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
        Me.KryptonPanel1 = New ComponentFactory.Krypton.Toolkit.KryptonPanel()
        Me.cmdValidate = New ComponentFactory.Krypton.Toolkit.KryptonButton()
        Me.KryptonLabel1 = New ComponentFactory.Krypton.Toolkit.KryptonLabel()
        Me.KryptonHeader1 = New ComponentFactory.Krypton.Toolkit.KryptonHeader()
        Me.cmdClose = New ComponentFactory.Krypton.Toolkit.KryptonButton()
        Me.cmdBrowse = New ComponentFactory.Krypton.Toolkit.KryptonButton()
        Me.txtPath = New ComponentFactory.Krypton.Toolkit.KryptonTextBox()
        Me.cmdPost = New ComponentFactory.Krypton.Toolkit.KryptonButton()
        CType(Me.KryptonPanel1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.KryptonPanel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'KryptonPanel1
        '
        Me.KryptonPanel1.Controls.Add(Me.cmdValidate)
        Me.KryptonPanel1.Controls.Add(Me.KryptonLabel1)
        Me.KryptonPanel1.Controls.Add(Me.KryptonHeader1)
        Me.KryptonPanel1.Controls.Add(Me.cmdClose)
        Me.KryptonPanel1.Controls.Add(Me.cmdBrowse)
        Me.KryptonPanel1.Controls.Add(Me.txtPath)
        Me.KryptonPanel1.Controls.Add(Me.cmdPost)
        Me.KryptonPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.KryptonPanel1.Location = New System.Drawing.Point(0, 0)
        Me.KryptonPanel1.Name = "KryptonPanel1"
        Me.KryptonPanel1.Size = New System.Drawing.Size(350, 209)
        Me.KryptonPanel1.TabIndex = 0
        '
        'cmdValidate
        '
        Me.cmdValidate.Location = New System.Drawing.Point(57, 94)
        Me.cmdValidate.Name = "cmdValidate"
        Me.cmdValidate.Size = New System.Drawing.Size(90, 25)
        Me.cmdValidate.TabIndex = 19
        Me.cmdValidate.Values.Text = "Validate File"
        '
        'KryptonLabel1
        '
        Me.KryptonLabel1.Location = New System.Drawing.Point(3, 52)
        Me.KryptonLabel1.Name = "KryptonLabel1"
        Me.KryptonLabel1.Size = New System.Drawing.Size(57, 20)
        Me.KryptonLabel1.TabIndex = 17
        Me.KryptonLabel1.Values.Text = "File Path"
        '
        'KryptonHeader1
        '
        Me.KryptonHeader1.Location = New System.Drawing.Point(3, 3)
        Me.KryptonHeader1.Name = "KryptonHeader1"
        Me.KryptonHeader1.Size = New System.Drawing.Size(229, 31)
        Me.KryptonHeader1.TabIndex = 16
        Me.KryptonHeader1.Values.Description = ""
        Me.KryptonHeader1.Values.Heading = "Import Purchase Order"
        '
        'cmdClose
        '
        Me.cmdClose.Location = New System.Drawing.Point(269, 172)
        Me.cmdClose.Name = "cmdClose"
        Me.cmdClose.Size = New System.Drawing.Size(71, 25)
        Me.cmdClose.TabIndex = 15
        Me.cmdClose.Values.Text = "Close"
        '
        'cmdBrowse
        '
        Me.cmdBrowse.Location = New System.Drawing.Point(310, 49)
        Me.cmdBrowse.Name = "cmdBrowse"
        Me.cmdBrowse.Size = New System.Drawing.Size(30, 25)
        Me.cmdBrowse.TabIndex = 11
        Me.cmdBrowse.Values.Text = "..."
        '
        'txtPath
        '
        Me.txtPath.Location = New System.Drawing.Point(57, 52)
        Me.txtPath.Name = "txtPath"
        Me.txtPath.Size = New System.Drawing.Size(247, 20)
        Me.txtPath.TabIndex = 10
        '
        'cmdPost
        '
        Me.cmdPost.Location = New System.Drawing.Point(269, 94)
        Me.cmdPost.Name = "cmdPost"
        Me.cmdPost.Size = New System.Drawing.Size(71, 25)
        Me.cmdPost.TabIndex = 9
        Me.cmdPost.Values.Text = "Post "
        '
        'frmImport
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(350, 209)
        Me.Controls.Add(Me.KryptonPanel1)
        Me.Name = "frmImport"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Import PO"
        CType(Me.KryptonPanel1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.KryptonPanel1.ResumeLayout(False)
        Me.KryptonPanel1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents KryptonPanel1 As ComponentFactory.Krypton.Toolkit.KryptonPanel
    Friend WithEvents KryptonLabel1 As ComponentFactory.Krypton.Toolkit.KryptonLabel
    Friend WithEvents KryptonHeader1 As ComponentFactory.Krypton.Toolkit.KryptonHeader
    Friend WithEvents cmdClose As ComponentFactory.Krypton.Toolkit.KryptonButton
    Friend WithEvents cmdBrowse As ComponentFactory.Krypton.Toolkit.KryptonButton
    Friend WithEvents txtPath As ComponentFactory.Krypton.Toolkit.KryptonTextBox
    Friend WithEvents cmdPost As ComponentFactory.Krypton.Toolkit.KryptonButton
    Friend WithEvents cmdValidate As ComponentFactory.Krypton.Toolkit.KryptonButton

End Class
