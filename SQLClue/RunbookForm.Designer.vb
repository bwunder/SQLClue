<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class RunbookForm
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
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle6 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle7 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle8 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle9 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle14 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle15 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle16 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle10 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle11 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle12 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle13 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(RunbookForm))
        Me.StatusStripSQLRunbook = New System.Windows.Forms.StatusStrip
        Me.ToolStripStatusLabelRunbook = New System.Windows.Forms.ToolStripStatusLabel
        Me.ToolStripContainer1 = New System.Windows.Forms.ToolStripContainer
        Me.SplitContainerResults = New System.Windows.Forms.SplitContainer
        Me.PanelTopicList = New System.Windows.Forms.Panel
        Me.PanelLabelTopicList = New System.Windows.Forms.Label
        Me.DataGridViewTopicList = New System.Windows.Forms.DataGridView
        Me.DataGridViewButtonColumnTopicRating = New System.Windows.Forms.DataGridViewButtonColumn
        Me.DataGridViewTextBoxColumnTopicId = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.DataGridViewButtonColumnTopicName = New System.Windows.Forms.DataGridViewButtonColumn
        Me.DataGridViewButtonColumnTopicNotes = New System.Windows.Forms.DataGridViewButtonColumn
        Me.DataGridViewTextBoxColumnTopicNbrDocs = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.DataGridViewTextBoxColumnTopicRatingCount = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.DataGridViewTextBoxColumnTopicRatingTally = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.DataGridViewComboBoxColumnTopicOwner = New System.Windows.Forms.DataGridViewComboBoxColumn
        Me.DataGridViewTextBoxColumnTopicRecCreatedDt = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.DataGridViewTextBoxColumnTopicRecCreatedUser = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.DataGridViewTextBoxColumnTopicLastUpdatedDt = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.DataGridViewTextBoxColumnTopicLastUpdatedUser = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.BindingSourceTopic = New System.Windows.Forms.BindingSource(Me.components)
        Me.DataSetSQLRunbook = New SQLClue.DataSetSQLRunbook
        Me.LinkLabel1 = New System.Windows.Forms.LinkLabel
        Me.Panel2 = New System.Windows.Forms.Panel
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer
        Me.ButtonSave = New System.Windows.Forms.Button
        Me.ButtonCancel = New System.Windows.Forms.Button
        Me.SplitContainerTopic = New System.Windows.Forms.SplitContainer
        Me.PanelTopicDetail = New System.Windows.Forms.Panel
        Me.PanelDocumentList = New System.Windows.Forms.Panel
        Me.Label8 = New System.Windows.Forms.Label
        Me.DataGridViewDocumentList = New System.Windows.Forms.DataGridView
        Me.DataGridViewButtonColumnDocumentRating = New System.Windows.Forms.DataGridViewButtonColumn
        Me.DataGridViewTextBoxColumnDocumentId = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.DataGridViewButtonColumnDocumentFile = New System.Windows.Forms.DataGridViewButtonColumn
        Me.DataGridViewButtonDocumentShortFile = New System.Windows.Forms.DataGridViewButtonColumn
        Me.DocumentDataGridViewColumnDocument = New System.Windows.Forms.DataGridViewButtonColumn
        Me.DataGridViewComboBoxColumnDocType = New System.Windows.Forms.DataGridViewComboBoxColumn
        Me.DataGridViewComboBoxColumnDocumentOwner = New System.Windows.Forms.DataGridViewComboBoxColumn
        Me.DataGridViewTextBoxColumnDocumentLastModifiedDt = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.DataGridViewTextBoxColumnDocumentRatingTally = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.DataGridViewTextBoxColumnDocumentRatingCount = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.DataGridViewCheckBoxDocumentWatchFileForChange = New System.Windows.Forms.DataGridViewCheckBoxColumn
        Me.BindingSourceDocument = New System.Windows.Forms.BindingSource(Me.components)
        Me.Label1 = New System.Windows.Forms.Label
        Me.ButtonNotes = New System.Windows.Forms.Button
        Me.ComboBoxTopicName = New System.Windows.Forms.ComboBox
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.PanelCategoryList = New System.Windows.Forms.Panel
        Me.FlowLayoutPanel1 = New System.Windows.Forms.FlowLayoutPanel
        Me.Label2 = New System.Windows.Forms.Label
        Me.LabelCategoryHelper = New System.Windows.Forms.Label
        Me.ListBoxCategory = New System.Windows.Forms.ListBox
        Me.ToolTipSQLRunbook = New System.Windows.Forms.ToolTip(Me.components)
        Me.OpenFileDialogDocument = New System.Windows.Forms.OpenFileDialog
        Me.TableAdapterDocument = New SQLClue.DataSetSQLRunbookTableAdapters.tDocumentTableAdapter
        Me.TableAdapterTopic = New SQLClue.DataSetSQLRunbookTableAdapters.tTopicTableAdapter
        Me.ToolStripContainer1.BottomToolStripPanel.SuspendLayout()
        Me.ToolStripContainer1.ContentPanel.SuspendLayout()
        Me.ToolStripContainer1.SuspendLayout()
        Me.SplitContainerResults.Panel1.SuspendLayout()
        Me.SplitContainerResults.Panel2.SuspendLayout()
        Me.SplitContainerResults.SuspendLayout()
        Me.PanelTopicList.SuspendLayout()
        CType(Me.DataGridViewTopicList, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BindingSourceTopic, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DataSetSQLRunbook, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel2.SuspendLayout()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        Me.SplitContainerTopic.Panel1.SuspendLayout()
        Me.SplitContainerTopic.Panel2.SuspendLayout()
        Me.SplitContainerTopic.SuspendLayout()
        Me.PanelTopicDetail.SuspendLayout()
        Me.PanelDocumentList.SuspendLayout()
        CType(Me.DataGridViewDocumentList, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BindingSourceDocument, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel1.SuspendLayout()
        Me.PanelCategoryList.SuspendLayout()
        Me.FlowLayoutPanel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'StatusStripSQLRunbook
        '
        Me.StatusStripSQLRunbook.BackColor = System.Drawing.SystemColors.Control
        Me.StatusStripSQLRunbook.Dock = System.Windows.Forms.DockStyle.None
        Me.StatusStripSQLRunbook.Location = New System.Drawing.Point(0, 0)
        Me.StatusStripSQLRunbook.Name = "StatusStripSQLRunbook"
        Me.StatusStripSQLRunbook.Size = New System.Drawing.Size(730, 22)
        Me.StatusStripSQLRunbook.TabIndex = 18
        Me.StatusStripSQLRunbook.Text = "StatusStripRunbook"
        '
        'ToolStripStatusLabelRunbook
        '
        Me.ToolStripStatusLabelRunbook.AutoSize = False
        Me.ToolStripStatusLabelRunbook.Name = "ToolStripStatusLabelRunbook"
        Me.ToolStripStatusLabelRunbook.Size = New System.Drawing.Size(715, 17)
        Me.ToolStripStatusLabelRunbook.Spring = True
        Me.ToolStripStatusLabelRunbook.Text = "Add or Select a Topic"
        Me.ToolStripStatusLabelRunbook.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ToolStripContainer1
        '
        '
        'ToolStripContainer1.BottomToolStripPanel
        '
        Me.ToolStripContainer1.BottomToolStripPanel.Controls.Add(Me.StatusStripSQLRunbook)
        '
        'ToolStripContainer1.ContentPanel
        '
        Me.ToolStripContainer1.ContentPanel.AutoScroll = True
        Me.ToolStripContainer1.ContentPanel.BackColor = System.Drawing.SystemColors.Window
        Me.ToolStripContainer1.ContentPanel.Controls.Add(Me.SplitContainerResults)
        Me.ToolStripContainer1.ContentPanel.Margin = New System.Windows.Forms.Padding(0)
        Me.ToolStripContainer1.ContentPanel.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional
        Me.ToolStripContainer1.ContentPanel.Size = New System.Drawing.Size(730, 408)
        Me.ToolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ToolStripContainer1.LeftToolStripPanelVisible = False
        Me.ToolStripContainer1.Location = New System.Drawing.Point(0, 0)
        Me.ToolStripContainer1.Name = "ToolStripContainer1"
        '
        'ToolStripContainer1.RightToolStripPanel
        '
        Me.ToolTipSQLRunbook.SetToolTip(Me.ToolStripContainer1.RightToolStripPanel, "Notes for the selected Topic")
        Me.ToolStripContainer1.RightToolStripPanelVisible = False
        Me.ToolStripContainer1.Size = New System.Drawing.Size(730, 430)
        Me.ToolStripContainer1.TabIndex = 19
        Me.ToolStripContainer1.Text = "ToolStripContainer1"
        Me.ToolStripContainer1.TopToolStripPanelVisible = False
        '
        'SplitContainerResults
        '
        Me.SplitContainerResults.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SplitContainerResults.BackColor = System.Drawing.SystemColors.GradientInactiveCaption
        Me.SplitContainerResults.Location = New System.Drawing.Point(4, 4)
        Me.SplitContainerResults.Name = "SplitContainerResults"
        Me.SplitContainerResults.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainerResults.Panel1
        '
        Me.SplitContainerResults.Panel1.BackColor = System.Drawing.SystemColors.Control
        Me.SplitContainerResults.Panel1.Controls.Add(Me.PanelTopicList)
        Me.SplitContainerResults.Panel1.Tag = "TopicListPanel"
        '
        'SplitContainerResults.Panel2
        '
        Me.SplitContainerResults.Panel2.BackColor = System.Drawing.SystemColors.Control
        Me.SplitContainerResults.Panel2.Controls.Add(Me.Panel2)
        Me.SplitContainerResults.Panel2.Tag = "TopicDetailPanel"
        Me.SplitContainerResults.Size = New System.Drawing.Size(722, 403)
        Me.SplitContainerResults.SplitterDistance = 150
        Me.SplitContainerResults.TabIndex = 21
        Me.SplitContainerResults.Tag = Global.SQLClue.My.Resources.Resources.HelpOverview
        '
        'PanelTopicList
        '
        Me.PanelTopicList.BackColor = System.Drawing.SystemColors.Control
        Me.PanelTopicList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.PanelTopicList.Controls.Add(Me.PanelLabelTopicList)
        Me.PanelTopicList.Controls.Add(Me.DataGridViewTopicList)
        Me.PanelTopicList.Controls.Add(Me.LinkLabel1)
        Me.PanelTopicList.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PanelTopicList.Location = New System.Drawing.Point(0, 0)
        Me.PanelTopicList.Name = "PanelTopicList"
        Me.PanelTopicList.Size = New System.Drawing.Size(722, 150)
        Me.PanelTopicList.TabIndex = 18
        '
        'PanelLabelTopicList
        '
        Me.PanelLabelTopicList.AutoEllipsis = True
        Me.PanelLabelTopicList.AutoSize = True
        Me.PanelLabelTopicList.BackColor = System.Drawing.SystemColors.Control
        Me.PanelLabelTopicList.Dock = System.Windows.Forms.DockStyle.Top
        Me.PanelLabelTopicList.Location = New System.Drawing.Point(0, 0)
        Me.PanelLabelTopicList.Name = "PanelLabelTopicList"
        Me.PanelLabelTopicList.Size = New System.Drawing.Size(160, 13)
        Me.PanelLabelTopicList.TabIndex = 16
        Me.PanelLabelTopicList.Text = "Topics Matching Lookup Criteria"
        Me.ToolTipSQLRunbook.SetToolTip(Me.PanelLabelTopicList, "Select one Topic to view details below")
        '
        'DataGridViewTopicList
        '
        Me.DataGridViewTopicList.AllowUserToDeleteRows = False
        Me.DataGridViewTopicList.AllowUserToOrderColumns = True
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control
        Me.DataGridViewTopicList.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle1
        Me.DataGridViewTopicList.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.DataGridViewTopicList.AutoGenerateColumns = False
        Me.DataGridViewTopicList.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DataGridViewTopicList.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle2
        Me.DataGridViewTopicList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridViewTopicList.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.DataGridViewButtonColumnTopicRating, Me.DataGridViewTextBoxColumnTopicId, Me.DataGridViewButtonColumnTopicName, Me.DataGridViewButtonColumnTopicNotes, Me.DataGridViewTextBoxColumnTopicNbrDocs, Me.DataGridViewTextBoxColumnTopicRatingCount, Me.DataGridViewTextBoxColumnTopicRatingTally, Me.DataGridViewComboBoxColumnTopicOwner, Me.DataGridViewTextBoxColumnTopicRecCreatedDt, Me.DataGridViewTextBoxColumnTopicRecCreatedUser, Me.DataGridViewTextBoxColumnTopicLastUpdatedDt, Me.DataGridViewTextBoxColumnTopicLastUpdatedUser})
        Me.DataGridViewTopicList.DataSource = Me.BindingSourceTopic
        DataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.DataGridViewTopicList.DefaultCellStyle = DataGridViewCellStyle6
        Me.DataGridViewTopicList.Location = New System.Drawing.Point(4, 16)
        Me.DataGridViewTopicList.MultiSelect = False
        Me.DataGridViewTopicList.Name = "DataGridViewTopicList"
        DataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle7.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DataGridViewTopicList.RowHeadersDefaultCellStyle = DataGridViewCellStyle7
        Me.DataGridViewTopicList.RowHeadersWidth = 30
        Me.DataGridViewTopicList.Size = New System.Drawing.Size(712, 128)
        Me.DataGridViewTopicList.TabIndex = 17
        '
        'DataGridViewButtonColumnTopicRating
        '
        Me.DataGridViewButtonColumnTopicRating.FillWeight = 50.0!
        Me.DataGridViewButtonColumnTopicRating.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.DataGridViewButtonColumnTopicRating.HeaderText = "Rating"
        Me.DataGridViewButtonColumnTopicRating.MinimumWidth = 45
        Me.DataGridViewButtonColumnTopicRating.Name = "DataGridViewButtonColumnTopicRating"
        Me.DataGridViewButtonColumnTopicRating.ToolTipText = "Select button to rate"
        Me.DataGridViewButtonColumnTopicRating.Width = 45
        '
        'DataGridViewTextBoxColumnTopicId
        '
        Me.DataGridViewTextBoxColumnTopicId.DataPropertyName = "Id"
        Me.DataGridViewTextBoxColumnTopicId.HeaderText = "Id"
        Me.DataGridViewTextBoxColumnTopicId.Name = "DataGridViewTextBoxColumnTopicId"
        Me.DataGridViewTextBoxColumnTopicId.ReadOnly = True
        Me.DataGridViewTextBoxColumnTopicId.Visible = False
        '
        'DataGridViewButtonColumnTopicName
        '
        Me.DataGridViewButtonColumnTopicName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
        Me.DataGridViewButtonColumnTopicName.DataPropertyName = "Name"
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        Me.DataGridViewButtonColumnTopicName.DefaultCellStyle = DataGridViewCellStyle3
        Me.DataGridViewButtonColumnTopicName.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.DataGridViewButtonColumnTopicName.HeaderText = "Topic Name  (select to edit)"
        Me.DataGridViewButtonColumnTopicName.MinimumWidth = 156
        Me.DataGridViewButtonColumnTopicName.Name = "DataGridViewButtonColumnTopicName"
        Me.DataGridViewButtonColumnTopicName.ReadOnly = True
        Me.DataGridViewButtonColumnTopicName.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DataGridViewButtonColumnTopicName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        Me.DataGridViewButtonColumnTopicName.ToolTipText = "Selecting existing Name to Edit"
        '
        'DataGridViewButtonColumnTopicNotes
        '
        Me.DataGridViewButtonColumnTopicNotes.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.DataGridViewButtonColumnTopicNotes.DataPropertyName = "Notes"
        Me.DataGridViewButtonColumnTopicNotes.DefaultCellStyle = DataGridViewCellStyle4
        Me.DataGridViewButtonColumnTopicNotes.FillWeight = 50.0!
        Me.DataGridViewButtonColumnTopicNotes.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.DataGridViewButtonColumnTopicNotes.HeaderText = "Notes"
        Me.DataGridViewButtonColumnTopicNotes.MinimumWidth = 45
        Me.DataGridViewButtonColumnTopicNotes.Name = "DataGridViewButtonColumnTopicNotes"
        Me.DataGridViewButtonColumnTopicNotes.ReadOnly = True
        Me.DataGridViewButtonColumnTopicNotes.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.DataGridViewButtonColumnTopicNotes.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        Me.DataGridViewButtonColumnTopicNotes.Text = Global.SQLClue.My.Resources.Resources.HelpOverview
        Me.DataGridViewButtonColumnTopicNotes.ToolTipText = "Select to View/Edit Notes , mouse over for preview..."
        Me.DataGridViewButtonColumnTopicNotes.Width = 45
        '
        'DataGridViewTextBoxColumnTopicNbrDocs
        '
        Me.DataGridViewTextBoxColumnTopicNbrDocs.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
        Me.DataGridViewTextBoxColumnTopicNbrDocs.DataPropertyName = "DocumentCount"
        DataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        Me.DataGridViewTextBoxColumnTopicNbrDocs.DefaultCellStyle = DataGridViewCellStyle5
        Me.DataGridViewTextBoxColumnTopicNbrDocs.FillWeight = 50.0!
        Me.DataGridViewTextBoxColumnTopicNbrDocs.HeaderText = "Nbr Docs"
        Me.DataGridViewTextBoxColumnTopicNbrDocs.MinimumWidth = 50
        Me.DataGridViewTextBoxColumnTopicNbrDocs.Name = "DataGridViewTextBoxColumnTopicNbrDocs"
        Me.DataGridViewTextBoxColumnTopicNbrDocs.ReadOnly = True
        Me.DataGridViewTextBoxColumnTopicNbrDocs.ToolTipText = "Select Name to load documents in Topic Editor"
        Me.DataGridViewTextBoxColumnTopicNbrDocs.Width = 71
        '
        'DataGridViewTextBoxColumnTopicRatingCount
        '
        Me.DataGridViewTextBoxColumnTopicRatingCount.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
        Me.DataGridViewTextBoxColumnTopicRatingCount.DataPropertyName = "RatingCount"
        Me.DataGridViewTextBoxColumnTopicRatingCount.HeaderText = "RatingCount"
        Me.DataGridViewTextBoxColumnTopicRatingCount.Name = "DataGridViewTextBoxColumnTopicRatingCount"
        Me.DataGridViewTextBoxColumnTopicRatingCount.ReadOnly = True
        Me.DataGridViewTextBoxColumnTopicRatingCount.Visible = False
        '
        'DataGridViewTextBoxColumnTopicRatingTally
        '
        Me.DataGridViewTextBoxColumnTopicRatingTally.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
        Me.DataGridViewTextBoxColumnTopicRatingTally.DataPropertyName = "RatingTally"
        Me.DataGridViewTextBoxColumnTopicRatingTally.HeaderText = "RatingTally"
        Me.DataGridViewTextBoxColumnTopicRatingTally.MinimumWidth = 50
        Me.DataGridViewTextBoxColumnTopicRatingTally.Name = "DataGridViewTextBoxColumnTopicRatingTally"
        Me.DataGridViewTextBoxColumnTopicRatingTally.ReadOnly = True
        Me.DataGridViewTextBoxColumnTopicRatingTally.Visible = False
        '
        'DataGridViewComboBoxColumnTopicOwner
        '
        Me.DataGridViewComboBoxColumnTopicOwner.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCellsExceptHeader
        Me.DataGridViewComboBoxColumnTopicOwner.DataPropertyName = "Owner"
        Me.DataGridViewComboBoxColumnTopicOwner.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.ComboBox
        Me.DataGridViewComboBoxColumnTopicOwner.HeaderText = "Owner"
        Me.DataGridViewComboBoxColumnTopicOwner.MinimumWidth = 100
        Me.DataGridViewComboBoxColumnTopicOwner.Name = "DataGridViewComboBoxColumnTopicOwner"
        Me.DataGridViewComboBoxColumnTopicOwner.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DataGridViewComboBoxColumnTopicOwner.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        Me.DataGridViewComboBoxColumnTopicOwner.ToolTipText = "Topic ownership can be assigned to any user in the dropdown list"
        '
        'DataGridViewTextBoxColumnTopicRecCreatedDt
        '
        Me.DataGridViewTextBoxColumnTopicRecCreatedDt.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
        Me.DataGridViewTextBoxColumnTopicRecCreatedDt.DataPropertyName = "RecCreatedDt"
        Me.DataGridViewTextBoxColumnTopicRecCreatedDt.HeaderText = "Date Created"
        Me.DataGridViewTextBoxColumnTopicRecCreatedDt.MinimumWidth = 65
        Me.DataGridViewTextBoxColumnTopicRecCreatedDt.Name = "DataGridViewTextBoxColumnTopicRecCreatedDt"
        Me.DataGridViewTextBoxColumnTopicRecCreatedDt.ReadOnly = True
        Me.DataGridViewTextBoxColumnTopicRecCreatedDt.Width = 88
        '
        'DataGridViewTextBoxColumnTopicRecCreatedUser
        '
        Me.DataGridViewTextBoxColumnTopicRecCreatedUser.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
        Me.DataGridViewTextBoxColumnTopicRecCreatedUser.DataPropertyName = "RecCreatedUser"
        Me.DataGridViewTextBoxColumnTopicRecCreatedUser.HeaderText = "Originator"
        Me.DataGridViewTextBoxColumnTopicRecCreatedUser.MinimumWidth = 100
        Me.DataGridViewTextBoxColumnTopicRecCreatedUser.Name = "DataGridViewTextBoxColumnTopicRecCreatedUser"
        Me.DataGridViewTextBoxColumnTopicRecCreatedUser.ReadOnly = True
        '
        'DataGridViewTextBoxColumnTopicLastUpdatedDt
        '
        Me.DataGridViewTextBoxColumnTopicLastUpdatedDt.DataPropertyName = "LastUpdatedDt"
        Me.DataGridViewTextBoxColumnTopicLastUpdatedDt.HeaderText = "LastUpdatedDt"
        Me.DataGridViewTextBoxColumnTopicLastUpdatedDt.Name = "DataGridViewTextBoxColumnTopicLastUpdatedDt"
        Me.DataGridViewTextBoxColumnTopicLastUpdatedDt.Visible = False
        '
        'DataGridViewTextBoxColumnTopicLastUpdatedUser
        '
        Me.DataGridViewTextBoxColumnTopicLastUpdatedUser.DataPropertyName = "LastUpdatedUser"
        Me.DataGridViewTextBoxColumnTopicLastUpdatedUser.HeaderText = "LastUpdatedUser"
        Me.DataGridViewTextBoxColumnTopicLastUpdatedUser.Name = "DataGridViewTextBoxColumnTopicLastUpdatedUser"
        Me.DataGridViewTextBoxColumnTopicLastUpdatedUser.Visible = False
        '
        'BindingSourceTopic
        '
        Me.BindingSourceTopic.AllowNew = True
        Me.BindingSourceTopic.DataMember = "tTopic"
        Me.BindingSourceTopic.DataSource = Me.DataSetSQLRunbook
        Me.BindingSourceTopic.Sort = "Name"
        '
        'DataSetSQLRunbook
        '
        Me.DataSetSQLRunbook.DataSetName = "DataSetSQLRunbook"
        Me.DataSetSQLRunbook.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        '
        'LinkLabel1
        '
        Me.LinkLabel1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.LinkLabel1.AutoSize = True
        Me.LinkLabel1.ImageAlign = System.Drawing.ContentAlignment.TopCenter
        Me.LinkLabel1.Location = New System.Drawing.Point(619, 0)
        Me.LinkLabel1.Name = "LinkLabel1"
        Me.LinkLabel1.Size = New System.Drawing.Size(99, 13)
        Me.LinkLabel1.TabIndex = 38
        Me.LinkLabel1.TabStop = True
        Me.LinkLabel1.Text = "Edit Lookup Criteria"
        Me.ToolTipSQLRunbook.SetToolTip(Me.LinkLabel1, "Change Lookup Criteria")
        '
        'Panel2
        '
        Me.Panel2.BackColor = System.Drawing.SystemColors.Window
        Me.Panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel2.Controls.Add(Me.SplitContainer1)
        Me.Panel2.Controls.Add(Me.SplitContainerTopic)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel2.Location = New System.Drawing.Point(0, 0)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(722, 249)
        Me.Panel2.TabIndex = 51
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SplitContainer1.IsSplitterFixed = True
        Me.SplitContainer1.Location = New System.Drawing.Point(523, 221)
        Me.SplitContainer1.Name = "SplitContainer1"
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.ButtonSave)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.ButtonCancel)
        Me.SplitContainer1.Size = New System.Drawing.Size(194, 22)
        Me.SplitContainer1.SplitterDistance = 95
        Me.SplitContainer1.TabIndex = 61
        '
        'ButtonSave
        '
        Me.ButtonSave.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ButtonSave.Location = New System.Drawing.Point(0, 0)
        Me.ButtonSave.MinimumSize = New System.Drawing.Size(50, 0)
        Me.ButtonSave.Name = "ButtonSave"
        Me.ButtonSave.Size = New System.Drawing.Size(95, 22)
        Me.ButtonSave.TabIndex = 43
        Me.ButtonSave.Text = "Save"
        Me.ButtonSave.UseVisualStyleBackColor = True
        '
        'ButtonCancel
        '
        Me.ButtonCancel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ButtonCancel.Location = New System.Drawing.Point(0, 0)
        Me.ButtonCancel.Name = "ButtonCancel"
        Me.ButtonCancel.Size = New System.Drawing.Size(95, 22)
        Me.ButtonCancel.TabIndex = 0
        Me.ButtonCancel.Text = "Cancel"
        Me.ButtonCancel.UseVisualStyleBackColor = True
        '
        'SplitContainerTopic
        '
        Me.SplitContainerTopic.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SplitContainerTopic.BackColor = System.Drawing.SystemColors.GradientInactiveCaption
        Me.SplitContainerTopic.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.SplitContainerTopic.Location = New System.Drawing.Point(4, 4)
        Me.SplitContainerTopic.Name = "SplitContainerTopic"
        '
        'SplitContainerTopic.Panel1
        '
        Me.SplitContainerTopic.Panel1.Controls.Add(Me.PanelTopicDetail)
        '
        'SplitContainerTopic.Panel2
        '
        Me.SplitContainerTopic.Panel2.Controls.Add(Me.Panel1)
        Me.SplitContainerTopic.Size = New System.Drawing.Size(712, 213)
        Me.SplitContainerTopic.SplitterDistance = 491
        Me.SplitContainerTopic.TabIndex = 50
        '
        'PanelTopicDetail
        '
        Me.PanelTopicDetail.BackColor = System.Drawing.SystemColors.Control
        Me.PanelTopicDetail.Controls.Add(Me.PanelDocumentList)
        Me.PanelTopicDetail.Controls.Add(Me.Label1)
        Me.PanelTopicDetail.Controls.Add(Me.ButtonNotes)
        Me.PanelTopicDetail.Controls.Add(Me.ComboBoxTopicName)
        Me.PanelTopicDetail.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PanelTopicDetail.Location = New System.Drawing.Point(0, 0)
        Me.PanelTopicDetail.Name = "PanelTopicDetail"
        Me.PanelTopicDetail.Size = New System.Drawing.Size(489, 211)
        Me.PanelTopicDetail.TabIndex = 49
        '
        'PanelDocumentList
        '
        Me.PanelDocumentList.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PanelDocumentList.BackColor = System.Drawing.SystemColors.Control
        Me.PanelDocumentList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.PanelDocumentList.Controls.Add(Me.Label8)
        Me.PanelDocumentList.Controls.Add(Me.DataGridViewDocumentList)
        Me.PanelDocumentList.Location = New System.Drawing.Point(4, 29)
        Me.PanelDocumentList.Name = "PanelDocumentList"
        Me.PanelDocumentList.Size = New System.Drawing.Size(481, 178)
        Me.PanelDocumentList.TabIndex = 62
        '
        'Label8
        '
        Me.Label8.Location = New System.Drawing.Point(3, 0)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(490, 13)
        Me.Label8.TabIndex = 8
        Me.Label8.Text = "Documents on this Topic"
        Me.ToolTipSQLRunbook.SetToolTip(Me.Label8, "Documents can belong to more than one Topic")
        '
        'DataGridViewDocumentList
        '
        Me.DataGridViewDocumentList.AllowUserToDeleteRows = False
        Me.DataGridViewDocumentList.AllowUserToOrderColumns = True
        DataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Control
        Me.DataGridViewDocumentList.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle8
        Me.DataGridViewDocumentList.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.DataGridViewDocumentList.AutoGenerateColumns = False
        Me.DataGridViewDocumentList.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        DataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle9.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle9.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle9.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle9.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle9.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DataGridViewDocumentList.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle9
        Me.DataGridViewDocumentList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridViewDocumentList.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.DataGridViewButtonColumnDocumentRating, Me.DataGridViewTextBoxColumnDocumentId, Me.DataGridViewButtonColumnDocumentFile, Me.DataGridViewButtonDocumentShortFile, Me.DocumentDataGridViewColumnDocument, Me.DataGridViewComboBoxColumnDocType, Me.DataGridViewComboBoxColumnDocumentOwner, Me.DataGridViewTextBoxColumnDocumentLastModifiedDt, Me.DataGridViewTextBoxColumnDocumentRatingTally, Me.DataGridViewTextBoxColumnDocumentRatingCount, Me.DataGridViewCheckBoxDocumentWatchFileForChange})
        Me.DataGridViewDocumentList.DataSource = Me.BindingSourceDocument
        DataGridViewCellStyle14.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle14.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle14.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle14.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle14.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle14.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle14.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.DataGridViewDocumentList.DefaultCellStyle = DataGridViewCellStyle14
        Me.DataGridViewDocumentList.Location = New System.Drawing.Point(4, 16)
        Me.DataGridViewDocumentList.MultiSelect = False
        Me.DataGridViewDocumentList.Name = "DataGridViewDocumentList"
        DataGridViewCellStyle15.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle15.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle15.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle15.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle15.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle15.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle15.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DataGridViewDocumentList.RowHeadersDefaultCellStyle = DataGridViewCellStyle15
        Me.DataGridViewDocumentList.RowHeadersWidth = 30
        DataGridViewCellStyle16.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle16.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle16.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle16.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle16.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        Me.DataGridViewDocumentList.RowsDefaultCellStyle = DataGridViewCellStyle16
        Me.DataGridViewDocumentList.Size = New System.Drawing.Size(471, 156)
        Me.DataGridViewDocumentList.TabIndex = 7
        '
        'DataGridViewButtonColumnDocumentRating
        '
        Me.DataGridViewButtonColumnDocumentRating.FillWeight = 50.0!
        Me.DataGridViewButtonColumnDocumentRating.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.DataGridViewButtonColumnDocumentRating.HeaderText = "Rating"
        Me.DataGridViewButtonColumnDocumentRating.MinimumWidth = 45
        Me.DataGridViewButtonColumnDocumentRating.Name = "DataGridViewButtonColumnDocumentRating"
        Me.DataGridViewButtonColumnDocumentRating.ReadOnly = True
        Me.DataGridViewButtonColumnDocumentRating.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        Me.DataGridViewButtonColumnDocumentRating.Width = 45
        '
        'DataGridViewTextBoxColumnDocumentId
        '
        Me.DataGridViewTextBoxColumnDocumentId.DataPropertyName = "Id"
        Me.DataGridViewTextBoxColumnDocumentId.HeaderText = "Id"
        Me.DataGridViewTextBoxColumnDocumentId.Name = "DataGridViewTextBoxColumnDocumentId"
        Me.DataGridViewTextBoxColumnDocumentId.ReadOnly = True
        Me.DataGridViewTextBoxColumnDocumentId.Visible = False
        '
        'DataGridViewButtonColumnDocumentFile
        '
        Me.DataGridViewButtonColumnDocumentFile.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
        Me.DataGridViewButtonColumnDocumentFile.DataPropertyName = "File"
        DataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopRight
        Me.DataGridViewButtonColumnDocumentFile.DefaultCellStyle = DataGridViewCellStyle10
        Me.DataGridViewButtonColumnDocumentFile.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.DataGridViewButtonColumnDocumentFile.HeaderText = "File"
        Me.DataGridViewButtonColumnDocumentFile.MinimumWidth = 255
        Me.DataGridViewButtonColumnDocumentFile.Name = "DataGridViewButtonColumnDocumentFile"
        Me.DataGridViewButtonColumnDocumentFile.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.DataGridViewButtonColumnDocumentFile.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        Me.DataGridViewButtonColumnDocumentFile.ToolTipText = "The file bits stored in the database and the Full Text Catalog are updated when c" & _
            "olumn is selected. SQLClue Service regularly does this re-sync when the FileWatc" & _
            "her is enabled"
        Me.DataGridViewButtonColumnDocumentFile.Visible = False
        '
        'DataGridViewButtonDocumentShortFile
        '
        Me.DataGridViewButtonDocumentShortFile.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
        DataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft
        Me.DataGridViewButtonDocumentShortFile.DefaultCellStyle = DataGridViewCellStyle11
        Me.DataGridViewButtonDocumentShortFile.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.DataGridViewButtonDocumentShortFile.HeaderText = "File (select to re-sync)"
        Me.DataGridViewButtonDocumentShortFile.MinimumWidth = 255
        Me.DataGridViewButtonDocumentShortFile.Name = "DataGridViewButtonDocumentShortFile"
        Me.DataGridViewButtonDocumentShortFile.ToolTipText = "Same operation as FileWatcher: a copy of the file bits are stored in the database" & _
            " and Full Text Indexes are updated as the textual content changes. "
        '
        'DocumentDataGridViewColumnDocument
        '
        Me.DocumentDataGridViewColumnDocument.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
        Me.DocumentDataGridViewColumnDocument.DataPropertyName = "Document"
        Me.DocumentDataGridViewColumnDocument.FillWeight = 50.0!
        Me.DocumentDataGridViewColumnDocument.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.DocumentDataGridViewColumnDocument.HeaderText = "Document"
        Me.DocumentDataGridViewColumnDocument.MinimumWidth = 65
        Me.DocumentDataGridViewColumnDocument.Name = "DocumentDataGridViewColumnDocument"
        Me.DocumentDataGridViewColumnDocument.ReadOnly = True
        Me.DocumentDataGridViewColumnDocument.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DocumentDataGridViewColumnDocument.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        Me.DocumentDataGridViewColumnDocument.Text = "Open"
        Me.DocumentDataGridViewColumnDocument.ToolTipText = "Select button to open document in associated application"
        Me.DocumentDataGridViewColumnDocument.UseColumnTextForButtonValue = True
        Me.DocumentDataGridViewColumnDocument.Width = 81
        '
        'DataGridViewComboBoxColumnDocType
        '
        Me.DataGridViewComboBoxColumnDocType.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
        Me.DataGridViewComboBoxColumnDocType.DataPropertyName = "DocumentType"
        DataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        Me.DataGridViewComboBoxColumnDocType.DefaultCellStyle = DataGridViewCellStyle12
        Me.DataGridViewComboBoxColumnDocType.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.ComboBox
        Me.DataGridViewComboBoxColumnDocType.FillWeight = 70.0!
        Me.DataGridViewComboBoxColumnDocType.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.DataGridViewComboBoxColumnDocType.HeaderText = "Document Type"
        Me.DataGridViewComboBoxColumnDocType.MinimumWidth = 65
        Me.DataGridViewComboBoxColumnDocType.Name = "DataGridViewComboBoxColumnDocType"
        Me.DataGridViewComboBoxColumnDocType.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DataGridViewComboBoxColumnDocType.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        Me.DataGridViewComboBoxColumnDocType.Width = 99
        '
        'DataGridViewComboBoxColumnDocumentOwner
        '
        Me.DataGridViewComboBoxColumnDocumentOwner.DataPropertyName = "Owner"
        Me.DataGridViewComboBoxColumnDocumentOwner.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.ComboBox
        Me.DataGridViewComboBoxColumnDocumentOwner.FillWeight = 50.0!
        Me.DataGridViewComboBoxColumnDocumentOwner.HeaderText = "Owner"
        Me.DataGridViewComboBoxColumnDocumentOwner.MinimumWidth = 100
        Me.DataGridViewComboBoxColumnDocumentOwner.Name = "DataGridViewComboBoxColumnDocumentOwner"
        Me.DataGridViewComboBoxColumnDocumentOwner.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DataGridViewComboBoxColumnDocumentOwner.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        '
        'DataGridViewTextBoxColumnDocumentLastModifiedDt
        '
        Me.DataGridViewTextBoxColumnDocumentLastModifiedDt.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCellsExceptHeader
        Me.DataGridViewTextBoxColumnDocumentLastModifiedDt.DataPropertyName = "LastModifiedDt"
        DataGridViewCellStyle13.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        Me.DataGridViewTextBoxColumnDocumentLastModifiedDt.DefaultCellStyle = DataGridViewCellStyle13
        Me.DataGridViewTextBoxColumnDocumentLastModifiedDt.FillWeight = 75.0!
        Me.DataGridViewTextBoxColumnDocumentLastModifiedDt.HeaderText = "Last Modified Date"
        Me.DataGridViewTextBoxColumnDocumentLastModifiedDt.MinimumWidth = 100
        Me.DataGridViewTextBoxColumnDocumentLastModifiedDt.Name = "DataGridViewTextBoxColumnDocumentLastModifiedDt"
        Me.DataGridViewTextBoxColumnDocumentLastModifiedDt.ReadOnly = True
        '
        'DataGridViewTextBoxColumnDocumentRatingTally
        '
        Me.DataGridViewTextBoxColumnDocumentRatingTally.DataPropertyName = "RatingTally"
        Me.DataGridViewTextBoxColumnDocumentRatingTally.HeaderText = "RatingTally"
        Me.DataGridViewTextBoxColumnDocumentRatingTally.Name = "DataGridViewTextBoxColumnDocumentRatingTally"
        Me.DataGridViewTextBoxColumnDocumentRatingTally.ReadOnly = True
        Me.DataGridViewTextBoxColumnDocumentRatingTally.Visible = False
        '
        'DataGridViewTextBoxColumnDocumentRatingCount
        '
        Me.DataGridViewTextBoxColumnDocumentRatingCount.DataPropertyName = "RatingCount"
        Me.DataGridViewTextBoxColumnDocumentRatingCount.HeaderText = "RatingCount"
        Me.DataGridViewTextBoxColumnDocumentRatingCount.Name = "DataGridViewTextBoxColumnDocumentRatingCount"
        Me.DataGridViewTextBoxColumnDocumentRatingCount.ReadOnly = True
        Me.DataGridViewTextBoxColumnDocumentRatingCount.Visible = False
        '
        'DataGridViewCheckBoxDocumentWatchFileForChange
        '
        Me.DataGridViewCheckBoxDocumentWatchFileForChange.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
        Me.DataGridViewCheckBoxDocumentWatchFileForChange.DataPropertyName = "WatchFileForChange"
        Me.DataGridViewCheckBoxDocumentWatchFileForChange.HeaderText = "Watch For Changes"
        Me.DataGridViewCheckBoxDocumentWatchFileForChange.MinimumWidth = 70
        Me.DataGridViewCheckBoxDocumentWatchFileForChange.Name = "DataGridViewCheckBoxDocumentWatchFileForChange"
        Me.DataGridViewCheckBoxDocumentWatchFileForChange.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        Me.DataGridViewCheckBoxDocumentWatchFileForChange.Width = 116
        '
        'BindingSourceDocument
        '
        Me.BindingSourceDocument.DataMember = "tDocument"
        Me.BindingSourceDocument.DataSource = Me.DataSetSQLRunbook
        Me.BindingSourceDocument.Sort = "File"
        '
        'Label1
        '
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(2, 7)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(85, 17)
        Me.Label1.TabIndex = 66
        Me.Label1.Text = "Selected Topic"
        Me.ToolTipSQLRunbook.SetToolTip(Me.Label1, "Name is not unique. A standard for the naming convention will emerge and should b" & _
                "e followed - as well as added to the Runbook's knowledge")
        '
        'ButtonNotes
        '
        Me.ButtonNotes.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ButtonNotes.BackColor = System.Drawing.SystemColors.Control
        Me.ButtonNotes.Location = New System.Drawing.Point(427, 4)
        Me.ButtonNotes.Name = "ButtonNotes"
        Me.ButtonNotes.Size = New System.Drawing.Size(57, 21)
        Me.ButtonNotes.TabIndex = 65
        Me.ButtonNotes.Text = "Notes"
        Me.ButtonNotes.UseVisualStyleBackColor = False
        '
        'ComboBoxTopicName
        '
        Me.ComboBoxTopicName.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ComboBoxTopicName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBoxTopicName.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ComboBoxTopicName.Location = New System.Drawing.Point(93, 4)
        Me.ComboBoxTopicName.MaxDropDownItems = 16
        Me.ComboBoxTopicName.Name = "ComboBoxTopicName"
        Me.ComboBoxTopicName.Size = New System.Drawing.Size(327, 21)
        Me.ComboBoxTopicName.TabIndex = 59
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.SystemColors.Control
        Me.Panel1.Controls.Add(Me.PanelCategoryList)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(215, 211)
        Me.Panel1.TabIndex = 0
        '
        'PanelCategoryList
        '
        Me.PanelCategoryList.BackColor = System.Drawing.SystemColors.Control
        Me.PanelCategoryList.Controls.Add(Me.FlowLayoutPanel1)
        Me.PanelCategoryList.Controls.Add(Me.ListBoxCategory)
        Me.PanelCategoryList.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PanelCategoryList.Location = New System.Drawing.Point(0, 0)
        Me.PanelCategoryList.Margin = New System.Windows.Forms.Padding(0)
        Me.PanelCategoryList.Name = "PanelCategoryList"
        Me.PanelCategoryList.Size = New System.Drawing.Size(215, 211)
        Me.PanelCategoryList.TabIndex = 60
        '
        'FlowLayoutPanel1
        '
        Me.FlowLayoutPanel1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.FlowLayoutPanel1.Controls.Add(Me.Label2)
        Me.FlowLayoutPanel1.Controls.Add(Me.LabelCategoryHelper)
        Me.FlowLayoutPanel1.Location = New System.Drawing.Point(4, 0)
        Me.FlowLayoutPanel1.Name = "FlowLayoutPanel1"
        Me.FlowLayoutPanel1.Size = New System.Drawing.Size(207, 14)
        Me.FlowLayoutPanel1.TabIndex = 61
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(3, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(60, 13)
        Me.Label2.TabIndex = 61
        Me.Label2.Text = "Catagories "
        '
        'LabelCategoryHelper
        '
        Me.LabelCategoryHelper.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.LabelCategoryHelper.AutoEllipsis = True
        Me.LabelCategoryHelper.AutoSize = True
        Me.LabelCategoryHelper.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelCategoryHelper.Location = New System.Drawing.Point(69, 0)
        Me.LabelCategoryHelper.Name = "LabelCategoryHelper"
        Me.LabelCategoryHelper.Size = New System.Drawing.Size(70, 9)
        Me.LabelCategoryHelper.TabIndex = 60
        Me.LabelCategoryHelper.Text = "Select all that apply"
        Me.LabelCategoryHelper.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.ToolTipSQLRunbook.SetToolTip(Me.LabelCategoryHelper, "     mouse click " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "              or " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "  press SPACEBAR " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "to select or deselect " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & _
                "    any Category" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10))
        '
        'ListBoxCategory
        '
        Me.ListBoxCategory.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ListBoxCategory.ColumnWidth = 150
        Me.ListBoxCategory.FormattingEnabled = True
        Me.ListBoxCategory.IntegralHeight = False
        Me.ListBoxCategory.Location = New System.Drawing.Point(4, 16)
        Me.ListBoxCategory.Name = "ListBoxCategory"
        Me.ListBoxCategory.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple
        Me.ListBoxCategory.Size = New System.Drawing.Size(207, 191)
        Me.ListBoxCategory.TabIndex = 57
        Me.ToolTipSQLRunbook.SetToolTip(Me.ListBoxCategory, "     mouse click " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "              or " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "  press SPACEBAR " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "to select or deselect " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & _
                "    any Category")
        '
        'OpenFileDialogDocument
        '
        Me.OpenFileDialogDocument.ReadOnlyChecked = True
        Me.OpenFileDialogDocument.SupportMultiDottedExtensions = True
        '
        'TableAdapterDocument
        '
        Me.TableAdapterDocument.ClearBeforeFill = True
        '
        'TableAdapterTopic
        '
        Me.TableAdapterTopic.ClearBeforeFill = True
        '
        'RunbookForm
        '
        Me.AcceptButton = Me.ButtonSave
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.ClientSize = New System.Drawing.Size(730, 430)
        Me.Controls.Add(Me.ToolStripContainer1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.HelpButton = True
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MinimumSize = New System.Drawing.Size(350, 167)
        Me.Name = "RunbookForm"
        Me.Text = "assigned in code"
        Me.ToolStripContainer1.BottomToolStripPanel.ResumeLayout(False)
        Me.ToolStripContainer1.BottomToolStripPanel.PerformLayout()
        Me.ToolStripContainer1.ContentPanel.ResumeLayout(False)
        Me.ToolStripContainer1.ResumeLayout(False)
        Me.ToolStripContainer1.PerformLayout()
        Me.SplitContainerResults.Panel1.ResumeLayout(False)
        Me.SplitContainerResults.Panel2.ResumeLayout(False)
        Me.SplitContainerResults.ResumeLayout(False)
        Me.PanelTopicList.ResumeLayout(False)
        Me.PanelTopicList.PerformLayout()
        CType(Me.DataGridViewTopicList, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BindingSourceTopic, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DataSetSQLRunbook, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel2.ResumeLayout(False)
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        Me.SplitContainer1.ResumeLayout(False)
        Me.SplitContainerTopic.Panel1.ResumeLayout(False)
        Me.SplitContainerTopic.Panel2.ResumeLayout(False)
        Me.SplitContainerTopic.ResumeLayout(False)
        Me.PanelTopicDetail.ResumeLayout(False)
        Me.PanelDocumentList.ResumeLayout(False)
        CType(Me.DataGridViewDocumentList, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BindingSourceDocument, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel1.ResumeLayout(False)
        Me.PanelCategoryList.ResumeLayout(False)
        Me.FlowLayoutPanel1.ResumeLayout(False)
        Me.FlowLayoutPanel1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents StatusStripSQLRunbook As System.Windows.Forms.StatusStrip
    Friend WithEvents ToolStripStatusLabelRunbook As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents ToolStripContainer1 As System.Windows.Forms.ToolStripContainer
    Friend WithEvents ToolTipSQLRunbook As System.Windows.Forms.ToolTip
    Friend WithEvents SplitContainerResults As System.Windows.Forms.SplitContainer
    Friend WithEvents OpenFileDialogDocument As System.Windows.Forms.OpenFileDialog
    Friend WithEvents PanelLabelTopicList As System.Windows.Forms.Label
    Friend WithEvents DataGridViewTopicList As System.Windows.Forms.DataGridView
    Friend WithEvents ButtonSave As System.Windows.Forms.Button
    Friend WithEvents BindingSourceTopic As System.Windows.Forms.BindingSource
    Friend WithEvents ListBoxCategory As System.Windows.Forms.ListBox
    Friend WithEvents PanelCategoryList As System.Windows.Forms.Panel
    Friend WithEvents PanelDocumentList As System.Windows.Forms.Panel
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents DataGridViewDocumentList As System.Windows.Forms.DataGridView
    Friend WithEvents BindingSourceDocument As System.Windows.Forms.BindingSource
    Friend WithEvents TSQLRunbookTopicTableAdapter As SQLClue.DataSetSQLRunbookTableAdapters.tTopicTableAdapter
    Friend WithEvents TSQLRunbookCategoryTableAdapter As SQLClue.DataSetSQLRunbookTableAdapters.tCategoryTableAdapter
    Friend WithEvents TSQLRunbookUserTableAdapter As SQLClue.DataSetSQLRunbookTableAdapters.tUserTableAdapter
    Friend WithEvents TSQLRunbookDocumentTableAdapter As SQLClue.DataSetSQLRunbookTableAdapters.tDocumentTableAdapter
    Friend WithEvents TSQLRunbookCategoryTopicTableAdapter As SQLClue.DataSetSQLRunbookTableAdapters.tCategoryTopicTableAdapter
    Friend WithEvents dsSQLRunbook1 As SQLClue.DataSetSQLRunbook
    Friend WithEvents SplitContainerTopic As System.Windows.Forms.SplitContainer
    Friend WithEvents PanelTopicDetail As System.Windows.Forms.Panel
    Friend WithEvents LabelCategoryHelper As System.Windows.Forms.Label
    Friend WithEvents PanelTopicList As System.Windows.Forms.Panel
    Friend WithEvents HasDocumentDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataSetSQLRunbook As SQLClue.DataSetSQLRunbook
    Friend WithEvents ComboBoxTopicName As System.Windows.Forms.ComboBox
    Friend WithEvents TableAdapterDocument As SQLClue.DataSetSQLRunbookTableAdapters.tDocumentTableAdapter
    Friend WithEvents TableAdapterTopic As SQLClue.DataSetSQLRunbookTableAdapters.tTopicTableAdapter
    Friend WithEvents FlowLayoutPanel1 As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents LinkLabel1 As System.Windows.Forms.LinkLabel
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
    Friend WithEvents ButtonCancel As System.Windows.Forms.Button
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents ButtonNotes As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents DataGridViewButtonColumnDocumentRating As System.Windows.Forms.DataGridViewButtonColumn
    Friend WithEvents DataGridViewTextBoxColumnDocumentId As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewButtonColumnDocumentFile As System.Windows.Forms.DataGridViewButtonColumn
    Friend WithEvents DataGridViewButtonDocumentShortFile As System.Windows.Forms.DataGridViewButtonColumn
    Friend WithEvents DocumentDataGridViewColumnDocument As System.Windows.Forms.DataGridViewButtonColumn
    Friend WithEvents DataGridViewComboBoxColumnDocType As System.Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents DataGridViewComboBoxColumnDocumentOwner As System.Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents DataGridViewTextBoxColumnDocumentLastModifiedDt As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumnDocumentRatingTally As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumnDocumentRatingCount As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewCheckBoxDocumentWatchFileForChange As System.Windows.Forms.DataGridViewCheckBoxColumn
    Friend WithEvents DataGridViewButtonColumnTopicRating As System.Windows.Forms.DataGridViewButtonColumn
    Friend WithEvents DataGridViewTextBoxColumnTopicId As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewButtonColumnTopicName As System.Windows.Forms.DataGridViewButtonColumn
    Friend WithEvents DataGridViewButtonColumnTopicNotes As System.Windows.Forms.DataGridViewButtonColumn
    Friend WithEvents DataGridViewTextBoxColumnTopicNbrDocs As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumnTopicRatingCount As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumnTopicRatingTally As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewComboBoxColumnTopicOwner As System.Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents DataGridViewTextBoxColumnTopicRecCreatedDt As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumnTopicRecCreatedUser As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumnTopicLastUpdatedDt As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumnTopicLastUpdatedUser As System.Windows.Forms.DataGridViewTextBoxColumn
End Class
