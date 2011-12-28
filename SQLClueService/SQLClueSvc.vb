Imports System.ComponentModel
Imports System.Deployment
Imports System.Timers

Public Class SQLClueSvc

    Private t As Timer
    Private DAL As New cCommon.cDataAccess
    Private SQLCfg As New cCommon.DataClassesSQLConfigurationDataContext
    Private ActiveArchiveId As Integer 'New cCommon.dsSQLConfiguration.tSQLCfgScheduleDataTable
    Delegate Sub ArchiveProgressEventHandlerDelegate(ByVal Item As String)
    Delegate Sub ArchiveDoWorkExceptionHandlerDelegate(ByVal ArchiveException As Exception)
    Private RunbookFileWatcherThread As System.Threading.Thread
    Friend Shared ConfiguredInstanceList() As String
    Private BeenNagged As Boolean
    Private RepositoryThreadCount As Integer
    Private RunbookThreadCount As Integer

    Protected Structure LicenseStruct
        Friend LicensedCompany As String
        Friend LocalKey As String
        Friend LicenseLevel As String
        Friend LicenseDate As Date
        Friend TargetCount As Integer
        Friend TargetCountDt As DateTime
    End Structure

    Private Sub WorkerLoop(ByVal sender As Object, ByVal e As ElapsedEventArgs)
        ' called every My.Settings.TimerInterval Seconds
        Try
            ' include all servers that are configured locally for archive in the a list
            ' reset the list every time the timer fires
            ' at system startup, the service may attemp this connection before the db is on line
            ' the timer may still need to fire a few times to get past this trap

            SQLCfg.Connection.ConnectionString = DAL.LocalRepositoryConnectionString
            ConfiguredInstanceList = DAL.GetConfiguredInstanceList()
            For i As Integer = 0 To ConfiguredInstanceList.Length - 1
                ConfiguredInstanceList(i) = UCase(ConfiguredInstanceList(i))
            Next
            RunScheduler()
        Catch ex As Exception
            HandleException(New ApplicationException(My.Resources.ServiceControlException, ex))
        End Try
    End Sub

    Private Sub RunScheduler()
        ' keep ping in sync with code in console
        Try
            ' each archive will run on it's on thread
            ' here, only one thread is possible but
            ' the call to async archive can create as many as desired I believe
            If My.Settings.RepositoryEnabled Then

                'PingSQLCfg leaks a bit of memory - do not use on main thread 

                If RepositoryThreadCount = 0 Then
                    RepositoryThreadCount = 1
                    ArchiveBackgroundWorker.RunWorkerAsync()
                End If ' archive thread is busy
                ' run the file watcher continuously 
                ' use lowest priorty thread
                ' loop continuously
                ' pause between documents 
                If My.Settings.RunbookEnabled _
                And RunbookThreadCount = 0 Then
                    RunbookThreadCount = 1
                    RunbookBackgroundWorker.RunWorkerAsync()
                End If
            End If 'repository enabled
        Catch ex As Exception
            HandleException(New ApplicationException(My.Resources.ServiceControlException, ex))
        End Try
    End Sub

