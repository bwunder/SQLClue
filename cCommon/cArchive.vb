' for the backgroundworker
Imports System.ComponentModel

' decoration prevents breakpoints but allows backgroundworker error handling to work
'<System.Diagnostics.DebuggerNonUserCodeAttribute()> _
Public Class cArchive

    '    Private DAL As New cDataAccess
    Private cCompare As New cCompare

    Private SQLInstance As String

    Private TargetEventNotificationDatabase As String

    ' need to accumulate these durng the archive
    ' accumulate the NbrItems... using the events raised by cCompare
    Private NbrDDLEventsProcessed As Integer
    Private NbrItemsProcessed As Integer
    ' subset of above from eventhandler
    ' was truing to use thes for actype counts below
    Private NbrItemsNotInRepository As Integer ' No ItemList2 element 
    Private NbrItemsDifferentTargetAndRepository As Integer ' names eq, new version added or eventdata added
    Private NbrItemsOnlyInRepository As Integer ' No ItemList1 element

    Private NbrItemsAdded As Integer ' action typs add 
    Private NbrItemsChanged As Integer ' action type change
    Private NbrItemsDeleted As Integer ' action type delete

    Private AddDatabasesUponDiscover As Boolean

    Public Event Archiving(ByVal Node As String)
    Public Event ArchivingItem(ByVal Item As String)
    Public Event ArchivingException(ByVal excpt As Exception)
    Public Event PercentDone(ByVal Value As Int32)

    ' these get set by the caller, come from app.settings 
    Public ArchiveComplete As String
    Public ArchiveCancelled As String
    Public ArchiveRescheduled As String
    Public HandshakeConnectionTimeOut As Integer

    Private bw_PercentDone As Int32
    Private EstimatedItemsToProcess As Int32

    Private ReadOnly Property TargetEventNotificationConnectionString(ByVal crow As cCommon.dsSQLConfiguration.tConnectionRow) As String
        Get
            Dim builder As New System.Data.SqlClient.SqlConnectionStringBuilder
            builder.DataSource = SQLInstance
            builder.InitialCatalog = TargetEventNotificationDatabase
            If crow.LoginSecure Then
                builder.IntegratedSecurity = crow.LoginSecure
            Else
                ' get the user name and password
                builder.UserID = ""
                builder.Password = ""
            End If
            builder.ConnectTimeout = crow.ConnectionTimeout
            builder.ApplicationName = My.Application.Info.ProductName
            'allow for a 'not specified' net
            If crow.NetworkProtocol <> "" Then
                builder.NetworkLibrary = CStr(If(InStr(crow.NetworkProtocol, " ") > 0, _
                                                  crow.NetworkProtocol.Split(Chr(32))(0), _
                                                  crow.NetworkProtocol))
            End If
            builder.Encrypt = crow.EncryptedConnection
            builder.TrustServerCertificate = crow.TrustServerCertificate
            builder.UserInstance = False
            builder.Enlist = True
            builder.PersistSecurityInfo = True
            Return builder.ConnectionString
        End Get
    End Property


#Region " background worker"

    Private WithEvents backgroundWorker1 As BackgroundWorker
    Private eventArgs As System.ComponentModel.DoWorkEventArgs

    Public RepositoryUseTrustedConnection As Boolean
    Public RepositoryDatabaseName As String
    Public RepositoryConnectionTimeout As Integer
    Public RepositoryInstanceName As String
    Public RepositorySQLLoginName As String
    Public RepositorySQLLoginPassword As String
    Public RepositoryEncryptConnection As Boolean
    Public RepositoryTrustServerCertificate As Boolean
    Public RepositoryNetworkLibrary As String

    Public Sub AsyncArchive(ByVal ScheduleId As Integer)
        Try

            Me.backgroundWorker1 = New BackgroundWorker
            Me.backgroundWorker1.WorkerReportsProgress = True
            Me.backgroundWorker1.WorkerSupportsCancellation = True
            Me.backgroundWorker1.RunWorkerAsync(ScheduleId)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub CancelAsyncArchive()
        If Not Me.backgroundWorker1 Is Nothing Then
            Me.backgroundWorker1.CancelAsync()
        End If
    End Sub

    <System.Diagnostics.DebuggerNonUserCodeAttribute()> _
    Private Sub backgroundWorker1_DoWork( _
    ByVal sender As Object, ByVal e As DoWorkEventArgs) _
    Handles backgroundWorker1.DoWork

        eventArgs = e

        ' Do not access the form's BackgroundWorker reference directly.
        ' Instead, use the reference provided by the sender parameter.
        Dim bw As BackgroundWorker = CType(sender, BackgroundWorker)

        ' Extract the argument.
        Dim ScheduleId As Integer = CInt(Fix(e.Argument))

        ' Start the time-consuming operation.
        e.Result = Archive(ScheduleId)

        ' If the operation was cancelled by the user, 
        ' set the DoWorkEventArgs.Cancel property to true.
        ' this is stoopid here. it;s already done by now but better mark it cancelled no matter
        If bw.CancellationPending Then
            e.Cancel = True
        End If

    End Sub

    Private Sub backgroundWorker1_ProgressChanged(ByVal sender As Object, ByVal e As System.ComponentModel.ProgressChangedEventArgs) Handles backgroundWorker1.ProgressChanged
        'runs on calling thread when Reportprogress is called on bw
        ' since the scripting op is atomic, makes sense to check for cancel around the scipting ops
        ' before first, between two script ops and before compare

        RaiseEvent PercentDone(e.ProgressPercentage)

    End Sub

    ' This event handler demonstrates how to interpret 
    ' the outcome of the asynchronous operation implemented
    ' in the DoWork event handler.
    Private Sub backgroundWorker1_RunWorkerCompleted( _
    ByVal sender As Object, ByVal e As RunWorkerCompletedEventArgs) _
    Handles backgroundWorker1.RunWorkerCompleted

        'need ArchiveComplete to toggle the controls
        ' do I need ArchiveCancelled??
        RaiseEvent Archiving(ArchiveComplete)
        If e.Cancelled Then
            ' The user cancelled the operation.
            RaiseEvent ArchivingItem(ArchiveCancelled)
        ElseIf (e.Error IsNot Nothing) Then
            If e.Error.GetBaseException.Message.ToString = ArchiveCancelled Then
                RaiseEvent ArchivingItem(ArchiveCancelled)
            Else
                RaiseEvent ArchivingItem("Error")
                RaiseEvent ArchivingException(e.Error)
            End If
        Else
            If CType(e.Result, Boolean) = True Then
                RaiseEvent ArchivingItem("")
            Else
                RaiseEvent ArchivingItem(String.Format(My.Resources.ArchiveUnexpectedResult, e.Result))
            End If
        End If

        ' dispose of the backgroundworker
        backgroundWorker1.Dispose()
        backgroundWorker1 = Nothing
        'End Try

    End Sub

#End Region

