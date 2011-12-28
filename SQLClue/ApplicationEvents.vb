Namespace My

    ' The following events are availble for MyApplication:
    ' Startup: Raised when the application starts, before the startup form is created.
    ' Shutdown: Raised after all application forms are closed. This event is not raised if the application terminates abnormally.
    ' UnhandledException: Raised if the application encounters an unhandled exception.
    ' StartupNextInstance: Raised when launching a single-instance application and the application is already active.
    ' NetworkAvailabilityChanged: Raised when the network connection is connected or disconnected.

    Partial Friend Class MyApplication

        Private Sub MyApplication_UnhandledException(ByVal sender As Object, _
                                                     ByVal e As Microsoft.VisualBasic.ApplicationServices.UnhandledExceptionEventArgs)
            ' here is a possibility that write premission will be denied... 
            Using sw As StreamWriter = New StreamWriter("SQLClue.log", True)
                sw.WriteLine(String.Format("{0} [{1}] {2}", Now, My.User.Name, e.Exception.GetBaseException.Message))
                sw.WriteLine("")
                sw.Write(e.Exception.GetBaseException.StackTrace.ToString)
                sw.WriteLine("")
                sw.WriteLine("")
            End Using
        End Sub

        Private Sub MyApplication_NetworkAvailabilityChanged(ByVal sender As Object, _
                                                             ByVal e As Microsoft.VisualBasic.Devices.NetworkAvailableEventArgs) _
        Handles Me.NetworkAvailabilityChanged

        End Sub

        Private Sub MyApplication_Shutdown(ByVal sender As Object, _
                                           ByVal e As System.EventArgs) _
        Handles Me.Shutdown

        End Sub

        Private Sub MyApplication_StartupNextInstance(ByVal sender As Object, _
                                                      ByVal e As Microsoft.VisualBasic.ApplicationServices.StartupNextInstanceEventArgs) _
        Handles Me.StartupNextInstance

        End Sub

        Private Sub MyApplication_Startup(ByVal sender As Object, _
                                          ByVal e As Microsoft.VisualBasic.ApplicationServices.StartupEventArgs) _
        Handles Me.Startup

            For Each s As String In My.Application.CommandLineArgs
                If s.ToLower = "\cmd" _
                Or s.ToLower = "/cmd" Then
                    s = "-cmd"
                End If
                If s.ToLower = "-cmd" Then
                    ' Stop the start form from loading.                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                  
                    e.Cancel = True
                End If
            Next

            If e.Cancel Then
                Me.HideSplashScreen()
                ' Call the main routine for windowless operation.
                Dim c As New Cmdln
                c.Main()
            End If
        End Sub

        Class Cmdln

            Private DAL As New cCommon.cDataAccess
            Private cmp As New cCommon.cCompare
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

            Sub Main()

                Try
                    'CommandLineArgs not valid for Clickonce deployed application 
                    DAL.RepositoryInstanceName = My.Settings.RepositoryInstanceName
                    DAL.RepositoryUseTrustedConnection = My.Settings.RepositoryUseTrustedConnection
                    DAL.RepositoryDatabaseName = My.Settings.RepositoryDatabaseName
                    DAL.RepositoryConnectionTimeout = My.Settings.RepositoryConnectionTimeout
                    DAL.RepositorySQLLoginName = My.Settings.RepositorySQLLoginName
                    DAL.RepositorySQLLoginPassword = My.Settings.RepositorySQLLoginPassword
                    DAL.RepositoryNetworkLibrary = My.Settings.RepositoryNetworkLibrary
                    DAL.RepositoryEncryptConnection = My.Settings.RepositoryEncryptConnection
                    DAL.RepositoryTrustServerCertificate = My.Settings.RepositoryTrustServerCertificate
                    If Not My.Application.CommandLineArgs.Count > 1 Then
                        Throw New Exception("No data store specified")
                    End If
                    Select Case My.Application.CommandLineArgs(1).ToLower
                        Case "compare"
                            Compare()
                        Case "runbook"
                            Runbook()
                        Case "archive"
                            Archive()
                        Case Else
                            Throw New Exception("Invalid data store specified")
                    End Select
                Catch ex As Exception
                    HandleException(New Exception("usage: SQLClue.exe -cmd {component} {parameters}" & vbCrLf & _
                    "  parameters by component (use one of these 4 command line options at a time):" & vbCrLf & _
                    "    -cmd Archive   ScheduleId (Integer)" & vbCrLf & _
                    "    -cmd Compare   OriginA {nodeA} ConnectStringA OriginB {nodeB} ConnectStringB True/False" & vbCrLf & _
                    "    -cmd Runbook   (no parameters)", ex))
                End Try

            End Sub

            Private Sub Compare()
                Try
                    Dim nodeA() As String = cmp.CrackFullPath(My.Application.CommandLineArgs(3))
                    Dim nodeB() As String = cmp.CrackFullPath(My.Application.CommandLineArgs(6))
                    RemoveHandler cmp.Excptn, AddressOf ExceptionEventHandler
                    RemoveHandler cmp.Comparing, AddressOf ComparingEventHandler
                    RemoveHandler cmp.NameResult, AddressOf WriteResultEventHandler
                    RemoveHandler cmp.LineResult, AddressOf WriteLineResultEventHandler
                    AddHandler cmp.Excptn, AddressOf ExceptionEventHandler
                    AddHandler cmp.Comparing, AddressOf ComparingEventHandler
                    AddHandler cmp.NameResult, AddressOf WriteResultEventHandler
                    AddHandler cmp.LineResult, AddressOf WriteLineResultEventHandler
                    If My.Application.CommandLineArgs(2) = "SQLInstance" Then
                        cmp.SqlServer1.ConnectionContext.ConnectionString = My.Application.CommandLineArgs(4)
                    ElseIf My.Application.CommandLineArgs(2) = "Repository" Then
                        cmp.Repository1.ConnectionContext.ConnectionString = My.Application.CommandLineArgs(4)
                    End If
                    If My.Application.CommandLineArgs(5) = "SQLInstance" Then
                        cmp.SqlServer2.ConnectionContext.ConnectionString = My.Application.CommandLineArgs(7)
                    ElseIf My.Application.CommandLineArgs(2) = "Repository" Then
                        cmp.Repository2.ConnectionContext.ConnectionString = My.Application.CommandLineArgs(7)
                    End If
                    SyncWorkingValues()
                    ' last parm is flag to get in to these drilldown compares
                    ' and then only if a schema, a database or a SQL Instance (root) 
                    If CBool(My.Application.CommandLineArgs(8)) Then
                        'Instance|Databases|DbName|Schemas|SchemaName 
                        If (nodeA(2) = "Schemas" And nodeB(2) = "Schemas") _
                        And Not (nodeA(3) = "" Or nodeB(3) = "") Then
                            cmp.CompareSchema(My.Application.CommandLineArgs(2), _
                                              nodeA(0), _
                                              nodeA(1), _
                                              nodeA(3), _
                                              My.Application.CommandLineArgs(5), _
                                              nodeB(0), _
                                              nodeB(1), _
                                              nodeB(3), _
                                              My.Application.CommandLineArgs(4), _
                                              My.Application.CommandLineArgs(7))
                            ' save the compare
                            ComparingEventHandler(My.Resources.CompareComplete, My.Resources.CompareComplete)
                        End If
                        ' not sure if
                        ' SQLInstance|Databases|DBName
                        ' or
                        ' SQLInstance|""|Databases|DBName should be this one
                        If (nodeA(2) = "Databases" And nodeB(2) = "Databases") _
                        And Not (nodeA(3) = "" Or nodeB(3) = "") Then
                            cmp.CompareDatabase(My.Application.CommandLineArgs(2), _
                                                nodeA(0), _
                                                nodeA(3), _
                                                My.Application.CommandLineArgs(5), _
                                                nodeB(0), _
                                                nodeB(3), _
                                                My.Application.CommandLineArgs(4), _
                                                My.Application.CommandLineArgs(7))
                            ' save the compare
                            ComparingEventHandler(My.Resources.CompareComplete, My.Resources.CompareComplete)
                        End If
                        If nodeA.Length = 1 _
                        AndAlso nodeB.Length = 1 Then
                            cmp.CompareServer(My.Application.CommandLineArgs(2), _
                                              nodeA(0), _
                                              My.Application.CommandLineArgs(5), _
                                              nodeB(0), _
                                              My.Application.CommandLineArgs(4), _
                                              My.Application.CommandLineArgs(7))
                            ' save the compare
                            ComparingEventHandler(My.Resources.CompareComplete, My.Resources.CompareComplete)
                        End If
                    Else
                        If nodeA(3) = "" And nodeB(3) = "" Then
                            If cmp.CompareAllItems(My.Application.CommandLineArgs(2), _
                                                   nodeA(0), _
                                                   nodeA(1), _
                                                   nodeA(2), _
                                                   "", _
                                                   My.Application.CommandLineArgs(5), _
                                                   nodeB(0), _
                                                   nodeB(1), _
                                                   nodeB(2), _
                                                   "", _
                                                   False, _
                                                   My.Application.CommandLineArgs(4), _
                                                   My.Application.CommandLineArgs(7)) Then
                                ' save the compare
                                ComparingEventHandler(My.Resources.CompareComplete, My.Resources.CompareComplete)
                            End If
                        Else
                            ' bool of comparetwoitems indicates if same or not, not if compare ran ok
                            cmp.CompareTwoItems(My.Application.CommandLineArgs(2), _
                                                nodeA(0), _
                                                nodeA(1), _
                                                nodeA(2), _
                                                nodeA(3), _
                                                0, _
                                                "", _
                                                My.Application.CommandLineArgs(5), _
                                                nodeB(0), _
                                                nodeB(1), _
                                                nodeB(2), _
                                                nodeB(3), _
                                                0, _
                                                "", _
                                                False, _
                                                Nothing, _
                                                "", _
                                                My.Application.CommandLineArgs(4), _
                                                My.Application.CommandLineArgs(7))
                            ' save the compare
                            ComparingEventHandler(My.Resources.CompareComplete, My.Resources.CompareComplete)
                        End If
                    End If
                Catch ex As Exception
                    HandleException(ex)
                End Try
            End Sub

            Friend Sub SyncWorkingValues()
                With My.Settings
                    cmp._BatchSeparator = .Scripting__Options_Batch__Separator
                    cmp._IgnoreBlankLines = .Misc_Ignore__Blank__Lines
                    cmp._ReportDetails = .Misc_Display__Output_Show__Comparison__Details
                    cmp._IncludeDrop = .Scripting__Options_Include__DROP__In__Script
                    cmp._IncludeIfExistsWithDrop = .Scripting__Options_Include__IF__EXISTS__With__Drop
                    cmp._IncludeUnmatched = .Misc_Display__Output_Show__Scripts__For__Unmatched__Items
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
                    If NameMatchOptions.CompareTo(cmp._NameMatchOptions) <> 0 _
                    Or Not (.Regular__Expressions_NameMatch__Pattern_4 = cmp._NameMatchPattern) _
                    Or cmp._NameMatchPattern Is Nothing Then
                        cmp._NameMatchPattern = .Regular__Expressions_NameMatch__Pattern_4
                        cmp._NameMatchOptions = NameMatchOptions
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
                    If LineSplitOptions.CompareTo(cmp._LineSplitOptions) <> 0 _
                    Or Not (cmp._LineSplitPattern = .Regular__Expressions_LineSplit__Pattern_3) _
                    Or cmp._LineSplitPattern Is Nothing Then
                        cmp._LineSplitOptions = LineSplitOptions
                        cmp._LineSplitPattern = .Regular__Expressions_LineSplit__Pattern_3
                        HTMLSide1 = "<table width=""100%"">"
                        HTMLSide2 = "<table width=""100%"">"
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
                    If LineReplaceOptions.CompareTo(cmp._LineReplaceOptions) <> 0 _
                    Or Not (cmp._LineReplacePattern = .Regular__Expressions_LineReplace__Pattern_1) _
                    Or Not (cmp._LineReplacement = .Regular__Expressions_LineReplace__Replacement_2) _
                    Or cmp._LineReplacePattern Is Nothing _
                    Or cmp._LineReplacement Is Nothing Then
                        cmp._LineReplaceOptions = LineReplaceOptions
                        cmp._LineReplacePattern = .Regular__Expressions_LineReplace__Pattern_1
                        cmp._LineReplacement = .Regular__Expressions_LineReplace__Replacement_2
                        HTMLSide1 = "<table width=""100%"">"
                        HTMLSide2 = "<table width=""100%"">"
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
                    If Not (TryScriptingOptions.ToString = cmp._ScriptingOptions.ToString) Then
                        cmp._ScriptingOptions = TryScriptingOptions
                    End If
                End With
            End Sub

            Private Sub Runbook()
                Try
                    'If My.Settings.RunbookEnabled And DAL.AvailableLicenses > -1 Then
                    If My.Settings.RunbookEnabled Then
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
                    End If
                Catch ex As Exception
                    HandleException(ex)
                End Try
            End Sub

            Private Sub Archive()
                Try
                    If My.Settings.RepositoryEnabled Then
                        'And DAL.AvailableLicenses >= 0 Then
                        ' set the repository connection string values
                        Dim ArchiveId As Integer = CInt(My.Application.CommandLineArgs(2))
                        Dim arch As New cCommon.cArchive
                        arch.RepositoryConnectionTimeout = My.Settings.RepositoryConnectionTimeout
                        arch.RepositoryDatabaseName = My.Settings.RepositoryDatabaseName
                        arch.RepositoryInstanceName = My.Settings.RepositoryInstanceName
                        arch.RepositoryUseTrustedConnection = My.Settings.RepositoryUseTrustedConnection
                        arch.RepositorySQLLoginName = My.Settings.RepositorySQLLoginName
                        arch.RepositorySQLLoginPassword = My.Settings.RepositorySQLLoginPassword
                        arch.RepositoryEncryptConnection = My.Settings.RepositoryEncryptConnection
                        arch.RepositoryTrustServerCertificate = My.Settings.RepositoryTrustServerCertificate
                        arch.RepositoryNetworkLibrary = My.Settings.RepositoryNetworkLibrary
                        arch.ArchiveComplete = My.Resources.ArchiveComplete
                        My.Application.Log.WriteEntry(String.Format(My.Resources.ArchiveStarting, ArchiveId), _
                                             TraceEventType.Information)
                        arch.Archive(ArchiveId)
                    End If
                Catch ex As Exception
                    HandleException(ex)
                End Try
            End Sub