#Region " Service Controls "

    Protected Overrides Sub OnStart(ByVal args() As String)
        Try
            EventLogSvc.WriteEntry(My.Resources.SvcStartMsg, EventLogEntryType.Information)
            ' the Console's SetupForm Service Control Manager will always pass the enable flags 
            ' and then all connection attributes for each component
            ' if there are args, parse and store in the config file
            ' blanks wont pass well, use a single dash "-" filler for any blank string value
            ' if no args then is a service manager operation so use existing settings
            ' to make the service more stable, put anything that does work and can error inside the timer 
            If args.Length > 0 Then
                My.Settings.RepositoryEnabled = CBool(args(0))
                My.Settings.RunbookEnabled = CBool(args(1))
                My.Settings.RepositoryInstanceName = args(3).ToString
                My.Settings.RepositoryDatabaseName = args(4).ToString
                My.Settings.RepositoryUseTrustedConnection = CBool(args(5))
                My.Settings.RepositorySQLLoginName = If(args(6).ToString = "-", "", args(6).ToString)
                My.Settings.RepositorySQLLoginPassword = If(args(7).ToString = "-", "", args(7).ToString)
                My.Settings.RepositoryNetworkLibrary = If(args(8).ToString = "-", "", args(8).ToString)
                My.Settings.RepositoryConnectionTimeout = CInt(args(9))
                My.Settings.RepositoryEncryptConnection = CBool(args(10))
                My.Settings.RepositoryTrustServerCertificate = CBool(args(11))
                My.Settings.RunbookInstanceName = If(args(12).ToString = "-", "", args(12).ToString)
                My.Settings.RunbookDatabaseName = If(args(13).ToString = "-", "", args(13).ToString)
                My.Settings.RunbookUseTrustedConnection = CBool(args(14))
                My.Settings.RunbookSQLLoginName = If(args(15).ToString = "-", "", args(15).ToString)
                My.Settings.RunbookSQLLoginPassword = If(args(16).ToString = "-", "", args(16).ToString)
                My.Settings.RunbookNetworkLibrary = If(args(17).ToString = "-", "", args(17).ToString)
                My.Settings.RunbookConnectionTimeout = CInt(args(18))
                My.Settings.RunbookEncryptConnection = CBool(args(19))
                My.Settings.RunbookTrustServerCertificate = CBool(args(20))
                My.Settings.TimerIntervalSeconds = CInt(args(31))
                My.Settings.DocumentMonitorSleepSeconds = CInt(args(31))
                My.Settings.WaitBetweenDocumentsSeconds = CInt(args(32))
                My.Settings.Save()
            End If
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
            RepositoryThreadCount = 0
            RunbookThreadCount = 0
            BeenNagged = False
            t = New Timer(My.Settings.TimerIntervalSeconds * 1000)
            RemoveHandler t.Elapsed, AddressOf WorkerLoop
            AddHandler t.Elapsed, AddressOf WorkerLoop
            With t
                .AutoReset = True
                .Enabled = True
                .Start()
            End With
        Catch ex As Exception
            HandleException(New ApplicationException(My.Resources.ServiceControlException, ex))
        End Try
    End Sub

    Protected Overrides Sub OnPause()
        Try
            EventLogSvc.WriteEntry(My.Resources.SvcPauseMsg, EventLogEntryType.Information)
            If Not t Is Nothing Then
                ' stop the timer
                t.Stop()
            End If
            ' active tasks should continue
        Catch ex As Exception
            HandleException(New ApplicationException(My.Resources.ServiceControlException, ex))
        End Try
    End Sub

    Protected Overrides Sub OnContinue()
        Try
            EventLogSvc.WriteEntry(My.Resources.SvcResumeMsg, EventLogEntryType.Information)
            ' only a restart can pickup connection changes
            'start the timer
            If t Is Nothing Then
                t = New Timer(My.Settings.TimerIntervalSeconds * 1000)
                RemoveHandler t.Elapsed, AddressOf WorkerLoop
                AddHandler t.Elapsed, AddressOf WorkerLoop
                With t
                    .AutoReset = True
                    .Enabled = True
                    .Start()
                End With
            Else
                t.Start()
            End If
        Catch ex As Exception
            HandleException(New ApplicationException(My.Resources.ServiceControlException, ex))
        End Try
    End Sub

    Protected Overrides Sub OnShutdown()
        Try
            EventLogSvc.WriteEntry(My.Resources.SvcStopMsg, EventLogEntryType.Information)
            ' stop the timer
            t.Stop()
            t.Dispose()
            t = Nothing
            ' cancel any active archive 
            If (RepositoryThreadCount > 0 _
                Or ArchiveBackgroundWorker.IsBusy) _
            And Not ArchiveBackgroundWorker.CancellationPending Then
                ' will finish processing current Item or Collection
                ArchiveBackgroundWorker.CancelAsync()
            End If
            If RunbookThreadCount > 0 _
            And Not RunbookBackgroundWorker.CancellationPending Then
                ' will finish processing current file  
                RunbookBackgroundWorker.CancelAsync()
            End If
        Catch ex As Exception
            HandleException(New ApplicationException(My.Resources.ServiceControlException, ex))
        End Try
    End Sub

    Protected Overrides Sub OnStop()
        Try
            ' stop the timer
            EventLogSvc.WriteEntry(My.Resources.SvcStopMsg, EventLogEntryType.Information)
            t.Stop()
            t.Dispose()
            t = Nothing
            ' cancel any active archive 
            If (RepositoryThreadCount = 1 _
                Or ArchiveBackgroundWorker.IsBusy) _
            And Not ArchiveBackgroundWorker.CancellationPending Then
                ' will finish processing current Item or Collection
                ArchiveBackgroundWorker.CancelAsync()
            End If
            If RunbookThreadCount = 1 _
            And Not RunbookBackgroundWorker.CancellationPending Then
                ' will finish processing current file  
                RunbookBackgroundWorker.CancelAsync()
            End If
        Catch ex As Exception
            HandleException(New ApplicationException(My.Resources.ServiceControlException, ex))
        End Try
    End Sub

    Public Sub New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
    End Sub

#End Region

