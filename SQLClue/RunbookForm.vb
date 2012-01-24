Imports System.Security.AccessControl
Imports System.Windows.Forms
Imports System.Drawing

Public Class RunbookForm
    Friend AllCategories As New DataSetSQLRunbook.tCategoryDataTable
    Friend AllUsers As New DataSetSQLRunbook.tUserDataTable
    Public AllTopics As New DataSetSQLRunbook.tTopicDataTable
    Friend AllDocTypes As New DataSetSQLRunbook.fulltext_document_typesDataTable
    Friend AllDocuments As New DataSetSQLRunbook.tDocumentDataTable
    Friend LookupTopics As New DataSetSQLRunbook.tTopicDataTable
    Private _CategoriesChanged As Boolean
    Private _DocumentChanged As Boolean
    Private _DocTypeChanged As Boolean
    Private _DocOwnerChanged As Boolean
    Private _WatchChanged As Boolean
    Private OldSpitterDistance As Integer
    Friend Structure StructDocInfo
        Friend Id As Integer
        Friend FileName As String
        Friend DocumentLength As Integer
        Friend DocumentType As String
        Friend Owner As String
        Friend LastModifiedDt As DateTime
        Friend WatchFileForChange As Boolean
        Friend IsAdmin As Boolean
    End Structure
    ' should be same as struc on rating form
    Friend Structure StructRating
        Friend Rating As Integer
        Friend ItemId As Integer
        Friend RatingType As String
        Friend Notes As String
        Friend AvgRating As Integer
        Friend NbrRatings As Integer
        Friend NewRating As Integer
        Friend NewNote As String
    End Structure

    Public ReadOnly Property sRunbookConnectionString() As String
        Get
            Dim builder As New System.Data.SqlClient.SqlConnectionStringBuilder
            If My.Settings.RunbookEnabled Then
                builder.DataSource = My.Settings.RunbookInstanceName
                builder.InitialCatalog = My.Settings.RunbookDatabaseName
                If My.Settings.RunbookUseTrustedConnection Then
                    builder.IntegratedSecurity = True
                Else
                    builder.UserID = My.Settings.RunbookSQLLoginName
                    builder.Password = My.Settings.RunbookSQLLoginPassword
                End If
                builder.ConnectTimeout = My.Settings.RunbookConnectionTimeout
                builder.ApplicationName = My.Application.Info.ProductName & " : " & Me.Text
                'allow for a 'not specified' net
                If My.Settings.RunbookNetworkLibrary <> "" Then
                    builder.NetworkLibrary = CStr(If(InStr(My.Settings.RunbookNetworkLibrary, " ") > 0, _
                                                      My.Settings.RunbookNetworkLibrary.Split(Chr(32))(0), _
                                                      My.Settings.RunbookNetworkLibrary))
                End If
                builder.Encrypt = My.Settings.RunbookEncryptConnection
                builder.TrustServerCertificate = My.Settings.RunbookTrustServerCertificate
                builder.Enlist = True
                builder.PersistSecurityInfo = False
                builder.UserInstance = False
            Else 'try remote runbook
                builder.DataSource = My.Settings.RemoteRunbookInstanceName
                builder.InitialCatalog = My.Settings.RemoteRunbookDatabaseName
                If My.Settings.RemoteRunbookUseTrustedConnection Then
                    builder.IntegratedSecurity = True
                Else
                    builder.UserID = My.Settings.RemoteRunbookSQLLoginName
                    builder.Password = My.Settings.RemoteRunbookSQLLoginPassword
                End If
                builder.ConnectTimeout = My.Settings.RemoteRunbookConnectionTimeout
                builder.ApplicationName = My.Application.Info.ProductName & " : " & Me.Text
                'allow for a 'not specified' net
                If My.Settings.RemoteRunbookNetworkLibrary <> "" Then
                    builder.NetworkLibrary = CStr(If(InStr(My.Settings.RemoteRunbookNetworkLibrary, " ") > 0, _
                                                      My.Settings.RemoteRunbookNetworkLibrary.Split(Chr(32))(0), _
                                                      My.Settings.RemoteRunbookNetworkLibrary))
                End If
                builder.Encrypt = My.Settings.RemoteRunbookEncryptConnection
                builder.TrustServerCertificate = My.Settings.RemoteRunbookTrustServerCertificate
                builder.Enlist = True
                builder.PersistSecurityInfo = False
                builder.UserInstance = False
            End If
            Return builder.ConnectionString
        End Get
    End Property

    Private Sub RunbookForm_Load(ByVal sender As System.Object, _
                                 ByVal e As System.EventArgs) _
                                 Handles MyBase.Load
        Try
            _CategoriesChanged = False
            _DocumentChanged = False
            PanelCategoryList.ForeColor = PanelTopicList.ForeColor
            PanelCategoryList.BackColor = PanelTopicList.BackColor
            PanelTopicDetail.ForeColor = PanelTopicList.ForeColor
            PanelTopicDetail.BackColor = PanelTopicList.BackColor
            PanelDocumentList.ForeColor = PanelTopicList.ForeColor
            PanelDocumentList.BackColor = PanelTopicList.BackColor
            ListBoxCategory.MultiColumn = My.Settings.RunbookCategoriesMultiColumn
            Mother.CategoriesMulticolumnToolStripMenuItem.Checked = My.Settings.RunbookCategoriesMultiColumn
            TableAdapterDocument.Connection.ConnectionString = sRunbookConnectionString
            TableAdapterTopic.Connection.ConnectionString = sRunbookConnectionString
            LoadRunbook(0)
            Me.AcceptButton = ButtonSave
        Catch ex As Exception
            ' let mother deal with it directly, same difference either way
            Throw New Exception(String.Format("({0}.RunbookForm_Load) Exception.", Me.Name), ex)
        End Try
    End Sub

    Friend Sub LoadRunbook(ByVal TopicId As Integer)
        Try
            ' refresh the work lists 
            'ClearDetailPanel()
            SetUserList()
            SetDocTypeList()
            SetDocumentList()
            SetCategoryList()
            SetAllTopics()
            LoadRunbookMenuItems()
            SetTopicGridItems()
            If TopicId > 0 Then
                SetTopicDetailItems(TopicId)
            End If
        Catch ex As Exception
            Throw New Exception(String.Format("({0}.LoadRunbook) Exception.", Me.Name), ex)
        End Try
    End Sub

    Private Sub LoadRunbookMenuItems()
        Try
            Using cn As New SqlConnection(sRunbookConnectionString)
                cn.Open()
                Using cm As New System.Data.SqlClient.SqlCommand
                    cm.Connection = cn
                    cm.CommandType = CommandType.StoredProcedure
                    cm.CommandText = "SQLRunbook.pOptionGet"
                    Dim EnforceOwnership As New SqlParameter()
                    With EnforceOwnership
                        .Direction = ParameterDirection.Output
                        .ParameterName = "@EnforceOwnership"
                        .SqlDbType = SqlDbType.Bit
                    End With
                    cm.Parameters.Add(EnforceOwnership)
                    Dim ScanForDocumentChanges As New SqlParameter()
                    With ScanForDocumentChanges
                        .Direction = ParameterDirection.Output
                        .ParameterName = "@ScanForDocumentChanges"
                        .SqlDbType = SqlDbType.Bit
                    End With
                    cm.Parameters.Add(ScanForDocumentChanges)
                    cm.ExecuteNonQuery()
                    Mother.ToolStripMenuItemEnforceOwnership.Checked = If(DBNull.Value.Equals(EnforceOwnership.Value), _
                                                                          True, _
                                                                          CBool(EnforceOwnership.Value))
                    Mother.ToolStripMenuItemFileWatcher.Checked = If(DBNull.Value.Equals(ScanForDocumentChanges.Value), _
                                                                     True, _
                                                                     CBool(ScanForDocumentChanges.Value))
                End Using
            End Using
        Catch ex As Exception
            Mother.HandleException(New Exception(String.Format("({0}.LoadRunbookMenuItems) Exception.", Me.Name), ex))
        End Try
    End Sub

    Friend Sub SetRunbookOptionFromMenuItems()
        Try
            Using cn As New SqlConnection(sRunbookConnectionString)
                cn.Open()
                Using cm As New System.Data.SqlClient.SqlCommand
                    cm.Connection = cn
                    cm.CommandType = CommandType.StoredProcedure
                    cm.CommandText = "SQLRunbook.pOptionSet"
                    Dim EnforceOwnership As New SqlParameter()
                    With EnforceOwnership
                        .Direction = ParameterDirection.Input
                        .ParameterName = "@EnforceOwnership"
                        .SqlDbType = SqlDbType.Bit
                        .Value = Mother.ToolStripMenuItemEnforceOwnership.Checked
                    End With
                    cm.Parameters.Add(EnforceOwnership)
                    Dim ScanForDocumentChanges As New SqlParameter()
                    With ScanForDocumentChanges
                        .Direction = ParameterDirection.Input
                        .ParameterName = "@ScanForDocumentChanges"
                        .SqlDbType = SqlDbType.Bit
                        .Value = Mother.ToolStripMenuItemFileWatcher.Checked
                    End With
                    cm.Parameters.Add(ScanForDocumentChanges)
                    cm.ExecuteNonQuery()
                End Using
            End Using
        Catch ex As Exception
            Mother.HandleException(New Exception(String.Format("({0}.SetRunbookOptionFromMenuItems) Exception.", Me.Name), ex))
        End Try
    End Sub

    Private Sub DataGridViewTopicList_CellClick(ByVal sender As System.Object, _
                                                ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) _
                                                Handles DataGridViewTopicList.CellClick
        Dim csr As Cursor = Me.Cursor
        Try
            Me.Cursor = Cursors.WaitCursor
            'ClearDetailPanel()
            Select Case e.RowIndex
                Case -1
                    ' otherwise nust be sorting
                    If e.ColumnIndex = -1 Then
                        ' refresh the datagridview
                        SetTopicGridItems()
                    End If
                Case Else
                    If DataGridViewTopicList.Rows(e.RowIndex).IsNewRow Then
                        Dim TopicName As String = InputBox("Enter Topic Name", "Add Runbook Topic", "").ToString
                        If Not TopicName = "" Then
                            With DataGridViewTopicList.Rows(e.RowIndex)
                                .Cells("DataGridViewButtonColumnTopicName").Value = TopicName
                                .Cells("DataGridViewButtonColumnTopicNotes").Value = ""
                                .Cells("DataGridViewComboBoxColumnTopicOwner").Value = My.User.Name
                                .Cells("DataGridViewTextBoxColumnTopicRecCreatedDt").Value = Now
                                .Cells("DataGridViewTextBoxColumnTopicRecCreatedUser").Value = My.User.Name
                                .Cells("DataGridViewTextBoxColumnTopicLastUpdatedDt").Value = Now
                                .Cells("DataGridViewTextBoxColumnTopicLastUpdatedUser").Value = My.User.Name
                            End With
                            SaveLookupTopics()
                            SetTopicGridItems()
                        End If
                    Else
                        'load the selected row
                        Dim SelectedTopicId As Integer = CInt(DataGridViewTopicList.Rows(e.RowIndex).Cells("DataGridViewTextBoxColumnTopicId").Value)
                        Dim SelectedTopicName As String = DataGridViewTopicList.Rows(e.RowIndex).Cells("DataGridViewButtonColumnTopicName").Value.ToString
                        Select Case e.ColumnIndex
                            Case -1
                                If MessageBox.Show(String.Format("Delete Topic '{0}'?", SelectedTopicName), _
                                                    "Confirm Change", _
                                                    MessageBoxButtons.YesNo, _
                                                    MessageBoxIcon.Question, _
                                                    MessageBoxDefaultButton.Button2, _
                                                    MessageBoxOptions.ServiceNotification, _
                                                    False) = DialogResult.Yes Then
                                    LookupTopics.FindById(SelectedTopicId).Delete()
                                    SaveLookupTopics()
                                    SetTopicGridItems()
                                End If
                            Case 0 ' rate
                                ' will get saved in dialog
                                DialogRating.ShowDialog("Topic", _
                                            SelectedTopicId, _
                                            SelectedTopicName, _
                                            sRunbookConnectionString, _
                                            Me)
                                ' reload 
                                If Not DialogRating.PeerReview.NewRating = 0 _
                                AndAlso DialogRating.PeerReview.Rating <> DialogRating.PeerReview.NewRating Then
                                    SetTopicGridItems()
                                End If
                                DialogRating.Dispose()
                                DataGridViewTopicList.ClearSelection()
                            Case 1 'id
                            Case 2 'name
                                SetTopicDetailItems(SelectedTopicId)
                                ToolStripStatusLabelRunbook.Text = My.Resources.Ready
                            Case 3 ' view notes
                                Dim Notes As String = DataGridViewTopicList.Rows(e.RowIndex).Cells("DataGridViewButtonColumnTopicNotes").Value.ToString
                                If DialogNotes.ShowDialog(Notes, _
                                                          Me.Text, _
                                                          "Topic", _
                                                          SelectedTopicName, _
                                                          Me) = Windows.Forms.DialogResult.OK Then
                                    If Not Notes = DataGridViewTopicList.Rows(e.RowIndex).Cells("DataGridViewButtonColumnTopicNotes").Value.ToString Then
                                        DataGridViewTopicList.Rows(e.RowIndex).Cells("DataGridViewButtonColumnTopicNotes").Value = Notes
                                        SaveLookupTopics()
                                        SetTopicGridItems()
                                    End If
                                End If
                            Case 4 'Nbr Docs
                            Case 5 'Rating Count
                            Case 6 'Rating Talley
                            Case 7 'Owner
                            Case 8 'DateCreated
                            Case 9 'Originator
                            Case Else
                        End Select
                    End If
            End Select
        Catch ex As Exception
            Mother.HandleException(New Exception(String.Format("({0}.DataGridViewTopicList_CellContentClick) Exception.", Me.Name), ex))
        Finally
            Me.Cursor = csr
        End Try
    End Sub

