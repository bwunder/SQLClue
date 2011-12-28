<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class DialogConnect
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(DialogConnect))
        Me.ConnectGroupBox = New System.Windows.Forms.GroupBox
        Me.GroupBox5 = New System.Windows.Forms.GroupBox
        Me.Password = New System.Windows.Forms.TextBox
        Me.UseTrusted = New System.Windows.Forms.CheckBox
        Me.SQLLogin = New System.Windows.Forms.TextBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.InstanceLabel = New System.Windows.Forms.Label
        Me.InstanceName = New System.Windows.Forms.ComboBox
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.UseEncryptedConnection = New System.Windows.Forms.CheckBox
        Me.ConnectionTimeout = New System.Windows.Forms.NumericUpDown
        Me.TrustServerCertificate = New System.Windows.Forms.CheckBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.NetProtocol = New System.Windows.Forms.ComboBox
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel
        Me.TableLayoutPanel2 = New System.Windows.Forms.TableLayoutPanel
        Me.OK_Button = New System.Windows.Forms.Button
        Me.ButtonCancel = New System.Windows.Forms.Button
        Me.ToolTipConnect = New System.Windows.Forms.ToolTip(Me.components)
        Me.Label10 = New System.Windows.Forms.Label
        Me.Label7 = New System.Windows.Forms.Label
        Me.Label5 = New System.Windows.Forms.Label
        Me.Label6 = New System.Windows.Forms.Label
        Me.TargetEncryptConnection = New System.Windows.Forms.CheckBox
        Me.TargetTrustServerCertificate = New System.Windows.Forms.CheckBox
        Me.Label9 = New System.Windows.Forms.Label
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.TargetDatabaseName = New System.Windows.Forms.ComboBox
        Me.TargetInstanceName = New System.Windows.Forms.ComboBox
        Me.Panel3 = New System.Windows.Forms.Panel
        Me.TargetConnectionTimeout = New System.Windows.Forms.NumericUpDown
        Me.Label8 = New System.Windows.Forms.Label
        Me.TargetNetworkProtocol = New System.Windows.Forms.ComboBox
        Me.Label12 = New System.Windows.Forms.Label
        Me.Panel2 = New System.Windows.Forms.Panel
        Me.TargetSQLLoginPassword = New System.Windows.Forms.TextBox
        Me.TargetUseTrustedConnection = New System.Windows.Forms.CheckBox
        Me.TargetSQLLoginName = New System.Windows.Forms.TextBox
        Me.Label11 = New System.Windows.Forms.Label
        Me.ConnectGroupBox.SuspendLayout()
        Me.GroupBox5.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        CType(Me.ConnectionTimeout, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TableLayoutPanel2.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.Panel3.SuspendLayout()
        CType(Me.TargetConnectionTimeout, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel2.SuspendLayout()
        Me.SuspendLayout()
        '
        'ConnectGroupBox
        '
        Me.ConnectGroupBox.Controls.Add(Me.GroupBox5)
        Me.ConnectGroupBox.Controls.Add(Me.InstanceLabel)
        Me.ConnectGroupBox.Controls.Add(Me.InstanceName)
        Me.ConnectGroupBox.Controls.Add(Me.GroupBox1)
        Me.ConnectGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.ConnectGroupBox.Location = New System.Drawing.Point(3, -40)
        Me.ConnectGroupBox.Name = "ConnectGroupBox"
        Me.ConnectGroupBox.Size = New System.Drawing.Size(287, 301)
        Me.ConnectGroupBox.TabIndex = 13
        Me.ConnectGroupBox.TabStop = False
        '
        'GroupBox5
        '
        Me.GroupBox5.Controls.Add(Me.Password)
        Me.GroupBox5.Controls.Add(Me.UseTrusted)
        Me.GroupBox5.Controls.Add(Me.SQLLogin)
        Me.GroupBox5.Controls.Add(Me.Label2)
        Me.GroupBox5.Controls.Add(Me.Label1)
        Me.GroupBox5.ImeMode = System.Windows.Forms.ImeMode.[On]
        Me.GroupBox5.Location = New System.Drawing.Point(10, 59)
        Me.GroupBox5.Name = "GroupBox5"
        Me.GroupBox5.Size = New System.Drawing.Size(267, 93)
        Me.GroupBox5.TabIndex = 16
        Me.GroupBox5.TabStop = False
        Me.GroupBox5.Text = "Authentication"
        '
        'Password
        '
        Me.Password.Enabled = False
        Me.Password.Location = New System.Drawing.Point(84, 61)
        Me.Password.Name = "Password"
        Me.Password.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.Password.Size = New System.Drawing.Size(171, 20)
        Me.Password.TabIndex = 5
        Me.Password.TabStop = False
        '
        'UseTrusted
        '
        Me.UseTrusted.AutoSize = True
        Me.UseTrusted.Checked = True
        Me.UseTrusted.CheckState = System.Windows.Forms.CheckState.Checked
        Me.UseTrusted.ImeMode = System.Windows.Forms.ImeMode.[On]
        Me.UseTrusted.Location = New System.Drawing.Point(10, 400)
        Me.UseTrusted.Name = "UseTrusted"
        Me.UseTrusted.Size = New System.Drawing.Size(163, 17)
        Me.UseTrusted.TabIndex = 3
        Me.UseTrusted.Text = "Use Windows Authentication"
        Me.UseTrusted.UseVisualStyleBackColor = True
        '
        'SQLLogin
        '
        Me.SQLLogin.Enabled = False
        Me.SQLLogin.Location = New System.Drawing.Point(84, 35)
        Me.SQLLogin.Name = "SQLLogin"
        Me.SQLLogin.Size = New System.Drawing.Size(170, 20)
        Me.SQLLogin.TabIndex = 4
        Me.SQLLogin.TabStop = False
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.ImeMode = System.Windows.Forms.ImeMode.[On]
        Me.Label2.Location = New System.Drawing.Point(13, 61)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(53, 13)
        Me.Label2.TabIndex = 18
        Me.Label2.Text = "Password"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.ImeMode = System.Windows.Forms.ImeMode.[On]
        Me.Label1.Location = New System.Drawing.Point(13, 35)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(57, 13)
        Me.Label1.TabIndex = 17
        Me.Label1.Text = "SQL Login"
        '
        'InstanceLabel
        '
        Me.InstanceLabel.AutoSize = True
        Me.InstanceLabel.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.InstanceLabel.Location = New System.Drawing.Point(7, 26)
        Me.InstanceLabel.Name = "InstanceLabel"
        Me.InstanceLabel.Size = New System.Drawing.Size(79, 13)
        Me.InstanceLabel.TabIndex = 11
        Me.InstanceLabel.Text = "Instance Name"
        '
        'InstanceName
        '
        Me.InstanceName.FormattingEnabled = True
        Me.InstanceName.Location = New System.Drawing.Point(94, 24)
        Me.InstanceName.Name = "InstanceName"
        Me.InstanceName.Size = New System.Drawing.Size(183, 21)
        Me.InstanceName.Sorted = True
        Me.InstanceName.TabIndex = 0
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.UseEncryptedConnection)
        Me.GroupBox1.Controls.Add(Me.ConnectionTimeout)
        Me.GroupBox1.Controls.Add(Me.TrustServerCertificate)
        Me.GroupBox1.Controls.Add(Me.Label3)
        Me.GroupBox1.Controls.Add(Me.Label4)
        Me.GroupBox1.Controls.Add(Me.NetProtocol)
        Me.GroupBox1.Location = New System.Drawing.Point(10, 158)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(267, 127)
        Me.GroupBox1.TabIndex = 23
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Connection Settings"
        '
        'UseEncryptedConnection
        '
        Me.UseEncryptedConnection.AutoSize = True
        Me.UseEncryptedConnection.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.UseEncryptedConnection.Location = New System.Drawing.Point(100, 100)
        Me.UseEncryptedConnection.Name = "UseEncryptedConnection"
        Me.UseEncryptedConnection.Size = New System.Drawing.Size(153, 17)
        Me.UseEncryptedConnection.TabIndex = 7
        Me.UseEncryptedConnection.Text = "Use Encrypted Connection"
        Me.UseEncryptedConnection.UseVisualStyleBackColor = True
        '
        'ConnectionTimeout
        '
        Me.ConnectionTimeout.Location = New System.Drawing.Point(197, 20)
        Me.ConnectionTimeout.Maximum = New Decimal(New Integer() {600, 0, 0, 0})
        Me.ConnectionTimeout.Name = "ConnectionTimeout"
        Me.ConnectionTimeout.Size = New System.Drawing.Size(58, 20)
        Me.ConnectionTimeout.TabIndex = 6
        Me.ConnectionTimeout.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.ConnectionTimeout.Value = New Decimal(New Integer() {20, 0, 0, 0})
        '
        'TrustServerCertificate
        '
        Me.TrustServerCertificate.AutoSize = True
        Me.TrustServerCertificate.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.TrustServerCertificate.Location = New System.Drawing.Point(35, 68)
        Me.TrustServerCertificate.Name = "TrustServerCertificate"
        Me.TrustServerCertificate.Size = New System.Drawing.Size(134, 17)
        Me.TrustServerCertificate.TabIndex = 8
        Me.TrustServerCertificate.Text = "Trust Server Certificate"
        Me.TrustServerCertificate.UseVisualStyleBackColor = True
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Label3.Location = New System.Drawing.Point(13, 22)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(151, 13)
        Me.Label3.TabIndex = 17
        Me.Label3.Text = "Connection Timeout (seconds)"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Label4.Location = New System.Drawing.Point(9, 94)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(89, 13)
        Me.Label4.TabIndex = 22
        Me.Label4.Text = "Network Protocol"
        '
        'NetProtocol
        '
        Me.NetProtocol.FormattingEnabled = True
        Me.NetProtocol.Location = New System.Drawing.Point(104, 91)
        Me.NetProtocol.Name = "NetProtocol"
        Me.NetProtocol.Size = New System.Drawing.Size(150, 21)
        Me.NetProtocol.TabIndex = 9
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TableLayoutPanel1.AutoSize = True
        Me.TableLayoutPanel1.ColumnCount = 1
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(1, 1)
        Me.TableLayoutPanel1.MaximumSize = New System.Drawing.Size(50, 50)
        Me.TableLayoutPanel1.MinimumSize = New System.Drawing.Size(50, 50)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 1
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(50, 50)
        Me.TableLayoutPanel1.TabIndex = 0
        '
        'TableLayoutPanel2
        '
        Me.TableLayoutPanel2.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TableLayoutPanel2.ColumnCount = 2
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.68493!))
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 49.31507!))
        Me.TableLayoutPanel2.Controls.Add(Me.OK_Button, 0, 0)
        Me.TableLayoutPanel2.Controls.Add(Me.ButtonCancel, 1, 0)
        Me.TableLayoutPanel2.Location = New System.Drawing.Point(114, 275)
        Me.TableLayoutPanel2.Margin = New System.Windows.Forms.Padding(0)
        Me.TableLayoutPanel2.Name = "TableLayoutPanel2"
        Me.TableLayoutPanel2.Padding = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.TableLayoutPanel2.RowCount = 1
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel2.Size = New System.Drawing.Size(194, 24)
        Me.TableLayoutPanel2.TabIndex = 12
        '
        'OK_Button
        '
        Me.OK_Button.Dock = System.Windows.Forms.DockStyle.Fill
        Me.OK_Button.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.OK_Button.Location = New System.Drawing.Point(2, 0)
        Me.OK_Button.Margin = New System.Windows.Forms.Padding(0)
        Me.OK_Button.Name = "OK_Button"
        Me.OK_Button.Size = New System.Drawing.Size(96, 24)
        Me.OK_Button.TabIndex = 1
        Me.OK_Button.Text = "OK"
        '
        'ButtonCancel
        '
        Me.ButtonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.ButtonCancel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ButtonCancel.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.ButtonCancel.Location = New System.Drawing.Point(98, 0)
        Me.ButtonCancel.Margin = New System.Windows.Forms.Padding(0)
        Me.ButtonCancel.Name = "ButtonCancel"
        Me.ButtonCancel.Size = New System.Drawing.Size(94, 24)
        Me.ButtonCancel.TabIndex = 2
        Me.ButtonCancel.Text = "Cancel"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(4, 33)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(99, 13)
        Me.Label10.TabIndex = 27
        Me.Label10.Text = "Database (optional)"
        Me.ToolTipConnect.SetToolTip(Me.Label10, "Blank to use login default db.")
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Label7.Location = New System.Drawing.Point(4, 8)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(72, 13)
        Me.Label7.TabIndex = 26
        Me.Label7.Text = "SQL Instance"
        Me.ToolTipConnect.SetToolTip(Me.Label7, "Must be a local instance. Once installed, the SQL Instance cannot be changed. ")
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.ImeMode = System.Windows.Forms.ImeMode.[On]
        Me.Label5.Location = New System.Drawing.Point(9, 66)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(53, 13)
        Me.Label5.TabIndex = 23
        Me.Label5.Text = "Password"
        Me.ToolTipConnect.SetToolTip(Me.Label5, "Will be stored in clear text on this computer.")
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.ImeMode = System.Windows.Forms.ImeMode.[On]
        Me.Label6.Location = New System.Drawing.Point(9, 41)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(57, 13)
        Me.Label6.TabIndex = 22
        Me.Label6.Text = "SQL Login"
        Me.ToolTipConnect.SetToolTip(Me.Label6, "Login must exists and have the ability to create and own a database on the SQL In" & _
                "stance.")
        '
        'TargetEncryptConnection
        '
        Me.TargetEncryptConnection.AutoSize = True
        Me.TargetEncryptConnection.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.TargetEncryptConnection.Location = New System.Drawing.Point(11, 63)
        Me.TargetEncryptConnection.Name = "TargetEncryptConnection"
        Me.TargetEncryptConnection.Size = New System.Drawing.Size(119, 17)
        Me.TargetEncryptConnection.TabIndex = 24
        Me.TargetEncryptConnection.Text = "Encrypt Connection"
        Me.ToolTipConnect.SetToolTip(Me.TargetEncryptConnection, "Encrypt data on the wire. Requires" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & " a locally installed SSL Certificate or Trust" & _
                " server Certificate ro be enable")
        Me.TargetEncryptConnection.UseVisualStyleBackColor = True
        '
        'TargetTrustServerCertificate
        '
        Me.TargetTrustServerCertificate.AutoSize = True
        Me.TargetTrustServerCertificate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!)
        Me.TargetTrustServerCertificate.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.TargetTrustServerCertificate.Location = New System.Drawing.Point(30, 84)
        Me.TargetTrustServerCertificate.Name = "TargetTrustServerCertificate"
        Me.TargetTrustServerCertificate.Size = New System.Drawing.Size(134, 17)
        Me.TargetTrustServerCertificate.TabIndex = 25
        Me.TargetTrustServerCertificate.Text = "Trust Server Certificate"
        Me.ToolTipConnect.SetToolTip(Me.TargetTrustServerCertificate, "Allow use of self-generated certificate to encrypt " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "data on the wire if SSL Cert" & _
                "ificate is not present. ")
        Me.TargetTrustServerCertificate.UseVisualStyleBackColor = True
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Label9.Location = New System.Drawing.Point(7, 41)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(127, 13)
        Me.Label9.TabIndex = 28
        Me.Label9.Text = "Network Library (optional)"
        Me.ToolTipConnect.SetToolTip(Me.Label9, "Blank to use SQLClient default")
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.SystemColors.Control
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.TargetDatabaseName)
        Me.Panel1.Controls.Add(Me.Label10)
        Me.Panel1.Controls.Add(Me.Label7)
        Me.Panel1.Controls.Add(Me.TargetInstanceName)
        Me.Panel1.Controls.Add(Me.Panel3)
        Me.Panel1.Controls.Add(Me.Panel2)
        Me.Panel1.Location = New System.Drawing.Point(4, 4)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(306, 267)
        Me.Panel1.TabIndex = 14
        '
        'TargetDatabaseName
        '
        Me.TargetDatabaseName.FormattingEnabled = True
        Me.TargetDatabaseName.Location = New System.Drawing.Point(107, 31)
        Me.TargetDatabaseName.Name = "TargetDatabaseName"
        Me.TargetDatabaseName.Size = New System.Drawing.Size(189, 21)
        Me.TargetDatabaseName.Sorted = True
        Me.TargetDatabaseName.TabIndex = 28
        '
        'TargetInstanceName
        '
        Me.TargetInstanceName.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.TargetInstanceName.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.TargetInstanceName.FormattingEnabled = True
        Me.TargetInstanceName.Location = New System.Drawing.Point(107, 5)
        Me.TargetInstanceName.Name = "TargetInstanceName"
        Me.TargetInstanceName.Size = New System.Drawing.Size(189, 21)
        Me.TargetInstanceName.Sorted = True
        Me.TargetInstanceName.TabIndex = 25
        '
        'Panel3
        '
        Me.Panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel3.Controls.Add(Me.TargetEncryptConnection)
        Me.Panel3.Controls.Add(Me.TargetConnectionTimeout)
        Me.Panel3.Controls.Add(Me.TargetTrustServerCertificate)
        Me.Panel3.Controls.Add(Me.Label8)
        Me.Panel3.Controls.Add(Me.Label9)
        Me.Panel3.Controls.Add(Me.TargetNetworkProtocol)
        Me.Panel3.Controls.Add(Me.Label12)
        Me.Panel3.Location = New System.Drawing.Point(4, 152)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(296, 109)
        Me.Panel3.TabIndex = 1
        '
        'TargetConnectionTimeout
        '
        Me.TargetConnectionTimeout.Location = New System.Drawing.Point(226, 15)
        Me.TargetConnectionTimeout.Maximum = New Decimal(New Integer() {600, 0, 0, 0})
        Me.TargetConnectionTimeout.Name = "TargetConnectionTimeout"
        Me.TargetConnectionTimeout.Size = New System.Drawing.Size(64, 20)
        Me.TargetConnectionTimeout.TabIndex = 23
        Me.TargetConnectionTimeout.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.TargetConnectionTimeout.Value = New Decimal(New Integer() {20, 0, 0, 0})
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Label8.Location = New System.Drawing.Point(7, 18)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(151, 13)
        Me.Label8.TabIndex = 27
        Me.Label8.Text = "Connection Timeout (seconds)"
        '
        'TargetNetworkProtocol
        '
        Me.TargetNetworkProtocol.FormattingEnabled = True
        Me.TargetNetworkProtocol.Location = New System.Drawing.Point(137, 39)
        Me.TargetNetworkProtocol.Name = "TargetNetworkProtocol"
        Me.TargetNetworkProtocol.Size = New System.Drawing.Size(153, 21)
        Me.TargetNetworkProtocol.TabIndex = 26
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label12.Location = New System.Drawing.Point(0, 0)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(102, 13)
        Me.Label12.TabIndex = 1
        Me.Label12.Text = "Connection Settings"
        '
        'Panel2
        '
        Me.Panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel2.Controls.Add(Me.TargetSQLLoginPassword)
        Me.Panel2.Controls.Add(Me.TargetUseTrustedConnection)
        Me.Panel2.Controls.Add(Me.TargetSQLLoginName)
        Me.Panel2.Controls.Add(Me.Label5)
        Me.Panel2.Controls.Add(Me.Label6)
        Me.Panel2.Controls.Add(Me.Label11)
        Me.Panel2.Location = New System.Drawing.Point(4, 57)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(296, 91)
        Me.Panel2.TabIndex = 0
        '
        'TargetSQLLoginPassword
        '
        Me.TargetSQLLoginPassword.Enabled = False
        Me.TargetSQLLoginPassword.Location = New System.Drawing.Point(85, 63)
        Me.TargetSQLLoginPassword.Name = "TargetSQLLoginPassword"
        Me.TargetSQLLoginPassword.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.TargetSQLLoginPassword.Size = New System.Drawing.Size(206, 20)
        Me.TargetSQLLoginPassword.TabIndex = 21
        Me.TargetSQLLoginPassword.TabStop = False
        '
        'TargetUseTrustedConnection
        '
        Me.TargetUseTrustedConnection.AutoSize = True
        Me.TargetUseTrustedConnection.Checked = True
        Me.TargetUseTrustedConnection.CheckState = System.Windows.Forms.CheckState.Checked
        Me.TargetUseTrustedConnection.ImeMode = System.Windows.Forms.ImeMode.[On]
        Me.TargetUseTrustedConnection.Location = New System.Drawing.Point(12, 17)
        Me.TargetUseTrustedConnection.Name = "TargetUseTrustedConnection"
        Me.TargetUseTrustedConnection.Size = New System.Drawing.Size(166, 17)
        Me.TargetUseTrustedConnection.TabIndex = 19
        Me.TargetUseTrustedConnection.Text = "Use Windows Authentication "
        Me.TargetUseTrustedConnection.UseVisualStyleBackColor = True
        '
        'TargetSQLLoginName
        '
        Me.TargetSQLLoginName.Enabled = False
        Me.TargetSQLLoginName.Location = New System.Drawing.Point(85, 38)
        Me.TargetSQLLoginName.Name = "TargetSQLLoginName"
        Me.TargetSQLLoginName.Size = New System.Drawing.Size(206, 20)
        Me.TargetSQLLoginName.TabIndex = 20
        Me.TargetSQLLoginName.TabStop = False
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label11.Location = New System.Drawing.Point(0, 0)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(75, 13)
        Me.Label11.TabIndex = 0
        Me.Label11.Text = "Authentication"
        '
        'DialogConnect
        '
        Me.AcceptButton = Me.OK_Button
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.CancelButton = Me.ButtonCancel
        Me.ClientSize = New System.Drawing.Size(314, 304)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.TableLayoutPanel2)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MaximumSize = New System.Drawing.Size(330, 338)
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(330, 338)
        Me.Name = "DialogConnect"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "assigned in code"
        Me.ConnectGroupBox.ResumeLayout(False)
        Me.ConnectGroupBox.PerformLayout()
        Me.GroupBox5.ResumeLayout(False)
        Me.GroupBox5.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        CType(Me.ConnectionTimeout, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TableLayoutPanel2.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.Panel3.ResumeLayout(False)
        Me.Panel3.PerformLayout()
        CType(Me.TargetConnectionTimeout, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ConnectGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBox5 As System.Windows.Forms.GroupBox
    Friend WithEvents Password As System.Windows.Forms.TextBox
    Friend WithEvents UseTrusted As System.Windows.Forms.CheckBox
    Friend WithEvents SQLLogin As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents InstanceLabel As System.Windows.Forms.Label
    Friend WithEvents InstanceName As System.Windows.Forms.ComboBox
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents UseEncryptedConnection As System.Windows.Forms.CheckBox
    Friend WithEvents ConnectionTimeout As System.Windows.Forms.NumericUpDown
    Friend WithEvents TrustServerCertificate As System.Windows.Forms.CheckBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents NetProtocol As System.Windows.Forms.ComboBox
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents TableLayoutPanel2 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents OK_Button As System.Windows.Forms.Button
    Friend WithEvents ButtonCancel As System.Windows.Forms.Button
    Friend WithEvents ToolTipConnect As System.Windows.Forms.ToolTip
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents TargetDatabaseName As System.Windows.Forms.ComboBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents TargetInstanceName As System.Windows.Forms.ComboBox
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents TargetEncryptConnection As System.Windows.Forms.CheckBox
    Friend WithEvents TargetConnectionTimeout As System.Windows.Forms.NumericUpDown
    Friend WithEvents TargetTrustServerCertificate As System.Windows.Forms.CheckBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents TargetNetworkProtocol As System.Windows.Forms.ComboBox
    Friend WithEvents TargetSQLLoginPassword As System.Windows.Forms.TextBox
    Friend WithEvents TargetUseTrustedConnection As System.Windows.Forms.CheckBox
    Friend WithEvents TargetSQLLoginName As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
End Class
