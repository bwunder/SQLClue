' for the backgroundworker
'Imports System.ComponentModel

' decoration prevents breakpoints but allows backgroundworker error handling to work
' for backgroundworker that accesses the class (eg cArchive)
'<System.Diagnostics.DebuggerNonUserCodeAttribute()> _
Public Class cCompare
    ' definitely a type here
    Public SqlServer1 As New Server
    Public SqlServer2 As New Server
    Public Repository1 As New Server
    Public Repository2 As New Server
    Public Event Excptn(ByVal exMsg As String)
    Public Event ArchiveAction(ByVal Action As String)
    Public Event Comparing(ByVal Item1 As String, _
                           ByVal Item2 As String)
    Public Event NameResult(ByVal ResultType As String, _
                            ByVal Item1 As String, _
                            ByVal Item2 As String)
    Public Event LineResult(ByVal ResultType As String, _
                            ByVal Item1 As String, _
                            ByVal Line1Number As Int32, _
                            ByVal Line1 As String, _
                            ByVal Item2 As String, _
                            ByVal Line2Number As Int32, _
                            ByVal Line2 As String)
    Public Event PercentDone(ByVal Value As Int32)
    Public CompareComplete As String
    Public CompareCancelled As String
    Public _ScriptingOptions As New ScriptingOptions
    Public _NameMatchPattern As String
    Public _NameMatchOptions As New RegexOptions
    Public _LineSplitPattern As String
    Public _LineSplitOptions As New RegexOptions
    Public _LineReplacePattern As String
    Public _LineReplacement As String
    Public _LineReplaceOptions As New RegexOptions
    Public _IgnoreBlankLines As Boolean
    Public _IncludeDrop As Boolean
    Public _IncludeIfExistsWithDrop As Boolean
    Public _BatchSeparator As String
    Public _IncludeUnmatched As Boolean
    Public _ReportDetails As Boolean
    Public _CompareCollection As Boolean
    Private bwPercentDone As Int32
    Private scrptr As New Scripter()
    Private cDbDocumentor1 As New cDbDocumentor
    ' wonder what the penalty is for carrying the extra instantiation when I don't need it 
    ' the archive will only ever use 1, only compare where SQL to SQL needs 2
    Private cDbDocumentor2 As New cDbDocumentor
    Public DAL As New cDataAccess

#Region " background worker"

    Private WithEvents backgroundWorker1 As BackgroundWorker
    Private eventArgs As System.ComponentModel.DoWorkEventArgs

    Public Sub AsyncCompareAllItems(ByVal Origin1 As String, _
                                    ByVal Instance1 As String, _
                                    ByVal DbName1 As String, _
                                    ByVal Collection1 As String, _
                                    ByVal ItemLabel1 As String, _
                                    ByVal Origin2 As String, _
                                    ByVal Instance2 As String, _
                                    ByVal DbName2 As String, _
                                    ByVal Collection2 As String, _
                                    ByVal ItemLabel2 As String, _
                                    Optional ByVal ArchiveIfDifferent As Boolean = False, _
                                    Optional ByVal RepositoryConnectionString1 As String = "", _
                                    Optional ByVal RepositoryConnectionString2 As String = "")
        Me.backgroundWorker1 = New BackgroundWorker
        Me.backgroundWorker1.WorkerReportsProgress = True
        Me.backgroundWorker1.WorkerSupportsCancellation = True
        Dim ParmBall As String()
        ReDim Preserve ParmBall(12)
        ParmBall(0) = Origin1
        ParmBall(1) = Instance1
        ParmBall(2) = DbName1
        ParmBall(3) = Collection1
        ParmBall(4) = ItemLabel1
        ParmBall(5) = Origin2
        ParmBall(6) = Instance2
        ParmBall(7) = DbName2
        ParmBall(8) = Collection2
        ParmBall(9) = ItemLabel2
        ParmBall(10) = ArchiveIfDifferent.ToString
        ParmBall(11) = RepositoryConnectionString1
        ParmBall(12) = RepositoryConnectionString2
        Me.backgroundWorker1.RunWorkerAsync(ParmBall)
    End Sub

    Public Sub AsyncCompareTwoItems(ByVal Origin1 As String, _
                                    ByVal Instance1 As String, _
                                    ByVal DbName1 As String, _
                                    ByVal Collection1 As String, _
                                    ByVal Item1 As String, _
                                    ByVal ItemVersion1 As Integer, _
                                    ByVal ItemLabel1 As String, _
                                    ByVal Origin2 As String, _
                                    ByVal Instance2 As String, _
                                    ByVal DbName2 As String, _
                                    ByVal Collection2 As String, _
                                    ByVal Item2 As String, _
                                    ByVal ItemVersion2 As Integer, _
                                    ByVal ItemLabel2 As String, _
                                    Optional ByVal RepositoryConnectionString1 As String = "", _
                                    Optional ByVal RepositoryConnectionString2 As String = "")
        Me.backgroundWorker1 = New BackgroundWorker
        Me.backgroundWorker1.WorkerReportsProgress = True
        Me.backgroundWorker1.WorkerSupportsCancellation = True
        Dim ParmBall As String()
        ReDim Preserve ParmBall(16)
        ParmBall(0) = Origin1
        ParmBall(1) = Instance1
        ParmBall(2) = DbName1
        ParmBall(3) = Collection1
        ParmBall(4) = Item1
        ParmBall(5) = ItemVersion1.ToString
        ParmBall(6) = ItemLabel1
        ParmBall(7) = Origin2
        ParmBall(8) = Instance2
        ParmBall(9) = DbName2
        ParmBall(10) = Collection2
        ParmBall(11) = Item2
        ParmBall(12) = ItemVersion2.ToString
        ParmBall(13) = ItemLabel2
        ParmBall(14) = RepositoryConnectionString1
        ParmBall(15) = RepositoryConnectionString2
        Me.backgroundWorker1.RunWorkerAsync(ParmBall)
    End Sub

    Public Sub CancelAsyncCompare()
        If Not Me.backgroundWorker1 Is Nothing Then
            Me.backgroundWorker1.CancelAsync()
        End If
    End Sub

    <System.Diagnostics.DebuggerNonUserCodeAttribute()> _
    Private Sub backgroundWorker1_DoWork( _
    ByVal sender As Object, ByVal e As DoWorkEventArgs) _
    Handles backgroundWorker1.DoWork
        'http://www.developerdotstar.com/community/node/671
        'http://msdn.microsoft.com/msdnmag/issues/05/03/AdvancedBasics/
        eventArgs = e
        ' Do not access the form's BackgroundWorker reference directly.
        ' Instead, use the reference provided by the sender parameter.
        Dim bw As BackgroundWorker = CType(sender, BackgroundWorker)
        ' Extract the arguments (see async call above
        Dim ParmBall As String() = CType(e.Argument, String())
        ' only want to submit if not running but want to check cancel often
        If ParmBall.Length = 13 Then
            ' Start the time-consuming operation.
            e.Result = CompareAllItems(ParmBall(0), ParmBall(1), ParmBall(2), ParmBall(3), _
                                       ParmBall(4), ParmBall(5), ParmBall(6), ParmBall(7), _
                                       ParmBall(8), ParmBall(9), CBool(ParmBall(10)), _
                                       ParmBall(11), ParmBall(12))
        Else
            e.Result = CompareTwoItems(ParmBall(0), ParmBall(1), ParmBall(2), ParmBall(3), _
                                       ParmBall(4), CInt(ParmBall(5)), ParmBall(6), ParmBall(7), _
                                       ParmBall(8), ParmBall(9), ParmBall(10), ParmBall(11), _
                                       CInt(ParmBall(12)), ParmBall(13), False, _
                                       Nothing, "", ParmBall(14), ParmBall(15))
        End If
        ' If the operation was cancelled by the user, 
        ' set the DoWorkEventArgs.Cancel property to true.
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

    Private Sub backgroundWorker1_RunWorkerCompleted( _
    ByVal sender As Object, ByVal e As RunWorkerCompletedEventArgs) _
    Handles backgroundWorker1.RunWorkerCompleted
        ' somewhere I read that when e is touched the unhandle exception could be handled, guess not...
        'Try
        If Not e.Error Is Nothing Then
            ' a local copy of the ex is used to concat the messages 
            Dim ex As System.Exception = e.Error
            Dim exMsg As String = ex.Message
            While Not ex.InnerException Is Nothing
                exMsg = exMsg & vbCrLf & vbCrLf & ex.InnerException.Message
                ex = ex.InnerException
            End While
            RaiseEvent Excptn(exMsg)
            'Throw New Exception("(cCompare.backgroundWorker1_RunWorkerCompleted) Exception", e.Error)
        ElseIf e.Cancelled Then
            ' The user cancelled the operation.
            RaiseEvent Comparing(CompareCancelled, CompareCancelled)
        Else
            RaiseEvent Comparing(CompareComplete, CompareComplete)
        End If
        ' dispose of the backgroundworker
        backgroundWorker1.Dispose()
        backgroundWorker1 = Nothing
        'Catch ex As Exception
        'Throw New Exception("(cCompare.backgroundWorker1_RunWorkerCompleted) Exception on background thread.", e.Error)
        'End Try
    End Sub

