
Namespace MyIdealProp.Workflow.Core.Model
    <Serializable>
    Public Class SuccessRuleDefinition
        Inherits BaseDefinition

        Public Property CodeRule As ActionDefinition

        Public Shared Function Create(name As String, action As ActionDefinition) As SuccessRuleDefinition
            Return New SuccessRuleDefinition With {
                .Name = name,
                .CodeRule = action
                }
        End Function
    End Class
End Namespace