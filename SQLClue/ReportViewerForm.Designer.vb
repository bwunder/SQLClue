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
        Me.ToolStripStatusLabel1 = New System.Windows.Forms.ToolStripStatusLabel
        Me.LocalToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.RVFile = New System.Windows.Forms.ToolStripComboBox
        Me.ToolStripMenuItem4 = New System.Windows.Forms.ToolStripMenuItem
        Me.ServerToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.StatusStripReportViewer = New System.Windows.Forms.StatusStrip
        Me.ToolStripStatusLabel2 = New System.Windows.Forms.ToolStripStatusLabel
        Me.ToolStripContainerRV = New System.Windows.Forms.ToolStripContainer
        Me.SplitContainerReportViewer = New System.Windows.Forms.SplitContainer
        Me.SplitContainerParms = New System.Windows.Forms.SplitContainer
        Me.FlowLayoutPanelParms = New System.Windows.Forms.FlowLayoutPanel
        Me.PanelSQLInstance = New System.Windows.Forms.Panel
        Me.LabelSQLInstance = New System.Windows.Forms.Label
        Me.ComboBoxSQLInstance = New System.Windows.Forms.ComboBox
        Me.PanelUsers = New System.Windows.Forms.Panel
        Me.ComboBoxLoginNames = New System.Windows.Forms.ComboBox
        Me.LabelLogin = New System.Windows.Forms.Label
        Me.PanelDays = New System.Windows.Forms.Panel
        Me.LabelDays = New System.Windows.Forms.Label
        Me.NumericUpDownDays = New System.Windows.Forms.NumericUpDown
        Me.PanelCfgInstance = New System.Windows.Forms.Panel
        Me.ComboBoxCfgInstance = New System.Windows.Forms.ComboBox
        Me.LabelCfgInstance = New System.Windows.Forms.Label
        Me.PanelCfgDatabase = New System.Windows.Forms.Panel
        Me.ComboBoxCfgDatabase = New System.Windows.Forms.ComboBox
        Me.LabelCfgDatabase = New System.Windows.Forms.Label
        Me.PanelCfgType = New System.Windows.Forms.Panel
        Me.ComboBoxCfgType = New System.Windows.Forms.ComboBox
        Me.LabelCfgType = New System.Windows.Forms.Label
        Me.PanelCfgSubType = New System.Windows.Forms.Panel
        Me.ComboBoxCfgSubType = New System.Windows.Forms.ComboBox
        Me.LabelCfgSubType = New System.Windows.Forms.Label
        Me.PanelCfgCollection = New System.Windows.Forms.Panel
        Me.ComboBoxCfgCollection = New System.Windows.Forms.ComboBox
        Me.LabelCfgCollection = New System.Windows.Forms.Label
        Me.PanelCfgItem = New System.Windows.Forms.Panel
        Me.ComboBoxCfgItem = New System.Windows.Forms.ComboBox
        Me.LabelCfgItem = New System.Windows.Forms.Label
        Me.PanelCfgVersion = New System.Windows.Forms.Panel
        Me.ComboBoxCfgVersion = New System.Windows.Forms.ComboBox
        Me.LabelCfgVersion = New System.Windows.Forms.Label
        Me.PanelRunbookSearch = New System.Windows.Forms.Panel
        Me.TextBoxRunbookSearchString = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.PanelArchiveSearch = New System.Windows.Forms.Panel
        Me.TextBoxArchiveSearchString = New System.Windows.Forms.TextBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.PanelArchiveSearchType = New System.Windows.Forms.Panel
        Me.ComboBoxArchiveSearchType = New System.Windows.Forms.ComboBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.PanelArchiveSearchLatest = New System.Windows.Forms.Panel
        Me.ComboBoxArchiveSearchLatest = New System.Windows.Forms.ComboBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.ButtonViewReport = New System.Windows.Forms.Button
        Me.ReportViewerSQLClue = New Microsoft.Reporting.WinForms.ReportViewer
        Me.StatusStripReportViewer.SuspendLayout()
        Me.ToolStripContainerRV.BottomToolStripPanel.SuspendLayout()
        Me.ToolStripContainerRV.ContentPanel.SuspendLayout()
        Me.ToolStripContainerRV.SuspendLayout()
        Me.SplitContainerReportViewer.Panel1.SuspendLayout()
        Me.SplitContainerReportViewer.Panel2.SuspendLayout()
        Me.SplitContainerReportViewer.SuspendLayout()
        Me.SplitContainerParms.Panel1.SuspendLayout()
        Me.SplitContainerParms.Panel2.SuspendLayout()
        Me.SplitContainerParms.SuspendLayout()
        Me.FlowLayoutPanelParms.SuspendLayout()
        Me.PanelSQLInstance.SuspendLayout()
        Me.PanelUsers.SuspendLayout()
        Me.PanelDays.SuspendLayout()
        CType(Me.NumericUpDownDays, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PanelCfgInstance.SuspendLayout()
        Me.PanelCfgDatabase.SuspendLayout()
        Me.PanelCfgType.SuspendLayout()
        Me.PanelCfgSubType.SuspendLayout()
        Me.PanelCfgCollection.SuspendLayout()
        Me.PanelCfgItem.SuspendLayout()
        Me.PanelCfgVersion.SuspendLayout()
        Me.PanelRunbookSearch.SuspendLayout()
        Me.PanelArchiveSearch.SuspendLayout()
        Me.PanelArchiveSearchType.SuspendLayout()
        Me.PanelArchiveSearchLatest.SuspendLayout()
        Me.Panel1.SuspendLayout()
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
        Me.SplitContainerParms.SplitterDistance = 629
        Me.SplitContainerParms.SplitterWidth = 1
        Me.SplitContainerParms.TabIndex = 12
        '
        'FlowLayoutPanelParms
        '
        Me.FlowLayoutPanelParms.AutoSize = True
        Me.FlowLayoutPanelParms.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.FlowLayoutPanelParms.BackColor = System.Drawing.SystemColors.Control
        Me.FlowLayoutPanelParms.Controls.Add(Me.PanelSQLInstance)
        Me.FlowLayoutPanelParms.Controls.Add(Me.PanelUsers)
        Me.FlowLayoutPanelParms.Controls.Add(Me.PanelDays)
        Me.FlowLayoutPanelParms.Controls.Add(Me.PanelCfgInstance)
        Me.FlowLayoutPanelParms.Controls.Add(Me.PanelCfgDatabase)
        Me.FlowLayoutPanelParms.Controls.Add(Me.PanelCfgType)
        Me.FlowLayoutPanelParms.Controls.Add(Me.PanelCfgSubType)
        Me.FlowLayoutPanelParms.Controls.Add(Me.PanelCfgCollection)
        Me.FlowLayoutPanelParms.Controls.Add(Me.PanelCfgItem)
        Me.FlowLayoutPanelParms.Controls.Add(Me.PanelCfgVersion)
        Me.FlowLayoutPanelParms.Controls.Add(Me.PanelRunbookSearch)
        Me.FlowLayoutPanelParms.Controls.Add(Me.PanelArchiveSearch)
        Me.FlowLayoutPanelParms.Controls.Add(Me.PanelArchiveSearchType)
        Me.FlowLayoutPanelParms.Controls.Add(Me.PanelArchiveSearchLatest)
        Me.FlowLayoutPanelParms.Dock = System.Windows.Forms.DockStyle.Top
        Me.FlowLayoutPanelParms.Location = New System.Drawing.Point(0, 0)
        Me.FlowLayoutPanelParms.Name = "FlowLayoutPanelParms"
        Me.FlowLayoutPanelParms.Size = New System.Drawing.Size(629, 218)
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
        'PanelUsers
        '
        Me.PanelUsers.AutoSize = True
        Me.PanelUsers.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.PanelUsers.Controls.Add(Me.ComboBoxLoginNames)
        Me.PanelUsers.Controls.Add(Me.LabelLogin)
        Me.PanelUsers.Location = New System.Drawing.Point(221, 3)
        Me.PanelUsers.Name = "PanelUsers"
        Me.PanelUsers.Size = New System.Drawing.Size(289, 25)
        Me.PanelUsers.TabIndex = 9
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
        'PanelCfgInstance
        '
        Me.PanelCfgInstance.AutoSize = True
        Me.PanelCfgInstance.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.PanelCfgInstance.Controls.Add(Me.ComboBoxCfgInstance)
        Me.PanelCfgInstance.Controls.Add(Me.LabelCfgInstance)
        Me.PanelCfgInstance.Location = New System.Drawing.Point(3, 34)
        Me.PanelCfgInstance.Name = "PanelCfgInstance"
        Me.PanelCfgInstance.Size = New System.Drawing.Size(220, 25)
        Me.PanelCfgInstance.TabIndex = 0
        '
        'ComboBoxCfgInstance
        '
        Me.ComboBoxCfgInstance.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBoxCfgInstance.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.ComboBoxCfgInstance.FormattingEnabled = True
        Me.ComboBoxCfgInstance.Location = New System.Drawing.Point(78, 1)
        Me.ComboBoxCfgInstance.Name = "ComboBoxCfgInstance"
        Me.ComboBoxCfgInstance.Size = New System.Drawing.Size(139, 21)
        Me.ComboBoxCfgInstance.TabIndex = 1
        '
        'LabelCfgInstance
        '
        Me.LabelCfgInstance.AutoSize = True
        Me.LabelCfgInstance.Location = New System.Drawing.Point(4, 5)
        Me.LabelCfgInstance.Name = "LabelCfgInstance"
        Me.LabelCfgInstance.Size = New System.Drawing.Size(72, 13)
        Me.LabelCfgInstance.TabIndex = 0
        Me.LabelCfgInstance.Text = "SQL Instance"
        '
        'PanelCfgDatabase
        '
        Me.PanelCfgDatabase.AutoSize = True
        Me.PanelCfgDatabase.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.PanelCfgDatabase.Controls.Add(Me.ComboBoxCfgDatabase)
        Me.PanelCfgDatabase.Controls.Add(Me.LabelCfgDatabase)
        Me.PanelCfgDatabase.Location = New System.Drawing.Point(229, 34)
        Me.PanelCfgDatabase.Name = "PanelCfgDatabase"
        Me.PanelCfgDatabase.Size = New System.Drawing.Size(203, 25)
        Me.PanelCfgDatabase.TabIndex = 0
        '
        'ComboBoxCfgDatabase
        '
        Me.ComboBoxCfgDatabase.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBoxCfgDatabase.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.ComboBoxCfgDatabase.FormattingEnabled = True
        Me.ComboBoxCfgDatabase.Location = New System.Drawing.Point(61, 1)
        Me.ComboBoxCfgDatabase.Name = "ComboBoxCfgDatabase"
        Me.ComboBoxCfgDatabase.Size = New System.Drawing.Size(139, 21)
        Me.ComboBoxCfgDatabase.TabIndex = 2
        '
        'LabelCfgDatabase
        '
        Me.LabelCfgDatabase.AutoSize = True
        Me.LabelCfgDatabase.Location = New System.Drawing.Point(4, 5)
        Me.LabelCfgDatabase.Name = "LabelCfgDatabase"
        Me.LabelCfgDatabase.Size = New System.Drawing.Size(53, 13)
        Me.LabelCfgDatabase.TabIndex = 0
        Me.LabelCfgDatabase.Text = "Database"
        '
        'PanelCfgType
        '
        Me.PanelCfgType.AutoSize = True
        Me.PanelCfgType.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.PanelCfgType.Controls.Add(Me.ComboBoxCfgType)
        Me.PanelCfgType.Controls.Add(Me.LabelCfgType)
        Me.PanelCfgType.Location = New System.Drawing.Point(438, 34)
        Me.PanelCfgType.Name = "PanelCfgType"
        Me.PanelCfgType.Size = New System.Drawing.Size(136, 25)
        Me.PanelCfgType.TabIndex = 0
        '
        'ComboBoxCfgType
        '
        Me.ComboBoxCfgType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBoxCfgType.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.ComboBoxCfgType.FormattingEnabled = True
        Me.ComboBoxCfgType.Location = New System.Drawing.Point(39, 1)
        Me.ComboBoxCfgType.Name = "ComboBoxCfgType"
        Me.ComboBoxCfgType.Size = New System.Drawing.Size(94, 21)
        Me.ComboBoxCfgType.TabIndex = 3
        '
        'LabelCfgType
        '
        Me.LabelCfgType.AutoSize = True
        Me.LabelCfgType.Location = New System.Drawing.Point(4, 5)
        Me.LabelCfgType.Name = "LabelCfgType"
        Me.LabelCfgType.Size = New System.Drawing.Size(31, 13)
        Me.LabelCfgType.TabIndex = 0
        Me.LabelCfgType.Text = "Type"
        '
        'PanelCfgSubType
        '
        Me.PanelCfgSubType.AutoSize = True
        Me.PanelCfgSubType.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.PanelCfgSubType.Controls.Add(Me.ComboBoxCfgSubType)
        Me.PanelCfgSubType.Controls.Add(Me.LabelCfgSubType)
        Me.PanelCfgSubType.Location = New System.Drawing.Point(3, 65)
        Me.PanelCfgSubType.Name = "PanelCfgSubType"
        Me.PanelCfgSubType.Size = New System.Drawing.Size(164, 26)
        Me.PanelCfgSubType.TabIndex = 0
        '
        'ComboBoxCfgSubType
        '
        Me.ComboBoxCfgSubType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBoxCfgSubType.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.ComboBoxCfgSubType.FormattingEnabled = True
        Me.ComboBoxCfgSubType.Location = New System.Drawing.Point(54, 2)
        Me.ComboBoxCfgSubType.Name = "ComboBoxCfgSubType"
        Me.ComboBoxCfgSubType.Size = New System.Drawing.Size(107, 21)
        Me.ComboBoxCfgSubType.TabIndex = 4
        '
        'LabelCfgSubType
        '
        Me.LabelCfgSubType.AutoSize = True
        Me.LabelCfgSubType.Location = New System.Drawing.Point(4, 5)
        Me.LabelCfgSubType.Name = "LabelCfgSubType"
        Me.LabelCfgSubType.Size = New System.Drawing.Size(50, 13)
        Me.LabelCfgSubType.TabIndex = 0
        Me.LabelCfgSubType.Text = "SubType"
        '
        'PanelCfgCollection
        '
        Me.PanelCfgCollection.AutoSize = True
        Me.PanelCfgCollection.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.PanelCfgCollection.Controls.Add(Me.ComboBoxCfgCollection)
        Me.PanelCfgCollection.Controls.Add(Me.LabelCfgCollection)
        Me.PanelCfgCollection.Location = New System.Drawing.Point(173, 65)
        Me.PanelCfgCollection.Name = "PanelCfgCollection"
        Me.PanelCfgCollection.Size = New System.Drawing.Size(185, 25)
        Me.PanelCfgCollection.TabIndex = 0
        '
        'ComboBoxCfgCollection
        '
        Me.ComboBoxCfgCollection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBoxCfgCollection.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.ComboBoxCfgCollection.FormattingEnabled = True
        Me.ComboBoxCfgCollection.Location = New System.Drawing.Point(62, 1)
        Me.ComboBoxCfgCollection.Name = "ComboBoxCfgCollection"
        Me.ComboBoxCfgCollection.Size = New System.Drawing.Size(120, 21)
        Me.ComboBoxCfgCollection.TabIndex = 5
        '
        'LabelCfgCollection
        '
        Me.LabelCfgCollection.AutoSize = True
        Me.LabelCfgCollection.Location = New System.Drawing.Point(4, 7)
        Me.LabelCfgCollection.Name = "LabelCfgCollection"
        Me.LabelCfgCollection.Size = New System.Drawing.Size(53, 13)
        Me.LabelCfgCollection.TabIndex = 0
        Me.LabelCfgCollection.Text = "Collection"
        '
        'PanelCfgItem
        '
        Me.PanelCfgItem.AutoSize = True
        Me.PanelCfgItem.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.PanelCfgItem.Controls.Add(Me.ComboBoxCfgItem)
        Me.PanelCfgItem.Controls.Add(Me.LabelCfgItem)
        Me.PanelCfgItem.Location = New System.Drawing.Point(3, 97)
        Me.PanelCfgItem.Name = "PanelCfgItem"
        Me.PanelCfgItem.Size = New System.Drawing.Size(287, 25)
        Me.PanelCfgItem.TabIndex = 0
        '
        'ComboBoxCfgItem
        '
        Me.ComboBoxCfgItem.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBoxCfgItem.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.ComboBoxCfgItem.FormattingEnabled = True
        Me.ComboBoxCfgItem.Location = New System.Drawing.Point(33, 1)
        Me.ComboBoxCfgItem.Name = "ComboBoxCfgItem"
        Me.ComboBoxCfgItem.Size = New System.Drawing.Size(251, 21)
        Me.ComboBoxCfgItem.TabIndex = 6
        '
        'LabelCfgItem
        '
        Me.LabelCfgItem.AutoSize = True
        Me.LabelCfgItem.Location = New System.Drawing.Point(4, 5)
        Me.LabelCfgItem.Name = "LabelCfgItem"
        Me.LabelCfgItem.Size = New System.Drawing.Size(27, 13)
        Me.LabelCfgItem.TabIndex = 0
        Me.LabelCfgItem.Text = "Item"
        '
        'PanelCfgVersion
        '
        Me.PanelCfgVersion.AutoSize = True
        Me.PanelCfgVersion.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.PanelCfgVersion.Controls.Add(Me.ComboBoxCfgVersion)
        Me.PanelCfgVersion.Controls.Add(Me.LabelCfgVersion)
        Me.PanelCfgVersion.Location = New System.Drawing.Point(296, 97)
        Me.PanelCfgVersion.Name = "PanelCfgVersion"
        Me.PanelCfgVersion.Size = New System.Drawing.Size(96, 25)
        Me.PanelCfgVersion.TabIndex = 0
        '
        'ComboBoxCfgVersion
        '
        Me.ComboBoxCfgVersion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBoxCfgVersion.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.ComboBoxCfgVersion.FormattingEnabled = True
        Me.ComboBoxCfgVersion.Location = New System.Drawing.Point(52, 1)
        Me.ComboBoxCfgVersion.Name = "ComboBoxCfgVersion"
        Me.ComboBoxCfgVersion.Size = New System.Drawing.Size(41, 21)
        Me.ComboBoxCfgVersion.TabIndex = 7
        '
        'LabelCfgVersion
        '
        Me.LabelCfgVersion.AutoSize = True
        Me.LabelCfgVersion.Location = New System.Drawing.Point(7, 5)
        Me.LabelCfgVersion.Name = "LabelCfgVersion"
        Me.LabelCfgVersion.Size = New System.Drawing.Size(42, 13)
        Me.LabelCfgVersion.TabIndex = 0
        Me.LabelCfgVersion.Text = "Version"
        '
        'PanelRunbookSearch
        '
        Me.PanelRunbookSearch.AutoSize = True
        Me.PanelRunbookSearch.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.PanelRunbookSearch.Controls.Add(Me.TextBoxRunbookSearchString)
        Me.PanelRunbookSearch.Controls.Add(Me.Label1)
        Me.PanelRunbookSearch.Location = New System.Drawing.Point(3, 128)
        Me.PanelRunbookSearch.Name = "PanelRunbookSearch"
        Me.PanelRunbookSearch.Size = New System.Drawing.Size(574, 25)
        Me.PanelRunbookSearch.TabIndex = 10
        '
        'TextBoxRunbookSearchString
        '
        Me.TextBoxRunbookSearchString.AllowDrop = True
        Me.TextBoxRunbookSearchString.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextBoxRunbookSearchString.Location = New System.Drawing.Point(81, 2)
        Me.TextBoxRunbookSearchString.Multiline = True
        Me.TextBoxRunbookSearchString.Name = "TextBoxRunbookSearchString"
        Me.TextBoxRunbookSearchString.Size = New System.Drawing.Size(490, 20)
        Me.TextBoxRunbookSearchString.TabIndex = 7
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(4, 5)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(71, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Search String"
        '
        'PanelArchiveSearch
        '
        Me.PanelArchiveSearch.AutoSize = True
        Me.PanelArchiveSearch.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.PanelArchiveSearch.Controls.Add(Me.TextBoxArchiveSearchString)
        Me.PanelArchiveSearch.Controls.Add(Me.Label2)
        Me.PanelArchiveSearch.Location = New System.Drawing.Point(3, 159)
        Me.PanelArchiveSearch.Name = "PanelArchiveSearch"
        Me.PanelArchiveSearch.Size = New System.Drawing.Size(574, 25)
        Me.PanelArchiveSearch.TabIndex = 14
        '
        'TextBoxArchiveSearchString
        '
        Me.TextBoxArchiveSearchString.AllowDrop = True
        Me.TextBoxArchiveSearchString.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextBoxArchiveSearchString.Location = New System.Drawing.Point(81, 2)
        Me.TextBoxArchiveSearchString.Multiline = True
        Me.TextBoxArchiveSearchString.Name = "TextBoxArchiveSearchString"
        Me.TextBoxArchiveSearchString.Size = New System.Drawing.Size(490, 20)
        Me.TextBoxArchiveSearchString.TabIndex = 7
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(4, 5)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(71, 13)
        Me.Label2.TabIndex = 0
        Me.Label2.Text = "Search String"
        '
        'PanelArchiveSearchType
        '
        Me.PanelArchiveSearchType.AutoSize = True
        Me.PanelArchiveSearchType.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.PanelArchiveSearchType.Controls.Add(Me.ComboBoxArchiveSearchType)
        Me.PanelArchiveSearchType.Controls.Add(Me.Label3)
        Me.PanelArchiveSearchType.Location = New System.Drawing.Point(3, 190)
        Me.PanelArchiveSearchType.Name = "PanelArchiveSearchType"
        Me.PanelArchiveSearchType.Size = New System.Drawing.Size(155, 25)
        Me.PanelArchiveSearchType.TabIndex = 15
        '
        'ComboBoxArchiveSearchType
        '
        Me.ComboBoxArchiveSearchType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBoxArchiveSearchType.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.ComboBoxArchiveSearchType.FormattingEnabled = True
        Me.ComboBoxArchiveSearchType.Items.AddRange(New Object() {"Metadata", "SQLInstance"})
        Me.ComboBoxArchiveSearchType.Location = New System.Drawing.Point(52, 1)
        Me.ComboBoxArchiveSearchType.MaxDropDownItems = 2
        Me.ComboBoxArchiveSearchType.Name = "ComboBoxArchiveSearchType"
        Me.ComboBoxArchiveSearchType.Size = New System.Drawing.Size(100, 21)
        Me.ComboBoxArchiveSearchType.TabIndex = 7
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(7, 5)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(31, 13)
        Me.Label3.TabIndex = 0
        Me.Label3.Text = "Type"
        '
        'PanelArchiveSearchLatest
        '
        Me.PanelArchiveSearchLatest.AutoSize = True
        Me.PanelArchiveSearchLatest.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.PanelArchiveSearchLatest.Controls.Add(Me.ComboBoxArchiveSearchLatest)
        Me.PanelArchiveSearchLatest.Controls.Add(Me.Label4)
        Me.PanelArchiveSearchLatest.Location = New System.Drawing.Point(164, 190)
        Me.PanelArchiveSearchLatest.Name = "PanelArchiveSearchLatest"
        Me.PanelArchiveSearchLatest.Size = New System.Drawing.Size(177, 25)
        Me.PanelArchiveSearchLatest.TabIndex = 16
        '
        'ComboBoxArchiveSearchLatest
        '
        Me.ComboBoxArchiveSearchLatest.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBoxArchiveSearchLatest.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.ComboBoxArchiveSearchLatest.FormattingEnabled = True
        Me.ComboBoxArchiveSearchLatest.Items.AddRange(New Object() {"False", "True"})
        Me.ComboBoxArchiveSearchLatest.Location = New System.Drawing.Point(114, 1)
        Me.ComboBoxArchiveSearchLatest.MaxDropDownItems = 2
        Me.ComboBoxArchiveSearchLatest.Name = "ComboBoxArchiveSearchLatest"
        Me.ComboBoxArchiveSearchLatest.Size = New System.Drawing.Size(60, 21)
        Me.ComboBoxArchiveSearchLatest.TabIndex = 7
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(7, 5)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(98, 13)
        Me.Label4.TabIndex = 0
        Me.Label4.Text = "Latest Version Only"
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.SystemColors.Control
        Me.Panel1.Controls.Add(Me.ButtonViewReport)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(85, 218)
        Me.Panel1.TabIndex = 0
        '
        'ButtonViewReport
        '
        Me.ButtonViewReport.Location = New System.Drawing.Point(8, 3)
        Me.ButtonViewReport.Name = "ButtonViewReport"
        Me.ButtonViewReport.Size = New System.Drawing.Size(75, 23)
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
        Me.SplitContainerReportViewer.ResumeLayout(False)
        Me.SplitContainerParms.Panel1.ResumeLayout(False)
        Me.SplitContainerParms.Panel1.PerformLayout()
        Me.SplitContainerParms.Panel2.ResumeLayout(False)
        Me.SplitContainerParms.ResumeLayout(False)
        Me.FlowLayoutPanelParms.ResumeLayout(False)
        Me.FlowLayoutPanelParms.PerformLayout()
        Me.PanelSQLInstance.ResumeLayout(False)
        Me.PanelSQLInstance.PerformLayout()
        Me.PanelUsers.ResumeLayout(False)
        Me.PanelUsers.PerformLayout()
        Me.PanelDays.ResumeLayout(False)
        Me.PanelDays.PerformLayout()
        CType(Me.NumericUpDownDays, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelCfgInstance.ResumeLayout(False)
        Me.PanelCfgInstance.PerformLayout()
        Me.PanelCfgDatabase.ResumeLayout(False)
        Me.PanelCfgDatabase.PerformLayout()
        Me.PanelCfgType.ResumeLayout(False)
        Me.PanelCfgType.PerformLayout()
        Me.PanelCfgSubType.ResumeLayout(False)
        Me.PanelCfgSubType.PerformLayout()
        Me.PanelCfgCollection.ResumeLayout(False)
        Me.PanelCfgCollection.PerformLayout()
        Me.PanelCfgItem.ResumeLayout(False)
        Me.PanelCfgItem.PerformLayout()
        Me.PanelCfgVersion.ResumeLayout(False)
        Me.PanelCfgVersion.PerformLayout()
        Me.PanelRunbookSearch.ResumeLayout(False)
        Me.PanelRunbookSearch.PerformLayout()
        Me.PanelArchiveSearch.ResumeLayout(False)
        Me.PanelArchiveSearch.PerformLayout()
        Me.PanelArchiveSearchType.ResumeLayout(False)
        Me.PanelArchiveSearchType.PerformLayout()
        Me.PanelArchiveSearchLatest.ResumeLayout(False)
        Me.PanelArchiveSearchLatest.PerformLayout()
        Me.Panel1.ResumeLayout(False)
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
    Friend WithEvents PanelUsers As System.Windows.Forms.Panel
    Friend WithEvents ComboBoxLoginNames As System.Windows.Forms.ComboBox
    Friend WithEvents LabelLogin As System.Windows.Forms.Label
    Friend WithEvents ComboBoxCfgInstance As System.Windows.Forms.ComboBox
    Friend WithEvents LabelCfgInstance As System.Windows.Forms.Label
    Friend WithEvents ComboBoxCfgDatabase As System.Windows.Forms.ComboBox
    Friend WithEvents LabelCfgDatabase As System.Windows.Forms.Label
    Friend WithEvents PanelCfgType As System.Windows.Forms.Panel
    Friend WithEvents ComboBoxCfgType As System.Windows.Forms.ComboBox
    Friend WithEvents LabelCfgType As System.Windows.Forms.Label
    Friend WithEvents PanelCfgSubType As System.Windows.Forms.Panel
    Friend WithEvents ComboBoxCfgSubType As System.Windows.Forms.ComboBox
    Friend WithEvents LabelCfgSubType As System.Windows.Forms.Label
    Friend WithEvents PanelCfgCollection As System.Windows.Forms.Panel
    Friend WithEvents ComboBoxCfgCollection As System.Windows.Forms.ComboBox
    Friend WithEvents LabelCfgCollection As System.Windows.Forms.Label
    Friend WithEvents PanelCfgItem As System.Windows.Forms.Panel
    Friend WithEvents ComboBoxCfgItem As System.Windows.Forms.ComboBox
    Friend WithEvents LabelCfgItem As System.Windows.Forms.Label
    Friend WithEvents PanelCfgVersion As System.Windows.Forms.Panel
    Friend WithEvents LabelCfgVersion As System.Windows.Forms.Label
    Friend WithEvents ComboBoxCfgVersion As System.Windows.Forms.ComboBox
    Friend WithEvents SplitContainerParms As System.Windows.Forms.SplitContainer
    Friend WithEvents PanelCfgInstance As System.Windows.Forms.Panel
    Friend WithEvents PanelCfgDatabase As System.Windows.Forms.Panel
    Friend WithEvents FlowLayoutPanelParms As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents PanelRunbookSearch As System.Windows.Forms.Panel
    Friend WithEvents TextBoxRunbookSearchString As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents PanelArchiveSearch As System.Windows.Forms.Panel
    Friend WithEvents TextBoxArchiveSearchString As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents PanelArchiveSearchType As System.Windows.Forms.Panel
    Friend WithEvents ComboBoxArchiveSearchType As System.Windows.Forms.ComboBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents PanelArchiveSearchLatest As System.Windows.Forms.Panel
    Friend WithEvents ComboBoxArchiveSearchLatest As System.Windows.Forms.ComboBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
End Class
