Imports System.Windows.Forms
Imports System.Drawing

Public Class CompareForm
    Friend cCompare As New cCommon.cCompare
    Private cTv As New cTreeView
    Delegate Sub ExceptionEventHandlerDelegate(ByVal exMsg As String)
    Delegate Sub ComparingEventHandlerDelegate(ByVal Item1 As String, ByVal Item2 As String)
    Delegate Sub WriteResultEventHandlerDelegate(ByVal ResultType As String, ByVal Item1 As String, ByVal Item2 As String)
    Delegate Sub WriteLineResultEventHandlerDelegate(ByVal ResultType As String, _
                                                     ByVal Item1 As String, _
                                                     ByVal Line1Number As Int32, _
                                                     ByVal Line1 As String, _
                                                     ByVal Item2 As String, _
                                                     ByVal Line2Number As Int32, _
                                                     ByVal Line2 As String)
    Delegate Sub PercentDoneEventHandlerDelegate(ByVal Value As Integer)
    Private dtRepositoryInstance1 As DataTable
    Private dtRepositoryInstance2 As DataTable
    Friend Panel1ConnectionSettingsChanged As Boolean
    Friend Panel2ConnectionSettingsChanged As Boolean
    Private HTMLSide1 As String
    Private HTMLSide2 As String

