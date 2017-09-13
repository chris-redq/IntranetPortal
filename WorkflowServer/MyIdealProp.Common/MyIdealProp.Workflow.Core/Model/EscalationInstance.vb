Namespace MyIdealProp.Workflow.Core.Model
    <Serializable>
    Public Class EscalationInstance
        Public Property ProcInstId As Integer
        Public Property ProcessInstance As ProcessInstance
        Public Property ActivityInstance As ActivityInstance
        Public Property Action As ActionDefinition
        Public Property ExecuteTime As DateTime
        Public Property ActionName As String
        Public Property RepeatNo As Integer

        Public Shared Function Create(procInst As ProcessInstance, actInst As ActivityInstance, escalDefinition As EscalationDefinition, executeTime As DateTime) As EscalationInstance
            Dim escalInst = New EscalationInstance With {
                .ProcessInstance = procInst,
                .ActivityInstance = actInst,
                .Action = escalDefinition.Action,
                .ActionName = escalDefinition.Action.Name,
                .ExecuteTime = executeTime
                }

            Return escalInst
        End Function

        Public Enum EscalationStatus
            Active
            Completed
            Expired
        End Enum
    End Class


End Namespace