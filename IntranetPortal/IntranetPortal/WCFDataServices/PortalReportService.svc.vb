﻿Imports System.ServiceModel.Web
Imports System.Runtime.CompilerServices
Imports System.IO
Imports System.ServiceModel


' NOTE: You can use the "Rename" command on the context menu to change the class name "PortalReportService" in code, svc and config file together.
' NOTE: In order to launch WCF Test Client for testing this service, please select PortalReportService.svc or PortalReportService.svc.vb at the Solution Explorer and start debugging.
Public Class PortalReportService
    Implements IPortalReportService

    Public Function EmployeeReports() As Channels.Message Implements IPortalReportService.EmployeeReports
        Using client As New CallTrackingService.CallTrackingServiceClient
            Return client.EmployeeStatisticData({"*"}).Where(Function(a) Not String.IsNullOrEmpty(a.EmployeeName) And a.Count > 10 And a.EmployeeName <> "Test").ToList.ToJson
        End Using
    End Function

    Public Function EmployeeCallLog(empName As String) As System.ServiceModel.Channels.Message Implements IPortalReportService.EmployeeCallLog
        Using client As New CallTrackingService.CallTrackingServiceClient
            Return client.EmployeeReports(empName).ToList.ToJson
        End Using
    End Function

    Public Function LoadAgentLeadsReport(agentName As String) As Channels.Message Implements IPortalReportService.LoadAgentLeadsReport
        Return GetProcessStatusReport({agentName})
    End Function

    Public Function LoadTeamLeadsReport(teamName As String) As Channels.Message Implements IPortalReportService.LoadTeamLeadsReport
        Dim emps = UserInTeam.GetTeamUsersArray(teamName)
        Return GetProcessStatusReport(emps)
    End Function

    Public Function LoadTeamInProcessReport(teamName As String) As List(Of LeadsStatusData) Implements IPortalReportService.LoadTeamInProcessReport
        Return GetInProcessReport(UserInTeam.GetTeamUsersArray(teamName))
    End Function

    Public Function LoadAgentInProcessReport(agentName As String) As List(Of LeadsStatusData) Implements IPortalReportService.LoadAgentInProcessReport
        Return GetInProcessReport({agentName})
    End Function

    Public Function LoadTeamInfo(teamName As String) As System.ServiceModel.Channels.Message Implements IPortalReportService.LoadTeamInfo
        Dim users = UserInTeam.GetTeamUsersArray(teamName)
        Dim inProcessCount = Utility.GetMgrLeadsCount(LeadStatus.InProcess, users)
        Dim info = New With {
                .TeamName = teamName,
                .TeamAgentCount = users.Count,
                .Users = users,
                .TotalDeals = inProcessCount,
                .EffeciencyScore = ""
            }
        Return info.ToJson
    End Function

    Public Function LoadAgentActivityReport(teamName As String, startDate As String, endDate As String) As Channels.Message Implements IPortalReportService.LoadAgentActivityReport
        Dim users = UserInTeam.GetTeamUsersArray(teamName)
        Dim dtStart = DateTime.Parse(startDate)
        Dim dtEnd = DateTime.Parse(endDate)

        Using ctx As New Entities
            Dim actionTypes = {LeadsActivityLog.EnumActionType.CallOwner,
                               LeadsActivityLog.EnumActionType.Comments,
                               LeadsActivityLog.EnumActionType.DoorKnock,
                               LeadsActivityLog.EnumActionType.FollowUp,
                               LeadsActivityLog.EnumActionType.SetAsTask,
                               LeadsActivityLog.EnumActionType.Appointment}

            Dim logSql = ctx.LeadsActivityLogs.Where(Function(al) users.Contains(al.EmployeeName) And al.ActivityDate < dtEnd And al.ActivityDate > dtStart AndAlso actionTypes.Contains(al.ActionType))
            Dim logs = logSql.ToList

            Dim result As New List(Of Object)
            For Each user In users
                'Dim actionTypes = {LeadsActivityLog.EnumActionType.CallOwner, LeadsActivityLog.EnumActionType.Comments, LeadsActivityLog.EnumActionType.DoorKnock,
                '                   LeadsActivityLog.EnumActionType.FollowUp, LeadsActivityLog.EnumActionType.SetAsTask, LeadsActivityLog.EnumActionType.Appointment}
                Dim userLogs = logs.Where(Function(l) l.EmployeeName = user)

                result.Add(New With {
                           .Name = user,
                           .CallCount = userLogs.Where(Function(l) l.ActionType.HasValue AndAlso l.ActionType = LeadsActivityLog.EnumActionType.CallOwner).Count,
                           .Comments = userLogs.Where(Function(l) l.ActionType.HasValue AndAlso l.ActionType = LeadsActivityLog.EnumActionType.Comments).Count,
                           .DoorKnock = userLogs.Where(Function(l) l.ActionType.HasValue AndAlso l.ActionType = LeadsActivityLog.EnumActionType.DoorKnock).Count,
                           .FollowUp = userLogs.Where(Function(l) l.ActionType.HasValue AndAlso l.ActionType = LeadsActivityLog.EnumActionType.FollowUp).Count,
                           .SetAsTask = userLogs.Where(Function(l) l.ActionType.HasValue AndAlso l.ActionType = LeadsActivityLog.EnumActionType.SetAsTask).Count,
                           .Appointment = userLogs.Where(Function(l) l.ActionType.HasValue AndAlso l.ActionType = LeadsActivityLog.EnumActionType.Appointment).Count,
                           .UniqueBBLE = userLogs.Select(Function(l) l.BBLE).Distinct.Count
                           })
            Next

            Return result.ToJson
        End Using
    End Function

    Public Function LoadAgentLeadsData(agentName As String, status As String) As Channels.Message Implements IPortalReportService.LoadAgentLeadsData
        Dim leadsData = Lead.GetUserLeadsData(agentName, CInt(status))
        Return LeadsGridJson(leadsData)
    End Function

    Public Function LoadTeamLeadsData(teamName As String, status As String) As Channels.Message Implements IPortalReportService.LoadTeamLeadsData
        Dim leadsData = Team.GetTeam(teamName).GetLeadsByStatus(CInt(status))
        Return LeadsGridJson(leadsData)
    End Function