#Region " Form Events "
    ' all methods in here should have error handling??!!
    Private Sub CompareForm_Load(ByVal sender As System.Object, _
                                      ByVal e As System.EventArgs) _
                                      Handles MyBase.Load
        Dim csr As Cursor = Nothing
        Try
            csr = Me.Cursor   ' Save the old cursor
            Me.Cursor = Cursors.WaitCursor   ' Display the waiting cursor
            Me.Text = My.Resources.CompareText
            KeyMatchLabel.ForeColor = My.Settings.CompareOutputBodyText
            KeyMatchLabel.BackColor = My.Settings.CompareOutputBodyBackground
            KeyNoMatchLabel.ForeColor = My.Settings.CompareOutputNoMatchText
            KeyNoMatchLabel.BackColor = My.Settings.CompareOutputNoMatchBackground
            KeyDifferentLabel.ForeColor = My.Settings.CompareOutputDifferentText
            KeyDifferentLabel.BackColor = My.Settings.CompareOutputDifferentBackground
            KeyHeaderLabel.ForeColor = My.Settings.CompareOutputHeaderText
            KeyHeaderLabel.BackColor = My.Settings.CompareOutputHeaderBackground
            RemoveHandler cCompare.Excptn, AddressOf ExceptionEventHandler
            RemoveHandler cCompare.Comparing, AddressOf ComparingEventHandler
            RemoveHandler cCompare.NameResult, AddressOf WriteResultEventHandler
            RemoveHandler cCompare.LineResult, AddressOf WriteLineResultEventHandler
            RemoveHandler cCompare.PercentDone, AddressOf PercentDoneEventHandler
            AddHandler cCompare.Excptn, AddressOf ExceptionEventHandler
            AddHandler cCompare.Comparing, AddressOf ComparingEventHandler
            AddHandler cCompare.NameResult, AddressOf WriteResultEventHandler
            AddHandler cCompare.LineResult, AddressOf WriteLineResultEventHandler
            AddHandler cCompare.PercentDone, AddressOf PercentDoneEventHandler
            'implement the syncronized swim (scroll)
            sClass1 = New Subclass(RichTextBox1.Handle)
            sClass2 = New Subclass(RichTextBox2.Handle)
            sClassF = New Subclass(Me.Handle)
            'Password is never saved so the mask is not needed now
            ' would be better security to NOT save the mask as it reveals the password length
            My.Settings.SQL__Instance__A_SQL__Login__Password_4 = ""
            My.Settings.SQL__Instance__B_SQL__Login__Password_4 = ""
            ' set up the form
            Origin1Label.Text = My.Resources.Origin
            Origin2Label.Text = My.Resources.Origin
            LabelInstance1.Text = My.Resources.SQLInstance
            LabelInstance2.Text = My.Resources.SQLInstance
            ' set the working values from the settings file
            SyncWorkingValues()
            ' cCompare class can't get to resources directly and needs this for FOC
            cCompare.CompareComplete = My.Resources.CompareComplete
            cCompare.CompareCancelled = My.Resources.CompareCancelled
            CompareUIEndWaitForCompare()
            If My.Settings.RepositoryEnabled Then
                cCompare.DAL.RepositoryConnectionTimeout = My.Settings.RepositoryConnectionTimeout
                cCompare.DAL.RepositoryDatabaseName = My.Settings.RepositoryDatabaseName
                cCompare.DAL.RepositoryEncryptConnection = My.Settings.RepositoryEncryptConnection
                cCompare.DAL.RepositoryInstanceName = My.Settings.RepositoryInstanceName
                cCompare.DAL.RepositoryNetworkLibrary = My.Settings.RepositoryNetworkLibrary
                cCompare.DAL.RepositorySQLLoginName = My.Settings.RepositorySQLLoginName
                cCompare.DAL.RepositorySQLLoginPassword = My.Settings.RepositorySQLLoginPassword
                cCompare.DAL.RepositoryTrustServerCertificate = My.Settings.RepositoryTrustServerCertificate
                cCompare.DAL.RepositoryUseTrustedConnection = My.Settings.RepositoryUseTrustedConnection
            End If
            InitPanel(Origin1, Instance1, HierarchyLabel1, smoTreeView1, cCompare.SqlServer1, ToolStripStatusLabel1)
            InitPanel(Origin2, Instance2, HierarchyLabel2, smoTreeView2, cCompare.SqlServer2, ToolStripStatusLabel2)
            Panel1ConnectionSettingsChanged = False
            Panel2ConnectionSettingsChanged = False
            RepositionComponents()
            ShowBasicHelp()
            Me.Show()
        Catch ex As Exception
            Mother.HandleException(ex)
        Finally
            Me.Cursor = csr ' restore the original cursor
        End Try
    End Sub

    Private Sub CancelButton_Click(ByVal sender As System.Object, _
                                   ByVal e As System.EventArgs) _
                                   Handles Cancel_Button.Click
        cCompare.CancelAsyncCompare()
    End Sub

    Private Sub CompareForm_FormClosing(ByVal sender As Object, _
                                        ByVal e As System.Windows.Forms.FormClosingEventArgs) _
                                        Handles Me.FormClosing
        ' savsettingsonexit is enabled - clear these so the password length is not stored
        My.Settings.SQL__Instance__A_SQL__Login__Password_4 = ""
        My.Settings.SQL__Instance__B_SQL__Login__Password_4 = ""
        cCompare.CloseConnections()
    End Sub

    Private Sub CompareButton_Click(ByVal sender As System.Object, _
                                    ByVal e As System.EventArgs) _
                                    Handles CompareButton.Click
        Dim csr As Cursor = Nothing
        csr = Me.Cursor ' Save the old cursor
        Me.Cursor = Cursors.WaitCursor ' Display the waiting cursor
        Try
            If IsValidCompare() Then
                RunCompare()
            End If ' is valid
        Catch ex As Exception
            Mother.HandleException(ex)
        Finally
            ' Restore the original cursor
            Me.Cursor = csr
        End Try
    End Sub

    Public Sub RunCompare()
        Try
            ' assert: repository node equals instance node 
            ' when 1. text is equal and 2. the smo node is a collection
            RichTextBox1.Clear()
            HTMLSide1 = ""
            RichTextBox2.Clear()
            HTMLSide2 = ""
            Dim DbName1 As String = ""
            Dim Collection1 As String = ""
            Dim Item1 As String = ""
            Dim DbName2 As String = ""
            Dim Collection2 As String = ""
            Dim Item2 As String = ""
            If Origin1.Text = My.Resources.OriginFile Then
                Item1 = CType(smoTreeView1.SelectedNode.Tag, cTreeView.structNodeTag).obj.ToString
            Else
                Dim Strings1 As String() = cCompare.CrackFullPath(smoTreeView1.SelectedNode.FullPath)
                DbName1 = Strings1(1)
                Collection1 = Strings1(2)
                Item1 = Strings1(3)
            End If
            If Origin2.Text = My.Resources.OriginFile Then
                Item2 = CType(smoTreeView2.SelectedNode.Tag, cTreeView.structNodeTag).obj.ToString
            Else
                Dim Strings2 As String() = cCompare.CrackFullPath(smoTreeView2.SelectedNode.FullPath)
                DbName2 = Strings2(1)
                Collection2 = Strings2(2)
                Item2 = Strings2(3)
            End If
            If Not smoTreeView1.SelectedNode.Parent Is Nothing _
            AndAlso CType(smoTreeView1.SelectedNode.Tag, cTreeView.structNodeTag).IsCollection _
            AndAlso Not smoTreeView1.SelectedNode.Parent.Text = "Databases" _
            AndAlso Not smoTreeView1.SelectedNode.Text = "ServiceBroker" Then
                cCompare._CompareCollection = True
                ' use user options to determine verbosity of compare output 
                cCompare._ReportDetails = My.Settings.Misc_Display__Output_Show__Comparison__Details
                CompareUIWaitForCompare()
                ' selected node is a collection
                cCompare.AsyncCompareAllItems(Origin1.Text, _
                                        Instance1.Text, _
                                        DbName1, _
                                        Collection1, _
                                        My.Settings.SQL__Repository__A_Filters_Label, _
                                        Origin2.Text, _
                                        Instance2.Text, _
                                        DbName2, _
                                        Collection2, _
                                        My.Settings.SQL__Repository__B_Filters_Label, _
                                        False, _
                                        If(Origin1.Text = My.Resources.OriginRepository, cCompare.Repository1.ConnectionContext.ConnectionString, ""), _
                                        If(Origin2.Text = My.Resources.OriginRepository, cCompare.Repository2.ConnectionContext.ConnectionString, ""))
                ToolStripStatusLabelCompare.Text = My.Resources.CompareStarting
                ToolStripStatusLabel1.Text = ""
                ToolStripStatusLabel2.Text = ""
            Else ' Compare One
                cCompare._CompareCollection = False
                '' always show details of item compare -- no, use set options
                'cCompare._ReportDetails = True
                cCompare.AsyncCompareTwoItems(Origin1.Text, _
                                              Instance1.Text, _
                                              DbName1, _
                                              Collection1, _
                                              Item1, _
                                              If(Origin1.Text = My.Resources.OriginRepository, _
                                                 My.Settings.SQL__Repository__A_Filters_Version, _
                                                 0), _
                                              My.Settings.SQL__Repository__A_Filters_Label, _
                                              Origin2.Text, _
                                              Instance2.Text, _
                                              DbName2, _
                                              Collection2, _
                                              Item2, _
                                              If(Origin2.Text = My.Resources.OriginRepository, _
                                                 My.Settings.SQL__Repository__B_Filters_Version, _
                                                 0), _
                                              My.Settings.SQL__Repository__B_Filters_Label, _
                                              If(Origin1.Text = My.Resources.OriginRepository, cCompare.Repository1.ConnectionContext.ConnectionString, ""), _
                                              If(Origin2.Text = My.Resources.OriginRepository, cCompare.Repository2.ConnectionContext.ConnectionString, ""))
            End If
            StatusStripCompare.Refresh()
            'if it worked, update the settings
            UpdateAutoCompleteLists()
            My.Settings.Save()
            ' if the save worked we are not dirty anymore 
            My.Settings.SettingsDirty = False
        Catch ex As Exception
            Mother.HandleException(ex)
        End Try
    End Sub

    Private Sub CompareUIWaitForCompare()
        SplitContainer1.Enabled = False
        ToolStripProgressBar1.Value = 0
        ToolStripProgressBar1.Visible = True
        Cancel_Button.Visible = True
        CompareButton.Visible = False
        ToolStripStatusLabelCompare.Text = My.Resources.CompareStarting
        My.Application.DoEvents()
    End Sub

    Private Sub CompareUIEndWaitForCompare()
        SplitContainer1.Enabled = True
        ToolStripProgressBar1.Visible = False
        Cancel_Button.Visible = False
        CompareButton.Visible = True
        ToolStripStatusLabelCompare.Text = ""
    End Sub

    Private Sub ToolStripComboBoxVersionA_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) _
    Handles ToolStripComboBoxVersionA.SelectedIndexChanged
        Dim ns As cTreeView.structNodeTag
        ns = CType(smoTreeView1.SelectedNode.Tag, cTreeView.structNodeTag)
        ns.Version = CInt(ToolStripComboBoxVersionA.SelectedItem)
        smoTreeView1.SelectedNode.Tag = ns
        My.Settings.SQL__Repository__A_Filters_Version = ns.Version
    End Sub

    Private Sub ToolStripComboBoxVersionB_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) _
    Handles ToolStripComboBoxVersionB.SelectedIndexChanged
        Dim ns As cTreeView.structNodeTag
        ns = CType(smoTreeView2.SelectedNode.Tag, cTreeView.structNodeTag)
        ns.Version = CInt(ToolStripComboBoxVersionB.SelectedItem)
        smoTreeView2.SelectedNode.Tag = ns
        My.Settings.SQL__Repository__B_Filters_Version = ns.Version
    End Sub

    Private Function IsValidCompare() As Boolean
        IsValidCompare = False
        Dim IsServer1 As Boolean = If(smoTreeView1.SelectedNode.Parent Is Nothing, True, False)
        Dim IsServer2 As Boolean = If(smoTreeView2.SelectedNode.Parent Is Nothing, True, False)
        Dim IsDatabase1 As Boolean = False
        Dim IsDatabase2 As Boolean = False
        Dim IsJobServer1 As Boolean = False
        Dim IsJobServer2 As Boolean = False
        Dim IsServiceBroker1 As Boolean = False
        Dim IsServiceBroker2 As Boolean = False
        Dim IsCollection1 As Boolean = False
        Dim IsCollection2 As Boolean = False
        Dim HasChildren1 As Boolean = False
        Dim HasChildren2 As Boolean = False
        Dim Type1 As String = ""
        Dim Type2 As String = ""
        If Not IsServer1 Then
            If CType(smoTreeView1.SelectedNode.Tag, cTreeView.structNodeTag).IsCollection _
            Or smoTreeView1.SelectedNode.Text = "JobServer" _
            Or smoTreeView1.SelectedNode.Text = "ServiceBroker" _
            Or smoTreeView1.SelectedNode.Parent.Text = "Databases" Then
                IsCollection1 = True
                IsDatabase1 = If(smoTreeView1.SelectedNode.Parent.Text = "Databases", True, False)
                Type1 = If(IsDatabase1, "Database", smoTreeView1.SelectedNode.Text)
                If smoTreeView1.SelectedNode.GetNodeCount(False) > 0 _
                Or smoTreeView1.SelectedNode.Nodes.Count > 0 Then
                    HasChildren1 = True
                End If
            End If
        Else
            Type1 = "Server"
        End If
        If Not IsServer2 Then
            If CType(smoTreeView2.SelectedNode.Tag, cTreeView.structNodeTag).IsCollection _
            Or smoTreeView2.SelectedNode.Text = "JobServer" _
            Or smoTreeView2.SelectedNode.Text = "ServiceBroker" _
            Or smoTreeView2.SelectedNode.Parent.Text = "Databases" Then
                IsCollection2 = True
                IsDatabase2 = If(smoTreeView2.SelectedNode.Parent.Text = "Databases", True, False)
                Type2 = If(IsDatabase2, "Database", smoTreeView2.SelectedNode.Text)
                If smoTreeView2.SelectedNode.GetNodeCount(False) > 0 _
                Or smoTreeView2.SelectedNode.Nodes.Count > 0 Then
                    HasChildren2 = True
                End If
            End If
        Else
            Type2 = "Server"
        End If
        If (Origin1.Text = "") Or (Origin2.Text = "") Then
            ' both sources have not been selected
            MessageBox.Show(String.Format(My.Resources.NoSelection, _
                                          My.Resources.Origin, _
                                          Origin1.Text, _
                                          Origin2.Text), _
                            My.Resources.CompareValidationFailureCaption, _
                            MessageBoxButtons.OK, _
                            MessageBoxIcon.Stop)
        ElseIf (Instance1.Text = "") Or (Instance2.Text = "") Then
            ' both servers have not been selected
            MessageBox.Show(String.Format(My.Resources.NoSelection, _
                                          My.Resources.SQLInstance, _
                                          Instance1.Text, _
                                          Instance2.Text), _
                            My.Resources.CompareValidationFailureCaption, _
                            MessageBoxButtons.OK, _
                            MessageBoxIcon.Stop)
        ElseIf Not Type1.Equals(Type2) And IsCollection1 And IsCollection2 Then
            ' not for two collections of different type items only for different items
            IsValidCompare = False
            MessageBox.Show(String.Format(My.Resources.InvalidCollectionCompare, _
                                          Type1, _
                                          Type2), _
                            My.Resources.CompareValidationFailureCaption, _
                            MessageBoxButtons.OK, _
                            MessageBoxIcon.Stop)
            '            ElseIf Not CType(smoTreeView1.SelectedNode.Tag, cTreeView.structNodeTag).IsCollection.Equals(CType(smoTreeView2.SelectedNode.Tag, cTreeView.structNodeTag).IsCollection)Then Then
            ' can't compare a collection and and item
            '                MessageBox.Show(String.Format(My.Resources.InvalidCompare, _
            '                                              If(IsCollection1, Type1, Type2), _
            '                                              If(IsCollection2, smoTreeView1.SelectedNode.Text, smoTreeView2.SelectedNode.Text)), _
            '                            My.Resources.CompareValidationFailureCaption, MessageBoxButtons.OK, MessageBoxIcon.Stop)

        ElseIf (IsCollection1 And Not HasChildren1) OrElse (IsCollection2 And Not HasChildren2) Then
            ' there are items in both collections to compare 
            MessageBox.Show(String.Format(My.Resources.NoItemInDB, _
                                          smoTreeView1.SelectedNode.Text), _
                        My.Resources.CompareValidationFailureCaption, _
                        MessageBoxButtons.OK, _
                        MessageBoxIcon.Stop)
        ElseIf Not (smoTreeView1.SelectedNode.Text = smoTreeView2.SelectedNode.Text) _
                    And HasChildren1 And HasChildren2 _
        AndAlso Not (IsServer1 And IsServer2) _
        AndAlso Not (IsDatabase1 And IsDatabase2) _
        AndAlso Not Type1.Equals(Type2) Then
            ' special case for server - node(0),node(0)
            ' special case for databases - because both item and collection
            ' not for two collections of different type only for items of different types
            MessageBox.Show(String.Format(My.Resources.InvalidCollectionCompare, _
                                          CType(smoTreeView1.SelectedNode.Tag, cTreeView.structNodeTag).obj.GetType.Name, _
                                          CType(smoTreeView2.SelectedNode.Tag, cTreeView.structNodeTag).obj.GetType.Name), _
                            My.Resources.CompareValidationFailureCaption, _
                            MessageBoxButtons.OK, _
                            MessageBoxIcon.Stop)
        ElseIf Not HasChildren1.Equals(HasChildren2) Then
            ' can't compare a collection and and item
            MessageBox.Show(String.Format(My.Resources.InvalidCompare, _
                                          CType(smoTreeView1.SelectedNode.Tag, cTreeView.structNodeTag).obj.GetType.FullName, _
                                          CType(smoTreeView2.SelectedNode.Tag, cTreeView.structNodeTag).obj.GetType.FullName), _
                            My.Resources.CompareValidationFailureCaption, _
                            MessageBoxButtons.OK, _
                            MessageBoxIcon.Stop)
        ElseIf ((IsCollection1 And Not HasChildren1) _
                OrElse (IsCollection2 And Not HasChildren2)) Then
            ' validate that there are items to compare - should allow for the list 
            ' to not be populated provided both sides are set to the default 
            MessageBox.Show(String.Format(My.Resources.NoItemInDB, _
                                          smoTreeView1.SelectedNode.Text), _
                            My.Resources.CompareValidationFailureCaption, _
                            MessageBoxButtons.OK, _
                            MessageBoxIcon.Stop)
        Else
            IsValidCompare = True
        End If
        'My.Resources.CompareDBAdvice
    End Function


    Private Sub Instance1_SelectedIndexChanged(ByVal sender As System.Object, _
                                               ByVal e As System.EventArgs) _
                                               Handles Instance1.SelectedIndexChanged
        Select Case Origin1.Text
            Case My.Resources.OriginFile
                TryLoadTreeView(Origin1, Instance1, smoTreeView1, cCompare.SqlServer1, ToolStripStatusLabel1, 1)
            Case My.Resources.OriginSQLInstance
                TryLoadTreeView(Origin1, Instance1, smoTreeView1, cCompare.SqlServer1, ToolStripStatusLabel1, 1)
            Case My.Resources.OriginRepository
                TryLoadTreeView(Origin1, Instance1, smoTreeView1, cCompare.Repository1, ToolStripStatusLabel1, 1)
        End Select
    End Sub

    Private Sub Instance2_SelectedIndexChanged(ByVal sender As System.Object, _
                                               ByVal e As System.EventArgs) _
                                               Handles Instance2.SelectedIndexChanged
        Select Case Origin2.Text
            Case My.Resources.OriginFile
                TryLoadTreeView(Origin2, Instance2, smoTreeView2, cCompare.SqlServer2, ToolStripStatusLabel2, 2)
            Case My.Resources.OriginSQLInstance
                TryLoadTreeView(Origin2, Instance2, smoTreeView2, cCompare.SqlServer2, ToolStripStatusLabel2, 2)
            Case My.Resources.OriginRepository
                TryLoadTreeView(Origin2, Instance2, smoTreeView2, cCompare.Repository2, ToolStripStatusLabel2, 2)
        End Select
    End Sub

    Public Function TryLoadTreeView(ByRef Origin As ComboBox, _
                                     ByRef Instance As ComboBox, _
                                     ByRef smoTreeView As TreeView, _
                                     ByRef SQLServer As Server, _
                                     ByRef TSLabel As ToolStripStatusLabel, _
                                     ByVal Side1or2 As Int32) As Boolean
        TryLoadTreeView = False
        ' Save the old cursor
        Dim csr As Cursor = Me.Cursor
        Try
            Me.Cursor = Cursors.WaitCursor   ' Display the waiting cursor
            Dim Connected As Boolean = False
            ' no matter what any existing treeview data is now out of date
            smoTreeView.Nodes.Clear()
            RichTextBox1.Clear()
            HTMLSide1 = ""
            RichTextBox2.Clear()
            HTMLSide2 = ""
            If Origin.Text = My.Resources.OriginFile Then
                Instance.Items.Clear()
                Instance.DropDownStyle = ComboBoxStyle.Simple
                If Side1or2 = 1 Then
                    LabelInstance1.Text = My.Resources.OriginFile
                Else
                    LabelInstance2.Text = My.Resources.OriginFile
                End If
                FolderBrowserDialogCompare.Description = My.Resources.SelectFileFolder
                Dim result As DialogResult
                result = FolderBrowserDialogCompare.ShowDialog(Me)
                If result = Windows.Forms.DialogResult.OK Then
                    'Instance.Items.Add(FolderBrowserDialogCompare.SelectedPath)
                    Instance.Text = FolderBrowserDialogCompare.SelectedPath
                    cTv.FileInitTreeView(Origin, Instance, smoTreeView)
                    TryLoadTreeView = True
                End If
            ElseIf Origin.Text = My.Resources.OriginSQLInstance Then
                Try
                    Instance.DropDownStyle = ComboBoxStyle.DropDownList
                    If Side1or2 = 1 Then
                        LabelInstance1.Text = My.Resources.CompareInstanceLabel
                    Else
                        LabelInstance2.Text = My.Resources.CompareInstanceLabel
                    End If
                    ' create the SMO connection
                    ' will not pick up name of a new'd SQL named instance correctly so don't use that connection    
                    If (SQLServer Is Nothing) _
                    OrElse Not (SQLServer.ConnectionContext.TrueName = Instance.Text) Then
                        If Side1or2 = 1 AndAlso Server.ReferenceEquals(SQLServer, cCompare.SqlServer1) Then
                            Connected = GetSQLConnection1()
                        ElseIf Side1or2 = 2 AndAlso Server.ReferenceEquals(SQLServer, cCompare.SqlServer2) Then
                            Connected = GetSQLConnection2()
                        End If
                        ' cleared the tree above so if reasonable, repaint it again now
                        ' Use name not truename here because...?
                        If Not SQLServer Is Nothing _
                        AndAlso smoTreeView.Nodes.Count = 0 _
                        AndAlso Not (SQLServer.Name = "." Or SQLServer.Name = "") _
                        AndAlso Not Instance.Text = "" Then
                            smoTreeView.Nodes.Add(cTreeView.CreateNode(Instance.Text, SQLServer))
                            smoTreeView.Sort()
                            ' is in the expand event that repository state is added to smo userdata 
                            smoTreeView.Nodes(0).Expand()
                            TryLoadTreeView = True
                        End If
                    End If
                Catch ex As Exception
                    Throw New Exception(String.Format(My.Resources.ConnectionToSQLFailed, _
                                                      If(Side1or2 = 1, ToolStripStatusLabel1.Tag, ToolStripStatusLabel2.Tag), _
                                                      Instance.Name), ex)
                End Try
            ElseIf Origin.Text = My.Resources.OriginRepository Then
                ' here the SQLServer references a selected the repository host
                ' processing is a double pass, once to select the repository, once to select the target instance 
                Try
                    Instance.DropDownStyle = ComboBoxStyle.DropDownList
                    If Side1or2 = 1 Then
                        LabelInstance1.Text = My.Resources.CompareInstanceLabel
                    Else
                        LabelInstance2.Text = My.Resources.CompareInstanceLabel
                    End If
                    ' create the SMO connection to the specifed repository
                    ' ResetPanel 
                    If (SQLServer Is Nothing) Then
                        Instance.Items.Clear()
                        If Side1or2 = 1 Then
                            If GetRepositoryConnection1() Then
                                ' once connected need to enum the instances archived on that host
                                Instance.Items.AddRange(Mother.DAL.GetConfiguredInstanceList(cCompare.Repository1.ConnectionContext.ConnectionString))
                                ' save the archive db name to the control tag for later use
                                Origin.Tag = String.Format("{0} [{1}].[{2}]", Origin.Text, _
                                                                              My.Settings.SQL__Repository__A_Name_1, _
                                                                              My.Settings.SQL__Repository__A_Database__Name_2)
                            End If
                        Else
                            If GetRepositoryConnection2() Then
                                ' once connected need to enum the instances archived on that host
                                Instance.Items.AddRange(Mother.DAL.GetConfiguredInstanceList(cCompare.Repository2.ConnectionContext.ConnectionString))
                                ' save the archive db name to the control tag for later use
                                Origin.Tag = String.Format("{0} [{1}].[{2}]", Origin.Text, _
                                                                              My.Settings.SQL__Repository__B_Name_1, _
                                                                              My.Settings.SQL__Repository__B_Database__Name_2)
                            End If
                        End If
                    End If
                    If Instance.Items.Count = 0 And Not Origin.Tag Is Nothing Then
                        TSLabel.Text = String.Format(My.Resources.InvalidRepository, _
                                                     TSLabel.Tag, _
                                                     Origin.Tag.ToString)
                    Else
                        If Not Instance.Text = "" AndAlso smoTreeView.Nodes.Count = 0 Then
                            ' cleared the tree above so load it now
                            cTv.LoadTreeFromRepository(Instance.Text, _
                                                       If(Side1or2 = 1, cCompare.Repository1, cCompare.Repository2), _
                                                       smoTreeView, _
                                                       If(Side1or2 = 1, dtRepositoryInstance1, dtRepositoryInstance2))
                            smoTreeView.Nodes(0).Expand()
                            ' only sees the root node here (using the before expand event to get others, but it nevers sees root node)
                            If Side1or2 = 1 Then
                                smoTreeView.Nodes(0).ContextMenuStrip = ContextMenuStripTreeItemA
                            Else
                                smoTreeView.Nodes(0).ContextMenuStrip = ContextMenuStripTreeItemB
                            End If
                        End If
                        TryLoadTreeView = True
                    End If
                Catch ex As Exception
                    Throw New Exception(String.Format(My.Resources.ConnectionToSQLFailed, _
                                                      If(Side1or2 = 1, ToolStripStatusLabel1.Tag, ToolStripStatusLabel2.Tag), _
                                                      Instance.Name), ex)
                End Try
            End If ' origin type
        Catch ex As Exception
            smoTreeView.Nodes.Clear()
            Mother.HandleException(New Exception(String.Format( _
                                   "({0}.TryLoadTreeView) Exception. Origin: {1}, Name: {2}", _
                                   Me.Text, Origin.Text, Instance.Text), ex))
        Finally
            Me.Cursor = csr  ' Restore the original cursor
        End Try
    End Function

    ' origin change should close connections reload instance list and clear treeview
    Private Sub Origin1_SelectedIndexChanged(ByVal sender As System.Object, _
                                             ByVal e As System.EventArgs) _
                                             Handles Origin1.SelectedIndexChanged
        Select Case Origin1.Text
            Case My.Resources.OriginFile
                ResetPanel(Origin1, Instance1, HierarchyLabel1, smoTreeView1, cCompare.SqlServer1, ToolStripStatusLabel1, 1)
            Case My.Resources.OriginRepository
                ResetPanel(Origin1, Instance1, HierarchyLabel1, smoTreeView1, cCompare.Repository1, ToolStripStatusLabel1, 1)
            Case My.Resources.OriginSQLInstance
                ResetPanel(Origin1, Instance1, HierarchyLabel1, smoTreeView1, cCompare.SqlServer1, ToolStripStatusLabel1, 1)
        End Select
    End Sub

    Private Sub Origin2_SelectedIndexChanged(ByVal sender As System.Object, _
                                             ByVal e As System.EventArgs) _
                                             Handles Origin2.SelectedIndexChanged
        Select Case Origin2.Text
            Case My.Resources.OriginFile
                ResetPanel(Origin2, Instance2, HierarchyLabel2, smoTreeView2, cCompare.SqlServer2, ToolStripStatusLabel2, 2)
            Case My.Resources.OriginRepository
                ResetPanel(Origin2, Instance2, HierarchyLabel2, smoTreeView2, cCompare.Repository2, ToolStripStatusLabel2, 2)
            Case My.Resources.OriginSQLInstance
                ResetPanel(Origin2, Instance2, HierarchyLabel2, smoTreeView2, cCompare.SqlServer2, ToolStripStatusLabel2, 2)
        End Select
    End Sub

    Public Sub ResetPanel(ByRef Origin As ComboBox, _
                           ByRef Instance As ComboBox, _
                           ByRef HierarchyLabel As Label, _
                           ByRef smoTreeView As TreeView, _
                           ByRef SQLServer As Server, _
                           ByRef TSLabel As ToolStripStatusLabel, _
                           ByVal Side1or2 As Int32)
        Dim csr As Cursor = Nothing
        Try
            csr = Me.Cursor   ' Save the old cursor
            Me.Cursor = Cursors.WaitCursor   ' Display the waiting cursor
            'If there was a connection to SQL Instance, need to destroy now
            If Not SQLServer Is Nothing Then
                Try
                    TSLabel.Text = String.Format(My.Resources.DisconnectingFromSQL, _
                                                 TSLabel.Tag, _
                                                 Instance.Text)
                    SQLServer.ConnectionContext.Disconnect()
                    SQLServer = Nothing
                Catch ex As ConnectionFailureException
                    ' no need to fail this ex since all we want to do is distroy it anyway
                    ex = Nothing
                End Try
            End If
            ' changing to blank will not try to load the tree - checked by TryLoadTreeView
            Instance.Text = ""
            Select Case Origin.Text
                Case My.Resources.OriginSQLInstance
                    If TryLoadTreeView(Origin, Instance, smoTreeView, SQLServer, TSLabel, Side1or2) Then
                        Instance.DropDownStyle = ComboBoxStyle.DropDownList
                        HierarchyLabel.Text = "SMO Tree View"
                        TSLabel.Text = String.Format(My.Resources.SelectANode, TSLabel.Tag)
                        If Not Instance.Text = "" _
                        AndAlso Not Instance.Items.Contains(Instance.Text) Then
                            Instance.Items.Add(Instance.Text)
                        End If
                    Else
                        InitPanel(Origin, Instance, HierarchyLabel, smoTreeView, SQLServer, TSLabel)
                    End If
                Case My.Resources.OriginRepository
                    If TryLoadTreeView(Origin, Instance, smoTreeView, SQLServer, TSLabel, Side1or2) Then
                        ' defaulted to app settings for local repository in cn call at tryloadtreeview
                        Instance.DropDownStyle = ComboBoxStyle.DropDownList
                        HierarchyLabel.Text = "Archive Tree View"
                        TSLabel.Text = String.Format(My.Resources.SelectServer, TSLabel.Tag)
                    Else
                        InitPanel(Origin, Instance, HierarchyLabel, smoTreeView, SQLServer, TSLabel)
                    End If
                Case My.Resources.OriginFile
                    If TryLoadTreeView(Origin, Instance, smoTreeView, SQLServer, TSLabel, Side1or2) Then
                        Instance.DropDownStyle = ComboBoxStyle.Simple
                        HierarchyLabel.Text = "Files in Folder"
                        TSLabel.Text = String.Format(My.Resources.SelectAFile, TSLabel.Tag)
                    Else
                        InitPanel(Origin, Instance, HierarchyLabel, smoTreeView, SQLServer, TSLabel)
                    End If
            End Select
        Catch ex As Exception
            Mother.HandleException(New Exception("(CompareForm.ResetPanel) Exception.", ex))
        Finally
            Me.Cursor = csr  ' Restore the original cursor
        End Try
    End Sub

    'AdvancedSettingsForm can call this method
    Friend Sub InitPanel(ByRef Origin As ComboBox, _
                         ByRef Instance As ComboBox, _
                         ByRef HierarchyLabel As Label, _
                         ByRef smoTreeView As TreeView, _
                         ByRef SQLServer As Server, _
                         ByRef TSLabel As ToolStripStatusLabel)
        Origin.Items.Clear()
        Origin.Text = ""
        Origin.Items.Add(My.Resources.OriginSQLInstance)
        Origin.Items.Add(My.Resources.OriginRepository)
        Origin.Items.Add(My.Resources.OriginFile)
        Instance.Items.Clear()
        Instance.Text = ""
        Instance.DropDownStyle = ComboBoxStyle.DropDownList
        HierarchyLabel.Text = "Available Items View"
        smoTreeView.Nodes.Clear()
        SQLServer = Nothing
        TSLabel.Text = String.Format(My.Resources.SelectOrigin, TSLabel.Tag)
    End Sub