#Region " Data Access "

    Friend Sub SetUserList()
        Try
            Using TableAdapterUsers As New DataSetSQLRunbookTableAdapters.tUserTableAdapter
                TableAdapterUsers.Connection.ConnectionString = sRunbookConnectionString
                AllUsers = TableAdapterUsers.GetData()
                AllUsers.AddtUserRow("", "", "", 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, Now)
            End Using
        Catch ex As Exception
            Throw New Exception(String.Format("({0}.SetUserList) Exception.", Me.Name), ex)
        End Try
    End Sub

    Friend Sub SetCategoryList(Optional ByVal TopicId As Integer = 0)
        Try
            Using TableAdapterCategories As New DataSetSQLRunbookTableAdapters.tCategoryTableAdapter
                TableAdapterCategories.Connection.ConnectionString = sRunbookConnectionString
                AllCategories = TableAdapterCategories.GetData()
                AllCategories.AddtCategoryRow("", "", Now, "", 0, 0, 0, Now, "")
                ListBoxCategory.DisplayMember = "Name"
                ListBoxCategory.ValueMember = "Id"
                ListBoxCategory.DataSource = AllCategories
                If Not TopicId < 1 Then
                    ' no choice but to re-select categories
                    SetSelectedCategories(TopicId)
                Else
                    ' don't leave the first item checked in this case
                    ListBoxCategory.ClearSelected()
                End If
            End Using
        Catch ex As Exception
            Throw New Exception(String.Format("({0}.SetCategoryList) Exception.", Me.Name), ex)
        End Try
    End Sub

    Private Sub SetDocTypeList()
        Try
            Using TableAdapterDocTypes As New DataSetSQLRunbookTableAdapters.fulltext_document_typesTableAdapter
                TableAdapterDocTypes.Connection.ConnectionString = sRunbookConnectionString
                TableAdapterDocTypes.ClearBeforeFill = True
                TableAdapterDocTypes.Fill(AllDocTypes, "all")
            End Using
            AllDocTypes.Addfulltext_document_typesRow("", Nothing, "", "", "")
            DataGridViewComboBoxColumnDocType.DisplayMember = "document_type"
            DataGridViewComboBoxColumnDocType.ValueMember = "document_type"
            DataGridViewComboBoxColumnDocType.DataSource = AllDocTypes

            '' don't bind, need to allow the the blanks to accept non-indexable
            'DataGridViewComboBoxColumnDocType.Items.Add("")
            'For Each r As DataRow In AllDocTypes.Rows
            '    DataGridViewComboBoxColumnDocType.Items.Add(r("document_type").ToString)
            'Next
        Catch ex As Exception
            Throw New Exception(String.Format("({0}.SetDocTypeList) Exception.", Me.Name), ex)
        End Try
    End Sub

    Private Sub SetDocumentList()
        Try
            Using TableAdapterDocuments As New DataSetSQLRunbookTableAdapters.tDocumentTableAdapter
                TableAdapterDocuments.Connection.ConnectionString = sRunbookConnectionString
                AllDocuments = TableAdapterDocuments.GetData()
                AllDocuments.AddtDocumentRow("", Nothing, "", Now, "", False, Now, "", 0, 0, Now, "")
            End Using
        Catch ex As Exception
            Throw New Exception(String.Format("({0}.SetDocumentList) Exception.", Me.Name), ex)
        End Try
    End Sub

    Private Sub SetDocumentsForSelectedTopicList(ByVal TopicName As String)
        Try
            DataGridViewComboBoxColumnDocumentOwner.DisplayMember = "OriginalLogin"
            DataGridViewComboBoxColumnDocumentOwner.ValueMember = "OriginalLogin"
            DataGridViewComboBoxColumnDocumentOwner.DataSource = AllUsers
            Using TableAdapterDocuments As New DataSetSQLRunbookTableAdapters.tDocumentTableAdapter
                TableAdapterDocuments.Connection.ConnectionString = sRunbookConnectionString
                TableAdapterDocuments.ClearBeforeFill = True
                TableAdapterDocuments.FillByTopicName(DataSetSQLRunbook.tDocument, TopicName)
            End Using
            SetDocumentDisplayColumns()
            ' make sure the rating color code is not obscured
            DataGridViewDocumentList.ClearSelection()
        Catch ex As Exception
            Throw New Exception(String.Format("({0}.SetDocumentsForSelectedTopicList) Exception.", Me.Name), ex)
        End Try
    End Sub

    Private Sub SaveLookupTopics()
        Try
            BindingSourceTopic.EndEdit()
            Using TableAdapterLookupTopic As New DataSetSQLRunbookTableAdapters.tTopicTableAdapter
                TableAdapterLookupTopic.Connection.ConnectionString = sRunbookConnectionString
                TableAdapterLookupTopic.Update(LookupTopics)
            End Using
            SetAllTopics()
        Catch ex As Exception
            Throw New Exception(String.Format("({0}.SaveLookupTopics) Exception.", Me.Name), ex)
        End Try
    End Sub

    Friend Sub SetTopicGridItems()
        Try
            DialogRunbookLookupCriteria.LoadDefaultSearch()
            Using TableAdapterTopic As New DataSetSQLRunbookTableAdapters.tTopicTableAdapter
                TableAdapterTopic.Connection.ConnectionString = sRunbookConnectionString
                TableAdapterTopic.ClearBeforeFill = True
                TableAdapterTopic.FillByCriteria(LookupTopics, _
                                                 If(DialogRunbookLookupCriteria.DateTimePickerStartDate.Checked, _
                                                    DialogRunbookLookupCriteria.DateTimePickerStartDate.Value, _
                                                    CDate("1970-01-01")), _
                                                 If(DialogRunbookLookupCriteria.DateTimePickerEndDate.Checked, _
                                                    DialogRunbookLookupCriteria.DateTimePickerEndDate.Value, _
                                                    Now), _
                                                 If(DialogRunbookLookupCriteria.ComboBoxTopic.Text = "", _
                                                    0, _
                                                    CInt(AllTopics.Select(String.Format("Name='{0}'", _
                                                                DialogRunbookLookupCriteria.ComboBoxTopic.Text))(0).Item("Id"))), _
                                                 If(DialogRunbookLookupCriteria.ComboBoxDocument.Text = "", _
                                                    0, _
                                                    CInt(AllDocuments.Select(String.Format("File='{0}'", _
                                                                DialogRunbookLookupCriteria.ComboBoxDocument.Text))(0).Item("Id"))), _
                                                 If(DialogRunbookLookupCriteria.ComboBoxCategory.Text = "", _
                                                    0, _
                                                    CInt(AllCategories.Select(String.Format("Name='{0}'", _
                                                                DialogRunbookLookupCriteria.ComboBoxCategory.Text))(0).Item("Id"))), _
                                                 DialogRunbookLookupCriteria.ComboBoxPerson.Text, _
                                                 DialogRunbookLookupCriteria.ListBoxFullText.SelectedItems.Contains("Categories"), _
                                                 DialogRunbookLookupCriteria.ListBoxFullText.SelectedItems.Contains("Topics"), _
                                                 DialogRunbookLookupCriteria.ListBoxFullText.SelectedItems.Contains("Documents"), _
                                                 DialogRunbookLookupCriteria.ListBoxFullText.SelectedItems.Contains("Ratings"), _
                                                 DialogRunbookLookupCriteria.ComboBoxRatingOperator.Text, _
                                                 CInt(DialogRunbookLookupCriteria.NumericUpDownRating.Value), _
                                                 DialogRunbookLookupCriteria.RichTextBoxContains.Text)
            End Using
            DataGridViewComboBoxColumnTopicOwner.DisplayMember = "OriginalLogin"
            DataGridViewComboBoxColumnTopicOwner.ValueMember = "OriginalLogin"
            DataGridViewComboBoxColumnTopicOwner.DataSource = AllUsers
            BindingSourceTopic.DataSource = LookupTopics
            SetTopicRatings()
            ' make sure the rating color code is not obscured
            DataGridViewTopicList.ClearSelection()
        Catch ex As Exception
            Throw New Exception(String.Format("({0}.SetTopicGridItems) Exception.", Me.Name), ex)
        End Try
    End Sub

    Private Sub SetTopicRatings()
        Try
            For Each r As DataGridViewRow In DataGridViewTopicList.Rows
                ' no division by zero
                If Not r.IsNewRow Then
                    If CInt(r.Cells("DataGridViewTextBoxColumnTopicRatingCount").Value) = 0 Then
                        r.Cells("DataGridViewButtonColumnTopicRating").Value = 0
                    Else
                        r.Cells("DataGridViewButtonColumnTopicRating").Value = _
                        CInt(r.Cells("DataGridViewTextBoxColumnTopicRatingTally").Value) / _
                             CInt(r.Cells("DataGridViewTextBoxColumnTopicRatingCount").Value)
                        ' this users last rating of this topic
                        r.Cells("DataGridViewButtonColumnTopicRating").Tag = GetRating("Topic", _
                                      CInt(r.Cells("DataGridViewTextBoxColumnTopicId").Value))
                        ' need this users rating
                        r.Cells("DataGridViewButtonColumnTopicRating").ToolTipText = _
                             String.Format("Your last rating {0}, rated by {1} {2}.", _
                                           CType(r.Cells("DataGridViewButtonColumnTopicRating").Tag, StructRating).Rating, _
                                           r.Cells("DataGridViewTextBoxColumnTopicRatingCount").Value, _
                                           If(r.Cells("DataGridViewTextBoxColumnTopicRatingCount").Value = 1, "person", "people"))
                        Select Case CInt(r.Cells("DataGridViewButtonColumnTopicRating").Value)
                            Case 0
                                r.Cells("DataGridViewButtonColumnTopicRating").Style.BackColor = My.Settings.Rating0
                            Case 1
                                r.Cells("DataGridViewButtonColumnTopicRating").Style.BackColor = My.Settings.Rating1
                            Case 2
                                r.Cells("DataGridViewButtonColumnTopicRating").Style.BackColor = My.Settings.Rating2
                            Case 3
                                r.Cells("DataGridViewButtonColumnTopicRating").Style.BackColor = My.Settings.Rating3
                            Case 4
                                r.Cells("DataGridViewButtonColumnTopicRating").Style.BackColor = My.Settings.Rating4
                            Case 5
                                r.Cells("DataGridViewButtonColumnTopicRating").Style.BackColor = My.Settings.Rating5
                            Case 6
                                r.Cells("DataGridViewButtonColumnTopicRating").Style.BackColor = My.Settings.Rating6
                            Case 7
                                r.Cells("DataGridViewButtonColumnTopicRating").Style.BackColor = My.Settings.Rating7
                        End Select
                    End If
                End If
            Next
        Catch ex As Exception
            Throw New Exception(String.Format("({0}.SetTopicRatings) Exception.", Me.Name), ex)
        End Try
    End Sub

    Friend Function GetRating(ByVal RatingType As String, _
                               ByVal TargetTypeId As Integer) As StructRating
        Dim Existing = New StructRating
        Try
            Using cn As New SqlConnection(sRunbookConnectionString)
                cn.Open()
                Using cm As New System.Data.SqlClient.SqlCommand
                    Dim LastDefinition As String = Nothing
                    cm.Connection = cn
                    cm.CommandType = CommandType.StoredProcedure
                    Select Case RatingType
                        Case "Topic"
                            cm.CommandText = "SQLRunbook.pTopicRatingGetByUser"
                        Case "Document"
                            cm.CommandText = "SQLRunbook.pDocumentRatingGetByUser"
                        Case "Category"
                            cm.CommandText = "SQLRunbook.pCategoryRatingGetByUser"
                    End Select
                    Dim TargetId As New SqlParameter()
                    With TargetId
                        .Direction = ParameterDirection.Input
                        .ParameterName = "@Id"
                        .SqlDbType = SqlDbType.Int
                        .Value = TargetTypeId
                    End With
                    cm.Parameters.Add(TargetId)
                    Dim OriginalLogin As New SqlParameter()
                    With OriginalLogin
                        .Direction = ParameterDirection.Input
                        .ParameterName = "@User"
                        .SqlDbType = SqlDbType.NVarChar
                        .Size = 128
                        .Value = My.User.Name
                    End With
                    cm.Parameters.Add(OriginalLogin)
                    Dim Rate As New SqlParameter()
                    With Rate
                        .Direction = ParameterDirection.Output
                        .ParameterName = "@Rating"
                        .SqlDbType = SqlDbType.Int
                        .Value = Existing.Rating
                    End With
                    cm.Parameters.Add(Rate)
                    Dim RatingNote As New SqlParameter()
                    With RatingNote
                        .Direction = ParameterDirection.Output
                        .ParameterName = "@Note"
                        .SqlDbType = SqlDbType.NVarChar
                        .Size = -1
                        .Value = Existing.Notes
                    End With
                    cm.Parameters.Add(RatingNote)
                    Dim AvgRat As New SqlParameter()
                    With AvgRat
                        .Direction = ParameterDirection.Output
                        .ParameterName = "@AvgRating"
                        .SqlDbType = SqlDbType.Int
                        '.Value = PeerReview.AvgRating
                    End With
                    cm.Parameters.Add(AvgRat)
                    Dim NbrRat As New SqlParameter()
                    With NbrRat
                        .Direction = ParameterDirection.Output
                        .ParameterName = "@NbrRatings"
                        .SqlDbType = SqlDbType.Int
                        '.Value = PeerReview.NbrRating
                    End With
                    cm.Parameters.Add(NbrRat)
                    cm.ExecuteNonQuery()
                    Existing.Rating = CInt(Rate.Value)
                    Existing.Notes = RatingNote.Value.ToString
                    Existing.AvgRating = CInt(AvgRat.Value)
                    Existing.NbrRatings = CInt(NbrRat.Value)
                    GetRating = Existing
                End Using
            End Using
        Catch ex As Exception
            Throw New Exception(String.Format("({0}.GetTopicRating) Exception.", Me.Name), ex)
        End Try
    End Function

    Private Function GetDocument(ByVal DocumentId As Integer) As Byte()
        Try
            Using cn As New SqlConnection(sRunbookConnectionString)
                cn.Open()
                Using cm As SqlCommand = cn.CreateCommand
                    cm.CommandText = "SQLRunbook.pDocumentGet"
                    cm.CommandType = CommandType.StoredProcedure
                    Dim DocId As New SqlParameter()
                    With DocId
                        .Direction = ParameterDirection.Input
                        .ParameterName = "@Id"
                        .SqlDbType = SqlDbType.Int
                        .Value = DocumentId
                        cm.Parameters.Add(DocId)
                    End With
                    Dim Doc As New SqlParameter()
                    With Doc
                        .Direction = ParameterDirection.Output
                        .ParameterName = "@Document"
                        .SqlDbType = SqlDbType.VarBinary
                        .Size = -1
                        cm.Parameters.Add(Doc)
                    End With
                    cm.BeginExecuteNonQuery()
                    GetDocument = CType(Doc.Value, Byte())
                End Using
            End Using
        Catch ex As Exception
            Throw New Exception(String.Format("({0}.GetDocument) Exception.", Me.Name), ex)
        End Try
    End Function

    Private Function GetSavedFileInfo(ByVal FileName As String) As StructDocInfo
        Try
            Using cn As New SqlConnection(sRunbookConnectionString)
                cn.Open()
                Using cm As SqlCommand = cn.CreateCommand
                    cm.CommandText = "SQLRunbook.pDocumentGetByFile"
                    cm.CommandType = CommandType.StoredProcedure
                    Dim DocFile As New SqlParameter()
                    With DocFile
                        .Direction = ParameterDirection.Input
                        .ParameterName = "@File"
                        .SqlDbType = SqlDbType.NVarChar
                        .Size = 450
                        .Value = FileName
                        cm.Parameters.Add(DocFile)
                    End With
                    Dim DocId As New SqlParameter()
                    With DocId
                        .Direction = ParameterDirection.Output
                        .ParameterName = "@Id"
                        .SqlDbType = SqlDbType.Int
                        cm.Parameters.Add(DocId)
                    End With
                    Dim DocLen As New SqlParameter()
                    With DocLen
                        .Direction = ParameterDirection.Output
                        .ParameterName = "@DocumentLength"
                        .SqlDbType = SqlDbType.Int
                        cm.Parameters.Add(DocLen)
                    End With
                    Dim DocTyp As New SqlParameter()
                    With DocTyp
                        .Direction = ParameterDirection.Output
                        .ParameterName = "@DocumentType"
                        .SqlDbType = SqlDbType.NVarChar
                        .Size = 8
                        cm.Parameters.Add(DocTyp)
                    End With
                    Dim LastModDt As New SqlParameter()
                    With LastModDt
                        .Direction = ParameterDirection.Output
                        .ParameterName = "@LastModifiedDt"
                        .SqlDbType = SqlDbType.DateTime
                        cm.Parameters.Add(LastModDt)
                    End With
                    Dim Onr As New SqlParameter()
                    With Onr
                        .Direction = ParameterDirection.Output
                        .ParameterName = "@Owner"
                        .SqlDbType = SqlDbType.NVarChar
                        .Size = 128
                        cm.Parameters.Add(Onr)
                    End With
                    Dim WatchForChange As New SqlParameter()
                    With WatchForChange
                        .Direction = ParameterDirection.Output
                        .ParameterName = "@WatchFileForChange"
                        .SqlDbType = SqlDbType.Bit
                        cm.Parameters.Add(WatchForChange)
                    End With
                    Dim IsAdmn As New SqlParameter()
                    With IsAdmn
                        .Direction = ParameterDirection.Output
                        .ParameterName = "@IsAdmin"
                        .SqlDbType = SqlDbType.Bit
                        cm.Parameters.Add(IsAdmn)
                    End With
                    cm.ExecuteNonQuery()
                    If Not DBNull.Value.Equals(DocId.Value) Then
                        GetSavedFileInfo.Id = CInt(DocId.Value)
                        GetSavedFileInfo.FileName = FileName
                        GetSavedFileInfo.DocumentLength = CInt(DocLen.Value)
                        GetSavedFileInfo.DocumentType = DocTyp.Value.ToString
                        GetSavedFileInfo.LastModifiedDt = CDate(LastModDt.Value)
                        GetSavedFileInfo.Owner = Onr.Value.ToString
                        GetSavedFileInfo.WatchFileForChange = CBool(WatchForChange.Value)
                        GetSavedFileInfo.IsAdmin = CBool(IsAdmn.Value)
                    Else
                        GetSavedFileInfo.Id = 0
                        GetSavedFileInfo.FileName = ""
                        GetSavedFileInfo.DocumentLength = 0
                        GetSavedFileInfo.DocumentType = ""
                        GetSavedFileInfo.Owner = My.User.Name
                        GetSavedFileInfo.WatchFileForChange = My.Settings.ServiceEnabled
                        GetSavedFileInfo.IsAdmin = CBool(IsAdmn.Value)
                    End If
                End Using
            End Using
        Catch ex As Exception
            Throw New Exception(String.Format("({0}.GetSaveFileInfo) Exception.", Me.Name), ex)
        End Try
    End Function

    Friend Function GetTopicDocumentsByDocumentId(ByVal DocumentId As Integer) As DataSetSQLRunbook.tTopicDocumentDataTable
        Try
            Using TableAdapterTopicDocument As New DataSetSQLRunbookTableAdapters.tTopicDocumentTableAdapter
                TableAdapterTopicDocument.ClearBeforeFill = True
                TableAdapterTopicDocument.Connection.ConnectionString = sRunbookConnectionString
                GetTopicDocumentsByDocumentId = TableAdapterTopicDocument.GetDataByDocumentId(DocumentId)
            End Using
        Catch ex As Exception
            Throw New Exception(String.Format("({0}.GetTopicDocumentsByDocumentId) Exception.", Me.Name), ex)
        End Try
    End Function

    Friend Sub SetTopicDocumentsByTopicId(ByVal TopicId As Integer)
        Try
            Using TableAdapterTopicDocument As New DataSetSQLRunbookTableAdapters.tTopicDocumentTableAdapter
                TableAdapterTopicDocument.ClearBeforeFill = True
                TableAdapterTopicDocument.Connection.ConnectionString = sRunbookConnectionString
                TableAdapterTopicDocument.FillByTopicId(DataSetSQLRunbook.tTopicDocument, TopicId)
            End Using
        Catch ex As Exception
            Throw New Exception(String.Format("({0}.GetTopicDocumentsByDocumentId) Exception.", Me.Name), ex)
        End Try
    End Sub

    Friend Sub SetAllTopics()
        Try
            AllTopics.Clear()
            Using TableAdapterTopic As New DataSetSQLRunbookTableAdapters.tTopicTableAdapter
                TableAdapterTopic.Connection.ConnectionString = sRunbookConnectionString
                AllTopics = TableAdapterTopic.GetData
            End Using
            Dim savetopic As String = ComboBoxTopicName.Text
            ComboBoxTopicName.Items.Clear()
            ComboBoxTopicName.Items.Add("")
            For Each r As DataRow In AllTopics
                ComboBoxTopicName.Items.Add(r("Name"))
            Next
            ComboBoxTopicName.Text = savetopic
        Catch ex As Exception
            Throw New Exception(String.Format("({0}.SetAllTopics) Exception.", Me.Name), ex)
        End Try
    End Sub

    Private Sub SetSelectedCategories(ByVal TopicId As Integer)
        Try
            Using TableAdapterCategoryTopic As New DataSetSQLRunbookTableAdapters.tCategoryTopicTableAdapter
                TableAdapterCategoryTopic.ClearBeforeFill = True
                TableAdapterCategoryTopic.Connection.ConnectionString = sRunbookConnectionString
                TableAdapterCategoryTopic.FillByTopicId(DataSetSQLRunbook.tCategoryTopic, _
                                                        CInt(TopicId))
            End Using
            ListBoxCategory.SelectedItems.Clear()
            For Each cat As DataSetSQLRunbook.tCategoryTopicRow _
            In DataSetSQLRunbook.tCategoryTopic.Rows
                ListBoxCategory.SetSelected(ListBoxCategory.FindStringExact(AllCategories.FindById(cat.CategoryId).Name), True)
            Next
        Catch ex As Exception
            Throw New Exception(String.Format("({0}.SetSelectedCategories) Exception.", Me.Name), ex)
        End Try
    End Sub

    Private Sub SaveSelectedCategories(ByVal TopicId As Integer)
        Try
            'remove any not now selected from associative table 
            'add any newly selected to associative table
            For Each cat As DataRowView In ListBoxCategory.Items
                Dim r As DataSetSQLRunbook.tCategoryTopicRow = _
                      DataSetSQLRunbook.tCategoryTopic.FindByCategoryIdTopicId(CInt(cat.Row.Item("Id")), TopicId)
                If Not r Is Nothing _
                And Not ListBoxCategory.SelectedItems.Contains(cat) Then
                    r.Delete()
                End If
                If r Is Nothing _
                And ListBoxCategory.SelectedItems.Contains(cat) Then
                    Dim selcat As DataSetSQLRunbook.tCategoryTopicRow = DataSetSQLRunbook.tCategoryTopic.NewtCategoryTopicRow
                    selcat.CategoryId = CInt(cat.Item("Id"))
                    selcat.TopicId = TopicId
                    DataSetSQLRunbook.tCategoryTopic.AddtCategoryTopicRow(selcat)
                End If
            Next
            Using TableAdapterCategoryTopic As New DataSetSQLRunbookTableAdapters.tCategoryTopicTableAdapter
                TableAdapterCategoryTopic.Connection.ConnectionString = sRunbookConnectionString
                TableAdapterCategoryTopic.Update(DataSetSQLRunbook.tCategoryTopic)
            End Using
        Catch ex As Exception
            Throw New Exception(String.Format("({0}.SaveSelectedCategories) Exception.", Me.Name), ex)
        End Try
    End Sub

    Private Sub SaveDocumentChanges()
        For Each r As DataGridViewRow In DataGridViewDocumentList.Rows
            If Not r.IsNewRow Then
                ' has to be doctype, owner or watch flag
                ' update the three columns that can change in DGV
                ' using tableadapter would require fetching each document
                ' this proc only updates of any of the 3 columns have changed
                Using cn As New SqlConnection(sRunbookConnectionString)
                    cn.Open()
                    Using cm As SqlCommand = cn.CreateCommand
                        cm.CommandText = "SQLRunbook.pDocumentFormUpdate"
                        cm.CommandType = CommandType.StoredProcedure
                        Dim DocId As New SqlParameter()
                        With DocId
                            .Direction = ParameterDirection.Input
                            .ParameterName = "@Id"
                            .SqlDbType = SqlDbType.Int
                            .Value = CInt(r.Cells("DataGridViewTextBoxColumnDocumentId").Value)
                            cm.Parameters.Add(DocId)
                        End With
                        Dim Extension As New SqlParameter()
                        With Extension
                            .Direction = ParameterDirection.Input
                            .ParameterName = "@DocumentType"
                            .SqlDbType = SqlDbType.NVarChar
                            .Size = 8
                            .Value = r.Cells("DataGridViewComboBoxColumnDocType").Value.ToString()
                            cm.Parameters.Add(Extension)
                        End With
                        Dim Owner As New SqlParameter()
                        With Owner
                            .Direction = ParameterDirection.Input
                            .ParameterName = "@Owner"
                            .SqlDbType = SqlDbType.NVarChar
                            .Size = 128
                            .Value = If(r.Cells("DataGridViewComboBoxColumnDocumentOwner").Value = "", My.User.Name, _
                                        r.Cells("DataGridViewComboBoxColumnDocumentOwner").Value.ToString())
                            cm.Parameters.Add(Owner)
                        End With
                        Dim WatchFileForChange As New SqlParameter()
                        With WatchFileForChange
                            .Direction = ParameterDirection.Input
                            .ParameterName = "@WatchFileForChange"
                            .SqlDbType = SqlDbType.Bit
                            .Value = CBool(r.Cells("DataGridViewCheckBoxDocumentWatchFileForChange").Value)
                            cm.Parameters.Add(WatchFileForChange)
                        End With
                        cm.ExecuteNonQuery()
                    End Using
                End Using
            End If
        Next
    End Sub

    Private Sub SaveChanges(ByVal TopicId As Integer)
        Try
            If Not TopicId = 0 _
            AndAlso DataSetSQLRunbook.tTopic.First.RowState = DataRowState.Modified Then
                Using TableAdapterTopic As New DataSetSQLRunbookTableAdapters.tTopicTableAdapter
                    TableAdapterTopic.Connection.ConnectionString = sRunbookConnectionString
                    TableAdapterTopic.Update(DataSetSQLRunbook.tTopic)
                End Using
                PanelTopicDetail.ForeColor = PanelTopicList.ForeColor
                PanelTopicDetail.BackColor = PanelTopicList.BackColor
            End If
            If _CategoriesChanged Then
                ' save category selection changes
                If Not TopicId = 0 Then
                    SaveSelectedCategories(TopicId)
                End If
                PanelCategoryList.ForeColor = PanelTopicList.ForeColor
                PanelCategoryList.BackColor = PanelTopicList.BackColor
                _CategoriesChanged = False
            End If
            If _DocumentChanged Then
                ' owner, watch flag or doc type
                SaveDocumentChanges()
                PanelDocumentList.ForeColor = PanelTopicList.ForeColor
                PanelDocumentList.BackColor = PanelTopicList.BackColor
                _DocumentChanged = False
            End If
            DataSetSQLRunbook.AcceptChanges()
            ToolStripStatusLabelRunbook.Text = "Topic Saved"
        Catch ex As Exception
            Throw New Exception(String.Format("({0}.SaveChanges) Exception.", Me.Name), ex)
        End Try
    End Sub

