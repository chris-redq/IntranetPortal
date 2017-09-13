
Namespace MyIdealProp.Workflow.Core.Model
    Public Class ProcessConfiguration
        Private Shared wfDatabase As New MyIdealProp.Workflow.Core.Data.WorkflowDatabase

        Public Property ProcessName As String
        Public Shared Function Settings(name As String) As String
            Return wfDatabase.GetSettingsValue("Server", name)
        End Function
    End Class
End Namespace