#End Region

#Region " Messaging Event Handlers "

    Private Sub ExceptionEventHandler(ByVal exMsg As String)
        If Me.InvokeRequired Then
            Dim ParmBall As String()
            ReDim ParmBall(0)
            ParmBall(0) = exMsg
            Me.Invoke(New ExceptionEventHandlerDelegate(AddressOf ExceptionEventHandler), ParmBall)
        Else
            MessageBox.Show(exMsg, _
                            "Exception During Asynchronous Compare Operation", _
                            MessageBoxButtons.OK, _
                            MessageBoxIcon.Error, _
                            MessageBoxDefaultButton.Button1, _
                            MessageBoxOptions.ServiceNotification, _
                            False)
            InitPanel(Origin1, Instance1, HierarchyLabel1, smoTreeView1, cCompare.SqlServer1, ToolStripStatusLabel1)
            RichTextBox1.Clear()
            HTMLSide1 = ""
            InitPanel(Origin2, Instance2, HierarchyLabel2, smoTreeView2, cCompare.SqlServer2, ToolStripStatusLabel2)
            RichTextBox2.Clear()
            HTMLSide2 = ""
        End If
    End Sub

    Private Sub ComparingEventHandler(ByVal Item1 As String, _
                                      ByVal Item2 As String)
        If Me.InvokeRequired Then
            Dim ParmBall As String()
            ReDim ParmBall(1)
            ParmBall(0) = Item1
            ParmBall(1) = Item2
            Me.Invoke(New ComparingEventHandlerDelegate(AddressOf ComparingEventHandler), ParmBall)
        Else
            Dim HTMLStartHeaderLine As String = String.Format(My.Resources.CompareHTMLStartLine, _
                                                              MakeRGB(My.Settings.CompareOutputHeaderBackground), _
                                                              MakeRGB(My.Settings.CompareOutputHeaderText))
            ToolStripStatusLabel1.Text = String.Format("{0} {1}", _
                                                       ToolStripStatusLabel1.Tag, _
                                                       Item1)
            ToolStripStatusLabel2.Text = String.Format("{0} {1}", _
                                                       ToolStripStatusLabel2.Tag, _
                                                       Item2)
            My.Application.DoEvents()
            If Item1 = My.Resources.CompareComplete _
            Or Item1 = My.Resources.CompareCancelled Then
                CompareUIEndWaitForCompare()
                If My.Settings.CompareResultsToFile Then
                    Dim Path As String
                    If My.Settings.CompareResultsFolder = "" Then
                        ' use mydocuments if blank
                        Path = My.Computer.FileSystem.SpecialDirectories.MyDocuments
                    Else
                        Path = My.Settings.CompareResultsFolder
                    End If
                    If Not Path.Last = "\" Then
                        Path += "\"
                    End If
                    Using sw As StreamWriter = New StreamWriter(String.Format("{0}\{1}", Path, My.Resources.CompareHTMLFileName))
                        sw.Write(My.Resources.CompareHTML)
                    End Using
                    Using sw As StreamWriter = New StreamWriter(String.Format("{0}\{1}", Path, My.Resources.CompareHTMLTitleFileName))
                        sw.Write(String.Format(My.Resources.CompareHTMLTitle, _
                                               smoTreeView1.SelectedNode.FullPath, _
                                               smoTreeView2.SelectedNode.FullPath))
                    End Using
                    Using sw As StreamWriter = New StreamWriter(String.Format("{0}\{1}", Path, My.Resources.CompareHTMLResultsAFileName))
                        ' no scroller on this side
                        sw.Write(String.Format(My.Resources.CompareHTMLResultsA, HTMLSide1))
                    End Using
                    Using sw As StreamWriter = New StreamWriter(String.Format("{0}\{1}", Path, My.Resources.CompareHTMLResultsBFileName))
                        ' scroller from http://www.webreference.com/programming/javascript/jf/column4/
                        'The javescript brakets {} won't work in the format target 
                        sw.Write(String.Format(My.Resources.CompareHTMLResultsB, _
                                               My.Resources.CompareHTMLScrollerJavaScript, _
                                               HTMLSide2))
                    End Using
                    Using sw As StreamWriter = New StreamWriter(String.Format("{0}\{1}", Path, My.Resources.CompareHTMLKeyFileName))
                        sw.Write(String.Format(My.Resources.CompareHTMLKey, _
                                               MakeRGB(My.Settings.CompareOutputBodyBackground), _
                                               MakeRGB(My.Settings.CompareOutputBodyText), _
                                               MakeRGB(My.Settings.CompareOutputDifferentBackground), _
                                               MakeRGB(My.Settings.CompareOutputDifferentText), _
                                               MakeRGB(My.Settings.CompareOutputNoMatchBackground), _
                                               MakeRGB(My.Settings.CompareOutputNoMatchText), _
                                               MakeRGB(My.Settings.CompareOutputHeaderBackground), _
                                               MakeRGB(My.Settings.CompareOutputHeaderText), _
                                               Now.ToString))
                    End Using
                End If
            Else
                If My.Settings.Misc_Display__Output_Show__Comparison__Details OrElse My.Settings.Misc_Display__Output_Show__Differences__Only Then
                    If Not Item1 = "" Then

                        RichTextBox1.SelectionStart = RichTextBox1.TextLength + 1
                        RichTextBox1.SelectionLength = Origin1.Text.Length + 11
                        RichTextBox1.SelectionColor = My.Settings.CompareOutputHeaderText
                        RichTextBox1.SelectionBackColor = My.Settings.CompareOutputHeaderBackground
                        RichTextBox1.AppendText(String.Format(My.Resources.CompareResultOriginLabel, _
                                                              If(Origin1.Text = My.Resources.OriginRepository, _
                                                                 Origin1.Tag.ToString, _
                                                                 Origin1.Text)) & vbCrLf)

                        HTMLSide1 += HTMLStartHeaderLine
                        HTMLSide1 += String.Format(My.Resources.CompareResultOriginLabel, _
                                                       If(Origin1.Text = My.Resources.OriginRepository, _
                                                          Origin1.Tag.ToString, _
                                                          Origin1.Text))
                        HTMLSide1 += My.Resources.CompareHTMLEndLine & vbCrLf
                        RichTextBox1.SelectionStart = RichTextBox1.TextLength + 1
                        RichTextBox1.SelectionLength = Item1.Length
                        RichTextBox1.SelectionColor = My.Settings.CompareOutputHeaderText
                        RichTextBox1.SelectionBackColor = My.Settings.CompareOutputHeaderBackground
                        HTMLSide1 += HTMLStartHeaderLine
                        If InStr(smoTreeView1.SelectedNode.FullPath.ToString, _
                                 Item1) > 0 _
                        AndAlso Mid(smoTreeView1.SelectedNode.FullPath.ToString, _
                                    InStr(smoTreeView1.SelectedNode.FullPath.ToString, _
                                    Item1)) = Item1 Then
                            RichTextBox1.AppendText(String.Format(My.Resources.CompareResultNodeLabel, _
                                                                  smoTreeView1.SelectedNode.FullPath.ToString) & vbCrLf)
                            HTMLSide1 += String.Format(My.Resources.CompareResultNodeLabel, _
                                                       smoTreeView1.SelectedNode.FullPath.ToString)
                        Else
                            RichTextBox1.AppendText(String.Format(My.Resources.CompareResultNodeItemLabel, _
                                                                  smoTreeView1.SelectedNode.FullPath.ToString, _
                                                                  Item1) & vbCrLf)
                            HTMLSide1 += String.Format(My.Resources.CompareResultNodeItemLabel, _
                                                       smoTreeView1.SelectedNode.FullPath.ToString, _
                                                       Item1)
                        End If
                        HTMLSide1 += My.Resources.CompareHTMLEndLine & vbCrLf
                        If My.Settings.SQL__Repository__A_Filters_Version = 0 Then
                            My.Settings.SQL__Repository__A_Filters_Version = CType(smoTreeView1.SelectedNode.Tag, cTreeView.structNodeTag).LatestVersion
                        End If

                        RichTextBox1.SelectionStart = RichTextBox1.TextLength + 1
                        Dim NewLine1 As String = String.Format(My.Resources.CompareResultVersionLabel, _
                                                               If(Origin1.Text = My.Resources.OriginRepository, _
                                                                  If(My.Settings.SQL__Repository__A_Filters_Version = 0, _
                                                                     CType(smoTreeView1.SelectedNode.Tag, cTreeView.structNodeTag).LatestVersion.ToString, _
                                                                     My.Settings.SQL__Repository__A_Filters_Version.ToString), _
                                                                  If(Origin1.Text = My.Resources.OriginSQLInstance, _
                                                                      "current", _
                                                                      "n/a")))
                        RichTextBox1.SelectionLength = NewLine1.Length
                        RichTextBox1.SelectionColor = My.Settings.CompareOutputHeaderText
                        RichTextBox1.SelectionBackColor = My.Settings.CompareOutputHeaderBackground
                        HTMLSide1 += HTMLStartHeaderLine
                        RichTextBox1.AppendText(NewLine1 & vbCrLf)
                        HTMLSide1 += NewLine1
                        HTMLSide1 += My.Resources.CompareHTMLEndLine & vbCrLf
                    Else
                        HTMLSide1 += HTMLStartHeaderLine & "&nbsp" & My.Resources.CompareHTMLEndLine & vbCrLf
                        HTMLSide1 += HTMLStartHeaderLine & "&nbsp" & My.Resources.CompareHTMLEndLine & vbCrLf
                        RichTextBox1.AppendText(Space(1) & vbCrLf & Space(1) & vbCrLf)
                    End If
                    If Not Item2 = "" Then
                        RichTextBox2.SelectionStart = RichTextBox2.TextLength + 1
                        RichTextBox2.SelectionLength = Origin2.Text.Length + 11
                        RichTextBox2.SelectionColor = My.Settings.CompareOutputHeaderText
                        RichTextBox2.SelectionBackColor = My.Settings.CompareOutputHeaderBackground
                        RichTextBox2.AppendText(String.Format(My.Resources.CompareResultOriginLabel, _
                                                              If(Origin2.Text = My.Resources.OriginRepository, _
                                                                 Origin2.Tag.ToString, _
                                                                 Origin2.Text)) & vbCrLf)
                        HTMLSide2 += HTMLStartHeaderLine
                        HTMLSide2 += String.Format(My.Resources.CompareResultOriginLabel, _
                                                       If(Origin2.Text = My.Resources.OriginRepository, _
                                                          Origin2.Tag.ToString, _
                                                          Origin2.Text))
                        HTMLSide2 += My.Resources.CompareHTMLEndLine & vbCrLf
                        RichTextBox2.SelectionStart = RichTextBox2.TextLength + 1
                        ' this is the wrong place to get the length from but seems to work
                        ' actuall adding the node string which can have two differnt lengths  
                        RichTextBox2.SelectionLength = Item2.Length
                        RichTextBox2.SelectionColor = My.Settings.CompareOutputHeaderText
                        RichTextBox2.SelectionBackColor = My.Settings.CompareOutputHeaderBackground
                        HTMLSide2 += HTMLStartHeaderLine
                        If InStr(smoTreeView2.SelectedNode.FullPath.ToString, _
                                  Item2) > 0 _
                        AndAlso Mid(smoTreeView2.SelectedNode.FullPath.ToString, _
                                    InStr(smoTreeView2.SelectedNode.FullPath.ToString, _
                                          Item2)) = Item2 Then
                            RichTextBox2.AppendText(String.Format(My.Resources.CompareResultNodeLabel, _
                                                                  smoTreeView2.SelectedNode.FullPath.ToString) & vbCrLf)
                            HTMLSide2 += String.Format(My.Resources.CompareResultNodeLabel, _
                                                           smoTreeView2.SelectedNode.FullPath.ToString)
                        Else
                            RichTextBox2.AppendText(String.Format(My.Resources.CompareResultNodeItemLabel, _
                                                                  smoTreeView2.SelectedNode.FullPath.ToString, _
                                                                  Item2) & vbCrLf)
                            HTMLSide2 += String.Format(My.Resources.CompareResultNodeItemLabel, _
                                                           smoTreeView2.SelectedNode.FullPath.ToString, _
                                                           Item2)
                        End If
                        HTMLSide2 += My.Resources.CompareHTMLEndLine & vbCrLf
                        Dim NewLine2 As String = String.Format(My.Resources.CompareResultVersionLabel, _
                                                               If(Origin2.Text = My.Resources.OriginRepository, _
                                                                  If(My.Settings.SQL__Repository__B_Filters_Version = 0, _
                                                                     CType(smoTreeView2.SelectedNode.Tag, cTreeView.structNodeTag).LatestVersion.ToString, _
                                                                     My.Settings.SQL__Repository__B_Filters_Version.ToString), _
                                                                  If(Origin2.Text = My.Resources.OriginSQLInstance, _
                                                                    "current", _
                                                                    "n/a")))
                        RichTextBox2.SelectionStart = RichTextBox2.TextLength + 1
                        ' maybe should use tem for length here too?
                        RichTextBox2.SelectionLength = NewLine2.Length
                        RichTextBox2.SelectionColor = My.Settings.CompareOutputHeaderText
                        RichTextBox2.SelectionBackColor = My.Settings.CompareOutputHeaderBackground
                        HTMLSide2 += HTMLStartHeaderLine
                        RichTextBox2.AppendText(NewLine2 & vbCrLf)
                        HTMLSide2 += String.Format(NewLine2)
                        HTMLSide2 += My.Resources.CompareHTMLEndLine & vbCrLf
                    Else
                        HTMLSide2 += HTMLStartHeaderLine & "&nbsp" & My.Resources.CompareHTMLEndLine & vbCrLf
                        HTMLSide2 += HTMLStartHeaderLine & "&nbsp" & My.Resources.CompareHTMLEndLine & vbCrLf
                        RichTextBox2.AppendText(Space(1) & vbCrLf & Space(1) & vbCrLf)
                    End If
                    WriteSpacer()
                    End If
            End If
        End If
    End Sub
    Private Function MakeRGB(ByVal clr As System.Drawing.Color) As String
        MakeRGB = If(Len(Hex(clr.R)) = 1, "0" & Hex(clr.R), Hex(clr.R)) & _
                  If(Len(Hex(clr.G)) = 1, "0" & Hex(clr.G), Hex(clr.G)) & _
                  If(Len(Hex(clr.B)) = 1, "0" & Hex(clr.B), Hex(clr.B))
    End Function


    Private Sub WriteResultEventHandler(ByVal ResultType As String, _
                                        ByVal Item1 As String, _
                                        ByVal Item2 As String)
        If Me.InvokeRequired Then
            Dim ParmBall As String()
            ReDim ParmBall(2)
            ParmBall(0) = ResultType
            ParmBall(1) = Item1
            ParmBall(2) = Item2
            Me.Invoke(New WriteResultEventHandlerDelegate(AddressOf WriteResultEventHandler), ParmBall)
        Else
            'munge the blank compare results before setting length
            If InStr(ResultType, "1Blank") > 0 Then
                If InStr(ResultType, "Name") = 1 _
                And Not My.Settings.Misc_Display__Output_Show__Comparison__Details Then
                    Item1 = Space(1)
                    Item2 = "-- " & smoTreeView2.SelectedNode.FullPath.ToString & ": " & Item2
                ElseIf InStr(ResultType, "Line") = 1 Then
                    Item1 = Space(1)
                End If
            ElseIf InStr(ResultType, "2Blank") > 0 Then
                If InStr(ResultType, "Name") = 1 _
                And Not My.Settings.Misc_Display__Output_Show__Comparison__Details Then
                    Item1 = "-- " & smoTreeView1.SelectedNode.FullPath.ToString & ": " & Item1
                    Item2 = Space(1)
                ElseIf InStr(ResultType, "Line") = 1 Then
                    Item2 = Space(1)
                End If
            End If
            RichTextBox1.SelectionStart = RichTextBox1.TextLength + 1
            RichTextBox1.SelectionLength = Item1.Length
            RichTextBox2.SelectionStart = RichTextBox2.TextLength + 1
            RichTextBox2.SelectionLength = Item2.Length
            If InStr(ResultType, "Match") > 0 Then
                If My.Settings.Misc_Display__Output_Show__Differences__Only And cCompare._CompareCollection Then
                    Exit Sub
                End If
                RichTextBox1.SelectionColor = My.Settings.CompareOutputBodyText
                RichTextBox1.SelectionBackColor = My.Settings.CompareOutputBodyBackground
                RichTextBox2.SelectionColor = My.Settings.CompareOutputBodyText
                RichTextBox2.SelectionBackColor = My.Settings.CompareOutputBodyBackground
                HTMLSide1 += String.Format(My.Resources.CompareHTMLStartLine, _
                                           MakeRGB(My.Settings.CompareOutputBodyBackground), _
                                           MakeRGB(My.Settings.CompareOutputHeaderText))
                HTMLSide2 += String.Format(My.Resources.CompareHTMLStartLine, _
                                           MakeRGB(My.Settings.CompareOutputBodyBackground), _
                                           MakeRGB(My.Settings.CompareOutputHeaderText))
            ElseIf InStr(ResultType, "Different") > 0 Then
                RichTextBox1.SelectionColor = My.Settings.CompareOutputDifferentText
                RichTextBox1.SelectionBackColor = My.Settings.CompareOutputDifferentBackground
                RichTextBox2.SelectionColor = My.Settings.CompareOutputDifferentText
                RichTextBox2.SelectionBackColor = My.Settings.CompareOutputDifferentBackground
                HTMLSide1 += String.Format(My.Resources.CompareHTMLStartLine, _
                                           MakeRGB(My.Settings.CompareOutputDifferentBackground), _
                                           MakeRGB(My.Settings.CompareOutputDifferentText))
                HTMLSide2 += String.Format(My.Resources.CompareHTMLStartLine, _
                                           MakeRGB(My.Settings.CompareOutputDifferentBackground), _
                                           MakeRGB(My.Settings.CompareOutputDifferentText))
            ElseIf InStr(ResultType, "Blank") > 0 Then
                RichTextBox1.SelectionColor = My.Settings.CompareOutputNoMatchText
                RichTextBox1.SelectionBackColor = My.Settings.CompareOutputNoMatchBackground
                RichTextBox2.SelectionColor = My.Settings.CompareOutputNoMatchText
                RichTextBox2.SelectionBackColor = My.Settings.CompareOutputNoMatchBackground
                HTMLSide1 += String.Format(My.Resources.CompareHTMLStartLine, _
                                           MakeRGB(My.Settings.CompareOutputNoMatchBackground), _
                                           MakeRGB(My.Settings.CompareOutputNoMatchText))
                HTMLSide2 += String.Format(My.Resources.CompareHTMLStartLine, _
                                           MakeRGB(My.Settings.CompareOutputNoMatchBackground), _
                                           MakeRGB(My.Settings.CompareOutputNoMatchText))
            End If
            HTMLSide1 += If(Trim(Item1) = "", "&nbsp;", Replace(Replace(Item1, "<", "&lt;"), ">", "&gt;"))
            HTMLSide1 += My.Resources.CompareHTMLEndLine & vbCrLf
            RichTextBox1.AppendText(Item1 & vbCrLf)
            HTMLSide2 += If(Trim(Item2) = "", "&nbsp;", Replace(Replace(Item2, "<", "&lt;"), ">", "&gt;"))
            HTMLSide2 += My.Resources.CompareHTMLEndLine & vbCrLf
            RichTextBox2.AppendText(Item2 & vbCrLf)
            WriteSpacer()
        End If
    End Sub

    Private Sub WriteLineResultEventHandler(ByVal ResultType As String, _
                                            ByVal Item1 As String, _
                                            ByVal Line1Number As Int32, _
                                            ByVal Line1 As String, _
                                            ByVal Item2 As String, _
                                            ByVal Line2Number As Int32, _
                                            ByVal Line2 As String)
        If Me.InvokeRequired Then
            Dim ParmBall As Object()
            ReDim ParmBall(6)
            ParmBall(0) = ResultType
            ParmBall(1) = Item1
            ParmBall(2) = Line1Number
            ParmBall(3) = Line1
            ParmBall(4) = Item2
            ParmBall(5) = Line2Number
            ParmBall(6) = Line2
            Me.Invoke(New WriteLineResultEventHandlerDelegate(AddressOf WriteLineResultEventHandler), ParmBall)
        Else
            If My.Settings.Misc_Display__Output_Number__Script__Lines Then
                ' couples compare2items to the event handler by rule for special case
                Line1 = If(Line1Number = -1, Line1, String.Format("{0}  {1}", Line1Number + 1, Line1))
                Line2 = If(Line2Number = -1, Line2, String.Format("{0}  {1}", Line2Number + 1, Line2))
            End If

            If My.Settings.Misc_Display__Output_Show__Object__Name__on__Script__Lines Then
                ' no labels, only numbers if comparing a single item
                If smoTreeView1.SelectedNode Is Nothing OrElse CType(smoTreeView1.SelectedNode.Tag, cTreeView.structNodeTag).IsCollection Then
                    Line1 = String.Format("{0} | {1}", Item1, Line1)
                    Line2 = String.Format("{0} | {1}", Item2, Line2)
                End If
            End If
            WriteResultEventHandler(ResultType, Line1, Line2)
        End If
    End Sub

    Private Sub WriteSpacer()
        Dim SaveFont1 As Font = RichTextBox1.Font
        RichTextBox1.SelectionFont = New Font(SaveFont1.FontFamily, 1, FontStyle.Regular)
        RichTextBox1.SelectionStart = RichTextBox1.TextLength + 1
        RichTextBox1.SelectionLength = 1
        RichTextBox1.AppendText(" " & vbCrLf)
        RichTextBox1.SelectionFont = SaveFont1
        Dim SaveFont2 As Font = RichTextBox2.Font
        RichTextBox2.SelectionFont = New Font(SaveFont2.FontFamily, 1, FontStyle.Regular)
        RichTextBox2.SelectionStart = RichTextBox2.TextLength + 1
        RichTextBox2.SelectionLength = 1
        RichTextBox2.AppendText(" " & vbCrLf)
        RichTextBox2.SelectionFont = SaveFont2
    End Sub

    Private Sub ShowBasicHelp()
        RichTextBox1.AppendText("  side 'A' comparison results" & vbCrLf & _
                                "  appear in this output panel" & vbCrLf & _
                                "" & vbCrLf & _
                                "" & vbCrLf & _
                                "Configurable Compare Settings" & vbCrLf & _
                                "  Many settings, defaults work great" & vbCrLf & _
                                "  Change from Tools menu" & vbCrLf & _
                                "    Regular Expressions" & vbCrLf & _
                                "    SMO ScriptingOptions" & vbCrLf & _
                                "    Output formating" & vbCrLf & _
                                "  Saved after each compare")
        ' move to the top of textbox
        RichTextBox1.Select(1, 1)
        RichTextBox1.DeselectAll()
        RichTextBox2.AppendText("  side 'B' comparison results" & vbCrLf & _
                                "  appear in this output panel" & vbCrLf & _
                                "" & vbCrLf & _
                                "" & vbCrLf & _
                                "  Compare Quick-Start" & vbCrLf & _
                                "" & vbCrLf & _
                                "  Compare two items" & vbCrLf & _
                                "  1. Select Origin A from list" & vbCrLf & _
                                "  2. Connect using dialog" & vbCrLf & _
                                "  3. Select item to compare" & vbCrLf & _
                                "  4. Repeat for Origin B" & vbCrLf & _
                                "  5. Tap the Compare button")
        ' move to the top of textbo
        RichTextBox2.Select(1, 1)
        RichTextBox2.DeselectAll()
    End Sub

    Private Sub PercentDoneEventHandler(ByVal PercentDone As Int32)
        If Me.InvokeRequired Then
            Dim ParmBall As Integer()
            ReDim ParmBall(1)
            ParmBall(0) = PercentDone
            Me.Invoke(New ComparingEventHandlerDelegate(AddressOf ComparingEventHandler), ParmBall)
        Else
            ToolStripProgressBar1.Value = PercentDone
            ToolStripProgressBar1.ToolTipText = String.Format("{0}%", PercentDone)
            My.Application.DoEvents()
        End If
    End Sub

