Imports System.Windows.Forms

Public Class RunFrequencySelector

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click

        If CheckRange() Then

            '  if anything but OK the previous values will remain in the tags
            If ComboBox1.Text = "" Then
                MessageBox.Show(My.Resources.ArchiveIntervalSelectFailed, _
                                My.Resources.ArchiveIntervalSelectFailedCaption, _
                                MessageBoxButtons.OK, _
                                MessageBoxIcon.Error, _
                                MessageBoxDefaultButton.Button1)
            Else
                ComboBox1.Tag = ComboBox1.Text
                NumericUpDown1.Tag = NumericUpDown1.Value
                Me.DialogResult = System.Windows.Forms.DialogResult.OK
                Me.Close()
            End If

        End If

    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click

        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()

    End Sub

    Private Function CheckRange() As Boolean

        Dim max As Int16

        Select Case UCase(ComboBox1.Text)
            Case UCase(DateInterval.Minute.ToString)
                max = 60
            Case UCase(DateInterval.Hour.ToString)
                max = 24
            Case UCase(DateInterval.Day.ToString)
                max = 365
            Case UCase(DateInterval.Month.ToString)
                max = 12
            Case UCase(DateInterval.Quarter.ToString)
                max = 4
            Case Else
                max = 1
        End Select

        If NumericUpDown1.Value < 1 Or NumericUpDown1.Value > max Then
            MessageBox.Show(String.Format(CultureInfo.CurrentUICulture, _
                                          My.Resources.ArchiveInterval, _
                                          max, _
                                          ComboBox1.Text), _
                            My.Resources.ArchiveIntervalSelectFailedCaption, _
                            MessageBoxButtons.OK, _
                            MessageBoxIcon.Error, _
                            MessageBoxDefaultButton.Button1)
            Return False
        Else
            Return True
        End If

    End Function

    Private Sub RunFrequencySelector_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        ComboBox1.Items.Clear()
        ComboBox1.Items.Add(DateInterval.Minute.ToString)
        ComboBox1.Items.Add(DateInterval.Hour.ToString)
        ComboBox1.Items.Add(DateInterval.Day.ToString)
        ComboBox1.Items.Add(DateInterval.Month.ToString)
        ComboBox1.Items.Add(DateInterval.Quarter.ToString)
        ComboBox1.Items.Add(DateInterval.Year.ToString)

        RefreshStatusLabel()

    End Sub

    Private Sub RefreshStatusLabel()
        If ComboBox1.Text = ComboBox1.Tag.ToString And NumericUpDown1.Value = CInt(NumericUpDown1.Tag) Then
            ToolStripStatusLabel1.Text = My.Resources.ArchiveIntervalSelect
        Else
            Dim plural As String = ""
            If NumericUpDown1.Value > 1 Then
                plural = My.Resources.ArchiveIntervalPlural
            End If
            ToolStripStatusLabel1.Text = String.Format(CultureInfo.CurrentUICulture, _
                                                       My.Resources.ArchiveIntervalSentence, _
                                                       NumericUpDown1.Value.ToString, _
                                                       ComboBox1.Text, _
                                                       plural)
        End If
    End Sub

    Private Sub ComboBox1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox1.TextChanged

        ' repaint the english expression
        If Not (ComboBox1.Tag) Is Nothing Then
            RefreshStatusLabel()
        End If

    End Sub

End Class
