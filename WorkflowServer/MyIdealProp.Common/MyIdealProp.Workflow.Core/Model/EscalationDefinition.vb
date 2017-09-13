
Namespace MyIdealProp.Workflow.Core.Model
    <Serializable>
    Public Class EscalationDefinition
        Inherits BaseDefinition

        Public Property DueTime As TimeSpan
        Public Property Action As ActionDefinition
        Public Property Repeat As Integer

        Public Shared Function Create(name As String, dueTime As String, action As ActionDefinition, Repeat As Integer) As EscalationDefinition
            Return New EscalationDefinition() With
                   {
                       .DueTime = TimeSpan.Parse(dueTime),
                       .Action = action,
                       .Repeat = Repeat
                       }
        End Function
    End Class
End Namespace