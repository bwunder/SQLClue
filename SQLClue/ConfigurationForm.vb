Imports System.Windows.Forms
Imports System.Drawing

Public Class ConfigurationForm

    Private InstanceName As String
    Private cTv As New cTreeView
    Private dtRepositoryInstance As DataTable
    ' the weakest case of all for a SQL Login is to the repository 
    ' but undoubtedly someone will decide it is the right way to go
    Friend RepositoryPassword As Security.SecureString
    Friend TargetInstance As Smo.Server
    Friend ScheduleHasBeenChanged As Boolean
    Private TreeHasBeenChanged As Boolean
    Private IsConnectedToTarget As Boolean
    Friend IntervalBaseDt As Date
    ' use mother's DAL 
    Private cArchive1 As cCommon.cArchive
    Private ArchiveIsRunning As Boolean
    Private CurrentCollection As String
    Delegate Sub ProgressEventHandlerDelegate(ByVal Item As String)
    Delegate Sub ProgressDetailEventHandlerDelegate(ByVal Item As String)
    Delegate Sub ArchiveDoWorkExceptionHandlerDelegate(ByVal ArchiveException As Exception)
    Private irow As cCommon.dsSQLConfiguration.tInstanceRow

    Private Sub ConfigurationForm_Load(ByVal sender As System.Object, _
                                       ByVal e As System.EventArgs) _
                                       Handles MyBase.Load
        Try
            ' set the repository connection string values first!
            Mother.DAL.LoadConfig()
            DataGridViewConnections.DataSource = Mother.DAL.dsSQLCfg
            DataGridViewConnections.DataMember = "tConnection"
            DataGridViewConnections.AutoSize = True
            DataGridViewConnections.AllowUserToResizeColumns = True
            DataGridViewConnections.AllowUserToResizeRows = True
            ToolStripStatusLabelConfiguration.Text = My.Resources.StartPlanning
            IsConnectedToTarget = False
            ArchiveCancelOff()
            Me.Show()
        Catch ex As Exception
            Mother.HandleException(New ApplicationException(String.Format("({0}.Load) Exception.", Me.Name), ex))
        End Try
    End Sub

    Private Sub DataGridViewConnections_CellClick(ByVal sender As Object, _
                                                  ByVal e As DataGridViewCellEventArgs) _
                                                  Handles DataGridViewConnections.CellClick
        ' for every grid cell use this: process changes and then set cell value
        Dim csr As Cursor = Me.Cursor
        Try
            Me.Cursor = Cursors.WaitCursor   ' Display the waiting cursor
            Me.ToolStripStatusLabelConfiguration.Text = ""
            ' do nothing if column header is clicked sorting handled by form autogen code
            Select Case e.RowIndex
                Case -1
                    If e.ColumnIndex = -1 Then
                        ' cutnpaste from form load
                        Mother.DAL.LoadConfig()
                        DataGridViewConnections.DataSource = Mother.DAL.dsSQLCfg
                        DataGridViewConnections.DataMember = "tConnection"
                        DataGridViewConnections.AutoSize = True
                        DataGridViewConnections.AllowUserToResizeColumns = True
                        DataGridViewConnections.AllowUserToResizeRows = True
                        ArchiveCancelOff()
                    End If
                Case Else
                    ' using CurrentRow doesn't work?
                    ' assert: if it's not a new row InstanceName can never be nothing
                    If Not DataGridViewConnections.Rows(e.RowIndex).IsNewRow Then
                        ' connection info is hidden on the form
                        If Not TryCast(DataGridViewConnections.Rows(e.RowIndex).Cells("DataGridViewButtonColumnConnectionInstanceName").Value, String) Is Nothing _
                        AndAlso InstanceName <> TryCast(DataGridViewConnections.Rows(e.RowIndex).Cells("DataGridViewButtonColumnConnectionInstanceName").Value, String) Then
                            InstanceName = TryCast(DataGridViewConnections.Rows(e.RowIndex).Cells("DataGridViewButtonColumnConnectionInstanceName").Value, String)
                            ' schedule is databound, treeview is not
                            smoTreeView.Nodes.Clear()
                        End If
                    Else
                        If InstanceName <> TryCast(DataGridViewConnections.Rows(e.RowIndex).Cells("DataGridViewButtonColumnConnectionInstanceName").Value, String) Then
                            InstanceName = ""
                            smoTreeView.Nodes.Clear()
                        End If
                    End If
                    LabelArchiveSchedule.Text = "Schedules"
                    LabelConfigfuration.Text = "Items to Archive"
                    ' if entire row or row header is selected see if a delete is desired
                    If e.ColumnIndex = -1 And Not DataGridViewConnections.Rows(e.RowIndex).IsNewRow _
                    AndAlso (MessageBox.Show(String.Format(My.Resources.DeleteConfigText, _
                                                            InstanceName), _
                                             My.Resources.DeleteConfigCaption, _
                                             MessageBoxButtons.YesNo, _
                                             MessageBoxIcon.Question, _
                                             MessageBoxDefaultButton.Button2) = DialogResult.Yes) Then
                        If Mother.DAL.TargetHandshake(InstanceName, My.Settings.TargetHandshakeConnectionTimeout) Then
                            If Not ConnectToTarget(InstanceName) = Windows.Forms.DialogResult.OK Then
                                Try
                                    Mother.DAL.DropSQLClueServiceAccountFromTarget(Mother.SQLClueServiceAccount, _
                                                                                   InstanceName, _
                                                                                   My.Settings.TargetHandshakeConnectionTimeout)
                                Catch ex As Exception
                                    Mother.HandleException(New Exception(String.Format(My.Resources.ServiceLoginDropFailed, _
                                                                                       InstanceName, _
                                                                                       Mother.SQLClueServiceAccount), _
                                                                         ex))
                                End Try
                                ' see if the instance has any evt-notifys metadata deployed that needs to be cleaned up
                                ' first try to connect if not connected to clean up the notifications config
                                If HasEnabledNotifications(InstanceName) Then
                                    Try
                                        DisableEventNotifications()
                                    Catch ex As Exception
                                        Mother.HandleException(New Exception(String.Format(My.Resources.EvtNotifyDropFailed, _
                                                                                           InstanceName, _
                                                                                           My.Settings.TargetEventNotificationDatabase), _
                                                                             ex))
                                    End Try
                                End If
                            End If
                        Else
                            Mother.HandleException(New Exception(String.Format(My.Resources.EvtNotifyDropFailed, _
                                                                               InstanceName, _
                                                                               My.Settings.TargetEventNotificationDatabase)))
                        End If

                        ' whether notify or not, remove the row now
                        ' the delete proc will only delete if no relationships
                        ' otherwise does a logical delete of the connection row and preserves archive
                        Mother.DAL.dsSQLCfg.tConnection.FindByInstanceName(InstanceName).Delete()
                        Mother.DAL.SaveConnections()
                        ' the connection delete also deals with the related schedules 
                        InstanceName = ""
                        'Mother.DAL.LoadConfig()
                        ScheduleHasBeenChanged = True
                        TreeHasBeenChanged = True
                        LoadPlan()
                    Else
                        ' the instance name was selected
                        ' from here regardless if clicked header or instance cell
                        ' never accept a change that does not work
                        ' if schedules or config has pending changes must save now
                        If Not (ScheduleHasBeenChanged Or TreeHasBeenChanged) _
                        OrElse ((ScheduleHasBeenChanged Or TreeHasBeenChanged) AndAlso AskAndSaveChanges()) Then
                            ' this will change InstanceName if changed in dialog
                            Dim cnResult As DialogResult = ConnectToTarget(InstanceName, "Archive Planning")
                            If cnResult = Windows.Forms.DialogResult.OK Then
                                ' if new, need to check license level and add to configured drop down list
                                'If Mother.DAL.AvailableLicenses >= 0 Then
                                If Not Mother.ConfiguredInstanceList.Contains(InstanceName) Then
                                    Mother.DAL.GetConfiguredInstanceList()
                                End If
                                'Else
                                '    Exit Try
                                'End If
                            End If
                            Me.ToolStripStatusLabelConfiguration.Text = String.Format(My.Resources.ConnectedToSQL, _
                                                                                      "", _
                                                                                      InstanceName)
                            LoadPlan()
                        Else 'cancel, abort, unknown...

                            ' could work disconnected, use the host's SMO hieararchy
                            If Not (ScheduleHasBeenChanged Or TreeHasBeenChanged) _
                            OrElse ((ScheduleHasBeenChanged Or TreeHasBeenChanged) AndAlso AskAndSaveChanges()) Then
                                ' this will change InstanceName if changed in dialog
                                Dim cnResult As DialogResult = ConnectToTarget(InstanceName, "Archive Planning")
                                If cnResult = Windows.Forms.DialogResult.OK Then
                                    ' if new, need to check license level and add to configured drop down list
                                    'If Mother.DAL.AvailableLicenses >= 0 Then
                                    If Not Mother.ConfiguredInstanceList.Contains(InstanceName) Then
                                        Mother.DAL.GetConfiguredInstanceList()
                                        'End If
                                        'Else
                                        Exit Try
                                        'End If
                                        'End If
                                        Me.ToolStripStatusLabelConfiguration.Text = String.Format(My.Resources.ConnectedToSQL, _
                                                                                                  "", _
                                                                                                  InstanceName)
                                        LoadPlan()
                                    Else 'cancel, abort, unknown...

                                        ' could work disconnected, use the host's SMO hieararchy
                                        If MsgBox("Continue with a limited planning sesssion without connecting to the target SQL Server?", MsgBoxStyle.YesNo, "") = MsgBoxResult.Yes Then
                                            ' IsConnectedToTarget was set false in the connextion dialog 
                                            LoadPlan()
                                        Else
                                            ClearSelection()
                                            ' no fail msg if a cancel
                                            If Not cnResult = Windows.Forms.DialogResult.Cancel Then

                                                ' could be a raiserror here except that cancel/abort of login dialog sends you here
                                                Me.ToolStripStatusLabelConfiguration.Text = String.Format(My.Resources.ConnectionToSQLFailed, _
                                                                                                          "", _
                                                                                                          InstanceName)
                                            End If
                                        End If
                                    End If
                                End If
                            End If
                        End If
                    End If
            End Select
        Catch ex As Exception
            Mother.HandleException(New ApplicationException(String.Format("({0}.Connections_CellClick) Exception.", Me.Name), ex))
        Finally
            smoTreeView.Scrollable = True
            'smoTreeView.EndUpdate()
            If smoTreeView.Nodes.Count = 1 _
            AndAlso smoTreeView.Nodes(0).Nodes.Count > 0 Then
                smoTreeView.Nodes(0).Expand()
                smoTreeView.Nodes(0).Nodes("Databases").Expand()
            End If
            Me.Cursor = csr ' restore the original cursor
        End Try
    End Sub

    Private Sub ClearSelection()
        'reset the labels
        InstanceName = ""
        smoTreeView.Nodes.Clear()
        ' with instancename blank this should return no rows
        LoadSchedule()
        DataGridViewConnections.ClearSelection()
    End Sub

    Private Function HasEnabledNotifications(ByVal InstanceName As String) As Boolean
        HasEnabledNotifications = False
        For Each r As DataGridViewRow In DataGridViewArchiveSchedules.Rows
            If CBool(r.Cells("DataGridViewCheckBoxColumnArchiveScheduleUseEventNotifications").Value) = True Then
                HasEnabledNotifications = True
                Exit For
            End If
        Next
    End Function

    Private Function ConnectToTarget(ByVal SQLInstance As String, Optional ByVal ConnectTitleText As String = "") As DialogResult
        'should only reconnect if the current connection is not the selected connection
        ' here you need the connection row
        Dim result As DialogResult = Windows.Forms.DialogResult.OK
        Try
            If Not (TargetInstance Is Nothing) Then
                ' this will cause some extra teardowns for named instances, try it and see what happens
                If Not TargetInstance.ConnectionContext.TrueName = SQLInstance Then
                    TargetInstance.ConnectionContext.Disconnect()
                End If
                TargetInstance = Nothing
            End If
            Dim LnSecure As Boolean = My.Settings.RepositoryUseTrustedConnection
            Dim Ln As String = My.Settings.RepositorySQLLoginName
            Dim Pwd As String = My.Settings.RepositorySQLLoginPassword
            Dim CnTOut As Integer = My.Settings.RepositoryConnectionTimeout
            Dim EncryptCn As Boolean = My.Settings.RepositoryEncryptConnection
            Dim TrustCert As Boolean = My.Settings.RepositoryTrustServerCertificate
            Dim NetProtocol As String = ""
            Dim Proto As String = ""
            If TargetInstance Is Nothing Then
                Dim crow As cCommon.dsSQLConfiguration.tConnectionRow = Nothing
                If SQLInstance <> "" Then
                    ' targetinstance is created then already connected to desired server
                    crow = Mother.DAL.dsSQLCfg.tConnection.FindByInstanceName(SQLInstance)
                    If Not crow.Item("NetworkProtocol").ToString = "" Then
                        For Each p As String In My.Settings.NetworkLibraries
                            If InStr(p, crow.Item("NetworkProtocol").ToString) > 0 Then
                                NetProtocol = p
                                Exit For
                            End If
                        Next
                    End If
                    LnSecure = CBool(crow.Item("LoginSecure"))
                    Ln = ""
                    Pwd = ""
                    CnTOut = CInt(crow.Item("ConnectionTimeout"))
                    EncryptCn = CBool(crow.Item("EncryptedConnection"))
                    TrustCert = CBool(crow.Item("TrustServerCertificate"))
                    Proto = NetProtocol
                    If LnSecure = False Then
                        Mother.DAL.GetSQLAuthenticator(InstanceName, Ln, Pwd)
                    End If
                End If
                Dim cn As DialogConnect = New DialogConnect
                result = cn.ShowDialog(TargetInstance, _
                                       SQLInstance, _
                                       LnSecure, _
                                       Ln, _
                                       Pwd, _
                                       CnTOut, _
                                       Proto, _
                                       EncryptCn, _
                                       TrustCert, _
                                       Me, _
                                       ConnectTitleText)
                If result = Windows.Forms.DialogResult.OK Then
                    IsConnectedToTarget = True
                    If InstanceName <> SQLInstance Then
                        InstanceName = SQLInstance
                    End If

                    ' try again? - in case the instance was previously marked for logical delete
                    crow = Mother.DAL.dsSQLCfg.tConnection.FindByInstanceName(SQLInstance)
                    If crow Is Nothing Then
                        crow = Mother.DAL.dsSQLCfg.tConnection.AddtConnectionRow(SQLInstance, _
                                                     EncryptCn, TrustCert, Proto, CnTOut, LnSecure)
                    ElseIf Not (LnSecure = CBool(crow.Item("LoginSecure"))) _
                    Or Not (CnTOut = CInt(crow.Item("ConnectionTimeout"))) _
                    Or Not (EncryptCn = CBool(crow.Item("EncryptedConnection"))) _
                    Or Not (TrustCert = CBool(crow.Item("TrustServerCertificate"))) _
                    Or Not (Proto = NetProtocol) Then
                        crow.Item("LoginSecure") = LnSecure
                        crow.Item("ConnectionTimeout") = CnTOut
                        crow.Item("EncryptedConnection") = EncryptCn
                        crow.Item("TrustServerCertificate") = TrustCert
                        For Each p As String In My.Settings.NetworkLibraries
                            If InStr(p, NetProtocol) > 0 Then
                                crow.Item("NetworkProtocol") = p
                                Exit For
                            End If
                        Next
                    End If
                    Mother.DAL.SaveConnections()
                    'Try
                    '    ' nothing to do unless it raises an error
                    '    Mother.DAL.AvailableLicenses()
                    'Catch ex As Exception
                    '    Mother.DAL.dsSQLCfg.tConnection.FindByInstanceName(InstanceName).Delete()
                    '    Mother.DAL.SaveConnections()
                    '    ' the connection delete also deals with the related schedules 
                    '    InstanceName = ""
                    '    Throw ex
                    'End Try
                    If LnSecure = False Then
                        Mother.DAL.SetSQLAuthenticator(InstanceName, Ln, Pwd)
                    End If
                Else
                    IsConnectedToTarget = False
                End If
            End If
            'success/fail messaging needs to be handled by caller
            ConnectToTarget = result
        Catch ex As Exception
            ConnectToTarget = DialogResult.Abort
            Throw New Exception(String.Format("({0}.ConnectToTarget) Exception.", Me.Name), ex)
        End Try
    End Function

    Private Sub ButtonSave_Click(ByVal sender As System.Object, _
                                  ByVal e As System.EventArgs) _
                                  Handles ButtonSave.Click
        Try
            If Not InstanceName Is Nothing _
            AndAlso Not InstanceName = "" Then
                LoadPlan()
            End If
        Catch ex As Exception
            Mother.HandleException(New ApplicationException(String.Format("({0}.ButtonSave_Click) Exception.", Me.Name), ex))
        End Try
    End Sub

    Private Sub ButtonCancel_Click(ByVal sender As System.Object, _
                                  ByVal e As System.EventArgs) _
                                  Handles ButtonCancel.Click
        Try
            UndoEventNotificationChanges()
            If Mother.DAL.dsSQLCfg.HasChanges Then
                Mother.DAL.dsSQLCfg.RejectChanges()
            End If
            LoadPlan()
        Catch ex As Exception
            Mother.HandleException(New ApplicationException(String.Format("({0}.ButtonCancel_Click) Exception.", Me.Name), ex))
        End Try
    End Sub

    Private Sub LoadPlan()
        Try
            SplashScreen1.ToggleSplashVisible()
            SplashScreen1.CurrentStatusEventHandler("Loading Archive Plan, this may take a moment....")
            My.Application.DoEvents()
            If ScheduleHasBeenChanged _
            Or DataGridViewArchiveSchedules.RowCount = 0 Then
                My.Application.DoEvents()
                BindingSourceArchiveSchedule.EndEdit()
                Mother.DAL.SaveSchedule()
                ScheduleHasBeenChanged = True
            End If
            If IsConnectedToTarget _
            And ((TreeHasBeenChanged _
                 Or smoTreeView.Nodes.Count = 0) _
                 AndAlso InstanceName <> "") Then
                My.Application.DoEvents()
                smoTreeView.Scrollable = False
                SyncDataSetToTree()
                InitTreeFromTargetInstance()
                TreeHasBeenChanged = True
            End If
            My.Application.DoEvents()
            If IsConnectedToTarget Then
                Mother.DAL.LoadConfig()
            End If
            LoadSchedule()
            If IsConnectedToTarget And TreeHasBeenChanged Then
                My.Application.DoEvents()
                LoadTreeFromDataSet()
                smoTreeView.Scrollable = True
                TreeHasBeenChanged = False
            End If
            If InstanceName = "" Then
                LabelArchiveSchedule.Text = "Schedules"
                LabelConfigfuration.Text = "Items to Archive"
            Else
                LabelArchiveSchedule.Text = String.Format(My.Resources.ArchiveScheduleViewTitle, InstanceName)
                LabelConfigfuration.Text = String.Format(My.Resources.ArchiveConfigurationTreeTitle, InstanceName)
                ' re-selected instance row DataGridViewConnections.
                For Each r As DataGridViewRow In DataGridViewConnections.Rows
                    If r.Cells("DataGridViewButtonColumnConnectionInstanceName").Value.ToString = InstanceName Then
                        r.Cells(0).Selected = True
                        Exit For
                    End If
                Next
            End If
        Catch ex As Exception
            Throw New Exception(String.Format("({0}.LoadPlan) Exception.", Me.Name), ex)
        Finally
            If smoTreeView.Nodes.Count = 1 _
            AndAlso smoTreeView.Nodes(0).Nodes.Count > 0 Then
                smoTreeView.Nodes(0).Expand()
                smoTreeView.Nodes(0).Nodes("Databases").Expand()
            End If
            SplashScreen1.CurrentStatusEventHandler("")
            SplashScreen1.ToggleSplashVisible()
        End Try
    End Sub

    Private Sub LoadSchedule()
        Try
            DataGridViewArchiveSchedules.DataBindings.DefaultDataSourceUpdateMode = DataSourceUpdateMode.OnPropertyChanged
            ' reset the schedule before the time consuming tree load (less confusing?)
            BindingSourceArchiveSchedule.DataSource = Mother.DAL.dsSQLCfg
            BindingSourceArchiveSchedule.DataMember = "tSchedule"
            BindingSourceArchiveSchedule.Sort = "Id"
            BindingSourceArchiveSchedule.Filter = String.Format("InstanceName = '{0}'", InstanceName)
            BindingSourceArchiveSchedule.ResetBindings(False)
            For Each r As DataGridViewRow In DataGridViewArchiveSchedules.Rows
                If Not r.IsNewRow Then
                    If r.Cells("DataGridViewCheckBoxColumnArchiveScheduleUseEventNotifications").Value = True Then
                        r.Cells("DataGridViewImageColumnArchiveScheduleViewQueue").Value = My.Resources.FormRunHS
                    Else
                        ' attempting to get at the nullvalue image in the datagridviewcellstyle
                        r.Cells("DataGridViewImageColumnArchiveScheduleViewQueue").Value = Nothing
                    End If
                End If
            Next
            ScheduleHasBeenChanged = False
        Catch ex As Exception
            Throw New Exception(String.Format("({0}.LoadSchedule) Exception.", Me.Name), ex)
        End Try
    End Sub

    Private Sub ConfigurationForm_FormClosing(ByVal sender As Object, _
                                                ByVal e As System.Windows.Forms.FormClosingEventArgs) _
                                                Handles Me.FormClosing
        Dim csr As Cursor = Me.Cursor
        Try
            Me.Cursor = Cursors.WaitCursor   ' Display the waiting cursor
            Try
                If (ArchiveIsRunning) Then
                    ' does this need an r-u-sure?
                    Dim result As DialogResult = MessageBox.Show(My.Resources.ActiveArchiveAbortText, _
                                                                 My.Resources.ActiveArchiveAbortCaption, _
                                                                 MessageBoxButtons.YesNo, _
                                                                 MessageBoxIcon.Warning, _
                                                                 MessageBoxDefaultButton.Button2)
                    If result = Windows.Forms.DialogResult.No Then
                        e.Cancel = True
                        Me.Cursor = csr
                        Exit Sub
                    End If
                    cArchive1.CancelAsyncArchive()
                    ArchiveIsRunning = False
                End If
                If Mother.DAL.dsSQLCfg.HasChanges AndAlso Not AskAndSaveChanges() Then
                    Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
                End If
            Catch ex As Exception
                Mother.HandleException(New ApplicationException(String.Format("({0}.FormClosing) Exception.", Me.Name), ex))
            Finally
                If Mother.DAL.dsSQLCfg.HasChanges Then
                    Mother.DAL.dsSQLCfg.RejectChanges()
                End If
            End Try
            ' this is OK whether the target is the target or is the host standing in
            If Not (TargetInstance Is Nothing) Then
                TargetInstance.ConnectionContext.Disconnect()
                TargetInstance = Nothing
            End If
        Catch ex As Exception
            Mother.HandleException(New ApplicationException(String.Format("({0}.FormClosing) Exception.", Me.Name), ex))
        Finally
            Me.Cursor = csr ' restore the original cursor
        End Try
    End Sub

    Private Function AskAndSaveChanges() As Boolean
        Try
            AskAndSaveChanges = False
            Dim answer As DialogResult = MessageBox.Show(My.Resources.ArchiveHasUnsavedChanges, _
                                                         My.Resources.UnsavedChangesCaption, _
                                                         MessageBoxButtons.YesNoCancel)
            If answer = Windows.Forms.DialogResult.Yes Then
                ' use whatever state the change flags are in
                LoadPlan()
                AskAndSaveChanges = True
            ElseIf answer = Windows.Forms.DialogResult.No Then
                UndoEventNotificationChanges()
                Mother.DAL.dsSQLCfg.RejectChanges()
                TreeHasBeenChanged = False
                ScheduleHasBeenChanged = False
                LoadPlan()
                AskAndSaveChanges = True
            ElseIf answer = Windows.Forms.DialogResult.Cancel Then
                AskAndSaveChanges = False
            End If
        Catch ex As Exception
            Throw New Exception(String.Format("({0}.AskAndSaveChanges) Exception.", Me.Name), ex)
        End Try
    End Function

    Private Sub UndoEventNotificationChanges()
        Try
            'fall through the rows and undo any notification config that was changed
            Dim srows As DataTable = Mother.DAL.dsSQLCfg.tSchedule.GetChanges()
            If Not srows Is Nothing Then
                For Each s As DataRow In srows.Rows
                    If (s.RowState = DataRowState.Modified _
                        AndAlso Not (Object.ReferenceEquals(s.Item("UseEventNotifications", DataRowVersion.Original), _
                                                            s.Item("UseEventNotifications", DataRowVersion.Current)))) _
                     OrElse (s.RowState = DataRowState.Added) Then
                        If CBool(s.Item("UseEventNotifications", DataRowVersion.Original)) Then
                            ToolStripStatusLabelConfiguration.Text = String.Format(My.Resources.EventNotificationsEnable, _
                                                                                   InstanceName)
                            EnableEventNotifications()
                        Else
                            ToolStripStatusLabelConfiguration.Text = String.Format(My.Resources.EventNotificationsDisable, _
                                                                                   InstanceName)
                            DisableEventNotifications()
                        End If
                    End If
                Next
                Mother.DAL.dsSQLCfg.tSchedule.RejectChanges()
            End If
        Catch ex As Exception
            Throw New Exception(String.Format("({0}.UndoEventNotificationChanges) Exception.", Me.Name), ex)
        End Try
    End Sub

    Private Sub Connections_DataError(ByVal sender As Object, _
                                      ByVal e As System.Windows.Forms.DataGridViewDataErrorEventArgs) _
                                      Handles DataGridViewConnections.DataError
        If DataGridViewConnections.CurrentRow Is Nothing Then
            e.Cancel = True
        End If
    End Sub

