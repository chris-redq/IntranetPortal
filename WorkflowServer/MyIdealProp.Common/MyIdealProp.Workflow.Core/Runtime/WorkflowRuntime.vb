Imports System.Collections.Generic
Imports System.Globalization
Imports System.Linq
Imports MyIdealProp.Workflow.Core.Builder
Imports MyIdealProp.Workflow.Core.Action
Imports MyIdealProp.Workflow.Core.Fault
Imports MyIdealProp.Workflow.Core.Model
Imports MyIdealProp.Common
Imports System
Imports System.Threading
Imports System.ServiceModel
Imports System.ServiceModel.Description

Namespace MyIdealProp.Workflow.Core.Runtime
    Public NotInheritable Class WorkflowRuntime

        Public Property Id() As Guid

        Friend _builder As IWorkflowBuilder
        Friend ReadOnly Property Builder() As IWorkflowBuilder
            Get
                Return _builder
            End Get
        End Property

        Public Sub Close()
            host.Close()
            _timer.Dispose()
            If timerExecute IsNot Nothing Then
                timerExecute.Dispose()
            End If

            Logger.Log.Info("Workflow Engine is closed.")
        End Sub

        Private wfDatabase As New Data.WorkflowDatabase

        Public Sub New(runtimeId As Guid)
            Id = runtimeId
        End Sub

        Private Shared runTimeforClient As WorkflowRuntime
        Public Shared Function GetInstance() As WorkflowRuntime
            Return runTimeforClient
        End Function

#Region "Workflow Service Host"

        Private host As ServiceHost

        Private Sub HostClientService()

            host = New ServiceHost(GetType(MyIdealProp.Workflow.Service.WorkflowService))
            host.Credentials.UserNameAuthentication.UserNamePasswordValidationMode = ServiceModel.Security.UserNamePasswordValidationMode.Custom
            host.Credentials.UserNameAuthentication.CustomUserNamePasswordValidator = New Service.ServiceUserNamePasswordValidator
            host.Open()

            Logger.Log.Info("Client Service Host is started.")
        End Sub

        Private mgrHost As ServiceHost
        Public Sub HostMngService()
            mgrHost = New ServiceHost(GetType(Service.ManagementService), New Uri("net.tcp://localhost:8000/ManagementService"))
            Dim smb = mgrHost.Description.Behaviors.Find(Of ServiceMetadataBehavior)()
            If smb Is Nothing Then
                smb = New ServiceMetadataBehavior
            End If

            smb.MetadataExporter.PolicyVersion = PolicyVersion.Policy15
            mgrHost.Description.Behaviors.Add(smb)

            mgrHost.AddServiceEndpoint(GetType(Service.IManagementService), New NetTcpBinding(SecurityMode.None, False), "")
            mgrHost.AddServiceEndpoint(ServiceMetadataBehavior.MexContractName, MetadataExchangeBindings.CreateMexTcpBinding(), "mex")

            mgrHost.Open()

            Logger.Log.Info("Management service host is started")
        End Sub
#End Region

#Region "Workflow Server engine"
        Private _isColdStart As Boolean

        Public Sub Start()
            Logger.Log.Info("Workflow server is starting.")

            Try
                HostClientService()
                HostMngService()

                _builder = New Builder.WorkflowBuilder(Of XElement)(New Parser.XmlWorkflowParser)

                StartTimer()
                StartProcInstExecutor()
                runTimeforClient = Me
                Logger.Log.Info("The Workflow engine is started and listen.")
            Catch ex As Exception
                Logger.Log.Error("Error Workflow engine Start", ex)
            End Try
        End Sub
#End Region

