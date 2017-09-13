Imports MyIdealProp.Workflow.Core.Runtime
Namespace MyIdealProp.Workflow.Service

    ' NOTE: You can use the "Rename" command on the context menu to change the class name "ManagementService" in both code and config file together.
    Public Class ManagementService
        Implements IManagementService

        Public Function GetAllErrorlogs() As List(Of MyIdealProp.Workflow.Core.Model.ErrorLog) Implements IManagementService.GetAllErrorlogs
            Dim runTime As WorkflowRuntime = WorkflowRuntime.GetInstance
            Return runTime.GetAllErrorLogs()
        End Function

        Public Sub RetryError(errorId As Integer) Implements IManagementService.RetryError
            Dim runTime As WorkflowRuntime = WorkflowRuntime.GetInstance
            runTime.RetryError(errorId)
        End Sub
    End Class
End Namespace