#Region " Error Handling "
    '' this region must remain consistent with the admin console error handler
    Friend Sub HandleException(ByVal ex As System.Exception)
        Dim EventLog1 As New System.Diagnostics.EventLog("Application", ".", My.Resources.ServiceName)
        Try
            Dim ex1 As System.Exception = ex
            Dim ex1Msg As String = ex1.Message & vbCrLf & vbCrLf
            While Not ex1.InnerException Is Nothing
                ex1Msg += ex1.InnerException.Message & vbCrLf & vbCrLf
                ex1 = ex1.InnerException
            End While
            ex1Msg += My.Resources.ExceptionInfoMsg
            EventLog1.WriteEntry(ex1Msg, EventLogEntryType.Error, 101)
        Catch x As Exception
            Try
                Dim x1 As System.Exception = x
                Dim Msg1 As String = x1.Message
                While Not x1.InnerException Is Nothing
                    Msg1 = Msg1 & vbCrLf & vbCrLf & x1.InnerException.Message & _
                           vbCrLf & vbCrLf & " while attempting to handle exception: " & _
                           If(ex Is Nothing, "?", ex.ToString)
                    x1 = x1.InnerException
                End While
                EventLog1.WriteEntry(Msg1, EventLogEntryType.Error, 101)
                Me.Stop()
            Catch y As Exception
                Throw y
            End Try
        End Try
    End Sub

#End Region

#Region " Configuration Archive "
    Private Sub ArchiveBackgroundWorker_DoWork(ByVal sender As Object, _
                                               ByVal e As System.ComponentModel.DoWorkEventArgs) _
    Handles ArchiveBackgroundWorker.DoWork
        Dim worker As BackgroundWorker = CType(sender, BackgroundWorker)
        e.Result = CheckForArchiveEvents(worker, e)
    End Sub

    Private Sub ArchiveBackgroundWorker_RunWorkerCompleted(ByVal sender As Object, _
                                                           ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) _
    Handles ArchiveBackgroundWorker.RunWorkerCompleted
        RepositoryThreadCount = 0
        If (e.Error IsNot Nothing) Then
            EventLogSvc.WriteEntry(String.Format(My.Resources.ArchiveException, ActiveArchiveId), EventLogEntryType.Error)
            HandleException(e.Error)
        ElseIf e.Cancelled Then
            EventLogSvc.WriteEntry(String.Format(My.Resources.ArchiveCancelled, ActiveArchiveId), EventLogEntryType.Warning)
        Else
            If CInt(e.Result) <> 0 Then
                EventLogSvc.WriteEntry(String.Format(My.Resources.ArchiveComplete, e.Result), EventLogEntryType.Information)
            End If
        End If
        ActiveArchiveId = 0
    End Sub

    Private Function CheckForArchiveEvents(ByVal worker As BackgroundWorker, _
                                           ByVal e As DoWorkEventArgs) As Int32
        If worker.CancellationPending Then
            e.Cancel = True
        Else
            Try
                'get the current schedules
                DAL.LoadConnections()
                DAL.LoadSchedule()
                ' get all archive schedule rows 
                Dim ArchiveScheduleView As DataView = New DataView()
                With ArchiveScheduleView
                    .Table = DAL.dsSQLCfg.tSchedule
                    .RowFilter = String.Format(My.Resources.ScheduleRowFilter, Now.ToString)
                    .Sort = My.Resources.ScheduleSortOrder
                    .RowStateFilter = DataViewRowState.CurrentRows
                End With
                For Each r As DataRowView In ArchiveScheduleView
                    ' one thread for archives
                    ' Could overwhelm host if multiple archives are running
                    ' would also need to prevent concurrent archives at any target 
                    If ActiveArchiveId = 0 Then
                        'AndAlso DAL.AvailableLicenses >= 0 Then
                        StartArchive(CInt(r("Id")), r("InstanceName").ToString)
                    End If ' active archive count for instance
                Next ' schedule
            Catch ex As Exception
                HandleException(New ApplicationException(String.Format(My.Resources.ArchiveException, ActiveArchiveId), ex))
            End Try
        End If ' e.cancel state
    End Function


    Public Function StartArchive(ByVal ArchiveId As Integer, ByVal InstanceName As String) As Integer
        Dim cArchive1 As New cCommon.cArchive
        cArchive1.RepositoryConnectionTimeout = My.Settings.RepositoryConnectionTimeout
        cArchive1.RepositoryDatabaseName = My.Settings.RepositoryDatabaseName
        cArchive1.RepositoryInstanceName = My.Settings.RepositoryInstanceName
        cArchive1.RepositoryUseTrustedConnection = My.Settings.RepositoryUseTrustedConnection
        cArchive1.RepositorySQLLoginName = My.Settings.RepositorySQLLoginName
        cArchive1.RepositorySQLLoginPassword = My.Settings.RepositorySQLLoginPassword
        cArchive1.RepositoryEncryptConnection = My.Settings.RepositoryEncryptConnection
        cArchive1.RepositoryTrustServerCertificate = My.Settings.RepositoryTrustServerCertificate
        cArchive1.RepositoryNetworkLibrary = My.Settings.RepositoryNetworkLibrary
        cArchive1.ArchiveComplete = String.Format(My.Resources.ArchiveComplete, InstanceName)
        cArchive1.ArchiveCancelled = String.Format(My.Resources.ArchiveCancelled, InstanceName)
        cArchive1.ArchiveRescheduled = My.Resources.ArchiveRescheduled
        'can be set but by default no need to make the handshake any faster than the connection from here  
        cArchive1.HandshakeConnectionTimeOut = If(My.Settings.TargetHandshakeConnectionTimeout < 1, _
                                                  My.Settings.RepositoryConnectionTimeout, _
                                                  My.Settings.TargetHandshakeConnectionTimeout)
        ActiveArchiveId = ArchiveId
        EventLogSvc.WriteEntry(String.Format(My.Resources.ArchiveStarting, ActiveArchiveId), _
                             EventLogEntryType.Information)
        If cArchive1.Archive(ArchiveId) Then
            StartArchive = ArchiveId
        End If
    End Function

    Private Sub ArchiveDoWorkExceptionHandler(ByVal ArchiveException As Exception)
        Try
            HandleException(New ApplicationException(String.Format(My.Resources.ArchiveWorkerExceptionInfoMsg, _
                                                                   My.Resources.ServiceName, _
                                                                   ArchiveException.GetType.ToString, _
                                                                   ActiveArchiveId), ArchiveException))
        Catch ex As Exception
            HandleException(New ApplicationException(String.Format(My.Resources.ArchiveException, ActiveArchiveId), ex))
        End Try
    End Sub