#Region "Process Executor"

        Private procInstToExecutor As List(Of ProcessInstance)

        Dim timerExecute As Threading.Timer
        Public Sub StartProcInstExecutor()
            Dim stateObj = New With {.InUse = False}

            Dim TimerDelegate As New System.Threading.TimerCallback(AddressOf RefreshProcInstBus)
            timerExecute = New System.Threading.Timer(TimerDelegate, stateObj, New TimeSpan(1000), New TimeSpan(0, 0, 2))

            Logger.Log.Info("Process Instance Executor is stared.")
        End Sub

        Public Sub RefreshProcInstBus(state As Object)
            If state.InUse Then
                Return
            Else
                state.InUse = True
            End If

            UpdateRunningTime()

            If procInstToExecutor Is Nothing OrElse procInstToExecutor.Count = 0 Then
                Try
                    procInstToExecutor = wfDatabase.GetProcInstToExecutor()
                Catch ex As Exception
                    Logger.Log.Error("Error in GetProcInstToExecutor", ex)
                End Try
            End If

            If procInstToExecutor IsNot Nothing AndAlso procInstToExecutor.Count > 0 Then
                For Each procInst In procInstToExecutor
                    'procInst = procInstToExecutor.First
                    Try
                        ExecutorProcInst(procInst)
                    Catch ex As Exception
                        LogProcInstError(ex, procInst)
                    End Try
                Next

                procInstToExecutor.Clear()
            End If

            state.InUse = False
        End Sub

        Public Sub UpdateRunningTime()
            Dim serverName = Environment.MachineName
            If String.IsNullOrEmpty(serverName) Then
                serverName = "localhost"
            End If

            Try
                wfDatabase.UpdateLastRunningTime(serverName)
            Catch ex As Exception
                Logger.Log.Error("Error in Update Running Time", ex)
            End Try
        End Sub

        Private Sub ExecutorProcInst(procInst As ProcessInstance)
            If procInst.ActivityInstanceLog.Count = 0 Then
                ExecuteRootActivity2(procInst)
            Else
                Dim runningActInsts = procInst.ActivityInstanceLog.Where(Function(act) act.Status = ActivityStatus.Running).ToList
                If runningActInsts IsNot Nothing AndAlso runningActInsts.Count > 0 Then
                    For Each actInst In runningActInsts
                        ExecutorActInst(actInst)

                        If actInst.Status = ActivityStatus.Completed Then
                            ExecuteOutcome(actInst)
                        Else
                            If actInst.IsClient Then
                                ExecuteWorklistItem(actInst)
                            End If
                        End If
                    Next
                Else
                    Dim lastActivity = procInst.ActivityInstanceLog.Last
                    If lastActivity.Status = ActivityStatus.Completed Then
                        ExecuteOutcome(lastActivity)
                    End If
                End If
            End If

            If procInst.Status = ProcessStatus.InError Then
                SetProcessNewStatus(procInst, ProcessStatus.InError)
            End If

            If procInst.Status = ProcessStatus.Running Then
                If procInst.ActivityInstanceLog.Where(Function(act) act.Status = ActivityStatus.Active Or act.Status = ActivityStatus.Running).Count = 0 Then
                    SetProcessNewStatus(procInst, ProcessStatus.Completed)
                Else
                    SetProcessNewStatus(procInst, ProcessStatus.Active)
                End If
            End If
        End Sub

        Private Sub ExecutorActInst(actInst As ActivityInstance)
            If actInst.EventInstanceLog Is Nothing Or actInst.EventInstanceLog.Count = 0 Then
                'Check if there is activity destination instance
                If actInst.ActivityDestinationInstances Is Nothing OrElse actInst.ActivityDestinationInstances.Count = 0 Then
                    ExecuteEvents(actInst)
                Else
                    Dim actDestInsts = actInst.ActivityDestinationInstances.Where(Function(act) act.Status = ActivityStatus.Running).ToList

                    For Each actDestInst In actDestInsts
                        Dim lastEvent = actDestInst.EventInstanceLog.Last

                        If lastEvent.Status = EventStatus.Completed Then
                            Dim events = actDestInst.ActivityInstance.ActivityDefinition.Events.Where(Function(evt) evt.Order > actDestInst.ClientEventInst.ActivityEvent.Order).ToList
                            If events.Count > 0 Then
                                For Each evt In events
                                    Dim eventInst = EventInstance.Create(actDestInst, evt)
                                    actDestInst.EventInstanceLog.Add(eventInst)

                                    wfDatabase.CreateEventInstance(eventInst)

                                    eventInst.Execute()

                                    If eventInst.IsClient Then
                                        eventInst.Status = EventStatus.Active
                                        actInst.IsClient = eventInst.IsClient
                                        actInst.ClientEventInst = eventInst
                                        Return
                                    End If

                                    eventInst.EndDate = DateTime.Now
                                    eventInst.Status = EventStatus.Completed
                                    wfDatabase.CompleteEvent(eventInst)
                                Next
                            Else
                                actDestInst.Status = ActivityStatus.Completed
                            End If
                        End If

                        If lastEvent.Status = EventStatus.Running Then
                            lastEvent.Execute()
                            lastEvent.EndDate = DateTime.Now
                            lastEvent.Status = EventStatus.Completed
                            wfDatabase.CompleteEvent(lastEvent)
                        End If

                        wfDatabase.UpdateActivityDestinationInstance(actDestInst)

                        actInst.ExecuteSuccessRule()
                        If actInst.SuccessRuleResult Then
                            CompleteActivityInstance(actInst)
                            Return
                        End If
                    Next

                    If actInst.Status = ActivityStatus.Running Then
                        actInst.Status = ActivityStatus.Active
                        wfDatabase.UpdateActivityInstance(actInst)
                    End If
                End If
            Else
                Dim lastEvent = actInst.EventInstanceLog.Last

                If lastEvent.Status = EventStatus.Completed Then
                    Dim events = actInst.ActivityDefinition.Events.Where(Function(evt) evt.Order > actInst.ClientEventInst.ActivityEvent.Order).ToList
                    If events.Count > 0 Then
                        For Each evt In events
                            Dim eventInst = EventInstance.Create(actInst, evt)
                            actInst.EventInstanceLog.Add(eventInst)

                            wfDatabase.CreateEventInstance(eventInst)

                            eventInst.Execute()

                            If eventInst.IsClient Then
                                eventInst.Status = EventStatus.Active
                                actInst.IsClient = eventInst.IsClient
                                actInst.ClientEventInst = eventInst
                                Return
                            End If

                            eventInst.EndDate = DateTime.Now
                            eventInst.Status = EventStatus.Completed
                            wfDatabase.CompleteEvent(eventInst)
                        Next
                    Else
                        actInst.Status = ActivityStatus.Completed
                    End If
                End If

                If lastEvent.Status = EventStatus.Running Then
                    lastEvent.Execute()
                    lastEvent.EndDate = DateTime.Now
                    lastEvent.Status = EventStatus.Completed
                    wfDatabase.CompleteEvent(lastEvent)
                End If

                wfDatabase.UpdateActivityInstance(actInst)
            End If
        End Sub

        Private Sub CompleteActivityInstance(actInst As ActivityInstance)
            For Each actdest In actInst.ActivityDestinationInstances
                If actdest.Status = ActivityStatus.Active Then
                    actdest.Status = ActivityStatus.Expired
                End If

                If actdest.Status = ActivityStatus.Running Then
                    actdest.Status = ActivityStatus.Expired
                End If

                wfDatabase.UpdateActivityDestinationInstance(actdest)
            Next

            actInst.Status = ActivityStatus.Completed
            wfDatabase.UpdateActivityInstance(actInst)
        End Sub

        Private Sub ExecuteRootActivity2(processInstance As ProcessInstance)
            Dim act = ActivityInstance.Create(processInstance, processInstance.ProcessScheme.InitialActivity)
            act.Status = ActivityStatus.Running
            wfDatabase.CreateActivityInstance(act)
            act.Status = ActivityStatus.Completed
            wfDatabase.UpdateActivityInstance(act)
            ExecuteOutcome(act)
        End Sub

        Private Sub ExecuteOutcome(actInst As ActivityInstance)
            Dim act = actInst.ActivityDefinition

            Dim lines = actInst.ProcessInstance.ProcessScheme.GetAllLinesForActivity(act)

            If lines.Count > 0 Then
                For Each line In lines
                    Dim lineInst = LineInstance.Create(actInst.ProcessInstance, line)
                    lineInst.Execute()

                    wfDatabase.CreateLineInstance(lineInst)

                    If lineInst.LineRuleResult Then
                        Dim NewAct = line.To
                        ExecuteActivity(actInst.ProcessInstance, NewAct)
                    End If
                Next
            Else

            End If
        End Sub

        Private Sub ExecuteActivity(processInstance As ProcessInstance, activity As ActivityDefinition)
            Dim actInst = ActivityInstance.Create(processInstance, activity)
            actInst.Status = ActivityStatus.Running
            wfDatabase.CreateActivityInstance(actInst)

            'Execute the activity destination instances
            If actInst.ActivityDestinationInstances IsNot Nothing AndAlso actInst.ActivityDestinationInstances.Count > 0 Then
                For Each actDestInst In actInst.ActivityDestinationInstances
                    wfDatabase.CreateActDestInst(actDestInst)

                    ExecuteEvents(actDestInst)

                    If actDestInst.IsClient Then
                        ExecuteWorklistItem(actDestInst)
                        wfDatabase.UpdateActivityDestinationInstance(actDestInst)
                        wfDatabase.UpdateActivityInstance(actInst)
                        Continue For
                    Else
                        actDestInst.Status = ActivityStatus.Completed
                    End If

                    wfDatabase.UpdateActivityDestinationInstance(actDestInst)

                    actDestInst.ActivityInstance.ExecuteSuccessRule()

                    If actInst.SuccessRuleResult Then
                        actInst.Status = ActivityStatus.Completed
                        Exit For
                    End If
                Next
            Else
                ExecuteEvents(actInst)

                If actInst.IsClient Then
                    ExecuteWorklistItem(actInst)
                Else
                    actInst.Status = ActivityStatus.Completed
                End If
            End If

            wfDatabase.UpdateActivityInstance(actInst)

            If actInst.Status = ActivityStatus.Completed Then
                ExecuteOutcome(actInst)
            End If
        End Sub

        'need change to activity destination instance
        Private Sub ExecuteWorklistItem(actInst As ActivityInstance)
            actInst.Status = ActivityStatus.Active
            For Each destUser In actInst.DestinationUsers
                Dim item = WorklistItem.Create(actInst.ClientEventInst, destUser)
                wfDatabase.AddWorklistItem(item)
            Next

            'SetProcessNewStatus(actInst.ProcessInstance, ProcessStatus.Active)
        End Sub

        Private Sub ExecuteWorklistItem(actDestInst As ActivityDestinationInstance)
            actDestInst.Status = ActivityStatus.Active
            actDestInst.ActivityInstance.Status = ActivityStatus.Active

            Dim item = WorklistItem.Create(actDestInst.ClientEventInst, actDestInst.DestinationUser)
            wfDatabase.AddWorklistItem(item)

            'SetProcessNewStatus(actDestInst.ProcessInstance, ProcessStatus.Active)
        End Sub

        Private Sub ExecuteEvents(actInst As ActivityInstance)
            For Each actEvent In actInst.ActivityDefinition.Events
                Dim eventInst = EventInstance.Create(actInst, actEvent)
                eventInst.Status = EventStatus.Running
                actInst.EventInstanceLog.Add(eventInst)

                'PersistenceProvider.InitializeEvent(eventInst)
                wfDatabase.CreateEventInstance(eventInst)

                eventInst.Execute()

                If eventInst.IsClient Then
                    eventInst.Status = EventStatus.Active
                    actInst.IsClient = eventInst.IsClient
                    actInst.ClientEventInst = eventInst
                    Return
                End If

                eventInst.EndDate = DateTime.Now
                eventInst.Status = EventStatus.Completed
                wfDatabase.CompleteEvent(eventInst)
            Next
        End Sub

        Private Sub ExecuteEvents(actDestInst As ActivityDestinationInstance)
            For Each actEvent In actDestInst.ActivityInstance.ActivityDefinition.Events
                Dim eventInst = EventInstance.Create(actDestInst, actEvent)
                eventInst.Status = EventStatus.Running
                actDestInst.EventInstanceLog.Add(eventInst)

                'PersistenceProvider.InitializeEvent(eventInst)
                wfDatabase.CreateEventInstance(eventInst)

                eventInst.Execute()

                If eventInst.IsClient Then
                    eventInst.Status = EventStatus.Active
                    actDestInst.IsClient = eventInst.IsClient
                    actDestInst.ClientEventInst = eventInst
                    Return
                End If

                eventInst.EndDate = DateTime.Now
                eventInst.Status = EventStatus.Completed
                wfDatabase.CompleteEvent(eventInst)
            Next
        End Sub

        Private Sub LogProcInstError(exp As Exception, procInst As ProcessInstance)
            Dim log = ErrorLog.Create(procInst, exp)
            wfDatabase.AddErrorLogs(log)
            SetProcessNewStatus(procInst, ProcessStatus.InError)
        End Sub
