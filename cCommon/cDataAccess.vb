Imports System.Runtime.InteropServices

Public Class cDataAccess

    Public dsSQLCfg As New dsSQLConfiguration
    Public dcSQLCfg As New DataClassesSQLConfigurationDataContext

    ' ui and service populate these
    Public RepositoryInstanceName As String
    Public RepositoryDatabaseName As String
    Public RepositoryUseTrustedConnection As Boolean
    Public RepositorySQLLoginName As String
    Public RepositorySQLLoginPassword As String
    Public RepositoryConnectionTimeout As Integer
    Public RepositoryNetworkLibrary As String ' not protocol here but libaray - need to build connectstring
    Public RepositoryEncryptConnection As Boolean
    Public RepositoryTrustServerCertificate As Boolean

    Public Enum PingSQLCfgResponse As Integer
        Undefined = -8
        NoServer = -7
        NoDatabase = -6
        NoTable = -5
        NoType = -4
        Expired = -3
        NoDataSet = -2
        NoData = -1
        Evaluation = 0
        Licensed = 1
    End Enum

    Public ReadOnly Property LocalRepositoryConnectionString() As String
        Get
            Dim builder As New System.Data.SqlClient.SqlConnectionStringBuilder
            builder.DataSource = RepositoryInstanceName
            builder.InitialCatalog = RepositoryDatabaseName
            If RepositoryUseTrustedConnection Then
                builder.IntegratedSecurity = RepositoryUseTrustedConnection
            Else
                builder.UserID = RepositorySQLLoginName
                builder.Password = RepositorySQLLoginPassword
            End If
            builder.ConnectTimeout = RepositoryConnectionTimeout
            builder.ApplicationName = My.Application.Info.ProductName
            'allow for a 'not specified' net
            If RepositoryNetworkLibrary <> "" Then
                builder.NetworkLibrary = CStr(If(InStr(RepositoryNetworkLibrary, " ") > 0, _
                                                  RepositoryNetworkLibrary.Split(Chr(32))(0), _
                                                  RepositoryNetworkLibrary))
            End If
            builder.Encrypt = RepositoryEncryptConnection
            builder.TrustServerCertificate = RepositoryTrustServerCertificate
            builder.UserInstance = False
            builder.Enlist = True
            builder.PersistSecurityInfo = True
            Return builder.ConnectionString
        End Get
    End Property

    ' see also DialogConnect
    Public ReadOnly Property TargetConnectionString(ByVal TargetInstanceName As String) As String
        Get
            'ElementAtOrDefault
            dcSQLCfg.Connection.ConnectionString = LocalRepositoryConnectionString
            Dim cn = (From c In dcSQLCfg.tConnections _
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
                    GetSQLAuthenticator(cn(0).InstanceName, builder.UserID, builder.Password)
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

    Public Function PingSQLCfg() As Integer
        ' just see if the database is there MUST NOT CHANGE ANY STATE
        ' try to avoid raising an error if it is not fatal
        ' still have an issue if it's there and the config is hacked        
        Dim osrv As New Server
        Try
            'need an instance name for the connectionstring datasource else forget it
            If RepositoryInstanceName Is Nothing OrElse RepositoryInstanceName = "" Then
                PingSQLCfg = PingSQLCfgResponse.Undefined
            Else
                'see if the server is there
                If (RepositoryUseTrustedConnection _
                    AndAlso (osrv.PingSqlServerVersion(RepositoryInstanceName) Is Nothing)) _
                OrElse (Not RepositoryUseTrustedConnection _
                        AndAlso (osrv.PingSqlServerVersion(RepositoryInstanceName, _
                                                           RepositorySQLLoginName, _
                                                           RepositorySQLLoginPassword) Is Nothing)) Then
                    ' settings based connection failed
                    PingSQLCfg = PingSQLCfgResponse.NoServer
                Else
                    Dim builder As New System.Data.SqlClient.SqlConnectionStringBuilder
                    builder.DataSource = RepositoryInstanceName
                    'just take the default builder.InitialCatalog = "master"
                    If RepositoryUseTrustedConnection Then
                        builder.IntegratedSecurity = RepositoryUseTrustedConnection
                    Else
                        builder.UserID = RepositorySQLLoginName
                        builder.Password = RepositorySQLLoginPassword
                    End If
                    builder.ConnectTimeout = RepositoryConnectionTimeout
                    builder.ApplicationName = My.Application.Info.ProductName
                    'allow for a 'not specified' net
                    If RepositoryNetworkLibrary <> "" Then
                        builder.NetworkLibrary = CStr(If(InStr(RepositoryNetworkLibrary, Space(1)) > 0, _
                                                         RepositoryNetworkLibrary.Split(Chr(32))(0), _
                                                         RepositoryNetworkLibrary))
                    End If
                    builder.Encrypt = RepositoryEncryptConnection
                    builder.TrustServerCertificate = RepositoryTrustServerCertificate
                    builder.UserInstance = False
                    osrv.ConnectionContext.ConnectionString = builder.ConnectionString

                    Try
                        'if a server version came back, see if the db is there 
                        If Not osrv.Databases.Contains(RepositoryDatabaseName) Then
                            PingSQLCfg = PingSQLCfgResponse.NoDatabase
                        ElseIf Not osrv.Databases(RepositoryDatabaseName).Tables.Contains("tSQLCfg", "SQLCfg") Then
                            ' make sure the license table is there
                            PingSQLCfg = PingSQLCfgResponse.NoTable
                        ElseIf Not osrv.Databases(RepositoryDatabaseName).UserDefinedTypes.Contains("SQLCfgNode", "SQLCfg") Then
                            ' make sure the data type is there
                            PingSQLCfg = PingSQLCfgResponse.NoType
                        Else
                            builder.InitialCatalog = RepositoryDatabaseName
                            Dim SQLCfg As New dsSQLConfiguration.tSQLCfgDataTable
                            Using taSQLCfg As New dsSQLConfigurationTableAdapters.tSQLCfgTableAdapter
                                taSQLCfg.ClearBeforeFill = True
                                taSQLCfg.Connection.ConnectionString = builder.ConnectionString
                                taSQLCfg.Fill(SQLCfg)
                            End Using
                            If dsSQLCfg.tSQLCfg.Rows.Count > 0 Then
                                If dsSQLCfg.tSQLCfg.Rows(0).Item("LicenseCode").ToString = "Evaluation" Then
                                    PingSQLCfg = PingSQLCfgResponse.Evaluation
                                Else
                                    PingSQLCfg = PingSQLCfgResponse.Licensed
                                End If
                            Else
                                'the license row is empty
                                PingSQLCfg = PingSQLCfgResponse.NoData
                            End If
                        End If
                    Catch ex As Exception
                        ' login failed, couldn't open database
                        If TypeOf ex.GetBaseException Is SqlException _
                        AndAlso CType(ex.GetBaseException, SqlException).ErrorCode = -2146232060 Then
                            PingSQLCfg = PingSQLCfgResponse.NoDatabase
                        End If
                    End Try
                End If
            End If
        Catch ex As Exception
            Throw New Exception("(cDataAccess.PingSQLCfg) Exception.", ex)
        Finally
            If osrv.ConnectionContext.IsOpen Then
                osrv.ConnectionContext.Disconnect()
            End If
            osrv = Nothing
        End Try
    End Function

    Public Function TargetHandshake(ByVal InstanceName As String, _
                                    ByVal ConnectionTimeoutSeconds As Integer) As Boolean
        ' verify that a target is available without doing any more than necessary 
        ' try to avoid raising any error if it is not fatal
        Try
            'need an instance name for the connectionstring datasource else forget it
            TargetHandshake = False
            If Not InstanceName Is Nothing AndAlso Not InstanceName = "" Then

                Dim drCn As dsSQLConfiguration.tConnectionRow = dsSQLCfg.tConnection.FindByInstanceName(InstanceName)
                Dim builder As New System.Data.SqlClient.SqlConnectionStringBuilder
                builder.DataSource = InstanceName
                'just take the default builder.InitialCatalog
                If drCn.LoginSecure Then
                    builder.IntegratedSecurity = drCn.LoginSecure
                Else
                    Dim SQLLoginName As String = ""
                    Dim SQLLoginPassword As String = ""
                    GetSQLAuthenticator(InstanceName, SQLLoginName, SQLLoginPassword)
                    builder.UserID = SQLLoginName
                    builder.Password = SQLLoginPassword
                End If
                builder.ConnectTimeout = ConnectionTimeoutSeconds
                builder.ApplicationName = My.Application.Info.ProductName
                'allow for a 'not specified' net
                If drCn.NetworkProtocol <> "" Then
                    builder.NetworkLibrary = CStr(If(InStr(drCn.NetworkProtocol, Space(1)) > 0, _
                                                     drCn.NetworkProtocol.Split(Chr(32))(0), _
                                                     drCn.NetworkProtocol))
                End If
                builder.Encrypt = drCn.EncryptedConnection
                builder.TrustServerCertificate = drCn.TrustServerCertificate
                builder.UserInstance = False

                Dim srvcon As New ServerConnection
                srvcon.ConnectionString = builder.ConnectionString
                Dim osrv As Server = New Server(srvcon)
                If osrv.ConnectionContext.TrueName = InstanceName Then
                    TargetHandshake = True
                End If
                osrv.ConnectionContext.Disconnect()
                osrv = Nothing
            End If
        Catch ex As Exception
            TargetHandshake = False
        End Try
    End Function

    Public Sub AddSQLClueAdminUser(ByVal AccountName As String, _
                                   ByVal DatabaseName As String)
        Dim oSrv As New Server
        Dim oDB As Database
        Dim lgn As Login
        Dim usr As User
        Try
            oSrv.ConnectionContext.ConnectionString = LocalRepositoryConnectionString
            If Not AccountName Is Nothing _
            AndAlso Not AccountName = "" Then
                If Not oSrv.Logins.Contains(AccountName) Then
                    lgn = New Login(oSrv, AccountName)
                End If
                oDB = oSrv.Databases(DatabaseName)
                If Not oDB.Users.Contains(AccountName) Then
                    usr = New User(oDB, AccountName)
                Else
                    usr = oDB.Users(AccountName)
                End If
                usr.AddToRole("SQLClueAdminRole")
            End If
        Catch ex As Exception
            Throw New Exception("(cDataAccess.AddSQLClueAdminUser) Exception.", ex)
        Finally
            oSrv.ConnectionContext.Disconnect()
            oSrv = Nothing
        End Try
    End Sub

    Public Sub AddSQLClueServiceAccountToTarget(ByVal ServiceAccountName As String, _
                                                ByVal TargetInstanceName As String, _
                                                ByVal HandshakeConnectionTimeout As Integer)

        Try
            dcSQLCfg.Connection.ConnectionString = LocalRepositoryConnectionString
            If (From c In dcSQLCfg.tConnections _
                Where (c.InstanceName) = TargetInstanceName _
                Select c).ToList(0).LoginSecure = True Then
                If TargetHandshake(TargetInstanceName, HandshakeConnectionTimeout) Then
                    Dim oSrv As New Server
                    Try
                        oSrv.ConnectionContext.ConnectionString = TargetConnectionString(TargetInstanceName)
                        ' this is a noop if the login alread exists 
                        ' this will allow removal of the account to presist
                        ' also a noop if SQL Authentication
                        If Not oSrv.Logins.Contains(ServiceAccountName) Then
                            Dim lgn As Login = New Login(oSrv, ServiceAccountName)
                            lgn.AddToRole(Microsoft.SqlServer.Management.Common.FixedServerRoles.SysAdmin.ToString)
                        End If
                    Catch ex As Exception
                        Throw New Exception(String.Format("SMO Exception. The login for service account {0} was not added to target {1}.", _
                                                          ServiceAccountName, TargetInstanceName), ex)
                    Finally
                        oSrv.ConnectionContext.Disconnect()
                        oSrv = Nothing
                    End Try
                Else
                    Throw New Exception(String.Format("Handshake failed. The login for service account {0} was not added to target {1}.", _
                                                      ServiceAccountName, TargetInstanceName))
                End If
            End If
        Catch ex As Exception
            Throw New Exception("(cDataAccess.AddSQLClueServiceAccountToTarget) Exception.", ex)
        End Try
    End Sub

    Public Sub DropSQLClueServiceAccountFromTarget(ByVal ServiceAccountName As String, _
                                                   ByVal TargetInstanceName As String, _
                                                   ByVal HandshakeConnectionTimeout As Integer)
        Try
            dcSQLCfg.Connection.ConnectionString = LocalRepositoryConnectionString
            If (From c In dcSQLCfg.tConnections _
                Where (c.InstanceName) = TargetInstanceName _
                Select c).ToList(0).LoginSecure = True Then
                If TargetHandshake(TargetInstanceName, HandshakeConnectionTimeout) Then
                    Dim oSrv As New Server
                    Try
                        oSrv.ConnectionContext.ConnectionString = TargetConnectionString(TargetInstanceName)
                        ' this is a noop if the login does not exists 
                        ' also a noop if SQL Authentication
                        If oSrv.Logins.Contains(ServiceAccountName) Then
                            Dim lgn As Login = oSrv.Logins(ServiceAccountName)
                            lgn.Drop()
                        End If
                    Catch ex As Exception
                        Throw New Exception(String.Format("SMO Exception. The login for service account {0} was not removed from target {1}.", _
                                                          ServiceAccountName, TargetInstanceName), ex)
                    Finally
                        oSrv.ConnectionContext.Disconnect()
                        oSrv = Nothing
                    End Try
                Else
                    Throw New Exception(String.Format("Handshake failed. The login for service account {0} was not removed from target {1}.", _
                                                      ServiceAccountName, TargetInstanceName))
                End If
            End If
        Catch ex As Exception
            Throw New Exception("(cDataAccess.DropSQLClueAddServiceAccountToTarget) Exception.", ex)
        End Try
    End Sub

    Public Sub AddServiceUser(ByVal SvcAccountName As String, _
                              ByVal DataStore As String, _
                              ByVal DatabaseName As String)
        Dim oSrv As New Server
        Dim oDB As Database
        Dim lgn As Login
        Dim usr As User
        Try
            oSrv.ConnectionContext.ConnectionString = LocalRepositoryConnectionString
            If Not SvcAccountName Is Nothing _
            AndAlso Not SvcAccountName = "" Then
                If Not oSrv.Logins.Contains(SvcAccountName) Then
                    lgn = New Login(oSrv, SvcAccountName)
                End If
                oDB = oSrv.Databases(DatabaseName)
                If Not oDB.Users.Contains(SvcAccountName) Then
                    usr = New User(oDB, SvcAccountName)
                Else
                    usr = oDB.Users(SvcAccountName)
                End If
                Select Case DataStore
                    Case "Repository"
                        usr.AddToRole("SQLCfgServiceRole")
                    Case "Runbook"
                        usr.AddToRole("SQLRunbookServiceRole")
                End Select
            End If
        Catch ex As Exception
            Throw New Exception("(cDataAccess.AddServiceUser) Exception.", ex)
        Finally
            oSrv.ConnectionContext.Disconnect()
            oSrv = Nothing
        End Try
    End Sub

    Public Sub AddComponentAdminUser(ByVal AccountName As String, _
                                     ByVal DataStore As String, _
                                     ByVal DatabaseName As String)
        Dim oSrv As New Server
        Dim oDB As Database
        Dim lgn As Login
        Dim usr As User
        Try
            oSrv.ConnectionContext.ConnectionString = LocalRepositoryConnectionString
            If Not AccountName Is Nothing _
            AndAlso Not AccountName = "" Then
                oSrv.Refresh()
                If Not oSrv.Logins.Contains(AccountName) Then
                    lgn = New Login(oSrv, AccountName)
                End If
                oDB = oSrv.Databases(DatabaseName)
                If Not oDB.Users.Contains(AccountName) Then
                    usr = New User(oDB, AccountName)
                Else
                    usr = oDB.Users(AccountName)
                End If
                Select Case DataStore
                    Case "Repository"
                        usr.AddToRole("SQLCfgAdminRole")
                    Case "Runbook"
                        usr.AddToRole("SQLRunbookAdminRole")
                End Select
            End If
        Catch ex As Exception
            Throw New Exception("(cDataAccess.AddComponentAdminUser) Exception.", ex)
        Finally
            oSrv.ConnectionContext.Disconnect()
            oSrv = Nothing
        End Try
    End Sub

    Public Sub AddComponentReportingUser(ByVal AccountName As String, _
                                         ByVal DataStore As String, _
                                         ByVal DatabaseName As String)
        Dim oSrv As New Server
        Dim oDB As Database
        Dim lgn As Login
        Dim usr As User
        Try
            oSrv.ConnectionContext.ConnectionString = LocalRepositoryConnectionString
            If Not AccountName Is Nothing _
            AndAlso Not AccountName = "" Then
                If Not oSrv.Logins.Contains(AccountName) Then
                    lgn = New Login(oSrv, AccountName)
                End If
                oDB = oSrv.Databases(DatabaseName)
                If Not oDB.Users.Contains(AccountName) Then
                    usr = New User(oDB, AccountName)
                Else
                    usr = oDB.Users(AccountName)
                End If
                Select Case DataStore
                    Case "Repository"
                        usr.AddToRole("SQLCfgReportingRole")
                    Case "Runbook"
                        usr.AddToRole("SQLRunbookUserRole")
                End Select
            End If
        Catch ex As Exception
            Throw New Exception("(cDataAccess.AddComponentReportingUser) Exception.", ex)
        Finally
            oSrv.ConnectionContext.Disconnect()
            oSrv = Nothing
        End Try
    End Sub

    Public Sub AddRunbookContributorUser(ByVal AccountName As String, _
                                         ByVal DatabaseName As String)
        Dim oSrv As New Server
        Dim oDB As Database
        Dim lgn As Login
        Dim usr As User
        Try
            oSrv.ConnectionContext.ConnectionString = LocalRepositoryConnectionString
            If Not AccountName Is Nothing _
            AndAlso Not AccountName = "" Then
                If Not oSrv.Logins.Contains(AccountName) Then
                    lgn = New Login(oSrv, AccountName)
                End If
                oDB = oSrv.Databases(DatabaseName)
                If Not oDB.Users.Contains(AccountName) Then
                    usr = New User(oDB, AccountName)
                Else
                    usr = oDB.Users(AccountName)
                End If
                usr.AddToRole("SQLRunbookContributorRole")
            End If
        Catch ex As Exception
            Throw New Exception("(cDataAccess.AddRunbookContributorUser) Exception.", ex)
        Finally
            oSrv.ConnectionContext.Disconnect()
            oSrv = Nothing
        End Try
    End Sub

    Public Sub LoadConfig(Optional ByVal sConnectionString As String = "")
        'My.Settings.SQLConfigurationRepositoryConnectionString = Me.sConnectionString
        ' before populating the dataset make sure no databases were added (or dropped)
        ' this is done in the scheduler, needs to happen batch too
        Try
            dsSQLCfg.Clear()
            If Not RepositoryInstanceName = "" Then
                LoadSQLCfg(sConnectionString)
                LoadConnections(sConnectionString)
                LoadInstance(sConnectionString)
                LoadJobServer(sConnectionString)
                LoadDatabase(sConnectionString)
                LoadServiceBroker(sConnectionString)
                LoadSchedule(sConnectionString)
            End If
        Catch ex As Exception
            Throw New Exception(String.Format("(cDataAccess.LoadConfig) Exception." & vbCrLf & "{0}", ex.StackTrace), ex)
        End Try
    End Sub

    Public Sub LoadConfigByInstance(ByVal InstanceName As String)
        Try
            dsSQLCfg.Clear()
            ' before populating the dataset make sure no databases were added (or dropped)
            ' this is done in the scheduler, needs to happen in batch too
            Dim bEncryptedConnection As System.Nullable(Of Boolean)
            Dim bTrustServerCertificate As System.Nullable(Of Boolean)
            Dim sNetworkProtocol As String = ""
            Dim iConnectionTimeout As System.Nullable(Of Integer)
            Dim bLoginSecure As System.Nullable(Of Boolean)
            ' need to stop this from wipes out the full list from the connection DGV 
            Using taConnection As New dsSQLConfigurationTableAdapters.tConnectionTableAdapter
                taConnection.Connection.ConnectionString = LocalRepositoryConnectionString
                taConnection.pConnectionGet(InstanceName, _
                                            bEncryptedConnection, _
                                            bTrustServerCertificate, _
                                            sNetworkProtocol, _
                                            iConnectionTimeout, _
                                            bLoginSecure)
            End Using
            dsSQLCfg.tConnection.AddtConnectionRow(InstanceName, _
                                                   CBool(bEncryptedConnection), _
                                                   CBool(bTrustServerCertificate), _
                                                   sNetworkProtocol, _
                                                   CInt(iConnectionTimeout), _
                                                   CBool(bLoginSecure))
            Dim bActiveDirectory As System.Nullable(Of Boolean)
            Dim bAudits As System.Nullable(Of Boolean)
            Dim bBackupdevices As System.Nullable(Of Boolean)
            Dim bConfiguration As System.Nullable(Of Boolean)
            Dim bCredentials As System.Nullable(Of Boolean)
            Dim bCryptographicProviders As System.Nullable(Of Boolean)
            Dim bDatabases As System.Nullable(Of Boolean)
            Dim bEndPoints As System.Nullable(Of Boolean)
            Dim bFullTextService As System.Nullable(Of Boolean)
            Dim bInformation As System.Nullable(Of Boolean)
            Dim bJobServer As System.Nullable(Of Boolean)
            Dim bLogins As System.Nullable(Of Boolean)
            Dim bLinkedServers As System.Nullable(Of Boolean)
            Dim bMail As System.Nullable(Of Boolean)
            Dim bProxyAccount As System.Nullable(Of Boolean)
            Dim bResourceGovernor As System.Nullable(Of Boolean)
            Dim bRoles As System.Nullable(Of Boolean)
            Dim bServerAuditSpecifications As System.Nullable(Of Boolean)
            Dim bSettings As System.Nullable(Of Boolean)
            Dim bTriggers As System.Nullable(Of Boolean)
            Dim bUserDefinedMessages As System.Nullable(Of Boolean)
            Using taInstance As New dsSQLConfigurationTableAdapters.tInstanceTableAdapter
                taInstance.Connection.ConnectionString = LocalRepositoryConnectionString
                Dim r As Integer = taInstance.pInstanceGet(InstanceName, _
                                                           bActiveDirectory, _
                                                           bAudits, _
                                                           bBackupdevices, _
                                                           bConfiguration, _
                                                           bCredentials, _
                                                           bCryptographicProviders, _
                                                           bDatabases, _
                                                           bEndPoints, _
                                                           bFullTextService, _
                                                           bInformation, _
                                                           bJobServer, _
                                                           bLogins, _
                                                           bLinkedServers, _
                                                           bMail, _
                                                           bProxyAccount, _
                                                           bResourceGovernor, _
                                                           bRoles, _
                                                           bServerAuditSpecifications, _
                                                           bSettings, _
                                                           bTriggers, _
                                                           bUserDefinedMessages)
                If (r = -1) Then
                    dsSQLCfg.tInstance.AddtInstanceRow(dsSQLCfg.tConnection.FindByInstanceName(InstanceName), _
                                                       CBool(bActiveDirectory), _
                                                       CBool(bAudits), _
                                                       CBool(bBackupdevices), _
                                                       CBool(bConfiguration), _
                                                       CBool(bCredentials), _
                                                       CBool(bCryptographicProviders), _
                                                       CBool(bDatabases), _
                                                       CBool(bEndPoints), _
                                                       CBool(bFullTextService), _
                                                       CBool(bInformation), _
                                                       CBool(bJobServer), _
                                                       CBool(bLogins), _
                                                       CBool(bLinkedServers), _
                                                       CBool(bMail), _
                                                       CBool(bProxyAccount), _
                                                       CBool(bResourceGovernor), _
                                                       CBool(bRoles), _
                                                       CBool(bServerAuditSpecifications), _
                                                       CBool(bSettings), _
                                                       CBool(bTriggers), _
                                                       CBool(bUserDefinedMessages))
                Else
                    Exit Sub
                End If
            End Using
            Dim bAlerts As System.Nullable(Of Boolean)
            Dim bAlertSystem As System.Nullable(Of Boolean)
            Dim bJobs As System.Nullable(Of Boolean)
            Dim bOperators As System.Nullable(Of Boolean)
            Dim bJSProxyAccounts As System.Nullable(Of Boolean)
            Dim bTargetServers As System.Nullable(Of Boolean)
            Using taJobServer As New dsSQLConfigurationTableAdapters.tJobServerTableAdapter
                taJobServer.Connection.ConnectionString = LocalRepositoryConnectionString
                taJobServer.pJobServerGet(InstanceName, _
                                                 bAlerts, _
                                                 bAlertSystem, _
                                                 bJobs, _
                                                 bOperators, _
                                                 bJSProxyAccounts, _
                                                 bTargetServers)
                dsSQLCfg.tJobServer.AddtJobServerRow(dsSQLCfg.tInstance.FindByName(InstanceName), _
                                                     CBool(bAlerts), _
                                                     CBool(bAlertSystem), _
                                                     CBool(bJobs), _
                                                     CBool(bOperators), _
                                                     CBool(bJSProxyAccounts), _
                                                     CBool(bTargetServers))
            End Using
            LoadDatabase("", InstanceName)
            LoadServiceBroker("", InstanceName)
            LoadSchedule("", InstanceName)
            ' never save the ServiceSettings here
            dsSQLCfg.AcceptChanges()
        Catch ex As Exception
            dsSQLCfg.RejectChanges()
            Throw New Exception("(cDataAccess.LoadConfigByInstance) Failure.", ex)
        End Try
    End Sub

    Public Sub LoadSQLCfg(Optional ByVal sConnectionString As String = "")
        Try
            Using taSQLCfg As New dsSQLConfigurationTableAdapters.tSQLCfgTableAdapter
                taSQLCfg.Connection.ConnectionString = If(sConnectionString = "", _
                                                                  LocalRepositoryConnectionString, _
                                                                  sConnectionString)
                taSQLCfg.ClearBeforeFill = True
                taSQLCfg.Fill(dsSQLCfg.tSQLCfg)
            End Using
        Catch ex As Exception
            Throw New Exception("(cDataAccess.LoadSQLCfg) Exception.", ex)
        End Try
    End Sub

    Public Sub LoadConnections(Optional ByVal sConnectionString As String = "")
        Try
            Using taConnection As New dsSQLConfigurationTableAdapters.tConnectionTableAdapter
                taConnection.Connection.ConnectionString = If(sConnectionString = "", _
                                                              LocalRepositoryConnectionString, _
                                                              sConnectionString)
                taConnection.ClearBeforeFill = True
                taConnection.Fill(dsSQLCfg.tConnection)
            End Using
        Catch ex As Exception
            Throw New Exception("(cDataAccess.LoadConnections) Exception.", ex)
        End Try
    End Sub

    Public Sub LoadInstance(Optional ByVal sConnectionString As String = "")
        Try
            Using taInstance As New dsSQLConfigurationTableAdapters.tInstanceTableAdapter
                taInstance.Connection.ConnectionString = If(sConnectionString = "", _
                                                            LocalRepositoryConnectionString, _
                                                            sConnectionString)
                taInstance.ClearBeforeFill = True
                taInstance.Fill(dsSQLCfg.tInstance)
            End Using
        Catch ex As Exception
            Throw New Exception("(cDataAccess.LoadInstance) Exception.", ex)
        End Try
    End Sub

    Public Sub LoadJobServer(Optional ByVal sConnectionString As String = "")
        Try
            Using taJobServer As New dsSQLConfigurationTableAdapters.tJobServerTableAdapter
                taJobServer.Connection.ConnectionString = If(sConnectionString = "", _
                                                             LocalRepositoryConnectionString, _
                                                             sConnectionString)
                taJobServer.ClearBeforeFill = True
                taJobServer.Fill(dsSQLCfg.tJobServer)
            End Using
        Catch ex As Exception
            Throw New Exception("(cDataAccess.LoadJobServer) Exception.", ex)
        End Try
    End Sub

    Public Sub LoadDatabase(Optional ByVal sConnectionString As String = "", _
                            Optional ByVal Instance As String = "")
        Try
            Using taDatabase As New dsSQLConfigurationTableAdapters.tDbTableAdapter
                taDatabase.Connection.ConnectionString = If(sConnectionString = "", _
                                                            LocalRepositoryConnectionString, _
                                                            sConnectionString)
                taDatabase.ClearBeforeFill = True
                If Instance = "" Then
                    taDatabase.Fill(dsSQLCfg.tDb)
                Else
                    taDatabase.FillByInstanceName(dsSQLCfg.tDb, Instance)
                End If
            End Using
        Catch ex As Exception
            Throw New Exception("(cDataAccess.LoadDatabase) Exception.", ex)
        End Try
    End Sub

    Public Sub LoadServiceBroker(Optional ByVal sConnectionString As String = "", _
                                Optional ByVal Instance As String = "")
        Try
            Using taServiceBroker As New dsSQLConfigurationTableAdapters.tServiceBrokerTableAdapter
                taServiceBroker.Connection.ConnectionString = If(sConnectionString = "", _
                                                                 LocalRepositoryConnectionString, _
                                                                 sConnectionString)
                taServiceBroker.ClearBeforeFill = True
                If Instance = "" Then
                    taServiceBroker.Fill(dsSQLCfg.tServiceBroker)
                Else
                    taServiceBroker.FillByInstanceName(dsSQLCfg.tServiceBroker, Instance)
                End If
            End Using
        Catch ex As Exception
            Throw New Exception("(cDataAccess.LoadServiceBroke) Exception.", ex)
        End Try
    End Sub

    Public Sub LoadSchedule(Optional ByVal sConnectionString As String = "", _
                            Optional ByVal Instance As String = "")
        Try
            Using taSchedule As New dsSQLConfigurationTableAdapters.tScheduleTableAdapter
                taSchedule.Connection.ConnectionString = If(sConnectionString = "", _
                                                            LocalRepositoryConnectionString, _
                                                            sConnectionString)
                taSchedule.ClearBeforeFill = True
                If Instance = "" Then
                    taSchedule.Fill(dsSQLCfg.tSchedule)
                Else
                    taSchedule.FillByInstanceName(dsSQLCfg.tSchedule, Instance)
                End If
            End Using
        Catch ex As Exception
            Throw New Exception("(cDataAccess.LoadSchedule) Exception.", ex)
        End Try
    End Sub

    Public Sub LoadServiceSettings(Optional ByVal sConnectionString As String = "", _
                                   Optional ByVal Name As String = "DEFAULT")
        Try
            Using taSettings As New dsSQLConfigurationTableAdapters.tServiceSettingsTableAdapter
                taSettings.Connection.ConnectionString = If(sConnectionString = "", _
                                                            LocalRepositoryConnectionString, _
                                                            sConnectionString)
                taSettings.ClearBeforeFill = True
                taSettings.Fill(dsSQLCfg.tServiceSettings, Name)
            End Using
        Catch ex As Exception
            Throw New Exception("(cDataAccess.LoadServiceSettings) Exception.", ex)
        End Try
    End Sub

    Public Sub SaveConfig()
        ' sql authentication login/pwd should be saved AFTER this is called
        Try
            If dsSQLCfg.HasChanges() Then

                ' before populating the dataset make sure no databases were added (or dropped)
                ' this is done in the scheduler, needs to happen batch too
                ' it will just happen of the dataset is handled as in the scheduler 
                ' (get from target, set checked from repository) new will use defaults of insert proc
                ' the case of a new instance is not quite right yet
                SaveInstance()
                SaveJobServer()
                SaveDatabase()
                SaveServiceBroker()
            End If
        Catch ex As Exception
            Throw New Exception("(cDataAccess.SaveConfig) Exception.", ex)
        End Try
    End Sub

    Public Sub SaveSQLCfg()
        Try
            Using taSQLCfg As New dsSQLConfigurationTableAdapters.tSQLCfgTableAdapter
                If dsSQLCfg.tSQLCfg.GetChanges().Rows.Count > 0 Then
                    taSQLCfg.Connection.ConnectionString = LocalRepositoryConnectionString
                    taSQLCfg.Update(dsSQLCfg.tSQLCfg)
                    dsSQLCfg.tSQLCfg.AcceptChanges()
                End If
            End Using
        Catch ex As Exception
            dsSQLCfg.tSQLCfg.RejectChanges()
            Throw New Exception("(cDataAccess.SaveSQLCfg) Exception.", ex)
        End Try
    End Sub

    Public Sub SaveConnections()
        Try
            Using taConnection As New dsSQLConfigurationTableAdapters.tConnectionTableAdapter
                If Not dsSQLCfg.tConnection.GetChanges() Is Nothing Then
                    taConnection.Connection.ConnectionString = LocalRepositoryConnectionString
                    taConnection.Update(dsSQLCfg.tConnection)
                    dsSQLCfg.tConnection.AcceptChanges()
                End If
            End Using
        Catch ex As Exception
            dsSQLCfg.tConnection.RejectChanges()
            Throw New Exception("(cDataAccess.SaveConnections) Exception.", ex)
        End Try
    End Sub

    Public Sub SaveInstance()
        Try
            Using taInstance As New dsSQLConfigurationTableAdapters.tInstanceTableAdapter
                If Not dsSQLCfg.tInstance.GetChanges() Is Nothing Then
                    taInstance.Connection.ConnectionString = LocalRepositoryConnectionString
                    taInstance.Update(dsSQLCfg.tInstance)
                    dsSQLCfg.tInstance.AcceptChanges()
                End If
            End Using
        Catch ex As Exception
            dsSQLCfg.tInstance.RejectChanges()
            Throw New Exception("(cDataAccess.SaveInstance) Exception.", ex)
        End Try
    End Sub

    Public Sub SaveJobServer()
        Try
            Using taJobServer As New dsSQLConfigurationTableAdapters.tJobServerTableAdapter
                If Not dsSQLCfg.tJobServer.GetChanges() Is Nothing Then
                    taJobServer.Connection.ConnectionString = LocalRepositoryConnectionString
                    taJobServer.Update(dsSQLCfg.tJobServer)
                    dsSQLCfg.tJobServer.AcceptChanges()
                End If
            End Using
        Catch ex As Exception
            dsSQLCfg.tJobServer.RejectChanges()
            Throw New Exception("(cDataAccess.SaveJobServer) Exception.", ex)
        End Try
    End Sub

    Public Sub SaveDatabase()
        Try
            Using taDatabase As New dsSQLConfigurationTableAdapters.tDbTableAdapter
                If Not dsSQLCfg.tDb.GetChanges() Is Nothing Then
                    taDatabase.Connection.ConnectionString = LocalRepositoryConnectionString
                    taDatabase.Update(dsSQLCfg.tDb)
                    dsSQLCfg.tDb.AcceptChanges()
                End If
            End Using
        Catch ex As Exception
            dsSQLCfg.tDb.RejectChanges()
            Throw New Exception("(cDataAccess.SaveDatabase) Exception.", ex)
        End Try
    End Sub

    Public Sub SaveServiceBroker()
        Try
            Using taServiceBroker As New dsSQLConfigurationTableAdapters.tServiceBrokerTableAdapter
                If Not dsSQLCfg.tServiceBroker.GetChanges() Is Nothing Then
                    taServiceBroker.Connection.ConnectionString = LocalRepositoryConnectionString
                    taServiceBroker.Update(dsSQLCfg.tServiceBroker)
                    dsSQLCfg.tServiceBroker.AcceptChanges()
                End If
            End Using
        Catch ex As Exception
            dsSQLCfg.tServiceBroker.RejectChanges()
            Throw New Exception("(cDataAccess.SaveServiceBroke) Exception.", ex)
        End Try
    End Sub

    Public Sub SaveSchedule()
        Try
            Using taSchedule As New dsSQLConfigurationTableAdapters.tScheduleTableAdapter
                If Not dsSQLCfg.tSchedule.GetChanges() Is Nothing Then
                    taSchedule.Connection.ConnectionString = LocalRepositoryConnectionString
                    taSchedule.Update(dsSQLCfg.tSchedule)
                    dsSQLCfg.tSchedule.AcceptChanges()
                End If
            End Using
        Catch ex As Exception
            dsSQLCfg.tSchedule.RejectChanges()
            Throw New Exception("(cDataAccess.SaveSchedule) Exception.", ex)
        End Try
    End Sub

    Public Sub SaveServiceSettings(ByVal Name As String)
        Try
            Using taSettings As New dsSQLConfigurationTableAdapters.tServiceSettingsTableAdapter
                If Not dsSQLCfg.tServiceSettings.GetChanges() Is Nothing Then
                    taSettings.Connection.ConnectionString = LocalRepositoryConnectionString
                    'taSettings.Update(dsSQLCfg.tSQLCfgServiceSettings)
                    Throw New Exception("Update is not a member of dsSQLConfigurationTableAdapters.tSQLCfgServiceSettingsTableAdapter")
                    dsSQLCfg.tSchedule.AcceptChanges()
                End If
            End Using
        Catch ex As Exception
            dsSQLCfg.tSchedule.RejectChanges()
            Throw New Exception("(cDataAccess.SaveServiceSettings) Exception.", ex)
        End Try
    End Sub

    Public Sub InitNewTargetInstance(ByVal InstanceName As String, _
                                     ByVal EngineEdition As Integer, _
                                     ByVal VersionMajor As Integer)
        Try
            Using taInstance As New dsSQLConfigurationTableAdapters.tInstanceTableAdapter
                taInstance.Connection.ConnectionString = LocalRepositoryConnectionString
                taInstance.pInstanceInit(InstanceName, _
                                         VersionMajor, _
                                         EngineEdition)
            End Using
        Catch ex As Exception
            Throw New Exception(String.Format("(cDataAccess.InitNewTargetInstance) Failure while adding Instance {0} to Repository.", InstanceName), ex)
        End Try
    End Sub

    Public Sub InitNewTargetDatabase(ByVal InstanceName As String, _
                                     ByVal DBName As String, _
                                     ByVal VersionMajor As Integer)
        Try
            Using taDatabase As New dsSQLConfigurationTableAdapters.tDbTableAdapter
                taDatabase.Connection.ConnectionString = LocalRepositoryConnectionString
                taDatabase.pDbInit(InstanceName, _
                                   DBName, _
                                   VersionMajor)
            End Using
        Catch ex As Exception
            Throw New Exception(String.Format("(cDataAccess.InitNewTargetDatabase) Failure while adding Instance {0} Database {1} to Repository.", InstanceName, DBName), ex)
        End Try
    End Sub

    Public Sub GetSQLAuthenticator(ByVal InstanceName As String, _
                                   ByRef LoginName As String, _
                                   ByRef Password As String)
        Try
            Using cn As New System.Data.SqlClient.SqlConnection(LocalRepositoryConnectionString)
                cn.Open()
                Using cm As New System.Data.SqlClient.SqlCommand
                    cm.Connection = cn
                    cm.CommandType = CommandType.StoredProcedure
                    cm.CommandText = "SQLCfg.pConnectionUserGet"
                    Dim SQLInstance As New SqlParameter()
                    With SQLInstance
                        .Direction = ParameterDirection.Input
                        .ParameterName = "@InstanceName"
                        .SqlDbType = SqlDbType.NVarChar
                        .Size = 128
                        .Value = InstanceName
                    End With
                    cm.Parameters.Add(SQLInstance)
                    Dim SQLLogin As New SqlParameter()
                    With SQLLogin
                        .Direction = ParameterDirection.Output
                        .ParameterName = "@LoginName"
                        .SqlDbType = SqlDbType.NVarChar
                        .Size = 128
                    End With
                    cm.Parameters.Add(SQLLogin)
                    Dim SQLPassword As New SqlParameter()
                    With SQLPassword
                        .Direction = ParameterDirection.Output
                        .ParameterName = "@Password"
                        .SqlDbType = SqlDbType.NVarChar
                        .Size = 128
                    End With
                    cm.Parameters.Add(SQLPassword)
                    cm.ExecuteNonQuery()
                    LoginName = SQLLogin.Value.ToString
                    Password = SQLPassword.Value.ToString
                End Using
            End Using
        Catch ex As Exception
            Throw New Exception("(cDataAccess.GetSQLAuthenticator) Exception", ex)
        End Try
    End Sub

    Public Sub SetSQLAuthenticator(ByVal InstanceName As String, _
                                   ByVal LoginName As String, _
                                   ByRef Password As String)
        Try
            Using cn As New System.Data.SqlClient.SqlConnection(LocalRepositoryConnectionString)
                cn.Open()
                Using cm As New System.Data.SqlClient.SqlCommand
                    cm.Connection = cn
                    cm.CommandType = CommandType.StoredProcedure
                    cm.CommandText = "SQLCfg.pConnectionUserUpdate"
                    Dim SQLInstance As New SqlParameter()
                    With SQLInstance
                        .Direction = ParameterDirection.Input
                        .ParameterName = "@InstanceName"
                        .SqlDbType = SqlDbType.NVarChar
                        .Size = 128
                        .Value = InstanceName
                        cm.Parameters.Add(SQLInstance)
                    End With
                    Dim SQLLogin As New SqlParameter()
                    With SQLLogin
                        .Direction = ParameterDirection.Input
                        .ParameterName = "@LoginName"
                        .SqlDbType = SqlDbType.NVarChar
                        .Size = 128
                        .Value = LoginName
                        cm.Parameters.Add(SQLLogin)
                    End With
                    Dim SQLPassword As New SqlParameter()
                    With SQLPassword
                        .Direction = ParameterDirection.Input
                        .ParameterName = "@Password"
                        .SqlDbType = SqlDbType.NVarChar
                        .Size = 128
                        .Value = Password
                        cm.Parameters.Add(SQLPassword)
                    End With
                    cm.ExecuteNonQuery()
                End Using
            End Using
        Catch ex As Exception
            Throw New Exception("(cDataAccess.SetSQLAuthenticator) Exception", ex)
        End Try
    End Sub

    Public Sub RepositoryInit()
        Dim osrv As New Server
        Try
            osrv.ConnectionContext.ConnectionString = LocalRepositoryConnectionString
            If osrv.Databases(RepositoryDatabaseName).StoredProcedures.Contains("pRepositoryInit", "SQLCfg") Then
                Using cn As New System.Data.SqlClient.SqlConnection(LocalRepositoryConnectionString)
                    cn.Open()
                    Using cm As New System.Data.SqlClient.SqlCommand
                        cm.Connection = cn
                        cm.CommandType = CommandType.StoredProcedure
                        cm.CommandText = "SQLCfg.pRepositoryInit"
                        cm.ExecuteNonQuery()
                    End Using
                End Using
            End If
        Catch ex As Exception
            Throw New Exception("(cDataAccess.RepositoryInit) Exception", ex)
        Finally
            osrv.ConnectionContext.Disconnect()
            osrv = Nothing
        End Try
    End Sub

    Public Function GetConfiguredInstanceList(Optional ByVal sConnectionString As String = "") As String()
        Try
            Dim worklist() As String = New String() {}
            Using cn As New System.Data.SqlClient.SqlConnection(If(sConnectionString = "", _
                                                       LocalRepositoryConnectionString, _
                                                       sConnectionString))
                cn.Open()
                Using cm As New System.Data.SqlClient.SqlCommand
                    cm.Connection = cn
                    cm.CommandType = CommandType.StoredProcedure
                    cm.CommandText = "SQLCfg.pInstanceSelectNameList"
                    Dim rdr As SqlDataReader = cm.ExecuteReader()
                    ReDim worklist(0)
                    worklist(0) = ""
                    Dim i As Int32 = 1
                    While rdr.Read()
                        ReDim Preserve worklist(i)
                        worklist(i) = rdr.Item("Name").ToString
                        i += 1
                    End While
                End Using
            End Using
            GetConfiguredInstanceList = worklist
        Catch ex As Exception
            Throw New Exception("(cDataAccess.GetConfiguredInstanceList) Exception", ex)
        End Try
    End Function

    Public Function GetLicensedInstanceList(Optional ByVal sConnectionString As String = "") As String()
        Try
            Dim worklist() As String = New String() {}
            Using cn As New System.Data.SqlClient.SqlConnection(If(sConnectionString = "", _
                                                       LocalRepositoryConnectionString, _
                                                       sConnectionString))
                cn.Open()
                Using cm As New System.Data.SqlClient.SqlCommand
                    cm.Connection = cn
                    cm.CommandType = CommandType.Text
                    cm.CommandText = "SELECT [InstanceName] AS [Name] " & _
                                     "FROM [SQLCfg].[tConnection] c " & _
                                     "WHERE EXISTS (SELECT [Node] " & _
                                                   "FROM [SQLCfg].[tChange] " & _
                                                   "WHERE [Node].[SQLInstance] = c.[InstanceName] " & _
                                                   "AND [Node].[Type] = 'SQLInstance')"
                    Dim rdr As SqlDataReader = cm.ExecuteReader()
                    Dim i As Int32 = 0
                    While rdr.Read()
                        ReDim Preserve worklist(i)
                        worklist(i) = rdr.Item("Name").ToString
                        i += 1
                    End While
                End Using
            End Using
            GetLicensedInstanceList = worklist
        Catch ex As Exception
            Throw New Exception("(cDataAccess.GetLicensedInstanceList) Exception", ex)
        End Try
    End Function

    Public Function GetNodeAttributes(ByVal Node As String, Optional ByVal sConnectionString As String = "") As String()
        Try
            GetNodeAttributes = Nothing
            Using cn As New System.Data.SqlClient.SqlConnection(If(sConnectionString = "", _
                                                       LocalRepositoryConnectionString, _
                                                       sConnectionString))
                cn.Open()
                ' an outrageous example of embedding SQL Injection vulnerable code in the application 
                Using cm As New System.Data.SqlClient.SqlCommand
                    cm.Connection = cn
                    cm.CommandType = CommandType.Text
                    cm.CommandText = "SELECT [Node].[SQLInstance] " & _
                                     ", [Node].[Database] AS [Database] " & _
                                     ", [Node].[Type] AS [Type] " & _
                                     ", [Node].[SubType] AS [SubType] " & _
                                     ", [Node].[Collection] AS [Collection] " & _
                                     ", [Node].[Item] AS [Item] " & _
                                     "FROM (SELECT CAST('" + Node + "' AS SQLCfg.SQLCfgNode) As [Node]) n"
                    Dim rdr As SqlDataReader = cm.ExecuteReader()
                    While rdr.Read()
                        Dim worklist As String() = {rdr.Item(0).ToString, _
                                                    rdr.Item(1).ToString, _
                                                    rdr.Item(2).ToString, _
                                                    rdr.Item(3).ToString, _
                                                    rdr.Item(4).ToString, _
                                                    rdr.Item(5).ToString}
                        GetNodeAttributes = worklist
                    End While
                End Using
            End Using
        Catch ex As Exception
            Throw New Exception("(cDataAccess.GetLicensedInstanceList) Exception", ex)
        End Try
    End Function


    Public Function GetArchivedDatabasesList(ByVal Instance As String, Optional ByVal sConnectionString As String = "") As String()
        Try
            Dim worklist() As String = New String() {}
            Using cn As New System.Data.SqlClient.SqlConnection(If(sConnectionString = "", _
                                                       LocalRepositoryConnectionString, _
                                                       sConnectionString))
                cn.Open()
                Using cm As New System.Data.SqlClient.SqlCommand
                    cm.Connection = cn
                    cm.CommandType = CommandType.StoredProcedure
                    cm.CommandText = "SQLCfg.pChangeSelectDatabaseListForInstance"
                    Dim SQLInstance As New SqlParameter()
                    With SQLInstance
                        .Direction = ParameterDirection.Input
                        .ParameterName = "@SQLInstance"
                        .SqlDbType = SqlDbType.NVarChar
                        .Size = 128
                        .Value = Instance
                        cm.Parameters.Add(SQLInstance)
                    End With
                    Dim rdr As SqlDataReader = cm.ExecuteReader()
                    ReDim worklist(0)
                    worklist(0) = ""
                    Dim i As Int32 = 1
                    While rdr.Read()
                        ReDim Preserve worklist(i)
                        worklist(i) = rdr.Item("DatabaseName").ToString
                        i += 1
                    End While
                End Using
            End Using
            GetArchivedDatabasesList = worklist
        Catch ex As Exception
            Throw New Exception("(cDataAccess.GetArchivedDatabasesList) Exception", ex)
        End Try
    End Function


    Public Function GetLatestVersionForNode(ByVal Node As String, Optional ByVal sConnectionString As String = "") As Int32
        Try
            Using cn As New System.Data.SqlClient.SqlConnection(If(sConnectionString = "", _
                                                       LocalRepositoryConnectionString, _
                                                       sConnectionString))
                cn.Open()
                ' an outrageous example of embedding a hint in compiled code
                Using cm As New System.Data.SqlClient.SqlCommand
                    cm.Connection = cn
                    cm.CommandType = CommandType.Text
                    cm.CommandText = "SELECT SQLCfg.fLastVersion ('" & Node + "')"
                    GetLatestVersionForNode = CInt(cm.ExecuteScalar())
                End Using
            End Using
        Catch ex As Exception
            Throw New Exception("(cDataAccess.GetLatestVersionForNode) Exception", ex)
        End Try
    End Function

    Public Function GetHierarchyNodesBySQLInstance(ByVal InstanceName As String, _
                                                   Optional ByVal sConnectionString As String = "") As DataTable
        Try
            Dim tbl As DataTable = New DataTable("Nodes")
            Dim clm1 As DataColumn = New DataColumn
            clm1.DataType = System.Type.GetType("System.Int32")
            clm1.ColumnName = "ChangeId"
            tbl.Columns.Add(clm1)
            Dim clm2 As DataColumn = New DataColumn
            clm2.DataType = System.Type.GetType("System.String")
            clm2.ColumnName = "TreeViewNodePath"
            tbl.Columns.Add(clm2)
            Dim clm3 As DataColumn = New DataColumn
            clm2.DataType = System.Type.GetType("System.Int32")
            clm2.ColumnName = "Version"
            tbl.Columns.Add(clm3)
            Using cn As New System.Data.SqlClient.SqlConnection(If(sConnectionString = "", _
                                                       LocalRepositoryConnectionString, _
                                                       sConnectionString))
                cn.Open()
                Using cm As New System.Data.SqlClient.SqlCommand
                    cm.Connection = cn
                    cm.CommandType = CommandType.StoredProcedure
                    cm.CommandText = "SQLCfg.pChangeGetLatestHierarchyForInstance"
                    Dim SQLInstance As New SqlParameter()
                    With SQLInstance
                        .Direction = ParameterDirection.Input
                        .ParameterName = "@SQLInstance"
                        .SqlDbType = SqlDbType.NVarChar
                        .Size = 128
                        .Value = InstanceName
                        cm.Parameters.Add(SQLInstance)
                    End With
                    Using da As New SqlDataAdapter(cm)
                        'If tbl def changed, take a look at copy in CompareForm.LoadTreeFromRepository too
                        da.Fill(tbl)
                    End Using
                End Using
            End Using
            GetHierarchyNodesBySQLInstance = tbl
        Catch ex As Exception
            Throw New Exception("(cDataAccess.GetHierarchyNodesBySQLInstance) Exception", ex)
        End Try
    End Function

    Public Function GetLatestItemsForNode(ByVal Node As String, _
                                          Optional ByVal sConnectionString As String = "") As DataTable
        Try
            Dim tbl As DataTable = New DataTable("Items")
            Dim clm1 As DataColumn = New DataColumn
            clm1.DataType = System.Type.GetType("System.Int32")
            clm1.ColumnName = "ChangeId"
            tbl.Columns.Add(clm1)
            Dim clm2 As DataColumn = New DataColumn
            clm2.DataType = System.Type.GetType("System.String")
            clm2.ColumnName = "TreeViewNodePath"
            tbl.Columns.Add(clm2)
            Dim clm3 As DataColumn = New DataColumn
            clm3.DataType = System.Type.GetType("System.String")
            clm3.ColumnName = "Item"
            tbl.Columns.Add(clm3)
            Dim clm4 As DataColumn = New DataColumn
            clm4.DataType = System.Type.GetType("System.Int32")
            clm4.ColumnName = "Version"
            tbl.Columns.Add(clm4)
            Using cn As New System.Data.SqlClient.SqlConnection(If(sConnectionString = "", _
                                                       LocalRepositoryConnectionString, _
                                                       sConnectionString))
                cn.Open()
                Using cm As New System.Data.SqlClient.SqlCommand
                    cm.Connection = cn
                    cm.CommandType = CommandType.StoredProcedure
                    cm.CommandText = "SQLCfg.pChangeGetLatestItemsForNode"
                    Dim ParentNode As New SqlParameter()
                    With ParentNode
                        .Direction = ParameterDirection.Input
                        .ParameterName = "@Node"
                        .SqlDbType = SqlDbType.NVarChar
                        .Size = 128
                        .Value = Node
                        cm.Parameters.Add(ParentNode)
                    End With
                    Using da As New SqlDataAdapter(cm)
                        da.Fill(tbl)
                    End Using
                End Using
            End Using
            GetLatestItemsForNode = tbl
        Catch ex As Exception
            Throw New Exception("(cDataAccess.GetLatestItemsForNode) Exception", ex)
        End Try
    End Function

    Public Function GetNodesForDateRange(ByVal BeginDt As DateTime, _
                                         ByVal EndDt As DateTime) As DataTable
        Try
            Dim tbl As DataTable = New DataTable("Items")
            Dim clm1 As DataColumn = New DataColumn
            clm1.DataType = System.Type.GetType("System.Int32")
            clm1.ColumnName = "ChangeId"
            tbl.Columns.Add(clm1)
            Dim clm2 As DataColumn = New DataColumn
            clm2.DataType = System.Type.GetType("System.String")
            clm2.ColumnName = "SQLInstance"
            tbl.Columns.Add(clm2)
            Dim clm3 As DataColumn = New DataColumn
            clm3.DataType = System.Type.GetType("System.String")
            clm3.ColumnName = "Action"
            tbl.Columns.Add(clm3)
            Dim clm4 As DataColumn = New DataColumn
            clm4.DataType = System.Type.GetType("System.DateTime")
            clm4.ColumnName = "RecordDate"
            tbl.Columns.Add(clm4)
            Using cn As New System.Data.SqlClient.SqlConnection(LocalRepositoryConnectionString)
                cn.Open()
                Using cm As New System.Data.SqlClient.SqlCommand
                    cm.Connection = cn
                    cm.CommandType = CommandType.StoredProcedure
                    cm.CommandText = "SQLCfg.pChangeSelectNodesForDateRange"
                    Dim FirstDay As New SqlParameter()
                    With FirstDay
                        .Direction = ParameterDirection.Input
                        .ParameterName = "@BeginDt"
                        .SqlDbType = SqlDbType.DateTime
                        .Value = BeginDt
                        cm.Parameters.Add(FirstDay)
                    End With
                    Dim LastDay As New SqlParameter()
                    With LastDay
                        .Direction = ParameterDirection.Input
                        .ParameterName = "@EndDt"
                        .SqlDbType = SqlDbType.DateTime
                        .Value = EndDt
                        cm.Parameters.Add(LastDay)
                    End With
                    Using da As New SqlDataAdapter(cm)
                        da.Fill(tbl)
                    End Using
                End Using
            End Using
            GetNodesForDateRange = tbl
        Catch ex As Exception
            Throw New Exception("(cDataAccess.GetNodesForDateRange) Exception", ex)
        End Try
    End Function

    Public Function GetConfigurationCatalog() As DataTable
        Try
            Dim tbl As DataTable = New DataTable("DailyChangeSummary")
            Dim SQLInstance As DataColumn = New DataColumn
            SQLInstance.DataType = System.Type.GetType("System.String")
            SQLInstance.ColumnName = "SQLInstance"
            tbl.Columns.Add(SQLInstance)
            Dim Database As DataColumn = New DataColumn
            Database.DataType = System.Type.GetType("System.String")
            Database.ColumnName = "Database"
            tbl.Columns.Add(Database)
            Dim Type As DataColumn = New DataColumn
            Type.DataType = System.Type.GetType("System.String")
            Type.ColumnName = "Type"
            tbl.Columns.Add(Type)
            Dim SubType As DataColumn = New DataColumn
            SubType.DataType = System.Type.GetType("System.String")
            SubType.ColumnName = "SubType"
            tbl.Columns.Add(SubType)
            Dim Collection As DataColumn = New DataColumn
            Collection.DataType = System.Type.GetType("System.String")
            Collection.ColumnName = "Collection"
            tbl.Columns.Add(Collection)
            Dim Item As DataColumn = New DataColumn
            Item.DataType = System.Type.GetType("System.String")
            Item.ColumnName = "Item"
            tbl.Columns.Add(Item)
            Dim Version As DataColumn = New DataColumn
            Version.DataType = System.Type.GetType("System.Int32")
            Version.ColumnName = "Version"
            tbl.Columns.Add(Version)
            Dim [Node] As DataColumn = New DataColumn
            [Node].DataType = System.Type.GetType("System.String")
            [Node].ColumnName = "Node"
            tbl.Columns.Add([Node])
            Using cn As New System.Data.SqlClient.SqlConnection(LocalRepositoryConnectionString)
                cn.Open()
                Using cm As New System.Data.SqlClient.SqlCommand
                    cm.Connection = cn
                    cm.CommandType = CommandType.StoredProcedure
                    cm.CommandText = "SQLCfg.pConfigurationCatalog"
                    Using da As New SqlDataAdapter(cm)
                        da.Fill(tbl)
                        GetConfigurationCatalog = tbl
                    End Using
                End Using
            End Using
        Catch ex As Exception
            Throw New Exception("(cDataAccess.GetConfigurationCatalog) Exception", ex)
        End Try
    End Function

    Public Function GetScheduledTasksWithLastNbrItemsProcessed(Optional ByVal sConnectionString As String = "") As DataTable
        Try
            Dim tbl As DataTable = New DataTable("Schedule")
            Dim [Id] As DataColumn = New DataColumn
            [Id].DataType = System.Type.GetType("System.Int32")
            [Id].ColumnName = "Id"
            tbl.Columns.Add([Id])
            Dim InstanceName As DataColumn = New DataColumn
            InstanceName.DataType = System.Type.GetType("System.String")
            InstanceName.ColumnName = "InstanceName"
            tbl.Columns.Add(InstanceName)
            Dim Interval As DataColumn = New DataColumn
            Interval.DataType = System.Type.GetType("System.Int32")
            Interval.ColumnName = "Interval"
            tbl.Columns.Add(Interval)
            Dim IntervalType As DataColumn = New DataColumn
            IntervalType.DataType = System.Type.GetType("System.String")
            IntervalType.ColumnName = "IntervalType"
            tbl.Columns.Add(IntervalType)
            Dim IntervalBaseDt As DataColumn = New DataColumn
            IntervalBaseDt.DataType = System.Type.GetType("System.DateTime")
            IntervalBaseDt.ColumnName = "IntervalBaseDt"
            tbl.Columns.Add(IntervalBaseDt)
            Dim UseEventNotifications As DataColumn = New DataColumn
            UseEventNotifications.DataType = System.Type.GetType("System.Boolean")
            UseEventNotifications.ColumnName = "UseEventNotifications"
            tbl.Columns.Add(UseEventNotifications)
            Dim IsActive As DataColumn = New DataColumn
            IsActive.DataType = System.Type.GetType("System.Boolean")
            IsActive.ColumnName = "IsActive"
            tbl.Columns.Add(IsActive)
            Dim LastNbrItemsProcessed As DataColumn = New DataColumn
            LastNbrItemsProcessed.DataType = System.Type.GetType("System.Int32")
            LastNbrItemsProcessed.ColumnName = "LastNbrItemsProcessed"
            tbl.Columns.Add(LastNbrItemsProcessed)
            Using cn As New System.Data.SqlClient.SqlConnection(If(sConnectionString = "", _
                                                       LocalRepositoryConnectionString, _
                                                       sConnectionString))
                cn.Open()
                Using cm As New System.Data.SqlClient.SqlCommand
                    cm.Connection = cn
                    cm.CommandType = CommandType.StoredProcedure
                    cm.CommandText = "SQLCfg.pScheduleSelectAllWithLastNbrItemsProcessed"
                    Using da As New SqlDataAdapter(cm)
                        da.Fill(tbl)
                    End Using
                End Using
            End Using
            GetScheduledTasksWithLastNbrItemsProcessed = tbl
        Catch ex As Exception
            Throw New Exception("(cDataAccess.GetScheduledTasksWithLastNbrItemsProcessed) Exception", ex)
        End Try
    End Function

    Public Function GetChangesForDate(ByVal ChangeDate As Date, _
                                      ByVal RootNode As String, _
                                      Optional ByVal sConnectionString As String = "") As DataTable
        Try
            Dim tbl As DataTable = New DataTable("ChangesForDate")
            Dim Node As DataColumn = New DataColumn
            Node.DataType = System.Type.GetType("System.String")
            Node.ColumnName = "Node"
            tbl.Columns.Add([Node])
            Dim Type As DataColumn = New DataColumn
            Type.DataType = System.Type.GetType("System.String")
            Type.ColumnName = "Type"
            tbl.Columns.Add(Type)
            Dim Collection As DataColumn = New DataColumn
            Collection.DataType = System.Type.GetType("System.String")
            Collection.ColumnName = "Collection"
            tbl.Columns.Add(Collection)
            Dim Item As DataColumn = New DataColumn
            Item.DataType = System.Type.GetType("System.String")
            Item.ColumnName = "Item"
            tbl.Columns.Add(Item)
            Dim Version As DataColumn = New DataColumn
            Version.DataType = System.Type.GetType("System.Int32")
            Version.ColumnName = "Version"
            tbl.Columns.Add(Version)
            Dim Action As DataColumn = New DataColumn
            Action.DataType = System.Type.GetType("System.String")
            Action.ColumnName = "Action"
            tbl.Columns.Add(Action)
            Dim [Date] As DataColumn = New DataColumn
            [Date].DataType = System.Type.GetType("System.DateTime")
            [Date].ColumnName = "Date"
            tbl.Columns.Add([Date])
            Using cn As New System.Data.SqlClient.SqlConnection(If(sConnectionString = "", _
                                                       LocalRepositoryConnectionString, _
                                                       sConnectionString))
                cn.Open()
                Using cm As New System.Data.SqlClient.SqlCommand
                    cm.Connection = cn
                    cm.CommandType = CommandType.StoredProcedure
                    cm.CommandText = "SQLCfg.pChangesForDate"
                    Dim ReportDate As New SqlParameter()
                    With ReportDate
                        .Direction = ParameterDirection.Input
                        .ParameterName = "@ChangeDate"
                        .SqlDbType = SqlDbType.DateTime
                        .Value = ChangeDate
                        cm.Parameters.Add(ReportDate)
                    End With
                    Dim ParentNode As New SqlParameter()
                    With ParentNode
                        .Direction = ParameterDirection.Input
                        .ParameterName = "@Node"
                        .SqlDbType = SqlDbType.NVarChar
                        .Size = 128
                        .Value = RootNode
                        cm.Parameters.Add(ParentNode)
                    End With
                    Using da As New SqlDataAdapter(cm)
                        da.Fill(tbl)
                        GetChangesForDate = tbl
                    End Using
                End Using
            End Using
        Catch ex As Exception
            Throw New Exception("(cDataAccess.GetChangesForDate) Exception", ex)
        End Try
    End Function

    Public Function GetChangesBySearchString(ByVal SearchString As String, _
                                             ByVal LatestVersion As Boolean, _
                                             ByVal NodeType As String) As DataTable
        Try
            Dim tbl As DataTable = New DataTable("ChangesBySearchString")
            Dim SQLInstance As DataColumn = New DataColumn
            SQLInstance.DataType = System.Type.GetType("System.String")
            SQLInstance.ColumnName = "SQLInstance"
            tbl.Columns.Add([SQLInstance])
            Dim Type As DataColumn = New DataColumn
            Type.DataType = System.Type.GetType("System.String")
            Type.ColumnName = "Type"
            tbl.Columns.Add(Type)
            Dim SubType As DataColumn = New DataColumn
            SubType.DataType = System.Type.GetType("System.String")
            SubType.ColumnName = "SubType"
            tbl.Columns.Add(SubType)
            Dim Database As DataColumn = New DataColumn
            Database.DataType = System.Type.GetType("System.String")
            Database.ColumnName = "Database"
            tbl.Columns.Add(Database)
            Dim Collection As DataColumn = New DataColumn
            Collection.DataType = System.Type.GetType("System.String")
            Collection.ColumnName = "Collection"
            tbl.Columns.Add(Collection)
            Dim Item As DataColumn = New DataColumn
            Item.DataType = System.Type.GetType("System.String")
            Item.ColumnName = "Item"
            tbl.Columns.Add(Item)
            Dim Version As DataColumn = New DataColumn
            Version.DataType = System.Type.GetType("System.Int32")
            Version.ColumnName = "Version"
            tbl.Columns.Add(Version)
            Dim Node As DataColumn = New DataColumn
            Node.DataType = System.Type.GetType("System.String")
            Node.ColumnName = "Node"
            tbl.Columns.Add(Node)
            Using cn As New System.Data.SqlClient.SqlConnection(LocalRepositoryConnectionString)
                cn.Open()
                Using cm As New System.Data.SqlClient.SqlCommand
                    cm.Connection = cn
                    cm.CommandType = CommandType.StoredProcedure
                    cm.CommandText = "SQLCfg.pChangeSelectByContains"
                    Dim SearchStr As New SqlParameter()
                    'wrapped in double quotes by caller
                    With SearchStr
                        .Direction = ParameterDirection.Input
                        .ParameterName = "@SearchString"
                        .SqlDbType = SqlDbType.NVarChar
                        .Size = 4000
                        .Value = SearchString
                    End With
                    cm.Parameters.Add(SearchStr)
                    Dim LatestVer As New SqlParameter()
                    With LatestVer
                        .Direction = ParameterDirection.Input
                        .ParameterName = "@LatestVersion"
                        .SqlDbType = SqlDbType.Bit
                        .Value = LatestVersion
                    End With
                    cm.Parameters.Add(LatestVer)
                    Dim Typ As New SqlParameter()
                    With Typ
                        .Direction = ParameterDirection.Input
                        .ParameterName = "@Type"
                        .SqlDbType = SqlDbType.NVarChar
                        .Size = 12
                        .Value = NodeType
                    End With
                    cm.Parameters.Add(Typ)
                    Using da As New SqlDataAdapter(cm)
                        da.Fill(tbl)
                        GetChangesBySearchString = tbl
                    End Using
                End Using
            End Using
        Catch ex As Exception
            Throw New Exception("(cDataAccess.GetChangesBySearchString) Exception", ex)
        End Try
    End Function

    Public Function GetSchedule(Optional ByVal sConnectionString As String = "") As DataTable
        Try
            Dim tbl As DataTable = New DataTable("Schedule")
            Dim [Id] As DataColumn = New DataColumn
            [Id].DataType = System.Type.GetType("System.Int32")
            [Id].ColumnName = "Id"
            tbl.Columns.Add([Id])
            Dim InstanceName As DataColumn = New DataColumn
            InstanceName.DataType = System.Type.GetType("System.String")
            InstanceName.ColumnName = "InstanceName"
            tbl.Columns.Add(InstanceName)
            Dim Interval As DataColumn = New DataColumn
            Interval.DataType = System.Type.GetType("System.Int32")
            Interval.ColumnName = "Interval"
            tbl.Columns.Add(Interval)
            Dim IntervalType As DataColumn = New DataColumn
            IntervalType.DataType = System.Type.GetType("System.String")
            IntervalType.ColumnName = "IntervalType"
            tbl.Columns.Add(IntervalType)
            Dim IntervalBaseDt As DataColumn = New DataColumn
            IntervalBaseDt.DataType = System.Type.GetType("System.DateTime")
            IntervalBaseDt.ColumnName = "IntervalBaseDt"
            tbl.Columns.Add(IntervalBaseDt)
            Dim UseEventNotifications As DataColumn = New DataColumn
            UseEventNotifications.DataType = System.Type.GetType("System.Boolean")
            UseEventNotifications.ColumnName = "UseEventNotifications"
            tbl.Columns.Add(UseEventNotifications)
            Dim IsActive As DataColumn = New DataColumn
            IsActive.DataType = System.Type.GetType("System.Boolean")
            IsActive.ColumnName = "IsActive"
            tbl.Columns.Add(IsActive)
            Using cn As New System.Data.SqlClient.SqlConnection(If(sConnectionString = "", _
                                                       LocalRepositoryConnectionString, _
                                                       sConnectionString))
                cn.Open()
                Using cm As New System.Data.SqlClient.SqlCommand
                    cm.Connection = cn
                    cm.CommandType = CommandType.StoredProcedure
                    cm.CommandText = "SQLCfg.pScheduleSelectAll"
                    Using da As New SqlDataAdapter(cm)
                        da.Fill(tbl)
                    End Using
                End Using
            End Using
            GetSchedule = tbl
        Catch ex As Exception
            Throw New Exception("(cDataAccess.GetSchedule) Exception", ex)
        End Try
    End Function

    Public Function GetScheduleCountForInstance(ByVal InstanceName As String) As Integer
        Try
            Using cn As New System.Data.SqlClient.SqlConnection(LocalRepositoryConnectionString)
                cn.Open()
                Using cm As New System.Data.SqlClient.SqlCommand
                    cm.Connection = cn
                    cm.CommandType = CommandType.StoredProcedure
                    cm.CommandText = "SQLCfg.pScheduleGetCountForInstance"
                    Dim SQLInstance As New SqlParameter()
                    With SQLInstance
                        .Direction = ParameterDirection.Input
                        .ParameterName = "@InstanceName"
                        .SqlDbType = SqlDbType.NVarChar
                        .Size = 128
                        .Value = InstanceName
                    End With
                    cm.Parameters.Add(SQLInstance)
                    Dim NbrSchedules As New SqlParameter()
                    With NbrSchedules
                        .Direction = ParameterDirection.Output
                        .ParameterName = "@Count"
                        .SqlDbType = SqlDbType.Int
                    End With
                    cm.Parameters.Add(NbrSchedules)
                    cm.ExecuteNonQuery()
                    GetScheduleCountForInstance = CInt(NbrSchedules.Value)
                End Using
            End Using
        Catch ex As Exception
            Throw New Exception("(cDataAccess.GetScheduleCountForInstance) Exception", ex)
        End Try
    End Function

    Public Function GetLastDefinitionWithId(ByVal Node As String, _
                                            Optional ByVal sConnectionString As String = "") As Object()
        Try
            Using cn As New System.Data.SqlClient.SqlConnection(If(sConnectionString = "", _
                                                       LocalRepositoryConnectionString, _
                                                       sConnectionString))
                cn.Open()
                Using cm As New System.Data.SqlClient.SqlCommand
                    Dim LastDefinition As String = Nothing
                    cm.Connection = cn
                    cm.CommandType = CommandType.StoredProcedure
                    cm.CommandText = "SQLCfg.pChangeLatestByItem"
                    Dim ChangeNode As New SqlParameter()
                    With ChangeNode
                        .Direction = ParameterDirection.Input
                        .ParameterName = "@Node"
                        .SqlDbType = SqlDbType.NVarChar
                        .Size = 786
                        .Value = Node
                    End With
                    cm.Parameters.Add(ChangeNode)
                    Dim Id As New SqlParameter()
                    With Id
                        .Direction = ParameterDirection.Output
                        .ParameterName = "@Id"
                        .SqlDbType = SqlDbType.Int
                    End With
                    cm.Parameters.Add(Id)
                    Dim Definition As New SqlParameter()
                    With Definition
                        .Direction = ParameterDirection.Output
                        .ParameterName = "@Definition"
                        .SqlDbType = SqlDbType.NVarChar
                        .Size = -1
                    End With
                    cm.Parameters.Add(Definition)
                    cm.ExecuteNonQuery()
                    Dim obj As Object() = {Id.Value, Definition.Value.ToString}
                    Return obj
                End Using
            End Using
        Catch ex As Exception
            Throw New Exception("(cDataAccess.GetLastDefinitionWithId) Exception", ex)
        End Try
    End Function

    Public Function GetInstanceNameByScheduleId(ByVal ScheduleId As Integer, _
                                                Optional ByVal sConnectionString As String = "") As String
        Try
            Using cn As New System.Data.SqlClient.SqlConnection(If(sConnectionString = "", _
                                                       LocalRepositoryConnectionString, _
                                                       sConnectionString))
                cn.Open()
                Using cm As New System.Data.SqlClient.SqlCommand
                    cm.Connection = cn
                    cm.CommandType = CommandType.StoredProcedure
                    cm.CommandText = "SQLCfg.pScheduleGetInstanceNameById"
                    Dim Id As New SqlParameter()
                    With Id
                        .Direction = ParameterDirection.Input
                        .ParameterName = "@Id"
                        .SqlDbType = SqlDbType.Int
                        .Value = ScheduleId
                    End With
                    cm.Parameters.Add(Id)
                    Dim SQLInstance As New SqlParameter()
                    With SQLInstance
                        .Direction = ParameterDirection.Output
                        .ParameterName = "@InstanceName"
                        .SqlDbType = SqlDbType.NVarChar
                        .Size = 128
                    End With
                    cm.Parameters.Add(SQLInstance)
                    cm.ExecuteNonQuery()
                    GetInstanceNameByScheduleId = SQLInstance.Value.ToString
                End Using
            End Using
        Catch ex As Exception
            Throw New Exception("(cDataAccess.GetInstanceNameByScheduleId) Exception", ex)
        End Try
    End Function

    Public Function GetLastNodeCount(ByVal Node As String, _
                                     Optional ByVal sConnectionString As String = "") As Integer
        Try
            Using cn As New System.Data.SqlClient.SqlConnection(If(sConnectionString = "", _
                                                       LocalRepositoryConnectionString, _
                                                       sConnectionString))
                cn.Open()
                Using cm As New System.Data.SqlClient.SqlCommand
                    Dim InstanceName As String = Nothing
                    cm.Connection = cn
                    cm.CommandType = CommandType.StoredProcedure
                    cm.CommandText = "SQLCfg.pChangeGetLastCountByNodeParent"
                    Dim n As New SqlParameter()
                    With n
                        .Direction = ParameterDirection.Input
                        .ParameterName = "@NodeParent"
                        .SqlDbType = SqlDbType.NVarChar
                        .Size = 786
                        .Value = Node
                    End With
                    cm.Parameters.Add(n)
                    Dim ct As New SqlParameter()
                    With ct
                        .Direction = ParameterDirection.Output
                        .ParameterName = "@NbrDescendents"
                        .SqlDbType = SqlDbType.Int
                    End With
                    cm.Parameters.Add(ct)
                    cm.ExecuteNonQuery()
                    GetLastNodeCount = CInt(ct.Value)
                End Using
            End Using
        Catch ex As Exception
            Throw New Exception("(cDataAccess.GetLastNodeCount) Exception", ex)
        End Try
    End Function

    Public Function GetItemList(ByVal NodeParent As String, _
                                Optional ByVal sConnectionString As String = "") As String()
        Try
            Dim worklist() As String = New String() {}
            Using cn As New System.Data.SqlClient.SqlConnection(If(sConnectionString = "", _
                                                       LocalRepositoryConnectionString, _
                                                       sConnectionString))
                cn.Open()
                Using cm As New System.Data.SqlClient.SqlCommand
                    cm.Connection = cn
                    cm.CommandType = CommandType.StoredProcedure
                    cm.CommandText = "SQLCfg.pChangeSelectLatestByNodeParent"
                    Dim Parent As New SqlParameter()
                    With Parent
                        .Direction = ParameterDirection.Input
                        .ParameterName = "@NodeParent"
                        .SqlDbType = SqlDbType.NVarChar
                        .Size = 786
                        .Value = NodeParent
                    End With
                    cm.Parameters.Add(Parent)
                    Dim rdr As SqlDataReader = cm.ExecuteReader()
                    Dim i As Int32 = 0
                    While rdr.Read()
                        ReDim Preserve worklist(i)
                        worklist(i) = rdr.Item(0).ToString
                        i += 1
                    End While
                End Using
            End Using
            GetItemList = worklist
        Catch ex As Exception
            Throw New Exception("(cDataAccess.GetItemList) Exception", ex)
        End Try
    End Function

    Public Function GetSchedulePathList(ByVal SQLInstance As String, _
                                        Optional ByVal sConnectionString As String = "") As String()
        Try
            Dim worklist() As String = New String() {}
            Using cn As New System.Data.SqlClient.SqlConnection(If(sConnectionString = "", _
                                                       LocalRepositoryConnectionString, _
                                                       sConnectionString))
                cn.Open()
                Using cm As New System.Data.SqlClient.SqlCommand
                    cm.Connection = cn
                    cm.CommandType = CommandType.StoredProcedure
                    cm.CommandText = "SQLCfg.pChangeSelectScheduleListForInstance"
                    Dim Instance As New SqlParameter()
                    With Instance
                        .Direction = ParameterDirection.Input
                        .ParameterName = "@SQLInstance"
                        .SqlDbType = SqlDbType.NVarChar
                        .Size = 128
                        .Value = SQLInstance
                    End With
                    cm.Parameters.Add(Instance)
                    Dim rdr As SqlDataReader = cm.ExecuteReader()
                    Dim i As Int32 = 0
                    While rdr.Read()
                        ReDim Preserve worklist(i)
                        worklist(i) = rdr.Item(0).ToString
                        i += 1
                    End While
                End Using
            End Using
            GetSchedulePathList = worklist
        Catch ex As Exception
            Throw New Exception("(cDataAccess.GetItemList) Exception", ex)
        End Try
    End Function

    Public Sub AddChangeDefinition(ByVal Node As String, _
                         ByVal Action As String, _
                         ByVal Definition As StringCollection, _
                         Optional ByVal EventData As SqlTypes.SqlXml = Nothing, _
                         Optional ByRef ChangeId As Integer = 0, _
                         Optional ByRef Version As Integer = 0)
        Try
            ' make one string from the collection for insert to db
            Dim Def As String = ""
            If Definition.Count > 0 Then
                Def = Definition(0)
                If Definition.Count > 1 Then
                    For i As Integer = 1 To Definition.Count - 1
                        Def = Def & vbCrLf & Definition(i)
                    Next
                End If
            End If
            Using cn As New System.Data.SqlClient.SqlConnection(LocalRepositoryConnectionString)
                cn.Open()
                Using cm As New System.Data.SqlClient.SqlCommand
                    cm.Connection = cn
                    cm.CommandType = CommandType.StoredProcedure
                    cm.CommandText = "SQLCfg.pChangeInsert"
                    Dim parmNode As New SqlParameter()
                    With parmNode
                        .Direction = ParameterDirection.Input
                        .ParameterName = "@Node"
                        .SqlDbType = SqlDbType.NVarChar
                        .Size = 786
                        .Value = Node
                        cm.Parameters.Add(parmNode)
                    End With
                    Dim parmAction As New SqlParameter()
                    With parmAction
                        .Direction = ParameterDirection.Input
                        .ParameterName = "@Action"
                        .SqlDbType = SqlDbType.NVarChar
                        .Size = 30
                        .Value = Action
                        cm.Parameters.Add(parmAction)
                    End With
                    Dim parmEventData As New SqlParameter()
                    With parmEventData
                        .Direction = ParameterDirection.Input
                        .ParameterName = "@EventData"
                        .SqlDbType = SqlDbType.Xml
                        .Size = -1
                        .Value = If(EventData Is Nothing, SqlTypes.SqlXml.Null, EventData)
                        cm.Parameters.Add(parmEventData)
                    End With
                    Dim parmDefinition As New SqlParameter()
                    With parmDefinition
                        .Direction = ParameterDirection.Input
                        .ParameterName = "@Definition"
                        .SqlDbType = SqlDbType.NVarChar
                        .Size = -1
                        .Value = Def
                        cm.Parameters.Add(parmDefinition)
                    End With
                    Dim parmChangeId As New SqlParameter()
                    With parmChangeId
                        .Direction = ParameterDirection.Output
                        .ParameterName = "@ChangeId"
                        .SqlDbType = SqlDbType.Int
                        cm.Parameters.Add(parmChangeId)
                    End With
                    Dim parmVersion As New SqlParameter()
                    With parmVersion
                        .Direction = ParameterDirection.Output
                        .ParameterName = "@Version"
                        .SqlDbType = SqlDbType.Int
                        cm.Parameters.Add(parmVersion)
                    End With
                    cm.ExecuteNonQuery()
                    ChangeId = CInt(parmChangeId.Value)
                    Version = CInt(parmVersion.Value)
                End Using
            End Using
        Catch ex As Exception
            Throw New Exception("(cDataAccess.AddChangeDefinition) Exception", ex)
        End Try
    End Sub

    Public Function RunSQLScript(ByVal TargetInstance As Server, _
                                 ByVal DatabaseName As String, _
                                 ByVal sScriptFileName As String) As Boolean
        RunSQLScript = False
        Dim strcolScript As ArrayList
        Try
            strcolScript = GetInstallScript(sScriptFileName)
            ' if a batch fails, fail the execution of the script and log the error
            'TargetInstance.ConnectionContext.SqlConnectionObject.FireInfoMessageEventOnUserErrors = True
            For Each obj As Object In strcolScript
                Try
                    TargetInstance.Databases(DatabaseName).ExecuteNonQuery(obj.ToString)
                Catch sqlex As SqlException
                    Throw New Exception(String.Format("The {0} has received a severity {1}," & _
                                        "state {2} error number {3}\n" & _
                                        "on line {4} of batch {5} of script {6} on server {7}:\n{8}", _
                                        sqlex.Source, sqlex.Class, sqlex.State, sqlex.Number, sqlex.LineNumber, _
                                        obj.ToString, sScriptFileName, sqlex.Server, sqlex.Message))
                End Try
            Next
            RunSQLScript = True
        Catch ex As Exception
            Throw New Exception(String.Format("(cDataAccess.RunSQLScript) Script [{0}] Exception.", sScriptFileName), ex)
        End Try
    End Function

    Friend Function GetInstallScript(ByVal sSourceFile As String) As ArrayList
        Try
            'assumes it will be in same folder as executable or name is provided as full path 
            Using rdr As New System.IO.StreamReader(sSourceFile)
                Dim strs As New ArrayList
                Dim pre As String = ""
                Dim str As String = ""
                Dim line As String
                line = rdr.ReadLine()
                While Not line Is Nothing
                    If Not (UCase(line) = "GO") Then
                        str = str + vbCrLf + line
                    Else
                        strs.Add(str)
                        str = pre
                    End If
                    line = rdr.ReadLine()
                End While
                GetInstallScript = strs
            End Using ' is this enough cleanup, too much?
        Catch ex As Exception
            GetInstallScript = Nothing
            Throw New Exception(String.Format("(cDataAccess.GetInstallScript) Error loading script {0} from SQLClue application folder.", sSourceFile), ex)
        End Try
    End Function

    Friend Function GetDDLEvent(ByVal TargetEventNotificationConnectionString As String) As SqlTypes.SqlXml
        'if any notifications in queue, process one at a time, caller must supply transaction, 
        'commit only when the message is sucessfully processed

        'There could be multiple events for an object that the user may consider to be only one change.
        'Filter these in reporting if necessary, DO NOT THROW ANY EVENTS ON THE FLOOR HERE
        'Query returns only the last unarchived event for each object 
        Try
            Using cn As New System.Data.SqlClient.SqlConnection(TargetEventNotificationConnectionString)
                cn.Open()
                Using cm As New System.Data.SqlClient.SqlCommand
                    cm.Connection = cn
                    cm.CommandType = CommandType.StoredProcedure
                    cm.CommandText = "dbo.pSQLClueGetDDLEvent"
                    ' size must be any positive value?
                    Dim EventData As New SqlParameter()
                    With EventData
                        .Direction = ParameterDirection.Output
                        .ParameterName = "@EventData"
                        .SqlDbType = SqlDbType.Xml
                        .Size = 1
                    End With
                    cm.Parameters.Add(EventData)
                    cm.ExecuteNonQuery()
                    GetDDLEvent = CType(EventData.SqlValue, SqlTypes.SqlXml)
                End Using
            End Using
        Catch ex As Exception
            Throw New Exception("(cDataAccess.GetDDLEvent) Exception", ex)
        End Try
    End Function

    Friend Sub AppendDDLEvent(ByVal ChangeId As Integer, _
                              ByVal DDLEvent As SqlTypes.SqlXml, _
                              ByVal Action As String)
        Try
            Using cn As New System.Data.SqlClient.SqlConnection(LocalRepositoryConnectionString)
                cn.Open()
                Using cm As New System.Data.SqlClient.SqlCommand
                    cm.Connection = cn
                    cm.CommandType = CommandType.StoredProcedure
                    cm.CommandText = "SQLCfg.pChangeAppendEvent"
                    Dim BaseNode As New SqlParameter()
                    With BaseNode
                        .Direction = ParameterDirection.Input
                        .ParameterName = "@ChangeId"
                        .SqlDbType = SqlDbType.Int
                        .Value = ChangeId
                    End With
                    cm.Parameters.Add(BaseNode)
                    Dim EventData As New SqlParameter()
                    With EventData
                        .Direction = ParameterDirection.Input
                        .ParameterName = "@EventData"
                        .SqlDbType = SqlDbType.Xml
                        .Size = -1
                        .Value = DDLEvent.Value
                    End With
                    cm.Parameters.Add(EventData)
                    Dim Act As New SqlParameter()
                    With Act
                        .Direction = ParameterDirection.Input
                        .ParameterName = "@Action"
                        .SqlDbType = SqlDbType.NVarChar
                        .Size = 20
                        .Value = Action
                    End With
                    cm.Parameters.Add(Act)
                    cm.ExecuteNonQuery()
                End Using
            End Using
        Catch ex As Exception
            Throw New Exception("(cDataAccess.AppendDDLEvent) Exception", ex)
        End Try
    End Sub

    Public Sub LogArchiveActivity(ByVal ScheduleId As Integer, _
                         ByVal InstanceName As String, _
                         ByVal NbrDDLEventsProcessed As Integer, _
                         ByVal NbrItemsProcessed As Integer, _
                         ByVal NbrItemsAdded As Integer, _
                         ByVal NbrItemsChanged As Integer, _
                         ByVal NbrItemsDeleted As Integer, _
                         ByVal ScheduledStartDt As DateTime, _
                         ByVal ActualStartDt As DateTime, _
                         ByVal ActualEndDt As DateTime, _
                         ByVal ArchiveErrorMsg As String)
        Try
            Using cn As New System.Data.SqlClient.SqlConnection(LocalRepositoryConnectionString)
                cn.Open()
                Using cm As New System.Data.SqlClient.SqlCommand
                    cm.Connection = cn
                    cm.CommandType = CommandType.StoredProcedure
                    cm.CommandText = "SQLCfg.pArchiveLogInsert"
                    Dim Schedule As New SqlParameter()
                    With Schedule
                        .Direction = ParameterDirection.Input
                        .ParameterName = "@ScheduleId"
                        .SqlDbType = SqlDbType.Int
                        .Value = ScheduleId
                        cm.Parameters.Add(Schedule)
                    End With
                    Dim Instance As New SqlParameter()
                    With Instance
                        .Direction = ParameterDirection.Input
                        .ParameterName = "@InstanceName"
                        .SqlDbType = SqlDbType.NVarChar
                        .Size = 128
                        .Value = InstanceName
                        cm.Parameters.Add(Instance)
                    End With
                    Dim parmAction As New SqlParameter()
                    Dim DDLEventsProcessed As New SqlParameter()
                    With DDLEventsProcessed
                        .Direction = ParameterDirection.Input
                        .ParameterName = "@NbrDDLEventsProcessed"
                        .SqlDbType = SqlDbType.Int
                        .Value = NbrDDLEventsProcessed
                        cm.Parameters.Add(DDLEventsProcessed)
                    End With
                    Dim ItemsProcessed As New SqlParameter()
                    With ItemsProcessed
                        .Direction = ParameterDirection.Input
                        .ParameterName = "@NbrItemsProcessed"
                        .SqlDbType = SqlDbType.Int
                        .Value = NbrItemsProcessed
                        cm.Parameters.Add(ItemsProcessed)
                    End With
                    Dim ItemsAdded As New SqlParameter()
                    With ItemsAdded
                        .Direction = ParameterDirection.Input
                        .ParameterName = "@NbrItemsAdded"
                        .SqlDbType = SqlDbType.Int
                        .Value = NbrItemsAdded
                        cm.Parameters.Add(ItemsAdded)
                    End With
                    Dim ItemsChanged As New SqlParameter()
                    With ItemsChanged
                        .Direction = ParameterDirection.Input
                        .ParameterName = "@NbrItemsChanged"
                        .SqlDbType = SqlDbType.Int
                        .Value = NbrItemsChanged
                        cm.Parameters.Add(ItemsChanged)
                    End With
                    Dim ItemsDeleted As New SqlParameter()
                    With ItemsDeleted
                        .Direction = ParameterDirection.Input
                        .ParameterName = "@NbrItemsDeleted"
                        .SqlDbType = SqlDbType.Int
                        .Value = NbrItemsDeleted
                        cm.Parameters.Add(ItemsDeleted)
                    End With
                    Dim ScheduledStart As New SqlParameter()
                    With ScheduledStart
                        .Direction = ParameterDirection.Input
                        .ParameterName = "@ScheduledStartDt"
                        .SqlDbType = SqlDbType.DateTime
                        .Value = ScheduledStartDt
                        cm.Parameters.Add(ScheduledStart)
                    End With
                    Dim ActualStart As New SqlParameter()
                    With ActualStart
                        .Direction = ParameterDirection.Input
                        .ParameterName = "@ActualStartDt"
                        .SqlDbType = SqlDbType.DateTime
                        .Value = ActualStartDt
                        cm.Parameters.Add(ActualStart)
                    End With
                    Dim ActualEnd As New SqlParameter()
                    With ActualEnd
                        .Direction = ParameterDirection.Input
                        .ParameterName = "@ActualEndDt"
                        .SqlDbType = SqlDbType.DateTime
                        .Value = ActualEndDt
                        cm.Parameters.Add(ActualEnd)
                    End With
                    Dim ArchiveError As New SqlParameter()
                    With ArchiveError
                        .Direction = ParameterDirection.Input
                        .ParameterName = "@ArchiveError"
                        .SqlDbType = SqlDbType.VarChar
                        .Size = -1
                        .Value = ArchiveErrorMsg
                        cm.Parameters.Add(ArchiveError)
                    End With
                    cm.ExecuteNonQuery()
                End Using
            End Using
        Catch ex As Exception
            Throw New Exception("(cDataAccess.LogArchiveActivity) Exception", ex)
        End Try
    End Sub

    Public Function AvailableLicenses() As Integer
        AvailableLicenses = 1
    End Function

End Class
