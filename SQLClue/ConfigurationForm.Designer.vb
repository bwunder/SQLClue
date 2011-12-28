<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ConfigurationForm
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ConfigurationForm))
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle6 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle7 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle8 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Me.ToolTipPlanning = New System.Windows.Forms.ToolTip(Me.components)
        Me.LabelConfigfuration = New System.Windows.Forms.Label
        Me.LabelArchiveSchedule = New System.Windows.Forms.Label
        Me.ButtonCancel = New System.Windows.Forms.Button
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer
        Me.DataGridViewConnections = New System.Windows.Forms.DataGridView
        Me.DataGridViewButtonColumnConnectionInstanceName = New System.Windows.Forms.DataGridViewButtonColumn
        Me.DataGridViewCheckBoxColumnConnectionEncryptedConnection = New System.Windows.Forms.DataGridViewCheckBoxColumn
        Me.DataGridViewCheckBoxColumnConnectionTrustServerCertificate = New System.Windows.Forms.DataGridViewCheckBoxColumn
        Me.DataGridViewTextBoxColumnConnectionNetworkProtocol = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.DataGridViewTextBoxColumnConnectionConnectionTimeout = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.DataGridViewCheckBoxColumnConnectionLoginSecure = New System.Windows.Forms.DataGridViewCheckBoxColumn
        Me.BindingSourceConnection = New System.Windows.Forms.BindingSource(Me.components)
        Me.smoTreeView = New System.Windows.Forms.TreeView
        Me.SplitContainerVertical = New System.Windows.Forms.SplitContainer
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.DataGridViewArchiveSchedules = New System.Windows.Forms.DataGridView
        Me.DataGridViewTextBoxColumnArchiveScheduleId = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.DataGridViewTextBoxColumnArchiveScheduleInstanceName = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.DataGridViewButtonColumnArchiveScheduleIntervalType = New System.Windows.Forms.DataGridViewButtonColumn
        Me.DataGridViewButtonColumnArchiveScheduleInterval = New System.Windows.Forms.DataGridViewButtonColumn
        Me.DataGridViewButtonColumnArchiveScheduleIntervalBaseDt = New System.Windows.Forms.DataGridViewButtonColumn
        Me.DataGridViewCheckBoxColumnArchiveScheduleIsActive = New System.Windows.Forms.DataGridViewCheckBoxColumn
        Me.DataGridViewImageColumnArchiveScheduleRunNow = New System.Windows.Forms.DataGridViewImageColumn
        Me.DataGridViewCheckBoxColumnArchiveScheduleUseEventNotifications = New System.Windows.Forms.DataGridViewCheckBoxColumn
        Me.DataGridViewImageColumnArchiveScheduleViewQueue = New System.Windows.Forms.DataGridViewImageColumn
        Me.BindingSourceArchiveSchedule = New System.Windows.Forms.BindingSource(Me.components)
        Me.BottomToolStripPanel = New System.Windows.Forms.ToolStripPanel
        Me.TopToolStripPanel = New System.Windows.Forms.ToolStripPanel
        Me.RightToolStripPanel = New System.Windows.Forms.ToolStripPanel
        Me.LeftToolStripPanel = New System.Windows.Forms.ToolStripPanel
        Me.ContentPanel = New System.Windows.Forms.ToolStripContentPanel
        Me.ToolStripContainer1 = New System.Windows.Forms.ToolStripContainer
        Me.ButtonCancelArchive = New System.Windows.Forms.Button
        Me.SplitContainer2 = New System.Windows.Forms.SplitContainer
        Me.ButtonSave = New System.Windows.Forms.Button
        Me.StatusStripPlanning = New System.Windows.Forms.StatusStrip
        Me.ToolStripStatusLabelConfiguration = New System.Windows.Forms.ToolStripStatusLabel
        Me.ToolStripProgressBarArchive = New System.Windows.Forms.ToolStripProgressBar
        Me.ContextMenuStripTreeNode = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ToolStripMenuItemLabelItem = New System.Windows.Forms.ToolStripMenuItem
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        CType(Me.DataGridViewConnections, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BindingSourceConnection, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainerVertical.Panel1.SuspendLayout()
        Me.SplitContainerVertical.Panel2.SuspendLayout()
        Me.SplitContainerVertical.SuspendLayout()
        Me.Panel1.SuspendLayout()
        CType(Me.DataGridViewArchiveSchedules, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BindingSourceArchiveSchedule, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ToolStripContainer1.ContentPanel.SuspendLayout()
        Me.ToolStripContainer1.SuspendLayout()
        Me.SplitContainer2.Panel1.SuspendLayout()
        Me.SplitContainer2.Panel2.SuspendLayout()
        Me.SplitContainer2.SuspendLayout()
        Me.StatusStripPlanning.SuspendLayout()
        Me.ContextMenuStripTreeNode.SuspendLayout()
        Me.SuspendLayout()
        '
        'LabelConfigfuration
        '
        Me.LabelConfigfuration.Dock = System.Windows.Forms.DockStyle.Top
        Me.LabelConfigfuration.Location = New System.Drawing.Point(0, 0)
        Me.LabelConfigfuration.Name = "LabelConfigfuration"
        Me.LabelConfigfuration.Size = New System.Drawing.Size(364, 21)
        Me.LabelConfigfuration.TabIndex = 5
        Me.LabelConfigfuration.Text = "Items to Archive" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
        Me.ToolTipPlanning.SetToolTip(Me.LabelConfigfuration, resources.GetString("LabelConfigfuration.ToolTip"))
        '
        'LabelArchiveSchedule
        '
        Me.LabelArchiveSchedule.Dock = System.Windows.Forms.DockStyle.Top
        Me.LabelArchiveSchedule.Location = New System.Drawing.Point(0, 0)
        Me.LabelArchiveSchedule.Name = "LabelArchiveSchedule"
        Me.LabelArchiveSchedule.Size = New System.Drawing.Size(720, 18)
        Me.LabelArchiveSchedule.TabIndex = 2
        Me.LabelArchiveSchedule.Text = "Schedules"
        Me.ToolTipPlanning.SetToolTip(Me.LabelArchiveSchedule, resources.GetString("LabelArchiveSchedule.ToolTip"))
        '
        'ButtonCancel
        '
        Me.ButtonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.ButtonCancel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ButtonCancel.Location = New System.Drawing.Point(0, 0)
        Me.ButtonCancel.Name = "ButtonCancel"
        Me.ButtonCancel.Size = New System.Drawing.Size(95, 22)
        Me.ButtonCancel.TabIndex = 24
        Me.ButtonCancel.Text = "Cancel"
        Me.ToolTipPlanning.SetToolTip(Me.ButtonCancel, "Discard all changes")
        Me.ButtonCancel.UseVisualStyleBackColor = True
        '
        'SplitContainer1
        '
        Me.SplitContainer1.BackColor = System.Drawing.SystemColors.GradientInactiveCaption
        Me.SplitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer1.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer1.Name = "SplitContainer1"
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.BackColor = System.Drawing.SystemColors.Control
        Me.SplitContainer1.Panel1.Controls.Add(Me.DataGridViewConnections)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.AutoScroll = True
        Me.SplitContainer1.Panel2.BackColor = System.Drawing.SystemColors.Control
        Me.SplitContainer1.Panel2.Controls.Add(Me.smoTreeView)
        Me.SplitContainer1.Panel2.Controls.Add(Me.LabelConfigfuration)
        Me.SplitContainer1.Size = New System.Drawing.Size(722, 217)
        Me.SplitContainer1.SplitterDistance = 352
        Me.SplitContainer1.TabIndex = 19
        '
        'DataGridViewConnections
        '
        Me.DataGridViewConnections.AllowUserToDeleteRows = False
        Me.DataGridViewConnections.AllowUserToOrderColumns = True
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.DataGridViewConnections.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle1
        Me.DataGridViewConnections.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.DataGridViewConnections.AutoGenerateColumns = False
        Me.DataGridViewConnections.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.DataGridViewConnections.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.DataGridViewConnections.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.[Single]
        Me.DataGridViewConnections.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridViewConnections.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.DataGridViewButtonColumnConnectionInstanceName, Me.DataGridViewCheckBoxColumnConnectionEncryptedConnection, Me.DataGridViewCheckBoxColumnConnectionTrustServerCertificate, Me.DataGridViewTextBoxColumnConnectionNetworkProtocol, Me.DataGridViewTextBoxColumnConnectionConnectionTimeout, Me.DataGridViewCheckBoxColumnConnectionLoginSecure})
        Me.DataGridViewConnections.Cursor = System.Windows.Forms.Cursors.Default
        Me.DataGridViewConnections.DataSource = Me.BindingSourceConnection
        Me.DataGridViewConnections.Location = New System.Drawing.Point(4, 4)
        Me.DataGridViewConnections.Margin = New System.Windows.Forms.Padding(0)
        Me.DataGridViewConnections.MultiSelect = False
        Me.DataGridViewConnections.Name = "DataGridViewConnections"
        Me.DataGridViewConnections.ReadOnly = True
        Me.DataGridViewConnections.RowHeadersWidth = 30
        Me.DataGridViewConnections.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.DataGridViewConnections.Size = New System.Drawing.Size(342, 207)
        Me.DataGridViewConnections.TabIndex = 16
        '
        'DataGridViewButtonColumnConnectionInstanceName
        '
        Me.DataGridViewButtonColumnConnectionInstanceName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
        Me.DataGridViewButtonColumnConnectionInstanceName.DataPropertyName = "InstanceName"
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle2.NullValue = "     new"
        Me.DataGridViewButtonColumnConnectionInstanceName.DefaultCellStyle = DataGridViewCellStyle2
        Me.DataGridViewButtonColumnConnectionInstanceName.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.DataGridViewButtonColumnConnectionInstanceName.HeaderText = "SQL Server Instance "
        Me.DataGridViewButtonColumnConnectionInstanceName.MinimumWidth = 25
        Me.DataGridViewButtonColumnConnectionInstanceName.Name = "DataGridViewButtonColumnConnectionInstanceName"
        Me.DataGridViewButtonColumnConnectionInstanceName.ReadOnly = True
        Me.DataGridViewButtonColumnConnectionInstanceName.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DataGridViewButtonColumnConnectionInstanceName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        Me.DataGridViewButtonColumnConnectionInstanceName.Text = "add new"
        '
        'DataGridViewCheckBoxColumnConnectionEncryptedConnection
        '
        Me.DataGridViewCheckBoxColumnConnectionEncryptedConnection.DataPropertyName = "EncryptedConnection"
        Me.DataGridViewCheckBoxColumnConnectionEncryptedConnection.HeaderText = "Encrypt Connection"
        Me.DataGridViewCheckBoxColumnConnectionEncryptedConnection.MinimumWidth = 70
        Me.DataGridViewCheckBoxColumnConnectionEncryptedConnection.Name = "DataGridViewCheckBoxColumnConnectionEncryptedConnection"
        Me.DataGridViewCheckBoxColumnConnectionEncryptedConnection.ReadOnly = True
        Me.DataGridViewCheckBoxColumnConnectionEncryptedConnection.ToolTipText = "Protect all data transmitted from the Instance on the wire using an SSL Certifica" & _
            "te installed on the Target Instance"
        Me.DataGridViewCheckBoxColumnConnectionEncryptedConnection.Visible = False
        '
        'DataGridViewCheckBoxColumnConnectionTrustServerCertificate
        '
        Me.DataGridViewCheckBoxColumnConnectionTrustServerCertificate.DataPropertyName = "TrustServerCertificate"
        Me.DataGridViewCheckBoxColumnConnectionTrustServerCertificate.HeaderText = "Trust Server Certificate"
        Me.DataGridViewCheckBoxColumnConnectionTrustServerCertificate.MinimumWidth = 70
        Me.DataGridViewCheckBoxColumnConnectionTrustServerCertificate.Name = "DataGridViewCheckBoxColumnConnectionTrustServerCertificate"
        Me.DataGridViewCheckBoxColumnConnectionTrustServerCertificate.ReadOnly = True
        Me.DataGridViewCheckBoxColumnConnectionTrustServerCertificate.ToolTipText = "Always Trust non-Authorative Certificates provided by the Target Instance. Use fo" & _
            "r self-signed Target Server Certificate. "
        Me.DataGridViewCheckBoxColumnConnectionTrustServerCertificate.Visible = False
        '
        'DataGridViewTextBoxColumnConnectionNetworkProtocol
        '
        Me.DataGridViewTextBoxColumnConnectionNetworkProtocol.DataPropertyName = "NetworkProtocol"
        Me.DataGridViewTextBoxColumnConnectionNetworkProtocol.HeaderText = "Network Protocol"
        Me.DataGridViewTextBoxColumnConnectionNetworkProtocol.MinimumWidth = 80
        Me.DataGridViewTextBoxColumnConnectionNetworkProtocol.Name = "DataGridViewTextBoxColumnConnectionNetworkProtocol"
        Me.DataGridViewTextBoxColumnConnectionNetworkProtocol.ReadOnly = True
        Me.DataGridViewTextBoxColumnConnectionNetworkProtocol.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DataGridViewTextBoxColumnConnectionNetworkProtocol.ToolTipText = "Can be blank. See SQL Server Books Online topic ""Choosing a Network Protocol"" bef" & _
            "ore using any other option. "
        Me.DataGridViewTextBoxColumnConnectionNetworkProtocol.Visible = False
        '
        'DataGridViewTextBoxColumnConnectionConnectionTimeout
        '
        Me.DataGridViewTextBoxColumnConnectionConnectionTimeout.DataPropertyName = "ConnectionTimeout"
        Me.DataGridViewTextBoxColumnConnectionConnectionTimeout.HeaderText = "Connection Timeout"
        Me.DataGridViewTextBoxColumnConnectionConnectionTimeout.MinimumWidth = 70
        Me.DataGridViewTextBoxColumnConnectionConnectionTimeout.Name = "DataGridViewTextBoxColumnConnectionConnectionTimeout"
        Me.DataGridViewTextBoxColumnConnectionConnectionTimeout.ReadOnly = True
        Me.DataGridViewTextBoxColumnConnectionConnectionTimeout.ToolTipText = "The number of seconds to allow the utility to try to establish a connection to th" & _
            "e Target Instance brfore the connection attemp is considered unsuccessful."
        Me.DataGridViewTextBoxColumnConnectionConnectionTimeout.Visible = False
        '
        'DataGridViewCheckBoxColumnConnectionLoginSecure
        '
        Me.DataGridViewCheckBoxColumnConnectionLoginSecure.DataPropertyName = "LoginSecure"
        Me.DataGridViewCheckBoxColumnConnectionLoginSecure.HeaderText = "Login Secure"
        Me.DataGridViewCheckBoxColumnConnectionLoginSecure.MinimumWidth = 70
        Me.DataGridViewCheckBoxColumnConnectionLoginSecure.Name = "DataGridViewCheckBoxColumnConnectionLoginSecure"
        Me.DataGridViewCheckBoxColumnConnectionLoginSecure.ReadOnly = True
        Me.DataGridViewCheckBoxColumnConnectionLoginSecure.ToolTipText = "If true, use the credentials of the logged in Windows Account, otherwise use prov" & _
            "ided SQL Server login and password."
        Me.DataGridViewCheckBoxColumnConnectionLoginSecure.Visible = False
        '
        'BindingSourceConnection
        '
        Me.BindingSourceConnection.DataMember = "tConnection"
        Me.BindingSourceConnection.DataSource = GetType(cCommon.dsSQLConfiguration)
        '
        'smoTreeView
        '
        Me.smoTreeView.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.smoTreeView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.smoTreeView.CheckBoxes = True
        Me.smoTreeView.Location = New System.Drawing.Point(4, 16)
        Me.smoTreeView.Name = "smoTreeView"
        Me.smoTreeView.PathSeparator = "|"
        Me.smoTreeView.ShowNodeToolTips = True
        Me.smoTreeView.Size = New System.Drawing.Size(356, 195)
        Me.smoTreeView.TabIndex = 4
        '
        'SplitContainerVertical
        '
        Me.SplitContainerVertical.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SplitContainerVertical.BackColor = System.Drawing.SystemColors.GradientInactiveCaption
        Me.SplitContainerVertical.Location = New System.Drawing.Point(4, 4)
        Me.SplitContainerVertical.Name = "SplitContainerVertical"
        Me.SplitContainerVertical.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainerVertical.Panel1
        '
        Me.SplitContainerVertical.Panel1.BackColor = System.Drawing.SystemColors.Control
        Me.SplitContainerVertical.Panel1.Controls.Add(Me.SplitContainer1)
        '
        'SplitContainerVertical.Panel2
        '
        Me.SplitContainerVertical.Panel2.BackColor = System.Drawing.SystemColors.Control
        Me.SplitContainerVertical.Panel2.Controls.Add(Me.Panel1)
        Me.SplitContainerVertical.Size = New System.Drawing.Size(722, 372)
        Me.SplitContainerVertical.SplitterDistance = 217
        Me.SplitContainerVertical.TabIndex = 0
        '
        'Panel1
        '
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.DataGridViewArchiveSchedules)
        Me.Panel1.Controls.Add(Me.LabelArchiveSchedule)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(722, 151)
        Me.Panel1.TabIndex = 3
        '
        'DataGridViewArchiveSchedules
        '
        Me.DataGridViewArchiveSchedules.AllowUserToOrderColumns = True
        DataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control
        Me.DataGridViewArchiveSchedules.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle3
        Me.DataGridViewArchiveSchedules.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.DataGridViewArchiveSchedules.AutoGenerateColumns = False
        Me.DataGridViewArchiveSchedules.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.DataGridViewArchiveSchedules.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.BottomCenter
        DataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DataGridViewArchiveSchedules.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle4
        Me.DataGridViewArchiveSchedules.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridViewArchiveSchedules.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.DataGridViewTextBoxColumnArchiveScheduleId, Me.DataGridViewTextBoxColumnArchiveScheduleInstanceName, Me.DataGridViewButtonColumnArchiveScheduleIntervalType, Me.DataGridViewButtonColumnArchiveScheduleInterval, Me.DataGridViewButtonColumnArchiveScheduleIntervalBaseDt, Me.DataGridViewCheckBoxColumnArchiveScheduleIsActive, Me.DataGridViewImageColumnArchiveScheduleRunNow, Me.DataGridViewCheckBoxColumnArchiveScheduleUseEventNotifications, Me.DataGridViewImageColumnArchiveScheduleViewQueue})
        Me.DataGridViewArchiveSchedules.DataSource = Me.BindingSourceArchiveSchedule
        Me.DataGridViewArchiveSchedules.Location = New System.Drawing.Point(4, 16)
        Me.DataGridViewArchiveSchedules.MultiSelect = False
        Me.DataGridViewArchiveSchedules.Name = "DataGridViewArchiveSchedules"
        Me.DataGridViewArchiveSchedules.RowHeadersWidth = 30
        Me.DataGridViewArchiveSchedules.Size = New System.Drawing.Size(712, 129)
        Me.DataGridViewArchiveSchedules.TabIndex = 1
        '
        'DataGridViewTextBoxColumnArchiveScheduleId
        '
        Me.DataGridViewTextBoxColumnArchiveScheduleId.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
        Me.DataGridViewTextBoxColumnArchiveScheduleId.DataPropertyName = "Id"
        DataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        Me.DataGridViewTextBoxColumnArchiveScheduleId.DefaultCellStyle = DataGridViewCellStyle5
        Me.DataGridViewTextBoxColumnArchiveScheduleId.FillWeight = 50.0!
        Me.DataGridViewTextBoxColumnArchiveScheduleId.HeaderText = "Schedule Id"
        Me.DataGridViewTextBoxColumnArchiveScheduleId.MinimumWidth = 60
        Me.DataGridViewTextBoxColumnArchiveScheduleId.Name = "DataGridViewTextBoxColumnArchiveScheduleId"
        Me.DataGridViewTextBoxColumnArchiveScheduleId.ReadOnly = True
        Me.DataGridViewTextBoxColumnArchiveScheduleId.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.DataGridViewTextBoxColumnArchiveScheduleId.Width = 82
        '
        'DataGridViewTextBoxColumnArchiveScheduleInstanceName
        '
        Me.DataGridViewTextBoxColumnArchiveScheduleInstanceName.DataPropertyName = "InstanceName"
        Me.DataGridViewTextBoxColumnArchiveScheduleInstanceName.HeaderText = "Instance Name"
        Me.DataGridViewTextBoxColumnArchiveScheduleInstanceName.MaxInputLength = 128
        Me.DataGridViewTextBoxColumnArchiveScheduleInstanceName.MinimumWidth = 130
        Me.DataGridViewTextBoxColumnArchiveScheduleInstanceName.Name = "DataGridViewTextBoxColumnArchiveScheduleInstanceName"
        Me.DataGridViewTextBoxColumnArchiveScheduleInstanceName.ReadOnly = True
        Me.DataGridViewTextBoxColumnArchiveScheduleInstanceName.Visible = False
        '
        'DataGridViewButtonColumnArchiveScheduleIntervalType
        '
        Me.DataGridViewButtonColumnArchiveScheduleIntervalType.DataPropertyName = "IntervalType"
        DataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText
        Me.DataGridViewButtonColumnArchiveScheduleIntervalType.DefaultCellStyle = DataGridViewCellStyle6
        Me.DataGridViewButtonColumnArchiveScheduleIntervalType.FillWeight = 50.0!
        Me.DataGridViewButtonColumnArchiveScheduleIntervalType.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.DataGridViewButtonColumnArchiveScheduleIntervalType.HeaderText = "Interval Type"
        Me.DataGridViewButtonColumnArchiveScheduleIntervalType.MinimumWidth = 60
        Me.DataGridViewButtonColumnArchiveScheduleIntervalType.Name = "DataGridViewButtonColumnArchiveScheduleIntervalType"
        Me.DataGridViewButtonColumnArchiveScheduleIntervalType.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DataGridViewButtonColumnArchiveScheduleIntervalType.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        Me.DataGridViewButtonColumnArchiveScheduleIntervalType.ToolTipText = "The period or duration of one Interval"
        '
        'DataGridViewButtonColumnArchiveScheduleInterval
        '
        Me.DataGridViewButtonColumnArchiveScheduleInterval.DataPropertyName = "Interval"
        DataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.WindowText
        Me.DataGridViewButtonColumnArchiveScheduleInterval.DefaultCellStyle = DataGridViewCellStyle7
        Me.DataGridViewButtonColumnArchiveScheduleInterval.FillWeight = 50.0!
        Me.DataGridViewButtonColumnArchiveScheduleInterval.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.DataGridViewButtonColumnArchiveScheduleInterval.HeaderText = "Interval"
        Me.DataGridViewButtonColumnArchiveScheduleInterval.MinimumWidth = 60
        Me.DataGridViewButtonColumnArchiveScheduleInterval.Name = "DataGridViewButtonColumnArchiveScheduleInterval"
        Me.DataGridViewButtonColumnArchiveScheduleInterval.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DataGridViewButtonColumnArchiveScheduleInterval.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        Me.DataGridViewButtonColumnArchiveScheduleInterval.ToolTipText = "The number of Interval Type periods to wait after the Interval Base Date before r" & _
            "unning the next configuration archive"
        '
        'DataGridViewButtonColumnArchiveScheduleIntervalBaseDt
        '
        Me.DataGridViewButtonColumnArchiveScheduleIntervalBaseDt.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
        Me.DataGridViewButtonColumnArchiveScheduleIntervalBaseDt.DataPropertyName = "IntervalBaseDt"
        DataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.WindowText
        Me.DataGridViewButtonColumnArchiveScheduleIntervalBaseDt.DefaultCellStyle = DataGridViewCellStyle8
        Me.DataGridViewButtonColumnArchiveScheduleIntervalBaseDt.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.DataGridViewButtonColumnArchiveScheduleIntervalBaseDt.HeaderText = "Next Run Date"
        Me.DataGridViewButtonColumnArchiveScheduleIntervalBaseDt.MinimumWidth = 100
        Me.DataGridViewButtonColumnArchiveScheduleIntervalBaseDt.Name = "DataGridViewButtonColumnArchiveScheduleIntervalBaseDt"
        Me.DataGridViewButtonColumnArchiveScheduleIntervalBaseDt.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DataGridViewButtonColumnArchiveScheduleIntervalBaseDt.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        Me.DataGridViewButtonColumnArchiveScheduleIntervalBaseDt.ToolTipText = "If the next time that this schedule will run."
        '
        'DataGridViewCheckBoxColumnArchiveScheduleIsActive
        '
        Me.DataGridViewCheckBoxColumnArchiveScheduleIsActive.DataPropertyName = "IsActive"
        Me.DataGridViewCheckBoxColumnArchiveScheduleIsActive.FalseValue = "False"
        Me.DataGridViewCheckBoxColumnArchiveScheduleIsActive.FillWeight = 50.0!
        Me.DataGridViewCheckBoxColumnArchiveScheduleIsActive.HeaderText = "Enabled"
        Me.DataGridViewCheckBoxColumnArchiveScheduleIsActive.MinimumWidth = 60
        Me.DataGridViewCheckBoxColumnArchiveScheduleIsActive.Name = "DataGridViewCheckBoxColumnArchiveScheduleIsActive"
        Me.DataGridViewCheckBoxColumnArchiveScheduleIsActive.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        Me.DataGridViewCheckBoxColumnArchiveScheduleIsActive.ToolTipText = "Schedule will be considered by the archive scheduler service only if checked"
        Me.DataGridViewCheckBoxColumnArchiveScheduleIsActive.TrueValue = "True"
        '
        'DataGridViewImageColumnArchiveScheduleRunNow
        '
        Me.DataGridViewImageColumnArchiveScheduleRunNow.FillWeight = 50.0!
        Me.DataGridViewImageColumnArchiveScheduleRunNow.HeaderText = "Run Now"
        Me.DataGridViewImageColumnArchiveScheduleRunNow.Image = CType(resources.GetObject("DataGridViewImageColumnArchiveScheduleRunNow.Image"), System.Drawing.Image)
        Me.DataGridViewImageColumnArchiveScheduleRunNow.MinimumWidth = 60
        Me.DataGridViewImageColumnArchiveScheduleRunNow.Name = "DataGridViewImageColumnArchiveScheduleRunNow"
        Me.DataGridViewImageColumnArchiveScheduleRunNow.ReadOnly = True
        Me.DataGridViewImageColumnArchiveScheduleRunNow.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        '
        'DataGridViewCheckBoxColumnArchiveScheduleUseEventNotifications
        '
        Me.DataGridViewCheckBoxColumnArchiveScheduleUseEventNotifications.DataPropertyName = "UseEventNotifications"
        Me.DataGridViewCheckBoxColumnArchiveScheduleUseEventNotifications.FillWeight = 50.0!
        Me.DataGridViewCheckBoxColumnArchiveScheduleUseEventNotifications.HeaderText = "Use Events"
        Me.DataGridViewCheckBoxColumnArchiveScheduleUseEventNotifications.MinimumWidth = 60
        Me.DataGridViewCheckBoxColumnArchiveScheduleUseEventNotifications.Name = "DataGridViewCheckBoxColumnArchiveScheduleUseEventNotifications"
        Me.DataGridViewCheckBoxColumnArchiveScheduleUseEventNotifications.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        Me.DataGridViewCheckBoxColumnArchiveScheduleUseEventNotifications.ToolTipText = "Improve the performance and audit detail information by using DDL Event notificat" & _
            "ions (SQL Server 2005 and later)"
        '
        'DataGridViewImageColumnArchiveScheduleViewQueue
        '
        Me.DataGridViewImageColumnArchiveScheduleViewQueue.FillWeight = 50.0!
        Me.DataGridViewImageColumnArchiveScheduleViewQueue.HeaderText = "View Queue"
        Me.DataGridViewImageColumnArchiveScheduleViewQueue.MinimumWidth = 60
        Me.DataGridViewImageColumnArchiveScheduleViewQueue.Name = "DataGridViewImageColumnArchiveScheduleViewQueue"
        '
        'BindingSourceArchiveSchedule
        '
        Me.BindingSourceArchiveSchedule.DataMember = "tSchedule"
        Me.BindingSourceArchiveSchedule.DataSource = GetType(cCommon.dsSQLConfiguration)
        Me.BindingSourceArchiveSchedule.Filter = Global.SQLClue.My.Resources.Resources.HelpOverview
        '
        'BottomToolStripPanel
        '
        Me.BottomToolStripPanel.Location = New System.Drawing.Point(0, 0)
        Me.BottomToolStripPanel.Name = "BottomToolStripPanel"
        Me.BottomToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal
        Me.BottomToolStripPanel.RowMargin = New System.Windows.Forms.Padding(3, 0, 0, 0)
        Me.BottomToolStripPanel.Size = New System.Drawing.Size(0, 0)
        '
        'TopToolStripPanel
        '
        Me.TopToolStripPanel.Location = New System.Drawing.Point(0, 0)
        Me.TopToolStripPanel.Name = "TopToolStripPanel"
        Me.TopToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal
        Me.TopToolStripPanel.RowMargin = New System.Windows.Forms.Padding(3, 0, 0, 0)
        Me.TopToolStripPanel.Size = New System.Drawing.Size(0, 0)
        '
        'RightToolStripPanel
        '
        Me.RightToolStripPanel.Location = New System.Drawing.Point(0, 0)
        Me.RightToolStripPanel.Name = "RightToolStripPanel"
        Me.RightToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal
        Me.RightToolStripPanel.RowMargin = New System.Windows.Forms.Padding(3, 0, 0, 0)
        Me.RightToolStripPanel.Size = New System.Drawing.Size(0, 0)
        '
        'LeftToolStripPanel
        '
        Me.LeftToolStripPanel.Location = New System.Drawing.Point(0, 0)
        Me.LeftToolStripPanel.Name = "LeftToolStripPanel"
        Me.LeftToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal
        Me.LeftToolStripPanel.RowMargin = New System.Windows.Forms.Padding(3, 0, 0, 0)
        Me.LeftToolStripPanel.Size = New System.Drawing.Size(0, 0)
        '
        'ContentPanel
        '
        Me.ContentPanel.AutoScroll = True
        Me.ContentPanel.BackColor = System.Drawing.SystemColors.Window
        Me.ContentPanel.Size = New System.Drawing.Size(700, 341)
        '
        'ToolStripContainer1
        '
        '
        'ToolStripContainer1.BottomToolStripPanel
        '
        Me.ToolStripContainer1.BottomToolStripPanel.BackColor = System.Drawing.SystemColors.Window
        Me.ToolStripContainer1.BottomToolStripPanelVisible = False
        '
        'ToolStripContainer1.ContentPanel
        '
        Me.ToolStripContainer1.ContentPanel.AutoScroll = True
        Me.ToolStripContainer1.ContentPanel.BackColor = System.Drawing.SystemColors.Window
        Me.ToolStripContainer1.ContentPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.ToolStripContainer1.ContentPanel.Controls.Add(Me.ButtonCancelArchive)
        Me.ToolStripContainer1.ContentPanel.Controls.Add(Me.SplitContainer2)
        Me.ToolStripContainer1.ContentPanel.Controls.Add(Me.SplitContainerVertical)
        Me.ToolStripContainer1.ContentPanel.Controls.Add(Me.StatusStripPlanning)
        Me.ToolStripContainer1.ContentPanel.Size = New System.Drawing.Size(730, 430)
        Me.ToolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ToolStripContainer1.LeftToolStripPanelVisible = False
        Me.ToolStripContainer1.Location = New System.Drawing.Point(0, 0)
        Me.ToolStripContainer1.Name = "ToolStripContainer1"
        Me.ToolStripContainer1.RightToolStripPanelVisible = False
        Me.ToolStripContainer1.Size = New System.Drawing.Size(730, 430)
        Me.ToolStripContainer1.TabIndex = 24
        Me.ToolStripContainer1.Text = "ToolStripContainer1"
        Me.ToolStripContainer1.TopToolStripPanelVisible = False
        '
        'ButtonCancelArchive
        '
        Me.ButtonCancelArchive.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ButtonCancelArchive.BackColor = System.Drawing.SystemColors.HotTrack
        Me.ButtonCancelArchive.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.ButtonCancelArchive.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.5!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonCancelArchive.ForeColor = System.Drawing.SystemColors.HighlightText
        Me.ButtonCancelArchive.Location = New System.Drawing.Point(614, 380)
        Me.ButtonCancelArchive.Name = "ButtonCancelArchive"
        Me.ButtonCancelArchive.Size = New System.Drawing.Size(102, 22)
        Me.ButtonCancelArchive.TabIndex = 11
        Me.ButtonCancelArchive.Text = "Cancel Archive"
        Me.ButtonCancelArchive.UseVisualStyleBackColor = False
        '
        'SplitContainer2
        '
        Me.SplitContainer2.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SplitContainer2.IsSplitterFixed = True
        Me.SplitContainer2.Location = New System.Drawing.Point(528, 380)
        Me.SplitContainer2.Name = "SplitContainer2"
        '
        'SplitContainer2.Panel1
        '
        Me.SplitContainer2.Panel1.Controls.Add(Me.ButtonSave)
        '
        'SplitContainer2.Panel2
        '
        Me.SplitContainer2.Panel2.Controls.Add(Me.ButtonCancel)
        Me.SplitContainer2.Size = New System.Drawing.Size(194, 22)
        Me.SplitContainer2.SplitterDistance = 95
        Me.SplitContainer2.TabIndex = 25
        '
        'ButtonSave
        '
        Me.ButtonSave.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ButtonSave.Location = New System.Drawing.Point(0, 0)
        Me.ButtonSave.Name = "ButtonSave"
        Me.ButtonSave.Size = New System.Drawing.Size(95, 22)
        Me.ButtonSave.TabIndex = 20
        Me.ButtonSave.Text = "Save"
        Me.ButtonSave.UseVisualStyleBackColor = True
        '
        'StatusStripPlanning
        '
        Me.StatusStripPlanning.BackColor = System.Drawing.SystemColors.Control
        Me.StatusStripPlanning.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.StatusStripPlanning.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripStatusLabelConfiguration, Me.ToolStripProgressBarArchive})
        Me.StatusStripPlanning.Location = New System.Drawing.Point(0, 405)
        Me.StatusStripPlanning.Name = "StatusStripPlanning"
        Me.StatusStripPlanning.Size = New System.Drawing.Size(730, 25)
        Me.StatusStripPlanning.TabIndex = 23
        Me.StatusStripPlanning.Text = "StatusStrip"
        '
        'ToolStripStatusLabelConfiguration
        '
        Me.ToolStripStatusLabelConfiguration.ForeColor = System.Drawing.SystemColors.ControlText
        Me.ToolStripStatusLabelConfiguration.Name = "ToolStripStatusLabelConfiguration"
        Me.ToolStripStatusLabelConfiguration.Size = New System.Drawing.Size(582, 20)
        Me.ToolStripStatusLabelConfiguration.Spring = True
        Me.ToolStripStatusLabelConfiguration.Text = "ToolStripStatusLabelConfiguration"
        Me.ToolStripStatusLabelConfiguration.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ToolStripProgressBarArchive
        '
        Me.ToolStripProgressBarArchive.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.ToolStripProgressBarArchive.Name = "ToolStripProgressBarArchive"
        Me.ToolStripProgressBarArchive.Size = New System.Drawing.Size(100, 19)
        '
        'ContextMenuStripTreeNode
        '
        Me.ContextMenuStripTreeNode.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItemLabelItem})
        Me.ContextMenuStripTreeNode.Name = "ContextMenuStripTreeNode"
        Me.ContextMenuStripTreeNode.ShowImageMargin = False
        Me.ContextMenuStripTreeNode.Size = New System.Drawing.Size(87, 26)
        '
        'ToolStripMenuItemLabelItem
        '
        Me.ToolStripMenuItemLabelItem.Name = "ToolStripMenuItemLabelItem"
        Me.ToolStripMenuItemLabelItem.Size = New System.Drawing.Size(86, 22)
        Me.ToolStripMenuItemLabelItem.Text = "Label..."
        '
        'ConfigurationForm
        '
        Me.AcceptButton = Me.ButtonSave
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Window
        Me.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.CancelButton = Me.ButtonCancel
        Me.ClientSize = New System.Drawing.Size(730, 430)
        Me.Controls.Add(Me.ToolStripContainer1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MinimumSize = New System.Drawing.Size(350, 167)
        Me.Name = "ConfigurationForm"
        Me.Text = "Archive Planning"
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        Me.SplitContainer1.ResumeLayout(False)
        CType(Me.DataGridViewConnections, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BindingSourceConnection, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainerVertical.Panel1.ResumeLayout(False)
        Me.SplitContainerVertical.Panel2.ResumeLayout(False)
        Me.SplitContainerVertical.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        CType(Me.DataGridViewArchiveSchedules, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BindingSourceArchiveSchedule, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ToolStripContainer1.ContentPanel.ResumeLayout(False)
        Me.ToolStripContainer1.ContentPanel.PerformLayout()
        Me.ToolStripContainer1.ResumeLayout(False)
        Me.ToolStripContainer1.PerformLayout()
        Me.SplitContainer2.Panel1.ResumeLayout(False)
        Me.SplitContainer2.Panel2.ResumeLayout(False)
        Me.SplitContainer2.ResumeLayout(False)
        Me.StatusStripPlanning.ResumeLayout(False)
        Me.StatusStripPlanning.PerformLayout()
        Me.ContextMenuStripTreeNode.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents BindingSourceConnection As System.Windows.Forms.BindingSource
    Friend WithEvents BindingSourceArchiveSchedule As System.Windows.Forms.BindingSource
    Friend WithEvents ToolTipPlanning As System.Windows.Forms.ToolTip
    Friend WithEvents ToolStripContainer1 As System.Windows.Forms.ToolStripContainer
    Friend WithEvents ButtonSave As System.Windows.Forms.Button
    Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
    Friend WithEvents SplitContainerVertical As System.Windows.Forms.SplitContainer
    Friend WithEvents DataGridViewConnections As System.Windows.Forms.DataGridView
    Friend WithEvents LabelArchiveSchedule As System.Windows.Forms.Label
    Friend WithEvents DataGridViewArchiveSchedules As System.Windows.Forms.DataGridView
    Friend WithEvents LabelConfigfuration As System.Windows.Forms.Label
    Friend WithEvents smoTreeView As System.Windows.Forms.TreeView
    Friend WithEvents ButtonCancelArchive As System.Windows.Forms.Button
    Friend WithEvents ButtonCancel As System.Windows.Forms.Button
    Friend WithEvents BottomToolStripPanel As System.Windows.Forms.ToolStripPanel
    Friend WithEvents TopToolStripPanel As System.Windows.Forms.ToolStripPanel
    Friend WithEvents RightToolStripPanel As System.Windows.Forms.ToolStripPanel
    Friend WithEvents LeftToolStripPanel As System.Windows.Forms.ToolStripPanel
    Friend WithEvents ContentPanel As System.Windows.Forms.ToolStripContentPanel
    Friend WithEvents ContextMenuStripTreeNode As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents ToolStripMenuItemLabelItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SplitContainer2 As System.Windows.Forms.SplitContainer
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents StatusStripPlanning As System.Windows.Forms.StatusStrip
    Friend WithEvents ToolStripStatusLabelConfiguration As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents ToolStripProgressBarArchive As System.Windows.Forms.ToolStripProgressBar
    Friend WithEvents DataGridViewTextBoxColumnArchiveScheduleId As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumnArchiveScheduleInstanceName As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewButtonColumnArchiveScheduleIntervalType As System.Windows.Forms.DataGridViewButtonColumn
    Friend WithEvents DataGridViewButtonColumnArchiveScheduleInterval As System.Windows.Forms.DataGridViewButtonColumn
    Friend WithEvents DataGridViewButtonColumnArchiveScheduleIntervalBaseDt As System.Windows.Forms.DataGridViewButtonColumn
    Friend WithEvents DataGridViewCheckBoxColumnArchiveScheduleIsActive As System.Windows.Forms.DataGridViewCheckBoxColumn
    Friend WithEvents DataGridViewImageColumnArchiveScheduleRunNow As System.Windows.Forms.DataGridViewImageColumn
    Friend WithEvents DataGridViewCheckBoxColumnArchiveScheduleUseEventNotifications As System.Windows.Forms.DataGridViewCheckBoxColumn
    Friend WithEvents DataGridViewImageColumnArchiveScheduleViewQueue As System.Windows.Forms.DataGridViewImageColumn
    Friend WithEvents DataGridViewButtonColumnConnectionInstanceName As System.Windows.Forms.DataGridViewButtonColumn
    Friend WithEvents DataGridViewCheckBoxColumnConnectionEncryptedConnection As System.Windows.Forms.DataGridViewCheckBoxColumn
    Friend WithEvents DataGridViewCheckBoxColumnConnectionTrustServerCertificate As System.Windows.Forms.DataGridViewCheckBoxColumn
    Friend WithEvents DataGridViewTextBoxColumnConnectionNetworkProtocol As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumnConnectionConnectionTimeout As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewCheckBoxColumnConnectionLoginSecure As System.Windows.Forms.DataGridViewCheckBoxColumn
End Class
