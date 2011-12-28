Imports System.Windows.Forms
Public Class DialogConnect

    Private TargetSQLServer As Server
    Private IsRepositoryConnection As Boolean

    Public ReadOnly Property sConnectionString() As String
        Get
            Dim builder As New System.Data.SqlClient.SqlConnectionStringBuilder
            builder.DataSource = TargetInstanceName.Text
            If IsRepositoryConnection Then
                builder.InitialCatalog = TargetDatabaseName.Text
            End If
            If TargetUseTrustedConnection.Checked Then
                builder.IntegratedSecurity = True
            Else
                builder.UserID = TargetSQLLoginName.Text
                builder.Password = TargetSQLLoginPassword.Text
            End If
            builder.ConnectTimeout = CInt(TargetConnectionTimeout.Value)
            builder.ApplicationName = My.Application.Info.ProductName
            ' all a "not specified" option
            If TargetNetworkProtocol.Text <> "" Then
                builder.NetworkLibrary = TargetNetworkProtocol.Text.Split(CChar(" "))(0)
            End If
            builder.Encrypt = TargetEncryptConnection.Checked
            builder.TrustServerCertificate = TargetTrustServerCertificate.Checked
            builder.Enlist = True
            builder.MultipleActiveResultSets = False
            builder.PersistSecurityInfo = True
            builder.UserInstance = False
            Return builder.ConnectionString
        End Get
    End Property

    ' for compare utility target (no database name)
    Public Overloads Function ShowDialog(ByRef SQLServer As Server, _
                                    ByRef SQLInstance As String, _
                                    ByRef UseTrustedConnection As Boolean, _
                                    ByRef SQLLogin As String, _
                                    ByRef Password As String, _
                                    ByRef ConnectionTimeout As Int32, _
                                    ByRef NetProtocol As String, _
                                    ByRef EncryptCn As Boolean, _
                                    ByRef TrustServer As Boolean, _
                                    ByVal ParentForm As IWin32Window, _
                                    Optional ByVal ConnectTitleText As String = "") As System.Windows.Forms.DialogResult
        Try
            Dim PassedSQLServer As Server = SQLServer
            TargetSQLServer = SQLServer
            TargetInstanceName.Text = SQLInstance
            TargetInstanceName.Tag = SQLInstance
            TargetDatabaseName.Text = ""
            TargetDatabaseName.Tag = ""
            TargetUseTrustedConnection.Checked = UseTrustedConnection
            ' the order of assignment --> value then tag is exploited in the checkchanged event
            TargetUseTrustedConnection.Tag = UseTrustedConnection
            TargetSQLLoginName.Text = SQLLogin
            TargetSQLLoginName.Tag = SQLLogin
            TargetSQLLoginPassword.Text = Password
            TargetSQLLoginPassword.Tag = Password
            TargetConnectionTimeout.Value = ConnectionTimeout
            TargetConnectionTimeout.Tag = ConnectionTimeout
            TargetNetworkProtocol.Text = NetProtocol
            TargetNetworkProtocol.Tag = NetProtocol
            TargetEncryptConnection.Checked = EncryptCn
            TargetEncryptConnection.Tag = EncryptCn
            TargetTrustServerCertificate.Checked = TrustServer
            TargetTrustServerCertificate.Tag = TrustServer
            IsRepositoryConnection = False
            Me.Text = If(ConnectTitleText = "", My.Resources.ConnectToAnySQL, ConnectTitleText)
            Dim result As DialogResult = Me.ShowDialog(ParentForm)

            Select Case result
                Case Windows.Forms.DialogResult.OK

                Case Windows.Forms.DialogResult.Cancel

                Case Else

            End Select

            If result = Windows.Forms.DialogResult.OK Then
                SQLServer = TargetSQLServer
                SQLInstance = TargetInstanceName.Text
                UseTrustedConnection = TargetUseTrustedConnection.Checked
                SQLLogin = TargetSQLLoginName.Text
                Password = TargetSQLLoginPassword.Text
                ConnectionTimeout = CInt(TargetConnectionTimeout.Value)
                NetProtocol = TargetNetworkProtocol.Text
                EncryptCn = TargetEncryptConnection.Checked
                TrustServer = TargetTrustServerCertificate.Checked
            Else
                ' try to put it back the way it was
                ' may be missing the passed server here??? can't seem to return it without creating another
                SQLServer = TargetSQLServer 
                SQLInstance = If(TargetInstanceName.Tag Is Nothing, Nothing, TargetInstanceName.Tag.ToString)
                UseTrustedConnection = CBool(TargetUseTrustedConnection.Tag)
                SQLLogin = TargetSQLLoginName.Tag.ToString
                Password = TargetSQLLoginPassword.Tag.ToString
                ConnectionTimeout = CInt(TargetConnectionTimeout.Tag)
                NetProtocol = TargetNetworkProtocol.Tag.ToString
                EncryptCn = CBool(TargetEncryptConnection.Tag)
                TrustServer = CBool(TargetTrustServerCertificate.Tag)
            End If
            Return result
        Catch ex As Exception
            Throw New Exception(String.Format("({0}.ShowDialog) Exception.", Me.Name), ex)
        End Try
    End Function

    ' for compare utility repository (include database name)
    Public Overloads Function ShowDialogForRepository(ByRef SQLServer As Server, _
                                    ByRef SQLInstance As String, _
                                    ByRef DatabaseName As String, _
                                    ByRef UseTrustedConnection As Boolean, _
                                    ByRef SQLLogin As String, _
                                    ByRef Password As String, _
                                    ByRef ConnectionTimeout As Int32, _
                                    ByRef NetProtocol As String, _
                                    ByRef EncryptCn As Boolean, _
                                    ByRef TrustServer As Boolean, _
                                    ByVal ParentForm As IWin32Window) As System.Windows.Forms.DialogResult
        Try
            Dim PassedSQLServer As Server = SQLServer
            TargetSQLServer = SQLServer
            TargetInstanceName.Text = If(SQLInstance = "", My.Settings.RepositoryInstanceName, SQLInstance)
            TargetInstanceName.Tag = SQLInstance
            TargetDatabaseName.Text = If(SQLInstance = "", My.Settings.RepositoryDatabaseName, DatabaseName)
            TargetDatabaseName.Tag = DatabaseName
            TargetUseTrustedConnection.Checked = If(SQLInstance = "", My.Settings.RepositoryUseTrustedConnection, UseTrustedConnection)
            ' the order of assignment --> value then tag is exploited in the checkchanged event
            TargetUseTrustedConnection.Tag = UseTrustedConnection
            TargetSQLLoginName.Text = If(SQLInstance = "", My.Settings.RepositorySQLLoginName, SQLLogin)
            TargetSQLLoginName.Tag = SQLLogin
            TargetSQLLoginPassword.Text = If(SQLInstance = "", My.Settings.RepositorySQLLoginPassword, Password)
            TargetSQLLoginPassword.Tag = Password
            TargetConnectionTimeout.Value = If(SQLInstance = "", My.Settings.RepositoryConnectionTimeout, ConnectionTimeout)
            TargetConnectionTimeout.Tag = ConnectionTimeout
            TargetNetworkProtocol.Text = If(SQLInstance = "", My.Settings.RepositoryNetworkLibrary, NetProtocol)
            TargetNetworkProtocol.Tag = NetProtocol
            TargetEncryptConnection.Checked = If(SQLInstance = "", My.Settings.RepositoryEncryptConnection, EncryptCn)
            TargetEncryptConnection.Tag = EncryptCn
            TargetTrustServerCertificate.Checked = If(SQLInstance = "", My.Settings.RepositoryTrustServerCertificate, TrustServer)
            TargetTrustServerCertificate.Tag = TrustServer
            IsRepositoryConnection = True
            Me.Text = My.Resources.ConnectToAnyRepository
            Dim result As DialogResult = Me.ShowDialog(ParentForm)
            If result = Windows.Forms.DialogResult.OK Then
                SQLServer = TargetSQLServer
                DatabaseName = TargetDatabaseName.Text
                SQLInstance = TargetInstanceName.Text
                UseTrustedConnection = TargetUseTrustedConnection.Checked
                SQLLogin = TargetSQLLoginName.Text
                Password = TargetSQLLoginPassword.Text
                ConnectionTimeout = CInt(TargetConnectionTimeout.Value)
                NetProtocol = TargetNetworkProtocol.Text
                EncryptCn = TargetEncryptConnection.Checked
                TrustServer = TargetTrustServerCertificate.Checked
            Else
                ' try to put it back the way it was
                ' may be missing the passed server here??? can't seem to return it without creating another
                SQLServer = PassedSQLServer
                SQLInstance = TargetInstanceName.Tag.ToString
                DatabaseName = TargetDatabaseName.Tag.ToString
                UseTrustedConnection = CBool(TargetUseTrustedConnection.Tag)
                SQLLogin = TargetSQLLoginName.Tag.ToString
                Password = TargetSQLLoginPassword.Tag.ToString
                ConnectionTimeout = CInt(TargetConnectionTimeout.Tag)
                NetProtocol = TargetNetworkProtocol.Tag.ToString
                EncryptCn = CBool(TargetEncryptConnection.Tag)
                TrustServer = CBool(TargetTrustServerCertificate.Tag)
            End If
            Return result
        Catch ex As Exception
            Throw New Exception(String.Format("({0}.ShowDialogForRepository) Exception.", Me.Name), ex)
        End Try
    End Function

    Private Sub CompareConnectForm_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            ' configure if  not configured
            If TargetInstanceName.Text = "" Or IsRepositoryConnection Then
                TargetInstanceName.Items.Clear()
                TargetInstanceName.Items.AddRange(Mother.InstanceList)
                ' addin a blank here gets wiped out when a connection is made
                ' don't need an option for blank anyway
                If Not TargetInstanceName.Text = "" _
                And Not TargetInstanceName.Items.Contains(TargetInstanceName.Text) Then
                    TargetInstanceName.Items.Add(TargetInstanceName.Text)
                End If
                TargetInstanceName.Sorted = True
            Else
                ' if already configured, can't change the instance name
                ' but allow connection settings changes
                ' the tag should already have the existing value
                ' so can put the settings back to there on error
                TargetInstanceName.Enabled = False
                TargetDatabaseName.Enabled = False
            End If
            If TargetUseTrustedConnection.Checked Then
                TargetSQLLoginName.Text = ""
                TargetSQLLoginName.Enabled = False
                TargetSQLLoginPassword.Text = ""
                TargetSQLLoginPassword.Enabled = False
            Else
                TargetSQLLoginName.Enabled = True
                'TargetSQLLoginPassword.Text = ""
                TargetSQLLoginPassword.Enabled = True
            End If
            For Each s As String In My.Settings.NetworkLibraries
                TargetNetworkProtocol.Items.Add(s)
            Next s
        Catch ex As Exception
            Mother.HandleException(ex)
        End Try
    End Sub

    Private Sub OKButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        If Not TargetInstanceName.Text = "" Then
            Dim csr As Cursor = Nothing
            Try
                csr = Me.Cursor   ' Save the old cursor
                Me.Cursor = Cursors.WaitCursor   ' Display the waiting cursor
                ' everything that gets inserted should be upper case  
                TargetInstanceName.Text = UCase(TargetInstanceName.Text)
                Dim srvcon As New ServerConnection
                srvcon.ConnectionString = sConnectionString
                ' if the instance is blank it picks up the server name w/o instance here as name
                TargetSQLServer = New Server(srvcon)
                'verify the connection
                If TargetSQLServer.Information.IsSingleUser Then
                    Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
                    TargetSQLServer.ConnectionContext.Disconnect()
                    TargetSQLServer = Nothing
                    Throw New Exception(String.Format(CultureInfo.CurrentUICulture, "SQL Instance [{0}] is in Single User Mode. Connection terminating...", TargetInstanceName.Text))
                Else
                    Me.DialogResult = System.Windows.Forms.DialogResult.OK
                End If
                Me.Close()
            Catch ex As Exception
                'Me.DialogResult = Windows.Forms.DialogResult.Cancel
                Mother.HandleException(ex)
            Finally
                Me.Cursor = csr ' restore the original cursor
            End Try
        End If
    End Sub

    Private Sub ButtonCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonCancel.Click
        Try
            If Not TargetSQLServer Is Nothing Then
                If TargetSQLServer.ConnectionContext.IsOpen Then
                    TargetSQLServer.ConnectionContext.Disconnect()
                End If
                TargetSQLServer = Nothing
                Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
                Me.Close()
            End If
        Catch ex As Exception
            Throw New Exception(String.Format("({0}.ButtonCancel_Click) Exception.", Me.Name), ex)
        End Try
    End Sub

    Private Sub UseEncryptedConnection_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) _
    Handles UseEncryptedConnection.CheckedChanged
        Try
            ' can only TrustServerCertificate if EncryptConnection 
            If Not (UseEncryptedConnection.Checked) Then
                TrustServerCertificate.Checked = False
            End If
        Catch ex As Exception
            Throw New Exception(String.Format("({0}.UseEncryptedConnection_CheckedChanged) Exception.", Me.Name), ex)
        End Try
    End Sub

    Private Sub TargetUseTrustedConnection_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) _
    Handles TargetUseTrustedConnection.CheckedChanged
        Try
            ' the SetupForm uses the same check changed event logic
            TargetSQLLoginName.Text = ""
            TargetSQLLoginPassword.Text = ""
            If TargetUseTrustedConnection.Checked Then
                TargetSQLLoginName.Enabled = False
                TargetSQLLoginPassword.Enabled = False
                TargetSQLLoginName.TabStop = False
                TargetSQLLoginPassword.TabStop = False
            Else
                'if the form is loading the tag will not be set yet
                If Not TargetUseTrustedConnection.Tag Is Nothing Then
                    VerifyEncryptionHierarchy()
                End If
                TargetSQLLoginName.Enabled = True
                TargetSQLLoginPassword.Enabled = True
                TargetSQLLoginName.TabStop = True
                TargetSQLLoginPassword.TabStop = True
            End If
        Catch ex As Exception
            Throw New Exception(String.Format("({0}.TargetUseTrustedConnection_CheckedChanged) Exception.", Me.Name), ex)
        End Try
    End Sub

    Private Function VerifyEncryptionHierarchy() As Boolean
        ' the SetupForm has a copy of this method, must stay the same 
        VerifyEncryptionHierarchy = False
        Dim srvcon As New ServerConnection
        Dim oRepository As Smo.Server
        Try
            srvcon.ConnectionString = Mother.DAL.LocalRepositoryConnectionString
            oRepository = New Server(srvcon)
            Dim oDb As Smo.Database = oRepository.Databases(My.Settings.RepositoryDatabaseName)
            ' check for the symmetric key 
            If Not (oDb.SymmetricKeys.Contains(My.Settings.RepositoryPasswordEncryptionKey)) Then
                DialogSetEncryptionHierarchy.Tag = oDb
                Dim r As DialogResult = DialogSetEncryptionHierarchy.ShowDialog(Me)
            Else
                MessageBox.Show(My.Resources.SQLLoginNotRecommendedText, _
                My.Resources.SQLLoginNotRecommendedCaption, _
                MessageBoxButtons.OK)
            End If
            VerifyEncryptionHierarchy = True
        Catch exSQL As SqlClient.SqlException
            Throw New Exception(String.Format("({0}.VerifyEncryptionHierarchy) Exception.", Me.Name), exSQL)
        Finally
            If Not (srvcon Is Nothing) Then
                srvcon.Disconnect()
                oRepository = Nothing
            End If
        End Try
    End Function

    Private Sub TargetDatabaseName_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles TargetDatabaseName.DropDown
        Try
            TargetDatabaseName.Items.Clear()
            If TargetInstanceName.Text <> "" Then
                Dim srvcon As New ServerConnection
                srvcon.ConnectionString = sConnectionString
                TargetSQLServer = New Server(srvcon)
                For Each db As Database In TargetSQLServer.Databases
                    If db.IsAccessible Then
                        TargetDatabaseName.Items.Add(db.Name)
                    End If
                Next
            End If
        Catch ex As Exception
            Throw New Exception(String.Format("({0}.TargetDatabaseName_DropDown) Exception.", Me.Name), ex)
        End Try
    End Sub

End Class