#End Region

#Region " Runbook "

    Private Sub RunbookBackgroundWorker_DoWork(ByVal sender As Object, _
                                               ByVal e As System.ComponentModel.DoWorkEventArgs) _
    Handles RunbookBackgroundWorker.DoWork
        Dim worker As BackgroundWorker = _
    CType(sender, BackgroundWorker)
        e.Result = CheckForRunbookEvents(worker, e)
    End Sub

    Private Sub RunbookBackgroundWorker_RunWorkerCompleted(ByVal sender As Object, _
                                                           ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) _
    Handles RunbookBackgroundWorker.RunWorkerCompleted
        RunbookThreadCount = 0
        If (e.Error IsNot Nothing) Then
            HandleException(New ApplicationException(My.Resources.RunbookException, e.Error))
        End If
    End Sub

    Private Function CheckForRunbookEvents(ByVal worker As BackgroundWorker, _
                                           ByVal e As DoWorkEventArgs) As Int32
        Try
            ' check for out of date indexes (d.LastModifiedDt > File last modified date)
            RunbookFileWatcherThread = New Thread(AddressOf RunbookMonitor)
            With RunbookFileWatcherThread
                .Priority = ThreadPriority.Lowest
                .Start()
            End With
            ' wait for the thread to finish
            RunbookFileWatcherThread.Join()
            'wait for DocumentMonitorSleepSeconds To check again
            Threading.Thread.Sleep(My.Settings.DocumentMonitorSleepSeconds * 1000) ' arg is in milliseconds
        Catch ex As Exception
            HandleException(New ApplicationException(My.Resources.RunbookException, ex))
        End Try
        Return 0
    End Function

    Private Sub RunbookMonitor()
        Try
            '            If DAL.AvailableLicenses >= 0 Then
            Dim run As New cCommon.cFileWatcher
            run.RunbookInstanceName = My.Settings.RunbookInstanceName
            run.RunbookDatabaseName = My.Settings.RunbookDatabaseName
            run.RunbookUseTrustedConnection = My.Settings.RunbookUseTrustedConnection
            run.RunbookSQLLoginName = My.Settings.RunbookSQLLoginName
            run.RunbookSQLLoginPassword = My.Settings.RunbookSQLLoginPassword
            run.RunbookConnectionTimeout = My.Settings.RunbookConnectionTimeout
            run.RunbookNetworkLibrary = My.Settings.RunbookNetworkLibrary
            run.RunbookEncryptConnection = My.Settings.RunbookEncryptConnection
            run.RunbookTrustServerCertificate = My.Settings.RunbookTrustServerCertificate
            ' one pass through docs currently marked "watch for change"
            run.RunbookMonitor(My.Settings.WaitBetweenDocumentsSeconds)
            '           End If
        Catch ex As Exception
            HandleException(New ApplicationException(My.Resources.RunbookException, ex))
        End Try
    End Sub

#End Region

End Class
