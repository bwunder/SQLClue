Imports System.Windows.Forms

Public Class DialogSetup

    Delegate Sub RunAsyncDelegate(ByVal TargetMaster As Server)

    Public Event SetupException(ByVal ex As Exception)
    Public Event SetupAction(ByVal action As String)

    Public Enum DataStoreSetupAction As Integer
        None = 0
        Enable = 1
        Modify = 2
        Disable = 3
        EnableNotifications = 4
        DisableNotifications = 5
        EnablePasswordEncryption = 6
        DisablePasswordEncryption = 7
        EnableRemoteRunbook = 8
    End Enum

    Private _Install As Boolean
    Private _DataStore As String
    Private _Action As DataStoreSetupAction
    Private _Script As String

    Public ReadOnly Property sTargetConnectionString() As String
        Get
            Dim builder As New System.Data.SqlClient.SqlConnectionStringBuilder
            builder.Clear()
            builder.DataSource = TargetInstanceName.Text
            builder.InitialCatalog = TargetDatabaseName.Text
            If TargetUseTrustedConnection.Checked Then
                builder.IntegratedSecurity = True
            Else
                builder.UserID = TargetSQLLoginName.Text
                builder.Password = TargetSQLLoginPassword.Text
            End If
            builder.ConnectTimeout = CInt(TargetConnectionTimeout.Value)
            builder.ApplicationName = My.Application.Info.ProductName & " : " & Me.Text
            'allow for a 'not specified' net
            If TargetNetworkProtocol.Text <> "" Then
                builder.NetworkLibrary = CStr(If(InStr(TargetNetworkProtocol.Text, " ") > 0, _
                                                  TargetNetworkProtocol.Text.Split(Chr(32))(0), _
                                                  TargetNetworkProtocol.Text))
            End If
            builder.Encrypt = TargetEncryptConnection.Checked
            builder.TrustServerCertificate = TargetTrustServerCertificate.Checked
            builder.Enlist = True
            builder.MultipleActiveResultSets = False
            builder.PersistSecurityInfo = False
            Return builder.ConnectionString
        End Get
    End Property

    Public ReadOnly Property sDbDropDownConnectionString() As String
        Get
            Dim builder As New System.Data.SqlClient.SqlConnectionStringBuilder
            builder.Clear()
            builder.DataSource = TargetInstanceName.Text
            'builder.InitialCatalog = TargetDatabaseName.Text
            If TargetUseTrustedConnection.Checked Then
                builder.IntegratedSecurity = True
            Else
                builder.UserID = TargetSQLLoginName.Text
                builder.Password = TargetSQLLoginPassword.Text
            End If
            builder.ConnectTimeout = CInt(TargetConnectionTimeout.Value)
            builder.ApplicationName = My.Application.Info.ProductName & " : " & Me.Text
            'allow for a 'not specified' net
            If TargetNetworkProtocol.Text <> "" Then
                builder.NetworkLibrary = CStr(If(InStr(TargetNetworkProtocol.Text, " ") > 0, _
                                                  TargetNetworkProtocol.Text.Split(Chr(32))(0), _
                                                  TargetNetworkProtocol.Text))
            End If
            builder.Encrypt = TargetEncryptConnection.Checked
            builder.TrustServerCertificate = TargetTrustServerCertificate.Checked
            builder.Enlist = True
            builder.MultipleActiveResultSets = False
            builder.PersistSecurityInfo = False
            Return builder.ConnectionString
        End Get
    End Property

    Public Overloads Function ShowDialog(ByVal Datastore As String, _
                                         ByVal Action As DataStoreSetupAction, _
                                         ByRef SQLInstance As String, _
                                         ByRef DatabaseName As String, _
                                         ByRef UseTrustedConnection As Boolean, _
                                         ByRef SQLLogin As String, _
                                         ByRef Password As String, _
                                         ByRef ConnectionTimeout As Int32, _
                                         ByRef NetProtocol As String, _
                                         ByRef EncryptCn As Boolean, _
                                         ByRef TrustServer As Boolean, _
                                         ByVal ParentForm As IWin32Window) As System.Windows.Forms.DialogResult
        Try
            _Action = Action
            _DataStore = Datastore
            ToolStripStatusLabelSetup.Text = String.Format("Setup Action: {0}; Data Store '{1}' ", _Action, _DataStore)
            TargetInstanceName.Text = SQLInstance
            TargetInstanceName.Tag = SQLInstance
            TargetDatabaseName.Text = DatabaseName
            TargetDatabaseName.Tag = DatabaseName
            TargetUseTrustedConnection.Checked = UseTrustedConnection
            TargetUseTrustedConnection.Tag = UseTrustedConnection
            TargetSQLLoginName.Text = SQLLogin
            TargetSQLLoginName.Tag = SQLLogin
            TargetSQLLoginPassword.Text = Password
            TargetSQLLoginPassword.Tag = Password
            TargetConnectionTimeout.Value = ConnectionTimeout
            TargetConnectionTimeout.Tag = ConnectionTimeout
            TargetNetworkProtocol.Text = NetProtocol
            TargetNetworkProtocol.Tag = NetProtocol
            TargetEncryptConnection.Checked = EncryptCn
            TargetEncryptConnection.Tag = EncryptCn
            TargetTrustServerCertificate.Checked = TrustServer
            TargetTrustServerCertificate.Tag = TrustServer
            ' get the script based on the datastore and action
            Select Case _DataStore
                Case ConfigurationForm.Text
                    Me.Text = "SQLClue: SQL Configuration Archive Setup"
                    Select Case _Action
                        Case DataStoreSetupAction.Enable
                            ' if this is an upgrade only run the alter
                            ' will figure out which to run in the OK Button handler
                            _Script = String.Format("{0}\{1}", My.Application.Info.DirectoryPath, My.Settings.instRepository)
                            ToolStripStatusLabelSetup.Text = "Install Archive Component"
                            TargetDatabaseName.Enabled = True
                        Case DataStoreSetupAction.Modify
                            '_Script = String.Format("{0}\{1}", My.Application.Info.DirectoryPath, My.Settings.alterRepository)
                            TargetDatabaseName.Enabled = False
                            ToolStripStatusLabelSetup.Text = "Change Archive Host Db Connection"
                        Case DataStoreSetupAction.Disable
                            _Script = String.Format("{0}\{1}", My.Application.Info.DirectoryPath, My.Settings.uninstRepository)
                            TargetDatabaseName.Enabled = False
                            ToolStripStatusLabelSetup.Text = "Uninstall SQL Configuration Archive Component"
                        Case DataStoreSetupAction.EnableNotifications
                            _Script = String.Format("{0}\{1}", My.Application.Info.DirectoryPath, My.Settings.instDDLEventNotifications)
                            TargetDatabaseName.Enabled = False
                            ToolStripStatusLabelSetup.Text = "Enable Archive Change Event Notifications"
                        Case DataStoreSetupAction.DisableNotifications
                            _Script = String.Format("{0}\{1}", My.Application.Info.DirectoryPath, My.Settings.uninstDDLEventNotifications)
                            TargetDatabaseName.Enabled = False
                            ToolStripStatusLabelSetup.Text = "Remove Archive Change Event Notifications"
                    End Select
                Case RunbookForm.Text
                    Me.Text = "SQLClue: Runbook Setup"
                    Select Case _Action
                        Case DataStoreSetupAction.Enable
                            TargetInstanceName.Text = My.Settings.RepositoryInstanceName
                            _Script = String.Format("{0}\{1}", My.Application.Info.DirectoryPath, My.Settings.instRunbook)
                            TargetInstanceName.Enabled = False
                            TargetDatabaseName.Enabled = True
                            ToolStripStatusLabelSetup.Text = "Install Runbook Component"
                        Case DataStoreSetupAction.Modify
                            '_Script = String.Format("{0}\{1}", My.Application.Info.DirectoryPath, My.Settings.alterRunbook)
                            TargetInstanceName.Enabled = False
                            TargetDatabaseName.Enabled = False
                            ToolStripStatusLabelSetup.Text = "Change Runbook Host Db Connection"
                        Case DataStoreSetupAction.Disable
                            _Script = String.Format("{0}\{1}", My.Application.Info.DirectoryPath, My.Settings.uninstRunbook)
                            TargetInstanceName.Enabled = False
                            TargetDatabaseName.Enabled = False
                            ToolStripStatusLabelSetup.Text = "Uninstall Runbook Component"
                        Case DataStoreSetupAction.EnableRemoteRunbook
                            _Script = ""
                            TargetInstanceName.Enabled = True
                            TargetDatabaseName.Enabled = True
                            ToolStripStatusLabelSetup.Text = "Attach to Remote SQLClue Runbook"
                    End Select
            End Select
            Dim result As DialogResult = Me.ShowDialog(ParentForm)
            If result = Windows.Forms.DialogResult.OK Then
                SQLInstance = TargetInstanceName.Text
                DatabaseName = TargetDatabaseName.Text
                UseTrustedConnection = TargetUseTrustedConnection.Checked
                SQLLogin = TargetSQLLoginName.Text
                Password = TargetSQLLoginPassword.Text
                ConnectionTimeout = CInt(TargetConnectionTimeout.Value)
                NetProtocol = TargetNetworkProtocol.Text
                EncryptCn = TargetEncryptConnection.Checked
                TrustServer = TargetTrustServerCertificate.Checked
            Else
                SQLInstance = TargetInstanceName.Tag.ToString
                DatabaseName = TargetDatabaseName.Tag.ToString
                UseTrustedConnection = CBool(TargetUseTrustedConnection.Tag)
                SQLLogin = TargetSQLLoginName.Tag.ToString
                Password = TargetSQLLoginPassword.Tag.ToString
                ConnectionTimeout = CInt(TargetConnectionTimeout.Tag)
                NetProtocol = TargetNetworkProtocol.Tag.ToString
                EncryptCn = CBool(TargetEncryptConnection.Tag)
                TrustServer = CBool(TargetTrustServerCertificate.Tag)
            End If
            Return result
        Catch ex As Exception
            Throw New Exception(String.Format("({0}.SetupForm_ShowDialog Component Overload) Exception.", Me.Text), ex)
        End Try
    End Function

    Private Sub SetupForm_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            TargetDatabaseName.Items.Clear()
            ' Database is specified only when an install script is also provided
            If TargetDatabaseName.Text <> "" Then
                Select Case _DataStore
                    Case My.Forms.ConfigurationForm.Text
                        ' no changing this one
                        TargetNetworkProtocol.Enabled = False
                    Case My.Forms.RunbookForm.Text
                        If _Action = DataStoreSetupAction.Enable _
                        Or _Action = DataStoreSetupAction.Modify _
                        Or _Action = DataStoreSetupAction.Disable Then
                            TargetInstanceName.Text = My.Settings.RepositoryInstanceName
                        End If
                        If _Action = DataStoreSetupAction.EnableRemoteRunbook Then
                            TargetInstanceName.Enabled = True
                            For Each i As String In Mother.InstanceList
                                If Not TargetInstanceName.Items.Contains(i) Then
                                    TargetInstanceName.Items.Add(i)
                                End If
                            Next
                            TargetInstanceName.Sorted = True
                            TargetDatabaseName.Enabled = True
                        End If
                End Select
                'data stores must use trusted connection
                TargetUseTrustedConnection.Enabled = False
                TargetSQLLoginName.Enabled = False
                TargetSQLLoginPassword.Enabled = False
                ' data stores always use shared memory
                TargetNetworkProtocol.Text = My.Settings.NetworkLibraries.Item(My.Settings.NetworkLibraries.IndexOf("dbmslpcn (Shared Memory)"))
                'if already configured, can't change the instance name
                'but need a way to allow connection settings changes
                If _Script = "" Then
                    _Install = False
                Else
                    _Install = True
                End If
            Else
                'if db is blank then this is a plain vanilla target server connection
                TargetUseTrustedConnection.Enabled = True
                TargetDatabaseName.Enabled = True
                _Install = False
            End If
            ' configure if  not configured
            ' only repository should drop through
            If TargetInstanceName.Text = "" Then
                TargetInstanceName.Items.Clear()
                Dim dtLocalInstance As New DataTable
                dtLocalInstance = SmoApplication.EnumAvailableSqlServers(True)
                If dtLocalInstance.Rows.Count > 0 Then
                    For i As Int32 = 0 To dtLocalInstance.Rows.Count - 1
                        TargetInstanceName.Items.Add(UCase(dtLocalInstance.Rows(i).Item("Name").ToString))
                    Next
                End If
                If My.Settings.RepositoryEnabled _
                AndAlso Not TargetInstanceName.Items.Contains(My.Settings.RepositoryInstanceName) Then
                    TargetInstanceName.Items.Add(My.Settings.RepositoryInstanceName)
                End If
                TargetInstanceName.Sorted = True
                If TargetInstanceName.Text = "" _
                And (TargetInstanceName.Items.Count = 0 _
                     OrElse TargetInstanceName.Items.Count = 1 And TargetInstanceName.Items(0).ToString = "") Then
                    Dim result As DialogResult = _
                    MessageBox.Show(String.Format(My.Resources.SetupNoServersMessage, _
                                                  Me.Text), My.Resources.SetupNoServersCaption, _
                                    MessageBoxButtons.OKCancel, _
                                    MessageBoxIcon.Exclamation, _
                                    MessageBoxDefaultButton.Button1, _
                                    0, _
                                    False)
                    If result <> Windows.Forms.DialogResult.OK Then
                        Me.Close()
                    End If
                End If
            Else
                ' the tag should already have the existing value
                ' so can put the settings back to there on error
                If TargetUseTrustedConnection.Checked Then
                    TargetSQLLoginName.Text = ""
                    TargetSQLLoginName.Enabled = False
                    TargetSQLLoginPassword.Text = ""
                    TargetSQLLoginPassword.Enabled = False
                Else
                    TargetSQLLoginName.Enabled = True
                    TargetSQLLoginPassword.Text = ""
                    TargetSQLLoginPassword.Enabled = True
                End If
            End If
            For Each protocol As String In My.Settings.NetworkLibraries
                If Not TargetNetworkProtocol.Items.Contains(protocol) Then
                    TargetNetworkProtocol.Items.Add(protocol)
                End If
            Next protocol
        Catch ex As Exception
            Throw New Exception(String.Format("({0}.SetupForm_Load) Exception.", Me.Text), ex)
        End Try
    End Sub

    Private Sub OKButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonOK.Click
        Dim csr As Cursor = Me.Cursor
        Dim TargetMaster As Server = Nothing
        If Not TargetInstanceName.Text = "" Then
            Try
                Me.Cursor = Cursors.WaitCursor   ' Display the waiting cursor
                ' everything that gets inserted should be upper case  
                TargetInstanceName.Text = UCase(TargetInstanceName.Text)
                ' create db and run script if new
                If _Install Then
                    ' use master to run the create script
                    TargetDatabaseName.Tag = TargetDatabaseName.Text
                    TargetDatabaseName.Text = "master"
                    Dim srvcon As New ServerConnection
                    srvcon.ConnectionString = sTargetConnectionString
                    TargetMaster = New Server(srvcon)
                    TargetDatabaseName.Text = TargetDatabaseName.Tag.ToString
                    ' find out if there is a database master key in master
                    ' need to know this for CLR credentials, SB, and to create strong named assembly
                    If TargetMaster.Databases("master").MasterKey Is Nothing _
                    OrElse Not TargetMaster.Databases("master").MasterKey.State = SqlSmoState.Existing Then
                        Dim r As DialogResult = MessageBox.Show("Create required 'master' database Master Key Now?" & vbCrLf & _
                                                                "Logged in user must have CONTROL right to the database.", _
                                                                "Required Encryption Hierarchy Not Found", MessageBoxButtons.YesNo)
                        If r = DialogResult.Yes Then
                            ' just in case it was manually created while prev dialog open
                            If TargetMaster.Databases("master").MasterKey Is Nothing _
                            OrElse Not TargetMaster.Databases("master").MasterKey.State = SqlSmoState.Existing Then
                                Dim pwd As String = InputBox("Enter a password for use in creating the master database master key")
                                Dim mkey As New MasterKey(TargetMaster.Databases("Master"))
                                mkey.Create(pwd)
                                MessageBox.Show("Database Master Key created in database master. This key should be backed up" & vbCrLf & _
                                                "now and the password used securely stored. See Books Online.")
                            End If
                        Else
                            ' just in case it was manually created while prev dialog open
                            If TargetMaster.Databases("master").MasterKey Is Nothing _
                            OrElse Not TargetMaster.Databases("master").MasterKey.State = SqlSmoState.Existing Then
                                MessageBox.Show("The Database Master Key must exist for the 'master' database to continue.", _
                                                "Verify Database Master Key", MessageBoxButtons.OK)
                                Exit Try
                            End If
                        End If
                    End If
                    'should be async here maybe?
                    'but thisngs get out of whack if async and fails due to thread concurrency issues
                    'ToolStripStatusLabelSetup.Text = String.Format("Creating db [{0}]", TargetDatabaseName.Text)
                    'backgroundWorker1 = New BackgroundWorker
                    'backgroundWorker1.RunWorkerAsync(New Object() {TargetMaster, TargetDatabaseName.Text, _Script})
                    RunScript(TargetMaster)
                End If
                Me.DialogResult = System.Windows.Forms.DialogResult.OK
                Me.Close()
            Catch ex As Exception
                Dim ExTop As New ApplicationException(Me.Text & " " & ex.GetType.ToString & " ", ex)
                ExTop.Source = Me.Text
                Dim emb As New ExceptionMessageBox(ExTop)
                emb.Beep = True
                emb.ShowToolBar = True
                emb.Show(Me)
            Finally
                If Not (TargetMaster Is Nothing) Then
                    TargetMaster.ConnectionContext.Disconnect()
                    TargetMaster = Nothing
                    Me.Cursor = csr
                End If
            End Try
        End If
    End Sub

    Private Sub RunScript(ByVal TargetMaster As Server)

        Dim Db As Database
        If Not TargetMaster.Databases.Contains(TargetDatabaseName.Text) Then
            Dim s As String = InputBox("Enter numeric value for database data file size (MB)." & vbCrLf & _
                                       "Database will be created at server default location.", "Enter Database Size", "100")
            If IsNumeric(s) Then
                Db = New Database(TargetMaster, TargetDatabaseName.Text)
                Db.DatabaseOptions.AutoClose = False
                Db.DatabaseOptions.RecoveryModel = RecoveryModel.Simple
                Db.DatabaseOptions.BrokerEnabled = True
                Db.Create()
                ' this works in lieu of above but adds one more change action to create 
                Db.FileGroups(0).Files(0).Size = CInt(s)
                ' what about the db master key? - not here, only if required 
            Else
                'could be invalid value or could be the inputbox was cancelled
                Throw New Exception("Data file size missing or not provided. Must be a numeric value.")
                Exit Sub
            End If
        Else
            Db = TargetMaster.Databases(TargetDatabaseName.Text)
            ' if the previous version was sucessfully installed just run the update script
            Dim CurrentVersion As String
            If Db.Tables.Contains("tSQLCfg", "SQLCfg") _
            AndAlso Db.Tables("tSQLCfg", "SQLCfg").Columns.Contains("CurrentVersion") Then
                Dim ds As DataSet = Db.ExecuteWithResults("SELECT [CurrentVersion] FROM [SQLCfg].[tSQLCfg]")
                CurrentVersion = ds.Tables(0).Rows(0).Item(0).ToString
            Else
                CurrentVersion = "0.0.0.0"
            End If
            ' use the update script
            If CurrentVersion <> "0.0.0.0" Then
                _Script = String.Format("{0}\{1}", _
                                        My.Application.Info.DirectoryPath, _
                                        Replace(My.Settings.alterRepository, _
                                                ".sql", _
                                                "_" & Replace(My.Application.Info.Version.ToString, ".", "_") & ".sql"))
            End If
        End If
        If Db.AutoClose Then
            Db.DatabaseOptions.AutoClose = False
        End If
        If Not Db.DatabaseOptions.RecoveryModel = RecoveryModel.Simple Then
            Db.DatabaseOptions.RecoveryModel = RecoveryModel.Simple
        End If
        If Not Db.DatabaseOptions.BrokerEnabled Then
            Db.DatabaseOptions.BrokerEnabled = True
        End If
        ' done with master, will clean up in the finally 
        ' the DAL method will change connection context to newly created database
        Try
            ToolStripStatusLabelSetup.Text = String.Format("Running {0}", _Script)
            If (Mother.DAL.RunSQLScript(TargetMaster, _
                                        TargetDatabaseName.Text, _
                                        _Script)) = True Then
                ' just want to make sure the correct db name is saved to the settings
                TargetDatabaseName.Text = TargetDatabaseName.Tag.ToString
            Else
                ' doubt it will ever be able to get here
                Throw New Exception(String.Format("(SetupForm.OKButton_Click) Failed. " & _
                                              "Script [{0}] execution exception" & _
                                              "on SQL Instance [{1}].", _
                                               _Script, TargetInstanceName.Text))
            End If
        Catch sex As SqlException
            If Mid(sex.GetBaseException.Message.ToString, 1) = "The CLR has been enabled on SQL instance" Then
                'cut-n-paste from BOL
                'mod to get the name of the service
                'Declare and create an instance of the ManagedComputer object that represents the WMI Provider services.
                Dim mc As ManagedComputer
                mc = New ManagedComputer()
                'Iterate through each service registered with the WMI Provider lookin for the SQL Instance process
                Dim svc As Wmi.Service = Nothing
                For Each svc In mc.ServerInstances
                    If svc.ProcessId = TargetMaster.Properties("ProcessId").Value Then
                        'Reference the Microsoft SQL Server service.
                        'svc = mc.Services("MSSQLSERVER")
                        'Stop the service if it is running and report on the status continuously until it has stopped.
                        If svc.ServiceState = ServiceState.Running Then
                            svc.Stop()
                            ToolStripStatusLabelSetup.Text = String.Format("{0} service {1}", svc.Name, svc.ServiceState)
                            Do Until String.Format("{0}", svc.ServiceState) = "Stopped"
                                ToolStripStatusLabelSetup.Text = String.Format("{0}", svc.ServiceState)
                                svc.Refresh()
                            Loop
                            ToolStripStatusLabelSetup.Text = String.Format("{0} service {1}", svc.Name, svc.ServiceState)
                            'Start the service and report on the status continuously until it has started.
                            svc.Start()
                            Do Until String.Format("{0}", svc.ServiceState) = "Running"
                                ToolStripStatusLabelSetup.Text = String.Format("{0}", svc.ServiceState)
                                svc.Refresh()
                            Loop
                            ToolStripStatusLabelSetup.Text = String.Format("{0} service {1}", svc.Name, svc.ServiceState)
                        End If
                    End If
                Next
            Else
                Throw New Exception(String.Format("(SetupForm.AsynchSetup) exception (script [{0}])", sex))
            End If
        End Try
    End Sub

    Private Sub ButtonCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonCancel.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub UseEncryptedConnection_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) _
    Handles UseEncryptedConnection.CheckedChanged
        ' can only TrustServerCertificate if EncryptConnection 
        If Not (UseEncryptedConnection.Checked) Then
            TrustServerCertificate.Checked = False
        End If
    End Sub

    Private Sub TargetUseTrustedConnection_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) _
    Handles TargetUseTrustedConnection.CheckedChanged
        ' the connectForm uses the same check changed event logic
        TargetSQLLoginName.Text = ""
        TargetSQLLoginPassword.Text = ""
        If TargetUseTrustedConnection.Checked Then
            TargetSQLLoginName.Enabled = False
            TargetSQLLoginPassword.Enabled = False
            TargetSQLLoginName.TabStop = False
            TargetSQLLoginPassword.TabStop = False
        Else
            If Not TargetUseTrustedConnection.Tag Is Nothing Then
                VerifyEncryptionHierarchy()
            End If
            TargetSQLLoginName.Enabled = True
            TargetSQLLoginPassword.Enabled = True
            TargetSQLLoginName.TabStop = True
            TargetSQLLoginPassword.TabStop = True
        End If
    End Sub

    Private Function VerifyEncryptionHierarchy() As Boolean
        ' the ConnectForm has a copy of this method, must stay the same 
        VerifyEncryptionHierarchy = False
        Dim srvcon As New ServerConnection
        Dim oRepository As Smo.Server
        Try
            srvcon.ConnectionString = Mother.DAL.LocalRepositoryConnectionString
            oRepository = New Server(srvcon)
            Dim oDb As Smo.Database = oRepository.Databases(My.Settings.RepositoryDatabaseName)
            ' check for the symmetric key 
            If Not (oDb.SymmetricKeys.Contains(My.Settings.RepositoryPasswordEncryptionKey)) Then
                DialogSetEncryptionHierarchy.Tag = oDb
                Dim r As DialogResult = DialogSetEncryptionHierarchy.ShowDialog(Me)
            Else
                MessageBox.Show(My.Resources.SQLLoginNotRecommendedText, _
                                My.Resources.SQLLoginNotRecommendedCaption, _
                                MessageBoxButtons.OK)
            End If
            VerifyEncryptionHierarchy = True
        Catch exSQL As SqlClient.SqlException
            Throw exSQL
        Finally
            If Not (srvcon Is Nothing) Then
                srvcon.Disconnect()
                oRepository = Nothing
            End If
        End Try
    End Function

    Private Sub TargetDatabaseName_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles TargetDatabaseName.DropDown
        Try
            TargetDatabaseName.Items.Clear()
            If TargetInstanceName.Text <> "" Then
                ' want to move this to the dropdown event but unsure
                ' also seems like it should use remote reporsitory conenction not LOCAL!
                ' already have repository connection so list the databases
                ' yes but if someone resets settings (no user.config so no Mother.DAL.LocalRepositoryConnectionString) 
                ' this will cause "value cannot be null" Mother.DAL.LocalRepositoryConnectionString so
                Dim smoSrv As New Server
                smoSrv.ConnectionContext.ConnectionString = sDbDropDownConnectionString
                smoSrv.ConnectionContext.Connect()
                For Each db As Database In smoSrv.Databases
                    If Not db.IsSystemObject And db.IsAccessible Then
                        TargetDatabaseName.Items.Add(db.Name)
                    End If
                Next
                smoSrv.ConnectionContext.Disconnect()
                smoSrv = Nothing
                If TargetDatabaseName.Enabled = True Then
                    If Not TargetDatabaseName.Items.Contains(My.Settings.RepositoryDatabaseName) Then
                        TargetDatabaseName.Items.Add(My.Settings.RepositoryDatabaseName)
                    End If
                    If Not TargetDatabaseName.Items.Contains(My.Settings.RunbookDatabaseName) Then
                        TargetDatabaseName.Items.Add(My.Settings.RunbookDatabaseName)
                    End If
                End If
            End If
            TargetDatabaseName.Sorted = True
        Catch ex As Exception
            Throw New Exception(String.Format("({0}.TargetDatabaseName_DropDown) Exception.", Me.Name), ex)
        End Try
    End Sub



End Class