Public NotInheritable Class SplashScreen1

    'TODO: This form can easily be set as the splash screen for the application by going to the "Application" tab
    '  of the Project Designer ("Properties" under the "Project" menu).

    Delegate Sub CurrentStatusEventHandlerDelegate(ByVal Message As String)
    Delegate Sub ToggleSplashVisibleDelegate()


    Public Sub CurrentStatusEventHandler(ByVal Message As String)

        If Me.InvokeRequired Then
            Me.Invoke(New CurrentStatusEventHandlerDelegate(AddressOf CurrentStatusEventHandler), Message)
        Else
            ToolStripStatusLabel1.Text = Message
            My.Application.DoEvents()
        End If

    End Sub
    Public Sub ToggleSplashVisible()

        If Me.InvokeRequired Then
            Me.Invoke(New ToggleSplashVisibleDelegate(AddressOf ToggleSplashVisible))
        Else
            If Me.Visible Then
                Me.Visible = False
            Else
                Me.Visible = True
            End If
            My.Application.DoEvents()
        End If

    End Sub

    Private Sub SplashScreen1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'Set up the dialog text at runtime according to the application's assembly information.  

        'TODO: Customize the application's assembly information in the "Application" pane of the project 
        '  properties dialog (under the "Project" menu).

        'Application title
        If My.Application.Info.Title <> "" Then
            ApplicationTitle.Text = My.Application.Info.Title
        Else
            'If the application title is missing, use the application name, without the extension
            ApplicationTitle.Text = System.IO.Path.GetFileNameWithoutExtension(My.Application.Info.AssemblyName)
        End If

        'Format the version information using the text set into the Version control at design time as the
        '  formatting string.  This allows for effective localization if desired.
        '  Build and revision information could be included by using the following code and changing the 
        '  Version control's designtime text to "Version {0}.{1:00}.{2}.{3}" or something similar.  See
        '  String.Format() in Help for more information.
        '
        Version.Text = System.String.Format(Version.Text, _
                                            My.Application.Info.Version.Major, _
                                            My.Application.Info.Version.Minor, _
                                            My.Application.Info.Version.Build, _
                                            My.Application.Info.Version.Revision)


        Copyright.Text = My.Application.Info.Copyright 
        'SplashMessage.Text = My.Application.Info.Description
    End Sub

    Private Sub SplashMessage_Click(sender As System.Object, e As System.EventArgs) Handles SplashMessage.Click

    End Sub
End Class
