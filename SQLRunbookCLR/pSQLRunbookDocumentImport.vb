Option Explicit On
Option Strict On
Option Infer On
Imports System
Imports System.Data
Imports System.Data.Sql
Imports System.Data.SqlClient
Imports System.Data.SqlTypes
Imports Microsoft.SqlServer.Server
Imports System.IO

Partial Public Class StoredProcedures
    <Microsoft.SqlServer.Server.SqlProcedure()> _
    Public Shared Sub pSQLRunbookDocumentImport(ByVal FileName As String, ByVal OwnerName As String)
        Try

            ' get the cart before the horse to ReadBytes
            Dim fInfo As New FileInfo(FileName)
            Dim numBytes As Long = fInfo.Length
            Dim fStream As New FileStream(FileName, _
                                          FileMode.Open, _
                                          FileAccess.Read)
            Dim br As New BinaryReader(fStream)
            Dim Document() As Byte = br.ReadBytes(CInt(numBytes) - 1)
            br.Close()
            fStream.Close()

            Using cn As New SqlConnection("context connection=true")
                If cn.State = ConnectionState.Closed Then
                    cn.Open()
                End If

                Dim cm As SqlCommand = cn.CreateCommand
                cm.CommandText = "dbo.pSQLRunbookDocumentUpsert"
                cm.CommandType = CommandType.StoredProcedure

                Dim FName As New SqlParameter()
                With FName
                    .Direction = ParameterDirection.Input
                    .ParameterName = "@File"
                    .SqlDbType = SqlDbType.NVarChar
                    .Size = 450
                    .Value = FileName
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

                Dim DocType As New SqlParameter()
                With DocType
                    .Direction = ParameterDirection.Input
                    .ParameterName = "@DocumentType"
                    .SqlDbType = SqlDbType.NVarChar
                    .Size = 8
                    .Value = fInfo.Extension.ToString
                    cm.Parameters.Add(DocType)
                End With

                Dim ModDate As New SqlParameter()
                With ModDate
                    .Direction = ParameterDirection.Input
                    .ParameterName = "@LastModifiedDt"
                    .SqlDbType = SqlDbType.DateTime
                    .Value = fInfo.LastWriteTime
                    cm.Parameters.Add(ModDate)
                End With

                Dim Owner As New SqlParameter()
                With Owner
                    .Direction = ParameterDirection.Input
                    .ParameterName = "@Owner"
                    .SqlDbType = SqlDbType.NVarChar
                    .Size = 128
                    .Value = OwnerName
                    cm.Parameters.Add(Owner)
                End With

                Dim WatchFileForChange As New SqlParameter()
                With WatchFileForChange
                    .Direction = ParameterDirection.Input
                    .ParameterName = "@WatchFileForChange"
                    .SqlDbType = SqlDbType.Bit
                    .Value = 1
                    cm.Parameters.Add(WatchFileForChange)
                End With

                cm.ExecuteNonQuery()

            End Using

        Catch ex As Exception

            Throw (New Exception("(pSQLRunbookDocumentImport) Exception.", ex))

        End Try

    End Sub

End Class
