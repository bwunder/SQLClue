Imports System.Windows.Forms

Public Class DialogDocumentInterval

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click

        My.Settings.WaitBetweenDocumentsSeconds = CInt(NumericUpDown1.Value)

        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()

    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub DialogDocumentInterval_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown

        NumericUpDown1.Value = My.Settings.WaitBetweenDocumentsSeconds

    End Sub

End Class
