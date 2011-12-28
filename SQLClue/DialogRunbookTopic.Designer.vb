<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class DialogRunbookTopic
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(DialogRunbookTopic))
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel
        Me.OK_Button = New System.Windows.Forms.Button
        Me.Cancel_Button = New System.Windows.Forms.Button
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.TopicDataGridView = New System.Windows.Forms.DataGridView
        Me.TTopicBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.DataSetSQLRunbook = New SQLClue.DataSetSQLRunbook
        Me.Label1 = New System.Windows.Forms.Label
        Me.TTopicTableAdapter = New SQLClue.DataSetSQLRunbookTableAdapters.tTopicTableAdapter
        Me.DataGridViewTextBoxColumnId = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.DataGridViewTextBoxColumnName = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.NotesDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.OwnerDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.RecCreatedDtDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.RecCreatedUserDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.DataGridViewTextBoxColumnNbrDocuments = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.RatingTallyDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.RatingCountDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.LastUpdatedDtDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.LastUpdatedUserDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.TableLayoutPanel1.SuspendLayout()
        Me.Panel1.SuspendLayout()
        CType(Me.TopicDataGridView, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TTopicBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DataSetSQLRunbook, System.ComponentModel.ISupportInitialize).BeginInit()
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
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(310, 357)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 1
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(146, 29)
        Me.TableLayoutPanel1.TabIndex = 0
        '
        'OK_Button
        '
        Me.OK_Button.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.OK_Button.Location = New System.Drawing.Point(3, 3)
        Me.OK_Button.Name = "OK_Button"
        Me.OK_Button.Size = New System.Drawing.Size(67, 23)
        Me.OK_Button.TabIndex = 0
        Me.OK_Button.Text = "OK"
        '
        'Cancel_Button
        '
        Me.Cancel_Button.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Cancel_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Cancel_Button.Location = New System.Drawing.Point(76, 3)
        Me.Cancel_Button.Name = "Cancel_Button"
        Me.Cancel_Button.Size = New System.Drawing.Size(67, 23)
        Me.Cancel_Button.TabIndex = 1
        Me.Cancel_Button.Text = "Cancel"
        '
        'Panel1
        '
        Me.Panel1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.TopicDataGridView)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Location = New System.Drawing.Point(4, 4)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(460, 345)
        Me.Panel1.TabIndex = 4
        '
        'TopicDataGridView
        '
        Me.TopicDataGridView.AllowUserToAddRows = False
        Me.TopicDataGridView.AutoGenerateColumns = False
        Me.TopicDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.TopicDataGridView.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.DataGridViewTextBoxColumnId, Me.DataGridViewTextBoxColumnName, Me.NotesDataGridViewTextBoxColumn, Me.OwnerDataGridViewTextBoxColumn, Me.RecCreatedDtDataGridViewTextBoxColumn, Me.RecCreatedUserDataGridViewTextBoxColumn, Me.DataGridViewTextBoxColumnNbrDocuments, Me.RatingTallyDataGridViewTextBoxColumn, Me.RatingCountDataGridViewTextBoxColumn, Me.LastUpdatedDtDataGridViewTextBoxColumn, Me.LastUpdatedUserDataGridViewTextBoxColumn})
        Me.TopicDataGridView.DataSource = Me.TTopicBindingSource
        Me.TopicDataGridView.Location = New System.Drawing.Point(4, 55)
        Me.TopicDataGridView.Name = "TopicDataGridView"
        Me.TopicDataGridView.Size = New System.Drawing.Size(450, 285)
        Me.TopicDataGridView.TabIndex = 5
        '
        'TTopicBindingSource
        '
        Me.TTopicBindingSource.DataMember = "tTopic"
        Me.TTopicBindingSource.DataSource = Me.DataSetSQLRunbook
        '
        'DataSetSQLRunbook
        '
        Me.DataSetSQLRunbook.DataSetName = "DataSetSQLRunbook"
        Me.DataSetSQLRunbook.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        '
        'Label1
        '
        Me.Label1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label1.Location = New System.Drawing.Point(4, 4)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(450, 47)
        Me.Label1.TabIndex = 4
        Me.Label1.Text = resources.GetString("Label1.Text")
        '
        'TTopicTableAdapter
        '
        Me.TTopicTableAdapter.ClearBeforeFill = True
        '
        'DataGridViewTextBoxColumnId
        '
        Me.DataGridViewTextBoxColumnId.DataPropertyName = "Id"
        Me.DataGridViewTextBoxColumnId.HeaderText = "Id"
        Me.DataGridViewTextBoxColumnId.Name = "DataGridViewTextBoxColumnId"
        Me.DataGridViewTextBoxColumnId.ReadOnly = True
        Me.DataGridViewTextBoxColumnId.Visible = False
        Me.DataGridViewTextBoxColumnId.Width = 50
        '
        'DataGridViewTextBoxColumnName
        '
        Me.DataGridViewTextBoxColumnName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
        Me.DataGridViewTextBoxColumnName.DataPropertyName = "Name"
        Me.DataGridViewTextBoxColumnName.HeaderText = "Name"
        Me.DataGridViewTextBoxColumnName.Name = "DataGridViewTextBoxColumnName"
        '
        'NotesDataGridViewTextBoxColumn
        '
        Me.NotesDataGridViewTextBoxColumn.DataPropertyName = "Notes"
        Me.NotesDataGridViewTextBoxColumn.HeaderText = "Notes"
        Me.NotesDataGridViewTextBoxColumn.Name = "NotesDataGridViewTextBoxColumn"
        Me.NotesDataGridViewTextBoxColumn.Visible = False
        '
        'OwnerDataGridViewTextBoxColumn
        '
        Me.OwnerDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
        Me.OwnerDataGridViewTextBoxColumn.DataPropertyName = "Owner"
        Me.OwnerDataGridViewTextBoxColumn.HeaderText = "Owner"
        Me.OwnerDataGridViewTextBoxColumn.Name = "OwnerDataGridViewTextBoxColumn"
        Me.OwnerDataGridViewTextBoxColumn.ReadOnly = True
        Me.OwnerDataGridViewTextBoxColumn.Visible = False
        Me.OwnerDataGridViewTextBoxColumn.Width = 63
        '
        'RecCreatedDtDataGridViewTextBoxColumn
        '
        Me.RecCreatedDtDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
        Me.RecCreatedDtDataGridViewTextBoxColumn.DataPropertyName = "RecCreatedDt"
        Me.RecCreatedDtDataGridViewTextBoxColumn.HeaderText = "Rec Created Datetime"
        Me.RecCreatedDtDataGridViewTextBoxColumn.Name = "RecCreatedDtDataGridViewTextBoxColumn"
        Me.RecCreatedDtDataGridViewTextBoxColumn.ReadOnly = True
        Me.RecCreatedDtDataGridViewTextBoxColumn.Visible = False
        Me.RecCreatedDtDataGridViewTextBoxColumn.Width = 137
        '
        'RecCreatedUserDataGridViewTextBoxColumn
        '
        Me.RecCreatedUserDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
        Me.RecCreatedUserDataGridViewTextBoxColumn.DataPropertyName = "RecCreatedUser"
        Me.RecCreatedUserDataGridViewTextBoxColumn.HeaderText = "Rec Created User"
        Me.RecCreatedUserDataGridViewTextBoxColumn.Name = "RecCreatedUserDataGridViewTextBoxColumn"
        Me.RecCreatedUserDataGridViewTextBoxColumn.ReadOnly = True
        Me.RecCreatedUserDataGridViewTextBoxColumn.Visible = False
        Me.RecCreatedUserDataGridViewTextBoxColumn.Width = 117
        '
        'DataGridViewTextBoxColumnNbrDocuments
        '
        Me.DataGridViewTextBoxColumnNbrDocuments.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
        Me.DataGridViewTextBoxColumnNbrDocuments.DataPropertyName = "DocumentCount"
        Me.DataGridViewTextBoxColumnNbrDocuments.HeaderText = "Document Count"
        Me.DataGridViewTextBoxColumnNbrDocuments.Name = "DataGridViewTextBoxColumnNbrDocuments"
        Me.DataGridViewTextBoxColumnNbrDocuments.ReadOnly = True
        Me.DataGridViewTextBoxColumnNbrDocuments.Width = 103
        '
        'RatingTallyDataGridViewTextBoxColumn
        '
        Me.RatingTallyDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCellsExceptHeader
        Me.RatingTallyDataGridViewTextBoxColumn.DataPropertyName = "RatingTally"
        Me.RatingTallyDataGridViewTextBoxColumn.HeaderText = "RatingTally"
        Me.RatingTallyDataGridViewTextBoxColumn.Name = "RatingTallyDataGridViewTextBoxColumn"
        Me.RatingTallyDataGridViewTextBoxColumn.ReadOnly = True
        Me.RatingTallyDataGridViewTextBoxColumn.Visible = False
        Me.RatingTallyDataGridViewTextBoxColumn.Width = 5
        '
        'RatingCountDataGridViewTextBoxColumn
        '
        Me.RatingCountDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCellsExceptHeader
        Me.RatingCountDataGridViewTextBoxColumn.DataPropertyName = "RatingCount"
        Me.RatingCountDataGridViewTextBoxColumn.HeaderText = "Rating Count"
        Me.RatingCountDataGridViewTextBoxColumn.Name = "RatingCountDataGridViewTextBoxColumn"
        Me.RatingCountDataGridViewTextBoxColumn.ReadOnly = True
        Me.RatingCountDataGridViewTextBoxColumn.Visible = False
        Me.RatingCountDataGridViewTextBoxColumn.Width = 5
        '
        'LastUpdatedDtDataGridViewTextBoxColumn
        '
        Me.LastUpdatedDtDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCellsExceptHeader
        Me.LastUpdatedDtDataGridViewTextBoxColumn.DataPropertyName = "LastUpdatedDt"
        Me.LastUpdatedDtDataGridViewTextBoxColumn.HeaderText = "Last Updated Datetime"
        Me.LastUpdatedDtDataGridViewTextBoxColumn.Name = "LastUpdatedDtDataGridViewTextBoxColumn"
        Me.LastUpdatedDtDataGridViewTextBoxColumn.ReadOnly = True
        Me.LastUpdatedDtDataGridViewTextBoxColumn.Width = 5
        '
        'LastUpdatedUserDataGridViewTextBoxColumn
        '
        Me.LastUpdatedUserDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCellsExceptHeader
        Me.LastUpdatedUserDataGridViewTextBoxColumn.DataPropertyName = "LastUpdatedUser"
        Me.LastUpdatedUserDataGridViewTextBoxColumn.HeaderText = "Last Updated User"
        Me.LastUpdatedUserDataGridViewTextBoxColumn.Name = "LastUpdatedUserDataGridViewTextBoxColumn"
        Me.LastUpdatedUserDataGridViewTextBoxColumn.ReadOnly = True
        Me.LastUpdatedUserDataGridViewTextBoxColumn.Width = 5
        '
        'DialogRunbookTopic
        '
        Me.AcceptButton = Me.OK_Button
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.Cancel_Button
        Me.ClientSize = New System.Drawing.Size(468, 398)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "DialogRunbookTopic"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "DialogRunbookTopic"
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        CType(Me.TopicDataGridView, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TTopicBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DataSetSQLRunbook, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents OK_Button As System.Windows.Forms.Button
    Friend WithEvents Cancel_Button As System.Windows.Forms.Button
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents TopicDataGridView As System.Windows.Forms.DataGridView
    Friend WithEvents DataSetSQLRunbook As SQLClue.DataSetSQLRunbook
    Friend WithEvents TTopicBindingSource As System.Windows.Forms.BindingSource
    Friend WithEvents TTopicTableAdapter As SQLClue.DataSetSQLRunbookTableAdapters.tTopicTableAdapter
    Friend WithEvents DataGridViewTextBoxColumnId As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumnName As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents NotesDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents OwnerDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents RecCreatedDtDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents RecCreatedUserDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumnNbrDocuments As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents RatingTallyDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents RatingCountDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents LastUpdatedDtDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents LastUpdatedUserDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn

End Class
