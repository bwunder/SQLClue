' decoration prevents breakpoints but allows backgroundworker error handling to work
'<System.Diagnostics.DebuggerNonUserCodeAttribute()> _
Public Class cDbDocumentor

    Public Event Status(ByVal Message As String)

    Private _oSQLServer As Server
    Private _oDatabase As Database
    Private _script As New StringCollection
    Private _oDropScripter As Scripter
    Private _oScripter As Scripter
    Private _bIncludeDrop As Boolean

    Public _BatchSeparator As String

    ' the parameters define the starting place in the hierarchy
    ' only useful when no comparison is necessary
    Friend Function ScriptItem(ByVal smoSQLServer As Server, _
                               ByVal DatabaseName As String, _
                               ByVal CollectionName As String, _
                               ByVal ItemName As String, _
                               ByVal SchemaName As String, _
                               ByVal _ScriptingOptions As ScriptingOptions, _
                               ByVal bIncludeDrop As Boolean, _
                               ByVal bIncludeIfExistsWithDrop As Boolean, _
                               ByVal bBatchSeparator As String, _
                               Optional ByVal EventDataCommandText As String = Nothing) As StringCollection
        Dim item As String
        If DatabaseName = "" Then
            If CollectionName = "" Then
                item = smoSQLServer.ConnectionContext.TrueName & My.Resources.NodePathDelimiter & ItemName
            Else
                item = smoSQLServer.ConnectionContext.TrueName & My.Resources.NodePathDelimiter & CollectionName _
                     & My.Resources.NodePathDelimiter & ItemName
            End If
        Else
            If CollectionName = "" Then
                item = smoSQLServer.ConnectionContext.TrueName & My.Resources.NodePathDelimiter _
                     & smoSQLServer.Databases.ToString & My.Resources.NodePathDelimiter & DatabaseName _
                     & My.Resources.NodePathDelimiter & ItemName
            Else
                If SchemaName = "" Then
                    item = smoSQLServer.ConnectionContext.TrueName & My.Resources.NodePathDelimiter _
                         & smoSQLServer.Databases.ToString & My.Resources.NodePathDelimiter & DatabaseName _
                         & My.Resources.NodePathDelimiter & CollectionName & My.Resources.NodePathDelimiter & ItemName
                Else
                    item = smoSQLServer.ConnectionContext.TrueName & My.Resources.NodePathDelimiter _
                         & smoSQLServer.Databases.ToString & My.Resources.NodePathDelimiter & DatabaseName _
                         & My.Resources.NodePathDelimiter & CollectionName & My.Resources.NodePathDelimiter & SchemaName & "." & ItemName
                End If
            End If
        End If
        Try
            RaiseEvent Status(String.Format(My.Resources.ItemScriptingStartMessage, item))
            _oSQLServer = smoSQLServer
            _bIncludeDrop = bIncludeDrop
            _BatchSeparator = bBatchSeparator
            _oSQLServer.ConnectionContext.Connect()
            _oScripter = New Scripter(_oSQLServer)
            _oScripter.Options = _ScriptingOptions
            ' do not script to a newer server version
            Select Case _oSQLServer.VersionMajor
                Case Is < 8
                    _oScripter.Options.TargetServerVersion = SqlServerVersion.Version80
                Case 8
                    _oScripter.Options.TargetServerVersion = SqlServerVersion.Version80
                Case 9
                    _oScripter.Options.TargetServerVersion = SqlServerVersion.Version90
                Case 10
                    _oScripter.Options.TargetServerVersion = SqlServerVersion.Version100
                Case Else
                    _oScripter.Options.TargetServerVersion = SqlServerVersion.Version100
            End Select
            ' trying to support versions that will script drops and 2005 that does not
            If _bIncludeDrop And Not (_oScripter.Options.ScriptDrops) Then
                _oDropScripter = New Scripter(_oSQLServer)
                _oDropScripter.Options.ContinueScriptingOnError = _oScripter.Options.ContinueScriptingOnError
                _oDropScripter.Options.Encoding = _oScripter.Options.Encoding
                _oDropScripter.Options.EnforceScriptingOptions = _oScripter.Options.EnforceScriptingOptions
                _oDropScripter.Options.IncludeDatabaseContext = _oScripter.Options.IncludeDatabaseContext
                _oDropScripter.Options.SchemaQualify = _oScripter.Options.SchemaQualify
                _oDropScripter.Options.TargetServerVersion = _oScripter.Options.TargetServerVersion
                _oDropScripter.Options.ScriptDrops = bIncludeDrop
                _oDropScripter.Options.IncludeIfNotExists = bIncludeIfExistsWithDrop
            End If
            _script.Clear()
            ' script it
            If DatabaseName = "" Then
                With _oSQLServer
                    If CollectionName <> "" Then
                        Select Case CollectionName
                            Case My.Resources.JobServer
                                ScriptSrvJsAlert(.JobServer.Alerts(ItemName))
                            Case My.Resources.Audits
                                ScriptSrvAudit(.Audits(ItemName))
                            Case My.Resources.BackupDevices
                                ScriptSrvBackupDevice(.BackupDevices(ItemName))
                            Case My.Resources.Credentials
                                DocumentSrvCredential(.Credentials(ItemName))
                            Case My.Resources.CryptographicProviders
                                ScriptSrvCryptographicProvider(.CryptographicProviders(ItemName))
                            Case My.Resources.Databases
                                _oDatabase = .Databases(ItemName)
                                'If ItemName = My.Resources.ActiveDirectory Then
                                '    DocumentDbActiveDirectory()
                                'Else
                                ScriptDatabase(.Databases(ItemName))
                                'End If
                            Case My.Resources.Endpoints
                                ScriptSrvEndpoint(.Endpoints(ItemName))
                            Case My.Resources.Jobs
                                ScriptSrvJsJob(.JobServer.Jobs(ItemName))
                            Case My.Resources.Operators
                                ScriptSrvJsOperator(.JobServer.Operators(ItemName))
                            Case My.Resources.ProxyAccounts
                                ScriptSrvJsProxyAccount(.JobServer.ProxyAccounts(ItemName))
                            Case My.Resources.LinkedServers
                                ScriptSrvLinkedServer(.LinkedServers(ItemName))
                            Case My.Resources.Logins
                                ScriptSrvLogin(.Logins(ItemName))
                            Case My.Resources.Roles
                                ScriptSrvRole(.Roles(ItemName))
                            Case My.Resources.ServerAuditSpecifications
                                ScriptSrvAuditSpecification(.ServerAuditSpecifications(ItemName))
                            Case My.Resources.Triggers
                                ScriptSrvTrigger(.Triggers(ItemName))
                            Case My.Resources.UserDefinedMessages
                                ScriptSrvUserMessage(.UserDefinedMessages(CType(Mid(ItemName, 1, InStr(ItemName, ":") - 1), Int32), _
                                                     Replace(Mid(ItemName, InStr(ItemName, ":") + 1), "'", "")))
                        End Select
                    Else
                        Select Case ItemName
                            Case _oSQLServer.ConnectionContext.TrueName
                                DocumentSrvServiceMasterKey()
                                DocumentSrvAttributes()
                                DocumentSrvProperties()
                                DocumentSrvPolicies()
                                DocumentSrvEventNotifications()
                                ScriptSrvStartupProcedures()
                            Case My.Resources.AlertSystem
                                DocumentSrvAlertSystem()
                                'Case My.Resources.ActiveDirectory
                                '    DocumentSrvActiveDirectory()
                            Case My.Resources.Configuration
                                DocumentSrvConfiguration()
                            Case My.Resources.FullTextService
                                DocumentSrvFullTextService()
                            Case My.Resources.Information
                                DocumentSrvInformation()
                            Case My.Resources.JobServer
                                DocumentSrvJobServer()
                            Case My.Resources.Mail
                                ScriptSrvMail()
                            Case My.Resources.ProxyAccount
                                DocumentSrvProxyAccount()
                            Case My.Resources.ResourceGovernor
                                ScriptSrvResourceGovernor()
                            Case My.Resources.Settings
                                DocumentSrvSettings()
                            Case My.Resources.TargetServers
                                DocumentSrvMsxTargetServers()
                        End Select
                    End If
                End With
            Else
                If (smoSQLServer.Databases(DatabaseName).IsAccessible) Then
                    _oDatabase = _oSQLServer.Databases(DatabaseName)
                    With _oDatabase
                        If CollectionName <> "" Then
                            Select Case CollectionName
                                Case My.Resources.ApplicationRoles
                                    ScriptDbApplicationRole(.ApplicationRoles(ItemName))
                                Case My.Resources.Assemblies
                                    ScriptDbAssembly(.Assemblies(ItemName))
                                Case My.Resources.Priorities
                                    ScriptDbBrokerPriority(.ServiceBroker.Priorities(ItemName))
                                Case My.Resources.AsymmetricKeys
                                    DocumentDbAsymmetricKey(.AsymmetricKeys(ItemName))
                                Case My.Resources.DatabaseAuditSpecifications
                                    ScriptDbAuditSpecification(.DatabaseAuditSpecifications(ItemName))
                                Case My.Resources.Certificates
                                    Try
                                        DocumentDbCertificate(.Certificates.Item(ItemName))
                                    Catch ex As Exception
                                        If Not ex.GetBaseException.Message = My.Resources.AllowedCertificateScriptingException Then
                                            Throw ex
                                        End If
                                    End Try
                                    'may be roles.databaseroles confusion
                                Case My.Resources.Roles
                                    ScriptDbRole(.Roles(ItemName))
                                Case My.Resources.Defaults
                                    ScriptDbDefault(.Defaults(ItemName))
                                Case My.Resources.FullTextCatalogs
                                    ScriptDbFullTextCatalog(.FullTextCatalogs(ItemName))
                                Case My.Resources.FullTextStopLists
                                    ScriptDbFullTextStopList(.FullTextStopLists(ItemName))
                                Case My.Resources.MessageTypes
                                    ScriptDbSbMessageType(.ServiceBroker.MessageTypes(ItemName))
                                Case My.Resources.PartitionFunctions
                                    ScriptDbPartitionFunction(.PartitionFunctions(ItemName))
                                Case My.Resources.PartitionSchemes
                                    ScriptDbPartitionScheme(.PartitionSchemes(ItemName))
                                Case My.Resources.PlanGuides
                                    ScriptDbPlanGuide(.PlanGuides(ItemName))
                                Case My.Resources.Rules
                                    ScriptDbRule(.Rules(ItemName))
                                Case My.Resources.Schemas
                                    ScriptDbSchema(.Schemas(ItemName))
                                Case My.Resources.Services
                                    ScriptDbSbService(.ServiceBroker.Services(ItemName))
                                Case My.Resources.ServiceContracts
                                    ScriptDbSbContract(.ServiceBroker.ServiceContracts(ItemName))
                                Case My.Resources.Queues
                                    ScriptDbSbQueue(.ServiceBroker.Queues(ItemName))
                                Case My.Resources.Routes
                                    ScriptDbSbRoute(.ServiceBroker.Routes(ItemName))
                                Case My.Resources.RemoteServiceBindings
                                    ScriptDbSbRemoteServiceBinding(.ServiceBroker.RemoteServiceBindings(ItemName))
                                Case My.Resources.StoredProcedures
                                    _oSQLServer.SetDefaultInitFields(GetType(StoredProcedure), My.Resources.SMOIsSystemObject)
                                    ScriptDbProcedure(.StoredProcedures(ItemName, SchemaName))
                                Case My.Resources.Synonyms
                                    _oSQLServer.SetDefaultInitFields(GetType(StoredProcedure), My.Resources.SMOIsSystemObject)
                                    ScriptDbProcedure(.StoredProcedures(ItemName, SchemaName))
                                Case My.Resources.SymmetricKeys
                                    DocumentDbSymmetricKey(.SymmetricKeys(ItemName))
                                Case My.Resources.Tables
                                    _oSQLServer.SetDefaultInitFields(GetType(Table), My.Resources.SMOIsSystemObject)
                                    ScriptDbTable(.Tables(ItemName, SchemaName))
                                Case My.Resources.Triggers
                                    ScriptDbDDLTrigger(.Triggers(ItemName))
                                Case My.Resources.UserDefinedAggregates
                                    ScriptDbUserAggregate(.UserDefinedAggregates(ItemName, SchemaName))
                                Case My.Resources.UserDefinedDataTypes
                                    ScriptDbUserDataType(.UserDefinedDataTypes(ItemName))
                                Case My.Resources.UserDefinedFunctions
                                    _oSQLServer.SetDefaultInitFields(GetType(UserDefinedFunction), My.Resources.SMOIsSystemObject)
                                    ScriptDbUserFunction(.UserDefinedFunctions(ItemName, SchemaName))
                                Case My.Resources.UserDefinedTableTypes
                                    ScriptDbUserTableType(.UserDefinedTableTypes(ItemName))
                                Case My.Resources.UserDefinedTypes
                                    ScriptDbUserType(.UserDefinedTypes(ItemName))
                                Case My.Resources.Users
                                    ScriptDbUser(.Users(ItemName))
                                Case My.Resources.Views
                                    _oSQLServer.SetDefaultInitFields(GetType(View), My.Resources.SMOIsSystemObject)
                                    ScriptDbView(.Views(ItemName, SchemaName))
                                Case My.Resources.XMLSchemaCollections
                                    ScriptDbXMLSchemaCollection(.XmlSchemaCollections(ItemName, SchemaName))
                            End Select
                        Else
                            Select Case ItemName
                                'Case My.Resources.ActiveDirectory
                                '    DocumentDbActiveDirectory()
                                Case My.Resources.DatabaseOptions
                                    DocumentDBOptions()
                                Case My.Resources.ServiceBroker
                                    DocumentDbServiceBroker()
                            End Select
                        End If
                    End With
                End If
            End If
            RaiseEvent Status(String.Format(My.Resources.ItemScriptingCompletionMessage, item))
            Return _script
        Catch ex As System.Exception
            ' if we fail here it is fatal
            Throw New Exception(String.Format("ScriptItem [{0}].", item), ex)
        Finally
            _oSQLServer.ConnectionContext.Disconnect()
        End Try
    End Function

