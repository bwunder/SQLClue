<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class DialogRating
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(DialogRating))
        Me.RadioButton2 = New System.Windows.Forms.RadioButton
        Me.RadioButton1 = New System.Windows.Forms.RadioButton
        Me.RadioButton3 = New System.Windows.Forms.RadioButton
        Me.RadioButton4 = New System.Windows.Forms.RadioButton
        Me.RadioButton5 = New System.Windows.Forms.RadioButton
        Me.RadioButton6 = New System.Windows.Forms.RadioButton
        Me.RadioButton7 = New System.Windows.Forms.RadioButton
        Me.RichTextBoxNewNote = New System.Windows.Forms.RichTextBox
        Me.LabelNote = New System.Windows.Forms.Label
        Me.ButtonSend = New System.Windows.Forms.Button
        Me.PanelRate = New System.Windows.Forms.Panel
        Me.Label13 = New System.Windows.Forms.Label
        Me.Panel2 = New System.Windows.Forms.Panel
        Me.Label5 = New System.Windows.Forms.Label
        Me.Label6 = New System.Windows.Forms.Label
        Me.Label7 = New System.Windows.Forms.Label
        Me.Label8 = New System.Windows.Forms.Label
        Me.Label9 = New System.Windows.Forms.Label
        Me.Label10 = New System.Windows.Forms.Label
        Me.Label11 = New System.Windows.Forms.Label
        Me.Label14 = New System.Windows.Forms.Label
        Me.Label12 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.ToolTipRating = New System.Windows.Forms.ToolTip(Me.components)
        Me.Label15 = New System.Windows.Forms.Label
        Me.Label16 = New System.Windows.Forms.Label
        Me.LabelAverageRating = New System.Windows.Forms.Label
        Me.LabelNumberOfRatings = New System.Windows.Forms.Label
        Me.RichTextBoxPreviousNotes = New System.Windows.Forms.RichTextBox
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer
        Me.LabelPreviousNotes = New System.Windows.Forms.Label
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.RichTextBoxGuidelines = New System.Windows.Forms.RichTextBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.PanelRate.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'RadioButton2
        '
        Me.RadioButton2.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.RadioButton2.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.RadioButton2.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.RadioButton2.Location = New System.Drawing.Point(66, 28)
        Me.RadioButton2.Name = "RadioButton2"
        Me.RadioButton2.Size = New System.Drawing.Size(96, 18)
        Me.RadioButton2.TabIndex = 2
        Me.RadioButton2.TabStop = True
        Me.RadioButton2.Tag = "2"
        Me.RadioButton2.Text = "&2 Approved"
        Me.ToolTipRating.SetToolTip(Me.RadioButton2, "Reviewed for complete and accurate information " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "with regard to the noted area of" & _
                " focus.")
        Me.RadioButton2.UseVisualStyleBackColor = True
        '
        'RadioButton1
        '
        Me.RadioButton1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.RadioButton1.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.RadioButton1.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.RadioButton1.Location = New System.Drawing.Point(66, 5)
        Me.RadioButton1.Name = "RadioButton1"
        Me.RadioButton1.Size = New System.Drawing.Size(96, 19)
        Me.RadioButton1.TabIndex = 1
        Me.RadioButton1.TabStop = True
        Me.RadioButton1.Tag = "1"
        Me.RadioButton1.Text = "&1 Verified"
        Me.ToolTipRating.SetToolTip(Me.RadioButton1, "Verified to produce accurate results" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Note work tasks used ")
        Me.RadioButton1.UseVisualStyleBackColor = True
        '
        'RadioButton3
        '
        Me.RadioButton3.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.RadioButton3.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.RadioButton3.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.RadioButton3.Location = New System.Drawing.Point(66, 50)
        Me.RadioButton3.Name = "RadioButton3"
        Me.RadioButton3.Size = New System.Drawing.Size(96, 19)
        Me.RadioButton3.TabIndex = 3
        Me.RadioButton3.TabStop = True
        Me.RadioButton3.Tag = "3"
        Me.RadioButton3.Text = "&3 Helpful"
        Me.ToolTipRating.SetToolTip(Me.RadioButton3, "Fundamentally sound. May have unclear details as noted. Owner is encouraged to re" & _
                "vise and resubmit.")
        Me.RadioButton3.UseVisualStyleBackColor = True
        '
        'RadioButton4
        '
        Me.RadioButton4.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.RadioButton4.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.RadioButton4.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.RadioButton4.Location = New System.Drawing.Point(66, 74)
        Me.RadioButton4.Name = "RadioButton4"
        Me.RadioButton4.Size = New System.Drawing.Size(96, 17)
        Me.RadioButton4.TabIndex = 4
        Me.RadioButton4.TabStop = True
        Me.RadioButton4.Tag = "4"
        Me.RadioButton4.Text = "&4 Uncertain"
        Me.ToolTipRating.SetToolTip(Me.RadioButton4, "Area(s) of missing or inaccurate detail as noted. Recommend revision and resubmit" & _
                "tal.")
        Me.RadioButton4.UseVisualStyleBackColor = True
        '
        'RadioButton5
        '
        Me.RadioButton5.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.RadioButton5.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.RadioButton5.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.RadioButton5.Location = New System.Drawing.Point(66, 97)
        Me.RadioButton5.Name = "RadioButton5"
        Me.RadioButton5.Size = New System.Drawing.Size(96, 17)
        Me.RadioButton5.TabIndex = 5
        Me.RadioButton5.TabStop = True
        Me.RadioButton5.Tag = "5"
        Me.RadioButton5.Text = "&5 Incorrect"
        Me.ToolTipRating.SetToolTip(Me.RadioButton5, "Contains errors, omissions or mistakes that produce incorrect results. The noted " & _
                "corrections and/or recovery actions required.")
        Me.RadioButton5.UseVisualStyleBackColor = True
        '
        'RadioButton6
        '
        Me.RadioButton6.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.RadioButton6.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.RadioButton6.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.RadioButton6.Location = New System.Drawing.Point(66, 120)
        Me.RadioButton6.Name = "RadioButton6"
        Me.RadioButton6.Size = New System.Drawing.Size(96, 19)
        Me.RadioButton6.TabIndex = 6
        Me.RadioButton6.TabStop = True
        Me.RadioButton6.Tag = "6"
        Me.RadioButton6.Text = "&6 Block"
        Me.ToolTipRating.SetToolTip(Me.RadioButton6, "Do not use this information for the reason(s) noted. Correct results are not poss" & _
                "ible using this information.")
        Me.RadioButton6.UseVisualStyleBackColor = True
        '
        'RadioButton7
        '
        Me.RadioButton7.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.RadioButton7.BackColor = System.Drawing.SystemColors.ControlText
        Me.RadioButton7.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.RadioButton7.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.RadioButton7.ForeColor = System.Drawing.SystemColors.Control
        Me.RadioButton7.Location = New System.Drawing.Point(69, 143)
        Me.RadioButton7.Name = "RadioButton7"
        Me.RadioButton7.Padding = New System.Windows.Forms.Padding(0, 0, 2, 0)
        Me.RadioButton7.Size = New System.Drawing.Size(95, 17)
        Me.RadioButton7.TabIndex = 7
        Me.RadioButton7.TabStop = True
        Me.RadioButton7.Tag = "7"
        Me.RadioButton7.Text = "&7 Stand-aside"
        Me.ToolTipRating.SetToolTip(Me.RadioButton7, "Willing to support as determined appropriate by team.")
        Me.RadioButton7.UseVisualStyleBackColor = False
        '
        'RichTextBoxNewNote
        '
        Me.RichTextBoxNewNote.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.RichTextBoxNewNote.Location = New System.Drawing.Point(4, 16)
        Me.RichTextBoxNewNote.Name = "RichTextBoxNewNote"
        Me.RichTextBoxNewNote.Size = New System.Drawing.Size(424, 46)
        Me.RichTextBoxNewNote.TabIndex = 1
        Me.RichTextBoxNewNote.Text = Global.SQLClue.My.Resources.Resources.HelpOverview
        '
        'LabelNote
        '
        Me.LabelNote.AutoSize = True
        Me.LabelNote.Dock = System.Windows.Forms.DockStyle.Top
        Me.LabelNote.Location = New System.Drawing.Point(0, 0)
        Me.LabelNote.Name = "LabelNote"
        Me.LabelNote.Size = New System.Drawing.Size(64, 13)
        Me.LabelNote.TabIndex = 2
        Me.LabelNote.Text = "Rating Note"
        Me.ToolTipRating.SetToolTip(Me.LabelNote, "A few words to explain less than 'Accurate' ratings else a ")
        '
        'ButtonSend
        '
        Me.ButtonSend.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ButtonSend.Location = New System.Drawing.Point(516, 357)
        Me.ButtonSend.Name = "ButtonSend"
        Me.ButtonSend.Size = New System.Drawing.Size(95, 22)
        Me.ButtonSend.TabIndex = 3
        Me.ButtonSend.Text = "OK"
        Me.ButtonSend.UseVisualStyleBackColor = True
        '
        'PanelRate
        '
        Me.PanelRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.PanelRate.Controls.Add(Me.Label13)
        Me.PanelRate.Controls.Add(Me.Panel2)
        Me.PanelRate.Controls.Add(Me.Label2)
        Me.PanelRate.Controls.Add(Me.Label1)
        Me.PanelRate.Controls.Add(Me.RadioButton7)
        Me.PanelRate.Controls.Add(Me.RadioButton6)
        Me.PanelRate.Controls.Add(Me.RadioButton5)
        Me.PanelRate.Controls.Add(Me.RadioButton4)
        Me.PanelRate.Controls.Add(Me.RadioButton3)
        Me.PanelRate.Controls.Add(Me.RadioButton2)
        Me.PanelRate.Controls.Add(Me.RadioButton1)
        Me.PanelRate.Controls.Add(Me.Label3)
        Me.PanelRate.Location = New System.Drawing.Point(5, 5)
        Me.PanelRate.Margin = New System.Windows.Forms.Padding(0)
        Me.PanelRate.Name = "PanelRate"
        Me.PanelRate.Size = New System.Drawing.Size(168, 168)
        Me.PanelRate.TabIndex = 7
        '
        'Label13
        '
        Me.Label13.BackColor = System.Drawing.SystemColors.ControlText
        Me.Label13.ForeColor = System.Drawing.SystemColors.Control
        Me.Label13.Location = New System.Drawing.Point(2, 143)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(50, 17)
        Me.Label13.TabIndex = 11
        Me.Label13.Text = "not rated"
        Me.Label13.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Panel2
        '
        Me.Panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel2.Controls.Add(Me.Label5)
        Me.Panel2.Controls.Add(Me.Label6)
        Me.Panel2.Controls.Add(Me.Label7)
        Me.Panel2.Controls.Add(Me.Label8)
        Me.Panel2.Controls.Add(Me.Label9)
        Me.Panel2.Controls.Add(Me.Label10)
        Me.Panel2.Controls.Add(Me.Label11)
        Me.Panel2.Controls.Add(Me.Label14)
        Me.Panel2.Controls.Add(Me.Label12)
        Me.Panel2.Location = New System.Drawing.Point(54, 3)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(13, 161)
        Me.Panel2.TabIndex = 10
        '
        'Label5
        '
        Me.Label5.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label5.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.Label5.Location = New System.Drawing.Point(-2, 3)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(15, 17)
        Me.Label5.TabIndex = 1
        '
        'Label6
        '
        Me.Label6.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label6.BackColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.Label6.Location = New System.Drawing.Point(-2, 20)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(15, 17)
        Me.Label6.TabIndex = 2
        '
        'Label7
        '
        Me.Label7.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label7.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.Label7.Location = New System.Drawing.Point(-2, 37)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(15, 17)
        Me.Label7.TabIndex = 3
        '
        'Label8
        '
        Me.Label8.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label8.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.Label8.Location = New System.Drawing.Point(-2, 54)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(15, 17)
        Me.Label8.TabIndex = 4
        '
        'Label9
        '
        Me.Label9.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label9.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.Label9.Location = New System.Drawing.Point(-2, 70)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(15, 17)
        Me.Label9.TabIndex = 5
        '
        'Label10
        '
        Me.Label10.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label10.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.Label10.Location = New System.Drawing.Point(-2, 87)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(15, 17)
        Me.Label10.TabIndex = 6
        '
        'Label11
        '
        Me.Label11.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label11.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.Label11.Location = New System.Drawing.Point(-2, 103)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(15, 17)
        Me.Label11.TabIndex = 7
        '
        'Label14
        '
        Me.Label14.BackColor = System.Drawing.SystemColors.ControlText
        Me.Label14.Location = New System.Drawing.Point(-2, 139)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(57, 17)
        Me.Label14.TabIndex = 10
        '
        'Label12
        '
        Me.Label12.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label12.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.Label12.Location = New System.Drawing.Point(-2, 119)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(15, 17)
        Me.Label12.TabIndex = 8
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(2, 65)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(62, 13)
        Me.Label2.TabIndex = 8
        Me.Label2.Text = "Adequate---"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(3, 37)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(62, 13)
        Me.Label1.TabIndex = 7
        Me.Label1.Text = "Accurate ---"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(13, 92)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(55, 13)
        Me.Label3.TabIndex = 9
        Me.Label3.Text = "Avoid   ----"
        '
        'Label15
        '
        Me.Label15.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label15.AutoSize = True
        Me.Label15.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.5!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label15.Location = New System.Drawing.Point(401, 353)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(73, 12)
        Me.Label15.TabIndex = 8
        Me.Label15.Text = "Average Rating:"
        '
        'Label16
        '
        Me.Label16.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label16.AutoSize = True
        Me.Label16.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.5!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label16.Location = New System.Drawing.Point(401, 367)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(85, 12)
        Me.Label16.TabIndex = 9
        Me.Label16.Text = "Number of Ratings:"
        '
        'LabelAverageRating
        '
        Me.LabelAverageRating.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.LabelAverageRating.AutoSize = True
        Me.LabelAverageRating.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.5!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelAverageRating.Location = New System.Drawing.Point(489, 353)
        Me.LabelAverageRating.Name = "LabelAverageRating"
        Me.LabelAverageRating.Size = New System.Drawing.Size(10, 12)
        Me.LabelAverageRating.TabIndex = 10
        Me.LabelAverageRating.Text = "0"
        '
        'LabelNumberOfRatings
        '
        Me.LabelNumberOfRatings.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.LabelNumberOfRatings.AutoSize = True
        Me.LabelNumberOfRatings.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.5!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelNumberOfRatings.Location = New System.Drawing.Point(489, 366)
        Me.LabelNumberOfRatings.Name = "LabelNumberOfRatings"
        Me.LabelNumberOfRatings.Size = New System.Drawing.Size(10, 12)
        Me.LabelNumberOfRatings.TabIndex = 11
        Me.LabelNumberOfRatings.Text = "0"
        '
        'RichTextBoxPreviousNotes
        '
        Me.RichTextBoxPreviousNotes.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.RichTextBoxPreviousNotes.Location = New System.Drawing.Point(4, 16)
        Me.RichTextBoxPreviousNotes.Name = "RichTextBoxPreviousNotes"
        Me.RichTextBoxPreviousNotes.ReadOnly = True
        Me.RichTextBoxPreviousNotes.Size = New System.Drawing.Size(424, 252)
        Me.RichTextBoxPreviousNotes.TabIndex = 12
        Me.RichTextBoxPreviousNotes.Text = Global.SQLClue.My.Resources.Resources.HelpOverview
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SplitContainer1.BackColor = System.Drawing.SystemColors.GradientInactiveCaption
        Me.SplitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.SplitContainer1.Location = New System.Drawing.Point(177, 6)
        Me.SplitContainer1.Margin = New System.Windows.Forms.Padding(0)
        Me.SplitContainer1.Name = "SplitContainer1"
        Me.SplitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.BackColor = System.Drawing.SystemColors.Control
        Me.SplitContainer1.Panel1.Controls.Add(Me.RichTextBoxPreviousNotes)
        Me.SplitContainer1.Panel1.Controls.Add(Me.LabelPreviousNotes)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.BackColor = System.Drawing.SystemColors.Control
        Me.SplitContainer1.Panel2.Controls.Add(Me.LabelNote)
        Me.SplitContainer1.Panel2.Controls.Add(Me.RichTextBoxNewNote)
        Me.SplitContainer1.Panel2MinSize = 60
        Me.SplitContainer1.Size = New System.Drawing.Size(434, 346)
        Me.SplitContainer1.SplitterDistance = 274
        Me.SplitContainer1.TabIndex = 13
        '
        'LabelPreviousNotes
        '
        Me.LabelPreviousNotes.AutoSize = True
        Me.LabelPreviousNotes.Dock = System.Windows.Forms.DockStyle.Top
        Me.LabelPreviousNotes.Location = New System.Drawing.Point(0, 0)
        Me.LabelPreviousNotes.Name = "LabelPreviousNotes"
        Me.LabelPreviousNotes.Size = New System.Drawing.Size(113, 13)
        Me.LabelPreviousNotes.TabIndex = 0
        Me.LabelPreviousNotes.Text = "Previous Rating Notes"
        '
        'Panel1
        '
        Me.Panel1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.RichTextBoxGuidelines)
        Me.Panel1.Controls.Add(Me.Label4)
        Me.Panel1.Location = New System.Drawing.Point(5, 177)
        Me.Panel1.MinimumSize = New System.Drawing.Size(168, 175)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(168, 175)
        Me.Panel1.TabIndex = 15
        '
        'RichTextBoxGuidelines
        '
        Me.RichTextBoxGuidelines.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.RichTextBoxGuidelines.BackColor = System.Drawing.SystemColors.Control
        Me.RichTextBoxGuidelines.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.RichTextBoxGuidelines.BulletIndent = 5
        Me.RichTextBoxGuidelines.Location = New System.Drawing.Point(4, 17)
        Me.RichTextBoxGuidelines.Name = "RichTextBoxGuidelines"
        Me.RichTextBoxGuidelines.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical
        Me.RichTextBoxGuidelines.Size = New System.Drawing.Size(158, 152)
        Me.RichTextBoxGuidelines.TabIndex = 16
        Me.RichTextBoxGuidelines.Text = Global.SQLClue.My.Resources.Resources.HelpOverview
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label4.Location = New System.Drawing.Point(0, 0)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(108, 13)
        Me.Label4.TabIndex = 15
        Me.Label4.Text = "Peer Review Defined"
        '
        'DialogRating
        '
        Me.AcceptButton = Me.ButtonSend
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(616, 382)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.PanelRate)
        Me.Controls.Add(Me.SplitContainer1)
        Me.Controls.Add(Me.LabelNumberOfRatings)
        Me.Controls.Add(Me.LabelAverageRating)
        Me.Controls.Add(Me.Label16)
        Me.Controls.Add(Me.Label15)
        Me.Controls.Add(Me.ButtonSend)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(410, 212)
        Me.Name = "DialogRating"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "SQLClue Runbook Peer Review"
        Me.PanelRate.ResumeLayout(False)
        Me.PanelRate.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel1.PerformLayout()
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        Me.SplitContainer1.Panel2.PerformLayout()
        Me.SplitContainer1.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents RadioButton7 As System.Windows.Forms.RadioButton
    Friend WithEvents RadioButton6 As System.Windows.Forms.RadioButton
    Friend WithEvents RadioButton5 As System.Windows.Forms.RadioButton
    Friend WithEvents RadioButton4 As System.Windows.Forms.RadioButton
    Friend WithEvents RadioButton3 As System.Windows.Forms.RadioButton
    Friend WithEvents RadioButton2 As System.Windows.Forms.RadioButton
    Friend WithEvents RadioButton1 As System.Windows.Forms.RadioButton
    Friend WithEvents RichTextBoxNewNote As System.Windows.Forms.RichTextBox
    Friend WithEvents LabelNote As System.Windows.Forms.Label
    Friend WithEvents ButtonSend As System.Windows.Forms.Button
    Friend WithEvents PanelRate As System.Windows.Forms.Panel
    Friend WithEvents ToolTipRating As System.Windows.Forms.ToolTip
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents LabelAverageRating As System.Windows.Forms.Label
    Friend WithEvents LabelNumberOfRatings As System.Windows.Forms.Label
    Friend WithEvents RichTextBoxPreviousNotes As System.Windows.Forms.RichTextBox
    Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
    Friend WithEvents LabelPreviousNotes As System.Windows.Forms.Label
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents RichTextBoxGuidelines As System.Windows.Forms.RichTextBox
End Class
