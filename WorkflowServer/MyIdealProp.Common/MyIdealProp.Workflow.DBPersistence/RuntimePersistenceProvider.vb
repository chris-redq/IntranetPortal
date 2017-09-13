Imports MyIdealProp.Workflow.Core.Model
Imports MyIdealProp.Workflow.Core
Imports MyIdealProp.Workflow.Core.Persistence

Public Class RuntimePersistenceProvider
    Implements IRuntimePersistence


    Public Function LoadNextTimer() As TimerInstance Implements IRuntimePersistence.LoadNextTimer
        'Dim timer = RuntimeTimer.GetNextTimer()

        'If timer IsNot Nothing Then
        '    Return New TimerInstance With
        '           {
        '               .ProcInstId = timer.ProcInstId,
        '               .ActInstId = timer.ActInstId,
        '               .ActionName = timer.ActionName,
        '               .EscalationId = timer.EscalationId,
        '               .DueTime = timer.DueTime,
        '               .TimerId = timer.TimerId
        '               }
        'End If

        'Return Nothing
    End Function

    Public Function LoadTimer(runtimeId As Integer) As TimerInstance Implements IRuntimePersistence.LoadTimer
        Throw New Exception("Not Implement")
    End Function

    Public Sub RemoveTimer(timerId As Integer) Implements IRuntimePersistence.RemoveTimer
        RuntimeTimer.RemoveTimer(timerId)
    End Sub
End Class
