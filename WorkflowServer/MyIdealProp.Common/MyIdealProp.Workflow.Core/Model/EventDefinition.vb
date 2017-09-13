Imports MyIdealProp.Workflow.Core.Action

Namespace MyIdealProp.Workflow.Core.Model
    <Serializable>
    Public Class EventDefinition
        Inherits BaseDefinition

        Public Property EventAction As ActionDefinition

        Public Overridable Sub Execute(eventInst As EventInstance)
            Executor.ExecuteAction(EventAction, eventInst)
        End Sub

        Public Shared Function Create(name As String, actDefinition As ActionDefinition, type As String) As EventDefinition
            Dim retEvent As EventDefinition
            If type = "ClientEvent" Then
                retEvent = New ClientEvent
            Else
                retEvent = New EventDefinition
            End If

            retEvent.Name = name
            retEvent.EventAction = actDefinition

            Return retEvent
        End Function
    End Class

    <Serializable>
    Public Class ClientEvent
        Inherits EventDefinition

        Public Property ClientData As String

        'Public Overrides Sub Execute(eventInst As EventInstance)
        '    Dim user = eventInst.ActivityInstance.DestinationUser

        'End Sub
    End Class

    <Serializable>
    Public Class ActivityEvent
        Public Property Order As Integer
        Public Property EventDefinition As EventDefinition

        Public Shared Function Create(name As String, action As ActionDefinition, order As Integer, eventType As String)
            Return New ActivityEvent With
                   {
                       .Order = order,
                       .EventDefinition = EventDefinition.Create(name, action, eventType)
                       }
        End Function
    End Class
End Namespace


