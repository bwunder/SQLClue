<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ReportViewerForm
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ReportViewerForm))
        Me.ToolStripStatusLabel1 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.LocalToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.RVFile = New System.Windows.Forms.ToolStripComboBox()
        Me.ToolStripMenuItem4 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ServerToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.StatusStripReportViewer = New System.Windows.Forms.StatusStrip()
        Me.ToolStripStatusLabel2 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ToolStripContainerRV = New System.Windows.Forms.ToolStripContainer()
        Me.SplitContainerReportViewer = New System.Windows.Forms.SplitContainer()
        Me.SplitContainerParms = New System.Windows.Forms.SplitContainer()
        Me.FlowLayoutPanelParms = New System.Windows.Forms.FlowLayoutPanel()
        Me.PanelSQLInstance = New System.Windows.Forms.Panel()
        Me.LabelSQLInstance = New System.Windows.Forms.Label()
        Me.ComboBoxSQLInstance = New System.Windows.Forms.ComboBox()
        Me.PanelLogins = New System.Windows.Forms.Panel()
        Me.ComboBoxLoginNames = New System.Windows.Forms.ComboBox()
        Me.LabelLogin = New System.Windows.Forms.Label()
        Me.PanelDays = New System.Windows.Forms.Panel()
        Me.LabelDays = New System.Windows.Forms.Label()
        Me.NumericUpDownDays = New System.Windows.Forms.NumericUpDown()
        Me.PanelDatabase = New System.Windows.Forms.Panel()
        Me.ComboBoxDatabase = New System.Windows.Forms.ComboBox()
        Me.LabelDatabase = New System.Windows.Forms.Label()
        Me.PanelType = New System.Windows.Forms.Panel()
        Me.ComboBoxType = New System.Windows.Forms.ComboBox()
        Me.LabelType = New System.Windows.Forms.Label()
        Me.PanelSubType = New System.Windows.Forms.Panel()
        Me.ComboBoxSubType = New System.Windows.Forms.ComboBox()
        Me.LabelSubType = New System.Windows.Forms.Label()
        Me.PanelCollection = New System.Windows.Forms.Panel()
        Me.ComboBoxCollection = New System.Windows.Forms.ComboBox()
        Me.LabelCollection = New System.Windows.Forms.Label()
        Me.PanelItem = New System.Windows.Forms.Panel()
        Me.ComboBoxItem = New System.Windows.Forms.ComboBox()
        Me.LabelItem = New System.Windows.Forms.Label()
        Me.PanelVersion = New System.Windows.Forms.Panel()
        Me.ComboBoxVersion = New System.Windows.Forms.ComboBox()
        Me.LabelVersion = New System.Windows.Forms.Label()
        Me.PanelSearch = New System.Windows.Forms.Panel()
        Me.TextBoxSearchString = New System.Windows.Forms.TextBox()
        Me.LabelSearchString = New System.Windows.Forms.Label()
        Me.PanelSearchLatest = New System.Windows.Forms.Panel()
        Me.ComboBoxSearchLatest = New System.Windows.Forms.ComboBox()
        Me.LabelSearchLatest = New System.Windows.Forms.Label()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.ButtonViewReport = New System.Windows.Forms.Button()
        Me.ReportViewerSQLClue = New Microsoft.Reporting.WinForms.ReportViewer()
        Me.PanelBeginDt = New System.Windows.Forms.Panel()
        Me.DateTimePicker1 = New System.Windows.Forms.DateTimePicker()
        Me.LabelBeginDt = New System.Windows.Forms.Label()
        Me.PanelEndDt = New System.Windows.Forms.Panel()
        Me.LabelEndDt = New System.Windows.Forms.Label()
        Me.DateTimePickerEndDt = New System.Windows.Forms.DateTimePicker()
        Me.PanelCategory = New System.Windows.Forms.Panel()
        Me.ComboBoxCategory = New System.Windows.Forms.ComboBox()
        Me.LabelCategory = New System.Windows.Forms.Label()
        Me.StatusStripReportViewer.SuspendLayout()
        Me.ToolStripContainerRV.BottomToolStripPanel.SuspendLayout()
        Me.ToolStripContainerRV.ContentPanel.SuspendLayout()
        Me.ToolStripContainerRV.SuspendLayout()
        CType(Me.SplitContainerReportViewer, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainerReportViewer.Panel1.SuspendLayout()
        Me.SplitContainerReportViewer.Panel2.SuspendLayout()
        Me.SplitContainerReportViewer.SuspendLayout()
        CType(Me.SplitContainerParms, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainerParms.Panel1.SuspendLayout()
        Me.SplitContainerParms.Panel2.SuspendLayout()
        Me.SplitContainerParms.SuspendLayout()
        Me.FlowLayoutPanelParms.SuspendLayout()
        Me.PanelSQLInstance.SuspendLayout()
        Me.PanelLogins.SuspendLayout()
        Me.PanelDays.SuspendLayout()
        CType(Me.NumericUpDownDays, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PanelDatabase.SuspendLayout()
        Me.PanelType.SuspendLayout()
        Me.PanelSubType.SuspendLayout()
        Me.PanelCollection.SuspendLayout()
        Me.PanelItem.SuspendLayout()
        Me.PanelVersion.SuspendLayout()
        Me.PanelSearch.SuspendLayout()
        Me.PanelSearchLatest.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.PanelBeginDt.SuspendLayout()
        Me.PanelEndDt.SuspendLayout()
        Me.PanelCategory.SuspendLayout()
        Me.SuspendLayout()
        '
        'ToolStripStatusLabel1
        '
        Me.ToolStripStatusLabel1.Name = "ToolStripStatusLabel1"
        Me.ToolStripStatusLabel1.Size = New System.Drawing.Size(39, 17)
        Me.ToolStripStatusLabel1.Text = "Ready"
        '
        'LocalToolStripMenuItem
        '
        Me.LocalToolStripMenuItem.AutoToolTip = True
        Me.LocalToolStripMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.LocalToolStripMenuItem.Name = "LocalToolStripMenuItem"
        Me.LocalToolStripMenuItem.Size = New System.Drawing.Size(163, 20)
        Me.LocalToolStripMenuItem.Text = "ReportViewer (rdlc) Reports"
        '
        'RVFile
        '
        Me.RVFile.BackColor = System.Drawing.SystemColors.Control
        Me.RVFile.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.RVFile.DropDownWidth = 300
        Me.RVFile.Items.AddRange(New Object() {Global.SQLClue.My.Resources.Resources.HelpOverview, "SQLClueDashboard.rdlc"})
        Me.RVFile.Name = "RVFile"
        Me.RVFile.Size = New System.Drawing.Size(300, 23)
        '
        'ToolStripMenuItem4
        '
        Me.ToolStripMenuItem4.Enabled = False
        Me.ToolStripMenuItem4.Image = CType(resources.GetObject("ToolStripMenuItem4.Image"), System.Drawing.Image)
        Me.ToolStripMenuItem4.Name = "ToolStripMenuItem4"
        Me.ToolStripMenuItem4.ShowShortcutKeys = False
        Me.ToolStripMenuItem4.Size = New System.Drawing.Size(28, 20)
        '
        'ServerToolStripMenuItem
        '
        Me.ServerToolStripMenuItem.Name = "ServerToolStripMenuItem"
        Me.ServerToolStripMenuItem.Size = New System.Drawing.Size(94, 20)
        Me.ServerToolStripMenuItem.Text = "Report Servers"
        '
        'StatusStripReportViewer
        '
        Me.StatusStripReportViewer.Dock = System.Windows.Forms.DockStyle.None
        Me.StatusStripReportViewer.GripStyle = System.Windows.Forms.ToolStripGripStyle.Visible
        Me.StatusStripReportViewer.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripStatusLabel2})
        Me.StatusStripReportViewer.Location = New System.Drawing.Point(0, 0)
        Me.StatusStripReportViewer.Name = "StatusStripReportViewer"
        Me.StatusStripReportViewer.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional
        Me.StatusStripReportViewer.Size = New System.Drawing.Size(730, 22)
        Me.StatusStripReportViewer.TabIndex = 3
        Me.StatusStripReportViewer.Text = "StatusStrip1"
        '
        'ToolStripStatusLabel2
        '
        Me.ToolStripStatusLabel2.Name = "ToolStripStatusLabel2"
        Me.ToolStripStatusLabel2.Size = New System.Drawing.Size(715, 17)
        Me.ToolStripStatusLabel2.Spring = True
        Me.ToolStripStatusLabel2.Text = "Select a Report"
        Me.ToolStripStatusLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ToolStripContainerRV
        '
        '
        'ToolStripContainerRV.BottomToolStripPanel
        '
        Me.ToolStripContainerRV.BottomToolStripPanel.Controls.Add(Me.StatusStripReportViewer)
        Me.ToolStripContainerRV.BottomToolStripPanel.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional
        '
        'ToolStripContainerRV.ContentPanel
        '
        Me.ToolStripContainerRV.ContentPanel.Controls.Add(Me.SplitContainerReportViewer)
        Me.ToolStripContainerRV.ContentPanel.Size = New System.Drawing.Size(730, 408)
        Me.ToolStripContainerRV.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ToolStripContainerRV.LeftToolStripPanelVisible = False
        Me.ToolStripContainerRV.Location = New System.Drawing.Point(0, 0)
        Me.ToolStripContainerRV.Name = "ToolStripContainerRV"
        Me.ToolStripContainerRV.RightToolStripPanelVisible = False
        Me.ToolStripContainerRV.Size = New System.Drawing.Size(730, 430)
        Me.ToolStripContainerRV.TabIndex = 3
        Me.ToolStripContainerRV.Text = "ToolStripContainerRV"
        Me.ToolStripContainerRV.TopToolStripPanelVisible = False
        '
        'SplitContainerReportViewer
        '
        Me.SplitContainerReportViewer.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SplitContainerReportViewer.BackColor = System.Drawing.SystemColors.GradientInactiveCaption
        Me.SplitContainerReportViewer.Location = New System.Drawing.Point(4, 4)
        Me.SplitContainerReportViewer.Name = "SplitContainerReportViewer"
        Me.SplitContainerReportViewer.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainerReportViewer.Panel1
        '
        Me.SplitContainerReportViewer.Panel1.BackColor = System.Drawing.SystemColors.Control
        Me.SplitContainerReportViewer.Panel1.Controls.Add(Me.SplitContainerParms)
        '
        'SplitContainerReportViewer.Panel2
        '
        Me.SplitContainerReportViewer.Panel2.Controls.Add(Me.ReportViewerSQLClue)
        Me.SplitContainerReportViewer.Size = New System.Drawing.Size(722, 400)
        Me.SplitContainerReportViewer.SplitterDistance = 245
        Me.SplitContainerReportViewer.TabIndex = 5
        '
        'SplitContainerParms
        '
        Me.SplitContainerParms.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SplitContainerParms.BackColor = System.Drawing.SystemColors.ActiveBorder
        Me.SplitContainerParms.FixedPanel = System.Windows.Forms.FixedPanel.Panel2
        Me.SplitContainerParms.IsSplitterFixed = True
        Me.SplitContainerParms.Location = New System.Drawing.Point(3, 4)
        Me.SplitContainerParms.Name = "SplitContainerParms"
        '
        'SplitContainerParms.Panel1
        '
        Me.SplitContainerParms.Panel1.Controls.Add(Me.FlowLayoutPanelParms)
        '
        'SplitContainerParms.Panel2
        '
        Me.SplitContainerParms.Panel2.Controls.Add(Me.Panel1)
        Me.SplitContainerParms.Size = New System.Drawing.Size(715, 218)
        Me.SplitContainerParms.SplitterDistance = 632
        Me.SplitContainerParms.SplitterWidth = 1
        Me.SplitContainerParms.TabIndex = 12
        '
        'FlowLayoutPanelParms
        '
        Me.FlowLayoutPanelParms.AutoSize = True
        Me.FlowLayoutPanelParms.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.FlowLayoutPanelParms.BackColor = System.Drawing.SystemColors.Control
        Me.FlowLayoutPanelParms.Controls.Add(Me.PanelSQLInstance)
        Me.FlowLayoutPanelParms.Controls.Add(Me.PanelLogins)
        Me.FlowLayoutPanelParms.Controls.Add(Me.PanelDays)
        Me.FlowLayoutPanelParms.Controls.Add(Me.PanelDatabase)
        Me.FlowLayoutPanelParms.Controls.Add(Me.PanelType)
        Me.FlowLayoutPanelParms.Controls.Add(Me.PanelSubType)
        Me.FlowLayoutPanelParms.Controls.Add(Me.PanelCollection)
        Me.FlowLayoutPanelParms.Controls.Add(Me.PanelItem)
        Me.FlowLayoutPanelParms.Controls.Add(Me.PanelVersion)
        Me.FlowLayoutPanelParms.Controls.Add(Me.PanelSearchLatest)
        Me.FlowLayoutPanelParms.Controls.Add(Me.PanelBeginDt)
        Me.FlowLayoutPanelParms.Controls.Add(Me.PanelSearch)
        Me.FlowLayoutPanelParms.Controls.Add(Me.PanelEndDt)
        Me.FlowLayoutPanelParms.Controls.Add(Me.PanelCategory)
        Me.FlowLayoutPanelParms.Dock = System.Windows.Forms.DockStyle.Top
        Me.FlowLayoutPanelParms.Location = New System.Drawing.Point(0, 0)
        Me.FlowLayoutPanelParms.Name = "FlowLayoutPanelParms"
        Me.FlowLayoutPanelParms.Size = New System.Drawing.Size(632, 187)
        Me.FlowLayoutPanelParms.TabIndex = 0
        '
        'PanelSQLInstance
        '
        Me.PanelSQLInstance.AutoSize = True
        Me.PanelSQLInstance.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.PanelSQLInstance.Controls.Add(Me.LabelSQLInstance)
        Me.PanelSQLInstance.Controls.Add(Me.ComboBoxSQLInstance)
        Me.PanelSQLInstance.Location = New System.Drawing.Point(3, 3)
        Me.PanelSQLInstance.Name = "PanelSQLInstance"
        Me.PanelSQLInstance.Size = New System.Drawing.Size(212, 25)
        Me.PanelSQLInstance.TabIndex = 5
        '
        'LabelSQLInstance
        '
        Me.LabelSQLInstance.AutoSize = True
        Me.LabelSQLInstance.Location = New System.Drawing.Point(4, 5)
        Me.LabelSQLInstance.Name = "LabelSQLInstance"
        Me.LabelSQLInstance.Size = New System.Drawing.Size(72, 13)
        Me.LabelSQLInstance.TabIndex = 0
        Me.LabelSQLInstance.Text = "SQL Instance"
        '
        'ComboBoxSQLInstance
        '
        Me.ComboBoxSQLInstance.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBoxSQLInstance.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.ComboBoxSQLInstance.FormattingEnabled = True
        Me.ComboBoxSQLInstance.Location = New System.Drawing.Point(78, 1)
        Me.ComboBoxSQLInstance.Name = "ComboBoxSQLInstance"
        Me.ComboBoxSQLInstance.Size = New System.Drawing.Size(131, 21)
        Me.ComboBoxSQLInstance.TabIndex = 0
        '
        'PanelLogins
        '
        Me.PanelLogins.AutoSize = True
        Me.PanelLogins.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.PanelLogins.Controls.Add(Me.ComboBoxLoginNames)
        Me.PanelLogins.Controls.Add(Me.LabelLogin)
        Me.PanelLogins.Location = New System.Drawing.Point(221, 3)
        Me.PanelLogins.Name = "PanelLogins"
        Me.PanelLogins.Size = New System.Drawing.Size(289, 25)
        Me.PanelLogins.TabIndex = 9
        '
        'ComboBoxLoginNames
        '
        Me.ComboBoxLoginNames.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBoxLoginNames.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.ComboBoxLoginNames.FormattingEnabled = True
        Me.ComboBoxLoginNames.Location = New System.Drawing.Point(69, 1)
        Me.ComboBoxLoginNames.Name = "ComboBoxLoginNames"
        Me.ComboBoxLoginNames.Size = New System.Drawing.Size(217, 21)
        Me.ComboBoxLoginNames.TabIndex = 1
        '
        'LabelLogin
        '
        Me.LabelLogin.AutoSize = True
        Me.LabelLogin.Location = New System.Drawing.Point(1, 5)
        Me.LabelLogin.Name = "LabelLogin"
        Me.LabelLogin.Size = New System.Drawing.Size(64, 13)
        Me.LabelLogin.TabIndex = 0
        Me.LabelLogin.Text = "Login Name"
        '
        'PanelDays
        '
        Me.PanelDays.AutoSize = True
        Me.PanelDays.Controls.Add(Me.LabelDays)
        Me.PanelDays.Controls.Add(Me.NumericUpDownDays)
        Me.PanelDays.Location = New System.Drawing.Point(516, 3)
        Me.PanelDays.Name = "PanelDays"
        Me.PanelDays.Size = New System.Drawing.Size(106, 25)
        Me.PanelDays.TabIndex = 8
        '
        'LabelDays
        '
        Me.LabelDays.AutoSize = True
        Me.LabelDays.Location = New System.Drawing.Point(4, 5)
        Me.LabelDays.Name = "LabelDays"
        Me.LabelDays.Size = New System.Drawing.Size(31, 13)
        Me.LabelDays.TabIndex = 8
        Me.LabelDays.Text = "Days"
        '
        'NumericUpDownDays
        '
        Me.NumericUpDownDays.Location = New System.Drawing.Point(41, 2)
        Me.NumericUpDownDays.Maximum = New Decimal(New Integer() {9999, 0, 0, 0})
        Me.NumericUpDownDays.Name = "NumericUpDownDays"
        Me.NumericUpDownDays.Size = New System.Drawing.Size(62, 20)
        Me.NumericUpDownDays.TabIndex = 7
        Me.NumericUpDownDays.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'PanelDatabase
        '
        Me.PanelDatabase.AutoSize = True
        Me.PanelDatabase.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.PanelDatabase.Controls.Add(Me.ComboBoxDatabase)
        Me.PanelDatabase.Controls.Add(Me.LabelDatabase)
        Me.PanelDatabase.Location = New System.Drawing.Point(3, 34)
        Me.PanelDatabase.Name = "PanelDatabase"
        Me.PanelDatabase.Size = New System.Drawing.Size(203, 25)
        Me.PanelDatabase.TabIndex = 0
        '
        'ComboBoxDatabase
        '
        Me.ComboBoxDatabase.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBoxDatabase.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.ComboBoxDatabase.FormattingEnabled = True
        Me.ComboBoxDatabase.Location = New System.Drawing.Point(61, 1)
        Me.ComboBoxDatabase.Name = "ComboBoxDatabase"
        Me.ComboBoxDatabase.Size = New System.Drawing.Size(139, 21)
        Me.ComboBoxDatabase.TabIndex = 2
        '
        'LabelDatabase
        '
        Me.LabelDatabase.AutoSize = True
        Me.LabelDatabase.Location = New System.Drawing.Point(4, 5)
        Me.LabelDatabase.Name = "LabelDatabase"
        Me.LabelDatabase.Size = New System.Drawing.Size(53, 13)
        Me.LabelDatabase.TabIndex = 0
        Me.LabelDatabase.Text = "Database"
        '
        'PanelType
        '
        Me.PanelType.AutoSize = True
        Me.PanelType.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.PanelType.Controls.Add(Me.ComboBoxType)
        Me.PanelType.Controls.Add(Me.LabelType)
        Me.PanelType.Location = New System.Drawing.Point(212, 34)
        Me.PanelType.Name = "PanelType"
        Me.PanelType.Size = New System.Drawing.Size(136, 25)
        Me.PanelType.TabIndex = 0
        '
        'ComboBoxType
        '
        Me.ComboBoxType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBoxType.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.ComboBoxType.FormattingEnabled = True
        Me.ComboBoxType.Location = New System.Drawing.Point(39, 1)
        Me.ComboBoxType.Name = "ComboBoxType"
        Me.ComboBoxType.Size = New System.Drawing.Size(94, 21)
        Me.ComboBoxType.TabIndex = 3
        '
        'LabelType
        '
        Me.LabelType.AutoSize = True
        Me.LabelType.Location = New System.Drawing.Point(4, 5)
        Me.LabelType.Name = "LabelType"
        Me.LabelType.Size = New System.Drawing.Size(31, 13)
        Me.LabelType.TabIndex = 0
        Me.LabelType.Text = "Type"
        '
        'PanelSubType
        '
        Me.PanelSubType.AutoSize = True
        Me.PanelSubType.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.PanelSubType.Controls.Add(Me.ComboBoxSubType)
        Me.PanelSubType.Controls.Add(Me.LabelSubType)
        Me.PanelSubType.Location = New System.Drawing.Point(354, 34)
        Me.PanelSubType.Name = "PanelSubType"
        Me.PanelSubType.Size = New System.Drawing.Size(164, 26)
        Me.PanelSubType.TabIndex = 0
        '
        'ComboBoxSubType
        '
        Me.ComboBoxSubType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBoxSubType.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.ComboBoxSubType.FormattingEnabled = True
        Me.ComboBoxSubType.Location = New System.Drawing.Point(54, 2)
        Me.ComboBoxSubType.Name = "ComboBoxSubType"
        Me.ComboBoxSubType.Size = New System.Drawing.Size(107, 21)
        Me.ComboBoxSubType.TabIndex = 4
        '
        'LabelSubType
        '
        Me.LabelSubType.AutoSize = True
        Me.LabelSubType.Location = New System.Drawing.Point(4, 5)
        Me.LabelSubType.Name = "LabelSubType"
        Me.LabelSubType.Size = New System.Drawing.Size(50, 13)
        Me.LabelSubType.TabIndex = 0
        Me.LabelSubType.Text = "SubType"
        '
        'PanelCollection
        '
        Me.PanelCollection.AutoSize = True
        Me.PanelCollection.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.PanelCollection.Controls.Add(Me.ComboBoxCollection)
        Me.PanelCollection.Controls.Add(Me.LabelCollection)
        Me.PanelCollection.Location = New System.Drawing.Point(3, 66)
        Me.PanelCollection.Name = "PanelCollection"
        Me.PanelCollection.Size = New System.Drawing.Size(185, 25)
        Me.PanelCollection.TabIndex = 0
        '
        'ComboBoxCollection
        '
        Me.ComboBoxCollection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBoxCollection.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.ComboBoxCollection.FormattingEnabled = True
        Me.ComboBoxCollection.Location = New System.Drawing.Point(62, 1)
        Me.ComboBoxCollection.Name = "ComboBoxCollection"
        Me.ComboBoxCollection.Size = New System.Drawing.Size(120, 21)
        Me.ComboBoxCollection.TabIndex = 5
        '
        'LabelCollection
        '
        Me.LabelCollection.AutoSize = True
        Me.LabelCollection.Location = New System.Drawing.Point(4, 7)
        Me.LabelCollection.Name = "LabelCollection"
        Me.LabelCollection.Size = New System.Drawing.Size(53, 13)
        Me.LabelCollection.TabIndex = 0
        Me.LabelCollection.Text = "Collection"
        '
        'PanelItem
        '
        Me.PanelItem.AutoSize = True
        Me.PanelItem.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.PanelItem.Controls.Add(Me.ComboBoxItem)
        Me.PanelItem.Controls.Add(Me.LabelItem)
        Me.PanelItem.Location = New System.Drawing.Point(194, 66)
        Me.PanelItem.Name = "PanelItem"
        Me.PanelItem.Size = New System.Drawing.Size(287, 25)
        Me.PanelItem.TabIndex = 0
        '
        'ComboBoxItem
        '
        Me.ComboBoxItem.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBoxItem.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.ComboBoxItem.FormattingEnabled = True
        Me.ComboBoxItem.Location = New System.Drawing.Point(33, 1)
        Me.ComboBoxItem.Name = "ComboBoxItem"
        Me.ComboBoxItem.Size = New System.Drawing.Size(251, 21)
        Me.ComboBoxItem.TabIndex = 6
        '
        'LabelItem
        '
        Me.LabelItem.AutoSize = True
        Me.LabelItem.Location = New System.Drawing.Point(4, 5)
        Me.LabelItem.Name = "LabelItem"
        Me.LabelItem.Size = New System.Drawing.Size(27, 13)
        Me.LabelItem.TabIndex = 0
        Me.LabelItem.Text = "Item"
        '
        'PanelVersion
        '
        Me.PanelVersion.AutoSize = True
        Me.PanelVersion.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.PanelVersion.Controls.Add(Me.ComboBoxVersion)
        Me.PanelVersion.Controls.Add(Me.LabelVersion)
        Me.PanelVersion.Location = New System.Drawing.Point(487, 66)
        Me.PanelVersion.Name = "PanelVersion"
        Me.PanelVersion.Size = New System.Drawing.Size(96, 25)
        Me.PanelVersion.TabIndex = 0
        '
        'ComboBoxVersion
        '
        Me.ComboBoxVersion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBoxVersion.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.ComboBoxVersion.FormattingEnabled = True
        Me.ComboBoxVersion.Location = New System.Drawing.Point(52, 1)
        Me.ComboBoxVersion.Name = "ComboBoxVersion"
        Me.ComboBoxVersion.Size = New System.Drawing.Size(41, 21)
        Me.ComboBoxVersion.TabIndex = 7
        '
        'LabelVersion
        '
        Me.LabelVersion.AutoSize = True
        Me.LabelVersion.Location = New System.Drawing.Point(7, 5)
        Me.LabelVersion.Name = "LabelVersion"
        Me.LabelVersion.Size = New System.Drawing.Size(42, 13)
        Me.LabelVersion.TabIndex = 0
        Me.LabelVersion.Text = "Version"
        '
        'PanelSearch
        '
        Me.PanelSearch.AutoSize = True
        Me.PanelSearch.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.PanelSearch.Controls.Add(Me.TextBoxSearchString)
        Me.PanelSearch.Controls.Add(Me.LabelSearchString)
        Me.PanelSearch.Location = New System.Drawing.Point(3, 128)
        Me.PanelSearch.Name = "PanelSearch"
        Me.PanelSearch.Size = New System.Drawing.Size(574, 25)
        Me.PanelSearch.TabIndex = 14
        '
        'TextBoxSearchString
        '
        Me.TextBoxSearchString.AllowDrop = True
        Me.TextBoxSearchString.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextBoxSearchString.Location = New System.Drawing.Point(81, 2)
        Me.TextBoxSearchString.Multiline = True
        Me.TextBoxSearchString.Name = "TextBoxSearchString"
        Me.TextBoxSearchString.Size = New System.Drawing.Size(490, 20)
        Me.TextBoxSearchString.TabIndex = 7
        '
        'LabelSearchString
        '
        Me.LabelSearchString.AutoSize = True
        Me.LabelSearchString.Location = New System.Drawing.Point(4, 5)
        Me.LabelSearchString.Name = "LabelSearchString"
        Me.LabelSearchString.Size = New System.Drawing.Size(71, 13)
        Me.LabelSearchString.TabIndex = 0
        Me.LabelSearchString.Text = "Search String"
        '
        'PanelSearchLatest
        '
        Me.PanelSearchLatest.AutoSize = True
        Me.PanelSearchLatest.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.PanelSearchLatest.Controls.Add(Me.ComboBoxSearchLatest)
        Me.PanelSearchLatest.Controls.Add(Me.LabelSearchLatest)
        Me.PanelSearchLatest.Location = New System.Drawing.Point(3, 97)
        Me.PanelSearchLatest.Name = "PanelSearchLatest"
        Me.PanelSearchLatest.Size = New System.Drawing.Size(177, 25)
        Me.PanelSearchLatest.TabIndex = 16
        '
        'ComboBoxSearchLatest
        '
        Me.ComboBoxSearchLatest.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBoxSearchLatest.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.ComboBoxSearchLatest.FormattingEnabled = True
        Me.ComboBoxSearchLatest.Items.AddRange(New Object() {"False", "True"})
        Me.ComboBoxSearchLatest.Location = New System.Drawing.Point(114, 1)
        Me.ComboBoxSearchLatest.MaxDropDownItems = 2
        Me.ComboBoxSearchLatest.Name = "ComboBoxSearchLatest"
        Me.ComboBoxSearchLatest.Size = New System.Drawing.Size(60, 21)
        Me.ComboBoxSearchLatest.TabIndex = 7
        '
        'LabelSearchLatest
        '
        Me.LabelSearchLatest.AutoSize = True
        Me.LabelSearchLatest.Location = New System.Drawing.Point(7, 5)
        Me.LabelSearchLatest.Name = "LabelSearchLatest"
        Me.LabelSearchLatest.Size = New System.Drawing.Size(98, 13)
        Me.LabelSearchLatest.TabIndex = 0
        Me.LabelSearchLatest.Text = "Latest Version Only"
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.SystemColors.Control
        Me.Panel1.Controls.Add(Me.ButtonViewReport)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(82, 218)
        Me.Panel1.TabIndex = 0
        '
        'ButtonViewReport
        '
        Me.ButtonViewReport.Location = New System.Drawing.Point(4, 3)
        Me.ButtonViewReport.Name = "ButtonViewReport"
        Me.ButtonViewReport.Size = New System.Drawing.Size(75, 25)
        Me.ButtonViewReport.TabIndex = 8
        Me.ButtonViewReport.Text = "View &Report"
        Me.ButtonViewReport.UseVisualStyleBackColor = True
        '
        'ReportViewerSQLClue
        '
        Me.ReportViewerSQLClue.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ReportViewerSQLClue.DocumentMapWidth = 70
        Me.ReportViewerSQLClue.Location = New System.Drawing.Point(0, 0)
        Me.ReportViewerSQLClue.Name = "ReportViewerSQLClue"
        Me.ReportViewerSQLClue.Size = New System.Drawing.Size(722, 151)
        Me.ReportViewerSQLClue.TabIndex = 9
        '
        'PanelBeginDt
        '
        Me.PanelBeginDt.Controls.Add(Me.LabelBeginDt)
        Me.PanelBeginDt.Controls.Add(Me.DateTimePicker1)
        Me.PanelBeginDt.Location = New System.Drawing.Point(186, 97)
        Me.PanelBeginDt.Name = "PanelBeginDt"
        Me.PanelBeginDt.Size = New System.Drawing.Size(259, 25)
        Me.PanelBeginDt.TabIndex = 17
        '
        'DateTimePicker1
        '
        Me.DateTimePicker1.Location = New System.Drawing.Point(56, 2)
        Me.DateTimePicker1.Name = "DateTimePicker1"
        Me.DateTimePicker1.Size = New System.Drawing.Size(200, 20)
        Me.DateTimePicker1.TabIndex = 0
        '
        'LabelBeginDt
        '
        Me.LabelBeginDt.AutoSize = True
        Me.LabelBeginDt.Location = New System.Drawing.Point(6, 6)
        Me.LabelBeginDt.Name = "LabelBeginDt"
        Me.LabelBeginDt.Size = New System.Drawing.Size(45, 13)
        Me.LabelBeginDt.TabIndex = 1
        Me.LabelBeginDt.Text = "BeginDt"
        '
        'PanelEndDt
        '
        Me.PanelEndDt.Controls.Add(Me.LabelEndDt)
        Me.PanelEndDt.Controls.Add(Me.DateTimePickerEndDt)
        Me.PanelEndDt.Location = New System.Drawing.Point(3, 159)
        Me.PanelEndDt.Name = "PanelEndDt"
        Me.PanelEndDt.Size = New System.Drawing.Size(259, 25)
        Me.PanelEndDt.TabIndex = 18
        '
        'LabelEndDt
        '
        Me.LabelEndDt.AutoSize = True
        Me.LabelEndDt.Location = New System.Drawing.Point(6, 6)
        Me.LabelEndDt.Name = "LabelEndDt"
        Me.LabelEndDt.Size = New System.Drawing.Size(37, 13)
        Me.LabelEndDt.TabIndex = 1
        Me.LabelEndDt.Text = "EndDt"
        '
        'DateTimePickerEndDt
        '
        Me.DateTimePickerEndDt.Location = New System.Drawing.Point(56, 2)
        Me.DateTimePickerEndDt.Name = "DateTimePickerEndDt"
        Me.DateTimePickerEndDt.Size = New System.Drawing.Size(200, 20)
        Me.DateTimePickerEndDt.TabIndex = 0
        Me.DateTimePickerEndDt.Value = New Date(2012, 1, 14, 12, 7, 43, 0)
        '
        'PanelCategory
        '
        Me.PanelCategory.AutoSize = True
        Me.PanelCategory.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.PanelCategory.Controls.Add(Me.ComboBoxCategory)
        Me.PanelCategory.Controls.Add(Me.LabelCategory)
        Me.PanelCategory.Location = New System.Drawing.Point(268, 159)
        Me.PanelCategory.Name = "PanelCategory"
        Me.PanelCategory.Size = New System.Drawing.Size(185, 25)
        Me.PanelCategory.TabIndex = 19
        '
        'ComboBoxCategory
        '
        Me.ComboBoxCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBoxCategory.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.ComboBoxCategory.FormattingEnabled = True
        Me.ComboBoxCategory.Location = New System.Drawing.Point(62, 1)
        Me.ComboBoxCategory.Name = "ComboBoxCategory"
        Me.ComboBoxCategory.Size = New System.Drawing.Size(120, 21)
        Me.ComboBoxCategory.TabIndex = 5
        '
        'LabelCategory
        '
        Me.LabelCategory.AutoSize = True
        Me.LabelCategory.Location = New System.Drawing.Point(4, 7)
        Me.LabelCategory.Name = "LabelCategory"
        Me.LabelCategory.Size = New System.Drawing.Size(49, 13)
        Me.LabelCategory.TabIndex = 0
        Me.LabelCategory.Text = "Category"
        '
        'ReportViewerForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoScroll = True
        Me.BackColor = System.Drawing.SystemColors.Window
        Me.ClientSize = New System.Drawing.Size(730, 430)
        Me.Controls.Add(Me.ToolStripContainerRV)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.HelpButton = True
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MinimumSize = New System.Drawing.Size(350, 167)
        Me.Name = "ReportViewerForm"
        Me.Text = "Reports"
        Me.StatusStripReportViewer.ResumeLayout(False)
        Me.StatusStripReportViewer.PerformLayout()
        Me.ToolStripContainerRV.BottomToolStripPanel.ResumeLayout(False)
        Me.ToolStripContainerRV.BottomToolStripPanel.PerformLayout()
        Me.ToolStripContainerRV.ContentPanel.ResumeLayout(False)
        Me.ToolStripContainerRV.ResumeLayout(False)
        Me.ToolStripContainerRV.PerformLayout()
        Me.SplitContainerReportViewer.Panel1.ResumeLayout(False)
        Me.SplitContainerReportViewer.Panel2.ResumeLayout(False)
        CType(Me.SplitContainerReportViewer, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainerReportViewer.ResumeLayout(False)
        Me.SplitContainerParms.Panel1.ResumeLayout(False)
        Me.SplitContainerParms.Panel1.PerformLayout()
        Me.SplitContainerParms.Panel2.ResumeLayout(False)
        CType(Me.SplitContainerParms, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainerParms.ResumeLayout(False)
        Me.FlowLayoutPanelParms.ResumeLayout(False)
        Me.FlowLayoutPanelParms.PerformLayout()
        Me.PanelSQLInstance.ResumeLayout(False)
        Me.PanelSQLInstance.PerformLayout()
        Me.PanelLogins.ResumeLayout(False)
        Me.PanelLogins.PerformLayout()
        Me.PanelDays.ResumeLayout(False)
        Me.PanelDays.PerformLayout()
        CType(Me.NumericUpDownDays, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelDatabase.ResumeLayout(False)
        Me.PanelDatabase.PerformLayout()
        Me.PanelType.ResumeLayout(False)
        Me.PanelType.PerformLayout()
        Me.PanelSubType.ResumeLayout(False)
        Me.PanelSubType.PerformLayout()
        Me.PanelCollection.ResumeLayout(False)
        Me.PanelCollection.PerformLayout()
        Me.PanelItem.ResumeLayout(False)
        Me.PanelItem.PerformLayout()
        Me.PanelVersion.ResumeLayout(False)
        Me.PanelVersion.PerformLayout()
        Me.PanelSearch.ResumeLayout(False)
        Me.PanelSearch.PerformLayout()
        Me.PanelSearchLatest.ResumeLayout(False)
        Me.PanelSearchLatest.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        Me.PanelBeginDt.ResumeLayout(False)
        Me.PanelBeginDt.PerformLayout()
        Me.PanelEndDt.ResumeLayout(False)
        Me.PanelEndDt.PerformLayout()
        Me.PanelCategory.ResumeLayout(False)
        Me.PanelCategory.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents LocalToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem4 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ServerToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripStatusLabel1 As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents RVFile As System.Windows.Forms.ToolStripComboBox
    Friend WithEvents StatusStripReportViewer As System.Windows.Forms.StatusStrip
    Friend WithEvents ToolStripStatusLabel2 As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents ToolStripContainerRV As System.Windows.Forms.ToolStripContainer
    Friend WithEvents ReportViewerSQLClue As Microsoft.Reporting.WinForms.ReportViewer
    Friend WithEvents SplitContainerReportViewer As System.Windows.Forms.SplitContainer
    Friend WithEvents LabelSQLInstance As System.Windows.Forms.Label
    Friend WithEvents ComboBoxSQLInstance As System.Windows.Forms.ComboBox
    Friend WithEvents ButtonViewReport As System.Windows.Forms.Button
    Friend WithEvents PanelSQLInstance As System.Windows.Forms.Panel
    Friend WithEvents PanelDays As System.Windows.Forms.Panel
    Friend WithEvents LabelDays As System.Windows.Forms.Label
    Friend WithEvents NumericUpDownDays As System.Windows.Forms.NumericUpDown
    Friend WithEvents PanelLogins As System.Windows.Forms.Panel
    Friend WithEvents ComboBoxLoginNames As System.Windows.Forms.ComboBox
    Friend WithEvents LabelLogin As System.Windows.Forms.Label
    Friend WithEvents ComboBoxDatabase As System.Windows.Forms.ComboBox
    Friend WithEvents LabelDatabase As System.Windows.Forms.Label
    Friend WithEvents PanelType As System.Windows.Forms.Panel
    Friend WithEvents ComboBoxType As System.Windows.Forms.ComboBox
    Friend WithEvents LabelType As System.Windows.Forms.Label
    Friend WithEvents PanelSubType As System.Windows.Forms.Panel
    Friend WithEvents ComboBoxSubType As System.Windows.Forms.ComboBox
    Friend WithEvents LabelSubType As System.Windows.Forms.Label
    Friend WithEvents PanelCollection As System.Windows.Forms.Panel
    Friend WithEvents ComboBoxCollection As System.Windows.Forms.ComboBox
    Friend WithEvents LabelCollection As System.Windows.Forms.Label
    Friend WithEvents PanelItem As System.Windows.Forms.Panel
    Friend WithEvents ComboBoxItem As System.Windows.Forms.ComboBox
    Friend WithEvents LabelItem As System.Windows.Forms.Label
    Friend WithEvents PanelVersion As System.Windows.Forms.Panel
    Friend WithEvents LabelVersion As System.Windows.Forms.Label
    Friend WithEvents ComboBoxVersion As System.Windows.Forms.ComboBox
    Friend WithEvents SplitContainerParms As System.Windows.Forms.SplitContainer
    Friend WithEvents PanelDatabase As System.Windows.Forms.Panel
    Friend WithEvents FlowLayoutPanelParms As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents PanelSearch As System.Windows.Forms.Panel
    Friend WithEvents TextBoxSearchString As System.Windows.Forms.TextBox
    Friend WithEvents LabelSearchString As System.Windows.Forms.Label
    Friend WithEvents PanelSearchLatest As System.Windows.Forms.Panel
    Friend WithEvents ComboBoxSearchLatest As System.Windows.Forms.ComboBox
    Friend WithEvents LabelSearchLatest As System.Windows.Forms.Label
    Friend WithEvents PanelBeginDt As System.Windows.Forms.Panel
    Friend WithEvents LabelBeginDt As System.Windows.Forms.Label
    Friend WithEvents DateTimePicker1 As System.Windows.Forms.DateTimePicker
    Friend WithEvents PanelEndDt As System.Windows.Forms.Panel
    Friend WithEvents LabelEndDt As System.Windows.Forms.Label
    Friend WithEvents DateTimePickerEndDt As System.Windows.Forms.DateTimePicker
    Friend WithEvents PanelCategory As System.Windows.Forms.Panel
    Friend WithEvents ComboBoxCategory As System.Windows.Forms.ComboBox
    Friend WithEvents LabelCategory As System.Windows.Forms.Label
End Class
