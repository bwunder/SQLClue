Imports System.Windows.Forms

Public Class DialogRunbookTopic

    Private Sub DialogRunbookTopic_Load(ByVal sender As Object, _
                                         ByVal e As System.EventArgs) _
    Handles Me.Load
        Try
            LoadTopics()
        Catch ex As Exception
            Throw (New ApplicationException("Topic Maintenance failed", ex))
        End Try

    End Sub

    Private Sub LoadTopics()
        RunbookForm.SetTopicGridItems()
        TTopicBindingSource.DataSource = RunbookForm.AllTopics
        TTopicBindingSource.ResetBindings(False)
        TopicDataGridView.ClearSelection()
    End Sub

    Private Sub OK_Button_Click(ByVal sender As System.Object, _
                                ByVal e As System.EventArgs) _
    Handles OK_Button.Click
        Try
            SaveTopicChanges()
            RunbookForm.SetAllTopics()
            RunbookForm.SetTopicGridItems()
            Me.DialogResult = System.Windows.Forms.DialogResult.OK
            Me.Dispose()
        Catch ex As Exception
            Throw New Exception("(DialogRunbookTopic.OK_Button_Click) Exception.", ex)
        End Try
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, _
                                    ByVal e As System.EventArgs) _
    Handles Cancel_Button.Click
        RunbookForm.AllTopics.RejectChanges()
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Dispose()
    End Sub

    Private Sub SaveTopicChanges()
        ' this is only place categories can be saved
        'remove the blank row
        Using TableAdapterTopics As New DataSetSQLRunbookTableAdapters.tTopicTableAdapter
            TableAdapterTopics.Connection.ConnectionString = RunbookForm.sRunbookConnectionString
            TableAdapterTopics.Update(RunbookForm.AllTopics)
        End Using
        LoadTopics()
    End Sub

    Private Sub TopicDataGridView_CellClick(ByVal sender As Object, _
                                               ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) _
    Handles TopicDataGridView.CellClick
        Select Case e.RowIndex
            Case -1
                If e.ColumnIndex = -1 Then
                    LoadTopics()
                End If
            Case Else
                Dim TopicId As Integer = CInt(TopicDataGridView.Rows(e.RowIndex).Cells("DataGridViewTextBoxColumnId").Value)
                Dim TopicName As String = TopicDataGridView.Rows(e.RowIndex).Cells("DataGridViewTextBoxColumnName").Value.ToString
                If UCase(My.User.Name) = UCase(My.Settings.LicensedUser) Then
                    Select Case e.ColumnIndex
                        Case -1
                            If CInt(TopicDataGridView.Rows(e.RowIndex).Cells("DataGridViewTextBoxColumnNbrDocuments").Value) = 0 Then
                                If MessageBox.Show(String.Format("Remove Topic '{0}' from the Runbook?", TopicName), _
                                                    "", _
                                                    MessageBoxButtons.OKCancel, _
                                                    MessageBoxIcon.Question, _
                                                    MessageBoxDefaultButton.Button2, _
                                                    0, _
                                                    False) = Windows.Forms.DialogResult.OK Then
                                    RunbookForm.AllTopics.FindById(TopicId).Delete()
                                End If
                            Else
                                Mother.HandleException(New Exception(String.Format("Cannot delete Topic '{0}'. There are Documents assigned.", _
                                                              TopicName)))
                            End If
                        Case 1 ' Name
                            TopicName = InputBox("Edit the Topic Name", _
                                               "Edit Topic", _
                                               TopicName).ToString
                            If TopicName <> TopicDataGridView.Rows(e.RowIndex).Cells("DataGridViewTextBoxColumnName").Value.ToString Then
                                RunbookForm.AllTopics.FindById(TopicId).Name = TopicName
                            End If
                    End Select ' column
                Else
                    Mother.HandleException(New Exception("Access denied."))
                End If ' is admin
        End Select ' row
    End Sub

End Class
