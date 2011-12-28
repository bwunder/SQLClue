Imports System.Windows.Forms
Imports System.ServiceProcess
Imports System.Management

Public Class Mother
    Friend Shared InstanceList() As String
    Friend Shared ConfiguredInstanceList() As String
    Friend Shared DAL As New cCommon.cDataAccess
    Friend SplashToggled As Boolean = False
    Private MotherHasBeenDisplayed As Boolean
    Private NeedToInstallReporting As Boolean
    Private NeedToInstallRepository As Boolean
    Private NeedToInstallRunbook As Boolean
    Private _LoadRemoteRunbook As Boolean
    Private ServiceSettings(33) As String
    Friend SQLClueServiceAccount As String
    Friend SQLClueHostSQLServerServiceAccount As String

    Private BeenNagged As Boolean

    Public ReadOnly Property LicensedInstanceList() As String()
        Get
            Return DAL.GetLicensedInstanceList
        End Get
    End Property

    Public ReadOnly Property RunbookIsLocal(ByVal RunbookHost As String) As Boolean
        Get
            If My.Settings.RepositoryInstanceName = RunbookHost Then
                Return True
            Else
                Return False
            End If
        End Get
    End Property

    Private Sub Mother_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        ' don't want to change anything accidentally
        My.Settings.Reload()
        If Me.WindowState = FormWindowState.Normal Then
            My.Settings.LastWidth = Me.Width
            My.Settings.LastHeight = Me.Height
        End If
        My.Settings.LastWindowState = CInt(Me.WindowState)
        My.Settings.Save()
    End Sub

    Private Sub Mother_Load(ByVal sender As Object, ByVal e As System.EventArgs) _
    Handles Me.Load
        Try
            ' seems to be some period when loading that the app chokes trying to display an EMB.
            ' looks like it is blocked waiting for form to finish loading.
            ' MotherHasBeenDisplayed is a work around to make sure errors can be handled at all times.
            MotherHasBeenDisplayed = False
            BeenNagged = False
            Me.Text = My.Application.Info.ProductName
            SplashScreen1.CurrentStatusEventHandler("Verifying Event Log")
            My.Application.DoEvents()
            InitEventLog()
            ToolStripComboBoxDiscoveryScope.SelectedItem = My.Settings.ServerDiscoveryScope
            SplashScreen1.CurrentStatusEventHandler(String.Format("Finding SQL Servers. {0} Search scope. See 'Tools' Menu to change.", My.Settings.ServerDiscoveryScope))
            SetServerList()
            _LoadRemoteRunbook = False
            LicenseToolStripMenuItem.ToolTipText = "License SQLClue for full feature support"
            RepositoryToolStripMenuItem.ToolTipText = "Configure SQL Configuration Data Store"
            RunbookToolStripMenuItem.ToolTipText = "Configure the Query SQLClue Runbook Data Store"
            ToolStripMenuItemDashboard.Enabled = My.Settings.RepositoryEnabled
            ToolStripMenuItemConfigurationReports.Enabled = My.Settings.RepositoryEnabled
            ToolStripMenuItemConfigurationReports.DropDown.Enabled = My.Settings.RepositoryEnabled
            ToolStripMenuItemRunbookReports.Enabled = My.Settings.RunbookEnabled
            ToolStripMenuItemRunbookReports.DropDown.Enabled = My.Settings.RunbookEnabled
            Me.MainMenuStrip = MenuStripMother
            Me.MainMenuStrip.AllowMerge = False
            SplashScreen1.CurrentStatusEventHandler("Loading Report Library")
            ReportViewerLoad()
            SplashScreen1.CurrentStatusEventHandler("Loading Configuration Compare")
            CompareLoad()
            ' keep this above the other license checking components  
            SplashScreen1.CurrentStatusEventHandler("Loading Archive Planning")
            If (RepositoryLoad()) Then
                SplashScreen1.CurrentStatusEventHandler("Loading Runbook")
                RunbookLoad()
            End If
            '<component>UninstallMenuItem.Enabled is bound to My.Settings.<component>Enabled in designer
            '<component>ConfigureMenuItem.Enabled is bound to My.Settings.<component>Enabled in designer
            RepositoryInstallMenuItem.Enabled = Not (My.Settings.RepositoryEnabled)
            RunbookInstallMenuItem.Enabled = Not (My.Settings.RunbookEnabled)
            SplashScreen1.CurrentStatusEventHandler("Loading Help System")
            HelpLoad()
            SplashScreen1.CurrentStatusEventHandler("Getting Automation Controller State")
            GetServiceStatus()
            If My.Settings.ServiceEnabled Then
                ServiceEnable()
            Else
                ServiceDisable()
            End If
            SetServiceSettings()
            SplashScreen1.CurrentStatusEventHandler("")
            ' always want ReportViewer to be on top
            TabForms.SelectTab(ReportViewerForm.Name & "Tab")
            Me.Width = My.Settings.LastWidth
            Me.Height = My.Settings.LastHeight
            Me.WindowState = CType(My.Settings.LastWindowState, System.Windows.Forms.FormWindowState)
            MotherHasBeenDisplayed = True
        Catch ex As Exception
            HandleException(ex)
            Close()
        End Try
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) _
    Handles ExitToolStripMenuItem.Click
        Try
            Close()
        Catch ex As Exception
            HandleException(ex)
        End Try
    End Sub

    Private Sub SetServerList()
        Try
            Dim BrowsableServerList As New DataTable
            If My.Settings.ServerDiscoveryScope Is Nothing Then
                My.Settings.ServerDiscoveryScope = "Local"
                My.Settings.Save()
            End If
            ' no need to go through this more than once per instantiation
            Select Case My.Settings.ServerDiscoveryScope
                Case "Network"
                    ' go for names on the network
                    BrowsableServerList = SmoApplication.EnumAvailableSqlServers()
                Case "Local"
                    ' only go for local names. Should be faster
                    BrowsableServerList = SmoApplication.EnumAvailableSqlServers(True)
                Case "None"
                    BrowsableServerList.Clear()
            End Select
            ' need to make install decision now because there
            ' has been no attemp to connect to any database yet
            Try
                If Not My.Settings.RepositoryInstanceName = "" Then
                    ' set the repository connection string values
                    DAL.RepositoryInstanceName = My.Settings.RepositoryInstanceName
                    DAL.RepositoryUseTrustedConnection = My.Settings.RepositoryUseTrustedConnection
                    DAL.RepositoryDatabaseName = My.Settings.RepositoryDatabaseName
                    DAL.RepositoryConnectionTimeout = My.Settings.RepositoryConnectionTimeout
                    DAL.RepositorySQLLoginName = My.Settings.RepositorySQLLoginName
                    DAL.RepositorySQLLoginPassword = My.Settings.RepositorySQLLoginPassword
                    DAL.RepositoryNetworkLibrary = My.Settings.RepositoryNetworkLibrary
                    DAL.RepositoryEncryptConnection = My.Settings.RepositoryEncryptConnection
                    DAL.RepositoryTrustServerCertificate = My.Settings.RepositoryTrustServerCertificate
                    ' include all servers that are configured locally for archive in the a list
                    ConfiguredInstanceList = DAL.GetConfiguredInstanceList()
                    For i As Integer = 0 To ConfiguredInstanceList.Length - 1
                        ConfiguredInstanceList(i) = UCase(ConfiguredInstanceList(i))
                    Next
                Else
                    NeedToInstallRepository = True
                End If
            Catch ex As Exception
                NeedToInstallRepository = False
            End Try
            Try
                If My.Settings.RunbookInstanceName = "" Then
                    NeedToInstallRunbook = True
                End If
            Catch ex As Exception
                NeedToInstallRunbook = True
                'HandleException(ex)
            End Try
            ' and use them to build uber list of configured plus detected
            If Not ConfiguredInstanceList Is Nothing Then
                InstanceList = ConfiguredInstanceList
            Else
                ' modules may ask for ConfiguredInstanceList or InstanceList
                ' both should never be Nothing once SetServerList is called
                ReDim InstanceList(0)
                InstanceList(0) = My.Computer.Name
                ReDim ConfiguredInstanceList(0)
                ConfiguredInstanceList(0) = ""
            End If
            'add the configured servers to the dropdown
            'configured list should all be ucase if the UI has always been used
            If Not BrowsableServerList Is Nothing Then
                'Array.Sort(InstanceList)
                If BrowsableServerList.Rows.Count > 0 Then
                    Dim i As Integer = InstanceList.Length
                    For r As Int32 = 0 To BrowsableServerList.Rows.Count - 1
                        If Array.BinarySearch(InstanceList, UCase(BrowsableServerList.Rows(r)("Name").ToString)) < 0 Then
                            ReDim Preserve InstanceList(i)
                            InstanceList(i) = UCase(BrowsableServerList.Rows(r)("Name").ToString)
                            i += 1
                        End If
                    Next
                End If
            End If
        Catch ex As Exception
            HandleException(ex)
        End Try
    End Sub

#Region " MDI "

    Private Sub Startup_MdiChildActivate(ByVal sender As Object, ByVal e As System.EventArgs) _
    Handles Me.MdiChildActivate
        Try
            Try
                If Not (Me.ActiveMdiChild Is Nothing) Then
                    If ActiveMdiChild.Name = "ConfigurationForm" _
                    Or ActiveMdiChild.Name = "CompareForm" _
                    Or ActiveMdiChild.Name = "HelpForm" _
                    Or ActiveMdiChild.Name = "ReportViewerForm" _
                    Or ActiveMdiChild.Name = "RunbookForm" Then
                        'MDIClient's ControlAdded/ControlRemoved events will be fired whenever a child form is added or removed.
                        'If child form is new and no has tabPage create new tabPage
                        If Me.ActiveMdiChild.Tag Is Nothing Then
                            'Add a tabPage to tabControl with child form caption
                            Dim tp As TabPage = New TabPage(Me.ActiveMdiChild.Text)
                            'display the overview statement for each form on the tab
                            Select Case ActiveMdiChild.Name
                                Case "ConfigurationForm"
                                    tp.ToolTipText = My.Resources.ArchivePlanningOverview
                                Case "CompareForm"
                                    tp.ToolTipText = My.Resources.CompareOverview
                                Case "HelpForm"
                                    tp.ToolTipText = My.Resources.HelpOverview
                                Case "ReportViewerForm"
                                    tp.ToolTipText = My.Resources.ReportViewerOverview
                                Case "RunbookForm"
                                    tp.ToolTipText = My.Resources.RunbookOverview
                            End Select
                            tp.Tag = Me.ActiveMdiChild
                            tp.BorderStyle = BorderStyle.None
                            Me.ActiveMdiChild.Tag = tp
                            ' This should put the child on the tab panel instead of under it
                            tp.Controls.Add(Me.ActiveMdiChild)
                            tp.UseVisualStyleBackColor = True
                            tp.Name = ActiveMdiChild.Name & "Tab"
                            tp.Parent = TabForms
                            TabForms.SelectedTab = tp
                        End If
                    End If
                End If
            Catch ex As Exception
                HandleException(ex)
            End Try
        Catch ex As Exception
            HandleException(ex)
        End Try
    End Sub

    Private Sub TabForms_Selected(ByVal sender As Object, ByVal e As System.Windows.Forms.TabControlEventArgs) _
    Handles TabForms.Selected
        ' sender is tabcontrol, if tabpage text is set to form text in Startup_MdiChildActivate 
        ' page creation statement it sticks on the last name loaded
        Try
            Me.ActivateMdiChild(CType(CType(sender, TabControl).SelectedTab.Tag, Form))
        Catch ex As Exception
            HandleException(ex)
        End Try
    End Sub