#Region "Private methods"

    Private Function LeadsGridJson(leadsData As List(Of Lead)) As Channels.Message
        Dim result As New List(Of Object)
        For Each ld In leadsData
            result.Add(New With {
                       .BBLE = ld.BBLE,
                       .LeadsName = ld.LeadsName,
                       .EmployeeName = ld.EmployeeName,
                       .LastUpdate = ld.LastUpdate,
                       .Callback = ld.CallbackDate,
                       .DeadReason = If(ld.DeadReason.HasValue, CType(ld.DeadReason, Lead.DeadReasonEnum).ToString, ""),
                       .Description = ld.Description
                       })
        Next

        Return result.ToJson
    End Function

    Private Function GetProcessStatusReport(users As String()) As Channels.Message
        Using Context As New Entities
            Dim source = (From ld In Context.Leads.Where(Function(ld) users.Contains(ld.EmployeeName))
                         Group ld By Status = ld.Status Into Count()).ToDictionary(Function(l) l.Status, Function(l) l.Count)

            Dim statusToShow = New Dictionary(Of Integer, Object)
            statusToShow.Add(0, "New Leads")
            statusToShow.Add(1, "Hot Leads")
            statusToShow.Add(2, "Doorknocks")
            statusToShow.Add(3, "Follow Up")
            statusToShow.Add(4, "Dead End")
            statusToShow.Add(5, "In Process")

            'NewLead = 0
            'Priority = 1
            'DoorKnocks = 2
            'Callback = 3
            'DeadEnd = 4
            'InProcess = 5

            Dim result As New List(Of LeadsStatusData)
            For Each item In statusToShow
                result.Add(New LeadsStatusData With {
                           .Status = item.Value,
                           .Count = If(source.ContainsKey(item.Key), source(item.Key), 0),
                           .StatusKey = item.Key
                           })
            Next

            Return result.ToJson
        End Using
    End Function

    Private Function GetInProcessReport(users As String()) As List(Of LeadsStatusData)
        Dim result As New List(Of LeadsStatusData)
        Using Context As New Entities
            Dim source = Context.Leads.Where(Function(ld) users.Contains(ld.EmployeeName) And ld.Status = LeadStatus.InProcess).Select(Function(ld) ld.BBLE).ToList
            Dim shortSale = IntranetPortal.ShortSale.ShortSaleCase.GetCaseByBBLEs(source).Select(Function(s) s.BBLE).ToList
            Dim EvictionCase = IntranetPortal.ShortSale.EvictionCas.GetCaseByBBLEs(source).Select(Function(s) s.BBLE).ToList
            Dim others = (From ld In source
                         Where Not shortSale.Contains(ld) AndAlso Not EvictionCase.Contains(ld)).Distinct.Count
            result.Add(New LeadsStatusData With {.Status = "Short Sale", .Count = shortSale.Count})
            result.Add(New LeadsStatusData With {.Status = "Eviction", .Count = EvictionCase.Count})
            result.Add(New LeadsStatusData With {.Status = "Others", .Count = others})
        End Using

        Return result
    End Function
#End Region
End Class

Public Module JsonExtension
    <Extension()>
    Public Function ToJson(ByVal obj As Object) As System.ServiceModel.Channels.Message
        Dim json = Newtonsoft.Json.JsonConvert.SerializeObject(obj)
        Dim ms As New MemoryStream(New UTF8Encoding().GetBytes(json))
        ms.Position = 0
        Return WebOperationContext.Current.CreateStreamResponse(ms, "application/json")
    End Function
End Module


Public Class EmpData
    Public Property Name As String
    Public Property Count As Integer
End Class

Public Class LeadsStatusData
    Public Property Status As String
    Public Property Count As Integer
    Public Property StatusKey As Integer
End Class
