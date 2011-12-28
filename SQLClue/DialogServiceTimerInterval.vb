Imports System.Windows.Forms

Public Class DialogServiceTimerInterval

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click

        My.Settings.ServiceTimerIntervalSeconds = CInt(NumericUpDownServiceTimerInterval.Value)

        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()

    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click

        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()

    End Sub

    Private Sub DialogServiceTimerInterval_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown

        NumericUpDownServiceTimerInterval.Value = My.Settings.ServiceTimerIntervalSeconds

    End Sub

End Class
