Imports System.Runtime.Serialization

Namespace MyIdealProp.Workflow.Core.Model

    <DataContract()>
    Public Class WorklistItem
        Private _id As Integer
        <DataMember>
        Public Property Id As Integer
            Get
                Return _id
            End Get
            Friend Set(value As Integer)
                _id = value
            End Set
        End Property

        Private _originator As String

        <DataMember>
        Public Property Originator As String
            Get
                Return _originator
            End Get
            Friend Set(value As String)
                _originator = value
            End Set
        End Property

        Private _procInst As ProcessInstance
        <DataMember>
        Public Property ProcessInstance As ProcessInstance
            Get
                If EventInstance IsNot Nothing Then
                    _procInst = EventInstance.ProcessInstance
                End If

                Return _procInst
            End Get
            Friend Set(value As ProcessInstance)
                _procInst = value
            End Set
        End Property

        Private _actDestInst As ActivityDestinationInstance
        <DataMember>
        Public Property ActivityDestinationInstance As ActivityDestinationInstance
            Get
                If _actDestInst Is Nothing AndAlso ActivityInstance IsNot Nothing AndAlso ActivityInstance.ActivityDestinationInstances IsNot Nothing Then
                    _actDestInst = ActivityInstance.ActivityDestinationInstances.SingleOrDefault(Function(act) act.DestinationUser.Equals(Destination, StringComparison.OrdinalIgnoreCase))
                End If

                Return _actDestInst
            End Get
            Friend Set(value As ActivityDestinationInstance)
                _actDestInst = value
            End Set
        End Property

        Private _actInst As ActivityInstance
        <DataMember>
        Public Property ActivityInstance As ActivityInstance
            Get
                If _actInst Is Nothing AndAlso EventInstance IsNot Nothing Then
                    _actInst = EventInstance.ActivityInstance
                End If

                Return _actInst
            End Get
            Friend Set(value As ActivityInstance)
                _actInst = value
            End Set
        End Property

        Private _processName As String

        <DataMember>
        Public Property ProcessName As String
            Get
                If String.IsNullOrEmpty(_processName) Then
                    If ProcessInstance IsNot Nothing Then
                        _processName = ProcessInstance.ProcessScheme.Name
                    End If
                End If

                Return _processName
            End Get
            Friend Set(value As String)
                _processName = value
            End Set
        End Property

        Private _actName As String
        <DataMember>
        Public Property ActivityName As String
            Get
                If ActivityInstance IsNot Nothing Then
                    Return ActivityInstance.ActivityDefinition.Name
                End If

                Return _actName
            End Get
            Friend Set(value As String)
                _actName = value
            End Set
        End Property

        Public Property EventInstance As EventInstance
        <DataMember>
        Public Property Destination As String
        <DataMember>
        Public Property DestinationSlot As DestinationSlot
        <DataMember>
        Public Property Status As WorklistItemStatu
        <DataMember>
        Public Property ProcInstId As Integer
        <DataMember>
        Public Property ActInstId As Integer
        <DataMember>
        Public Property EventInstId As Integer
        <DataMember>
        Public Property CreateDate As DateTime
        <DataMember>
        Public Property ItemData As String
        <DataMember>
        Public Property Priority As ProcessInstance.ProcessPriority
        <DataMember>
        Public Property ProcSchemeDisplayName As String

        Private _displayName As String
        <DataMember>
        Public Property ProcessDisplayName As String
            Get
                If ProcessInstance IsNot Nothing Then
                    Return ProcessInstance.DisplayName
                End If

                Return _displayName
            End Get
            Friend Set(value As String)
                _displayName = value
            End Set
        End Property

        Public Sub Complete()
            Status = WorklistItemStatu.Completed
        End Sub

        Public Shared Function Create(eventInst As EventInstance, dest As String) As WorklistItem
            Return New WorklistItem() With
            {
                .EventInstance = eventInst,
                .Destination = dest,
                .Status = WorklistItemStatu.Avaiable,
                .ItemData = eventInst.ItemData
                }
        End Function

        Public Shared Function Create(procInstId As Integer, actInstId As Integer, eventInstId As Integer, ItemData As String, actName As String, destUser As String, createDate As DateTime, status As Integer, displayName As String, procName As String, originator As String, priority As Integer, procSchemeDisplayName As String)
            Return New WorklistItem With {
                .ProcInstId = procInstId,
                .ActInstId = actInstId,
                .EventInstId = eventInstId,
                .ItemData = ItemData,
                .ProcessDisplayName = displayName,
                .ProcessName = procName,
                .ActivityName = actName,
                .Destination = destUser,
                .CreateDate = createDate,
                .Status = CType(status, WorklistItemStatu),
                ._originator = originator,
                .Priority = CType(priority, ProcessInstance.ProcessPriority),
                .ProcSchemeDisplayName = procSchemeDisplayName
                }
        End Function

        Public Shared Function Create(procInstId As Integer, actInstId As Integer, eventInstId As Integer, itemData As String, actName As String, destUser As String, createDate As DateTime, status As Integer)
            Return New WorklistItem With {
                .ProcInstId = procInstId,
                .ActInstId = actInstId,
                .EventInstId = eventInstId,
                .ItemData = itemData,
                .ActivityName = actName,
                .Destination = destUser,
                .CreateDate = createDate,
                .Status = CType(status, WorklistItemStatu)
                }
        End Function

        Public Sub Open()

        End Sub
    End Class

    Public Enum WorklistItemStatu
        Avaiable = 0
        Open = 1
        Allocated = 2
        Completed = 3
        Expired = 4
    End Enum
End Namespace