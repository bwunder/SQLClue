Public Class ReportViewerForm

    Private Sub ReportingForm_Load(ByVal sender As System.Object, _
                                   ByVal e As System.EventArgs) _
                                   Handles MyBase.Load
        Try
            RemoveHandler ReportViewerSQLClue.Drillthrough, AddressOf SQLClueDrillthroughEventHandler
            AddHandler ReportViewerSQLClue.Drillthrough, AddressOf SQLClueDrillthroughEventHandler
            RemoveHandler ReportViewerSQLClue.LocalReport.SubreportProcessing, AddressOf SQLClueSubreportProcessingEventHandler
            AddHandler ReportViewerSQLClue.LocalReport.SubreportProcessing, AddressOf SQLClueSubreportProcessingEventHandler
            SplitContainerReportViewer.Panel1Collapsed = True
            FlowLayoutPanelParms.Controls.Clear()
            If My.Settings.RepositoryEnabled Then
                LoadSplashReport("Console")
            Else
                LoadSplashReport("Workstation")
            End If
            ToolStripStatusLabel1.Text = My.Resources.Ready
        Catch ex As Exception
            Mother.HandleException(ex)
        End Try
    End Sub

    Private Sub ButtonViewReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonViewReport.Click
        Try
            ' this can't just load 
            If FlowLayoutPanelParms.Contains(PanelSQLInstance) Then
                LoadArchiveByInstance(ComboBoxSQLInstance.Text)
            ElseIf FlowLayoutPanelParms.Contains(PanelDays) Then
                LoadSQLConfigurationSummary(CInt(NumericUpDownDays.Value), "All")
            ElseIf FlowLayoutPanelParms.Contains(PanelLogins) Then
                'LoadSQLRunbookUser(ComboBoxLoginNames.Text)
            ElseIf FlowLayoutPanelParms.Contains(PanelSQLInstance) Then
                ' make sure the node exists else don't even bother
                LoadConfigurationCatalog(ComboBoxSQLInstance.Text, ComboBoxDatabase.Text, ComboBoxType.Text, _
                                         ComboBoxSubType.Text, ComboBoxCollection.Text, ComboBoxItem.Text, _
                                         ComboBoxItem.Tag.ToString, ComboBoxVersion.Text, "none")
            ElseIf FlowLayoutPanelParms.Contains(PanelSearch) Then
                LoadRunbookSearch(TextBoxSearchString.Text)
            ElseIf FlowLayoutPanelParms.Contains(PanelSearch) Then
                LoadArchiveSearch(TextBoxSearchString.Text, CBool(ComboBoxSearchLatest.Text), ComboBoxType.Text)
            End If
        Catch ex As Exception
            Mother.HandleException(ex)
        End Try
    End Sub

