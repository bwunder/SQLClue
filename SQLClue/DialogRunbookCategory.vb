Imports System.Windows.Forms

Public Class DialogRunbookCategory

    Private Sub RunbookCategoryForm_Load(ByVal sender As Object, _
                                         ByVal e As System.EventArgs) _
    Handles Me.Load
        Try
            LoadCategories()
        Catch ex As Exception
            Throw (New ApplicationException("Category Maintenance failed", ex))
        End Try

    End Sub

    Private Sub LoadCategories()
        RunbookForm.SetCategoryList()
        TSQLRunbookCategoryBindingSource.DataSource = RunbookForm.AllCategories
        TSQLRunbookCategoryBindingSource.ResetBindings(False)
        SetRatings()
        CategoryDataGridView.ClearSelection()
    End Sub

    Private Sub OK_Button_Click(ByVal sender As System.Object, _
                                ByVal e As System.EventArgs) _
    Handles OK_Button.Click
        Try
            SaveCategoryChanges()
            Me.DialogResult = System.Windows.Forms.DialogResult.OK
            Me.Dispose()
        Catch ex As Exception
            Throw New Exception("(RunbookCategoryForm.OK_Button_Click) Exception.", ex)
        End Try
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, _
                                    ByVal e As System.EventArgs) _
    Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Dispose()
    End Sub

    Private Sub SaveCategoryChanges()
        ' this is only place categories can be saved
        Try
            'remove the blank row
            RunbookForm.AllCategories.FindById(0).Delete()
            Using TableAdapterCategories As New DataSetSQLRunbookTableAdapters.tCategoryTableAdapter
                TableAdapterCategories.Connection.ConnectionString = RunbookForm.sRunbookConnectionString
                TableAdapterCategories.Update(RunbookForm.AllCategories)
            End Using
            LoadCategories()
        Catch ex As Exception
            Throw New Exception("(RunbookCategoryForm.SaveCategoryChanges) Exception.", ex)
        End Try
    End Sub

    Private Sub CategoryDataGridView_CellClick(ByVal sender As Object, _
                                               ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) _
    Handles CategoryDataGridView.CellClick
        Try
            Select Case e.RowIndex
                Case -1
                    If e.ColumnIndex = -1 Then
                        LoadCategories()
                    End If
                Case Else
                    If CategoryDataGridView.Rows(e.RowIndex).IsNewRow Then
                        If UCase(My.User.Name) = UCase(My.Settings.LicensedUser) _
                        Or Not My.Settings.RunbookCategoryLocked Then
                            DialogCategoryAdd.TextBoxName.Text = ""
                            DialogCategoryAdd.RichTextBoxNote.Text = ""
                            If DialogCategoryAdd.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                                If Not DialogCategoryAdd.TextBoxName.Text = "" _
                                AndAlso RunbookForm.AllCategories.Select(String.Format("Name='{0}'", DialogCategoryAdd.TextBoxName.Text)).Count = 0 Then
                                    RunbookForm.AllCategories.AddtCategoryRow(DialogCategoryAdd.TextBoxName.Text, _
                                                                              DialogCategoryAdd.RichTextBoxNote.Text, _
                                                                              Now, _
                                                                              My.User.Name, _
                                                                              0, _
                                                                              0, _
                                                                              0, _
                                                                              Now, _
                                                                              My.User.Name)
                                    SaveCategoryChanges()
                                Else
                                    If DialogCategoryAdd.TextBoxName.Text = "" Then
                                        Mother.HandleException(New Exception("Category Name cannot be blank."))
                                    Else
                                        Mother.HandleException(New Exception(String.Format("'{0}' is already in the Category List.", DialogCategoryAdd.TextBoxName.Text)))
                                    End If
                                End If
                            End If
                        Else
                            Mother.HandleException(New Exception(String.Format("User [{0}] is not authorized to modify the Category List.", My.User.Name)))
                        End If ' is admin
                    Else
                        Dim CatId As Integer = CInt(CategoryDataGridView.Rows(e.RowIndex).Cells("DataGridViewTextBoxColumnCategoryId").Value)
                        Dim CatName As String = CategoryDataGridView.Rows(e.RowIndex).Cells("DataGridViewTextBoxColumnCategoryName").Value.ToString
                        Select Case e.ColumnIndex
                            Case -1
                                If UCase(My.User.Name) = UCase(My.Settings.LicensedUser) _
                                Or Not My.Settings.RunbookCategoryLocked Then
                                    If CInt(CategoryDataGridView.Rows(e.RowIndex).Cells("DataGridViewTextBoxColumnNbrTopics").Value) = 0 Then
                                        If MessageBox.Show(String.Format("Remove Category '{0}' from the Runbook?", CatName), _
                                                            "", _
                                                            MessageBoxButtons.OKCancel, _
                                                            MessageBoxIcon.Question, _
                                                            MessageBoxDefaultButton.Button2, _
                                                            0, _
                                                            False) = Windows.Forms.DialogResult.OK Then
                                            RunbookForm.AllCategories.FindById(CatId).Delete()
                                            SaveCategoryChanges()
                                        End If
                                    Else
                                        Mother.HandleException(New Exception(String.Format("Cannot delete Category '{0}'. There are Topics assigned.", _
                                                                      CatName)))
                                    End If
                                Else
                                    Mother.HandleException(New Exception("Access denied."))
                                End If ' is admin
                            Case 0 ' Rating
                                ' will get saved in dialog
                                If DialogRating.ShowDialog("Category", _
                                        CatId, _
                                        CatName, _
                                        RunbookForm.sRunbookConnectionString, _
                                        Me) = Windows.Forms.DialogResult.OK Then
                                    SaveCategoryChanges()
                                    DialogRating.Dispose()
                                End If
                            Case 2 ' Name
                                CatName = InputBox("Edit the Category Name (one word name recommended)", _
                                                   "Edit Category", _
                                                   CatName).ToString
                                If CatName <> CategoryDataGridView.Rows(e.RowIndex).Cells("DataGridViewTextBoxColumnCategoryName").Value.ToString Then
                                    RunbookForm.AllCategories.FindById(CatId).Name = CatName
                                    SaveCategoryChanges()
                                End If
                            Case 3 ' Notes
                                If UCase(My.User.Name) = UCase(My.Settings.LicensedUser) _
                                Or Not My.Settings.RunbookCategoryLocked Then
                                    Dim Notes As String = CategoryDataGridView.Rows(e.RowIndex).Cells("DataGridViewButtonColumnCategoryNotes").Value.ToString
                                    If DialogNotes.ShowDialog(Notes, _
                                                              Me.Text, _
                                                              "Category", _
                                                              CatName, _
                                                              Me) = Windows.Forms.DialogResult.OK Then
                                        If Not CategoryDataGridView.Rows(e.RowIndex).Cells("DataGridViewButtonColumnCategoryNotes").Value.ToString = Notes Then
                                            RunbookForm.AllCategories.FindById(CatId).Notes = Notes
                                            SaveCategoryChanges()
                                        End If
                                    End If
                                Else
                                    Mother.HandleException(New Exception("Access denied."))
                                End If ' is admin
                                'Case Else
                                ' noop
                        End Select ' column
                    End If ' not new row
            End Select ' row
        Catch ex As Exception
            Throw New Exception(String.Format("({0}.CategoryDataGridView_CellContentClick) Exception.", _
                                                              Me.Name), ex)
        End Try
    End Sub

    Private Sub SetRatings()
        Try
            For Each r As DataGridViewRow In CategoryDataGridView.Rows
                ' no division by zero
                If CInt(r.Cells("DataGridViewTextBoxColumnCategoryRatingCount").Value) = 0 Then
                    r.Cells("DataGridViewButtonColumnCategoryRating").Value = 0
                Else
                    r.Cells("DataGridViewButtonColumnCategoryRating").Value = _
                    CInt(r.Cells("DataGridViewTextBoxColumnCategoryRatingTally").Value) / _
                           CInt(r.Cells("DataGridViewTextBoxColumnCategoryRatingCount").Value)

                    ' need this users rating
                    r.Cells("DataGridViewButtonColumnCategoryRating").Tag = RunbookForm.GetRating("Category", _
                                  CInt(r.Cells("DataGridViewTextBoxColumnCategoryId").Value))

                    r.Cells("DataGridViewButtonColumnCategoryRating").ToolTipText = _
                         String.Format("Your last rating {0}, {1} total ratings. Select button to rate.", _
                                       CType(r.Cells("DataGridViewButtonColumnCategoryRating").Tag, RunbookForm.StructRating).Rating, _
                                       r.Cells("DataGridViewTextBoxColumnCategoryRatingCount").Value)
                    Select Case CInt(r.Cells("DataGridViewButtonColumnCategoryRating").Value)
                        Case 0
                            r.Cells("DataGridViewButtonColumnCategoryRating").Style.BackColor = My.Settings.Rating0
                        Case 1
                            r.Cells("DataGridViewButtonColumnCategoryRating").Style.BackColor = My.Settings.Rating1
                        Case 2
                            r.Cells("DataGridViewButtonColumnCategoryRating").Style.BackColor = My.Settings.Rating2
                        Case 3
                            r.Cells("DataGridViewButtonColumnCategoryRating").Style.BackColor = My.Settings.Rating3
                        Case 4
                            r.Cells("DataGridViewButtonColumnCategoryRating").Style.BackColor = My.Settings.Rating4
                        Case 5
                            r.Cells("DataGridViewButtonColumnCategoryRating").Style.BackColor = My.Settings.Rating5
                        Case 6
                            r.Cells("DataGridViewButtonColumnCategoryRating").Style.BackColor = My.Settings.Rating6
                    End Select
                End If
            Next
        Catch ex As Exception
            Throw (New ApplicationException("Category Maintenance failed"))
        End Try
    End Sub

End Class
