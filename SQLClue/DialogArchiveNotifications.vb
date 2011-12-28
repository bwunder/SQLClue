Imports System.Windows.Forms

Public Class DialogArchiveNotifications
    Private SuccessNotifications As DataTable
    Private FailNotifications As DataTable

    Private ReadOnly Property oRepository() As Smo.Server
        Get
            Return CType(Me.Tag, Smo.Server)
        End Get
    End Property

    Private Sub DialogArchiveNotifications_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ' the menu item on Mother only allows non-SQLExpress with operators to get here
        SuccessNotifications = oRepository.JobServer.Alerts(My.Resources.ArchiveSuccessNotificationName).EnumNotifications
        FailNotifications = oRepository.JobServer.Alerts(My.Resources.ArchiveFailNotificationName).EnumNotifications
        SetSuccess()
        SetFail()
    End Sub

    Private Sub SetSuccess()
        CheckBoxSuccessEmail.Enabled = False
        CheckBoxSuccessPager.Enabled = False
        CheckBoxSuccessNetSend.Enabled = False
        If Not oRepository.JobServer.Alerts(My.Resources.ArchiveSuccessNotificationName) Is Nothing Then
            CheckBoxAlertOnSuccess.Enabled = True
            CheckBoxAlertOnSuccess.Checked = oRepository.JobServer.Alerts(My.Resources.ArchiveSuccessNotificationName).IsEnabled
            If CheckBoxAlertOnSuccess.Checked Then
                ' if 0 (no) or 1 operator configured we can proceed
                If SuccessNotifications.Rows.Count < 2 Then
                    CheckBoxNotifyOnSuccess.Checked = CBool(oRepository.JobServer.Alerts(My.Resources.ArchiveSuccessNotificationName).HasNotification)
                    If CheckBoxNotifyOnSuccess.Checked Then
                        If oRepository.JobServer.Operators.Count > 0 Then
                            If SuccessNotifications.Rows.Count = 1 Then
                                ComboBoxSuccessOperator.Items.Clear()
                                For Each op In oRepository.JobServer.Operators
                                    ComboBoxSuccessOperator.Items.Add(op.Name.ToString)
                                Next
                                ComboBoxSuccessOperator.Text = SuccessNotifications.Rows(0).Item("OperatorName").ToString
                                CheckBoxSuccessEmail.Checked = CBool(SuccessNotifications.Rows(0).Item("UseEmail"))
                                CheckBoxSuccessPager.Checked = CBool(SuccessNotifications.Rows(0).Item("UsePager"))
                                CheckBoxSuccessNetSend.Checked = CBool(SuccessNotifications.Rows(0).Item("UseNetSend"))
                            End If
                            RichTextBoxSuccessMessages.Text = ""
                        Else
                            RichTextBoxSuccessMessages.Text = My.Resources.ArchiveNotificationOperatorRequired
                        End If
                    End If
                Else
                    RichTextBoxSuccessMessages.Text = My.Resources.ArchiveNotificationHasBeenCustomized
                End If
            End If
        Else
            RichTextBoxSuccessMessages.Text = String.Format(My.Resources.ArchiveNotificationAlertNotFound, _
                                                            My.Resources.ArchiveSuccessNotificationName)
        End If

    End Sub

    Private Sub CheckBoxAlertOnSuccess_CheckStateChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CheckBoxAlertOnSuccess.CheckStateChanged
        If CType(sender, CheckBox).Checked Then
            ' from disabled to enabled check for customized 
            If SuccessNotifications.Rows.Count < 2 Then
                CheckBoxNotifyOnSuccess.Enabled = True
            Else
                RichTextBoxSuccessMessages.Text = My.Resources.ArchiveNotificationHasBeenCustomized
            End If
        Else
            If CheckBoxNotifyOnSuccess.Checked Then
                CheckBoxNotifyOnSuccess.Checked = False
            End If
            CheckBoxNotifyOnSuccess.Enabled = False
            RichTextBoxSuccessMessages.Text = ""
        End If
    End Sub

    Private Sub CheckBoxNotifyOnSuccess_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CheckBoxNotifyOnSuccess.CheckedChanged
        ResetSuccessNotify()
    End Sub

    Private Sub ResetSuccessNotify()
        CheckBoxSuccessEmail.Checked = False
        CheckBoxSuccessPager.Checked = False
        CheckBoxSuccessNetSend.Checked = False
        If CheckBoxNotifyOnSuccess.Checked Then
            ComboBoxSuccessOperator.Enabled = True
        Else
            ComboBoxSuccessOperator.Enabled = False
        End If
        ComboBoxSuccessOperator.Text = ""
        RichTextBoxSuccessMessages.Text = ""
    End Sub

    Private Sub ComboBoxSuccessOperator_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles ComboBoxSuccessOperator.DropDown
        If oRepository.JobServer.Operators.Count > 0 Then
            ComboBoxSuccessOperator.Items.Clear()
            For Each op In oRepository.JobServer.Operators
                ComboBoxSuccessOperator.Items.Add(op.Name.ToString)
            Next
            RichTextBoxSuccessMessages.Text = ""
        Else
            RichTextBoxSuccessMessages.Text = My.Resources.ArchiveNotificationOperatorRequired
        End If
    End Sub

    Private Sub ComboBoxSuccessOperator_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ComboBoxSuccessOperator.TextChanged
        If ComboBoxSuccessOperator.Text <> "" Then
            If oRepository.JobServer.Operators(ComboBoxSuccessOperator.Text).EmailAddress <> "" Then
                CheckBoxSuccessEmail.Enabled = True
            End If
            If oRepository.JobServer.Operators(ComboBoxSuccessOperator.Text).PagerAddress <> "" Then
                CheckBoxSuccessPager.Enabled = True
            End If
            If oRepository.JobServer.Operators(ComboBoxSuccessOperator.Text).NetSendAddress <> "" Then
                CheckBoxSuccessNetSend.Enabled = True
            End If
            RichTextBoxSuccessMessages.Text = My.Resources.ArchiveNotificationSelectMethods
        Else
            CheckBoxSuccessEmail.Enabled = False
            CheckBoxSuccessPager.Enabled = False
            CheckBoxSuccessNetSend.Enabled = False
            RichTextBoxSuccessMessages.Text = ""
        End If
    End Sub

    Private Sub SetFail()
        CheckBoxFailEmail.Enabled = False
        CheckBoxFailPager.Enabled = False
        CheckBoxFailNetSend.Enabled = False
        If Not oRepository.JobServer.Alerts(My.Resources.ArchiveFailNotificationName) Is Nothing Then
            CheckBoxAlertOnFail.Enabled = True
            CheckBoxAlertOnFail.Checked = oRepository.JobServer.Alerts(My.Resources.ArchiveFailNotificationName).IsEnabled
            If CheckBoxAlertOnFail.Checked Then
                ' if 0 (no) or 1 operator configured we can proceed
                If FailNotifications.Rows.Count < 2 Then
                    CheckBoxNotifyOnFail.Checked = CBool(oRepository.JobServer.Alerts(My.Resources.ArchiveFailNotificationName).HasNotification)
                    If CheckBoxNotifyOnFail.Checked Then
                        If oRepository.JobServer.Operators.Count > 0 Then
                            If FailNotifications.Rows.Count = 1 Then
                                ComboBoxFailOperator.Items.Clear()
                                For Each op In oRepository.JobServer.Operators
                                    ComboBoxFailOperator.Items.Add(op.Name.ToString)
                                Next
                                ComboBoxFailOperator.Text = FailNotifications.Rows(0).Item("OperatorName").ToString
                                CheckBoxFailEmail.Checked = CBool(FailNotifications.Rows(0).Item("UseEmail"))
                                CheckBoxFailPager.Checked = CBool(FailNotifications.Rows(0).Item("UsePager"))
                                CheckBoxFailNetSend.Checked = CBool(FailNotifications.Rows(0).Item("UseNetSend"))
                            End If
                            RichTextBoxFailMessages.Text = ""
                        Else
                            RichTextBoxFailMessages.Text = My.Resources.ArchiveNotificationOperatorRequired
                        End If
                    End If
                Else
                    RichTextBoxFailMessages.Text = My.Resources.ArchiveNotificationHasBeenCustomized
                End If
            End If
        Else
            RichTextBoxSuccessMessages.Text = String.Format(My.Resources.ArchiveNotificationAlertNotFound, _
                                                            My.Resources.ArchiveFailNotificationName)
        End If

    End Sub

    Private Sub CheckBoxAlertOnFail_CheckStateChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CheckBoxAlertOnFail.CheckStateChanged
        If CType(sender, CheckBox).Checked Then
            ' from disabled to enabled check for customized 
            If FailNotifications.Rows.Count < 2 Then
                CheckBoxNotifyOnFail.Enabled = True
            Else
                RichTextBoxFailMessages.Text = My.Resources.ArchiveNotificationHasBeenCustomized
            End If
        Else
            If CheckBoxNotifyOnFail.Checked Then
                CheckBoxNotifyOnFail.Checked = False
            End If
            CheckBoxNotifyOnFail.Enabled = False
            RichTextBoxFailMessages.Text = ""
        End If
    End Sub

    Private Sub CheckBoxNotifyOnFail_CheckStateChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CheckBoxNotifyOnFail.CheckStateChanged
        ResetFailNotify()
    End Sub

    Private Sub ResetFailNotify()
        CheckBoxFailEmail.Checked = False
        CheckBoxFailPager.Checked = False
        CheckBoxFailNetSend.Checked = False
        If CheckBoxNotifyOnFail.Checked Then
            ComboBoxFailOperator.Enabled = True
        Else
            ComboBoxFailOperator.Enabled = False
        End If
        ComboBoxFailOperator.Text = ""
        RichTextBoxFailMessages.Text = ""
    End Sub

    Private Sub ComboBoxFailOperator_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles ComboBoxFailOperator.DropDown
        If oRepository.JobServer.Operators.Count > 0 Then
            ComboBoxFailOperator.Items.Clear()
            For Each op In oRepository.JobServer.Operators
                ComboBoxFailOperator.Items.Add(op.Name.ToString)
            Next
            RichTextBoxFailMessages.Text = ""
        Else
            RichTextBoxSuccessMessages.Text = My.Resources.ArchiveNotificationOperatorRequired
        End If
    End Sub

    Private Sub ComboBoxFailOperator_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ComboBoxFailOperator.TextChanged
        If ComboBoxFailOperator.Text <> "" Then
            If oRepository.JobServer.Operators(ComboBoxFailOperator.Text).EmailAddress <> "" Then
                CheckBoxFailEmail.Enabled = True
            End If
            If oRepository.JobServer.Operators(ComboBoxFailOperator.Text).PagerAddress <> "" Then
                CheckBoxFailPager.Enabled = True
            End If
            If oRepository.JobServer.Operators(ComboBoxFailOperator.Text).NetSendAddress <> "" Then
                CheckBoxFailNetSend.Enabled = True
            End If
            RichTextBoxFailMessages.Text = My.Resources.ArchiveNotificationSelectMethods
        Else
            CheckBoxFailEmail.Enabled = False
            CheckBoxFailPager.Enabled = False
            CheckBoxFailNetSend.Enabled = False
            RichTextBoxFailMessages.Text = ""
        End If
    End Sub

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        With oRepository.JobServer.Alerts(My.Resources.ArchiveSuccessNotificationName)
            If SuccessNotifications.Rows.Count = 1 _
            AndAlso SuccessNotifications.Rows(0).Item("OperatorName").ToString <> ComboBoxSuccessOperator.Text Then
                .RemoveNotification(SuccessNotifications.Rows(0).Item("OperatorName").ToString)
            End If
            If CheckBoxAlertOnSuccess.Checked Then
                .IsEnabled = CheckBoxAlertOnSuccess.Checked
                If CheckBoxNotifyOnSuccess.Checked Then
                    If ComboBoxSuccessOperator.Text <> "" Then
                        Dim SuccessNotifyMethods As Integer = CInt(If(CheckBoxSuccessEmail.Checked, NotifyMethods.NotifyEmail, NotifyMethods.None)) + _
                                                              CInt(If(CheckBoxSuccessNetSend.Checked, NotifyMethods.NetSend, NotifyMethods.None)) + _
                                                              CInt(If(CheckBoxSuccessPager.Checked, NotifyMethods.Pager, NotifyMethods.None))
                        If (SuccessNotifyMethods > 0) Then
                            If CheckBoxNotifyOnSuccess.Checked Then
                                If oRepository.JobServer.Alerts(My.Resources.ArchiveSuccessNotificationName).EnumNotifications.Rows.Count = 0 Then
                                    .AddNotification(ComboBoxSuccessOperator.Text, CType(SuccessNotifyMethods, NotifyMethods))
                                Else
                                    ' update if something changed
                                    If CheckBoxSuccessEmail.Checked <> CBool(SuccessNotifications.Rows(0).Item("UseEmail")) _
                                    Or CheckBoxSuccessPager.Checked <> CBool(SuccessNotifications.Rows(0).Item("UsePager")) _
                                    Or CheckBoxSuccessNetSend.Checked <> CBool(SuccessNotifications.Rows(0).Item("UseNetSend")) Then
                                        .UpdateNotification(ComboBoxSuccessOperator.Text, CType(SuccessNotifyMethods, NotifyMethods))
                                    End If
                                End If
                            End If
                        Else
                            Mother.HandleException(New ApplicationException(String.Format(My.Resources.ArchiveNotificationNoMethodsEnabled, _
                                                                                          ComboBoxSuccessOperator.Text, "Success")))
                            .RemoveNotification(SuccessNotifications.Rows(0).Item("OperatorName").ToString)
                        End If
                    End If
                End If
            End If
        End With
        With oRepository.JobServer.Alerts(My.Resources.ArchiveFailNotificationName)
            If FailNotifications.Rows.Count = 1 _
            AndAlso FailNotifications.Rows(0).Item("OperatorName").ToString <> ComboBoxFailOperator.Text Then
                .RemoveNotification(FailNotifications.Rows(0).Item("OperatorName").ToString)
            End If
            If CheckBoxAlertOnFail.Checked Then
                .IsEnabled = CheckBoxAlertOnFail.Checked
                If CheckBoxNotifyOnFail.Checked Then
                    If ComboBoxFailOperator.Text <> "" Then
                        Dim FailNotifyMethods As Integer = CInt(If(CheckBoxFailEmail.Checked, NotifyMethods.NotifyEmail, NotifyMethods.None)) + _
                                                           CInt(If(CheckBoxFailNetSend.Checked, NotifyMethods.NetSend, NotifyMethods.None)) + _
                                                           CInt(If(CheckBoxFailPager.Checked, NotifyMethods.Pager, NotifyMethods.None))
                        If (FailNotifyMethods > 0) Then
                            If CheckBoxNotifyOnFail.Checked Then
                                If oRepository.JobServer.Alerts(My.Resources.ArchiveFailNotificationName).EnumNotifications.Rows.Count = 0 Then
                                    .AddNotification(ComboBoxFailOperator.Text, CType(FailNotifyMethods, NotifyMethods))
                                Else
                                    ' update if something changed
                                    If CheckBoxFailEmail.Checked <> CBool(FailNotifications.Rows(0).Item("UseEmail")) _
                                    Or CheckBoxFailPager.Checked <> CBool(FailNotifications.Rows(0).Item("UsePager")) _
                                    Or CheckBoxFailNetSend.Checked <> CBool(FailNotifications.Rows(0).Item("UseNetSend")) Then
                                        .UpdateNotification(ComboBoxFailOperator.Text, CType(FailNotifyMethods, NotifyMethods))
                                    End If
                                End If
                            End If
                        Else
                            Mother.HandleException(New ApplicationException(String.Format(My.Resources.ArchiveNotificationNoMethodsEnabled, _
                                                                                          ComboBoxSuccessOperator.Text, "Archive Failure")))
                            .RemoveNotification(FailNotifications.Rows(0).Item("OperatorName").ToString)
                        End If
                    End If
                End If
            End If
        End With
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub DialogArchiveNotifications_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        CheckBoxAlertOnSuccess.Checked = False
        CheckBoxAlertOnFail.Checked = False
        ResetSuccessNotify()
        ResetFailNotify()
    End Sub

End Class