#Region "  Configuration tree view "

    Private Sub SyncDataSetToTree()
        ' this SHOULD only be called if the node exists
        Try
            If TreeHasBeenChanged Then
                With smoTreeView.Nodes(InstanceName)
                    If Not .Nodes("ActiveDirectory") Is Nothing Then
                        irow("ActiveDirectory") = .Nodes("ActiveDirectory").Checked
                    End If
                    If Not .Nodes("Audits") Is Nothing Then
                        irow("Audits") = .Nodes("Audits").Checked
                    End If
                    If Not .Nodes("BackupDevices") Is Nothing Then
                        irow("BackupDevices") = .Nodes("BackupDevices").Checked
                    End If
                    If Not .Nodes("Configuration") Is Nothing Then
                        irow("Configuration") = .Nodes("Configuration").Checked
                    End If
                    If Not .Nodes("Credentials") Is Nothing Then
                        irow("Credentials") = .Nodes("Credentials").Checked
                    End If
                    If Not .Nodes("CryptographicProviders") Is Nothing Then
                        irow("CryptographicProviders") = .Nodes("CryptographicProviders").Checked
                    End If
                    If Not .Nodes("Databases") Is Nothing Then
                        irow("Databases") = .Nodes("Databases").Checked
                    End If
                    If Not .Nodes("EndPoints") Is Nothing Then
                        irow("EndPoints") = .Nodes("EndPoints").Checked
                    End If
                    If Not .Nodes("FullTextService") Is Nothing Then
                        irow("FullTextService") = .Nodes("FullTextService").Checked
                    End If
                    If Not .Nodes("Information") Is Nothing Then
                        irow("Information") = .Nodes("Information").Checked
                    End If
                    If Not .Nodes("JobServer") Is Nothing Then
                        irow("JobServer") = .Nodes("JobServer").Checked
                    End If
                    If Not .Nodes("Logins") Is Nothing Then
                        irow("Logins") = .Nodes("Logins").Checked
                    End If
                    If Not .Nodes("LinkedServers") Is Nothing Then
                        irow("LinkedServers") = .Nodes("LinkedServers").Checked
                    End If
                    If Not .Nodes("Mail") Is Nothing Then
                        irow("Mail") = .Nodes("Mail").Checked
                    End If
                    If Not .Nodes("ProxyAccount") Is Nothing Then
                        irow("ProxyAccount") = .Nodes("ProxyAccount").Checked
                    End If
                    If Not .Nodes("ResourceGovernor") Is Nothing Then
                        irow("ResourceGovernor") = .Nodes("ResourceGovernor").Checked
                    End If
                    If Not .Nodes("Roles") Is Nothing Then
                        irow("Roles") = .Nodes("Roles").Checked
                    End If
                    If Not .Nodes("ServerAuditSpecifications") Is Nothing Then
                        irow("ServerAuditSpecifications") = .Nodes("ServerAuditSpecifications").Checked
                    End If
                    If Not .Nodes("Settings") Is Nothing Then
                        irow("Settings") = .Nodes("Settings").Checked
                    End If
                    If Not .Nodes("Triggers") Is Nothing Then
                        irow("Triggers") = .Nodes("Triggers").Checked
                    End If
                    If Not .Nodes("UserDefinedMessages") Is Nothing Then
                        irow("UserDefinedMessages") = .Nodes("UserDefinedMessages").Checked
                    End If
                    Dim jrow As DataRow = Mother.DAL.dsSQLCfg.tJobServer.FindByInstanceName(InstanceName)
                    If Not .Nodes("JobServer") Is Nothing Then
                        SplashScreen1.CurrentStatusEventHandler("Loading Archive Plan, this may take a moment....")
                        My.Application.DoEvents()
                        With .Nodes("JobServer")
                            If Not .Nodes("Alerts") Is Nothing Then
                                jrow("Alerts") = .Nodes("Alerts").Checked
                            End If
                            If Not .Nodes("AlertSystem") Is Nothing Then
                                jrow("AlertSystem") = .Nodes("AlertSystem").Checked
                            End If
                            If Not .Nodes("Jobs") Is Nothing Then
                                jrow("Jobs") = .Nodes("Jobs").Checked
                            End If
                            If Not .Nodes("Operators") Is Nothing Then
                                jrow("Operators") = .Nodes("Operators").Checked
                            End If
                            If Not .Nodes("ProxyAccounts") Is Nothing Then
                                jrow("ProxyAccounts") = .Nodes("ProxyAccounts").Checked
                            End If
                            If Not .Nodes("TargetServers") Is Nothing Then
                                jrow("TargetServers") = .Nodes("TargetServers").Checked
                            End If
                        End With
                    End If
                End With
                ' could be a new database but should never be new instance or jobserver here
                ' I have a case where the install failed and now i need a new instance and jobserver because I deleted all
                For Each db As TreeNode In smoTreeView.Nodes(InstanceName).Nodes("Databases").Nodes
                    SplashScreen1.CurrentStatusEventHandler("Loading Archive Plan, this may take a moment....")
                    My.Application.DoEvents()
                    Dim drow As cCommon.dsSQLConfiguration.tDbRow = _
                                Mother.DAL.dsSQLCfg.tDb.FindByNameInstanceName(db.Text, InstanceName)
                    Dim AddDb As Boolean = False
                    If drow Is Nothing Then
                        drow = Mother.DAL.dsSQLCfg.tDb.NewtDbRow()
                        drow("Name") = db.Name
                        drow("InstanceName") = InstanceName
                        AddDb = True
                    End If
                    If db.Nodes("ActiveDirectory") Is Nothing Then
                        drow("ActiveDirectory") = False
                    Else
                        drow("ActiveDirectory") = db.Nodes("ActiveDirectory").Checked
                    End If
                    If db.Nodes("ApplicationRoles") Is Nothing Then
                        drow("ApplicationRoles") = False
                    Else
                        drow("ApplicationRoles") = db.Nodes("ApplicationRoles").Checked
                    End If
                    If db.Nodes("Assemblies") Is Nothing Then
                        drow("Assemblies") = False
                    Else
                        drow("Assemblies") = db.Nodes("Assemblies").Checked
                    End If
                    If db.Nodes("AsymmetricKeys") Is Nothing Then
                        drow("AsymmetricKeys") = False
                    Else
                        drow("AsymmetricKeys") = db.Nodes("AsymmetricKeys").Checked
                    End If
                    If db.Nodes("Certificates") Is Nothing Then
                        drow("Certificates") = False
                    Else
                        drow("Certificates") = db.Nodes("Certificates").Checked
                    End If
                    If db.Nodes("DatabaseAuditSpecifications") Is Nothing Then
                        drow("DatabaseAuditSpecifications") = False
                    Else
                        drow("DatabaseAuditSpecifications") = db.Nodes("DatabaseAuditSpecifications").Checked
                    End If
                    If db.Nodes("DatabaseOptions") Is Nothing Then
                        drow("DatabaseOptions") = False
                    Else
                        drow("DatabaseOptions") = db.Nodes("DatabaseOptions").Checked
                    End If
                    If db.Nodes("Defaults") Is Nothing Then
                        drow("Defaults") = False
                    Else
                        drow("Defaults") = db.Nodes("Defaults").Checked
                    End If
                    If db.Nodes("FullTextCatalogs") Is Nothing Then
                        drow("FullTextCatalogs") = False
                    Else
                        drow("FullTextCatalogs") = db.Nodes("FullTextCatalogs").Checked
                    End If
                    If db.Nodes("FullTextStopLists") Is Nothing Then
                        drow("FullTextStopLists") = False
                    Else
                        drow("FullTextStopLists") = db.Nodes("FullTextStopLists").Checked
                    End If
                    If db.Nodes("PartitionFunctions") Is Nothing Then
                        drow("PartitionFunctions") = False
                    Else
                        drow("PartitionFunctions") = db.Nodes("PartitionFunctions").Checked
                    End If
                    If db.Nodes("PartitionSchemes") Is Nothing Then
                        drow("PartitionSchemes") = False
                    Else
                        drow("PartitionSchemes") = db.Nodes("PartitionSchemes").Checked
                    End If
                    If db.Nodes("PlanGuides") Is Nothing Then
                        drow("PlanGuides") = False
                    Else
                        drow("PlanGuides") = db.Nodes("PlanGuides").Checked
                    End If
                    If db.Nodes("Roles") Is Nothing Then
                        drow("Roles") = False
                    Else
                        drow("Roles") = db.Nodes("Roles").Checked
                    End If
                    If db.Nodes("Rules") Is Nothing Then
                        drow("Rules") = False
                    Else
                        drow("Rules") = db.Nodes("Rules").Checked
                    End If
                    If db.Nodes("Schemas") Is Nothing Then
                        drow("Schemas") = False
                    Else
                        drow("Schemas") = db.Nodes("Schemas").Checked
                    End If
                    If db.Nodes("ServiceBroker") Is Nothing Then
                        drow("ServiceBroker") = False
                    Else
                        drow("ServiceBroker") = db.Nodes("ServiceBroker").Checked
                    End If
                    If db.Nodes("StoredProcedures") Is Nothing Then
                        drow("StoredProcedures") = False
                    Else
                        drow("StoredProcedures") = db.Nodes("StoredProcedures").Checked
                    End If
                    If db.Nodes("SymmetricKeys") Is Nothing Then
                        drow("SymmetricKeys") = False
                    Else
                        drow("SymmetricKeys") = db.Nodes("SymmetricKeys").Checked
                    End If
                    If db.Nodes("Synonyms") Is Nothing Then
                        drow("Synonyms") = False
                    Else
                        drow("Synonyms") = db.Nodes("Synonyms").Checked
                    End If
                    If db.Nodes("Tables") Is Nothing Then
                        drow("Tables") = False
                    Else
                        drow("Tables") = db.Nodes("Tables").Checked
                    End If
                    If db.Nodes("Triggers") Is Nothing Then
                        drow("Triggers") = False
                    Else
                        drow("Triggers") = db.Nodes("Triggers").Checked
                    End If
                    If db.Nodes("UserDefinedAggregates") Is Nothing Then
                        drow("UserDefinedAggregates") = False
                    Else
                        drow("UserDefinedAggregates") = db.Nodes("UserDefinedAggregates").Checked
                    End If
                    If db.Nodes("UserDefinedDataTypes") Is Nothing Then
                        drow("UserDefinedDataTypes") = False
                    Else
                        drow("UserDefinedDataTypes") = db.Nodes("UserDefinedDataTypes").Checked
                    End If
                    If db.Nodes("UserDefinedFunctions") Is Nothing Then
                        drow("UserDefinedFunctions") = False
                    Else
                        drow("UserDefinedFunctions") = db.Nodes("UserDefinedFunctions").Checked
                    End If
                    If db.Nodes("UserDefinedTableTypes") Is Nothing Then
                        drow("UserDefinedTableTypes") = False
                    Else
                        drow("UserDefinedTableTypes") = db.Nodes("UserDefinedTableTypes").Checked
                    End If
                    If db.Nodes("UserDefinedTypes") Is Nothing Then
                        drow("UserDefinedTypes") = False
                    Else
                        drow("UserDefinedTypes") = db.Nodes("UserDefinedTypes").Checked
                    End If
                    If db.Nodes("Users") Is Nothing Then
                        drow("Users") = False
                    Else
                        drow("Users") = db.Nodes("Users").Checked
                    End If
                    If db.Nodes("Views") Is Nothing Then
                        drow("Views") = False
                    Else
                        drow("Views") = db.Nodes("Views").Checked
                    End If
                    If db.Nodes("XMLSchemaCollections") Is Nothing Then
                        drow("XMLSchemaCollections") = False
                    Else
                        drow("XMLSchemaCollections") = db.Nodes("XMLSchemaCollections").Checked
                    End If
                    If AddDb Then
                        Mother.DAL.dsSQLCfg.tDb.AddtDbRow(drow)
                    End If
                    If Not db.Nodes("ServiceBroker") Is Nothing Then
                        SplashScreen1.CurrentStatusEventHandler("Loading Archive Plan, this may take a moment....")
                        My.Application.DoEvents()
                        Dim srow As cCommon.dsSQLConfiguration.tServiceBrokerRow = _
                                    Mother.DAL.dsSQLCfg.tServiceBroker.FindByDatabaseNameInstanceName(db.Text, InstanceName)
                        Dim AddServiceBroker As Boolean = False
                        If srow Is Nothing Then
                            srow = Mother.DAL.dsSQLCfg.tServiceBroker.NewtServiceBrokerRow
                            srow("DatabaseName") = db.Name
                            srow("InstanceName") = InstanceName
                            AddServiceBroker = True
                        End If
                        With db.Nodes("ServiceBroker")
                            If .Nodes("MessageTypes") Is Nothing Then
                                srow("MessageTypes") = False
                            Else
                                srow("MessageTypes") = .Nodes("MessageTypes").Checked
                            End If
                            If .Nodes("Priorities") Is Nothing Then
                                srow("Priorities") = False
                            Else
                                srow("Priorities") = .Nodes("Priorities").Checked
                            End If
                            If .Nodes("Queues") Is Nothing Then
                                srow("Queues") = False
                            Else
                                srow("Queues") = .Nodes("Queues").Checked
                            End If
                            If .Nodes("RemoteServiceBindings") Is Nothing Then
                                srow("RemoteServiceBindings") = False
                            Else
                                srow("RemoteServiceBindings") = .Nodes("RemoteServiceBindings").Checked
                            End If
                            If .Nodes("Routes") Is Nothing Then
                                srow("Routes") = False
                            Else
                                srow("Routes") = .Nodes("Routes").Checked
                            End If
                            If .Nodes("ServiceContracts") Is Nothing Then
                                srow("ServiceContracts") = False
                            Else
                                srow("ServiceContracts") = .Nodes("ServiceContracts").Checked
                            End If
                            If .Nodes("Services") Is Nothing Then
                                srow("Services") = False
                            Else
                                srow("Services") = .Nodes("Services").Checked
                            End If
                        End With
                        If AddServiceBroker Then
                            Mother.DAL.dsSQLCfg.tServiceBroker.AddtServiceBrokerRow(srow)
                        End If
                    End If
                Next
                Mother.DAL.SaveConfig()
            End If
        Catch ex As Exception
            Throw New Exception(String.Format("({0}.SyncDataSetToTree) Exception.", Me.Name), ex)
        End Try
    End Sub

    Private Sub LoadTreeFromDataSet()
        Try
            If Not smoTreeView.Nodes(InstanceName) Is Nothing Then
                smoTreeView.BeginUpdate()
                irow = Nothing
                ' verify that the selected instance is in the datasest
                irow = Mother.DAL.dsSQLCfg.tInstance.FindByName(InstanceName)
                ' if this is a new server nothing to do, leave everything checked
                ' if an existing server only leave checked what is currently enabled in the cfg db
                If Not (irow Is Nothing) Then
                    My.Application.DoEvents()
                    ' if a child node is selected, the parent node is implicitly selected 
                    ' this means the check boxes must be set from the top down here
                    ' because when the user selects the parent it must also select children  
                    Dim jrow As cCommon.dsSQLConfiguration.tJobServerRow = _
                              Mother.DAL.dsSQLCfg.tJobServer.FindByInstanceName(InstanceName)
                    If (CBool(irow("ActiveDirectory")) _
                    Or CBool(irow("Audits")) _
                    Or CBool(irow("BackupDevices")) _
                    Or CBool(irow("Configuration")) _
                    Or CBool(irow("Credentials")) _
                    Or CBool(irow("CryptographicProviders")) _
                    Or CBool(irow("Databases")) _
                    Or CBool(irow("EndPoints")) _
                    Or CBool(irow("FullTextService")) _
                    Or CBool(irow("Information")) _
                    Or CBool(irow("JobServer")) _
                    Or CBool(irow("Logins")) _
                    Or CBool(irow("LinkedServers")) _
                    Or CBool(irow("Mail")) _
                    Or CBool(irow("ProxyAccount")) _
                    Or CBool(irow("ResourceGovernor")) _
                    Or CBool(irow("Roles")) _
                    Or CBool(irow("ServerAuditSpecifications")) _
                    Or CBool(irow("Settings")) _
                    Or CBool(irow("Triggers")) _
                    Or CBool(irow("UserDefinedMessages"))) Then
                        ' no JobServer in tree if SQLExpress
                        If Not smoTreeView.Nodes(InstanceName).Nodes("JobServer") Is Nothing _
                        AndAlso smoTreeView.Nodes(InstanceName).Nodes("JobServer").Checked _
                        AndAlso Not smoTreeView.Nodes(InstanceName).Checked Then
                            smoTreeView.Nodes(InstanceName).Checked = True
                        End If
                    End If
                    ' set the treeview checkboxes - top down since the default behaviour of 
                    ' children inheriting from parents will 
                    With smoTreeView.Nodes(InstanceName)
                        My.Application.DoEvents()
                        If Not (.Nodes("ActiveDirectory") Is Nothing) Then
                            .Nodes("ActiveDirectory").Checked = irow.ActiveDirectory
                        End If
                        If Not (.Nodes("Audits") Is Nothing) Then
                            .Nodes("Audits").Checked = irow.Audits
                        End If
                        If Not (.Nodes("BackupDevices") Is Nothing) Then
                            .Nodes("BackupDevices").Checked = irow.BackupDevices
                        End If
                        If Not (.Nodes("Configuration") Is Nothing) Then
                            .Nodes("Configuration").Checked = irow.Configuration
                        End If
                        If Not (.Nodes("Credentials") Is Nothing) Then
                            .Nodes("Credentials").Checked = irow.Credentials
                        End If
                        If Not (.Nodes("CryptographicProviders") Is Nothing) Then
                            .Nodes("CryptographicProviders").Checked = irow.CryptographicProviders
                        End If
                        If Not (.Nodes("Databases") Is Nothing) Then
                            .Nodes("Databases").Checked = irow.Databases
                        End If
                        If Not (.Nodes("EndPoints") Is Nothing) Then
                            .Nodes("EndPoints").Checked = irow.EndPoints
                        End If
                        If Not (.Nodes("FullTextService") Is Nothing) Then
                            .Nodes("FullTextService").Checked = irow.FullTextService
                        End If
                        If Not (.Nodes("Information") Is Nothing) Then
                            .Nodes("Information").Checked = irow.Information
                        End If
                        If Not (.Nodes("JobServer") Is Nothing) Then
                            With .Nodes("JobServer")
                                My.Application.DoEvents()
                                .Checked = irow.JobServer
                                If jrow Is Nothing Then
                                    Mother.DAL.dsSQLCfg.tJobServer.AddtJobServerRow(irow, _
                                                        If(.Nodes("Alerts") Is Nothing, False, Not .Nodes("Alerts").Checked), _
                                                        If(.Nodes("AlertsSystem") Is Nothing, False, Not .Nodes("AlertsSystem").Checked), _
                                                        If(.Nodes("Jobs") Is Nothing, False, Not .Nodes("Jobs").Checked), _
                                                        If(.Nodes("Operators") Is Nothing, False, Not .Nodes("Operators").Checked), _
                                                        If(.Nodes("ProxyAccounts") Is Nothing, False, Not .Nodes("ProxyAccounts").Checked), _
                                                        If(.Nodes("TargetServer") Is Nothing, False, Not .Nodes("TargetServer").Checked))
                                Else
                                    If Not (.Nodes("Alerts") Is Nothing) Then
                                        .Nodes("Alerts").Checked = jrow.Alerts
                                    End If
                                    If Not (.Nodes("AlertSystem") Is Nothing) Then
                                        .Nodes("AlertSystem").Checked = jrow.AlertSystem
                                    End If
                                    If Not (.Nodes("Jobs") Is Nothing) Then
                                        .Nodes("Jobs").Checked = jrow.Jobs
                                    End If
                                    If Not (.Nodes("Operators") Is Nothing) Then
                                        .Nodes("Operators").Checked = jrow.Operators
                                    End If
                                    If Not (.Nodes("ProxyAccounts") Is Nothing) Then
                                        .Nodes("ProxyAccounts").Checked = jrow.ProxyAccounts
                                    End If
                                    If Not (.Nodes("TargetServers") Is Nothing) Then
                                        .Nodes("TargetServers").Checked = jrow.TargetServers
                                    End If
                                End If
                            End With
                        End If
                        If Not (.Nodes("Logins") Is Nothing) Then
                            .Nodes("Logins").Checked = irow.Logins
                        End If
                        If Not (.Nodes("LinkedServers") Is Nothing) Then
                            .Nodes("LinkedServers").Checked = irow.LinkedServers
                        End If
                        If Not (.Nodes("Mail") Is Nothing) Then
                            .Nodes("Mail").Checked = irow.Mail
                        End If
                        If Not (.Nodes("ProxyAccount") Is Nothing) Then
                            .Nodes("ProxyAccount").Checked = irow.ProxyAccount
                        End If
                        If Not (.Nodes("ResourceGovernor") Is Nothing) Then
                            .Nodes("ResourceGovernor").Checked = irow.ResourceGovernor
                        End If
                        If Not (.Nodes("Roles") Is Nothing) Then
                            .Nodes("Roles").Checked = irow.Roles
                        End If
                        If Not (.Nodes("ServerAuditSpecifications") Is Nothing) Then
                            .Nodes("ServerAuditSpecifications").Checked = irow.ServerAuditSpecifications
                        End If
                        If Not (.Nodes("Settings") Is Nothing) Then
                            .Nodes("Settings").Checked = irow.Settings
                        End If
                        If Not (.Nodes("Triggers") Is Nothing) Then
                            .Nodes("Triggers").Checked = irow.Triggers
                        End If
                        If Not (.Nodes("UserDefinedMessages") Is Nothing) Then
                            .Nodes("UserDefinedMessages").Checked = irow.UserDefinedMessages
                        End If
                    End With
                    For Each db As TreeNode In smoTreeView.Nodes(InstanceName).Nodes("Databases").Nodes
                        My.Application.DoEvents()
                        'db.Checked = False
                        Dim drow As cCommon.dsSQLConfiguration.tDbRow = _
                                    Mother.DAL.dsSQLCfg.tDb.FindByNameInstanceName(db.Text, InstanceName)
                        If drow Is Nothing Then
                            Mother.DAL.dsSQLCfg.tDb.AddtDbRow(db.Text, _
                                                irow, _
                                                If(db.Nodes("ActiveDirectory") Is Nothing, False, Not (db.Nodes("ActiveDirectory").Checked)), _
                                                If(db.Nodes("ApplicationRoles") Is Nothing, False, Not (db.Nodes("ApplicationRoles").Checked)), _
                                                If(db.Nodes("Assemblies") Is Nothing, False, Not (db.Nodes("Assemblies").Checked)), _
                                                If(db.Nodes("AsymmetricKeys") Is Nothing, False, Not (db.Nodes("AsymmetricKeys").Checked)), _
                                                If(db.Nodes("Certificates") Is Nothing, False, Not (db.Nodes("Certificates").Checked)), _
                                                If(db.Nodes("DatabaseAuditSpecifications") Is Nothing, False, Not (db.Nodes("DatabaseAuditSpecifications").Checked)), _
                                                If(db.Nodes("DatabaseOptions") Is Nothing, False, Not (db.Nodes("DatabaseOptions").Checked)), _
                                                If(db.Nodes("Defaults") Is Nothing, False, Not (db.Nodes("Defaults").Checked)), _
                                                If(db.Nodes("FullTextCatalogs") Is Nothing, False, Not (db.Nodes("FullTextCatalogs").Checked)), _
                                                If(db.Nodes("FullTextStopLists") Is Nothing, False, Not (db.Nodes("FullTextStopLists").Checked)), _
                                                If(db.Nodes("PartitionFunctions") Is Nothing, False, Not (db.Nodes("PartitionFunctions").Checked)), _
                                                If(db.Nodes("PartitionSchemes") Is Nothing, False, Not (db.Nodes("PartitionSchemes").Checked)), _
                                                If(db.Nodes("PlanGuides") Is Nothing, False, Not (db.Nodes("PlanGuides").Checked)), _
                                                If(db.Nodes("Roles") Is Nothing, False, Not (db.Nodes("Roles").Checked)), _
                                                If(db.Nodes("Rules") Is Nothing, False, Not (db.Nodes("Rules").Checked)), _
                                                If(db.Nodes("Schemas") Is Nothing, False, Not (db.Nodes("Schemas").Checked)), _
                                                If(db.Nodes("ServiceBroker") Is Nothing, False, Not (db.Nodes("ServiceBroker").Checked)), _
                                                If(db.Nodes("StoredProcedures") Is Nothing, False, Not (db.Nodes("StoredProcedures").Checked)), _
                                                If(db.Nodes("SymmetricKeys") Is Nothing, False, Not (db.Nodes("SymmetricKeys").Checked)), _
                                                If(db.Nodes("Synonyms") Is Nothing, False, Not (db.Nodes("Synonyms").Checked)), _
                                                If(db.Nodes("Tables") Is Nothing, False, Not (db.Nodes("Tables").Checked)), _
                                                If(db.Nodes("Triggers") Is Nothing, False, Not (db.Nodes("Triggers").Checked)), _
                                                If(db.Nodes("UserDefinedAggregates") Is Nothing, False, Not (db.Nodes("UserDefinedAggregates").Checked)), _
                                                If(db.Nodes("UserDefinedDataTypes") Is Nothing, False, Not (db.Nodes("UserDefinedDataTypes").Checked)), _
                                                If(db.Nodes("UserDefinedFunctions") Is Nothing, False, Not (db.Nodes("UserDefinedFunctions").Checked)), _
                                                If(db.Nodes("UserDefinedTableTypes") Is Nothing, False, Not (db.Nodes("UserDefinedTableTypes").Checked)), _
                                                If(db.Nodes("UserDefinedTypes") Is Nothing, False, Not (db.Nodes("UserDefinedTypes").Checked)), _
                                                If(db.Nodes("Users") Is Nothing, False, Not (db.Nodes("Users").Checked)), _
                                                If(db.Nodes("Views") Is Nothing, False, Not (db.Nodes("Views").Checked)), _
                                                If(db.Nodes("XMLSchemaCollections") Is Nothing, False, Not (db.Nodes("XMLSchemaCollections").Checked)))
                        Else
                            ' if anything checked, the db is checked
                            ' db check is logical since there is no boolean to indicate
                            ' must mark checkbox of db node before setting any children
                            If (drow.ActiveDirectory _
                             Or drow.ApplicationRoles _
                             Or drow.Assemblies _
                             Or drow.AsymmetricKeys _
                             Or drow.Certificates _
                             Or drow.DatabaseAuditSpecifications _
                             Or drow.DatabaseOptions _
                             Or drow.Defaults _
                             Or drow.FullTextCatalogs _
                             Or drow.FullTextStopLists _
                             Or drow.PartitionFunctions _
                             Or drow.PartitionSchemes _
                             Or drow.PlanGuides _
                             Or drow.Roles _
                             Or drow.Rules _
                             Or drow.Schemas _
                             Or drow.ServiceBroker _
                             Or drow.StoredProcedures _
                             Or drow.SymmetricKeys _
                             Or drow.Synonyms _
                             Or drow.Tables _
                             Or drow.Triggers _
                             Or drow.UserDefinedAggregates _
                             Or drow.UserDefinedDataTypes _
                             Or drow.UserDefinedFunctions _
                             Or drow.UserDefinedTableTypes _
                             Or drow.UserDefinedTypes _
                             Or drow.Users _
                             Or drow.Views _
                             Or drow.XMLSchemaCollections) _
                            AndAlso irow.Databases Then
                                db.Checked = True
                            End If
                            If Not (db.Nodes("ActiveDirectory") Is Nothing) Then
                                db.Nodes("ActiveDirectory").Checked = drow.ActiveDirectory
                            End If
                            If Not (db.Nodes("ApplicationRoles") Is Nothing) Then
                                db.Nodes("ApplicationRoles").Checked = drow.ApplicationRoles
                            End If
                            If Not (db.Nodes("Assemblies") Is Nothing) Then
                                db.Nodes("Assemblies").Checked = drow.Assemblies
                            End If
                            If Not (db.Nodes("AsymmetricKeys") Is Nothing) Then
                                db.Nodes("AsymmetricKeys").Checked = drow.AsymmetricKeys
                            End If
                            If Not (db.Nodes("Certificates") Is Nothing) Then
                                db.Nodes("Certificates").Checked = drow.Certificates
                            End If
                            If Not (db.Nodes("DatabaseAuditSpecifications") Is Nothing) Then
                                db.Nodes("DatabaseAuditSpecifications").Checked = drow.DatabaseAuditSpecifications
                            End If
                            If Not (db.Nodes("DatabaseOptions") Is Nothing) Then
                                db.Nodes("DatabaseOptions").Checked = drow.DatabaseOptions
                            End If
                            If Not (db.Nodes("Defaults") Is Nothing) Then
                                db.Nodes("Defaults").Checked = drow.Defaults
                            End If
                            If Not (db.Nodes("FullTextCatalogs") Is Nothing) Then
                                db.Nodes("FullTextCatalogs").Checked = drow.FullTextCatalogs
                            End If
                            If Not (db.Nodes("FullTextStopLists") Is Nothing) Then
                                db.Nodes("FullTextStopLists").Checked = drow.FullTextStopLists
                            End If
                            If Not (db.Nodes("PartitionFunctions") Is Nothing) Then
                                db.Nodes("PartitionFunctions").Checked = drow.PartitionFunctions
                            End If
                            If Not (db.Nodes("PartitionSchemes") Is Nothing) Then
                                db.Nodes("PartitionSchemes").Checked = drow.PartitionSchemes
                            End If
                            If Not (db.Nodes("PlanGuides") Is Nothing) Then
                                db.Nodes("PlanGuides").Checked = drow.PlanGuides
                            End If
                            If Not (db.Nodes("Roles") Is Nothing) Then
                                db.Nodes("Roles").Checked = drow.Roles
                            End If
                            If Not (db.Nodes("Rules") Is Nothing) Then
                                db.Nodes("Rules").Checked = drow.Rules
                            End If
                            If Not (db.Nodes("Schemas") Is Nothing) Then
                                db.Nodes("Schemas").Checked = drow.Schemas
                            End If
                            If Not (db.Nodes("ServiceBroker") Is Nothing) Then
                                db.Nodes("ServiceBroker").Checked = drow.ServiceBroker
                            End If
                            If Not (db.Nodes("StoredProcedures") Is Nothing) Then
                                db.Nodes("StoredProcedures").Checked = drow.StoredProcedures
                            End If
                            If Not (db.Nodes("SymmetricKeys") Is Nothing) Then
                                db.Nodes("SymmetricKeys").Checked = drow.SymmetricKeys
                            End If
                            If Not (db.Nodes("Synonyms") Is Nothing) Then
                                db.Nodes("Synonyms").Checked = drow.Synonyms
                            End If
                            If Not (db.Nodes("Tables") Is Nothing) Then
                                db.Nodes("Tables").Checked = drow.Tables
                            End If
                            If Not (db.Nodes("Triggers") Is Nothing) Then
                                db.Nodes("Triggers").Checked = drow.Triggers
                            End If
                            If Not (db.Nodes("UserDefinedAggregates") Is Nothing) Then
                                db.Nodes("UserDefinedAggregates").Checked = drow.UserDefinedAggregates
                            End If
                            If Not (db.Nodes("UserDefinedDataTypes") Is Nothing) Then
                                db.Nodes("UserDefinedDataTypes").Checked = drow.UserDefinedDataTypes
                            End If
                            If Not (db.Nodes("UserDefinedFunctions") Is Nothing) Then
                                db.Nodes("UserDefinedFunctions").Checked = drow.UserDefinedFunctions
                            End If
                            If Not (db.Nodes("UserDefinedTableTypes") Is Nothing) Then
                                db.Nodes("UserDefinedTableTypes").Checked = drow.UserDefinedTableTypes
                            End If
                            If Not (db.Nodes("UserDefinedTypes") Is Nothing) Then
                                db.Nodes("UserDefinedTypes").Checked = drow.UserDefinedTypes
                            End If
                            If Not (db.Nodes("Users") Is Nothing) Then
                                db.Nodes("Users").Checked = drow.Users
                            End If
                            If Not (db.Nodes("Views") Is Nothing) Then
                                db.Nodes("Views").Checked = drow.Views
                            End If
                            If Not (db.Nodes("XMLSchemaCollections") Is Nothing) Then
                                db.Nodes("XMLSchemaCollections").Checked = drow.XMLSchemaCollections
                            End If
                            ' a repeat of the test from above but will searialize object creation
                            If Not (db.Nodes("ServiceBroker") Is Nothing) Then
                                My.Application.DoEvents()
                                Dim srow As cCommon.dsSQLConfiguration.tServiceBrokerRow = _
                                                Mother.DAL.dsSQLCfg.tServiceBroker.FindByDatabaseNameInstanceName(db.Text, InstanceName)
                                With db.Nodes("ServiceBroker")
                                    If srow Is Nothing Then
                                        Mother.DAL.dsSQLCfg.tServiceBroker.AddtServiceBrokerRow(db.Text, _
                                                            InstanceName, _
                                                            If(.Nodes("MessageTypes") Is Nothing, False, Not .Nodes("MessageTypes").Checked), _
                                                            If(.Nodes("Priorities") Is Nothing, False, Not .Nodes("Priorities").Checked), _
                                                            If(.Nodes("Queues") Is Nothing, False, Not .Nodes("Queues").Checked), _
                                                            If(.Nodes("RemoteServiceBindings") Is Nothing, False, Not .Nodes("RemoteServiceBindings").Checked), _
                                                            If(.Nodes("Routes") Is Nothing, False, Not .Nodes("Routes").Checked), _
                                                            If(.Nodes("ServiceContracts") Is Nothing, False, Not .Nodes("ServiceContracts").Checked), _
                                                            If(.Nodes("Services") Is Nothing, False, Not .Nodes("Services").Checked))
                                    Else
                                        If Not (.Nodes("MessageTypes") Is Nothing) Then
                                            .Nodes("MessageTypes").Checked = srow.MessageTypes
                                        End If
                                        If Not (.Nodes("Priorities") Is Nothing) Then
                                            .Nodes("Priorities").Checked = srow.Priorities
                                        End If
                                        If Not (.Nodes("Queues") Is Nothing) Then
                                            .Nodes("Queues").Checked = srow.Queues
                                        End If
                                        If Not (.Nodes("RemoteServiceBindings") Is Nothing) Then
                                            .Nodes("RemoteServiceBindings").Checked = srow.RemoteServiceBindings
                                        End If
                                        If Not (.Nodes("Routes") Is Nothing) Then
                                            .Nodes("Routes").Checked = srow.Routes
                                        End If
                                        If Not (.Nodes("ServiceContracts") Is Nothing) Then
                                            .Nodes("ServiceContracts").Checked = srow.ServiceContracts
                                        End If
                                        If Not (.Nodes("Services") Is Nothing) Then
                                            .Nodes("Services").Checked = srow.Services
                                        End If
                                    End If
                                End With
                            End If
                        End If
                    Next
                End If
                smoTreeView.EndUpdate()
                smoTreeView_NotDirty()
            End If
        Catch ex As Exception
            Throw New Exception(String.Format("({0}.LoadTreeFromDataSet) Exception.", Me.Name), ex)
        End Try
    End Sub

    Private Sub InitTreeFromTargetInstance()
        Try
            smoInitTreeView()
            My.Application.DoEvents()
            ' select node 0 to populate now
            smoTreeView.SelectedNode = smoTreeView.Nodes(0)
            ' if the selected instance is not currently configured
            ' configure it with the default settings
            Dim ConfiguredInstanceList() As String = Mother.DAL.GetConfiguredInstanceList()
            Dim bFound As Boolean = False
            For Each iName As String In ConfiguredInstanceList
                ' should always be upper case if the UI has been used
                If UCase(iName) = UCase(InstanceName) Then
                    bFound = True
                    Exit For
                End If
            Next
            ' nothing below should happen of the host is standing in for an unreachable target
            If IsConnectedToTarget Then

                If Not (bFound) Then
                    ' add it now, will not count against the license  until it gets
                    ' change rows that are not metadata changes (actual use)
                    Mother.DAL.InitNewTargetInstance(InstanceName, _
                                              CInt(TargetInstance.Information.EngineEdition), _
                                              TargetInstance.Information.Version.Major)
                End If
                ' coming through on the delete of a nonconnected instance if chokes on nonexistant db collection node
                If Not smoTreeView.Nodes.Item(0).Nodes("Databases") Is Nothing Then
                    My.Application.DoEvents()
                    ' get a list of DBs in the repository
                    Dim DbList As String() = Mother.DAL.GetItemList(smoTreeView.Nodes.Item(0).Nodes("Databases").FullPath)
                    ' compare it to the smo list looking for new databases
                    For Each n As TreeNode In smoTreeView.Nodes.Item(0).Nodes("Databases").Nodes
                        If DbList.Length = 0 OrElse Array.BinarySearch(DbList, n.Name) < 0 Then
                            Mother.DAL.InitNewTargetDatabase(InstanceName, _
                                                      n.Name, _
                                                      TargetInstance.Information.Version.Major)
                        End If
                    Next
                    ' compare it to the smo list looking for databases no longer on server
                    For Each n As TreeNode In smoTreeView.Nodes.Item(0).Nodes("Databases").Nodes
                        My.Application.DoEvents()
                        If Not (Array.BinarySearch(DbList, n.Name) < 0) Then
                            Mother.DAL.InitNewTargetDatabase(InstanceName, _
                                                      n.Name, _
                                                      TargetInstance.Information.Version.Major)
                        End If
                    Next
                End If
            End If
        Catch ex As Exception
            Throw New Exception(String.Format("({0}.InitTreeFromTargetInstance) Exception.", Me.Name), ex)
        End Try
    End Sub

    Private Sub smoInitTreeView()
        Try
            smoTreeView.Nodes.Clear()
            'a work table of the latest changes for a server
            dtRepositoryInstance = Mother.DAL.GetHierarchyNodesBySQLInstance(InstanceName)
            If dtRepositoryInstance.Select(String.Format("TreeViewNodePath='{0}'", InstanceName)).Length > 0 Then
                TargetInstance.UserData = dtRepositoryInstance.Select(String.Format("TreeViewNodePath='{0}'", InstanceName))(0).Item("ChangeId")
            End If
            ' add the root node
            smoTreeView.Nodes.Add(cTreeView.CreateNode(String.Format("{0}", InstanceName), _
                                                       TargetInstance))
            My.Application.DoEvents()
            smoTreeView.Sort()
            My.Application.DoEvents()
            smoTreeView.Nodes(0).ExpandAll()
            smoTreeView.Nodes(0).Checked = True
            smoTreeView.Nodes(0).ContextMenuStrip = ContextMenuStripTreeNode
        Catch ex As Exception
            Throw New Exception(String.Format("({0}.smoInitTreeView) Exception.", Me.Name), ex)
        End Try
    End Sub

    Private Sub smoTreeView_AfterCheck(ByVal sender As Object, _
                                       ByVal e As System.Windows.Forms.TreeViewEventArgs) _
                                       Handles smoTreeView.AfterCheck
        Try
            If Not (TreeHasBeenChanged) Then
                'mark it as changed now
                smoTreeView_Dirty()
            End If
            If Not e.Node.Parent Is Nothing Then
                If (e.Node.Parent.Name = "Databases" _
                    Or e.Node.Name = "JobServer" _
                    Or e.Node.Name = "ServiceBroker") Then
                    ' SubTypes cascade the check
                    For Each n As TreeNode In e.Node.Nodes
                        n.Checked = e.Node.Checked
                    Next
                End If
            End If
        Catch ex As Exception
            Mother.HandleException(New ApplicationException(String.Format("({0}.smoTreeView_AfterCheck) Exception.", Me.Name), ex))
        End Try
    End Sub

    Private Sub smoTreeView_BeforeSelect(ByVal sender As Object, _
                                         ByVal e As System.Windows.Forms.TreeViewCancelEventArgs) _
                                         Handles smoTreeView.BeforeSelect
        Try
            e.Node.Checked = Not (e.Node.Checked)
            e.Node.TreeView.Nodes.Item(0).Checked = True
            ' toggle the child nodes to the same state as the selected node
            If Not (e.Node.IsExpanded) Then
                ToggleChildNodes(e.Node)
            End If
            e.Cancel = True
        Catch ex As Exception
            Mother.HandleException(New ApplicationException(String.Format("({0}.smoTreeView_BeforeSelect) Exception.", Me.Name), ex))
        End Try
    End Sub

    Private Sub ToggleChildNodes(ByVal parent As TreeNode)
        Try
            For Each child As TreeNode In parent.Nodes
                Dim save As Color = child.BackColor
                child.BackColor = child.ForeColor
                child.ForeColor = save
                child.Checked = parent.Checked
                ToggleChildNodes(child)
            Next
        Catch ex As Exception
            Mother.HandleException(New ApplicationException(String.Format("({0}.ToggleChildNodes) Exception.", Me.Name), ex))
        End Try
    End Sub

    Private Sub smoTreeView_BeforeExpand(ByVal sender As Object, _
                                         ByVal e As System.Windows.Forms.TreeViewCancelEventArgs) _
                                         Handles smoTreeView.BeforeExpand
        Dim csr As Cursor = Nothing
        Try
            csr = Me.Cursor   ' Save the old cursor
            Me.Cursor = Cursors.WaitCursor   ' Display the waiting cursor
            cTv.DrillDown(My.Resources.OriginArchiveTarget, e.Node)
        Catch ex As Exception
            Mother.HandleException(New ApplicationException(String.Format("({0}.smoTreeView_BeforeExpand) Exception.", Me.Name), ex))
        Finally
            Me.Cursor = csr ' restore the original cursor
        End Try
    End Sub

    ' clipped from here

