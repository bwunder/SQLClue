<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class DialogRunbookLookupCriteria
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
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel
        Me.ButtonCancel = New System.Windows.Forms.Button
        Me.ButtonSearch = New System.Windows.Forms.Button
        Me.ComboBoxPerson = New System.Windows.Forms.ComboBox
        Me.Label6 = New System.Windows.Forms.Label
        Me.ComboBoxCategory = New System.Windows.Forms.ComboBox
        Me.LabelCategoryLookup = New System.Windows.Forms.Label
        Me.LabelBeginDate = New System.Windows.Forms.Label
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.LabelMembers = New System.Windows.Forms.Label
        Me.LabelContains = New System.Windows.Forms.Label
        Me.Panel5 = New System.Windows.Forms.Panel
        Me.RadioButtonAnd = New System.Windows.Forms.RadioButton
        Me.RadioButtonOr = New System.Windows.Forms.RadioButton
        Me.ListBoxFullText = New System.Windows.Forms.ListBox
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.RichTextBoxContains = New System.Windows.Forms.RichTextBox
        Me.FlowLayoutPanel1 = New System.Windows.Forms.FlowLayoutPanel
        Me.Label5 = New System.Windows.Forms.Label
        Me.Panel2 = New System.Windows.Forms.Panel
        Me.Panel4 = New System.Windows.Forms.Panel
        Me.ComboBoxRatingOperator = New System.Windows.Forms.ComboBox
        Me.NumericUpDownRating = New System.Windows.Forms.NumericUpDown
        Me.Label4 = New System.Windows.Forms.Label
        Me.ComboBoxDocument = New System.Windows.Forms.ComboBox
        Me.ComboBoxTopic = New System.Windows.Forms.ComboBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.Panel3 = New System.Windows.Forms.Panel
        Me.LabelEndDate = New System.Windows.Forms.Label
        Me.DateTimePickerEndDate = New System.Windows.Forms.DateTimePicker
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.DateTimePickerStartDate = New System.Windows.Forms.DateTimePicker
        Me.ResetButton = New System.Windows.Forms.Button
        Me.TableLayoutPanel1.SuspendLayout()
        Me.Panel5.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.FlowLayoutPanel1.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.Panel4.SuspendLayout()
        CType(Me.NumericUpDownRating, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel3.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TableLayoutPanel1.ColumnCount = 2
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.ButtonCancel, 1, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.ButtonSearch, 0, 0)
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(315, 290)
        Me.TableLayoutPanel1.Margin = New System.Windows.Forms.Padding(0)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 1
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(194, 22)
        Me.TableLayoutPanel1.TabIndex = 0
        '
        'ButtonCancel
        '
        Me.ButtonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.ButtonCancel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ButtonCancel.Location = New System.Drawing.Point(97, 0)
        Me.ButtonCancel.Margin = New System.Windows.Forms.Padding(0)
        Me.ButtonCancel.Name = "ButtonCancel"
        Me.ButtonCancel.Size = New System.Drawing.Size(97, 22)
        Me.ButtonCancel.TabIndex = 1
        Me.ButtonCancel.Text = "Cancel"
        '
        'ButtonSearch
        '
        Me.ButtonSearch.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ButtonSearch.Location = New System.Drawing.Point(0, 0)
        Me.ButtonSearch.Margin = New System.Windows.Forms.Padding(0)
        Me.ButtonSearch.Name = "ButtonSearch"
        Me.ButtonSearch.Size = New System.Drawing.Size(97, 22)
        Me.ButtonSearch.TabIndex = 0
        Me.ButtonSearch.Text = "Search"
        '
        'ComboBoxPerson
        '
        Me.ComboBoxPerson.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.ComboBoxPerson.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.ComboBoxPerson.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBoxPerson.DropDownWidth = 300
        Me.ComboBoxPerson.FormattingEnabled = True
        Me.ComboBoxPerson.Location = New System.Drawing.Point(310, 54)
        Me.ComboBoxPerson.Name = "ComboBoxPerson"
        Me.ComboBoxPerson.Size = New System.Drawing.Size(190, 21)
        Me.ComboBoxPerson.Sorted = True
        Me.ComboBoxPerson.TabIndex = 43
        Me.ComboBoxPerson.Tag = "Person"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(277, 57)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(29, 13)
        Me.Label6.TabIndex = 42
        Me.Label6.Text = "User"
        '
        'ComboBoxCategory
        '
        Me.ComboBoxCategory.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.ComboBoxCategory.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.ComboBoxCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBoxCategory.DropDownWidth = 300
        Me.ComboBoxCategory.FormattingEnabled = True
        Me.ComboBoxCategory.Location = New System.Drawing.Point(77, 54)
        Me.ComboBoxCategory.Name = "ComboBoxCategory"
        Me.ComboBoxCategory.Size = New System.Drawing.Size(189, 21)
        Me.ComboBoxCategory.TabIndex = 40
        Me.ComboBoxCategory.Tag = "Category"
        '
        'LabelCategoryLookup
        '
        Me.LabelCategoryLookup.AutoSize = True
        Me.LabelCategoryLookup.Location = New System.Drawing.Point(8, 57)
        Me.LabelCategoryLookup.Name = "LabelCategoryLookup"
        Me.LabelCategoryLookup.Size = New System.Drawing.Size(49, 13)
        Me.LabelCategoryLookup.TabIndex = 41
        Me.LabelCategoryLookup.Text = "Category"
        '
        'LabelBeginDate
        '
        Me.LabelBeginDate.AutoSize = True
        Me.LabelBeginDate.Location = New System.Drawing.Point(3, 2)
        Me.LabelBeginDate.Name = "LabelBeginDate"
        Me.LabelBeginDate.Size = New System.Drawing.Size(30, 13)
        Me.LabelBeginDate.TabIndex = 39
        Me.LabelBeginDate.Text = "Date"
        Me.ToolTip1.SetToolTip(Me.LabelBeginDate, "Check only Start Date or set both to same date for one day ")
        '
        'LabelMembers
        '
        Me.LabelMembers.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.LabelMembers.Location = New System.Drawing.Point(420, 2)
        Me.LabelMembers.Name = "LabelMembers"
        Me.LabelMembers.Size = New System.Drawing.Size(63, 18)
        Me.LabelMembers.TabIndex = 54
        Me.LabelMembers.Text = "Search In"
        Me.ToolTip1.SetToolTip(Me.LabelMembers, "Select the entities to include in the Full-Text Search. ")
        '
        'LabelContains
        '
        Me.LabelContains.AutoSize = True
        Me.LabelContains.Location = New System.Drawing.Point(3, 0)
        Me.LabelContains.Name = "LabelContains"
        Me.LabelContains.Size = New System.Drawing.Size(62, 13)
        Me.LabelContains.TabIndex = 49
        Me.LabelContains.Text = "CONTAINS"
        Me.ToolTip1.SetToolTip(Me.LabelContains, "See SQL Server CONTAINS predicate documentation for all valid usage")
        '
        'Panel5
        '
        Me.Panel5.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel5.Controls.Add(Me.RadioButtonAnd)
        Me.Panel5.Controls.Add(Me.RadioButtonOr)
        Me.Panel5.Location = New System.Drawing.Point(407, 128)
        Me.Panel5.Name = "Panel5"
        Me.Panel5.Size = New System.Drawing.Size(93, 18)
        Me.Panel5.TabIndex = 57
        Me.ToolTip1.SetToolTip(Me.Panel5, "Query for any (Or) or all (And) criteria with values. Unspecified criteria are al" & _
                "ways ignored. ")
        '
        'RadioButtonAnd
        '
        Me.RadioButtonAnd.AutoSize = True
        Me.RadioButtonAnd.Location = New System.Drawing.Point(47, -1)
        Me.RadioButtonAnd.Name = "RadioButtonAnd"
        Me.RadioButtonAnd.Size = New System.Drawing.Size(44, 17)
        Me.RadioButtonAnd.TabIndex = 1
        Me.RadioButtonAnd.Text = "And"
        Me.RadioButtonAnd.UseVisualStyleBackColor = True
        '
        'RadioButtonOr
        '
        Me.RadioButtonOr.AutoSize = True
        Me.RadioButtonOr.Checked = True
        Me.RadioButtonOr.Location = New System.Drawing.Point(4, -1)
        Me.RadioButtonOr.Name = "RadioButtonOr"
        Me.RadioButtonOr.Size = New System.Drawing.Size(36, 17)
        Me.RadioButtonOr.TabIndex = 0
        Me.RadioButtonOr.TabStop = True
        Me.RadioButtonOr.Text = "Or"
        Me.RadioButtonOr.UseVisualStyleBackColor = True
        '
        'ListBoxFullText
        '
        Me.ListBoxFullText.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ListBoxFullText.Items.AddRange(New Object() {"(none)", "Categories", "Topics", "Documents", "Ratings", "(all)"})
        Me.ListBoxFullText.Location = New System.Drawing.Point(419, 19)
        Me.ListBoxFullText.Name = "ListBoxFullText"
        Me.ListBoxFullText.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple
        Me.ListBoxFullText.Size = New System.Drawing.Size(71, 95)
        Me.ListBoxFullText.TabIndex = 55
        '
        'Panel1
        '
        Me.Panel1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.RichTextBoxContains)
        Me.Panel1.Controls.Add(Me.FlowLayoutPanel1)
        Me.Panel1.Controls.Add(Me.ListBoxFullText)
        Me.Panel1.Controls.Add(Me.LabelMembers)
        Me.Panel1.Location = New System.Drawing.Point(4, 151)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(496, 123)
        Me.Panel1.TabIndex = 56
        '
        'RichTextBoxContains
        '
        Me.RichTextBoxContains.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.RichTextBoxContains.Location = New System.Drawing.Point(4, 18)
        Me.RichTextBoxContains.Margin = New System.Windows.Forms.Padding(0)
        Me.RichTextBoxContains.Name = "RichTextBoxContains"
        Me.RichTextBoxContains.Size = New System.Drawing.Size(411, 99)
        Me.RichTextBoxContains.TabIndex = 57
        Me.RichTextBoxContains.Text = Global.SQLClue.My.Resources.Resources.HelpOverview
        '
        'FlowLayoutPanel1
        '
        Me.FlowLayoutPanel1.Controls.Add(Me.LabelContains)
        Me.FlowLayoutPanel1.Controls.Add(Me.Label5)
        Me.FlowLayoutPanel1.Location = New System.Drawing.Point(5, 3)
        Me.FlowLayoutPanel1.Name = "FlowLayoutPanel1"
        Me.FlowLayoutPanel1.Size = New System.Drawing.Size(411, 19)
        Me.FlowLayoutPanel1.TabIndex = 56
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.5!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.ForeColor = System.Drawing.SystemColors.Highlight
        Me.Label5.Location = New System.Drawing.Point(71, 0)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(290, 12)
        Me.Label5.TabIndex = 50
        Me.Label5.Text = "any valid SQL Server Full-Text CONTAINS predicate search condition"
        '
        'Panel2
        '
        Me.Panel2.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel2.Controls.Add(Me.Panel5)
        Me.Panel2.Controls.Add(Me.ComboBoxPerson)
        Me.Panel2.Controls.Add(Me.Panel1)
        Me.Panel2.Controls.Add(Me.Panel4)
        Me.Panel2.Controls.Add(Me.ComboBoxDocument)
        Me.Panel2.Controls.Add(Me.ComboBoxTopic)
        Me.Panel2.Controls.Add(Me.Label6)
        Me.Panel2.Controls.Add(Me.Label3)
        Me.Panel2.Controls.Add(Me.LabelCategoryLookup)
        Me.Panel2.Controls.Add(Me.Label2)
        Me.Panel2.Controls.Add(Me.Panel3)
        Me.Panel2.Controls.Add(Me.ComboBoxCategory)
        Me.Panel2.Location = New System.Drawing.Point(4, 5)
        Me.Panel2.Margin = New System.Windows.Forms.Padding(0)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(506, 280)
        Me.Panel2.TabIndex = 57
        '
        'Panel4
        '
        Me.Panel4.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel4.Controls.Add(Me.ComboBoxRatingOperator)
        Me.Panel4.Controls.Add(Me.NumericUpDownRating)
        Me.Panel4.Controls.Add(Me.Label4)
        Me.Panel4.Location = New System.Drawing.Point(407, 80)
        Me.Panel4.Name = "Panel4"
        Me.Panel4.Size = New System.Drawing.Size(93, 44)
        Me.Panel4.TabIndex = 52
        '
        'ComboBoxRatingOperator
        '
        Me.ComboBoxRatingOperator.AutoCompleteCustomSource.AddRange(New String() {">", ">=", "=", "<", "<="})
        Me.ComboBoxRatingOperator.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.ComboBoxRatingOperator.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.ComboBoxRatingOperator.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboBoxRatingOperator.Items.AddRange(New Object() {" >", " =", " <"})
        Me.ComboBoxRatingOperator.Location = New System.Drawing.Point(4, 18)
        Me.ComboBoxRatingOperator.MaxDropDownItems = 3
        Me.ComboBoxRatingOperator.MaxLength = 2
        Me.ComboBoxRatingOperator.Name = "ComboBoxRatingOperator"
        Me.ComboBoxRatingOperator.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ComboBoxRatingOperator.Size = New System.Drawing.Size(37, 21)
        Me.ComboBoxRatingOperator.TabIndex = 3
        Me.ComboBoxRatingOperator.Text = ">"
        '
        'NumericUpDownRating
        '
        Me.NumericUpDownRating.Location = New System.Drawing.Point(45, 18)
        Me.NumericUpDownRating.Maximum = New Decimal(New Integer() {7, 0, 0, 0})
        Me.NumericUpDownRating.Name = "NumericUpDownRating"
        Me.NumericUpDownRating.Size = New System.Drawing.Size(41, 20)
        Me.NumericUpDownRating.TabIndex = 2
        Me.NumericUpDownRating.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label4
        '
        Me.Label4.Location = New System.Drawing.Point(2, 0)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(82, 14)
        Me.Label4.TabIndex = 0
        Me.Label4.Text = "Peer Rating"
        '
        'ComboBoxDocument
        '
        Me.ComboBoxDocument.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ComboBoxDocument.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.ComboBoxDocument.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.ComboBoxDocument.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBoxDocument.DropDownWidth = 600
        Me.ComboBoxDocument.FormattingEnabled = True
        Me.ComboBoxDocument.Location = New System.Drawing.Point(77, 29)
        Me.ComboBoxDocument.MaxLength = 450
        Me.ComboBoxDocument.Name = "ComboBoxDocument"
        Me.ComboBoxDocument.Size = New System.Drawing.Size(423, 21)
        Me.ComboBoxDocument.TabIndex = 42
        '
        'ComboBoxTopic
        '
        Me.ComboBoxTopic.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ComboBoxTopic.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.ComboBoxTopic.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.ComboBoxTopic.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBoxTopic.DropDownWidth = 600
        Me.ComboBoxTopic.FormattingEnabled = True
        Me.ComboBoxTopic.Location = New System.Drawing.Point(77, 4)
        Me.ComboBoxTopic.Name = "ComboBoxTopic"
        Me.ComboBoxTopic.Size = New System.Drawing.Size(423, 21)
        Me.ComboBoxTopic.TabIndex = 41
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(7, 32)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(56, 13)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "Document"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(8, 7)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(34, 13)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Topic"
        '
        'Panel3
        '
        Me.Panel3.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel3.Controls.Add(Me.LabelEndDate)
        Me.Panel3.Controls.Add(Me.DateTimePickerEndDate)
        Me.Panel3.Controls.Add(Me.GroupBox1)
        Me.Panel3.Controls.Add(Me.LabelBeginDate)
        Me.Panel3.Location = New System.Drawing.Point(4, 79)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(399, 67)
        Me.Panel3.TabIndex = 0
        '
        'LabelEndDate
        '
        Me.LabelEndDate.AutoSize = True
        Me.LabelEndDate.Location = New System.Drawing.Point(48, 46)
        Me.LabelEndDate.Name = "LabelEndDate"
        Me.LabelEndDate.Size = New System.Drawing.Size(26, 13)
        Me.LabelEndDate.TabIndex = 49
        Me.LabelEndDate.Text = "End"
        '
        'DateTimePickerEndDate
        '
        Me.DateTimePickerEndDate.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.DateTimePickerEndDate.CustomFormat = "MMMM dd, yyyy"
        Me.DateTimePickerEndDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.DateTimePickerEndDate.Location = New System.Drawing.Point(87, 39)
        Me.DateTimePickerEndDate.Name = "DateTimePickerEndDate"
        Me.DateTimePickerEndDate.ShowCheckBox = True
        Me.DateTimePickerEndDate.Size = New System.Drawing.Size(300, 20)
        Me.DateTimePickerEndDate.TabIndex = 48
        Me.DateTimePickerEndDate.Tag = "Date"
        Me.DateTimePickerEndDate.Value = New Date(2008, 4, 1, 16, 20, 0, 0)
        '
        'GroupBox1
        '
        Me.GroupBox1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Controls.Add(Me.DateTimePickerStartDate)
        Me.GroupBox1.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.5!)
        Me.GroupBox1.ForeColor = System.Drawing.SystemColors.Highlight
        Me.GroupBox1.Location = New System.Drawing.Point(39, 0)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(354, 36)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "One Day"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!)
        Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label1.Location = New System.Drawing.Point(9, 18)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(29, 13)
        Me.Label1.TabIndex = 54
        Me.Label1.Text = "Start"
        '
        'DateTimePickerStartDate
        '
        Me.DateTimePickerStartDate.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.DateTimePickerStartDate.CustomFormat = "MMMM dd, yyyy"
        Me.DateTimePickerStartDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DateTimePickerStartDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.DateTimePickerStartDate.Location = New System.Drawing.Point(48, 11)
        Me.DateTimePickerStartDate.Name = "DateTimePickerStartDate"
        Me.DateTimePickerStartDate.ShowCheckBox = True
        Me.DateTimePickerStartDate.Size = New System.Drawing.Size(300, 20)
        Me.DateTimePickerStartDate.TabIndex = 53
        Me.DateTimePickerStartDate.Tag = "Date"
        Me.DateTimePickerStartDate.Value = New Date(2008, 3, 29, 0, 0, 0, 0)
        '
        'ResetButton
        '
        Me.ResetButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.ResetButton.Location = New System.Drawing.Point(9, 289)
        Me.ResetButton.Name = "ResetButton"
        Me.ResetButton.Size = New System.Drawing.Size(75, 23)
        Me.ResetButton.TabIndex = 58
        Me.ResetButton.Text = "Reset Form"
        Me.ResetButton.UseVisualStyleBackColor = True
        '
        'DialogRunbookLookupCriteria
        '
        Me.AcceptButton = Me.ButtonSearch
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.ButtonCancel
        Me.ClientSize = New System.Drawing.Size(514, 316)
        Me.Controls.Add(Me.ResetButton)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.Controls.Add(Me.Panel2)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(530, 350)
        Me.Name = "DialogRunbookLookupCriteria"
        Me.Padding = New System.Windows.Forms.Padding(4)
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "SQL Runbook Lookup Criteria"
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.Panel5.ResumeLayout(False)
        Me.Panel5.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        Me.FlowLayoutPanel1.ResumeLayout(False)
        Me.FlowLayoutPanel1.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.Panel4.ResumeLayout(False)
        CType(Me.NumericUpDownRating, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel3.ResumeLayout(False)
        Me.Panel3.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents ButtonSearch As System.Windows.Forms.Button
    Friend WithEvents ButtonCancel As System.Windows.Forms.Button
    Friend WithEvents ComboBoxPerson As System.Windows.Forms.ComboBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents ComboBoxCategory As System.Windows.Forms.ComboBox
    Friend WithEvents LabelCategoryLookup As System.Windows.Forms.Label
    Friend WithEvents LabelBeginDate As System.Windows.Forms.Label
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents LabelMembers As System.Windows.Forms.Label
    Friend WithEvents ListBoxFullText As System.Windows.Forms.ListBox
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents DateTimePickerStartDate As System.Windows.Forms.DateTimePicker
    Friend WithEvents LabelEndDate As System.Windows.Forms.Label
    Friend WithEvents DateTimePickerEndDate As System.Windows.Forms.DateTimePicker
    Friend WithEvents ComboBoxDocument As System.Windows.Forms.ComboBox
    Friend WithEvents ComboBoxTopic As System.Windows.Forms.ComboBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents ResetButton As System.Windows.Forms.Button
    Friend WithEvents Panel4 As System.Windows.Forms.Panel
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents NumericUpDownRating As System.Windows.Forms.NumericUpDown
    Friend WithEvents ComboBoxRatingOperator As System.Windows.Forms.ComboBox
    Friend WithEvents RichTextBoxContains As System.Windows.Forms.RichTextBox
    Friend WithEvents FlowLayoutPanel1 As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents LabelContains As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Panel5 As System.Windows.Forms.Panel
    Friend WithEvents RadioButtonOr As System.Windows.Forms.RadioButton
    Friend WithEvents RadioButtonAnd As System.Windows.Forms.RadioButton

End Class
