Public Class frmLogin 

    Private Sub frmLogin_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        _sysUsername = ""
        _sysPass = ""
        _sysComp = ""
        _sysCompPass = ""
    End Sub

    Private Sub btnLogin_Click(sender As Object, e As EventArgs) Handles btnAccept.Click
        If Trim(txtUser.Text) <> "" Then
            If Trim(txtPass.Text) <> "" Then
                If Trim(txtComp.Text) <> "" Then
                    If Trim(txtCompPass.Text) <> "" Then
                        _sysUsername = Trim(txtUser.Text)
                        _sysPass = Trim(txtPass.Text)
                        _sysComp = Trim(txtComp.Text)
                        _sysCompPass = Trim(txtCompPass.Text)
                        frmImportPo2.Show()
                        Me.Close()
                    Else
                        MsgBox("Please enter the Syspro company password", MsgBoxStyle.Critical)
                        txtCompPass.Focus()
                    End If
                Else
                    MsgBox("Please enter the Sypro company", MsgBoxStyle.Critical)
                    txtComp.Focus()
                End If
            Else
                MsgBox("Please enter your user password", MsgBoxStyle.Critical)
                txtPass.Focus()
            End If
        Else
            MsgBox("Please enter your Syspro usrname", MsgBoxStyle.Critical)
            txtUser.Focus()
        End If

    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        End
    End Sub
End Class