#End Region

#Region " edit the config file Category List "
    'Private Sub LinkLabel1_LinkClicked(ByVal sender As System.Object, _
    '                                  ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) _
    '                                 Handles LinkLabelCategoryMaintenance.LinkClicked
    'Dim config As System.Configuration.Configuration = _
    'ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None)

    'Dim sectionGroups As ConfigurationSectionGroupCollection = config.SectionGroups

    '   ShowSectionGroupCollectionInfo(sectionGroups)

    '    End Sub

    ''static void?
    '   Private Sub ShowSectionGroupCollectionInfo(ByVal sectionGroups As ConfigurationSectionGroupCollection)

    '  Dim clientSection As ClientSettingsSection
    ' Dim value As SettingValueElement
    '    For Each group As ConfigurationSectionGroup In sectionGroups
    '       If Not (group.IsDeclared) Then
    '' Only the ones which are actually defined in app.config
    '            Exit For
    '        End If

    '        Debug.WriteLine("Group {0}", group.Name)

    '    ' get all sections inside group

    '         For Each section As ConfigurationSection In group.Sections
    '            clientSection = CType(section, ClientSettingsSection)
    '           Debug.WriteLine("\tSection: {0}", section.ToString)
    '
    '           If clientSection Is Nothing Then
    '              Exit For
    '         End If


    '        For Each st As SettingElement In clientSection.Settings
    '           value = CType(st.Value, SettingValueElement)
    '' print out value of each section
    '              Console.WriteLine("\t\t{0}: {1}", _
    '            st.Name, value.ValueXml.InnerText)
    '        Next
    '   Next
    ' Next

    '    End Sub