#Region " Archive processing "

    Public Function Archive(ByVal ScheduleId As Integer) As Boolean
        Try
            Archive = False
            Dim ArchiveErrorMsg As String = ""
            ' this is the only path for the service and for the scheduleForm virtual column
            ' NOTE!!! may be able to optimize the first pull by skipping the compare
            Dim ActualStartDt As DateTime = Now
            RemoveHandler cCompare.NameResult, AddressOf CurrentItemEventHandler
            RemoveHandler cCompare.ArchiveAction, AddressOf ActionEventHandler
            AddHandler cCompare.NameResult, AddressOf CurrentItemEventHandler
            AddHandler cCompare.ArchiveAction, AddressOf ActionEventHandler
            NbrDDLEventsProcessed = 0
            NbrItemsProcessed = 0
            NbrItemsNotInRepository = 0
            NbrItemsDifferentTargetAndRepository = 0
            NbrItemsOnlyInRepository = 0
            NbrItemsAdded = 0
            NbrItemsChanged = 0
            NbrItemsDeleted = 0
            ' set the repository connection string values
            cCompare.DAL.RepositoryUseTrustedConnection = RepositoryUseTrustedConnection
            cCompare.DAL.RepositoryDatabaseName = RepositoryDatabaseName
            cCompare.DAL.RepositoryConnectionTimeout = RepositoryConnectionTimeout
            cCompare.DAL.RepositoryInstanceName = RepositoryInstanceName
            cCompare.DAL.RepositorySQLLoginName = RepositorySQLLoginName
            cCompare.DAL.RepositorySQLLoginPassword = RepositorySQLLoginPassword
            cCompare.DAL.RepositoryNetworkLibrary = RepositoryNetworkLibrary
            cCompare.DAL.RepositoryEncryptConnection = RepositoryEncryptConnection
            cCompare.DAL.RepositoryTrustServerCertificate = RepositoryTrustServerCertificate
            LoadServiceSettings()
            cCompare.DAL.LoadSchedule()
            Dim srow As dsSQLConfiguration.tScheduleRow = cCompare.DAL.dsSQLCfg.tSchedule.FindById(ScheduleId)
            If srow Is Nothing Then
                Throw New Exception(String.Format(My.Resources.ArchiveNoRow, "SQLCfg.tSchedule", ScheduleId, SQLInstance))
            End If
            SQLInstance = srow.InstanceName
            Dim IntervalBaseDt As DateTime = srow.IntervalBaseDt
            Dim IntervalType As String = srow.IntervalType
            Dim UseEventNotifications As Boolean = srow.UseEventNotifications
            cCompare.DAL.LoadConfigByInstance(SQLInstance)
            Try
                If cCompare.DAL.TargetHandshake(SQLInstance, HandshakeConnectionTimeOut) Then
                    ' avoid reloading anything into the dataset that will be referenced later to avoid System.Data.RowNotInTableException
                    If Not (cCompare.SqlServer1 Is Nothing) Then
                        cCompare.SqlServer1.ConnectionContext.Disconnect()
                        cCompare.SqlServer1 = Nothing
                    End If
                    Dim srvcon As New ServerConnection()
                    srvcon.ConnectionString = cCompare.DAL.TargetConnectionString(SQLInstance)
                    cCompare.SqlServer1 = New Server(srvcon)
                    cCompare.SetSmoFieldsToLoad(cCompare.SqlServer1)
                    'shared memory or kernal mode local named pipe is "preferred" network protocol to local SQL Instance 
                    RaiseEvent Archiving(String.Format(My.Resources.ArchiveConnected, SQLInstance))
                    ' don't try to process if connenction faield but do wire a log record and update the schedule
                    ' First see if there are any databases on the target that are not registered in the repository
                    ' register them with the default settings
                    Dim NewDBsAdded As Boolean = False
                    For Each oDb As Database In cCompare.SqlServer1.Databases
                        If Not (oDb.Name = "tempdb") Then
                            Dim drow As dsSQLConfiguration.tDbRow = cCompare.DAL.dsSQLCfg.tDb.FindByNameInstanceName(oDb.Name, _
                                                                                                                     SQLInstance)
                            If drow Is Nothing Then
                                RaiseEvent Archiving(String.Format(My.Resources.ArchiveAddDatabase, oDb.Name))
                                ' Add the database and Service Broker config
                                cCompare.DAL.InitNewTargetDatabase(SQLInstance, _
                                                                   oDb.Name, _
                                                                   cCompare.SqlServer1.Information.Version.Major)
                                NewDBsAdded = True
                            End If
                        End If
                    Next
                    ' and remove rows if the DB is gone now
                    Dim DroppedDBsRemoved As Boolean = False
                    For Each rDB As DataRow In cCompare.DAL.dsSQLCfg.tDb.Rows
                        If Not cCompare.SqlServer1.Databases.Contains(rDB("Name").ToString) Then
                            rDB.Delete()
                            DroppedDBsRemoved = True
                        End If
                    Next
                    ' refresh to pick up the add/delete changes
                    If NewDBsAdded Or DroppedDBsRemoved Then
                        cCompare.DAL.LoadConfigByInstance(SQLInstance)
                        srow = cCompare.DAL.dsSQLCfg.tSchedule.FindById(ScheduleId)
                    End If
                    If UseEventNotifications Then
                        EstimatedItemsToProcess = 250
                        ProcessDDLEvents()
                    Else
                        EstimatedItemsToProcess = CInt(cCompare.DAL.GetLastNodeCount(SQLInstance) * 1.1)
                        If EstimatedItemsToProcess = 0 Then
                            EstimatedItemsToProcess = 1000
                        End If
                    End If
                    Dim irow As dsSQLConfiguration.tInstanceRow = cCompare.DAL.dsSQLCfg.tInstance.FindByName(SQLInstance)
                    If irow Is Nothing Then
                        Throw New Exception(String.Format(My.Resources.ArchiveNoRow, _
                                                          "SQLCfg.tInstance", ScheduleId, SQLInstance))
                    End If
                    Dim jrow As dsSQLConfiguration.tJobServerRow = cCompare.DAL.dsSQLCfg.tJobServer.FindByInstanceName(SQLInstance)
                    If jrow Is Nothing Then
                        Throw New Exception(String.Format(My.Resources.ArchiveNoRow, _
                                                          "SQLCfg.tJobServer", ScheduleId, SQLInstance))
                    End If
                    If Not (ArchiveSrv(irow, jrow)) Then
                        If backgroundWorker1.CancellationPending Then
                            Throw New Exception(ArchiveCancelled)
                        Else
                            Throw New Exception(String.Format(My.Resources.ArchiveOfServerFailed, _
                                                My.Resources.Server, SQLInstance))
                        End If
                    End If
                    For Each oDb As Database In cCompare.SqlServer1.Databases
                        If Not (oDb.Name = "tempdb") Then
                            ' need to figure out if the db list in the archive matches the db list on the instance (above now isn't it?)
                            ' This should get handled from the server level routine as well
                            If Not (ArchiveDB(oDb, UseEventNotifications, ScheduleId)) Then
                                If backgroundWorker1.CancellationPending Then
                                    Throw New Exception(ArchiveCancelled)
                                Else
                                    Throw New Exception(String.Format(My.Resources.ArchiveOfDatabaseFailed, _
                                                                          SQLInstance, oDb.Name))
                                End If
                            End If
                        End If
                    Next
                Else
                    ArchiveErrorMsg = String.Format(My.Resources.ArchiveHandShakeFailed, SQLInstance)
                    RaiseEvent ArchivingException(New Exception(ArchiveErrorMsg))
                End If
            Catch ex As Exception
                ArchiveErrorMsg = ex.GetBaseException.Message
                Throw ex
            Finally
                RaiseEvent Archiving(My.Resources.ArchiveLogResult)
                ' write an archive log record
                cCompare.DAL.LogArchiveActivity(ScheduleId, SQLInstance, NbrDDLEventsProcessed, NbrItemsProcessed, NbrItemsAdded, _
                                                NbrItemsChanged, NbrItemsDeleted, IntervalBaseDt, ActualStartDt, Now, ArchiveErrorMsg)

                RaiseEvent Archiving(My.Resources.ArchiveReschedule)
                ' update the schedule with the next trigger date
                srow = cCompare.DAL.dsSQLCfg.tSchedule.FindById(ScheduleId)
                ' this will be the nearest time after now that maintains the established Interval criteria
                While srow.IntervalBaseDt < Now
                    Dim val As DateInterval
                    Select Case srow.IntervalType
                        Case "Minute"
                            val = DateInterval.Minute
                        Case "Hour"
                            val = DateInterval.Hour
                        Case "Day"
                            val = DateInterval.Day
                        Case "Month"
                            val = DateInterval.Month
                        Case "Quarter"
                            val = DateInterval.Quarter
                        Case "Year"
                            val = DateInterval.Year
                    End Select
                    srow.IntervalBaseDt = DateAdd(val, srow.Interval, srow.IntervalBaseDt)
                End While
                ' refresh the schedule in the database
                Archive = True
                cCompare.DAL.SaveSchedule()
                cCompare.DAL.dsSQLCfg.Clear()
                cCompare.CloseConnections()
            End Try
        Catch ex As Exception
            Throw ex
        Finally
            RemoveHandler cCompare.NameResult, AddressOf CurrentItemEventHandler
        End Try
    End Function

    Private Sub ActionEventHandler(ByVal Action As String)
        Select Case Action
            Case My.Resources.ArchiveAdd
                NbrItemsAdded += 1
            Case My.Resources.ArchiveDelete
                NbrItemsDeleted += 1
            Case My.Resources.ArchiveChange
                NbrItemsChanged += 1
            Case Else
        End Select
    End Sub

    Private Sub CurrentItemEventHandler(ByVal ResultType As String, _
                                        ByVal Item1 As String, _
                                        ByVal Item2 As String)
        NbrItemsProcessed += 1


        ' logic behind rest of math is bogus
        ' change should be incremented when a "Match" and new version is added to the database unless the previous version is a delete
        ' delete

        If InStr(ResultType, "Match") > 0 Then
            RaiseEvent ArchivingItem(Item1)
        ElseIf InStr(ResultType, "Different") > 0 Then

            NbrItemsDifferentTargetAndRepository += 1
            RaiseEvent ArchivingItem(Item1)
        Else
            If InStr(ResultType, "1Blank") > 0 Then
                ' only in repository
                NbrItemsOnlyInRepository += 1
                RaiseEvent ArchivingItem(Item2)
            ElseIf InStr(ResultType, "2Blank") > 0 Then
                'not in repository  
                NbrItemsNotInRepository += 1
                RaiseEvent ArchivingItem(Item1)
            End If
        End If
        ' only if async
        If Not backgroundWorker1 Is Nothing AndAlso backgroundWorker1.IsBusy Then
            If backgroundWorker1.CancellationPending() Then
                Me.eventArgs.Cancel = True
                Throw New Exception(ArchiveCancelled)
            Else
                If NbrItemsProcessed > 0 And EstimatedItemsToProcess > 0 Then
                    If EstimatedItemsToProcess < NbrItemsProcessed Then
                        EstimatedItemsToProcess = EstimatedItemsToProcess * 2
                    End If
                    bw_PercentDone = 3 + CInt((NbrItemsProcessed / EstimatedItemsToProcess) * 100)
                    If bw_PercentDone > 99 Then
                        bw_PercentDone = 0
                    End If
                Else
                    If bw_PercentDone > 99 Then
                        bw_PercentDone = 0
                    End If
                    bw_PercentDone += 1
                End If
                backgroundWorker1.ReportProgress(bw_PercentDone)
            End If
        End If
    End Sub

    Friend Sub LoadServiceSettings(Optional ByVal SettingsName As String = "DEFAULT")
        ' always reload the settings from the db
        Try
            cCompare.DAL.LoadServiceSettings("", SettingsName)
            With cCompare.DAL.dsSQLCfg.tServiceSettings.Rows(0)
                AddDatabasesUponDiscover = CBool(.Item("AddDatabasesUponDiscovery"))
                TargetEventNotificationDatabase = .Item("TargetEventNotificationDatabase").ToString
                cCompare._BatchSeparator = .Item("Scripting__Options_Batch__Separator").ToString
                cCompare._IgnoreBlankLines = CBool(.Item("Misc_Ignore__Blank__Lines"))
                cCompare._ReportDetails = CBool(.Item("Misc_Display__Output_Show__Comparison__Details"))
                cCompare._IncludeDrop = CBool(.Item("Scripting__Options_Include__DROP__In__Script"))
                cCompare._IncludeIfExistsWithDrop = CBool(.Item("Scripting__Options_Include__IF__EXISTS__With__Drop"))
                cCompare._IncludeUnmatched = CBool(.Item("Misc_Display__Output_Show__Scripts__For__Unmatched__Items"))
                Dim NameMatchOptions As New RegexOptions
                NameMatchOptions = RegexOptions.None
                If CBool(.Item("Regular__Expressions_NameMatch__Regex__Options_IgnoreCase_5")) Then
                    NameMatchOptions = (NameMatchOptions Or RegexOptions.IgnoreCase)
                End If
                If CBool(.Item("Regular__Expressions_NameMatch__Regex__Options_Multiline_7")) Then
                    NameMatchOptions = (NameMatchOptions Or RegexOptions.Multiline)
                End If
                If CBool(.Item("Regular__Expressions_NameMatch__Regex__Options_ExplicitCapture_4")) Then
                    NameMatchOptions = (NameMatchOptions Or RegexOptions.ExplicitCapture)
                End If
                If CBool(.Item("Regular__Expressions_NameMatch__Regex__Options_Compiled_1")) Then
                    NameMatchOptions = (NameMatchOptions Or RegexOptions.Compiled)
                End If
                If CBool(.Item("Regular__Expressions_NameMatch__Regex__Options_SingleLine_9")) Then
                    NameMatchOptions = (NameMatchOptions Or RegexOptions.Singleline)
                End If
                If CBool(.Item("Regular__Expressions_NameMatch__Regex__Options_IgnorePatternWhiteSpace_6")) Then
                    NameMatchOptions = (NameMatchOptions Or RegexOptions.IgnorePatternWhitespace)
                End If
                If CBool(.Item("Regular__Expressions_NameMatch__Regex__Options_RightToLeft_8")) Then
                    NameMatchOptions = (NameMatchOptions Or RegexOptions.RightToLeft)
                End If
                ' this is read only from db so validation not needed here
                'can only be used with ignore case, multilne and compiled
                If CBool(.Item("Regular__Expressions_NameMatch__Regex__Options_ECMAScript_3")) Then
                    NameMatchOptions = (NameMatchOptions Or RegexOptions.ECMAScript)
                End If
                If CBool(.Item("Regular__Expressions_NameMatch__Regex__Options_CultureInvariant_2")) Then
                    NameMatchOptions = (NameMatchOptions Or RegexOptions.CultureInvariant)
                End If
                cCompare._NameMatchPattern = .Item("Regular__Expressions_NameMatch__Pattern_4").ToString
                cCompare._NameMatchOptions = NameMatchOptions
                Dim LineSplitOptions As New RegexOptions
                LineSplitOptions = RegexOptions.None
                If CBool(.Item("Regular__Expressions_LineSplit__Regex__Options_IgnoreCase_5")) Then
                    LineSplitOptions = (LineSplitOptions Or RegexOptions.IgnoreCase)
                End If
                If CBool(.Item("Regular__Expressions_LineSplit__Regex__Options_Multiline_7")) Then
                    LineSplitOptions = (LineSplitOptions Or RegexOptions.Multiline)
                End If
                If CBool(.Item("Regular__Expressions_LineSplit__Regex__Options_ExplicitCapture_4")) Then
                    LineSplitOptions = (LineSplitOptions Or RegexOptions.ExplicitCapture)
                End If
                If CBool(.Item("Regular__Expressions_LineSplit__Regex__Options_Compiled_1")) Then
                    LineSplitOptions = (LineSplitOptions Or RegexOptions.Compiled)
                End If
                If CBool(.Item("Regular__Expressions_LineSplit__Regex__Options_SingleLine_9")) Then
                    LineSplitOptions = (LineSplitOptions Or RegexOptions.Singleline)
                End If
                If CBool(.Item("Regular__Expressions_LineSplit__Regex__Options_IgnorePatternWhiteSpace_6")) Then
                    LineSplitOptions = (LineSplitOptions Or RegexOptions.IgnorePatternWhitespace)
                End If
                If CBool(.Item("Regular__Expressions_LineSplit__Regex__Options_RightToLeft_8")) Then
                    LineSplitOptions = (LineSplitOptions Or RegexOptions.RightToLeft)
                End If
                'can only be used with ignore case, multilne and compiled
                If CBool(.Item("Regular__Expressions_LineSplit__Regex__Options_ECMAScript_3")) Then
                    LineSplitOptions = (LineSplitOptions Or RegexOptions.ECMAScript)
                End If
                If CBool(.Item("Regular__Expressions_LineSplit__Regex__Options_CultureInvariant_2")) Then
                    LineSplitOptions = (LineSplitOptions Or RegexOptions.CultureInvariant)
                End If
                cCompare._LineSplitOptions = LineSplitOptions
                cCompare._LineSplitPattern = .Item("Regular__Expressions_LineSplit__Pattern_3").ToString
                Dim LineReplaceOptions As New RegexOptions
                LineReplaceOptions = RegexOptions.None
                If CBool(.Item("Regular__Expressions_LineReplace__Regex__Options_IgnoreCase_5")) Then
                    LineReplaceOptions = (LineReplaceOptions Or RegexOptions.IgnoreCase)
                End If
                If CBool(.Item("Regular__Expressions_LineReplace__Regex__Options_Multiline_7")) Then
                    LineReplaceOptions = (LineReplaceOptions Or RegexOptions.Multiline)
                End If
                If CBool(.Item("Regular__Expressions_LineReplace__Regex__Options_ExplicitCapture_4")) Then
                    LineReplaceOptions = (LineReplaceOptions Or RegexOptions.ExplicitCapture)
                End If
                If CBool(.Item("Regular__Expressions_LineReplace__Regex__Options_Compiled_1")) Then
                    LineReplaceOptions = (LineReplaceOptions Or RegexOptions.Compiled)
                End If
                If CBool(.Item("Regular__Expressions_LineReplace__Regex__Options_SingleLine_9")) Then
                    LineReplaceOptions = (LineReplaceOptions Or RegexOptions.Singleline)
                End If
                If CBool(.Item("Regular__Expressions_LineReplace__Regex__Options_IgnorePatternWhiteSpace_6")) Then
                    LineReplaceOptions = (LineReplaceOptions Or RegexOptions.IgnorePatternWhitespace)
                End If
                If CBool(.Item("Regular__Expressions_LineReplace__Regex__Options_RightToLeft_8")) Then
                    LineReplaceOptions = (LineReplaceOptions Or RegexOptions.RightToLeft)
                End If
                'can only be used with ignore case, multilne and compiled
                If CBool(.Item("Regular__Expressions_LineReplace__Regex__Options_ECMAScript_3")) Then
                    LineReplaceOptions = (LineReplaceOptions Or RegexOptions.ECMAScript)
                End If
                If CBool(.Item("Regular__Expressions_LineReplace__Regex__Options_CultureInvariant_2")) Then
                    LineReplaceOptions = (LineReplaceOptions Or RegexOptions.CultureInvariant)
                End If
                cCompare._LineReplaceOptions = LineReplaceOptions
                cCompare._LineReplacePattern = .Item("Regular__Expressions_LineReplace__Pattern_1").ToString
                cCompare._LineReplacement = .Item("Regular__Expressions_LineReplace__Replacement_2").ToString
                Dim TryScriptingOptions As New ScriptingOptions
                TryScriptingOptions.AgentAlertJob = CBool(.Item("Scripting__Options_SMO_AgentAlertJob_1"))
                TryScriptingOptions.AgentJobId = CBool(.Item("Scripting__Options_SMO_AgentJobId_2"))
                TryScriptingOptions.AgentNotify = CBool(.Item("Scripting__Options_SMO_AgentNotify_3"))
                TryScriptingOptions.AllowSystemObjects = CBool(.Item("Scripting__Options_SMO_AllowSystemObjects_10"))
                TryScriptingOptions.AnsiFile = CBool(.Item("Scripting__Options_SMO_AnsiFile_20"))
                TryScriptingOptions.AnsiPadding = CBool(.Item("Scripting__Options_SMO_AnsiPadding_30"))
                TryScriptingOptions.BatchSize = CInt(.Item("Scripting__Options_SMO_BatchSize_35"))
                TryScriptingOptions.Bindings = CBool(.Item("Scripting__Options_SMO_Bindings_40"))
                TryScriptingOptions.ChangeTracking = CBool(.Item("Scripting__Options_SMO_ChangeTracking_45"))
                TryScriptingOptions.ClusteredIndexes = CBool(.Item("Scripting__Options_SMO_ClusteredIndexes_50"))
                TryScriptingOptions.ContinueScriptingOnError = CBool(.Item("Scripting__Options_SMO_ContinueScriptingOnError_60"))
                TryScriptingOptions.ConvertUserDefinedDataTypesToBaseType = CBool(.Item("Scripting__Options_SMO_ConvertUserDefinedDataTypesToBaseType_70"))
                TryScriptingOptions.DdlBodyOnly = CBool(.Item("Scripting__Options_SMO_DdlBodyOnly_80"))
                TryScriptingOptions.DdlHeaderOnly = CBool(.Item("Scripting__Options_SMO_DdlHeaderOnly_90"))
                TryScriptingOptions.Default = CBool(.Item("Scripting__Options_SMO_Default_100"))
                TryScriptingOptions.DriAll = CBool(.Item("Scripting__Options_SMO_DriAll_110"))
                TryScriptingOptions.DriAllConstraints = CBool(.Item("Scripting__Options_SMO_DriAllConstraints_120"))
                TryScriptingOptions.DriAllKeys = CBool(.Item("Scripting__Options_SMO_DriAllKeys_130"))
                TryScriptingOptions.DriChecks = CBool(.Item("Scripting__Options_SMO_DriChecks_140"))
                TryScriptingOptions.DriClustered = CBool(.Item("Scripting__Options_SMO_DriClustered_150"))
                TryScriptingOptions.DriDefaults = CBool(.Item("Scripting__Options_SMO_DriDefaults_160"))
                TryScriptingOptions.DriForeignKeys = CBool(.Item("Scripting__Options_SMO_DriForeignKeys_170"))
                TryScriptingOptions.DriIncludeSystemNames = CBool(.Item("Scripting__Options_SMO_DriIncludeSystemNames_180"))
                TryScriptingOptions.DriIndexes = CBool(.Item("Scripting__Options_SMO_DriIndexes_190"))
                TryScriptingOptions.DriNonClustered = CBool(.Item("Scripting__Options_SMO_DriNonClustered_200"))
                TryScriptingOptions.DriPrimaryKey = CBool(.Item("Scripting__Options_SMO_DriPrimaryKey_210"))
                TryScriptingOptions.DriUniqueKeys = CBool(.Item("Scripting__Options_SMO_DriUniqueKeys_220"))
                TryScriptingOptions.DriWithNoCheck = CBool(.Item("Scripting__Options_SMO_DriWithNoCheck_230"))
                If Not (.Item("Scripting__Options_SMO_Encoding_240").ToString = "") Then
                    TryScriptingOptions.Encoding = System.Text.Encoding.GetEncoding(.Item("Scripting__Options_SMO_Encoding_240").ToString)
                End If
                TryScriptingOptions.EnforceScriptingOptions = CBool(.Item("Scripting__Options_SMO_EnforceScriptingOptions_250"))
                TryScriptingOptions.ExtendedProperties = CBool(.Item("Scripting__Options_SMO_ExtendedProperties_260"))
                TryScriptingOptions.FullTextCatalogs = CBool(.Item("Scripting__Options_SMO_FullTextCatalogs_270"))
                TryScriptingOptions.FullTextIndexes = CBool(.Item("Scripting__Options_SMO_FullTextindexes_280"))
                TryScriptingOptions.FullTextStopLists = CBool(.Item("Scripting__Options_SMO_FullTextStopLists_285"))
                TryScriptingOptions.IncludeDatabaseContext = CBool(.Item("Scripting__Options_SMO_IncludeDatabaseContext_290"))
                TryScriptingOptions.IncludeDatabaseRoleMemberships = CBool(.Item("Scripting__Options_SMO_IncludeDatabaseRoleMemberships_291"))
                TryScriptingOptions.IncludeFullTextCatalogRootPath = CBool(.Item("Scripting__Options_SMO_IncludeFullTextCatalogRootPath_295"))
                TryScriptingOptions.IncludeHeaders = CBool(.Item("Scripting__Options_SMO_IncludeHeaders_300"))
                TryScriptingOptions.IncludeIfNotExists = CBool(.Item("Scripting__Options_SMO_IncludeIfNotExists_301"))
                TryScriptingOptions.Indexes = CBool(.Item("Scripting__Options_SMO_Indexes_310"))
                TryScriptingOptions.LoginSid = CBool(.Item("Scripting__Options_SMO_LoginSID_320"))
                TryScriptingOptions.NoAssemblies = CBool(.Item("Scripting__Options_SMO_NoAssemblies_330"))
                TryScriptingOptions.NoCollation = CBool(.Item("Scripting__Options_SMO_NoCollation_340"))
                TryScriptingOptions.NoCommandTerminator = CBool(.Item("Scripting__Options_SMO_NoCommandTerminator_350"))
                TryScriptingOptions.NoExecuteAs = CBool(.Item("Scripting__Options_SMO_NoExecuteAs_360"))
                TryScriptingOptions.NoFileGroup = CBool(.Item("Scripting__Options_SMO_NoFilegroup_370"))
                TryScriptingOptions.NoFileStream = CBool(.Item("Scripting__Options_SMO_NoFileStream_373"))
                TryScriptingOptions.NoFileStreamColumn = CBool(.Item("Scripting__Options_SMO_NoFileStreamColumn_376"))
                TryScriptingOptions.NoIdentities = CBool(.Item("Scripting__Options_SMO_NoIdentities_380"))
                TryScriptingOptions.NoIndexPartitioningSchemes = CBool(.Item("Scripting__Options_SMO_NoIndexPartitioningSchemes_390"))
                TryScriptingOptions.NoMailProfileAccounts = CBool(.Item("Scripting__Options_SMO_NoMailProfileAccounts_400"))
                TryScriptingOptions.NoMailProfilePrincipals = CBool(.Item("Scripting__Options_SMO_NoMailProfilePrincipals_410"))
                TryScriptingOptions.NonClusteredIndexes = CBool(.Item("Scripting__Options_SMO_NonClusteredIndexes_420"))
                TryScriptingOptions.NoTablePartitioningSchemes = CBool(.Item("Scripting__Options_SMO_NoTablePartitioningSchemes_430"))
                TryScriptingOptions.NoVardecimal = CBool(.Item("Scripting__Options_SMO_NoVarDecimal_431"))
                TryScriptingOptions.NoViewColumns = CBool(.Item("Scripting__Options_SMO_NoViewColumns_440"))
                TryScriptingOptions.NoXmlNamespaces = CBool(.Item("Scripting__Options_SMO_NoXMLNameSpaces_450"))
                TryScriptingOptions.OptimizerData = CBool(.Item("Scripting__Options_SMO_OptimizerData_460"))
                TryScriptingOptions.Permissions = CBool(.Item("Scripting__Options_SMO_Permissions_470"))
                TryScriptingOptions.PrimaryObject = CBool(.Item("Scripting__Options_SMO_PrimaryObject_480"))
                TryScriptingOptions.SchemaQualify = CBool(.Item("Scripting__Options_SMO_SchemaQualify_490"))
                TryScriptingOptions.SchemaQualifyForeignKeysReferences = CBool(.Item("Scripting__Options_SMO_SchemaQualifyForeignKeysReferences_500"))
                TryScriptingOptions.ScriptBatchTerminator = CBool(.Item("Scripting__Options_SMO_ScriptBatchTerminator_502"))
                TryScriptingOptions.ScriptData = CBool(.Item("Scripting__Options_SMO_ScriptData_503"))
                TryScriptingOptions.ScriptDataCompression = CBool(.Item("Scripting__Options_SMO_ScriptDataCompression_505"))
                TryScriptingOptions.ScriptDrops = CBool(.Item("Scripting__Options_SMO_ScriptDrops_506"))
                TryScriptingOptions.ScriptOwner = CBool(.Item("Scripting__Options_SMO_ScriptOwner_508"))
                TryScriptingOptions.ScriptSchema = CBool(.Item("Scripting__Options_SMO_ScriptSchema_509"))
                TryScriptingOptions.Statistics = CBool(.Item("Scripting__Options_SMO_Statistics_510"))
                TryScriptingOptions.TimestampToBinary = CBool(.Item("Scripting__Options_SMO_TimestampToBinary_520"))
                If CInt(.Item("Scripting__Options_SMO_TargetServerVersion_530")) > 0 Then
                    TryScriptingOptions.TargetServerVersion = CType(.Item("Scripting__Options_SMO_TargetServerVersion_530"), SqlServerVersion)
                    'Else
                    'skip it, need a blank not a zero to get correct result
                End If
                TryScriptingOptions.Triggers = CBool(.Item("Scripting__Options_SMO_Triggers_540"))
                TryScriptingOptions.WithDependencies = CBool(.Item("Scripting__Options_SMO_WithDependencies_550"))
                TryScriptingOptions.XmlIndexes = CBool(.Item("Scripting__Options_SMO_XMLIndexes_560"))
                cCompare._ScriptingOptions = TryScriptingOptions
            End With
        Catch ex As Exception
            Throw New Exception(String.Format(My.Resources.ArchiveSettingsLoadFailed, SettingsName), ex)
        End Try
    End Sub

