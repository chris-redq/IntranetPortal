﻿
Public Enum LeadStatus
    NewLead = 0
    Priority = 1
    DoorKnocks = 2
    Callback = 3
    DeadEnd = 4
    InProcess = 5
    Task = 6
    Closed = 7
    Create = 10
    Deleted = 11
    MgrApproval = 12
    ALL = 13
End Enum

Public Class Utility

    Public Shared Function GetLeadStatus(cateName As String) As LeadStatus
        Dim category As Integer = LeadStatus.NewLead
        Select Case cateName
            Case "New Leads"
                category = LeadStatus.NewLead
            Case "Priority", "Hot Leads"
                category = LeadStatus.Priority
            Case "Door Knock"
                category = LeadStatus.DoorKnocks
            Case "Call Back", "Follow Up"
                category = LeadStatus.Callback
            Case "Dead Lead"
                category = LeadStatus.DeadEnd
            Case "In Process"
                category = LeadStatus.InProcess
            Case "Task"
                category = LeadStatus.Task
            Case "Closed"
                category = LeadStatus.Closed
            Case "Create" 'Save as new leads
                category = LeadStatus.NewLead
        End Select

        Return category
    End Function

    Public Shared Function GetLeadsName(leadData As LeadsInfo) As String
        Dim leadsName = ""

        If leadData IsNot Nothing AndAlso Not String.IsNullOrEmpty(leadData.PropertyAddress) Then
            If String.IsNullOrEmpty(leadData.Owner) Then
                Return leadData.PropertyAddress
            End If

            leadsName = String.Format("{0} {1} - {2}", leadData.Number, leadData.StreetName, leadData.Owner)
            leadsName = leadsName.TrimStart(" ")
            If Not String.IsNullOrEmpty(leadData.CoOwner) Then
                leadsName += "; " & leadData.CoOwner
            End If

        End If
        Return leadsName
    End Function

    Public Shared Function GetLeadsCount(status As LeadStatus, emp As String) As Integer
        Using context As New Entities
            Return context.Leads.Where(Function(l) l.EmployeeName = emp And l.Status = status).Count
        End Using
    End Function

    Public Shared Function GetMgrLeadsCount(status As LeadStatus, emp As String()) As Integer
        Using context As New Entities
            If status = LeadStatus.ALL Then
                Return context.Leads.Where(Function(l) emp.Contains(l.EmployeeName)).Count
            End If

            Return context.Leads.Where(Function(l) emp.Contains(l.EmployeeName) And l.Status = status).Count
        End Using
    End Function

    Public Shared Function GetOfficeLeadsCount(status As LeadStatus, officeName As String) As Integer
        If status = LeadStatus.InProcess Then
            Return GetMgrLeadsCount(status, Employee.GetAllDeptUsers(officeName))
        End If

        If status = LeadStatus.ALL Then
            Return GetMgrLeadsCount(status, Employee.GetDeptUsers(officeName)) + GetMgrLeadsCount(LeadStatus.InProcess, Employee.GetUnActiveUser(officeName))
        End If

        Dim emps = Employee.GetDeptUsers(officeName)
        Return GetMgrLeadsCount(status, emps)
    End Function

    Public Shared Function GetUnAssignedLeadsCount() As Integer
        Using context As New Entities

            If HttpContext.Current.User.IsInRole("Admin") Then
                Return context.LeadsInfoes.Where(Function(li) li.Lead Is Nothing Or li.Lead.Employee.Active = False And li.Lead.Status <> LeadStatus.InProcess).Count
            Else
                If Employee.IsManager(HttpContext.Current.User.Identity.Name) Then
                    Dim name = HttpContext.Current.User.Identity.Name
                    Return context.LeadsInfoes.Where(Function(li) li.Lead.EmployeeName = name And li.Lead.Status = LeadStatus.NewLead).Count
                End If
            End If

            Return 0
        End Using
    End Function

    Public Shared Function GetUnAssignedLeadsCount(office As String) As Integer
        Using context As New Entities
            Dim officeName = office & " Office"
            Dim unActiveUser = Employee.GetDeptUsersList(office, False).Select(Function(emp) emp.Name).ToArray

            Dim count = (From ld In context.Leads
                                   Where ld.EmployeeName = officeName Or (unActiveUser.Contains(ld.EmployeeName) And ld.Status <> LeadStatus.InProcess)
                                   Select ld).Count
            Return count
        End Using
    End Function

    Public Shared Function TotalLeadsCount() As Integer
        Using Context As New Entities
            Dim subOridates = Employee.GetSubOrdinate(HttpContext.Current.User.Identity.Name)
            Return Context.Leads.Where(Function(ld) subOridates.Contains(ld.EmployeeID)).Count
        End Using
    End Function

    Public Shared Function TotalDealsCount() As Integer
        Return GetLeadsCount(LeadStatus.InProcess, HttpContext.Current.User.Identity.Name)
    End Function

End Class