#End Region

#Region " Error Handling "

    ' this region must remain consistent with the service error handler
    Friend Sub HandleException(ByVal ex As System.Exception)
        ' any error is not handled by this method is bad bad bad 
        Try
            ' a local copy of the ex is used to concat the messages 
            Dim ex1 As System.Exception = ex
            Dim ex1Msg As String = My.Application.Info.ProductName & _
                                   If(ex.TargetSite Is Nothing, "", "." & ex.TargetSite.Name) & _
                                   vbCrLf & vbCrLf & _
                                   ex1.Message & _
                                   vbCrLf & vbCrLf
            While Not ex1.InnerException Is Nothing
                ex1Msg += ex1.InnerException.Message & vbCrLf & vbCrLf
                ex1 = ex1.InnerException
            End While
            ex1Msg += My.Resources.ExceptionInfoMsg

            ex1Msg += If(ex.StackTrace Is Nothing, _
                         If(ex.GetBaseException.StackTrace Is Nothing, _
                            "", _
                            "-base exception stack trace-" & vbCrLf & _
                            ex.GetBaseException.StackTrace), _
                         "-stack trace-" & vbCrLf & _
                         ex.StackTrace)
            ' add the exception to the log
            EventLogMother.WriteEntry(ex1Msg, EventLogEntryType.Error, 100)
            ' display the error dialog
            ' (section omitted from Service)
            ' handle the case where an error occurs during initial load
            Dim ExTop As New Exception(Me.Text & " " & ex.GetType.ToString & " ", ex)
            ExTop.Source = My.Application.Info.ProductName
            ' deal with the possibility that an error occurs before the form displays
            ' with out this, the ap seems to hand at the emb.Show forever
            If MotherHasBeenDisplayed Then
                Dim emb As New ExceptionMessageBox(ExTop)
                emb.Beep = True
                emb.ShowToolBar = True
                emb.Show(Me)
            Else
                If My.Forms.SplashScreen1.Visible Then
                    My.Forms.SplashScreen1.ToggleSplashVisible()
                End If
                ' concat the message
                Dim ExceptionButNoWindow As System.Exception = ExTop
                Dim ExceptionButNoWindowMsg As String = ExceptionButNoWindow.Message
                While Not ExceptionButNoWindow.InnerException Is Nothing
                    ExceptionButNoWindowMsg += ExceptionButNoWindow.InnerException.Message & vbCrLf & vbCrLf
                    ExceptionButNoWindow = ExceptionButNoWindow.InnerException
                End While
                ExceptionButNoWindowMsg += "- - - - - - - - - - - - - - - - - -" & vbCrLf & vbCrLf & _
                                           "SQLClue is unable to complete the application startup sequence." & vbCrLf & _
                                           "The Application Event Log may contain additional information." & vbCrLf & _
                                           "- - - - - - - - - - - - - - - - - -" & vbCrLf & vbCrLf
                ExceptionButNoWindowMsg += If(ExceptionButNoWindow.StackTrace Is Nothing, _
                                              If(ExceptionButNoWindow.GetBaseException.StackTrace Is Nothing, _
                                                 "", _
                                                 "-base exception stack trace-" & vbCrLf & _
                                                 ExceptionButNoWindow.GetBaseException.StackTrace), _
                                              "-stack trace-" & vbCrLf & _
                                              ExceptionButNoWindow.StackTrace)
                ' display the error but don't use the emb so there is no dependency 
                ' on the this form being all the way open when an error occurs
                ' get the spalsh screen out of the way
                EventLogMother.WriteEntry(ExceptionButNoWindowMsg, EventLogEntryType.Error, 100)
                MessageBox.Show(ExceptionButNoWindowMsg, _
                                "SQLClue Console Bootstrap Exception Handler", _
                                MessageBoxButtons.OK, _
                                MessageBoxIcon.Stop, _
                                MessageBoxDefaultButton.Button1)
            End If
        Catch x As Exception
            EventLogMother.WriteEntry(x.ToString, EventLogEntryType.Error, 1)
            ' close the form
            Me.Close()
        End Try
    End Sub

    Friend Sub InitEventLog()
        Try
            ' only admins can create log metadata in VS2008
            ' to avoid issues use the Application Event Log
            If Not EventLog.SourceExists(My.Application.Info.ProductName) Then
                EventLog.CreateEventSource(My.Application.Info.ProductName, EventLogMother.Log)
                EventLogMother.WriteEntry(String.Format("{0} Event Log Source created.", My.Application.Info.ProductName), _
                                     EventLogEntryType.SuccessAudit)
            End If
            If Not EventLog.SourceExists(My.Resources.SQLClueService) Then
                EventLog.CreateEventSource(My.Resources.SQLClueService, EventLogMother.Log)
                EventLogMother.WriteEntry(String.Format("{0} Event Log Source created.", My.Resources.SQLClueService), _
                                     EventLogEntryType.SuccessAudit)
            End If
        Catch ex As Exception
            HandleException(ex)
        End Try
    End Sub

#End Region