#Region " Database Documentation "

    'Private Sub DocumentDbActiveDirectory()
    '    ' DatabaseActiveDirectory is obsolete
    '    If Not _oDatabase.ActiveDirectory Is Nothing Then
    '        For Each prop As Microsoft.SqlServer.Management.Smo.Property In _oDatabase.ActiveDirectory.Properties
    '            If prop.Value Is Nothing Then
    '                _script.Add(String.Format("{0} =", prop.Name))
    '            Else
    '                _script.Add(String.Format("{0} = {1}", prop.Name, prop.Value))
    '            End If
    '        Next
    '    End If
    'End Sub

    Private Sub DocumentDbAsymmetricKey(ByVal oAsymmetricKey As AsymmetricKey)
        If Not oAsymmetricKey Is Nothing Then
            For Each prop As Microsoft.SqlServer.Management.Smo.Property In oAsymmetricKey.Properties
                If prop.Value Is Nothing Then
                    _script.Add(String.Format(" {0} = {1}", prop.Name, ""))
                ElseIf TypeOf prop.Value Is Byte() Then
                    _script.Add(String.Format(" {0} = {1}", _
                                              prop.Name, _
                                              bRayToHexStr(CType(prop.Value, Byte()))))
                Else
                    _script.Add(String.Format(" {0} = {1}", prop.Name, prop.Value))
                End If
            Next
        End If
    End Sub

    Private Sub DocumentDbCertificate(ByVal oCertificate As Certificate)

        'oCertificate.Export(pubkeypath, privkeypath, filephrase, encryptionphrase)

        If Not oCertificate Is Nothing Then
            For Each prop As Microsoft.SqlServer.Management.Smo.Property In oCertificate.Properties
                If prop.Value Is Nothing Then
                    _script.Add(String.Format(" {0} = {1}", prop.Name, ""))
                ElseIf TypeOf prop.Value Is Byte() Then
                    _script.Add(String.Format(" {0} = {1}", _
                                              prop.Name, _
                                              bRayToHexStr(CType(prop.Value, Byte()))))
                Else
                    _script.Add(String.Format(" {0} = {1}", prop.Name, prop.Value))
                End If
            Next
        End If
    End Sub

    Private Sub DocumentDbEventNotifications()
        ' so no good way to enum in the tree so just put them all in one one place
        ' in fact not sure how I will get this event notifications in the tree at all
        ' so will add to server script, just as db events will be added to db script
        If _oSQLServer.VersionMajor > 8 And _oDatabase.CompatibilityLevel > CompatibilityLevel.Version70 Then
            Dim sen As DataSet = _oSQLServer.Databases(_oDatabase.Name).ExecuteWithResults( _
                                                                String.Format(My.Resources.sEventNotificationsQueryTemplate, _
                                                                              My.Resources.DatabaseEventNotifciationsCatalog))
            If (sen.Tables(0).Rows.Count > 0) Then
                _script.Add(String.Format(My.Resources.CommentBegin, My.Resources.EventNotifications))
                For Each dr As DataRow In sen.Tables(0).Rows
                    For i As Int32 = 0 To sen.Tables(0).Columns.Count - 1
                        _script.Add(String.Format("{0} = {1}", sen.Tables(0).Columns(i).ColumnName, dr(i)))
                    Next
                    _script.Add("")
                Next
                _script.Add(String.Format(My.Resources.CommentEnd, My.Resources.EventNotifications))
            End If
        End If
    End Sub

    Private Sub DocumentDBOptions()
        For Each prop As [Property] In _oDatabase.DatabaseOptions.Properties
            If Not prop.Name = "ActiveConnections" _
            And Not prop.Name = "DataSpaceUsage" _
            And Not prop.Name = "DboLogin" _
            And Not prop.Name = "IndexSpaceUsage" _
            And Not InStr(prop.Name, "IsDb") = 1 _
            And Not InStr(prop.Name, "Last") = 1 _
            And Not prop.Name = "Size" _
            And Not prop.Name = "SpaceAvailable" _
            And Not prop.Name = "LogReuseWaitStatus" _
            And Not prop.Name = "MirroringFailoverLogSequenceNumber" Then
                _script.Add(String.Format(" {0} = {1}", prop.Name, prop.Value))
            End If
        Next
    End Sub

    Private Sub DocumentDbMasterKey()
        ' currently no code path to get here...
        ' add a a table to capture an encrypted password and path for service master 
        ' and then do it to it need same thing from db master key
        ' also need to provide instructions  for securing  the folder
        ' put database master keys in same location
        If _oDatabase.MasterKey Is Nothing Then
            _script.Add(String.Format("CreateDate {0}", _oDatabase.MasterKey.CreateDate))
            _script.Add(String.Format("DateLastModified {0}", _oDatabase.MasterKey.DateLastModified))
            _script.Add(String.Format("IsEncryptedByServer {0}", _oDatabase.MasterKey.IsEncryptedByServer))
            _script.Add(String.Format("State {0}", _oDatabase.MasterKey.State))
        End If
    End Sub

    Private Sub DocumentDbServiceBroker()
        If Not _oDatabase.ServiceBroker Is Nothing Then
            _script.Add(String.Format("{0} = {1}", "ServiceBroker GUID", _oDatabase.ServiceBrokerGuid))
            _script.Add(String.Format("{0} = {1}", "State", _oDatabase.ServiceBroker.State))
            _script.Add(String.Format("{0} = {1}", "MessageTypes.Count", _oDatabase.ServiceBroker.MessageTypes.Count))
            If _oSQLServer.VersionMajor > 9 Then
                _script.Add(String.Format("{0} = {1}", "Priorities.Count", _oDatabase.ServiceBroker.Priorities.Count))
            End If
            _script.Add(String.Format("{0} = {1}", "Queues.Count", _oDatabase.ServiceBroker.Queues.Count))
            _script.Add(String.Format("{0} = {1}", "RemoteServiceBindings.Count", _oDatabase.ServiceBroker.RemoteServiceBindings.Count))
            _script.Add(String.Format("{0} = {1}", "Routes.Count", _oDatabase.ServiceBroker.Routes.Count))
            _script.Add(String.Format("{0} = {1}", "ServiceContracts.Count", _oDatabase.ServiceBroker.ServiceContracts.Count))
            _script.Add(String.Format("{0} = {1}", "Services.Count", _oDatabase.ServiceBroker.Services.Count))
        End If
    End Sub

    Private Sub DocumentDbSymmetricKey(ByVal oSymmetricKey As SymmetricKey)
        If Not oSymmetricKey Is Nothing Then
            For Each prop As Microsoft.SqlServer.Management.Smo.Property In oSymmetricKey.Properties
                If prop.Value Is Nothing Then
                    _script.Add(String.Format(" {0} = {1}", prop.Name, ""))
                ElseIf TypeOf prop.Value Is Byte() Then
                    _script.Add(String.Format(" {0} = {1}", _
                                              prop.Name, _
                                              bRayToHexStr(CType(prop.Value, Byte()))))
                Else
                    _script.Add(String.Format(" {0} = {1}", prop.Name, prop.Value))
                End If
            Next
        End If
    End Sub

#End Region