#End Region

#Region "Escalation Timer"

        Private Sub StartTimer()
            Dim autoEvent As New AutoResetEvent(False)
            _timer = New System.Threading.Timer(AddressOf Me.RefreshTimer)
            RefreshTimer(New Object())

            Logger.Log.Info("Workflow Timer is started.")
        End Sub

        Private _timer As System.Threading.Timer
        Private Sub RefreshTimer(state As Object)

            Try
                Dim nextTimer = wfDatabase.GetNextTimer()

                If nextTimer IsNot Nothing Then
                    Dim interval = nextTimer.DueTime.Subtract(DateTime.UtcNow)
                    If interval.TotalMilliseconds < 0 Then
                        ExecuteTimerAction(nextTimer)
                        RefreshTimer(state)
                        Return
                    End If
                    _timer.Change(interval, New TimeSpan(0, 0, 0, 0, -1))
                Else
                    _timer.Change(New TimeSpan(0, 0, 1, 0), New TimeSpan(0, 0, 2, 0))
                End If
            Catch ex As Exception
                Logger.Log.Error("Error in RefreshTimer", ex)
                _timer.Change(New TimeSpan(0, 0, 1, 0), New TimeSpan(0, 0, 2, 0))
            End Try
        End Sub

        Private Sub ExecuteTimerAction(timer As TimerInstance)
            Dim procInst = wfDatabase.GetRunningProcInst(timer.ProcInstId)
            SetProcessNewStatus(procInst, ProcessStatus.Running)
            Dim actInst = procInst.ActivityInstanceLog.Single(Function(act) act.ActInstId = timer.ActInstId)
            Executor.ExecuteAction(procInst.ProcessScheme.Actions.Single(Function(act) act.Name = timer.ActionName), actInst)
            wfDatabase.FinishTimer(timer.TimerId)
            SetProcessNewStatus(procInst, ProcessStatus.Active)
        End Sub
