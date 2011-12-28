Public Class DialogRating
    ' should be same as struct on runbook form
    Friend Structure StructRating
        Friend Rating As Integer
        Friend ItemId As Integer
        Friend RatingType As String
        Friend Notes As String
        Friend AvgRating As Integer
        Friend NbrRatings As Integer
        Friend NewRating As Integer
        Friend NewNote As String
    End Structure
    Friend PeerReview As StructRating
    Private _cnString As String

    Public Overloads Function ShowDialog(ByVal RatingType As String, _
                                         ByVal ItemId As Integer, _
                                         ByVal ItemLiteral As String, _
                                         ByVal sRunbookConnectionString As String, _
                                         ByVal ParentForm As IWin32Window) As System.Windows.Forms.DialogResult
        Try
            ' init the form
            RadioButton1.Checked = False
            RadioButton2.Checked = False
            RadioButton3.Checked = False
            RadioButton4.Checked = False
            RadioButton5.Checked = False
            RadioButton6.Checked = False
            RadioButton7.Checked = False
            RichTextBoxPreviousNotes.Text = ""
            RichTextBoxNewNote.Text = ""
            LabelAverageRating.Text = ""
            LabelNumberOfRatings.Text = ""
            PeerReview = New StructRating
            PeerReview.RatingType = RatingType
            PeerReview.ItemId = ItemId
            _cnString = sRunbookConnectionString
            Me.Text = String.Format("Peer Review of {0} '{1}'", PeerReview.RatingType, ItemLiteral)
            Return Me.ShowDialog(ParentForm)
        Catch ex As Exception
            Throw New Exception(String.Format("({0}.ShowDialog) Exception.", Me.Text), ex)
        End Try
    End Function

    Private Sub RatingForm_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ' check for existing rating and show those values if present
        ' otherwise blank slate for rating
        Try
            RichTextBoxNewNote.Text = ""
            Using cn As New SqlClient.SqlConnection(_cnString)
                If cn.State <> ConnectionState.Open Then
                    cn.Open()
                End If
                Dim cm As New System.Data.SqlClient.SqlCommand
                cm.Connection = cn
                cm.CommandType = CommandType.StoredProcedure
                Select Case PeerReview.RatingType
                    Case "Topic"
                        cm.CommandText = "SQLRunbook.pTopicRatingGetByUser"
                    Case "Document"
                        cm.CommandText = "SQLRunbook.pDocumentRatingGetByUser"
                    Case "Category"
                        cm.CommandText = "SQLRunbook.pCategoryRatingGetByUser"
                    Case Else
                        Throw New Exception("Invalid Rating Type.")
                End Select
                Dim RunbookItemId As New SqlParameter()
                With RunbookItemId
                    .Direction = ParameterDirection.Input
                    .ParameterName = "@Id"
                    .SqlDbType = SqlDbType.Int
                    .Value = PeerReview.ItemId
                End With
                cm.Parameters.Add(RunbookItemId)
                Dim UserName As New SqlParameter()
                With UserName
                    .Direction = ParameterDirection.Input
                    .ParameterName = "@User"
                    .SqlDbType = SqlDbType.NVarChar
                    .Size = 128
                    .Value = My.User.Name
                End With
                cm.Parameters.Add(UserName)
                Dim Rat As New SqlParameter()
                With Rat
                    .Direction = ParameterDirection.Output
                    .ParameterName = "@Rating"
                    .SqlDbType = SqlDbType.Int
                    '.Value = PeerReview.Rating
                End With
                cm.Parameters.Add(Rat)
                Dim Note As New SqlParameter()
                With Note
                    .Direction = ParameterDirection.Output
                    .ParameterName = "@Note"
                    .SqlDbType = SqlDbType.NVarChar
                    .Size = -1
                    '.Value = PeerReview.Notes
                End With
                cm.Parameters.Add(Note)
                Dim AvgRat As New SqlParameter()
                With AvgRat
                    .Direction = ParameterDirection.Output
                    .ParameterName = "@AvgRating"
                    .SqlDbType = SqlDbType.Int
                    '.Value = PeerReview.AvgRating
                End With
                cm.Parameters.Add(AvgRat)
                Dim NbrRat As New SqlParameter()
                With NbrRat
                    .Direction = ParameterDirection.Output
                    .ParameterName = "@NbrRatings"
                    .SqlDbType = SqlDbType.Int
                    '.Value = PeerReview.NbrRating
                End With
                cm.Parameters.Add(NbrRat)
                cm.ExecuteNonQuery()
                PeerReview.Rating = CInt(Rat.Value)
                PeerReview.Notes = Note.Value.ToString
                PeerReview.AvgRating = CInt(AvgRat.Value)
                PeerReview.NbrRatings = CInt(NbrRat.Value)
                cn.Close()
            End Using
            Select Case PeerReview.Rating
                Case 1
                    RadioButton1.Checked = True
                Case 2
                    RadioButton2.Checked = True
                Case 3
                    RadioButton3.Checked = True
                Case 4
                    RadioButton4.Checked = True
                Case 5
                    RadioButton5.Checked = True
                Case 6
                    RadioButton6.Checked = True
                Case 7
                    RadioButton7.Checked = True
            End Select
            RichTextBoxPreviousNotes.Text = PeerReview.Notes
            RichTextBoxPreviousNotes.ReadOnly = True
            LabelAverageRating.Text = PeerReview.AvgRating.ToString
            LabelNumberOfRatings.Text = PeerReview.NbrRatings.ToString
            RichTextBoxGuidelines.Text = "In the SQLClue Runbook, Peer Review is the protocol used by the designated " & _
                                         "data layer technical support team to define or improve the quality of the " & _
                                         "Runbook content. Peer Review provides a means for each team member to share their " & _
                                         "unique perspectives and expertise to help improve all Runbook Topics and " & _
                                         "Documents. More importantly the feedback can be used to improve the processes, " & _
                                         "systems and practices that are described by the Runbook." & vbCrLf & vbCrLf & _
                                         "Peer Review consists of a rating of the item under review and a mindful rating " & _
                                         "explanation. The statement content should adhere to generally accepted etiquette " & _
                                         "in written communications. "
        Catch ex As Exception
            Throw New Exception(String.Format("({0}.Load) Exception.", Me.Text), ex)
        End Try
    End Sub

    Private Sub ButtonSend_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonSend.Click
        Try
            With PeerReview
                If RadioButton1.Checked Then
                    .NewRating = 1
                ElseIf RadioButton2.Checked Then
                    .NewRating = 2
                ElseIf RadioButton3.Checked Then
                    .NewRating = 3
                ElseIf RadioButton4.Checked Then
                    .NewRating = 4
                ElseIf RadioButton5.Checked Then
                    .NewRating = 5
                ElseIf RadioButton6.Checked Then
                    .NewRating = 6
                ElseIf RadioButton7.Checked Then
                    .NewRating = 7
                Else
                    MessageBox.Show("Select a rating.", "", MessageBoxButtons.OK)
                    Exit Sub
                End If
                If RichTextBoxNewNote.Text = "" Then
                    MessageBox.Show("Provide a brief explanation for the rating.", "", MessageBoxButtons.OK)
                    Exit Sub
                Else
                    If .Rating > 0 Then
                        .NewNote = String.Format("(rating changed from {0} to {1})", .Rating, .NewRating) & vbCrLf & RichTextBoxNewNote.Text
                    Else
                        .NewNote = RichTextBoxNewNote.Text
                    End If
                End If
            End With
            ' not even a note change if the rating is unchanged, you had your chance
            If PeerReview.Rating <> PeerReview.NewRating Or Trim(RichTextBoxNewNote.Text) <> "" Then
                Using cn As New SqlClient.SqlConnection(_cnString)
                    If cn.State <> ConnectionState.Open Then
                        cn.Open()
                    End If
                    Using cm As New System.Data.SqlClient.SqlCommand
                        cm.Connection = cn
                        cm.CommandType = CommandType.StoredProcedure
                        Select Case PeerReview.RatingType
                            Case "Topic"
                                cm.CommandText = "SQLRunbook.pTopicRatingUpsert"
                            Case "Document"
                                cm.CommandText = "SQLRunbook.pDocumentRatingUpsert"
                            Case "Category"
                                cm.CommandText = "SQLRunbook.pCategoryRatingUpsert"
                            Case Else
                                Throw New Exception("Invalid Rating Type.")
                        End Select
                        Dim RunbookItemId As New SqlParameter()
                        With RunbookItemId
                            .Direction = ParameterDirection.Input
                            .ParameterName = "@Id"
                            .SqlDbType = SqlDbType.Int
                            .Value = PeerReview.ItemId
                        End With
                        cm.Parameters.Add(RunbookItemId)
                        Dim RatingId As New SqlParameter()
                        With RatingId
                            .Direction = ParameterDirection.Input
                            .ParameterName = "@RatingId"
                            .SqlDbType = SqlDbType.NVarChar
                            .Size = 128
                            .Value = PeerReview.NewRating
                        End With
                        cm.Parameters.Add(RatingId)
                        Dim Note As New SqlParameter()
                        With Note
                            .Direction = ParameterDirection.Input
                            .ParameterName = "@Notes"
                            .SqlDbType = SqlDbType.NVarChar
                            .Size = -1
                            .Value = PeerReview.NewNote
                        End With
                        cm.Parameters.Add(Note)
                        cm.ExecuteNonQuery()
                    End Using
                End Using
            End If
            Me.DialogResult = Windows.Forms.DialogResult.OK
        Catch ex As Exception
            Throw New Exception(String.Format("({0}.ButtonSend_Click) {1} Rating Exception.", Me.Text, PeerReview.RatingType), ex)
        End Try
    End Sub

    Private Sub RichTextBoxPreviousNotes_TextChanged(ByVal sender As System.Object, _
                                                     ByVal e As System.EventArgs) _
                                                     Handles RichTextBoxPreviousNotes.Click
        If (DialogNotes.ShowDialog(PeerReview.Notes, _
                                  Me.Text, _
                                  String.Format("{0} Rating", PeerReview.RatingType), _
                                  PeerReview.ItemId.ToString, _
                                  Me)) = Windows.Forms.DialogResult.OK Then
            If Not DialogNotes.RichTextBoxNotes.Text = PeerReview.Notes Then
                PeerReview.Notes = DialogNotes.RichTextBoxNotes.Text
            End If
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

    Private Sub SplitContainer1_MouseLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles SplitContainer1.MouseLeave
        SplitContainer1.BackColor = SystemColors.GradientInactiveCaption
    End Sub

End Class