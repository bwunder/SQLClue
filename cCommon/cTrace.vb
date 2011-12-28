Public Class cTrace

    'Public tSQLCfgConnection As dsSQLConfiguration.tSQLCfgConnectionDataTable
    'Public tSQLCfgSchedule As dsSQLConfiguration.tSQLCfgScheduleDataTable
    'Public dsQueryBaseline As New DataSetQueryBaseline

    Public Baseline As New cCommon.DataClassesQueryBaselineDataContext
    Public PlanData As IEnumerable(Of cCommon.tPlan)
    Public TemplateData As IEnumerable(Of cCommon.tTemplate)
    Public DeleteArchiveData As IEnumerable(Of cCommon.tDeleteArchiveQueue)
    Public ApplicationsToExcludeData As IEnumerable(Of cCommon.tApplicationsToExcludeFromArchive)
    Public GroupDataToExcludeData As IEnumerable(Of cCommon.tGroupDataToExcludeFromArchive)
    Public ThePlan As IEnumerable(Of cCommon.pGetPlanResult)
    Public SQLCfg As New cCommon.DataClassesSQLConfigurationDataContext
    Public ConnectionData As IEnumerable(Of cCommon.tConnection)
    Public ScheduleData As IEnumerable(Of cCommon.tSchedule)
    Public BaselineInstanceName As String
    Public BaselineDatabaseName As String
    Public BaselineUseTrustedConnection As Boolean
    Public BaselineSQLLoginName As String
    Public BaselineSQLLoginPassword As String
    Public BaselineConnectionTimeout As Integer
    Public BaselineNetworkLibrary As String
    Public BaselineEncryptConnection As Boolean
    Public BaselineTrustServerCertificate As Boolean
    Public TargetBaselineTraceProcDatabase As String
    Private drTracePlan As DataRow
    Private dsDBList As New DataSet
    Private daDBList As SqlDataAdapter = New SqlDataAdapter()
    Public sSQLServer As String
    Public TraceStopDt As DateTime
    Public TraceStopUTCDt As DateTime
    Public TraceName As String
    Public TraceStatus As Integer
    Public StatusMessage As String

    Public ReadOnly Property sBaselineConnectionString() As String
        Get
            Dim builder As New System.Data.SqlClient.SqlConnectionStringBuilder
            builder.DataSource = BaselineInstanceName
            builder.InitialCatalog = BaselineDatabaseName
            If BaselineUseTrustedConnection Then
                builder.IntegratedSecurity = True
            Else
                builder.UserID = BaselineSQLLoginName
                builder.Password = BaselineSQLLoginPassword
            End If
            builder.ConnectTimeout = BaselineConnectionTimeout
            builder.ApplicationName = My.Application.Info.ProductName & " : Query Baseline"
            'allow for default of 'not specified' netlib
            If BaselineNetworkLibrary <> "" Then
                builder.NetworkLibrary = CStr(If(InStr(BaselineNetworkLibrary, " ") > 0, _
                                                 BaselineNetworkLibrary.Split(Chr(32))(0), _
                                                 BaselineNetworkLibrary))
            End If
            builder.Encrypt = BaselineEncryptConnection
            builder.TrustServerCertificate = BaselineTrustServerCertificate
            builder.Enlist = True
            builder.PersistSecurityInfo = False
            builder.UserInstance = False
            Return builder.ConnectionString
        End Get
    End Property

    ' see also DialogConnect
    Public ReadOnly Property TargetConnectionString(ByVal TargetInstanceName As String) As String
        Get
            'Same as cDataAccess.TargetConnectionString
            Dim cn = (From c In SQLCfg.tConnections _
                     Where c.InstanceName = TargetInstanceName _
                     Select c).ToList
            ' keep seeing the row disappear mid stream using this when making a runbook connection
            'Dim drCn As dsSQLConfiguration.tConnectionRow = dsSQLCfg.tConnection.FindByInstanceName(TargetInstanceName)
            If cn Is Nothing OrElse cn(0).InstanceName = "" Then
                Return ""
            Else
                Dim builder As New System.Data.SqlClient.SqlConnectionStringBuilder
                builder.DataSource = cn(0).InstanceName
                builder.IntegratedSecurity = cn(0).LoginSecure
                If Not builder.IntegratedSecurity Then
                    SQLCfg.pConnectionUserGet(cn(0).InstanceName, builder.UserID, builder.Password)
                End If
                builder.ConnectTimeout = cn(0).ConnectionTimeout
                builder.ApplicationName = My.Application.Info.ProductName
                ' all a "not specified" option
                If cn(0).NetworkProtocol <> "" Then
                    builder.NetworkLibrary = cn(0).NetworkProtocol.Split(CChar(" "))(0)
                End If
                builder.Encrypt = cn(0).EncryptedConnection
                builder.TrustServerCertificate = cn(0).TrustServerCertificate
                builder.Enlist = True
                builder.MultipleActiveResultSets = False
                builder.PersistSecurityInfo = True
                builder.UserInstance = False
                Return builder.ConnectionString
            End If
        End Get
    End Property
    ' two steps- Start Trace is called, then LoadTraceFileToTable is called when trace completes
    Public Sub StartTrace(ByVal PlanId As Int32, _
                          ByVal TargetConnectionString As String, _
                          ByVal ScheduleId As Int32, _
                          ByRef TrcName As String, _
                          ByRef TrcStatus As Int32, _
                          ByRef StsMessage As String)
        Try
            'SMO Trace Object not supported for 64 bit BOL says, The SMOTracer sample excludes 
            'express, all that is left is trace by procedure...
            'write to the table save all scrubbing for later
            ThePlan = Baseline.pGetPlan(PlanId).ToList
            sSQLServer = ThePlan(0).SQLInstance
            ' if there is a scheuldule being used, reschedule now
            If ScheduleId > 0 Then
                Dim ThisSchedule = (From s In SQLCfg.tSchedules _
                                   Where s.Id = ScheduleId _
                                   Select s).ToList(0)

                Dim InstanceName As String = ThisSchedule.InstanceName
                Dim BaselinePlanId As Integer = ThisSchedule.BaselinePlanId
                Dim Interval As Integer = ThisSchedule.Interval
                Dim IntervalType As String = ThisSchedule.IntervalType
                Dim IntervalBaseDt As DateTime = CDate(ThisSchedule.IntervalBaseDt)
                Dim UseEventNotifications As Boolean = ThisSchedule.UseEventNotifications
                Dim IsActive As Boolean = CBool(ThisSchedule.IsActive)
                Dim NextRunDate As DateTime
                ' compute the next schedule date
                ' this will be the nearest time after now that maintains the established Interval criteria
                Dim val As DateInterval
                Select Case IntervalType
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
                NextRunDate = If(ThePlan(0).IsPlandomized, _
                                 CDate(Baseline.pGetNextRunDate(PlanId).Single.NextRunDate), _
                                 DateAdd(val, Interval, IntervalBaseDt))
                Dim UpdateSchedule = SQLCfg.pScheduleUpdate(ScheduleId, _
                                                            BaselinePlanId, _
                                                            InstanceName, _
                                                            Interval, _
                                                            IntervalType, _
                                                            NextRunDate, _
                                                            UseEventNotifications, _
                                                            IsActive)
            End If
            LoadTraceProcOnTarget(TargetConnectionString)
            StartTraceOnTarget(sSQLServer, TargetConnectionString)
            ' set the return values
            TrcName = TraceName
            TrcStatus = TraceStatus
            StsMessage = StatusMessage
            If Not TraceName = "" _
            And TraceStatus = 0 Then
                LogTrace(PlanId)
            Else
                ' no need to go on if the trace start failed
                Throw New Exception(String.Format("Sample SQL Trace status [{0}] at startup of trace [{1}]. Status Message: {2}", _
                                                             TraceStatus, If(TraceName = "", "unknown", TraceName), StatusMessage))
            End If
        Catch ex As Exception
            Throw New Exception(String.Format("(cTrace.StartTrace) Exception."), ex)
        End Try
    End Sub

    Public Sub StartTest(ByVal PlanId As Int32, _
                         ByVal TargetConnectionString As String, _
                         ByRef TrcName As String, _
                         ByRef TrcStatus As Int32, _
                         ByRef StsMessage As String)
        Try
            ThePlan = Baseline.pGetPlan(PlanId).ToList
            sSQLServer = ThePlan(0).SQLInstance
            ThePlan(0).IsActive = True
            ThePlan(0).MaxFileSizeMB = 1
            ThePlan(0).TemplateName = "Test"
            ThePlan(0).MinutesToTrace = 1
            ThePlan(0).AllowFileRollover = False
            ThePlan(0).IncludeAttentions = False
            ThePlan(0).IncludeAutoGrow = False
            ThePlan(0).IncludeAutoStats = False
            ThePlan(0).IncludeBatches = False
            ThePlan(0).IncludeBatchStarts = False
            ThePlan(0).IncludeErrors = False
            ThePlan(0).IncludeObjectsCreated = False
            ThePlan(0).IncludeRecompiles = False
            ThePlan(0).IncludeScans = False
            ThePlan(0).IncludeShowplanAll = False
            ThePlan(0).IncludeStatements = False
            ThePlan(0).IncludeStatementStarts = False
            ThePlan(0).IncludeSystemObjects = False
            ThePlan(0).IncludeWarnings = False

            LoadTraceProcOnTarget(TargetConnectionString)
            StartTraceOnTarget(sSQLServer, TargetConnectionString)
            ' set the return values
            TrcName = TraceName
            TrcStatus = TraceStatus
            StsMessage = StatusMessage
            If TraceName = "" _
            Or TraceStatus <> 0 Then
                ' no need to go on if the trace start failed
                Throw New Exception(String.Format("Test SQL Trace status [{0}] at startup of trace [{1}]. Status Message: {2}", _
                                                             TraceStatus, If(TraceName = "", "unknown", TraceName), StatusMessage))
            End If
        Catch ex As Exception
            Throw New Exception(String.Format("(cTrace.StartTest) Exception."), ex)
        End Try
    End Sub

    ' two steps- Start Trace is called, then LoadTraceFileToTable is called when trace completes
    Public Sub LoadTraceFileToTable(ByVal PlanId As Int32, _
                                    ByVal TargetConnectionString As String, _
                                    ByVal NewTraceName As String)

        Baseline.Connection.ConnectionString = sBaselineConnectionString
        ThePlan = Baseline.pGetPlan(PlanId).ToList

        LoadSample(TargetConnectionString, NewTraceName)

        StartDeleteArchiveConversation()

        ' remove the trace files when load is successful if host reference is configured
        If Not ThePlan(0).OutputFolderHostReference Is Nothing _
        AndAlso Not ThePlan(0).OutputFolderHostReference = "" Then
            For Each f In My.Computer.FileSystem.GetDirectoryInfo(ThePlan(0).OutputFolderHostReference).GetFiles(TraceName & "*.trc")
                Try
                    f.Delete()
                Catch ex As Exception
                    Throw New Exception(String.Format("Exception while attempting to remove trace output file [{0}]", f.FullName))
                End Try
            Next
        End If

    End Sub

    Sub LoadSample(ByVal TargetConnectionString As String, _
                   ByVal NewTraceName As String)
        ' the target instance is the source of this bulk copy
        ' and the host is the destination 
        Try
            Baseline.Connection.ConnectionString = sBaselineConnectionString
            Dim fnGetTraceTableQuery = (From c In Baseline.tConfigs _
                                        Select c.fnGetTraceQuery).ToList(0).ToString

            TraceName = NewTraceName
            CreateSampleTable()
            Dim FileName As String = If(Right(ThePlan(0).OutputFolder, 1) = "\", _
                                              ThePlan(0).OutputFolder, _
                                              ThePlan(0).OutputFolder & "\") & NewTraceName & ".trc"
            ' the goal is a fire hose into an empty and unindexed table
            Using sourceConnection As SqlConnection = _
               New SqlConnection(TargetConnectionString)
                sourceConnection.Open()
                ' Get data from the source table as a SqlDataReader.
                Dim commandSourceData As SqlCommand = New SqlCommand( _
                   String.Format(fnGetTraceTableQuery, FileName), sourceConnection)
                Dim reader As SqlDataReader = commandSourceData.ExecuteReader
                ' Open the destination (QueryBaseline) connection.
                Using destinationConnection As SqlConnection = _
                    New SqlConnection(sBaselineConnectionString)
                    destinationConnection.Open()
                    ' Set up the bulk copy object. 

                    Using bulkCopy As SqlBulkCopy = _
                      New SqlBulkCopy(destinationConnection)
                        bulkCopy.DestinationTableName = _
                        String.Format("QueryBaseline.{0}", TraceName)
                        bulkCopy.ColumnMappings.Add("EventClass", "EventClass")
                        bulkCopy.ColumnMappings.Add("EventSubClass", "EventSubClass")
                        bulkCopy.ColumnMappings.Add("TextData", "TextData")
                        bulkCopy.ColumnMappings.Add("IntegerData", "IntegerData")
                        bulkCopy.ColumnMappings.Add("BinaryData", "BinaryData")
                        bulkCopy.ColumnMappings.Add("DatabaseID", "DatabaseID")
                        bulkCopy.ColumnMappings.Add("DatabaseName", "DatabaseName")
                        bulkCopy.ColumnMappings.Add("ObjectName", "ObjectName")
                        bulkCopy.ColumnMappings.Add("ObjectID", "ObjectID")
                        bulkCopy.ColumnMappings.Add("IndexID", "IndexID")
                        bulkCopy.ColumnMappings.Add("NTUserName", "NTUserName")
                        bulkCopy.ColumnMappings.Add("ApplicationName", "ApplicationName")
                        bulkCopy.ColumnMappings.Add("HostName", "HostName")
                        bulkCopy.ColumnMappings.Add("LoginName", "LoginName")
                        bulkCopy.ColumnMappings.Add("Severity", "Severity")
                        bulkCopy.ColumnMappings.Add("State", "State")
                        bulkCopy.ColumnMappings.Add("Error", "Error")
                        bulkCopy.ColumnMappings.Add("Duration", "Duration")
                        bulkCopy.ColumnMappings.Add("StartTime", "StartTime")
                        bulkCopy.ColumnMappings.Add("EndTime", "EndTime")
                        bulkCopy.ColumnMappings.Add("Reads", "Reads")
                        bulkCopy.ColumnMappings.Add("Writes", "Writes")
                        bulkCopy.ColumnMappings.Add("CPU", "CPU")
                        bulkCopy.ColumnMappings.Add("ClientProcessID", "ClientProcessID")
                        bulkCopy.ColumnMappings.Add("SPID", "SPID")
                        Try
                            ' Write from the source to the destination.
                            bulkCopy.WriteToServer(reader)
                        Catch ex As Exception
                            Throw New Exception(String.Format("Sample import to repository failed for trace {0}", TraceName), ex)
                        Finally
                            ' Close the SqlDataReader. The SqlBulkCopy
                            ' object is automatically closed at the end
                            ' of the Using block.
                            reader.Close()
                        End Try
                    End Using
                End Using
            End Using
        Catch ex As Exception
            Throw New Exception("(cTrace.LoadSample) Exception.", ex)
        End Try

    End Sub

    Public ReadOnly Property sTraceProcName(ByVal TargetConnectionString As String) As String
        Get
            Using cnTraceTarget As New System.Data.SqlClient.SqlConnection(TargetConnectionString)
                cnTraceTarget.Open()
                Select Case CInt(Mid(cnTraceTarget.ServerVersion.ToString, 1, InStr(cnTraceTarget.ServerVersion.ToString, ".")))
                    Case 8
                        sTraceProcName = "dbo.SQLClueBaselineTrace2000"
                    Case 9
                        sTraceProcName = "dbo.SQLClueBaselineTrace2005"
                    Case 10
                        sTraceProcName = "dbo.SQLClueBaselineTrace2008"
                    Case Else
                        Throw New Exception(String.Format("Don't know how to trace SQL version {0}", cnTraceTarget.ServerVersion))
                End Select
            End Using
        End Get
    End Property

    Public Sub LoadTraceProcOnTarget(ByVal TargetConnectionString As String)
        ' make sure the target has the current version of the trace proc then start the trace
        Try

            '!! this is the logic to copy any procedure/trigger/function/view
            '   the drop supports only procedures 

            Dim sCreateScript As String = ""
            ' pull the script from the SQLClue Host's QueryBaseline DB  
            Using cnQueryBaselines As New System.Data.SqlClient.SqlConnection(sBaselineConnectionString)
                cnQueryBaselines.Open()
                Using cmdGetScript As New SqlCommand
                    cmdGetScript.Connection = cnQueryBaselines
                    cmdGetScript.CommandText = String.Format("SELECT OBJECT_DEFINITION (OBJECT_ID('{0}'))", _
                                                             sTraceProcName(TargetConnectionString))
                    cmdGetScript.CommandType = CommandType.Text
                    Dim rdrScript As SqlDataReader = cmdGetScript.ExecuteReader
                    While rdrScript.Read
                        sCreateScript = sCreateScript + rdrScript(0).ToString
                    End While
                End Using
            End Using
            ' drop any existing trace proc
            Using cnTraceTarget As New System.Data.SqlClient.SqlConnection(TargetConnectionString)
                cnTraceTarget.Open()
                Using cmdDropTraceProc As New SqlCommand
                    cmdDropTraceProc.Connection = cnTraceTarget
                    cmdDropTraceProc.Connection.ChangeDatabase(TargetBaselineTraceProcDatabase)
                    cmdDropTraceProc.CommandText = String.Format("IF OBJECT_ID('{0}','P') IS NOT NULL DROP PROCEDURE {0}", _
                                                                 sTraceProcName(TargetConnectionString))
                    cmdDropTraceProc.CommandType = CommandType.Text
                    cmdDropTraceProc.ExecuteNonQuery()
                End Using
            End Using
            ' create a clone of the trace proc as found in the admin db of the repository server
            Using cnTraceTarget As New System.Data.SqlClient.SqlConnection(TargetConnectionString)
                cnTraceTarget.Open()
                Using cmdCreateTraceProc As New SqlCommand
                    cmdCreateTraceProc.Connection = cnTraceTarget
                    cmdCreateTraceProc.Connection.ChangeDatabase(TargetBaselineTraceProcDatabase)
                    cmdCreateTraceProc.CommandText = sCreateScript
                    cmdCreateTraceProc.CommandType = CommandType.Text
                    cmdCreateTraceProc.ExecuteNonQuery()
                End Using
            End Using
        Catch ex As Exception
            Throw New Exception(String.Format("(cTrace.LoadTraceProcOnTarget) Exception."), ex)
        End Try
    End Sub

    Public Sub CreateSampleTable()
        Try
            ' log trace execution
            Using cnQueryBaselines As New System.Data.SqlClient.SqlConnection(sBaselineConnectionString)
                cnQueryBaselines.Open()
                Using cmdLogTrace As New System.Data.SqlClient.SqlCommand
                    cmdLogTrace.Connection = cnQueryBaselines
                    cmdLogTrace.CommandText = "QueryBaseline.pCreateSampleTable"
                    cmdLogTrace.CommandType = CommandType.StoredProcedure
                    cmdLogTrace.Parameters.Add("@TraceName", SqlDbType.NVarChar, 128).Direction = ParameterDirection.Input
                    cmdLogTrace.Parameters("@TraceName").Value = TraceName
                    cmdLogTrace.ExecuteNonQuery()
                End Using
            End Using
        Catch ex As Exception
            Throw New Exception(String.Format("(cTrace.CreateSampleTable) Exception"), ex)
        End Try
    End Sub

    Public Sub StartDeleteArchiveConversation()
        Try
            ' log trace execution
            Using cnQueryBaselines As New System.Data.SqlClient.SqlConnection(sBaselineConnectionString)
                cnQueryBaselines.Open()
                Using cmdLogTrace As New System.Data.SqlClient.SqlCommand
                    cmdLogTrace.Connection = cnQueryBaselines
                    cmdLogTrace.CommandText = "QueryBaseline.pSendStartMessage"
                    cmdLogTrace.CommandType = CommandType.StoredProcedure
                    cmdLogTrace.Parameters.Add("@TraceName", SqlDbType.NVarChar, 128).Direction = ParameterDirection.Input
                    cmdLogTrace.Parameters("@TraceName").Value = TraceName
                    cmdLogTrace.Parameters.Add("@ConversationHandle", SqlDbType.UniqueIdentifier).Direction = ParameterDirection.Input
                    cmdLogTrace.Parameters("@ConversationHandle").Value = DBNull.Value
                    cmdLogTrace.Parameters.Add("@LoadDt", SqlDbType.DateTime).Direction = ParameterDirection.Input
                    cmdLogTrace.Parameters("@LoadDt").Value = Now
                    cmdLogTrace.ExecuteNonQuery()
                End Using
            End Using
        Catch ex As Exception
            Throw New Exception(String.Format("(cTrace.StartDeleteArchiveConversation) Exception"), ex)
        End Try
    End Sub

    Public Sub ResetQueues()
        Try
            ' log trace execution
            Using cnQueryBaselines As New System.Data.SqlClient.SqlConnection(sBaselineConnectionString)
                cnQueryBaselines.Open()
                Using cmdReset As New System.Data.SqlClient.SqlCommand
                    cmdReset.Connection = cnQueryBaselines
                    cmdReset.CommandText = "QueryBaseline.pResetQueueStatus"
                    cmdReset.CommandType = CommandType.StoredProcedure
                    cmdReset.ExecuteNonQuery()
                End Using
            End Using
        Catch ex As Exception
            Throw New Exception(String.Format("(cTrace.ResetQueues) Exception"), ex)
        End Try
    End Sub

    Public Sub StartTraceOnTarget(ByVal SQLInstance As String, _
                        ByVal TargetConnectionString As String)
        Try
            ' run trace
            Using cnTraceTarget As New System.Data.SqlClient.SqlConnection(TargetConnectionString)
                cnTraceTarget.Open()
                Using cmdStartTrace As New SqlCommand
                    cmdStartTrace.Connection = cnTraceTarget
                    cmdStartTrace.Connection.ChangeDatabase(TargetBaselineTraceProcDatabase)
                    cmdStartTrace.CommandText = sTraceProcName(TargetConnectionString)
                    cmdStartTrace.CommandType = CommandType.StoredProcedure
                    cmdStartTrace.Parameters.Add("@OutputFolder", SqlDbType.NVarChar, 256).Direction = ParameterDirection.Input
                    cmdStartTrace.Parameters("@OutputFolder").Value = ThePlan(0).OutputFolder
                    cmdStartTrace.Parameters.Add("@MinutesToTrace", SqlDbType.TinyInt).Direction = ParameterDirection.Input
                    cmdStartTrace.Parameters("@MinutesToTrace").Value = ThePlan(0).MinutesToTrace
                    cmdStartTrace.Parameters.Add("@MinDurationMillisec", SqlDbType.BigInt).Direction = ParameterDirection.Input
                    cmdStartTrace.Parameters("@MinDurationMillisec").Value = ThePlan(0).MinDurationMilliSec
                    cmdStartTrace.Parameters.Add("@ApplicationExcludeFilter", SqlDbType.NVarChar, 1024).Direction = ParameterDirection.Input
                    cmdStartTrace.Parameters("@ApplicationExcludeFilter").Value = ThePlan(0).ApplicationExcludeFilter
                    cmdStartTrace.Parameters.Add("@ApplicationIncludeFilter", SqlDbType.NVarChar, 1024).Direction = ParameterDirection.Input
                    cmdStartTrace.Parameters("@ApplicationIncludeFilter").Value = ThePlan(0).ApplicationIncludeFilter
                    cmdStartTrace.Parameters.Add("@TextIncludeFilter", SqlDbType.NVarChar, 1024).Direction = ParameterDirection.Input
                    cmdStartTrace.Parameters("@TextIncludeFilter").Value = ThePlan(0).TextIncludeFilter
                    cmdStartTrace.Parameters.Add("@DBIDIncludeFilter", SqlDbType.Int).Direction = ParameterDirection.Input
                    cmdStartTrace.Parameters("@DBIDIncludeFilter").Value = ThePlan(0).DBIDIncludeFilter
                    cmdStartTrace.Parameters.Add("@DBNameIncludeFilter", SqlDbType.NVarChar, 128).Direction = ParameterDirection.Input
                    cmdStartTrace.Parameters("@DBNameIncludeFilter").Value = ThePlan(0).DBNameIncludeFilter
                    cmdStartTrace.Parameters.Add("@MinReadPages", SqlDbType.BigInt).Direction = ParameterDirection.Input
                    cmdStartTrace.Parameters("@MinReadPages").Value = ThePlan(0).MinReadPages
                    cmdStartTrace.Parameters.Add("@MaxFileSizeMB", SqlDbType.BigInt).Direction = ParameterDirection.Input
                    cmdStartTrace.Parameters("@MaxFileSizeMB").Value = ThePlan(0).MaxFileSizeMB
                    cmdStartTrace.Parameters.Add("@AllowFileRollover", SqlDbType.Bit).Direction = ParameterDirection.Input
                    cmdStartTrace.Parameters("@AllowFileRollover").Value = ThePlan(0).AllowFileRollover
                    cmdStartTrace.Parameters.Add("@IncludeBatches", SqlDbType.Bit).Direction = ParameterDirection.Input
                    cmdStartTrace.Parameters("@IncludeBatches").Value = ThePlan(0).IncludeBatches
                    cmdStartTrace.Parameters.Add("@IncludeBatchStarts", SqlDbType.Bit).Direction = ParameterDirection.Input
                    cmdStartTrace.Parameters("@IncludeBatchStarts").Value = ThePlan(0).IncludeBatchStarts
                    cmdStartTrace.Parameters.Add("@IncludeRecompiles", SqlDbType.Bit).Direction = ParameterDirection.Input
                    cmdStartTrace.Parameters("@IncludeRecompiles").Value = ThePlan(0).IncludeRecompiles
                    cmdStartTrace.Parameters.Add("@IncludeObjectsCreated", SqlDbType.Bit).Direction = ParameterDirection.Input
                    cmdStartTrace.Parameters("@IncludeObjectsCreated").Value = ThePlan(0).IncludeObjectsCreated
                    cmdStartTrace.Parameters.Add("@IncludeScans", SqlDbType.Bit).Direction = ParameterDirection.Input
                    cmdStartTrace.Parameters("@IncludeScans").Value = ThePlan(0).IncludeScans
                    cmdStartTrace.Parameters.Add("@IncludeAutoStats", SqlDbType.Bit).Direction = ParameterDirection.Input
                    cmdStartTrace.Parameters("@IncludeAutoStats").Value = ThePlan(0).IncludeAutoStats
                    cmdStartTrace.Parameters.Add("@IncludeAttentions", SqlDbType.Bit).Direction = ParameterDirection.Input
                    cmdStartTrace.Parameters("@IncludeAttentions").Value = ThePlan(0).IncludeAttentions
                    cmdStartTrace.Parameters.Add("@IncludeWarnings", SqlDbType.Bit).Direction = ParameterDirection.Input
                    cmdStartTrace.Parameters("@IncludeWarnings").Value = ThePlan(0).IncludeWarnings
                    cmdStartTrace.Parameters.Add("@IncludeErrors", SqlDbType.Bit).Direction = ParameterDirection.Input
                    cmdStartTrace.Parameters("@IncludeErrors").Value = ThePlan(0).IncludeErrors
                    cmdStartTrace.Parameters.Add("@IncludeAutoGrow", SqlDbType.Bit).Direction = ParameterDirection.Input
                    cmdStartTrace.Parameters("@IncludeAutoGrow").Value = ThePlan(0).IncludeAutoGrow
                    cmdStartTrace.Parameters.Add("@IncludeStatements", SqlDbType.Bit).Direction = ParameterDirection.Input
                    cmdStartTrace.Parameters("@IncludeStatements").Value = ThePlan(0).IncludeStatements
                    cmdStartTrace.Parameters.Add("@IncludeStatementStarts", SqlDbType.Bit).Direction = ParameterDirection.Input
                    cmdStartTrace.Parameters("@IncludeStatementStarts").Value = ThePlan(0).IncludeStatementStarts
                    cmdStartTrace.Parameters.Add("@IncludeShowplanAll", SqlDbType.Bit).Direction = ParameterDirection.Input
                    cmdStartTrace.Parameters("@IncludeShowplanAll").Value = ThePlan(0).IncludeShowplanAll
                    cmdStartTrace.Parameters.Add("@IncludeSystemObjects", SqlDbType.Bit).Direction = ParameterDirection.Input
                    cmdStartTrace.Parameters("@IncludeSystemObjects").Value = ThePlan(0).IncludeSystemObjects
                    Dim rdrStartTrace As SqlDataReader = cmdStartTrace.ExecuteReader()
                    rdrStartTrace.Read()
                    TraceName = rdrStartTrace.Item("TraceName").ToString
                    If Not IsDBNull(rdrStartTrace.Item("TraceStopDt")) Then
                        TraceStopDt = CDate(rdrStartTrace.Item("TraceStopDt"))
                    End If
                    If Not IsDBNull(rdrStartTrace.Item("TraceStopUTCDt")) Then
                        TraceStopUTCDt = CDate(rdrStartTrace.Item("TraceStopUTCDt"))
                    End If
                    If Not IsDBNull(rdrStartTrace.Item("TraceStatus")) Then
                        TraceStatus = CInt(rdrStartTrace.Item("TraceStatus"))
                    End If
                    If Not IsDBNull(rdrStartTrace.Item("StatusMessage")) Then
                        StatusMessage = rdrStartTrace.Item("StatusMessage").ToString
                    End If
                End Using
            End Using
        Catch ex As Exception
            Throw New Exception(String.Format("(cTrace.StartTraceOnTarget) Failed to start Trace {0} on [{1}]", TraceName, sSQLServer), ex)
        End Try
    End Sub

    Public Sub LogTrace(ByVal PlanId As Integer)
        Try
            ' log trace execution
            Using cnQueryBaselines As New System.Data.SqlClient.SqlConnection(sBaselineConnectionString)
                cnQueryBaselines.Open()
                Using cmdLogTrace As New System.Data.SqlClient.SqlCommand
                    cmdLogTrace.Connection = cnQueryBaselines
                    cmdLogTrace.CommandText = "QueryBaseline.pWriteLog"
                    cmdLogTrace.CommandType = CommandType.StoredProcedure
                    cmdLogTrace.Parameters.Add("@PlanId", SqlDbType.Int).Direction = ParameterDirection.Input
                    cmdLogTrace.Parameters("@PlanId").Value = PlanId
                    cmdLogTrace.Parameters.Add("@SQLInstance", SqlDbType.NVarChar, 128).Direction = ParameterDirection.Input
                    cmdLogTrace.Parameters("@SQLInstance").Value = ThePlan(0).SQLInstance
                    cmdLogTrace.Parameters.Add("@TemplateName", SqlDbType.NVarChar, 128).Direction = ParameterDirection.Input
                    cmdLogTrace.Parameters("@TemplateName").Value = ThePlan(0).TemplateName
                    cmdLogTrace.Parameters.Add("@OutputFolder", SqlDbType.NVarChar, 256).Direction = ParameterDirection.Input
                    cmdLogTrace.Parameters("@OutputFolder").Value = ThePlan(0).OutputFolder
                    cmdLogTrace.Parameters.Add("@OutputFolderHostReference", SqlDbType.NVarChar, 256).Direction = ParameterDirection.Input
                    cmdLogTrace.Parameters("@OutputFolderHostReference").Value = ThePlan(0).OutputFolderHostReference
                    cmdLogTrace.Parameters.Add("@MinutesToTrace", SqlDbType.TinyInt).Direction = ParameterDirection.Input
                    cmdLogTrace.Parameters("@MinutesToTrace").Value = ThePlan(0).MinutesToTrace
                    cmdLogTrace.Parameters.Add("@MinDurationMillisec", SqlDbType.BigInt).Direction = ParameterDirection.Input
                    cmdLogTrace.Parameters("@MinDurationMillisec").Value = ThePlan(0).MinDurationMilliSec
                    cmdLogTrace.Parameters.Add("@ApplicationExcludeFilter", SqlDbType.NVarChar, 1024).Direction = ParameterDirection.Input
                    cmdLogTrace.Parameters("@ApplicationExcludeFilter").Value = ThePlan(0).ApplicationExcludeFilter
                    cmdLogTrace.Parameters.Add("@ApplicationIncludeFilter", SqlDbType.NVarChar, 1024).Direction = ParameterDirection.Input
                    cmdLogTrace.Parameters("@ApplicationIncludeFilter").Value = ThePlan(0).ApplicationIncludeFilter
                    cmdLogTrace.Parameters.Add("@TextIncludeFilter", SqlDbType.NVarChar, 1024).Direction = ParameterDirection.Input
                    cmdLogTrace.Parameters("@TextIncludeFilter").Value = ThePlan(0).TextIncludeFilter
                    cmdLogTrace.Parameters.Add("@DBIDIncludeFilter", SqlDbType.Int).Direction = ParameterDirection.Input
                    cmdLogTrace.Parameters("@DBIDIncludeFilter").Value = ThePlan(0).DBIDIncludeFilter
                    cmdLogTrace.Parameters.Add("@DBNameIncludeFilter", SqlDbType.NVarChar, 128).Direction = ParameterDirection.Input
                    cmdLogTrace.Parameters("@DBNameIncludeFilter").Value = ThePlan(0).DBNameIncludeFilter
                    cmdLogTrace.Parameters.Add("@MinReadPages", SqlDbType.BigInt).Direction = ParameterDirection.Input
                    cmdLogTrace.Parameters("@MinReadPages").Value = ThePlan(0).MinReadPages
                    cmdLogTrace.Parameters.Add("@MaxFileSizeMB", SqlDbType.BigInt).Direction = ParameterDirection.Input
                    cmdLogTrace.Parameters("@MaxFileSizeMB").Value = ThePlan(0).MaxFileSizeMB
                    cmdLogTrace.Parameters.Add("@AllowFileRollover", SqlDbType.Bit).Direction = ParameterDirection.Input
                    cmdLogTrace.Parameters("@AllowFileRollover").Value = ThePlan(0).AllowFileRollover
                    cmdLogTrace.Parameters.Add("@IncludeBatches", SqlDbType.Bit).Direction = ParameterDirection.Input
                    cmdLogTrace.Parameters("@IncludeBatches").Value = ThePlan(0).IncludeBatches
                    cmdLogTrace.Parameters.Add("@IncludeBatchStarts", SqlDbType.Bit).Direction = ParameterDirection.Input
                    cmdLogTrace.Parameters("@IncludeBatchStarts").Value = ThePlan(0).IncludeBatchStarts
                    cmdLogTrace.Parameters.Add("@IncludeRecompiles", SqlDbType.Bit).Direction = ParameterDirection.Input
                    cmdLogTrace.Parameters("@IncludeRecompiles").Value = ThePlan(0).IncludeRecompiles
                    cmdLogTrace.Parameters.Add("@IncludeObjectsCreated", SqlDbType.Bit).Direction = ParameterDirection.Input
                    cmdLogTrace.Parameters("@IncludeObjectsCreated").Value = ThePlan(0).IncludeObjectsCreated
                    cmdLogTrace.Parameters.Add("@IncludeScans", SqlDbType.Bit).Direction = ParameterDirection.Input
                    cmdLogTrace.Parameters("@IncludeScans").Value = ThePlan(0).IncludeScans
                    cmdLogTrace.Parameters.Add("@IncludeAutoStats", SqlDbType.Bit).Direction = ParameterDirection.Input
                    cmdLogTrace.Parameters("@IncludeAutoStats").Value = ThePlan(0).IncludeAutoStats
                    cmdLogTrace.Parameters.Add("@IncludeAttentions", SqlDbType.Bit).Direction = ParameterDirection.Input
                    cmdLogTrace.Parameters("@IncludeAttentions").Value = ThePlan(0).IncludeAttentions
                    cmdLogTrace.Parameters.Add("@IncludeWarnings", SqlDbType.Bit).Direction = ParameterDirection.Input
                    cmdLogTrace.Parameters("@IncludeWarnings").Value = ThePlan(0).IncludeWarnings
                    cmdLogTrace.Parameters.Add("@IncludeErrors", SqlDbType.Bit).Direction = ParameterDirection.Input
                    cmdLogTrace.Parameters("@IncludeErrors").Value = ThePlan(0).IncludeErrors
                    cmdLogTrace.Parameters.Add("@IncludeAutoGrow", SqlDbType.Bit).Direction = ParameterDirection.Input
                    cmdLogTrace.Parameters("@IncludeAutoGrow").Value = ThePlan(0).IncludeAutoGrow
                    cmdLogTrace.Parameters.Add("@IncludeStatements", SqlDbType.Bit).Direction = ParameterDirection.Input
                    cmdLogTrace.Parameters("@IncludeStatements").Value = ThePlan(0).IncludeStatements
                    cmdLogTrace.Parameters.Add("@IncludeStatementStarts", SqlDbType.Bit).Direction = ParameterDirection.Input
                    cmdLogTrace.Parameters("@IncludeStatementStarts").Value = ThePlan(0).IncludeStatementStarts
                    cmdLogTrace.Parameters.Add("@IncludeShowplanAll", SqlDbType.Bit).Direction = ParameterDirection.Input
                    cmdLogTrace.Parameters("@IncludeShowplanAll").Value = ThePlan(0).IncludeShowplanAll
                    cmdLogTrace.Parameters.Add("@IncludeSystemObjects", SqlDbType.Bit).Direction = ParameterDirection.Input
                    cmdLogTrace.Parameters("@IncludeSystemObjects").Value = ThePlan(0).IncludeSystemObjects
                    cmdLogTrace.Parameters.Add("@TraceName", SqlDbType.NVarChar, 128).Direction = ParameterDirection.Input
                    cmdLogTrace.Parameters("@TraceName").Value = TraceName
                    cmdLogTrace.Parameters.Add("@TraceStopDt", SqlDbType.DateTime).Direction = ParameterDirection.Input
                    cmdLogTrace.Parameters("@TraceStopDt").Value = TraceStopDt
                    cmdLogTrace.Parameters.Add("@TraceStopUTCDt", SqlDbType.DateTime).Direction = ParameterDirection.Input
                    cmdLogTrace.Parameters("@TraceStopUTCDt").Value = TraceStopUTCDt
                    cmdLogTrace.Parameters.Add("@ReturnCode", SqlDbType.Int).Direction = ParameterDirection.Input
                    cmdLogTrace.Parameters("@ReturnCode").Value = TraceStatus
                    cmdLogTrace.Parameters.Add("@StatusMessage", SqlDbType.NVarChar, 128).Direction = ParameterDirection.Input
                    cmdLogTrace.Parameters("@StatusMessage").Value = StatusMessage
                    cmdLogTrace.ExecuteNonQuery()
                End Using
            End Using
        Catch ex As Exception
            Throw New Exception(String.Format("(cTrace.LogTrace) Exception while writing log record for Trace {0}", TraceName), ex)
        End Try
    End Sub

    Public Sub LoadQueue()
        Me.DeleteArchiveData = From daq In Baseline.tDeleteArchiveQueues _
                            Select daq
    End Sub

    Public Sub SaveQueue()
        Baseline.SubmitChanges()
    End Sub

    Public Sub LoadPlans()
        Me.PlanData = From p In Baseline.tPlans _
                      Select p
    End Sub

    Public Sub SavePlans()
        Baseline.SubmitChanges()
    End Sub

    Public Sub LoadApplicationsToExcludeFromArchive()

        ApplicationsToExcludeData = From ax In Baseline.tApplicationsToExcludeFromArchives _
                                    Select ax

    End Sub

    Public Sub SaveApplicationsToExcludeFromArchive()
        Baseline.SubmitChanges()
    End Sub

    Public Sub LoadGroupDataToExcludeFromArchive()
        GroupDataToExcludeData = From gx In Baseline.tGroupDataToExcludeFromArchives _
                                 Select gx
    End Sub

    Public Sub SaveGroupDataToExcludeFromArchive()
        Baseline.SubmitChanges()
    End Sub

    Public Sub LoadTemplates()
        TemplateData = From t In Baseline.tTemplates _
                       Select t
    End Sub

    Public Sub SaveTemplates()
        Baseline.SubmitChanges()
    End Sub

End Class
