Imports MyIdealProp.Workflow.Core.Persistence
Imports MyIdealProp.Workflow.Core.Model

Public Class DBPersistenceProvider
    Implements IPersistenceProvider


    Public Sub AddWorklistItem(eventInst As Core.Model.EventInstance, dest As String) Implements IPersistenceProvider.AddWorklistItem
        DBPersistence.Worklist.AddWorklistItem(eventInst.ProcessInstance.Id, eventInst.ActivityInstance.ActInstId, 0, eventInst.ActivityName, dest)
    End Sub

    Public Sub BindProcessToNewScheme(processInstance As Core.Model.ProcessInstance) Implements IPersistenceProvider.BindProcessToNewScheme

    End Sub

    Public Sub BindProcessToNewScheme(processInstance As Core.Model.ProcessInstance, resetIsDeterminingParametersChanged As Boolean) Implements IPersistenceProvider.BindProcessToNewScheme

    End Sub

    Public Sub FillPersistedProcessParameters(processInstance As Core.Model.ProcessInstance) Implements IPersistenceProvider.FillPersistedProcessParameters

    End Sub

    Public Sub FillProcessParameters(processInstance As Core.Model.ProcessInstance) Implements IPersistenceProvider.FillProcessParameters

    End Sub

    Public Sub FillSystemProcessParameters(processInstance As Core.Model.ProcessInstance) Implements IPersistenceProvider.FillSystemProcessParameters

    End Sub

    Public Function GetInstanceStatus(processId As Guid) As ProcessStatus Implements IPersistenceProvider.GetInstanceStatus

    End Function

    Public Function GetProcessInstanceLog() As List(Of Core.Model.ProcessInstance) Implements IPersistenceProvider.GetProcessInstanceLog

    End Function

    Public Function GetWorklist(dest As Object) As List(Of Core.Model.WorklistItem) Implements IPersistenceProvider.GetWorklist
        Return DBPersistence.Worklist.GetUserWorklist(dest.ToString).Select(Function(wl) GetWorklistItem(wl.ProcInstId, wl.ActInstId)).ToList()
    End Function

    Public Function GetWorklistItem(sn As String) As Core.Model.WorklistItem Implements IPersistenceProvider.GetWorklistItem
        Dim procInstId = sn.Split("-")(0)
        Dim actInstId = sn.Split("-")(1)

        Dim wl = DBPersistence.Worklist.GetWorklistItem(procInstId, actInstId)
        Dim item As New WorklistItem
        item.Destination = wl.DestinationUser
        Dim procInst = DBPersistence.ProcessInstance.GetRuntimeProcess(procInstId)
        item.EventInstance = procInst.ActivityInstanceLog.Single(Function(act) act.ActInstId = actInstId).ClientEventInst
        item.ActInstId = actInstId
        item.ProcInstId = procInstId
        Return item
    End Function

    Private Function GetWorklistItem(procInstId As Integer, actInstId As Integer) As Core.Model.WorklistItem
        Dim wl = DBPersistence.Worklist.GetWorklistItem(procInstId, actInstId)
        Dim item As New WorklistItem
        item.Destination = wl.DestinationUser

        Dim procInst = DBPersistence.ProcessInstance.GetRuntimeProcess(procInstId)
        item.EventInstance = procInst.ActivityInstanceLog.Single(Function(act) act.ActInstId = actInstId).ClientEventInst
        item.ActInstId = actInstId
        item.ProcInstId = procInstId
        Return item
    End Function

    Public Sub InitializeProcess(processInstance As Core.Model.ProcessInstance) Implements IPersistenceProvider.InitializeProcess
        'DBPersistence.ProcessInstance.SaveProcessInstance(processInstance)
    End Sub

    Public Function IsProcessExists(processId As Guid) As Boolean Implements IPersistenceProvider.IsProcessExists

    End Function

    Public Sub ResetWorkflowRunning() Implements IPersistenceProvider.ResetWorkflowRunning

    End Sub

    Public Sub SavePersistenceParameters(processInstance As Core.Model.ProcessInstance) Implements IPersistenceProvider.SavePersistenceParameters
        DBPersistence.ProcessInstance.SaveProcessRuntime(processInstance)
    End Sub

    Public Sub SetWorkflowFinalized(processInstance As Core.Model.ProcessInstance) Implements IPersistenceProvider.SetWorkflowFinalized
        processInstance.Status = ProcessStatus.Completed
        DBPersistence.ProcessInstance.RemoveProcessRuntime(processInstance.Id)
    End Sub

    Public Sub SetWorkflowIdled(processInstance As Core.Model.ProcessInstance) Implements IPersistenceProvider.SetWorkflowIdled
        processInstance.Status = ProcessStatus.Active
        DBPersistence.ProcessInstance.SaveProcessRuntime(processInstance)
    End Sub

    Public Sub SetWorkflowIniialized(processInstance As Core.Model.ProcessInstance) Implements IPersistenceProvider.SetWorkflowIniialized
        processInstance.Status = ProcessStatus.Initialized
    End Sub

    Public Sub SetWorkflowRunning(processInstance As Core.Model.ProcessInstance) Implements IPersistenceProvider.SetWorkflowRunning
        processInstance.Status = ProcessStatus.Running
    End Sub

    Public Sub SetWorkflowTerminated(processInstance As Core.Model.ProcessInstance, level As ErrorLevel, errorMessage As String) Implements IPersistenceProvider.SetWorkflowTerminated
        processInstance.Status = ProcessStatus.Terminated
        DBPersistence.ProcessInstance.SaveProcessRuntime(processInstance)
    End Sub

    Public Sub UpdatePersistenceState(processInstance As Core.Model.ProcessInstance, transition As Core.Model.LineDefinition) Implements IPersistenceProvider.UpdatePersistenceState

    End Sub

    Public Sub FinalizedActivity(actInst As Core.Model.ActivityInstance) Implements IPersistenceProvider.FinalizedActivity

    End Sub

    Public Sub FinalizedEvent(eventInst As Core.Model.EventInstance) Implements IPersistenceProvider.FinalizedEvent
        DBPersistence.EventInstance.CompletEvent(eventInst)
    End Sub

    Public Sub InitializeActivity(actInst As Core.Model.ActivityInstance) Implements IPersistenceProvider.InitializeActivity
        DBPersistence.ActivityInstance.SaveActivityInstance(actInst)

    End Sub

    Public Sub InitializeEvent(eventInst As Core.Model.EventInstance) Implements IPersistenceProvider.InitializeEvent
        DBPersistence.EventInstance.SaveEventInstance(eventInst)
    End Sub

    Public Sub SaveLineInstance(linInst As Core.Model.LineInstance) Implements IPersistenceProvider.SaveLineInstance
        DBPersistence.LineInstance.SaveLineInstance(linInst)
    End Sub

    Public Sub CompleteWorklist(wli As WorklistItem) Implements IPersistenceProvider.CompleteWorklist
        DBPersistence.Worklist.CompleteWorklistItem(wli.ProcInstId, wli.ActInstId, wli.Destination)
    End Sub

    Public Function GetRuntimeProcessInstance(procInstId As Integer) As Core.Model.ProcessInstance Implements IPersistenceProvider.GetRuntimeProcessInstance
        Return DBPersistence.ProcessInstance.GetRuntimeProcess(procInstId)
    End Function
End Class
