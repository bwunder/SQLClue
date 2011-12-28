Imports System.Windows.Forms

Public Class DialogRunbookSecurity

    Public Overloads Function ShowDialog(ByVal Document As String, _
                                         ByVal NowRequired As Boolean, _
                                         ByVal OKMsg As String, _
                                         ByVal ParentForm As IWin32Window) As System.Windows.Forms.DialogResult
        Dim result As DialogResult = Windows.Forms.DialogResult.Ignore
        If Not Mother.SQLClueServiceAccount = "" Then
            Label1.Text = String.Format(My.Resources.DocumentSecurityMessage, My.Settings.LicensedUser)
            RichTextBox1.Text = _
            String.Format("SQLClue Runbook Automation Controller start-up account [{0}] read access is {1} required for document" & vbCrLf & vbCrLf & _
                  " {2}", _
                  Mother.SQLClueServiceAccount, _
                  If(NowRequired, "now", "no longer"), _
                  Document)
            'Rights change requirements can be copied from the text box. 
            Label2.Text = String.Format("Action is required to complete this task.." & vbCrLf & _
                                        "Access change will not be made by SQLClue." & vbCrLf & _
                                        "Select [OK] to {0}.", OKMsg)
            result = Me.ShowDialog(ParentForm)
        End If
        Return result
    End Function

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

End Class
