Imports System.ServiceProcess

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class SQLClueSvc
    Inherits System.ServiceProcess.ServiceBase

    'UserService overrides dispose to clean up the component list.
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

    ' The main entry point for the process
    <MTAThread()> _
    <System.Diagnostics.DebuggerNonUserCode()> _
    Shared Sub Main()

        ' this SUPPOSEDLY allows the onstart to be debugged but the service cannot run
        ' not sue how to restate this in VB
        '(new ManagedWindowsService()).OnStart(); // allows easy debugging of OnStart()
        'ServiceBase.Run( new ManagedWindowsService() );



        ' this allows the process to run as a non-service.
        ' It will kick off the service start point, but never kill it.
        ' Shut down the debugger to exit
        ''Dim service As SQLClueSvc = New SQLClueSvc
        ''service.RunComponentWatchers()
        'Put a breakpoint on the following line to always catch
        'service when it has finished its work
        ''System.Threading.Thread.Sleep(System.Threading.Timeout.Infinite)


        ' More than one NT Service may run within the same process. To add
        ' another service to this process, change the following line to
        ' create a second service object. For example,
        '
        '   ServicesToRun = New System.ServiceProcess.ServiceBase () {New Service1, New MySecondUserService}
        '
        Try

            Dim ServicesToRun() As System.ServiceProcess.ServiceBase
            ServicesToRun = New System.ServiceProcess.ServiceBase() {New SQLClueSvc}
            System.ServiceProcess.ServiceBase.Run(ServicesToRun)

        Catch ex As Exception

            Dim ex1 As System.Exception = ex
            Dim ex1Msg As String = ex1.Message
            While Not ex1.InnerException Is Nothing
                ex1Msg = ex1Msg & vbCrLf & vbCrLf & ex1.InnerException.Message
                ex1 = ex1.InnerException
            End While
            ex1Msg = ex1Msg & vbCrLf & vbCrLf & "For Additional help with this error check the web at http://www.bwunder.com or email Bill at bwunder@yahoo.com (Send the error too - Use the icon below to copy the error)."

            My.Application.Log.WriteEntry(ex1Msg, _
                                          TraceEventType.Error)

        End Try

    End Sub

    'Required by the Component Designer
    Private components As System.ComponentModel.IContainer

    ' NOTE: The following procedure is required by the Component Designer
    ' It can be modified using the Component Designer.  
    ' Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.EventLogSvc = New System.Diagnostics.EventLog
        Me.ArchiveBackgroundWorker = New System.ComponentModel.BackgroundWorker
        Me.RunbookBackgroundWorker = New System.ComponentModel.BackgroundWorker
        Me.BaselineBackgroundWorker = New System.ComponentModel.BackgroundWorker
        CType(Me.EventLogSvc, System.ComponentModel.ISupportInitialize).BeginInit()
        '
        'EventLogSvc
        '
        Me.EventLogSvc.Log = "Application"
        Me.EventLogSvc.Source = "SQLClueSvc"
        '
        'ArchiveBackgroundWorker
        '
        Me.ArchiveBackgroundWorker.WorkerSupportsCancellation = True
        '
        'RunbookBackgroundWorker
        '
        Me.RunbookBackgroundWorker.WorkerSupportsCancellation = True
        '
        'BaselineBackgroundWorker
        '
        Me.BaselineBackgroundWorker.WorkerSupportsCancellation = True
        '
        'SQLClueSvc
        '
        Me.CanHandlePowerEvent = True
        Me.CanHandleSessionChangeEvent = True
        Me.CanPauseAndContinue = True
        Me.CanShutdown = True
        Me.ServiceName = "SQLClueService"
        CType(Me.EventLogSvc, System.ComponentModel.ISupportInitialize).EndInit()

    End Sub
    Friend WithEvents EventLogSvc As System.Diagnostics.EventLog
    Friend WithEvents ArchiveBackgroundWorker As System.ComponentModel.BackgroundWorker
    Friend WithEvents RunbookBackgroundWorker As System.ComponentModel.BackgroundWorker
    Friend WithEvents BaselineBackgroundWorker As System.ComponentModel.BackgroundWorker

End Class
