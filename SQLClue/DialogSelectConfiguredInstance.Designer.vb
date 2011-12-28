<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class DialogSelectConfiguredInstance
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(DialogSelectConfiguredInstance))
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel
        Me.OK_Button = New System.Windows.Forms.Button
        Me.Cancel_Button = New System.Windows.Forms.Button
        Me.TSQLCfgConnectionBindingNavigator = New System.Windows.Forms.BindingNavigator(Me.components)
        Me.TSQLCfgConnectionBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.BindingNavigatorCountItem = New System.Windows.Forms.ToolStripLabel
        Me.BindingNavigatorMoveFirstItem = New System.Windows.Forms.ToolStripButton
        Me.BindingNavigatorMovePreviousItem = New System.Windows.Forms.ToolStripButton
        Me.BindingNavigatorSeparator = New System.Windows.Forms.ToolStripSeparator
        Me.BindingNavigatorPositionItem = New System.Windows.Forms.ToolStripTextBox
        Me.BindingNavigatorSeparator1 = New System.Windows.Forms.ToolStripSeparator
        Me.BindingNavigatorMoveNextItem = New System.Windows.Forms.ToolStripButton
        Me.BindingNavigatorMoveLastItem = New System.Windows.Forms.ToolStripButton
        Me.BindingNavigatorSeparator2 = New System.Windows.Forms.ToolStripSeparator
        Me.TSQLCfgConnectionDataGridView = New System.Windows.Forms.DataGridView
        Me.InstanceNameDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.EncryptedConnectionDataGridViewCheckBoxColumn = New System.Windows.Forms.DataGridViewCheckBoxColumn
        Me.TrustServerCertificateDataGridViewCheckBoxColumn = New System.Windows.Forms.DataGridViewCheckBoxColumn
        Me.NetworkProtocolDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.ConnectionTimeoutDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.LoginSecureDataGridViewCheckBoxColumn = New System.Windows.Forms.DataGridViewCheckBoxColumn
        Me.TableLayoutPanel1.SuspendLayout()
        CType(Me.TSQLCfgConnectionBindingNavigator, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TSQLCfgConnectionBindingNavigator.SuspendLayout()
        CType(Me.TSQLCfgConnectionBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TSQLCfgConnectionDataGridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TableLayoutPanel1.ColumnCount = 2
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.OK_Button, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.Cancel_Button, 1, 0)
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(160, 238)
        Me.TableLayoutPanel1.Margin = New System.Windows.Forms.Padding(0)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 1
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(194, 24)
        Me.TableLayoutPanel1.TabIndex = 0
        '
        'OK_Button
        '
        Me.OK_Button.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.OK_Button.Location = New System.Drawing.Point(1, 1)
        Me.OK_Button.Margin = New System.Windows.Forms.Padding(0)
        Me.OK_Button.Name = "OK_Button"
        Me.OK_Button.Size = New System.Drawing.Size(95, 22)
        Me.OK_Button.TabIndex = 0
        Me.OK_Button.Text = "OK"
        '
        'Cancel_Button
        '
        Me.Cancel_Button.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Cancel_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Cancel_Button.Location = New System.Drawing.Point(98, 1)
        Me.Cancel_Button.Margin = New System.Windows.Forms.Padding(0)
        Me.Cancel_Button.Name = "Cancel_Button"
        Me.Cancel_Button.Size = New System.Drawing.Size(95, 22)
        Me.Cancel_Button.TabIndex = 1
        Me.Cancel_Button.Text = "Cancel"
        '
        'TSQLCfgConnectionBindingNavigator
        '
        Me.TSQLCfgConnectionBindingNavigator.AddNewItem = Nothing
        Me.TSQLCfgConnectionBindingNavigator.BindingSource = Me.TSQLCfgConnectionBindingSource
        Me.TSQLCfgConnectionBindingNavigator.CountItem = Me.BindingNavigatorCountItem
        Me.TSQLCfgConnectionBindingNavigator.DeleteItem = Nothing
        Me.TSQLCfgConnectionBindingNavigator.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.BindingNavigatorMoveFirstItem, Me.BindingNavigatorMovePreviousItem, Me.BindingNavigatorSeparator, Me.BindingNavigatorPositionItem, Me.BindingNavigatorCountItem, Me.BindingNavigatorSeparator1, Me.BindingNavigatorMoveNextItem, Me.BindingNavigatorMoveLastItem, Me.BindingNavigatorSeparator2})
        Me.TSQLCfgConnectionBindingNavigator.Location = New System.Drawing.Point(0, 0)
        Me.TSQLCfgConnectionBindingNavigator.MoveFirstItem = Me.BindingNavigatorMoveFirstItem
        Me.TSQLCfgConnectionBindingNavigator.MoveLastItem = Me.BindingNavigatorMoveLastItem
        Me.TSQLCfgConnectionBindingNavigator.MoveNextItem = Me.BindingNavigatorMoveNextItem
        Me.TSQLCfgConnectionBindingNavigator.MovePreviousItem = Me.BindingNavigatorMovePreviousItem
        Me.TSQLCfgConnectionBindingNavigator.Name = "TSQLCfgConnectionBindingNavigator"
        Me.TSQLCfgConnectionBindingNavigator.PositionItem = Me.BindingNavigatorPositionItem
        Me.TSQLCfgConnectionBindingNavigator.Size = New System.Drawing.Size(359, 25)
        Me.TSQLCfgConnectionBindingNavigator.TabIndex = 1
        Me.TSQLCfgConnectionBindingNavigator.Text = "BindingNavigator1"
        '
        'TSQLCfgConnectionBindingSource
        '
        Me.TSQLCfgConnectionBindingSource.DataMember = "tConnection"
        Me.TSQLCfgConnectionBindingSource.DataSource = GetType(cCommon.dsSQLConfiguration)
        '
        'BindingNavigatorCountItem
        '
        Me.BindingNavigatorCountItem.Name = "BindingNavigatorCountItem"
        Me.BindingNavigatorCountItem.Size = New System.Drawing.Size(35, 22)
        Me.BindingNavigatorCountItem.Text = "of {0}"
        Me.BindingNavigatorCountItem.ToolTipText = "Total number of items"
        '
        'BindingNavigatorMoveFirstItem
        '
        Me.BindingNavigatorMoveFirstItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.BindingNavigatorMoveFirstItem.Image = CType(resources.GetObject("BindingNavigatorMoveFirstItem.Image"), System.Drawing.Image)
        Me.BindingNavigatorMoveFirstItem.Name = "BindingNavigatorMoveFirstItem"
        Me.BindingNavigatorMoveFirstItem.RightToLeftAutoMirrorImage = True
        Me.BindingNavigatorMoveFirstItem.Size = New System.Drawing.Size(23, 22)
        Me.BindingNavigatorMoveFirstItem.Text = "Move first"
        '
        'BindingNavigatorMovePreviousItem
        '
        Me.BindingNavigatorMovePreviousItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.BindingNavigatorMovePreviousItem.Image = CType(resources.GetObject("BindingNavigatorMovePreviousItem.Image"), System.Drawing.Image)
        Me.BindingNavigatorMovePreviousItem.Name = "BindingNavigatorMovePreviousItem"
        Me.BindingNavigatorMovePreviousItem.RightToLeftAutoMirrorImage = True
        Me.BindingNavigatorMovePreviousItem.Size = New System.Drawing.Size(23, 22)
        Me.BindingNavigatorMovePreviousItem.Text = "Move previous"
        '
        'BindingNavigatorSeparator
        '
        Me.BindingNavigatorSeparator.Name = "BindingNavigatorSeparator"
        Me.BindingNavigatorSeparator.Size = New System.Drawing.Size(6, 25)
        '
        'BindingNavigatorPositionItem
        '
        Me.BindingNavigatorPositionItem.AccessibleName = "Position"
        Me.BindingNavigatorPositionItem.AutoSize = False
        Me.BindingNavigatorPositionItem.Name = "BindingNavigatorPositionItem"
        Me.BindingNavigatorPositionItem.Size = New System.Drawing.Size(50, 23)
        Me.BindingNavigatorPositionItem.Text = "0"
        Me.BindingNavigatorPositionItem.ToolTipText = "Current position"
        '
        'BindingNavigatorSeparator1
        '
        Me.BindingNavigatorSeparator1.Name = "BindingNavigatorSeparator1"
        Me.BindingNavigatorSeparator1.Size = New System.Drawing.Size(6, 25)
        '
        'BindingNavigatorMoveNextItem
        '
        Me.BindingNavigatorMoveNextItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.BindingNavigatorMoveNextItem.Image = CType(resources.GetObject("BindingNavigatorMoveNextItem.Image"), System.Drawing.Image)
        Me.BindingNavigatorMoveNextItem.Name = "BindingNavigatorMoveNextItem"
        Me.BindingNavigatorMoveNextItem.RightToLeftAutoMirrorImage = True
        Me.BindingNavigatorMoveNextItem.Size = New System.Drawing.Size(23, 22)
        Me.BindingNavigatorMoveNextItem.Text = "Move next"
        '
        'BindingNavigatorMoveLastItem
        '
        Me.BindingNavigatorMoveLastItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.BindingNavigatorMoveLastItem.Image = CType(resources.GetObject("BindingNavigatorMoveLastItem.Image"), System.Drawing.Image)
        Me.BindingNavigatorMoveLastItem.Name = "BindingNavigatorMoveLastItem"
        Me.BindingNavigatorMoveLastItem.RightToLeftAutoMirrorImage = True
        Me.BindingNavigatorMoveLastItem.Size = New System.Drawing.Size(23, 22)
        Me.BindingNavigatorMoveLastItem.Text = "Move last"
        '
        'BindingNavigatorSeparator2
        '
        Me.BindingNavigatorSeparator2.Name = "BindingNavigatorSeparator2"
        Me.BindingNavigatorSeparator2.Size = New System.Drawing.Size(6, 25)
        '
        'TSQLCfgConnectionDataGridView
        '
        Me.TSQLCfgConnectionDataGridView.AllowUserToAddRows = False
        Me.TSQLCfgConnectionDataGridView.AllowUserToDeleteRows = False
        Me.TSQLCfgConnectionDataGridView.AllowUserToOrderColumns = True
        Me.TSQLCfgConnectionDataGridView.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TSQLCfgConnectionDataGridView.AutoGenerateColumns = False
        Me.TSQLCfgConnectionDataGridView.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.TSQLCfgConnectionDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.TSQLCfgConnectionDataGridView.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.InstanceNameDataGridViewTextBoxColumn, Me.EncryptedConnectionDataGridViewCheckBoxColumn, Me.TrustServerCertificateDataGridViewCheckBoxColumn, Me.NetworkProtocolDataGridViewTextBoxColumn, Me.ConnectionTimeoutDataGridViewTextBoxColumn, Me.LoginSecureDataGridViewCheckBoxColumn})
        Me.TSQLCfgConnectionDataGridView.DataSource = Me.TSQLCfgConnectionBindingSource
        Me.TSQLCfgConnectionDataGridView.Location = New System.Drawing.Point(4, 26)
        Me.TSQLCfgConnectionDataGridView.Name = "TSQLCfgConnectionDataGridView"
        Me.TSQLCfgConnectionDataGridView.ReadOnly = True
        Me.TSQLCfgConnectionDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect
        Me.TSQLCfgConnectionDataGridView.Size = New System.Drawing.Size(350, 206)
        Me.TSQLCfgConnectionDataGridView.TabIndex = 2
        '
        'InstanceNameDataGridViewTextBoxColumn
        '
        Me.InstanceNameDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
        Me.InstanceNameDataGridViewTextBoxColumn.DataPropertyName = "InstanceName"
        Me.InstanceNameDataGridViewTextBoxColumn.HeaderText = "InstanceName"
        Me.InstanceNameDataGridViewTextBoxColumn.Name = "InstanceNameDataGridViewTextBoxColumn"
        Me.InstanceNameDataGridViewTextBoxColumn.ReadOnly = True
        '
        'EncryptedConnectionDataGridViewCheckBoxColumn
        '
        Me.EncryptedConnectionDataGridViewCheckBoxColumn.DataPropertyName = "EncryptedConnection"
        Me.EncryptedConnectionDataGridViewCheckBoxColumn.HeaderText = "Encrypted Connection"
        Me.EncryptedConnectionDataGridViewCheckBoxColumn.MinimumWidth = 70
        Me.EncryptedConnectionDataGridViewCheckBoxColumn.Name = "EncryptedConnectionDataGridViewCheckBoxColumn"
        Me.EncryptedConnectionDataGridViewCheckBoxColumn.ReadOnly = True
        Me.EncryptedConnectionDataGridViewCheckBoxColumn.ToolTipText = "Encrypt the data on the wire. By default an SSL certificate must first be install" & _
            "ed"
        Me.EncryptedConnectionDataGridViewCheckBoxColumn.Visible = False
        Me.EncryptedConnectionDataGridViewCheckBoxColumn.Width = 70
        '
        'TrustServerCertificateDataGridViewCheckBoxColumn
        '
        Me.TrustServerCertificateDataGridViewCheckBoxColumn.DataPropertyName = "TrustServerCertificate"
        Me.TrustServerCertificateDataGridViewCheckBoxColumn.HeaderText = "Trust Server Certificate"
        Me.TrustServerCertificateDataGridViewCheckBoxColumn.MinimumWidth = 70
        Me.TrustServerCertificateDataGridViewCheckBoxColumn.Name = "TrustServerCertificateDataGridViewCheckBoxColumn"
        Me.TrustServerCertificateDataGridViewCheckBoxColumn.ReadOnly = True
        Me.TrustServerCertificateDataGridViewCheckBoxColumn.ToolTipText = "Let the server self-generate a certificate when an SSL certificate is not install" & _
            "ed"
        Me.TrustServerCertificateDataGridViewCheckBoxColumn.Visible = False
        Me.TrustServerCertificateDataGridViewCheckBoxColumn.Width = 70
        '
        'NetworkProtocolDataGridViewTextBoxColumn
        '
        Me.NetworkProtocolDataGridViewTextBoxColumn.DataPropertyName = "NetworkProtocol"
        Me.NetworkProtocolDataGridViewTextBoxColumn.HeaderText = "Network Protocol"
        Me.NetworkProtocolDataGridViewTextBoxColumn.MinimumWidth = 70
        Me.NetworkProtocolDataGridViewTextBoxColumn.Name = "NetworkProtocolDataGridViewTextBoxColumn"
        Me.NetworkProtocolDataGridViewTextBoxColumn.ReadOnly = True
        Me.NetworkProtocolDataGridViewTextBoxColumn.Visible = False
        Me.NetworkProtocolDataGridViewTextBoxColumn.Width = 70
        '
        'ConnectionTimeoutDataGridViewTextBoxColumn
        '
        Me.ConnectionTimeoutDataGridViewTextBoxColumn.DataPropertyName = "ConnectionTimeout"
        Me.ConnectionTimeoutDataGridViewTextBoxColumn.HeaderText = "Connection Timeout"
        Me.ConnectionTimeoutDataGridViewTextBoxColumn.MinimumWidth = 70
        Me.ConnectionTimeoutDataGridViewTextBoxColumn.Name = "ConnectionTimeoutDataGridViewTextBoxColumn"
        Me.ConnectionTimeoutDataGridViewTextBoxColumn.ReadOnly = True
        Me.ConnectionTimeoutDataGridViewTextBoxColumn.ToolTipText = "In seconds"
        Me.ConnectionTimeoutDataGridViewTextBoxColumn.Visible = False
        Me.ConnectionTimeoutDataGridViewTextBoxColumn.Width = 70
        '
        'LoginSecureDataGridViewCheckBoxColumn
        '
        Me.LoginSecureDataGridViewCheckBoxColumn.DataPropertyName = "LoginSecure"
        Me.LoginSecureDataGridViewCheckBoxColumn.HeaderText = "Login Secure"
        Me.LoginSecureDataGridViewCheckBoxColumn.MinimumWidth = 50
        Me.LoginSecureDataGridViewCheckBoxColumn.Name = "LoginSecureDataGridViewCheckBoxColumn"
        Me.LoginSecureDataGridViewCheckBoxColumn.ReadOnly = True
        Me.LoginSecureDataGridViewCheckBoxColumn.ToolTipText = "Use trusted Windows authentication "
        Me.LoginSecureDataGridViewCheckBoxColumn.Visible = False
        Me.LoginSecureDataGridViewCheckBoxColumn.Width = 50
        '
        'DialogSelectConfiguredInstance
        '
        Me.AcceptButton = Me.OK_Button
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.Cancel_Button
        Me.ClientSize = New System.Drawing.Size(359, 266)
        Me.Controls.Add(Me.TSQLCfgConnectionDataGridView)
        Me.Controls.Add(Me.TSQLCfgConnectionBindingNavigator)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(375, 300)
        Me.Name = "DialogSelectConfiguredInstance"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Select SQL Instance for New Query Baseline Plan"
        Me.TableLayoutPanel1.ResumeLayout(False)
        CType(Me.TSQLCfgConnectionBindingNavigator, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TSQLCfgConnectionBindingNavigator.ResumeLayout(False)
        Me.TSQLCfgConnectionBindingNavigator.PerformLayout()
        CType(Me.TSQLCfgConnectionBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TSQLCfgConnectionDataGridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents OK_Button As System.Windows.Forms.Button
    Friend WithEvents Cancel_Button As System.Windows.Forms.Button
    Friend WithEvents TSQLCfgConnectionBindingSource As System.Windows.Forms.BindingSource
    Friend WithEvents TSQLCfgConnectionBindingNavigator As System.Windows.Forms.BindingNavigator
    Friend WithEvents BindingNavigatorCountItem As System.Windows.Forms.ToolStripLabel
    Friend WithEvents BindingNavigatorMoveFirstItem As System.Windows.Forms.ToolStripButton
    Friend WithEvents BindingNavigatorMovePreviousItem As System.Windows.Forms.ToolStripButton
    Friend WithEvents BindingNavigatorSeparator As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents BindingNavigatorPositionItem As System.Windows.Forms.ToolStripTextBox
    Friend WithEvents BindingNavigatorSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents BindingNavigatorMoveNextItem As System.Windows.Forms.ToolStripButton
    Friend WithEvents BindingNavigatorMoveLastItem As System.Windows.Forms.ToolStripButton
    Friend WithEvents BindingNavigatorSeparator2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents TSQLCfgConnectionDataGridView As System.Windows.Forms.DataGridView
    Friend WithEvents InstanceNameDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents EncryptedConnectionDataGridViewCheckBoxColumn As System.Windows.Forms.DataGridViewCheckBoxColumn
    Friend WithEvents TrustServerCertificateDataGridViewCheckBoxColumn As System.Windows.Forms.DataGridViewCheckBoxColumn
    Friend WithEvents NetworkProtocolDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ConnectionTimeoutDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents LoginSecureDataGridViewCheckBoxColumn As System.Windows.Forms.DataGridViewCheckBoxColumn

End Class
