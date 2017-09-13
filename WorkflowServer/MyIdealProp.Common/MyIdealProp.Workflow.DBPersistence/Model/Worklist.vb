Partial Public Class Worklist

    Public ReadOnly Property ProcessInstance As ProcessInstance
        Get
            Using ctx As New DataModelContainer
                Return ctx.ProcessInstances.Find(ProcInstId)
            End Using
        End Get
    End Property

    Private _processName As String
    Public Property ProcessName As String
        Get
            If String.IsNullOrEmpty(_processName) Then
                If ProcessInstance IsNot Nothing Then
                    _processName = ProcessInstance.ProcessSchemeDisplayName
                Else
                    _processName = ProcInstId
                End If
            End If

            Return _processName
        End Get
        Friend Set(value As String)
            _processName = value
        End Set
    End Property

    Private _displayName As String
    Public Property DisplayName As String
        Get
            If String.IsNullOrEmpty(_displayName) Then
                If ProcessInstance IsNot Nothing Then
                    _displayName = ProcessInstance.DisplayName
                Else
                    _displayName = ProcInstId
                End If
            End If

            Return _displayName
        End Get
        Friend Set(value As String)
            _displayName = value
        End Set
    End Property

    Private _actName As String
    Public Property ActivityName As String
        Get
            If String.IsNullOrEmpty(_actName) Then
                _actName = ActivityInstance.ActivityName
            End If

            Return _actName
        End Get
        Friend Set(value As String)
            _actName = value
        End Set
    End Property

    Public ReadOnly Property Duration As TimeSpan
        Get
            If EndDate.HasValue Then
                Return EndDate - CreateDate
            End If

            Return Nothing
        End Get
    End Property

    Public ReadOnly Property ActivityInstance As ActivityInstance
        Get
            Using ctx As New DataModelContainer
                Return ctx.ActivityInstances.Find(ActInstId)
            End Using
        End Get
    End Property

    Public Shared Sub AddWorklistItem(procInstId As Integer, actInstId As Integer, eventInstId As Integer, actName As String, destUser As String)
        Using ctx As New DataModelContainer
            Dim item As New Worklist
            item.ProcInstId = procInstId
            item.ActInstId = actInstId
            item.EventInstId = eventInstId
            item.DestinationUser = destUser
            item.ActvityName = actName
            item.Status = 0

            ctx.Worklists.Add(item)
            ctx.SaveChanges()
        End Using
    End Sub

    Public Shared Function GetAllPendingWorklist() As List(Of Worklist)
        Using ctx As New DataModelContainer
            Return ctx.Worklists.ToList
        End Using
    End Function

    Public Shared Function GetWorklistStatistic() As List(Of Object)
        Using ctx As New DataModelContainer
            Dim result = (From wl In ctx.Worklists
                         Group wl By wl.DestinationUser, wl.Status Into g =
                         Group Let grp = New With
                                {.User = DestinationUser,
                                 .Status = Status,
                                 .Amount = g.Count
                                    }
                                Select grp
                                ).ToList

            Dim wlStatis As New List(Of Object)
            For Each user In result.Select(Function(r) r.User.ToLower).Distinct
                Dim userItems = result.Where(Function(r) r.User.ToLower = user).ToDictionary(Function(r) r.Status, Function(r) r.Amount)
                wlStatis.Add(New With {
                             .Name = StrConv(user, VbStrConv.ProperCase),
                             .Active = If(userItems.Keys.Contains(0), userItems(0), 0),
                             .Open = If(userItems.Keys.Contains(1), userItems(1), 0),
                             .Completed = If(userItems.Keys.Contains(3), userItems(3), 0),
                             .Expired = If(userItems.Keys.Contains(4), userItems(4), 0),
                             .Total = userItems.Select(Function(u) u.Value).Sum()})
            Next

            Return wlStatis
        End Using
    End Function

    Public Shared Function GetUserWorklistReport(destUser As String) As Object
        Using ctx As New DataModelContainer
            Dim result = (From wl In ctx.Worklists
                         Join pInst In ctx.ProcessInstances On wl.ProcInstId Equals pInst.Id
                         Join pScheme In ctx.ProcessSchemes On pInst.ProcessId Equals pScheme.ProcessId
                         Join aInst In ctx.ActivityInstances On aInst.Id Equals wl.ActInstId
                         Where wl.DestinationUser = destUser
                         Select wl, pInst.DisplayName, ProcessName = pScheme.DisplayName, aInst.ActivityName).ToList

            'New With {
            '                 .Id = wl.Id,
            '                 .DisplayName = pInst.DisplayName,
            '                 .ProcessName = pScheme.DisplayName,
            '                 .ActivityName = aInst.ActivityName,
            '                 .DestinationUser = wl.DestinationUser,
            '                 .CreateDate = wl.CreateDate,
            '                 .EndDate = wl.EndDate,
            '                 .Status = wl.Status,
            '.Duration = ""
            '                 }

            Dim wls As New List(Of Worklist)
            For Each item In result
                Dim wl = item.wl
                wl.DisplayName = item.DisplayName
                wl.ProcessName = item.ProcessName
                wl.ActivityName = item.ActivityName

                wls.Add(wl)
            Next

            Return wls
        End Using
    End Function

    Public Shared Function GetUserWorklist(destUser As String) As List(Of Worklist)
        Using ctx As New DataModelContainer
            Return ctx.Worklists.Where(Function(wl) wl.DestinationUser = destUser).ToList
        End Using
    End Function

    Public Shared Function GetProcInstWorkList(procInstId As Integer) As List(Of Worklist)
        Using ctx As New DataModelContainer
            Return ctx.Worklists.Where(Function(wl) wl.ProcInstId = procInstId).ToList
        End Using
    End Function

    Public Shared Function GetWorklistItem(procInstId As Integer, actInstId As Integer) As Worklist
        Using ctx As New DataModelContainer
            Return ctx.Worklists.SingleOrDefault(Function(wl) wl.ProcInstId = procInstId And wl.ActInstId = actInstId)
        End Using
    End Function

    Public Shared Function CompleteWorklistItem(procInstId As Integer, actInstId As Integer, destUser As String) As Boolean
        Using ctx As New DataModelContainer
            Dim wli = ctx.Worklists.SingleOrDefault(Function(wl) wl.ProcInstId = procInstId And wl.ActInstId = actInstId)
            If wli IsNot Nothing Then
                ctx.Worklists.Remove(wli)
                ctx.SaveChanges()
            End If

            Return True
        End Using
    End Function
End Class
