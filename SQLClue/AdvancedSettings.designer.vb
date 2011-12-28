<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmOptions
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmOptions))
        Me.cmdApply = New System.Windows.Forms.Button
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.tlp = New System.Windows.Forms.TableLayoutPanel
        Me.chkSaveOnExit = New System.Windows.Forms.CheckBox
        Me.hp1 = New System.Windows.Forms.HelpProvider
        Me.cmdOK = New System.Windows.Forms.Button
        Me.ResetButton = New System.Windows.Forms.Button
        Me.ReloadButton = New System.Windows.Forms.Button
        Me.pgb = New System.Windows.Forms.ProgressBar
        Me.tvCategories = New System.Windows.Forms.TreeView
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label5 = New System.Windows.Forms.Label
        Me.Label6 = New System.Windows.Forms.Label
        Me.Label7 = New System.Windows.Forms.Label
        Me.Label8 = New System.Windows.Forms.Label
        Me.Label9 = New System.Windows.Forms.Label
        Me.Label10 = New System.Windows.Forms.Label
        Me.Label11 = New System.Windows.Forms.Label
        Me.Label12 = New System.Windows.Forms.Label
        Me.Label13 = New System.Windows.Forms.Label
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.LabelCategory = New System.Windows.Forms.Label
        Me.LabelSettings = New System.Windows.Forms.Label
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer
        Me.Button1 = New System.Windows.Forms.Button
        Me.Button2 = New System.Windows.Forms.Button
        Me.Button3 = New System.Windows.Forms.Button
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        Me.SuspendLayout()
        '
        'cmdApply
        '
        Me.cmdApply.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.hp1.SetHelpString(Me.cmdApply, "Applies the changes without closing the window.")
        Me.cmdApply.Location = New System.Drawing.Point(346, 321)
        Me.cmdApply.Name = "cmdApply"
        Me.hp1.SetShowHelp(Me.cmdApply, True)
        Me.cmdApply.Size = New System.Drawing.Size(95, 22)
        Me.cmdApply.TabIndex = 1
        Me.cmdApply.Text = "&Apply"
        Me.cmdApply.UseVisualStyleBackColor = True
        '
        'cmdCancel
        '
        Me.cmdCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.hp1.SetHelpString(Me.cmdCancel, "Closes the dialog box without applying the changes.")
        Me.cmdCancel.Location = New System.Drawing.Point(544, 321)
        Me.cmdCancel.Name = "cmdCancel"
        Me.hp1.SetShowHelp(Me.cmdCancel, True)
        Me.cmdCancel.Size = New System.Drawing.Size(95, 22)
        Me.cmdCancel.TabIndex = 2
        Me.cmdCancel.Text = "&Cancel"
        Me.cmdCancel.UseVisualStyleBackColor = True
        '
        'tlp
        '
        Me.tlp.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tlp.AutoScroll = True
        Me.tlp.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.tlp.BackColor = System.Drawing.SystemColors.Window
        Me.tlp.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.[Single]
        Me.tlp.ColumnCount = 2
        Me.tlp.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle)
        Me.tlp.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle)
        Me.hp1.SetHelpString(Me.tlp, "This pane contains the setting you are able to change from the selected category." & _
                "")
        Me.tlp.Location = New System.Drawing.Point(4, 20)
        Me.tlp.Margin = New System.Windows.Forms.Padding(0)
        Me.tlp.Name = "tlp"
        Me.tlp.RowCount = 1
        Me.tlp.RowStyles.Add(New System.Windows.Forms.RowStyle)
        Me.tlp.RowStyles.Add(New System.Windows.Forms.RowStyle)
        Me.tlp.RowStyles.Add(New System.Windows.Forms.RowStyle)
        Me.hp1.SetShowHelp(Me.tlp, True)
        Me.tlp.Size = New System.Drawing.Size(428, 286)
        Me.tlp.TabIndex = 4
        '
        'chkSaveOnExit
        '
        Me.chkSaveOnExit.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.chkSaveOnExit.AutoSize = True
        Me.hp1.SetHelpString(Me.chkSaveOnExit, "Determines if the application will save your settings when you close the applicat" & _
                "ion.")
        Me.chkSaveOnExit.Location = New System.Drawing.Point(16, 324)
        Me.chkSaveOnExit.Name = "chkSaveOnExit"
        Me.hp1.SetShowHelp(Me.chkSaveOnExit, True)
        Me.chkSaveOnExit.Size = New System.Drawing.Size(124, 17)
        Me.chkSaveOnExit.TabIndex = 5
        Me.chkSaveOnExit.Text = "&Save settings on exit"
        Me.chkSaveOnExit.UseVisualStyleBackColor = True
        '
        'cmdOK
        '
        Me.cmdOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.hp1.SetHelpString(Me.cmdOK, "Applies the settings and closes the dialog box.")
        Me.cmdOK.Location = New System.Drawing.Point(445, 321)
        Me.cmdOK.Name = "cmdOK"
        Me.hp1.SetShowHelp(Me.cmdOK, True)
        Me.cmdOK.Size = New System.Drawing.Size(95, 22)
        Me.cmdOK.TabIndex = 3
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.UseVisualStyleBackColor = True
        '
        'ResetButton
        '
        Me.ResetButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ResetButton.BackColor = System.Drawing.Color.Red
        Me.ResetButton.ForeColor = System.Drawing.SystemColors.HighlightText
        Me.hp1.SetHelpString(Me.ResetButton, "Revert all settings to app.config defaults. Removes all user.config settings for " & _
                "this user..")
        Me.ResetButton.Location = New System.Drawing.Point(175, 322)
        Me.ResetButton.Name = "ResetButton"
        Me.hp1.SetShowHelp(Me.ResetButton, True)
        Me.ResetButton.Size = New System.Drawing.Size(69, 22)
        Me.ResetButton.TabIndex = 6
        Me.ResetButton.Text = "&Reset All"
        Me.ToolTip1.SetToolTip(Me.ResetButton, "Reset SQLClue to application defaults. " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "WARNING! Clears all user.confg values. " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "All your settings - including all cached " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "connections - will be lost! ")
        Me.ResetButton.UseVisualStyleBackColor = False
        '
        'ReloadButton
        '
        Me.ReloadButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.hp1.SetHelpString(Me.ReloadButton, "Revert all settings to the last saved values")
        Me.ReloadButton.Location = New System.Drawing.Point(248, 321)
        Me.ReloadButton.Name = "ReloadButton"
        Me.hp1.SetShowHelp(Me.ReloadButton, True)
        Me.ReloadButton.Size = New System.Drawing.Size(69, 22)
        Me.ReloadButton.TabIndex = 7
        Me.ReloadButton.Text = "Re&load All"
        Me.ToolTip1.SetToolTip(Me.ReloadButton, "Reload the last saved user.config state. " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Abandons all changes made since the " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & _
                "Advanced Options form was opened.")
        Me.ReloadButton.UseVisualStyleBackColor = True
        '
        'pgb
        '
        Me.hp1.SetHelpString(Me.pgb, "Refreshing the dynamic contrls")
        Me.pgb.Location = New System.Drawing.Point(284, 144)
        Me.pgb.Name = "pgb"
        Me.hp1.SetShowHelp(Me.pgb, True)
        Me.pgb.Size = New System.Drawing.Size(253, 30)
        Me.pgb.Style = System.Windows.Forms.ProgressBarStyle.Continuous
        Me.pgb.TabIndex = 0
        Me.pgb.UseWaitCursor = True
        Me.pgb.Visible = False
        '
        'tvCategories
        '
        Me.tvCategories.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tvCategories.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.hp1.SetHelpString(Me.tvCategories, "Contains the categories of settings you may change.  Select a category to see the" & _
                " settings in the panel on the right.")
        Me.tvCategories.Location = New System.Drawing.Point(4, 20)
        Me.tvCategories.Margin = New System.Windows.Forms.Padding(0)
        Me.tvCategories.Name = "tvCategories"
        Me.tvCategories.PathSeparator = "."
        Me.hp1.SetShowHelp(Me.tvCategories, True)
        Me.tvCategories.Size = New System.Drawing.Size(187, 286)
        Me.tvCategories.TabIndex = 0
        '
        'Label1
        '
        Me.Label1.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(80, 11)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(39, 13)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "WHERE IS LABEL1?"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label2.Location = New System.Drawing.Point(80, 46)
        Me.Label2.MaximumSize = New System.Drawing.Size(50, 50)
        Me.Label2.MinimumSize = New System.Drawing.Size(50, 50)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(50, 50)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "WHERE IS LABEL2?"
        '
        'Label3
        '
        Me.Label3.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(80, 81)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(39, 13)
        Me.Label3.TabIndex = 4
        Me.Label3.Text = "WHERE IS LABEL3?"
        '
        'Label4
        '
        Me.Label4.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(80, 116)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(39, 13)
        Me.Label4.TabIndex = 5
        Me.Label4.Text = "WHERE IS LABEL4?"
        '
        'Label5
        '
        Me.Label5.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(80, 151)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(39, 13)
        Me.Label5.TabIndex = 6
        Me.Label5.Text = "WHERE IS LABEL5?"
        '
        'Label6
        '
        Me.Label6.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(80, 186)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(39, 13)
        Me.Label6.TabIndex = 7
        Me.Label6.Text = "WHERE IS LABEL6?"
        '
        'Label7
        '
        Me.Label7.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(80, 221)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(39, 13)
        Me.Label7.TabIndex = 8
        Me.Label7.Text = "WHERE IS LABEL7?"
        '
        'Label8
        '
        Me.Label8.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(80, 256)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(39, 13)
        Me.Label8.TabIndex = 9
        Me.Label8.Text = "WHERE IS LABEL8?"
        '
        'Label9
        '
        Me.Label9.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(80, 291)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(39, 13)
        Me.Label9.TabIndex = 10
        Me.Label9.Text = "WHERE IS LABEL 9?"
        '
        'Label10
        '
        Me.Label10.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(77, 326)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(45, 13)
        Me.Label10.TabIndex = 11
        Me.Label10.Text = "WHERE IS LABEL10?"
        '
        'Label11
        '
        Me.Label11.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(77, 361)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(45, 13)
        Me.Label11.TabIndex = 12
        Me.Label11.Text = "WHERE IS LABEL11?"
        '
        'Label12
        '
        Me.Label12.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(77, 396)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(45, 13)
        Me.Label12.TabIndex = 13
        Me.Label12.Text = "WHERE IS LABEL12?"
        '
        'Label13
        '
        Me.Label13.Location = New System.Drawing.Point(12, 9)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(625, 16)
        Me.Label13.TabIndex = 8
        '
        'LabelCategory
        '
        Me.LabelCategory.AutoSize = True
        Me.LabelCategory.Dock = System.Windows.Forms.DockStyle.Top
        Me.LabelCategory.Location = New System.Drawing.Point(4, 4)
        Me.LabelCategory.Name = "LabelCategory"
        Me.LabelCategory.Size = New System.Drawing.Size(98, 13)
        Me.LabelCategory.TabIndex = 9
        Me.LabelCategory.Text = "Settings Catagories"
        '
        'LabelSettings
        '
        Me.LabelSettings.AutoSize = True
        Me.LabelSettings.Dock = System.Windows.Forms.DockStyle.Top
        Me.LabelSettings.Location = New System.Drawing.Point(4, 4)
        Me.LabelSettings.Name = "LabelSettings"
        Me.LabelSettings.Size = New System.Drawing.Size(45, 13)
        Me.LabelSettings.TabIndex = 10
        Me.LabelSettings.Text = "Settings"
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
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.BackColor = System.Drawing.SystemColors.Control
        Me.SplitContainer1.Panel1.Controls.Add(Me.LabelCategory)
        Me.SplitContainer1.Panel1.Controls.Add(Me.tvCategories)
        Me.SplitContainer1.Panel1.Padding = New System.Windows.Forms.Padding(4)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.BackColor = System.Drawing.SystemColors.Control
        Me.SplitContainer1.Panel2.Controls.Add(Me.LabelSettings)
        Me.SplitContainer1.Panel2.Controls.Add(Me.tlp)
        Me.SplitContainer1.Panel2.Padding = New System.Windows.Forms.Padding(4)
        Me.SplitContainer1.Size = New System.Drawing.Size(639, 312)
        Me.SplitContainer1.SplitterDistance = 197
        Me.SplitContainer1.TabIndex = 11
        '
        'Button1
        '
        Me.Button1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button1.Location = New System.Drawing.Point(346, 321)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(95, 22)
        Me.Button1.TabIndex = 1
        Me.Button1.Text = "&Apply"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'Button2
        '
        Me.Button2.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button2.BackColor = System.Drawing.Color.Red
        Me.Button2.ForeColor = System.Drawing.SystemColors.HighlightText
        Me.Button2.Location = New System.Drawing.Point(175, 322)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(69, 22)
        Me.Button2.TabIndex = 6
        Me.Button2.Text = "&Reset All"
        Me.Button2.UseVisualStyleBackColor = False
        '
        'Button3
        '
        Me.Button3.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button3.Location = New System.Drawing.Point(248, 321)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(69, 22)
        Me.Button3.TabIndex = 7
        Me.Button3.Text = "Re&load All"
        Me.Button3.UseVisualStyleBackColor = True
        '
        'frmOptions
        '
        Me.AcceptButton = Me.cmdOK
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.CancelButton = Me.cmdCancel
        Me.ClientSize = New System.Drawing.Size(648, 349)
        Me.Controls.Add(Me.SplitContainer1)
        Me.Controls.Add(Me.Label13)
        Me.Controls.Add(Me.Button3)
        Me.Controls.Add(Me.ReloadButton)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.ResetButton)
        Me.Controls.Add(Me.chkSaveOnExit)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdApply)
        Me.Controls.Add(Me.pgb)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmOptions"
        Me.Padding = New System.Windows.Forms.Padding(4)
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "assigned in code"
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel1.PerformLayout()
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        Me.SplitContainer1.Panel2.PerformLayout()
        Me.SplitContainer1.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents cmdApply As System.Windows.Forms.Button
    Friend WithEvents cmdCancel As System.Windows.Forms.Button
    Friend WithEvents cmdOK As System.Windows.Forms.Button
    Friend WithEvents tlp As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents chkSaveOnExit As System.Windows.Forms.CheckBox
    Friend WithEvents hp1 As System.Windows.Forms.HelpProvider
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents pnlFFBar As System.Windows.Forms.Panel
    Friend WithEvents pnlFFPane As System.Windows.Forms.Panel
    Friend WithEvents ResetButton As System.Windows.Forms.Button
    Friend WithEvents ReloadButton As System.Windows.Forms.Button
    Friend WithEvents pgb As System.Windows.Forms.ProgressBar
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents LabelCategory As System.Windows.Forms.Label
    Friend WithEvents LabelSettings As System.Windows.Forms.Label
    Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
    Friend WithEvents tvCategories As System.Windows.Forms.TreeView
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents Button3 As System.Windows.Forms.Button

End Class