#Region " Help "

    Private Sub Mother_HelpRequested(ByVal sender As Object, ByVal hlpevent As System.Windows.Forms.HelpEventArgs) _
    Handles Me.HelpRequested

        Try
            hlpevent.Handled = True

            Select Case ActiveMdiChild.Name

                Case "ConfigurationForm"
                    HelpForm.Show("Repository.htm")
                Case "CompareForm"
                    HelpForm.Show("Compare.htm")
                Case "RunbookForm"
                    HelpForm.Show("Runbook.htm")
                Case "ReportViewerForm"
                    HelpForm.Show("Reports.htm")
            End Select

            TabForms.SelectTab(HelpForm.Name & "Tab")

        Catch ex As Exception
            HandleException(ex)
        End Try

    End Sub

    Private Sub IndexToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) _
    Handles IndexToolStripMenuItem.Click

        HelpForm.Show("SQLClue.htm")
        Me.TabForms.SelectTab(HelpForm.Name & "Tab")

    End Sub

    Private Sub BrowserIndexToolStripMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) _
    Handles BrowserIndexToolStripMenuItem.Click

        Try
            System.Diagnostics.Process.Start("IExplore", My.Application.Info.DirectoryPath & "\" & My.Settings.HelpRoot)
        Catch ex As Exception
            HandleException(ex)
        End Try

    End Sub

    Private Sub HelpLoad()

        HelpForm.MdiParent = Me
        HelpForm.Dock = DockStyle.Fill
        HelpForm.Show("SQLClue.htm")

    End Sub

#End Region

#Region " Repository "

    Private Sub CompareLoad()
        CompareForm.MdiParent = Me
        CompareForm.Dock = DockStyle.Fill
        CompareForm.Show()
    End Sub

    Private Sub SyncRepositoryConnectionWithSettings()
        DAL.RepositoryInstanceName = My.Settings.RepositoryInstanceName
        DAL.RepositoryUseTrustedConnection = My.Settings.RepositoryUseTrustedConnection
        DAL.RepositoryDatabaseName = My.Settings.RepositoryDatabaseName
        DAL.RepositoryConnectionTimeout = My.Settings.RepositoryConnectionTimeout
        DAL.RepositorySQLLoginName = My.Settings.RepositorySQLLoginName
        DAL.RepositorySQLLoginPassword = My.Settings.RepositorySQLLoginPassword
        DAL.RepositoryNetworkLibrary = My.Settings.RepositoryNetworkLibrary
        DAL.RepositoryEncryptConnection = My.Settings.RepositoryEncryptConnection
        DAL.RepositoryTrustServerCertificate = My.Settings.RepositoryTrustServerCertificate
    End Sub


    Friend Function RepositoryLoad() As Boolean
        Try
            'need to make sure the config row is loaded now. This is one reason the
            'this method needs to be called before other licensed features
            If My.Settings.RepositoryEnabled Then
                ' set the repository connection string values
                SyncRepositoryConnectionWithSettings()

                ' DAL.AvailableLicenses only pings in unlicensed mod
                Dim r As Integer = DAL.PingSQLCfg()
                Select Case r
                    Case cCommon.cDataAccess.PingSQLCfgResponse.Licensed
                        ' ping found the row
                        My.Settings.RepositoryEnabled = True
                    Case cCommon.cDataAccess.PingSQLCfgResponse.Evaluation
                        ' ping found the row
                        My.Settings.RepositoryEnabled = True
                    Case cCommon.cDataAccess.PingSQLCfgResponse.NoData
                        'here if no data is appropriate to try load
                        DAL.LoadSQLCfg()
                        Dim rJr As Integer = DAL.PingSQLCfg()
                        Select Case rJr
                            Case cCommon.cDataAccess.PingSQLCfgResponse.Licensed
                                ' ping found the row
                                My.Settings.RepositoryEnabled = True
                            Case cCommon.cDataAccess.PingSQLCfgResponse.Evaluation
                                ' ping found the row
                                My.Settings.RepositoryEnabled = True
                            Case Else
                                DAL.RepositoryInstanceName = ""
                                RepositoryDisable()
                                Throw New Exception(String.Format("Unexpected response [{0}] -> [{1}] from SQL Configuration Repository.", _
                                                                             CType(r, cCommon.cDataAccess.PingSQLCfgResponse).ToString, _
                                                                             CType(rJr, cCommon.cDataAccess.PingSQLCfgResponse).ToString))
                        End Select
                    Case Else
                        DAL.RepositoryInstanceName = ""
                        RepositoryDisable()
                        Throw New Exception(String.Format("Unexpected response [{0}] from SQL Configuration Repository.", _
                                                                     CType(r, cCommon.cDataAccess.PingSQLCfgResponse).ToString))
                End Select
            End If
            'license check will prevent application load, so need to handle grace period here 
            'If My.Settings.RepositoryEnabled AndAlso DAL.AvailableLicenses >= 0 Then
            If My.Settings.RepositoryEnabled Then
                If DAL.dsSQLCfg.tSQLCfg(0).LicensedUser = "" Then
                    DAL.dsSQLCfg.tSQLCfg(0).LicensedUser = My.User.Name
                    DAL.SaveSQLCfg()
                    DAL.LoadSQLCfg()
                    DAL.AddSQLClueAdminUser(DAL.dsSQLCfg.tSQLCfg(0).LicensedUser, _
                                            My.Settings.RepositoryDatabaseName)
                End If
                If Not UCase(My.Settings.LicensedUser) = UCase(DAL.dsSQLCfg.tSQLCfg(0).LicensedUser) Then
                    My.Settings.LicensedUser = DAL.dsSQLCfg.tSQLCfg(0).LicensedUser
                    My.Settings.Save()
                End If
                ConfigurationForm.MdiParent = Me
                ConfigurationForm.Dock = DockStyle.Fill
                ConfigurationForm.Show()
                RepositoryEnable()
                RepositoryLoad = True
                If My.Settings.RunbookEnabled Then
                    RunbookEnable()
                End If
                If My.Settings.ServiceEnabled Then
                    ServiceInstallMenuItem.Enabled = False
                    ServiceUninstallMenuItem.Enabled = True
                    ServiceToolStripMenuItem.Checked = True
                End If
            Else
                RepositoryDisable()
                RepositoryLoad = False
            End If
        Catch ex As Exception
            RepositoryLoad = False
            Throw New Exception(String.Format("({0}.RepositoryLoad) Exception.", My.Application.Info.ProductName), ex)
        End Try
    End Function

    Private Sub RepositoryInstallMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) _
    Handles RepositoryInstallMenuItem.Click
        Dim csr As Cursor = Me.Cursor
        Try
            Me.Cursor = Cursors.WaitCursor
            MenuStripMother.Enabled = False
            RepositorySetup()
        Catch ex As Exception
            HandleException(ex)
        Finally
            MenuStripMother.Enabled = True
            Me.Cursor = csr
        End Try
    End Sub

    Private Sub RepositoryConfigureMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) _
    Handles RepositoryConfigureMenuItem.Click
        Dim csr As Cursor = Me.Cursor
        Try
            Me.Cursor = Cursors.WaitCursor
            'TabForms.Hide()
            MenuStripMother.Enabled = False
            RepositorySetup()
        Catch ex As Exception
            HandleException(ex)
        Finally
            MenuStripMother.Enabled = True
            'TabForms.Show()
            Me.Cursor = csr
        End Try
    End Sub

    Private Sub RepositoryEnable()
        Try
            RepositoryToolStripMenuItem.Checked = True
            RepositoryInstallMenuItem.Enabled = False
            RepositoryConfigureMenuItem.Enabled = True
            RepositoryUninstallMenuItem.Enabled = True
            RepositoryArchiveAlertsMenuItem.Enabled = True
            ArchiveTutorialToolStripMenuItem.Enabled = True
            ' once there is a repository the other componets are OK to install
            ' but don't mess with them if already installed  
            If Not (My.Settings.RunbookEnabled) Then
                RunbookInstallMenuItem.Enabled = True
            End If
            ToolStripMenuItemConfigurationReports.Enabled = True
            ToolStripMenuItemConfigurationReports.DropDown.Enabled = True
            ToolStripMenuItemDashboard.Enabled = True
            ToolStripMenuItemConsoleReport.Enabled = True
            My.Settings.RepositoryEnabled = True
        Catch ex As Exception
            Throw New Exception(String.Format("({0}.RepositoryEnable) Exception.", My.Application.Info.ProductName), ex)
        End Try
    End Sub

    Private Sub RepositoryDisable()
        Try
            RepositoryToolStripMenuItem.Checked = False
            RepositoryInstallMenuItem.Enabled = True
            RepositoryConfigureMenuItem.Enabled = False
            RepositoryUninstallMenuItem.Enabled = False
            RepositoryArchiveAlertsMenuItem.Enabled = False
            ArchiveTutorialToolStripMenuItem.Enabled = False
            My.Settings.RunbookEnabled = False
            RunbookInstallMenuItem.Enabled = False
            ToolStripMenuItemConfigurationReports.Enabled = False
            ToolStripMenuItemConfigurationReports.DropDown.Enabled = False
            ToolStripMenuItemDashboard.Enabled = False
            ToolStripMenuItemConsoleReport.Enabled = False
            ' the service will keep retrying every minute, even if no SQL Server... so?  
            My.Settings.RepositoryEnabled = False
        Catch ex As Exception
            Throw New Exception(String.Format("({0}.RepositoryDisable) Exception.", My.Application.Info.ProductName), ex)
        End Try
    End Sub

    Protected Friend Sub RepositorySetup()
        Try
            ' ping will NOT load tSQLCfg, returns NoData NoDataSet NoTable, NoDb NoSrv, etc
            SyncRepositoryConnectionWithSettings()
            Dim r As Integer = DAL.PingSQLCfg
            If Not (r = cCommon.cDataAccess.PingSQLCfgResponse.Licensed _
            Or r = cCommon.cDataAccess.PingSQLCfgResponse.Evaluation) Then
                DAL.dsSQLCfg.Clear()
            End If
            If (My.Settings.RepositoryInstanceName = "" _
                And DAL.dsSQLCfg.tSQLCfg.Rows.Count <> 0) _
            Or (Not (My.Settings.RepositoryInstanceName = "") _
                And DAL.dsSQLCfg.tSQLCfg.Rows.Count = 0) Then
                If MessageBox.Show(My.Resources.RepositoryCorrupt) = DialogResult.OK Then
                    'make sure the setup form doesn't think this is a plain vanilla change
                    My.Settings.RepositoryEnabled = False
                Else
                    Exit Try
                End If
            End If
            ' the connection parms are by reference
            ' so this will update the settimgs values
            If DialogSetup.ShowDialog(ConfigurationForm.Text, _
                                    If(My.Settings.RepositoryEnabled, _
                                       DialogSetup.DataStoreSetupAction.Modify, _
                                       DialogSetup.DataStoreSetupAction.Enable), _
                                    My.Settings.RepositoryInstanceName, _
                                    My.Settings.RepositoryDatabaseName, _
                                    My.Settings.RepositoryUseTrustedConnection, _
                                    My.Settings.RepositorySQLLoginName, _
                                    My.Settings.RepositorySQLLoginPassword, _
                                    My.Settings.RepositoryConnectionTimeout, _
                                    My.Settings.RepositoryNetworkLibrary, _
                                    My.Settings.RepositoryEncryptConnection, _
                                    My.Settings.RepositoryTrustServerCertificate, _
                                    Me) = Windows.Forms.DialogResult.OK Then
                SyncRepositoryConnectionWithSettings()
                DAL.AddSQLClueAdminUser(If(DAL.dsSQLCfg.tSQLCfg.Count = 0, _
                                           My.User.Name, _
                                           DAL.dsSQLCfg.tSQLCfg(0).LicensedUser), _
                                        My.Settings.RepositoryDatabaseName)
                DAL.AddComponentAdminUser(My.User.Name, _
                                          "Repository", _
                                          My.Settings.RepositoryDatabaseName)

                If Not GetServiceStatus() = "" Then
                    DAL.AddServiceUser(SQLClueServiceAccount, _
                                       "Repository", _
                                       My.Settings.RepositoryDatabaseName)
                End If
                'always expecting that the script will have recreated this run-once and drop self proc
                DAL.RepositoryInit()
                ' if we got here the repository is ready
                RepositoryEnable()
                ' save any changes to the connection
                ' may need to do this above too if problems arise, don't see why right now...
                SaveSettingsAndSendToService()
                Me.TabForms.SelectTab(ReportViewerForm.Name & "Tab")
                My.Forms.ReportViewerForm.LoadSplashReport("Console")
                RepositoryLoad()
            Else
                My.Settings.Reload()
            End If
        Catch ex As Exception
            HandleException(ex)
        End Try
    End Sub

    Private Sub RepositoryUninstallMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) _
    Handles RepositoryUninstallMenuItem.Click
        Dim csr As Cursor = Me.Cursor
        Try
            Me.Cursor = Cursors.WaitCursor
            'TabForms.Hide()
            MenuStripMother.Enabled = False
            RepositoryRemove()
        Catch ex As Exception
            HandleException(ex)
        Finally
            MenuStripMother.Enabled = True
            'TabForms.Show()
            Me.Cursor = csr
        End Try
    End Sub

    Private Sub RepositoryRemove()
        Try
            If My.Settings.RunbookEnabled Then
                If MessageBox.Show("The SQL Configuration data store cannot be uninstalled" & vbCrLf & _
                           "until the SQLClue Runbook is uninstalled. Remove the SQLClue Runbook now?", _
                           "SQLClue Runbook Dependency", _
                           MessageBoxButtons.YesNo, _
                           MessageBoxIcon.Question, _
                           MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
                    RunbookRemove()
                Else
                    Exit Try
                End If
            End If
            ' remove the EVENT queue config from all targets
            ConfigurationForm.RemoveAllEventNotifications()
            ' run the uninst script
            ' the connection parms are by reference
            Dim result As DialogResult = DialogSetup.ShowDialog(ConfigurationForm.Text, _
                                                              DialogSetup.DataStoreSetupAction.Disable, _
                                                              My.Settings.RepositoryInstanceName, _
                                                              My.Settings.RepositoryDatabaseName, _
                                                              My.Settings.RepositoryUseTrustedConnection, _
                                                              My.Settings.RepositorySQLLoginName, _
                                                              My.Settings.RepositorySQLLoginPassword, _
                                                              My.Settings.RepositoryConnectionTimeout, _
                                                              My.Settings.RepositoryNetworkLibrary, _
                                                              My.Settings.RepositoryEncryptConnection, _
                                                              My.Settings.RepositoryTrustServerCertificate, _
                                                              Me)
            If result = Windows.Forms.DialogResult.OK Then
                ' if we got here the repository is gone
                RepositoryDisable()
                ' save any changes to the connection
                SaveSettingsAndSendToService()
                ' clean up the ui - but wait for GC?
                CType(My.Forms.ConfigurationForm.Tag, TabPage).Dispose()
                My.Forms.ConfigurationForm.Close()
                Me.Refresh()
                ' get the correct reporting splash 
                My.Forms.ReportViewerForm.LoadSplashReport("Workstation")
                Me.TabForms.SelectTab(ReportViewerForm.Name & "Tab")
            Else
                My.Settings.Reload()
            End If
        Catch ex As Exception
            Throw New Exception(String.Format("({0}.RepositoryRemove) Exception.", My.Application.Info.ProductName), ex)
        End Try
    End Sub

#End Region

#Region " Runbook "

    Private Sub ToolStripMenuItemConnectRemoteRunbook_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) _
    Handles ToolStripMenuItemConnectRemoteRunbook.Click
        Dim csr As Cursor = Me.Cursor
        Try
            Me.Cursor = Cursors.WaitCursor
            'TabForms.Hide()
            MenuStripMother.Enabled = False
            _LoadRemoteRunbook = True
            ' these parms are passed by reference
            ' that way if they are changed in the dialog
            ' and the connection is successfull, the new settings can be saved here else thrown away
            Dim result As DialogResult = DialogSetup.ShowDialog(RunbookForm.Text, _
                                                              DialogSetup.DataStoreSetupAction.EnableRemoteRunbook, _
                                                              My.Settings.RemoteRunbookInstanceName, _
                                                              My.Settings.RemoteRunbookDatabaseName, _
                                                              My.Settings.RemoteRunbookUseTrustedConnection, _
                                                              My.Settings.RemoteRunbookSQLLoginName, _
                                                              My.Settings.RemoteRunbookSQLLoginPassword, _
                                                              My.Settings.RemoteRunbookConnectionTimeout, _
                                                              My.Settings.RemoteRunbookNetworkLibrary, _
                                                              My.Settings.RemoteRunbookEncryptConnection, _
                                                              My.Settings.RemoteRunbookTrustServerCertificate, _
                                                              Me)
            If result = Windows.Forms.DialogResult.OK Then
                ' only after install will this be true
                ' after config, the setting would already have been true n'est pas?
                If Not RunbookIsLocal(My.Settings.RemoteRunbookInstanceName) Then
                    ' don't mess with the ui for remote load
                    'RunbookEnable()
                    RunbookLoad()
                Else
                    Throw New Exception("Remote Runbook cannot be local")
                End If
            Else
                My.Settings.Reload()
            End If
        Catch ex As Exception
            HandleException(ex)
        Finally
            _LoadRemoteRunbook = False
            MenuStripMother.Enabled = True
            'TabForms.Show()
            Me.Cursor = csr
        End Try
    End Sub

    Private Sub RunbookLoad()
        Try
            'runbook can be opened against a remote db only if not licensed
            If My.Settings.RunbookEnabled And RunbookIsLocal(My.Settings.RunbookInstanceName) Then
                'If DAL.AvailableLicenses >= 0 Then
                RunbookForm.Text = String.Format("SQLClue Runbook")
                RunbookForm.MdiParent = Me
                RunbookForm.Dock = DockStyle.Fill
                RunbookForm.Show()
                RunbookEnable()
                'Else
                '    RunbookDisable()
                'End If
            ElseIf _LoadRemoteRunbook Then
                RunbookForm.Text = String.Format("Remote Runbook [{0}].[{1}]", _
                                                 My.Settings.RemoteRunbookInstanceName, _
                                                 My.Settings.RemoteRunbookDatabaseName)
                RunbookForm.MdiParent = Me
                RunbookForm.Dock = DockStyle.Fill
                RunbookForm.Show()
            End If
        Catch ex As Exception
            Throw New Exception(String.Format("({0}.RunbookLoad) Exception.", My.Application.Info.ProductName), ex)
        End Try
    End Sub

    Private Sub RunbookEnable()
        Try
            ToolStripMenuItemFileWatcher.Enabled = True
            ToolStripMenuItemEnforceOwnership.Enabled = True
            ToolStripMenuItemConnectRemoteRunbook.Enabled = False
            RunbookToolStripMenuItem.Checked = True
            RunbookInstallMenuItem.Enabled = False
            RunbookConfigureMenuItem.Enabled = True
            RunbookUninstallMenuItem.Enabled = True
            RunbookContributorTutorialToolStripMenuItem.Enabled = True
            TopicsEditToolStripMenuItem.Enabled = True
            CategoriesEditToolStripMenuItem.Enabled = True
            CategoriesMulticolumnToolStripMenuItem.Enabled = True
            ToolStripMenuItemRunbookReports.Enabled = True
            ToolStripMenuItemRunbookReports.DropDown.Enabled = True
            My.Settings.RunbookEnabled = True
            My.Settings.Save()
            SaveSettingsAndSendToService()
        Catch ex As Exception
            Throw New Exception(String.Format("({0}.RunbookEnable) Exception.", My.Application.Info.ProductName), ex)
        End Try
    End Sub

    Private Sub RunbookDisable()
        Try
            ToolStripMenuItemFileWatcher.Enabled = False
            ToolStripMenuItemEnforceOwnership.Enabled = False
            ToolStripMenuItemConnectRemoteRunbook.Enabled = True
            RunbookToolStripMenuItem.Checked = False
            RunbookInstallMenuItem.Enabled = True
            RunbookConfigureMenuItem.Enabled = False
            RunbookUninstallMenuItem.Enabled = False
            RunbookContributorTutorialToolStripMenuItem.Enabled = False
            TopicsEditToolStripMenuItem.Enabled = False
            CategoriesEditToolStripMenuItem.Enabled = False
            CategoriesMulticolumnToolStripMenuItem.Enabled = False
            ToolStripMenuItemRunbookReports.Enabled = False
            ToolStripMenuItemRunbookReports.DropDown.Enabled = False
            My.Settings.RunbookEnabled = False
            My.Settings.Save()
            SaveSettingsAndSendToService()
        Catch ex As Exception
            Throw New Exception(String.Format("({0}.RunbookDisable) Exception.", My.Application.Info.ProductName), ex)
        End Try
    End Sub

    Private Sub RunbookInstallMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) _
    Handles RunbookInstallMenuItem.Click
        Dim csr As Cursor = Me.Cursor
        Try
            Me.Cursor = Cursors.WaitCursor
            'TabForms.Hide()
            MenuStripMother.Enabled = False
            RunbookSetup()
        Catch ex As Exception
            HandleException(ex)
        Finally
            MenuStripMother.Enabled = True
            'TabForms.Show()
            Me.Cursor = csr
        End Try
    End Sub

    Private Sub RunbookConfigureMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) _
    Handles RunbookConfigureMenuItem.Click
        Dim csr As Cursor = Me.Cursor
        Try
            Me.Cursor = Cursors.WaitCursor
            'TabForms.Hide()
            MenuStripMother.Enabled = False
            RunbookSetup()
        Catch ex As Exception
            HandleException(ex)
        Finally
            MenuStripMother.Enabled = True
            'TabForms.Show()
            Me.Cursor = csr
        End Try
    End Sub

    Private Sub RunbookSetup()
        Try
            If Not My.Settings.RepositoryEnabled Then
                If MessageBox.Show("The SQL Configuration data store database must be installed before " & vbCrLf & _
                                   "the SQLClue Runbook data store can be installed. Run SQL Configuration " & vbCrLf & _
                                   "Repository Setup now?", _
                                   "SQL Configuration Repository Dependency", _
                                   MessageBoxButtons.OKCancel, _
                                   MessageBoxIcon.Asterisk, _
                                   MessageBoxDefaultButton.Button1) = Windows.Forms.DialogResult.OK Then
                    RepositorySetup()
                Else
                    Exit Try
                End If
            End If
            ' only if the repository is already installed
            ' go ahead and load setup even if the runbook is already enabled.
            ' how else would the connection be maintained? but not so sure about this, Well see..
            If My.Settings.RepositoryEnabled Then
                ' no clue whay I did this, it breaks the rconfigure connection... 
                'If Not My.Forms.RunbookForm.Tag Is Nothing AndAlso CType(My.Forms.RunbookForm.Tag, TabPage).Created Then
                ' clean up the ui - but wait for GC?
                'CType(My.Forms.RunbookForm.Tag, TabPage).Dispose()
                'My.Forms.RunbookForm.Close()
                'Me.Refresh()
                'End If
                DialogSetup.Text = "SQLClue: SQLClue Runbook Setup"
                ' these parms are passed by reference
                ' that way if they are changed in the dialog
                ' and the connection is successfull, the new settings can be saved here else thrown away
                Dim result As DialogResult = DialogSetup.ShowDialog(RunbookForm.Text, _
                                                                    If(My.Settings.RunbookEnabled, _
                                                                       DialogSetup.DataStoreSetupAction.Modify, _
                                                                       DialogSetup.DataStoreSetupAction.Enable), _
                                                                       My.Settings.RunbookInstanceName, _
                                                                       My.Settings.RunbookDatabaseName, _
                                                                       My.Settings.RunbookUseTrustedConnection, _
                                                                       My.Settings.RunbookSQLLoginName, _
                                                                       My.Settings.RunbookSQLLoginPassword, _
                                                                       My.Settings.RunbookConnectionTimeout, _
                                                                       My.Settings.RunbookNetworkLibrary, _
                                                                       My.Settings.RunbookEncryptConnection, _
                                                                       My.Settings.RunbookTrustServerCertificate, _
                                                                       Me)
                If result = Windows.Forms.DialogResult.OK Then
                    DAL.AddSQLClueAdminUser(DAL.dsSQLCfg.tSQLCfg(0).LicensedUser, _
                        My.Settings.RunbookDatabaseName)
                    DAL.AddComponentAdminUser(My.User.Name, _
                          "Runbook", _
                          My.Settings.RunbookDatabaseName)
                    If Not GetServiceStatus() = "" Then
                        DAL.AddServiceUser(SQLClueServiceAccount, _
                                           "Runbook", _
                                           My.Settings.RunbookDatabaseName)
                    End If
                    ' only after install will this be true
                    If Not My.Settings.RunbookEnabled _
                    AndAlso RunbookIsLocal(My.Settings.RunbookInstanceName) Then
                        RunbookEnable()
                        RunbookLoad()
                    End If
                Else
                    My.Settings.Reload()
                End If
            End If
        Catch ex As Exception
            HandleException(ex)
        End Try
    End Sub

    Private Sub RunbookRemove()
        Try
            ' remote local just needs to be disabled
            If My.Settings.RunbookEnabled AndAlso RunbookIsLocal(My.Settings.RunbookInstanceName) Then
                ' these parms are passed by reference
                ' that way if they are changed in the dialog
                ' and the connection is successfull, the new settings can be saved here else thrown away
                Dim result As DialogResult = DialogSetup.ShowDialog(RunbookForm.Text, _
                                                                  DialogSetup.DataStoreSetupAction.Disable, _
                                                                  My.Settings.RunbookInstanceName, _
                                                                  My.Settings.RunbookDatabaseName, _
                                                                  My.Settings.RunbookUseTrustedConnection, _
                                                                  My.Settings.RunbookSQLLoginName, _
                                                                  My.Settings.RunbookSQLLoginPassword, _
                                                                  My.Settings.RunbookConnectionTimeout, _
                                                                  My.Settings.RunbookNetworkLibrary, _
                                                                  My.Settings.RunbookEncryptConnection, _
                                                                  My.Settings.RunbookTrustServerCertificate, _
                                                                  Me)
                If result = Windows.Forms.DialogResult.OK Then
                    RunbookDisable()
                    ' clean up the ui - but wait for GC?
                    CType(My.Forms.RunbookForm.Tag, TabPage).Dispose()
                    My.Forms.RunbookForm.Close()
                    Me.Refresh()
                Else
                    My.Settings.Reload()
                    If My.Settings.RunbookEnabled Then
                        RunbookEnable()
                    Else
                        RunbookDisable()
                    End If
                End If
            Else
                ' ony need to do work here if remote runbook
                If My.Settings.RunbookEnabled Then
                    RunbookDisable()
                End If
            End If
        Catch ex As Exception
            Throw New Exception(String.Format("({0}.RunbookRemove) Exception.", My.Application.Info.ProductName), ex)
        End Try
    End Sub

    Private Sub RunbookUninstallMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) _
    Handles RunbookUninstallMenuItem.Click
        Dim csr As Cursor = Me.Cursor
        Try
            Me.Cursor = Cursors.WaitCursor
            'TabForms.Hide()
            MenuStripMother.Enabled = False
            RunbookRemove()
        Catch ex As Exception
            HandleException(ex)
        Finally
            Me.Cursor = csr
            MenuStripMother.Enabled = True
            'TabForms.Show()
        End Try
    End Sub

    Private Sub TopicsEditToolStripMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles TopicsEditToolStripMenuItem.Click
        Dim csr As Cursor = Nothing
        Try
            csr = Me.Cursor   ' Save the old cursor
            Me.Cursor = Cursors.WaitCursor   ' Display the waiting cursor
            If RunbookForm.Enabled Then
                DialogRunbookTopic.ShowDialog(Me)
            End If
        Catch ex As Exception
            HandleException(ex)
        Finally
            Me.Cursor = csr
        End Try
    End Sub

    Private Sub CategoriesEditToolStripMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CategoriesEditToolStripMenuItem.Click
        Dim csr As Cursor = Nothing
        Try
            csr = Me.Cursor   ' Save the old cursor
            Me.Cursor = Cursors.WaitCursor   ' Display the waiting cursor
            If RunbookForm.Enabled Then
                DialogRunbookCategory.ShowDialog(Me)
            End If
        Catch ex As Exception
            HandleException(ex)
        Finally
            Me.Cursor = csr
        End Try
    End Sub

    Private Sub CategoriesMulticolumnToolStripMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CategoriesMulticolumnToolStripMenuItem.Click
        Try
            CategoriesMulticolumnToolStripMenuItem.Checked = Not CategoriesMulticolumnToolStripMenuItem.Checked
            My.Settings.RunbookCategoriesMultiColumn = CategoriesMulticolumnToolStripMenuItem.Checked
            My.Settings.Save()
            RunbookForm.ListBoxCategory.MultiColumn = CategoriesMulticolumnToolStripMenuItem.Checked
            RunbookForm.ListBoxCategory.Refresh()
        Catch ex As Exception
            HandleException(ex)
        End Try
    End Sub

    Private Sub ToolStripMenuItemEnforceOwnership_Click(ByVal sender As Object, ByVal e As System.EventArgs) _
    Handles ToolStripMenuItemEnforceOwnership.Click
        Try
            ToolStripMenuItemEnforceOwnership.Checked = Not ToolStripMenuItemEnforceOwnership.Checked
            RunbookForm.SetRunbookOptionFromMenuItems()
        Catch ex As Exception
            HandleException(ex)
        End Try
    End Sub

    Private Sub ToolStripMenuItemFileWatcher_Click(ByVal sender As Object, ByVal e As System.EventArgs) _
    Handles ToolStripMenuItemFileWatcher.Click
        Try
            ToolStripMenuItemFileWatcher.Checked = Not ToolStripMenuItemFileWatcher.Checked
            RunbookForm.SetRunbookOptionFromMenuItems()
        Catch ex As Exception
            HandleException(ex)
        End Try
    End Sub

