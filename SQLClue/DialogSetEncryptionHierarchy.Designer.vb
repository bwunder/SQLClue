<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class DialogSetEncryptionHierarchy
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
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel
        Me.OK_Button = New System.Windows.Forms.Button
        Me.Cancel_Button = New System.Windows.Forms.Button
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.RichTextBoxDbMasterKeyPassword = New System.Windows.Forms.RichTextBox
        Me.LabelDbMasterKeyPassword = New System.Windows.Forms.Label
        Me.ComboBoxAlgorithm = New System.Windows.Forms.ComboBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.CheckBoxMasterKeyExists = New System.Windows.Forms.CheckBox
        Me.CheckBoxCertificateExists = New System.Windows.Forms.CheckBox
        Me.CheckBoxKeyExists = New System.Windows.Forms.CheckBox
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip
        Me.ToolStripStatusLabelSetEncryptionHierarchy = New System.Windows.Forms.ToolStripStatusLabel
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.TableLayoutPanel1.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.StatusStrip1.SuspendLayout()
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
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(159, 132)
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
        'Panel1
        '
        Me.Panel1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.RichTextBoxDbMasterKeyPassword)
        Me.Panel1.Controls.Add(Me.LabelDbMasterKeyPassword)
        Me.Panel1.Controls.Add(Me.ComboBoxAlgorithm)
        Me.Panel1.Controls.Add(Me.Label4)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Controls.Add(Me.CheckBoxMasterKeyExists)
        Me.Panel1.Controls.Add(Me.CheckBoxCertificateExists)
        Me.Panel1.Controls.Add(Me.CheckBoxKeyExists)
        Me.Panel1.Location = New System.Drawing.Point(4, 5)
        Me.Panel1.Margin = New System.Windows.Forms.Padding(0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(351, 122)
        Me.Panel1.TabIndex = 16
        '
        'RichTextBoxDbMasterKeyPassword
        '
        Me.RichTextBoxDbMasterKeyPassword.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.RichTextBoxDbMasterKeyPassword.Location = New System.Drawing.Point(131, 26)
        Me.RichTextBoxDbMasterKeyPassword.Multiline = False
        Me.RichTextBoxDbMasterKeyPassword.Name = "RichTextBoxDbMasterKeyPassword"
        Me.RichTextBoxDbMasterKeyPassword.Size = New System.Drawing.Size(214, 22)
        Me.RichTextBoxDbMasterKeyPassword.TabIndex = 10
        Me.RichTextBoxDbMasterKeyPassword.Text = Global.SQLClue.My.Resources.Resources.HelpOverview
        '
        'LabelDbMasterKeyPassword
        '
        Me.LabelDbMasterKeyPassword.AutoSize = True
        Me.LabelDbMasterKeyPassword.Location = New System.Drawing.Point(23, 28)
        Me.LabelDbMasterKeyPassword.Name = "LabelDbMasterKeyPassword"
        Me.LabelDbMasterKeyPassword.Size = New System.Drawing.Size(106, 13)
        Me.LabelDbMasterKeyPassword.TabIndex = 14
        Me.LabelDbMasterKeyPassword.Text = "Encryption Password"
        '
        'ComboBoxAlgorithm
        '
        Me.ComboBoxAlgorithm.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ComboBoxAlgorithm.FormattingEnabled = True
        Me.ComboBoxAlgorithm.Location = New System.Drawing.Point(131, 96)
        Me.ComboBoxAlgorithm.Name = "ComboBoxAlgorithm"
        Me.ComboBoxAlgorithm.Size = New System.Drawing.Size(214, 21)
        Me.ComboBoxAlgorithm.TabIndex = 1
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(75, 99)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(50, 13)
        Me.Label4.TabIndex = 6
        Me.Label4.Text = "Algorithm"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(36, 28)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(0, 13)
        Me.Label1.TabIndex = 11
        '
        'CheckBoxMasterKeyExists
        '
        Me.CheckBoxMasterKeyExists.AutoSize = True
        Me.CheckBoxMasterKeyExists.Enabled = False
        Me.CheckBoxMasterKeyExists.Location = New System.Drawing.Point(5, 4)
        Me.CheckBoxMasterKeyExists.Name = "CheckBoxMasterKeyExists"
        Me.CheckBoxMasterKeyExists.Size = New System.Drawing.Size(158, 17)
        Me.CheckBoxMasterKeyExists.TabIndex = 7
        Me.CheckBoxMasterKeyExists.Text = "Database Master Key Exists"
        Me.CheckBoxMasterKeyExists.UseVisualStyleBackColor = True
        '
        'CheckBoxCertificateExists
        '
        Me.CheckBoxCertificateExists.AutoSize = True
        Me.CheckBoxCertificateExists.Enabled = False
        Me.CheckBoxCertificateExists.Location = New System.Drawing.Point(27, 54)
        Me.CheckBoxCertificateExists.Name = "CheckBoxCertificateExists"
        Me.CheckBoxCertificateExists.Size = New System.Drawing.Size(167, 17)
        Me.CheckBoxCertificateExists.TabIndex = 8
        Me.CheckBoxCertificateExists.Text = "Archive User Certificate Exists"
        Me.CheckBoxCertificateExists.UseVisualStyleBackColor = True
        '
        'CheckBoxKeyExists
        '
        Me.CheckBoxKeyExists.AutoSize = True
        Me.CheckBoxKeyExists.Enabled = False
        Me.CheckBoxKeyExists.Location = New System.Drawing.Point(47, 75)
        Me.CheckBoxKeyExists.Name = "CheckBoxKeyExists"
        Me.CheckBoxKeyExists.Size = New System.Drawing.Size(125, 17)
        Me.CheckBoxKeyExists.TabIndex = 9
        Me.CheckBoxKeyExists.Text = "Symmetric Key Exists"
        Me.CheckBoxKeyExists.UseVisualStyleBackColor = True
        '
        'StatusStrip1
        '
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripStatusLabelSetEncryptionHierarchy})
        Me.StatusStrip1.Location = New System.Drawing.Point(4, 160)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Size = New System.Drawing.Size(351, 22)
        Me.StatusStrip1.TabIndex = 18
        Me.StatusStrip1.Text = "StatusStrip1"
        '
        'ToolStripStatusLabelSetEncryptionHierarchy
        '
        Me.ToolStripStatusLabelSetEncryptionHierarchy.Name = "ToolStripStatusLabelSetEncryptionHierarchy"
        Me.ToolStripStatusLabelSetEncryptionHierarchy.Size = New System.Drawing.Size(336, 17)
        Me.ToolStripStatusLabelSetEncryptionHierarchy.Spring = True
        Me.ToolStripStatusLabelSetEncryptionHierarchy.Text = "ToolStripStatusLabel1"
        '
        'DialogSetEncryptionHierarchy
        '
        Me.AcceptButton = Me.OK_Button
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.Cancel_Button
        Me.ClientSize = New System.Drawing.Size(359, 186)
        Me.Controls.Add(Me.StatusStrip1)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow
        Me.MaximizeBox = False
        Me.MaximumSize = New System.Drawing.Size(999, 235)
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(375, 220)
        Me.Name = "DialogSetEncryptionHierarchy"
        Me.Padding = New System.Windows.Forms.Padding(4)
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Add Required Database Encryption Hierarchy"
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.StatusStrip1.ResumeLayout(False)
        Me.StatusStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents OK_Button As System.Windows.Forms.Button
    Friend WithEvents Cancel_Button As System.Windows.Forms.Button
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents RichTextBoxDbMasterKeyPassword As System.Windows.Forms.RichTextBox
    Friend WithEvents LabelDbMasterKeyPassword As System.Windows.Forms.Label
    Friend WithEvents ComboBoxAlgorithm As System.Windows.Forms.ComboBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents CheckBoxMasterKeyExists As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBoxCertificateExists As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBoxKeyExists As System.Windows.Forms.CheckBox
    Friend WithEvents StatusStrip1 As System.Windows.Forms.StatusStrip
    Friend WithEvents ToolStripStatusLabelSetEncryptionHierarchy As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip

End Class