#End Region

#Region " Archive Iterators "

    Private Function ArchiveDB(ByVal oDb As Smo.Database, _
                               ByVal bUseEventNotifications As Boolean, _
                               ByVal ScheduleId As Integer) _
                               As Boolean
        Dim SaveAutoClose As Boolean = False
        Dim topic As String = "ArchiveDb"
        Try
            If oDb.DatabaseOptions.AutoClose Then
                SaveAutoClose = True
                oDb.DatabaseOptions.AutoClose = False
            End If
            Dim drow As dsSQLConfiguration.tDbRow = cCompare.DAL.dsSQLCfg.tDb.FindByNameInstanceName(oDb.Name, SQLInstance)
            If drow Is Nothing Then
                Throw New Exception(String.Format(My.Resources.ArchiveNoRow, _
                                                  "SQLCfg.tDB", ScheduleId, SQLInstance))
            End If
            Dim brow As dsSQLConfiguration.tServiceBrokerRow = cCompare.DAL.dsSQLCfg.tServiceBroker.FindByDatabaseNameInstanceName(oDb.Name, SQLInstance)
            If brow Is Nothing Then
                Throw New Exception(String.Format(My.Resources.ArchiveNoRow, _
                                                  "SQLCfg.tServiceBroker", ScheduleId, SQLInstance))
            End If
            ' Is the DB EVEN available for scripting?
            ' so far just guessing that one of these covers a mirror
            ' for sure a log shipping target will be skipped
            If Not (oDb.Status And DatabaseStatus.Normal) = DatabaseStatus.Normal _
            And Not (oDb.Status And DatabaseStatus.Standby) = DatabaseStatus.Standby Then
                If oDb.Status = DatabaseStatus.EmergencyMode Then
                    RaiseEvent ArchivingException(New ApplicationException(String.Format(My.Resources.ArchiveStatusLiteral, SQLInstance, oDb.Name, "EmergencyMode")))
                ElseIf oDb.Status = DatabaseStatus.Inaccessible Then
                    RaiseEvent ArchivingException(New ApplicationException(String.Format(My.Resources.ArchiveStatusLiteral, SQLInstance, oDb.Name, "Inaccessible")))
                ElseIf oDb.Status = DatabaseStatus.Restoring Then
                    RaiseEvent ArchivingException(New ApplicationException(String.Format(My.Resources.ArchiveStatusLiteral, SQLInstance, oDb.Name, "Restoring")))
                ElseIf oDb.Status = DatabaseStatus.Offline Then
                    RaiseEvent ArchivingException(New ApplicationException(String.Format(My.Resources.ArchiveStatusLiteral, SQLInstance, oDb.Name, "Offline")))
                ElseIf oDb.Status = DatabaseStatus.Recovering Then
                    RaiseEvent ArchivingException(New ApplicationException(String.Format(My.Resources.ArchiveStatusLiteral, SQLInstance, oDb.Name, "Recovering")))
                ElseIf oDb.Status = DatabaseStatus.RecoveryPending Then
                    RaiseEvent ArchivingException(New ApplicationException(String.Format(My.Resources.ArchiveStatusLiteral, SQLInstance, oDb.Name, "RecoveryPending")))
                ElseIf oDb.Status = DatabaseStatus.Shutdown Then
                    RaiseEvent ArchivingException(New ApplicationException(String.Format(My.Resources.ArchiveStatusLiteral, SQLInstance, oDb.Name, "Shutdown")))
                ElseIf oDb.Status = DatabaseStatus.Suspect Then
                    RaiseEvent ArchivingException(New ApplicationException(String.Format(My.Resources.ArchiveStatusLiteral, SQLInstance, oDb.Name, "Suspect")))
                ElseIf oDb.Status = DatabaseStatus.Standby Then
                    RaiseEvent ArchivingException(New ApplicationException(String.Format(My.Resources.ArchiveStatusLiteral, SQLInstance, oDb.Name, "Standby")))
                Else
                    RaiseEvent ArchivingException(New ApplicationException(String.Format(My.Resources.ArchiveStatusLiteral, SQLInstance, oDb.Name, "unknown")))
                End If
            Else
                Dim sMirrorQuery As String = String.Format(My.Resources.ArchiveMirrorQuery, oDb.Name)
                If oDb.DatabaseOptions.UserAccess = DatabaseUserAccess.Restricted Then
                    RaiseEvent ArchivingException(New ApplicationException(String.Format(My.Resources.ArchiveStatusLiteral, SQLInstance, oDb.Name, "DBOUseOnly")))
                ElseIf oDb.DatabaseOptions.UserAccess = DatabaseUserAccess.Single Then
                    RaiseEvent ArchivingException(New ApplicationException(String.Format(My.Resources.ArchiveStatusLiteral, SQLInstance, oDb.Name, "SingleUser")))
                ElseIf cCompare.SqlServer1.VersionMajor > 8 AndAlso oDb.IsDatabaseSnapshot Then
                    RaiseEvent ArchivingException(New ApplicationException(String.Format(My.Resources.ArchiveStatusLiteral, SQLInstance, oDb.Name, "IsDatabaseSnapshot")))
                ElseIf cCompare.SqlServer1.VersionMajor > 8 _
                AndAlso (oDb.IsMirroringEnabled _
                          AndAlso oDb.ExecuteWithResults(sMirrorQuery).Tables(0).Rows(0).Item(0).ToString = My.Resources.MirrorRoleDescription) Then
                    RaiseEvent ArchivingException(New ApplicationException(String.Format(My.Resources.ArchiveStatusLiteral, SQLInstance, oDb.Name, My.Resources.MirrorRoleDescription)))
                Else
                    If Not (bUseEventNotifications) Then
                        ' Only if not handled from the change queue
                        If drow.ApplicationRoles Then
                            RaiseEvent Archiving(String.Format("{0}.{1}.{2}", SQLInstance, oDb.Name, My.Resources.ApplicationRoles))
                            cCompare.CompareAllItems(My.Resources.OriginSQLInstance, SQLInstance, oDb.Name, My.Resources.ApplicationRoles, "", _
                                                     My.Resources.OriginRepository, SQLInstance, oDb.Name, My.Resources.ApplicationRoles, "", True)
                        End If
                        If drow.Assemblies And cCompare.SqlServer1.VersionMajor > 8 Then
                            RaiseEvent Archiving(String.Format("{0}.{1}.{2}", SQLInstance, oDb.Name, My.Resources.Assemblies))
                            cCompare.CompareAllItems(My.Resources.OriginSQLInstance, SQLInstance, oDb.Name, My.Resources.Assemblies, "", _
                                                     My.Resources.OriginRepository, SQLInstance, oDb.Name, My.Resources.Assemblies, "", True)
                        End If
                        If drow.AsymmetricKeys And cCompare.SqlServer1.VersionMajor > 8 Then
                            RaiseEvent Archiving(String.Format("{0}.{1}.{2}", SQLInstance, oDb.Name, My.Resources.AsymmetricKeys))
                            cCompare.CompareAllItems(My.Resources.OriginSQLInstance, SQLInstance, oDb.Name, My.Resources.AsymmetricKeys, "", _
                                                     My.Resources.OriginRepository, SQLInstance, oDb.Name, My.Resources.AsymmetricKeys, "", True)
                        End If
                        If drow.Certificates And cCompare.SqlServer1.VersionMajor > 8 Then
                            RaiseEvent Archiving(String.Format("{0}.{1}.{2}", SQLInstance, oDb.Name, My.Resources.Certificates))
                            cCompare.CompareAllItems(My.Resources.OriginSQLInstance, SQLInstance, oDb.Name, My.Resources.Certificates, "", _
                                                     My.Resources.OriginRepository, SQLInstance, oDb.Name, My.Resources.Certificates, "", True)
                        End If
                        If drow.DatabaseAuditSpecifications And cCompare.SqlServer1.VersionMajor > 9 Then
                            RaiseEvent Archiving(String.Format("{0}.{1}.{2}", SQLInstance, oDb.Name, My.Resources.DatabaseAuditSpecifications))
                            cCompare.CompareAllItems(My.Resources.OriginSQLInstance, SQLInstance, oDb.Name, My.Resources.DatabaseAuditSpecifications, "", _
                                                     My.Resources.OriginRepository, SQLInstance, oDb.Name, My.Resources.DatabaseAuditSpecifications, "", True)
                        End If
                        If drow.DatabaseOptions Then
                            RaiseEvent Archiving(String.Format("{0}.{1}.{2}", SQLInstance, oDb.Name, My.Resources.DatabaseOptions))
                            cCompare.CompareAllItems(My.Resources.OriginSQLInstance, SQLInstance, oDb.Name, My.Resources.DatabaseOptions, "", _
                                                     My.Resources.OriginRepository, SQLInstance, oDb.Name, My.Resources.DatabaseOptions, "", True)
                        End If
                        If drow.FullTextCatalogs Then
                            RaiseEvent Archiving(String.Format("{0}.{1}.{2}", SQLInstance, oDb.Name, My.Resources.FullTextCatalogs))
                            cCompare.CompareAllItems(My.Resources.OriginSQLInstance, SQLInstance, oDb.Name, My.Resources.FullTextCatalogs, "", _
                                                     My.Resources.OriginRepository, SQLInstance, oDb.Name, My.Resources.FullTextCatalogs, "", True)
                        End If
                        If drow.FullTextStopLists And cCompare.SqlServer1.VersionMajor > 9 Then
                            RaiseEvent Archiving(String.Format("{0}.{1}.{2}", SQLInstance, oDb.Name, My.Resources.FullTextStopLists))
                            cCompare.CompareAllItems(My.Resources.OriginSQLInstance, SQLInstance, oDb.Name, My.Resources.FullTextStopLists, "", _
                                                     My.Resources.OriginRepository, SQLInstance, oDb.Name, My.Resources.FullTextStopLists, "", True)
                        End If
                        If drow.PartitionFunctions And cCompare.SqlServer1.VersionMajor > 8 Then
                            RaiseEvent Archiving(String.Format("{0}.{1}.{2}", SQLInstance, oDb.Name, My.Resources.PartitionFunctions))
                            cCompare.CompareAllItems(My.Resources.OriginSQLInstance, SQLInstance, oDb.Name, My.Resources.PartitionFunctions, "", _
                                                     My.Resources.OriginRepository, SQLInstance, oDb.Name, My.Resources.PartitionFunctions, "", True)
                        End If
                        If drow.PartitionSchemes And cCompare.SqlServer1.VersionMajor > 8 Then
                            RaiseEvent Archiving(String.Format("{0}.{1}.{2}", SQLInstance, oDb.Name, My.Resources.PartitionSchemes))
                            cCompare.CompareAllItems(My.Resources.OriginSQLInstance, SQLInstance, oDb.Name, My.Resources.PartitionSchemes, "", _
                                                     My.Resources.OriginRepository, SQLInstance, oDb.Name, My.Resources.PartitionSchemes, "", True)
                        End If
                        If drow.PlanGuides And cCompare.SqlServer1.VersionMajor > 8 Then
                            RaiseEvent Archiving(String.Format("{0}.{1}.{2}", SQLInstance, oDb.Name, My.Resources.PlanGuides))
                            cCompare.CompareAllItems(My.Resources.OriginSQLInstance, SQLInstance, oDb.Name, My.Resources.PlanGuides, "", _
                                                     My.Resources.OriginRepository, SQLInstance, oDb.Name, My.Resources.PlanGuides, "", True)
                        End If
                        If drow.Roles Then
                            RaiseEvent Archiving(String.Format("{0}.{1}.{2}", SQLInstance, oDb.Name, My.Resources.Roles))
                            cCompare.CompareAllItems(My.Resources.OriginSQLInstance, SQLInstance, oDb.Name, My.Resources.Roles, "", _
                                                     My.Resources.OriginRepository, SQLInstance, oDb.Name, My.Resources.Roles, "", True)
                        End If
                        If drow.Schemas Then
                            RaiseEvent Archiving(String.Format("{0}.{1}.{2}", SQLInstance, oDb.Name, My.Resources.Schemas))
                            cCompare.CompareAllItems(My.Resources.OriginSQLInstance, SQLInstance, oDb.Name, My.Resources.Schemas, "", _
                                                     My.Resources.OriginRepository, SQLInstance, oDb.Name, My.Resources.Schemas, "", True)
                        End If
                        If drow.StoredProcedures Then
                            RaiseEvent Archiving(String.Format("{0}.{1}.{2}", SQLInstance, oDb.Name, My.Resources.StoredProcedures))
                            cCompare.CompareAllItems(My.Resources.OriginSQLInstance, SQLInstance, oDb.Name, My.Resources.StoredProcedures, "", _
                                                     My.Resources.OriginRepository, SQLInstance, oDb.Name, My.Resources.StoredProcedures, "", True)
                        End If
                        If drow.SymmetricKeys And cCompare.SqlServer1.VersionMajor > 8 Then
                            RaiseEvent Archiving(String.Format("{0}.{1}.{2}", SQLInstance, oDb.Name, My.Resources.SymmetricKeys))
                            cCompare.CompareAllItems(My.Resources.OriginSQLInstance, SQLInstance, oDb.Name, My.Resources.SymmetricKeys, "", _
                                                     My.Resources.OriginRepository, SQLInstance, oDb.Name, My.Resources.SymmetricKeys, "", True)
                        End If
                        If drow.Synonyms And cCompare.SqlServer1.VersionMajor > 8 Then
                            RaiseEvent Archiving(String.Format("{0}.{1}.{2}", SQLInstance, oDb.Name, My.Resources.Synonyms))
                            cCompare.CompareAllItems(My.Resources.OriginSQLInstance, SQLInstance, oDb.Name, My.Resources.Synonyms, "", _
                                                     My.Resources.OriginRepository, SQLInstance, oDb.Name, My.Resources.Synonyms, "", True)
                        End If
                        If drow.Tables Then
                            RaiseEvent Archiving(String.Format("{0}.{1}.{2}", SQLInstance, oDb.Name, My.Resources.Tables))
                            cCompare.CompareAllItems(My.Resources.OriginSQLInstance, SQLInstance, oDb.Name, My.Resources.Tables, "", _
                                                     My.Resources.OriginRepository, SQLInstance, oDb.Name, My.Resources.Tables, "", True)
                        End If
                        If drow.Triggers Then
                            RaiseEvent Archiving(String.Format("{0}.{1}.{2}", SQLInstance, oDb.Name, My.Resources.Triggers))
                            cCompare.CompareAllItems(My.Resources.OriginSQLInstance, SQLInstance, oDb.Name, My.Resources.Triggers, "", _
                                                     My.Resources.OriginRepository, SQLInstance, oDb.Name, My.Resources.Triggers, "", True)
                        End If
                        If drow.UserDefinedAggregates And cCompare.SqlServer1.VersionMajor > 8 Then
                            RaiseEvent Archiving(String.Format("{0}.{1}.{2}", SQLInstance, oDb.Name, My.Resources.UserDefinedAggregates))
                            cCompare.CompareAllItems(My.Resources.OriginSQLInstance, SQLInstance, oDb.Name, My.Resources.UserDefinedAggregates, "", _
                                                     My.Resources.OriginRepository, SQLInstance, oDb.Name, My.Resources.UserDefinedAggregates, "", True)
                        End If
                        If drow.UserDefinedDataTypes And cCompare.SqlServer1.VersionMajor > 8 Then
                            RaiseEvent Archiving(String.Format("{0}.{1}.{2}", SQLInstance, oDb.Name, My.Resources.UserDefinedDataTypes))
                            cCompare.CompareAllItems(My.Resources.OriginSQLInstance, SQLInstance, oDb.Name, My.Resources.UserDefinedDataTypes, "", _
                                                     My.Resources.OriginRepository, SQLInstance, oDb.Name, My.Resources.UserDefinedDataTypes, "", True)
                        End If
                        If drow.UserDefinedFunctions And cCompare.SqlServer1.VersionMajor > 8 Then
                            RaiseEvent Archiving(String.Format("{0}.{1}.{2}", SQLInstance, oDb.Name, My.Resources.UserDefinedFunctions))
                            cCompare.CompareAllItems(My.Resources.OriginSQLInstance, SQLInstance, oDb.Name, My.Resources.UserDefinedFunctions, "", _
                                                     My.Resources.OriginRepository, SQLInstance, oDb.Name, My.Resources.UserDefinedFunctions, "", True)
                        End If
                        If drow.UserDefinedTypes And cCompare.SqlServer1.VersionMajor > 8 Then
                            RaiseEvent Archiving(String.Format("{0}.{1}.{2}", SQLInstance, oDb.Name, My.Resources.UserDefinedTypes))
                            cCompare.CompareAllItems(My.Resources.OriginSQLInstance, SQLInstance, oDb.Name, My.Resources.UserDefinedTypes, "", _
                                                     My.Resources.OriginRepository, SQLInstance, oDb.Name, My.Resources.UserDefinedTypes, "", True)
                        End If
                        If drow.UserDefinedTypes And cCompare.SqlServer1.VersionMajor > 9 Then
                            RaiseEvent Archiving(String.Format("{0}.{1}.{2}", SQLInstance, oDb.Name, My.Resources.UserDefinedTableTypes))
                            cCompare.CompareAllItems(My.Resources.OriginSQLInstance, SQLInstance, oDb.Name, My.Resources.UserDefinedTableTypes, "", _
                                                     My.Resources.OriginRepository, SQLInstance, oDb.Name, My.Resources.UserDefinedTableTypes, "", True)
                        End If
                        If drow.Users Then
                            RaiseEvent Archiving(String.Format("{0}.{1}.{2}", SQLInstance, oDb.Name, My.Resources.Users))
                            cCompare.CompareAllItems(My.Resources.OriginSQLInstance, SQLInstance, oDb.Name, My.Resources.Users, "", _
                                                     My.Resources.OriginRepository, SQLInstance, oDb.Name, My.Resources.Users, "", True)
                        End If
                        If drow.Views Then
                            RaiseEvent Archiving(String.Format("{0}.{1}.{2}", SQLInstance, oDb.Name, My.Resources.Views))
                            cCompare.CompareAllItems(My.Resources.OriginSQLInstance, SQLInstance, oDb.Name, My.Resources.Views, "", _
                                                     My.Resources.OriginRepository, SQLInstance, oDb.Name, My.Resources.Views, "", True)
                        End If
                        If drow.XMLSchemaCollections And cCompare.SqlServer1.VersionMajor > 8 Then
                            RaiseEvent Archiving(String.Format("{0}.{1}.{2}", SQLInstance, oDb.Name, My.Resources.XMLSchemaCollections))
                            cCompare.CompareAllItems(My.Resources.OriginSQLInstance, SQLInstance, oDb.Name, My.Resources.XMLSchemaCollections, "", _
                                                     My.Resources.OriginRepository, SQLInstance, oDb.Name, My.Resources.XMLSchemaCollections, "", True)
                        End If
                        If brow.Priorities And cCompare.SqlServer1.VersionMajor > 9 Then
                            RaiseEvent Archiving(String.Format("{0}.{1}.{2}", SQLInstance, oDb.Name, My.Resources.Priorities))
                            cCompare.CompareAllItems(My.Resources.OriginSQLInstance, SQLInstance, oDb.Name, My.Resources.Priorities, "", _
                                                     My.Resources.OriginRepository, SQLInstance, oDb.Name, My.Resources.Priorities, "", True)
                        End If
                        If brow.ServiceContracts And cCompare.SqlServer1.VersionMajor > 8 Then
                            RaiseEvent Archiving(String.Format("{0}.{1}.{2}", SQLInstance, oDb.Name, My.Resources.ServiceContracts))
                            cCompare.CompareAllItems(My.Resources.OriginSQLInstance, SQLInstance, oDb.Name, My.Resources.ServiceContracts, "", _
                                                     My.Resources.OriginRepository, SQLInstance, oDb.Name, My.Resources.ServiceContracts, "", True)
                        End If
                        If brow.MessageTypes And cCompare.SqlServer1.VersionMajor > 8 Then
                            RaiseEvent Archiving(String.Format("{0}.{1}.{2}", SQLInstance, oDb.Name, My.Resources.MessageTypes))
                            cCompare.CompareAllItems(My.Resources.OriginSQLInstance, SQLInstance, oDb.Name, My.Resources.MessageTypes, "", _
                                                     My.Resources.OriginRepository, SQLInstance, oDb.Name, My.Resources.MessageTypes, "", True)
                        End If
                        If brow.RemoteServiceBindings And cCompare.SqlServer1.VersionMajor > 8 Then
                            RaiseEvent Archiving(String.Format("{0}.{1}.{2}", SQLInstance, oDb.Name, My.Resources.RemoteServiceBindings))
                            cCompare.CompareAllItems(My.Resources.OriginSQLInstance, SQLInstance, oDb.Name, My.Resources.RemoteServiceBindings, "", _
                                                     My.Resources.OriginRepository, SQLInstance, oDb.Name, My.Resources.RemoteServiceBindings, "", True)
                        End If
                        If brow.Routes And cCompare.SqlServer1.VersionMajor > 8 Then
                            RaiseEvent Archiving(String.Format("{0}.{1}.{2}", SQLInstance, oDb.Name, My.Resources.Routes))
                            cCompare.CompareAllItems(My.Resources.OriginSQLInstance, SQLInstance, oDb.Name, My.Resources.Routes, "", _
                                                     My.Resources.OriginRepository, SQLInstance, oDb.Name, My.Resources.Routes, "", True)
                        End If
                    End If
                    If drow.ActiveDirectory And cCompare.SqlServer1.VersionMajor > 8 Then
                        RaiseEvent Archiving(String.Format("{0}.{1}.{2}", SQLInstance, oDb.Name, My.Resources.ActiveDirectory))
                        cCompare.CompareTwoItems(My.Resources.OriginSQLInstance, SQLInstance, oDb.Name, "", My.Resources.ActiveDirectory, 0, "", _
                                                     My.Resources.OriginRepository, SQLInstance, oDb.Name, "", My.Resources.ActiveDirectory, 0, "", True)
                    End If
                    If drow.ServiceBroker And cCompare.SqlServer1.VersionMajor > 8 Then
                        RaiseEvent Archiving(String.Format("{0}.{1}.{2}", SQLInstance, oDb.Name, My.Resources.ServiceBroker))
                        cCompare.CompareTwoItems(My.Resources.OriginSQLInstance, SQLInstance, oDb.Name, "", My.Resources.ServiceBroker, 0, "", _
                                                     My.Resources.OriginRepository, SQLInstance, oDb.Name, "", My.Resources.ServiceBroker, 0, "", True)
                    End If
                    If drow.Defaults Then
                        RaiseEvent Archiving(String.Format("{0}.{1}.{2}", SQLInstance, oDb.Name, My.Resources.Defaults))
                        cCompare.CompareAllItems(My.Resources.OriginSQLInstance, SQLInstance, oDb.Name, My.Resources.Defaults, "", _
                                                     My.Resources.OriginRepository, SQLInstance, oDb.Name, My.Resources.Defaults, "", True)
                    End If
                    If drow.Rules Then
                        RaiseEvent Archiving(String.Format("{0}.{1}.{2}", SQLInstance, oDb.Name, My.Resources.Rules))
                        cCompare.CompareAllItems(My.Resources.OriginSQLInstance, SQLInstance, oDb.Name, My.Resources.Rules, "", _
                                                     My.Resources.OriginRepository, SQLInstance, oDb.Name, My.Resources.Rules, "", True)
                    End If
                    If brow.Queues And cCompare.SqlServer1.VersionMajor > 8 Then
                        RaiseEvent Archiving(String.Format("{0}.{1}.{2}", SQLInstance, oDb.Name, My.Resources.Queues))
                        cCompare.CompareAllItems(My.Resources.OriginSQLInstance, SQLInstance, oDb.Name, My.Resources.Queues, "", _
                                                     My.Resources.OriginRepository, SQLInstance, oDb.Name, My.Resources.Queues, "", True)
                    End If
                    If brow.Services And cCompare.SqlServer1.VersionMajor > 8 Then
                        RaiseEvent Archiving(String.Format("{0}.{1}.{2}", SQLInstance, oDb.Name, My.Resources.Services))
                        cCompare.CompareAllItems(My.Resources.OriginSQLInstance, SQLInstance, oDb.Name, My.Resources.Services, "", _
                                                     My.Resources.OriginRepository, SQLInstance, oDb.Name, My.Resources.Services, "", True)
                    End If
                    If brow.ServiceContracts And cCompare.SqlServer1.VersionMajor > 8 Then
                        RaiseEvent Archiving(String.Format("{0}.{1}.{2}", SQLInstance, oDb.Name, My.Resources.ServiceContracts))
                        cCompare.CompareAllItems(My.Resources.OriginSQLInstance, SQLInstance, oDb.Name, My.Resources.ServiceContracts, "", _
                                                     My.Resources.OriginRepository, SQLInstance, oDb.Name, My.Resources.ServiceContracts, "", True)
                    End If
                End If ' status OK for scripting
            End If
            ArchiveDB = True
        Catch ex As Exception
            If Not ex.GetBaseException.Message = ArchiveCancelled Then
                ArchiveDB = False
                RaiseEvent ArchivingException(New ApplicationException(String.Format(My.Resources.ArchiveException, _
                                              ex.TargetSite.Name, SQLInstance, oDb.Name), ex))
            End If
        Finally
            If SaveAutoClose Then
                oDb.DatabaseOptions.AutoClose = SaveAutoClose
            End If
        End Try
    End Function

    Private Function ArchiveSrv(ByVal irow As dsSQLConfiguration.tInstanceRow, _
                                ByVal jrow As dsSQLConfiguration.tJobServerRow) As Boolean
        Dim topic As String = "ArchiveSrv"
        Try
            RaiseEvent Archiving(irow.Name)
            Try
                cCompare.CompareTwoItems(My.Resources.OriginSQLInstance, irow.Name, "", "", irow.Name, 0, "", _
                                         My.Resources.OriginRepository, irow.Name, "", "", irow.Name, 0, "", True)
            Catch ex As ApplicationException
                If Not ex.GetBaseException.Message.ToString = ArchiveCancelled Then
                    RaiseEvent ArchivingException(New ApplicationException(String.Format(My.Resources.ArchiveException, _
                                                      ex.TargetSite.Name, SQLInstance, My.Resources.OriginSQLInstance), ex))
                End If
            End Try
            If irow.ActiveDirectory And cCompare.SqlServer1.VersionMajor > 8 Then
                RaiseEvent Archiving(String.Format("{0}.{1}", irow.Name, My.Resources.ActiveDirectory))
                Try
                    cCompare.CompareTwoItems(My.Resources.OriginSQLInstance, irow.Name, "", "", My.Resources.ActiveDirectory, 0, "", _
                                             My.Resources.OriginRepository, irow.Name, "", "", My.Resources.ActiveDirectory, 0, "", True)
                Catch ex As ApplicationException
                    If Not ex.GetBaseException.Message.ToString = ArchiveCancelled Then
                        RaiseEvent ArchivingException(New ApplicationException(String.Format(My.Resources.ArchiveException, _
                                                              ex.TargetSite.Name, SQLInstance, My.Resources.ActiveDirectory), ex))
                    End If
                End Try
            End If
            If irow.Audits Then
                RaiseEvent Archiving(String.Format("{0}.{1}", irow.Name, My.Resources.Audits))
                Try
                    cCompare.CompareAllItems(My.Resources.OriginSQLInstance, irow.Name, "", My.Resources.Audits, "", _
                                             My.Resources.OriginRepository, irow.Name, "", My.Resources.Audits, "", True)
                Catch ex As ApplicationException
                    If Not ex.GetBaseException.Message.ToString = ArchiveCancelled Then
                        RaiseEvent ArchivingException(New ApplicationException(String.Format(My.Resources.ArchiveException, _
                                                              ex.TargetSite.Name, SQLInstance, My.Resources.Audits), ex))
                    End If
                End Try
            End If
            If irow.BackupDevices Then
                RaiseEvent Archiving(String.Format("{0}.{1}", irow.Name, My.Resources.BackupDevices))
                Try
                    cCompare.CompareAllItems(My.Resources.OriginSQLInstance, irow.Name, "", My.Resources.BackupDevices, "", _
                                             My.Resources.OriginRepository, irow.Name, "", My.Resources.BackupDevices, "", True)
                Catch ex As ApplicationException
                    If Not ex.GetBaseException.Message.ToString = ArchiveCancelled Then
                        RaiseEvent ArchivingException(New ApplicationException(String.Format(My.Resources.ArchiveException, _
                                                              ex.TargetSite.Name, SQLInstance, My.Resources.BackupDevices), ex))
                    End If
                End Try
            End If
            If irow.Configuration Then
                RaiseEvent Archiving(String.Format("{0}.{1}", irow.Name, My.Resources.Configuration))
                Try
                    cCompare.CompareTwoItems(My.Resources.OriginSQLInstance, irow.Name, "", "", My.Resources.Configuration, 0, "", _
                                             My.Resources.OriginRepository, irow.Name, "", "", My.Resources.Configuration, 0, "", True)
                Catch ex As ApplicationException
                    If Not ex.GetBaseException.Message.ToString = ArchiveCancelled Then
                        RaiseEvent ArchivingException(New ApplicationException(String.Format(My.Resources.ArchiveException, _
                                                              ex.TargetSite.Name, SQLInstance, My.Resources.Configuration), ex))
                    End If
                End Try
            End If
            If irow.Credentials And cCompare.SqlServer1.VersionMajor > 8 Then
                RaiseEvent Archiving(String.Format("{0}.{1}", irow.Name, My.Resources.Credentials))
                Try
                    cCompare.CompareAllItems(My.Resources.OriginSQLInstance, irow.Name, "", My.Resources.Credentials, "", _
                                             My.Resources.OriginRepository, irow.Name, "", My.Resources.Credentials, "", True)
                Catch ex As ApplicationException
                    If Not ex.GetBaseException.Message.ToString = ArchiveCancelled Then
                        RaiseEvent ArchivingException(New ApplicationException(String.Format(My.Resources.ArchiveException, _
                                                                    ex.TargetSite.Name, SQLInstance, My.Resources.Credentials), ex))
                    End If
                End Try
            End If
            If irow.CryptographicProviders And cCompare.SqlServer1.VersionMajor > 9 Then
                RaiseEvent Archiving(String.Format("{0}.{1}", irow.Name, My.Resources.CryptographicProviders))
                Try
                    cCompare.CompareAllItems(My.Resources.OriginSQLInstance, irow.Name, "", My.Resources.CryptographicProviders, "", _
                                             My.Resources.OriginRepository, irow.Name, "", My.Resources.CryptographicProviders, "", True)
                Catch ex As ApplicationException
                    If Not ex.GetBaseException.Message.ToString = ArchiveCancelled Then
                        RaiseEvent ArchivingException(New ApplicationException(String.Format(My.Resources.ArchiveException, _
                                                                    ex.TargetSite.Name, SQLInstance, My.Resources.CryptographicProviders), ex))
                    End If
                End Try
            End If
            If irow.Databases Then
                RaiseEvent Archiving(String.Format("{0}.{1}", irow.Name, My.Resources.Databases))
                Try
                    cCompare.CompareAllItems(My.Resources.OriginSQLInstance, irow.Name, "", My.Resources.Databases, "", _
                                             My.Resources.OriginRepository, irow.Name, "", My.Resources.Databases, "", True)
                Catch ex As ApplicationException
                    If Not ex.GetBaseException.Message.ToString = ArchiveCancelled Then
                        RaiseEvent ArchivingException(New ApplicationException(String.Format(My.Resources.ArchiveException, _
                                                                                  ex.TargetSite.Name, SQLInstance, My.Resources.Databases), ex))
                    End If
                End Try
            End If
            If irow.EndPoints And cCompare.SqlServer1.VersionMajor > 8 Then
                RaiseEvent Archiving(String.Format("{0}.{1}", irow.Name, My.Resources.Endpoints))
                Try
                    cCompare.CompareAllItems(My.Resources.OriginSQLInstance, irow.Name, "", My.Resources.Endpoints, "", _
                                             My.Resources.OriginRepository, irow.Name, "", My.Resources.Endpoints, "", True)
                Catch ex As ApplicationException
                    If Not ex.GetBaseException.Message.ToString = ArchiveCancelled Then
                        RaiseEvent ArchivingException(New ApplicationException(String.Format(My.Resources.ArchiveException, _
                                                                                  ex.TargetSite.Name, SQLInstance, My.Resources.Endpoints), ex))
                    End If
                End Try
            End If
            If irow.FullTextService Then
                RaiseEvent Archiving(String.Format("{0}.{1}", irow.Name, My.Resources.FullTextService))
                Try
                    cCompare.CompareTwoItems(My.Resources.OriginSQLInstance, irow.Name, "", "", My.Resources.FullTextService, 0, "", _
                                             My.Resources.OriginRepository, irow.Name, "", "", My.Resources.FullTextService, 0, "", True)
                Catch ex As ApplicationException
                    If Not ex.GetBaseException.Message.ToString = ArchiveCancelled Then
                        RaiseEvent ArchivingException(New ApplicationException(String.Format(My.Resources.ArchiveException, _
                                                                                  ex.TargetSite.Name, SQLInstance, My.Resources.FullTextService), ex))
                    End If
                End Try
            End If
            If irow.Information Then
                RaiseEvent Archiving(String.Format("{0}.{1}", irow.Name, My.Resources.Information))
                Try
                    cCompare.CompareTwoItems(My.Resources.OriginSQLInstance, irow.Name, "", "", My.Resources.Information, 0, "", _
                                             My.Resources.OriginRepository, irow.Name, "", "", My.Resources.Information, 0, "", True)
                Catch ex As ApplicationException
                    If Not ex.GetBaseException.Message.ToString = ArchiveCancelled Then
                        RaiseEvent ArchivingException(New ApplicationException(String.Format(My.Resources.ArchiveException, _
                                                                                  ex.TargetSite.Name, SQLInstance, My.Resources.Information), ex))
                    End If
                End Try
            End If
            ' no job server on SQL Express
            If Not (cCompare.SqlServer1.Information.EngineEdition = Edition.Express) Then
                If irow.Information Then
                    RaiseEvent Archiving(String.Format("{0}.{1}", irow.Name, My.Resources.JobServer))
                    Try
                        cCompare.CompareTwoItems(My.Resources.OriginSQLInstance, irow.Name, "", "", My.Resources.JobServer, 0, "", _
                                             My.Resources.OriginRepository, irow.Name, "", "", My.Resources.JobServer, 0, "", True)
                    Catch ex As ApplicationException
                        If Not ex.GetBaseException.Message.ToString = ArchiveCancelled Then
                            RaiseEvent ArchivingException(New ApplicationException(String.Format(My.Resources.ArchiveException, _
                                                                                   ex.TargetSite.Name, SQLInstance, My.Resources.JobServer), ex))
                        End If
                    End Try
                End If
                If jrow.Alerts Then
                    RaiseEvent Archiving(String.Format("{0}.{1}", irow.Name, My.Resources.Alerts))
                    Try
                        cCompare.CompareAllItems(My.Resources.OriginSQLInstance, irow.Name, "", My.Resources.Alerts, "", _
                                             My.Resources.OriginRepository, irow.Name, "", My.Resources.Alerts, "", True)
                    Catch ex As ApplicationException
                        If Not ex.GetBaseException.Message.ToString = ArchiveCancelled Then
                            RaiseEvent ArchivingException(New ApplicationException(String.Format(My.Resources.ArchiveException, _
                                                                                                 ex.TargetSite.Name, SQLInstance, My.Resources.Alerts), ex))
                        End If
                    End Try
                End If
                If jrow.AlertSystem Then
                    RaiseEvent Archiving(String.Format("{0}.{1}", irow.Name, My.Resources.AlertSystem))
                    Try
                        cCompare.CompareTwoItems(My.Resources.OriginSQLInstance, irow.Name, "", "", My.Resources.AlertSystem, 0, "", _
                                             My.Resources.OriginRepository, irow.Name, "", "", My.Resources.AlertSystem, 0, "", True)
                    Catch ex As ApplicationException
                        If Not ex.GetBaseException.Message.ToString = ArchiveCancelled Then
                            RaiseEvent ArchivingException(New ApplicationException(String.Format(My.Resources.ArchiveException, _
                                                                                                 ex.TargetSite.Name, SQLInstance, My.Resources.AlertSystem), ex))
                        End If
                    End Try
                End If
                If jrow.Jobs Then
                    RaiseEvent Archiving(String.Format("{0}.{1}", irow.Name, My.Resources.Jobs))
                    Try
                        cCompare.CompareAllItems(My.Resources.OriginSQLInstance, irow.Name, "", My.Resources.Jobs, "", _
                                             My.Resources.OriginRepository, irow.Name, "", My.Resources.Jobs, "", True)
                    Catch ex As ApplicationException
                        If Not ex.GetBaseException.Message.ToString = ArchiveCancelled Then
                            RaiseEvent ArchivingException(New ApplicationException(String.Format(My.Resources.ArchiveException, _
                                                                                                 ex.TargetSite.Name, SQLInstance, My.Resources.Jobs), ex))
                        End If
                    End Try
                End If
                If jrow.Operators Then
                    RaiseEvent Archiving(String.Format("{0}.{1}", irow.Name, My.Resources.Operators))
                    Try
                        cCompare.CompareAllItems(My.Resources.OriginSQLInstance, irow.Name, "", My.Resources.Operators, "", _
                                             My.Resources.OriginRepository, irow.Name, "", My.Resources.Operators, "", True)
                    Catch ex As ApplicationException
                        If Not ex.GetBaseException.Message.ToString = ArchiveCancelled Then
                            RaiseEvent ArchivingException(New ApplicationException(String.Format(My.Resources.ArchiveException, _
                                                                                                 ex.TargetSite.Name, SQLInstance, My.Resources.Operators), ex))
                        End If
                    End Try
                End If
                If jrow.ProxyAccounts And cCompare.SqlServer1.VersionMajor > 8 Then
                    RaiseEvent Archiving(String.Format("{0}.{1}", irow.Name, My.Resources.ProxyAccounts))
                    Try
                        cCompare.CompareAllItems(My.Resources.OriginSQLInstance, irow.Name, "", My.Resources.ProxyAccounts, "", _
                                             My.Resources.OriginRepository, irow.Name, "", My.Resources.ProxyAccounts, "", True)
                    Catch ex As ApplicationException
                        If Not ex.GetBaseException.Message.ToString = ArchiveCancelled Then
                            RaiseEvent ArchivingException(New ApplicationException(String.Format(My.Resources.ArchiveException, _
                                                                                                 ex.TargetSite.Name, SQLInstance, My.Resources.ProxyAccounts), ex))
                        End If
                    End Try
                End If
                If jrow.TargetServers And cCompare.SqlServer1.VersionMajor > 8 Then
                    RaiseEvent Archiving(String.Format("{0}.{1}", irow.Name, My.Resources.TargetServers))
                    Try
                        cCompare.CompareAllItems(My.Resources.OriginSQLInstance, irow.Name, "", My.Resources.TargetServers, "", _
                                             My.Resources.OriginRepository, irow.Name, "", My.Resources.TargetServers, "", True)
                    Catch ex As ApplicationException
                        If Not ex.GetBaseException.Message.ToString = ArchiveCancelled Then
                            RaiseEvent ArchivingException(New ApplicationException(String.Format(My.Resources.ArchiveException, _
                                                                                                 ex.TargetSite.Name, SQLInstance, My.Resources.TargetServers), ex))
                        End If
                    End Try
                End If
            End If
            If irow.LinkedServers Then
                RaiseEvent Archiving(String.Format("{0}.{1}", irow.Name, My.Resources.LinkedServers))
                Try
                    cCompare.CompareAllItems(My.Resources.OriginSQLInstance, irow.Name, "", My.Resources.LinkedServers, "", _
                                             My.Resources.OriginRepository, irow.Name, "", My.Resources.LinkedServers, "", True)
                Catch ex As ApplicationException
                    If Not ex.GetBaseException.Message.ToString = ArchiveCancelled Then
                        RaiseEvent ArchivingException(New ApplicationException(String.Format(My.Resources.ArchiveException, _
                                                                                             ex.TargetSite.Name, SQLInstance, My.Resources.LinkedServers), ex))
                    End If
                End Try
            End If
            If irow.Logins Then
                RaiseEvent Archiving(String.Format("{0}.{1}", irow.Name, My.Resources.Logins))
                Try
                    cCompare.CompareAllItems(My.Resources.OriginSQLInstance, irow.Name, "", My.Resources.Logins, "", _
                                             My.Resources.OriginRepository, irow.Name, "", My.Resources.Logins, "", True)
                Catch ex As ApplicationException
                    If Not ex.GetBaseException.Message.ToString = ArchiveCancelled Then
                        RaiseEvent ArchivingException(New ApplicationException(String.Format(My.Resources.ArchiveException, _
                                                                                             ex.TargetSite.Name, SQLInstance, My.Resources.Logins), ex))
                    End If
                End Try
            End If
            If irow.Mail And cCompare.SqlServer1.VersionMajor > 8 Then
                RaiseEvent Archiving(String.Format("{0}.{1}", irow.Name, My.Resources.Mail))
                Try
                    cCompare.CompareTwoItems(My.Resources.OriginSQLInstance, irow.Name, "", "", My.Resources.Mail, 0, "", _
                                             My.Resources.OriginRepository, irow.Name, "", "", My.Resources.Mail, 0, "", True)
                Catch ex As ApplicationException
                    If Not ex.GetBaseException.Message.ToString = ArchiveCancelled Then
                        RaiseEvent ArchivingException(New ApplicationException(String.Format(My.Resources.ArchiveException, _
                                                                                             ex.TargetSite.Name, SQLInstance, My.Resources.Mail), ex))
                    End If
                End Try
            End If
            If irow.ProxyAccount Then
                RaiseEvent Archiving(String.Format("{0}.{1}", irow.Name, My.Resources.ProxyAccount))
                Try
                    cCompare.CompareTwoItems(My.Resources.OriginSQLInstance, irow.Name, "", My.Resources.ProxyAccount, "", 0, "", _
                                             My.Resources.OriginRepository, irow.Name, "", My.Resources.ProxyAccount, "", 0, "", True)
                Catch ex As ApplicationException
                    If Not ex.GetBaseException.Message.ToString = ArchiveCancelled Then
                        RaiseEvent ArchivingException(New ApplicationException(String.Format(My.Resources.ArchiveException, _
                                                                                             ex.TargetSite.Name, SQLInstance, My.Resources.ProxyAccount), ex))
                    End If

                End Try
            End If
            If irow.ResourceGovernor And cCompare.SqlServer1.VersionMajor > 8 Then
                RaiseEvent Archiving(String.Format("{0}.{1}", irow.Name, My.Resources.ResourceGovernor))
                Try
                    cCompare.CompareTwoItems(My.Resources.OriginSQLInstance, irow.Name, "", "", My.Resources.ResourceGovernor, 0, "", _
                                             My.Resources.OriginRepository, irow.Name, "", "", My.Resources.ResourceGovernor, 0, "", True)
                Catch ex As ApplicationException
                    If Not ex.GetBaseException.Message.ToString = ArchiveCancelled Then
                        RaiseEvent ArchivingException(New ApplicationException(String.Format(My.Resources.ArchiveException, _
                                                                                             ex.TargetSite.Name, SQLInstance, My.Resources.ResourceGovernor), ex))
                    End If
                End Try
            End If
            If irow.Roles Then
                RaiseEvent Archiving(String.Format("{0}.{1}", irow.Name, My.Resources.Roles))
                Try
                    cCompare.CompareAllItems(My.Resources.OriginSQLInstance, irow.Name, "", My.Resources.Roles, "", _
                                             My.Resources.OriginRepository, irow.Name, "", My.Resources.Roles, "", True)
                Catch ex As ApplicationException
                    If Not ex.GetBaseException.Message.ToString = ArchiveCancelled Then
                        RaiseEvent ArchivingException(New ApplicationException(String.Format(My.Resources.ArchiveException, _
                                                                               ex.TargetSite.Name, SQLInstance, My.Resources.Roles), ex))
                    End If
                End Try
            End If
            If irow.Settings Then
                RaiseEvent Archiving(String.Format("{0}.{1}", irow.Name, My.Resources.Settings))
                Try
                    cCompare.CompareTwoItems(My.Resources.OriginSQLInstance, irow.Name, "", "", My.Resources.Settings, 0, "", _
                                             My.Resources.OriginRepository, irow.Name, "", "", My.Resources.Settings, 0, "", True)
                Catch ex As ApplicationException
                    If Not ex.GetBaseException.Message.ToString = ArchiveCancelled Then
                        RaiseEvent ArchivingException(New ApplicationException(String.Format(My.Resources.ArchiveException, _
                                                                                             ex.TargetSite.Name, SQLInstance, My.Resources.Settings), ex))
                    End If
                End Try
            End If
            If irow.ServerAuditSpecifications And cCompare.SqlServer1.VersionMajor > 9 Then
                RaiseEvent Archiving(String.Format("{0}.{1}", irow.Name, My.Resources.ServerAuditSpecifications))
                Try
                    cCompare.CompareAllItems(My.Resources.OriginSQLInstance, irow.Name, "", My.Resources.ServerAuditSpecifications, "", _
                                             My.Resources.OriginRepository, irow.Name, "", My.Resources.ServerAuditSpecifications, "", True)
                Catch ex As ApplicationException
                    If Not ex.GetBaseException.Message.ToString = ArchiveCancelled Then
                        RaiseEvent ArchivingException(New ApplicationException(String.Format(My.Resources.ArchiveException, _
                                                                                             ex.TargetSite.Name, SQLInstance, My.Resources.ServerAuditSpecifications), ex))
                    End If
                End Try
            End If
            If irow.Triggers And cCompare.SqlServer1.VersionMajor > 8 Then
                RaiseEvent Archiving(String.Format("{0}.{1}", irow.Name, My.Resources.Triggers))
                Try
                    cCompare.CompareAllItems(My.Resources.OriginSQLInstance, irow.Name, "", My.Resources.Triggers, "", _
                                             My.Resources.OriginRepository, irow.Name, "", My.Resources.Triggers, "", True)
                Catch ex As ApplicationException
                    If Not ex.GetBaseException.Message.ToString = ArchiveCancelled Then
                        RaiseEvent ArchivingException(New ApplicationException(String.Format(My.Resources.ArchiveException, _
                                                                                             ex.TargetSite.Name, SQLInstance, My.Resources.Triggers), ex))
                    End If
                End Try
            End If
            If irow.UserDefinedMessages And cCompare.SqlServer1.VersionMajor > 8 Then
                RaiseEvent Archiving(String.Format("{0}.{1}", irow.Name, My.Resources.UserDefinedMessages))
                Try
                    cCompare.CompareAllItems(My.Resources.OriginSQLInstance, irow.Name, "", My.Resources.UserDefinedMessages, "", _
                                             My.Resources.OriginRepository, irow.Name, "", My.Resources.UserDefinedMessages, "", True)
                Catch ex As ApplicationException
                    If Not ex.GetBaseException.Message.ToString = ArchiveCancelled Then
                        RaiseEvent ArchivingException(New ApplicationException(String.Format(My.Resources.ArchiveException, _
                                                                                             ex.TargetSite.Name, SQLInstance, My.Resources.UserDefinedMessages), ex))
                    End If
                End Try
            End If
            ArchiveSrv = True
        Catch ex As Exception
            ArchiveSrv = False
            Throw ex
        End Try
    End Function

    Friend Function ProcessDDLEvents() As Integer
        Try
            Dim HasMoreEvents As Boolean
            HasMoreEvents = (cCompare.SqlServer1.VersionMajor > 8)
            While HasMoreEvents
                ' Start a transaction for each item read from queue, must use tx to pull a row from queue 
                ' want this to assure that the event is properly transferred to the repository from the source queue
                Using scope As New TransactionScope
                    ' force a rollback using the try-catch to wrap the error in my custom message
                    Try
                        Dim EventData As SqlTypes.SqlXml = cCompare.DAL.GetDDLEvent(TargetEventNotificationConnectionString(cCompare.DAL.dsSQLCfg.tConnection.FindByInstanceName(SQLInstance)))
                        If EventData Is Nothing Or EventData.IsNull Then
                            HasMoreEvents = False
                        Else
                            ' have to crack the XML here to know what to do with it
                            Dim rdr As Xml.XmlReader = EventData.CreateReader()
                            Dim DBName As String = ""
                            Dim Collection As String = ""
                            Dim Schema As String = ""
                            Dim Item As String = ""
                            Dim Action As String = ""
                            Dim EventType As String = ""
                            Dim ObjectType As String = ""
                            Dim PropertyType As String = ""
                            ' only supposed to be one row
                            ' first tag
                            rdr.Read()
                            While Not rdr.EOF
                                Select Case UCase(rdr.Name)
                                    Case "DATABASENAME"
                                        DBName = rdr.ReadElementString
                                    Case "EVENT_INSTANCE"
                                        rdr.Read()
                                    Case "EVENTTYPE"
                                        'sys.event_notification_event_types
                                        EventType = UCase(rdr.ReadElementString)
                                        ' this may need more for audit events
                                        If InStr(EventType, "CREATE") = 1 Then
                                            ' can be INDEX, FULLTEXT INDEX, NONCLUSTERED INDEX, maybe more 
                                            ' InStr 0 based, Len 1 based 
                                            If InStr(EventType, "INDEX") = Len(EventType) - 4 Then
                                                Action = My.Resources.ArchiveChange
                                            Else
                                                Action = My.Resources.ArchiveAdd
                                            End If
                                        ElseIf InStr(EventType, "ADD") = 1 Then
                                            'ADD_ROLE_MEMBER
                                            Action = My.Resources.ArchiveChange
                                        ElseIf InStr(EventType, "ALTER") = 1 Then
                                            Action = My.Resources.ArchiveChange
                                            If InStr(EventType, "DATABASE") = Len(EventType) - 7 Then
                                                ' no guarantee that DB will aleady be parsed when we get here
                                                Item = DBName
                                            End If
                                            If InStr(EventType, "INSTANCE") = Len(EventType) - 7 Then
                                                Item = SQLInstance
                                            End If
                                        ElseIf InStr(EventType, "GRANT") = 1 Then
                                            Action = My.Resources.ArchiveChange
                                        ElseIf InStr(EventType, "DENY") = 1 Then
                                            Action = My.Resources.ArchiveChange
                                        ElseIf InStr(EventType, "REVOKE") = 1 Then
                                            Action = My.Resources.ArchiveChange
                                            If InStr(EventType, "SERVER") = Len(EventType) - 5 Then
                                                ' is a login change but is now processed as a server a 'delete'
                                            End If
                                        ElseIf InStr(EventType, "DROP") = 1 Then
                                            If InStr(EventType, "INDEX") = Len(EventType) - 4 Then
                                                Action = My.Resources.ArchiveChange
                                            Else
                                                Action = My.Resources.ArchiveDelete
                                            End If
                                        End If
                                    Case "OBJECTNAME"
                                        Select Case EventType
                                            Case "ALTER_AUTHORIZATION_DATABASE"
                                                ' item must be blank for db
                                                rdr.Skip()
                                            Case "ADD_ROLE_MEMBER"
                                                ' use "RoleName" node
                                                rdr.Skip()
                                            Case "CREATE_INDEX"
                                                ' use "TargetObjectName"
                                                rdr.Skip()
                                            Case Else
                                                Item = rdr.ReadElementString
                                        End Select
                                    Case "OBJECTTYPE"
                                        ObjectType = rdr.ReadElementString
                                        Select Case UCase(ObjectType)
                                            Case "ASSEMBLY"
                                                Collection = My.Resources.Assemblies
                                            Case "AGGREGATE"
                                                Collection = My.Resources.UserDefinedAggregates
                                            Case "APPLICATION ROLE"
                                                Collection = My.Resources.ApplicationRoles
                                            Case "ASSYMETRIC KEY"
                                                Collection = My.Resources.AsymmetricKeys
                                            Case "BROKER PRIORITY" '?
                                                Collection = My.Resources.Priorities
                                            Case "CERTIFICATE"
                                                Collection = My.Resources.Certificates
                                            Case "CONTRACT"
                                                Collection = My.Resources.ServiceContracts
                                            Case "CRYPTOGRAPHIC PROVIDER"
                                                Collection = My.Resources.CryptographicProviders
                                            Case "DATA TYPE"
                                                Collection = My.Resources.UserDefinedDataTypes
                                            Case "DATABASE"
                                                Collection = My.Resources.Databases
                                            Case "DATABASE AUDIT"
                                                Collection = My.Resources.Audits
                                            Case "FULL TEXT CATALOG"
                                                Collection = My.Resources.FullTextCatalogs
                                            Case "FUNCTION"
                                                Collection = My.Resources.UserDefinedFunctions
                                            Case "INDEX"
                                                'no-op will pick this up in targetobjecttype
                                                'Collection = "Tables"
                                                'Collection = "Views"
                                            Case "MESSAGE TYPE"
                                                Collection = My.Resources.MessageTypes
                                            Case "PARTITION FUNCTION"
                                                Collection = My.Resources.PartitionFunctions
                                            Case "PARTITION SCHEME"
                                                Collection = My.Resources.PartitionSchemes
                                            Case "PLAN GUIDE"
                                                Collection = My.Resources.PlanGuides
                                            Case "PROCEDURE"
                                                Collection = My.Resources.StoredProcedures
                                            Case "REMOTE SERVICE BINDING"
                                                Collection = My.Resources.RemoteServiceBindings
                                            Case "ROLE"
                                                Collection = My.Resources.Roles
                                            Case "ROUTE"
                                                Collection = My.Resources.Routes
                                            Case "SCHEMA"
                                                Collection = My.Resources.Schemas
                                            Case "SERVER AUDIT"
                                                Collection = My.Resources.Audits
                                            Case "SQL USER"
                                                ' use TargetObjectType
                                            Case "SYNONYM"
                                                Collection = My.Resources.Synonyms
                                            Case "SYMETRIC KEY"
                                                Collection = My.Resources.SymmetricKeys
                                            Case "TABLE"
                                                Collection = My.Resources.Tables
                                            Case "TRIGGER"
                                                Collection = My.Resources.Triggers
                                            Case "TYPE"
                                                Collection = My.Resources.UserDefinedTypes
                                            Case "VIEW"
                                                Collection = My.Resources.Views
                                            Case "WINDOWS USER"
                                                If EventType = "DROP_USER" Then
                                                    Collection = My.Resources.Users
                                                    'Else
                                                    ' use TargetObjectType
                                                End If
                                            Case "USER"
                                                Collection = My.Resources.Users
                                            Case "XML SCHEMA COLLECTION"
                                                Collection = My.Resources.XMLSchemaCollections
                                            Case Else
                                                Collection = ObjectType
                                        End Select
                                    Case "PROPERTYNAME"
                                        PropertyType = UCase(rdr.ReadElementString)
                                        Select Case PropertyType
                                            Case "SHOW ADVANCED OPTIONS"
                                                ' Done with event, this is not to be considered a change
                                                scope.Complete()
                                                Exit Try
                                            Case Else
                                                rdr.Skip()
                                        End Select
                                    Case "ROLENAME"
                                        If EventType = "ADD_ROLE_MEMBER" Then
                                            Collection = My.Resources.Roles
                                            Item = rdr.ReadElementString
                                        Else
                                            rdr.Skip()
                                        End If
                                    Case "SCHEMANAME"
                                        Schema = rdr.ReadElementString
                                    Case "TARGETOBJECTNAME"
                                        Select Case EventType
                                            Case "ALTER_INDEX"
                                                Item = rdr.ReadElementString
                                            Case "CREATE_INDEX"
                                                Item = rdr.ReadElementString
                                            Case Else
                                                rdr.Skip()
                                        End Select
                                    Case "TARGETOBJECTTYPE"
                                        Select Case EventType
                                            Case "ALTER_INDEX"
                                                Select Case UCase(rdr.ReadElementString)
                                                    Case "TABLE"
                                                        Collection = My.Resources.Tables
                                                    Case "VIEW"
                                                        Collection = My.Resources.Views
                                                End Select
                                            Case "CREATE_INDEX"
                                                Select Case UCase(rdr.ReadElementString)
                                                    Case "TABLE"
                                                        Collection = My.Resources.Tables
                                                    Case "VIEW"
                                                        Collection = My.Resources.Views
                                                End Select
                                            Case Else
                                                rdr.Skip()
                                        End Select
                                    Case Else
                                        rdr.Skip()
                                End Select
                            End While
                            rdr.Close()
                            rdr = Nothing
                            If Not Schema = "" Then
                                Item = Schema & "." & Item
                            End If
                            If DBName = "" Then
                                If Item = "" Then
                                    If Collection = "" Then
                                        RaiseEvent Archiving(String.Format("Event [{0}]", SQLInstance))
                                    Else
                                        RaiseEvent Archiving(String.Format("Event [{0}].[{1}]", SQLInstance, Collection))
                                    End If
                                Else
                                    RaiseEvent Archiving(String.Format("Event [{0}]", SQLInstance))
                                End If
                            Else
                                If Item = "" Then
                                    RaiseEvent Archiving(String.Format("Event [{0}]", SQLInstance))
                                Else
                                    If Collection = "" Then
                                        RaiseEvent Archiving(String.Format("Event [{0}].[{1}]", SQLInstance, DBName))
                                    Else
                                        RaiseEvent Archiving(String.Format("Event [{0}].[{1}].[{2}]", SQLInstance, DBName, Collection))
                                    End If
                                End If
                            End If
                            cCompare.CompareTwoItems(My.Resources.OriginSQLInstance, _
                                                     cCompare.SqlServer1.ConnectionContext.TrueName, _
                                                     DBName, _
                                                     Collection, _
                                                     If((Action = My.Resources.ArchiveDelete), "", Item), _
                                                     0, _
                                                     "", _
                                                     My.Resources.OriginRepository, _
                                                     cCompare.SqlServer1.ConnectionContext.TrueName, _
                                                     DBName, _
                                                     Collection, _
                                                     Item, _
                                                     0, _
                                                     "", _
                                                     True, _
                                                     EventData, _
                                                     Action)
                        End If
                        scope.Complete()
                    Catch ex As Exception
                        ' if scope.Complete is never called a rollback will occur when this exception is thrown
                        Throw New Exception(String.Format(My.Resources.ArchiveDDLEventsException, _
                                                          cCompare.SqlServer1.ConnectionContext.TrueName), ex)
                    End Try
                End Using
                ' increment accumulator for archive log record
                NbrDDLEventsProcessed += 1
            End While
            Return NbrDDLEventsProcessed
        Catch ex As Exception
            Throw ex
        End Try
    End Function

#End Region

End Class