#End Region

#Region " Work with Settings "

    Private Sub UpdateAutoCompleteLists()
        ' updates are pushed to the ap.config
        With My.Settings
            If Not (.Regular__Expressions_NameMatch_AutoComplete__Patterns.Contains(.Regular__Expressions_NameMatch__Pattern_4)) Then
                My.Settings.Regular__Expressions_NameMatch_AutoComplete__Patterns.Add(.Regular__Expressions_NameMatch__Pattern_4)
            End If
            If Not (.Regular__Expressions_LineSplit_AutoComplete__Patterns.Contains(.Regular__Expressions_LineSplit__Pattern_3)) Then
                .Regular__Expressions_LineSplit_AutoComplete__Patterns.Add(.Regular__Expressions_LineSplit__Pattern_3)
            End If
            If Not (.Regular__Expressions_LineReplace_AutoComplete__Patterns.Contains(.Regular__Expressions_LineReplace__Pattern_1)) Then
                .Regular__Expressions_LineReplace_AutoComplete__Patterns.Add(.Regular__Expressions_LineReplace__Pattern_1)
            End If
            If Not (.Regular__Expressions_LineReplace_AutoComplete__Replacements.Contains(.Regular__Expressions_LineReplace__Replacement_2)) Then
                .Regular__Expressions_LineReplace_AutoComplete__Replacements.Add(.Regular__Expressions_LineReplace__Replacement_2)
            End If
        End With
    End Sub

    Friend Sub SyncWorkingValues()
        With My.Settings
            cCompare._BatchSeparator = .Scripting__Options_Batch__Separator
            cCompare._IgnoreBlankLines = .Misc_Ignore__Blank__Lines
            cCompare._ReportDetails = .Misc_Display__Output_Show__Comparison__Details
            cCompare._IncludeDrop = .Scripting__Options_Include__DROP__In__Script
            cCompare._IncludeIfExistsWithDrop = .Scripting__Options_Include__IF__EXISTS__With__Drop
            cCompare._IncludeUnmatched = .Misc_Display__Output_Show__Scripts__For__Unmatched__Items
            Dim NameMatchOptions As New RegexOptions
            NameMatchOptions = RegexOptions.None
            If .Regular__Expressions_NameMatch__Regex__Options_IgnoreCase_5 Then
                NameMatchOptions = (NameMatchOptions Or RegexOptions.IgnoreCase)
            End If
            If .Regular__Expressions_NameMatch__Regex__Options_Multiline_7 Then
                NameMatchOptions = (NameMatchOptions Or RegexOptions.Multiline)
            End If
            If .Regular__Expressions_NameMatch__Regex__Options_ExplicitCapture_4 Then
                NameMatchOptions = (NameMatchOptions Or RegexOptions.ExplicitCapture)
            End If
            If .Regular__Expressions_NameMatch__Regex__Options_Compiled_1 Then
                NameMatchOptions = (NameMatchOptions Or RegexOptions.Compiled)
            End If
            If .Regular__Expressions_NameMatch__Regex__Options_SingleLine_9 Then
                NameMatchOptions = (NameMatchOptions Or RegexOptions.Singleline)
            End If
            If .Regular__Expressions_NameMatch__Regex__Options_IgnorePatternWhiteSpace_6 Then
                NameMatchOptions = (NameMatchOptions Or RegexOptions.IgnorePatternWhitespace)
            End If
            If .Regular__Expressions_NameMatch__Regex__Options_RightToLeft_8 Then
                NameMatchOptions = (NameMatchOptions Or RegexOptions.RightToLeft)
            End If
            'can only be used with ignore case, multilne and compiled
            If .Regular__Expressions_NameMatch__Regex__Options_ECMAScript_3 Then
                If .Regular__Expressions_NameMatch__Regex__Options_ExplicitCapture_4 _
                Or .Regular__Expressions_NameMatch__Regex__Options_SingleLine_9 _
                Or .Regular__Expressions_NameMatch__Regex__Options_IgnorePatternWhiteSpace_6 _
                Or .Regular__Expressions_NameMatch__Regex__Options_RightToLeft_8 _
                Or .Regular__Expressions_NameMatch__Regex__Options_CultureInvariant_2 Then
                    .Regular__Expressions_NameMatch__Regex__Options_ECMAScript_3 = False
                    Throw New Exception(My.Resources.InvalidECMAScriptSelection)
                Else
                    NameMatchOptions = (NameMatchOptions Or RegexOptions.ECMAScript)
                End If
            End If
            If .Regular__Expressions_NameMatch__Regex__Options_CultureInvariant_2 Then
                NameMatchOptions = (NameMatchOptions Or RegexOptions.CultureInvariant)
            End If
            If NameMatchOptions.CompareTo(cCompare._NameMatchOptions) <> 0 _
            Or Not (.Regular__Expressions_NameMatch__Pattern_4 = cCompare._NameMatchPattern) _
            Or cCompare._NameMatchPattern Is Nothing Then
                cCompare._NameMatchPattern = .Regular__Expressions_NameMatch__Pattern_4
                cCompare._NameMatchOptions = NameMatchOptions
            End If
            Dim LineSplitOptions As New RegexOptions
            LineSplitOptions = RegexOptions.None
            If .Regular__Expressions_LineSplit__Regex__Options_IgnoreCase_5 Then
                LineSplitOptions = (LineSplitOptions Or RegexOptions.IgnoreCase)
            End If
            If .Regular__Expressions_LineSplit__Regex__Options_Multiline_7 Then
                LineSplitOptions = (LineSplitOptions Or RegexOptions.Multiline)
            End If
            If .Regular__Expressions_LineSplit__Regex__Options_ExplicitCapture_4 Then
                LineSplitOptions = (LineSplitOptions Or RegexOptions.ExplicitCapture)
            End If
            If .Regular__Expressions_LineSplit__Regex__Options_Compiled_1 Then
                LineSplitOptions = (LineSplitOptions Or RegexOptions.Compiled)
            End If
            If .Regular__Expressions_LineSplit__Regex__Options_SingleLine_9 Then
                LineSplitOptions = (LineSplitOptions Or RegexOptions.Singleline)
            End If
            If .Regular__Expressions_LineSplit__Regex__Options_IgnorePatternWhiteSpace_6 Then
                LineSplitOptions = (LineSplitOptions Or RegexOptions.IgnorePatternWhitespace)
            End If
            If .Regular__Expressions_LineSplit__Regex__Options_RightToLeft_8 Then
                LineSplitOptions = (LineSplitOptions Or RegexOptions.RightToLeft)
            End If
            'can only be used with ignore case, multilne and compiled
            If .Regular__Expressions_LineSplit__Regex__Options_ECMAScript_3 Then
                If .Regular__Expressions_LineSplit__Regex__Options_ExplicitCapture_4 _
                Or .Regular__Expressions_LineSplit__Regex__Options_SingleLine_9 _
                Or .Regular__Expressions_LineSplit__Regex__Options_IgnorePatternWhiteSpace_6 _
                Or .Regular__Expressions_LineSplit__Regex__Options_RightToLeft_8 _
                Or .Regular__Expressions_LineSplit__Regex__Options_CultureInvariant_2 Then
                    .Regular__Expressions_LineSplit__Regex__Options_ECMAScript_3 = False
                    Throw New Exception(My.Resources.InvalidECMAScriptSelection)
                Else
                    LineSplitOptions = (LineSplitOptions Or RegexOptions.ECMAScript)
                End If
            End If
            If .Regular__Expressions_LineSplit__Regex__Options_CultureInvariant_2 Then
                LineSplitOptions = (LineSplitOptions Or RegexOptions.CultureInvariant)
            End If
            If LineSplitOptions.CompareTo(cCompare._LineSplitOptions) <> 0 _
            Or Not (cCompare._LineSplitPattern = .Regular__Expressions_LineSplit__Pattern_3) _
            Or cCompare._LineSplitPattern Is Nothing Then
                cCompare._LineSplitOptions = LineSplitOptions
                cCompare._LineSplitPattern = .Regular__Expressions_LineSplit__Pattern_3
                RichTextBox1.Clear()
                HTMLSide1 = ""
                RichTextBox2.Clear()
                HTMLSide2 = ""
            End If
            Dim LineReplaceOptions As New RegexOptions
            LineReplaceOptions = RegexOptions.None
            If .Regular__Expressions_LineReplace__Regex__Options_IgnoreCase_5 Then
                LineReplaceOptions = (LineReplaceOptions Or RegexOptions.IgnoreCase)
            End If
            If .Regular__Expressions_LineReplace__Regex__Options_Multiline_7 Then
                LineReplaceOptions = (LineReplaceOptions Or RegexOptions.Multiline)
            End If
            If .Regular__Expressions_LineReplace__Regex__Options_ExplicitCapture_4 Then
                LineReplaceOptions = (LineReplaceOptions Or RegexOptions.ExplicitCapture)
            End If
            If .Regular__Expressions_LineReplace__Regex__Options_Compiled_1 Then
                LineReplaceOptions = (LineReplaceOptions Or RegexOptions.Compiled)
            End If
            If .Regular__Expressions_LineReplace__Regex__Options_SingleLine_9 Then
                LineReplaceOptions = (LineReplaceOptions Or RegexOptions.Singleline)
            End If
            If .Regular__Expressions_LineReplace__Regex__Options_IgnorePatternWhiteSpace_6 Then
                LineReplaceOptions = (LineReplaceOptions Or RegexOptions.IgnorePatternWhitespace)
            End If
            If .Regular__Expressions_LineReplace__Regex__Options_RightToLeft_8 Then
                LineReplaceOptions = (LineReplaceOptions Or RegexOptions.RightToLeft)
            End If
            'can only be used with ignore case, multilne and compiled
            If .Regular__Expressions_LineReplace__Regex__Options_ECMAScript_3 Then
                If .Regular__Expressions_LineReplace__Regex__Options_ExplicitCapture_4 _
                Or .Regular__Expressions_LineReplace__Regex__Options_SingleLine_9 _
                Or .Regular__Expressions_LineReplace__Regex__Options_IgnorePatternWhiteSpace_6 _
                Or .Regular__Expressions_LineReplace__Regex__Options_RightToLeft_8 _
                Or .Regular__Expressions_LineReplace__Regex__Options_CultureInvariant_2 Then
                    .Regular__Expressions_LineReplace__Regex__Options_ECMAScript_3 = False
                    Throw New Exception(My.Resources.InvalidECMAScriptSelection)
                Else
                    LineReplaceOptions = (LineReplaceOptions Or RegexOptions.ECMAScript)
                End If
            End If
            If .Regular__Expressions_LineReplace__Regex__Options_CultureInvariant_2 Then
                LineReplaceOptions = (LineReplaceOptions Or RegexOptions.CultureInvariant)
            End If
            If LineReplaceOptions.CompareTo(cCompare._LineReplaceOptions) <> 0 _
            Or Not (cCompare._LineReplacePattern = .Regular__Expressions_LineReplace__Pattern_1) _
            Or Not (cCompare._LineReplacement = .Regular__Expressions_LineReplace__Replacement_2) _
            Or cCompare._LineReplacePattern Is Nothing _
            Or cCompare._LineReplacement Is Nothing Then
                cCompare._LineReplaceOptions = LineReplaceOptions
                cCompare._LineReplacePattern = .Regular__Expressions_LineReplace__Pattern_1
                cCompare._LineReplacement = .Regular__Expressions_LineReplace__Replacement_2
                RichTextBox1.Clear()
                HTMLSide1 = ""
                RichTextBox2.Clear()
                HTMLSide2 = ""
            End If
            Dim TryScriptingOptions As New ScriptingOptions
            TryScriptingOptions.AgentAlertJob = .Scripting__Options_SMO_AgentAlertJob_1
            TryScriptingOptions.AgentJobId = .Scripting__Options_SMO_AgentJobId_2
            TryScriptingOptions.AgentNotify = .Scripting__Options_SMO_AgentNotify_3
            TryScriptingOptions.AllowSystemObjects = .Scripting__Options_SMO_AllowSystemObjects_10
            TryScriptingOptions.AnsiFile = .Scripting__Options_SMO_AnsiFile_20
            TryScriptingOptions.AnsiPadding = .Scripting__Options_SMO_AnsiPadding_30
            TryScriptingOptions.BatchSize = .Scripting__Options_SMO_BatchSize_35
            TryScriptingOptions.Bindings = .Scripting__Options_SMO_Bindings_40
            TryScriptingOptions.ChangeTracking = .Scripting__Options_SMO_ChangeTracking_45
            TryScriptingOptions.ClusteredIndexes = .Scripting__Options_SMO_ClusteredIndexes_50
            TryScriptingOptions.ContinueScriptingOnError = .Scripting__Options_SMO_ContinueScriptingOnError_60
            TryScriptingOptions.ConvertUserDefinedDataTypesToBaseType = .Scripting__Options_SMO_ConvertUserDefinedDataTypesToBaseType_70
            TryScriptingOptions.DdlBodyOnly = .Scripting__Options_SMO_DdlBodyOnly_80
            TryScriptingOptions.DdlHeaderOnly = .Scripting__Options_SMO_DdlHeaderOnly_90
            TryScriptingOptions.Default = .Scripting__Options_SMO_Default_100
            TryScriptingOptions.DriAll = .Scripting__Options_SMO_DriAll_110
            TryScriptingOptions.DriAllConstraints = .Scripting__Options_SMO_DriAllConstraints_120
            TryScriptingOptions.DriAllKeys = .Scripting__Options_SMO_DriAllKeys_130
            TryScriptingOptions.DriChecks = .Scripting__Options_SMO_DriChecks_140
            TryScriptingOptions.DriClustered = .Scripting__Options_SMO_DriClustered_150
            TryScriptingOptions.DriDefaults = .Scripting__Options_SMO_DriDefaults_160
            TryScriptingOptions.DriForeignKeys = .Scripting__Options_SMO_DriForeignKeys_170
            TryScriptingOptions.DriIncludeSystemNames = .Scripting__Options_SMO_DriIncludeSystemNames_180
            TryScriptingOptions.DriIndexes = .Scripting__Options_SMO_DriIndexes_190
            TryScriptingOptions.DriNonClustered = .Scripting__Options_SMO_DriNonClustered_200
            TryScriptingOptions.DriPrimaryKey = .Scripting__Options_SMO_DriPrimaryKey_210
            TryScriptingOptions.DriUniqueKeys = .Scripting__Options_SMO_DriUniqueKeys_220
            TryScriptingOptions.DriWithNoCheck = .Scripting__Options_SMO_DriWithNoCheck_230
            If Not (.Scripting__Options_SMO_Encoding_240 = "") Then
                TryScriptingOptions.Encoding = System.Text.Encoding.GetEncoding(.Scripting__Options_SMO_Encoding_240)
            End If
            TryScriptingOptions.EnforceScriptingOptions = .Scripting__Options_SMO_EnforceScriptingOptions_250
            TryScriptingOptions.ExtendedProperties = .Scripting__Options_SMO_ExtendedProperties_260
            TryScriptingOptions.FullTextCatalogs = .Scripting__Options_SMO_FullTextCatalogs_270
            TryScriptingOptions.FullTextIndexes = .Scripting__Options_SMO_FullTextindexes_280
            TryScriptingOptions.FullTextStopLists = .Scripting__Options_SMO_FullTextStopLists_285
            TryScriptingOptions.IncludeDatabaseContext = .Scripting__Options_SMO_IncludeDatabaseContext_290
            TryScriptingOptions.IncludeDatabaseRoleMemberships = .Scripting__Options_SMO_IncludeDatabaseRoleMemberships_291
            TryScriptingOptions.IncludeFullTextCatalogRootPath = .Scripting__Options_SMO_IncludeFullTextCatalogRootPath_295
            TryScriptingOptions.IncludeHeaders = .Scripting__Options_SMO_IncludeHeaders_300
            TryScriptingOptions.IncludeIfNotExists = .Scripting__Options_SMO_IncludeIfNotExists_301
            TryScriptingOptions.Indexes = .Scripting__Options_SMO_Indexes_310
            TryScriptingOptions.LoginSid = .Scripting__Options_SMO_LoginSID_320
            TryScriptingOptions.NoAssemblies = .Scripting__Options_SMO_NoAssemblies_330
            TryScriptingOptions.NoCollation = .Scripting__Options_SMO_NoCollation_340
            TryScriptingOptions.NoCommandTerminator = .Scripting__Options_SMO_NoCommandTerminator_350
            TryScriptingOptions.NoExecuteAs = .Scripting__Options_SMO_NoExecuteAs_360
            TryScriptingOptions.NoFileGroup = .Scripting__Options_SMO_NoFilegroup_370
            TryScriptingOptions.NoFileStream = .Scripting__Options_SMO_NoFileStream_373
            TryScriptingOptions.NoFileStreamColumn = .Scripting__Options_SMO_NoFileStreamColumn_376
            TryScriptingOptions.NoIdentities = .Scripting__Options_SMO_NoIdentities_380
            TryScriptingOptions.NoIndexPartitioningSchemes = .Scripting__Options_SMO_NoIndexPartitioningSchemes_390
            TryScriptingOptions.NoMailProfileAccounts = .Scripting__Options_SMO_NoMailProfileAccounts_400
            TryScriptingOptions.NoMailProfilePrincipals = .Scripting__Options_SMO_NoMailProfilePrincipals_410
            TryScriptingOptions.NonClusteredIndexes = .Scripting__Options_SMO_NonClusteredIndexes_420
            TryScriptingOptions.NoTablePartitioningSchemes = .Scripting__Options_SMO_NoTablePartitioningSchemes_430
            TryScriptingOptions.NoVardecimal = .Scripting__Options_SMO_NoVardecimal_431
            TryScriptingOptions.NoViewColumns = .Scripting__Options_SMO_NoViewColumns_440
            TryScriptingOptions.NoXmlNamespaces = .Scripting__Options_SMO_NoXMLNameSpaces_450
            TryScriptingOptions.OptimizerData = .Scripting__Options_SMO_OptimizerData_460
            TryScriptingOptions.Permissions = .Scripting__Options_SMO_Permissions_470
            TryScriptingOptions.PrimaryObject = .Scripting__Options_SMO_PrimaryObject_480
            TryScriptingOptions.SchemaQualify = .Scripting__Options_SMO_SchemaQualify_490
            TryScriptingOptions.SchemaQualifyForeignKeysReferences = .Scripting__Options_SMO_SchemaQualifyForeignKeysReferences_500
            TryScriptingOptions.ScriptBatchTerminator = .Scripting__Options_SMO_ScriptBatchTerminator_502
            TryScriptingOptions.ScriptData = .Scripting__Options_SMO_ScriptData_503
            TryScriptingOptions.ScriptDataCompression = .Scripting__Options_SMO_ScriptDataCompression_505
            TryScriptingOptions.ScriptDrops = .Scripting__Options_SMO_ScriptDrops_506
            TryScriptingOptions.ScriptOwner = .Scripting__Options_SMO_ScriptOwner_508
            TryScriptingOptions.ScriptSchema = .Scripting__Options_SMO_ScriptSchema_509
            TryScriptingOptions.Statistics = .Scripting__Options_SMO_Statistics_510
            If Not .Scripting__Options_SMO_TargetServerVersion_530 = 0 Then
                TryScriptingOptions.TargetServerVersion = .Scripting__Options_SMO_TargetServerVersion_530
            End If
            TryScriptingOptions.TimestampToBinary = .Scripting__Options_SMO_TimestampToBinary_520
            TryScriptingOptions.Triggers = .Scripting__Options_SMO_Triggers_540
            TryScriptingOptions.WithDependencies = .Scripting__Options_SMO_WithDependencies_550
            TryScriptingOptions.XmlIndexes = .Scripting__Options_SMO_XMLIndexes_560
            If Not (TryScriptingOptions.ToString = cCompare._ScriptingOptions.ToString) Then
                cCompare._ScriptingOptions = TryScriptingOptions
            End If

            ' these never carry over from last run
            My.Settings.SQL__Repository__A_Filters_Label = ""
            My.Settings.SQL__Repository__A_Filters_Version = 0

            My.Settings.SQL__Repository__B_Filters_Label = ""
            My.Settings.SQL__Repository__B_Filters_Version = 0

        End With
    End Sub

    Private Function GetSQLConnection1() As Boolean
        Try
            If Not (cCompare.SqlServer1 Is Nothing) Then
                If cCompare.SqlServer1.ConnectionContext.IsOpen Then
                    cCompare.SqlServer1.ConnectionContext.Disconnect()
                End If
                cCompare.SqlServer1 = Nothing
            End If
            My.Settings.SQL__Instance__A_Name_1 = Instance1.Text
            cCompare.SqlServer1 = New Server(My.Settings.SQL__Instance__A_Name_1)
            Dim cn As DialogConnect = New DialogConnect
            Dim result As DialogResult = cn.ShowDialog(cCompare.SqlServer1, _
                          My.Settings.SQL__Instance__A_Name_1, _
                          My.Settings.SQL__Instance__A_Use__Trusted__Connection_2, _
                          My.Settings.SQL__Instance__A_SQL__Login__Name_3, _
                          My.Settings.SQL__Instance__A_SQL__Login__Password_4, _
                          My.Settings.SQL__Instance__A_Connection__Timeout_5, _
                          My.Settings.SQL__Instance__A_Network__Protocol_8, _
                          My.Settings.SQL__Instance__A_Encrypted__Connection_6, _
                          My.Settings.SQL__Instance__A_Trust__Server__Certificate_7, _
                          Me)
            If result = Windows.Forms.DialogResult.OK Then
                My.Settings.Save()
                ' if the server just connected is not in the InstanceList, add it
                If Not Mother.InstanceList.Contains(UCase(My.Settings.SQL__Instance__A_Name_1)) Then
                    ' the array is 0 based, so redim to the length is perfect for add one
                    ReDim Preserve Mother.InstanceList(Mother.InstanceList.Length)
                    Mother.InstanceList(Mother.InstanceList.Length - 1) = UCase(My.Settings.SQL__Instance__A_Name_1)
                    ' reload the combo drop down
                    Instance1.Items.Clear()
                End If
                ' no need to reload if already identical
                If Instance1.Items.Count = 0 Then
                    Instance1.Items.AddRange(Mother.InstanceList)
                    Instance1.Sorted = True
                End If
                ' this fires the Instance1.SelectedIndexChanged
                Instance1.Text = My.Settings.SQL__Instance__A_Name_1
                Me.ToolStripStatusLabel1.Text = String.Format(My.Resources.ConnectedToSQL, _
                                                              ToolStripStatusLabel1.Tag, _
                                                              My.Settings.SQL__Instance__A_Name_1)
                cCompare.SetSmoFieldsToLoad(cCompare.SqlServer1)
                Return True
            Else
                Me.ToolStripStatusLabel1.Text = String.Format(My.Resources.ConnectionToSQLFailed, _
                                                              ToolStripStatusLabel1.Tag, _
                                                              Instance1.Text)
                Instance1.Text = ""
                Return False
            End If
        Catch ex As Exception
            Me.ToolStripStatusLabel1.Text = String.Format(My.Resources.ConnectionToSQLFailed, _
                                                          ToolStripStatusLabel1.Tag, _
                                                          Instance1.Text)
            Mother.HandleException(ex)
            Return False
        End Try
    End Function

    Private Function GetSQLConnection2() As Boolean
        Try
            If Not (cCompare.SqlServer2 Is Nothing) Then
                If cCompare.SqlServer2.ConnectionContext.IsOpen Then
                    cCompare.SqlServer2.ConnectionContext.Disconnect()
                End If
                cCompare.SqlServer2 = Nothing
            End If
            My.Settings.SQL__Instance__B_Name_1 = Instance2.Text
            cCompare.SqlServer2 = New Server(My.Settings.SQL__Instance__B_Name_1)
            Dim cn As DialogConnect = New DialogConnect
            Dim result As DialogResult = cn.ShowDialog(cCompare.SqlServer2, _
                          My.Settings.SQL__Instance__B_Name_1, _
                          My.Settings.SQL__Instance__B_Use__Trusted__Connection_2, _
                          My.Settings.SQL__Instance__B_SQL__Login__Name_3, _
                          My.Settings.SQL__Instance__B_SQL__Login__Password_4, _
                          My.Settings.SQL__Instance__B_Connection__Timeout_5, _
                          My.Settings.SQL__Instance__B_Network__Protocol_8, _
                          My.Settings.SQL__Instance__B_Encrypted__Connection_6, _
                          My.Settings.SQL__Instance__B_Trust__Server__Certificate_7, _
                          Me)
            If result = Windows.Forms.DialogResult.OK Then
                My.Settings.Save()
                If Not Mother.InstanceList.Contains(UCase(My.Settings.SQL__Instance__B_Name_1)) Then
                    ReDim Preserve Mother.InstanceList(Mother.InstanceList.Length)
                    Mother.InstanceList(Mother.InstanceList.Length - 1) = UCase(My.Settings.SQL__Instance__B_Name_1)
                    Instance2.Items.Clear()
                End If
                If Instance2.Items.Count = 0 Then
                    Instance2.Items.AddRange(Mother.InstanceList)
                    Instance2.Sorted = True
                End If
                ' this fires the Instance2.SelectedIndexChanged
                Instance2.Text = My.Settings.SQL__Instance__B_Name_1
                Me.ToolStripStatusLabel2.Text = String.Format(My.Resources.ConnectedToSQL, _
                                                              ToolStripStatusLabel2.Tag, _
                                                              My.Settings.SQL__Instance__B_Name_1)
                cCompare.SetSmoFieldsToLoad(cCompare.SqlServer2)
                Return True
            Else
                Me.ToolStripStatusLabel2.Text = String.Format(My.Resources.ConnectionToSQLFailed, _
                                                              ToolStripStatusLabel2.Tag, _
                                                              Instance2.Text)
                Instance2.Text = ""
                Return False
            End If
        Catch ex As Exception
            Me.ToolStripStatusLabel2.Text = String.Format(My.Resources.ConnectionToSQLFailed, _
                                                          ToolStripStatusLabel2.Tag, _
                                                          Instance2.Text)
            Mother.HandleException(ex)
            Return False
        End Try
    End Function

    Private Function GetRepositoryConnection1() As Boolean
        Try
            ' if is local repository, reuse the connection and avoid the login prompt
            If Not (cCompare.Repository1 Is Nothing) Then
                If cCompare.Repository1.ConnectionContext.IsOpen Then
                    cCompare.Repository1.ConnectionContext.Disconnect()
                End If
                cCompare.Repository1 = Nothing
            End If
            cCompare.Repository1 = New Server()
            If My.Settings.RepositoryInstanceName = My.Settings.SQL__Repository__A_Name_1 _
            And My.Settings.RepositoryDatabaseName = My.Settings.SQL__Repository__A_Database__Name_2 _
            And My.Settings.RepositoryUseTrustedConnection _
            And My.Settings.SQL__Repository__A_Use__Trusted__Connection_3 Then
                cCompare.Repository1.ConnectionContext.ConnectionString = Mother.DAL.LocalRepositoryConnectionString
                Return True
            Else
                Dim cn As DialogConnect = New DialogConnect
                ' cn settings are by ref so will be changed if modified in .show overload
                Dim result As DialogResult = cn.ShowDialogForRepository(cCompare.Repository1, _
                              My.Settings.SQL__Repository__A_Name_1, _
                              My.Settings.SQL__Repository__A_Database__Name_2, _
                              My.Settings.SQL__Repository__A_Use__Trusted__Connection_3, _
                              My.Settings.SQL__Repository__A_SQL__Login__Name_4, _
                              My.Settings.SQL__Repository__A_SQL__Login__Password_5, _
                              My.Settings.SQL__Repository__A_Connection__Timeout_7, _
                              My.Settings.SQL__Repository__A_Network__Protocol_9, _
                              My.Settings.SQL__Repository__A_Encrypted__Connection_6, _
                              My.Settings.SQL__Repository__A_Trust__Server__Certificate_8, _
                              Me)
                If result = Windows.Forms.DialogResult.OK Then
                    My.Settings.Save()
                    Me.ToolStripStatusLabel1.Text = String.Format(My.Resources.ConnectedToSQL, _
                                                                  Me.ToolStripStatusLabel1.Text, _
                                                                  My.Settings.SQL__Repository__A_Name_1)
                    Return True
                Else
                    Me.ToolStripStatusLabel1.Text = String.Format(My.Resources.ConnectionToSQLFailed, _
                                                                  Me.ToolStripStatusLabel1.Text, _
                                                                  My.Settings.SQL__Repository__A_Name_1)
                    Return False
                End If
            End If
        Catch ex As Exception
            Me.ToolStripStatusLabel1.Text = String.Format(My.Resources.ConnectionToSQLFailed, _
                                                          Me.ToolStripStatusLabel1.Text, _
                                                          My.Settings.SQL__Repository__A_Name_1)
            Mother.HandleException(ex)
            Return False
        End Try
    End Function

    Private Function GetRepositoryConnection2() As Boolean
        Try
            ' if is local repository, reuse the connection and avoid the login prompt
            If Not (cCompare.Repository2 Is Nothing) Then
                If cCompare.Repository2.ConnectionContext.IsOpen Then
                    cCompare.Repository2.ConnectionContext.Disconnect()
                End If
                cCompare.Repository2 = Nothing
            End If
            cCompare.Repository2 = New Server()
            If My.Settings.RepositoryInstanceName = My.Settings.SQL__Repository__B_Name_1 _
            And My.Settings.RepositoryDatabaseName = My.Settings.SQL__Repository__B_Database__Name_2 _
            And My.Settings.RepositoryUseTrustedConnection _
            And My.Settings.SQL__Repository__B_Use__Trusted__Connection_3 Then
                cCompare.Repository2.ConnectionContext.ConnectionString = Mother.DAL.LocalRepositoryConnectionString
                Return True
            Else
                Dim cn As DialogConnect = New DialogConnect
                ' cn settings are by ref so will be changed if modified in .show overload
                Dim result As DialogResult = cn.ShowDialogForRepository(cCompare.Repository2, _
                              My.Settings.SQL__Repository__B_Name_1, _
                              My.Settings.SQL__Repository__B_Database__Name_2, _
                              My.Settings.SQL__Repository__B_Use__Trusted__Connection_3, _
                              My.Settings.SQL__Repository__B_SQL__Login__Name_4, _
                              My.Settings.SQL__Repository__B_SQL__Login__Password_5, _
                              My.Settings.SQL__Repository__B_Connection__Timeout_7, _
                              My.Settings.SQL__Repository__B_Network__Protocol_9, _
                              My.Settings.SQL__Repository__B_Encrypted__Connection_6, _
                              My.Settings.SQL__Repository__B_Trust__Server__Certificate_8, _
                              Me)
                If result = Windows.Forms.DialogResult.OK Then
                    My.Settings.Save()
                    Me.ToolStripStatusLabel2.Text = String.Format(My.Resources.ConnectedToSQL, _
                                                                  Me.ToolStripStatusLabel2.Tag, _
                                                                  My.Settings.SQL__Repository__B_Name_1)
                    cCompare.SetSmoFieldsToLoad(cCompare.Repository2)
                    Return True
                Else
                    Me.ToolStripStatusLabel2.Text = String.Format(My.Resources.ConnectionToSQLFailed, _
                                                                  Me.ToolStripStatusLabel2.Tag, _
                                                                  My.Settings.SQL__Repository__B_Name_1)
                    Return False
                End If
            End If
        Catch ex As Exception
            Me.ToolStripStatusLabel2.Text = String.Format(My.Resources.ConnectionToSQLFailed, _
                                                          Me.ToolStripStatusLabel2.Tag, _
                                                          My.Settings.SQL__Repository__B_Name_1)
            Mother.HandleException(ex)
            Return False
        End Try
    End Function

