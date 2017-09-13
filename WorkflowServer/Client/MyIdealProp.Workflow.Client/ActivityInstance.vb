Public Class ActivityInstance

    Private _activityName As String
    Public ReadOnly Property ActivityName As DateTime
        Get
            Return _activityName
        End Get
    End Property

    Private _startDate As DateTime
    Public ReadOnly Property StartDate As DateTime
        Get
            Return _startDate
        End Get
    End Property

    Private _procInst As ProcessInstance
    Public ReadOnly Property ProcessInstance As ProcessInstance
        Get
            Return _procInst
        End Get
    End Property
End Class

Public Class ActivityDestinationInstance

    Private _destUser As String
    Public ReadOnly Property DestinationUser As String
        Get
            Return _destUser
        End Get
    End Property

    Private _startDate As DateTime
    Public ReadOnly Property StartDate As DateTime
        Get
            Return _startDate
        End Get
    End Property

    Private _datafields As Dictionary(Of String, Object)
    Public ReadOnly Property DataFields As Dictionary(Of String, Object)
        Get
            Return _datafields
        End Get
    End Property

    Private _activityName As String
    Public ReadOnly Property ActivityName As DateTime
        Get
            Return _activityName
        End Get
    End Property

    Public Shared Function Create(actDestInst As WorkflowServer.ActivityDestinationInstance) As ActivityDestinationInstance
        Dim dfs = New Dictionary(Of String, Object)
        For Each df In actDestInst.DataFields
            dfs.Add(df.Name, df.Value)
        Next

        Return New ActivityDestinationInstance With
               {
                   ._destUser = actDestInst.DestinationUser,
                   ._startDate = actDestInst.StartTime,
                   ._datafields = dfs,
                   ._activityName = actDestInst.ActivityName
                   }
    End Function
End Class