#End Region

#Region " TopicDetail "

    Private Sub SetTopicDetailItems(ByVal TopicId As Int32)
        Try
            DataSetSQLRunbook.Clear()
            If TopicId > 0 Then
                DataSetSQLRunbook.tTopic.ImportRow(AllTopics.FindById(TopicId))
                ComboBoxTopicName.Text = DataSetSQLRunbook.tTopic(0).Name.ToString
                'loads cat list and marks selected
                SetSelectedCategories(TopicId)
                ' no doc now, just rest of row
                SetDocumentsForSelectedTopicList(ComboBoxTopicName.Text)
                ' load the xref data for the selected topic
                SetTopicDocumentsByTopicId(TopicId)
            Else
                ClearDetailPanel()
            End If
        Catch ex As Exception
            Throw New Exception(String.Format("({0}.SetTopicDetailItems) Exception.", Me.Name), ex)
        End Try
    End Sub

    Private Sub SetDocumentDisplayColumns()
        Try
            For Each r As DataGridViewRow In DataGridViewDocumentList.Rows
                If Not r.IsNewRow Then
                    ' Rating
                    ' no division by zero
                    If CInt(r.Cells("DataGridViewTextBoxColumnDocumentRatingCount").Value) = 0 Then
                        r.Cells("DataGridViewButtonColumnDocumentRating").Value = 0
                    Else
                        r.Cells("DataGridViewButtonColumnDocumentRating").Value = _
                        CInt(r.Cells("DataGridViewTextBoxColumnDocumentRatingTally").Value) / _
                               CInt(r.Cells("DataGridViewTextBoxColumnDocumentRatingCount").Value)
                        ' this users last rating of this topic
                        r.Cells("DataGridViewButtonColumnDocumentRating").Tag = GetRating("Document", _
                                      CInt(r.Cells("DataGridViewTextBoxColumnDocumentId").Value))
                        ' need this users rating
                        r.Cells("DataGridViewButtonColumnDocumentRating").ToolTipText = _
                        String.Format("Your last rating {0}, rated by {1} {2}.", _
                                      CType(r.Cells("DataGridViewButtonColumnDocumentRating").Tag, StructRating).Rating, _
                                      r.Cells("DataGridViewTextBoxColumnDocumentRatingCount").Value, _
                                      If(r.Cells("DataGridViewTextBoxColumnDocumentRatingCount").Value = 1, "person", "people"))
                        Select Case CInt(r.Cells("DataGridViewButtonColumnDocumentRating").Value)
                            Case 0
                                r.Cells("DataGridViewButtonColumnDocumentRating").Style.BackColor = My.Settings.Rating0
                            Case 1
                                r.Cells("DataGridViewButtonColumnDocumentRating").Style.BackColor = My.Settings.Rating1
                            Case 2
                                r.Cells("DataGridViewButtonColumnDocumentRating").Style.BackColor = My.Settings.Rating2
                            Case 3
                                r.Cells("DataGridViewButtonColumnDocumentRating").Style.BackColor = My.Settings.Rating3
                            Case 4
                                r.Cells("DataGridViewButtonColumnDocumentRating").Style.BackColor = My.Settings.Rating4
                            Case 5
                                r.Cells("DataGridViewButtonColumnDocumentRating").Style.BackColor = My.Settings.Rating5
                            Case 6
                                r.Cells("DataGridViewButtonColumnDocumentRating").Style.BackColor = My.Settings.Rating6
                        End Select
                    End If
                    ' ShortFile in column, full path in tooltip
                    Dim s() As String
                    s = r.Cells("DataGridViewButtonColumnDocumentFile").Value.ToString.Split(CChar("\"))
                    r.Cells("DataGridViewButtonDocumentShortFile").ToolTipText = r.Cells("DataGridViewButtonColumnDocumentFile").Value.ToString
                    r.Cells("DataGridViewButtonDocumentShortFile").Value = _
                         s.GetValue(s.GetUpperBound(0)).ToString
                End If
            Next
        Catch ex As Exception
            Throw New Exception(String.Format("({0}.SetDocumentDisplayColumns) Exception.", Me.Name), ex)
        End Try
    End Sub

    Friend Sub ClearDetailPanel()
        Try
            ComboBoxTopicName.Text = ""
            ListBoxCategory.SelectedItems.Clear()
            If _CategoriesChanged Then
                PanelCategoryList.BackColor = PanelTopicList.BackColor
                PanelCategoryList.ForeColor = PanelTopicList.ForeColor
                _CategoriesChanged = False
            End If
            If _DocumentChanged Then
                PanelDocumentList.BackColor = PanelTopicList.BackColor
                PanelDocumentList.ForeColor = PanelTopicList.ForeColor
                _DocumentChanged = False
            End If
            SplitContainerResults.Panel1.Enabled = True
        Catch ex As Exception
            Throw New Exception(String.Format("({0}.ClearDetailPanel) Exception.", Me.Name), ex)
        End Try
    End Sub

    Private Sub ButtonCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonCancel.Click
        Try
            LoadRunbook(If(DataSetSQLRunbook.tTopic.Rows.Count = 0, _
                           0, _
                           CInt(AllTopics.Select(String.Format("Name='{0}'", ""))(0)("Id"))))
        Catch ex As Exception
            Mother.HandleException(New Exception(String.Format("({0}.ButtonCancel_Click) Exception.", Me.Name), ex))
        End Try
    End Sub

    Private Sub ButtonSave_Click(ByVal sender As System.Object, _
                                      ByVal e As System.EventArgs) _
                                      Handles ButtonSave.Click
        Dim csr As Cursor = Me.Cursor
        Try
            Me.Cursor = Cursors.WaitCursor
            ' this should not happen because you have to enter a name to get here ... I think
            If DataSetSQLRunbook.tTopic.Rows.Count = 0 Then
                ToolStripStatusLabelRunbook.Text = "No Topic Selected"
            Else
                Dim TopicId As Integer = CInt(DataSetSQLRunbook.tTopic.Rows(0).Item("Id"))
                SplitContainerResults.Panel1.Enabled = True
                If AllTopics.FindById(TopicId).Name.ToString = ComboBoxTopicName.Text Then
                    'topic exists but not in last critera lookup so reset criteria to find it
                    DialogRunbookLookupCriteria.DateTimePickerStartDate.Checked = False
                    DialogRunbookLookupCriteria.DateTimePickerEndDate.Checked = False
                    DialogRunbookLookupCriteria.ComboBoxCategory.Text = ""
                    DialogRunbookLookupCriteria.ComboBoxDocument.Text = ""
                    DialogRunbookLookupCriteria.ComboBoxPerson.Text = ""
                    DialogRunbookLookupCriteria.ComboBoxRatingOperator.Text = ">"
                    DialogRunbookLookupCriteria.NumericUpDownRating.Value = 0


                    '????? may not work need to test
                    DialogRunbookLookupCriteria.ComboBoxTopic.SelectedItem = ComboBoxTopicName.Text

                    DialogRunbookLookupCriteria.RichTextBoxContains.Text = ""
                End If
                SaveChanges(TopicId)
                LoadRunbook(TopicId)
                ' SaveChanges writes a completion message
                If ListBoxCategory.SelectedItems.Count = 0 Then
                    ' want to have some categories selected
                    ToolStripStatusLabelRunbook.Text = ToolStripStatusLabelRunbook.Text & Space(1) & "No Categories have been selected."
                End If
            End If
        Catch ex As Exception
            Mother.HandleException(New Exception(String.Format("({0}.ButtonSaveTopic_Click) Exception.", Me.Name), ex))
        Finally
            Me.Cursor = csr
        End Try
    End Sub

#End Region

    'Private Sub ComboBoxTopicName_DropDown(ByVal sender As Object, _
    '                                       ByVal e As System.EventArgs) _
    '                                       Handles ComboBoxTopicName.DropDown
    '    Dim csr As Cursor = Me.Cursor
    '    Try
    '        Me.Cursor = Cursors.WaitCursor   ' Display the waiting cursor
    '        If ComboBoxTopicName.Items.Count = 0 Then
    '            SetAllTopics()
    '        End If
    '    Catch ex As Exception
    '        Mother.HandleException(New Exception(String.Format("({0}.ComboBoxTopicName_DropDown) Exception.", Me.Name), ex))
    '    Finally
    '        Me.Cursor = csr
    '    End Try
    'End Sub

    Private Sub DataGridViewDocumentList_CellClick(ByVal sender As Object, _
                                                   ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) _
                                                   Handles DataGridViewDocumentList.CellClick
        Try
            If Not DataSetSQLRunbook.tTopic.Rows.Count = 0 Then
                Dim DocumentId As Integer
                Dim IsWatched As Boolean = True
                Dim FileName As String = ""
                Dim SelectedTopicId As Integer = CInt(DataSetSQLRunbook.tTopic.Rows(0).Item("Id"))
                Dim SelectedTopicName As String = DataSetSQLRunbook.tTopic.Rows(0).Item("Name").ToString
                ' if clicked column headers, must be sorting
                Select Case e.RowIndex
                    Case -1
                        If e.ColumnIndex = 1 Then
                            ' reload the DGV
                            SetDocumentsForSelectedTopicList(SelectedTopicName)
                        ElseIf e.ColumnIndex = 3 Then ' shortfile
                            'need to sort unbound column
                            DataGridViewDocumentList.Sort(DataGridViewButtonColumnDocumentFile, ListSortDirection.Ascending)
                        End If
                    Case Else
                        If DataGridViewDocumentList.CurrentRow.IsNewRow Then
                            LaunchDocumentDialog("")
                        Else
                            ' need some working values from the row
                            DocumentId = CInt(DataGridViewDocumentList.Rows(e.RowIndex).Cells("DataGridViewTextBoxColumnDocumentId").Value)
                            IsWatched = CBool(DataGridViewDocumentList.Rows(e.RowIndex).Cells("DataGridViewCheckBoxDocumentWatchFileForChange").Value)
                            FileName = DataGridViewDocumentList.Rows(e.RowIndex).Cells("DataGridViewButtonColumnDocumentFile").Value.ToString
                            ' get a reference copy of the document 
                            Dim SavedFileInfo As StructDocInfo = GetSavedFileInfo(FileName)
                            Select Case e.ColumnIndex
                                Case -1 'assert: delete request
                                    ' row header click is a request to delete?
                                    Dim td As DataSetSQLRunbook.tTopicDocumentDataTable
                                    Using TableAdapterTopicDocument As New DataSetSQLRunbookTableAdapters.tTopicDocumentTableAdapter
                                        TableAdapterTopicDocument.Connection.ConnectionString = sRunbookConnectionString
                                        td = TableAdapterTopicDocument.GetDataByDocumentId(DocumentId)
                                    End Using
                                    ' could use the dataset topicdoc table but this will eliminate race
                                    If td.Count = 0 Then
                                        ToolStripStatusLabelRunbook.Text = String.Format("Topic Document Corrupt.")
                                    ElseIf td.Count = 1 _
                                    AndAlso CInt(td.Rows(0).Item("TopicId")) = SelectedTopicId Then
                                        Dim r As DialogResult
                                        If Not IsWatched Then
                                            r = MessageBox.Show(String.Format("Remove the Document from the Runbook Topic?"), _
                                                                   "", _
                                                                   MessageBoxButtons.OKCancel, _
                                                                   MessageBoxIcon.Question, _
                                                                   MessageBoxDefaultButton.Button2, _
                                                                   0, _
                                                                   False)
                                        Else
                                            r = DialogRunbookSecurity.ShowDialog(FileName, _
                                                                                 False, _
                                                                                 "remove the Document from the Runbook Topic", _
                                                                                 Me)
                                        End If
                                        If r = Windows.Forms.DialogResult.OK Or r = Windows.Forms.DialogResult.Ignore Then
                                            ' remove the topicdocument and then the document row
                                            DataSetSQLRunbook.tTopicDocument.FindByTopicIdDocumentId(SelectedTopicId, DocumentId).Delete()
                                            Using TableAdapterTopicDocument = New DataSetSQLRunbookTableAdapters.tTopicDocumentTableAdapter
                                                TableAdapterTopicDocument.Connection.ConnectionString = sRunbookConnectionString
                                                TableAdapterTopicDocument.Update(DataSetSQLRunbook.tTopicDocument)
                                            End Using
                                            DataGridViewDocumentList.Rows.RemoveAt(e.RowIndex)
                                            ToolStripStatusLabelRunbook.Text = String.Format("Document Removed")
                                        Else
                                            Exit Try
                                        End If
                                    Else
                                        DataSetSQLRunbook.tTopicDocument.FindByTopicIdDocumentId(SelectedTopicId, DocumentId).Delete()
                                        ToolStripStatusLabelRunbook.Text = String.Format("Topic's Document Reference Removed")
                                    End If
                                    SaveChanges(SelectedTopicId)
                                    LoadRunbook(SelectedTopicId)
                                Case 0 ' rating button

                                    ' will get saved in dialog
                                    DialogRating.ShowDialog("Document", _
                                                          DocumentId, _
                                                          FileName, _
                                                          sRunbookConnectionString, _
                                                          Me)
                                    If Not DialogRating.PeerReview.NewRating = 0 _
                                    AndAlso DialogRating.PeerReview.Rating <> DialogRating.PeerReview.NewRating Then
                                        ' reload ratings
                                        SetDocumentsForSelectedTopicList(SelectedTopicName)
                                    End If
                                    DialogRating.Dispose()
                                    DataGridViewDocumentList.ClearSelection()
                                Case 1 ' id
                                Case 2, 3 ' file, short file
                                    LaunchDocumentDialog(FileName)
                                Case 4 ' open document
                                    OpenDocument(FileName)
                                Case 5 ' doc type
                                    'CurrentCellDirtyStateChanged will handle changes
                                Case 6 ' owner
                                    'CurrentCellDirtyStateChanged will handle changes
                                Case 7 ' LastModifiedDt
                                Case 8 ' rating tally
                                Case 9 ' rating count
                                Case 10 ' watch file for changes
                                    If My.Settings.ServiceEnabled Then
                                        'change the check state when the cell is clicked
                                        If Not DialogRunbookSecurity.ShowDialog(FileName, _
                                                                                IsWatched, _
                                                                                String.Format("{0} watching this document for changes", _
                                                                                              If(IsWatched, "begin", "stop ")), _
                                                                                Me) = Windows.Forms.DialogResult.OK Then
                                            DataGridViewDocumentList.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = Not (IsWatched)
                                        Else
                                            If Not CBool(DataGridViewDocumentList.Rows(e.RowIndex).Cells(e.ColumnIndex).Value) = SavedFileInfo.WatchFileForChange Then
                                                SaveDocumentChanges()
                                                SetDocumentsForSelectedTopicList(SelectedTopicName)
                                            End If
                                        End If
                                    Else
                                        MessageBox.Show("Automation Controller must be installed before document change monitoing can be enabled.", _
                                                        "File Watcher Unavailable", _
                                                        MessageBoxButtons.OK, _
                                                        MessageBoxIcon.Error, _
                                                        MessageBoxDefaultButton.Button1, _
                                                        MessageBoxOptions.ServiceNotification, _
                                                        False)
                                        DataGridViewDocumentList.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = False
                                    End If
                            End Select
                        End If
                End Select
            End If
        Catch ex As Exception
            Mother.HandleException(New Exception(String.Format("({0}.DataGridViewDocumentList_CellClick) Exception.", Me.Name), ex))
        End Try
    End Sub

    Public Sub OpenDocument(ByVal FileName As String)
        Dim p As New Process
        p.StartInfo.CreateNoWindow = False
        p.StartInfo.FileName = FileName
        p.StartInfo.UseShellExecute = True
        Try
            p.Start()
        Catch ex As Exception
            If (MessageBox.Show(String.Format("Error {0}.", ex.GetBaseException.Message) & vbCrLf & vbCrLf & _
                                              "Try to restore the file image stored in the database?", _
                                              "Document Open Failure", MessageBoxButtons.YesNo) = DialogResult.Yes) Then
                ExportDocument(FileName, _
                               FileName)
            End If
        End Try
    End Sub


    Private Sub ExportDocument(ByVal FileName As String, _
                               ByVal ExportFileName As String)
        Dim fStream As New FileStream(ExportFileName, _
                                  FileMode.CreateNew, _
                                  FileAccess.ReadWrite)
        Try
            ' get the doc from the db 
            Dim SavedFileInfo = GetSavedFileInfo(FileName)
            If SavedFileInfo.DocumentLength > 0 Then
                Dim Document(SavedFileInfo.DocumentLength) As Byte
                Document = GetDocument(SavedFileInfo.Id)
                If Not Mother.ToolStripMenuItemEnforceOwnership.Checked _
                OrElse UCase(My.User.Name) = UCase(SavedFileInfo.Owner) OrElse _
                SavedFileInfo.IsAdmin Then
                    fStream.Write(Document, 0, Document.Length)
                Else
                    Throw (New Exception(String.Format("{0} is not authorized to create requested Document, DocId: {1}.", _
                                                       My.User.Name, _
                                                       SavedFileInfo.Id)))
                End If
            End If
        Catch ex As Exception
            Mother.HandleException(New Exception(String.Format("({0}.ExportDocument) Exception.", Me.Name), ex))
        Finally
            fStream.Dispose()
            fStream = Nothing
        End Try
    End Sub

    Private Sub LaunchDocumentDialog(ByVal FileName As String)
        Try
            If DataSetSQLRunbook.tTopic.Rows.Count = 0 Then
                ToolStripStatusLabelRunbook.Text = "Create or add topic before adding documents."
            Else
                OpenFileDialogDocument.SupportMultiDottedExtensions = True
                If Not FileName = "" Then
                    OpenFileDialogDocument.InitialDirectory = Mid(FileName, _
                                                                  1, _
                                                                  InStrRev(FileName, _
                                                                           "\"))
                    OpenFileDialogDocument.FileName = Mid(FileName, _
                                                          InStrRev(FileName, _
                                                                   "\") + 1)
                End If
                If OpenFileDialogDocument.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                    ' create the document row and a topicdocument row
                    Dim fInfo As New FileInfo(OpenFileDialogDocument.FileName)
                    Dim SavedFileInfo = GetSavedFileInfo(OpenFileDialogDocument.FileName)
                    If SavedFileInfo.Id = 0 _
                    OrElse SavedFileInfo.Owner Is Nothing _
                    OrElse DateDiff(DateInterval.Second, fInfo.LastWriteTime, SavedFileInfo.LastModifiedDt) = 0 Then
                        'Admin can change user name to a friendly name
                        ' this has a risk if is assigned an admin acount and a 'normal' account
                        Dim result As DialogResult = DialogRunbookSecurity.ShowDialog(fInfo.FullName.ToString, _
                                                                                      True, _
                                                                                      "add document to Runbook Topic", _
                                                                                      Me)
                        If result = Windows.Forms.DialogResult.OK Or result = Windows.Forms.DialogResult.Ignore Then
                            ' ignore means the service is not installed
                            ' get the cart before the horse and use fInfo size to ReadBytes
                            Dim fStream As New FileStream(fInfo.FullName, _
                                                          FileMode.Open, _
                                                          FileAccess.Read)
                            Dim br As New BinaryReader(fStream)
                            Dim Document() As Byte = br.ReadBytes(CInt(fInfo.Length) - 1)
                            br.Close()
                            fStream.Close()
                            Using cn As New SqlConnection(sRunbookConnectionString)
                                cn.Open()
                                Using cm As SqlCommand = cn.CreateCommand
                                    cm.CommandText = "SQLRunbook.pDocumentUpsert"
                                    cm.CommandType = CommandType.StoredProcedure
                                    Dim FName As New SqlParameter()
                                    With FName
                                        .Direction = ParameterDirection.Input
                                        .ParameterName = "@File"
                                        .SqlDbType = SqlDbType.NVarChar
                                        .Size = 450
                                        .Value = fInfo.FullName.ToString
                                        cm.Parameters.Add(FName)
                                    End With
                                    Dim Doc As New SqlParameter()
                                    With Doc
                                        .Direction = ParameterDirection.Input
                                        .ParameterName = "@Document"
                                        .SqlDbType = SqlDbType.VarBinary
                                        .Size = -1
                                        .Value = Document
                                        cm.Parameters.Add(Doc)
                                    End With
                                    Dim Extension As New SqlParameter()
                                    With Extension
                                        .Direction = ParameterDirection.Input
                                        .ParameterName = "@DocumentType"
                                        .SqlDbType = SqlDbType.NVarChar
                                        .Size = 8
                                        .Value = fInfo.Extension.ToString
                                        cm.Parameters.Add(Extension)
                                    End With
                                    Dim ModDate As New SqlParameter()
                                    With ModDate
                                        .Direction = ParameterDirection.Input
                                        .ParameterName = "@LastModifiedDt"
                                        .SqlDbType = SqlDbType.DateTime
                                        .Value = fInfo.LastWriteTime
                                        cm.Parameters.Add(ModDate)
                                    End With
                                    Dim Owner As New SqlParameter()
                                    With Owner
                                        .Direction = ParameterDirection.Input
                                        .ParameterName = "@Owner"
                                        .SqlDbType = SqlDbType.NVarChar
                                        .Size = 128
                                        .Value = SavedFileInfo.Owner
                                        cm.Parameters.Add(Owner)
                                    End With
                                    Dim WatchFileForChange As New SqlParameter()
                                    With WatchFileForChange
                                        .Direction = ParameterDirection.Input
                                        .ParameterName = "@WatchFileForChange"
                                        .SqlDbType = SqlDbType.Bit
                                        .Value = SavedFileInfo.WatchFileForChange
                                        cm.Parameters.Add(WatchFileForChange)
                                    End With
                                    Dim DocumentId As New SqlParameter()
                                    With DocumentId
                                        .Direction = ParameterDirection.Output
                                        .ParameterName = "@DocumentId"
                                        .SqlDbType = SqlDbType.Int
                                        cm.Parameters.Add(DocumentId)
                                    End With
                                    cm.ExecuteNonQuery()
                                    SavedFileInfo.Id = CInt(DocumentId.Value)
                                End Using
                            End Using
                        End If
                    End If
                    Using cn As New SqlConnection(sRunbookConnectionString)
                        cn.Open()
                        Using cm As New System.Data.SqlClient.SqlCommand
                            cm.Connection = cn
                            cm.CommandType = CommandType.StoredProcedure
                            cm.CommandText = "SQLRunbook.pTopicDocumentInsert"
                            Dim TopicId As New SqlParameter()
                            With TopicId
                                .Direction = ParameterDirection.Input
                                .ParameterName = "@TopicId"
                                .SqlDbType = SqlDbType.Int
                                .Value = CInt(DataSetSQLRunbook.tTopic.Rows(0).Item("Id"))
                                cm.Parameters.Add(TopicId)
                            End With
                            Dim DocumentId As New SqlParameter()
                            With DocumentId
                                .Direction = ParameterDirection.Input
                                .ParameterName = "@DocumentId"
                                .SqlDbType = SqlDbType.Int
                                .Value = SavedFileInfo.Id
                                cm.Parameters.Add(DocumentId)
                            End With
                            cm.ExecuteNonQuery()
                        End Using
                    End Using
                    SetTopicDocumentsByTopicId(CInt(DataSetSQLRunbook.tTopic.Rows(0).Item("Id")))
                    SetDocumentsForSelectedTopicList(DataSetSQLRunbook.tTopic.Rows(0).Item("Name").ToString)
                End If
            End If
        Catch ex As Exception
            OpenFileDialogDocument.Dispose()
        End Try
    End Sub

    Private Sub LinkLabel1_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) _
    Handles LinkLabel1.LinkClicked
        Try
            DialogRunbookLookupCriteria.ShowDialog(Me)
        Catch ex As Exception
            Mother.HandleException(New Exception(String.Format("({0}.LinkLabel1_LinkClicked) Exception.", _
                                                                          Me.Name), ex))
        End Try
    End Sub

    Private Sub ButtonNotes_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonNotes.Click
        Try
            If Not DataSetSQLRunbook.tTopic.Rows.Count = 0 Then
                Dim Notes As String = DataSetSQLRunbook.tTopic.Rows(0).Item("Notes").ToString
                If DialogNotes.ShowDialog(Notes, _
                              Me.Text, _
                              "Topic", _
                              DataSetSQLRunbook.tTopic.Rows(0).Item("Name").ToString, _
                              Me) = Windows.Forms.DialogResult.OK Then
                    If Not Notes = DataSetSQLRunbook.tTopic.Rows(0).Item("Notes").ToString Then
                        ' notes changed in the dialog - save any changed doc attribs or cat selections too 
                        DataSetSQLRunbook.tTopic.Rows(0).Item("Notes") = Notes
                        'DataSetSQLRunbook.tSQLRunbookTopic.Rows(0).EndEdit()
                        SaveChanges(CInt(DataSetSQLRunbook.tTopic.Rows(0).Item("Id")))
                        LoadRunbook(CInt(DataSetSQLRunbook.tTopic.Rows(0).Item("Id")))
                    End If
                    ToolStripStatusLabelRunbook.Text = My.Resources.Ready
                End If
            End If
        Catch ex As Exception
            Mother.HandleException(New Exception(String.Format("({0}.ButtonNotes_Click) Exception.", _
                                                              Me.Name), ex))
        End Try
    End Sub

    Private Sub DataGridViewTopicList_Sorted(ByVal sender As Object, ByVal e As System.EventArgs) _
    Handles DataGridViewTopicList.Sorted
        SetTopicRatings()
    End Sub

    Private Sub DataGridViewDocumentList_CurrentCellDirtyStateChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DataGridViewDocumentList.CurrentCellDirtyStateChanged
        Select Case DataGridViewDocumentList.CurrentCell.OwningColumn.Name
            Case DataGridViewComboBoxColumnDocType.Name
                PanelDocumentList.ForeColor = PanelTopicList.ForeColor
                PanelDocumentList.BackColor = PanelTopicList.BackColor
            Case DataGridViewComboBoxColumnDocumentOwner.Name
                PanelDocumentList.ForeColor = PanelTopicList.ForeColor
                PanelDocumentList.BackColor = PanelTopicList.BackColor
        End Select
    End Sub

    Private Sub DataGridViewDocumentList_DataError(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewDataErrorEventArgs) Handles DataGridViewDocumentList.DataError
        If DataGridViewDocumentList.Columns(e.ColumnIndex).Name = "DataGridViewComboBoxColumnDocType" Then
            If Not DataGridViewComboBoxColumnDocType.Items.Contains(DataGridViewDocumentList.CurrentRow.Cells("DataGridViewComboBoxColumnDocType").Value.ToString) Then
                '                DataGridViewComboBoxColumnDocType.Items.Add(DataGridViewDocumentList.CurrentRow.Cells("DataGridViewComboBoxColumnDocType").Value.ToString)
                e.Cancel = True
            End If
        End If
    End Sub

    Private Sub DataGridViewDocumentList_Sorted(ByVal sender As Object, ByVal e As System.EventArgs) Handles DataGridViewDocumentList.Sorted
        SetDocumentDisplayColumns()
    End Sub

    Private Sub SplitContainerResults_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles SplitContainerResults.GotFocus
        SplitContainerResults.BackColor = SystemColors.GradientActiveCaption
    End Sub

    Private Sub SplitContainerResults_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles SplitContainerResults.LostFocus
        SplitContainerResults.BackColor = SystemColors.GradientInactiveCaption
    End Sub

    Private Sub SplitContainerResults_MouseEnter(ByVal sender As Object, ByVal e As System.EventArgs) Handles SplitContainerResults.MouseEnter
        SplitContainerResults.BackColor = SystemColors.GradientActiveCaption
    End Sub

    Private Sub SplitContainerResults_MouseLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles SplitContainerResults.MouseLeave
        SplitContainerResults.BackColor = SystemColors.GradientInactiveCaption
    End Sub

    Private Sub SplitContainerTopic_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles SplitContainerTopic.GotFocus
        SplitContainerTopic.BackColor = SystemColors.GradientActiveCaption
    End Sub

    Private Sub SplitContainerTopic_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles SplitContainerTopic.LostFocus
        SplitContainerTopic.BackColor = SystemColors.GradientInactiveCaption
    End Sub

    Private Sub SplitContainerTopic_MouseEnter(ByVal sender As Object, ByVal e As System.EventArgs) Handles SplitContainerTopic.MouseEnter
        SplitContainerTopic.BackColor = SystemColors.GradientActiveCaption
    End Sub

    Private Sub SplitContainerTopic_MouseLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles SplitContainerTopic.MouseLeave
        SplitContainerTopic.BackColor = SystemColors.GradientInactiveCaption
    End Sub

    Private Sub ListBoxCategory_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ListBoxCategory.Click
        'update catgories in database when save is pushed
        PanelCategoryList.BackColor = PanelTopicList.ForeColor
        PanelCategoryList.ForeColor = PanelTopicList.BackColor
        _CategoriesChanged = True
    End Sub

    Private Sub ComboBoxTopicName_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ComboBoxTopicName.SelectedIndexChanged
        Dim TopicId As Integer
        If ComboBoxTopicName.Text = "" Then
            TopicId = 0
        Else
            TopicId = CInt(AllTopics.Select(String.Format("Name='{0}'", ComboBoxTopicName.Text))(0)("Id"))
        End If
        SetTopicDetailItems(TopicId)
    End Sub

End Class

