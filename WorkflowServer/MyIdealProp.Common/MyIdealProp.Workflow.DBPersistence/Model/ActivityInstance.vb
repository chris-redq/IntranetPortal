Partial Public Class ActivityInstance
    Private _eventInstances As List(Of EventInstance)
    Public ReadOnly Property EventInstances As List(Of EventInstance)
        Get
            If _eventInstances Is Nothing Then
                _eventInstances = New List(Of EventInstance)
                _eventInstances = EventInstance.GetActivityEventInstances(Id)
            End If

            Return _eventInstances
        End Get
    End Property

    'Public ReadOnly Property EndDate As DateTime?
    '    Get
    '        If _eventInstances IsNot Nothing AndAlso -_eventInstances.Count > 0 Then
    '            Return _eventInstances.Last.EndDate
    '        End If

    '        Return Nothing
    '    End Get
    'End Property

    Public ReadOnly Property Duration As TimeSpan
        Get
            If EndDate.HasValue Then
                Return EndDate - StartDate
            End If

            Return Nothing
        End Get
    End Property

    Public ReadOnly Property StatusText As String
        Get
            Return CType(Status, ActivityStatus).ToString
        End Get
    End Property

    Public Shared Function GetProcessActivityInstances(procInstId As Integer)
        Using ctx As New DataModelContainer
            Return ctx.ActivityInstances.Where(Function(act) act.ProcInstId = procInstId).ToList
        End Using
    End Function

    Public Enum ActivityStatus
        Running = 0
        Active = 1
        Completed = 2
        Expired = 3
    End Enum

End Class