#End Region

#Region " Menuitem dialogs "

    Private Sub ToolStripMenuItemLastCompare_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ToolStripMenuItemLastCompare.Click
        Try
            System.Diagnostics.Process.Start("IExplore", My.Computer.FileSystem.SpecialDirectories.MyDocuments & "\SQLClueCompare.htm")
        Catch ex As Exception
            HandleException(ex)
        End Try

    End Sub

    Private Sub LicenseToolStripMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) _
    Handles LicenseToolStripMenuItem.Click
        Try
            For Each f As Form In My.Application.OpenForms
                If f.Name = "LicenseForm" Then
                    f.Focus()
                    Exit Sub
                End If
            Next
            DialogLicense.StartPosition = FormStartPosition.CenterParent
            DialogLicense.ShowDialog(Me)
        Catch ex As Exception
            HandleException(ex)
        End Try
    End Sub

    Private Sub AboutToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) _
    Handles AboutToolStripMenuItem.Click
        Try
            For Each f As Form In My.Application.OpenForms
                If f.Name = "AboutBoxForm" Then
                    f.Focus()
                    Exit Sub
                End If
            Next
            AboutBoxForm.StartPosition = FormStartPosition.CenterParent
            AboutBoxForm.ShowDialog(Me)
        Catch ex As Exception
            HandleException(ex)
        End Try
    End Sub

    Private Sub ToolStripMenuItemCheckforUpdates_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

        Try
            For Each f As Form In My.Application.OpenForms
                If f.Name = "CheckForUpdatesForm" Then
                    f.Focus()
                    Exit Sub
                End If
            Next
            MessageBox.Show(String.Format("Check for latest SQLClue release at http://www.bwunder.com" & vbCrLf _
                                        & "This installation is at version  {0}", My.Application.Info.Version))
        Catch ex As Exception
            HandleException(ex)
        End Try
    End Sub

    Private Sub ToolStripMenuItemAdvancedSettings_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) _
    Handles ToolStripMenuItemAdvancedSettings.Click
        Dim csr As Cursor = Me.Cursor
        Try
            Me.Cursor = Cursors.WaitCursor
            TabForms.Hide()
            My.Settings.SettingsDirty = True
            If frmOptions.ShowDialog() = Windows.Forms.DialogResult.OK Then
                My.Forms.CompareForm.SyncWorkingValues()
                If My.Forms.CompareForm.Panel1ConnectionSettingsChanged Then
                    CompareForm.InitPanel(CompareForm.Origin1, _
                                          CompareForm.Instance1, _
                                          CompareForm.HierarchyLabel1, _
                                          CompareForm.smoTreeView1, _
                                          CompareForm.cCompare.SqlServer1, _
                                          CompareForm.ToolStripStatusLabel1)

                    My.Forms.CompareForm.Panel1ConnectionSettingsChanged = False
                End If
                If My.Forms.CompareForm.Panel2ConnectionSettingsChanged Then
                    CompareForm.InitPanel(CompareForm.Origin2, _
                          CompareForm.Instance2, _
                          CompareForm.HierarchyLabel2, _
                          CompareForm.smoTreeView2, _
                          CompareForm.cCompare.SqlServer2, _
                          CompareForm.ToolStripStatusLabel2)
                    My.Forms.CompareForm.Panel2ConnectionSettingsChanged = False
                End If
            End If
            frmOptions.Dispose()
        Catch ex As Exception
            HandleException(ex)
        Finally
            Me.Cursor = csr
            TabForms.Show()
        End Try
    End Sub

    Private Sub RepositoryArchiveAlertsMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RepositoryArchiveAlertsMenuItem.Click
        Dim srvcon As New ServerConnection
        Dim oRepository As Smo.Server
        Try
            srvcon.ConnectionString = DAL.LocalRepositoryConnectionString
            oRepository = New Server(srvcon)
            If Not (oRepository.EngineEdition = Edition.Express) Then
                If oRepository.JobServer.Operators.Count > 0 Then
                    DialogArchiveNotifications.Tag = oRepository
                    DialogArchiveNotifications.ShowDialog(Me)
                Else
                    HandleException(New ApplicationException(My.Resources.ArchiveNotificationOperatorRequired))
                End If
            Else
                HandleException(New ApplicationException(My.Resources.ArchiveNotificationNoSQLExpress))
            End If
        Catch exSQL As SqlClient.SqlException
            HandleException(exSQL)
        Finally
            If Not (srvcon Is Nothing) Then
                srvcon.Disconnect()
                oRepository = Nothing
            End If
        End Try

    End Sub

