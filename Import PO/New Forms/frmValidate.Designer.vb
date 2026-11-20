<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmValidate
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmValidate))
        Me.GroupControl1 = New DevExpress.XtraEditors.GroupControl()
        Me.ListBox1 = New System.Windows.Forms.ListBox()
        Me.lblErr = New DevExpress.XtraEditors.LabelControl()
        Me.SimpleButton1 = New DevExpress.XtraEditors.SimpleButton()
        CType(Me.GroupControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupControl1.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupControl1
        '
        Me.GroupControl1.Controls.Add(Me.ListBox1)
        Me.GroupControl1.Location = New System.Drawing.Point(12, 56)
        Me.GroupControl1.Name = "GroupControl1"
        Me.GroupControl1.Size = New System.Drawing.Size(678, 507)
        Me.GroupControl1.TabIndex = 0
        Me.GroupControl1.Text = "Results"
        '
        'ListBox1
        '
        Me.ListBox1.Font = New System.Drawing.Font("Tahoma", 7.0!)
        Me.ListBox1.ForeColor = System.Drawing.Color.Red
        Me.ListBox1.FormattingEnabled = True
        Me.ListBox1.ItemHeight = 17
        Me.ListBox1.Location = New System.Drawing.Point(7, 24)
        Me.ListBox1.Name = "ListBox1"
        Me.ListBox1.Size = New System.Drawing.Size(666, 463)
        Me.ListBox1.TabIndex = 1
        '
        'lblErr
        '
        Me.lblErr.Location = New System.Drawing.Point(19, 581)
        Me.lblErr.Name = "lblErr"
        Me.lblErr.Size = New System.Drawing.Size(120, 13)
        Me.lblErr.TabIndex = 3
        Me.lblErr.Text = "Number of Errors found: "
        '
        'SimpleButton1
        '
        Me.SimpleButton1.ImageOptions.Image = CType(resources.GetObject("SimpleButton1.ImageOptions.Image"), System.Drawing.Image)
        Me.SimpleButton1.Location = New System.Drawing.Point(12, 12)
        Me.SimpleButton1.Name = "SimpleButton1"
        Me.SimpleButton1.Size = New System.Drawing.Size(43, 38)
        Me.SimpleButton1.TabIndex = 4
        '
        'frmValidate
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(695, 606)
        Me.Controls.Add(Me.SimpleButton1)
        Me.Controls.Add(Me.lblErr)
        Me.Controls.Add(Me.GroupControl1)
        Me.Name = "frmValidate"
        Me.Text = "Results"
        CType(Me.GroupControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupControl1.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents GroupControl1 As DevExpress.XtraEditors.GroupControl
    Friend WithEvents ListBox1 As System.Windows.Forms.ListBox
    Friend WithEvents lblErr As DevExpress.XtraEditors.LabelControl
    Friend WithEvents SimpleButton1 As DevExpress.XtraEditors.SimpleButton
End Class
