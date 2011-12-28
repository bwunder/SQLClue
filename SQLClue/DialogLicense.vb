Public Class DialogLicense

    Private Sub LicenseForm_HelpRequested(ByVal sender As Object, ByVal hlpevent As System.Windows.Forms.HelpEventArgs) Handles Me.HelpRequested
        System.Diagnostics.Process.Start("IExplore", My.Application.Info.DirectoryPath & "\" & My.Settings.HelpRoot)
    End Sub

    Private Sub License_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        ' repository must be installed and a trial archive run before can be licensed 

        ' the rule is, a will not count against the license  until it gets
        ' change rows that are not metadata changes (actual use) so need to 
        ' if exists (select * from tsqlcfgchange where Node.Type = 'SQLInstance' and Node.Server = RepositoryInstanceName

        ' so needs at lease one configured instance and some history before can be licensed
        ' be good to also support just the compare and runbook without a full install of the repository
        Dim oRepository As Smo.Server
        Dim srvcon As New ServerConnection
        Try
            If Not My.Settings.RepositoryEnabled _
            Or My.Settings.RepositoryInstanceName = "" _
            Or My.Settings.RepositoryDatabaseName = "" Then
                MessageBox.Show(Me, My.Resources.OnlyLicenceIfDBInstalled, "SQLClue Repository Installation Required", _
                                     MessageBoxButtons.OK, _
                                     MessageBoxIcon.Information, _
                                     MessageBoxDefaultButton.Button1)
                Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
                Exit Try
            End If
            srvcon.ConnectionString = Mother.DAL.LocalRepositoryConnectionString
            oRepository = New Server(srvcon)

            If Not My.Settings.RepositoryEnabled _
            Or Not oRepository.Databases.Contains(My.Settings.RepositoryDatabaseName) Then
                Throw New Exception("SQLClue Repository SQL Database Validation Failed.")
            End If
            If Not UCase(oRepository.ConnectionContext.TrueName) = UCase(My.Settings.RepositoryInstanceName) Then
                Throw New Exception("SQLClue Repository SQL Instance Validation Failed.")
            End If
            ' the point is to uniquely identify the installation. no need to obfuscate
            ' the license date must be a valid date on the local machine so get it here to make sure the right format is used to gen key 
            LocalKey.Text = String.Format("{0}->{1}.{2} {3}", _
                                          My.Computer.Name, _
                                          oRepository.ConnectionContext.TrueName, _
                                          My.Settings.RepositoryDatabaseName, _
                                          Today).ToString
            SetFormValuesFromDb()
        Catch exSQL As SqlClient.SqlException
            Throw exSQL
        Finally
            If Not (srvcon Is Nothing) Then
                srvcon.Disconnect()
                oRepository = Nothing
            End If
        End Try
    End Sub
    Private Sub SetFormValuesFromDb()
        Try
            ' already know the db exist by now so get the master record
            Mother.DAL.LoadSQLCfg()
            LicensedCompany.Text = Mother.DAL.dsSQLCfg.tSQLCfg.Rows(0).Item("LicensedCompany").ToString
            LicensedCompany.Tag = Mother.DAL.dsSQLCfg.tSQLCfg.Rows(0).Item("LicensedCompany").ToString
            LicensedUser.Text = Mother.DAL.dsSQLCfg.tSQLCfg.Rows(0).Item("LicensedUser").ToString
            LicensedUser.Tag = Mother.DAL.dsSQLCfg.tSQLCfg.Rows(0).Item("LicensedUser").ToString
            LicenseDateTime.Text = Mother.DAL.dsSQLCfg.tSQLCfg.Rows(0).Item("LicenseDate").ToString
            LicenseDateTime.Tag = Mother.DAL.dsSQLCfg.tSQLCfg.Rows(0).Item("LicenseDate").ToString
            LicensedInstanceCount.SelectedItem = Mother.DAL.dsSQLCfg.tSQLCfg.Rows(0).Item("LicensedInstanceCount").ToString
            LicensedInstanceCount.Tag = Mother.DAL.dsSQLCfg.tSQLCfg.Rows(0).Item("LicensedInstanceCount").ToString
            LicenseCode.Text = Mother.DAL.dsSQLCfg.tSQLCfg.Rows(0).Item("LicenseCode").ToString
            LicenseCode.Tag = Mother.DAL.dsSQLCfg.tSQLCfg.Rows(0).Item("LicenseCode").ToString
        Catch ex As Exception
            Throw New Exception("(LicenseForm.SetFormValuesFromDb) Exception.", ex)
        End Try
    End Sub


    Private Sub ApplyButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ApplyButton.Click
        Try
            SaveChangesToDataSet()
        Catch ex As Exception
            Mother.HandleException(New Exception("(LicenseForm.ApplyButton_Click) Exception.", ex))
        End Try
    End Sub

    Private Sub CancelButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cancel.Click
        Try
            RevertChangesToDataSet()
            Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.Close()
        Catch ex As Exception
            Throw New Exception("(LicenseForm.CancelButton_Click) Exception.", ex)
        End Try
    End Sub

    Private Sub OKButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles OKButton.Click
        Try
            'license check in save
            SaveChangesToDataSet()
            Me.DialogResult = System.Windows.Forms.DialogResult.OK
            Me.Close()
        Catch ex As Exception
            Throw New Exception("(LicenseForm.OKButton_Click) Exception.", ex)
        End Try
    End Sub

    Private Sub ResetButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ResetButton.Click
        Try
            LicensedCompany.Text = ""
            LicensedUser.Text = ""
            LicensedInstanceCount.SelectedItem = "3"
            LicenseDateTime.Text = Today.ToString("MMMM dd, yyyy")
            LicenseCode.Text = "Unlicensed Trial Software"
        Catch ex As Exception
            Throw New Exception("(LicenseForm.ResetButton_Click) Exception.", ex)
        End Try
    End Sub

    Private Sub SaveChangesToDataSet()
        Try
            'If Mother.DAL.AvailableLicenses < 0 Then
            '    RevertChangesToDataSet()
            'End If
            LicensedCompany.Text = Trim(LicensedCompany.Text)
            LicensedUser.Text = Trim(LicensedUser.Text)
            LicenseCode.Text = Trim(LicenseCode.Text)
            Mother.DAL.dsSQLCfg.tSQLCfg.Rows(0).Item("LicensedCompany") = LicensedCompany.Text
            Mother.DAL.dsSQLCfg.tSQLCfg.Rows(0).Item("LicensedUser") = LicensedUser.Text
            Mother.DAL.dsSQLCfg.tSQLCfg.Rows(0).Item("LicenseDate") = LicenseDateTime.Text
            Mother.DAL.dsSQLCfg.tSQLCfg.Rows(0).Item("LicensedInstanceCount") = LicensedInstanceCount.SelectedItem
            Mother.DAL.dsSQLCfg.tSQLCfg.Rows(0).Item("LicenseCode") = LicenseCode.Text
            Mother.DAL.SaveSQLCfg()
            ' everything that runs in the background uses static data, if settings reload causes losts changes it will
            ' only be for things left in progress when the license was accessed, not very likely
            ' because this needs to work rather than trying to work along side the unknown
            ' estimated usage is very low (10 times maybe?) for the life of the installation
            My.Settings.Reload()
            My.Settings.LicensedCompany = LicensedCompany.Text
            My.Settings.LicensedUser = LicensedUser.Text
            My.Settings.LicensedInstanceCount = CInt(LicensedInstanceCount.SelectedItem)
            My.Settings.LicenseDate = LicenseDateTime.Text
            My.Settings.LicenseCode = LicenseCode.Text
            My.Settings.Save()
            SetFormValuesFromDb()
        Catch ex As Exception
            Throw New Exception("(LicenseForm.SaveChangesToDataSet) Exception.", ex)
        End Try
    End Sub

    Private Sub RevertChangesToDataSet()
        Try
            LicensedCompany.Text = LicensedCompany.Tag.ToString
            LicensedUser.Text = LicensedUser.Tag.ToString
            LicenseDateTime.Text = LicenseDateTime.Tag.ToString
            LicensedInstanceCount.SelectedItem = LicensedInstanceCount.Tag.ToString
            LicenseCode.Text = LicenseCode.Tag.ToString
        Catch ex As Exception
            Throw New Exception("(LicenseForm.RevertChangesToDataSet) Exception.", ex)
        End Try
    End Sub

End Class