#End Region

#Region "Process Instance"

        Public Function CreateProcessInstance(processName As String) As ProcessInstance
            Console.WriteLine("Create Process Instance " & processName)
            Dim procInst = Builder.CreateNewProcess(0, processName, New Dictionary(Of String, IEnumerable(Of Object))())
            Return procInst
        End Function

        Public Sub StartProcessInstance(ProcessInstance As ProcessInstance)
            SetProcessNewStatus(ProcessInstance, ProcessStatus.Initialized)

            Try
                SetProcessNewStatus(ProcessInstance, ProcessStatus.Running)
                'ExecuteRootActivity2(ProcessInstance)
            Catch generatedExceptionName As Exception
                Logger.Log.Error("Error Start Process Instance", generatedExceptionName)
            End Try
            'SetProcessNewStatus(ProcessInstance, ProcessStatus.Active)
            Logger.Log.Info("Start Process Instance " & ProcessInstance.Id)
        End Sub

        Public Sub GotoActivity(procInstId As Integer, actName As String)
            Dim procInst = wfDatabase.GetRunningProcInst(procInstId)
            Dim activeActs = procInst.ActivityInstanceLog.Where(Function(act) act.Status = ActivityStatus.Active)

            For Each act In activeActs
                act.Status = ActivityStatus.Expired
                wfDatabase.UpdateActivityInstance(act)

                wfDatabase.ExpiredWorklistItem(procInstId, act.ActInstId)
            Next

            Dim newAct = procInst.ActivityInstanceLog.Single(Function(act) act.ActivityName = actName)
            If newAct IsNot Nothing Then
                newAct.Status = ActivityStatus.Running
                wfDatabase.UpdateActivityInstance(newAct)

                SetProcessNewStatus(procInst, ProcessStatus.Running)
            Else
                Throw New Exception("Activity Name " & actName & " doesn't existed.")
            End If
        End Sub

        Public Sub TeminateProcessInstance(procInstId As Integer)
            Dim procInst = wfDatabase.GetRunningProcInst(procInstId)
            Dim activeActs = procInst.ActivityInstanceLog.Where(Function(act) act.Status = ActivityStatus.Active)

            For Each act In activeActs
                act.Status = ActivityStatus.Expired
                wfDatabase.UpdateActivityInstance(act)

                wfDatabase.ExpiredWorklistItem(procInstId, act.ActInstId)
            Next

            SetProcessNewStatus(procInst, ProcessStatus.Terminated)
        End Sub

        Public Function GetProcInstsByDataFields(procName As String, key As String, value As String) As List(Of Integer)
            Return wfDatabase.GetProcInstsByDataFields(procName, key, value)
        End Function