#End Region

#Region " Service "

    Private Sub SetServiceSettings()
        ServiceSettings(0) = My.Settings.RepositoryEnabled.ToString 'CBool
        ServiceSettings(1) = My.Settings.RunbookEnabled.ToString 'CBool
        ServiceSettings(2) = "-" 'not used
        ServiceSettings(3) = My.Settings.RepositoryInstanceName
        ServiceSettings(4) = My.Settings.RepositoryDatabaseName
        ServiceSettings(5) = My.Settings.RepositoryUseTrustedConnection.ToString 'CBool
        ServiceSettings(6) = If(My.Settings.RepositorySQLLoginName = "", "-", My.Settings.RepositorySQLLoginName)
        ServiceSettings(7) = If(My.Settings.RepositorySQLLoginPassword = "", "-", My.Settings.RepositorySQLLoginPassword)
        ServiceSettings(8) = If(My.Settings.RepositoryNetworkLibrary = "", "-", My.Settings.RepositoryNetworkLibrary)
        ServiceSettings(9) = My.Settings.RepositoryConnectionTimeout.ToString 'CInt
        ServiceSettings(10) = My.Settings.RepositoryEncryptConnection.ToString 'CBool
        ServiceSettings(11) = My.Settings.RepositoryTrustServerCertificate.ToString 'CBool
        ServiceSettings(12) = If(My.Settings.RunbookInstanceName = "", "-", My.Settings.RunbookInstanceName)
        ServiceSettings(13) = If(My.Settings.RunbookDatabaseName = "", "-", My.Settings.RunbookDatabaseName)
        ServiceSettings(14) = My.Settings.RunbookUseTrustedConnection.ToString 'CBool
        ServiceSettings(15) = If(My.Settings.RunbookSQLLoginName = "", "-", My.Settings.RunbookSQLLoginName)
        ServiceSettings(16) = If(My.Settings.RunbookSQLLoginPassword = "", "-", My.Settings.RunbookSQLLoginPassword)
        ServiceSettings(17) = If(My.Settings.RunbookNetworkLibrary = "", "-", My.Settings.RunbookNetworkLibrary)
        ServiceSettings(18) = My.Settings.RunbookConnectionTimeout.ToString 'CInt
        ServiceSettings(19) = My.Settings.RunbookEncryptConnection.ToString 'CBool
        ServiceSettings(20) = My.Settings.RunbookTrustServerCertificate.ToString 'CBool
        ServiceSettings(21) = "-" 'not used
        ServiceSettings(22) = "-" 'not used
        ServiceSettings(23) = "-" 'not used
        ServiceSettings(24) = "-" 'not used
        ServiceSettings(25) = "-" 'not used
        ServiceSettings(26) = "-" 'not used
        ServiceSettings(27) = "-" 'not used
        ServiceSettings(28) = "-" 'not used
        ServiceSettings(29) = "-" 'not used
        ServiceSettings(30) = "-" 'not used
        ServiceSettings(31) = My.Settings.ServiceTimerIntervalSeconds.ToString
        ServiceSettings(32) = My.Settings.DocumentMonitorSleepSeconds.ToString
        ServiceSettings(33) = My.Settings.WaitBetweenDocumentsSeconds.ToString
    End Sub

    Private Function SaveSettingsAndSendToService() As Boolean
        ' return true if saved to service settings else false
        Try
            SaveSettingsAndSendToService = False
            My.Settings.Save()
            If My.Settings.ServiceEnabled And MotherHasBeenDisplayed Then
                If Not (ServiceSettings(0) = My.Settings.RepositoryEnabled.ToString _
                        And ServiceSettings(1) = My.Settings.RunbookEnabled.ToString _
                        And ServiceSettings(3) = My.Settings.RepositoryInstanceName _
                        And ServiceSettings(4) = My.Settings.RepositoryDatabaseName _
                        And ServiceSettings(5) = My.Settings.RepositoryUseTrustedConnection.ToString _
                        And ServiceSettings(6) = If(My.Settings.RepositorySQLLoginName = "", "-", My.Settings.RepositorySQLLoginName) _
                        And ServiceSettings(7) = If(My.Settings.RepositorySQLLoginPassword = "", "-", My.Settings.RepositorySQLLoginPassword) _
                        And ServiceSettings(8) = If(My.Settings.RepositoryNetworkLibrary = "", "-", My.Settings.RepositoryNetworkLibrary) _
                        And ServiceSettings(9) = My.Settings.RepositoryConnectionTimeout.ToString _
                        And ServiceSettings(10) = My.Settings.RepositoryEncryptConnection.ToString _
                        And ServiceSettings(11) = My.Settings.RepositoryTrustServerCertificate.ToString _
                        And ServiceSettings(12) = If(My.Settings.RunbookInstanceName = "", "-", My.Settings.RunbookInstanceName) _
                        And ServiceSettings(13) = If(My.Settings.RunbookDatabaseName = "", "-", My.Settings.RunbookDatabaseName) _
                        And ServiceSettings(14) = My.Settings.RunbookUseTrustedConnection.ToString _
                        And ServiceSettings(15) = If(My.Settings.RunbookSQLLoginName = "", "-", My.Settings.RunbookSQLLoginName) _
                        And ServiceSettings(16) = If(My.Settings.RunbookSQLLoginPassword = "", "-", My.Settings.RunbookSQLLoginPassword) _
                        And ServiceSettings(17) = If(My.Settings.RunbookNetworkLibrary = "", "-", My.Settings.RunbookNetworkLibrary) _
                        And ServiceSettings(18) = My.Settings.RunbookConnectionTimeout.ToString _
                        And ServiceSettings(19) = My.Settings.RunbookEncryptConnection.ToString _
                        And ServiceSettings(20) = My.Settings.RunbookTrustServerCertificate.ToString _
                        And ServiceSettings(31) = My.Settings.ServiceTimerIntervalSeconds.ToString _
                        And ServiceSettings(32) = My.Settings.DocumentMonitorSleepSeconds.ToString _
                        And ServiceSettings(33) = My.Settings.WaitBetweenDocumentsSeconds.ToString) Then
                    ' stop it running and if was running or My.Settings.AlwaysStartServer is true 
                    ' start it with the new settings as args
                    If (StopSQLClueService() Or My.Settings.AlwaysStartService) Then
                        StartSQLClueService()
                    End If
                    SaveSettingsAndSendToService = True
                End If 'changes
            End If ' enabled
        Catch ex As Exception
            HandleException(ex)
        End Try
    End Function

    Friend Function GetServiceStatus() As String
        Dim SvcCtl As ServiceController = Nothing
        Dim ObjectSearcher As ManagementObjectSearcher = Nothing
        Try
            ' since service .msi can be run outside of ui always set to correct state
            ObjectSearcher = New ManagementObjectSearcher(New SelectQuery("Win32_Service", "Name='" & My.Resources.SQLClueService & "'"))
            If ObjectSearcher.Get().Count = 0 Then
                GetServiceStatus = ""
                If My.Settings.ServiceEnabled = True Then
                    My.Settings.ServiceEnabled = False
                    My.Settings.Save()
                    EventLogMother.WriteEntry(String.Format(My.Resources.ServiceEnabledButNotInstalled, My.Computer.Name), _
                                              EventLogEntryType.Warning)
                End If
            Else
                For Each service As ManagementObject In ObjectSearcher.Get()
                    ' need the service acount name for granting permissions at the targets
                    SQLClueServiceAccount = Replace(service.GetPropertyValue("StartName").ToString, ".", My.Computer.Name)
                    Exit For
                Next
                SvcCtl = New ServiceController(My.Resources.SQLClueService, My.Computer.Name)
                GetServiceStatus = SvcCtl.Status.ToString
                My.Settings.ServiceEnabled = True
                If My.Settings.ServiceEnabled = False Then
                    My.Settings.ServiceEnabled = True
                    My.Settings.Save()
                    EventLogMother.WriteEntry(String.Format(My.Resources.ServiceInstalledButNotEnabled, My.Computer.Name), _
                                              EventLogEntryType.Warning)
                End If
            End If
            Select Case GetServiceStatus
                Case ServiceControllerStatus.Running.ToString
                    ServiceStartMenuItem.Enabled = False
                    ServiceContinueMenuItem.Enabled = False
                    ServicePauseMenuItem.Enabled = True
                    ServiceStopMenuItem.Enabled = True
                Case ServiceControllerStatus.Paused.ToString
                    ServiceStartMenuItem.Enabled = False
                    ServiceContinueMenuItem.Enabled = True
                    ServicePauseMenuItem.Enabled = False
                    ServiceStopMenuItem.Enabled = True
                Case ServiceControllerStatus.Stopped.ToString
                    ServiceStartMenuItem.Enabled = True
                    ServiceContinueMenuItem.Enabled = False
                    ServicePauseMenuItem.Enabled = False
                    ServiceStopMenuItem.Enabled = False
                Case Else
                    ServiceStartMenuItem.Enabled = False
                    ServiceContinueMenuItem.Enabled = False
                    ServicePauseMenuItem.Enabled = False
                    ServiceStopMenuItem.Enabled = False
            End Select
        Catch ex As Exception
            Throw New Exception(String.Format("({0}.GetServiceStatus) Exception.", My.Application.Info.ProductName), ex)
        Finally
            ObjectSearcher.Dispose()
            ObjectSearcher = Nothing
            If Not SvcCtl Is Nothing Then
                If Not SvcCtl.ServiceHandle.IsClosed Then
                    SvcCtl.Close()
                End If
                SvcCtl.Dispose()
                SvcCtl = Nothing
            End If
        End Try
    End Function

    Private Function ServiceSetup() As Integer
        Try
            If My.User.IsInRole(Microsoft.VisualBasic.ApplicationServices.BuiltInRole.Administrator) Then
                Dim s As String = GetServiceStatus()
                Dim r As DialogResult
                If s = "" Then
                    r = MessageBox.Show(String.Format(My.Resources.ServiceSetupMessage, _
                                                      My.Computer.Name, _
                                                      My.Resources.SQLClueService), _
                                        String.Format("Confirm [{0}] Authority?", My.User.Name), _
                                        MessageBoxButtons.OKCancel, _
                                        MessageBoxIcon.Exclamation, _
                                        MessageBoxDefaultButton.Button2, _
                                        MessageBoxOptions.DefaultDesktopOnly)
                End If
                ' uninstall does not need to see the message
                If Not (s = "") OrElse r = Windows.Forms.DialogResult.OK Then
                    Dim p As New System.Diagnostics.Process
                    p.StartInfo.FileName = "msiexec"
                    p.StartInfo.Arguments = If(s = "", "/package """, "/uninstall """) & My.Application.Info.DirectoryPath & "\ServiceSetup\SQLClueServiceSetup.msi"" /passive"
                    p.StartInfo.CreateNoWindow = True
                    p.Start()
                    p.WaitForExit()
                    ServiceSetup = p.ExitCode
                    ' need to go back to the menu item event handle to see if the service should be started or not
                Else
                    ServiceSetup = -1
                End If
            Else
                Throw New Exception(My.Resources.UnauthorizedInstallUser)
            End If
        Catch ex As Exception
            Throw New Exception(String.Format("({0}.ServiceSetup) Exception.", My.Application.Info.ProductName), ex)
        End Try
    End Function

    Private Sub ServiceInstallMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) _
    Handles ServiceInstallMenuItem.Click
        Dim csr As Cursor = Me.Cursor
        Try
            Me.Cursor = Cursors.WaitCursor
            MenuStripMother.Enabled = False
            'TabForms.Hide()
            ' if found now big mess so were done
            If Not SaveSettingsAndSendToService() _
            AndAlso GetServiceStatus() = "" Then
                Dim r As Integer = ServiceSetup()
                If r = -1 Then
                    Throw New Exception("The service installation was aborted.")
                ElseIf r = 0 Then
                    If My.Settings.RepositoryEnabled Then
                        If Not GetServiceStatus() = "" Then
                            DAL.AddServiceUser(SQLClueServiceAccount, _
                                               "Repository", _
                                               My.Settings.RepositoryDatabaseName)
                            For Each TargetInstanceName In LicensedInstanceList
                                Try
                                    DAL.AddSQLClueServiceAccountToTarget(SQLClueServiceAccount, _
                                                                         TargetInstanceName, _
                                                                         My.Settings.TargetHandshakeConnectionTimeout)
                                Catch ex As Exception
                                    HandleException(New Exception(String.Format("Failed to add Automation Controller service account on target instance [{0}]", TargetInstanceName), ex))
                                End Try
                            Next
                        End If
                    End If
                    If My.Settings.RunbookEnabled Then
                        If Not GetServiceStatus() = "" Then
                            DAL.AddServiceUser(SQLClueServiceAccount, _
                                               "Runbook", _
                                               My.Settings.RunbookDatabaseName)
                        End If
                    End If
                    Try
                        StartSQLClueService()
                    Catch ex As Exception
                        Throw New Exception(String.Format("The {0} service failed to start.", My.Resources.SQLClueService), ex)
                    End Try
                    ServiceEnable()
                Else
                    ServiceDisable()
                    Throw New Exception(String.Format("Service failed to install. Installer Process exit code: {0}.", r))
                End If
            End If
        Catch ex As Exception
            HandleException(ex)
        Finally
            MenuStripMother.Enabled = True
            'TabForms.Show()
            Me.Cursor = csr
        End Try
    End Sub

    Private Sub ToolStripMenuItemRepairService_Click(ByVal sender As Object, ByVal e As System.EventArgs) _
    Handles ToolStripMenuItemRepairService.Click
        ' make the UI accurately reflect what is there, don't change state
        Dim csr As Cursor = Me.Cursor
        Try
            Me.Cursor = Cursors.WaitCursor
            'TabForms.Hide()
            MenuStripMother.Enabled = False
            Dim CurState As String = GetServiceStatus()
            If Not CurState = "" Then
                If InStr(CurState, "Pending") > 0 Then
                    Throw New Exception(String.Format("The service is in transitional state {0}. " & _
                                                      "Retry the repair request after the transition is complete.", CurState))
                ElseIf CurState = ServiceControllerStatus.Running.ToString _
                Or CurState = ServiceControllerStatus.Paused.ToString Then
                    StopSQLClueService()
                    Dim p As New System.Diagnostics.Process
                    p.StartInfo.FileName = "msiexec"
                    p.StartInfo.Arguments = "/fomus """ & My.Application.Info.DirectoryPath & "\ServiceSetup\SQLClueServiceSetup.msi"" /qb"
                    p.StartInfo.CreateNoWindow = True
                    p.Start()
                    p.WaitForExit()
                End If
            End If
            If GetServiceStatus() <> "" Then
                If CurState = ServiceControllerStatus.Stopped.ToString Then
                    StopSQLClueService()
                Else
                    StartSQLClueService()
                End If
                ServiceEnable()
            Else
                ServiceDisable()
            End If
        Catch ex As Exception
            HandleException(ex)
        Finally
            MenuStripMother.Enabled = True
            'TabForms.Show()
            Me.Cursor = csr
        End Try
    End Sub

    Private Sub ServiceUninstallMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) _
    Handles ServiceUninstallMenuItem.Click
        Dim csr As Cursor = Me.Cursor
        Try
            Me.Cursor = Cursors.WaitCursor
            'TabForms.Hide()
            MenuStripMother.Enabled = False
            Dim CurState As String
            CurState = GetServiceStatus()
            If CurState <> ServiceControllerStatus.Stopped.ToString Then
                StopSQLClueService()
            End If
            Dim r As Integer = ServiceSetup()
            If r = 0 Then
                For Each TargetInstanceName In LicensedInstanceList
                    Try
                        DAL.DropSQLClueServiceAccountFromTarget(SQLClueServiceAccount, _
                                                                TargetInstanceName, _
                                                                My.Settings.TargetHandshakeConnectionTimeout)
                    Catch ex As Exception
                        HandleException(New Exception(String.Format("Failed to remove service account on target instance [{0}]", TargetInstanceName), ex))
                    End Try
                Next
                ServiceDisable()
            Else
                MessageBox.Show(String.Format("Unable to uninstall existing service. Installer process exit code: {0}." & vbCrLf & _
                                              "Close the SQLClue Console and use Control Panel to remove the existing Automation Controller." & vbCrLf & _
                                              "Note also that Service account [{1}] may not been removed from all targets.", r, SQLClueServiceAccount))
            End If
        Catch ex As Exception
            HandleException(ex)
        Finally
            MenuStripMother.Enabled = True
            'TabForms.Show()
            Me.Cursor = csr
        End Try
    End Sub

    Private Sub ServiceStartMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) _
    Handles ServiceStartMenuItem.Click
        Dim csr As Cursor = Me.Cursor
        Try
            Me.Cursor = Cursors.WaitCursor
            'TabForms.Hide()
            MenuStripMother.Enabled = False
            StartSQLClueService()
        Catch ex As Exception
            HandleException(ex)
        Finally
            MenuStripMother.Enabled = True
            'TabForms.Show()
            Me.Cursor = csr
        End Try
    End Sub

    Private Sub ServicePauseMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) _
    Handles ServicePauseMenuItem.Click
        Dim csr As Cursor = Me.Cursor
        Try
            Me.Cursor = Cursors.WaitCursor
            'TabForms.Hide()
            MenuStripMother.Enabled = False
            PauseSQLClueService()
        Catch ex As Exception
            HandleException(ex)
        Finally
            MenuStripMother.Enabled = True
            'TabForms.Show()
            Me.Cursor = csr
        End Try
    End Sub

    Private Sub ServiceContinueMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) _
    Handles ServiceContinueMenuItem.Click
        Dim csr As Cursor = Me.Cursor
        Try
            Me.Cursor = Cursors.WaitCursor
            'TabForms.Hide()
            MenuStripMother.Enabled = False
            ContinueSQLClueService()
        Catch ex As Exception
            HandleException(ex)
        Finally
            MenuStripMother.Enabled = True
            'TabForms.Show()
            Me.Cursor = csr
        End Try
    End Sub

    Private Sub ServiceStopMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) _
    Handles ServiceStopMenuItem.Click
        Dim csr As Cursor = Me.Cursor
        Try
            Me.Cursor = Cursors.WaitCursor
            'TabForms.Hide()
            MenuStripMother.Enabled = False
            StopSQLClueService()
        Catch ex As Exception
            HandleException(ex)
        Finally
            MenuStripMother.Enabled = True
            'TabForms.Show()
            Me.Cursor = csr
        End Try
    End Sub

    Private Function StartSQLClueService() As Boolean
        Dim SvcCtl As ServiceController = Nothing
        Try
            StartSQLClueService = False
            SplashScreen1.ToggleSplashVisible()
            SplashScreen1.CurrentStatusEventHandler(My.Resources.ServiceStartupMessage)
            My.Application.DoEvents()
            'use function to check status to avoid error and to get current serviceaccount
            'service stop and start should be rare events
            If GetServiceStatus() = ServiceControllerStatus.Stopped.ToString Then
                SvcCtl = New ServiceController(My.Resources.SQLClueService, My.Computer.Name)
                SetServiceSettings()
                SvcCtl.Start(ServiceSettings)
                SvcCtl.WaitForStatus(ServiceControllerStatus.Running, TimeSpan.FromSeconds(30))
                If SvcCtl.Status <> ServiceControllerStatus.Running Then
                    Throw New Exception(My.Resources.ServiceDidNotStartMessage)
                End If
                StartSQLClueService = True
            End If
        Catch ex As Exception
            Throw New Exception(String.Format("({0}.StartSQLClueService) Exception.", My.Application.Info.ProductName), ex)
        Finally
            If Not SvcCtl Is Nothing Then
                If Not SvcCtl.ServiceHandle.IsClosed Then
                    SvcCtl.Close()
                End If
                SvcCtl.Dispose()
                SvcCtl = Nothing
            End If
            SplashScreen1.CurrentStatusEventHandler("")
            SplashScreen1.ToggleSplashVisible()
        End Try
    End Function

    Private Function PauseSQLClueService() As Boolean
        Dim SvcCtl As ServiceController = Nothing
        Try
            PauseSQLClueService = False
            SplashScreen1.ToggleSplashVisible()
            SplashScreen1.CurrentStatusEventHandler("Pausing SQLClue Service...")
            'use function to check status to avoid error and to get current serviceaccount
            'service stop and start should be rare events
            If GetServiceStatus() = ServiceControllerStatus.Running.ToString Then
                SvcCtl = New ServiceController(My.Resources.SQLClueService, My.Computer.Name)
                SvcCtl.Pause()
                SvcCtl.WaitForStatus(ServiceControllerStatus.Paused, TimeSpan.FromSeconds(30))
                If Not SvcCtl.Status = ServiceControllerStatus.Paused Then
                    Throw New Exception("The Service could not be paused at this time.")
                End If
                PauseSQLClueService = True
            End If
        Catch ex As Exception
            Throw New Exception(String.Format("({0}.PauseSQLClueService) Exception.", My.Application.Info.ProductName), ex)
        Finally
            If Not SvcCtl Is Nothing Then
                If Not SvcCtl.ServiceHandle.IsClosed Then
                    SvcCtl.Close()
                End If
                SvcCtl.Dispose()
                SvcCtl = Nothing
            End If
            SplashScreen1.CurrentStatusEventHandler("")
            SplashScreen1.ToggleSplashVisible()
        End Try
    End Function

    Private Function ContinueSQLClueService() As Boolean
        Dim SvcCtl As ServiceController = Nothing
        Try
            ContinueSQLClueService = False
            SplashScreen1.CurrentStatusEventHandler("Continuing SQLClue Service....")
            SplashScreen1.ToggleSplashVisible()
            SvcCtl = New ServiceController(My.Resources.SQLClueService, My.Computer.Name)
            'use function to check status to avoid error and to get current serviceaccount
            'service stop and start should be rare events
            If GetServiceStatus() = ServiceControllerStatus.Paused.ToString Then
                SvcCtl.Continue()
                SvcCtl.WaitForStatus(ServiceControllerStatus.Running, TimeSpan.FromSeconds(30))
                If Not SvcCtl.Status = ServiceControllerStatus.Running Then
                    Throw New Exception("The Service could not continue at this time.")
                End If
                ContinueSQLClueService = True
            End If
        Catch ex As Exception
            Throw New Exception(String.Format("({0}.ContinueSQLClueService) Exception.", My.Application.Info.ProductName), ex)
        Finally
            If Not SvcCtl Is Nothing Then
                If Not SvcCtl.ServiceHandle.IsClosed Then
                    SvcCtl.Close()
                End If
                SvcCtl.Dispose()
                SvcCtl = Nothing
            End If
            SplashScreen1.ToggleSplashVisible()
            SplashScreen1.CurrentStatusEventHandler("")
        End Try
    End Function

    Private Function StopSQLClueService() As Boolean
        Dim SvcCtl As ServiceController = Nothing
        Try
            StopSQLClueService = False
            SplashScreen1.CurrentStatusEventHandler("Stopping SQLClue Service....")
            SplashScreen1.ToggleSplashVisible()
            SvcCtl = New ServiceController(My.Resources.SQLClueService, My.Computer.Name)
            'use function to check status to avoid error and to get current serviceaccount
            'service stop and start should be rare events
            If GetServiceStatus() = ServiceControllerStatus.Running.ToString Then
                SvcCtl.Stop()
                SvcCtl.WaitForStatus(ServiceControllerStatus.Stopped, TimeSpan.FromSeconds(30))
                If Not SvcCtl.Status = ServiceControllerStatus.Stopped Then
                    Throw New Exception("The Service could not be stopped at this time.")
                End If
                StopSQLClueService = True
            End If
        Catch ex As Exception
            Throw New Exception(String.Format("({0}.StopSQLClueService) Exception.", My.Application.Info.ProductName), ex)
        Finally
            If Not SvcCtl Is Nothing Then
                If Not SvcCtl.ServiceHandle.IsClosed Then
                    SvcCtl.Close()
                End If
                SvcCtl.Dispose()
                SvcCtl = Nothing
            End If
            SplashScreen1.ToggleSplashVisible()
            SplashScreen1.CurrentStatusEventHandler("")
        End Try
    End Function

    Private Sub ServiceEnable()
        Try
            If My.Settings.ServiceEnabled = False Then
                ' the uninstall has been run so toggle enabled switch before anything else
                My.Settings.ServiceEnabled = True
                My.Settings.Save()
            End If
            ServiceInstallMenuItem.Enabled = False
            ServiceUninstallMenuItem.Enabled = True
            ToolStripMenuItemServiceTimerInterval.Enabled = True
            ToolStripMenuItemDocumentMonitorSleep.Enabled = True
            ToolStripMenuItemWaitBetweenDocumentsSeconds.Enabled = True
            ServiceToolStripMenuItem.Checked = True
        Catch ex As Exception
            Throw New Exception(String.Format("({0}.ServiceEnable) Exception.", My.Application.Info.ProductName), ex)
        End Try
    End Sub

    Private Sub ServiceDisable()
        Try
            If My.Settings.ServiceEnabled = True Then
                ' the uninstall has been run so toggle enabled switch before anything else
                My.Settings.ServiceEnabled = False
                My.Settings.Save()
            End If
            ServiceInstallMenuItem.Enabled = True
            ServiceUninstallMenuItem.Enabled = False
            ServiceToolStripMenuItem.Checked = False
            ToolStripMenuItemServiceTimerInterval.Enabled = False
            ToolStripMenuItemDocumentMonitorSleep.Enabled = False
            ToolStripMenuItemWaitBetweenDocumentsSeconds.Enabled = False
        Catch ex As Exception
            Throw New Exception(String.Format("({0}.ServiceDisable) Exception.", My.Application.Info.ProductName), ex)
        End Try
    End Sub

    Private Sub ServiceToolStripMenuItem_DropDownOpening(ByVal sender As Object, _
                                                         ByVal e As System.EventArgs) _
    Handles ServiceToolStripMenuItem.DropDownOpening
        Try
            GetServiceStatus()
        Catch ex As Exception
            HandleException(ex)
        End Try
    End Sub

    Private Sub ToolStripMenuItemServiceTimerInterval_Click(ByVal sender As Object, ByVal e As System.EventArgs) _
    Handles ToolStripMenuItemServiceTimerInterval.Click
        Try
            Dim r As DialogResult = DialogServiceTimerInterval.ShowDialog(Me)
            If r = Windows.Forms.DialogResult.OK Then
                SaveSettingsAndSendToService()
            End If
        Catch ex As Exception
            HandleException(ex)
        End Try
    End Sub

    Private Sub ToolStripMenuItemDocumentMonitorSleep_Click(ByVal sender As Object, ByVal e As System.EventArgs) _
    Handles ToolStripMenuItemDocumentMonitorSleep.Click
        Try
            Dim r As DialogResult = DialogDocumentMonitorInterval.ShowDialog(Me)
            If r = Windows.Forms.DialogResult.OK Then
                SaveSettingsAndSendToService()
            End If
        Catch ex As Exception
            HandleException(ex)
        End Try
    End Sub

    Private Sub ToolStripMenuItemWaitBetweenDocumentsSeconds_Click(ByVal sender As Object, ByVal e As System.EventArgs) _
    Handles ToolStripMenuItemWaitBetweenDocumentsSeconds.Click
        Try
            Dim r As DialogResult = DialogDocumentInterval.ShowDialog(Me)
            If r = Windows.Forms.DialogResult.OK Then
                SaveSettingsAndSendToService()
            End If
        Catch ex As Exception
            HandleException(ex)
        End Try
    End Sub