#End Region

#Region " schedule "

    Private Sub DataGridViewArchiveSchedules_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DataGridViewArchiveSchedules.CellClick
        Dim csr As Cursor = Me.Cursor
        Try
            Me.Cursor = Cursors.WaitCursor   ' Display the waiting cursor
            Select Case e.RowIndex
                Case -1
                    If e.ColumnIndex = -1 Then
                        LoadSchedule()
                    End If
                Case Else
                    ' do nothing if the header row is clicked
                    If e.RowIndex > -1 Then
                        'work with the selected row - only if connected (is instance name) 
                        If InstanceName Is Nothing Then
                            ToolStripStatusLabelConfiguration.Text = My.Resources.ArchivePlanningRequestInvalid
                        Else
                            Select Case e.ColumnIndex
                                Case -1 ' row header
                                    If DataGridViewArchiveSchedules.Rows(e.RowIndex).IsNewRow Then
                                        With DataGridViewArchiveSchedules.Rows(e.RowIndex)
                                            ' adds new in content event below too :(
                                            .Cells("DataGridViewTextBoxColumnArchiveScheduleInstanceName").Value = InstanceName
                                            .Cells("DataGridViewButtonColumnArchiveScheduleIntervalType").Value = DateInterval.Day.ToString
                                            .Cells("DataGridViewButtonColumnArchiveScheduleInterval").Value = 1
                                            .Cells("DataGridViewButtonColumnArchiveScheduleIntervalBaseDt").Value = DateAdd(DateInterval.Hour, _
                                                                                                                    DatePart(DateInterval.Hour, Now) + 1, _
                                                                                                                    Today)
                                            .Cells("DataGridViewCheckBoxColumnArchiveScheduleIsActive").Value = True
                                            .Cells("DataGridViewCheckBoxColumnArchiveScheduleUseEventNotifications").Value = False
                                        End With
                                    Else
                                        ' row header click is a request to delete?
                                        If MessageBox.Show("Delete This Schedule?", _
                                                           "Confirm Change", _
                                                           MessageBoxButtons.YesNo, _
                                                           MessageBoxIcon.Question, _
                                                           MessageBoxDefaultButton.Button2, _
                                                           0, _
                                                           False) = DialogResult.Yes Then
                                            'need to remove notifications on the target server too if checked
                                            If DataGridViewArchiveSchedules.Rows(e.RowIndex).Cells("DataGridViewCheckBoxColumnArchiveScheduleUseEventNotifications").Value = True Then
                                                If DisableEventNotifications() Then
                                                    DataGridViewArchiveSchedules.Rows(e.RowIndex).Cells("DataGridViewCheckBoxColumnArchiveScheduleUseEventNotifications").Value = False
                                                End If
                                            End If
                                            Try
                                                DataGridViewArchiveSchedules.Rows.RemoveAt(e.RowIndex)
                                                'remove the SQLClue service account from the target if there 
                                                'are no archive or trace schedules left for the target
                                                If Mother.DAL.GetScheduleCountForInstance(InstanceName) = 0 _
                                                AndAlso TargetInstance.Logins.Contains(Mother.SQLClueServiceAccount) Then
                                                    TargetInstance.Logins(Mother.SQLClueServiceAccount).Drop()
                                                End If
                                            Catch ex As InvalidOperationException
                                                DataGridViewArchiveSchedules.Rows.Item(e.RowIndex).Dispose()
                                            End Try
                                            ScheduleHasBeenChanged = True
                                            LoadPlan()
                                        End If
                                    End If
                            End Select
                        End If
                    End If
            End Select
        Catch ex As Exception
            Mother.HandleException(New ApplicationException(String.Format("({0}.DataGridViewArchiveSchedules_CellClick) Exception.", Me.Name), ex))
        Finally
            Me.Cursor = csr ' restore the original cursor
            Me.Enabled = True
        End Try

    End Sub


    Private Sub DataGridViewArchiveSchedules_CellContentClick(ByVal sender As Object, _
                                                       ByVal e As DataGridViewCellEventArgs) _
                                                       Handles DataGridViewArchiveSchedules.CellContentClick
        ' I don't think the event responds to -1 (header) events so added cellclick handler above
        ' one problem with doing eveything in the cell-click is that the checkboxes are too easy to toggle on accident
        ' here the check box must be hit not just the cell containing the checkbox
        Dim csr As Cursor = Me.Cursor
        Try
            Me.Cursor = Cursors.WaitCursor   ' Display the waiting cursor
            Select Case e.RowIndex
                Case -1
                    If e.ColumnIndex = -1 Then
                        LoadSchedule()
                    End If
                Case Else
                    If DataGridViewArchiveSchedules.Rows(e.RowIndex).IsNewRow Then
                        With DataGridViewArchiveSchedules.Rows(e.RowIndex)
                            'adds in cell event above too :(
                            .Cells("DataGridViewTextBoxColumnArchiveScheduleInstanceName").Value = InstanceName
                            .Cells("DataGridViewButtonColumnArchiveScheduleIntervalType").Value = DateInterval.Day.ToString
                            .Cells("DataGridViewButtonColumnArchiveScheduleInterval").Value = 1
                            .Cells("DataGridViewButtonColumnArchiveScheduleIntervalBaseDt").Value = DateAdd(DateInterval.Hour, _
                                                                                                    DatePart(DateInterval.Hour, Now) + 1, _
                                                                                                    Today)
                            .Cells("DataGridViewCheckBoxColumnArchiveScheduleIsActive").Value = True
                            .Cells("DataGridViewCheckBoxColumnArchiveScheduleUseEventNotifications").Value = False
                        End With
                        If InstanceName <> "" Then
                            ScheduleHasBeenChanged = True
                            LoadPlan()
                            'make sure the SQLClue service account can script this target
                            If Not Mother.SQLClueServiceAccount Is Nothing _
                            AndAlso Not Mother.SQLClueServiceAccount = "" _
                            AndAlso Not TargetInstance.Logins.Contains(Mother.SQLClueServiceAccount) Then
                                Try
                                    'checks and skips if SQL Authentication
                                    Mother.DAL.AddSQLClueServiceAccountToTarget(Mother.SQLClueServiceAccount, _
                                                                                InstanceName, _
                                                                                My.Settings.TargetHandshakeConnectionTimeout)
                                Catch ex As Exception
                                    Mother.HandleException(New Exception(String.Format("Failed to add service account on target instance [{0}]", InstanceName), ex))
                                End Try
                            End If
                        End If
                    Else
                        ' do nothing if the header row is clicked
                        If e.RowIndex > -1 Then
                            'work with the selected row - only if connected (is instance name) 
                            If InstanceName Is Nothing Then
                                ToolStripStatusLabelConfiguration.Text = My.Resources.ArchivePlanningRequestInvalid
                            Else
                                Select Case e.ColumnIndex
                                    Case -1 ' row header
                                        ' its in the cell_click handler
                                    Case 0, 1 ' Id, name
                                        'cannot be selected
                                    Case 2, 3 'Interval+Type
                                        Dim freq As RunFrequencySelector = New RunFrequencySelector
                                        If TypeOf DataGridViewArchiveSchedules.Rows(e.RowIndex).Cells(2).Value Is System.DBNull Then
                                            freq.ComboBox1.DroppedDown = True
                                        Else
                                            freq.ComboBox1.Text = DataGridViewArchiveSchedules.Rows(e.RowIndex).Cells(2).Value.ToString
                                            freq.ComboBox1.Tag = DataGridViewArchiveSchedules.Rows(e.RowIndex).Cells(2).Value.ToString
                                        End If
                                        If TypeOf DataGridViewArchiveSchedules.Rows(e.RowIndex).Cells(3).Value Is System.DBNull Then
                                            freq.NumericUpDown1.Value = 1
                                        Else
                                            freq.NumericUpDown1.Value = CInt(DataGridViewArchiveSchedules.Rows(e.RowIndex).Cells(3).Value)
                                            freq.NumericUpDown1.Tag = CInt(DataGridViewArchiveSchedules.Rows(e.RowIndex).Cells(3).Value)
                                        End If
                                        freq.ShowDialog(Me)
                                        If freq.DialogResult = Windows.Forms.DialogResult.OK Then
                                            ' if anything changed set the dataset dirty
                                            If Not CStr(DataGridViewArchiveSchedules.Rows(e.RowIndex).Cells(2).Value) = CStr(freq.ComboBox1.Tag) _
                                            Or Not CInt(DataGridViewArchiveSchedules.Rows(e.RowIndex).Cells(3).Value) = CInt(freq.NumericUpDown1.Tag) Then
                                                DataGridViewArchiveSchedules.Rows(e.RowIndex).Cells(2).Value = CStr(freq.ComboBox1.Tag)
                                                DataGridViewArchiveSchedules.Rows(e.RowIndex).Cells(3).Value = CInt(freq.NumericUpDown1.Tag)
                                                ScheduleHasBeenChanged = True
                                            End If
                                        End If
                                    Case 4 ' IntervalDt (nerd) NExt Run Date
                                        Dim pick As DialogDateTimePicker = New DialogDateTimePicker
                                        pick.Tag = DataGridViewArchiveSchedules.Rows(e.RowIndex).Cells(e.ColumnIndex).Value
                                        If TypeOf DataGridViewArchiveSchedules.Rows(e.RowIndex).Cells(e.ColumnIndex).Value Is System.DBNull Then
                                            DataGridViewArchiveSchedules.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = Now
                                        End If
                                        pick.ShowDialog(CDate(DataGridViewArchiveSchedules.Rows(e.RowIndex).Cells(e.ColumnIndex).Value), Me)
                                        If pick.DialogResult = Windows.Forms.DialogResult.OK Then
                                            If Not (CDate(DataGridViewArchiveSchedules.Rows(e.RowIndex).Cells(e.ColumnIndex).Value) = CDate(pick.Tag)) Then
                                                DataGridViewArchiveSchedules.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = pick.Tag
                                                ScheduleHasBeenChanged = True
                                            End If
                                        End If
                                    Case 5 ' IsActive or Enabled
                                        DataGridViewArchiveSchedules.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = Not (DataGridViewArchiveSchedules.Rows(e.RowIndex).Cells(e.ColumnIndex).Value)
                                        ScheduleHasBeenChanged = True
                                    Case 6 ' Run Now
                                        Me.Enabled = False
                                        StartArchive(e.RowIndex)
                                    Case 7 ' UseEventNotifications
                                        Me.Enabled = False
                                        If DataGridViewArchiveSchedules.Rows(e.RowIndex).Cells("DataGridViewCheckBoxColumnArchiveScheduleUseEventNotifications").Value = True Then
                                            ' not for SQL 2000
                                            ' connect to the target instance
                                            If ConnectToTarget(DataGridViewArchiveSchedules.Rows(e.RowIndex).Cells("DataGridViewTextBoxColumnArchiveScheduleInstanceName").Value.ToString, _
                                                               "Target Event Notification Configuration") = Windows.Forms.DialogResult.OK Then
                                                If TargetInstance.Information.Version.Major > 8 Then
                                                    Dim cell As DataGridViewImageCell = _
                                                         CType(DataGridViewArchiveSchedules.Rows(e.RowIndex).Cells( _
                                                               "DataGridViewImageColumnArchiveScheduleViewQueue"), DataGridViewImageCell)
                                                    If EnableEventNotifications() Then
                                                        DataGridViewArchiveSchedules.Rows(e.RowIndex).Cells("DataGridViewCheckBoxColumnArchiveScheduleUseEventNotifications").Value = True
                                                        DataGridViewArchiveSchedules.Rows(e.RowIndex).Cells("DataGridViewImageColumnArchiveScheduleViewQueue").Value = My.Resources.FormRunHS
                                                    Else
                                                        DataGridViewArchiveSchedules.Rows(e.RowIndex).Cells("DataGridViewCheckBoxColumnArchiveScheduleUseEventNotifications").Value = False
                                                        DataGridViewArchiveSchedules.Rows(e.RowIndex).Cells("DataGridViewImageColumnArchiveScheduleViewQueue").Value = Nothing
                                                    End If
                                                Else
                                                    ToolStripStatusLabelConfiguration.Text = _
                                                    String.Format(My.Resources.EventNotificationsNotSupported, _
                                                                  TargetInstance.ConnectionContext.TrueName, _
                                                                  TargetInstance.Information.VersionString)
                                                    DataGridViewArchiveSchedules.Rows(e.RowIndex).Cells("DataGridViewCheckBoxColumnArchiveScheduleUseEventNotifications").Value = False
                                                    DataGridViewArchiveSchedules.Rows(e.RowIndex).Cells("DataGridViewImageColumnArchiveScheduleViewQueue").Value = Nothing
                                                End If
                                            End If
                                        Else
                                            'What if there are still items in the queue?
                                            If MessageBox.Show(String.Format("Any changes currently queued on SQL Instance {0} will not be " & vbCrLf & _
                                                                             "archived if Event Notifications are disabled. Select [OK] to disable the " & vbCrLf & _
                                                                             "queue discarding any queued changes. Select [Cancel] to return to the " & vbCrLf & _
                                                                             "Configuration Form and view the queue.", InstanceName), _
                                                                             "Remove Event Notifications?", MessageBoxButtons.OKCancel) = Windows.Forms.DialogResult.OK Then
                                                If DisableEventNotifications() Then
                                                    DataGridViewArchiveSchedules.Rows(e.RowIndex).Cells("DataGridViewCheckBoxColumnArchiveScheduleUseEventNotifications").Value = False
                                                    DataGridViewArchiveSchedules.Rows(e.RowIndex).Cells("DataGridViewImageColumnArchiveScheduleViewQueue").Value = Nothing
                                                Else
                                                    DataGridViewArchiveSchedules.Rows(e.RowIndex).Cells("DataGridViewCheckBoxColumnArchiveScheduleUseEventNotifications").Value = True
                                                    DataGridViewArchiveSchedules.Rows(e.RowIndex).Cells("DataGridViewImageColumnArchiveScheduleViewQueue").Value = My.Resources.FormRunHS
                                                End If
                                            Else
                                                DataGridViewArchiveSchedules.Rows(e.RowIndex).Cells("DataGridViewCheckBoxColumnArchiveScheduleUseEventNotifications").Value = True
                                                DataGridViewArchiveSchedules.Rows(e.RowIndex).Cells("DataGridViewImageColumnArchiveScheduleViewQueue").Value = My.Resources.FormRunHS
                                            End If
                                        End If
                                        ScheduleHasBeenChanged = True
                                    Case 8 ' view queue
                                        If DataGridViewArchiveSchedules.Rows(e.RowIndex).Cells("DataGridViewCheckBoxColumnArchiveScheduleUseEventNotifications").Value = True Then
                                            ' use the current connection info
                                            DialogViewQueue.ShowDialog(Me)
                                        End If
                                        ' not dirty, read only, don't reload
                                        Exit Try
                                End Select
                                'ScheduleHasBeenChanged = True
                                LoadPlan()
                            End If
                            If TypeOf DataGridViewArchiveSchedules.Rows(e.RowIndex).Cells(1).Value Is System.DBNull Then
                                DataGridViewArchiveSchedules.Rows(e.RowIndex).Cells(1).Value = InstanceName
                            End If
                        End If
                    End If ' row > -1
            End Select
        Catch ex As Exception
            Mother.HandleException(New ApplicationException(String.Format("({0}.DataGridViewArchiveSchedules_CellClick) Exception.", Me.Name), ex))
        Finally
            Me.Cursor = csr ' restore the original cursor
            Me.Enabled = True
        End Try
    End Sub

    Private Sub StartArchive(ByVal ScheduleRowId As Integer)
        Try
            If Not ScheduleHasBeenChanged Then
                Dim tryrow As DataRow = Mother.DAL.dsSQLCfg.tInstance.FindByName(InstanceName)
                ' could tryrow ever be nothing? think RunNow is not available if not already usings the Instance's data
                If tryrow Is Nothing Then
                    Throw New Exception(String.Format("No SQLClue Archives Found for SQL Server [{0}]", _
                                                                 InstanceName))
                Else
                    ArchiveCancelOn()
                    'save any pending changes else we will loose them below
                    If ScheduleHasBeenChanged Or TreeHasBeenChanged Then
                        LoadPlan()
                    End If
                    cArchive1 = New cCommon.cArchive
                    RemoveHandler cArchive1.Archiving, AddressOf ProgressEventHandler
                    RemoveHandler cArchive1.ArchivingItem, AddressOf ProgressDetailEventHandler
                    RemoveHandler cArchive1.ArchivingException, AddressOf ArchiveDoWorkExceptionHandler
                    RemoveHandler cArchive1.PercentDone, AddressOf PercentDoneEventHandler
                    AddHandler cArchive1.Archiving, AddressOf ProgressEventHandler
                    AddHandler cArchive1.ArchivingItem, AddressOf ProgressDetailEventHandler
                    AddHandler cArchive1.ArchivingException, AddressOf ArchiveDoWorkExceptionHandler
                    AddHandler cArchive1.PercentDone, AddressOf PercentDoneEventHandler
                    ' archive using the settings associated with this row
                    ' the called module will always create a new cCompare and reload the dataset
                    ' data changes will be picked up here even while the service is running
                    'stopping an Archive is not good but can be long running, may need it
                    ArchiveIsRunning = True
                    ' feed the connection info to the async task once the task class is instantiated
                    cArchive1.RepositoryConnectionTimeout = My.Settings.RepositoryConnectionTimeout
                    cArchive1.RepositoryDatabaseName = My.Settings.RepositoryDatabaseName
                    cArchive1.RepositoryInstanceName = My.Settings.RepositoryInstanceName
                    cArchive1.RepositoryUseTrustedConnection = My.Settings.RepositoryUseTrustedConnection
                    cArchive1.RepositorySQLLoginName = My.Settings.RepositorySQLLoginName
                    cArchive1.RepositorySQLLoginPassword = My.Settings.RepositorySQLLoginPassword
                    cArchive1.RepositoryEncryptConnection = My.Settings.RepositoryEncryptConnection
                    cArchive1.RepositoryTrustServerCertificate = My.Settings.RepositoryTrustServerCertificate
                    cArchive1.RepositoryNetworkLibrary = My.Settings.RepositoryNetworkLibrary
                    cArchive1.ArchiveCancelled = String.Format(My.Resources.ArchiveCancelled, _
                                                              DataGridViewArchiveSchedules.Rows(ScheduleRowId).Cells(0).Value)
                    cArchive1.ArchiveComplete = String.Format(My.Resources.ArchiveComplete, _
                                                              DataGridViewArchiveSchedules.Rows(ScheduleRowId).Cells(0).Value)
                    cArchive1.ArchiveRescheduled = My.Resources.ArchiveRescheduled
                    cArchive1.HandshakeConnectionTimeOut = My.Settings.TargetHandshakeConnectionTimeout
                    cArchive1.AsyncArchive(CInt(DataGridViewArchiveSchedules.Rows(ScheduleRowId).Cells(0).Value))
                End If
            End If
        Catch ex As Exception
            Mother.HandleException(New ApplicationException(String.Format("({0}.StartArchive) Exception.", Me.Name), ex))
        End Try
    End Sub

    Private Sub ButtonCancelArchive_Click(ByVal sender As System.Object, _
                                          ByVal e As System.EventArgs) _
                                          Handles ButtonCancelArchive.Click
        Try
            cArchive1.CancelAsyncArchive()
            ButtonCancelArchive.Text = "Cancel Pending"
            ButtonCancelArchive.Enabled = False
        Catch ex As Exception
            Mother.HandleException(New ApplicationException(String.Format("({0}.ButtonCancelArchive_Click) Exception.", Me.Name), ex))
        End Try
    End Sub

    Private Sub ArchiveCancelOn()
        ButtonCancelArchive.Text = "Cancel Archive"
        ButtonCancelArchive.Visible = True
        ButtonCancelArchive.Enabled = True
        Me.CancelButton = ButtonCancelArchive
        ToolStripProgressBarArchive.Value = 0
        ToolStripProgressBarArchive.Visible = True
        ButtonSave.Visible = False
        ButtonCancel.Visible = False
        DataGridViewConnections.Enabled = False
        DataGridViewArchiveSchedules.Enabled = False
        smoTreeView.Enabled = False
        My.Application.DoEvents()
    End Sub

    Private Sub ArchiveCancelOff()
        ButtonCancelArchive.Text = "Cancel Archive"
        ButtonCancelArchive.Visible = False
        ButtonCancelArchive.Enabled = False
        Me.CancelButton = ButtonCancel
        ToolStripProgressBarArchive.Value = 0
        ToolStripProgressBarArchive.Visible = False
        ButtonSave.Visible = True
        ButtonCancel.Visible = True
        DataGridViewConnections.Enabled = True
        DataGridViewArchiveSchedules.Enabled = True
        smoTreeView.Enabled = True
        My.Application.DoEvents()
    End Sub

    Private Sub PercentDoneEventHandler(ByVal PercentDone As Int32)
        Try
            ToolStripProgressBarArchive.Value = PercentDone
            ToolStripProgressBarArchive.ToolTipText = String.Format("{0}%", PercentDone)
            My.Application.DoEvents()
        Catch ex As Exception
            Mother.HandleException(New ApplicationException(String.Format("({0}.PercentDoneEventHandler) Exception.", Me.Name), ex))
        End Try
    End Sub

    Private Sub ProgressEventHandler(ByVal Item As String)
        Try
            If Me.InvokeRequired Then
                Me.Invoke(New ProgressEventHandlerDelegate(AddressOf ProgressEventHandler), Item)
            Else
                If Item = cArchive1.ArchiveComplete Then
                    ArchiveIsRunning = False
                    ArchiveCancelOff()
                    ' need a full refresh of the display
                    ScheduleHasBeenChanged = True
                    TreeHasBeenChanged = True
                    LoadPlan()
                End If
                If Item = cArchive1.ArchiveRescheduled Then
                    LoadSchedule()
                End If
                CurrentCollection = Item
                ToolStripStatusLabelConfiguration.Text = Item
                My.Application.DoEvents()
            End If
        Catch ex As Exception
            Mother.HandleException(New ApplicationException(String.Format("({0}.ProgressEventHandler) Exception.", Me.Name), ex))
        End Try
    End Sub

    Private Sub ProgressDetailEventHandler(ByVal Item As String)
        Try
            If Me.InvokeRequired Then
                Me.Invoke(New ProgressDetailEventHandlerDelegate(AddressOf ProgressDetailEventHandler), Item)
            Else
                ToolStripStatusLabelConfiguration.Text = CurrentCollection & "." & Item
                My.Application.DoEvents()
            End If
        Catch ex As Exception
            Mother.HandleException(New ApplicationException(String.Format("({0}.ProgressDetailEventHandler) Exception.", Me.Name), ex))
        End Try
    End Sub

    Private Sub ArchiveDoWorkExceptionHandler(ByVal ArchiveException As Exception)
        Try
            If Me.InvokeRequired Then
                Me.Invoke(New ArchiveDoWorkExceptionHandlerDelegate(AddressOf ArchiveDoWorkExceptionHandler), ArchiveException)
            Else
                Dim txt As String = "{0} {1}" & vbCrLf & _
                "This Archive Operation exception is written to the Application Event Log and " & _
                "processing of the archive will continue when the OK buttion is selected below " & _
                "in most scenarios. In rare cases the exception may prove fatal to archive process. " & _
                "When an exception is encountered by the SQLClue Automation Controller Service no " & _
                "message is displayed to the screen. It is only written to the Application Event Log. " & _
                "This is by desogn, The message is displayed now to help in troubleshooting unexpected archive problems."
                Mother.HandleException(New ApplicationException(String.Format(txt, _
                                                                              Me.Name, _
                                                                              ArchiveException.GetType.ToString), ArchiveException))
                My.Application.DoEvents()
            End If
        Catch ex As Exception
            Mother.HandleException(New ApplicationException(String.Format("({0}.ArchiveDoWorkExceptionHandler) Exception.", Me.Name), ex))
        End Try
    End Sub

    Public Function EnableEventNotifications() As Boolean

        EnableEventNotifications = False
        Try
            If IsConnectedToTarget Then
                Dim r As DialogResult = _
                MessageBox.Show(String.Format("It is recommended that a full Archive is completed at the time." & vbCrLf & _
                                              "" & vbCrLf & _
                                              "Until a full Archive - one with [Use Events] not checked - is processed the" & vbCrLf & _
                                              "Configuration Archive will remain incomplete. A full Archive can be used to " & vbCrLf & _
                                              "syncronize the SQLClue Archive data store with the Target SQL Instance at" & vbCrLf & _
                                              "any time. This allows the synchronizing non-event based archive to be run" & vbCrLf & _
                                              "during off peak or unattended hours if desired." & vbCrLf & _
                                              "" & vbCrLf & _
                                              "User data changes are acceptable on the server while the full Archive is" & vbCrLf & _
                                              "active. In most scenarios this allows applications to run concurrently with" & vbCrLf & _
                                              "the full Archive." & vbCrLf & _
                                              "" & vbCrLf & _
                                              "Any full Archive may be a long running activity and may have a small" & vbCrLf & _
                                              "performance impact at the Target SQL Instance while active." & vbCrLf & _
                                              "" & vbCrLf & _
                                              "A few configuration changes are not detected by events, SQLClue will check" & vbCrLf & _
                                              "these settings and properties in full, even during an event based Archive." & vbCrLf & _
                                              "This assures optimal concurrency between the Archive data store and the" & vbCrLf & _
                                              "Target SQL Instance." & vbCrLf & _
                                              "" & vbCrLf & _
                                              "    [Yes] - to complete the full archive at this time and [Use Events]." & vbCrLf & _
                                              "    [No] - to skip the full archive at this time and [Use Events] anyway." & vbCrLf & _
                                              "    [Cancel] - to terminate the request to [Use Events] at this time.", _
                                                 InstanceName), _
                                   "Synchronize Archive With Target Instance Now?", _
                                   MessageBoxButtons.YesNoCancel)
                If r = Windows.Forms.DialogResult.Yes Then
                    StartArchive(DataGridViewArchiveSchedules.CurrentRow.Index)
                ElseIf r = Windows.Forms.DialogResult.Cancel Then
                    Exit Try
                End If
                ToolStripStatusLabelConfiguration.Text = String.Format(My.Resources.EventNotificationStateChanging, _
                                                           My.Resources.Enabled, _
                                                           InstanceName)
                StatusStripPlanning.Refresh()
                'add the notification config from the server
                If Not Mother.DAL.RunSQLScript(TargetInstance, _
                                 My.Settings.TargetEventNotificationDatabase, _
                                 My.Settings.instDDLEventNotifications) Then
                    Throw New Exception(String.Format("Script [{0}] execution returned FALSE" & vbCrLf & _
                                                                 "for SQL Instance [{1}].", _
                                                   My.Settings.instDDLEventNotifications, InstanceName))
                End If
                EnableEventNotifications = True
            End If
        Catch ex As ApplicationException
            Throw New Exception(String.Format("({0}.EnableEventNotifications) Exception.", Me.Name), ex)
        End Try
    End Function

    Public Function DisableEventNotifications() As Boolean
        DisableEventNotifications = False
        Try
            If IsConnectedToTarget Then
                ToolStripStatusLabelConfiguration.Text = String.Format(My.Resources.EventNotificationStateChanging, _
                                                           My.Resources.Disabled, _
                                                           InstanceName)
                StatusStripPlanning.Refresh()
                'remove the notification config from the server
                If Not Mother.DAL.RunSQLScript(TargetInstance, _
                                        My.Settings.TargetEventNotificationDatabase, _
                                        My.Settings.uninstDDLEventNotifications) Then
                    Throw New Exception(String.Format("Script [{0}] execution returned FALSE" & vbCrLf & _
                                                                 "for SQL Instance [{1}].", _
                                                   My.Settings.uninstDDLEventNotifications, InstanceName))
                End If
                DisableEventNotifications = True
            Else
                Mother.HandleException(New Exception(String.Format(My.Resources.EvtNotifyDropFailed, _
                                                     InstanceName, _
                                                     My.Settings.TargetEventNotificationDatabase)))
            End If
        Catch ex As ApplicationException
            Throw New Exception(String.Format("({0}.DisableEventNotifications) Failed", Me.Name), ex)
        End Try
    End Function

    Public Sub RemoveAllEventNotifications()
        Try
            'fall through the schedule rows and remove config on target
            If Mother.DAL.dsSQLCfg.tSchedule.Rows.Count > 0 Then
                For Each r As cCommon.dsSQLConfiguration.tScheduleRow In Mother.DAL.dsSQLCfg.tSchedule.Rows
                    If r.UseEventNotifications Then
                        ToolStripStatusLabelConfiguration.Text = String.Format(My.Resources.EventNotificationsDisable, _
                                                                               r.InstanceName)
                        If ConnectToTarget(r.InstanceName, "Remove Event Notification Configuration") = Windows.Forms.DialogResult.OK Then
                            DisableEventNotifications()
                        Else
                            Mother.HandleException(New Exception(String.Format(My.Resources.EvtNotifyDropFailed, _
                                                                 InstanceName, _
                                                                 My.Settings.TargetEventNotificationDatabase)))
                        End If
                    End If
                Next
            End If
        Catch ex As Exception
            Throw New Exception(String.Format("({0}.RemoveAllEventNotifications) Exception.", Me.Name), ex)
        End Try
    End Sub

