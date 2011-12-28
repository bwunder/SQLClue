Imports System.Windows.Forms

Public Class DialogDateTimePicker

    Private PassedDate As Date

    Public Overloads Sub ShowDialog(ByRef IntervalBaseDt As Date, _
                                    ByVal parent As IWin32Window)

        Try

            ' Put the customer id in the window title.
            Me.Text = "Select a Date and Time"

            PassedDate = IntervalBaseDt
            IntervalBaseDate.Text = CStr(PassedDate)
            IntervalBaseDate.Format = DateTimePickerFormat.Short
            IntervalBaseHour.Value = PassedDate.Hour
            IntervalBaseMinute.Value = PassedDate.Minute

            ' Show the dialog.
            Me.ShowDialog(parent)
        Catch ex As Exception
            Mother.HandleException(ex)
        End Try

    End Sub

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        PassedDate = CDate(IntervalBaseDate.Text)
        PassedDate = PassedDate.AddHours(IntervalBaseHour.Value)
        PassedDate = PassedDate.AddMinutes(IntervalBaseMinute.Value)
        Me.Tag = PassedDate
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

End Class
