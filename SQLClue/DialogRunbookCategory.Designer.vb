<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class DialogRunbookCategory
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(DialogRunbookCategory))
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel
        Me.OK_Button = New System.Windows.Forms.Button
        Me.Cancel_Button = New System.Windows.Forms.Button
        Me.TSQLRunbookCategoryBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.DsSQLRunbook = New SQLClue.DataSetSQLRunbook
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.Label1 = New System.Windows.Forms.Label
        Me.CategoryDataGridView = New System.Windows.Forms.DataGridView
        Me.DataGridViewButtonColumnCategoryRating = New System.Windows.Forms.DataGridViewButtonColumn
        Me.DataGridViewTextBoxColumnCategoryId = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.DataGridViewTextBoxColumnCategoryName = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.DataGridViewButtonColumnCategoryNotes = New System.Windows.Forms.DataGridViewButtonColumn
        Me.DataGridViewTextBoxColumnCategoryRecCreatedDt = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.DataGridViewTextBoxColumnCategoryRecCreatedUser = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.DataGridViewTextBoxColumnNbrTopics = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.DataGridViewTextBoxColumnCategoryRatingTally = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.DataGridViewTextBoxColumnCategoryRatingCount = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.TableLayoutPanel1.SuspendLayout()
        CType(Me.TSQLRunbookCategoryBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DsSQLRunbook, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel1.SuspendLayout()
        CType(Me.CategoryDataGridView, System.ComponentModel.ISupportInitialize).BeginInit()
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
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(270, 323)
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
        'TSQLRunbookCategoryBindingSource
        '
        Me.TSQLRunbookCategoryBindingSource.DataMember = "tCategory"
        Me.TSQLRunbookCategoryBindingSource.DataSource = Me.DsSQLRunbook
        '
        'DsSQLRunbook
        '
        Me.DsSQLRunbook.DataSetName = "DataSetSQLRunbook"
        Me.DsSQLRunbook.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        '
        'Panel1
        '
        Me.Panel1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Controls.Add(Me.CategoryDataGridView)
        Me.Panel1.Location = New System.Drawing.Point(5, 5)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(459, 314)
        Me.Panel1.TabIndex = 3
        '
        'Label1
        '
        Me.Label1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label1.Location = New System.Drawing.Point(4, 4)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(449, 47)
        Me.Label1.TabIndex = 4
        Me.Label1.Text = resources.GetString("Label1.Text")
        '
        'CategoryDataGridView
        '
        Me.CategoryDataGridView.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.CategoryDataGridView.AutoGenerateColumns = False
        Me.CategoryDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.CategoryDataGridView.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.CategoryDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.CategoryDataGridView.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.DataGridViewButtonColumnCategoryRating, Me.DataGridViewTextBoxColumnCategoryId, Me.DataGridViewTextBoxColumnCategoryName, Me.DataGridViewButtonColumnCategoryNotes, Me.DataGridViewTextBoxColumnCategoryRecCreatedDt, Me.DataGridViewTextBoxColumnCategoryRecCreatedUser, Me.DataGridViewTextBoxColumnNbrTopics, Me.DataGridViewTextBoxColumnCategoryRatingTally, Me.DataGridViewTextBoxColumnCategoryRatingCount})
        Me.CategoryDataGridView.DataSource = Me.TSQLRunbookCategoryBindingSource
        Me.CategoryDataGridView.Location = New System.Drawing.Point(4, 55)
        Me.CategoryDataGridView.Margin = New System.Windows.Forms.Padding(0)
        Me.CategoryDataGridView.MultiSelect = False
        Me.CategoryDataGridView.Name = "CategoryDataGridView"
        Me.CategoryDataGridView.Size = New System.Drawing.Size(449, 253)
        Me.CategoryDataGridView.TabIndex = 3
        '
        'DataGridViewButtonColumnCategoryRating
        '
        Me.DataGridViewButtonColumnCategoryRating.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
        Me.DataGridViewButtonColumnCategoryRating.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.DataGridViewButtonColumnCategoryRating.HeaderText = "Rating"
        Me.DataGridViewButtonColumnCategoryRating.MinimumWidth = 50
        Me.DataGridViewButtonColumnCategoryRating.Name = "DataGridViewButtonColumnCategoryRating"
        Me.DataGridViewButtonColumnCategoryRating.ReadOnly = True
        Me.DataGridViewButtonColumnCategoryRating.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        Me.DataGridViewButtonColumnCategoryRating.Width = 63
        '
        'DataGridViewTextBoxColumnCategoryId
        '
        Me.DataGridViewTextBoxColumnCategoryId.DataPropertyName = "Id"
        Me.DataGridViewTextBoxColumnCategoryId.HeaderText = "Id"
        Me.DataGridViewTextBoxColumnCategoryId.Name = "DataGridViewTextBoxColumnCategoryId"
        Me.DataGridViewTextBoxColumnCategoryId.ReadOnly = True
        Me.DataGridViewTextBoxColumnCategoryId.Visible = False
        '
        'DataGridViewTextBoxColumnCategoryName
        '
        Me.DataGridViewTextBoxColumnCategoryName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
        Me.DataGridViewTextBoxColumnCategoryName.DataPropertyName = "Name"
        Me.DataGridViewTextBoxColumnCategoryName.HeaderText = "Name"
        Me.DataGridViewTextBoxColumnCategoryName.MinimumWidth = 100
        Me.DataGridViewTextBoxColumnCategoryName.Name = "DataGridViewTextBoxColumnCategoryName"
        '
        'DataGridViewButtonColumnCategoryNotes
        '
        Me.DataGridViewButtonColumnCategoryNotes.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.DataGridViewButtonColumnCategoryNotes.DataPropertyName = "Notes"
        Me.DataGridViewButtonColumnCategoryNotes.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.DataGridViewButtonColumnCategoryNotes.HeaderText = "Notes"
        Me.DataGridViewButtonColumnCategoryNotes.MinimumWidth = 60
        Me.DataGridViewButtonColumnCategoryNotes.Name = "DataGridViewButtonColumnCategoryNotes"
        Me.DataGridViewButtonColumnCategoryNotes.ReadOnly = True
        Me.DataGridViewButtonColumnCategoryNotes.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.DataGridViewButtonColumnCategoryNotes.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        Me.DataGridViewButtonColumnCategoryNotes.Text = Global.SQLClue.My.Resources.Resources.HelpOverview
        Me.DataGridViewButtonColumnCategoryNotes.ToolTipText = "Select to view all Notes for Category"
        Me.DataGridViewButtonColumnCategoryNotes.Width = 60
        '
        'DataGridViewTextBoxColumnCategoryRecCreatedDt
        '
        Me.DataGridViewTextBoxColumnCategoryRecCreatedDt.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
        Me.DataGridViewTextBoxColumnCategoryRecCreatedDt.DataPropertyName = "RecCreatedDt"
        Me.DataGridViewTextBoxColumnCategoryRecCreatedDt.HeaderText = "Rec Created Dt"
        Me.DataGridViewTextBoxColumnCategoryRecCreatedDt.Name = "DataGridViewTextBoxColumnCategoryRecCreatedDt"
        Me.DataGridViewTextBoxColumnCategoryRecCreatedDt.Visible = False
        Me.DataGridViewTextBoxColumnCategoryRecCreatedDt.Width = 106
        '
        'DataGridViewTextBoxColumnCategoryRecCreatedUser
        '
        Me.DataGridViewTextBoxColumnCategoryRecCreatedUser.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
        Me.DataGridViewTextBoxColumnCategoryRecCreatedUser.DataPropertyName = "RecCreatedUser"
        Me.DataGridViewTextBoxColumnCategoryRecCreatedUser.HeaderText = "Rec Created User"
        Me.DataGridViewTextBoxColumnCategoryRecCreatedUser.Name = "DataGridViewTextBoxColumnCategoryRecCreatedUser"
        Me.DataGridViewTextBoxColumnCategoryRecCreatedUser.Visible = False
        Me.DataGridViewTextBoxColumnCategoryRecCreatedUser.Width = 117
        '
        'DataGridViewTextBoxColumnNbrTopics
        '
        Me.DataGridViewTextBoxColumnNbrTopics.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCellsExceptHeader
        Me.DataGridViewTextBoxColumnNbrTopics.DataPropertyName = "TopicCount"
        Me.DataGridViewTextBoxColumnNbrTopics.HeaderText = "Topics Assigned"
        Me.DataGridViewTextBoxColumnNbrTopics.MinimumWidth = 60
        Me.DataGridViewTextBoxColumnNbrTopics.Name = "DataGridViewTextBoxColumnNbrTopics"
        Me.DataGridViewTextBoxColumnNbrTopics.ReadOnly = True
        Me.DataGridViewTextBoxColumnNbrTopics.Width = 60
        '
        'DataGridViewTextBoxColumnCategoryRatingTally
        '
        Me.DataGridViewTextBoxColumnCategoryRatingTally.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCellsExceptHeader
        Me.DataGridViewTextBoxColumnCategoryRatingTally.DataPropertyName = "RatingTally"
        Me.DataGridViewTextBoxColumnCategoryRatingTally.HeaderText = "Rating Tally"
        Me.DataGridViewTextBoxColumnCategoryRatingTally.Name = "DataGridViewTextBoxColumnCategoryRatingTally"
        Me.DataGridViewTextBoxColumnCategoryRatingTally.ReadOnly = True
        Me.DataGridViewTextBoxColumnCategoryRatingTally.Visible = False
        Me.DataGridViewTextBoxColumnCategoryRatingTally.Width = 21
        '
        'DataGridViewTextBoxColumnCategoryRatingCount
        '
        Me.DataGridViewTextBoxColumnCategoryRatingCount.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCellsExceptHeader
        Me.DataGridViewTextBoxColumnCategoryRatingCount.DataPropertyName = "RatingCount"
        Me.DataGridViewTextBoxColumnCategoryRatingCount.HeaderText = "Rating Count"
        Me.DataGridViewTextBoxColumnCategoryRatingCount.Name = "DataGridViewTextBoxColumnCategoryRatingCount"
        Me.DataGridViewTextBoxColumnCategoryRatingCount.ReadOnly = True
        Me.DataGridViewTextBoxColumnCategoryRatingCount.Visible = False
        Me.DataGridViewTextBoxColumnCategoryRatingCount.Width = 21
        '
        'DialogRunbookCategory
        '
        Me.AcceptButton = Me.OK_Button
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.Cancel_Button
        Me.ClientSize = New System.Drawing.Size(469, 352)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.Controls.Add(Me.Panel1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow
        Me.HelpButton = True
        Me.MinimumSize = New System.Drawing.Size(485, 386)
        Me.Name = "DialogRunbookCategory"
        Me.Padding = New System.Windows.Forms.Padding(4)
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Category Maintenance"
        Me.TableLayoutPanel1.ResumeLayout(False)
        CType(Me.TSQLRunbookCategoryBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DsSQLRunbook, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel1.ResumeLayout(False)
        CType(Me.CategoryDataGridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents OK_Button As System.Windows.Forms.Button
    Friend WithEvents Cancel_Button As System.Windows.Forms.Button
    Friend WithEvents TSQLRunbookCategoryBindingSource As System.Windows.Forms.BindingSource
    Friend WithEvents DsSQLRunbook As SQLClue.DataSetSQLRunbook
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents CategoryDataGridView As System.Windows.Forms.DataGridView
    Friend WithEvents DataGridViewButtonColumnCategoryRating As System.Windows.Forms.DataGridViewButtonColumn
    Friend WithEvents DataGridViewTextBoxColumnCategoryId As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumnCategoryName As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewButtonColumnCategoryNotes As System.Windows.Forms.DataGridViewButtonColumn
    Friend WithEvents DataGridViewTextBoxColumnCategoryRecCreatedDt As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumnCategoryRecCreatedUser As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumnNbrTopics As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumnCategoryRatingTally As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumnCategoryRatingCount As System.Windows.Forms.DataGridViewTextBoxColumn

End Class