#End Region

#Region " show the dirt "

    Private Sub DataGridViewArchiveSchedules_CurrentCellDirtyStateChanged(ByVal sender As Object, _
                                                                          ByVal e As System.EventArgs) _
                                                                          Handles DataGridViewArchiveSchedules.CurrentCellDirtyStateChanged
        Try
            If Not ScheduleHasBeenChanged Then
                'ScheduleHasBeenChanged = True
                ' schedule changes are immediately saved to db so no need for this
                'Dim oldback As Color = SplitContainerVertical.Panel2.BackColor
                'SplitContainerVertical.Panel2.BackColor = SplitContainerVertical.Panel1.ForeColor
                'SplitContainerVertical.Panel2.ForeColor = oldback
                'ToolStripStatusLabelConfiguration.Text = My.Resources.Dirty
            End If
        Catch ex As Exception
            Mother.HandleException(New ApplicationException(String.Format("({0}.DataGridViewArchiveSchedules_CurrentCellDirtyStateChanged) Exception.", Me.Name), ex))
        End Try
    End Sub

    Private Sub smoTreeView_Dirty()
        Try
            ' called when a checkbox is clicked or when node text is clicked
            TreeHasBeenChanged = True
            SplitContainer1.Panel2.BackColor = SplitContainer1.Panel1.ForeColor
            SplitContainer1.Panel2.ForeColor = SplitContainer1.Panel1.BackColor
            ToolStripStatusLabelConfiguration.Text = My.Resources.Dirty
        Catch ex As ApplicationException
            Throw New Exception(String.Format("({0}.smoTreeView_Dirty) Exception.", Me.Name), ex)
        End Try
    End Sub

    Private Sub smoTreeView_NotDirty()

        Try
            TreeHasBeenChanged = False
            SplitContainer1.Panel2.BackColor = SplitContainer1.Panel1.BackColor
            SplitContainer1.Panel2.ForeColor = SplitContainer1.Panel1.ForeColor
            ToolStripStatusLabelConfiguration.Text = ""
        Catch ex As ApplicationException
            Throw New Exception(String.Format("({0}.smoTreeView_NotDirty) Exception.", Me.Name), ex)
        End Try
    End Sub