#Region " Server Documentation "

    Private Sub DocumentSrvJobServer()
        ' no events for any of this except server event notifications
        Try
            If Not _oSQLServer.JobServer Is Nothing Then
                For Each prop As Microsoft.SqlServer.Management.Smo.Property In _oSQLServer.JobServer.Properties
                    If prop.Value Is Nothing Then
                        _script.Add(String.Format("{0} = {1)", prop.Name, ""))
                    Else
                        _script.Add(String.Format("{0} = {1}", prop.Name, prop.Value))
                    End If
                Next
            End If
        Catch ex As Exception
            If TryCast(ex.InnerException, SqlException).Number = 15281 Then
                _script.Add(ex.InnerException.Message)
            End If
        End Try
    End Sub

    Private Sub DocumentSrvAlertSystem()
        ' no events for any of this except server event notifications
        If Not _oSQLServer.JobServer.AlertSystem Is Nothing Then
            For Each prop As Microsoft.SqlServer.Management.Smo.Property In _oSQLServer.JobServer.AlertSystem.Properties
                If prop.Value Is Nothing Then
                    _script.Add(String.Format("{0} =", prop.Name))
                Else
                    _script.Add(String.Format("{0} = {1}", prop.Name, prop.Value))
                End If
            Next
        End If
    End Sub

    'Private Sub DocumentSrvActiveDirectory()
    '    If Not _oSQLServer.ActiveDirectory Is Nothing Then
    '        For Each prop As Microsoft.SqlServer.Management.Smo.Property In _oSQLServer.ActiveDirectory.Properties
    '            If prop.Value Is Nothing Then
    '                _script.Add(String.Format("{0} =", prop.Name))
    '            Else
    '                _script.Add(String.Format("{0} = {1}", prop.Name, prop.Value))
    '            End If
    '        Next
    '    End If
    'End Sub

    Private Sub DocumentSrvAttributes()
        ' will write startup procs to same record so may as well make it a usable script
        _script.Add(String.Format(My.Resources.CommentBegin, My.Resources.Attributes))
        Dim dt As DataTable = _oSQLServer.EnumServerAttributes()
        If dt.Rows.Count > 0 Then
            For Each dr As DataRow In dt.Rows
                _script.Add(String.Format("{0} - {1} = {2}", dr.Item(0).ToString, dr.Item(1).ToString, dr.Item(2).ToString))
            Next
        End If
        _script.Add(String.Format(My.Resources.CommentEnd, My.Resources.Attributes))
    End Sub

    Private Sub DocumentSrvConfiguration()
        If Not _oSQLServer.Configuration Is Nothing Then
            For Each prop As Microsoft.SqlServer.Management.Smo.ConfigProperty In _oSQLServer.Configuration.Properties
                _script.Add(String.Format("{0} = {1}", _
                                                   prop.DisplayName, prop.ConfigValue))
            Next
        End If
    End Sub

    Private Sub DocumentSrvCredential(ByVal oCredential As Credential)
        ' scripter won't script, is it because of secret? seems kinds stupid because it scripts munged sql logins
        If Not oCredential Is Nothing Then
            _script.Add(String.Format("{0} = {1}", "Name", oCredential.Name))
            _script.Add(String.Format("{0} = {1}", "Identity", oCredential.Identity))
            For Each prop As [Property] In oCredential.Properties
                _script.Add(String.Format("{0} = {1}", _
                                                   prop.Name, prop.Value))
            Next
        End If
    End Sub

    Private Sub DocumentSrvEventNotifications()
        ' so no good way to enum in the tree so just put them all in one one place
        ' in fact not sure how I will get this event notifications in the tree at all
        ' so will add to server script, just as db events will be added to db script
        If _oSQLServer.VersionMajor > 8 Then
            Dim sen As DataSet = _oSQLServer.Databases("master").ExecuteWithResults( _
                                                    String.Format(My.Resources.sEventNotificationsQueryTemplate, _
                                                                  My.Resources.ServerEventNotificationsCatalog))
            If (sen.Tables(0).Rows.Count > 0) Then
                _script.Add(String.Format(My.Resources.CommentBegin, My.Resources.EventNotifications))
                For Each dr As DataRow In sen.Tables(0).Rows
                    For i As Int32 = 0 To sen.Tables(0).Columns.Count - 1
                        _script.Add(String.Format("{0} = {1}", sen.Tables(0).Columns(i).ColumnName, dr(i).ToString))
                    Next
                Next
                _script.Add(String.Format(My.Resources.CommentEnd, My.Resources.EventNotifications))
            End If
        End If
    End Sub

    Private Sub DocumentSrvFullTextService()
        If Not _oSQLServer.FullTextService Is Nothing Then
            For Each prop As Microsoft.SqlServer.Management.Smo.Property In _oSQLServer.FullTextService.Properties
                If prop.Value Is Nothing Then
                    _script.Add(String.Format("{0} =", prop.Name))
                Else
                    _script.Add(String.Format("{0} = {1}", prop.Name, prop.Value))
                End If
            Next
        End If
    End Sub

    Private Sub DocumentSrvInformation()
        If Not _oSQLServer.Information Is Nothing Then
            For Each prop As Microsoft.SqlServer.Management.Smo.Property In _oSQLServer.Information.Properties
                If prop.Value Is Nothing Then
                    _script.Add(String.Format("{0} = {1}", prop.Name, ""))
                Else
                    _script.Add(String.Format("{0} = {1}", prop.Name, prop.Value))
                End If
            Next
        End If
    End Sub

    Private Sub DocumentSrvMsxTargetServers()
        If _oSQLServer.JobServer.TargetServers.Count > 0 Then
            _script.Add(String.Format(My.Resources.CommentBegin, My.Resources.TargetServers))
            For Each oTargetServer As TargetServer In _oSQLServer.JobServer.TargetServers
                If Not oTargetServer.Properties Is Nothing Then
                    _script.Add(String.Format("Target Server [{0}]", oTargetServer.Name))
                    For Each prop As Microsoft.SqlServer.Management.Smo.Property In oTargetServer.Properties
                        If prop.Value Is Nothing Then
                            _script.Add(String.Format("{0} = {1}", prop.Name, ""))
                        Else
                            _script.Add(String.Format("{0} = {1}", prop.Name, prop.Value))
                        End If
                    Next
                    _script.Add("")
                End If
            Next
            _script.Add(String.Format(My.Resources.CommentEnd, My.Resources.TargetServers))
        End If
    End Sub

    Private Sub DocumentSrvOleDBProviderSettings()
        For Each s As OleDbProviderSettings In _oSQLServer.OleDbProviderSettings
            For Each prop As [Property] In s.Properties
                If prop.Value Is Nothing Then
                    _script.Add(String.Format("{0} = {1}", prop.Name, ""))
                Else
                    _script.Add(String.Format("{0} = {1}", prop.Name, prop.Value))
                End If
            Next
            _script.Add("")
        Next
    End Sub

    Private Sub DocumentSrvServiceMasterKey()

        ' _oSQLServer.ServiceMasterKey.Export(Path, password)

        If _oSQLServer.VersionMajor > 8 _
            AndAlso Not _oSQLServer.ServiceMasterKey Is Nothing Then
            _script.Add(String.Format(My.Resources.CommentBegin, My.Resources.ServiceMasterKey))
            _script.Add(String.Format("State = {0}", _oSQLServer.ServiceMasterKey.State))
            ' looks like there may never be properties
            If _oSQLServer.ServiceMasterKey.State = SqlSmoState.Existing Then
                For Each prop As [Property] In _oSQLServer.ServiceMasterKey.Properties
                    If prop.Value Is Nothing Then
                        _script.Add(String.Format("{0} = {1}", prop.Name, ""))
                    Else
                        _script.Add(String.Format("{0} = {1}", prop.Name, prop.Value))
                    End If
                Next
            End If
            _script.Add(String.Format(My.Resources.CommentEnd, My.Resources.ServiceMasterKey))
        End If
    End Sub

    Private Sub DocumentSrvPolicies()
        If Not _oSQLServer Is Nothing _
        AndAlso _oSQLServer.VersionMajor >= 10 Then
            Dim sc As New SqlStoreConnection(_oSQLServer.ConnectionContext.SqlConnectionObject)
            Dim ps As New PolicyStore(sc)
            If ps.Policies.Count > 0 Then
                _script.Add(String.Format(My.Resources.CommentBegin, My.Resources.Policies))
                For Each pol As Dmf.Policy In ps.Policies
                    _script.Add(String.Format("Dmf.Policy: {0}", pol.Name))
                    _script.Add(pol.ScriptCreate.ToString.Replace(", @", vbCrLf & vbTab & ", @"))
                Next
                For Each cd As Dmf.Condition In ps.Conditions
                    _script.Add(String.Format("Dmf.Condition: {0}", cd.Name))
                    _script.Add(cd.ScriptCreate.ToString.Replace(", @", vbCrLf & vbTab & ", @"))
                Next
                _script.Add(String.Format(My.Resources.CommentEnd, My.Resources.Policies))
            End If
        End If
    End Sub

    Private Sub DocumentSrvProperties()
        ' no events for any of this except server event notifications
        If Not _oSQLServer Is Nothing Then
            _script.Add(String.Format(My.Resources.CommentBegin, My.Resources.Properties))
            For Each prop As [Property] In _oSQLServer.Properties
                If prop.Value Is Nothing Then
                    _script.Add(String.Format("{0} = {1}", prop.Name, ""))
                Else
                    _script.Add(String.Format("{0} = {1}", prop.Name, prop.Value))
                End If
            Next
            _script.Add(String.Format(My.Resources.CommentEnd, My.Resources.Properties))
        End If
    End Sub

    Private Sub DocumentSrvProxyAccount()
        If Not _oSQLServer.ProxyAccount Is Nothing Then
            For Each prop As [Property] In _oSQLServer.ProxyAccount.Properties
                If prop.Value Is Nothing Then
                    _script.Add(String.Format("{0} = {1}", prop.Name, ""))
                Else
                    _script.Add(String.Format("{0} = {1}", prop.Name, prop.Value))
                End If
            Next
        End If
    End Sub

    Private Sub DocumentSrvSettings()
        If Not _oSQLServer.Settings Is Nothing Then
            For Each prop As Microsoft.SqlServer.Management.Smo.Property In _oSQLServer.Settings.Properties
                If prop.Value Is Nothing Then
                    _script.Add(String.Format("{0} = {1}", prop.Name, ""))
                Else
                    _script.Add(String.Format("{0} = {1}", prop.Name, prop.Value))
                End If
            Next
        End If
    End Sub

#End Region

