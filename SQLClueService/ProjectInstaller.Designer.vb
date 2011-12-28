﻿<System.ComponentModel.RunInstaller(True)> Partial Class ProjectInstaller
    Inherits System.Configuration.Install.Installer

    'Installer overrides dispose to clean up the component list.
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

    'Required by the Component Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Component Designer
    'It can be modified using the Component Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.ServiceProcessInstallerSQLClueService = New System.ServiceProcess.ServiceProcessInstaller
        Me.ServiceInstallerSQLClueService = New System.ServiceProcess.ServiceInstaller
        '
        'ServiceProcessInstallerSQLClueService
        '
        Me.ServiceProcessInstallerSQLClueService.Password = Nothing
        Me.ServiceProcessInstallerSQLClueService.Username = Nothing
        '
        'ServiceInstallerSQLClueService
        '
        Me.ServiceInstallerSQLClueService.Description = "SQLClue Automation Controller Service"
        Me.ServiceInstallerSQLClueService.DisplayName = "SQLClue Automation Controller Service"
        Me.ServiceInstallerSQLClueService.ServiceName = "SQLClueSvc"
        Me.ServiceInstallerSQLClueService.StartType = System.ServiceProcess.ServiceStartMode.Automatic
        '
        'ProjectInstaller
        '
        Me.Installers.AddRange(New System.Configuration.Install.Installer() {Me.ServiceProcessInstallerSQLClueService, Me.ServiceInstallerSQLClueService})

    End Sub
    Friend WithEvents ServiceProcessInstallerSQLClueService As System.ServiceProcess.ServiceProcessInstaller
    Friend WithEvents ServiceInstallerSQLClueService As System.ServiceProcess.ServiceInstaller

End Class
