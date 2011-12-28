Imports System.Windows.Forms

Public Class DialogRunbookLookupCriteria

    Private Sub RunbookLookupCriteriaDialog_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim csr As Cursor = Me.Cursor
        Try
            Me.Cursor = Cursors.WaitCursor
            LoadDefaultSearch()
            Me.StartPosition = FormStartPosition.Manual
            Me.Location = New Point(CInt(Mother.Location.X + 0.2 * Mother.Width), _
                                    CInt(Mother.Location.Y + 0.5 * Mother.Height))
            Me.Height = CInt(My.Forms.Mother.Height * 0.5)
            Me.Width = CInt(My.Forms.Mother.Width * 0.8)
        Catch ex As Exception
            Mother.HandleException(New ApplicationException(String.Format("({0}.Load) Exception.", Me.Text), ex))
        Finally
            Me.Cursor = csr
        End Try
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, _
                                    ByVal e As System.EventArgs) _
                                    Handles ButtonCancel.Click
        Try
            Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Catch ex As Exception
            Mother.HandleException(New ApplicationException(String.Format("({0}.Cancel_Button_Click) Exception.", Me.Text), ex))
        Finally
            Me.Dispose()
        End Try
    End Sub

    Private Sub ListBoxFullText_SelectedValueChanged(ByVal sender As Object, _
                                                     ByVal e As System.EventArgs) _
                                                     Handles ListBoxFullText.SelectedValueChanged
        Dim csr As Cursor = Me.Cursor
        Try
            Me.Cursor = Cursors.WaitCursor
            If ListBoxFullText.SelectedItems.Contains(ListBoxFullText.Items(0).ToString) Then
                ' none selected
                ListBoxFullText.SelectedItems.Clear()
            End If
            If ListBoxFullText.SelectedItems.Contains(ListBoxFullText.Items(5).ToString) Then
                ' and seleceted
                ListBoxFullText.SelectedItems.Clear()
                ListBoxFullText.SelectedItems.Add(ListBoxFullText.Items(1))
                ListBoxFullText.SelectedItems.Add(ListBoxFullText.Items(2))
                ListBoxFullText.SelectedItems.Add(ListBoxFullText.Items(3))
                ListBoxFullText.SelectedItems.Add(ListBoxFullText.Items(4))
            End If
        Catch ex As Exception
            Mother.HandleException(New ApplicationException(String.Format("({0}.ListBoxFullText_SelectedValueChanged) Exception.", Me.Text), ex))
        Finally
            Me.Cursor = csr
        End Try
    End Sub

    Friend Sub LoadDefaultSearch()
        Try
            ' 
            DateTimePickerEndDate.Checked = True
            DateTimePickerEndDate.Value = Now
            DateTimePickerStartDate.Checked = True
            DateTimePickerStartDate.Value = DateAdd(DateInterval.Day, -7, Today)
            ComboBoxTopic.Text = ""
            ComboBoxDocument.Text = ""
            ComboBoxCategory.Text = ""
            ComboBoxPerson.Text = My.User.Name
            ListBoxFullText.Items.Clear()
            ListBoxFullText.Items.Add("(none)")
            ListBoxFullText.Items.Add("Categories")
            ListBoxFullText.Items.Add("Topics")
            ListBoxFullText.Items.Add("Documents")
            ListBoxFullText.Items.Add("Ratings")
            ListBoxFullText.Items.Add("(any)")
            ListBoxFullText.SelectedItems.Add(ListBoxFullText.Items(1).ToString)
            ListBoxFullText.SelectedItems.Add(ListBoxFullText.Items(2).ToString)
            ListBoxFullText.SelectedItems.Add(ListBoxFullText.Items(3).ToString)
            ListBoxFullText.SelectedItems.Add(ListBoxFullText.Items(4).ToString)
            ComboBoxRatingOperator.SelectedText = ">"
            NumericUpDownRating.Value = 0
            RichTextBoxContains.Text = ""
        Catch ex As Exception
            Mother.HandleException(New ApplicationException(String.Format("({0}.LoadDefaultSearch) Exception.", Me.Text), ex))
        End Try
    End Sub

    Private Sub ButtonSearch_Click(ByVal sender As Object, _
                                   ByVal e As System.EventArgs) _
                                   Handles ButtonSearch.Click
        Dim csr As Cursor = Me.Cursor
        Try
            Me.Cursor = Cursors.WaitCursor
            RunbookForm.LoadRunbook(0)
            My.Settings.Save()
            Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Catch ex As Exception
            My.Settings.Reload()
            Mother.HandleException(New ApplicationException(String.Format("({0}.ButtonSearch_Click) Exception.", Me.Text), ex))
        Finally
            Me.Cursor = csr
            'no no still need to access the values from these controls 
            'Me.Dispose()
        End Try
    End Sub

    Private Sub ComboBoxTopic_DropDown(ByVal sender As Object, _
                                          ByVal e As System.EventArgs) _
                                          Handles ComboBoxTopic.DropDown
        Dim csr As Cursor = Me.Cursor
        Try
            Me.Cursor = Cursors.WaitCursor
            ComboBoxTopic.Items.Clear()
            ComboBoxTopic.DisplayMember = "Name"
            ComboBoxTopic.ValueMember = "Id"
            ComboBoxTopic.DataSource = RunbookForm.AllTopics
        Catch ex As Exception
            Mother.HandleException(New ApplicationException(String.Format("({0}.ComboBoxTopic_DropDown) Exception.", Me.Text), ex))
        Finally
            Me.Cursor = csr
        End Try
    End Sub

    Private Sub ComboBoxDocument_DropDown(ByVal sender As Object, _
                                          ByVal e As System.EventArgs) _
                                          Handles ComboBoxDocument.DropDown
        Dim csr As Cursor = Me.Cursor
        Try
            Me.Cursor = Cursors.WaitCursor
            ComboBoxDocument.Items.Clear()
            ComboBoxDocument.DisplayMember = "File"
            ComboBoxDocument.ValueMember = "Id"
            ComboBoxDocument.DataSource = RunbookForm.AllDocuments
        Catch ex As Exception
            Mother.HandleException(New ApplicationException(String.Format("({0}.ComboBoxTopic_DropDown) Exception.", Me.Text), ex))
        Finally
            Me.Cursor = csr
        End Try
    End Sub

    Private Sub ComboBoxCategory_DropDown(ByVal sender As Object, _
                                          ByVal e As System.EventArgs) _
                                          Handles ComboBoxCategory.DropDown
        Dim csr As Cursor = Me.Cursor
        Try
            Me.Cursor = Cursors.WaitCursor
            ComboBoxCategory.DataSource = Nothing
            ComboBoxCategory.Items.Clear()
            ComboBoxCategory.Items.Add("")
            ComboBoxCategory.DisplayMember = "Name"
            ComboBoxCategory.ValueMember = "Id"
            ComboBoxCategory.DataSource = RunbookForm.AllCategories
        Catch ex As Exception
            Mother.HandleException(New ApplicationException(String.Format("({0}.ComboBoxCategory_DropDown) Exception.", Me.Text), ex))
        Finally
            Me.Cursor = csr
        End Try
    End Sub

    Private Sub ComboBoxPerson_DropDown(ByVal sender As Object, _
                                        ByVal e As System.EventArgs) _
                                        Handles ComboBoxPerson.DropDown
        Dim csr As Cursor = Me.Cursor
        Try
            Me.Cursor = Cursors.WaitCursor
            ComboBoxPerson.Items.Clear()
            ComboBoxPerson.DisplayMember = "OriginalLogin"
            ComboBoxPerson.ValueMember = "OriginalLogin"
            ComboBoxPerson.DataSource = RunbookForm.AllUsers
        Catch ex As Exception
            Mother.HandleException(New ApplicationException(String.Format("({0}.ComboBoxPerson_DropDown) Exception.", Me.Text), ex))
        Finally
            Me.Cursor = csr
        End Try
    End Sub

    Private Sub ResetButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ResetButton.Click
        Try
            LoadDefaultSearch()
        Catch ex As Exception
            Mother.HandleException(New ApplicationException(String.Format("({0}.ResetButton_Click) Exception.", Me.Text), ex))
        End Try
    End Sub

End Class