#End Region

#Region "Retry Error"
        Public Sub RetryError(erroId As Integer)
            Dim err = wfDatabase.GetErrorLog(erroId)
            Dim procInst = wfDatabase.GetRunningProcInst(err.ProcInstId)
            'procInst.UpdateProcessScheme(GetProcessScheme(procInst.ProcessName))
            'procInst.ActivityInstanceLog.Clear()
            'For Each actInst In procInst.ActivityInstanceLog
            '    actInst.ActivityDefinition = procInst.ProcessScheme.Activities.Find(Function(ac) ac.Name = actInst.ActivityDefinition.Name)
            'Next

            Dim act = procInst.ActivityInstanceLog.SingleOrDefault(Function(a) a.ActInstId = err.ActInstId)

            If act IsNot Nothing AndAlso act.EventInstanceLog IsNot Nothing AndAlso act.EventInstanceLog.Count > 0 Then
                act.Status = ActivityStatus.Running

                Dim evt = act.EventInstanceLog.SingleOrDefault(Function(ev) ev.Id = err.EventInstId)
                If evt IsNot Nothing Then
                    evt.Status = EventStatus.Running
                End If
            End If

            If procInst.Status = ProcessStatus.InError Then
                wfDatabase.RetryError(err.LogId)
                SetProcessNewStatus(procInst, ProcessStatus.Running)
            End If
        End Sub

        Private Sub UpdateAction()

        End Sub

        Public Function GetAllErrorLogs() As List(Of ErrorLog)
            Return wfDatabase.GetAllErrorLogs()
        End Function
