
Partial Public Class EventInstance
    'Public Shared Sub SaveEventInstance(eventInst As Model.EventInstance)
    '    Using ctx As New DataModelContainer
    '        Dim evt As New EventInstance
    '        evt.ActInstId = eventInst.ActivityInstance.ActInstId
    '        evt.Name = eventInst.EventName
    '        evt.Type = eventInst.IsClient
    '        evt.StartDate = eventInst.StartDate

    '        ctx.EventInstances.Add(evt)
    '        ctx.SaveChanges()

    '        eventInst.Id = evt.Id
    '    End Using
    'End Sub

    'Public Shared Sub CompletEvent(eventInst As Model.EventInstance)
    '    Using ctx As New DataModelContainer
    '        Dim evt = ctx.EventInstances.Find(eventInst.Id)
    '        evt.EndDate = DateTime.Now
    '        ctx.SaveChanges()
    '    End Using
    'End Sub

    Public Shared Function GetActivityEventInstances(actInstId As Integer) As List(Of EventInstance)
        Using ctx As New DataModelContainer
            Return ctx.EventInstances.Where(Function(evt) evt.ActInstId = actInstId).ToList
        End Using
    End Function
End Class
