Imports System.Runtime.Serialization.Formatters.Binary
Imports System.IO

Partial Public Class ProcessInstance
    Private _activityInstances As List(Of ActivityInstance)
    Public ReadOnly Property ActivityInstances As List(Of ActivityInstance)
        Get
            If _activityInstances Is Nothing Then
                _activityInstances = New List(Of ActivityInstance)
                _activityInstances = ActivityInstance.GetProcessActivityInstances(Id)
            End If

            Return _activityInstances
        End Get
    End Property

    Public Function GetDataFieldValue(dfName As String) As Object
        Using ctx As New DataModelContainer
            Dim dfield = ctx.DataFields.SingleOrDefault(Function(df) df.ProcInstId = Id And df.FieldName = dfName)

            If dfield IsNot Nothing Then
                Return dfield.FieldValue
            End If
        End Using
        Return Nothing
    End Function

    Private _procName As String
    Public Property ProcessName As String
        Get
            If String.IsNullOrEmpty(_procName) Then
                _procName = ProcessScheme.GetProcessName(ProcessId)
            End If

            Return _procName
        End Get
        Friend Set(value As String)
            _procName = value
        End Set
    End Property


    Private _procDisplayName As String
    Public Property ProcessSchemeDisplayName As String
        Get
            If String.IsNullOrEmpty(_procDisplayName) Then
                _procDisplayName = ProcessScheme.GetProcessDisplayName(ProcessId)
            End If

            Return _procDisplayName
        End Get
        Friend Set(value As String)
            _procDisplayName = value
        End Set
    End Property

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
            If Status.HasValue Then
                Return CType(Status, ProcessStatus).ToString
            End If

            Return ""
        End Get
    End Property

    Public Shared Function LoadProcInstById(procInstId As Integer) As ProcessInstance
        Using ctx As New DataModelContainer
            Dim procInst = ctx.ProcessInstances.Find(procInstId)
            Return procInst
        End Using
    End Function

    Public Shared Function GetMyApplication(originator As String) As List(Of ProcessInstance)
        Using ctx As New DataModelContainer
            Return ctx.ProcessInstances.Where(Function(pi) pi.Originator = originator And pi.Archived <> True).ToList
        End Using
    End Function

    Public Shared Function GetMyProcessed(userName As String) As List(Of ProcessInstance)
        Using ctx As New DataModelContainer
            Dim procIsnts = (From pi In ctx.ProcessInstances
                            Join act In ctx.ActivityInstances On pi.Id Equals act.ProcInstId
                            Where act.DestinationUser.Contains(userName)
                            Select pi).Distinct.ToList

            Return procIsnts
        End Using
    End Function

    Public Shared Sub Archive(procInstId As Integer)
        Using ctx As New DataModelContainer
            Dim procInst = ctx.ProcessInstances.Find(procInstId)
            If procInst IsNot Nothing Then
                procInst.Archived = True
                ctx.SaveChanges()
            End If
        End Using
    End Sub

    Public Shared Sub SaveProcessStatus(procInstId As Integer, status As Integer)
        Using ctx As New DataModelContainer
            Dim procInst = ctx.ProcessInstances.Find(procInstId)
            procInst.Status = status
            ctx.SaveChanges()
        End Using
    End Sub

    Public Shared Function GetAllProcessInstances() As List(Of ProcessInstance)
        Using ctx As New DataModelContainer
            Dim result = (From pInst In ctx.ProcessInstances
                         Join pScheme In ctx.ProcessSchemes On pInst.ProcessId Equals pScheme.ProcessId
                         Select pInst, pScheme.Name, pSchemeDisplayName = pScheme.DisplayName)
            Dim procInsts As New List(Of ProcessInstance)
            For Each item In result
                Dim tmpInst = item.pInst
                tmpInst.ProcessSchemeDisplayName = item.pSchemeDisplayName
                tmpInst.ProcessName = item.Name

                procInsts.Add(tmpInst)
            Next

            Return procInsts
        End Using
    End Function

    Public Shared Sub RemoveProcessRuntime(procInstId As Integer)
        'Using ctx As New DataModelContainer
        '    Dim proc = ctx.ProcessRuntimes.Find(procInstId)
        '    ctx.ProcessRuntimes.Remove(proc)
        '    ctx.SaveChanges()
        'End Using
    End Sub

    Public Shared Function GetProcInstStatistic() As List(Of Object)
        Using ctx As New DataModelContainer
            Dim result = (From pInst In ctx.ProcessInstances
                          Join pScheme In ctx.ProcessSchemes On pInst.ProcessId Equals pScheme.ProcessId
                         Group pInst By pScheme.DisplayName, pInst.Status Into g =
                         Group Let grp = New With
                                {.ProcessName = DisplayName,
                                 .Status = Status,
                                 .Amount = g.Count
                                }
                                Select grp
                                ).ToList

            Dim wlStatis As New List(Of Object)
            For Each proc In result.Select(Function(r) r.ProcessName).Distinct
                Dim items = result.Where(Function(r) r.ProcessName = proc).ToDictionary(Function(r) r.Status, Function(r) r.Amount)
                wlStatis.Add(New With {
                             .ProcessName = proc,
                             .Active = If(items.Keys.Contains(2), items(2), 0),
                             .Completed = If(items.Keys.Contains(3), items(3), 0),
                             .Terminated = If(items.Keys.Contains(4), items(4), 0),
                             .InError = If(items.Keys.Contains(5), items(5), 0),
                             .Total = items.Select(Function(u) u.Value).Sum()})
            Next

            Return wlStatis
        End Using
    End Function


    Enum ProcessStatus
        Initialized = 0
        Running = 1
        Active = 2
        Completed = 3
        Terminated = 4
        InError = 5
    End Enum
End Class
