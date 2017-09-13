Public Class ProcessInstance
    Private _id As Integer
    Public Property Id As Integer
        Get
            Return _id
        End Get
        Friend Set(value As Integer)
            _id = value
        End Set
    End Property

    Public Property DisplayName As String
    Private _originator As String
    Public ReadOnly Property Originator As String
        Get
            Return _originator
        End Get
    End Property

    Public Property Priority As ProcessPriority

    Public Property Description As String

    Private _startDate As DateTime
    Public ReadOnly Property StateDate As DateTime
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

    Private _processName As String
    Public ReadOnly Property ProcessName As String
        Get
            Return _processName
        End Get
    End Property

    Private Sub New()

    End Sub

    Friend Shared Function Create(procInst As WorkflowServer.ProcessInstance) As ProcessInstance
        Dim dfs = New Dictionary(Of String, Object)
        For Each df In procInst.DataFields
            dfs.Add(df.Name, df.Value)
        Next

        Return New ProcessInstance With {
            ._processName = procInst.ProcessName,
            ._originator = procInst.Originator,
            ._startDate = procInst.StartDate,
            .Description = procInst.Description,
            .DisplayName = procInst.DisplayName,
            ._datafields = dfs,
            .WFProcess = procInst,
            .Priority = CType(procInst.Priority, ProcessPriority)
            }
    End Function

    Friend Shared Function Create(procName As String) As ProcessInstance
        Return New ProcessInstance With
               {
                   ._processName = procName,
                   ._datafields = New Dictionary(Of String, Object)
                   }
    End Function

    Private _wfProcess As WorkflowServer.ProcessInstance
    Friend Property WFProcess As WorkflowServer.ProcessInstance
        Get
            _wfProcess.DisplayName = DisplayName

            For Each df In DataFields
                _wfProcess.DataFields.Single(Function(d) d.Name = df.Key).Value = df.Value
            Next

            Return _wfProcess
        End Get
        Set(value As WorkflowServer.ProcessInstance)
            _wfProcess = value
        End Set
    End Property
End Class

Public Enum ProcessPriority
    Normal = 0
    Important = 1
    Urgent = 2
End Enum