#Region " Error Handling "

            Private Sub HandleException(ByVal ex As System.Exception)
                Dim EventLog1 As New System.Diagnostics.EventLog("Application", ".", My.Application.Info.ProductName & "CmdLn")
                Try
                    ' a local copy of the ex is used to concat the messages 
                    Dim ex1 As System.Exception = ex
                    Dim ex1Msg As String = ex1.Message & vbCrLf & vbCrLf
                    While Not ex1.InnerException Is Nothing
                        ex1Msg += ex1.InnerException.Message & vbCrLf & vbCrLf
                        ex1 = ex1.InnerException
                    End While
                    ex1Msg += My.Resources.ExceptionInfoMsg
                    EventLog1.WriteEntry(ex1Msg, EventLogEntryType.Error, 100)
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
                    Catch y As Exception
                        Throw y
                    End Try
                End Try
            End Sub

#End Region

#Region " compare event handlers "

            Private HTMLSide1 As String
            Private HTMLSide2 As String

            Private Sub ExceptionEventHandler(ByVal exMsg As String)
                ' need to add the call stack
                HandleException(New Exception(exMsg))
                HTMLSide1 = "<table width=""100%"">"
                HTMLSide2 = "<table width=""100%"">"
            End Sub

            Private Sub ComparingEventHandler(ByVal Item1 As String, _
                                              ByVal Item2 As String)
                If Item1 = My.Resources.CompareComplete _
                    Or Item1 = My.Resources.CompareCancelled Then
                    HTMLSide1 += "</table>"
                    HTMLSide2 += "</table>"

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
                    Dim ts As String = Now.ToString("yyyyMMMddhhmmss")
                    Using sw As StreamWriter = New StreamWriter(String.Format("{0}\SQLClueCompare{1}.htm", Path, ts))
                        sw.WriteLine("<!DOCTYPE html PUBLIC ""-//W3C//DTD XHTML 1.1//EN"" ""http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd"">")
                        sw.WriteLine("<html xmlns=""http://www.w3.org/1999/xhtml""><head><title>" & My.Application.Info.ProductName & " Compare</title></head>")
                        sw.WriteLine("<frameset cols=""100%"" rows=""10%,80%,10%"" frameborder=""1"">")
                        sw.WriteLine(String.Format("<frame src=""SQLClueTitle{0}.htm"" scrolling=""no"" name=""top"">", ts))
                        sw.WriteLine("<frameset cols=""50%,50%"" rows=""100%"" frameborder=""1"">")
                        sw.WriteLine(String.Format("<frame src=""SQLClueResultsA{0}.htm"" scrolling=""yes"" name=""left"">", ts))
                        sw.WriteLine(String.Format("<frame src=""SQLClueResultsB{0}.htm"" scrolling=""yes"" name=""right"">", ts))
                        sw.WriteLine("</frameset>")
                        sw.WriteLine(String.Format("<frame src=""SQLClueKey{0}.htm"" scrolling=""no"" name=""bottom"">", ts))
                        sw.WriteLine("<noframes><body>")
                        sw.WriteLine("A browser that supports HTML Frames is required.")
                        sw.WriteLine("</body></noframes></frameset></html>")
                    End Using
                    Using sw As StreamWriter = New StreamWriter(String.Format("{0}\SQLClueTitle{1}.htm", Path, ts))
                        sw.WriteLine("<!DOCTYPE html PUBLIC ""-//W3C//DTD XHTML 1.1//EN"" ""http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd"">")
                        sw.WriteLine("<html xmlns=""http://www.w3.org/1999/xhtml"">")
                        sw.WriteLine("<head>")
                        sw.WriteLine("<title>" & My.Application.Info.ProductName & " Compare Key</title>")
                        sw.WriteLine("</head>")
                        sw.WriteLine("<body style=""font-family: Lucida Console; font-size: 10px"">")
                        sw.WriteLine("<table width=""100%"">")
                        sw.WriteLine("<tr>")
                        sw.WriteLine("<td style=""font-weight: bold; text-align: left"">")
                        sw.WriteLine(String.Format("  {0}", My.Application.CommandLineArgs(3)))
                        sw.WriteLine("</td>")
                        sw.WriteLine("<td style=""font-weight: bold; text-align: right"">")
                        sw.WriteLine(String.Format("  {0}", My.Application.CommandLineArgs(6)))
                        sw.WriteLine("</td>")
                        sw.WriteLine("</tr>")
                        sw.WriteLine("<tr>")
                        sw.WriteLine("<td width=""50%"" style=""text-align: left"">")
                        sw.WriteLine(String.Format("Origin: {0}", My.Application.CommandLineArgs(2)))
                        sw.WriteLine("</td>")
                        sw.WriteLine("<td width=""50%"" style=""text-align: right"">")
                        sw.Write(String.Format("Origin: {0}", My.Application.CommandLineArgs(5)))
                        sw.WriteLine("</td>")
                        sw.WriteLine("</tr>")
                        sw.WriteLine("</table>")
                        sw.WriteLine("</body></html>")
                    End Using
                    Using sw As StreamWriter = New StreamWriter(String.Format("{0}\SQLClueResultsA{1}.htm", Path, ts))
                        sw.WriteLine("<!DOCTYPE html PUBLIC ""-//W3C//DTD XHTML 1.1//EN"" ""http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd"">")
                        sw.WriteLine("<html xmlns=""http://www.w3.org/1999/xhtml""><head><title>" & My.Application.Info.ProductName & " Compare Panel 1</title></head>")
                        sw.Write(String.Format("<body style=""font-family: Lucida Console; font-size: 10px"">{0}</body></html>", HTMLSide1))
                    End Using
                    Using sw As StreamWriter = New StreamWriter(String.Format("{0}\SQLClueResultsB{1}.htm", Path, ts))
                        ' scroller from http://www.webreference.com/programming/javascript/jf/column4/
                        sw.WriteLine("<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.01//EN"">")
                        sw.WriteLine("<html>")
                        sw.WriteLine("<head>")
                        sw.WriteLine("<meta http-equiv=""Content-Type"" content=""text/html; charset=UTF-8"">")
                        sw.WriteLine("<title>" & My.Application.Info.ProductName & " Compare Panel 2</title>")
                        sw.WriteLine("<script type=""text/javascript"">")
                        sw.WriteLine("var _run;")
                        sw.WriteLine("if(navigator.userAgent.indexOf(""Firefox"")!=-1||navigator.appName==""Microsoft Internet Explorer"")")
                        sw.WriteLine("{_run=false;}")
                        sw.WriteLine("else {_run= true;}")
                        sw.WriteLine("function scrollR()")
                        sw.WriteLine("{")
                        sw.WriteLine("var left = (window.pageXOffset)?(window.pageXOffset):(document.documentElement)?document.documentElement.scrollLeft:document.body.scrollLeft;")
                        sw.WriteLine("var top = (window.pageYOffset)?(window.pageYOffset):(document.documentElement)?document.documentElement.scrollTop:document.body.scrollTop;")
                        sw.WriteLine("parent.frames[""left""].scrollTo(left,top);")
                        sw.WriteLine("}")
                        sw.WriteLine("function searchScroll(){")
                        sw.WriteLine("var left = (window.pageXOffset)?(window.pageXOffset):(document.documentElement)?document.documentElement.scrollLeft:document.body.scrollLeft;")
                        sw.WriteLine("var top = (window.pageYOffset)?(window.pageYOffset):(document.documentElement)?document.documentElement.scrollTop:document.body.scrollTop;")
                        sw.WriteLine("parent.frames[""left""].scrollTo(left,top);")
                        sw.WriteLine("window.setTimeout(""searchScroll();"",1);")
                        sw.WriteLine("}")
                        sw.WriteLine("if(_run == false)")
                        sw.WriteLine("{")
                        sw.WriteLine("window.onscroll=function(){scrollR();}")
                        sw.WriteLine("} else { ")
                        sw.WriteLine("window.onload=function(){searchScroll()}")
                        sw.WriteLine("}")
                        sw.WriteLine("</script>")
                        sw.WriteLine("</head>")
                        sw.Write(String.Format("<body style=""font-family: Lucida Console; font-size: 10px"">{0}</body></html>", HTMLSide2))
                    End Using
                    Using sw As StreamWriter = New StreamWriter(String.Format("{0}\SQLClueKey{1}.htm", Path, ts))
                        sw.WriteLine("<!DOCTYPE html PUBLIC ""-//W3C//DTD XHTML 1.1//EN"" ""http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd"">")
                        sw.WriteLine("<html xmlns=""http://www.w3.org/1999/xhtml""><head><title>" & My.Application.Info.ProductName & " Compare Key</title></head>")
                        sw.WriteLine("<body style=""font-family: Lucida Console; font-size: 10px"">")
                        sw.WriteLine("<table width=""100%""><tr><td style=""text-align: center"">")
                        sw.WriteLine("<table border=""1"" width=""40%""><tr><td>Results Key</td>")
                        sw.WriteLine("<td style=""background-color: White; color: Black"">matched</td>")
                        sw.WriteLine("<td style=""background-color: Yellow; color: Teal"">different</td>")
                        sw.WriteLine("<td style=""background-color: Teal; color: White"">no match</td>")
                        sw.WriteLine("<td style=""background-color: Silver; color: Black"">label</td>")
                        sw.WriteLine("</tr></table>")
                        sw.WriteLine(String.Format("<table><tr><td>{0}</td></tr></table>", Now.ToString))
                        sw.WriteLine("</td></tr></table>")
                        sw.WriteLine("</body></html>")
                    End Using
                Else
                    If My.Settings.Misc_Display__Output_Show__Comparison__Details OrElse My.Settings.Misc_Display__Output_Show__Differences__Only Then
                        If Not Item1 = "" Then

                            HTMLSide1 += "<tr><td style=""background-color: Silver; color: Black"">" & vbCrLf
                            HTMLSide1 += String.Format("-- Origin: [{0}]" & vbCrLf, _
                                                           My.Application.CommandLineArgs(2))
                            HTMLSide1 += "</td></tr>" & vbCrLf
                            If InStr(My.Application.CommandLineArgs(3), _
                                     Item1) > 0 _
                            AndAlso Mid(My.Application.CommandLineArgs(3), _
                                        InStr(My.Application.CommandLineArgs(3), _
                                              Item1)) = Item1 Then
                                HTMLSide1 += "<tr><td style=""background-color: Silver; color: Black"">" & vbCrLf
                                HTMLSide1 += String.Format("-- Node: [{0}]" & vbCrLf, _
                                                               My.Application.CommandLineArgs(3))
                                HTMLSide1 += "</td></tr>" & vbCrLf
                            Else
                                HTMLSide1 += "<tr><td style=""background-color: Silver; color: Black"">" & vbCrLf
                                HTMLSide1 += String.Format("-- Node: [{0}|{1}]" & vbCrLf, _
                                                               My.Application.CommandLineArgs(3), _
                                                               Item1)
                                HTMLSide1 += "</td></tr>" & vbCrLf
                            End If
                        Else
                            HTMLSide1 += "<tr><td style=""background-color: Silver; color: Black"">&nbsp</td></tr>" & vbCrLf
                            HTMLSide1 += "<tr><td style=""background-color: Silver; color: Black"">&nbsp</td></tr>" & vbCrLf
                        End If
                        If Not Item2 = "" Then
                            HTMLSide2 += "<tr><td style=""background-color: Silver; color: Black"">" & vbCrLf
                            HTMLSide2 += String.Format("-- Origin: [{0}]" & vbCrLf, _
                                                           My.Application.CommandLineArgs(5))
                            HTMLSide2 += "</td></tr>" & vbCrLf
                            If InStr(My.Application.CommandLineArgs(6), _
                                     Item2) > 0 _
                            AndAlso Mid(My.Application.CommandLineArgs(6), _
                                        InStr(My.Application.CommandLineArgs(6), _
                                              Item2)) = Item2 Then
                                HTMLSide2 += "<tr><td style=""background-color: Silver; color: Black"">" & vbCrLf
                                HTMLSide2 += String.Format("-- Node: [{0}]" & vbCrLf, _
                                                               My.Application.CommandLineArgs(6))
                                HTMLSide2 += "</td></tr>" & vbCrLf
                            Else
                                HTMLSide2 += "<tr><td style=""background-color: Silver; color: Black"">" & vbCrLf
                                HTMLSide2 += String.Format("-- Node: [{0}|{1}]" & vbCrLf, _
                                                               My.Application.CommandLineArgs(6), _
                                                               Item2)
                                HTMLSide2 += "</td></tr>" & vbCrLf
                            End If
                        Else
                            HTMLSide2 += "<tr><td style=""background-color: Silver; color: Black"">&nbsp</td></tr>" & vbCrLf
                            HTMLSide2 += "<tr><td style=""background-color: Silver; color: Black"">&nbsp</td></tr>" & vbCrLf
                        End If
                    End If
                End If
            End Sub

            Private Sub WriteResultEventHandler(ByVal ResultType As String, _
                                                ByVal Item1 As String, _
                                                ByVal Item2 As String)
                'munge the blank compare results before setting length
                If InStr(ResultType, "1Blank") > 0 Then
                    If InStr(ResultType, "Name") = 1 _
                    And Not My.Settings.Misc_Display__Output_Show__Comparison__Details Then
                        Item1 = Space(1)
                        Item2 = "-- " & My.Application.CommandLineArgs(6) & ": " & Item2
                    ElseIf InStr(ResultType, "Line") = 1 Then
                        Item1 = Space(1)
                    End If
                ElseIf InStr(ResultType, "2Blank") > 0 Then
                    If InStr(ResultType, "Name") = 1 _
                    And Not My.Settings.Misc_Display__Output_Show__Comparison__Details Then
                        Item1 = "-- " & My.Application.CommandLineArgs(3) & ": " & Item1
                        Item2 = Space(1)
                    ElseIf InStr(ResultType, "Line") = 1 Then
                        Item2 = Space(1)
                    End If
                End If
                If InStr(ResultType, "Match") > 0 Then
                    If My.Settings.Misc_Display__Output_Show__Differences__Only And cmp._CompareCollection Then
                        Exit Sub
                    End If
                    HTMLSide1 += "<tr><td style=""background-color: White; color: Black"">" & vbCrLf
                    HTMLSide2 += "<tr><td style=""background-color: White; color: Black"">" & vbCrLf
                ElseIf InStr(ResultType, "Different") > 0 Then
                    HTMLSide1 += "<tr><td style=""background-color: Yellow; color: Teal"">" & vbCrLf
                    HTMLSide2 += "<tr><td style=""background-color: Yellow; color: Teal"">" & vbCrLf
                ElseIf InStr(ResultType, "Blank") > 0 Then
                    HTMLSide1 += "<tr><td style=""background-color: Teal; color: White"">" & vbCrLf
                    HTMLSide2 += "<tr><td style=""background-color: Teal; color: White"">" & vbCrLf
                End If
                HTMLSide1 += If(Trim(Item1) = "", "&nbsp;", Item1) & "</td></tr>" & vbCrLf
                HTMLSide2 += If(Trim(Item2) = "", "&nbsp;", Item2) & "</td></tr>" & vbCrLf
            End Sub

            Private Sub WriteLineResultEventHandler(ByVal ResultType As String, _
                                                    ByVal Item1 As String, _
                                                    ByVal Line1Number As Int32, _
                                                    ByVal Line1 As String, _
                                                    ByVal Item2 As String, _
                                                    ByVal Line2Number As Int32, _
                                                    ByVal Line2 As String)
                If My.Settings.Misc_Display__Output_Number__Script__Lines Then
                    Line1 = String.Format("{0}  {1}", Line1Number + 1, Line1)
                    Line2 = String.Format("{0}  {1}", Line2Number + 1, Line2)
                End If
                If My.Settings.Misc_Display__Output_Show__Object__Name__on__Script__Lines Then
                    ' no labels, only numbers if comparing a single item
                    If My.Application.CommandLineArgs(3) Is Nothing OrElse cmp.IsCollection(My.Application.CommandLineArgs(3)) Then
                        Line1 = String.Format("{0} | {1}", Item1, Line1)
                        Line2 = String.Format("{0} | {1}", Item2, Line2)
                    End If
                End If
                WriteResultEventHandler(ResultType, Line1, Line2)
            End Sub

#End Region


        End Class

    End Class

End Namespace




