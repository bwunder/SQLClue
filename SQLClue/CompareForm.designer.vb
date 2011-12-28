<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class CompareForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.StatusStripCompare = New System.Windows.Forms.StatusStrip
        Me.ToolStripStatusLabelCompare = New System.Windows.Forms.ToolStripStatusLabel
        Me.ToolStripStatusLabel1 = New System.Windows.Forms.ToolStripStatusLabel
        Me.ToolStripStatusLabel2 = New System.Windows.Forms.ToolStripStatusLabel
        Me.ToolStripProgressBar1 = New System.Windows.Forms.ToolStripProgressBar
        Me.FileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.OpenSettingsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.SaveSettingsToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.AdvancedOptions = New System.Windows.Forms.ToolStripMenuItem
        Me.HelpToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.AboutToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ReadmetxtToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.PanelResultsKey = New System.Windows.Forms.Panel
        Me.Label3 = New System.Windows.Forms.Label
        Me.KeyHeaderLabel = New System.Windows.Forms.Label
        Me.KeyMatchLabel = New System.Windows.Forms.Label
        Me.KeyNoMatchLabel = New System.Windows.Forms.Label
        Me.KeyDifferentLabel = New System.Windows.Forms.Label
        Me.CompareButton = New System.Windows.Forms.Button
        Me.Cancel_Button = New System.Windows.Forms.Button
        Me.SplitContainer2 = New System.Windows.Forms.SplitContainer
        Me.RichTextBox2 = New System.Windows.Forms.RichTextBox
        Me.ContextMenuStripCompareOutput = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.MenuItemZoomIn = New System.Windows.Forms.ToolStripMenuItem
        Me.MenuItemZoomNormal = New System.Windows.Forms.ToolStripMenuItem
        Me.MenuItemZoomOut = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator
        Me.SaveAsRichText = New System.Windows.Forms.ToolStripMenuItem
        Me.LabelInstance2 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.HierarchyLabel2 = New System.Windows.Forms.Label
        Me.Origin2Label = New System.Windows.Forms.Label
        Me.Origin2 = New System.Windows.Forms.ComboBox
        Me.smoTreeView2 = New System.Windows.Forms.TreeView
        Me.Instance2 = New System.Windows.Forms.ComboBox
        Me.SplitContainer3 = New System.Windows.Forms.SplitContainer
        Me.LabelInstance1 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.HierarchyLabel1 = New System.Windows.Forms.Label
        Me.smoTreeView1 = New System.Windows.Forms.TreeView
        Me.Origin1Label = New System.Windows.Forms.Label
        Me.Origin1 = New System.Windows.Forms.ComboBox
        Me.Instance1 = New System.Windows.Forms.ComboBox
        Me.RichTextBox1 = New System.Windows.Forms.RichTextBox
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer
        Me.FolderBrowserDialogCompare = New System.Windows.Forms.FolderBrowserDialog
        Me.ToolTipCompare = New System.Windows.Forms.ToolTip(Me.components)
        Me.ButtonMaxInput = New System.Windows.Forms.Button
        Me.ButtonMaxOutput = New System.Windows.Forms.Button
        Me.SaveFileDialogCompareOutput = New System.Windows.Forms.SaveFileDialog
        Me.Label7 = New System.Windows.Forms.Label
        Me.Label8 = New System.Windows.Forms.Label
        Me.ContextMenuStripTreeItemA = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ToolStripMenuItemNodeSetVersionA = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripComboBoxVersionA = New System.Windows.Forms.ToolStripComboBox
        Me.ToolStripContainer1 = New System.Windows.Forms.ToolStripContainer
        Me.SplitContainer4 = New System.Windows.Forms.SplitContainer
        Me.ContextMenuStripTreeItemB = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ToolStripMenuItemNodeSetVersionB = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripComboBoxVersionB = New System.Windows.Forms.ToolStripComboBox
        Me.PanelResultsKey.SuspendLayout()
        Me.SplitContainer2.Panel1.SuspendLayout()
        Me.SplitContainer2.Panel2.SuspendLayout()
        Me.SplitContainer2.SuspendLayout()
        Me.ContextMenuStripCompareOutput.SuspendLayout()
        Me.SplitContainer3.Panel1.SuspendLayout()
        Me.SplitContainer3.Panel2.SuspendLayout()
        Me.SplitContainer3.SuspendLayout()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        Me.ContextMenuStripTreeItemA.SuspendLayout()
        Me.ToolStripContainer1.BottomToolStripPanel.SuspendLayout()
        Me.ToolStripContainer1.ContentPanel.SuspendLayout()
        Me.ToolStripContainer1.SuspendLayout()
        Me.SplitContainer4.Panel1.SuspendLayout()
        Me.SplitContainer4.Panel2.SuspendLayout()
        Me.SplitContainer4.SuspendLayout()
        Me.ContextMenuStripTreeItemB.SuspendLayout()
        Me.SuspendLayout()
        '
        'StatusStripCompare
        '
        Me.StatusStripCompare.BackColor = System.Drawing.SystemColors.Control
        Me.StatusStripCompare.Dock = System.Windows.Forms.DockStyle.None
        Me.StatusStripCompare.Location = New System.Drawing.Point(0, 0)
        Me.StatusStripCompare.Name = "StatusStripCompare"
        Me.StatusStripCompare.Size = New System.Drawing.Size(730, 22)
        Me.StatusStripCompare.TabIndex = 0
        Me.StatusStripCompare.Text = "StatusStripCompare"
        '
        'ToolStripStatusLabelCompare
        '
        Me.ToolStripStatusLabelCompare.AutoSize = False
        Me.ToolStripStatusLabelCompare.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.ToolStripStatusLabelCompare.Name = "ToolStripStatusLabelCompare"
        Me.ToolStripStatusLabelCompare.Size = New System.Drawing.Size(139, 25)
        Me.ToolStripStatusLabelCompare.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ToolStripStatusLabel1
        '
        Me.ToolStripStatusLabel1.AutoSize = False
        Me.ToolStripStatusLabel1.BackColor = System.Drawing.SystemColors.Control
        Me.ToolStripStatusLabel1.BorderSides = CType((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Right), System.Windows.Forms.ToolStripStatusLabelBorderSides)
        Me.ToolStripStatusLabel1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.ToolStripStatusLabel1.Margin = New System.Windows.Forms.Padding(0, 0, 4, 0)
        Me.ToolStripStatusLabel1.MergeAction = System.Windows.Forms.MergeAction.Replace
        Me.ToolStripStatusLabel1.Name = "ToolStripStatusLabel1"
        Me.ToolStripStatusLabel1.Size = New System.Drawing.Size(218, 25)
        Me.ToolStripStatusLabel1.Tag = "A:"
        Me.ToolStripStatusLabel1.Text = "ToolStripStatusLabel1"
        Me.ToolStripStatusLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ToolStripStatusLabel2
        '
        Me.ToolStripStatusLabel2.AutoSize = False
        Me.ToolStripStatusLabel2.BackColor = System.Drawing.SystemColors.Control
        Me.ToolStripStatusLabel2.BorderSides = CType((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left Or System.Windows.Forms.ToolStripStatusLabelBorderSides.Right), System.Windows.Forms.ToolStripStatusLabelBorderSides)
        Me.ToolStripStatusLabel2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.ToolStripStatusLabel2.Margin = New System.Windows.Forms.Padding(0, 0, 4, 0)
        Me.ToolStripStatusLabel2.MergeIndex = 2
        Me.ToolStripStatusLabel2.Name = "ToolStripStatusLabel2"
        Me.ToolStripStatusLabel2.Size = New System.Drawing.Size(222, 25)
        Me.ToolStripStatusLabel2.Tag = "B:"
        Me.ToolStripStatusLabel2.Text = "ToolStripStatusLabel2"
        Me.ToolStripStatusLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ToolStripProgressBar1
        '
        Me.ToolStripProgressBar1.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.ToolStripProgressBar1.MergeIndex = -3
        Me.ToolStripProgressBar1.Name = "ToolStripProgressBar1"
        Me.ToolStripProgressBar1.Size = New System.Drawing.Size(100, 19)
        '
        'FileToolStripMenuItem
        '
        Me.FileToolStripMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.FileToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.OpenSettingsToolStripMenuItem, Me.SaveSettingsToolStripMenuItem1, Me.ExitToolStripMenuItem})
        Me.FileToolStripMenuItem.Name = "FileToolStripMenuItem"
        Me.FileToolStripMenuItem.Size = New System.Drawing.Size(35, 20)
        Me.FileToolStripMenuItem.Text = "&File"
        '
        'OpenSettingsToolStripMenuItem
        '
        Me.OpenSettingsToolStripMenuItem.Name = "OpenSettingsToolStripMenuItem"
        Me.OpenSettingsToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F2
        Me.OpenSettingsToolStripMenuItem.Size = New System.Drawing.Size(231, 22)
        Me.OpenSettingsToolStripMenuItem.Text = "&Open Saved Settings..."
        '
        'SaveSettingsToolStripMenuItem1
        '
        Me.SaveSettingsToolStripMenuItem1.Name = "SaveSettingsToolStripMenuItem1"
        Me.SaveSettingsToolStripMenuItem1.ShortcutKeys = System.Windows.Forms.Keys.F3
        Me.SaveSettingsToolStripMenuItem1.Size = New System.Drawing.Size(231, 22)
        Me.SaveSettingsToolStripMenuItem1.Text = "&Save Current Settings as ..."
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Alt Or System.Windows.Forms.Keys.X), System.Windows.Forms.Keys)
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(231, 22)
        Me.ExitToolStripMenuItem.Text = "E&xit"
        '
        'AdvancedOptions
        '
        Me.AdvancedOptions.Name = "AdvancedOptions"
        Me.AdvancedOptions.Size = New System.Drawing.Size(107, 20)
        Me.AdvancedOptions.Text = "&Advanced Options"
        '
        'HelpToolStripMenuItem
        '
        Me.HelpToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AboutToolStripMenuItem, Me.ReadmetxtToolStripMenuItem})
        Me.HelpToolStripMenuItem.Name = "HelpToolStripMenuItem"
        Me.HelpToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F1
        Me.HelpToolStripMenuItem.Size = New System.Drawing.Size(40, 20)
        Me.HelpToolStripMenuItem.Text = "&Help"
        '
        'AboutToolStripMenuItem
        '
        Me.AboutToolStripMenuItem.Name = "AboutToolStripMenuItem"
        Me.AboutToolStripMenuItem.Size = New System.Drawing.Size(158, 22)
        Me.AboutToolStripMenuItem.Text = "&About..."
        '
        'ReadmetxtToolStripMenuItem
        '
        Me.ReadmetxtToolStripMenuItem.Name = "ReadmetxtToolStripMenuItem"
        Me.ReadmetxtToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F1
        Me.ReadmetxtToolStripMenuItem.Size = New System.Drawing.Size(158, 22)
        Me.ReadmetxtToolStripMenuItem.Text = "&readme.txt..."
        '
        'PanelResultsKey
        '
        Me.PanelResultsKey.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.PanelResultsKey.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.PanelResultsKey.Controls.Add(Me.Label3)
        Me.PanelResultsKey.Controls.Add(Me.KeyHeaderLabel)
        Me.PanelResultsKey.Controls.Add(Me.KeyMatchLabel)
        Me.PanelResultsKey.Controls.Add(Me.KeyNoMatchLabel)
        Me.PanelResultsKey.Controls.Add(Me.KeyDifferentLabel)
        Me.PanelResultsKey.Location = New System.Drawing.Point(197, 359)
        Me.PanelResultsKey.Name = "PanelResultsKey"
        Me.PanelResultsKey.Size = New System.Drawing.Size(337, 19)
        Me.PanelResultsKey.TabIndex = 17
        '
        'Label3
        '
        Me.Label3.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.5!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(15, 3)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(60, 12)
        Me.Label3.TabIndex = 0
        Me.Label3.Text = "Results Key: "
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'KeyHeaderLabel
        '
        Me.KeyHeaderLabel.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.KeyHeaderLabel.BackColor = System.Drawing.Color.Silver
        Me.KeyHeaderLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.5!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.KeyHeaderLabel.ForeColor = System.Drawing.Color.Black
        Me.KeyHeaderLabel.Location = New System.Drawing.Point(280, 2)
        Me.KeyHeaderLabel.Margin = New System.Windows.Forms.Padding(10, 0, 10, 0)
        Me.KeyHeaderLabel.Name = "KeyHeaderLabel"
        Me.KeyHeaderLabel.Size = New System.Drawing.Size(40, 13)
        Me.KeyHeaderLabel.TabIndex = 1
        Me.KeyHeaderLabel.Text = "label"
        Me.KeyHeaderLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.ToolTipCompare.SetToolTip(Me.KeyHeaderLabel, "This is NOT compared. Added to output for clarity.")
        '
        'KeyMatchLabel
        '
        Me.KeyMatchLabel.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.KeyMatchLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.KeyMatchLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.5!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.KeyMatchLabel.Location = New System.Drawing.Point(77, 2)
        Me.KeyMatchLabel.Name = "KeyMatchLabel"
        Me.KeyMatchLabel.Size = New System.Drawing.Size(55, 13)
        Me.KeyMatchLabel.TabIndex = 2
        Me.KeyMatchLabel.Text = "matched"
        Me.KeyMatchLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.ToolTipCompare.SetToolTip(Me.KeyMatchLabel, "No differences using the specified compare fidelity.")
        '
        'KeyNoMatchLabel
        '
        Me.KeyNoMatchLabel.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.KeyNoMatchLabel.BackColor = System.Drawing.SystemColors.Highlight
        Me.KeyNoMatchLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.5!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.KeyNoMatchLabel.ForeColor = System.Drawing.SystemColors.HighlightText
        Me.KeyNoMatchLabel.Location = New System.Drawing.Point(201, 2)
        Me.KeyNoMatchLabel.Name = "KeyNoMatchLabel"
        Me.KeyNoMatchLabel.Size = New System.Drawing.Size(55, 13)
        Me.KeyNoMatchLabel.TabIndex = 4
        Me.KeyNoMatchLabel.Text = "unmatched"
        Me.KeyNoMatchLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.ToolTipCompare.SetToolTip(Me.KeyNoMatchLabel, "Found in one location, but not the other")
        '
        'KeyDifferentLabel
        '
        Me.KeyDifferentLabel.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.KeyDifferentLabel.BackColor = System.Drawing.SystemColors.ActiveCaption
        Me.KeyDifferentLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.KeyDifferentLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.5!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.KeyDifferentLabel.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.KeyDifferentLabel.Location = New System.Drawing.Point(139, 2)
        Me.KeyDifferentLabel.Name = "KeyDifferentLabel"
        Me.KeyDifferentLabel.Size = New System.Drawing.Size(55, 13)
        Me.KeyDifferentLabel.TabIndex = 3
        Me.KeyDifferentLabel.Text = "different"
        Me.KeyDifferentLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.ToolTipCompare.SetToolTip(Me.KeyDifferentLabel, "Different than the adjacent line in the other location.")
        '
        'CompareButton
        '
        Me.CompareButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.CompareButton.Location = New System.Drawing.Point(618, 357)
        Me.CompareButton.Name = "CompareButton"
        Me.CompareButton.Size = New System.Drawing.Size(95, 22)
        Me.CompareButton.TabIndex = 14
        Me.CompareButton.Text = "&Compare"
        Me.CompareButton.UseVisualStyleBackColor = True
        '
        'Cancel_Button
        '
        Me.Cancel_Button.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Cancel_Button.BackColor = System.Drawing.SystemColors.HotTrack
        Me.Cancel_Button.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.5!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Cancel_Button.ForeColor = System.Drawing.SystemColors.HighlightText
        Me.Cancel_Button.Location = New System.Drawing.Point(602, 358)
        Me.Cancel_Button.Name = "Cancel_Button"
        Me.Cancel_Button.Size = New System.Drawing.Size(107, 22)
        Me.Cancel_Button.TabIndex = 15
        Me.Cancel_Button.Text = "Cancel Compare"
        Me.Cancel_Button.UseVisualStyleBackColor = False
        '
        'SplitContainer2
        '
        Me.SplitContainer2.BackColor = System.Drawing.SystemColors.GradientInactiveCaption
        Me.SplitContainer2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.SplitContainer2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer2.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer2.Margin = New System.Windows.Forms.Padding(0)
        Me.SplitContainer2.Name = "SplitContainer2"
        '
        'SplitContainer2.Panel1
        '
        Me.SplitContainer2.Panel1.Controls.Add(Me.RichTextBox2)
        '
        'SplitContainer2.Panel2
        '
        Me.SplitContainer2.Panel2.BackColor = System.Drawing.SystemColors.Control
        Me.SplitContainer2.Panel2.Controls.Add(Me.LabelInstance2)
        Me.SplitContainer2.Panel2.Controls.Add(Me.Label2)
        Me.SplitContainer2.Panel2.Controls.Add(Me.HierarchyLabel2)
        Me.SplitContainer2.Panel2.Controls.Add(Me.Origin2Label)
        Me.SplitContainer2.Panel2.Controls.Add(Me.Origin2)
        Me.SplitContainer2.Panel2.Controls.Add(Me.smoTreeView2)
        Me.SplitContainer2.Panel2.Controls.Add(Me.Instance2)
        Me.SplitContainer2.Size = New System.Drawing.Size(359, 350)
        Me.SplitContainer2.SplitterDistance = 220
        Me.SplitContainer2.TabIndex = 11
        '
        'RichTextBox2
        '
        Me.RichTextBox2.AutoWordSelection = True
        Me.RichTextBox2.BackColor = System.Drawing.SystemColors.Window
        Me.RichTextBox2.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.RichTextBox2.ContextMenuStrip = Me.ContextMenuStripCompareOutput
        Me.RichTextBox2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.RichTextBox2.Font = New System.Drawing.Font("Lucida Console", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RichTextBox2.HideSelection = False
        Me.RichTextBox2.Location = New System.Drawing.Point(0, 0)
        Me.RichTextBox2.Margin = New System.Windows.Forms.Padding(1)
        Me.RichTextBox2.Name = "RichTextBox2"
        Me.RichTextBox2.ReadOnly = True
        Me.RichTextBox2.ShowSelectionMargin = True
        Me.RichTextBox2.Size = New System.Drawing.Size(218, 348)
        Me.RichTextBox2.TabIndex = 12
        Me.RichTextBox2.Text = "assigned in code"
        Me.RichTextBox2.WordWrap = False
        '
        'ContextMenuStripCompareOutput
        '
        Me.ContextMenuStripCompareOutput.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MenuItemZoomIn, Me.MenuItemZoomNormal, Me.MenuItemZoomOut, Me.ToolStripSeparator1, Me.SaveAsRichText})
        Me.ContextMenuStripCompareOutput.Name = "ZoomContextMenuStrip"
        Me.ContextMenuStripCompareOutput.ShowImageMargin = False
        Me.ContextMenuStripCompareOutput.Size = New System.Drawing.Size(219, 98)
        '
        'MenuItemZoomIn
        '
        Me.MenuItemZoomIn.Name = "MenuItemZoomIn"
        Me.MenuItemZoomIn.Size = New System.Drawing.Size(218, 22)
        Me.MenuItemZoomIn.Text = "zoom in..."
        '
        'MenuItemZoomNormal
        '
        Me.MenuItemZoomNormal.Checked = True
        Me.MenuItemZoomNormal.CheckState = System.Windows.Forms.CheckState.Checked
        Me.MenuItemZoomNormal.Name = "MenuItemZoomNormal"
        Me.MenuItemZoomNormal.Size = New System.Drawing.Size(218, 22)
        Me.MenuItemZoomNormal.Text = "zoom normal..."
        '
        'MenuItemZoomOut
        '
        Me.MenuItemZoomOut.Name = "MenuItemZoomOut"
        Me.MenuItemZoomOut.Size = New System.Drawing.Size(218, 22)
        Me.MenuItemZoomOut.Text = "zoom out..."
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(215, 6)
        '
        'SaveAsRichText
        '
        Me.SaveAsRichText.Name = "SaveAsRichText"
        Me.SaveAsRichText.Size = New System.Drawing.Size(218, 22)
        Me.SaveAsRichText.Text = "Save This Results Panel as Text..."
        '
        'LabelInstance2
        '
        Me.LabelInstance2.AutoSize = True
        Me.LabelInstance2.Location = New System.Drawing.Point(3, 59)
        Me.LabelInstance2.Name = "LabelInstance2"
        Me.LabelInstance2.Size = New System.Drawing.Size(48, 13)
        Me.LabelInstance2.TabIndex = 11
        Me.LabelInstance2.Text = "Instance"
        Me.ToolTipCompare.SetToolTip(Me.LabelInstance2, "SQL Instance or Folder")
        '
        'Label2
        '
        Me.Label2.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(0, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(133, 23)
        Me.Label2.TabIndex = 0
        Me.Label2.Text = "B"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'HierarchyLabel2
        '
        Me.HierarchyLabel2.AutoSize = True
        Me.HierarchyLabel2.Location = New System.Drawing.Point(3, 81)
        Me.HierarchyLabel2.Name = "HierarchyLabel2"
        Me.HierarchyLabel2.Size = New System.Drawing.Size(103, 13)
        Me.HierarchyLabel2.TabIndex = 0
        Me.HierarchyLabel2.Text = "Available Items Tree"
        Me.ToolTipCompare.SetToolTip(Me.HierarchyLabel2, "Select the Item or collection of items that you wish to compare. Selecting the na" & _
                "me instead of the check box of a collapsed node, toggles all decendant nodes to " & _
                "the same state.")
        '
        'Origin2Label
        '
        Me.Origin2Label.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Origin2Label.AutoSize = True
        Me.Origin2Label.Location = New System.Drawing.Point(3, 32)
        Me.Origin2Label.Name = "Origin2Label"
        Me.Origin2Label.Size = New System.Drawing.Size(34, 13)
        Me.Origin2Label.TabIndex = 0
        Me.Origin2Label.Text = "Origin"
        Me.ToolTipCompare.SetToolTip(Me.Origin2Label, "Document Source")
        '
        'Origin2
        '
        Me.Origin2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Origin2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.Origin2.DropDownWidth = 200
        Me.Origin2.FormattingEnabled = True
        Me.Origin2.Items.AddRange(New Object() {"SQLInstance", "Repository"})
        Me.Origin2.Location = New System.Drawing.Point(53, 29)
        Me.Origin2.Name = "Origin2"
        Me.Origin2.Size = New System.Drawing.Size(73, 21)
        Me.Origin2.TabIndex = 8
        Me.ToolTipCompare.SetToolTip(Me.Origin2, "Select an Origin")
        '
        'smoTreeView2
        '
        Me.smoTreeView2.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.smoTreeView2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.smoTreeView2.HideSelection = False
        Me.smoTreeView2.HotTracking = True
        Me.smoTreeView2.Indent = 15
        Me.smoTreeView2.Location = New System.Drawing.Point(4, 94)
        Me.smoTreeView2.Name = "smoTreeView2"
        Me.smoTreeView2.PathSeparator = "|"
        Me.smoTreeView2.Size = New System.Drawing.Size(125, 250)
        Me.smoTreeView2.TabIndex = 10
        '
        'Instance2
        '
        Me.Instance2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Instance2.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest
        Me.Instance2.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.Instance2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.Instance2.DropDownWidth = 200
        Me.Instance2.FormattingEnabled = True
        Me.Instance2.Location = New System.Drawing.Point(53, 56)
        Me.Instance2.Name = "Instance2"
        Me.Instance2.Size = New System.Drawing.Size(73, 21)
        Me.Instance2.TabIndex = 9
        Me.ToolTipCompare.SetToolTip(Me.Instance2, "Select or enter a SQL Instance or Filesystem Folder Name" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "after you select the Or" & _
                "igin.")
        '
        'SplitContainer3
        '
        Me.SplitContainer3.BackColor = System.Drawing.SystemColors.GradientInactiveCaption
        Me.SplitContainer3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.SplitContainer3.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer3.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer3.Margin = New System.Windows.Forms.Padding(0)
        Me.SplitContainer3.Name = "SplitContainer3"
        '
        'SplitContainer3.Panel1
        '
        Me.SplitContainer3.Panel1.BackColor = System.Drawing.SystemColors.Control
        Me.SplitContainer3.Panel1.Controls.Add(Me.LabelInstance1)
        Me.SplitContainer3.Panel1.Controls.Add(Me.Label1)
        Me.SplitContainer3.Panel1.Controls.Add(Me.HierarchyLabel1)
        Me.SplitContainer3.Panel1.Controls.Add(Me.smoTreeView1)
        Me.SplitContainer3.Panel1.Controls.Add(Me.Origin1Label)
        Me.SplitContainer3.Panel1.Controls.Add(Me.Origin1)
        Me.SplitContainer3.Panel1.Controls.Add(Me.Instance1)
        '
        'SplitContainer3.Panel2
        '
        Me.SplitContainer3.Panel2.Controls.Add(Me.RichTextBox1)
        Me.SplitContainer3.Size = New System.Drawing.Size(359, 350)
        Me.SplitContainer3.SplitterDistance = 139
        Me.SplitContainer3.TabIndex = 5
        '
        'LabelInstance1
        '
        Me.LabelInstance1.AutoSize = True
        Me.LabelInstance1.Location = New System.Drawing.Point(2, 59)
        Me.LabelInstance1.Name = "LabelInstance1"
        Me.LabelInstance1.Size = New System.Drawing.Size(48, 13)
        Me.LabelInstance1.TabIndex = 4
        Me.LabelInstance1.Text = "Instance"
        Me.ToolTipCompare.SetToolTip(Me.LabelInstance1, "SQL Instance or Folder")
        '
        'Label1
        '
        Me.Label1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(0, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(137, 23)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "A"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'HierarchyLabel1
        '
        Me.HierarchyLabel1.AutoSize = True
        Me.HierarchyLabel1.Location = New System.Drawing.Point(1, 81)
        Me.HierarchyLabel1.Name = "HierarchyLabel1"
        Me.HierarchyLabel1.Size = New System.Drawing.Size(103, 13)
        Me.HierarchyLabel1.TabIndex = 0
        Me.HierarchyLabel1.Text = "Available Items Tree"
        Me.ToolTipCompare.SetToolTip(Me.HierarchyLabel1, "Select the item or collection of items that you wish to compare. " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Server, JobSer" & _
                "ver, Database, and Service Broker are items.  ")
        '
        'smoTreeView1
        '
        Me.smoTreeView1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.smoTreeView1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.smoTreeView1.HideSelection = False
        Me.smoTreeView1.HotTracking = True
        Me.smoTreeView1.Indent = 15
        Me.smoTreeView1.Location = New System.Drawing.Point(4, 94)
        Me.smoTreeView1.Name = "smoTreeView1"
        Me.smoTreeView1.PathSeparator = "|"
        Me.smoTreeView1.Size = New System.Drawing.Size(129, 250)
        Me.smoTreeView1.TabIndex = 3
        Me.ToolTipCompare.SetToolTip(Me.smoTreeView1, "" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10))
        '
        'Origin1Label
        '
        Me.Origin1Label.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Origin1Label.AutoSize = True
        Me.Origin1Label.Location = New System.Drawing.Point(2, 32)
        Me.Origin1Label.Name = "Origin1Label"
        Me.Origin1Label.Size = New System.Drawing.Size(34, 13)
        Me.Origin1Label.TabIndex = 0
        Me.Origin1Label.Text = "Origin"
        Me.ToolTipCompare.SetToolTip(Me.Origin1Label, "Document Source Type")
        '
        'Origin1
        '
        Me.Origin1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Origin1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.Origin1.DropDownWidth = 200
        Me.Origin1.FormattingEnabled = True
        Me.Origin1.Items.AddRange(New Object() {"SQLInstance", "Repository"})
        Me.Origin1.Location = New System.Drawing.Point(50, 29)
        Me.Origin1.Name = "Origin1"
        Me.Origin1.Size = New System.Drawing.Size(79, 21)
        Me.Origin1.TabIndex = 1
        '
        'Instance1
        '
        Me.Instance1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Instance1.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest
        Me.Instance1.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.Instance1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.Instance1.DropDownWidth = 200
        Me.Instance1.FormattingEnabled = True
        Me.Instance1.Location = New System.Drawing.Point(50, 56)
        Me.Instance1.Name = "Instance1"
        Me.Instance1.Size = New System.Drawing.Size(79, 21)
        Me.Instance1.TabIndex = 2
        '
        'RichTextBox1
        '
        Me.RichTextBox1.AutoWordSelection = True
        Me.RichTextBox1.BackColor = System.Drawing.SystemColors.Window
        Me.RichTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.RichTextBox1.ContextMenuStrip = Me.ContextMenuStripCompareOutput
        Me.RichTextBox1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.RichTextBox1.Font = New System.Drawing.Font("Lucida Console", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RichTextBox1.HideSelection = False
        Me.RichTextBox1.Location = New System.Drawing.Point(0, 0)
        Me.RichTextBox1.Margin = New System.Windows.Forms.Padding(1)
        Me.RichTextBox1.Name = "RichTextBox1"
        Me.RichTextBox1.ReadOnly = True
        Me.RichTextBox1.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Horizontal
        Me.RichTextBox1.ShowSelectionMargin = True
        Me.RichTextBox1.Size = New System.Drawing.Size(214, 348)
        Me.RichTextBox1.TabIndex = 6
        Me.RichTextBox1.Text = "assigned in code"
        Me.RichTextBox1.WordWrap = False
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SplitContainer1.BackColor = System.Drawing.SystemColors.GradientInactiveCaption
        Me.SplitContainer1.Location = New System.Drawing.Point(4, 4)
        Me.SplitContainer1.Margin = New System.Windows.Forms.Padding(0)
        Me.SplitContainer1.Name = "SplitContainer1"
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.SplitContainer3)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.SplitContainer2)
        Me.SplitContainer1.Size = New System.Drawing.Size(722, 350)
        Me.SplitContainer1.SplitterDistance = 359
        Me.SplitContainer1.TabIndex = 7
        '
        'FolderBrowserDialogCompare
        '
        Me.FolderBrowserDialogCompare.ShowNewFolderButton = False
        '
        'ButtonMaxInput
        '
        Me.ButtonMaxInput.BackgroundImage = Global.SQLClue.My.Resources.Resources.CompOut
        Me.ButtonMaxInput.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.ButtonMaxInput.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ButtonMaxInput.FlatAppearance.BorderColor = System.Drawing.SystemColors.Window
        Me.ButtonMaxInput.FlatAppearance.BorderSize = 0
        Me.ButtonMaxInput.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonMaxInput.Location = New System.Drawing.Point(0, 0)
        Me.ButtonMaxInput.Margin = New System.Windows.Forms.Padding(0)
        Me.ButtonMaxInput.Name = "ButtonMaxInput"
        Me.ButtonMaxInput.Size = New System.Drawing.Size(24, 20)
        Me.ButtonMaxInput.TabIndex = 0
        Me.ToolTipCompare.SetToolTip(Me.ButtonMaxInput, "Toggle Collapse State of Input Panels ")
        Me.ButtonMaxInput.UseVisualStyleBackColor = True
        '
        'ButtonMaxOutput
        '
        Me.ButtonMaxOutput.BackgroundImage = Global.SQLClue.My.Resources.Resources.CompIn
        Me.ButtonMaxOutput.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.ButtonMaxOutput.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ButtonMaxOutput.FlatAppearance.BorderColor = System.Drawing.SystemColors.Window
        Me.ButtonMaxOutput.FlatAppearance.BorderSize = 0
        Me.ButtonMaxOutput.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonMaxOutput.Location = New System.Drawing.Point(0, 0)
        Me.ButtonMaxOutput.Margin = New System.Windows.Forms.Padding(0)
        Me.ButtonMaxOutput.Name = "ButtonMaxOutput"
        Me.ButtonMaxOutput.Size = New System.Drawing.Size(24, 20)
        Me.ButtonMaxOutput.TabIndex = 18
        Me.ToolTipCompare.SetToolTip(Me.ButtonMaxOutput, "Toggle Collapse State of Output Panels")
        Me.ButtonMaxOutput.UseVisualStyleBackColor = True
        '
        'SaveFileDialogCompareOutput
        '
        Me.SaveFileDialogCompareOutput.DefaultExt = "rtf"
        Me.SaveFileDialogCompareOutput.ShowHelp = True
        Me.SaveFileDialogCompareOutput.SupportMultiDottedExtensions = True
        '
        'Label7
        '
        Me.Label7.BackColor = System.Drawing.SystemColors.Control
        Me.Label7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label7.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label7.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label7.Location = New System.Drawing.Point(0, 0)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(36, 16)
        Me.Label7.TabIndex = 13
        Me.Label7.Text = "label"
        '
        'Label8
        '
        Me.Label8.BackColor = System.Drawing.SystemColors.Control
        Me.Label8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label8.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label8.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label8.Location = New System.Drawing.Point(0, 0)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(37, 16)
        Me.Label8.TabIndex = 12
        Me.Label8.Text = "label"
        '
        'ContextMenuStripTreeItemA
        '
        Me.ContextMenuStripTreeItemA.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItemNodeSetVersionA})
        Me.ContextMenuStripTreeItemA.Name = "ContextMenuStrip1"
        Me.ContextMenuStripTreeItemA.Size = New System.Drawing.Size(320, 26)
        '
        'ToolStripMenuItemNodeSetVersionA
        '
        Me.ToolStripMenuItemNodeSetVersionA.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripComboBoxVersionA})
        Me.ToolStripMenuItemNodeSetVersionA.Name = "ToolStripMenuItemNodeSetVersionA"
        Me.ToolStripMenuItemNodeSetVersionA.Size = New System.Drawing.Size(319, 22)
        Me.ToolStripMenuItemNodeSetVersionA.Text = "Set Version  - for Item -vs- Item Compare Only"
        '
        'ToolStripComboBoxVersionA
        '
        Me.ToolStripComboBoxVersionA.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ToolStripComboBoxVersionA.DropDownWidth = 60
        Me.ToolStripComboBoxVersionA.Name = "ToolStripComboBoxVersionA"
        Me.ToolStripComboBoxVersionA.Size = New System.Drawing.Size(75, 23)
        '
        'ToolStripContainer1
        '
        '
        'ToolStripContainer1.BottomToolStripPanel
        '
        Me.ToolStripContainer1.BottomToolStripPanel.Controls.Add(Me.StatusStripCompare)
        '
        'ToolStripContainer1.ContentPanel
        '
        Me.ToolStripContainer1.ContentPanel.Controls.Add(Me.SplitContainer1)
        Me.ToolStripContainer1.ContentPanel.Controls.Add(Me.CompareButton)
        Me.ToolStripContainer1.ContentPanel.Controls.Add(Me.Cancel_Button)
        Me.ToolStripContainer1.ContentPanel.Controls.Add(Me.PanelResultsKey)
        Me.ToolStripContainer1.ContentPanel.Controls.Add(Me.SplitContainer4)
        Me.ToolStripContainer1.ContentPanel.Size = New System.Drawing.Size(730, 383)
        Me.ToolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ToolStripContainer1.Location = New System.Drawing.Point(0, 0)
        Me.ToolStripContainer1.Name = "ToolStripContainer1"
        Me.ToolStripContainer1.Size = New System.Drawing.Size(730, 430)
        Me.ToolStripContainer1.TabIndex = 8
        Me.ToolStripContainer1.Text = "ToolStripContainer1"
        '
        'SplitContainer4
        '
        Me.SplitContainer4.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.SplitContainer4.IsSplitterFixed = True
        Me.SplitContainer4.Location = New System.Drawing.Point(21, 358)
        Me.SplitContainer4.Margin = New System.Windows.Forms.Padding(0)
        Me.SplitContainer4.Name = "SplitContainer4"
        '
        'SplitContainer4.Panel1
        '
        Me.SplitContainer4.Panel1.Controls.Add(Me.ButtonMaxOutput)
        Me.SplitContainer4.Panel1MinSize = 20
        '
        'SplitContainer4.Panel2
        '
        Me.SplitContainer4.Panel2.Controls.Add(Me.ButtonMaxInput)
        Me.SplitContainer4.Panel2MinSize = 20
        Me.SplitContainer4.Size = New System.Drawing.Size(50, 20)
        Me.SplitContainer4.SplitterDistance = 24
        Me.SplitContainer4.SplitterWidth = 2
        Me.SplitContainer4.TabIndex = 19
        '
        'ContextMenuStripTreeItemB
        '
        Me.ContextMenuStripTreeItemB.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItemNodeSetVersionB})
        Me.ContextMenuStripTreeItemB.Name = "ContextMenuStrip1"
        Me.ContextMenuStripTreeItemB.Size = New System.Drawing.Size(320, 26)
        '
        'ToolStripMenuItemNodeSetVersionB
        '
        Me.ToolStripMenuItemNodeSetVersionB.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripComboBoxVersionB})
        Me.ToolStripMenuItemNodeSetVersionB.Name = "ToolStripMenuItemNodeSetVersionB"
        Me.ToolStripMenuItemNodeSetVersionB.Size = New System.Drawing.Size(319, 22)
        Me.ToolStripMenuItemNodeSetVersionB.Text = "Set Version  - for Item -vs- Item Compare Only"
        '
        'ToolStripComboBoxVersionB
        '
        Me.ToolStripComboBoxVersionB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ToolStripComboBoxVersionB.DropDownWidth = 60
        Me.ToolStripComboBoxVersionB.Name = "ToolStripComboBoxVersionB"
        Me.ToolStripComboBoxVersionB.Size = New System.Drawing.Size(75, 23)
        '
        'CompareForm
        '
        Me.AcceptButton = Me.CompareButton
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Window
        Me.ClientSize = New System.Drawing.Size(730, 430)
        Me.Controls.Add(Me.ToolStripContainer1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "CompareForm"
        Me.Text = "Configuration Compare"
        Me.PanelResultsKey.ResumeLayout(False)
        Me.PanelResultsKey.PerformLayout()
        Me.SplitContainer2.Panel1.ResumeLayout(False)
        Me.SplitContainer2.Panel2.ResumeLayout(False)
        Me.SplitContainer2.Panel2.PerformLayout()
        Me.SplitContainer2.ResumeLayout(False)
        Me.ContextMenuStripCompareOutput.ResumeLayout(False)
        Me.SplitContainer3.Panel1.ResumeLayout(False)
        Me.SplitContainer3.Panel1.PerformLayout()
        Me.SplitContainer3.Panel2.ResumeLayout(False)
        Me.SplitContainer3.ResumeLayout(False)
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        Me.SplitContainer1.ResumeLayout(False)
        Me.ContextMenuStripTreeItemA.ResumeLayout(False)
        Me.ToolStripContainer1.BottomToolStripPanel.ResumeLayout(False)
        Me.ToolStripContainer1.BottomToolStripPanel.PerformLayout()
        Me.ToolStripContainer1.ContentPanel.ResumeLayout(False)
        Me.ToolStripContainer1.ResumeLayout(False)
        Me.ToolStripContainer1.PerformLayout()
        Me.SplitContainer4.Panel1.ResumeLayout(False)
        Me.SplitContainer4.Panel2.ResumeLayout(False)
        Me.SplitContainer4.ResumeLayout(False)
        Me.ContextMenuStripTreeItemB.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents StatusStripCompare As System.Windows.Forms.StatusStrip
    Friend WithEvents FileToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents OpenSettingsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SaveSettingsToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents AdvancedOptions As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents HelpToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents AboutToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ReadmetxtToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CompareButton As System.Windows.Forms.Button
    Friend WithEvents SplitContainer2 As System.Windows.Forms.SplitContainer
    Friend WithEvents RichTextBox2 As System.Windows.Forms.RichTextBox
    Friend WithEvents Origin2Label As System.Windows.Forms.Label
    Friend WithEvents SplitContainer3 As System.Windows.Forms.SplitContainer
    Friend WithEvents Origin1Label As System.Windows.Forms.Label
    Friend WithEvents RichTextBox1 As System.Windows.Forms.RichTextBox
    Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
    Friend WithEvents ToolStripStatusLabel1 As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents FolderBrowserDialogCompare As System.Windows.Forms.FolderBrowserDialog
    Friend WithEvents ToolStripStatusLabel2 As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents ToolTipCompare As System.Windows.Forms.ToolTip
    Friend WithEvents ContextMenuStripCompareOutput As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents MenuItemZoomIn As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuItemZoomNormal As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuItemZoomOut As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents SaveAsRichText As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SaveFileDialogCompareOutput As System.Windows.Forms.SaveFileDialog
    Friend WithEvents ToolStripProgressBar1 As System.Windows.Forms.ToolStripProgressBar
    Friend WithEvents Cancel_Button As System.Windows.Forms.Button
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents ContextMenuStripTreeItemA As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents ToolStripMenuItemNodeSetVersionA As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripComboBoxVersionA As System.Windows.Forms.ToolStripComboBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents KeyHeaderLabel As System.Windows.Forms.Label
    Friend WithEvents KeyMatchLabel As System.Windows.Forms.Label
    Friend WithEvents KeyDifferentLabel As System.Windows.Forms.Label
    Friend WithEvents KeyNoMatchLabel As System.Windows.Forms.Label
    Friend WithEvents PanelResultsKey As System.Windows.Forms.Panel
    Friend WithEvents LabelInstance2 As System.Windows.Forms.Label
    Friend WithEvents LabelInstance1 As System.Windows.Forms.Label
    Friend WithEvents ToolStripStatusLabelCompare As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents ToolStripContainer1 As System.Windows.Forms.ToolStripContainer
    Friend WithEvents SplitContainer4 As System.Windows.Forms.SplitContainer
    Friend WithEvents ButtonMaxOutput As System.Windows.Forms.Button
    Friend WithEvents ButtonMaxInput As System.Windows.Forms.Button
    Friend WithEvents ContextMenuStripTreeItemB As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents ToolStripMenuItemNodeSetVersionB As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripComboBoxVersionB As System.Windows.Forms.ToolStripComboBox
    Public WithEvents Origin1 As System.Windows.Forms.ComboBox
    Public WithEvents Instance1 As System.Windows.Forms.ComboBox
    Public WithEvents Origin2 As System.Windows.Forms.ComboBox
    Public WithEvents smoTreeView2 As System.Windows.Forms.TreeView
    Public WithEvents Instance2 As System.Windows.Forms.ComboBox
    Public WithEvents smoTreeView1 As System.Windows.Forms.TreeView
    Public WithEvents HierarchyLabel2 As System.Windows.Forms.Label
    Public WithEvents HierarchyLabel1 As System.Windows.Forms.Label

End Class
