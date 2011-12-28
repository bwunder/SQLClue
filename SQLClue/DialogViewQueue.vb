Imports System.Windows.Forms

Public Class DialogViewQueue

    Public ReadOnly Property sConnectionString() As String
        Get

            Dim builder As New System.Data.SqlClient.SqlConnectionStringBuilder
            builder.ConnectionString = ConfigurationForm.TargetInstance.ConnectionContext.ConnectionString
            builder.InitialCatalog = My.Settings.TargetEventNotificationDatabase
            Return builder.ConnectionString

        End Get

    End Property

    Private Sub ButtonClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonClose.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub DialogViewQueue_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        PSQLClueCheckQueueStatusTableAdapter.Connection.ConnectionString = sConnectionString
        PSQLClueCheckQueueStatusTableAdapter.ClearBeforeFill = True
        Me.PSQLClueCheckQueueStatusTableAdapter.Fill(Me.DataSetQueuedChanges.pSQLClueCheckQueueStatus)

        'If a queue is disabled, show the enable button
        For Each r As DataGridViewRow In DataGridViewQueueStatus.Rows
            If UCase(r.Cells("ReceiveStateDataGridViewTextBoxColumn").Value.ToString) = "DISABLED" _
            OrElse UCase(r.Cells("EnqueueStateDataGridViewTextBoxColumn").Value.ToString) = "DISABLED" _
            OrElse (UCase(r.Cells("ActivationStateDataGridViewTextBoxColumn").Value.ToString) = "DISABLED" _
                    And UCase(r.Cells("ActivationProcedureDataGridViewTextBoxColumn").Value.ToString) <> "NONE") Then
                ButtonResetQueues.Enabled = True
                Exit For
            End If
        Next

        PSQLClueShowQueueTableAdapter.Connection.ConnectionString = sConnectionString
        PSQLClueShowQueueTableAdapter.ClearBeforeFill = True
        Me.PSQLClueShowQueueTableAdapter.Fill(Me.DataSetQueuedChanges.pSQLClueShowQueue)

        Me.Text = String.Format("Event Queue for SQL Instance [{0}]", PSQLClueCheckQueueStatusTableAdapter.Connection.DataSource)

    End Sub

    Private Sub ButtonResetQueues_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonResetQueues.Click

        Try

            Using cn As New SqlClient.SqlConnection(sConnectionString)

                If Not (cn Is Nothing) AndAlso (cn.State = ConnectionState.Closed) Then
                    cn.Open()
                End If

                Dim cm As New System.Data.SqlClient.SqlCommand

                cm.Connection = cn

                cm.CommandType = CommandType.StoredProcedure
                cm.CommandText = "dbo.pSQLClueResetQueueStatus"

                cm.ExecuteNonQuery()

                cn.Close()

                Me.PSQLClueCheckQueueStatusTableAdapter.Fill(Me.DataSetQueuedChanges.pSQLClueCheckQueueStatus)
                PSQLClueCheckQueueStatusBindingSource.ResetBindings(False)

            End Using

        Catch ex As Exception

            Throw New Exception("(cDataAccess.ButtonResetQueues_Click) Exception", ex)

        End Try

    End Sub

    Private Sub DataGridViewQueuedEvents_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DataGridViewQueuedEvents.CellClick
        If e.RowIndex > -1 Then

            Select Case e.ColumnIndex

                Case 0 ' Queue item
                    'show the EventData xml for the event

                    Using cn As New SqlClient.SqlConnection(sConnectionString)

                        If Not (cn Is Nothing) AndAlso (cn.State = ConnectionState.Closed) Then
                            cn.Open()
                        End If

                        Dim cm As New System.Data.SqlClient.SqlCommand

                        cm.Connection = cn

                        cm.CommandType = CommandType.StoredProcedure
                        cm.CommandText = "dbo.pSQLClueGetEvent"

                        Dim QueuingOrder As New SqlParameter()
                        With QueuingOrder
                            .Direction = ParameterDirection.Input
                            .ParameterName = "@QueuingOrder"
                            .SqlDbType = SqlDbType.BigInt
                            .Value = CInt(DataGridViewQueuedEvents.CurrentCell.Value)
                        End With
                        cm.Parameters.Add(QueuingOrder)

                        Dim MessageBody As New SqlParameter()
                        With MessageBody
                            .Direction = ParameterDirection.Output
                            .ParameterName = "@MessageBody"
                            .SqlDbType = SqlDbType.Xml
                            .Size = -1
                        End With
                        cm.Parameters.Add(MessageBody)

                        cm.ExecuteNonQuery()

                        DialogViewEventData.RichTextBoxXML.Text = Replace(MessageBody.Value.ToString, "><", ">" & vbCrLf & "<")

                        cn.Close()

                        DialogViewEventData.ShowDialog(Me)
                        'Dim msg As String = ""
                        'Dim rdr As Xml.XmlNodeReader = evt.CreateReader()
                        'rdr.Read()

                        'While Not rdr.EOF
                        ' msg = msg + rdr.ReadString + vbCrLf
                        'rdr.Read()
                        'End While

                        'MessageBox.Show(msg, _
                        '                DataGridViewQueuedEvents.CurrentRow.Cells("ObjectNameDataGridViewTextBoxColumn").Value.ToString, _
                        '                MessageBoxButtons.OK, _
                        '                MessageBoxIcon.None, _
                        '                MessageBoxDefaultButton.Button1, _
                        '                MessageBoxOptions.ServiceNotification, _
                        '                False)


                    End Using

                Case Else
                    'do nothing
            End Select
        End If
    End Sub

    Private Sub SplitContainer1_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles SplitContainer1.GotFocus
        SplitContainer1.BackColor = SystemColors.GradientActiveCaption
    End Sub

    Private Sub SplitContainer1_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles SplitContainer1.LostFocus
        SplitContainer1.BackColor = SystemColors.GradientInactiveCaption
    End Sub

    Private Sub SplitContainer1_MouseEnter(ByVal sender As Object, ByVal e As System.EventArgs) Handles SplitContainer1.MouseEnter
        SplitContainer1.BackColor = SystemColors.GradientActiveCaption
    End Sub

    Private Sub SplitContainerVertical_MouseLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles SplitContainer1.MouseLeave
        SplitContainer1.BackColor = SystemColors.GradientInactiveCaption
    End Sub

End Class