#End Region

    Public Sub SetSmoFieldsToLoad(ByVal smoSQLServer As Server)
        Try
            smoSQLServer.SetDefaultInitFields(GetType(Agent.Alert), New String() {My.Resources.SMOName})
            smoSQLServer.SetDefaultInitFields(GetType(ApplicationRole), New String() {My.Resources.SMOName})
            smoSQLServer.SetDefaultInitFields(GetType(Audit), New String() {My.Resources.SMOName})
            smoSQLServer.SetDefaultInitFields(GetType(SqlAssembly), New String() {My.Resources.SMOName})
            smoSQLServer.SetDefaultInitFields(GetType(AsymmetricKey), New String() {My.Resources.SMOName})
            smoSQLServer.SetDefaultInitFields(GetType(BackupDevice), New String() {My.Resources.SMOName})
            smoSQLServer.SetDefaultInitFields(GetType(BrokerService), New String() {My.Resources.SMOName, _
                                                                                    My.Resources.SMOIsSystemObject})
            smoSQLServer.SetDefaultInitFields(GetType(BrokerPriority), New String() {My.Resources.SMOName})
            smoSQLServer.SetDefaultInitFields(GetType(Certificate), New String() {My.Resources.SMOName})
            smoSQLServer.SetDefaultInitFields(GetType(Credential), New String() {My.Resources.SMOName})
            smoSQLServer.SetDefaultInitFields(GetType(Database), New String() {My.Resources.SMOName, _
                                                                               My.Resources.SMOIsSystemObject, _
                                                                               My.Resources.SMOIsAccessible})
            smoSQLServer.SetDefaultInitFields(GetType(DatabaseAuditSpecification), New String() {My.Resources.SMOName})
            smoSQLServer.SetDefaultInitFields(GetType(DatabaseDdlTrigger), New String() {My.Resources.SMOName, _
                                                                                         My.Resources.SMOIsSystemObject})
            smoSQLServer.SetDefaultInitFields(GetType(DatabaseRole), New String() {My.Resources.SMOName, _
                                                                                   My.Resources.SMOIsFixedRole})
            smoSQLServer.SetDefaultInitFields(GetType(Endpoint), New String() {My.Resources.SMOName, _
                                                                               My.Resources.SMOIsSystemObject})
            smoSQLServer.SetDefaultInitFields(GetType(ExtendedStoredProcedure), New String() {My.Resources.SMOName, _
                                                                                              My.Resources.SMOIsSystemObject})
            smoSQLServer.SetDefaultInitFields(GetType(FullTextCatalog), New String() {My.Resources.SMOName})
            smoSQLServer.SetDefaultInitFields(GetType(FullTextStopList), New String() {My.Resources.SMOName})
            smoSQLServer.SetDefaultInitFields(GetType(Agent.Job), New String() {My.Resources.SMOName})
            smoSQLServer.SetDefaultInitFields(GetType(LinkedServer), New String() {My.Resources.SMOName})
            smoSQLServer.SetDefaultInitFields(GetType(Login), New String() {My.Resources.SMOName, _
                                                                            My.Resources.SMOIsSystemObject})
            smoSQLServer.SetDefaultInitFields(GetType(MessageType), New String() {My.Resources.SMOName, _
                                                                                  My.Resources.SMOIsSystemObject})
            smoSQLServer.SetDefaultInitFields(GetType(Agent.Operator), New String() {My.Resources.SMOName})
            smoSQLServer.SetDefaultInitFields(GetType(PartitionFunction), New String() {My.Resources.SMOName})
            smoSQLServer.SetDefaultInitFields(GetType(PartitionScheme), New String() {My.Resources.SMOName})
            smoSQLServer.SetDefaultInitFields(GetType(PlanGuide), New String() {My.Resources.SMOName})
            smoSQLServer.SetDefaultInitFields(GetType(Agent.ProxyAccount), New String() {My.Resources.SMOName})
            smoSQLServer.SetDefaultInitFields(GetType(ResourcePool), New String() {My.Resources.SMOName})
            smoSQLServer.SetDefaultInitFields(GetType(ServerDdlTrigger), New String() {My.Resources.SMOName, _
                                                                                       My.Resources.SMOIsSystemObject})
            smoSQLServer.SetDefaultInitFields(GetType(ServerRole), New String() {My.Resources.SMOName})
            smoSQLServer.SetDefaultInitFields(GetType(Smo.Rule), New String() {My.Resources.SMOName})
            smoSQLServer.SetDefaultInitFields(GetType(Schema), New String() {My.Resources.SMOName})
            smoSQLServer.SetDefaultInitFields(GetType(ServerAuditSpecification), New String() {My.Resources.SMOName})
            smoSQLServer.SetDefaultInitFields(GetType(ServiceQueue), New String() {My.Resources.SMOName, _
                                                                                   My.Resources.SMOSchema})
            smoSQLServer.SetDefaultInitFields(GetType(ServiceRoute), New String() {My.Resources.SMOName})
            smoSQLServer.SetDefaultInitFields(GetType(ServiceContract), New String() {My.Resources.SMOName, _
                                                                                      My.Resources.SMOIsSystemObject})
            smoSQLServer.SetDefaultInitFields(GetType(StoredProcedure), New String() {My.Resources.SMOName, _
                                                                                      My.Resources.SMOSchema, _
                                                                                      My.Resources.SMOIsSystemObject})
            smoSQLServer.SetDefaultInitFields(GetType(SymmetricKey), New String() {My.Resources.SMOName})
            smoSQLServer.SetDefaultInitFields(GetType(Synonym), New String() {My.Resources.SMOName, _
                                                                              My.Resources.SMOSchema})
            smoSQLServer.SetDefaultInitFields(GetType(Table), New String() {My.Resources.SMOName, _
                                                                            My.Resources.SMOSchema, _
                                                                            My.Resources.SMOIsSystemObject})
            smoSQLServer.SetDefaultInitFields(GetType(View), New String() {My.Resources.SMOName, _
                                                                               My.Resources.SMOSchema, _
                                                                               My.Resources.SMOIsSystemObject})
            smoSQLServer.SetDefaultInitFields(GetType(Trigger), New String() {My.Resources.SMOName})
            smoSQLServer.SetDefaultInitFields(GetType(User), New String() {My.Resources.SMOName, _
                                                                           My.Resources.SMOIsSystemObject})
            smoSQLServer.SetDefaultInitFields(GetType(UserDefinedAggregate), New String() {My.Resources.SMOName, _
                                                                                           My.Resources.SMOSchema})
            smoSQLServer.SetDefaultInitFields(GetType(UserDefinedDataType), New String() {My.Resources.SMOName, _
                                                                                          My.Resources.SMOSchema})
            smoSQLServer.SetDefaultInitFields(GetType(UserDefinedFunction), New String() {My.Resources.SMOName, _
                                                                                          My.Resources.SMOSchema, _
                                                                                          My.Resources.SMOIsSystemObject})
            smoSQLServer.SetDefaultInitFields(GetType(UserDefinedTableType), New String() {My.Resources.SMOName, _
                                                                                           My.Resources.SMOSchema})
            smoSQLServer.SetDefaultInitFields(GetType(UserDefinedMessage), New String() {My.Resources.SMOID, _
                                                                                         My.Resources.SMOLanguage})
            smoSQLServer.SetDefaultInitFields(GetType(UserDefinedType), New String() {My.Resources.SMOName, _
                                                                                      My.Resources.SMOSchema})
            smoSQLServer.SetDefaultInitFields(GetType(XmlSchemaCollection), New String() {My.Resources.SMOName, _
                                                                                          My.Resources.SMOSchema})
        Catch ex As Exception
            Throw New Exception(String.Format("(cCompare.SetSmoFieldsToLoad) Exception while specifying SMO " & _
                                                         "prefetch columns for Instance [{0}].", smoSQLServer.ConnectionContext.TrueName), ex)
        End Try
    End Sub

    Public Function GetDatabaseList(ByVal SqlServer As Server) As String()
        ' used by gui for interactive drill down operations 
        ' the caller must handle errors
        Try
            ' this should get moved to the settings?
            ' actually seems to be obsolet at some point maber ,net3.0 don't know for sure
            'SqlServer.ConnectionContext.AutoDisconnectMode = AutoDisconnectMode.DisconnectIfPooled
            ' Limit the properties returned to just those that we use
            ' SetSmoFieldsToLoad should already have been called by now
            ' first find out how big the array nees to be
            Dim DatabaseList() As String = New String() {}
            Dim i As Int32 = 0
            For Each db As Database In SqlServer.Databases
                If Not (db.IsSystemObject) And db.IsAccessible Then
                    ReDim Preserve DatabaseList(i)
                    DatabaseList(i) = db.Name
                    i += 1
                End If
            Next
            Return DatabaseList
        Catch ex As Exception
            Throw New Exception(String.Format("(cCompare.GetDatabaseList) Exception while fetching database " & _
                                                         "list from Instance [{0}].", SqlServer.ConnectionContext.TrueName), ex)
        End Try
    End Function

    Public Function GetItemList(ByVal Origin As String, _
                                ByVal InstanceName As String, _
                                ByVal DbName As String, _
                                ByVal CollectionName As String, _
                                ByVal ItemLabel As String, _
                                ByVal Side1or2 As Int32, _
                                Optional ByVal RepositoryConnectionString As String = "", _
                                Optional ByVal SchemaToMatch As String = "") As String()
        ' used by gui for interactive drill down operations 
        ' and by recursive compares
        ' the caller must handle errors
        Dim ItemList() As String = {}
        Try
            Dim i As Int32 = 0
            'Need to make sure this sticks to the right connection
            'if INSTANCE_A=INSTANCE_B this must use correct one
            'passes indicator solves the problem but a little messy
            ' any list could be derived from the smo hierarchy or from the reporitory
            ' only if from the repository does itemlabel or itemversion mean anything
            If Origin = My.Resources.OriginSQLInstance Then
                Dim smoSQLServer As Server
                If Side1or2 = 1 Then
                    smoSQLServer = SqlServer1
                Else
                    smoSQLServer = SqlServer2
                End If
                ' server level (db = "") or db level
                ' SchemaToMatch with no db will return empty array
                With smoSQLServer
                    If DbName = "" And SchemaToMatch = "" Then
                        Select Case CollectionName
                            Case My.Resources.Alerts
                                For Each a As Alert In .JobServer.Alerts
                                    If Regex.Match(a.Name, _
                                                   _NameMatchPattern, _
                                                   _NameMatchOptions).Success Then
                                        ReDim Preserve ItemList(i)
                                        ItemList(i) = a.Name
                                        i += 1
                                    End If
                                Next
                            Case My.Resources.Audits
                                If .VersionMajor > 8 Then
                                    For Each a As Audit In .Audits
                                        If Regex.Match(a.Name, _
                                                       _NameMatchPattern, _
                                                       _NameMatchOptions).Success Then
                                            ReDim Preserve ItemList(i)
                                            ItemList(i) = a.Name
                                            i += 1
                                        End If
                                    Next
                                End If
                            Case My.Resources.BackupDevices
                                For Each b As BackupDevice In .BackupDevices
                                    If Regex.Match(b.Name, _
                                                   _NameMatchPattern, _
                                                   _NameMatchOptions).Success Then
                                        ReDim Preserve ItemList(i)
                                        ItemList(i) = b.Name
                                        i += 1
                                    End If
                                Next
                            Case My.Resources.Credentials
                                If .VersionMajor > 8 Then
                                    For Each c As Credential In .Credentials
                                        If Regex.Match(c.Name, _
                                                       _NameMatchPattern, _
                                                       _NameMatchOptions).Success Then
                                            ReDim Preserve ItemList(i)
                                            ItemList(i) = c.Name
                                            i += 1
                                        End If
                                    Next
                                End If
                            Case My.Resources.CryptographicProviders
                                If .VersionMajor > 9 Then
                                    For Each c As CryptographicProvider In .CryptographicProviders
                                        If Regex.Match(c.Name, _
                                                       _NameMatchPattern, _
                                                       _NameMatchOptions).Success Then
                                            ReDim Preserve ItemList(i)
                                            ItemList(i) = c.Name
                                            i += 1
                                        End If
                                    Next
                                End If
                            Case My.Resources.Databases
                                For Each d As Database In .Databases
                                    If LCase(d.Name) <> "tempdb" Then
                                        If Regex.Match(d.Name, _
                                                       _NameMatchPattern, _
                                                       _NameMatchOptions).Success Then
                                            ReDim Preserve ItemList(i)
                                            ItemList(i) = d.Name
                                            i += 1
                                        End If
                                    End If
                                Next
                            Case My.Resources.Endpoints
                                If .VersionMajor > 8 Then
                                    For Each e As Endpoint In .Endpoints
                                        If Regex.Match(e.Name, _
                                                       _NameMatchPattern, _
                                                       _NameMatchOptions).Success Then
                                            ReDim Preserve ItemList(i)
                                            ItemList(i) = e.Name
                                            i += 1
                                        End If
                                    Next
                                End If
                            Case My.Resources.Jobs
                                For Each j As Job In .JobServer.Jobs
                                    If Regex.Match(j.Name, _
                                                   _NameMatchPattern, _
                                                   _NameMatchOptions).Success Then
                                        ReDim Preserve ItemList(i)
                                        ItemList(i) = j.Name
                                        i += 1
                                    End If
                                Next
                            Case My.Resources.LinkedServers
                                For Each l As LinkedServer In .LinkedServers
                                    If Regex.Match(l.Name, _
                                                   _NameMatchPattern, _
                                                   _NameMatchOptions).Success Then
                                        ReDim Preserve ItemList(i)
                                        ItemList(i) = l.Name
                                        i += 1
                                    End If
                                Next
                            Case My.Resources.Logins
                                For Each l As Login In .Logins
                                    If Regex.Match(l.Name, _
                                                   _NameMatchPattern, _
                                                   _NameMatchOptions).Success Then
                                        ReDim Preserve ItemList(i)
                                        ItemList(i) = l.Name
                                        i += 1
                                    End If
                                Next
                                'not clear how to enum these - an SMO bug me thinks
                                'Case("OleDBProviderSettings")
                                'For Each o As OleDbProviderSettings In OleDbProviderSettings.Equals
                                'If Regex.Match(o.Name, _
                                '              _NameMatchPattern, _
                                '             _NameMatchOptions).Success Then
                                'ReDim Preserve ItemList(i)
                                'ItemList(i) = o.Name
                                'i += 1
                                'End If
                                'Next
                            Case My.Resources.Operators
                                For Each o As Agent.Operator In .JobServer.Operators
                                    If Regex.Match(o.Name, _
                                                   _NameMatchPattern, _
                                                   _NameMatchOptions).Success Then
                                        ReDim Preserve ItemList(i)
                                        ItemList(i) = o.Name
                                        i += 1
                                    End If
                                Next
                            Case My.Resources.ProxyAccounts
                                If .VersionMajor > 8 Then
                                    For Each p As Agent.ProxyAccount In .JobServer.ProxyAccounts
                                        If Regex.Match(p.Name, _
                                                       _NameMatchPattern, _
                                                       _NameMatchOptions).Success Then
                                            ReDim Preserve ItemList(i)
                                            ItemList(i) = p.Name
                                            i += 1
                                        End If
                                    Next
                                End If
                            Case My.Resources.Roles
                                For Each r As ServerRole In .Roles
                                    If Regex.Match(r.Name, _
                                                   _NameMatchPattern, _
                                                   _NameMatchOptions).Success Then
                                        ReDim Preserve ItemList(i)
                                        ItemList(i) = r.Name
                                        i += 1
                                    End If
                                Next
                            Case My.Resources.ServerAuditSpecifications
                                If .VersionMajor > 8 Then
                                    For Each a As AuditSpecification In .ServerAuditSpecifications
                                        If Regex.Match(a.Name, _
                                                       _NameMatchPattern, _
                                                       _NameMatchOptions).Success Then
                                            ReDim Preserve ItemList(i)
                                            ItemList(i) = a.Name
                                            i += 1
                                        End If
                                    Next
                                End If
                            Case My.Resources.TargetServers
                                For Each o As Agent.Operator In .JobServer.TargetServers
                                    If Regex.Match(o.Name, _
                                                   _NameMatchPattern, _
                                                   _NameMatchOptions).Success Then
                                        ReDim Preserve ItemList(i)
                                        ItemList(i) = o.Name
                                        i += 1
                                    End If
                                Next
                            Case My.Resources.Triggers
                                If .VersionMajor > 8 Then
                                    For Each t As Trigger In .Triggers
                                        If Regex.Match(t.Name, _
                                                       _NameMatchPattern, _
                                                       _NameMatchOptions).Success Then
                                            ReDim Preserve ItemList(i)
                                            ItemList(i) = t.Name
                                            i += 1
                                        End If
                                    Next
                                End If
                            Case My.Resources.UserDefinedMessages
                                If .VersionMajor > 8 Then
                                    For Each m As UserDefinedMessage In .UserDefinedMessages
                                        If Regex.Match(m.ID.ToString, _
                                                       _NameMatchPattern, _
                                                       _NameMatchOptions).Success Then
                                            ReDim Preserve ItemList(i)
                                            ItemList(i) = m.ToString
                                            i += 1
                                        End If
                                    Next
                                End If
                        End Select
                    Else
                        Dim Db As Database = smoSQLServer.Databases(DbName)
                        ' Limit the properties returned to just those that we are interested in now
                        Select Case CollectionName
                            ' each case will
                            '   prefetch the interesting attributes
                            '   find out how many qualified
                            Case My.Resources.ApplicationRoles
                                For Each a As ApplicationRole In Db.ApplicationRoles
                                    If Regex.Match(a.Name, _
                                                   _NameMatchPattern, _
                                                   _NameMatchOptions).Success Then
                                        ReDim Preserve ItemList(i)
                                        ItemList(i) = a.Name
                                        i += 1
                                    End If
                                Next
                            Case My.Resources.Assemblies
                                If .VersionMajor > 8 Then
                                    Db.PrefetchObjects(GetType(SqlAssembly))
                                    For Each a As SqlAssembly In Db.Assemblies
                                        If Regex.Match(a.Name, _
                                                       _NameMatchPattern, _
                                                       _NameMatchOptions).Success Then
                                            ReDim Preserve ItemList(i)
                                            ItemList(i) = a.Name
                                            i += 1
                                        End If
                                    Next
                                End If
                            Case My.Resources.AsymmetricKeys
                                If .VersionMajor > 8 Then
                                    For Each a As AsymmetricKey In Db.AsymmetricKeys
                                        If Regex.Match(a.Name, _
                                                       _NameMatchPattern, _
                                                       _NameMatchOptions).Success Then
                                            ReDim Preserve ItemList(i)
                                            ItemList(i) = a.Name
                                            i += 1
                                        End If
                                    Next
                                End If
                            Case My.Resources.Priorities
                                If .VersionMajor > 8 Then
                                    For Each bp As BrokerPriority In Db.ServiceBroker.Priorities
                                        If Regex.Match(bp.Name, _
                                                       _NameMatchPattern, _
                                                       _NameMatchOptions).Success Then
                                            ReDim Preserve ItemList(i)
                                            ItemList(i) = bp.Name
                                            i += 1
                                        End If
                                    Next
                                End If
                            Case My.Resources.Certificates
                                If .VersionMajor > 8 Then
                                    For Each c As Certificate In Db.Certificates
                                        If Regex.Match(c.Name, _
                                                       _NameMatchPattern, _
                                                       _NameMatchOptions).Success Then
                                            ReDim Preserve ItemList(i)
                                            ItemList(i) = c.Name
                                            i += 1
                                        End If
                                    Next
                                End If
                            Case My.Resources.DatabaseAuditSpecifications
                                If .VersionMajor > 8 Then
                                    For Each a As AuditSpecification In Db.DatabaseAuditSpecifications
                                        If Regex.Match(a.Name, _
                                                       _NameMatchPattern, _
                                                       _NameMatchOptions).Success Then
                                            ReDim Preserve ItemList(i)
                                            ItemList(i) = a.Name
                                            i += 1
                                        End If
                                    Next
                                End If
                            Case My.Resources.Defaults
                                ' first find out how mant qualified procedures there are
                                For Each d As Smo.Default In Db.Defaults
                                    If (SchemaToMatch = "" OrElse d.Schema = SchemaToMatch) _
                                    AndAlso Regex.Match(d.Schema + "." + d.Name, _
                                                        _NameMatchPattern, _
                                                        _NameMatchOptions).Success Then
                                        ReDim Preserve ItemList(i)
                                        ItemList(i) = d.Schema + "." + d.Name
                                        i += 1
                                    End If
                                Next
                            Case My.Resources.FullTextCatalogs
                                For Each f As FullTextCatalog In Db.FullTextCatalogs
                                    If Regex.Match(f.Name, _
                                                   _NameMatchPattern, _
                                                   _NameMatchOptions).Success Then
                                        ReDim Preserve ItemList(i)
                                        ItemList(i) = f.Name
                                        i += 1
                                    End If
                                Next
                            Case My.Resources.FullTextStopLists
                                If .VersionMajor > 9 Then
                                    For Each f As FullTextStopList In Db.FullTextStopLists
                                        If Regex.Match(f.Name, _
                                                       _NameMatchPattern, _
                                                       _NameMatchOptions).Success Then
                                            ReDim Preserve ItemList(i)
                                            ItemList(i) = f.Name
                                            i += 1
                                        End If
                                    Next
                                End If
                            Case My.Resources.MessageTypes
                                If .VersionMajor > 8 Then
                                    For Each mt As MessageType In Db.ServiceBroker.MessageTypes
                                        If Not (mt.IsSystemObject) _
                                        AndAlso Regex.Match(mt.Name, _
                                                            _NameMatchPattern, _
                                                            _NameMatchOptions).Success Then
                                            ReDim Preserve ItemList(i)
                                            ItemList(i) = mt.Name
                                            i += 1
                                        End If
                                    Next
                                End If
                            Case My.Resources.PartitionFunctions
                                If .VersionMajor > 8 Then
                                    For Each pf As PartitionFunction In Db.PartitionFunctions
                                        If Regex.Match(pf.Name, _
                                                       _NameMatchPattern, _
                                                       _NameMatchOptions).Success Then
                                            ReDim Preserve ItemList(i)
                                            ItemList(i) = pf.Name
                                            i += 1
                                        End If
                                    Next
                                End If
                            Case My.Resources.PartitionSchemes
                                If .VersionMajor > 8 Then
                                    For Each ps As PartitionScheme In Db.PartitionSchemes
                                        If Regex.Match(ps.Name, _
                                                            _NameMatchPattern, _
                                                            _NameMatchOptions).Success Then
                                            ReDim Preserve ItemList(i)
                                            ItemList(i) = ps.Name
                                            i += 1
                                        End If
                                    Next
                                End If
                            Case My.Resources.PlanGuides
                                If .VersionMajor > 8 Then
                                    For Each p As PlanGuide In Db.PlanGuides
                                        If Regex.Match(p.Name, _
                                                        _NameMatchPattern, _
                                                        _NameMatchOptions).Success Then
                                            ReDim Preserve ItemList(i)
                                            ItemList(i) = p.Name
                                            i += 1
                                        End If
                                    Next
                                End If
                            Case My.Resources.Queues
                                If .VersionMajor > 8 Then
                                    For Each sq As ServiceQueue In Db.ServiceBroker.Queues
                                        If (SchemaToMatch = "" OrElse sq.Schema = SchemaToMatch) _
                                        AndAlso Regex.Match(sq.Schema + "." + sq.Name, _
                                                            _NameMatchPattern, _
                                                            _NameMatchOptions).Success Then
                                            ReDim Preserve ItemList(i)
                                            ItemList(i) = sq.Schema + "." + sq.Name
                                            i += 1
                                        End If
                                    Next
                                End If
                            Case My.Resources.RemoteServiceBindings
                                If .VersionMajor > 8 Then
                                    For Each r As RemoteServiceBinding In Db.ServiceBroker.RemoteServiceBindings
                                        If Regex.Match(r.Name, _
                                                            _NameMatchPattern, _
                                                            _NameMatchOptions).Success Then
                                            ReDim Preserve ItemList(i)
                                            ItemList(i) = r.Name
                                            i += 1
                                        End If
                                    Next
                                End If
                            Case My.Resources.Roles
                                For Each r As DatabaseRole In Db.Roles
                                    If Regex.Match(r.Name, _
                                                        _NameMatchPattern, _
                                                        _NameMatchOptions).Success Then
                                        ReDim Preserve ItemList(i)
                                        ItemList(i) = r.Name
                                        i += 1
                                    End If
                                Next
                            Case My.Resources.Routes
                                If .VersionMajor > 8 Then
                                    For Each r As ServiceRoute In Db.ServiceBroker.Routes
                                        If Regex.Match(r.Name, _
                                                            _NameMatchPattern, _
                                                            _NameMatchOptions).Success Then
                                            ReDim Preserve ItemList(i)
                                            ItemList(i) = r.Name
                                            i += 1
                                        End If
                                    Next
                                End If
                            Case My.Resources.Rules
                                For Each r As Smo.Rule In Db.Rules
                                    If Regex.Match(r.Name, _
                                                        _NameMatchPattern, _
                                                        _NameMatchOptions).Success Then
                                        ReDim Preserve ItemList(i)
                                        ItemList(i) = r.Name
                                        i += 1
                                    End If
                                Next
                            Case My.Resources.Schemas
                                If .VersionMajor > 8 Then
                                    For Each s As Schema In Db.Schemas
                                        If Regex.Match(s.Name, _
                                                       _NameMatchPattern, _
                                                       _NameMatchOptions).Success Then
                                            If Not (s.Name = My.Resources.SchemaSys) And Not (s.Name = My.Resources.SchemaINFORMATION_SCHEMA) Then
                                                ReDim Preserve ItemList(i)
                                                ItemList(i) = s.Name
                                                i += 1
                                            End If
                                        End If
                                    Next
                                End If
                            Case My.Resources.ServiceContracts
                                If .VersionMajor > 8 Then
                                    For Each sc As ServiceContract In Db.ServiceBroker.ServiceContracts
                                        If Not (sc.IsSystemObject) _
                                        AndAlso Regex.Match(sc.Name, _
                                                            _NameMatchPattern, _
                                                            _NameMatchOptions).Success Then
                                            ReDim Preserve ItemList(i)
                                            ItemList(i) = sc.Name
                                            i += 1
                                        End If
                                    Next
                                End If
                            Case My.Resources.Services
                                If .VersionMajor > 8 Then
                                    For Each bs As BrokerService In Db.ServiceBroker.Services
                                        If Not (bs.IsSystemObject) _
                                        AndAlso Regex.Match(bs.Name, _
                                                            _NameMatchPattern, _
                                                            _NameMatchOptions).Success Then
                                            ReDim Preserve ItemList(i)
                                            ItemList(i) = bs.Name
                                            i += 1
                                        End If
                                    Next
                                End If
                            Case My.Resources.StoredProcedures
                                Db.PrefetchObjects(GetType(StoredProcedure))
                                For Each p As StoredProcedure In Db.StoredProcedures
                                    If (p.IsSystemObject = False) _
                                    AndAlso (SchemaToMatch = "" OrElse p.Schema = SchemaToMatch) _
                                    AndAlso Regex.Match(p.Schema + "." + p.Name, _
                                                        _NameMatchPattern, _
                                                        _NameMatchOptions).Success Then
                                        ReDim Preserve ItemList(i)
                                        ItemList(i) = p.Schema + "." + p.Name
                                        i += 1
                                    End If
                                Next
                            Case My.Resources.SymmetricKeys
                                If .VersionMajor > 8 Then
                                    For Each s As SymmetricKey In Db.SymmetricKeys
                                        If Regex.Match(s.Name, _
                                                       _NameMatchPattern, _
                                                       _NameMatchOptions).Success Then
                                            ReDim Preserve ItemList(i)
                                            ItemList(i) = s.Name
                                            i += 1
                                        End If
                                    Next
                                End If
                            Case My.Resources.Synonyms
                                If .VersionMajor > 8 Then
                                    For Each s As Synonym In Db.Synonyms
                                        If (SchemaToMatch = "" OrElse s.Schema = SchemaToMatch) _
                                        AndAlso Regex.Match(s.Schema + "." + s.Name, _
                                                            _NameMatchPattern, _
                                                            _NameMatchOptions).Success Then
                                            ReDim Preserve ItemList(i)
                                            ItemList(i) = s.Schema + "." + s.Name
                                            i += 1
                                        End If
                                    Next
                                End If
                            Case My.Resources.Tables
                                Db.PrefetchObjects(GetType(Table))
                                For Each t As Table In Db.Tables
                                    If (t.IsSystemObject = False) _
                                    AndAlso (SchemaToMatch = "" OrElse t.Schema = SchemaToMatch) _
                                    AndAlso Not (t.Schema = My.Resources.SchemaSys) _
                                    AndAlso Regex.Match(t.Schema + "." + t.Name, _
                                                        _NameMatchPattern, _
                                                        _NameMatchOptions).Success Then
                                        ReDim Preserve ItemList(i)
                                        ItemList(i) = t.Schema + "." + t.Name
                                        i += 1
                                    End If
                                Next
                            Case My.Resources.Triggers
                                If .VersionMajor > 8 Then
                                    For Each t As DatabaseDdlTrigger In Db.Triggers
                                        If (t.IsSystemObject = False) _
                                        AndAlso Regex.Match(t.Name, _
                                                            _NameMatchPattern, _
                                                            _NameMatchOptions).Success Then
                                            ReDim Preserve ItemList(i)
                                            ItemList(i) = t.Name
                                            i += 1
                                        End If
                                    Next
                                End If
                            Case My.Resources.Users
                                Db.PrefetchObjects(GetType(User))
                                For Each u As User In Db.Users
                                    If Regex.Match(u.Name, _
                                                    _NameMatchPattern, _
                                                    _NameMatchOptions).Success Then
                                        ReDim Preserve ItemList(i)
                                        ItemList(i) = u.Name
                                        i += 1
                                    End If
                                Next
                            Case My.Resources.UserDefinedAggregates
                                If .VersionMajor > 8 Then
                                    'Db.PrefetchObjects(GetType(UserDefinedAggregate))
                                    For Each a As UserDefinedAggregate In Db.UserDefinedAggregates
                                        If (SchemaToMatch = "" OrElse a.Schema = SchemaToMatch) _
                                        AndAlso Regex.Match(a.Schema + "." + a.Name, _
                                                            _NameMatchPattern, _
                                                            _NameMatchOptions).Success Then
                                            ReDim Preserve ItemList(i)
                                            ItemList(i) = a.Schema + "." + a.Name
                                            i += 1
                                        End If
                                    Next
                                End If
                            Case My.Resources.UserDefinedDataTypes
                                If .VersionMajor > 8 Then
                                    For Each d As UserDefinedDataType In Db.UserDefinedDataTypes
                                        If (SchemaToMatch = "" OrElse d.Schema = SchemaToMatch) _
                                        AndAlso Regex.Match(d.Schema + "." + d.Name, _
                                                            _NameMatchPattern, _
                                                            _NameMatchOptions).Success Then
                                            ReDim Preserve ItemList(i)
                                            ItemList(i) = d.Schema + "." + d.Name
                                            i += 1
                                        End If
                                    Next
                                End If
                            Case My.Resources.UserDefinedFunctions
                                For Each f As UserDefinedFunction In Db.UserDefinedFunctions
                                    If (f.IsSystemObject = False) _
                                    AndAlso (SchemaToMatch = "" OrElse f.Schema = SchemaToMatch) _
                                    AndAlso Regex.Match(f.Schema + "." + f.Name, _
                                                        _NameMatchPattern, _
                                                        _NameMatchOptions).Success Then
                                        ReDim Preserve ItemList(i)
                                        ItemList(i) = f.Schema + "." + f.Name
                                        i += 1
                                    End If
                                Next
                            Case My.Resources.UserDefinedTableTypes
                                If .VersionMajor > 9 Then
                                    For Each t As UserDefinedTableType In Db.UserDefinedTableTypes
                                        If (SchemaToMatch = "" OrElse t.Schema = SchemaToMatch) _
                                        AndAlso Regex.Match(t.Schema + "." + t.Name, _
                                                            _NameMatchPattern, _
                                                            _NameMatchOptions).Success Then
                                            ReDim Preserve ItemList(i)
                                            ItemList(i) = t.Schema + "." + t.Name
                                            i += 1
                                        End If
                                    Next
                                End If
                            Case My.Resources.UserDefinedTypes
                                If .VersionMajor > 8 Then
                                    For Each y As UserDefinedType In Db.UserDefinedTypes
                                        If (SchemaToMatch = "" OrElse y.Schema = SchemaToMatch) _
                                        AndAlso Regex.Match(y.Schema + "." + y.Name, _
                                                            _NameMatchPattern, _
                                                            _NameMatchOptions).Success Then
                                            ReDim Preserve ItemList(i)
                                            ItemList(i) = y.Schema + "." + y.Name
                                            i += 1
                                        End If
                                    Next
                                End If
                            Case My.Resources.Views
                                Db.PrefetchObjects(GetType(View))
                                For Each v As View In Db.Views
                                    If (v.IsSystemObject = False) _
                                    AndAlso (SchemaToMatch = "" OrElse v.Schema = SchemaToMatch) _
                                    AndAlso Not (v.Schema = My.Resources.SchemaSys Or v.Schema = My.Resources.SchemaINFORMATION_SCHEMA) _
                                    AndAlso Regex.Match(v.Schema + "." + v.Name, _
                                                        _NameMatchPattern, _
                                                        _NameMatchOptions).Success Then
                                        ReDim Preserve ItemList(i)
                                        ItemList(i) = v.Schema + "." + v.Name
                                        i += 1
                                    End If
                                Next
                            Case My.Resources.XmlSchemaCollections
                                If .VersionMajor > 8 Then
                                    Db.PrefetchObjects(GetType(XmlSchemaCollection))
                                    For Each x As XmlSchemaCollection In Db.XmlSchemaCollections
                                        If (SchemaToMatch = "" OrElse x.Schema = SchemaToMatch) _
                                        AndAlso Regex.Match(x.Schema + "." + x.Name, _
                                                            _NameMatchPattern, _
                                                            _NameMatchOptions).Success Then
                                            ReDim Preserve ItemList(i)
                                            ItemList(i) = x.Schema + "." + x.Name
                                            i += 1
                                        End If
                                    Next
                                End If
                        End Select
                    End If
                End With
            ElseIf Origin = My.Resources.OriginRepository Then
                ' if run from UI compare form, the repository can be remote 
                If ItemLabel = "" Then
                    ' query the repository for the latest version of all items in the collection
                    ' if schema compare and is one of the schema bound collections remove items 
                    ' that do not begin with the schamaname and a dot
                    ' but first, determine the subtype 

                    If SchemaToMatch = "" Then
                        ItemList = DAL.GetItemList(MakeNode(InstanceName, DbName, CollectionName, ""), RepositoryConnectionString)
                    Else
                        For Each s As String In DAL.GetItemList(MakeNode(InstanceName, DbName, CollectionName, ""), RepositoryConnectionString)
                            If InStr(s, SchemaToMatch & ".") = 1 Then
                                ReDim Preserve ItemList(i)
                                ItemList(i) = s
                                i += 1
                            End If
                        Next
                    End If
                Else
                    Throw New Exception("(cCompare.GetItemList) Item Label not implemented at this time.")
                End If
            End If
            Array.Sort(ItemList)
        Catch ex As Exception
            ' let version compatibility errors fall through
            If Not (ex.Message = "This method or property is accessible only while working against SQL Server 2008 or later.") _
            And Not (ex.Message = "Full-text is not supported on this edition of SQL Server.") Then
                If DbName = "" Then
                    Throw New Exception(String.Format("(cCompare.GetItemList) Exception while fetching item list for comparison of " & _
                                                                 "Collection [{0}] on Instance [{1}].", _
                                                                 CollectionName, InstanceName), ex)
                Else
                    Throw New Exception(String.Format("(cCompare.GetItemList) Exception while fetching item list for comparison of " & _
                                                                 "Collection [{0}] in Database [{1}] on Instance [{2}].", _
                                                                 CollectionName, DbName, InstanceName), ex)
                End If
            End If
        End Try
        Return ItemList
    End Function

    Public Sub CloseConnections()
        If Not (SqlServer1 Is Nothing) Then
            If SqlServer1.ConnectionContext.IsOpen Then
                SqlServer1.ConnectionContext.Disconnect()
            End If
            SqlServer1 = Nothing
        End If
        If Not (SqlServer2 Is Nothing) Then
            If SqlServer2.ConnectionContext.IsOpen Then
                SqlServer2.ConnectionContext.Disconnect()
            End If
            SqlServer2 = Nothing
        End If
    End Sub

    Public Function CompareSchema(ByVal Origin1 As String, _
                                  ByVal Instance1 As String, _
                                  ByVal DbName1 As String, _
                                  ByVal Schema1 As String, _
                                  ByVal Origin2 As String, _
                                  ByVal Instance2 As String, _
                                  ByVal DbName2 As String, _
                                  ByVal Schema2 As String, _
                                  Optional ByVal ConnectionString1 As String = "", _
                                  Optional ByVal ConnectionString2 As String = "") As Boolean
        Try
            CompareSchema = False
            'compare all objects of schema1 & all objects of schema2 
            If CompareTwoItems(Origin1, Instance1, DbName1, My.Resources.Schemas, Schema1, 0, "", _
                               Origin2, Instance2, DbName2, My.Resources.Schemas, Schema2, 0, "", _
                               False, Nothing, "", ConnectionString1, ConnectionString1) Then
                'compare each collection that has items o either side in this schema
                CompareAllItems(Origin1, Instance1, DbName1, My.Resources.Defaults, "", _
                                Origin2, Instance2, DbName2, My.Resources.Defaults, "", _
                                False, ConnectionString1, ConnectionString1, Schema1, Schema2)
                CompareAllItems(Origin1, Instance1, DbName1, My.Resources.Queues, "", _
                                Origin2, Instance2, DbName2, My.Resources.Queues, "", _
                                False, ConnectionString1, ConnectionString1, Schema1, Schema2)
                CompareAllItems(Origin1, Instance1, DbName1, My.Resources.StoredProcedures, "", _
                                Origin2, Instance2, DbName2, My.Resources.StoredProcedures, "", _
                                False, ConnectionString1, ConnectionString1, Schema1, Schema2)
                CompareAllItems(Origin1, Instance1, DbName1, My.Resources.Synonyms, "", _
                                Origin2, Instance2, DbName2, My.Resources.Synonyms, "", _
                                False, ConnectionString1, ConnectionString1, Schema1, Schema2)
                CompareAllItems(Origin1, Instance1, DbName1, My.Resources.Tables, "", _
                                Origin2, Instance2, DbName2, My.Resources.Tables, "", _
                                False, ConnectionString1, ConnectionString1, Schema1, Schema2)
                CompareAllItems(Origin1, Instance1, DbName1, My.Resources.Views, "", _
                                Origin2, Instance2, DbName2, My.Resources.Views, "", _
                                False, ConnectionString1, ConnectionString1, Schema1, Schema2)
                CompareAllItems(Origin1, Instance1, DbName1, My.Resources.UserDefinedAggregates, "", _
                                Origin2, Instance2, DbName2, My.Resources.UserDefinedAggregates, "", _
                                False, ConnectionString1, ConnectionString1, Schema1, Schema2)
                CompareAllItems(Origin1, Instance1, DbName1, My.Resources.UserDefinedDataTypes, "", _
                                Origin2, Instance2, DbName2, My.Resources.UserDefinedDataTypes, "", _
                                False, ConnectionString1, ConnectionString1, Schema1, Schema2)
                CompareAllItems(Origin1, Instance1, DbName1, My.Resources.UserDefinedFunctions, "", _
                                Origin2, Instance2, DbName2, My.Resources.UserDefinedFunctions, "", _
                                False, ConnectionString1, ConnectionString1, Schema1, Schema2)
                CompareAllItems(Origin1, Instance1, DbName1, My.Resources.UserDefinedTypes, "", _
                                Origin2, Instance2, DbName2, My.Resources.UserDefinedTypes, "", _
                                False, ConnectionString1, ConnectionString1, Schema1, Schema2)
                CompareAllItems(Origin1, Instance1, DbName1, My.Resources.XMLSchemaCollections, "", _
                                Origin2, Instance2, DbName2, My.Resources.XMLSchemaCollections, "", _
                                False, ConnectionString1, ConnectionString1, Schema1, Schema2)
            End If
            CompareSchema = True
        Catch ex As Exception
            Throw New Exception("(cCompare.CompareSchema) Exception.", ex)
        End Try
    End Function

    Public Function CompareDatabase(ByVal Origin1 As String, _
                                    ByVal Instance1 As String, _
                                    ByVal DbName1 As String, _
                                    ByVal Origin2 As String, _
                                    ByVal Instance2 As String, _
                                    ByVal DbName2 As String, _
                                    Optional ByVal ConnectionString1 As String = "", _
                                    Optional ByVal ConnectionString2 As String = "") As Boolean
        Try
            CompareDatabase = False
            'compared DbName1 and DbName2

            'compare each db collection that has items on either side
            'compare all objects of schema1 & all objects of schema2 
            If CompareTwoItems(Origin1, Instance1, "", My.Resources.Databases, DbName1, 0, "", _
                                   Origin2, Instance2, "", My.Resources.Databases, DbName2, 0, "", _
                                   False, Nothing, "", ConnectionString1, ConnectionString1) Then
                CompareTwoItems(Origin1, Instance1, DbName1, "", My.Resources.ActiveDirectory, 0, "", _
                                Origin2, Instance2, DbName2, "", My.Resources.ActiveDirectory, 0, "", _
                               False, Nothing, "", ConnectionString1, ConnectionString1)
                CompareAllItems(Origin1, Instance1, DbName1, My.Resources.ApplicationRoles, "", _
                                Origin2, Instance2, DbName2, My.Resources.ApplicationRoles, "", _
                                False, ConnectionString1, ConnectionString1)
                CompareAllItems(Origin1, Instance1, DbName1, My.Resources.Assemblies, "", _
                                Origin2, Instance2, DbName2, My.Resources.Assemblies, "", _
                                False, ConnectionString1, ConnectionString1)
                CompareAllItems(Origin1, Instance1, DbName1, My.Resources.AsymmetricKeys, "", _
                                Origin2, Instance2, DbName2, My.Resources.AsymmetricKeys, "", _
                                False, ConnectionString1, ConnectionString1)
                CompareAllItems(Origin1, Instance1, DbName1, My.Resources.Certificates, "", _
                                Origin2, Instance2, DbName2, My.Resources.Certificates, "", _
                                False, ConnectionString1, ConnectionString1)
                CompareTwoItems(Origin1, Instance1, DbName1, "", My.Resources.DatabaseOptions, 0, "", _
                                Origin2, Instance2, DbName2, "", My.Resources.DatabaseOptions, 0, "", _
                               False, Nothing, "", ConnectionString1, ConnectionString1)
                CompareAllItems(Origin1, Instance1, DbName1, My.Resources.DatabaseAuditSpecifications, "", _
                                Origin2, Instance2, DbName2, My.Resources.DatabaseAuditSpecifications, "", _
                                False, ConnectionString1, ConnectionString1)
                CompareAllItems(Origin1, Instance1, DbName1, My.Resources.Defaults, "", _
                                Origin2, Instance2, DbName2, My.Resources.Defaults, "", _
                                False, ConnectionString1, ConnectionString1)
                CompareAllItems(Origin1, Instance1, DbName1, My.Resources.FullTextCatalogs, "", _
                                Origin2, Instance2, DbName2, My.Resources.FullTextCatalogs, "", _
                                False, ConnectionString1, ConnectionString1)
                CompareAllItems(Origin1, Instance1, DbName1, My.Resources.FullTextStopLists, "", _
                                Origin2, Instance2, DbName2, My.Resources.FullTextStopLists, "", _
                                False, ConnectionString1, ConnectionString1)
                CompareAllItems(Origin1, Instance1, DbName1, My.Resources.MessageTypes, "", _
                                Origin2, Instance2, DbName2, My.Resources.MessageTypes, "", _
                                False, ConnectionString1, ConnectionString1)
                CompareAllItems(Origin1, Instance1, DbName1, My.Resources.PartitionFunctions, "", _
                                Origin2, Instance2, DbName2, My.Resources.PartitionFunctions, "", _
                                False, ConnectionString1, ConnectionString1)
                CompareAllItems(Origin1, Instance1, DbName1, My.Resources.PartitionSchemes, "", _
                                Origin2, Instance2, DbName2, My.Resources.PartitionSchemes, "", _
                                False, ConnectionString1, ConnectionString1)
                CompareAllItems(Origin1, Instance1, DbName1, My.Resources.PlanGuides, "", _
                                Origin2, Instance2, DbName2, My.Resources.PlanGuides, "", _
                                False, ConnectionString1, ConnectionString1)
                CompareAllItems(Origin1, Instance1, DbName1, My.Resources.Priorities, "", _
                                Origin2, Instance2, DbName2, My.Resources.Priorities, "", _
                                False, ConnectionString1, ConnectionString1)
                CompareAllItems(Origin1, Instance1, DbName1, My.Resources.Queues, "", _
                                Origin2, Instance2, DbName2, My.Resources.Queues, "", _
                                False, ConnectionString1, ConnectionString1)
                CompareAllItems(Origin1, Instance1, DbName1, My.Resources.RemoteServiceBindings, "", _
                                Origin2, Instance2, DbName2, My.Resources.RemoteServiceBindings, "", _
                                False, ConnectionString1, ConnectionString1)
                CompareAllItems(Origin1, Instance1, DbName1, My.Resources.Roles, "", _
                                Origin2, Instance2, DbName2, My.Resources.Roles, "", _
                                False, ConnectionString1, ConnectionString1)
                CompareAllItems(Origin1, Instance1, DbName1, My.Resources.Routes, "", _
                                Origin2, Instance2, DbName2, My.Resources.Routes, "", _
                                False, ConnectionString1, ConnectionString1)
                CompareAllItems(Origin1, Instance1, DbName1, My.Resources.Rules, "", _
                                Origin2, Instance2, DbName2, My.Resources.Rules, "", _
                                False, ConnectionString1, ConnectionString1)
                CompareAllItems(Origin1, Instance1, DbName1, My.Resources.Schemas, "", _
                                Origin2, Instance2, DbName2, My.Resources.Schemas, "", _
                                False, ConnectionString1, ConnectionString1)
                CompareTwoItems(Origin1, Instance1, DbName1, "", My.Resources.ServiceBroker, 0, "", _
                                Origin2, Instance2, DbName2, "", My.Resources.ServiceBroker, 0, "", _
                               False, Nothing, "", ConnectionString1, ConnectionString1)
                CompareAllItems(Origin1, Instance1, DbName1, My.Resources.ServiceContracts, "", _
                                Origin2, Instance2, DbName2, My.Resources.ServiceContracts, "", _
                                False, ConnectionString1, ConnectionString1)
                CompareAllItems(Origin1, Instance1, DbName1, My.Resources.Services, "", _
                                Origin2, Instance2, DbName2, My.Resources.Services, "", _
                                False, ConnectionString1, ConnectionString1)
                CompareAllItems(Origin1, Instance1, DbName1, My.Resources.StoredProcedures, "", _
                                Origin2, Instance2, DbName2, My.Resources.StoredProcedures, "", _
                                False, ConnectionString1, ConnectionString1)
                CompareAllItems(Origin1, Instance1, DbName1, My.Resources.SymmetricKeys, "", _
                                Origin2, Instance2, DbName2, My.Resources.SymmetricKeys, "", _
                                False, ConnectionString1, ConnectionString1)
                CompareAllItems(Origin1, Instance1, DbName1, My.Resources.Synonyms, "", _
                                Origin2, Instance2, DbName2, My.Resources.Synonyms, "", _
                                False, ConnectionString1, ConnectionString1)
                CompareAllItems(Origin1, Instance1, DbName1, My.Resources.Tables, "", _
                                Origin2, Instance2, DbName2, My.Resources.Tables, "", _
                                False, ConnectionString1, ConnectionString1)
                CompareAllItems(Origin1, Instance1, DbName1, My.Resources.Triggers, "", _
                                Origin2, Instance2, DbName2, My.Resources.Triggers, "", _
                                False, ConnectionString1, ConnectionString1)
                CompareAllItems(Origin1, Instance1, DbName1, My.Resources.UserDefinedAggregates, "", _
                                Origin2, Instance2, DbName2, My.Resources.UserDefinedAggregates, "", _
                                False, ConnectionString1, ConnectionString1)
                CompareAllItems(Origin1, Instance1, DbName1, My.Resources.UserDefinedDataTypes, "", _
                                Origin2, Instance2, DbName2, My.Resources.UserDefinedDataTypes, "", _
                                False, ConnectionString1, ConnectionString1)
                CompareAllItems(Origin1, Instance1, DbName1, My.Resources.UserDefinedFunctions, "", _
                                Origin2, Instance2, DbName2, My.Resources.UserDefinedFunctions, "", _
                                False, ConnectionString1, ConnectionString1)
                CompareAllItems(Origin1, Instance1, DbName1, My.Resources.UserDefinedTableTypes, "", _
                                Origin2, Instance2, DbName2, My.Resources.UserDefinedTableTypes, "", _
                                False, ConnectionString1, ConnectionString1)
                CompareAllItems(Origin1, Instance1, DbName1, My.Resources.UserDefinedTypes, "", _
                                Origin2, Instance2, DbName2, My.Resources.UserDefinedTypes, "", _
                                False, ConnectionString1, ConnectionString1)
                CompareAllItems(Origin1, Instance1, DbName1, My.Resources.Users, "", _
                                Origin2, Instance2, DbName2, My.Resources.Users, "", _
                                False, ConnectionString1, ConnectionString1)
                CompareAllItems(Origin1, Instance1, DbName1, My.Resources.Views, "", _
                                Origin2, Instance2, DbName2, My.Resources.Views, "", _
                                False, ConnectionString1, ConnectionString1)
                CompareAllItems(Origin1, Instance1, DbName1, My.Resources.XMLSchemaCollections, "", _
                                Origin2, Instance2, DbName2, My.Resources.XMLSchemaCollections, "", _
                                False, ConnectionString1, ConnectionString1)
            End If
            CompareDatabase = True
        Catch ex As Exception
            Throw New Exception("(cCompare.CompareDatabase) Exception.", ex)
        End Try

    End Function

    Public Function CompareServer(ByVal Origin1 As String, _
                                  ByVal Instance1 As String, _
                                  ByVal Origin2 As String, _
                                  ByVal Instance2 As String, _
                                  Optional ByVal ConnectionString1 As String = "", _
                                  Optional ByVal ConnectionString2 As String = "") As Boolean
        Try
            CompareServer = False
            ' compare server
            If CompareTwoItems(Origin1, Instance1, "", "", "", 0, "", _
                               Origin2, Instance2, "", "", "", 0, "", _
                               False, Nothing, "", ConnectionString1, ConnectionString1) Then
                CompareTwoItems(Origin1, Instance1, "", "", My.Resources.ActiveDirectory, 0, "", _
                                Origin2, Instance2, "", "", My.Resources.ActiveDirectory, 0, "", _
                                False, Nothing, "", ConnectionString1, ConnectionString1)
                CompareTwoItems(Origin1, Instance1, "", "", My.Resources.AlertSystem, 0, "", _
                                Origin2, Instance2, "", "", My.Resources.AlertSystem, 0, "", _
                                False, Nothing, "", ConnectionString1, ConnectionString1)
                CompareAllItems(Origin1, Instance1, "", My.Resources.Alerts, "", _
                                Origin2, Instance2, "", My.Resources.Alerts, "", _
                                False, ConnectionString1, ConnectionString1)
                CompareAllItems(Origin1, Instance1, "", My.Resources.Audits, "", _
                                Origin2, Instance2, "", My.Resources.Audits, "", _
                                False, ConnectionString1, ConnectionString1)
                CompareAllItems(Origin1, Instance1, "", My.Resources.BackupDevices, "", _
                                Origin2, Instance2, "", My.Resources.BackupDevices, "", _
                                False, ConnectionString1, ConnectionString1)
                CompareTwoItems(Origin1, Instance1, "", "", My.Resources.Configuration, 0, "", _
                                Origin2, Instance2, "", "", My.Resources.Configuration, 0, "", _
                                False, Nothing, "", ConnectionString1, ConnectionString1)
                CompareAllItems(Origin1, Instance1, "", My.Resources.Credentials, "", _
                                Origin2, Instance2, "", My.Resources.Credentials, "", _
                                False, ConnectionString1, ConnectionString1)
                CompareAllItems(Origin1, Instance1, "", My.Resources.CryptographicProviders, "", _
                                Origin2, Instance2, "", My.Resources.CryptographicProviders, "", _
                                False, ConnectionString1, ConnectionString1)
                CompareAllItems(Origin1, Instance1, "", My.Resources.Endpoints, "", _
                                Origin2, Instance2, "", My.Resources.Endpoints, "", _
                                False, ConnectionString1, ConnectionString1)
                CompareTwoItems(Origin1, Instance1, "", "", My.Resources.FullTextService, 0, "", _
                                Origin2, Instance2, "", "", My.Resources.FullTextService, 0, "", _
                                False, Nothing, "", ConnectionString1, ConnectionString1)
                CompareTwoItems(Origin1, Instance1, "", "", My.Resources.Information, 0, "", _
                                Origin2, Instance2, "", "", My.Resources.Information, 0, "", _
                                False, Nothing, "", ConnectionString1, ConnectionString1)
                CompareTwoItems(Origin1, Instance1, "", "", My.Resources.JobServer, 0, "", _
                                Origin2, Instance2, "", "", My.Resources.JobServer, 0, "", _
                                False, Nothing, "", ConnectionString1, ConnectionString1)
                CompareAllItems(Origin1, Instance1, "", My.Resources.Jobs, "", _
                                Origin2, Instance2, "", My.Resources.Jobs, "", _
                                False, ConnectionString1, ConnectionString1)
                CompareAllItems(Origin1, Instance1, "", My.Resources.LinkedServers, "", _
                                Origin2, Instance2, "", My.Resources.LinkedServers, "", _
                                False, ConnectionString1, ConnectionString1)
                CompareAllItems(Origin1, Instance1, "", My.Resources.Logins, "", _
                                Origin2, Instance2, "", My.Resources.Logins, "", _
                                False, ConnectionString1, ConnectionString1)
                CompareTwoItems(Origin1, Instance1, "", "", My.Resources.Mail, 0, "", _
                                Origin2, Instance2, "", "", My.Resources.Mail, 0, "", _
                                False, Nothing, "", ConnectionString1, ConnectionString1)
                CompareAllItems(Origin1, Instance1, "", My.Resources.Operators, "", _
                                Origin2, Instance2, "", My.Resources.Operators, "", _
                                False, ConnectionString1, ConnectionString1)
                CompareTwoItems(Origin1, Instance1, "", "", My.Resources.ProxyAccount, 0, "", _
                                Origin2, Instance2, "", "", My.Resources.ProxyAccount, 0, "", _
                                False, Nothing, "", ConnectionString1, ConnectionString1)
                CompareAllItems(Origin1, Instance1, "", My.Resources.ProxyAccounts, "", _
                                Origin2, Instance2, "", My.Resources.ProxyAccounts, "", _
                                False, ConnectionString1, ConnectionString1)
                CompareTwoItems(Origin1, Instance1, "", "", My.Resources.ResourceGovernor, 0, "", _
                                Origin2, Instance2, "", "", My.Resources.ResourceGovernor, 0, "", _
                                False, Nothing, "", ConnectionString1, ConnectionString1)
                CompareAllItems(Origin1, Instance1, "", My.Resources.Roles, "", _
                                Origin2, Instance2, "", My.Resources.Roles, "", _
                                False, ConnectionString1, ConnectionString1)
                CompareAllItems(Origin1, Instance1, "", My.Resources.ServerAuditSpecifications, "", _
                                Origin2, Instance2, "", My.Resources.ServerAuditSpecifications, "", _
                                False, ConnectionString1, ConnectionString1)
                CompareTwoItems(Origin1, Instance1, "", "", My.Resources.Settings, 0, "", _
                                Origin2, Instance2, "", "", My.Resources.Settings, 0, "", _
                                False, Nothing, "", ConnectionString1, ConnectionString1)
                CompareTwoItems(Origin1, Instance1, "", "", My.Resources.TargetServers, 0, "", _
                                Origin2, Instance2, "", "", My.Resources.TargetServers, 0, "", _
                                False, Nothing, "", ConnectionString1, ConnectionString1)
                CompareAllItems(Origin1, Instance1, "", My.Resources.Triggers, "", _
                                Origin2, Instance2, "", My.Resources.Triggers, "", _
                                False, ConnectionString1, ConnectionString1)
                CompareAllItems(Origin1, Instance1, "", My.Resources.UserDefinedMessages, "", _
                                Origin2, Instance2, "", My.Resources.UserDefinedMessages, "", _
                                False, ConnectionString1, ConnectionString1)

                Dim DbList1 As String() = GetItemList(Origin1, _
                                                        Instance1, _
                                                        "", _
                                                        My.Resources.Databases, _
                                                        "", _
                                                        1, _
                                                        ConnectionString1)
                Dim DbList2 As String() = GetItemList(Origin2, _
                                                        Instance2, _
                                                        "", _
                                                        My.Resources.Databases, _
                                                        "", _
                                                        2, _
                                                        ConnectionString2)
                ' can never be empty, will always be system dbs
                ' make a combined list of distinct databases. Compare every db in list
                For Each db As String In DbList2
                    If Array.IndexOf(DbList1, db) < 0 Then
                        Dim i As Integer = DbList1.Length
                        ReDim Preserve DbList1(i)
                        DbList1(i) = db
                    End If
                Next
                For Each db As String In DbList1
                    CompareDatabase(Origin1, Instance1, db, _
                                    Origin2, Instance2, db, _
                                    ConnectionString1, _
                                    ConnectionString2)
                Next
            End If
            CompareServer = True
        Catch ex As Exception
            Throw New Exception("(cCompare.CompareServer) Exception.", ex)
        End Try
    End Function

    Public Function CompareAllItems(ByVal Origin1 As String, _
                                    ByVal Instance1 As String, _
                                    ByVal DbName1 As String, _
                                    ByVal Collection1 As String, _
                                    ByVal ItemLabel1 As String, _
                                    ByVal Origin2 As String, _
                                    ByVal Instance2 As String, _
                                    ByVal DbName2 As String, _
                                    ByVal Collection2 As String, _
                                    ByVal ItemLabel2 As String, _
                                    Optional ByVal ArchiveIfDifferent As Boolean = False, _
                                    Optional ByVal RepositoryConnectionString1 As String = "", _
                                    Optional ByVal RepositoryConnectionString2 As String = "", _
                                    Optional ByVal Schema1 As String = "", _
                                    Optional ByVal Schema2 As String = "") As Boolean
        Try
            'errors must be handled by caller
            Dim ItemList1 As String() = GetItemList(Origin1, _
                                                    Instance1, _
                                                    DbName1, _
                                                    Collection1, _
                                                    ItemLabel1, _
                                                    1, _
                                                    RepositoryConnectionString1, _
                                                    Schema1)
            Dim ItemList2 As String() = GetItemList(Origin2, _
                                                    Instance2, _
                                                    DbName2, _
                                                    Collection2, _
                                                    ItemLabel2, _
                                                    2, _
                                                    RepositoryConnectionString2, _
                                                    Schema2)
            ' should speed up the archive and handles the empty list problem
            ' the case where either or both item lists are empty or contain only system objects
            If ((ItemList1.Length = 0) OrElse (ItemList1(0) = "" And ItemList1.Length = 1)) _
            AndAlso ((ItemList2.Length = 0) OrElse (ItemList2(0) = "" And ItemList2.Length = 1)) Then
                'Nothing to compare
                Exit Try
            End If
            If ArchiveIfDifferent Then
                ' empty list from instance
                If (ItemList1.Length = 0) OrElse (ItemList1(0) = "" And ItemList1.Length = 1) Then
                    For Each Item As String In ItemList2
                        RaiseEvent NameResult(My.Resources.CompareName1Blank, "", Item)
                        CompareTwoItems(Origin1, _
                                    Instance1, _
                                    DbName1, _
                                    Collection1, _
                                    "", _
                                    0, _
                                    ItemLabel1, _
                                    Origin2, _
                                    Instance2, _
                                    DbName2, _
                                    Collection2, _
                                    Item, _
                                    0, _
                                    ItemLabel1, _
                                    ArchiveIfDifferent, _
                                    Nothing, _
                                    "", _
                                    RepositoryConnectionString1, _
                                    RepositoryConnectionString2)
                    Next
                    Exit Try
                End If
                ' empty list from repository
                If (ItemList2.Length = 0) OrElse (ItemList2(0) = "" And ItemList2.Length = 1) Then
                    For Each Item As String In ItemList1
                        RaiseEvent NameResult(My.Resources.CompareName2Blank, Item, "")
                        CompareTwoItems(Origin1, _
                                    Instance1, _
                                    DbName1, _
                                    Collection1, _
                                    Item, _
                                    0, _
                                    ItemLabel1, _
                                    Origin2, _
                                    Instance2, _
                                    DbName2, _
                                    Collection2, _
                                    "", _
                                    0, _
                                    ItemLabel1, _
                                    ArchiveIfDifferent, _
                                    Nothing, _
                                    "", _
                                    RepositoryConnectionString1, _
                                    RepositoryConnectionString2)
                    Next
                    Exit Try
                End If
            End If
            ' make a single pass through the list of items  
            Dim iComp As Int32
            Dim bIgnoreCase As Boolean
            If (_NameMatchOptions And RegexOptions.IgnoreCase) = 1 Then
                bIgnoreCase = True
            Else
                bIgnoreCase = False
            End If
            Dim i As Int32 = 0
            Dim j As Int32 = 0
            Dim maxi As Int32 = ItemList1.Length - 1
            Dim maxj As Int32 = ItemList2.Length - 1
            Do While i <= maxi And j <= maxj
                If Not backgroundWorker1 Is Nothing AndAlso backgroundWorker1.CancellationPending Then
                    eventArgs.Cancel = True
                    Exit Function
                End If
                ' update a value that can be used by the backgroundworkers ReportProgress event
                If maxi > maxj Then
                    If maxi = 0 Then
                        bwPercentDone = 90
                    Else
                        bwPercentDone = CInt((i / maxi) * 100)
                    End If
                Else
                    If maxj = 0 Then
                        bwPercentDone = 90
                    Else
                        bwPercentDone = CInt((j / maxj) * 100)
                    End If
                End If
                If bwPercentDone > 100 Then
                    bwPercentDone = 100
                End If
                ' when comparing collections only items with matching names are considered
                iComp = String.Compare(ItemList1(i), ItemList2(j), bIgnoreCase)
                If iComp = 0 Then
                    If Not (CompareTwoItems(Origin1, _
                                            Instance1, _
                                            DbName1, _
                                            Collection1, _
                                            ItemList1(i), _
                                            0, _
                                            ItemLabel1, _
                                            Origin2, _
                                            Instance2, _
                                            DbName2, _
                                            Collection2, _
                                            ItemList2(j), _
                                            0, _
                                            ItemLabel2, _
                                            ArchiveIfDifferent, _
                                            Nothing, _
                                            "", _
                                            RepositoryConnectionString1, _
                                            RepositoryConnectionString2)) Then

                        If Not (_ReportDetails) Then
                            RaiseEvent NameResult(My.Resources.CompareNamesDifferent, ItemList1(i), ItemList2(j))
                        End If
                    Else
                        If Not (_ReportDetails) Then
                            RaiseEvent NameResult(My.Resources.CompareNamesMatch, ItemList1(i), ItemList2(j))
                        End If
                    End If
                    i += 1
                    j += 1
                Else
                    'look ahead to try to resolved differences as new procs inserted
                    Dim FoundMatch As Boolean = False
                    ' list are sorted so look down the list for a match for the lower valued of the two
                    If iComp < 1 Then ' list 1 string is first alphabetically
                        For k As Int32 = j + 1 To maxj
                            If String.Compare(ItemList1(i), ItemList2(k), bIgnoreCase) = 0 Then
                                FoundMatch = True
                                For l As Int32 = j To k - 1
                                    RaiseEvent NameResult(My.Resources.CompareName1Blank, "", ItemList2(k))
                                    If (_IncludeUnmatched) And (_ReportDetails) _
                                    Or ArchiveIfDifferent Then
                                        CompareTwoItems(Origin1, _
                                                Instance1, _
                                                DbName1, _
                                                Collection1, _
                                                "", _
                                                0, _
                                                ItemLabel1, _
                                                Origin2, _
                                                Instance2, _
                                                DbName2, _
                                                Collection2, _
                                                ItemList2(k), _
                                                0, _
                                                ItemLabel1, _
                                                ArchiveIfDifferent, _
                                                Nothing, _
                                                "", _
                                                RepositoryConnectionString1, _
                                                RepositoryConnectionString2)
                                    End If
                                Next
                                j = k
                            End If
                            If FoundMatch Then Exit For
                        Next
                        If Not (FoundMatch) Then
                            RaiseEvent NameResult(My.Resources.CompareName2Blank, ItemList1(i), "")
                            If _IncludeUnmatched And _ReportDetails _
                            Or ArchiveIfDifferent Then
                                CompareTwoItems(Origin1, _
                                        Instance1, _
                                        DbName1, _
                                        Collection1, _
                                        ItemList1(i), _
                                        0, _
                                        ItemLabel1, _
                                        Origin2, _
                                        Instance2, _
                                        DbName2, _
                                        Collection2, _
                                        "", _
                                        0, _
                                        ItemLabel1, _
                                        ArchiveIfDifferent, _
                                        Nothing, _
                                        "", _
                                        RepositoryConnectionString1, _
                                        RepositoryConnectionString2)
                            End If
                            i += 1
                        End If
                    Else ' iComp < 1 or list 2 string is before list 1 string alphabetically
                        For k As Int32 = i + 1 To maxi
                            If String.Compare(ItemList2(j), ItemList1(k).ToString, bIgnoreCase) = 0 Then
                                FoundMatch = True
                                For l As Int32 = i To k - 1
                                    RaiseEvent NameResult(My.Resources.CompareName2Blank, ItemList1(i), "")
                                    If _IncludeUnmatched And _ReportDetails _
                                    Or ArchiveIfDifferent Then
                                        CompareTwoItems(Origin1, _
                                                Instance1, _
                                                DbName1, _
                                                Collection1, _
                                                ItemList1(i), _
                                                0, _
                                                ItemLabel1, _
                                                Origin2, _
                                                Instance2, _
                                                DbName2, _
                                                Collection2, _
                                                "", _
                                                0, _
                                                ItemLabel1, _
                                                ArchiveIfDifferent, _
                                                Nothing, _
                                                "", _
                                                RepositoryConnectionString1, _
                                                RepositoryConnectionString2)
                                    End If
                                Next
                                i = k
                            End If
                            If FoundMatch Then
                                Exit For
                            End If
                        Next
                        If Not (FoundMatch) Then
                            RaiseEvent NameResult(My.Resources.CompareName1Blank, "", ItemList2(j))
                            If (_IncludeUnmatched) And (_ReportDetails) _
                            Or ArchiveIfDifferent Then
                                CompareTwoItems(Origin1, _
                                        Instance1, _
                                        DbName1, _
                                        Collection1, _
                                        "", _
                                        0, _
                                        ItemLabel1, _
                                        Origin2, _
                                        Instance2, _
                                        DbName2, _
                                        Collection2, _
                                        ItemList2(j), _
                                        0, _
                                        ItemLabel1, _
                                        ArchiveIfDifferent, _
                                        Nothing, _
                                        "", _
                                        RepositoryConnectionString1, _
                                        RepositoryConnectionString2)
                            End If
                            j += 1
                        End If
                    End If ' scan name list for a match
                End If ' names match
            Loop
            ' either list may have more entries at the end
            Do While i <= maxi
                RaiseEvent NameResult(My.Resources.CompareName2Blank, ItemList1(i), "")
                If (_IncludeUnmatched) And (_ReportDetails) _
                Or ArchiveIfDifferent Then
                    CompareTwoItems(Origin1, _
                            Instance1, _
                            DbName1, _
                            Collection1, _
                            ItemList1(i), _
                            0, _
                            ItemLabel1, _
                            Origin2, _
                            Instance2, _
                            DbName2, _
                            Collection2, _
                            "", _
                            0, _
                            ItemLabel1, _
                            ArchiveIfDifferent, _
                            Nothing, _
                            "", _
                            RepositoryConnectionString1, _
                            RepositoryConnectionString2)
                End If
                i += 1
            Loop
            Do While j <= maxj
                RaiseEvent NameResult(My.Resources.CompareName1Blank, "", ItemList2(j))
                If (_IncludeUnmatched) And (_ReportDetails) _
                Or ArchiveIfDifferent Then
                    CompareTwoItems(Origin1, _
                            Instance1, _
                            DbName1, _
                            Collection1, _
                            "", _
                            0, _
                            ItemLabel1, _
                            Origin2, _
                            Instance2, _
                            DbName2, _
                            Collection2, _
                            ItemList2(j), _
                            0, _
                            ItemLabel1, _
                            ArchiveIfDifferent, _
                            Nothing, _
                            "", _
                            RepositoryConnectionString1, _
                            RepositoryConnectionString2)
                End If
                j += 1
            Loop
        Catch ex As Exception
            Dim c1 As String = MakeNode(Origin1, Instance1, DbName1, Collection1)
            If ArchiveIfDifferent Then
                Throw New Exception(String.Format("(cCompare.CompareAllItems) Exception while Archiving Collection [{0}].", c1), ex)
            Else
                Dim c2 As String = MakeNode(Origin2, Instance2, DbName2, Collection2)
                Throw New Exception(String.Format("(cCompare.CompareAllItems) Exception while Comparing Collections [{0}] and [{1}].", c1, c2), ex)
            End If
        End Try
    End Function

    Private Function GetDefinition(ByVal Origin As String, _
                                   ByVal Instance As String, _
                                   ByVal DBName As String, _
                                   ByVal Collection As String, _
                                   ByVal Item As String, _
                                   ByVal SQLServer As Server, _
                                   Optional ByVal RepositoryConnectionString As String = "", _
                                   Optional ByRef ItemVersion As Integer = 0, _
                                   Optional ByRef ChangeId As Integer = 0) As StringCollection
        Try
            Dim Definition As New StringCollection
            Select Case Origin
                Case My.Resources.OriginSQLInstance
                    Dim ItemName As String
                    Dim ItemSchema As String
                    ' esp from Q, possibility that DB is gone now
                    ' this will cause check for schema to fail 

                    If (Collection = My.Resources.Queues _
                        Or Collection = My.Resources.StoredProcedures _
                        Or Collection = My.Resources.Synonyms _
                        Or Collection = My.Resources.Tables _
                        Or Collection = My.Resources.Views _
                        Or Collection = My.Resources.UserDefinedAggregates _
                        Or Collection = My.Resources.UserDefinedDataTypes _
                        Or Collection = My.Resources.UserDefinedFunctions _
                        Or Collection = My.Resources.UserDefinedTableTypes _
                        Or Collection = My.Resources.UserDefinedTypes _
                        Or Collection = My.Resources.XMLSchemaCollections) _
                    AndAlso InStr(Item, ".") > 0 Then
                        ItemName = Right(Item, Len(Item) - InStr(Item, "."))
                        ItemSchema = Left(Item, InStr(Item, ".") - 1)
                    Else
                        ItemName = Item
                        ItemSchema = ""
                    End If
                    If (DBName = "" _
                    OrElse (SQLServer.Databases.Contains(DBName) _
                            AndAlso (ItemSchema = "" _
                                     OrElse ((SQLServer.VersionMajor = 8) _
                                              OrElse SQLServer.Databases(DBName).Schemas.Contains(ItemSchema))))) Then
                        Definition = cDbDocumentor1.ScriptItem(SQLServer, _
                                                               DBName, _
                                                               Collection, _
                                                               ItemName, _
                                                               ItemSchema, _
                                                               _ScriptingOptions, _
                                                               _IncludeDrop, _
                                                               _IncludeIfExistsWithDrop, _
                                                               _BatchSeparator)
                    End If
                    ' returns empty initialized string collection if not seen on server
                Case My.Resources.OriginRepository
                    ' always give back the changeid and version if different
                    Dim d1 As String
                    If Not ItemVersion = 0 Then
                        Using TableAdapterSQLCfgDefinition As New cCommon.dsSQLConfigurationTableAdapters.pChangeSelectDefinitionByVersionTableAdapter
                            TableAdapterSQLCfgDefinition.Connection.ConnectionString = RepositoryConnectionString
                            TableAdapterSQLCfgDefinition.ClearBeforeFill = True
                            Dim dtDefByVersion As dsSQLConfiguration.pChangeSelectDefinitionByVersionDataTable
                            dtDefByVersion = TableAdapterSQLCfgDefinition.GetData(MakeNode(Instance, DBName, Collection, Item), ItemVersion)
                            ChangeId = dtDefByVersion(0).id
                            d1 = dtDefByVersion(0).Definition
                        End Using
                    Else
                        Dim obj As Object() = DAL.GetLastDefinitionWithId(MakeNode(Instance, DBName, Collection, Item), RepositoryConnectionString)
                        ChangeId = CInt(obj(0))
                        d1 = obj(1).ToString
                    End If
                    If Not (d1 Is Nothing) Then
                        Definition.AddRange(Regex.Split(d1, _LineSplitPattern, _LineSplitOptions))
                    End If
                Case My.Resources.OriginFile
                    Try
                        ' Create an instance of StreamReader to read from a file.
                        Using sr As StreamReader = New StreamReader(Item)
                            Dim line As String
                            ' Read and display the lines from the file until the end 
                            ' of the file is reached.
                            line = sr.ReadLine()
                            While Not sr.EndOfStream
                                line = sr.ReadLine()
                                Definition.AddRange(Regex.Split(line, _LineSplitPattern, _LineSplitOptions))
                            End While
                            sr.Dispose()
                            'cant set read only 'sr' to nothing
                        End Using
                    Catch ex As Exception
                        Throw New Exception(String.Format("(cCompare.GetDefinition) Exception while loading file [{0}]", Item), ex)
                    End Try
            End Select
            Return Definition
        Catch ex As Exception
            Throw New Exception("(cCompare.GetDefinition) Exception.", ex)
        End Try
    End Function

    Public Function CompareTwoItems(ByVal Origin1 As String, _
                                    ByVal Instance1 As String, _
                                    ByVal DbName1 As String, _
                                    ByVal Collection1 As String, _
                                    ByVal Item1 As String, _
                                    ByVal Item1Version As Integer, _
                                    ByVal Item1Label As String, _
                                    ByVal Origin2 As String, _
                                    ByVal Instance2 As String, _
                                    ByVal DbName2 As String, _
                                    ByVal Collection2 As String, _
                                    ByVal Item2 As String, _
                                    ByVal Item2Version As Integer, _
                                    ByVal Item2Label As String, _
                                    Optional ByVal ArchiveIfDifferent As Boolean = False, _
                                    Optional ByVal EventData As SqlTypes.SqlXml = Nothing, _
                                    Optional ByVal Action As String = "", _
                                    Optional ByVal RepositoryConnectionString1 As String = "", _
                                    Optional ByVal RepositoryConnectionString2 As String = "") As Boolean
        Try
            Dim LastChangeId As Integer = 0
            Dim NewVersion As Integer = 0
            Dim NewChangeId As Integer = 0
            If Not backgroundWorker1 Is Nothing AndAlso backgroundWorker1.CancellationPending Then
                eventArgs.Cancel = True
                Exit Function
            End If
            CompareTwoItems = True
            If DbName1 = "" And Collection1 = "" And Item1 = "" _
            And DbName2 = "" And Collection2 = "" And Item2 = "" Then
                RaiseEvent Comparing(Instance1, Instance2)
            Else
                RaiseEvent Comparing(Item1, Item2)
            End If
            ' only time blank items should get to here is when the UI is requesting scripts for unmatched items
            'not true, what if there is a setting in the reposit but not on the server or visa versa, either would cause a blank here
            ' need to suppress this even for that case 
            If Item1 <> "" And Item2 <> "" Then
                ' might be a different thread. really need the bw thread instance handle, saw an example once...
                If Not backgroundWorker1 Is Nothing AndAlso backgroundWorker1.IsBusy Then
                    If backgroundWorker1.CancellationPending() Then
                        eventArgs.Cancel = True
                        Exit Function
                    End If
                    backgroundWorker1.ReportProgress(bwPercentDone)
                End If
            End If
            Dim script1 As New StringCollection
            Dim Definition1 As New StringCollection
            Dim script2 As New StringCollection
            Dim Definition2 As New StringCollection
            ' OrElse is to allow the server attributes to be scripted
            If Item1 <> "" _
            OrElse (Instance1 <> "" And Item1 = "" And DbName1 = "" And Collection1 = "" _
                    And Instance2 <> "" And Item2 = "" And DbName2 = "" And Collection2 = "") Then
                Definition1 = GetDefinition(Origin1, _
                                            Instance1, _
                                            DbName1, _
                                            Collection1, _
                                            Item1, _
                                            SqlServer1, _
                                            RepositoryConnectionString1, _
                                            Item1Version)
                If Not backgroundWorker1 Is Nothing AndAlso backgroundWorker1.CancellationPending Then
                    eventArgs.Cancel = True
                    Exit Function
                End If
                ' slurp it into the compare script (default '\r\n|\n' should produce single line strings)
                If Definition1.Count > 0 Then
                    For Each str As String In Definition1
                        script1.AddRange(Regex.Split(str, _LineSplitPattern, _LineSplitOptions))
                    Next
                Else
                    script1.Add("")
                End If
            End If
            If Item2 <> "" _
            OrElse (Instance1 <> "" And Item1 = "" And DbName1 = "" And Collection1 = "" _
                    And Instance2 <> "" And Item2 = "" And DbName2 = "" And Collection2 = "") Then
                Definition2 = GetDefinition(Origin2, _
                                            Instance2, _
                                            DbName2, _
                                            Collection2, _
                                            Item2, _
                                            SqlServer2, _
                                            RepositoryConnectionString2, _
                                            Item2Version, _
                                            LastChangeId)
                If Not backgroundWorker1 Is Nothing AndAlso backgroundWorker1.CancellationPending Then
                    eventArgs.Cancel = True
                    Exit Function
                End If
                ' slurp it into the compare script (default '\r\n|\n' should produce single line strings)
                If Definition2.Count > 0 Then
                    For Each str As String In Definition2
                        script2.AddRange(Regex.Split(str, _LineSplitPattern, _LineSplitOptions))
                    Next
                Else
                    script2.Add("")
                End If
            End If
            If ArchiveIfDifferent Then
                ' need for line by line compare or events but only do it if the other (fast)
                ' tests do not indicate an action
                ' above will prevent script with out at least one blank row!
                ' if both empty or equal no action required
                ' the Archive class also deals with the append if EventData is Present
                Dim Script1IsEmpty As Boolean = If((script1.Count = 0 OrElse (script1.Count = 1 AndAlso script1(0) = "")), True, False)
                Dim Script2IsEmpty As Boolean = If((script2.Count = 0 OrElse (script2.Count = 1 AndAlso script2(0) = "")), True, False)

                If Script1IsEmpty AndAlso Not Script2IsEmpty Then
                    ' found only in repository 
                    DAL.AddChangeDefinition(MakeNode(SqlServer1.ConnectionContext.TrueName, _
                                                     DbName2, _
                                                     Collection2, _
                                                     Item2), _
                                            My.Resources.SQLCfgArchiveTypeDelete, _
                                            script1, _
                                            EventData, _
                                            NewChangeId, _
                                            NewVersion)
                    If NewVersion > 0 Then
                        RaiseEvent ArchiveAction(My.Resources.SQLCfgArchiveTypeDelete)
                    End If
                ElseIf Not Script1IsEmpty _
                AndAlso Script2IsEmpty Then
                    DAL.AddChangeDefinition(MakeNode(SqlServer1.ConnectionContext.TrueName, _
                                                     DbName1, _
                                                     Collection1, _
                                                     Item1), _
                                            My.Resources.SQLCfgArchiveTypeAdd, _
                                            script1, _
                                            EventData, _
                                            NewChangeId, _
                                            NewVersion)
                    If NewVersion > 0 Then
                        RaiseEvent ArchiveAction(My.Resources.SQLCfgArchiveTypeAdd)
                    End If
                Else
                    If Not Script1IsEmpty _
                    And Not Script2IsEmpty Then
                        ' now do the textual compare
                        ' unfortunately this is most likely path for most objects
                        ' plus will have to go end to end in the collection for matched items
                        Dim same As Boolean = True
                        If script1.Count = script2.Count Then
                            For i As Integer = 0 To script1.Count - 1
                                If Not script1(i) = script2(i) Then
                                    same = False
                                    Exit For
                                End If
                            Next
                        Else
                            same = False
                        End If
                        'what about 
                        If Not same Then
                            DAL.AddChangeDefinition(MakeNode(SqlServer1.ConnectionContext.TrueName, _
                                                              DbName1, _
                                                              Collection1, _
                                                              Item1), _
                                                    My.Resources.SQLCfgArchiveTypeChange, _
                                                    script1, _
                                                    EventData, _
                                                    NewChangeId, _
                                                    NewVersion)
                            If NewVersion > 0 Then
                                RaiseEvent ArchiveAction(My.Resources.SQLCfgArchiveTypeChange)
                            End If
                        ElseIf Not EventData Is Nothing Then
                            ' record DDL event when no diff between db and last archive record
                            DAL.AppendDDLEvent(LastChangeId, _
                                               EventData, _
                                               Action)
                        End If
                    End If
                End If
            Else
                Dim i As Int32 = 0
                Dim j As Int32 = 0
                Dim maxi As Int32 = script1.Count - 1
                Dim maxj As Int32 = script2.Count - 1
                Dim LineReplaceRegex As Regex = New Regex(_LineReplacePattern, _LineReplaceOptions)
                Dim line1 As String
                Dim line2 As String
                Dim workline As String
                Do While i <= maxi And j <= maxj
                    If Not backgroundWorker1 Is Nothing AndAlso backgroundWorker1.CancellationPending Then
                        eventArgs.Cancel = True
                        Exit Function
                    End If
                    line1 = LineReplaceRegex.Replace(script1.Item(i).ToString, _LineReplacement, _LineReplaceOptions)
                    line2 = LineReplaceRegex.Replace(script2.Item(j).ToString, _LineReplacement, _LineReplaceOptions)
                    If String.Compare(line1, line2) = 0 Then
                        If _ReportDetails Then
                            RaiseEvent LineResult(My.Resources.CompareLineMatch, Item1, i, line1, Item2, j, line2)
                        End If
                        i += 1
                        j += 1
                    ElseIf (line1 = vbCrLf OrElse Trim(line1) = "") AndAlso Not (line2 = vbCrLf OrElse Trim(line2) = "") Then
                        If Not (_IgnoreBlankLines) Then
                            CompareTwoItems = False
                            If (_ReportDetails) Then
                                RaiseEvent LineResult(My.Resources.CompareLine2Blank, Item1, i, line1, "", 0, "")
                            Else
                                Return CompareTwoItems
                            End If
                        Else
                            If (_ReportDetails) Then
                                ' output the blank line -1 flags a fake match
                                RaiseEvent LineResult(My.Resources.CompareLineMatch, Item1, i, line1, "", -1, "")
                            End If
                        End If
                        i += 1
                    ElseIf Not (line1 = vbCrLf OrElse Trim(line1) = "") AndAlso (line2 = vbCrLf OrElse Trim(line2) = "") Then
                        If Not (_IgnoreBlankLines) Then
                            CompareTwoItems = False
                            If (_ReportDetails) Then
                                RaiseEvent LineResult(My.Resources.CompareLine1Blank, "", 0, "", Item2, j, line2)
                            Else
                                Return CompareTwoItems
                            End If
                        Else
                            If (_ReportDetails) Then
                                ' output the blank line -1 flags a fake match
                                RaiseEvent LineResult(My.Resources.CompareLineMatch, "", -1, "", Item2, j, line2)
                            End If
                        End If
                            j += 1
                    Else
                            CompareTwoItems = False
                            If Not (_ReportDetails) Then
                                Return CompareTwoItems
                            End If
                            ' look ahead to try to resolved differences as inserted lines of code
                            Dim FoundMatch As Boolean = False
                            For k As Int32 = j + 1 To maxj
                                workline = LineReplaceRegex.Replace(script2.Item(k).ToString, _LineReplacement, _LineReplaceOptions)
                                If String.Compare(line1, workline) = 0 Then
                                    FoundMatch = True
                                    For l As Int32 = j To k - 1
                                        If Not (script2.Item(l).ToString = "" And _IgnoreBlankLines) Then
                                            RaiseEvent LineResult(My.Resources.CompareLine1Blank, "", 0, "", Item2, l, script2.Item(l).ToString)
                                        Else
                                            ' output the blank line -1 flags a fake match
                                            RaiseEvent LineResult(My.Resources.CompareLineMatch, "", -1, "", Item2, l, script2.Item(l).ToString)
                                        End If
                                    Next
                                    j = k
                                    Exit For
                                End If
                            Next
                            If Not (FoundMatch) Then
                                For k As Int32 = i + 1 To maxi
                                    workline = LineReplaceRegex.Replace(script1.Item(k).ToString, _LineReplacement, _LineReplaceOptions)
                                    If String.Compare(workline, line2) = 0 Then
                                        FoundMatch = True
                                        For l As Int32 = i To k - 1
                                            If Not (script1.Item(l).ToString = "" And _IgnoreBlankLines) Then
                                                RaiseEvent LineResult(My.Resources.CompareLine2Blank, Item1, l, script1.Item(l).ToString, "", 0, "")
                                            Else
                                                ' output the blank line -1 flags a fake match
                                                RaiseEvent LineResult(My.Resources.CompareLineMatch, Item1, l, script1.Item(l).ToString, "", -1, "")
                                            End If
                                        Next
                                        i = k
                                        Exit For
                                    End If
                                Next
                                If Not (FoundMatch) Then
                                    ' if the first line didn't match don't mate the lines 
                                    If j = 0 Then
                                        If (_ReportDetails) Then
                                            If Not (_IgnoreBlankLines) Then
                                                RaiseEvent LineResult(My.Resources.CompareLine1Blank, "", 0, "", Item2, j, script2.Item(j).ToString)
                                            Else
                                                RaiseEvent LineResult(My.Resources.CompareLine2Blank, Item1, i, script1.Item(i).ToString, "", 0, "")
                                            End If
                                        End If
                                        i += 1
                                    Else
                                        If (_ReportDetails) Then
                                            RaiseEvent LineResult(My.Resources.CompareLineDifferent, Item1, i, script1.Item(i).ToString, Item2, j, script2.Item(j).ToString)
                                        End If
                                        i += 1
                                        j += 1
                                    End If
                                End If
                            End If
                    End If
                Loop
                ' take care of the special case where one script has more lines at the bottom
                For k As Int32 = i To maxi
                    If Not (script1.Item(k).ToString = "" And _IgnoreBlankLines) Then
                        CompareTwoItems = False

                        If Not (_ReportDetails) Then
                            Return CompareTwoItems
                        Else
                            RaiseEvent LineResult(My.Resources.CompareLine2Blank, Item1, k, script1.Item(k).ToString, "", 0, "")
                        End If
                    End If
                Next
                For l As Int32 = j To maxj
                    If Not (script2.Item(l).ToString = "" And _IgnoreBlankLines) Then
                        CompareTwoItems = False

                        If Not (_ReportDetails) Then
                            Return CompareTwoItems
                        Else
                            RaiseEvent LineResult(My.Resources.CompareLine1Blank, "", 0, "", Item2, l, script2.Item(l).ToString)
                        End If
                    End If
                Next
            End If ' line by line compare (no archive)
        Catch ex As Exception
            Dim msg As String
            Dim i1 As String = MakeNode(Instance1, DbName1, Collection1, Item1)
            If ArchiveIfDifferent Then
                msg = String.Format("(cCompare.CompareTwoItems) Exception while archiving [{0}].", i1)
            Else
                Dim i2 As String = MakeNode(Instance2, DbName2, Collection2, Item2)
                msg = String.Format("(cCompare.CompareTwoItems) Exception while Comparing {0} node [{1}] and {2} node [{3}].", Origin1, i1, Origin2, i2)
            End If
            Throw New Exception(msg, ex)
        End Try
    End Function

    Function MakeNode(ByVal Instance As String, _
                      ByVal DbName As String, _
                      ByVal Collection As String, _
                      ByVal Item As String) As String
        Try
            Dim Node As String = ""
            Dim SubType As String = ""
            If DbName = "" Then
                If Collection = My.Resources.Alerts _
                Or Collection = My.Resources.Jobs _
                Or Collection = My.Resources.Operators _
                Or Collection = My.Resources.ProxyAccounts _
                Or Collection = My.Resources.TargetServers _
                Or Item = My.Resources.AlertSystem Then
                    SubType = My.Resources.JobServer
                Else
                    SubType = My.Resources.Server
                End If
            Else
                If Collection = My.Resources.MessageTypes _
                Or Collection = My.Resources.Priorities _
                Or Collection = My.Resources.Queues _
                Or Collection = My.Resources.RemoteServiceBindings _
                Or Collection = My.Resources.Routes _
                Or Collection = My.Resources.ServiceContracts _
                Or Collection = My.Resources.Services Then
                    SubType = My.Resources.ServiceBroker
                Else
                    SubType = My.Resources.Database
                End If
            End If
            ' each appended node level includes the pipe symbol before it in the string
            ' Server is the only self-parented subtype
            If SubType = My.Resources.Server Then
                Node = Instance & _
                       If(Collection = "", "", My.Resources.NodePathDelimiter & Collection) & _
                       If(Instance = Item, "", If(Item = "", "", My.Resources.NodePathDelimiter & Item))
            ElseIf SubType = My.Resources.JobServer Then
                Node = Instance & _
                        My.Resources.NodePathDelimiter & My.Resources.JobServer & _
                        If(Collection = "", "", My.Resources.NodePathDelimiter & Collection) & _
                        If(Item = "", "", My.Resources.NodePathDelimiter & Item)
            ElseIf SubType = My.Resources.Database Then
                Node = Instance & _
                       My.Resources.NodePathDelimiter & My.Resources.Databases & _
                       If(DbName = "", "", My.Resources.NodePathDelimiter & DbName) & _
                       If(Collection = "", "", My.Resources.NodePathDelimiter & Collection) & _
                       If(Item = "", "", My.Resources.NodePathDelimiter & Item)
            ElseIf SubType = My.Resources.ServiceBroker Then
                Node = Instance & _
                       My.Resources.NodePathDelimiter & My.Resources.Databases & _
                       If(DbName = "", "", My.Resources.NodePathDelimiter & DbName) & _
                       My.Resources.NodePathDelimiter & My.Resources.ServiceBroker & _
                       If(Collection = "", "", My.Resources.NodePathDelimiter & Collection) & _
                       If(Item = "", "", My.Resources.NodePathDelimiter & Item)
                End If
                Return Node
        Catch ex As Exception
            Throw New Exception("(cCompare.MakeNode) Exception.", ex)
        End Try
    End Function

    Function IsCollection(ByVal Node As String) As Boolean
        IsCollection = False
        '{SQLInstance,DbName,Collection,Item}
        Dim cracker As String() = CrackFullPath(Node)
        ' server is  not a collection
        If Not (cracker(3) = cracker(0)) _
        OrElse (cracker(1) = "" And cracker(2) = "" And cracker(3) = "") Then
            If Not (cracker(2) = "") _
            And cracker(3) = "" Then
                IsCollection = True
            End If
        End If
    End Function

    Public Function CrackFullPath(ByVal NodeFullPath As String) As String()
        Dim Strings As String() = NodeFullPath.Split(CChar("|"))
        '{SQLInstance,DbName,Collection,Item}
        Dim cracker() As String = {"", "", "", ""}
        '0 - Instance
        '1 - "" or SrvItem or SrvCollection or "Databases" or "JobSever"
        '2 - "" or SrvCollectionItem or JSItem or JSCollection or or DbName  
        '3 - "" or DBItem or DbCollection or JSCollectionItem or "ServiceBroker" 
        '4 - "" or SBITem or SBCollectionItem
        '5 - "" or SBCollectionItem
        'same as the switch in the node type
        'If Strings.Length = 1 Then
        'smoParms(2) = "ServerAttributes"
        'Else
        cracker(0) = Strings(0)
        If Strings.Length = 1 Then
            cracker(3) = Strings(0)
        Else
            If Strings(1) = My.Resources.ActiveDirectory _
            Or Strings(1) = My.Resources.Configuration _
            Or Strings(1) = My.Resources.FullTextService _
            Or Strings(1) = My.Resources.Information _
            Or Strings(1) = My.Resources.Mail _
            Or Strings(1) = My.Resources.ProxyAccount _
            Or Strings(1) = My.Resources.ResourceGovernor _
            Or Strings(1) = My.Resources.Settings _
            Or Strings(1) = My.Resources.TargetServers Then
                ' only one possible level
                cracker(3) = Strings(1)
            ElseIf Strings(1) = My.Resources.BackupDevices _
            Or Strings(1) = My.Resources.Audits _
            Or Strings(1) = My.Resources.Credentials _
            Or Strings(1) = My.Resources.CryptographicProviders _
            Or Strings(1) = My.Resources.Endpoints _
            Or Strings(1) = My.Resources.LinkedServers _
            Or Strings(1) = My.Resources.Logins _
            Or Strings(1) = My.Resources.Roles _
            Or Strings(1) = My.Resources.ServerAuditSpecifications _
            Or Strings(1) = My.Resources.Triggers _
            Or Strings(1) = My.Resources.UserDefinedMessages Then
                ' two possible levels
                cracker(2) = Strings(1)
                If Strings.Length = 3 Then
                    cracker(3) = Strings(2)
                End If
            ElseIf Strings(1) = My.Resources.JobServer Then
                'BILL_VU\EEEVAL|JobServer|ProxyAccounts|MergeReplProxy
                If Strings.Length = 2 Then
                    cracker(3) = Strings(1)
                Else
                    If Strings(2) = My.Resources.AlertSystem Then
                        cracker(3) = Strings(2)
                    Else
                        cracker(2) = Strings(2)
                    End If
                    If Strings.Length = 4 Then
                        cracker(3) = Strings(3)
                    End If
                End If
            ElseIf Strings(1) = My.Resources.Databases Then
                ' Databases collection or database compare
                If Strings.Length = 2 Then
                    cracker(2) = Strings(1)
                ElseIf Strings.Length = 3 Then
                    ' a specific db
                    cracker(2) = Strings(1)
                    cracker(3) = Strings(2)
                Else
                    ' work inside a db
                    cracker(1) = Strings(2)
                    ' an item just under the db
                    If Strings(3) = My.Resources.ActiveDirectory _
                    Or Strings(3) = My.Resources.DatabaseOptions Then
                        cracker(3) = Strings(3)
                    ElseIf Strings(3) = My.Resources.ServiceBroker Then
                        ' SB has no items
                        If Strings.Length = 4 Then
                            cracker(3) = Strings(3)
                        Else
                            cracker(2) = Strings(4)
                            If Strings.Length = 6 Then
                                cracker(3) = Strings(5)
                            End If
                        End If
                    Else
                        cracker(2) = Strings(3)
                        If Strings.Length = 5 Then
                            cracker(3) = Strings(4)
                        End If
                    End If
                End If
            End If
        End If
        Return cracker
    End Function

End Class
