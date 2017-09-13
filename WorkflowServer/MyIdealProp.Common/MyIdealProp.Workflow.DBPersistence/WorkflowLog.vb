Public Class WorkflowLog
    Public Shared Function GetAllProcessInstance() As List(Of ProcessInstance)
        Return ProcessInstance.GetAllProcessInstances
    End Function
End Class
