Imports System.Windows.Forms

Public Class DialogSelectConfiguredInstance

    Friend SelectedInstance As String

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        SelectedInstance = TSQLCfgConnectionDataGridView.CurrentRow.Cells("InstanceNameDataGridViewTextBoxColumn").Value.ToString
        If SelectedInstance = "" Then
            MessageBox.Show("No SQL Instance selected!")
        Else
            Me.DialogResult = System.Windows.Forms.DialogResult.OK
            Me.Close()
        End If
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        SelectedInstance = ""
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub DialogSelectConfiguredInstance_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        SelectedInstance = ""

        TSQLCfgConnectionBindingSource.DataSource = Mother.DAL.dsSQLCfg
        TSQLCfgConnectionBindingSource.DataMember = "tConnection"
        TSQLCfgConnectionBindingSource.ResetBindings(False)

        If TSQLCfgConnectionDataGridView.Rows.Count = 0 Then
            MessageBox.Show("No Configured SQL Servers found!")
            Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.Close()
        End If

        OK_Button.NotifyDefault(True)

    End Sub

End Class
