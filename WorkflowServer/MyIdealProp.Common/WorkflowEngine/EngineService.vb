Imports System.ServiceProcess
Imports MyIdealProp.Workflow.Core
Imports MyIdealProp.Workflow.Core.Runtime

Public Class EngineService

    Private EventLog1 As System.Diagnostics.EventLog
    Private runTime As MyIdealProp.Workflow.Core.Runtime.WorkflowRuntime

    Public Sub New(ByVal cmdArgs() As String)
        InitializeComponent()
        Dim eventSourceName As String = "WorkflowEngine"
        Dim logName As String = "EngineLog"
        If (cmdArgs.Count() > 0) Then
            eventSourceName = cmdArgs(0)
        End If

        If (cmdArgs.Count() > 1) Then
            logName = cmdArgs(1)
        End If

        Me.EventLog1 = New System.Diagnostics.EventLog
        If Not System.Diagnostics.EventLog.SourceExists(eventSourceName) Then
            System.Diagnostics.EventLog.CreateEventSource(eventSourceName, logName)
        End If
        EventLog1.Source = eventSourceName
        EventLog1.Log = logName
    End Sub

    Shared Sub Main(ByVal cmdArgs() As String)
        Dim ServicesToRun() As System.ServiceProcess.ServiceBase
        ServicesToRun = New System.ServiceProcess.ServiceBase() {New EngineService(cmdArgs)}
        System.ServiceProcess.ServiceBase.Run(ServicesToRun)
    End Sub


    Protected Overrides Sub OnStart(ByVal args() As String)
        ' Add code here to start your service. This method should set things
        ' in motion so your service can do its work.

        EventLog1.WriteEntry("In OnStart")

        If runTime Is Nothing Then
            runTime = New WorkflowRuntime(Guid.NewGuid)
           
            'orkflowRuntimeConfigurationExtension.WithRuntimePersistance(_wfRunTime, New RuntimePersistenceProvider)

            runTime.Start()
        End If

        EventLog1.WriteEntry("Engine Started")
    End Sub
    
    Protected Overrides Sub OnStop()
        EventLog1.WriteEntry("Engine Stoped")
        ' Add code here to perform any tear-down necessary to stop your service.
        If runTime IsNot Nothing Then
            runTime.Close()
        End If
    End Sub
End Class