#Region " Database Scripting "

    Private Sub ScriptDatabase(ByVal oDatabase As Database)
        Dim SaveWithDependencies As Boolean = _oScripter.Options.WithDependencies
        Try
            If Not _oDatabase Is Nothing Then
                _oScripter.Options.WithDependencies = False
                ScriptDrop(oDatabase.Urn)
                Try
                    ScriptObject(oDatabase.Urn)
                Catch ex As Exception
                    _script.Add(String.Format(My.Resources.CommentBegin, My.Resources.Properties))
                    If ex.GetBaseException.Message = "To accomplish this action, set property EncryptionAlgorithm." Then
                        _script.Add(My.Resources.SMOBadState)
                        'put the properties in the file instead
                        For Each prop As Microsoft.SqlServer.Management.Smo.Property In _oDatabase.Properties
                            Try
                                If prop.Value Is Nothing Then
                                    _script.Add(String.Format("{0} = {1}", prop.Name, ""))
                                Else
                                    _script.Add(String.Format("{0} = {1}", prop.Name, prop.Value))
                                End If
                            Catch ex1 As Exception
                                _script.Add(String.Format("{0} = {1}", prop.Name, ex1.GetBaseException.Message))
                            End Try
                        Next
                    ElseIf Not oDatabase.IsAccessible Then
                        _script.Add(String.Format(My.Resources.DBOffline, oDatabase.Name, _oSQLServer.ConnectionContext.TrueName))
                        _script.Add(ex.ToString)
                        Exit Sub
                    Else
                        Throw ex
                    End If
                    _script.Add(String.Format(My.Resources.CommentEnd, My.Resources.Properties))
                End Try
                If _oSQLServer.VersionMajor > 8 Then
                    If Not (oDatabase.MasterKey Is Nothing) Then
                        _script.Add(String.Format(My.Resources.CommentBegin, My.Resources.DatabaseMasterKey))
                        With oDatabase.MasterKey
                            _script.Add(" Create Date = " & .CreateDate.ToString)
                            _script.Add(" Date Last Modified = " & .DateLastModified.ToString)
                            _script.Add(" Is Encrypted By Server = " & .IsEncryptedByServer.ToString)
                        End With
                        _script.Add(String.Format(My.Resources.CommentEnd, My.Resources.DatabaseMasterKey))
                    End If
                    DocumentDbEventNotifications()
                End If
            End If
        Catch ex As Exception
            Throw ex
        Finally
            _oScripter.Options.WithDependencies = SaveWithDependencies
        End Try
    End Sub

    Private Sub ScriptDbAssembly(ByVal oAssembly As SqlAssembly)
        Dim SaveNoAssemblies As Boolean = _oScripter.Options.NoAssemblies
        Try
            If Not oAssembly Is Nothing Then
                _oScripter.Options.NoAssemblies = False
                ScriptDrop(oAssembly.Urn)
                ScriptObject(oAssembly.Urn)
            End If
        Catch ex As Exception
            Throw ex
        Finally
            _oScripter.Options.NoAssemblies = SaveNoAssemblies
        End Try
    End Sub

    Private Sub ScriptDbAuditSpecification(ByVal oAuditSpecification As AuditSpecification)
        Dim SaveWithDependencies As Boolean = _oScripter.Options.WithDependencies
        Try
            If Not oAuditSpecification Is Nothing Then
                _oScripter.Options.WithDependencies = False
                ' doesn't script drop correctly using scripter object 
                ScriptDrop(oAuditSpecification.Urn)
                ScriptObject(oAuditSpecification.Urn)
            End If
        Catch ex As Exception
            Throw ex
        Finally
            _oScripter.Options.WithDependencies = SaveWithDependencies
        End Try
    End Sub

    Private Sub ScriptDbBrokerPriority(ByVal oPriority As BrokerPriority)
        Dim SaveWithDependencies As Boolean = _oScripter.Options.WithDependencies
        Try
            If Not oPriority Is Nothing Then
                _oScripter.Options.WithDependencies = False
                ' doesn't script drop correctly using scripter object 
                ScriptDrop(oPriority.Urn)
                ScriptObject(oPriority.Urn)
            End If
        Catch ex As Exception
            Throw ex
        Finally
            _oScripter.Options.WithDependencies = SaveWithDependencies
        End Try
    End Sub

    Private Sub ScriptDbUserDataType(ByVal oUserDefinedDataType As UserDefinedDataType)
        If Not oUserDefinedDataType Is Nothing Then
            ScriptDrop(oUserDefinedDataType.Urn)
            ScriptObject(oUserDefinedDataType.Urn)
        End If
    End Sub

    Private Sub ScriptDbDefault(ByVal oDefault As [Default])
        If Not oDefault Is Nothing Then
            ScriptDrop(oDefault.Urn)
            ScriptObject(oDefault.Urn)
        End If
    End Sub

    Private Sub ScriptDbFullTextCatalog(ByVal oCatalog As FullTextCatalog)
        Dim SaveWithDependencies As Boolean = _oScripter.Options.WithDependencies
        Try
            If Not oCatalog Is Nothing Then
                _oScripter.Options.WithDependencies = False
                ScriptDrop(oCatalog.Urn)
                ScriptObject(oCatalog.Urn)
            End If
        Catch ex As Exception
            Throw ex
        Finally
            _oScripter.Options.WithDependencies = SaveWithDependencies
        End Try
    End Sub

    Private Sub ScriptDbFullTextStopList(ByVal oFullTextStopList As FullTextStopList)
        Dim SaveWithDependencies As Boolean = _oScripter.Options.WithDependencies
        Try
            If Not oFullTextStopList Is Nothing Then
                _oScripter.Options.WithDependencies = False
                ScriptDrop(oFullTextStopList.Urn)
                ScriptObject(oFullTextStopList.Urn)
            End If
        Catch ex As Exception
            Throw ex
        Finally
            _oScripter.Options.WithDependencies = SaveWithDependencies
        End Try
    End Sub

    Private Sub ScriptDbPartitionFunction(ByVal oFunction As PartitionFunction)
        If Not oFunction Is Nothing Then
            ScriptDrop(oFunction.Urn)
            ScriptObject(oFunction.Urn)
        End If
    End Sub

    Private Sub ScriptDbPartitionScheme(ByVal oScheme As PartitionScheme)
        If Not oScheme Is Nothing Then
            ScriptDrop(oScheme.Urn)
            ScriptObject(oScheme.Urn)
        End If
    End Sub

    Private Sub ScriptDbPlanGuide(ByVal oPlanGuide As PlanGuide)
        If Not oPlanGuide Is Nothing Then
            ScriptDrop(oPlanGuide.Urn)
            ScriptObject(oPlanGuide.Urn)
        End If
    End Sub

    Private Sub ScriptDbProcedure(ByVal oStoredProcedure As StoredProcedure)
        Dim SavePrimaryObject As Boolean = _oScripter.Options.PrimaryObject
        Try
            If Not oStoredProcedure Is Nothing Then
                If oStoredProcedure.IsSystemObject Or oStoredProcedure.Schema = "sys" Then
                    _oScripter.Options.PrimaryObject = False
                End If
                ' filter out the crap that is system but not marked as system
                If Not (oStoredProcedure.Schema = "dbo" _
                       And (oStoredProcedure.Name = "dt_addtosourcecontrol" _
                            Or oStoredProcedure.Name = "dt_addtosourcecontrol_u" _
                            Or oStoredProcedure.Name = "dt_adduserobject" _
                            Or oStoredProcedure.Name = "dt_adduserobject_vcs" _
                            Or oStoredProcedure.Name = "dt_checkinobject" _
                            Or oStoredProcedure.Name = "dt_checkinobject_u" _
                            Or oStoredProcedure.Name = "dt_checkoutobject" _
                            Or oStoredProcedure.Name = "dt_checkoutobject_u" _
                            Or oStoredProcedure.Name = "dt_displayoaerror" _
                            Or oStoredProcedure.Name = "dt_displayoaerror_u" _
                            Or oStoredProcedure.Name = "dt_droppropertiesbyid" _
                            Or oStoredProcedure.Name = "dt_dropuserobjectbyid" _
                            Or oStoredProcedure.Name = "dt_generateansiname" _
                            Or oStoredProcedure.Name = "dt_getobjwithprop" _
                            Or oStoredProcedure.Name = "dt_getobjwithprop_u" _
                            Or oStoredProcedure.Name = "dt_getpropertiesbyid" _
                            Or oStoredProcedure.Name = "dt_getpropertiesbyid_u" _
                            Or oStoredProcedure.Name = "dt_getpropertiesbyid_vcs" _
                            Or oStoredProcedure.Name = "dt_getpropertiesbyid_vcs_u" _
                            Or oStoredProcedure.Name = "dt_isundersourcecontrol" _
                            Or oStoredProcedure.Name = "dt_isundersourcecontrol_u" _
                            Or oStoredProcedure.Name = "dt_removefromsourcecontrol" _
                            Or oStoredProcedure.Name = "dt_setpropertybyid" _
                            Or oStoredProcedure.Name = "dt_setpropertybyid_u" _
                            Or oStoredProcedure.Name = "dt_validateloginparams" _
                            Or oStoredProcedure.Name = "dt_validateloginparams_u" _
                            Or oStoredProcedure.Name = "dt_vcsenabled" _
                            Or oStoredProcedure.Name = "dt_verstamp006" _
                            Or oStoredProcedure.Name = "dt_whocheckedout" _
                            Or oStoredProcedure.Name = "dt_whocheckedout_u" _
                            Or oStoredProcedure.Name = "sp_alterdiagram" _
                            Or oStoredProcedure.Name = "sp_creatediagram" _
                            Or oStoredProcedure.Name = "sp_dropdiagram" _
                            Or oStoredProcedure.Name = "sp_helpdiagramdefinition" _
                            Or oStoredProcedure.Name = "sp_helpdiagrams" _
                            Or oStoredProcedure.Name = "sp_renamediagram" _
                            Or oStoredProcedure.Name = "sp_upgraddiagrams")) _
                And Not oStoredProcedure.Schema = "sys" Then
                    ScriptDrop(oStoredProcedure.Urn)
                    ScriptObject(oStoredProcedure.Urn)
                End If
            End If
        Catch ex As Exception
            Throw ex
        Finally
            _oScripter.Options.PrimaryObject = SavePrimaryObject
        End Try
    End Sub

    Private Sub ScriptDbRole(ByVal oRole As DatabaseRole)
        Dim SaveWithDependencies As Boolean = _oScripter.Options.WithDependencies
        Dim SavePrimaryObject As Boolean = _oScripter.Options.PrimaryObject
        Try
            If Not oRole Is Nothing Then
                If oRole.IsFixedRole Then
                    _oScripter.Options.PrimaryObject = False
                End If
                _oScripter.Options.WithDependencies = False
                If Not (oRole.IsFixedRole) And Not (oRole.Name = "Public") Then
                    If Not (oRole.Name = "public") Then
                        ScriptDrop(oRole.Urn)
                        ScriptObject(oRole.Urn)
                    End If
                End If
                If _oSQLServer.VersionMajor = 8 Then
                    ScriptObject(oRole.Urn)
                Else
                    If oRole.EnumMembers.Count > 0 Then
                        Try
                            For Each mem As String In oRole.EnumMembers
                                _script.Add("Exec sp_addrolemember @rolename = '" & oRole.Name & "', @membername = '" & mem & "'" & _
                                        If(_oScripter.Options.NoCommandTerminator, "", ";"))

                                If _oScripter.Options.ScriptBatchTerminator Then
                                    _script.Add("GO")
                                End If
                            Next
                        Catch ex As Exception
                            _script.Add("(ScriptDbRole) Exception while enumerating members.")
                            Throw ex
                        End Try
                    End If
                    If _oScripter.Options.Permissions = True Then
                        If _oDatabase.EnumObjectPermissions(oRole.Name).Length > 0 Then
                            Try
                                For Each perm As ObjectPermissionInfo In _oDatabase.EnumObjectPermissions(oRole.Name)
                                    If perm.PermissionState.ToString = "GrantWithGrant" Then
                                        _script.Add("GRANT " & perm.PermissionType.ToString & _
                                                    " on [" & perm.ObjectSchema & _
                                                    "].[" & perm.ObjectName & "] to [" & perm.Grantee & "] WITH GRANT" & _
                                        If(_oScripter.Options.NoCommandTerminator, "", ";"))

                                        If _oScripter.Options.ScriptBatchTerminator Then
                                            _script.Add("GO")
                                        End If
                                    Else
                                        _script.Add(perm.PermissionState.ToString & Space(1) & _
                                                    perm.PermissionType.ToString & " on [" & _
                                                    perm.ObjectSchema & "].[" & perm.ObjectName & "] to [" & _
                                                    perm.Grantee & "]" & _
                                        If(_oScripter.Options.NoCommandTerminator, "", ";"))

                                        If _oScripter.Options.ScriptBatchTerminator Then
                                            _script.Add("GO")
                                        End If
                                    End If
                                Next
                            Catch ex As Exception
                                _script.Add("(ScriptDbRole) Exception while enumerating object permissions.")
                                Throw ex
                            End Try
                        End If
                        If _oDatabase.EnumDatabasePermissions(oRole.Name).Length > 0 Then
                            Try
                                For Each perm As DatabasePermissionInfo In _oDatabase.EnumDatabasePermissions(oRole.Name)
                                    If perm.PermissionState.ToString = "GrantWithGrant" Then
                                        _script.Add("GRANT " & perm.PermissionType.ToString & _
                                                    " TO [" & perm.Grantee & "] WITH GRANT" & _
                                        If(_oScripter.Options.NoCommandTerminator, "", ";"))

                                        If _oScripter.Options.ScriptBatchTerminator Then
                                            _script.Add("GO")
                                        End If
                                    Else
                                        _script.Add(perm.PermissionState.ToString & Space(1) & _
                                                    perm.PermissionType.ToString & _
                                                    " TO [" & perm.Grantee & "]" & _
                                        If(_oScripter.Options.NoCommandTerminator, "", ";"))

                                        If _oScripter.Options.ScriptBatchTerminator Then
                                            _script.Add("GO")
                                        End If
                                    End If
                                Next
                            Catch ex As Exception
                                _script.Add("(ScriptDbRole) Exception while enumerating database permissions.")
                                Throw ex
                            End Try
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            Throw ex
        Finally
            _oScripter.Options.PrimaryObject = SavePrimaryObject
            _oScripter.Options.WithDependencies = SaveWithDependencies
        End Try
    End Sub

    Private Sub ScriptDbApplicationRole(ByVal oRole As ApplicationRole)
        Dim SaveWithDependencies As Boolean = _oScripter.Options.WithDependencies
        Try
            If Not oRole Is Nothing Then
                _oScripter.Options.WithDependencies = False
                ScriptDrop(oRole.Urn)
                If _oSQLServer.VersionMajor = 8 Then
                    ScriptObject(oRole.Urn)
                Else
                    Try
                        _script.Add("CREATE APPLICATION ROLE [" & oRole.Name & "]")
                        _script.Add("WITH DEFAULT_SCHEMA = [" & oRole.DefaultSchema & "]")
                        _script.Add(" , PASSWORD = not provided" & _
                                        If(_oScripter.Options.NoCommandTerminator, "", ";"))
                        If _oScripter.Options.ScriptBatchTerminator Then
                            _script.Add("GO")
                        End If
                    Catch ex As Exception
                        _script.Add("(ScriptDbApplicationRole) Exception while scripting role.")
                        Throw ex
                    End Try
                    If _oScripter.Options.Permissions = True Then
                        If _oDatabase.EnumObjectPermissions(oRole.Name).Length > 0 Then
                            Try
                                For Each perm As ObjectPermissionInfo In _oDatabase.EnumObjectPermissions(oRole.Name)
                                    If perm.PermissionState.ToString = "GrantWithGrant" Then
                                        _script.Add("GRANT " & perm.PermissionType.ToString & " on [" _
                                                   & perm.ObjectSchema & "].[" & perm.ObjectName & "] to [" _
                                                   & perm.Grantee & "] WITH GRANT" & _
                                        If(_oScripter.Options.NoCommandTerminator, "", ";"))
                                        If _oScripter.Options.ScriptBatchTerminator Then
                                            _script.Add("GO")
                                        End If
                                    Else
                                        _script.Add(perm.PermissionState.ToString & Space(1) & _
                                                    perm.PermissionType.ToString & " on [" & _
                                                    perm.ObjectSchema & "].[" & perm.ObjectName & "] to [" & _
                                                    perm.Grantee & "]" & _
                                        If(_oScripter.Options.NoCommandTerminator, "", ";"))

                                        If _oScripter.Options.ScriptBatchTerminator Then
                                            _script.Add("GO")
                                        End If
                                    End If
                                Next
                            Catch ex As Exception
                                _script.Add("(ScriptDbApplicationRole) Exception while enumeration object permissions.")
                                Throw ex
                            End Try
                        End If
                        If _oDatabase.EnumDatabasePermissions(oRole.Name).Length > 0 Then
                            Try
                                For Each perm As DatabasePermissionInfo In _oDatabase.EnumDatabasePermissions(oRole.Name)
                                    If perm.PermissionState.ToString = "GrantWithGrant" Then
                                        _script.Add("GRANT " & perm.PermissionType.ToString & _
                                                   " TO [" & perm.Grantee & "] WITH GRANT" & _
                                        If(_oScripter.Options.NoCommandTerminator, "", ";"))

                                        If _oScripter.Options.ScriptBatchTerminator Then
                                            _script.Add("GO")
                                        End If
                                    Else
                                        _script.Add(perm.PermissionState.ToString & Space(1) & _
                                                    perm.PermissionType.ToString & _
                                                    " TO [" & perm.Grantee & "]" & _
                                        If(_oScripter.Options.NoCommandTerminator, "", ";"))

                                        If _oScripter.Options.ScriptBatchTerminator Then
                                            _script.Add("GO")
                                        End If
                                    End If
                                Next
                            Catch ex As Exception
                                _script.Add("(ScriptDbApplicationRole) Exception while enumerating database permissions.")
                                Throw ex
                            End Try
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            Throw ex
        Finally
            _oScripter.Options.WithDependencies = SaveWithDependencies
        End Try
    End Sub

    Private Sub ScriptDbRule(ByVal oRule As Microsoft.SqlServer.Management.Smo.Rule)
        If Not oRule Is Nothing Then
            ScriptDrop(oRule.Urn)
            ScriptObject(oRule.Urn)
        End If
    End Sub

    Private Sub ScriptDbSchema(ByVal oSchema As Schema)
        Dim SaveWithDependencies As Boolean = _oScripter.Options.WithDependencies
        Try
            If Not oSchema Is Nothing Then
                _oScripter.Options.WithDependencies = False
                ' don't even make files for sys and INFORMATION_SCHEMA or if nothing to script
                If Not (oSchema.Name = "sys" OrElse oSchema.Name = "INFORMATION_SCHEMA") Then
                    ScriptDrop(oSchema.Urn)
                    ScriptObject(oSchema.Urn)
                End If
                If _oScripter.Options.Permissions = True Then
                    If _oDatabase.EnumObjectPermissions(oSchema.Name).Length > 0 Then
                        Try
                            For Each perm As ObjectPermissionInfo In _oDatabase.EnumObjectPermissions(oSchema.Name)
                                If perm.PermissionState.ToString = "GrantWithGrant" Then
                                    _script.Add("GRANT " & perm.PermissionType.ToString & " on [" & _
                                               perm.ObjectSchema & "].[" & perm.ObjectName & "] to [" & _
                                               perm.Grantee & "] WITH GRANT" & _
                                    If(_oScripter.Options.NoCommandTerminator, "", ";"))
                                    If _oScripter.Options.ScriptBatchTerminator Then
                                        _script.Add("GO")
                                    End If
                                Else
                                    _script.Add(perm.PermissionState.ToString & Space(1) & _
                                                perm.PermissionType.ToString & " on [" & _
                                                perm.ObjectSchema & "].[" & perm.ObjectName & "] to [" & _
                                                perm.Grantee & "]" & _
                                    If(_oScripter.Options.NoCommandTerminator, "", ";"))

                                    If _oScripter.Options.ScriptBatchTerminator Then
                                        _script.Add("GO")
                                    End If
                                End If
                            Next
                        Catch ex As Exception
                            _script.Add("Exception while enumerating object permissions.")
                            Throw ex
                        End Try
                    End If
                    If _oDatabase.EnumDatabasePermissions(oSchema.Name).Length > 0 Then
                        Try
                            For Each perm As DatabasePermissionInfo In _oDatabase.EnumDatabasePermissions(oSchema.Name)
                                If perm.PermissionState.ToString = "GrantWithGrant" Then
                                    _script.Add("GRANT " & perm.PermissionType.ToString & _
                                                " TO [" & perm.Grantee & "] WITH GRANT" & _
                                    If(_oScripter.Options.NoCommandTerminator, "", ";"))
                                    If _oScripter.Options.ScriptBatchTerminator Then
                                        _script.Add("GO")
                                    End If
                                Else
                                    _script.Add(perm.PermissionState.ToString & Space(1) & _
                                                perm.PermissionType.ToString & _
                                                " TO [" & perm.Grantee & "]" & _
                                    If(_oScripter.Options.NoCommandTerminator, "", ";"))
                                    If _oScripter.Options.ScriptBatchTerminator Then
                                        _script.Add("GO")
                                    End If
                                End If
                            Next
                        Catch ex As Exception
                            _script.Add("Exception while enumerating database permissions.")
                            Throw ex
                        End Try
                    End If
                End If
            End If
        Catch ex As Exception
            Throw ex
        Finally
            _oScripter.Options.WithDependencies = SaveWithDependencies
        End Try
    End Sub

    Private Sub ScriptDbSbContract(ByVal oContract As ServiceContract)
        Dim SaveWithDependencies As Boolean = _oScripter.Options.WithDependencies
        Try
            If Not oContract Is Nothing Then
                _oScripter.Options.WithDependencies = False
                ScriptDrop(oContract.Urn)
                ScriptObject(oContract.Urn)
            End If
        Catch ex As Exception
            Throw ex
        Finally
            _oScripter.Options.WithDependencies = SaveWithDependencies
        End Try
    End Sub

    Private Sub ScriptDbSbMessageType(ByVal oMessageType As MessageType)
        Dim SaveWithDependencies As Boolean = _oScripter.Options.WithDependencies
        Dim SavePrimaryObject As Boolean = _oScripter.Options.PrimaryObject
        Try
            If Not oMessageType Is Nothing Then
                _oScripter.Options.WithDependencies = False
                If oMessageType.IsSystemObject Then
                    _oScripter.Options.PrimaryObject = False
                End If
                ScriptDrop(oMessageType.Urn)
                ScriptObject(oMessageType.Urn)
            End If
        Catch ex As Exception
            Throw ex
        Finally
            _oScripter.Options.WithDependencies = SaveWithDependencies
            _oScripter.Options.PrimaryObject = SavePrimaryObject
        End Try
    End Sub

    Private Sub ScriptDbSbQueue(ByVal oQueue As ServiceQueue)
        Dim SaveWithDependencies As Boolean = _oScripter.Options.WithDependencies
        Try
            If Not oQueue Is Nothing Then
                _oScripter.Options.WithDependencies = False
                ScriptDrop(oQueue.Urn)
                ScriptObject(oQueue.Urn)
            End If
        Catch ex As Exception
            Throw ex
        Finally
            _oScripter.Options.WithDependencies = SaveWithDependencies
        End Try
    End Sub

    Private Sub ScriptDbSbRemoteServiceBinding(ByVal oBinding As RemoteServiceBinding)
        Dim SaveWithDependencies As Boolean = _oScripter.Options.WithDependencies
        Try
            If Not oBinding Is Nothing Then
                _oScripter.Options.WithDependencies = False
                ScriptDrop(oBinding.Urn)
                ScriptObject(oBinding.Urn)
            End If
        Catch ex As Exception
            Throw ex
        Finally
            _oScripter.Options.WithDependencies = SaveWithDependencies
        End Try
    End Sub

    Private Sub ScriptDbSbRoute(ByVal oRoute As ServiceRoute)
        Dim SaveWithDependencies As Boolean = _oScripter.Options.WithDependencies
        Try
            If Not oRoute Is Nothing Then
                _oScripter.Options.WithDependencies = False
                ScriptDrop(oRoute.Urn)
                ScriptObject(oRoute.Urn)
            End If
        Catch ex As Exception
            Throw ex
        Finally
            _oScripter.Options.WithDependencies = SaveWithDependencies
        End Try
    End Sub

    Private Sub ScriptDbSbService(ByVal oService As BrokerService)
        Dim SaveWithDependencies As Boolean = _oScripter.Options.WithDependencies
        Try
            If Not oService Is Nothing Then
                _oScripter.Options.WithDependencies = False
                ScriptDrop(oService.Urn)
                ScriptObject(oService.Urn)
            End If
        Catch ex As Exception
            Throw ex
        Finally
            _oScripter.Options.WithDependencies = SaveWithDependencies
        End Try
    End Sub

    Private Sub ScriptDbSynonym(ByVal oSynonym As Synonym)
        If Not oSynonym Is Nothing Then
            ScriptDrop(oSynonym.Urn, True)
            ScriptObject(oSynonym.Urn)
        End If
    End Sub

    Private Sub ScriptDbTable(ByVal oTable As Table)
        Dim SavePrimaryObject As Boolean = _oScripter.Options.PrimaryObject
        Dim SaveScriptSchema As Boolean = _oScripter.Options.ScriptSchema
        Try
            If Not oTable Is Nothing Then
                'Never need the text of system objects and never want to drop them
                If oTable.IsSystemObject Or oTable.Schema = "sys" Then
                    _oScripter.Options.PrimaryObject = False
                End If
                'If FullTextCatalogs are scripted include the full text indexes here
                If _oScripter.Options.FullTextCatalogs = True AndAlso _oScripter.Options.FullTextIndexes = False Then
                    _oScripter.Options.FullTextIndexes = True
                End If
                ScriptDrop(oTable.Urn)
                ScriptObject(oTable.Urn)
                If _oScripter.Options.ScriptData Then
                    _oScripter.Options.ScriptSchema = False
                    For Each str As String In _oScripter.EnumScript(New Urn(0) {oTable.Urn})
                        _script.Add(str)
                        If _oScripter.Options.ScriptBatchTerminator Then
                            _script.Add("GO")
                        End If
                    Next
                End If
            End If
        Catch ex As Exception
            Throw ex
        Finally
            _oScripter.Options.PrimaryObject = SavePrimaryObject
            _oScripter.Options.ScriptSchema = SaveScriptSchema
        End Try
    End Sub

    Private Sub ScriptDbTrigger(ByVal oTrigger As Trigger)
        If Not oTrigger Is Nothing Then
            ' no replication triggers
            If Not (Left(oTrigger.Name, 5) = "sp_MS") Then
                ScriptDrop(oTrigger.Urn)
                ScriptObject(oTrigger.Urn)
            End If
        End If
    End Sub

    Private Sub ScriptDbDDLTrigger(ByVal oTrigger As DatabaseDdlTrigger)
        Dim SaveWithDependencies As Boolean = _oScripter.Options.WithDependencies
        Dim SavePrimaryObject As Boolean = _oScripter.Options.PrimaryObject
        Try
            If Not oTrigger Is Nothing Then
                _oScripter.Options.WithDependencies = False
                If oTrigger.IsSystemObject Then
                    _oScripter.Options.PrimaryObject = False
                End If
                ScriptDrop(oTrigger.Urn)
                ScriptObject(oTrigger.Urn)
            End If
        Catch ex As Exception
            Throw ex
        Finally
            _oScripter.Options.WithDependencies = SaveWithDependencies
            _oScripter.Options.PrimaryObject = SavePrimaryObject
        End Try
    End Sub

    Private Sub ScriptDbUserAggregate(ByVal oAggregate As UserDefinedAggregate)
        If Not oAggregate Is Nothing Then
            ScriptDrop(oAggregate.Urn)
            ScriptObject(oAggregate.Urn)
        End If
    End Sub

    Private Sub ScriptDbUserFunction(ByVal oFunction As UserDefinedFunction)
        Dim SavePrimaryObject As Boolean = _oScripter.Options.PrimaryObject
        Try
            If Not oFunction Is Nothing Then
                If oFunction.IsSystemObject Or oFunction.Schema = "sys" _
                Or (oFunction.Schema = "dbo" And oFunction.Name = "fn_diagramobjects") Then
                    _oScripter.Options.PrimaryObject = False
                End If
                If Not (oFunction.Schema = "dbo" And oFunction.Name = "fn_diagramobjects") _
                And Not oFunction.Schema = "sys" Then
                    ScriptDrop(oFunction.Urn)
                    ScriptObject(oFunction.Urn)
                End If
            End If
        Catch ex As Exception
            Throw ex
        Finally
            _oScripter.Options.PrimaryObject = SavePrimaryObject
        End Try
    End Sub

    Private Sub ScriptDbUserTableType(ByVal oType As UserDefinedTableType)
        If Not oType Is Nothing Then
            ScriptDrop(oType.Urn)
            ScriptObject(oType.Urn)
        End If
    End Sub

    Private Sub ScriptDbUserType(ByVal oType As UserDefinedType)
        If Not oType Is Nothing Then
            ScriptDrop(oType.Urn)
            ScriptObject(oType.Urn)
        End If
    End Sub

    Private Sub ScriptDbUser(ByVal oUser As User)
        Dim SaveWithDependencies As Boolean = _oScripter.Options.WithDependencies
        Dim SavePrimaryObject As Boolean = _oScripter.Options.PrimaryObject
        Try
            If Not oUser Is Nothing Then
                If oUser.IsSystemObject _
                Or Not (oUser.Name = "sys" Or oUser.Name = "INFORMATION_SCHEMA" Or oUser.Name = "guest") Then
                    _oScripter.Options.PrimaryObject = False
                End If
                _oScripter.Options.WithDependencies = False
                ScriptDrop(oUser.Urn)
                ScriptObject(oUser.Urn)
                If _oScripter.Options.Permissions = True Then
                    If _oDatabase.EnumObjectPermissions(oUser.Name).Length > 0 Then
                        Try
                            For Each perm As ObjectPermissionInfo In _oDatabase.EnumObjectPermissions(oUser.Name)
                                If perm.PermissionState.ToString = "GrantWithGrant" Then
                                    _script.Add("GRANT " & perm.PermissionType.ToString & " on [" & _
                                                perm.ObjectSchema & "].[" & perm.ObjectName & "] to [" & _
                                                perm.Grantee & "] WITH GRANT" & _
                                    If(_oScripter.Options.NoCommandTerminator, "", ";"))
                                    If _oScripter.Options.ScriptBatchTerminator Then
                                        _script.Add("GO")
                                    End If
                                Else
                                    _script.Add(perm.PermissionState.ToString & Space(1) & _
                                                perm.PermissionType.ToString & " on [" & _
                                                perm.ObjectSchema & "].[" & perm.ObjectName & "] to [" & _
                                                perm.Grantee & "]" & _
                                    If(_oScripter.Options.NoCommandTerminator, "", ";"))
                                    If _oScripter.Options.ScriptBatchTerminator Then
                                        _script.Add("GO")
                                    End If
                                End If
                                _script.Add("GO")
                            Next
                        Catch ex As Exception
                            _script.Add("(ScriptDbUser) Exception while enumerating object permissions.")
                            Throw ex
                        End Try
                    End If
                    If _oDatabase.EnumDatabasePermissions(oUser.Name).Length > 0 Then
                        Try
                            For Each perm As DatabasePermissionInfo In _oDatabase.EnumDatabasePermissions(oUser.Name)
                                If perm.PermissionState.ToString = "GrantWithGrant" Then
                                    _script.Add("GRANT " & perm.PermissionType.ToString & _
                                               " TO [" & perm.Grantee & "] WITH GRANT" & _
                                    If(_oScripter.Options.NoCommandTerminator, "", ";"))

                                    If _oScripter.Options.ScriptBatchTerminator Then
                                        _script.Add("GO")
                                    End If
                                Else
                                    _script.Add(perm.PermissionState.ToString & Space(1) & _
                                                perm.PermissionType.ToString & _
                                                " TO [" & perm.Grantee & "]" & _
                                    If(_oScripter.Options.NoCommandTerminator, "", ";"))

                                    If _oScripter.Options.ScriptBatchTerminator Then
                                        _script.Add("GO")
                                    End If
                                End If
                            Next
                        Catch ex As Exception
                            _script.Add("(ScriptDbUser) Exception while enumerating object permissions")
                            Throw ex
                        End Try
                    End If
                End If
            End If
        Catch ex As Exception
            Throw ex
        Finally
            _oScripter.Options.PrimaryObject = SavePrimaryObject
            _oScripter.Options.WithDependencies = SaveWithDependencies
        End Try
    End Sub

    Private Sub ScriptDbView(ByVal oView As View)
        Dim SavePrimaryObject As Boolean = _oScripter.Options.PrimaryObject
        Try
            If Not oView Is Nothing Then
                If oView.IsSystemObject Or oView.Schema = "sys" Then
                    _oScripter.Options.PrimaryObject = False
                End If
                ScriptDrop(oView.Urn)
                ScriptObject(oView.Urn)
            End If
        Catch ex As Exception
            Throw ex
        Finally
            _oScripter.Options.PrimaryObject = SavePrimaryObject
        End Try
    End Sub

    Private Sub ScriptDbXMLSchemaCollection(ByVal oSchemaCollection As XmlSchemaCollection)
        If Not oSchemaCollection Is Nothing Then
            ScriptDrop(oSchemaCollection.Urn)
            ScriptObject(oSchemaCollection.Urn)
        End If
    End Sub