#End Region

#Region " Scroll Bar Sync "
    ' http://www.codeproject.com/vb/net/VbNetScrolling.asp
    Private Sub RichTextBox1_HScroll(ByVal sender As Object, ByVal e As System.EventArgs) Handles RichTextBox1.HScroll
        If Control.MouseButtons = Windows.Forms.MouseButtons.Left Then Exit Sub
        If Me.RichTextBox1.ContainsFocus Then
            Dim RTB1Position As Integer
            RTB1Position = GetScrollPos(RichTextBox1.Handle, SBS_HORZ)
            PostMessageA(RichTextBox2.Handle, WM_HSCROLL, SB_THUMBPOSITION + &H10000 * RTB1Position, 0)
        End If
    End Sub

    Private Sub RichTextBox2_HScroll(ByVal sender As Object, ByVal e As System.EventArgs)
        If Control.MouseButtons = Windows.Forms.MouseButtons.Left Then Exit Sub
        If Me.RichTextBox2.ContainsFocus Then
            Dim RTB2Position As Integer
            RTB2Position = GetScrollPos(RichTextBox2.Handle, SBS_HORZ)
            PostMessageA(RichTextBox1.Handle, WM_HSCROLL, SB_THUMBPOSITION + &H10000 * RTB2Position, 0)
        End If
    End Sub

    Private Sub RichTextBox1_VScroll(ByVal sender As Object, ByVal e As System.EventArgs) Handles RichTextBox1.VScroll
        If Me.RichTextBox1.ContainsFocus Then
            Dim RTB1Position As Integer
            RTB1Position = GetScrollPos(RichTextBox1.Handle, SBS_VERT)
            PostMessageA(RichTextBox2.Handle, WM_VSCROLL, SB_THUMBPOSITION + &H10000 * RTB1Position, 0)
        End If
    End Sub

    Private Sub RichTextBox2_VScroll(ByVal sender As Object, ByVal e As System.EventArgs)
        If Me.RichTextBox2.ContainsFocus Then
            Dim RTB2Position As Integer
            RTB2Position = GetScrollPos(RichTextBox2.Handle, SBS_VERT)
            PostMessageA(RichTextBox1.Handle, WM_VSCROLL, SB_THUMBPOSITION + &H10000 * RTB2Position, 0)
        End If
    End Sub

    Public Function GetHighWord(ByRef pintValue As Int32) As Int32
        If (pintValue And &H80000000) = &H80000000 Then
            Return CInt(((pintValue And &H7FFF0000) \ &H10000) Or &H8000&)
        Else
            Return CInt((pintValue And &HFFFF0000) \ &H10000)
        End If
    End Function

    ' for NativeWindow and PostMessageA
    Private Const WM_USER As Integer = &H400
    Private Const WM_COMMAND As Integer = &H111

    Private Const WM_HSCROLL As Integer = &H114
    Private Const WM_VSCROLL As Integer = &H115

    Private Const EN_UPDATE As Integer = &H400

    Private Const EM_GETTHUMB As Integer = &HBE

    Private Const SBS_HORZ As Integer = 0
    Private Const SBS_VERT As Integer = 1

    Private Const SB_THUMBPOSITION As Integer = 4
    ' for SubClassing
    Private WithEvents sClass1 As Subclass
    Private WithEvents sClass2 As Subclass
    Private WithEvents sClassF As Subclass

    Private Declare Function GetScrollPos Lib "user32.dll" ( _
         ByVal hWnd As IntPtr, _
         ByVal nBar As Integer) As Integer

    Private Declare Function SetScrollPos Lib "user32.dll" ( _
        ByVal hWnd As IntPtr, _
        ByVal nBar As Integer, _
        ByVal nPos As Integer, _
        ByVal bRedraw As Boolean) As Integer

    Private Declare Function PostMessageA Lib "user32.dll" ( _
        ByVal hwnd As IntPtr, _
        ByVal wMsg As Integer, _
        ByVal wParam As Integer, _
        ByVal lParam As Integer) As Integer

    Public Sub sClass_WindowProcedure(ByRef uMsg As Message) Handles sClass1.WindowProcedure, sClass2.WindowProcedure, sClassF.WindowProcedure
        Select Case uMsg.Msg
            Case WM_VSCROLL ' WM_VSCROLL Message's for RTB's
                If uMsg.HWnd.Equals(RichTextBox1.Handle) Then
                    RemoveHandler sClass1.WindowProcedure, AddressOf sClass_WindowProcedure
                    sClass1.SendWndProc(Message.Create(RichTextBox2.Handle, uMsg.Msg, uMsg.WParam, uMsg.LParam))
                    AddHandler sClass1.WindowProcedure, AddressOf sClass_WindowProcedure
                End If
                If uMsg.HWnd.Equals(RichTextBox2.Handle) Then
                    RemoveHandler sClass2.WindowProcedure, AddressOf sClass_WindowProcedure
                    sClass2.SendWndProc(Message.Create(RichTextBox1.Handle, uMsg.Msg, uMsg.WParam, uMsg.LParam))
                    AddHandler sClass2.WindowProcedure, AddressOf sClass_WindowProcedure
                End If
            Case WM_HSCROLL ' WM_HSCROLL Message's for RTB's
                If uMsg.HWnd.Equals(RichTextBox1.Handle) Then
                    RemoveHandler sClass1.WindowProcedure, AddressOf sClass_WindowProcedure
                    sClass1.SendWndProc(Message.Create(RichTextBox2.Handle, uMsg.Msg, uMsg.WParam, uMsg.LParam))
                    AddHandler sClass1.WindowProcedure, AddressOf sClass_WindowProcedure
                End If
                If uMsg.HWnd.Equals(RichTextBox2.Handle) Then
                    RemoveHandler sClass2.WindowProcedure, AddressOf sClass_WindowProcedure
                    sClass2.SendWndProc(Message.Create(RichTextBox1.Handle, uMsg.Msg, uMsg.WParam, uMsg.LParam))
                    AddHandler sClass2.WindowProcedure, AddressOf sClass_WindowProcedure
                End If
            Case WM_COMMAND ' Form1 Command messages
                If uMsg.LParam.Equals(RichTextBox1.Handle) Then
                    Dim rtbUpdate As Integer = GetHighWord(uMsg.WParam.ToInt32)
                    'The EN_UPDATE notification message is sent when an
                    'edit control is about to redraw itself
                    'so we will look for the EN_UPDATE event message
                    If rtbUpdate = EN_UPDATE Then '(1024)
                        'The EM_GETTHUMB message retrieves the position of the
                        'scroll box (thumb) in the vertical scroll bar only.
                        '(TODO: Why there is NO horizontal message I can find.)
                        Dim msg1 As Message
                        uMsg.Msg = EM_GETTHUMB
                        msg1 = Message.Create(RichTextBox1.Handle, uMsg.Msg, IntPtr.Zero, IntPtr.Zero)
                        ' send message
                        RemoveHandler sClass1.WindowProcedure, AddressOf sClass_WindowProcedure
                        sClass1.SendWndProc(msg1)
                        AddHandler sClass1.WindowProcedure, AddressOf sClass_WindowProcedure
                        ' make sure we resend the message to rtb1 in case we're on rtb
                        SetScrollPos(RichTextBox1.Handle, SBS_VERT, msg1.Result.ToInt32, True)
                        SetScrollPos(RichTextBox2.Handle, SBS_VERT, msg1.Result.ToInt32, True)
                    End If
                End If
        End Select
    End Sub

