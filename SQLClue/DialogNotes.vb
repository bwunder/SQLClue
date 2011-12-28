Imports System.Windows.Forms

Public Class DialogNotes
    ' for target instance and compare utility
    Public Overloads Function ShowDialog(ByRef Notes As String, _
                                         ByVal NoteComponent As String, _
                                         ByVal NoteType As String, _
                                         ByVal NoteName As String, _
                                         ByVal ParentForm As IWin32Window) As System.Windows.Forms.DialogResult
        Try
            Me.Text = String.Format("Notes for {0} {1} '{2}'", NoteComponent, NoteType, NoteName)

            RichTextBoxNotes.Text = Notes
            RichTextBoxNotes.Tag = Notes

            Me.Height = CInt(My.Forms.Mother.Height * 0.8)
            Me.Width = CInt(My.Forms.Mother.Width * 0.8)

            RichTextBoxNotes.ReadOnly = True
            LinkLabel1.Enabled = True

            Dim result As DialogResult = Me.ShowDialog(ParentForm)
            'if pass back the Notes that has been displayed it does not always evalute to equal 
            'when unchanged and used hits OK instead of cancel
            If result = Windows.Forms.DialogResult.OK Then
                Notes = RichTextBoxNotes.Text
            Else
                Notes = RichTextBoxNotes.Tag.ToString
            End If
            Return result
        Catch ex As Exception
        Finally
            Me.Dispose()
        End Try
    End Function

    ' up to the caller to decide what to do with OK & Cancel
    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Try
            If Trim(RichTextBoxNewNote.Text) <> "" Then
                RichTextBoxNotes.Text = RichTextBoxNotes.Text & _
                                        Now & " : " & My.User.Name & vbCrLf & _
                                        RichTextBoxNewNote.Text
                RichTextBoxNewNote.Text = ""
            End If
            Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Catch ex As Exception
            Throw New Exception(String.Format("({0}.OK_Button_Click) Exception.", Me.Name), ex)
        End Try
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Try
            RichTextBoxNotes.Text = RichTextBoxNotes.Tag.ToString
            RichTextBoxNewNote.Text = ""
            Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Catch ex As Exception
            Throw New Exception(String.Format("({0}.OK_Button_Click) Exception.", Me.Name), ex)
        End Try
    End Sub

    Private Sub MenuItemZoomIn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItemZoomIn.Click
        If CType(ContextMenuStripZoom.SourceControl, RichTextBox).ZoomFactor < 10 Then
            If CType(ContextMenuStripZoom.SourceControl, RichTextBox).ZoomFactor > 1 Then
                CType(ContextMenuStripZoom.SourceControl, RichTextBox).ZoomFactor += 1
            Else
                CType(ContextMenuStripZoom.SourceControl, RichTextBox).ZoomFactor += CType(0.1, Single)
            End If
        End If
    End Sub

    Private Sub MenuItemZoomNormal_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItemZoomNormal.Click
        CType(ContextMenuStripZoom.SourceControl, RichTextBox).ZoomFactor = 1

    End Sub

    Private Sub MenuItemZoomOut_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItemZoomOut.Click
        If CType(ContextMenuStripZoom.SourceControl, RichTextBox).ZoomFactor > 0.5 Then
            If CType(ContextMenuStripZoom.SourceControl, RichTextBox).ZoomFactor > 1.5 Then
                CType(ContextMenuStripZoom.SourceControl, RichTextBox).ZoomFactor -= 1
            Else
                CType(ContextMenuStripZoom.SourceControl, RichTextBox).ZoomFactor -= CType(0.1, Single)
            End If
        End If
    End Sub

    Private Sub LinkLabel1_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        RichTextBoxNotes.ReadOnly = False
        LinkLabel1.Enabled = False
    End Sub

    Private Sub SplitContainer2_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles SplitContainer2.GotFocus
        SplitContainer2.BackColor = SystemColors.GradientActiveCaption
    End Sub

    Private Sub SplitContainer2_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles SplitContainer2.LostFocus
        SplitContainer2.BackColor = SystemColors.GradientInactiveCaption
    End Sub

    Private Sub SplitContainer2_MouseEnter(ByVal sender As Object, ByVal e As System.EventArgs) Handles SplitContainer2.MouseEnter
        SplitContainer2.BackColor = SystemColors.GradientActiveCaption
    End Sub

    Private Sub SplitContainer2_MouseLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles SplitContainer2.MouseLeave
        SplitContainer2.BackColor = SystemColors.GradientInactiveCaption
    End Sub

End Class