#End Region

#Region " Server Scripting "

    Private Sub ScriptSrvJsAlert(ByVal oAlert As Agent.Alert)
        If Not (_oSQLServer.Information.Edition.ToString = "Express Edition") Then
            Dim SaveWithDependencies As Boolean = _oScripter.Options.WithDependencies
            Dim SaveIncludeIfNotExists As Boolean = _oScripter.Options.IncludeIfNotExists
            Try
                If Not oAlert Is Nothing Then
                    _oScripter.Options.WithDependencies = False
                    ScriptDrop(oAlert.Urn)
                    ' always want the ifnotexists option for the category
                    _oScripter.Options.IncludeIfNotExists = True
                    ScriptObject(_oSQLServer.JobServer.AlertCategories(oAlert.CategoryName).Urn)
                    _oScripter.Options.IncludeIfNotExists = SaveIncludeIfNotExists
                    ScriptObject(oAlert.Urn)
                End If
            Catch ex As Exception
                Throw ex
            Finally
                ' have to do this twice in case it blows up while scripting category
                _oScripter.Options.IncludeIfNotExists = SaveIncludeIfNotExists
                _oScripter.Options.WithDependencies = SaveWithDependencies
            End Try
        Else
            RaiseEvent Status(My.Resources.NoExpressSupport)
        End If
    End Sub

    Private Sub ScriptSrvAudit(ByVal oAudit As Audit)
        Dim SaveWithDependencies As Boolean = _oScripter.Options.WithDependencies
        Try
            If Not oAudit Is Nothing Then
                _oScripter.Options.WithDependencies = False
                ' doesn't script drop correctly using scripter object 
                ScriptDrop(oAudit.Urn)
                ScriptObject(oAudit.Urn)
            End If
        Catch ex As Exception
            Throw ex
        Finally
            _oScripter.Options.WithDependencies = SaveWithDependencies
        End Try
    End Sub

    Private Sub ScriptSrvAuditSpecification(ByVal oAuditSpecification As AuditSpecification)
        Dim SaveWithDependencies As Boolean = _oScripter.Options.WithDependencies
        Try
            If Not oAuditSpecification Is Nothing Then
                _oScripter.Options.WithDependencies = False
                ' doesn't script drop correctly using scripter object 
                ScriptDrop(oAuditSpecification.Urn)
                ScriptObject(oAuditSpecification.Urn)
            End If
        Catch ex As Exception
            Throw ex
        Finally
            _oScripter.Options.WithDependencies = SaveWithDependencies
        End Try
    End Sub

    Private Sub ScriptSrvBackupDevice(ByVal oBackupDevice As BackupDevice)
        Dim SaveWithDependencies As Boolean = _oScripter.Options.WithDependencies
        Try
            If Not oBackupDevice Is Nothing Then
                _oScripter.Options.WithDependencies = False
                ' doesn't script drop correctly using scripter object 
                If _bIncludeDrop Then
                    _script.Add("EXEC master.dbo.sp_dropdevice @logicalname = '" & oBackupDevice.Name & "', @delfile = 'delfile'" & _
                                    If(_oScripter.Options.NoCommandTerminator, "", ";"))
                    If _oScripter.Options.ScriptBatchTerminator Then
                        _script.Add("GO")
                    End If
                End If
                ScriptObject(oBackupDevice.Urn)
            End If
        Catch ex As Exception
            Throw ex
        Finally
            _oScripter.Options.WithDependencies = SaveWithDependencies
        End Try
    End Sub

    Private Sub ScriptSrvCryptographicProvider(ByVal oCryptographicProvider As CryptographicProvider)
        ' no event to detect changes
        Dim SaveWithDependencies As Boolean = _oScripter.Options.WithDependencies
        Try
            _oScripter.Options.WithDependencies = False
            ScriptDrop(oCryptographicProvider.Urn)
            ScriptObject(oCryptographicProvider.Urn)
        Catch ex As Exception
            Throw ex
        Finally
            _oScripter.Options.WithDependencies = SaveWithDependencies
        End Try
    End Sub

    Private Sub ScriptSrvEndpoint(ByVal oEndpoint As Endpoint)
        ' no event to detect alert changes
        Dim SaveWithDependencies As Boolean = _oScripter.Options.WithDependencies
        Dim SavePrimaryObject As Boolean = _oScripter.Options.PrimaryObject
        Try
            If Not oEndpoint Is Nothing Then

                'PERMISSIONS?
                If oEndpoint.IsSystemObject Then
                    _oScripter.Options.PrimaryObject = False
                End If
                _oScripter.Options.WithDependencies = False
                ScriptDrop(oEndpoint.Urn)
                ScriptObject(oEndpoint.Urn)
            End If
        Catch ex As Exception
            Throw ex
        Finally
            _oScripter.Options.PrimaryObject = SavePrimaryObject
            _oScripter.Options.WithDependencies = SaveWithDependencies
        End Try
    End Sub

    Private Sub ScriptSrvJsJob(ByVal oJob As Agent.Job)
        If Not (_oSQLServer.Information.Edition.ToString = "Express Edition") Then
            ' no event to detect job changes
            ' no event to detect alert changes
            Dim SaveWithDependencies As Boolean = _oScripter.Options.WithDependencies
            Try
                If Not oJob Is Nothing Then
                    'unlike alerts and opertors, jobs seems to handle categories in the script
                    _oScripter.Options.WithDependencies = False
                    ScriptDrop(oJob.Urn)
                    ScriptObject(oJob.Urn)
                End If
            Catch ex As Exception
                Throw ex
            Finally
                _oScripter.Options.WithDependencies = SaveWithDependencies
            End Try
        Else
            RaiseEvent Status(My.Resources.NoExpressSupport)
        End If
    End Sub

    Private Sub ScriptSrvLinkedServer(ByVal oLinkedServer As LinkedServer)
        ' no event to detect alert changes
        Dim SaveWithDependencies As Boolean = _oScripter.Options.WithDependencies
        Try
            If Not oLinkedServer Is Nothing Then
                _oScripter.Options.WithDependencies = False
                ScriptDrop(oLinkedServer.Urn)
                ScriptObject(oLinkedServer.Urn)
                If oLinkedServer.LinkedServerLogins.Count > 0 Then
                    For Each login As LinkedServerLogin In oLinkedServer.LinkedServerLogins
                        ScriptObject(login.Urn)
                    Next
                End If
                Try
                    If Not oLinkedServer.ProviderName Is Nothing Then
                        _script.Add(String.Format(My.Resources.CommentBegin, My.Resources.OleDbProviders))
                        _script.Add(String.Format("OLE DB Provider {0} settings", oLinkedServer.ProviderName))
                        For Each prop As [Property] In _oSQLServer.OleDbProviderSettings(oLinkedServer.ProviderName).Properties
                            If prop.Value Is Nothing Then
                                _script.Add(String.Format("{0} = {1}", prop.Name, ""))
                            Else
                                _script.Add(String.Format("{0} = {1}", prop.Name, prop.Value))
                            End If
                        Next
                    End If
                Catch ex As Exception
                    _script.Add(ex.GetBaseException.Message)
                Finally
                    _script.Add(String.Format(My.Resources.CommentEnd, My.Resources.OleDbProviders))
                End Try
            End If
        Catch ex As Exception
            Throw ex
        Finally
            _oScripter.Options.WithDependencies = SaveWithDependencies
        End Try
    End Sub

    Private Sub ScriptSrvLogin(ByVal oLogin As Login)
        ' no event to detect alert changes
        Dim SaveWithDependencies As Boolean = _oScripter.Options.WithDependencies
        Dim SavePrimaryObject As Boolean = _oScripter.Options.PrimaryObject
        Try
            If Not oLogin Is Nothing Then

                If oLogin.IsSystemObject Then
                    _oScripter.Options.PrimaryObject = False
                End If
                _oScripter.Options.WithDependencies = False
                If oLogin.Name = "sa" Then
                    ScriptDrop(oLogin.Urn, True)

                Else
                    ScriptDrop(oLogin.Urn, False)
                End If
                If _oSQLServer.VersionMajor = 8 Then
                    ScriptObject(oLogin.Urn)
                Else
                    ' may need to prevent a run time error if the type does not support lock?
                    If (oLogin.LoginType = LoginType.SqlLogin _
                             AndAlso oLogin.IsLocked = True) Then
                        _script.Add("-- locked out")
                    End If
                    If _oSQLServer.VersionMajor > 8 _
                    AndAlso (oLogin.LoginType = LoginType.SqlLogin _
                             AndAlso oLogin.IsPasswordExpired = True) Then
                        _script.Add("-- password expired")
                    End If
                    If _oScripter.Options.PrimaryObject = True Then

                        _script.Add("CREATE LOGIN [" & oLogin.Name & "]")
                        If oLogin.LoginType = LoginType.WindowsUser Or oLogin.LoginType = LoginType.WindowsGroup Then
                            _script.Add(" FROM WINDOWS")
                        End If
                        If oLogin.LoginType = LoginType.SqlLogin Then
                            _script.Add("WITH PASSWORD = unknown")
                            If oLogin.MustChangePassword = True Then
                                _script.Add(" MUST_CHANGE")
                            End If
                            _script.Add(" , SID = " & bRayToHexStr(oLogin.Sid))
                            Dim sOnOff As String
                            If oLogin.PasswordExpirationEnabled Then
                                sOnOff = "ON"
                            Else
                                sOnOff = "OFF"
                            End If
                            _script.Add(" , CHECK_EXPIRATION = " & sOnOff)
                            If oLogin.PasswordPolicyEnforced Then
                                sOnOff = "ON"
                            Else
                                sOnOff = "OFF"
                            End If
                            _script.Add(" , CHECK_POLICY = " & sOnOff)
                            If Not (oLogin.Credential = "") Then
                                _script.Add(" , CREDENTIAL = " & oLogin.Credential.ToString)
                            End If
                        End If
                        If Not (oLogin.DefaultDatabase Is Nothing) And Not (oLogin.Language Is Nothing) _
                        And (oLogin.LoginType = LoginType.SqlLogin Or oLogin.LoginType = LoginType.WindowsGroup Or oLogin.LoginType = LoginType.WindowsUser) Then
                            Dim Part1 As String
                            If Not (oLogin.LoginType = LoginType.SqlLogin) Then
                                Part1 = "WITH "
                            Else
                                Part1 = " ,"
                            End If
                            If Not (oLogin.DefaultDatabase Is Nothing) Then
                                _script.Add(Part1 & " DEFAULT_DATABASE = " & oLogin.DefaultDatabase)
                                If Not (oLogin.Language Is Nothing) Then
                                    Part1 = " ,"
                                End If
                            End If
                            If Not (oLogin.Language Is Nothing) Then
                                _script.Add(Part1 & " DEFAULT_LANGUAGE = " & oLogin.Language)
                            End If
                        End If
                        If oLogin.LoginType = LoginType.AsymmetricKey Then
                            _script.Add("FROM ASYMMETRIC KEY " & oLogin.AsymmetricKey.ToString & _
                                        If(_oScripter.Options.NoCommandTerminator, "", ";"))
                        End If
                        If oLogin.LoginType = LoginType.Certificate Then
                            _script.Add("FROM CERTIFICATE " & oLogin.Certificate.ToString & _
                                        If(_oScripter.Options.NoCommandTerminator, "", ";"))
                        End If
                        If _oScripter.Options.ScriptBatchTerminator Then
                            _script.Add("GO")
                        End If
                    End If
                    If oLogin.IsDisabled = True Then
                        _script.Add("ALTER LOGIN [" & oLogin.Name & "] DISABLE" & _
                                        If(_oScripter.Options.NoCommandTerminator, "", ";"))
                        If _oScripter.Options.ScriptBatchTerminator Then
                            _script.Add("GO")
                        End If
                    End If
                    If _oScripter.Options.Permissions = True Then
                        If _oSQLServer.EnumServerPermissions(oLogin.Name).Length > 0 Then
                            Try
                                For Each perm As ServerPermissionInfo In _oSQLServer.EnumServerPermissions(oLogin.Name)
                                    'what if multiple permissiontypes?
                                    If perm.PermissionState.ToString = "GrantWithGrant" Then
                                        _script.Add("GRANT " & perm.PermissionType.ToString & _
                                                                 " TO [" & perm.Grantee & "] WITH GRANT" & _
                                        If(_oScripter.Options.NoCommandTerminator, "", ";"))
                                        If _oScripter.Options.ScriptBatchTerminator Then
                                            _script.Add("GO")
                                        End If
                                    Else
                                        _script.Add(perm.PermissionState.ToString & " " & perm.PermissionType.ToString & _
                                        " TO [" & perm.Grantee & "]" & _
                                        If(_oScripter.Options.NoCommandTerminator, "", ";"))
                                    End If
                                    If _oScripter.Options.ScriptBatchTerminator Then
                                        _script.Add("GO")
                                    End If
                                Next
                            Catch ex As Exception
                                _script.Add("(ScriptSrvLogin) Exception while enumerating server permissions")
                                Throw ex
                            End Try
                        End If
                        If _oSQLServer.EnumObjectPermissions(oLogin.Name).Length > 0 Then
                            Try
                                For Each perm As ServerPermissionInfo In _oSQLServer.EnumServerPermissions(oLogin.Name)
                                    'what if multiple permissiontypes?
                                    If perm.PermissionState.ToString = "GrantWithGrant" Then
                                        _script.Add("GRANT " & perm.PermissionType.ToString & " on [" & perm.ObjectSchema & _
                                                             "].[" & perm.ObjectName & "] to [" & perm.Grantee & "] WITH GRANT" & _
                                        If(_oScripter.Options.NoCommandTerminator, "", ";"))

                                        If _oScripter.Options.ScriptBatchTerminator Then
                                            _script.Add("GO")
                                        End If
                                    Else
                                        _script.Add(perm.PermissionState.ToString & Space(1) & perm.PermissionType.ToString & " on [" & _
                                                             perm.ObjectSchema & "].[" & perm.ObjectName & "] to [" & perm.Grantee & "]" & _
                                        If(_oScripter.Options.NoCommandTerminator, "", ";"))
                                    End If
                                    If _oScripter.Options.ScriptBatchTerminator Then
                                        _script.Add("GO")
                                    End If
                                Next
                            Catch ex As Exception
                                _script.Add("(ScriptSrvLogin) Exception while enumerating object permissions")
                                Throw ex
                            End Try
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            Throw ex
        Finally
            _oScripter.Options.PrimaryObject = SavePrimaryObject
            _oScripter.Options.WithDependencies = SaveWithDependencies
        End Try
    End Sub

    Private Sub ScriptSrvMail()
        ' account will need to look in profile collection to define accountprofiles 
        ' no event to detect alert changes
        Dim SaveWithDependencies As Boolean = _oScripter.Options.WithDependencies
        Try
            If Not _oSQLServer.EngineEdition = Edition.Express Then
                _oScripter.Options.WithDependencies = False
                RaiseEvent Status("DatabaseMail")
                RaiseEvent Status("Profiles")
                If Not _oSQLServer.Mail Is Nothing Then
                    For Each prof As MailProfile In _oSQLServer.Mail.Profiles
                        Dim dtPrin As DataTable = prof.EnumPrincipals()
                        ' drop the principal associations for this profile
                        If _bIncludeDrop Then
                            Try
                                For Each dr As DataRow In dtPrin.Rows
                                    'item(0) - PrincipalName
                                    'item(1) - PrincipalID
                                    'item(2) - ProfileName
                                    'item(3) - ProfileID
                                    'item(4) - IsDefault (a string)
                                    If dr.Item(4).ToString = "True" Then
                                        _script.Add("-- CAUTION, this is a default principal")
                                        _script.Add("-- dropping may break existing calls to Database Mail")
                                    End If
                                    _script.Add("IF EXISTS(SELECT *")
                                    _script.Add("          FROM [msdb].[dbo].[sysmail_principalprofile] prin")
                                    _script.Add("          JOIN [msdb].[dbo].[sysmail_profile] prof")
                                    _script.Add("          ON prin.[profile_id] = prof.[profile_id]")
                                    _script.Add("          WHERE prof.[name] = '" & dr.Item(2).ToString & "')")
                                    _script.Add("    EXEC [msdb].[dbo].[sysmail_delete_principalprofile_sp]")
                                    _script.Add("                @principal_name = '" & dr.Item(0).ToString & "'")
                                    _script.Add("              , @profile_name = '" & dr.Item(2).ToString & "'" & _
                                    If(_oScripter.Options.NoCommandTerminator, "", ";"))

                                    If _oScripter.Options.ScriptBatchTerminator Then
                                        _script.Add("GO")
                                    End If
                                Next
                            Catch ex As Exception
                                _script.Add("(ScriptSrvMail) Exception while scripting [msdb.dbo.sysmail_delete_principalprofile_sp]")
                                Throw ex
                            End Try
                            ' drop the account associations with the profile 
                            Try
                                Dim dtAcct As DataTable = prof.EnumAccounts()
                                'item(0) - ProfileName
                                'item(1) - ProfileID
                                'item(2) - AccountName
                                'item(3) - AccountID
                                'item(4) - SequenceNumber
                                For Each dr As DataRow In dtAcct.Rows
                                    _script.Add("IF EXISTS (SELECT * ")
                                    _script.Add("           FROM [msdb].[dbo].[sysmail_profileaccount] prac")
                                    _script.Add("           JOIN [msdb].[dbo].[sysmail_profile] prof")
                                    _script.Add("           ON prac.[profile_id] = prof.[profile_id]")
                                    _script.Add("           JOIN [msdb].[dbo].[sysmail_account] acct")
                                    _script.Add("           ON prac.[account_id] = acct.[account_id]")
                                    _script.Add("           WHERE prof.[name] = '" & dr.Item(0).ToString & "'")
                                    _script.Add("           AND acct.[name] = '" & dr.Item(2).ToString & "')")
                                    _script.Add("    EXEC [msdb].[dbo].[sysmail_delete_profileaccount_sp]")
                                    _script.Add("                @profile_name = '" & dr.Item(0).ToString & "'")
                                    _script.Add("              , @account_name = '" & dr.Item(2).ToString & "'" & _
                                    If(_oScripter.Options.NoCommandTerminator, "", ";"))
                                    If _oScripter.Options.ScriptBatchTerminator Then
                                        _script.Add("GO")
                                    End If
                                Next
                            Catch ex As Exception
                                _script.Add("(ScriptSrvMail) Exception while scripting [msdb.dbo.sysmail_delete_profileaccount_sp]")
                                Throw ex
                            End Try
                        End If
                        ScriptDrop(prof.Urn)
                        ScriptObject(prof.Urn)
                    Next
                    RaiseEvent Status("Accounts")
                    For Each acct As MailAccount In _oSQLServer.Mail.Accounts
                        If _bIncludeDrop Then
                            ' drop the account associations with any profile 
                            Try
                                For Each prof As MailProfile In _oSQLServer.Mail.Profiles
                                    Dim dtAcct As DataTable = prof.EnumAccounts()
                                    For Each dr As DataRow In dtAcct.Rows
                                        If acct.Name = dr.Item(2).ToString Then
                                            _script.Add("IF EXISTS (SELECT * ")
                                            _script.Add("           FROM [msdb].[dbo].[sysmail_profileaccount] prac")
                                            _script.Add("           JOIN [msdb].[dbo].[sysmail_profile] prof")
                                            _script.Add("           ON prac.[profile_id] = prof.[profile_id]")
                                            _script.Add("           JOIN [msdb].[dbo].[sysmail_account acct]")
                                            _script.Add("           ON prac.[account_id] = acct.[account_id]")
                                            _script.Add("           WHERE prof.[name] = '" & dr.Item(0).ToString & "'")
                                            _script.Add("           AND acct.[name] = '" & dr.Item(2).ToString & "')")
                                            _script.Add("    EXEC [msdb].[dbo].[sysmail_delete_profileaccount_sp]")
                                            _script.Add("                @profile_name = '" & dr.Item(0).ToString & "'" & _
                                            If(_oScripter.Options.NoCommandTerminator, "", ";"))

                                            If _oScripter.Options.ScriptBatchTerminator Then
                                                _script.Add("GO")
                                            End If
                                        End If
                                    Next
                                Next
                            Catch ex As Exception
                                _script.Add("(ScriptSrvMail) Exception while scripting [msdb.dbo.sysmail_delete_profileaccount_sp]")
                                Throw ex
                            End Try
                        End If
                        ScriptDrop(acct.Urn)
                        ScriptObject(acct.Urn)
                        Try
                            For Each prof As MailProfile In _oSQLServer.Mail.Profiles
                                Dim dtAcct As DataTable = prof.EnumAccounts()
                                For Each dr As DataRow In dtAcct.Rows
                                    If acct.Name = dr.Item(2).ToString Then
                                        _script.Add("EXEC [msdb].[dbo].[sysmail_add_profileaccount_sp]")
                                        _script.Add("           @profile_name = '" & dr.Item(0).ToString & "'")
                                        _script.Add("         , @account_name = '" & dr.Item(2).ToString & "'")
                                        _script.Add("         , @sequence_number = '" & dr.Item(4).ToString & "'" & _
                                        If(_oScripter.Options.NoCommandTerminator, "", ";"))

                                        If _oScripter.Options.ScriptBatchTerminator Then
                                            _script.Add("GO")
                                        End If
                                    End If
                                Next
                            Next
                        Catch ex As Exception
                            _script.Add("(ScriptSrvMail) Exception while scripting [msdb.dbo.sysmail_add_profileaccount_sp]")
                            Throw ex
                        End Try
                    Next
                End If
            End If
        Catch ex As Exception
            Throw ex
        Finally
            _oScripter.Options.WithDependencies = SaveWithDependencies
        End Try
    End Sub

    Private Sub ScriptSrvJsOperator(ByVal oOperator As Agent.Operator)
        If Not (_oSQLServer.Information.Edition.ToString = "Express Edition") Then
            ' no event to detect alert changes
            Dim SaveWithDependencies As Boolean = _oScripter.Options.WithDependencies
            Dim SaveIncludeIfNotExists As Boolean = _oScripter.Options.IncludeIfNotExists
            Try
                If Not oOperator Is Nothing Then
                    _oScripter.Options.WithDependencies = False
                    ScriptDrop(oOperator.Urn)
                    _oScripter.Options.IncludeIfNotExists = True
                    ScriptObject(_oSQLServer.JobServer.OperatorCategories(oOperator.CategoryName).Urn)
                    _oScripter.Options.IncludeIfNotExists = SaveIncludeIfNotExists
                    ScriptObject(oOperator.Urn)
                End If
            Catch ex As Exception
                Throw ex
            Finally
                ' have to do this twice in case it blows up while scripting the category
                _oScripter.Options.IncludeIfNotExists = SaveIncludeIfNotExists
                _oScripter.Options.WithDependencies = SaveWithDependencies
            End Try
        Else
            RaiseEvent Status(My.Resources.NoExpressSupport)
        End If
    End Sub

    Private Sub ScriptSrvJsProxyAccount(ByVal oProxyAccount As Agent.ProxyAccount)
        If Not (_oSQLServer.Information.Edition = "Express Edition") Then
            Dim SaveWithDependencies As Boolean = _oScripter.Options.WithDependencies
            Try
                If Not oProxyAccount Is Nothing Then
                    _oScripter.Options.WithDependencies = False
                    ScriptDrop(oProxyAccount.Urn)
                    ScriptObject(oProxyAccount.Urn)
                End If
            Catch ex As Exception
                Throw ex
            Finally
                _oScripter.Options.WithDependencies = SaveWithDependencies
            End Try
        Else
            RaiseEvent Status(My.Resources.NoExpressSupport)
        End If
    End Sub

    Private Sub ScriptSrvStartupProcedures()
        Dim save_oDatabase As Database = _oDatabase
        _oDatabase = _oSQLServer.Databases("master")
        Try
            Dim dt As DataTable = _oSQLServer.EnumStartupProcedures()
            If dt.Rows.Count > 0 Then
                _script.Add(String.Format(My.Resources.CommentBegin, My.Resources.StartupProcedures))
                For Each dr As DataRow In dt.Rows
                    ScriptDbProcedure(_oDatabase.StoredProcedures(dr.Item(1).ToString & "." & dr.Item(0).ToString))
                    _script.Add("EXEC sp_procoption '" & dr.Item(1).ToString & "." & dr.Item(0).ToString & "', 'startup', 'true'" & _
                                        If(_oScripter.Options.NoCommandTerminator, "", ";"))
                    If _oScripter.Options.ScriptBatchTerminator Then
                        _script.Add("GO")
                    End If
                Next
                _script.Add(String.Format(My.Resources.CommentEnd, My.Resources.StartupProcedures))
            End If
        Catch ex As Exception
            Throw ex
        Finally
            _oDatabase = save_oDatabase
        End Try
    End Sub

    Private Sub ScriptSrvResourceGovernor()
        Dim SaveWithDependencies As Boolean = _oScripter.Options.WithDependencies
        Try
            If Not _oSQLServer.ResourceGovernor Is Nothing Then
                ' withdependencies invalid here
                _oScripter.Options.WithDependencies = False
                ' drop of Governor and system pools not possible
                For Each pool As ResourcePool In _oSQLServer.ResourceGovernor.ResourcePools
                    If Not pool.IsSystemObject Then
                        ScriptDrop(pool.Urn)
                    End If
                    For Each g As WorkloadGroup In pool.WorkloadGroups
                        If Not g.IsSystemObject Then
                            ScriptDrop(g.Urn)
                        End If
                    Next
                Next
                ScriptObject(_oSQLServer.ResourceGovernor.Urn)
                For Each pool As ResourcePool In _oSQLServer.ResourceGovernor.ResourcePools
                    ScriptObject(pool.Urn)
                    For Each g As WorkloadGroup In pool.WorkloadGroups
                        ScriptObject(g.Urn)
                    Next
                Next
            End If
        Catch ex As Exception
            If ex.GetBaseException.Message.ToString.Contains("Resource Governor is not supported") Then
                _script.Add(ex.GetBaseException.Message.ToString)
            Else
                Throw ex
            End If
        Finally
            _oScripter.Options.WithDependencies = SaveWithDependencies
        End Try
    End Sub

    Private Sub ScriptSrvRole(ByVal oRole As ServerRole)
        ' no event to detect alert changes
        Dim SaveWithDependencies As Boolean = _oScripter.Options.WithDependencies
        Try
            If Not oRole Is Nothing Then
                _oScripter.Options.WithDependencies = False
                If oRole.EnumMemberNames.Count > 0 Then
                    Try
                        For Each mem As String In oRole.EnumMemberNames
                            _script.Add("Exec sp_addsrvrolemember @loginName = '" & mem & "', @rolename = '" & oRole.Name & "'" & _
                                        If(_oScripter.Options.NoCommandTerminator, "", ";"))

                            If _oScripter.Options.ScriptBatchTerminator Then
                                _script.Add("GO")
                            End If
                        Next
                    Catch ex As Exception
                        Throw New Exception(String.Format("(cDBDocumentor.ScriptSrvRole) Exception." & _
                                                                     "SQLInstance: [{0}]; ServerRole: [{1}].", _
                                                                     oRole.Name, _oSQLServer.ConnectionContext.TrueName), ex)
                    End Try
                End If

                ' PERMISSIONS?

            End If
        Catch ex As Exception
            Throw ex
        Finally
            _oScripter.Options.WithDependencies = SaveWithDependencies
        End Try
    End Sub

    Private Sub ScriptSrvTrigger(ByVal oTrigger As ServerDdlTrigger)
        ' no event to detect alert changes
        Dim SaveTriggers As Boolean = _oScripter.Options.Triggers
        Dim SaveWithDependencies As Boolean = _oScripter.Options.WithDependencies
        Dim SavePrimaryObject As Boolean = _oScripter.Options.PrimaryObject
        Try
            If Not oTrigger Is Nothing Then
                If oTrigger.IsSystemObject Then
                    _oScripter.Options.PrimaryObject = False
                End If
                _oScripter.Options.WithDependencies = False
                _oScripter.Options.Triggers = True
                ScriptDrop(oTrigger.Urn)
                ScriptObject(oTrigger.Urn)
            End If
        Catch ex As Exception
            Throw ex
        Finally
            _oScripter.Options.PrimaryObject = SavePrimaryObject
            _oScripter.Options.Triggers = SaveTriggers
            _oScripter.Options.WithDependencies = SaveWithDependencies
        End Try
    End Sub

    Private Sub ScriptSrvUserMessage(ByVal oMessage As UserDefinedMessage)
        Dim SaveWithDependencies As Boolean = _oScripter.Options.WithDependencies
        Try
            If Not oMessage Is Nothing Then
                _oScripter.Options.WithDependencies = False
                ScriptDrop(oMessage.Urn)
                ScriptObject(oMessage.Urn)
            End If
        Catch ex As Exception
            Throw ex
        Finally
            _oScripter.Options.WithDependencies = SaveWithDependencies
        End Try
    End Sub

