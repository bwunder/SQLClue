Public Class cFileWatcher

    Public WaitBetweenDocumentsSeconds As Integer
    Public RunbookInstanceName As String
    Public RunbookDatabaseName As String
    Public RunbookUseTrustedConnection As Boolean
    Public RunbookSQLLoginName As String
    Public RunbookSQLLoginPassword As String
    Public RunbookConnectionTimeout As Integer
    Public RunbookNetworkLibrary As String
    Public RunbookEncryptConnection As Boolean
    Public RunbookTrustServerCertificate As Boolean
    Public Event WatcherInfo(ByVal Message As String)
    Public Event WatcherProblem(ByRef ex As Exception)
    Public Event SyncState(ByVal Message As String)

    Public ReadOnly Property sRunbookConnectionString() As String
        Get
            Dim builder As New System.Data.SqlClient.SqlConnectionStringBuilder
            builder.DataSource = RunbookInstanceName
            builder.InitialCatalog = RunbookDatabaseName
            If RunbookUseTrustedConnection Then
                builder.IntegratedSecurity = True
            Else
                builder.UserID = RunbookSQLLoginName
                builder.Password = RunbookSQLLoginPassword
            End If
            builder.ConnectTimeout = RunbookConnectionTimeout
            builder.ApplicationName = My.Application.Info.ProductName & " : SQLClueService"
            'allow for a 'not specified' net
            If RunbookNetworkLibrary <> "" Then
                builder.NetworkLibrary = CStr(If(InStr(RunbookNetworkLibrary, " ") > 0, _
                                                 RunbookNetworkLibrary.Split(Chr(32))(0), _
                                                 RunbookNetworkLibrary))
            End If
            builder.Encrypt = RunbookEncryptConnection
            builder.TrustServerCertificate = RunbookTrustServerCertificate
            builder.Enlist = True
            builder.PersistSecurityInfo = False
            builder.UserInstance = False
            Return builder.ConnectionString
        End Get
    End Property

    Public Sub RunbookMonitor(ByVal WaitBetweenDocumentsSeconds As Integer)
        Try
            Dim FilesToCheck As DataTable = New DataTable("FilesToCheck")
            Dim File As DataColumn = New DataColumn
            File.DataType = System.Type.GetType("System.String")
            File.ColumnName = "File"
            FilesToCheck.Columns.Add(File)
            Dim LastModifiedDt As DataColumn = New DataColumn
            LastModifiedDt.DataType = System.Type.GetType("System.DateTime")
            LastModifiedDt.ColumnName = "LastModifiedDt"
            FilesToCheck.Columns.Add(LastModifiedDt)
            Dim Owner As DataColumn = New DataColumn
            Owner.DataType = System.Type.GetType("System.String")
            Owner.ColumnName = "Owner"
            FilesToCheck.Columns.Add(Owner)
            Using cn As New System.Data.SqlClient.SqlConnection(sRunbookConnectionString)
                cn.Open()
                Using cmMonitor As SqlCommand = cn.CreateCommand
                    cmMonitor.CommandType = CommandType.StoredProcedure
                    cmMonitor.CommandText = "SQLRunbook.pDocumentSelectForMonitor"
                    Dim da As SqlDataAdapter = New SqlDataAdapter(cmMonitor)
                    da.Fill(FilesToCheck)
                End Using
            End Using
            RaiseEvent WatcherInfo(String.Format("Checking {0} documents for changes.", FilesToCheck.Rows.Count))
            For Each r As DataRow In FilesToCheck.Rows
                RaiseEvent WatcherInfo(r("File").ToString)
                Dim fInfo As New FileInfo(r("File").ToString)
                ' time stamp milliseconds can be off due to rounding
                If CType(fInfo.LastWriteTime, Date) > CDate(r("LastModifiedDt")).AddMilliseconds(50) Then
                    Try
                        RaiseEvent WatcherInfo("has changes")
                        ' get the cart before the horse to ReadBytes
                        Dim fStream As New FileStream(fInfo.FullName, _
                                                      FileMode.Open, _
                                                      FileAccess.Read)
                        Dim br As New BinaryReader(fStream)
                        Dim Document() As Byte = br.ReadBytes(CInt(fInfo.Length) - 1)
                        br.Close()
                        br = Nothing
                        fStream.Close()
                        fStream = Nothing
                        Using cn As New SqlConnection(sRunbookConnectionString)
                            If cn.State = ConnectionState.Closed Then
                                cn.Open()
                            End If
                            Using cm As SqlCommand = cn.CreateCommand
                                cm.CommandText = "SQLRunbook.pDocumentUpsert"
                                cm.CommandType = CommandType.StoredProcedure
                                Dim FName As New SqlParameter()
                                With FName
                                    .Direction = ParameterDirection.Input
                                    .ParameterName = "@File"
                                    .SqlDbType = SqlDbType.NVarChar
                                    .Size = 450
                                    .Value = fInfo.FullName.ToString
                                    cm.Parameters.Add(FName)
                                End With
                                Dim Doc As New SqlParameter()
                                With Doc
                                    .Direction = ParameterDirection.Input
                                    .ParameterName = "@Document"
                                    .SqlDbType = SqlDbType.VarBinary
                                    .Size = -1
                                    .Value = Document
                                    cm.Parameters.Add(Doc)
                                End With
                                Dim Extension As New SqlParameter()
                                With Extension
                                    .Direction = ParameterDirection.Input
                                    .ParameterName = "@DocumentType"
                                    .SqlDbType = SqlDbType.NVarChar
                                    .Size = 8
                                    .Value = fInfo.Extension.ToString
                                    cm.Parameters.Add(Extension)
                                End With
                                Dim ModDate As New SqlParameter()
                                With ModDate
                                    .Direction = ParameterDirection.Input
                                    .ParameterName = "@LastModifiedDt"
                                    .SqlDbType = SqlDbType.DateTime
                                    .Value = fInfo.LastWriteTime
                                    cm.Parameters.Add(ModDate)
                                End With
                                Dim Ownr As New SqlParameter()
                                With Ownr
                                    .Direction = ParameterDirection.Input
                                    .ParameterName = "@Owner"
                                    .SqlDbType = SqlDbType.NVarChar
                                    .Size = 128
                                    .Value = r("Owner").ToString
                                    cm.Parameters.Add(Ownr)
                                End With
                                Dim WatchFileForChange As New SqlParameter()
                                With WatchFileForChange
                                    .Direction = ParameterDirection.Input
                                    .ParameterName = "@WatchFileForChange"
                                    .SqlDbType = SqlDbType.Bit
                                    .Value = True
                                    cm.Parameters.Add(WatchFileForChange)
                                End With
                                Dim DocumentId As New SqlParameter()
                                With DocumentId
                                    .Direction = ParameterDirection.Output
                                    .ParameterName = "@DocumentId"
                                    .SqlDbType = SqlDbType.Int
                                    cm.Parameters.Add(DocumentId)
                                End With
                                cm.ExecuteNonQuery()
                                'throw the Id on the floor
                            End Using
                        End Using
                        RaiseEvent SyncState("syncronized")
                    Catch ex As Exception
                        RaiseEvent SyncState("failed")
                        RaiseEvent WatcherProblem(ex)
                    End Try
                Else
                    RaiseEvent SyncState("same")
                End If ' newer
                If WaitBetweenDocumentsSeconds > 0 Then
                    Threading.Thread.Sleep(WaitBetweenDocumentsSeconds * 1000) ' cvt to milliseconds
                End If
            Next
        Catch ex As Exception
            Throw New Exception(String.Format("(cFileWatcher.RunbookMonitor) Exception."), ex)
        End Try
    End Sub

End Class