#End Region

#Region "Execute Process"

        Private Sub ExecutePostEvents(actInst As ActivityInstance)
            actInst.ClientEventInst.EndDate = DateTime.Now
            wfDatabase.CompleteEvent(actInst.ClientEventInst)

            Dim events = actInst.ActivityDefinition.Events.Where(Function(evt) evt.Order > actInst.ClientEventInst.ActivityEvent.Order)
            If events.Count > 0 Then
                For Each evt In events
                    Dim eventInst = EventInstance.Create(actInst, evt)
                    actInst.EventInstanceLog.Add(eventInst)

                    wfDatabase.CreateEventInstance(eventInst)

                    eventInst.Execute()
                    eventInst.EndDate = DateTime.Now

                    wfDatabase.CompleteEvent(eventInst)
                Next
            End If
        End Sub


#End Region

#Region "Work list"

        Public Function GetWorklist(dest As String) As List(Of WorklistItem)
            Return wfDatabase.GetMyWorklist(dest)
        End Function

        Public Function OpenWorklistItem(procInstId As Integer, actInstId As Integer, destUser As String) As WorklistItem
            Return wfDatabase.OpenWorklistItem(procInstId, actInstId, destUser)
        End Function

        Public Function OpenWorklistItem(item As WorklistItem) As WorklistItem
            Dim procInst = wfDatabase.GetRunningProcInst(item.ProcInstId)
            item.EventInstance = procInst.GetClientInstance(item.ActInstId)

            If item.EventInstance Is Nothing Then
                item.EventInstance = procInst.GetClientInstance(item.ActInstId, item.Destination)
            End If

            Return item
        End Function

        'Public Sub CompleteWorklist(seriesNumber As String)
        '    'CompleteWorklist(PersistenceProvider.GetWorklistItem(seriesNumber))
        'End Sub

        Public Sub CompleteWorklist(procInstId As Integer, actInstId As Integer, destUser As String, datafields As Dictionary(Of String, Object))

            Dim wli = wfDatabase.OpenWorklistItem(procInstId, actInstId, destUser)
            For Each df In datafields
                wli.ProcessInstance.SetDataFieldValue(df.Key, df.Value)
            Next
            CompleteWorklist(wli)
        End Sub

        Public Sub CompleteWorklist(procInstId As Integer, actInstId As Integer, destUser As String, datafields As Dictionary(Of String, Object), actDataFields As Dictionary(Of String, Object))
            Dim wli = wfDatabase.OpenWorklistItem(procInstId, actInstId, destUser)

            For Each df In datafields
                wli.ProcessInstance.SetDataFieldValue(df.Key, df.Value)
            Next

            If actDataFields IsNot Nothing Then
                For Each df In actDataFields
                    wli.ActivityDestinationInstance.SetDataFieldValue(df.Key, df.Value)
                Next

                wfDatabase.SaveActivityDataFields(wli.ActivityDestinationInstance)
            End If

            CompleteWorklist(wli)
        End Sub

        Public Sub CompleteWorklist(wli As WorklistItem)
            wli.Status = WorklistItemStatu.Completed
            wfDatabase.CompleteWorklistItem(wli)

            Dim actInst = wli.ActivityInstance
            actInst.Status = ActivityStatus.Running

            If wli.ActivityDestinationInstance IsNot Nothing Then
                Dim actDestInst = wli.ActivityDestinationInstance
                actDestInst.Status = ActivityStatus.Running
                actDestInst.ClientEventInst.Status = EventStatus.Completed

                wfDatabase.UpdateActivityDestinationInstance(actDestInst)
            Else
                actInst.ClientEventInst.Status = EventStatus.Completed
            End If

            wfDatabase.UpdateActivityInstance(actInst)
            SetProcessNewStatus(wli.ProcessInstance, ProcessStatus.Running)

            'ExecutePostEvents(actInst)
            'ExecuteOutcome(actInst)

            'If actInst.ProcessInstance.Status Is ProcessStatus.Running Then
            '    SetProcessNewStatus(actInst.ProcessInstance, ProcessStatus.Completed)
            'End If
        End Sub