#End Region

#Region " Scripting  "

    Private Sub ScriptDrop(ByVal oUrn As Urn, _
                           Optional ByVal AsComment As Boolean = False)
        'noop if not 1
        If _bIncludeDrop Then
            Dim ObjectToScript(0) As Urn
            ObjectToScript(0) = oUrn
            Try
                For Each str As String In _oDropScripter.Script(ObjectToScript)
                    If AsComment Then
                        _script.Add("/********** Drop Scripted as comment")
                    End If
                    ' bug in if exist for some object ? causes run on words
                    _script.Add(Replace(Replace(Replace(str, _
                                                           ")EXEC", _
                                                            ")" & vbCrLf & vbTab & "EXEC"), _
                                                    " WHERE ", _
                                                    vbCrLf & Space(12) & "WHERE "), _
                                           " AND ", _
                                        vbCrLf & Space(12) & "AND " & _
                                        If(_oScripter.Options.NoCommandTerminator, "", ";")))
                    If _oScripter.Options.ScriptBatchTerminator Then
                        _script.Add("GO")
                    End If
                    If AsComment Then
                        _script.Add("**********/")
                    End If
                Next
            Catch ex As Exception
                Throw New Exception(String.Format("(DbDocumentor.ScriptDrop) Exception." & _
                                                             "generating script for URN [{0}].", _
                                                             oUrn.ToString), ex)
            End Try
        End If 'noop
    End Sub

    Private Sub ScriptObject(ByVal oUrn As Urn)
        Dim ObjectToScript() As Urn = New Urn(0) {oUrn}
        'ObjectToScript = New Urn(0) {}
        'ObjectToScript(0) = oUrn
        Dim SaveIncludeDatabaseContext As Boolean = _oScripter.Options.IncludeDatabaseContext
        Dim SaveScriptData As Boolean = _oScripter.Options.ScriptData
        Try
            'will raise "Method does not supoort scripting data" error
            If (SaveScriptData) Then
                _oScripter.Options.ScriptData = False
            End If
            ' don't need the database context twice if a drop is scripted too
            If (_bIncludeDrop) Then
                _oScripter.Options.IncludeDatabaseContext = False
            End If
            'Dim strs As StringCollection = _oScripter.Script(ObjectToScript)
            'For Each str As String In strs '
            For Each str As String In _oScripter.Script(ObjectToScript)
                _script.Add(str)
                If _oScripter.Options.ScriptBatchTerminator Then
                    _script.Add("GO")
                End If
            Next
        Catch MayBeEncrypted As Dmf.FailedOperationException
            'the VS help doc said in thread safety context that FailedOperationException was raised 
            ' only in debug mode and not when the IDE is not in use
            ' if that is true here may need to create a 'MustBeEncrypted' exception handler 
            ' not likely would simply not  be the inner exception
            If Not (MayBeEncrypted.InnerException Is Nothing) _
            AndAlso TypeOf MayBeEncrypted.InnerException Is PropertyCannotBeRetrievedException _
            AndAlso MayBeEncrypted.InnerException.Message.Contains("Property TextHeader is not available") Then
                _script.Add(MayBeEncrypted.InnerException.Message)
            Else
                Throw New Exception(String.Format("(DbDocumentor.ScriptObject) " & _
                                                             "Exception while generating script for URN {0}.", _
                                                             oUrn.ToString), MayBeEncrypted)
            End If
        Catch ex As Exception
            Throw New Exception(String.Format("(DbDocumentor.ScriptObject) " & _
                                                         "Exception while generating script for URN {0}.", _
                                                         oUrn.ToString), ex)
        Finally
            _oScripter.Options.IncludeDatabaseContext = SaveIncludeDatabaseContext
            _oScripter.Options.ScriptData = SaveScriptData
        End Try
    End Sub

#End Region

#Region " Utility "

    Function bRayToHexStr(ByVal bRay As Byte()) As String
        Dim sHex As String = "0x"
        For Each b As Byte In bRay
            sHex = sHex & Hex(b)
        Next
        bRayToHexStr = sHex
    End Function

#End Region

End Class