#End Region

#Region " Report Viewer "

    Private Sub ReportViewerLoad()
        Try
            ReportViewerForm.MdiParent = Me
            ReportViewerForm.Dock = DockStyle.Fill
            ReportViewerForm.Show()
        Catch ex As Exception
            Throw New Exception(String.Format("({0}.ReportViewerLoad) Exception.", My.Application.Info.ProductName), ex)
        End Try
    End Sub

    Private Sub ToolStripMenuItemDashboard_Click(ByVal sender As Object, ByVal e As System.EventArgs) _
    Handles ToolStripMenuItemDashboard.Click
        Try
            ' supply populated datasets
            If ToolStripMenuItemDashboard.Enabled Then
                Me.TabForms.SelectTab(ReportViewerForm.Name & "Tab")
                My.Forms.ReportViewerForm.LoadDashboard(3, 30, 365, DateAdd(DateInterval.Day, 1, Today))
            End If
        Catch ex As Exception
            HandleException(ex)
        End Try
    End Sub

    Private Sub ToolStripMenuItemScheduledTasks_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ToolStripMenuItemScheduledTasks.Click
        Try
            If ToolStripMenuItemDashboard.Enabled Then
                Me.TabForms.SelectTab(ReportViewerForm.Name & "Tab")
                My.Forms.ReportViewerForm.LoadScheduledTasks()
            End If
        Catch ex As Exception
            HandleException(ex)
        End Try
    End Sub

    Private Sub ToolStripMenuItemRunbookCatalog_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ToolStripMenuItemRunbookCatalog.Click
        Try
            If ToolStripMenuItemDashboard.Enabled Then
                Me.TabForms.SelectTab(ReportViewerForm.Name & "Tab")
                My.Forms.ReportViewerForm.LoadRunbookCatalog(Nothing, Nothing, 0, "none")
            End If
        Catch ex As Exception
            HandleException(ex)
        End Try
    End Sub

    Private Sub ToolStripMenuItemConfigurationCatalog_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ToolStripMenuItemConfigurationCatalog.Click
        Try
            If ToolStripMenuItemDashboard.Enabled Then
                Me.TabForms.SelectTab(ReportViewerForm.Name & "Tab")
                ' could make the parms settings to allow user to choose default load
                My.Forms.ReportViewerForm.ComboBoxCfgInstance.Items.Clear()
                My.Forms.ReportViewerForm.LoadConfigurationCatalog("", "", "Metadata", "License", "SQLCfg.tSQLCfg", "License", "SQLCfgMetadata|SQLCfg.tSQLCfg", "", "none")
            End If
        Catch ex As Exception
            HandleException(ex)
        End Try
    End Sub

    Private Sub ToolStripMenuItemConfigurationSummary_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) _
    Handles ToolStripMenuItemConfigurationSummary.Click
        Try
            Me.TabForms.SelectTab(ReportViewerForm.Name & "Tab")
            My.Forms.ReportViewerForm.LoadSQLConfigurationSummary(30, "All")
        Catch ex As Exception
            HandleException(ex)
        End Try
    End Sub

    Private Sub ToolStripMenuItemViewArchiveByInstance_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ToolStripMenuItemViewArchiveByInstance.Click
        Try
            Me.TabForms.SelectTab(ReportViewerForm.Name & "Tab")
            My.Forms.ReportViewerForm.LoadArchiveByInstance("")
        Catch ex As Exception
            HandleException(ex)
        End Try
    End Sub

    Private Sub ToolStripMenuItemRunbookUsers_Click(ByVal sender As System.Object, _
                                                    ByVal e As System.EventArgs) _
    Handles ToolStripMenuItemRunbookUsers.Click
        Dim csr As Cursor = Me.Cursor ' Save the old cursor
        Try
            Me.Cursor = Cursors.WaitCursor
            Me.TabForms.SelectTab(ReportViewerForm.Name & "Tab")
            My.Forms.ReportViewerForm.LoadSQLRunbookContributorScoring()
        Catch ex As Exception
            HandleException(ex)
        Finally
            Me.Cursor = csr
        End Try
    End Sub

    Private Sub ToolStripMenuItemIFilters_Click(ByVal sender As System.Object, _
                                                ByVal e As System.EventArgs) _
    Handles ToolStripMenuItemIFilters.Click
        Dim csr As Cursor = Me.Cursor ' Save the old cursor
        Try
            Me.Cursor = Cursors.WaitCursor
            Me.TabForms.SelectTab(ReportViewerForm.Name & "Tab")
            My.Forms.ReportViewerForm.LoadAvailableIFilters("all")
        Catch ex As Exception
            HandleException(ex)
        Finally
            Me.Cursor = csr
        End Try
    End Sub

    Private Sub ToolStripMenuItemWorkstationReport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) _
    Handles ToolStripMenuItemWorkstationReport.Click
        Dim csr As Cursor = Me.Cursor
        Try
            Me.Cursor = Cursors.WaitCursor
            Me.TabForms.SelectTab(ReportViewerForm.Name & "Tab")
            My.Forms.ReportViewerForm.LoadSplashReport("Workstation")
        Catch ex As Exception
            HandleException(ex)
        Finally
            Me.Cursor = csr
        End Try
    End Sub

    Private Sub ToolStripMenuItemConsoleReport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) _
    Handles ToolStripMenuItemConsoleReport.Click
        Dim csr As Cursor = Me.Cursor
        Try
            Me.Cursor = Cursors.WaitCursor
            Me.TabForms.SelectTab(ReportViewerForm.Name & "Tab")
            My.Forms.ReportViewerForm.LoadSplashReport("Console")
        Catch ex As Exception
            HandleException(ex)
        Finally
            Me.Cursor = csr
        End Try
    End Sub

    Private Sub ToolStripMenuItemArchiveScriptingSettings_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ToolStripMenuItemArchiveScriptingSettings.Click
        Dim csr As Cursor = Me.Cursor
        Try
            Me.Cursor = Cursors.WaitCursor
            Me.TabForms.SelectTab(ReportViewerForm.Name & "Tab")
            My.Forms.ReportViewerForm.LoadArchiveSettings()
        Catch ex As Exception
            HandleException(ex)
        Finally
            Me.Cursor = csr
        End Try
    End Sub

    Private Sub ToolStripMenuItemSearchRunbook_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ToolStripMenuItemSearchRunbook.Click
        Dim csr As Cursor = Me.Cursor
        Try
            Me.Cursor = Cursors.WaitCursor
            Me.TabForms.SelectTab(ReportViewerForm.Name & "Tab")
            My.Forms.ReportViewerForm.LoadRunbookSearch()
        Catch ex As Exception
            HandleException(ex)
        Finally
            Me.Cursor = csr
        End Try
    End Sub

    Private Sub ToolStripMenuItemSearchArchive_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ToolStripMenuItemSearchArchive.Click
        Dim csr As Cursor = Me.Cursor
        Try
            Me.Cursor = Cursors.WaitCursor
            Me.TabForms.SelectTab(ReportViewerForm.Name & "Tab")
            My.Forms.ReportViewerForm.LoadArchiveSearch("", True, My.Resources.OriginSQLInstance)
        Catch ex As Exception
            HandleException(ex)
        Finally
            Me.Cursor = csr
        End Try
    End Sub

    Private Sub ToolStripMenuItemArchiveErrors_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItemArchiveErrors.Click
        Dim csr As Cursor = Me.Cursor
        Try
            Me.Cursor = Cursors.WaitCursor
            Me.TabForms.SelectTab(ReportViewerForm.Name & "Tab")
            My.Forms.ReportViewerForm.LoadRecentArchiveSQLError(My.Settings.ArchiveErrorsDaysToShow)
        Catch ex As Exception
            HandleException(ex)
        Finally
            Me.Cursor = csr
        End Try
    End Sub


