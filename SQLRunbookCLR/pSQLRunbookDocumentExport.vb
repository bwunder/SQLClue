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
    Public Shared Sub pSQLRunbookDocumentExport(ByVal FileName As String, _
                                                ByVal OwnerName As String, _
                                                Optional ByVal ExportFileName As String = Nothing)

        Try

            'send a little info about what is happening to the caller
            Dim pInfo As SqlPipe = SqlContext.Pipe
            pInfo.Send("Original file: " & FileName)

            ' get the doc from the db 
            Using cn As New SqlConnection("context connection=true")
                If cn.State = ConnectionState.Closed Then
                    cn.Open()
                End If

                Dim cm As SqlCommand = cn.CreateCommand
                cm.CommandText = "dbo.pSQLRunbookDocumentSelectByFile"
                cm.CommandType = CommandType.StoredProcedure
                Dim DocFile As New SqlParameter()
                With DocFile
                    .Direction = ParameterDirection.Input
                    .ParameterName = "@File"
                    .SqlDbType = SqlDbType.NVarChar
                    .Size = 450
                    .Value = FileName
                    cm.Parameters.Add(DocFile)
                End With

                Dim rdr As SqlDataReader = cm.ExecuteReader()

                rdr.Read()

                Dim DocId As String = rdr.Item(0).ToString

                ' size a Byte array after the varbinary column
                Dim Document(CInt(rdr.GetBytes(1, 0, Nothing, 0, _
                                     Integer.MaxValue) - 1)) As Byte
                ' and load it with the Varbinary column's data  
                rdr.GetBytes(1, 0, Document, 0, Document.Length)

                Dim DocType As String = rdr.Item(2).ToString
                Dim LastUpdateDate As String = rdr.Item(3).ToString
                Dim OwnerOfRecord As String = rdr.Item(4).ToString

                rdr.Close()
                cn.Close()

                If OwnerName = OwnerOfRecord Then

                    ' never implicitly overwrite the source file
                    Dim NewFileName As String
                    If ExportFileName Is Nothing OrElse ExportFileName = "" Then

                        NewFileName = String.Format( _
                            "{0}__DocumentId__{1}__ExportedFromDbOn_{2}{3}", _
                            Mid(FileName, 1, FileName.Length - DocType.Length), _
                            DocId.ToString, _
                            Now.ToString("yyyyMMddHHmmss"), _
                            DocType)

                    Else

                        NewFileName = ExportFileName

                    End If

                    'pInfo.Send("Exported to file: " & NewFileName)

                    Dim fStream As New FileStream(NewFileName, _
                                                  FileMode.CreateNew, _
                                                  FileAccess.ReadWrite)
                    fStream.Write(Document, 0, Document.Length)
                    fStream.Close()

                Else

                    Throw (New Exception(String.Format("{0} is not the Owner of Record for the requested Document, DocId:{1}", OwnerName, DocId)))

                End If


            End Using

        Catch ex As Exception

            Throw (New Exception("(clrspFullTextFileExport) failed.", ex))

        End Try

    End Sub
End Class