#End Region

#Region " Treeview Processing "

    Private Sub smoTreeView1_BeforeSelect(ByVal sender As Object, _
                                          ByVal e As System.Windows.Forms.TreeViewCancelEventArgs) _
    Handles smoTreeView1.BeforeSelect
        If Not smoTreeView1.SelectedNode Is Nothing Then
            cTv.InvertNodeColors(smoTreeView1.SelectedNode)
        End If
    End Sub

    Private Sub smoTreeView2_BeforeSelect(ByVal sender As Object, _
                                          ByVal e As System.Windows.Forms.TreeViewCancelEventArgs) _
    Handles smoTreeView2.BeforeSelect
        If Not smoTreeView2.SelectedNode Is Nothing Then
            cTv.InvertNodeColors(smoTreeView2.SelectedNode)
        End If
    End Sub

    ' all errors - except a failure of the event handlers method call itself? - are handeled in the called module 
    Private Sub smoTreeView1_AfterSelect(ByVal sender As Object, _
                                         ByVal e As System.Windows.Forms.TreeViewEventArgs) _
    Handles smoTreeView1.AfterSelect
        cTv.GetNextTreeLevel(Origin1.Text, _
                             e.Node, _
                             dtRepositoryInstance1)
        ToolStripStatusLabel1.Text = String.Format(My.Resources.NodeSelected, _
                                                   ToolStripStatusLabel1.Tag, _
                                                   e.Node.Name)
        My.Settings.SQL__Repository__A_Filters_Label = ""
        My.Settings.SQL__Repository__A_Filters_Version = 0
    End Sub

    Private Sub smoTreeView1_BeforeExpand(ByVal sender As Object, _
                                          ByVal e As System.Windows.Forms.TreeViewCancelEventArgs) _
    Handles smoTreeView1.BeforeExpand
        cTv.GetNextTreeLevel(Origin1.Text, e.Node, dtRepositoryInstance1)
        For Each n As TreeNode In e.Node.Nodes
            n.ContextMenuStrip = ContextMenuStripTreeItemA
        Next
    End Sub

    Private Sub smoTreeView2_AfterSelect(ByVal sender As Object, _
                                         ByVal e As System.Windows.Forms.TreeViewEventArgs) _
    Handles smoTreeView2.AfterSelect
        cTv.GetNextTreeLevel(Origin2.Text, _
                             e.Node, _
                             dtRepositoryInstance2)
        ToolStripStatusLabel2.Text = String.Format(My.Resources.NodeSelected, _
                                                   ToolStripStatusLabel2.Tag, _
                                                   e.Node.Name)
        My.Settings.SQL__Repository__B_Filters_Label = ""
        My.Settings.SQL__Repository__B_Filters_Version = 0
    End Sub

    Private Sub smoTreeView2_BeforeExpand(ByVal sender As Object, _
                                          ByVal e As System.Windows.Forms.TreeViewCancelEventArgs) _
    Handles smoTreeView2.BeforeExpand
        cTv.GetNextTreeLevel(Origin2.Text, e.Node, dtRepositoryInstance2)
        For Each n As TreeNode In e.Node.Nodes
            n.ContextMenuStrip = ContextMenuStripTreeItemB
        Next
    End Sub

    Private Sub ContextMenuStripTreeItemA_Opening(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) _
    Handles ContextMenuStripTreeItemA.Opening

        ToolStripComboBoxVersionA.Items.Clear()
        ' only put the context menu on the leaf nodes
        ToolStripMenuItemNodeSetVersionA.Visible = If(cCompare.CrackFullPath(smoTreeView1.SelectedNode.FullPath)(3) <> "", True, False)
        If Origin1.Text = My.Resources.OriginRepository Then
            For i As Integer = 1 To CType(smoTreeView1.SelectedNode.Tag, cTreeView.structNodeTag).LatestVersion
                ToolStripComboBoxVersionA.Items.Add(i)
            Next
            ToolStripComboBoxVersionA.SelectedItem = If(My.Settings.SQL__Repository__A_Filters_Version = 0, _
                                                        CType(smoTreeView1.SelectedNode.Tag, cTreeView.structNodeTag).LatestVersion, _
                                                        My.Settings.SQL__Repository__A_Filters_Version)
        End If

    End Sub

    Private Sub ContextMenuStripTreeItemB_Opening(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) _
    Handles ContextMenuStripTreeItemB.Opening

        ToolStripComboBoxVersionB.Items.Clear()
        ' only put the context menu on the leaf nodes
        ToolStripMenuItemNodeSetVersionB.Visible = If(cCompare.CrackFullPath(smoTreeView2.SelectedNode.FullPath)(3) <> "", True, False)
        If Origin2.Text = My.Resources.OriginRepository Then
            For i As Integer = 1 To CType(smoTreeView2.SelectedNode.Tag, cTreeView.structNodeTag).LatestVersion
                ToolStripComboBoxVersionB.Items.Add(i)
            Next
            ToolStripComboBoxVersionB.SelectedItem = If(My.Settings.SQL__Repository__B_Filters_Version = 0, _
                                                        CType(smoTreeView2.SelectedNode.Tag, cTreeView.structNodeTag).LatestVersion, _
                                                        My.Settings.SQL__Repository__B_Filters_Version)
        End If

    End Sub

    Private Sub smoTreeView1_NodeMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeNodeMouseClickEventArgs) Handles smoTreeView1.NodeMouseClick
        smoTreeView1.SelectedNode = e.Node
    End Sub

    Private Sub smoTreeView2_NodeMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeNodeMouseClickEventArgs) Handles smoTreeView2.NodeMouseClick
        smoTreeView2.SelectedNode = e.Node
    End Sub

