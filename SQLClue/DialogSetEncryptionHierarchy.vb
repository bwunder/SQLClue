Imports System.Windows.Forms

Public Class DialogSetEncryptionHierarchy
    Private ReadOnly Property oDb() As Smo.Database
        Get
            Return CType(Me.Tag, Smo.Database)
        End Get
    End Property

    Private ReadOnly Property SQLInstance() As String
        Get
            Return CType(Me.Tag, Smo.Database).Parent.ConnectionContext.TrueName
        End Get
    End Property

    Private Sub DialogSetEncryptionHierarchy_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'ASSERT Service Master key is found
        ' not supposed to be here if the key exists
        If oDb.MasterKey Is Nothing Then
            CheckBoxMasterKeyExists.Checked = False
            RichTextBoxDbMasterKeyPassword.Enabled = True
            ToolStripStatusLabelSetEncryptionHierarchy.Text = "Create Database Master Key?"
        Else
            CheckBoxMasterKeyExists.Checked = True
            RichTextBoxDbMasterKeyPassword.Enabled = False
            If oDb.Certificates.Contains(My.Settings.RepositoryPasswordEncryptionCertificate) Then
                CheckBoxCertificateExists.Checked = True
                If oDb.SymmetricKeys.Contains(My.Settings.RepositoryPasswordEncryptionKey) Then
                    ComboBoxAlgorithm.Enabled = False
                Else
                    ComboBoxAlgorithm.Enabled = True
                    ComboBoxAlgorithm.Items.AddRange([Enum].GetNames(GetType(Smo.SymmetricKeyEncryptionAlgorithm)))
                    ToolStripStatusLabelSetEncryptionHierarchy.Text = "Select Encryption Algorithm?"
                End If
            Else
                CheckBoxCertificateExists.Checked = False
                ToolStripStatusLabelSetEncryptionHierarchy.Text = "Create Certificate?"
            End If
        End If
    End Sub

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click

        Try

            If Not (CheckBoxMasterKeyExists.Checked) Then

                If RichTextBoxDbMasterKeyPassword.Text <> "" Then
                    Dim oDBMasterKey As New MasterKey(oDb)
                    oDBMasterKey.Create(RichTextBoxDbMasterKeyPassword.Text)
                    oDBMasterKey.AddServiceKeyEncryption()
                Else
                    MessageBox.Show("Enter an Encryption Password for the Database Master Key.", "Missing Value")
                End If

            End If

            If Not (CheckBoxCertificateExists.Checked) Then
                Dim cert As New Certificate
                With cert
                    .Parent = oDb
                    .Name = My.Settings.RepositoryPasswordEncryptionCertificate
                    .Subject = "SQL Configuration monitored SQL Server SQL authentication password encryption certificate"
                    .Create()
                End With
            End If

            Dim SKeyEncrypt As New SymmetricKeyEncryption(KeyEncryptionType.Certificate, _
                                                          My.Settings.RepositoryPasswordEncryptionCertificate)
            Dim SKey As New SymmetricKey(oDb, _
                                         My.Settings.RepositoryPasswordEncryptionKey)
            SKey.Create(SKeyEncrypt, _
                        CType([Enum].Parse(GetType(SymmetricKeyEncryptionAlgorithm), _
                                           ComboBoxAlgorithm.SelectedItem.ToString),  _
                        Smo.SymmetricKeyEncryptionAlgorithm))

            MessageBox.Show("Backup the Service Master Key and the Database Master Key as soon as possible.", _
                            "Encryption Hierarchy Created", _
                            MessageBoxButtons.OK)

            Me.DialogResult = System.Windows.Forms.DialogResult.OK
            Me.Close()

        Catch ex As Exception

            Throw New Exception(String.Format("({0}.OK_Button_Click) Exception.", Me.Text), ex)

        End Try

    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click

        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()

    End Sub

    Private Sub BackupHierarchy()
        Try

            Dim DftPass As String = "<password>"

            Dim SMKeyFilePass As String = DftPass

            While SMKeyFilePass = DftPass
                SMKeyFilePass = InputBox("Enter a password for encrypting the Service Master Key in the " & _
                       "backup file (subject to host policy password complexity checks):", "Service Master Key Backup File Password", DftPass)
                If SMKeyFilePass = "" Then Exit Sub
            End While
            Dim SMKeyPath As String = ""
            Dim SMKeyPathPicker As New FolderBrowserDialog
            While SMKeyPath = ""

                SMKeyPathPicker.Description = "Select a location to store the exported Service Master Key." & _
                                              "The SQL Server Service account of the [" & SQLInstance & "] SQL " & _
                                              "Instance must have write access to the selected location."
                SMKeyPathPicker.RootFolder = Environment.SpecialFolder.MyComputer
                SMKeyPathPicker.ShowNewFolderButton = True
                SMKeyPathPicker.SelectedPath = oDb.Parent.Information.ErrorLogPath
                Dim SMKeyPathPickerAnswer As DialogResult = SMKeyPathPicker.ShowDialog(Me)
                SMKeyPath = SMKeyPathPicker.SelectedPath
                If SMKeyPathPickerAnswer = Windows.Forms.DialogResult.Cancel Then Exit While
                Try
                    oDb.Parent.ServiceMasterKey.Export(SMKeyPath & "\" & Replace(SQLInstance, "\", "$") & "_Service_Master_Key.BAK", SMKeyFilePass)

                Catch ex As Exception
                    My.Application.Log.WriteEntry(ex.StackTrace.ToString, TraceEventType.Error)
                    Dim ExTop As New ApplicationException(Me.Text & "" & ex.GetType.ToString, ex)
                    ExTop.Source = Me.Text

                    Dim emb As New ExceptionMessageBox(ExTop)
                    emb.Show(Me)

                    SMKeyPath = ""
                End Try
            End While

            Dim reply As DialogResult = MessageBox.Show("The encryption hiereachy required to store passwords as encrypted data is created. " & _
                                               vbCrLf & vbCrLf & "Backup the Service Master Key and the Database Master Key now?", _
                                               "Encryption Hierarchy Created", MessageBoxButtons.YesNo)

            Dim MKeyFilePass As String = DftPass
            While MKeyFilePass = DftPass
                MKeyFilePass = InputBox("Enter a password for encrypting the Service Master Key in the " & _
                       "backup file (subject to policy password complexity checks):", "Service Master Key Backup File Password", DftPass)
                ' cancel returns an empty string
                If MKeyFilePass = "" Then Exit While
            End While
            Dim MKeyPath As String = ""
            Dim MKeyPathPicker As New FolderBrowserDialog
            While MKeyPath = ""
                MKeyPathPicker.Description = "Select a location to store the exported [" & oDb.Name & "] Database Master Key" & _
                                             "The SQL Server Service account of the [" & SQLInstance & "] SQL " & _
                                             "Instance must have write access to the selected location."
                MKeyPathPicker.RootFolder = Environment.SpecialFolder.MyComputer
                MKeyPathPicker.ShowNewFolderButton = True
                MKeyPathPicker.SelectedPath = oDb.Parent.Information.ErrorLogPath
                Dim MKeyPathPickerAnswer As DialogResult = MKeyPathPicker.ShowDialog(Me)
                MKeyPath = SMKeyPathPicker.SelectedPath
                If MKeyPathPickerAnswer = Windows.Forms.DialogResult.Cancel Then Exit Sub
                Try
                    oDb.Parent.ServiceMasterKey.Export(SMKeyPath & "\" & Replace(SQLInstance, "\", "$") & "_" & oDb.Name & "_Database_Master_Key.BAK", MKeyFilePass)
                Catch ex As Exception
                    My.Application.Log.WriteEntry(ex.StackTrace.ToString,TraceEventType.Error)
                    Dim ExTop As New ApplicationException(Me.Text & "" & ex.GetType.ToString, ex)
                    ExTop.Source = Me.Text

                    Dim emb As New ExceptionMessageBox(ExTop)
                    emb.Show(Me)

                    MKeyPath = ""
                End Try
            End While

            Me.DialogResult = System.Windows.Forms.DialogResult.OK
            Me.Close()

        Catch ex As Exception

            Throw New Exception(String.Format("({0}.BackupHierarchy) Exception.", Me.Text), ex)

        End Try

    End Sub

End Class