#End Region

    Private Sub ToolStripMenuItemLabelItem_Click(ByVal sender As Object, _
                                                 ByVal e As System.EventArgs) _
                                                 Handles ToolStripMenuItemLabelItem.Click
        Try
            MessageBox.Show("apply label stub pSQLCfgChangeApplyLabel")
        Catch ex As Exception
            Mother.HandleException(New ApplicationException(String.Format("({0}.ToolStripMenuItemLabelItem_Click) Exception.", Me.Name), ex))
        End Try
    End Sub

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

    Private Sub SplitContainerVertical_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles SplitContainerVertical.GotFocus
        SplitContainerVertical.BackColor = SystemColors.GradientActiveCaption
    End Sub

    Private Sub SplitContainerVertical_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles SplitContainerVertical.LostFocus
        SplitContainerVertical.BackColor = SystemColors.GradientInactiveCaption
    End Sub

    Private Sub SplitContainerVertical_MouseEnter(ByVal sender As Object, ByVal e As System.EventArgs) Handles SplitContainerVertical.MouseEnter
        SplitContainerVertical.BackColor = SystemColors.GradientActiveCaption
    End Sub

    Private Sub SplitContainerVertical_MouseLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles SplitContainerVertical.MouseLeave
        SplitContainerVertical.BackColor = SystemColors.GradientInactiveCaption
    End Sub

    Private Sub DataGridViewArchiveSchedules_DataError(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewDataErrorEventArgs) Handles DataGridViewArchiveSchedules.DataError
        ' most likely trying to add without first selecting an instance
        Mother.HandleException(e.Exception)
    End Sub
End Class