#End Region

#Region " ContextMenu Processing "

    ' Use My.Computer.Clipboard to insert the selected text or images into the clipboard
    'wont need to Use My.Computer.Clipboard.GetText() or My.Computer.Clipboard.GetData to retrieve information from the clipboard.

    Private Sub SaveAsRichText_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SaveAsRichText.Click
        Dim SaveFileDialog As New SaveFileDialog
        SaveFileDialog.InitialDirectory = My.Computer.FileSystem.SpecialDirectories.MyDocuments
        SaveFileDialog.Filter = My.Resources.CompareSaveTextFilter
        SaveFileDialog.ValidateNames = True
        If (SaveFileDialog.ShowDialog(Me) = System.Windows.Forms.DialogResult.OK) Then
            Dim FileName As String = SaveFileDialog.FileName
            CType(ContextMenuStripCompareOutput.SourceControl, RichTextBox).SaveFile(SaveFileDialog.FileName)
        End If
    End Sub

    Private Sub MenuItemZoomIn_Click(ByVal sender As System.Object, _
                                     ByVal e As System.EventArgs) _
                                     Handles MenuItemZoomIn.Click
        If CType(ContextMenuStripCompareOutput.SourceControl, RichTextBox).ZoomFactor < 3 Then
            If CType(ContextMenuStripCompareOutput.SourceControl, RichTextBox).ZoomFactor > 1 Then
                CType(ContextMenuStripCompareOutput.SourceControl, RichTextBox).ZoomFactor += CType(0.25, Single)
            Else
                CType(ContextMenuStripCompareOutput.SourceControl, RichTextBox).ZoomFactor += CType(0.1, Single)
            End If
        End If
    End Sub

    Private Sub MenuItemZoomNormal_Click(ByVal sender As System.Object, _
                                         ByVal e As System.EventArgs) _
                                         Handles MenuItemZoomNormal.Click
        CType(ContextMenuStripCompareOutput.SourceControl, RichTextBox).ZoomFactor = 1
    End Sub

    Private Sub MenuItemZoomOut_Click(ByVal sender As System.Object, _
                                      ByVal e As System.EventArgs) _
                                      Handles MenuItemZoomOut.Click
        If CType(ContextMenuStripCompareOutput.SourceControl, RichTextBox).ZoomFactor > 0.5 Then
            If CType(ContextMenuStripCompareOutput.SourceControl, RichTextBox).ZoomFactor > 1.5 Then
                CType(ContextMenuStripCompareOutput.SourceControl, RichTextBox).ZoomFactor -= CType(0.25, Single)
            Else
                CType(ContextMenuStripCompareOutput.SourceControl, RichTextBox).ZoomFactor -= CType(0.1, Single)
            End If
        End If
    End Sub

