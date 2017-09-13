Partial Public Class RuntimeTimer
    Public Shared Function GetNextTimer() As RuntimeTimer
        'Using ctx As New DataModelContainer
        '    Dim timer = ctx.RuntimeTimers.OrderBy(Function(rt) rt.DueTime).FirstOrDefault
        '    Return timer
        'End Using
    End Function

    Public Shared Sub RemoveTimer(timerId As Integer)
        'Using ctx As New DataModelContainer
        '    Dim timer = ctx.RuntimeTimers.Find(timerId)
        '    ctx.RuntimeTimers.Remove(timer)

        '    Dim escal = ctx.EscalationInstances.Find(timer.EscalationId)
        '    escal.ExecuteTime = DateTime.Now
        '    escal.Status = "Complete"

        '    ctx.SaveChanges()
        'End Using
    End Sub
End Class
