Imports System.Runtime.Serialization

Namespace MyIdealProp.Workflow.Core.Model
    <Serializable()>
      <DataContract>
    Public Class ActivityInstance
        Public Property ActInstId As Integer
        Public Property ProcessInstance As ProcessInstance
        Public Property ActivityDefinition As ActivityDefinition
        Public Property DestinationUsers As New List(Of String)
        <DataMember>
        Public Property StartDate As DateTime
        Public Property IsClient As Boolean
        Public Property ClientEventInst As EventInstance
        Public Property EventInstanceLog As List(Of EventInstance)
        Public Property EscalationInstances As List(Of EscalationInstance)
        Public Property ActivityDestinationInstances As List(Of ActivityDestinationInstance)
        <DataMember>
        Public Property Status As ActivityStatus
        Public Property SuccessRuleResult As Boolean = True

        Public ReadOnly Property EndDate As DateTime?
            Get
                If EventInstanceLog.Count > 0 Then
                    Return EventInstanceLog.Last.EndDate
                End If

                Return Nothing
            End Get
        End Property

        Private _actName As String
        <DataMember>
        Public Property ActivityName As String
            Get
                If String.IsNullOrEmpty(_actName) Then
                    _actName = ActivityDefinition.Name
                End If
                Return _actName
            End Get
            Friend Set(value As String)
                _actName = value
            End Set
        End Property

        Public Delegate Sub AddClientWorklistItem(eventInst As EventInstance, destination As String)
        Public Event OnAddClientWorklistItem As AddClientWorklistItem

        Public Shared Function Create(procInst As ProcessInstance, activityDefinition As ActivityDefinition) As ActivityInstance
            Dim actInst = New ActivityInstance With {
                .ProcessInstance = procInst,
                .ActivityDefinition = activityDefinition,
                .StartDate = DateTime.Now,
                .EventInstanceLog = New List(Of EventInstance),
                .EscalationInstances = New List(Of EscalationInstance),
                .ActivityDestinationInstances = New List(Of ActivityDestinationInstance),
                .Status = ActivityStatus.Running
                }

            'If actInst.ActivityDefinition.Destinations IsNot Nothing AndAlso actInst.ActivityDefinition.Destinations.Count > 0 Then
            '    actInst.DestinationUser = actInst.ActivityDefinition.Destinations(0).DestinationName
            'Else
            '    actInst.DestinationUser = "WFServer"
            'End If

            actInst.ExecuteDestinationRule()
            actInst.ExecuteEscalation()

            procInst.AddActivityInstance(actInst)
            Return actInst
        End Function

        Public Sub ExecuteDestinationRule()
            If Me.ActivityDefinition.Destinations IsNot Nothing AndAlso Me.ActivityDefinition.Destinations.Count > 0 Then
                For Each dest In Me.ActivityDefinition.Destinations
                    If dest.CodeRule IsNot Nothing Then
                        Action.Executor.ExecuteAction(dest.CodeRule, Me)
                    Else
                        If Not Me.DestinationUsers.Any(Function(du) du.Equals(dest.DestinationName, StringComparison.OrdinalIgnoreCase)) Then
                            Me.DestinationUsers.Add(dest.DestinationName)
                        End If
                    End If
                Next

                If DestinationUsers.Count > 0 Then
                    Me.ActivityDestinationInstances = New List(Of ActivityDestinationInstance)
                    For Each user In DestinationUsers
                        Me.ActivityDestinationInstances.Add(ActivityDestinationInstance.Create(Me, user))
                    Next
                Else
                    Me.DestinationUsers.Add("WFServer")
                End If
            End If
        End Sub

        Public Sub ExecuteEscalation()
            If Me.ActivityDefinition.Escalations IsNot Nothing AndAlso Me.ActivityDefinition.Escalations.Count > 0 Then
                For Each escal In Me.ActivityDefinition.Escalations
                    Dim dueTime = DateTime.Now.AddMinutes(escal.DueTime.TotalMinutes)
                    Me.EscalationInstances.Add(EscalationInstance.Create(ProcessInstance, Me, escal, dueTime))
                Next
            End If
        End Sub

        Public Sub ExecuteSuccessRule()
            If Me.ActivityDefinition.SuccessRule IsNot Nothing Then
                Action.Executor.ExecuteAction(Me.ActivityDefinition.SuccessRule.CodeRule, Me)
            End If
        End Sub
    End Class

    Public Enum ActivityStatus
        Running = 0
        Active = 1
        Completed = 2
        Expired = 3
    End Enum
End Namespace