#End Region

#Region " form actions "

    Private Sub SplitContainer1_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles SplitContainer1.GotFocus
        SplitContainer1.BackColor = SystemColors.GradientActiveCaption
    End Sub

    Private Sub SplitContainer1_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles SplitContainer1.LostFocus
        SplitContainer1.BackColor = SystemColors.GradientInactiveCaption
    End Sub

    Private Sub SplitContainer1_MouseEnter(ByVal sender As Object, ByVal e As System.EventArgs) Handles SplitContainer1.MouseEnter
        SplitContainer1.BackColor = SystemColors.GradientActiveCaption
    End Sub

    Private Sub SplitContainer1_MouseLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles SplitContainer1.MouseLeave
        SplitContainer1.BackColor = SystemColors.GradientInactiveCaption
    End Sub

    Private Sub SplitContainer2_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles SplitContainer2.GotFocus
        SplitContainer2.BackColor = SystemColors.GradientActiveCaption
    End Sub

    Private Sub SplitContainer2_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles SplitContainer2.LostFocus
        SplitContainer2.BackColor = SystemColors.GradientInactiveCaption
    End Sub

    Private Sub SplitContainer2_MouseEnter(ByVal sender As Object, ByVal e As System.EventArgs) Handles SplitContainer2.MouseEnter
        SplitContainer2.BackColor = SystemColors.GradientActiveCaption
    End Sub

    Private Sub SplitContainer2_MouseLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles SplitContainer2.MouseLeave
        SplitContainer2.BackColor = SystemColors.GradientInactiveCaption
    End Sub

    Private Sub SplitContainer3_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles SplitContainer3.GotFocus
        SplitContainer3.BackColor = SystemColors.GradientActiveCaption
    End Sub

    Private Sub SplitContainer3_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles SplitContainer3.LostFocus
        SplitContainer3.BackColor = SystemColors.GradientInactiveCaption
    End Sub

    Private Sub SplitContainer3_MouseEnter(ByVal sender As Object, ByVal e As System.EventArgs) Handles SplitContainer3.MouseEnter
        SplitContainer3.BackColor = SystemColors.GradientActiveCaption
    End Sub

    Private Sub SplitContainer3_MouseLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles SplitContainer3.MouseLeave
        SplitContainer3.BackColor = SystemColors.GradientInactiveCaption
    End Sub

    Private Sub ButtonMaxInput_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonMaxInput.Click
        SplitContainer2.Panel1Collapsed = False
        SplitContainer3.Panel2Collapsed = False
        If SplitContainer2.Panel2Collapsed = True Then
            SplitContainer2.Panel2Collapsed = False
            SplitContainer3.Panel1Collapsed = False
        Else
            SplitContainer2.Panel2Collapsed = True
            SplitContainer3.Panel1Collapsed = True
        End If
    End Sub

    Private Sub ButtonMaxOutput_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonMaxOutput.Click
        SplitContainer2.Panel2Collapsed = False
        SplitContainer3.Panel1Collapsed = False
        If SplitContainer2.Panel1Collapsed = True Then
            SplitContainer2.Panel1Collapsed = False
            SplitContainer3.Panel2Collapsed = False
        Else
            SplitContainer2.Panel1Collapsed = True
            SplitContainer3.Panel2Collapsed = True
        End If
    End Sub

    Private Sub RepositionComponents()
        ToolStripStatusLabelCompare.Width = If(SplitContainer3.SplitterDistance < 100, 100, SplitContainer3.SplitterDistance)
        ToolStripStatusLabel1.Width = If(SplitContainer1.SplitterDistance - SplitContainer3.SplitterDistance < 100, _
                                         100, _
                                         SplitContainer1.SplitterDistance - SplitContainer3.SplitterDistance - SplitContainer1.SplitterWidth)
        ToolStripStatusLabel2.Width = If(SplitContainer2.Width - SplitContainer2.SplitterDistance < 100, _
                                         SplitContainer2.Width - 100, _
                                         SplitContainer2.SplitterDistance)
    End Sub

    Private Sub CompareForm_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize
        RepositionComponents()
    End Sub

    Private Sub SplitContainer1_SplitterMoved(ByVal sender As Object, ByVal e As System.Windows.Forms.SplitterEventArgs) Handles SplitContainer1.SplitterMoved
        RepositionComponents()
    End Sub

    Private Sub SplitContainer2_SplitterMoved(ByVal sender As Object, ByVal e As System.Windows.Forms.SplitterEventArgs) Handles SplitContainer2.SplitterMoved
        RepositionComponents()
    End Sub

    Private Sub SplitContainer3_SplitterMoved(ByVal sender As Object, ByVal e As System.Windows.Forms.SplitterEventArgs) Handles SplitContainer3.SplitterMoved
        RepositionComponents()
    End Sub

#End Region

End Class

Public Class Subclass

    ' http://www.codeproject.com/vb/net/VbNetScrolling.asp

    ' NativeWindow Subclassing
    Inherits System.Windows.Forms.NativeWindow
    Public Event WindowProcedure(ByRef uMsg As Message)

    Public Sub New(ByVal pWindowHandle As IntPtr)
        MyBase.AssignHandle(pWindowHandle)
    End Sub

    Protected Overrides Sub WndProc(ByRef uMsg As System.Windows.Forms.Message)
        MyBase.WndProc(uMsg)
        RaiseEvent WindowProcedure(uMsg)
    End Sub

    Public Sub SendWndProc(ByRef uMsg As System.Windows.Forms.Message)
        MyBase.WndProc(uMsg)
    End Sub

End Class


