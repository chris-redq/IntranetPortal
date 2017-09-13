Imports MyIdealProp.Workflow.Core
Imports MyIdealProp.Workflow.Core.Runtime
Imports MyIdealProp.Workflow.Core.Model
Imports MyIdealProp.Workflow.DBPersistence

Public Class WorkflowServer
    Private Shared _wfRunTime As WorkflowRuntime
    Public Shared Function GetInstance() As WorkflowRuntime
        If _wfRunTime Is Nothing Then
            _wfRunTime = New Runtime.WorkflowRuntime(Guid.NewGuid)
            _wfRunTime.Start()
        End If

        Return _wfRunTime
    End Function
End Class