#Region " SQLClue Reports "

    Friend Sub LoadSplashReport(ByVal SplashType As String)
        Try
            ' no report parms 
            SplitContainerReportViewer.Panel1Collapsed = True
            FlowLayoutPanelParms.Controls.Clear()
            Me.ReportViewerSQLClue.ZoomMode = ZoomMode.FullPage
            Me.ReportViewerSQLClue.ShowToolBar = False
            Me.ReportViewerSQLClue.Reset()
            Me.ReportViewerSQLClue.ProcessingMode = ProcessingMode.Local
            Me.ReportViewerSQLClue.LocalReport.DataSources.Clear()
            Dim CurrentVersion As ReportParameter = New ReportParameter("CurrentVersion", My.Application.Info.Version.ToString)
            If My.Settings.RepositoryEnabled _
            And SplashType = "Console" Then
                If Mother.DAL.dsSQLCfg.tSQLCfg.Count = 0 Then
                    Mother.DAL.LoadSQLCfg()
                End If
                ' get the admins friendly name if there is one
                ' uses db collation so case insensitve is default
                Me.ReportViewerSQLClue.LocalReport.DataSources.Add(New ReportDataSource("dsSQLConfiguration_tSQLCfg", _
                                                                   CType(Mother.DAL.dsSQLCfg.tSQLCfg, DataTable)))
                Me.ReportViewerSQLClue.LocalReport.ReportPath = My.Application.Info.DirectoryPath & "\ReportViewerReports\SQLClueConsole.rdlc"
                Dim CurrentLicenseLevel As ReportParameter = New ReportParameter("CurrentInstanceCount", Mother.LicensedInstanceList.Count.ToString)
                Me.ReportViewerSQLClue.LocalReport.SetParameters(New ReportParameter() {CurrentLicenseLevel, CurrentVersion})
            Else
                Me.ReportViewerSQLClue.LocalReport.ReportPath = My.Application.Info.DirectoryPath & "\ReportViewerReports\SQLClueWorkstation.rdlc"
                Me.ReportViewerSQLClue.LocalReport.SetParameters(New ReportParameter() {CurrentVersion})
            End If
            'this will refresh the report in the viewer with the above settings
            Me.ReportViewerSQLClue.RefreshReport()
            ToolStripStatusLabel1.Text = My.Resources.Ready
            RemoveHandler ReportViewerSQLClue.Drillthrough, AddressOf SQLClueDrillthroughEventHandler
        Catch ex As Exception
            Mother.HandleException(ex)
        End Try
    End Sub

    Friend Sub LoadDashboard(ByVal Level1Days As Integer, _
                             ByVal Level2Days As Integer, _
                             ByVal Level3Days As Integer, _
                             ByVal EndDt As DateTime)
        Try
            If My.Settings.RepositoryEnabled Then
                ReportViewerSQLClue.ZoomMode = ZoomMode.Percent
                ReportViewerSQLClue.ZoomPercent = 100
                Me.ReportViewerSQLClue.ShowToolBar = True
                SplitContainerReportViewer.Panel1Collapsed = True
                FlowLayoutPanelParms.Controls.Clear()
                Me.ReportViewerSQLClue.Reset()
                Me.ReportViewerSQLClue.ProcessingMode = ProcessingMode.Local
                Me.ReportViewerSQLClue.LocalReport.DataSources.Clear()
                Me.ReportViewerSQLClue.LocalReport.ReportPath = My.Application.Info.DirectoryPath & "\ReportViewerReports\SqlClueDashboard.rdlc"
                Dim Parms(3) As ReportParameter
                Parms(0) = New ReportParameter("Level1Days", Level1Days.ToString)
                Parms(1) = New ReportParameter("Level2Days", Level2Days.ToString)
                Parms(2) = New ReportParameter("Level3Days", Level3Days.ToString)
                Parms(3) = New ReportParameter("EndDt", EndDt.ToString)
                Me.ReportViewerSQLClue.LocalReport.SetParameters(Parms)
                RemoveHandler ReportViewerSQLClue.Drillthrough, AddressOf SQLClueDrillthroughEventHandler
                AddHandler ReportViewerSQLClue.Drillthrough, AddressOf SQLClueDrillthroughEventHandler
                RemoveHandler ReportViewerSQLClue.LocalReport.SubreportProcessing, AddressOf SQLClueSubreportProcessingEventHandler
                AddHandler ReportViewerSQLClue.LocalReport.SubreportProcessing, AddressOf SQLClueSubreportProcessingEventHandler
                Me.ReportViewerSQLClue.RefreshReport()
            End If
            ToolStripStatusLabel1.Text = My.Resources.Ready
        Catch ex As Exception
            Mother.HandleException(ex)
        End Try
    End Sub

    Friend Sub LoadDefaultTrace(ByVal SQLInstance As String, _
                                ByVal BeginDt As DateTime, _
                                ByVal EndDt As DateTime)
        Try
            ReportViewerSQLClue.ZoomMode = ZoomMode.Percent
            ReportViewerSQLClue.ZoomPercent = 100
            Me.ReportViewerSQLClue.ShowToolBar = True
            SplitContainerReportViewer.Panel1Collapsed = False
            FlowLayoutPanelParms.Controls.Clear()

            ' select an instance 
            Dim Target As Server = connSQLInstance()
            If (Target.Configuration.DefaultTraceEnabled.ConfigValue = 0) Then
                Throw New Exception("Default Trace not enabled on this SQL instance.")
            End If

            Me.ReportViewerSQLClue.Reset()
            Me.ReportViewerSQLClue.ProcessingMode = ProcessingMode.Local
            Me.ReportViewerSQLClue.LocalReport.DataSources.Clear()
            Dim db As New DefaultTraceContainer
            db.Configuration.AutoDetectChangesEnabled = False

            'needed only if SQLQuery element type omitted

            db.Database.Connection.ConnectionString = Target.ConnectionContext.ConnectionString

            Dim QueryParms(2) As Object
            QueryParms(0) = New ReportParameter("BeginDt", BeginDt.ToString)
            QueryParms(1) = New ReportParameter("EndDt", EndDt.ToString)

            db.Database.Connection.Open()
            db.TraceEvents.SqlQuery(My.Resources.DefaultTraceEventsQuery, QueryParms)
            ' TODO - populate the lookups from the context not the db, keep work off target SQL
            db.Databases.SqlQuery(String.Format(My.Resources.DefaultTraceDatabasesQuery, QueryParms))
            db.Applications.SqlQuery(My.Resources.DefaultTraceApplicationsQuery, QueryParms)
            db.Logins.SqlQuery(My.Resources.DefaultTraceLoginsQuery, QueryParms)
            db.Hosts.SqlQuery(My.Resources.DefaultTraceHostsQuery, QueryParms)

            Me.ReportViewerSQLClue.LocalReport.DataSources.Add(New ReportDataSource("Databases", db.Databases))
            Me.ReportViewerSQLClue.LocalReport.DataSources.Add(New ReportDataSource("Logins", db.Logins))
            Me.ReportViewerSQLClue.LocalReport.DataSources.Add(New ReportDataSource("Applications", db.Applications))
            Me.ReportViewerSQLClue.LocalReport.DataSources.Add(New ReportDataSource("Hosts", db.Hosts))
            Me.ReportViewerSQLClue.LocalReport.DataSources.Add(New ReportDataSource("TraceEvents", db.TraceEvents))

            Me.ReportViewerSQLClue.LocalReport.ReportPath = My.Application.Info.DirectoryPath & "\ReportViewerReports\SqlClueDefaultTrace.rdlc"
            Dim Parms(3) As ReportParameter
            Parms(0) = New ReportParameter("SQLInstance", Target.ConnectionContext.TrueName)
            Parms(1) = New ReportParameter("BeginDt", BeginDt.ToString)
            Parms(2) = New ReportParameter("EndDt", EndDt.ToString)
            Me.ReportViewerSQLClue.LocalReport.SetParameters(Parms)
            RemoveHandler ReportViewerSQLClue.Drillthrough, AddressOf SQLClueDrillthroughEventHandler
            AddHandler ReportViewerSQLClue.Drillthrough, AddressOf SQLClueDrillthroughEventHandler
            RemoveHandler ReportViewerSQLClue.LocalReport.SubreportProcessing, AddressOf SQLClueSubreportProcessingEventHandler
            AddHandler ReportViewerSQLClue.LocalReport.SubreportProcessing, AddressOf SQLClueSubreportProcessingEventHandler
            Me.ReportViewerSQLClue.RefreshReport()
            ToolStripStatusLabel1.Text = My.Resources.Ready
        Catch ex As Exception
            Mother.HandleException(ex)
        End Try
    End Sub

    Private Function connSQLInstance() As Server
        Try
            connSQLInstance = New Server()
            Dim cn As DialogConnect = New DialogConnect
            ' a bit hokey to use the compare connection as default, but still must click OK before connection
            Dim result As DialogResult = cn.ShowDialog(connSQLInstance, _
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
                ' if the server just connected is not in the InstanceList, add it
                If Not Mother.InstanceList.Contains(UCase(My.Settings.SQL__Instance__A_Name_1)) Then
                    ' the array is 0 based, so redim to the length is perfect for add one
                    ReDim Preserve Mother.InstanceList(Mother.InstanceList.Length)
                    Mother.InstanceList(Mother.InstanceList.Length - 1) = UCase(My.Settings.SQL__Instance__A_Name_1)
                    ' reload the combo drop down
                    ComboBoxSQLInstance.Items.Clear()
                End If
                ' no need to reload if already identical
                If ComboBoxSQLInstance.Items.Count = 0 Then
                    ComboBoxSQLInstance.Items.AddRange(Mother.InstanceList)
                    ComboBoxSQLInstance.Sorted = True
                End If
                '? in compare this fires the Instance1.SelectedIndexChanged will it need to do that here?
                ComboBoxSQLInstance.Text = My.Settings.SQL__Instance__A_Name_1
                Me.ToolStripStatusLabel1.Text = String.Format(My.Resources.ConnectedToSQL, _
                                                              ToolStripStatusLabel1.Tag, _
                                                              My.Settings.SQL__Instance__A_Name_1)
            Else
                Me.ToolStripStatusLabel1.Text = String.Format(My.Resources.ConnectionToSQLFailed, _
                                                              ToolStripStatusLabel1.Tag, _
                                                              ComboBoxSQLInstance.Text)
            End If
            Return connSQLInstance
        Catch ex As Exception
            Me.ToolStripStatusLabel1.Text = String.Format(My.Resources.ConnectionToSQLFailed, _
                                                          ToolStripStatusLabel1.Tag, _
                                                          ComboBoxSQLInstance.Text)
            Mother.HandleException(ex)
            Return Nothing
        End Try
    End Function


    Public Sub SQLClueDrillthroughEventHandler(ByVal sender As Object, ByVal e As DrillthroughEventArgs)
        Dim rpt As LocalReport
        rpt = CType(e.Report, LocalReport)
        Select Case e.ReportPath
            Case "SQLClueDashboard"
                e.Cancel = True
                LoadDashboard(CInt(e.Report.GetParameters("Level2Days").Values(0)), _
                              CInt(e.Report.GetParameters("Level2Days").Values(0)), _
                              CInt(e.Report.GetParameters("Level3Days").Values(0)), _
                              CDate(e.Report.GetParameters("EndDt").Values(0)))
            Case "DefaultTrace"
                e.Cancel = True
                LoadDefaultTrace(e.Report.GetParameters("SQLInstance").Values(0).ToString, _
                                 CDate(e.Report.GetParameters("BeginDt").Values(0)), _
                                 CDate(e.Report.GetParameters("EndDt").Values(0)))
            Case "SQLConfigurationArchiveByInstance"
                Using TableAdapterSQLCfgChanges As New cCommon.dsSQLConfigurationTableAdapters.tChangeTableAdapter
                    TableAdapterSQLCfgChanges.Connection.ConnectionString = Mother.DAL.LocalRepositoryConnectionString
                    TableAdapterSQLCfgChanges.ClearBeforeFill = True
                    Dim dt As New cCommon.dsSQLConfiguration.tChangeDataTable
                    TableAdapterSQLCfgChanges.FillByInstance(dt, _
                                                             e.Report.GetParameters("SQLInstance").Values(0))
                    rpt.DataSources.Add(New ReportDataSource("AllNodesForInstance", CType(dt, DataTable)))
                End Using
                ' works but loose 'back' link
                'e.Cancel = True
                'LoadArchiveByInstance(e.Report.GetParameters("SQLInstance").Values(0))
            Case "SQLConfigurationArchiveHistoryByScheduleId"
                Using TableAdapterArchiveHistory As New cCommon.dsSQLConfigurationTableAdapters.tArchiveLogTableAdapter
                    TableAdapterArchiveHistory.Connection.ConnectionString = Mother.DAL.LocalRepositoryConnectionString
                    TableAdapterArchiveHistory.ClearBeforeFill = True
                    Dim dt As New cCommon.dsSQLConfiguration.tArchiveLogDataTable
                    TableAdapterArchiveHistory.FillByScheduleId(dt, _
                                                    CInt(e.Report.GetParameters("ScheduleId").Values(0)), _
                                                    CDate(e.Report.GetParameters("BeginDt").Values(0)), _
                                                    CDate(e.Report.GetParameters("EndDt").Values(0)))
                    rpt.DataSources.Add(New ReportDataSource("ArchiveHistoryList", CType(dt, DataTable)))
                End Using
            Case "SQLConfigurationArchiveHistoryBySQLInstanceByScheduleId"
                Using TableAdapterArchiveHistory As New cCommon.dsSQLConfigurationTableAdapters.tArchiveLogTableAdapter
                    TableAdapterArchiveHistory.Connection.ConnectionString = Mother.DAL.LocalRepositoryConnectionString
                    TableAdapterArchiveHistory.ClearBeforeFill = True
                    Dim dt As New cCommon.dsSQLConfiguration.tArchiveLogDataTable
                    TableAdapterArchiveHistory.FillBySQLInstanceByScheduleId(dt, _
                                                    CInt(e.Report.GetParameters("ScheduleId").Values(0)), _
                                                    CDate(e.Report.GetParameters("BeginDt").Values(0)), _
                                                    CDate(e.Report.GetParameters("EndDt").Values(0)))
                    rpt.DataSources.Add(New ReportDataSource("ArchiveHistoryList", CType(dt, DataTable)))
                End Using
            Case "SQLConfigurationArchiveHistoryByInstanceName"
                Using TableAdapterArchiveHistory As New cCommon.dsSQLConfigurationTableAdapters.tArchiveLogTableAdapter
                    TableAdapterArchiveHistory.Connection.ConnectionString = Mother.DAL.LocalRepositoryConnectionString
                    TableAdapterArchiveHistory.ClearBeforeFill = True
                    Dim dt As New cCommon.dsSQLConfiguration.tArchiveLogDataTable
                    TableAdapterArchiveHistory.Fill(dt, _
                                                    e.Report.GetParameters("SQLInstance").Values(0), _
                                                    CDate(e.Report.GetParameters("BeginDt").Values(0)), _
                                                    CDate(e.Report.GetParameters("EndDt").Values(0)))
                    rpt.DataSources.Add(New ReportDataSource("ArchiveHistoryList", CType(dt, DataTable)))
                End Using
            Case "SQLConfigurationChangesForDate"
                Dim ChangeDate As DateTime = CDate(e.Report.GetParameters("ChangeDate").Values(0))
                'Dim RootNode = e.Report.GetParameters("RootNode").Values(0)
                Using TableAdapterChangesForDate As New cCommon.dsSQLConfigurationTableAdapters.pChangesForDateTableAdapter
                    TableAdapterChangesForDate.Connection.ConnectionString = Mother.DAL.LocalRepositoryConnectionString
                    TableAdapterChangesForDate.ClearBeforeFill = True
                    Dim dt As New cCommon.dsSQLConfiguration.pChangesForDateDataTable
                    TableAdapterChangesForDate.Fill(dt, _
                                                    CDate(e.Report.GetParameters("ChangeDate").Values(0)), _
                                                    e.Report.GetParameters("SQLInstance").Values(0), _
                                                    e.Report.GetParameters("NodeType").Values(0), _
                                                    e.Report.GetParameters("Action").Values(0))
                    rpt.DataSources.Add(New ReportDataSource("ChangeList", CType(dt, DataTable)))
                End Using
            Case "SQLConfigurationChangesForArchive"
                Using TableAdapterChangesForArchive As New cCommon.dsSQLConfigurationTableAdapters.pChangesForArchiveTableAdapter
                    TableAdapterChangesForArchive.Connection.ConnectionString = Mother.DAL.LocalRepositoryConnectionString
                    TableAdapterChangesForArchive.ClearBeforeFill = True
                    Dim dt As New cCommon.dsSQLConfiguration.pChangesForArchiveDataTable
                    TableAdapterChangesForArchive.Fill(dt, _
                                                       CInt(e.Report.GetParameters("ArchiveLogId").Values(0)), _
                                                       e.Report.GetParameters("RootNode").Values(0))
                    rpt.DataSources.Add(New ReportDataSource("ChangeList", CType(dt, DataTable)))
                End Using
            Case "SQLConfigurationCatalog"
                e.Cancel = True
                ' details of selected item
                Dim DefinitionByVersion As New cCommon.dsSQLConfiguration.pChangeSelectDefinitionByVersionDataTable
                Using TableAdapterSQLCfgDefinition As New cCommon.dsSQLConfigurationTableAdapters.pChangeSelectDefinitionByVersionTableAdapter
                    TableAdapterSQLCfgDefinition.Connection.ConnectionString = Mother.DAL.LocalRepositoryConnectionString
                    TableAdapterSQLCfgDefinition.ClearBeforeFill = True
                    TableAdapterSQLCfgDefinition.Fill(DefinitionByVersion, _
                                                      e.Report.GetParameters("Node").Values(0), _
                                                      CInt(e.Report.GetParameters("Version").Values(0)))
                End Using
                Dim msg As String = e.Report.GetParameters("DrillThroughAction").Values(0).ToString
                If InStr(msg, "Compare") = 1 Then
                    My.Settings.SQL__Repository__A_Name_1 = Mother.DAL.RepositoryInstanceName
                    My.Settings.SQL__Repository__A_Database__Name_2 = Mother.DAL.RepositoryDatabaseName
                    My.Settings.SQL__Repository__A_Use__Trusted__Connection_3 = Mother.DAL.RepositoryUseTrustedConnection
                    My.Settings.SQL__Repository__A_SQL__Login__Name_4 = Mother.DAL.RepositorySQLLoginName
                    My.Settings.SQL__Repository__A_SQL__Login__Password_5 = Mother.DAL.RepositorySQLLoginPassword
                    My.Settings.SQL__Repository__A_Connection__Timeout_7 = Mother.DAL.RepositoryConnectionTimeout
                    My.Settings.SQL__Repository__A_Network__Protocol_9 = Mother.DAL.RepositoryNetworkLibrary
                    My.Settings.SQL__Repository__A_Encrypted__Connection_6 = Mother.DAL.RepositoryEncryptConnection
                    My.Settings.SQL__Repository__A_Trust__Server__Certificate_8 = Mother.DAL.RepositoryTrustServerCertificate
                    CompareForm.cCompare.Repository1.ConnectionContext.ConnectionString = Mother.DAL.LocalRepositoryConnectionString
                    CompareForm.Origin1.SelectedItem = My.Resources.OriginRepository
                    'selecting the instance invoke tryloadtreeview
                    CompareForm.Instance1.SelectedItem = CompareForm.cCompare.CrackFullPath(e.Report.GetParameters("Node").Values(0))(0)
                    Dim Nodes1() As TreeNode = CompareForm.smoTreeView1.Nodes.Find(CompareForm.cCompare.CrackFullPath(e.Report.GetParameters("Node").Values(0))(3), True)
                    For Each n As TreeNode In Nodes1
                        If n.FullPath = e.Report.GetParameters("Node").Values(0) Then
                            CompareForm.smoTreeView1.SelectedNode = n
                            Exit For
                        End If
                    Next
                    My.Settings.SQL__Repository__A_Filters_Version = CInt(DefinitionByVersion.Rows(0)("Version"))
                    My.Settings.SQL__Repository__B_Name_1 = Mother.DAL.RepositoryInstanceName
                    My.Settings.SQL__Repository__B_Database__Name_2 = Mother.DAL.RepositoryDatabaseName
                    My.Settings.SQL__Repository__B_Use__Trusted__Connection_3 = Mother.DAL.RepositoryUseTrustedConnection
                    My.Settings.SQL__Repository__B_SQL__Login__Name_4 = Mother.DAL.RepositorySQLLoginName
                    My.Settings.SQL__Repository__B_SQL__Login__Password_5 = Mother.DAL.RepositorySQLLoginPassword
                    My.Settings.SQL__Repository__B_Connection__Timeout_7 = Mother.DAL.RepositoryConnectionTimeout
                    My.Settings.SQL__Repository__B_Network__Protocol_9 = Mother.DAL.RepositoryNetworkLibrary
                    My.Settings.SQL__Repository__B_Encrypted__Connection_6 = Mother.DAL.RepositoryEncryptConnection
                    My.Settings.SQL__Repository__B_Trust__Server__Certificate_8 = Mother.DAL.RepositoryTrustServerCertificate
                    CompareForm.cCompare.Repository2.ConnectionContext.ConnectionString = Mother.DAL.LocalRepositoryConnectionString
                    CompareForm.Origin2.SelectedItem = My.Resources.OriginRepository
                    'selecting the instance invokes tryloadtreeview
                    CompareForm.Instance2.SelectedItem = CompareForm.cCompare.CrackFullPath(e.Report.GetParameters("Node").Values(0))(0)
                    Dim Nodes2() As TreeNode = CompareForm.smoTreeView2.Nodes.Find(CompareForm.cCompare.CrackFullPath(e.Report.GetParameters("Node").Values(0))(3), True)
                    For Each n As TreeNode In Nodes2
                        If n.FullPath = e.Report.GetParameters("Node").Values(0) Then
                            CompareForm.smoTreeView2.SelectedNode = n
                            Exit For
                        End If
                    Next
                End If
                ' try to avoid a complete page reload
                Dim OkToSkipLoad As Boolean = True
                Select Case msg
                    ' respond to literals embedded in the navigation links
                    ' the report determines which the viewer shows
                    Case "CopyDefinitionToClipboard"
                        My.Computer.Clipboard.SetData(DataFormats.Text, DefinitionByVersion.Rows(0)("Definition"))
                    Case "CopyEventDataToClipboard"
                        My.Computer.Clipboard.SetData(DataFormats.Text, DefinitionByVersion.Rows(0)("EventData"))
                    Case "CompareToOriginal"
                        My.Settings.SQL__Repository__B_Filters_Version = CInt(DefinitionByVersion.Rows(0)("MinVersion"))
                    Case "CompareToPrevious"
                        ' set the settings version before the call then set the context menu version value after the call
                        My.Settings.SQL__Repository__B_Filters_Version = CInt(DefinitionByVersion.Rows(0)("Version")) - 1
                    Case "CompareToSelected"
                        ' validate that the number entered is between min and max and not eq curr
                        DialogSelectNodeVersion.LabelVersion.Text = String.Format(DialogSelectNodeVersion.LabelVersion.Text, e.Report.GetParameters("Node").Values(0))
                        DialogSelectNodeVersion.NumericUpDownVersion.Maximum = CInt(DefinitionByVersion.Rows(0)("MaxVersion"))
                        DialogSelectNodeVersion.NumericUpDownVersion.Minimum = CInt(DefinitionByVersion.Rows(0)("MinVersion"))
                        DialogSelectNodeVersion.NumericUpDownVersion.Value = CInt(e.Report.GetParameters("Version").Values(0))
                        If DialogSelectNodeVersion.ShowDialog() = Windows.Forms.DialogResult.OK Then
                            My.Settings.SQL__Repository__B_Filters_Version = CInt(DialogSelectNodeVersion.NumericUpDownVersion.Value)
                        Else
                            Exit Select
                        End If
                    Case "CompareToNext"
                        ' TODO should skiptonext if immediate antecedent is a delete and version < current version
                        My.Settings.SQL__Repository__B_Filters_Version = CInt(DefinitionByVersion.Rows(0)("Version")) + 1
                    Case "CompareToLatest"
                        My.Settings.SQL__Repository__B_Filters_Version = CInt(DefinitionByVersion.Rows(0)("MaxVersion"))
                    Case Else ' "none" falls through to here
                        OkToSkipLoad = False
                        ' no-op
                End Select
                If Not OkToSkipLoad Then
                    ' reset the action message, it is now handled
                    LoadConfigurationCatalog(e.Report.GetParameters("SQLInstance").Values(0), _
                                             e.Report.GetParameters("Database").Values(0), _
                                             e.Report.GetParameters("Type").Values(0), _
                                             e.Report.GetParameters("SubType").Values(0), _
                                             e.Report.GetParameters("Collection").Values(0), _
                                             e.Report.GetParameters("Item").Values(0), _
                                             e.Report.GetParameters("Node").Values(0), _
                                             e.Report.GetParameters("Version").Values(0), _
                                             "none")
                End If
                If InStr(msg, "Compare") = 1 Then
                    CompareForm.RunCompare()
                    Mother.TabForms.SelectTab(CompareForm.Name & "Tab")
                    CompareForm.ToolStripComboBoxVersionA.SelectedItem = CType(CompareForm.smoTreeView1.SelectedNode.Tag, cTreeView.structNodeTag).Version
                    CompareForm.ToolStripComboBoxVersionB.SelectedItem = CType(CompareForm.smoTreeView2.SelectedNode.Tag, cTreeView.structNodeTag).Version
                End If
            Case "SQLRunbookCatalog"
                ' handle any DrillThroughActions here
                Dim parms As ReportParameterInfoCollection = e.Report.GetParameters
                Dim Category As String = Nothing
                If Not parms("Category").State = ParameterState.MissingValidValue Then
                    Category = parms("Category").Values(0)
                End If
                Dim Topic As String = Nothing
                If Not parms("Topic").State = ParameterState.MissingValidValue Then
                    Topic = parms("Topic").Values(0)
                End If
                Dim DocumentId As Integer = Nothing
                If Not parms("DocumentId").State = ParameterState.MissingValidValue Then
                    DocumentId = CInt(parms("DocumentId").Values(0))
                End If
                Dim DrillThroughAction As String = "none"
                If Not parms("DrillThroughAction").State = ParameterState.MissingValidValue Then
                    DrillThroughAction = parms("DrillThroughAction").Values(0)
                    e.Cancel = True
                    Select Case DrillThroughAction
                        Case "OpenDocumentInApplicationProcess"
                            Using taFile As New DataSetSQLRunbookTableAdapters.tDocumentTableAdapter
                                taFile.Connection.ConnectionString = RunbookForm.sRunbookConnectionString
                                RunbookForm.OpenDocument(taFile.GetDataByDocumentId(DocumentId)(0)("File").ToString)
                                DrillThroughAction = "none"
                            End Using
                        Case "CopyPathToClipboard"
                            Using taFile As New DataSetSQLRunbookTableAdapters.tDocumentTableAdapter
                                taFile.Connection.ConnectionString = RunbookForm.sRunbookConnectionString
                                My.Computer.Clipboard.SetData(DataFormats.Text, taFile.GetDataByDocumentId(DocumentId)(0)("File").ToString)
                                DrillThroughAction = "none"
                            End Using
                    End Select
                End If
                LoadRunbookCatalog(Category, _
                                   Topic, _
                                   DocumentId, _
                                   DrillThroughAction)
            Case "SQLRunbookCategoriesAddedInDateRange"
                Using TableAdapterDocument As New DataSetSQLRunbookTableAdapters.tDocumentTableAdapter
                    TableAdapterDocument.Connection.ConnectionString = RunbookForm.sRunbookConnectionString
                    TableAdapterDocument.ClearBeforeFill = True
                    rpt.DataSources.Add(New ReportDataSource("Categories", _
                            CType(TableAdapterDocument.GetDataByAddDateRange(CDate(e.Report.GetParameters("StartDt").Values(0)), _
                                                                       CDate(e.Report.GetParameters("EndDt").Values(0))), DataTable)))
                End Using
            Case "SQLRunbookCategoryTopicsAddedInDateRange"
                Using TableAdapterDocument As New DataSetSQLRunbookTableAdapters.tDocumentTableAdapter
                    TableAdapterDocument.Connection.ConnectionString = RunbookForm.sRunbookConnectionString
                    TableAdapterDocument.ClearBeforeFill = True
                    rpt.DataSources.Add(New ReportDataSource("CategoryTopics", _
                            CType(TableAdapterDocument.GetDataByAddDateRange(CDate(e.Report.GetParameters("BeginDt").Values(0)), _
                                                                       CDate(e.Report.GetParameters("EndDt").Values(0))), DataTable)))
                End Using
            Case "SQLRunbookContributor"
                Dim Parms As ReportParameterInfoCollection = e.Report.GetParameters
                Dim OriginalLogin As String = Parms("OriginalLogin").Values(0)
                Dim ShowTopics As Boolean = False
                If Not Parms("ShowTopics").State = ParameterState.MissingValidValue Then
                    ShowTopics = CBool(Parms("ShowTopics").Values(0))
                End If
                Dim ShowTopicRatings As Boolean = False
                If Not Parms("ShowTopicRatings").State = ParameterState.MissingValidValue Then
                    ShowTopicRatings = CBool(Parms("ShowTopicRatings").Values(0))
                End If
                Dim ShowDocuments As Boolean = False
                If Not Parms("ShowDocuments").State = ParameterState.MissingValidValue Then
                    ShowDocuments = CBool(Parms("ShowDocuments").Values(0))
                End If
                Dim ShowDocumentRatings As Boolean = False
                If Not Parms("ShowDocumentRatings").State = ParameterState.MissingValidValue Then
                    ShowDocumentRatings = CBool(Parms("ShowDocumentRatings").Values(0))
                End If
                e.Cancel = True
                LoadSQLRunbookContributor(OriginalLogin, ShowTopics, ShowTopicRatings, ShowDocuments, ShowDocumentRatings)
            Case "SQLRunbookDocumentsAddedInDateRange"
                Using TableAdapterDocument As New DataSetSQLRunbookTableAdapters.tDocumentTableAdapter
                    TableAdapterDocument.Connection.ConnectionString = RunbookForm.sRunbookConnectionString
                    TableAdapterDocument.ClearBeforeFill = True
                    rpt.DataSources.Add(New ReportDataSource("Documents", _
                            CType(TableAdapterDocument.GetDataByAddDateRange(CDate(e.Report.GetParameters("BeginDt").Values(0)), _
                                                                       CDate(e.Report.GetParameters("EndDt").Values(0))), DataTable)))
                End Using
            Case "SQLRunbookDocumentsChangedInDateRange"
                Using TableAdapterDocument As New DataSetSQLRunbookTableAdapters.tDocumentTableAdapter
                    TableAdapterDocument.Connection.ConnectionString = RunbookForm.sRunbookConnectionString
                    TableAdapterDocument.ClearBeforeFill = True
                    rpt.DataSources.Add(New ReportDataSource("Documents", _
                            CType(TableAdapterDocument.GetDataByChangedDateRange(CDate(e.Report.GetParameters("BeginDt").Values(0)), _
                                                                           CDate(e.Report.GetParameters("EndDt").Values(0))), DataTable)))
                End Using
            Case "SQLRunbookDocumentsReviewedInDateRange"
                Using TableAdapterDocumentRating As New DataSetSQLRunbookTableAdapters.tDocumentRatingTableAdapter
                    TableAdapterDocumentRating.Connection.ConnectionString = RunbookForm.sRunbookConnectionString
                    TableAdapterDocumentRating.ClearBeforeFill = True
                    rpt.DataSources.Add(New ReportDataSource("DocumentRatings", _
                            CType(TableAdapterDocumentRating.GetDataByDateRange(CDate(e.Report.GetParameters("BeginDt").Values(0)), _
                                                                          CDate(e.Report.GetParameters("EndDt").Values(0))), DataTable)))
                End Using
            Case "SQLRunbookDocumentTypes"
                Using TableAdapterIFilters As New DataSetSQLRunbookTableAdapters.fulltext_document_typesTableAdapter
                    TableAdapterIFilters.Connection.ConnectionString = RunbookForm.sRunbookConnectionString
                    rpt.DataSources.Add(New ReportDataSource("iFilters", _
                                                             CType(TableAdapterIFilters.GetData(e.Report.GetParameters("DocumentType").Values(0)), DataTable)))
                End Using
            Case "SQLRunbookTopicsAddedInDateRange"
                Using TableAdapterTopic As New DataSetSQLRunbookTableAdapters.tTopicTableAdapter
                    TableAdapterTopic.Connection.ConnectionString = RunbookForm.sRunbookConnectionString
                    TableAdapterTopic.ClearBeforeFill = True
                    rpt.DataSources.Add(New ReportDataSource("Topics", _
                            CType(TableAdapterTopic.GetDataByAddDateRange(CDate(e.Report.GetParameters("BeginDt").Values(0)), _
                                                                    CDate(e.Report.GetParameters("EndDt").Values(0))), DataTable)))
                End Using
            Case "SQLRunbookTopicsChangedInDateRange"
                Using TableAdapterTopic As New DataSetSQLRunbookTableAdapters.tTopicTableAdapter
                    TableAdapterTopic.Connection.ConnectionString = RunbookForm.sRunbookConnectionString
                    TableAdapterTopic.ClearBeforeFill = True
                    rpt.DataSources.Add(New ReportDataSource("Topics", _
                            CType(TableAdapterTopic.GetDataByChangedInDateRange(CDate(e.Report.GetParameters("BeginDt").Values(0)), _
                                                                          CDate(e.Report.GetParameters("EndDt").Values(0))), DataTable)))
                End Using
            Case "SQLRunbookTopicsReviewedInDateRange"
                Using TableAdapterTopicRating As New DataSetSQLRunbookTableAdapters.tTopicRatingTableAdapter
                    TableAdapterTopicRating.Connection.ConnectionString = RunbookForm.sRunbookConnectionString
                    TableAdapterTopicRating.ClearBeforeFill = True
                    rpt.DataSources.Add(New ReportDataSource("TopicRatings", _
                            CType(TableAdapterTopicRating.GetDataByDateRange(CDate(e.Report.GetParameters("BeginDt").Values(0)), _
                                                                       CDate(e.Report.GetParameters("EndDt").Values(0))), DataTable)))
                End Using
            Case Else
                'no-op
        End Select
    End Sub

    ' beware subreport performance especially in a list
    Public Sub SQLClueSubreportProcessingEventHandler(ByVal sender As Object, ByVal e As SubreportProcessingEventArgs)
        ' need to always provide a correctly defined reportdatasource, even if component disabled 
        Mother.DAL.RepositoryInstanceName = My.Settings.RepositoryInstanceName
        Mother.DAL.RepositoryDatabaseName = My.Settings.RepositoryDatabaseName
        Mother.DAL.RepositoryUseTrustedConnection = My.Settings.RepositoryUseTrustedConnection
        Mother.DAL.RepositorySQLLoginName = My.Settings.RepositorySQLLoginName
        Mother.DAL.RepositorySQLLoginPassword = My.Settings.RepositorySQLLoginPassword
        Mother.DAL.RepositoryConnectionTimeout = My.Settings.RepositoryConnectionTimeout
        Mother.DAL.RepositoryNetworkLibrary = My.Settings.RepositoryNetworkLibrary
        Mother.DAL.RepositoryEncryptConnection = My.Settings.RepositoryEncryptConnection
        Mother.DAL.RepositoryTrustServerCertificate = My.Settings.RepositoryTrustServerCertificate
        ' always provide a correctly defined reportdatasource, even if component disabled (give empty rowset) 
        Select Case e.ReportPath
            Case "SQLClueScheduledTasks"
                e.DataSources.Add(New ReportDataSource("ScheduledTasks", Mother.DAL.GetScheduledTasksWithLastNbrItemsProcessed))
            Case "SQLClueServiceStatus"
                Dim tbl As DataTable = New DataTable("ServiceInfo")
                Dim ServiceStatus As DataColumn = New DataColumn
                ServiceStatus.DataType = System.Type.GetType("System.String")
                ServiceStatus.ColumnName = "ServiceStatus"
                tbl.Columns.Add(ServiceStatus)
                Dim ServiceAccount As DataColumn = New DataColumn
                ServiceAccount.DataType = System.Type.GetType("System.String")
                ServiceAccount.ColumnName = "ServiceAccount"
                tbl.Columns.Add(ServiceAccount)
                Dim rw As DataRow = tbl.NewRow()
                rw("ServiceStatus") = Mother.GetServiceStatus
                rw("ServiceAccount") = Mother.SQLClueServiceAccount
                tbl.Rows.Add(rw)
                e.DataSources.Add(New ReportDataSource("ServiceInfo", tbl))
            Case "SQLConfigurationCatalogIndex"
                e.DataSources.Add(New ReportDataSource("SQLConfiguration", Mother.DAL.GetConfigurationCatalog))
            Case "SQLConfigurationChangeDefinitionByVersion"
                Using TableAdapterSQLCfgDefinition As New cCommon.dsSQLConfigurationTableAdapters.pChangeSelectDefinitionByVersionTableAdapter
                    TableAdapterSQLCfgDefinition.Connection.ConnectionString = Mother.DAL.LocalRepositoryConnectionString
                    TableAdapterSQLCfgDefinition.ClearBeforeFill = True
                    e.DataSources.Add(New ReportDataSource("DefinitionByVersion", _
                                      CType(TableAdapterSQLCfgDefinition.GetData(e.Parameters("Node").Values(0), _
                                                                           CInt(e.Parameters("Version").Values(0))), DataTable)))
                End Using
            Case "SQLConfigurationChange"
                e.DataSources.Add(New ReportDataSource("DailyChangeSummary", _
                                                       Mother.DAL.GetNodesForDateRange(DateAdd("d", _
                                                                                       -1 * CInt(e.Parameters("Level3Days").Values(0)), _
                                                                                       CDate(e.Parameters("EndDt").Values(0))), _
                                                       CDate(e.Parameters("EndDt").Values(0)))))
            Case "SQLRunbookChange"
                If My.Settings.RunbookEnabled Then
                    Using taActivity As New DataSetSQLRunbookTableAdapters.pRunbookActivityTableAdapter
                        taActivity.Connection.ConnectionString = RunbookForm.sRunbookConnectionString
                        e.DataSources.Add(New ReportDataSource("RunbookActivity", _
                                                               CType(taActivity.GetData(CInt(e.Parameters("Level3Days").Values(0)), _
                                                               Now), DataTable)))
                    End Using
                Else
                    ' an empty data set to render dashboard without errors 
                    e.DataSources.Add(New ReportDataSource("RunbookActivity", _
                                                           CType(New DataSetSQLRunbook.pRunbookActivityDataTable, DataTable)))
                End If
            Case Else
                'no-op
        End Select
    End Sub

#End Region

#Region " SQLCfg "

    Friend Sub LoadArchiveByInstance(ByVal SQLInstance As String)
        Try
            ' no report parms 
            Me.ReportViewerSQLClue.ZoomMode = ZoomMode.Percent
            Me.ReportViewerSQLClue.ZoomPercent = 100
            Me.ReportViewerSQLClue.ShowToolBar = True
            FlowLayoutPanelParms.Controls.Clear()
            FlowLayoutPanelParms.Controls.Add(PanelSQLInstance)
            'FlowLayoutPanelParms.Controls.Add(ButtonViewReport)
            SplitContainerParms.Height = FlowLayoutPanelParms.Height
            SplitContainerReportViewer.Panel1Collapsed = False
            SplitContainerReportViewer.SplitterDistance = FlowLayoutPanelParms.Height
            ComboBoxSQLInstance.Items.Clear()
            ComboBoxSQLInstance.Items.AddRange(Mother.ConfiguredInstanceList)
            ComboBoxSQLInstance.Text = SQLInstance
            Me.ReportViewerSQLClue.Reset()
            Me.ReportViewerSQLClue.ProcessingMode = ProcessingMode.Local
            Me.ReportViewerSQLClue.LocalReport.DataSources.Clear()
            If My.Settings.RepositoryEnabled And Not SQLInstance = "" Then
                'one datasource still goes as a collections
                Using TableAdapterSQLCfgChanges As New cCommon.dsSQLConfigurationTableAdapters.tChangeTableAdapter
                    TableAdapterSQLCfgChanges.Connection.ConnectionString = Mother.DAL.LocalRepositoryConnectionString
                    TableAdapterSQLCfgChanges.ClearBeforeFill = True
                    ReportViewerSQLClue.LocalReport.DataSources.Add(New ReportDataSource("AllNodesForInstance", _
                                                                    CType(TableAdapterSQLCfgChanges.GetDataByInstance(SQLInstance), DataTable)))
                End Using
                Me.ReportViewerSQLClue.LocalReport.ReportPath = My.Application.Info.DirectoryPath & _
                                                                "\ReportViewerReports\SQLConfigurationArchiveByInstance.rdlc"
                ' get source before setting parm 
                Dim x(0) As ReportParameter
                x(0) = New ReportParameter("SQLInstance", SQLInstance, True)
                Me.ReportViewerSQLClue.LocalReport.SetParameters(x)
                'this will refresh the report in the viewer with the above settings
                Me.ReportViewerSQLClue.RefreshReport()
            End If
            ToolStripStatusLabel1.Text = My.Resources.Ready
            RemoveHandler ReportViewerSQLClue.Drillthrough, AddressOf SQLClueDrillthroughEventHandler
            AddHandler ReportViewerSQLClue.Drillthrough, AddressOf SQLClueDrillthroughEventHandler
            RemoveHandler ReportViewerSQLClue.LocalReport.SubreportProcessing, AddressOf SQLClueSubreportProcessingEventHandler
            AddHandler ReportViewerSQLClue.LocalReport.SubreportProcessing, AddressOf SQLClueSubreportProcessingEventHandler
        Catch ex As Exception
            Mother.HandleException(ex)
        End Try
    End Sub

    Friend Sub LoadConfigurationCatalog(ByVal SQLInstance As String, _
                                        ByVal Database As String, _
                                        ByVal Type As String, _
                                        ByVal SubType As String, _
                                        ByVal Collection As String, _
                                        ByVal Item As String, _
                                        ByVal Node As String, _
                                        ByVal Version As String, _
                                        ByVal DrillThroughAction As String)
        Try
            Me.ReportViewerSQLClue.ZoomMode = ZoomMode.Percent
            Me.ReportViewerSQLClue.ZoomPercent = 100
            Me.ReportViewerSQLClue.ShowToolBar = True

            'if only node and version is passed, get the rest now
            If Not (Node Is Nothing) And Not (Version Is Nothing) Then
                If SQLInstance Is Nothing _
                And Database Is Nothing _
                And Type Is Nothing _
                And SubType Is Nothing _
                And Collection Is Nothing _
                And Item Is Nothing Then
                    Dim WorkList As String() = Mother.DAL.GetNodeAttributes(Node)
                    SQLInstance = WorkList(0)
                    Database = WorkList(1)
                    Type = WorkList(2)
                    SubType = WorkList(3)
                    Collection = WorkList(4)
                    Item = WorkList(5)
                End If
            End If

            ' send parms use control values set in called method 
            InitCfgItemLists(SQLInstance, Database, Type, SubType, Collection, Item, Node, Version)

            Me.ReportViewerSQLClue.Reset()
            Me.ReportViewerSQLClue.ProcessingMode = ProcessingMode.Local
            Me.ReportViewerSQLClue.LocalReport.DataSources.Clear()
            If My.Settings.RepositoryEnabled Then
                Me.ReportViewerSQLClue.LocalReport.ReportPath = My.Application.Info.DirectoryPath & "\ReportViewerReports\SQLConfigurationCatalog.rdlc"
                Dim Parms(8) As ReportParameter
                Parms(0) = New ReportParameter("SQLInstance", ComboBoxSQLInstance.Text)
                Parms(1) = New ReportParameter("Database", ComboBoxDatabase.Text)
                Parms(2) = New ReportParameter("Type", ComboBoxType.Text)
                Parms(3) = New ReportParameter("SubType", ComboBoxSubType.Text)
                Parms(4) = New ReportParameter("Collection", ComboBoxCollection.Text)
                Parms(5) = New ReportParameter("Item", ComboBoxItem.Text)
                Parms(6) = New ReportParameter("Node", ComboBoxItem.Tag.ToString)
                Parms(7) = New ReportParameter("Version", ComboBoxVersion.Text)
                Parms(8) = New ReportParameter("DrillThroughAction", DrillThroughAction)
                Me.ReportViewerSQLClue.LocalReport.SetParameters(Parms)
                RemoveHandler ReportViewerSQLClue.Drillthrough, AddressOf SQLClueDrillthroughEventHandler
                AddHandler ReportViewerSQLClue.Drillthrough, AddressOf SQLClueDrillthroughEventHandler
                RemoveHandler ReportViewerSQLClue.LocalReport.SubreportProcessing, AddressOf SQLClueSubreportProcessingEventHandler
                AddHandler ReportViewerSQLClue.LocalReport.SubreportProcessing, AddressOf SQLClueSubreportProcessingEventHandler
                'this will refresh the report in the viewer with the above settings
                Me.ReportViewerSQLClue.RefreshReport()
            End If
            ToolStripStatusLabel1.Text = My.Resources.Ready
        Catch ex As Exception
            Mother.HandleException(ex)
        End Try
    End Sub

    Friend Sub LoadScheduledTasks()
        Try
            Me.ReportViewerSQLClue.ZoomMode = ZoomMode.Percent
            Me.ReportViewerSQLClue.ZoomPercent = 100
            Me.ReportViewerSQLClue.ShowToolBar = True
            SplitContainerReportViewer.Panel1Collapsed = True
            FlowLayoutPanelParms.Controls.Clear()
            Me.ReportViewerSQLClue.Reset()
            Me.ReportViewerSQLClue.ProcessingMode = ProcessingMode.Local
            Me.ReportViewerSQLClue.LocalReport.DataSources.Clear()
            'one datasource still goes as a collections
            If My.Settings.RepositoryEnabled Then
                Using TableAdapterSchedules As New cCommon.dsSQLConfigurationTableAdapters.tScheduleTableAdapter
                    TableAdapterSchedules.Connection.ConnectionString = Mother.DAL.LocalRepositoryConnectionString
                    TableAdapterSchedules.ClearBeforeFill = True
                    Me.ReportViewerSQLClue.LocalReport.DataSources.Add(New ReportDataSource("ScheduledTasks", _
                                                                                            CType(TableAdapterSchedules.GetData(), DataTable)))
                End Using
                Me.ReportViewerSQLClue.LocalReport.ReportPath = My.Application.Info.DirectoryPath & "\ReportViewerReports\SQLClueScheduledTasks.rdlc"
                'this will refresh the report in the viewer with the above settings
                Me.ReportViewerSQLClue.RefreshReport()
            End If
            ToolStripStatusLabel1.Text = My.Resources.Ready
            RemoveHandler ReportViewerSQLClue.Drillthrough, AddressOf SQLClueDrillthroughEventHandler
            AddHandler ReportViewerSQLClue.Drillthrough, AddressOf SQLClueDrillthroughEventHandler
            RemoveHandler ReportViewerSQLClue.LocalReport.SubreportProcessing, AddressOf SQLClueSubreportProcessingEventHandler
            AddHandler ReportViewerSQLClue.LocalReport.SubreportProcessing, AddressOf SQLClueSubreportProcessingEventHandler
        Catch ex As Exception
            Mother.HandleException(ex)
        End Try
    End Sub

    Friend Sub LoadSQLConfigurationSummary(ByVal NbrDays As Integer, Optional ByVal node As String = "All")
        Try
            Me.ReportViewerSQLClue.ZoomMode = ZoomMode.Percent
            Me.ReportViewerSQLClue.ZoomPercent = 100
            Me.ReportViewerSQLClue.ShowToolBar = True

            FlowLayoutPanelParms.Controls.Clear()
            FlowLayoutPanelParms.Controls.Add(PanelDays)
            'FlowLayoutPanelParms.Controls.Add(ButtonViewReport)
            SplitContainerReportViewer.Panel1Collapsed = False
            SplitContainerReportViewer.SplitterDistance = FlowLayoutPanelParms.Height

            NumericUpDownDays.Value = NbrDays
            Me.ReportViewerSQLClue.Reset()
            Me.ReportViewerSQLClue.ProcessingMode = ProcessingMode.Local
            Me.ReportViewerSQLClue.LocalReport.DataSources.Clear()
            'one datasource still goes as a collections
            If My.Settings.RepositoryEnabled And NbrDays > 0 Then
                Using TableAdapterSQLCfgSummary As New cCommon.dsSQLConfigurationTableAdapters.pChangeActionSummaryByNodeForDaysTableAdapter
                    TableAdapterSQLCfgSummary.Connection.ConnectionString = Mother.DAL.LocalRepositoryConnectionString
                    TableAdapterSQLCfgSummary.ClearBeforeFill = True
                    Me.ReportViewerSQLClue.LocalReport.DataSources.Add(New ReportDataSource("SummaryByNodeForDays", _
                                                                                            CType(TableAdapterSQLCfgSummary.GetData(NbrDays, node), DataTable)))
                End Using
                Me.ReportViewerSQLClue.LocalReport.ReportPath = My.Application.Info.DirectoryPath & "\ReportViewerReports\SQLConfigurationSummary.rdlc"
                Dim Parms(0) As ReportParameter
                Parms(0) = New ReportParameter("Days", NbrDays.ToString, False)
                Me.ReportViewerSQLClue.LocalReport.SetParameters(Parms)
                'this will refresh the report in the viewer with the above settings
                Me.ReportViewerSQLClue.RefreshReport()
            End If
            ToolStripStatusLabel1.Text = My.Resources.Ready
            RemoveHandler ReportViewerSQLClue.Drillthrough, AddressOf SQLClueDrillthroughEventHandler
            AddHandler ReportViewerSQLClue.Drillthrough, AddressOf SQLClueDrillthroughEventHandler
            RemoveHandler ReportViewerSQLClue.LocalReport.SubreportProcessing, AddressOf SQLClueSubreportProcessingEventHandler
            AddHandler ReportViewerSQLClue.LocalReport.SubreportProcessing, AddressOf SQLClueSubreportProcessingEventHandler
        Catch ex As Exception
            Mother.HandleException(ex)
        End Try
    End Sub

    Friend Sub LoadArchiveSearch(ByVal SearchString As String, _
                                 ByVal LatestVersionOnly As Boolean, _
                                 ByVal Type As String)
        Try
            Me.ReportViewerSQLClue.ZoomMode = ZoomMode.Percent
            Me.ReportViewerSQLClue.ZoomPercent = 100
            Me.ReportViewerSQLClue.ShowToolBar = True

            FlowLayoutPanelParms.Controls.Clear()
            FlowLayoutPanelParms.Controls.Add(PanelSearch)
            FlowLayoutPanelParms.Controls.Add(PanelSearch)
            FlowLayoutPanelParms.Controls.Add(PanelType)

            TextBoxSearchString.Text = SearchString
            ComboBoxSearchLatest.SelectedItem = LatestVersionOnly.ToString
            ComboBoxType.SelectedItem = Type

            SplitContainerReportViewer.Panel1Collapsed = False
            SplitContainerReportViewer.SplitterDistance = FlowLayoutPanelParms.Height

            Me.ReportViewerSQLClue.Reset()
            Me.ReportViewerSQLClue.ProcessingMode = ProcessingMode.Local
            Me.ReportViewerSQLClue.LocalReport.DataSources.Clear()
            If SearchString <> "" Then
                Me.ReportViewerSQLClue.LocalReport.DataSources.Add(New ReportDataSource("Nodes", _
                                         Mother.DAL.GetChangesBySearchString(String.Format("""{0}""", SearchString), LatestVersionOnly, Type)))
                Me.ReportViewerSQLClue.LocalReport.ReportPath = My.Application.Info.DirectoryPath & "\ReportViewerReports\SQLConfigurationArchiveSimpleSearch.rdlc"
                Dim Parms(2) As ReportParameter
                Parms(0) = New ReportParameter("SearchString", SearchString)
                Parms(1) = New ReportParameter("LatestVersion", LatestVersionOnly.ToString)
                Parms(2) = New ReportParameter("Type", Type)
                Me.ReportViewerSQLClue.LocalReport.SetParameters(Parms)
                RemoveHandler ReportViewerSQLClue.Drillthrough, AddressOf SQLClueDrillthroughEventHandler
                AddHandler ReportViewerSQLClue.Drillthrough, AddressOf SQLClueDrillthroughEventHandler
                RemoveHandler ReportViewerSQLClue.LocalReport.SubreportProcessing, AddressOf SQLClueSubreportProcessingEventHandler
                AddHandler ReportViewerSQLClue.LocalReport.SubreportProcessing, AddressOf SQLClueSubreportProcessingEventHandler
                Me.ReportViewerSQLClue.RefreshReport()
            End If
            ToolStripStatusLabel1.Text = My.Resources.Ready
        Catch ex As Exception
            Mother.HandleException(ex)
        End Try
    End Sub

    Friend Sub LoadArchiveSettings()
        Try
            Me.ReportViewerSQLClue.ZoomMode = ZoomMode.Percent
            Me.ReportViewerSQLClue.ZoomPercent = 100
            Me.ReportViewerSQLClue.ShowToolBar = True
            SplitContainerReportViewer.Panel1Collapsed = True
            FlowLayoutPanelParms.Controls.Clear()
            Me.ReportViewerSQLClue.Reset()
            Me.ReportViewerSQLClue.ProcessingMode = ProcessingMode.Local
            Me.ReportViewerSQLClue.LocalReport.DataSources.Clear()
            'one datasource still goes as a collections
            If My.Settings.RepositoryEnabled Then
                Using TableAdapterSettings As New cCommon.dsSQLConfigurationTableAdapters.tServiceSettingsTableAdapter
                    TableAdapterSettings.Connection.ConnectionString = Mother.DAL.LocalRepositoryConnectionString
                    TableAdapterSettings.ClearBeforeFill = True
                    Me.ReportViewerSQLClue.LocalReport.DataSources.Add(New ReportDataSource("SQLConfigurationArchiveSettings", _
                                                                                            CType(TableAdapterSettings.GetData("Default"), DataTable)))
                End Using
                Me.ReportViewerSQLClue.LocalReport.ReportPath = My.Application.Info.DirectoryPath & "\ReportViewerReports\SQLConfigurationArchiveSettings.rdlc"
                'this will refresh the report in the viewer with the above settings
                Me.ReportViewerSQLClue.RefreshReport()
            End If
            ToolStripStatusLabel1.Text = My.Resources.Ready
            'RemoveHandler ReportViewerSQLClue.Drillthrough, AddressOf SQLClueDrillthroughEventHandler
            'AddHandler ReportViewerSQLClue.Drillthrough, AddressOf SQLClueDrillthroughEventHandler
            'RemoveHandler ReportViewerSQLClue.LocalReport.SubreportProcessing, AddressOf SQLClueSubreportProcessingEventHandler
            'AddHandler ReportViewerSQLClue.LocalReport.SubreportProcessing, AddressOf SQLClueSubreportProcessingEventHandler
        Catch ex As Exception
            Mother.HandleException(ex)
        End Try
    End Sub

    Friend Sub LoadRecentArchiveSQLError(ByVal DaysToGet As Integer)
        Try
            Me.ReportViewerSQLClue.ZoomMode = ZoomMode.Percent
            Me.ReportViewerSQLClue.ZoomPercent = 100
            Me.ReportViewerSQLClue.ShowToolBar = True
            SplitContainerReportViewer.Panel1Collapsed = True
            FlowLayoutPanelParms.Controls.Clear()
            Me.ReportViewerSQLClue.Reset()
            Me.ReportViewerSQLClue.ProcessingMode = ProcessingMode.Local
            Me.ReportViewerSQLClue.LocalReport.DataSources.Clear()
            'one datasource still goes as a collections
            If My.Settings.RepositoryEnabled Then
                Me.ReportViewerSQLClue.LocalReport.DataSources.Add(New ReportDataSource("RecentSQLErrors", _
                                                                                            Mother.DAL.dcSQLCfg.pSQLErrorLogSelectMostRecent(DaysToGet)))
                Me.ReportViewerSQLClue.LocalReport.ReportPath = My.Application.Info.DirectoryPath & "\ReportViewerReports\SQLConfigurationRecentSQLErrors.rdlc"
                'this will refresh the report in the viewer with the above settings
                Me.ReportViewerSQLClue.RefreshReport()
            End If
            ToolStripStatusLabel1.Text = My.Resources.Ready
        Catch ex As Exception
            Mother.HandleException(ex)
        End Try
    End Sub

#End Region

#Region " Runbook "

    Friend Sub LoadRunbookCatalog(ByVal Category As String, _
                                  ByVal Topic As String, _
                                  ByVal DocumentId As Integer, _
                                  ByVal DrillThroughAction As String)
        Try
            Me.ReportViewerSQLClue.ZoomMode = ZoomMode.Percent
            Me.ReportViewerSQLClue.ZoomPercent = 100
            Me.ReportViewerSQLClue.ShowToolBar = True
            SplitContainerReportViewer.Panel1Collapsed = True
            FlowLayoutPanelParms.Controls.Clear()
            Me.ReportViewerSQLClue.Reset()
            Me.ReportViewerSQLClue.ProcessingMode = ProcessingMode.Local
            Me.ReportViewerSQLClue.LocalReport.DataSources.Clear()
            'one datasource still goes as a collections
            If My.Settings.RepositoryEnabled Then
                Using taRunbookCatalog As New DataSetSQLRunbookTableAdapters.pRunbookCatalogTableAdapter
                    taRunbookCatalog.Connection.ConnectionString = RunbookForm.sRunbookConnectionString
                    Me.ReportViewerSQLClue.LocalReport.DataSources.Add(New ReportDataSource("SQLRunbook", CType(taRunbookCatalog.GetData(), DataTable)))
                End Using
                Using taCategory As New DataSetSQLRunbookTableAdapters.tCategoryTableAdapter
                    taCategory.Connection.ConnectionString = RunbookForm.sRunbookConnectionString
                    Me.ReportViewerSQLClue.LocalReport.DataSources.Add(New ReportDataSource("GetCategory", CType(taCategory.GetDataByCategoryName(Category), DataTable)))
                End Using
                Using taCategoryTopics As New DataSetSQLRunbookTableAdapters.tTopicTableAdapter
                    taCategoryTopics.Connection.ConnectionString = RunbookForm.sRunbookConnectionString
                    Me.ReportViewerSQLClue.LocalReport.DataSources.Add(New ReportDataSource("TopicsByCategory", CType(taCategoryTopics.GetDataByCategoryName(Category), DataTable)))
                End Using
                Using taTopic As New DataSetSQLRunbookTableAdapters.tTopicTableAdapter
                    taTopic.Connection.ConnectionString = RunbookForm.sRunbookConnectionString
                    Me.ReportViewerSQLClue.LocalReport.DataSources.Add(New ReportDataSource("Topic", CType(taTopic.GetDataByTopicName(Topic), DataTable)))
                End Using
                Using taTopicRating As New DataSetSQLRunbookTableAdapters.tTopicRatingTableAdapter
                    taTopicRating.Connection.ConnectionString = RunbookForm.sRunbookConnectionString
                    Me.ReportViewerSQLClue.LocalReport.DataSources.Add(New ReportDataSource("TopicRatingsByTopic", CType(taTopicRating.GetDataByTopicName(Topic), DataTable)))
                End Using
                Using taTopicDocuments As New DataSetSQLRunbookTableAdapters.tDocumentTableAdapter
                    taTopicDocuments.Connection.ConnectionString = RunbookForm.sRunbookConnectionString
                    Me.ReportViewerSQLClue.LocalReport.DataSources.Add(New ReportDataSource("DocumentsByTopic", CType(taTopicDocuments.GetDataByTopicName(Topic), DataTable)))
                End Using
                Using taDocument As New DataSetSQLRunbookTableAdapters.tDocumentTableAdapter
                    taDocument.Connection.ConnectionString = RunbookForm.sRunbookConnectionString
                    Me.ReportViewerSQLClue.LocalReport.DataSources.Add(New ReportDataSource("Document", CType(taDocument.GetDataByDocumentId(DocumentId), DataTable)))
                End Using
                Using taDocumentRating As New DataSetSQLRunbookTableAdapters.tDocumentRatingTableAdapter
                    taDocumentRating.Connection.ConnectionString = RunbookForm.sRunbookConnectionString
                    Me.ReportViewerSQLClue.LocalReport.DataSources.Add(New ReportDataSource("DocumentRatingsByDocument", CType(taDocumentRating.GetDataByDocumentId(DocumentId), DataTable)))
                End Using
                Me.ReportViewerSQLClue.LocalReport.ReportPath = My.Application.Info.DirectoryPath & "\ReportViewerReports\SQLRunbookCatalog.rdlc"
                Dim Parms(3) As ReportParameter
                Parms(0) = New ReportParameter("Category", Category)
                Parms(1) = New ReportParameter("Topic", Topic)
                Parms(2) = New ReportParameter("DocumentId", DocumentId.ToString)
                Parms(3) = New ReportParameter("DrillThroughAction", DrillThroughAction)
                Me.ReportViewerSQLClue.LocalReport.SetParameters(Parms)
                RemoveHandler ReportViewerSQLClue.Drillthrough, AddressOf SQLClueDrillthroughEventHandler
                AddHandler ReportViewerSQLClue.Drillthrough, AddressOf SQLClueDrillthroughEventHandler
                RemoveHandler ReportViewerSQLClue.LocalReport.SubreportProcessing, AddressOf SQLClueSubreportProcessingEventHandler
                AddHandler ReportViewerSQLClue.LocalReport.SubreportProcessing, AddressOf SQLClueSubreportProcessingEventHandler
                Me.ReportViewerSQLClue.RefreshReport()
            End If
            ToolStripStatusLabel1.Text = My.Resources.Ready
        Catch ex As Exception
            Mother.HandleException(ex)
        End Try
    End Sub

    Friend Sub LoadSQLRunbookContributorScoring()
        Try
            If My.Settings.RunbookEnabled Then
                Me.ReportViewerSQLClue.ZoomMode = ZoomMode.Percent
                Me.ReportViewerSQLClue.ZoomPercent = 100
                Me.ReportViewerSQLClue.ShowToolBar = True
                SplitContainerReportViewer.Panel1Collapsed = True
                FlowLayoutPanelParms.Controls.Clear()
                Me.ReportViewerSQLClue.Reset()
                Me.ReportViewerSQLClue.ProcessingMode = ProcessingMode.Local
                Me.ReportViewerSQLClue.LocalReport.DataSources.Clear()
                Using TableAdapterUsers As New DataSetSQLRunbookTableAdapters.pUserSelectAllWithContributorScoringTableAdapter
                    TableAdapterUsers.Connection.ConnectionString = RunbookForm.sRunbookConnectionString
                    Me.ReportViewerSQLClue.LocalReport.DataSources.Add(New ReportDataSource("Users", _
                                                                                            CType(TableAdapterUsers.GetData(), DataTable)))
                End Using
                Me.ReportViewerSQLClue.LocalReport.ReportPath = My.Application.Info.DirectoryPath & "\ReportViewerReports\SQLRunbookContributorScores.rdlc"
                RemoveHandler ReportViewerSQLClue.Drillthrough, AddressOf SQLClueDrillthroughEventHandler
                AddHandler ReportViewerSQLClue.Drillthrough, AddressOf SQLClueDrillthroughEventHandler
                RemoveHandler ReportViewerSQLClue.LocalReport.SubreportProcessing, AddressOf SQLClueSubreportProcessingEventHandler
                AddHandler ReportViewerSQLClue.LocalReport.SubreportProcessing, AddressOf SQLClueSubreportProcessingEventHandler
                Me.ReportViewerSQLClue.RefreshReport()
                ToolStripStatusLabel1.Text = My.Resources.Ready
            End If
        Catch ex As Exception
            Mother.HandleException(ex)
        End Try
    End Sub

    Friend Sub LoadSQLRunbookContributor(ByVal OriginalLogin As String, _
                                         ByVal ShowTopics As Boolean, _
                                         ByVal ShowTopicRatings As Boolean, _
                                         ByVal ShowDocuments As Boolean, _
                                         ByVal ShowDocumentRatings As Boolean)
        Try
            If My.Settings.RunbookEnabled Then
                Me.ReportViewerSQLClue.ZoomMode = ZoomMode.Percent
                Me.ReportViewerSQLClue.ZoomPercent = 100
                Me.ReportViewerSQLClue.ShowToolBar = True
                SplitContainerReportViewer.Panel1Collapsed = True
                FlowLayoutPanelParms.Controls.Clear()
                Me.ReportViewerSQLClue.Reset()
                Me.ReportViewerSQLClue.ProcessingMode = ProcessingMode.Local
                Me.ReportViewerSQLClue.LocalReport.DataSources.Clear()
                Using TableAdapterUser As New DataSetSQLRunbookTableAdapters.pUserSelectDetailsTableAdapter
                    TableAdapterUser.Connection.ConnectionString = RunbookForm.sRunbookConnectionString
                    Me.ReportViewerSQLClue.LocalReport.DataSources.Add(New ReportDataSource("User", _
                                                                                            CType(TableAdapterUser.GetData(OriginalLogin), DataTable)))
                End Using
                Using TableAdapterDocument As New DataSetSQLRunbookTableAdapters.tDocumentTableAdapter
                    TableAdapterDocument.Connection.ConnectionString = RunbookForm.sRunbookConnectionString
                    TableAdapterDocument.ClearBeforeFill = True
                    Me.ReportViewerSQLClue.LocalReport.DataSources.Add(New ReportDataSource("Documents", _
                            CType(TableAdapterDocument.GetDataByOwner(OriginalLogin), DataTable)))
                End Using
                Using TableAdapterDocumentRating As New DataSetSQLRunbookTableAdapters.tDocumentRatingTableAdapter
                    TableAdapterDocumentRating.Connection.ConnectionString = RunbookForm.sRunbookConnectionString
                    TableAdapterDocumentRating.ClearBeforeFill = True
                    Me.ReportViewerSQLClue.LocalReport.DataSources.Add(New ReportDataSource("ReviewedDocuments", _
                            CType(TableAdapterDocumentRating.GetDataByByReviewer(OriginalLogin), DataTable)))
                End Using
                Using TableAdapterTopicRating As New DataSetSQLRunbookTableAdapters.tTopicRatingTableAdapter
                    TableAdapterTopicRating.Connection.ConnectionString = RunbookForm.sRunbookConnectionString
                    TableAdapterTopicRating.ClearBeforeFill = True
                    Me.ReportViewerSQLClue.LocalReport.DataSources.Add(New ReportDataSource("ReviewedTopics", _
                            CType(TableAdapterTopicRating.GetDataByReviewer(OriginalLogin), DataTable)))
                End Using
                Using TableAdapterTopic As New DataSetSQLRunbookTableAdapters.tTopicTableAdapter
                    TableAdapterTopic.Connection.ConnectionString = RunbookForm.sRunbookConnectionString
                    TableAdapterTopic.ClearBeforeFill = True
                    Me.ReportViewerSQLClue.LocalReport.DataSources.Add(New ReportDataSource("Topics", _
                            CType(TableAdapterTopic.GetDataByOwner(OriginalLogin), DataTable)))
                End Using
                Me.ReportViewerSQLClue.LocalReport.ReportPath = My.Application.Info.DirectoryPath & "\ReportViewerReports\SQLRunbookContributor.rdlc"
                Dim Parms(4) As ReportParameter
                Parms(0) = New ReportParameter("OriginalLogin", OriginalLogin)
                Parms(1) = New ReportParameter("ShowTopics", ShowTopics.ToString)
                Parms(2) = New ReportParameter("ShowTopicRatings", ShowTopicRatings.ToString)
                Parms(3) = New ReportParameter("ShowDocuments", ShowDocuments.ToString)
                Parms(4) = New ReportParameter("ShowDocumentRatings", ShowDocumentRatings.ToString)
                Me.ReportViewerSQLClue.LocalReport.SetParameters(Parms)
                RemoveHandler ReportViewerSQLClue.Drillthrough, AddressOf SQLClueDrillthroughEventHandler
                AddHandler ReportViewerSQLClue.Drillthrough, AddressOf SQLClueDrillthroughEventHandler
                RemoveHandler ReportViewerSQLClue.LocalReport.SubreportProcessing, AddressOf SQLClueSubreportProcessingEventHandler
                AddHandler ReportViewerSQLClue.LocalReport.SubreportProcessing, AddressOf SQLClueSubreportProcessingEventHandler
                Me.ReportViewerSQLClue.RefreshReport()
                ToolStripStatusLabel1.Text = My.Resources.Ready
            End If
        Catch ex As Exception
            Mother.HandleException(ex)
        End Try
    End Sub

    Friend Sub LoadRunbookSearch(Optional ByVal SearchString As String = "")
        Try
            If My.Settings.RunbookEnabled Then
                Me.ReportViewerSQLClue.ZoomMode = ZoomMode.Percent
                Me.ReportViewerSQLClue.ZoomPercent = 100
                Me.ReportViewerSQLClue.ShowToolBar = True

                FlowLayoutPanelParms.Controls.Clear()
                FlowLayoutPanelParms.Controls.Add(PanelSearch)
                SplitContainerReportViewer.Panel1Collapsed = False
                SplitContainerReportViewer.SplitterDistance = FlowLayoutPanelParms.Height

                Me.ReportViewerSQLClue.Reset()
                Me.ReportViewerSQLClue.ProcessingMode = ProcessingMode.Local
                Me.ReportViewerSQLClue.LocalReport.DataSources.Clear()
                If SearchString <> "" Then
                    Using TableAdapterTopics As New DataSetSQLRunbookTableAdapters.tTopicTableAdapter
                        TableAdapterTopics.Connection.ConnectionString = RunbookForm.sRunbookConnectionString
                        Me.ReportViewerSQLClue.LocalReport.DataSources.Add(New ReportDataSource("Topics", _
                                             CType(TableAdapterTopics.GetDataBySearchString(String.Format("""{0}""", SearchString)), DataTable)))
                    End Using
                    Me.ReportViewerSQLClue.LocalReport.ReportPath = My.Application.Info.DirectoryPath & "\ReportViewerReports\SQLRunbookSimpleSearch.rdlc"
                    Dim Parms(0) As ReportParameter
                    Parms(0) = New ReportParameter("SearchString", SearchString)
                    Me.ReportViewerSQLClue.LocalReport.SetParameters(Parms)
                    RemoveHandler ReportViewerSQLClue.Drillthrough, AddressOf SQLClueDrillthroughEventHandler
                    AddHandler ReportViewerSQLClue.Drillthrough, AddressOf SQLClueDrillthroughEventHandler
                    RemoveHandler ReportViewerSQLClue.LocalReport.SubreportProcessing, AddressOf SQLClueSubreportProcessingEventHandler
                    AddHandler ReportViewerSQLClue.LocalReport.SubreportProcessing, AddressOf SQLClueSubreportProcessingEventHandler
                    Me.ReportViewerSQLClue.RefreshReport()
                End If
                ToolStripStatusLabel1.Text = My.Resources.Ready
            End If
        Catch ex As Exception
            Mother.HandleException(ex)
        End Try
    End Sub

    Friend Sub LoadAvailableIFilters(ByVal DocumentType As String)
        Try
            If My.Settings.RunbookEnabled Then
                Me.ReportViewerSQLClue.ZoomMode = ZoomMode.Percent
                Me.ReportViewerSQLClue.ZoomPercent = 100
                Me.ReportViewerSQLClue.ShowToolBar = True
                SplitContainerReportViewer.Panel1Collapsed = True
                FlowLayoutPanelParms.Controls.Clear()
                Me.ReportViewerSQLClue.Reset()
                Me.ReportViewerSQLClue.ProcessingMode = ProcessingMode.Local
                Me.ReportViewerSQLClue.LocalReport.DataSources.Clear()
                Using TableAdapterIFilters As New DataSetSQLRunbookTableAdapters.fulltext_document_typesTableAdapter
                    TableAdapterIFilters.Connection.ConnectionString = RunbookForm.sRunbookConnectionString
                    Me.ReportViewerSQLClue.LocalReport.DataSources.Add(New ReportDataSource("iFilters", _
                                                                                           CType(TableAdapterIFilters.GetData(DocumentType), DataTable)))
                End Using
                Me.ReportViewerSQLClue.LocalReport.ReportPath = My.Application.Info.DirectoryPath & "\ReportViewerReports\SQLRunbookDocumentTypes.rdlc"
                Dim DocType As ReportParameter = New ReportParameter("DocumentType", DocumentType)
                Me.ReportViewerSQLClue.LocalReport.SetParameters(New ReportParameter() {DocType})
                RemoveHandler ReportViewerSQLClue.Drillthrough, AddressOf SQLClueDrillthroughEventHandler
                AddHandler ReportViewerSQLClue.Drillthrough, AddressOf SQLClueDrillthroughEventHandler
                RemoveHandler ReportViewerSQLClue.LocalReport.SubreportProcessing, AddressOf SQLClueSubreportProcessingEventHandler
                AddHandler ReportViewerSQLClue.LocalReport.SubreportProcessing, AddressOf SQLClueSubreportProcessingEventHandler
                Me.ReportViewerSQLClue.RefreshReport()
                ToolStripStatusLabel1.Text = My.Resources.Ready
            End If
        Catch ex As Exception
            Mother.HandleException(ex)
        End Try
    End Sub

#End Region

#Region " Configuration catalog browsing"

    Private Sub InitCfgItemLists(ByVal CfgSQLInstance As String, _
                                 ByVal CfgDatabase As String, _
                                 ByVal CfgType As String, _
                                 ByVal CfgSubType As String, _
                                 ByVal CfgCollection As String, _
                                 ByVal CfgItem As String, _
                                 ByVal CfgNode As String, _
                                 ByVal CfgVersion As String)
        ' if the node and version are known, get the other attribs from the db

        If ComboBoxSQLInstance.Tag Is Nothing Then
            ComboBoxSQLInstance.Tag = ""
        End If

        If ComboBoxDatabase.Tag Is Nothing Then
            ComboBoxDatabase.Tag = ""
        End If

        If ComboBoxType.Tag Is Nothing Then
            ComboBoxType.Tag = ""
        End If

        If ComboBoxSubType.Tag Is Nothing Then
            ComboBoxSubType.Tag = ""
        End If

        If ComboBoxCollection.Tag Is Nothing Then
            ComboBoxCollection.Tag = ""
        End If

        If ComboBoxItem.Tag Is Nothing Then
            ComboBoxItem.Tag = ""
        End If

        If ComboBoxVersion.Tag Is Nothing Then
            ComboBoxVersion.Tag = ""
        End If

        If ComboBoxSQLInstance.Items.Count = 0 Then
            ' should only get here first time loaded  
            ' this will include a blank instance
            ComboBoxSQLInstance.Items.AddRange(Mother.ConfiguredInstanceList)
            ' in the session of console install, the cfgInstList is empty
            If ComboBoxSQLInstance.Items.Count = 1 Then
                ComboBoxSQLInstance.Items.AddRange(Mother.DAL.GetLicensedInstanceList())
            End If
        End If

        ' get the list of databases for the selected instance

        If ComboBoxSQLInstance.Tag.ToString <> CfgSQLInstance Then
            ComboBoxDatabase.Items.Clear()
            If CfgSQLInstance <> "" Then
                ComboBoxDatabase.Items.AddRange(Mother.DAL.GetArchivedDatabasesList(CfgSQLInstance))
            End If
        End If

        If ComboBoxType.Items.Count = 0 Then
            ' the initial load should show the license record 
            ComboBoxType.Items.Add("")
            ComboBoxType.Items.Add("Metadata")
            ComboBoxType.Items.Add("SQLInstance")
        End If

        If ComboBoxType.Tag.ToString <> CfgType _
        Or CfgType = "Metadata" Then
            ComboBoxSubType.Items.Clear()
        End If
        If ComboBoxSubType.Items.Count = 0 Then
            If CfgType <> "" Then
                If CfgType = "Metadata" Then
                    ComboBoxSubType.Items.Add("")
                    If CfgSQLInstance = "" Then
                        ComboBoxSubType.Items.Add("License")
                    Else
                        If CfgDatabase = "" Then
                            ComboBoxSubType.Items.Add("Connection")
                            ComboBoxSubType.Items.Add("JobServer")
                            ComboBoxSubType.Items.Add("Schedule")
                            ComboBoxSubType.Items.Add("Server")
                        Else
                            ComboBoxSubType.Items.Add("Database")
                            ComboBoxSubType.Items.Add("ServiceBroker")
                        End If
                    End If
                Else
                    If CfgDatabase = "" Then
                        ComboBoxSubType.Items.Add("JobServer")
                        ComboBoxSubType.Items.Add("Server")
                    Else
                        ComboBoxSubType.Items.Add("Database")
                        ComboBoxSubType.Items.Add("ServiceBroker")
                    End If
                End If
            End If
        End If

        ' for metadata only 
        Dim CfgPath As String = ""

        If ComboBoxSubType.Tag.ToString <> CfgSubType _
        Or ComboBoxType.Tag.ToString <> CfgType Then
            ComboBoxCollection.Items.Clear()
        End If
        If ComboBoxCollection.Items.Count = 0 Then
            If CfgSubType <> "" Then
                Select Case CfgType
                    Case "Metadata"
                        Select Case CfgSubType
                            Case "Connection"
                                CfgCollection = "SQLCfg.tConnection"
                                CfgPath = "SQLCfgMetadata|" & CfgCollection
                                CfgItem = CfgSQLInstance
                            Case "Database"
                                CfgCollection = "SQLCfg.tDb"
                                CfgPath = "SQLCfgMetadata|" & CfgCollection & "|" & CfgSQLInstance
                                CfgItem = CfgDatabase
                            Case "JobServer"
                                CfgCollection = "SQLCfg.tJobServer"
                                CfgPath = "SQLCfgMetadata|" & CfgCollection
                                CfgItem = CfgSQLInstance
                            Case "License"
                                CfgCollection = "SQLCfg.tSQLCfg"
                                CfgPath = "SQLCfgMetadata|" & CfgCollection
                                CfgItem = "License"
                            Case "Schedule"
                                ' path looks like "SQLCfgMetadata|SQLCfg.tSchedule|ScheduleId=1"
                                ' node looks like "SQLCfgMetadata|SQLCfg.tSchedule|BILL764\NOVCTP|ScheduleId=1"

                                ' if the node is known, build the path by stripping instance out of the node
                                ' if the node is not known, the user must select a schedule id before building the node
                                CfgCollection = "SQLCfg.tSchedule"
                                If CfgNode.Contains("SQLCfgMetadata|SQLCfg.tSchedule|" & CfgSQLInstance & "|ScheduleId=") Then
                                    CfgPath = Replace(CfgNode, "|" & CfgSQLInstance & "|", "|")
                                    CfgItem = CfgSQLInstance
                                Else
                                    If CfgItem.Contains("SQLCfgMetadata|SQLCfg.tSchedule|ScheduleId=") Then
                                        CfgNode = Replace(CfgItem, "|ScheduleId=", "|" & CfgSQLInstance & "|ScheduleId=")
                                        CfgPath = CfgItem
                                        CfgItem = CfgSQLInstance
                                        ComboBoxItem.Items.Clear()
                                        ComboBoxItem.Items.Add(CfgSQLInstance)
                                    Else
                                        CfgItem = ""
                                    End If
                                End If
                            Case "Server"
                                CfgCollection = "SQLCfg.tInstance"
                                CfgPath = "SQLCfgMetadata|" & CfgCollection
                                CfgItem = CfgSQLInstance
                            Case "ServiceBroker"
                                CfgCollection = "SQLCfg.tServiceBroker"
                                CfgPath = "SQLCfgMetadata|" & CfgCollection & "|" & CfgSQLInstance
                                CfgItem = CfgDatabase
                        End Select
                        ComboBoxCollection.Items.Add(CfgCollection)
                        If CfgItem <> "" Then
                            If CfgItem = "License" Then
                                ' special case where they are the same
                                CfgNode = CfgPath
                            Else
                                If CfgSubType <> "Schedule" Then
                                    CfgNode = CfgPath & "|" & CfgItem
                                End If
                            End If
                        End If
                    Case "SQLInstance"
                        ' only SQLInstance needs a blank collection
                        ComboBoxCollection.Items.Add("")
                        Select Case CfgSubType
                            Case "Database"
                                ComboBoxCollection.Items.Add("Assemblies")
                                ComboBoxCollection.Items.Add("AsymmetricKeys")
                                ComboBoxCollection.Items.Add("Certificates")
                                ComboBoxCollection.Items.Add("DatabaseAuditSpecifications")
                                ComboBoxCollection.Items.Add("FullTextCatalogs")
                                ComboBoxCollection.Items.Add("Roles")
                                ComboBoxCollection.Items.Add("Schemas")
                                ComboBoxCollection.Items.Add("StoredProcedures")
                                ComboBoxCollection.Items.Add("SymmetricKeys")
                                ComboBoxCollection.Items.Add("Tables")
                                ComboBoxCollection.Items.Add("Triggers")
                                ComboBoxCollection.Items.Add("UserDefinedAggregates")
                                ComboBoxCollection.Items.Add("UserDefinedDataTypes")
                                ComboBoxCollection.Items.Add("UserDefinedFunctions")
                                ComboBoxCollection.Items.Add("UserDefinedTypes")
                                ComboBoxCollection.Items.Add("Users")
                                ComboBoxCollection.Items.Add("Views")
                                ComboBoxCollection.Items.Add("XMLSchemaCollections")
                            Case "JobServer"
                                ComboBoxCollection.Items.Add("Alerts")
                                ComboBoxCollection.Items.Add("Categories")
                                ComboBoxCollection.Items.Add("Jobs")
                                ComboBoxCollection.Items.Add("Operators")
                                ComboBoxCollection.Items.Add("ProxyAccounts")
                            Case "Server"
                                ComboBoxCollection.Items.Add("Attributes")
                                ComboBoxCollection.Items.Add("Audits")
                                ComboBoxCollection.Items.Add("BackupDevices")
                                ComboBoxCollection.Items.Add("Configuration")
                                ComboBoxCollection.Items.Add("Credentials")
                                ComboBoxCollection.Items.Add("Databases")
                                ComboBoxCollection.Items.Add("Endpoints")
                                ComboBoxCollection.Items.Add("ExtendedEvents")
                                ComboBoxCollection.Items.Add("LinkedServers")
                                ComboBoxCollection.Items.Add("Logins")
                                ComboBoxCollection.Items.Add("Policies")
                                ComboBoxCollection.Items.Add("Roles")
                                ComboBoxCollection.Items.Add("ServerAuditSpecifications")
                                ComboBoxCollection.Items.Add("UserDefinedMessages")
                            Case "ServiceBroker"
                                ComboBoxCollection.Items.Add("MessageTypes")
                                ComboBoxCollection.Items.Add("Queues")
                                ComboBoxCollection.Items.Add("Routes")
                                ComboBoxCollection.Items.Add("ServiceContracts")
                                ComboBoxCollection.Items.Add("Services")
                        End Select
                End Select
            End If
        End If

        If ComboBoxSubType.Tag.ToString <> CfgSubType _
        Or ComboBoxCollection.Tag.ToString <> CfgCollection Then
            ComboBoxItem.Items.Clear()
        End If
        If ComboBoxItem.Items.Count = 0 Then
            If CfgSubType <> "" Then
                If CfgType = "SQLInstance" Then
                    ComboBoxItem.Items.AddRange(Mother.DAL.GetItemList(CompareForm.cCompare.MakeNode(CfgSQLInstance, _
                                                                                                        CfgDatabase, _
                                                                                                        CfgCollection, _
                                                                                                        "")))
                Else
                    If CfgSubType = "Schedule" Then
                        ' item list for scheule would just be repeating list of instancename
                        ' need the display to include the scheduleid
                        ' but the item must be only the instance and the node must identify the scheduleId
                        ' The next pass through the logic above will fix the item 
                        If CfgItem = "" Then
                            ComboBoxItem.Items.AddRange(Mother.DAL.GetSchedulePathList(CfgSQLInstance))
                        Else
                            ComboBoxItem.Items.Add(CfgItem)
                        End If
                    Else
                        ComboBoxItem.Items.AddRange(Mother.DAL.GetItemList(CfgPath))
                    End If
                End If
            End If
        End If

        If ComboBoxItem.Tag.ToString <> CfgNode Then
            ComboBoxVersion.Items.Clear()
        End If
        If ComboBoxVersion.Items.Count = 0 Then
            If CfgItem <> "" Then
                ' the tag is the latest version
                ComboBoxVersion.Tag = Mother.DAL.GetLatestVersionForNode(CfgNode).ToString
                For i As Int32 = CInt(ComboBoxVersion.Tag) To 1 Step -1
                    ComboBoxVersion.Items.Add(i.ToString)
                Next
            End If
        End If

        ' use the latest if a version is not specified
        If CfgItem <> "" And CfgVersion = "" Then
            CfgVersion = ComboBoxVersion.Tag.ToString
        End If

        If CfgVersion = "" Then
            ButtonViewReport.Enabled = False
        Else
            ButtonViewReport.Enabled = True
        End If

        ComboBoxVersion.Text = CfgVersion
        ComboBoxItem.Text = CfgItem
        ComboBoxCollection.Text = CfgCollection
        ComboBoxSubType.Text = CfgSubType
        ComboBoxType.Text = CfgType
        ComboBoxDatabase.Text = CfgDatabase
        ComboBoxSQLInstance.Text = CfgSQLInstance

        ComboBoxItem.Tag = CfgNode
        ComboBoxCollection.Tag = CfgCollection
        ComboBoxSubType.Tag = CfgSubType
        ComboBoxType.Tag = CfgType
        ComboBoxDatabase.Tag = CfgDatabase
        ComboBoxSQLInstance.Tag = CfgSQLInstance

        SetComboBoxWidth(ComboBoxSQLInstance)
        SetComboBoxWidth(ComboBoxDatabase)
        SetComboBoxWidth(ComboBoxType)
        SetComboBoxWidth(ComboBoxSubType)
        SetComboBoxWidth(ComboBoxCollection)
        SetComboBoxWidth(ComboBoxItem)
        SetComboBoxWidth(ComboBoxVersion)

        FlowLayoutPanelParms.Controls.Clear()

        FlowLayoutPanelParms.Controls.Add(PanelSQLInstance)
        FlowLayoutPanelParms.Controls.Add(PanelDatabase)
        FlowLayoutPanelParms.Controls.Add(PanelType)
        FlowLayoutPanelParms.Controls.Add(PanelSubType)
        FlowLayoutPanelParms.Controls.Add(PanelCollection)
        FlowLayoutPanelParms.Controls.Add(PanelItem)
        FlowLayoutPanelParms.Controls.Add(PanelVersion)

        SplitContainerParms.Height = FlowLayoutPanelParms.Size.Height + HeightInPixels()
        SplitContainerReportViewer.Panel1Collapsed = False
        SplitContainerReportViewer.SplitterDistance = FlowLayoutPanelParms.Height

    End Sub

    Private Function WidthInPixels(ByVal s As String) As Int32
        Using graphics As System.Drawing.Graphics = System.Drawing.Graphics.FromImage(New Bitmap(1, 1))
            Return CInt(graphics.MeasureString(s, _
                            New Font(My.Application.ApplicationContext.MainForm.Font.SystemFontName, _
                                  My.Application.ApplicationContext.MainForm.Font.Size, _
                                  My.Application.ApplicationContext.MainForm.Font.Style, _
                                  GraphicsUnit.Pixel)).Width)
        End Using
    End Function

    Private Function HeightInPixels() As Int32
        Using graphics As System.Drawing.Graphics = System.Drawing.Graphics.FromImage(New Bitmap(1, 1))
            Return CInt(graphics.MeasureString("1", _
                            New Font(My.Application.ApplicationContext.MainForm.Font.SystemFontName, _
                                  My.Application.ApplicationContext.MainForm.Font.Size, _
                                  My.Application.ApplicationContext.MainForm.Font.Style, _
                                  GraphicsUnit.Pixel)).Height)
        End Using
    End Function

    Private Sub SetComboBoxWidth(ByVal cb As System.Windows.Forms.ComboBox)
        Dim Longest As String = cb.Text
        For Each i As String In cb.Items
            If i.Length > Longest.Length Then
                Longest = i
            End If
        Next
        cb.Width = WidthInPixels(Longest) + 50
    End Sub

    Private Sub ComboBoxSQLInstance_SelectionChangeCommitted(ByVal sender As Object, ByVal e As System.EventArgs)
        InitCfgItemLists(ComboBoxSQLInstance.SelectedItem.ToString, _
                         "", _
                         "", _
                         "", _
                         "", _
                         "", _
                         "", _
                         "")
    End Sub

    Private Sub ComboBoxDatabase_SelectionChangeCommitted(ByVal sender As Object, ByVal e As System.EventArgs) Handles ComboBoxDatabase.SelectionChangeCommitted
        InitCfgItemLists(ComboBoxSQLInstance.Text, _
                         ComboBoxDatabase.SelectedItem.ToString, _
                         "", _
                         "", _
                         "", _
                         "", _
                         "", _
                         "")
    End Sub

    Private Sub ComboBoxType_SelectionChangeCommitted(ByVal sender As Object, ByVal e As System.EventArgs) Handles ComboBoxType.SelectionChangeCommitted
        InitCfgItemLists(ComboBoxSQLInstance.Text, _
                         ComboBoxDatabase.Text, _
                         ComboBoxType.SelectedItem.ToString, _
                         "", _
                         "", _
                         "", _
                         "", _
                         "")
    End Sub

    Private Sub ComboBoxSubType_SelectionChangeCommitted(ByVal sender As Object, ByVal e As System.EventArgs) Handles ComboBoxSubType.SelectionChangeCommitted
        InitCfgItemLists(ComboBoxSQLInstance.Text, _
                         ComboBoxDatabase.Text, _
                         ComboBoxType.Text, _
                         ComboBoxSubType.SelectedItem.ToString, _
                         "", _
                         "", _
                         "", _
                         "")
    End Sub

    Private Sub ComboBoxCollection_SelectionChangeCommitted(ByVal sender As Object, ByVal e As System.EventArgs) Handles ComboBoxCollection.SelectionChangeCommitted
        InitCfgItemLists(ComboBoxSQLInstance.Text, _
                         ComboBoxDatabase.Text, _
                         ComboBoxType.Text, _
                         ComboBoxSubType.Text, _
                         ComboBoxCollection.SelectedItem.ToString, _
                         "", _
                         "", _
                         "")
    End Sub

    Private Sub ComboBoxItem_SelectionChangeCommitted(ByVal sender As Object, ByVal e As System.EventArgs) Handles ComboBoxItem.SelectionChangeCommitted
        Dim CfgNode As String
        If ComboBoxType.Text = "Metadata" Then
            CfgNode = ""
        Else
            CfgNode = My.Forms.CompareForm.cCompare.MakeNode(ComboBoxSQLInstance.Text, _
                                                             ComboBoxDatabase.Text, _
                                                             ComboBoxCollection.Text, _
                                                             ComboBoxItem.Text)
        End If

        InitCfgItemLists(ComboBoxSQLInstance.Text, _
                         ComboBoxDatabase.Text, _
                         ComboBoxType.Text, _
                         ComboBoxSubType.Text, _
                         ComboBoxCollection.Text, _
                         ComboBoxItem.SelectedItem.ToString, _
                         CfgNode, _
                         "")
    End Sub

#End Region

    Private Sub ReportViewerForm_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize
        If Not (SplitContainerReportViewer.Panel1Collapsed) Then
            SplitContainerParms.Height = FlowLayoutPanelParms.Size.Height + 8
            SplitContainerReportViewer.SplitterDistance = FlowLayoutPanelParms.Height
        End If
    End Sub

    Private Sub ComboBoxSQLInstance_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBoxSQLInstance.SelectedIndexChanged
        If ComboBoxSQLInstance.Text <> "" Then
            ButtonViewReport.Enabled = True
        Else
            ButtonViewReport.Enabled = False
        End If
    End Sub

End Class