#End Region

#Region " Tutorials "

    Private Sub RunbookContributorTutorialToolStripMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RunbookContributorTutorialToolStripMenuItem.Click
        HelpForm.Show("RunbookContributorTutorial.htm")
        Me.TabForms.SelectTab(HelpForm.Name & "Tab")
    End Sub

    Private Sub RunbookPeerReviewTotorialToolStripMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RunbookRatingTotorialToolStripMenuItem.Click
        HelpForm.Show("RunbookPeerReviewTutorial.htm")
        Me.TabForms.SelectTab(HelpForm.Name & "Tab")
    End Sub

    Private Sub RunbookUserTutorialToolStripMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RunbookUserTutorialToolStripMenuItem.Click
        HelpForm.Show("RunbookSearchTutorial.htm")
        Me.TabForms.SelectTab(HelpForm.Name & "Tab")
    End Sub

    Private Sub ArchiveTutorialToolStripMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ArchiveTutorialToolStripMenuItem.Click
        HelpForm.Show("ConfigurationArchiveTutorial.htm")
        Me.TabForms.SelectTab(HelpForm.Name & "Tab")
    End Sub

    Private Sub CompareTutorialToolStripMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CompareTutorialToolStripMenuItem.Click
        HelpForm.Show("ConfigurationCompareTutorial.htm")
        Me.TabForms.SelectTab(HelpForm.Name & "Tab")
    End Sub

#End Region

    Private Sub ToolStripComboBoxDiscoveryScope_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ToolStripComboBoxDiscoveryScope.TextChanged
        My.Settings.ServerDiscoveryScope = ToolStripComboBoxDiscoveryScope.Text
        My.Settings.Save()
    End Sub

End Class