#End Region

#Region "Workflow Scheme"
        Public Function DeployProcess(name As String, displayName As String, description As String, proc As String) As Integer
            Return wfDatabase.SaveScheme(name, displayName, description, proc)
        End Function

        Friend Function GetProcessScheme(processId As Integer) As ProcessDefinition
            Return Builder.GetProcessInstance(processId).ProcessScheme
        End Function

        Public Function GetProcessScheme(procName As String)
            Return Builder.GetProcessScheme(procName)
        End Function

        Friend Function GetAllProcessScheme() As List(Of ProcessDefinition)
            Return Builder.GetAllProcessScheme()
        End Function
#End Region

        Private Sub SetProcessNewStatus(processInstance As ProcessInstance, newStatus As ProcessStatus)
            Dim oldStatus = processInstance.Status
            processInstance.Status = newStatus
            If newStatus = ProcessStatus.Completed Then
                wfDatabase.SaveProcessStatus(processInstance)
            ElseIf newStatus = ProcessStatus.Active Then

                wfDatabase.SaveProcessStatus(processInstance)
            ElseIf newStatus = ProcessStatus.Initialized Then
                wfDatabase.InitialProcessInstance(processInstance)
            ElseIf newStatus = ProcessStatus.Running Then
                wfDatabase.SaveProcessStatus(processInstance)
            ElseIf newStatus = ProcessStatus.Terminated Then
                wfDatabase.SaveProcessStatus(processInstance)
            ElseIf newStatus = ProcessStatus.InError Then
                wfDatabase.SaveProcessStatus(processInstance)
            Else
                Return
            End If
        End Sub

    End Class
End Namespace
