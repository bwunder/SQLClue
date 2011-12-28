<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class DialogNotes
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(DialogNotes))
        Me.OK_Button = New System.Windows.Forms.Button
        Me.RichTextBoxNotes = New System.Windows.Forms.RichTextBox
        Me.ContextMenuStripZoom = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.MenuItemZoomIn = New System.Windows.Forms.ToolStripMenuItem
        Me.MenuItemZoomNormal = New System.Windows.Forms.ToolStripMenuItem
        Me.MenuItemZoomOut = New System.Windows.Forms.ToolStripMenuItem
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer
        Me.Cancel_Button = New System.Windows.Forms.Button
        Me.SplitContainer2 = New System.Windows.Forms.SplitContainer
        Me.FlowLayoutPanel1 = New System.Windows.Forms.FlowLayoutPanel
        Me.Label2 = New System.Windows.Forms.Label
        Me.LinkLabel1 = New System.Windows.Forms.LinkLabel
        Me.Label1 = New System.Windows.Forms.Label
        Me.RichTextBoxNewNote = New System.Windows.Forms.RichTextBox
        Me.ToolTipNotes = New System.Windows.Forms.ToolTip(Me.components)
        Me.ContextMenuStripZoom.SuspendLayout()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        Me.SplitContainer2.Panel1.SuspendLayout()
        Me.SplitContainer2.Panel2.SuspendLayout()
        Me.SplitContainer2.SuspendLayout()
        Me.FlowLayoutPanel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'OK_Button
        '
        Me.OK_Button.Dock = System.Windows.Forms.DockStyle.Fill
        Me.OK_Button.Location = New System.Drawing.Point(0, 0)
        Me.OK_Button.Name = "OK_Button"
        Me.OK_Button.Size = New System.Drawing.Size(93, 24)
        Me.OK_Button.TabIndex = 2
        Me.OK_Button.Text = "OK"
        '
        'RichTextBoxNotes
        '
        Me.RichTextBoxNotes.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.RichTextBoxNotes.BackColor = System.Drawing.SystemColors.Window
        Me.RichTextBoxNotes.ContextMenuStrip = Me.ContextMenuStripZoom
        Me.RichTextBoxNotes.Location = New System.Drawing.Point(4, 20)
        Me.RichTextBoxNotes.Margin = New System.Windows.Forms.Padding(0)
        Me.RichTextBoxNotes.Name = "RichTextBoxNotes"
        Me.RichTextBoxNotes.ReadOnly = True
        Me.RichTextBoxNotes.Size = New System.Drawing.Size(415, 270)
        Me.RichTextBoxNotes.TabIndex = 4
        Me.RichTextBoxNotes.Text = Global.SQLClue.My.Resources.Resources.HelpOverview
        '
        'ContextMenuStripZoom
        '
        Me.ContextMenuStripZoom.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MenuItemZoomIn, Me.MenuItemZoomNormal, Me.MenuItemZoomOut})
        Me.ContextMenuStripZoom.Name = "ZoomContextMenuStrip"
        Me.ContextMenuStripZoom.ShowCheckMargin = True
        Me.ContextMenuStripZoom.ShowImageMargin = False
        Me.ContextMenuStripZoom.Size = New System.Drawing.Size(155, 70)
        '
        'MenuItemZoomIn
        '
        Me.MenuItemZoomIn.Name = "MenuItemZoomIn"
        Me.MenuItemZoomIn.Size = New System.Drawing.Size(154, 22)
        Me.MenuItemZoomIn.Text = "zoom in..."
        '
        'MenuItemZoomNormal
        '
        Me.MenuItemZoomNormal.Name = "MenuItemZoomNormal"
        Me.MenuItemZoomNormal.Size = New System.Drawing.Size(154, 22)
        Me.MenuItemZoomNormal.Text = "zoom normal..."
        '
        'MenuItemZoomOut
        '
        Me.MenuItemZoomOut.Name = "MenuItemZoomOut"
        Me.MenuItemZoomOut.Size = New System.Drawing.Size(154, 22)
        Me.MenuItemZoomOut.Text = "zoom out..."
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SplitContainer1.IsSplitterFixed = True
        Me.SplitContainer1.Location = New System.Drawing.Point(235, 385)
        Me.SplitContainer1.Margin = New System.Windows.Forms.Padding(0)
        Me.SplitContainer1.Name = "SplitContainer1"
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.OK_Button)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.Cancel_Button)
        Me.SplitContainer1.Size = New System.Drawing.Size(194, 24)
        Me.SplitContainer1.SplitterDistance = 93
        Me.SplitContainer1.TabIndex = 3
        '
        'Cancel_Button
        '
        Me.Cancel_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Cancel_Button.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Cancel_Button.Location = New System.Drawing.Point(0, 0)
        Me.Cancel_Button.Name = "Cancel_Button"
        Me.Cancel_Button.Size = New System.Drawing.Size(97, 24)
        Me.Cancel_Button.TabIndex = 1
        Me.Cancel_Button.Text = "Cancel"
        Me.Cancel_Button.UseVisualStyleBackColor = True
        '
        'SplitContainer2
        '
        Me.SplitContainer2.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SplitContainer2.BackColor = System.Drawing.SystemColors.GradientInactiveCaption
        Me.SplitContainer2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.SplitContainer2.Location = New System.Drawing.Point(4, 4)
        Me.SplitContainer2.Margin = New System.Windows.Forms.Padding(0)
        Me.SplitContainer2.Name = "SplitContainer2"
        Me.SplitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainer2.Panel1
        '
        Me.SplitContainer2.Panel1.BackColor = System.Drawing.SystemColors.Control
        Me.SplitContainer2.Panel1.Controls.Add(Me.RichTextBoxNotes)
        Me.SplitContainer2.Panel1.Controls.Add(Me.FlowLayoutPanel1)
        Me.SplitContainer2.Panel1.Padding = New System.Windows.Forms.Padding(4)
        '
        'SplitContainer2.Panel2
        '
        Me.SplitContainer2.Panel2.BackColor = System.Drawing.SystemColors.Control
        Me.SplitContainer2.Panel2.Controls.Add(Me.Label1)
        Me.SplitContainer2.Panel2.Controls.Add(Me.RichTextBoxNewNote)
        Me.SplitContainer2.Panel2.Padding = New System.Windows.Forms.Padding(4)
        Me.SplitContainer2.Size = New System.Drawing.Size(425, 377)
        Me.SplitContainer2.SplitterDistance = 296
        Me.SplitContainer2.TabIndex = 4
        Me.ToolTipNotes.SetToolTip(Me.SplitContainer2, "\/ Move Splitter /\")
        '
        'FlowLayoutPanel1
        '
        Me.FlowLayoutPanel1.Controls.Add(Me.Label2)
        Me.FlowLayoutPanel1.Controls.Add(Me.LinkLabel1)
        Me.FlowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.FlowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft
        Me.FlowLayoutPanel1.Location = New System.Drawing.Point(4, 4)
        Me.FlowLayoutPanel1.Margin = New System.Windows.Forms.Padding(0)
        Me.FlowLayoutPanel1.Name = "FlowLayoutPanel1"
        Me.FlowLayoutPanel1.Size = New System.Drawing.Size(415, 17)
        Me.FlowLayoutPanel1.TabIndex = 4
        '
        'Label2
        '
        Me.Label2.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.5!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(327, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(85, 12)
        Me.Label2.TabIndex = 0
        Me.Label2.Text = "(not recommended)"
        Me.ToolTipNotes.SetToolTip(Me.Label2, "Changes will be not be tracked.")
        '
        'LinkLabel1
        '
        Me.LinkLabel1.AutoSize = True
        Me.LinkLabel1.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.5!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LinkLabel1.Location = New System.Drawing.Point(219, 0)
        Me.LinkLabel1.Name = "LinkLabel1"
        Me.LinkLabel1.Size = New System.Drawing.Size(102, 12)
        Me.LinkLabel1.TabIndex = 3
        Me.LinkLabel1.TabStop = True
        Me.LinkLabel1.Text = "edit previous notes"
        Me.LinkLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label1.Location = New System.Drawing.Point(4, 4)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(61, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Add a Note"
        '
        'RichTextBoxNewNote
        '
        Me.RichTextBoxNewNote.AcceptsTab = True
        Me.RichTextBoxNewNote.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.RichTextBoxNewNote.ContextMenuStrip = Me.ContextMenuStripZoom
        Me.RichTextBoxNewNote.DetectUrls = False
        Me.RichTextBoxNewNote.Location = New System.Drawing.Point(4, 20)
        Me.RichTextBoxNewNote.Margin = New System.Windows.Forms.Padding(0)
        Me.RichTextBoxNewNote.Name = "RichTextBoxNewNote"
        Me.RichTextBoxNewNote.Size = New System.Drawing.Size(415, 51)
        Me.RichTextBoxNewNote.TabIndex = 0
        Me.RichTextBoxNewNote.Text = Global.SQLClue.My.Resources.Resources.HelpOverview
        '
        'DialogNotes
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(434, 414)
        Me.Controls.Add(Me.SplitContainer2)
        Me.Controls.Add(Me.SplitContainer1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(375, 300)
        Me.Name = "DialogNotes"
        Me.Padding = New System.Windows.Forms.Padding(4)
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Notes"
        Me.ContextMenuStripZoom.ResumeLayout(False)
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        Me.SplitContainer1.ResumeLayout(False)
        Me.SplitContainer2.Panel1.ResumeLayout(False)
        Me.SplitContainer2.Panel2.ResumeLayout(False)
        Me.SplitContainer2.Panel2.PerformLayout()
        Me.SplitContainer2.ResumeLayout(False)
        Me.FlowLayoutPanel1.ResumeLayout(False)
        Me.FlowLayoutPanel1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents OK_Button As System.Windows.Forms.Button
    Friend WithEvents RichTextBoxNotes As System.Windows.Forms.RichTextBox
    Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
    Friend WithEvents Cancel_Button As System.Windows.Forms.Button
    Friend WithEvents SplitContainer2 As System.Windows.Forms.SplitContainer
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents RichTextBoxNewNote As System.Windows.Forms.RichTextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents LinkLabel1 As System.Windows.Forms.LinkLabel
    Friend WithEvents ContextMenuStripZoom As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents MenuItemZoomIn As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuItemZoomNormal As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuItemZoomOut As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents FlowLayoutPanel1 As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents ToolTipNotes As System.Windows.Forms.ToolTip

End Class
