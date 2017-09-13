Public Class WorklistItem

    Private Sub New()

    End Sub

    Private _id As Integer
    Public ReadOnly Property Id As Integer
        Get
            Return _id
        End Get
    End Property

    Public ReadOnly Property SeriesNumber As String
        Get
            Return String.Format("{0}_{1}", ProcInstId, ActInstId)
        End Get
    End Property

    Private _displayName As String
    Public ReadOnly Property DisplayName As String
        Get
            Return _displayName
        End Get
    End Property


    Private _destUser As String
    Public ReadOnly Property DestinationUser As String
        Get
            Return _destUser
        End Get
    End Property

    Private _originator As String
    Public ReadOnly Property Originator As String
        Get
            Return _originator
        End Get
    End Property

    Private _procInstId As Integer
    Public ReadOnly Property ProcInstId As Integer
        Get
            Return _procInstId
        End Get
    End Property

    Private _procInst As ProcessInstance
    Public Property ProcessInstance As ProcessInstance
        Get
            Return _procInst
        End Get
        Set(value As ProcessInstance)
            _procInst = value
        End Set
    End Property

    Private _actInstId As Integer
    Public ReadOnly Property ActInstId As Integer
        Get
            Return _actInstId
        End Get
    End Property

    Private _actInst As ActivityInstance
    Public ReadOnly Property ActivityInstance As ActivityInstance
        Get
            Return _actInst
        End Get
    End Property

    Private _actDestInst As ActivityDestinationInstance
    Public ReadOnly Property ActivityDestinationInstance As ActivityDestinationInstance
        Get
            Return _actDestInst
        End Get
    End Property

    Private _evtName As String
    Public ReadOnly Property EventName As String
        Get
            Return _evtName
        End Get
    End Property

    Private _status As WorklistItemStatus
    Public ReadOnly Property Status As WorklistItemStatus
        Get
            Return _status
        End Get
    End Property

    Private _procName As String
    Public ReadOnly Property ProcessName As String
        Get
            Return _procName
        End Get
    End Property

    Private _actName As String
    Public ReadOnly Property ActivityName As String
        Get
            Return _actName
        End Get
    End Property

    Private _priority As Integer
    Public ReadOnly Property Priority As ProcessPriority
        Get
            Return CType(_priority, ProcessPriority)
        End Get
    End Property

    Private _processSchemeDisplayName As String
    Public ReadOnly Property ProcSchemeDisplayName As String
        Get
            Return _processSchemeDisplayName
        End Get
    End Property


    Private _startDate As DateTime
    Public ReadOnly Property StartDate As DateTime
        Get
            Return _startDate
        End Get
    End Property

    Private _itemData As String
    Public ReadOnly Property ItemData As String
        Get
            Return _itemData
        End Get
    End Property

    Friend Property Connection As Connection

    Friend Shared Function Create(wli As WorkflowServer.WorklistItem) As WorklistItem
        Return New WorklistItem() With
               {
                   ._actName = wli.ActivityName,
                   ._destUser = wli.Destination,
                   ._evtName = "",
                   ._startDate = wli.CreateDate,
                    ._procName = wli.ProcessName,
                   ._displayName = wli.ProcessDisplayName,
                   ._procInstId = wli.ProcInstId,
                   ._actInstId = wli.ActInstId,
                   ._itemData = wli.ItemData,
                   ._procInst = If(wli.ProcessInstance IsNot Nothing, ProcessInstance.Create(wli.ProcessInstance), Nothing),
                   ._actDestInst = If(wli.ActivityDestinationInstance IsNot Nothing, ActivityDestinationInstance.Create(wli.ActivityDestinationInstance), Nothing),
                   ._priority = wli.Priority,
                   ._processSchemeDisplayName = wli.ProcSchemeDisplayName,
                   ._originator = wli.Originator
                   }
    End Function

    Public Sub Update()

    End Sub

    Public Sub Finish()
        Connection.CompleteWorklistItem(Me)
        Connection.Close()
    End Sub

    Public Sub GotoActivity(activityName As String)
        Connection.GotoActivity(ProcInstId, activityName)
        Connection.Close()
    End Sub
End Class

Public Enum WorklistItemStatus
    Avaiable = 0
    Open = 1
    Allocated = 2
    Completed = 3
End Enum