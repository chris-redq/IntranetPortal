Namespace MyIdealProp.Workflow.Core.Model
    <Serializable>
    Public Class EventInstance
        Public ReadOnly Property ProcessInstance As ProcessInstance
            Get
                Return ActivityInstance.ProcessInstance
            End Get
        End Property

        Public ReadOnly Property ActivityName As String
            Get
                Return ActivityInstance.ActivityDefinition.Name
            End Get
        End Property

        Public ReadOnly Property EventName As String
            Get
                Return ActivityEvent.EventDefinition.Name
            End Get
        End Property

        Public Property Id As Integer
        Public Property ActivityInstance As ActivityInstance
        Public Property ActivityDestnationInstance As ActivityDestinationInstance
        Public Property ActivityEvent As ActivityEvent
        Public Property StartDate As DateTime
        Public Property EndDate As DateTime?
        Public Property ItemData As String
        Public Property Status As EventStatus


        Public ReadOnly Property IsClient As Boolean
            Get
                Return ActivityEvent.EventDefinition.GetType() Is GetType(ClientEvent)
            End Get
        End Property

        Public ReadOnly Property ClientSeriesNumber As String
            Get
                If IsClient Then
                    Return String.Format("{0}_{1}", ProcessInstance.Id, ActivityInstance.ActInstId)
                End If

                Return ""
            End Get
        End Property

        Public Shared Function Create(actInst As ActivityInstance, eventDefi As ActivityEvent) As EventInstance

            Return New EventInstance() With
                   {
                       .ActivityInstance = actInst,
                       .ActivityEvent = eventDefi,
                       .StartDate = DateTime.Now,
                       .Status = EventStatus.Running
                       }
        End Function

        Public Shared Function Create(actDestInst As ActivityDestinationInstance, eventDefi As ActivityEvent) As EventInstance

            Return New EventInstance() With
                   {
                       .ActivityInstance = actDestInst.ActivityInstance,
                       .ActivityDestnationInstance = actDestInst,
                       .ActivityEvent = eventDefi,
                       .StartDate = DateTime.Now,
                       .Status = EventStatus.Running
                       }
        End Function

        Public Sub Execute()
            ActivityEvent.EventDefinition.Execute(Me)
        End Sub
    End Class

    Public Enum EventStatus
        Running = 0
        Active = 1
        Completed = 2
    End Enum
End Namespace