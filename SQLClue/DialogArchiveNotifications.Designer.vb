<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class DialogArchiveNotifications
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
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel
        Me.OK_Button = New System.Windows.Forms.Button
        Me.Cancel_Button = New System.Windows.Forms.Button
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.Panel3 = New System.Windows.Forms.Panel
        Me.CheckBoxSuccessNetSend = New System.Windows.Forms.CheckBox
        Me.CheckBoxSuccessPager = New System.Windows.Forms.CheckBox
        Me.CheckBoxSuccessEmail = New System.Windows.Forms.CheckBox
        Me.RichTextBoxSuccessMessages = New System.Windows.Forms.RichTextBox
        Me.CheckBoxAlertOnSuccess = New System.Windows.Forms.CheckBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.CheckBoxNotifyOnSuccess = New System.Windows.Forms.CheckBox
        Me.ComboBoxSuccessOperator = New System.Windows.Forms.ComboBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.Panel2 = New System.Windows.Forms.Panel
        Me.CheckBoxFailNetSend = New System.Windows.Forms.CheckBox
        Me.CheckBoxFailPager = New System.Windows.Forms.CheckBox
        Me.CheckBoxFailEmail = New System.Windows.Forms.CheckBox
        Me.RichTextBoxFailMessages = New System.Windows.Forms.RichTextBox
        Me.CheckBoxAlertOnFail = New System.Windows.Forms.CheckBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.CheckBoxNotifyOnFail = New System.Windows.Forms.CheckBox
        Me.ComboBoxFailOperator = New System.Windows.Forms.ComboBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.TableLayoutPanel1.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.Panel3.SuspendLayout()
        Me.Panel2.SuspendLayout()
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
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(234, 313)
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
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.Panel3)
        Me.Panel1.Controls.Add(Me.Panel2)
        Me.Panel1.Location = New System.Drawing.Point(5, 5)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(381, 301)
        Me.Panel1.TabIndex = 1
        '
        'Panel3
        '
        Me.Panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel3.Controls.Add(Me.CheckBoxSuccessNetSend)
        Me.Panel3.Controls.Add(Me.CheckBoxSuccessPager)
        Me.Panel3.Controls.Add(Me.CheckBoxSuccessEmail)
        Me.Panel3.Controls.Add(Me.RichTextBoxSuccessMessages)
        Me.Panel3.Controls.Add(Me.CheckBoxAlertOnSuccess)
        Me.Panel3.Controls.Add(Me.Label3)
        Me.Panel3.Controls.Add(Me.CheckBoxNotifyOnSuccess)
        Me.Panel3.Controls.Add(Me.ComboBoxSuccessOperator)
        Me.Panel3.Controls.Add(Me.Label4)
        Me.Panel3.Location = New System.Drawing.Point(5, 5)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(369, 142)
        Me.Panel3.TabIndex = 5
        '
        'CheckBoxSuccessNetSend
        '
        Me.CheckBoxSuccessNetSend.AutoSize = True
        Me.CheckBoxSuccessNetSend.Location = New System.Drawing.Point(163, 118)
        Me.CheckBoxSuccessNetSend.Name = "CheckBoxSuccessNetSend"
        Me.CheckBoxSuccessNetSend.Size = New System.Drawing.Size(67, 17)
        Me.CheckBoxSuccessNetSend.TabIndex = 13
        Me.CheckBoxSuccessNetSend.Text = "net send"
        Me.CheckBoxSuccessNetSend.UseVisualStyleBackColor = True
        '
        'CheckBoxSuccessPager
        '
        Me.CheckBoxSuccessPager.AutoSize = True
        Me.CheckBoxSuccessPager.Location = New System.Drawing.Point(163, 103)
        Me.CheckBoxSuccessPager.Name = "CheckBoxSuccessPager"
        Me.CheckBoxSuccessPager.Size = New System.Drawing.Size(53, 17)
        Me.CheckBoxSuccessPager.TabIndex = 12
        Me.CheckBoxSuccessPager.Text = "pager"
        Me.CheckBoxSuccessPager.UseVisualStyleBackColor = True
        '
        'CheckBoxSuccessEmail
        '
        Me.CheckBoxSuccessEmail.AutoSize = True
        Me.CheckBoxSuccessEmail.Location = New System.Drawing.Point(163, 88)
        Me.CheckBoxSuccessEmail.Name = "CheckBoxSuccessEmail"
        Me.CheckBoxSuccessEmail.Size = New System.Drawing.Size(51, 17)
        Me.CheckBoxSuccessEmail.TabIndex = 11
        Me.CheckBoxSuccessEmail.Text = "Email"
        Me.CheckBoxSuccessEmail.UseVisualStyleBackColor = True
        '
        'RichTextBoxSuccessMessages
        '
        Me.RichTextBoxSuccessMessages.BackColor = System.Drawing.SystemColors.Control
        Me.RichTextBoxSuccessMessages.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.RichTextBoxSuccessMessages.Enabled = False
        Me.RichTextBoxSuccessMessages.Location = New System.Drawing.Point(239, 88)
        Me.RichTextBoxSuccessMessages.Name = "RichTextBoxSuccessMessages"
        Me.RichTextBoxSuccessMessages.Size = New System.Drawing.Size(124, 48)
        Me.RichTextBoxSuccessMessages.TabIndex = 10
        Me.RichTextBoxSuccessMessages.Text = Global.SQLClue.My.Resources.Resources.HelpOverview
        '
        'CheckBoxAlertOnSuccess
        '
        Me.CheckBoxAlertOnSuccess.AutoSize = True
        Me.CheckBoxAlertOnSuccess.Enabled = False
        Me.CheckBoxAlertOnSuccess.Location = New System.Drawing.Point(13, 12)
        Me.CheckBoxAlertOnSuccess.Name = "CheckBoxAlertOnSuccess"
        Me.CheckBoxAlertOnSuccess.Size = New System.Drawing.Size(321, 17)
        Me.CheckBoxAlertOnSuccess.TabIndex = 9
        Me.CheckBoxAlertOnSuccess.Text = "Enable Alert 'SQLClue SQL Configuration Archive Succeeded' "
        Me.CheckBoxAlertOnSuccess.UseVisualStyleBackColor = True
        '
        'Label3
        '
        Me.Label3.Location = New System.Drawing.Point(47, 87)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(110, 49)
        Me.Label3.TabIndex = 7
        Me.Label3.Text = "Notification Method(s)"
        '
        'CheckBoxNotifyOnSuccess
        '
        Me.CheckBoxNotifyOnSuccess.AutoSize = True
        Me.CheckBoxNotifyOnSuccess.Enabled = False
        Me.CheckBoxNotifyOnSuccess.Location = New System.Drawing.Point(31, 38)
        Me.CheckBoxNotifyOnSuccess.Name = "CheckBoxNotifyOnSuccess"
        Me.CheckBoxNotifyOnSuccess.Size = New System.Drawing.Size(281, 17)
        Me.CheckBoxNotifyOnSuccess.TabIndex = 6
        Me.CheckBoxNotifyOnSuccess.Text = "Notify an Operator when an Archive ends successfully"
        Me.CheckBoxNotifyOnSuccess.UseVisualStyleBackColor = True
        '
        'ComboBoxSuccessOperator
        '
        Me.ComboBoxSuccessOperator.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBoxSuccessOperator.Enabled = False
        Me.ComboBoxSuccessOperator.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.ComboBoxSuccessOperator.FormattingEnabled = True
        Me.ComboBoxSuccessOperator.Location = New System.Drawing.Point(147, 60)
        Me.ComboBoxSuccessOperator.Name = "ComboBoxSuccessOperator"
        Me.ComboBoxSuccessOperator.Size = New System.Drawing.Size(217, 21)
        Me.ComboBoxSuccessOperator.TabIndex = 5
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(47, 63)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(90, 13)
        Me.Label4.TabIndex = 4
        Me.Label4.Text = "Operator to Notify"
        '
        'Panel2
        '
        Me.Panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel2.Controls.Add(Me.CheckBoxFailNetSend)
        Me.Panel2.Controls.Add(Me.CheckBoxFailPager)
        Me.Panel2.Controls.Add(Me.CheckBoxFailEmail)
        Me.Panel2.Controls.Add(Me.RichTextBoxFailMessages)
        Me.Panel2.Controls.Add(Me.CheckBoxAlertOnFail)
        Me.Panel2.Controls.Add(Me.Label2)
        Me.Panel2.Controls.Add(Me.CheckBoxNotifyOnFail)
        Me.Panel2.Controls.Add(Me.ComboBoxFailOperator)
        Me.Panel2.Controls.Add(Me.Label1)
        Me.Panel2.Location = New System.Drawing.Point(5, 152)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(369, 142)
        Me.Panel2.TabIndex = 4
        '
        'CheckBoxFailNetSend
        '
        Me.CheckBoxFailNetSend.AutoSize = True
        Me.CheckBoxFailNetSend.Location = New System.Drawing.Point(163, 118)
        Me.CheckBoxFailNetSend.Name = "CheckBoxFailNetSend"
        Me.CheckBoxFailNetSend.Size = New System.Drawing.Size(67, 17)
        Me.CheckBoxFailNetSend.TabIndex = 14
        Me.CheckBoxFailNetSend.Text = "net send"
        Me.CheckBoxFailNetSend.UseVisualStyleBackColor = True
        '
        'CheckBoxFailPager
        '
        Me.CheckBoxFailPager.AutoSize = True
        Me.CheckBoxFailPager.Location = New System.Drawing.Point(163, 103)
        Me.CheckBoxFailPager.Name = "CheckBoxFailPager"
        Me.CheckBoxFailPager.Size = New System.Drawing.Size(53, 17)
        Me.CheckBoxFailPager.TabIndex = 13
        Me.CheckBoxFailPager.Text = "pager"
        Me.CheckBoxFailPager.UseVisualStyleBackColor = True
        '
        'CheckBoxFailEmail
        '
        Me.CheckBoxFailEmail.AutoSize = True
        Me.CheckBoxFailEmail.Location = New System.Drawing.Point(163, 87)
        Me.CheckBoxFailEmail.Name = "CheckBoxFailEmail"
        Me.CheckBoxFailEmail.Size = New System.Drawing.Size(51, 17)
        Me.CheckBoxFailEmail.TabIndex = 12
        Me.CheckBoxFailEmail.Text = "Email"
        Me.CheckBoxFailEmail.UseVisualStyleBackColor = True
        '
        'RichTextBoxFailMessages
        '
        Me.RichTextBoxFailMessages.BackColor = System.Drawing.SystemColors.Control
        Me.RichTextBoxFailMessages.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.RichTextBoxFailMessages.Enabled = False
        Me.RichTextBoxFailMessages.Location = New System.Drawing.Point(239, 87)
        Me.RichTextBoxFailMessages.Name = "RichTextBoxFailMessages"
        Me.RichTextBoxFailMessages.Size = New System.Drawing.Size(124, 48)
        Me.RichTextBoxFailMessages.TabIndex = 11
        Me.RichTextBoxFailMessages.Text = Global.SQLClue.My.Resources.Resources.HelpOverview
        '
        'CheckBoxAlertOnFail
        '
        Me.CheckBoxAlertOnFail.AutoSize = True
        Me.CheckBoxAlertOnFail.Enabled = False
        Me.CheckBoxAlertOnFail.Location = New System.Drawing.Point(15, 13)
        Me.CheckBoxAlertOnFail.Name = "CheckBoxAlertOnFail"
        Me.CheckBoxAlertOnFail.Size = New System.Drawing.Size(291, 17)
        Me.CheckBoxAlertOnFail.TabIndex = 9
        Me.CheckBoxAlertOnFail.Text = "Enable Alert 'SQLClue SQL Configuration Archive Failed'"
        Me.CheckBoxAlertOnFail.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(47, 87)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(110, 49)
        Me.Label2.TabIndex = 7
        Me.Label2.Text = "Notification Method(s)"
        '
        'CheckBoxNotifyOnFail
        '
        Me.CheckBoxNotifyOnFail.AutoSize = True
        Me.CheckBoxNotifyOnFail.Enabled = False
        Me.CheckBoxNotifyOnFail.Location = New System.Drawing.Point(32, 38)
        Me.CheckBoxNotifyOnFail.Name = "CheckBoxNotifyOnFail"
        Me.CheckBoxNotifyOnFail.Size = New System.Drawing.Size(216, 17)
        Me.CheckBoxNotifyOnFail.TabIndex = 6
        Me.CheckBoxNotifyOnFail.Text = "Notify an Operator when an Archive fails"
        Me.CheckBoxNotifyOnFail.UseVisualStyleBackColor = True
        '
        'ComboBoxFailOperator
        '
        Me.ComboBoxFailOperator.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBoxFailOperator.Enabled = False
        Me.ComboBoxFailOperator.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.ComboBoxFailOperator.FormattingEnabled = True
        Me.ComboBoxFailOperator.Location = New System.Drawing.Point(147, 60)
        Me.ComboBoxFailOperator.Name = "ComboBoxFailOperator"
        Me.ComboBoxFailOperator.Size = New System.Drawing.Size(217, 21)
        Me.ComboBoxFailOperator.TabIndex = 5
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(47, 64)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(90, 13)
        Me.Label1.TabIndex = 4
        Me.Label1.Text = "Operator to Notify"
        '
        'DialogArchiveNotifications
        '
        Me.AcceptButton = Me.OK_Button
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.Cancel_Button
        Me.ClientSize = New System.Drawing.Size(392, 353)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "DialogArchiveNotifications"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "SQL Configuration Archive Completion Notifications"
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        Me.Panel3.ResumeLayout(False)
        Me.Panel3.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents OK_Button As System.Windows.Forms.Button
    Friend WithEvents Cancel_Button As System.Windows.Forms.Button
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents CheckBoxNotifyOnFail As System.Windows.Forms.CheckBox
    Friend WithEvents ComboBoxFailOperator As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Friend WithEvents CheckBoxAlertOnSuccess As System.Windows.Forms.CheckBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents CheckBoxNotifyOnSuccess As System.Windows.Forms.CheckBox
    Friend WithEvents ComboBoxSuccessOperator As System.Windows.Forms.ComboBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents CheckBoxAlertOnFail As System.Windows.Forms.CheckBox
    Friend WithEvents RichTextBoxSuccessMessages As System.Windows.Forms.RichTextBox
    Friend WithEvents RichTextBoxFailMessages As System.Windows.Forms.RichTextBox
    Friend WithEvents CheckBoxSuccessNetSend As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBoxSuccessPager As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBoxSuccessEmail As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBoxFailNetSend As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBoxFailPager As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBoxFailEmail As System.Windows.Forms.CheckBox

End Class
