<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class DialogViewQueue
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
        Me.ButtonClose = New System.Windows.Forms.Button
        Me.DataGridViewQueueStatus = New System.Windows.Forms.DataGridView
        Me.NameDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.ReceiveStateDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.EnqueueStateDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.ActivationStateDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.ActivationProcedureDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.PSQLClueCheckQueueStatusBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.DataSetQueuedChanges = New SQLClue.DataSetQueuedChanges
        Me.DataGridViewQueuedEvents = New System.Windows.Forms.DataGridView
        Me.DataGridViewButtonColumnQueuingOrder = New System.Windows.Forms.DataGridViewButtonColumn
        Me.EventTypeDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.DatabaseNameDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.SchemaNameDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.ObjectNameDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.PostTimeDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.PSQLClueShowQueueBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.ButtonResetQueues = New System.Windows.Forms.Button
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.ToolTipQueueViewer = New System.Windows.Forms.ToolTip(Me.components)
        Me.TableAdapterManager = New SQLClue.DataSetQueuedChangesTableAdapters.TableAdapterManager
        Me.PSQLClueCheckQueueStatusTableAdapter = New SQLClue.DataSetQueuedChangesTableAdapters.pSQLClueCheckQueueStatusTableAdapter
        Me.PSQLClueShowQueueTableAdapter = New SQLClue.DataSetQueuedChangesTableAdapters.pSQLClueShowQueueTableAdapter
        CType(Me.DataGridViewQueueStatus, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PSQLClueCheckQueueStatusBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DataSetQueuedChanges, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DataGridViewQueuedEvents, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PSQLClueShowQueueBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        Me.SuspendLayout()
        '
        'ButtonClose
        '
        Me.ButtonClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ButtonClose.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.ButtonClose.Location = New System.Drawing.Point(476, 356)
        Me.ButtonClose.Margin = New System.Windows.Forms.Padding(0)
        Me.ButtonClose.Name = "ButtonClose"
        Me.ButtonClose.Size = New System.Drawing.Size(95, 22)
        Me.ButtonClose.TabIndex = 0
        Me.ButtonClose.Text = "Close"
        '
        'DataGridViewQueueStatus
        '
        Me.DataGridViewQueueStatus.AllowUserToAddRows = False
        Me.DataGridViewQueueStatus.AllowUserToDeleteRows = False
        Me.DataGridViewQueueStatus.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.DataGridViewQueueStatus.AutoGenerateColumns = False
        Me.DataGridViewQueueStatus.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader
        Me.DataGridViewQueueStatus.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders
        Me.DataGridViewQueueStatus.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.DataGridViewQueueStatus.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridViewQueueStatus.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.NameDataGridViewTextBoxColumn, Me.ReceiveStateDataGridViewTextBoxColumn, Me.EnqueueStateDataGridViewTextBoxColumn, Me.ActivationStateDataGridViewTextBoxColumn, Me.ActivationProcedureDataGridViewTextBoxColumn})
        Me.DataGridViewQueueStatus.DataSource = Me.PSQLClueCheckQueueStatusBindingSource
        Me.DataGridViewQueueStatus.Location = New System.Drawing.Point(4, 20)
        Me.DataGridViewQueueStatus.Margin = New System.Windows.Forms.Padding(0)
        Me.DataGridViewQueueStatus.Name = "DataGridViewQueueStatus"
        Me.DataGridViewQueueStatus.ReadOnly = True
        Me.DataGridViewQueueStatus.Size = New System.Drawing.Size(557, 129)
        Me.DataGridViewQueueStatus.TabIndex = 2
        '
        'NameDataGridViewTextBoxColumn
        '
        Me.NameDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
        Me.NameDataGridViewTextBoxColumn.DataPropertyName = "Name"
        Me.NameDataGridViewTextBoxColumn.HeaderText = "Queue Name"
        Me.NameDataGridViewTextBoxColumn.MinimumWidth = 100
        Me.NameDataGridViewTextBoxColumn.Name = "NameDataGridViewTextBoxColumn"
        Me.NameDataGridViewTextBoxColumn.ReadOnly = True
        '
        'ReceiveStateDataGridViewTextBoxColumn
        '
        Me.ReceiveStateDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.ReceiveStateDataGridViewTextBoxColumn.DataPropertyName = "ReceiveState"
        Me.ReceiveStateDataGridViewTextBoxColumn.HeaderText = "Receive State"
        Me.ReceiveStateDataGridViewTextBoxColumn.MinimumWidth = 70
        Me.ReceiveStateDataGridViewTextBoxColumn.Name = "ReceiveStateDataGridViewTextBoxColumn"
        Me.ReceiveStateDataGridViewTextBoxColumn.ReadOnly = True
        Me.ReceiveStateDataGridViewTextBoxColumn.Width = 70
        '
        'EnqueueStateDataGridViewTextBoxColumn
        '
        Me.EnqueueStateDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.EnqueueStateDataGridViewTextBoxColumn.DataPropertyName = "EnqueueState"
        Me.EnqueueStateDataGridViewTextBoxColumn.HeaderText = "Enqueue State"
        Me.EnqueueStateDataGridViewTextBoxColumn.MinimumWidth = 70
        Me.EnqueueStateDataGridViewTextBoxColumn.Name = "EnqueueStateDataGridViewTextBoxColumn"
        Me.EnqueueStateDataGridViewTextBoxColumn.ReadOnly = True
        Me.EnqueueStateDataGridViewTextBoxColumn.Width = 70
        '
        'ActivationStateDataGridViewTextBoxColumn
        '
        Me.ActivationStateDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.ActivationStateDataGridViewTextBoxColumn.DataPropertyName = "ActivationState"
        Me.ActivationStateDataGridViewTextBoxColumn.HeaderText = "Activation State"
        Me.ActivationStateDataGridViewTextBoxColumn.MinimumWidth = 70
        Me.ActivationStateDataGridViewTextBoxColumn.Name = "ActivationStateDataGridViewTextBoxColumn"
        Me.ActivationStateDataGridViewTextBoxColumn.ReadOnly = True
        Me.ActivationStateDataGridViewTextBoxColumn.Width = 70
        '
        'ActivationProcedureDataGridViewTextBoxColumn
        '
        Me.ActivationProcedureDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
        Me.ActivationProcedureDataGridViewTextBoxColumn.DataPropertyName = "ActivationProcedure"
        Me.ActivationProcedureDataGridViewTextBoxColumn.HeaderText = "Activation Procedure"
        Me.ActivationProcedureDataGridViewTextBoxColumn.MinimumWidth = 100
        Me.ActivationProcedureDataGridViewTextBoxColumn.Name = "ActivationProcedureDataGridViewTextBoxColumn"
        Me.ActivationProcedureDataGridViewTextBoxColumn.ReadOnly = True
        '
        'PSQLClueCheckQueueStatusBindingSource
        '
        Me.PSQLClueCheckQueueStatusBindingSource.DataMember = "pSQLClueCheckQueueStatus"
        Me.PSQLClueCheckQueueStatusBindingSource.DataSource = Me.DataSetQueuedChanges
        '
        'DataSetQueuedChanges
        '
        Me.DataSetQueuedChanges.DataSetName = "DataSetQueuedChanges"
        Me.DataSetQueuedChanges.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        '
        'DataGridViewQueuedEvents
        '
        Me.DataGridViewQueuedEvents.AllowUserToAddRows = False
        Me.DataGridViewQueuedEvents.AllowUserToDeleteRows = False
        Me.DataGridViewQueuedEvents.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.DataGridViewQueuedEvents.AutoGenerateColumns = False
        Me.DataGridViewQueuedEvents.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader
        Me.DataGridViewQueuedEvents.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders
        Me.DataGridViewQueuedEvents.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.DataGridViewQueuedEvents.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridViewQueuedEvents.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.DataGridViewButtonColumnQueuingOrder, Me.EventTypeDataGridViewTextBoxColumn, Me.DatabaseNameDataGridViewTextBoxColumn, Me.SchemaNameDataGridViewTextBoxColumn, Me.ObjectNameDataGridViewTextBoxColumn, Me.PostTimeDataGridViewTextBoxColumn})
        Me.DataGridViewQueuedEvents.DataSource = Me.PSQLClueShowQueueBindingSource
        Me.DataGridViewQueuedEvents.Location = New System.Drawing.Point(4, 20)
        Me.DataGridViewQueuedEvents.Name = "DataGridViewQueuedEvents"
        Me.DataGridViewQueuedEvents.ReadOnly = True
        Me.DataGridViewQueuedEvents.Size = New System.Drawing.Size(557, 160)
        Me.DataGridViewQueuedEvents.TabIndex = 3
        '
        'DataGridViewButtonColumnQueuingOrder
        '
        Me.DataGridViewButtonColumnQueuingOrder.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
        Me.DataGridViewButtonColumnQueuingOrder.DataPropertyName = "QueuingOrder"
        Me.DataGridViewButtonColumnQueuingOrder.HeaderText = "Queue Item"
        Me.DataGridViewButtonColumnQueuingOrder.MinimumWidth = 50
        Me.DataGridViewButtonColumnQueuingOrder.Name = "DataGridViewButtonColumnQueuingOrder"
        Me.DataGridViewButtonColumnQueuingOrder.ReadOnly = True
        Me.DataGridViewButtonColumnQueuingOrder.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DataGridViewButtonColumnQueuingOrder.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        Me.DataGridViewButtonColumnQueuingOrder.Text = "View"
        Me.DataGridViewButtonColumnQueuingOrder.ToolTipText = "Select to View XML"
        Me.DataGridViewButtonColumnQueuingOrder.Width = 80
        '
        'EventTypeDataGridViewTextBoxColumn
        '
        Me.EventTypeDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
        Me.EventTypeDataGridViewTextBoxColumn.DataPropertyName = "EventType"
        Me.EventTypeDataGridViewTextBoxColumn.HeaderText = "Event Type"
        Me.EventTypeDataGridViewTextBoxColumn.MinimumWidth = 70
        Me.EventTypeDataGridViewTextBoxColumn.Name = "EventTypeDataGridViewTextBoxColumn"
        Me.EventTypeDataGridViewTextBoxColumn.ReadOnly = True
        Me.EventTypeDataGridViewTextBoxColumn.Width = 80
        '
        'DatabaseNameDataGridViewTextBoxColumn
        '
        Me.DatabaseNameDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
        Me.DatabaseNameDataGridViewTextBoxColumn.DataPropertyName = "DatabaseName"
        Me.DatabaseNameDataGridViewTextBoxColumn.HeaderText = "Database Name"
        Me.DatabaseNameDataGridViewTextBoxColumn.MinimumWidth = 60
        Me.DatabaseNameDataGridViewTextBoxColumn.Name = "DatabaseNameDataGridViewTextBoxColumn"
        Me.DatabaseNameDataGridViewTextBoxColumn.ReadOnly = True
        '
        'SchemaNameDataGridViewTextBoxColumn
        '
        Me.SchemaNameDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
        Me.SchemaNameDataGridViewTextBoxColumn.DataPropertyName = "SchemaName"
        Me.SchemaNameDataGridViewTextBoxColumn.HeaderText = "Schema Name"
        Me.SchemaNameDataGridViewTextBoxColumn.MinimumWidth = 50
        Me.SchemaNameDataGridViewTextBoxColumn.Name = "SchemaNameDataGridViewTextBoxColumn"
        Me.SchemaNameDataGridViewTextBoxColumn.ReadOnly = True
        Me.SchemaNameDataGridViewTextBoxColumn.Width = 94
        '
        'ObjectNameDataGridViewTextBoxColumn
        '
        Me.ObjectNameDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
        Me.ObjectNameDataGridViewTextBoxColumn.DataPropertyName = "ObjectName"
        Me.ObjectNameDataGridViewTextBoxColumn.HeaderText = "Object Name"
        Me.ObjectNameDataGridViewTextBoxColumn.MinimumWidth = 50
        Me.ObjectNameDataGridViewTextBoxColumn.Name = "ObjectNameDataGridViewTextBoxColumn"
        Me.ObjectNameDataGridViewTextBoxColumn.ReadOnly = True
        Me.ObjectNameDataGridViewTextBoxColumn.Width = 87
        '
        'PostTimeDataGridViewTextBoxColumn
        '
        Me.PostTimeDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
        Me.PostTimeDataGridViewTextBoxColumn.DataPropertyName = "PostTime"
        Me.PostTimeDataGridViewTextBoxColumn.HeaderText = "Event Post Time"
        Me.PostTimeDataGridViewTextBoxColumn.MinimumWidth = 50
        Me.PostTimeDataGridViewTextBoxColumn.Name = "PostTimeDataGridViewTextBoxColumn"
        Me.PostTimeDataGridViewTextBoxColumn.ReadOnly = True
        Me.PostTimeDataGridViewTextBoxColumn.Width = 80
        '
        'PSQLClueShowQueueBindingSource
        '
        Me.PSQLClueShowQueueBindingSource.DataMember = "pSQLClueShowQueue"
        Me.PSQLClueShowQueueBindingSource.DataSource = Me.DataSetQueuedChanges
        '
        'ButtonResetQueues
        '
        Me.ButtonResetQueues.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ButtonResetQueues.Enabled = False
        Me.ButtonResetQueues.Location = New System.Drawing.Point(378, 356)
        Me.ButtonResetQueues.Margin = New System.Windows.Forms.Padding(0)
        Me.ButtonResetQueues.Name = "ButtonResetQueues"
        Me.ButtonResetQueues.Size = New System.Drawing.Size(95, 22)
        Me.ButtonResetQueues.TabIndex = 4
        Me.ButtonResetQueues.Text = "Enable Queues"
        Me.ToolTipQueueViewer.SetToolTip(Me.ButtonResetQueues, "Reset the Event Queue and the Monitor Queue to the state required for normal SQLC" & _
                "lue processing")
        Me.ButtonResetQueues.UseVisualStyleBackColor = True
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SplitContainer1.BackColor = System.Drawing.SystemColors.GradientInactiveCaption
        Me.SplitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.SplitContainer1.Location = New System.Drawing.Point(4, 4)
        Me.SplitContainer1.Margin = New System.Windows.Forms.Padding(0)
        Me.SplitContainer1.Name = "SplitContainer1"
        Me.SplitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.BackColor = System.Drawing.SystemColors.Control
        Me.SplitContainer1.Panel1.Controls.Add(Me.Label1)
        Me.SplitContainer1.Panel1.Controls.Add(Me.DataGridViewQueueStatus)
        Me.SplitContainer1.Panel1.Padding = New System.Windows.Forms.Padding(4)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.BackColor = System.Drawing.SystemColors.Control
        Me.SplitContainer1.Panel2.Controls.Add(Me.DataGridViewQueuedEvents)
        Me.SplitContainer1.Panel2.Controls.Add(Me.Label2)
        Me.SplitContainer1.Panel2.Padding = New System.Windows.Forms.Padding(4)
        Me.SplitContainer1.Size = New System.Drawing.Size(567, 347)
        Me.SplitContainer1.SplitterDistance = 156
        Me.SplitContainer1.TabIndex = 5
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label1.Location = New System.Drawing.Point(4, 4)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(67, 13)
        Me.Label1.TabIndex = 3
        Me.Label1.Text = "Queue State"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label2.Location = New System.Drawing.Point(4, 4)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(81, 13)
        Me.Label2.TabIndex = 4
        Me.Label2.Text = "Queued Events"
        Me.ToolTipQueueViewer.SetToolTip(Me.Label2, "All events queued at the target server awaiting archive to the SQLClue Repository" & _
                "")
        '
        'TableAdapterManager
        '
        Me.TableAdapterManager.BackupDataSetBeforeUpdate = False
        Me.TableAdapterManager.Connection = Nothing
        Me.TableAdapterManager.UpdateOrder = SQLClue.DataSetQueuedChangesTableAdapters.TableAdapterManager.UpdateOrderOption.InsertUpdateDelete
        '
        'PSQLClueCheckQueueStatusTableAdapter
        '
        Me.PSQLClueCheckQueueStatusTableAdapter.ClearBeforeFill = True
        '
        'PSQLClueShowQueueTableAdapter
        '
        Me.PSQLClueShowQueueTableAdapter.ClearBeforeFill = True
        '
        'DialogViewQueue
        '
        Me.AcceptButton = Me.ButtonClose
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.CancelButton = Me.ButtonClose
        Me.ClientSize = New System.Drawing.Size(576, 382)
        Me.Controls.Add(Me.SplitContainer1)
        Me.Controls.Add(Me.ButtonResetQueues)
        Me.Controls.Add(Me.ButtonClose)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(500, 300)
        Me.Name = "DialogViewQueue"
        Me.Padding = New System.Windows.Forms.Padding(4)
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "assigned in code"
        CType(Me.DataGridViewQueueStatus, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PSQLClueCheckQueueStatusBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DataSetQueuedChanges, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DataGridViewQueuedEvents, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PSQLClueShowQueueBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel1.PerformLayout()
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        Me.SplitContainer1.Panel2.PerformLayout()
        Me.SplitContainer1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ButtonClose As System.Windows.Forms.Button
    Friend WithEvents DataSetQueuedChanges As SQLClue.DataSetQueuedChanges
    Friend WithEvents TableAdapterManager As SQLClue.DataSetQueuedChangesTableAdapters.TableAdapterManager
    Friend WithEvents DataGridViewQueueStatus As System.Windows.Forms.DataGridView
    Friend WithEvents PSQLClueCheckQueueStatusBindingSource As System.Windows.Forms.BindingSource
    Friend WithEvents PSQLClueCheckQueueStatusTableAdapter As SQLClue.DataSetQueuedChangesTableAdapters.pSQLClueCheckQueueStatusTableAdapter
    Friend WithEvents DataGridViewQueuedEvents As System.Windows.Forms.DataGridView
    Friend WithEvents PSQLClueShowQueueBindingSource As System.Windows.Forms.BindingSource
    Friend WithEvents PSQLClueShowQueueTableAdapter As SQLClue.DataSetQueuedChangesTableAdapters.pSQLClueShowQueueTableAdapter
    Friend WithEvents ButtonResetQueues As System.Windows.Forms.Button
    Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents ToolTipQueueViewer As System.Windows.Forms.ToolTip
    Friend WithEvents NameDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ReceiveStateDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents EnqueueStateDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ActivationStateDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ActivationProcedureDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewButtonColumnQueuingOrder As System.Windows.Forms.DataGridViewButtonColumn
    Friend WithEvents EventTypeDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DatabaseNameDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents SchemaNameDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ObjectNameDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents PostTimeDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn

End Class
