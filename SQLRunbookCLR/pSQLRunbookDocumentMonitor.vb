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
Imports System.Threading

Partial Public Class StoredProcedures
    <Microsoft.SqlServer.Server.SqlProcedure()> _
    Public Shared Sub pSQLRunbookDocumentMonitor(ByVal WaitBetweenDocumentsSeconds As Integer)
        Try

            Using cn As New SqlConnection("context connection=true")
                If cn.State = ConnectionState.Closed Then
                    cn.Open()
                End If

                Dim cmMonitor As SqlCommand = cn.CreateCommand
                cmMonitor.CommandType = CommandType.StoredProcedure
                cmMonitor.CommandText = "dbo.pSQLRunbookDocumentSelectForMonitor"

                Dim da As SqlDataAdapter = New SqlDataAdapter(cmMonitor)

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

                da.Fill(FilesToCheck)

                cn.Close()

                For Each r As DataRow In FilesToCheck.Rows

                    Dim fInfo As New FileInfo(r("File").ToString)

                    If CType(fInfo.LastWriteTime, Date) > CDate(r("LastModifiedDt")) Then

                        If cn.State = ConnectionState.Closed Then
                            cn.Open()
                        End If

                        Dim cm As SqlCommand = cn.CreateCommand
                        cm.CommandText = "dbo.pSQLRunbookDocumentImport"
                        cm.CommandType = CommandType.StoredProcedure

                        Dim FName As New SqlParameter()
                        With FName
                            .Direction = ParameterDirection.Input
                            .ParameterName = "@File"
                            .SqlDbType = SqlDbType.NVarChar
                            .Size = 450
                            .Value = r("File").ToString
                            cm.Parameters.Add(FName)
                        End With

                        Dim Onr As New SqlParameter()
                        With Onr
                            .Direction = ParameterDirection.Input
                            .ParameterName = "@Owner"
                            .SqlDbType = SqlDbType.VarBinary
                            .Size = 128
                            .Value = r("Owner").ToString
                            cm.Parameters.Add(Onr)
                        End With

                        cm.ExecuteNonQuery()

                        cn.Close()

                    End If ' newer

                    'hmmm what are the consequences of sleeping on a clr integration thread?
                    Threading.Thread.Sleep(WaitBetweenDocumentsSeconds * 1000) ' arg is in milliseconds

                Next

            End Using

        Catch ex As Exception

            Throw (New Exception("(pSQLRunbookDocumentImport) Exception.", ex))

        End Try

    End Sub

End Class
