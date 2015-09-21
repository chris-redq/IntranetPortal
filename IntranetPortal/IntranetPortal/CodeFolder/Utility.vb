﻿Imports DevExpress.Web.ASPxHtmlEditor

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
    MgrApprovalInWf = 14
    Declined = 15
    Publishing = 16
    Published = 17
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
    Public Shared Function Enum2Dictinary(cEnum As Type) As Dictionary(Of Integer, String)
        Dim VenderTypes = New Dictionary(Of Integer, String)

        Dim vals = [Enum].GetValues(cEnum)
        For Each v In vals

            VenderTypes.Add(CInt(v), Core.Utility.GetEnumDescription(v))
        Next
        Return VenderTypes
    End Function
    '<System.Runtime.CompilerServices.Extension> _
    'Public Shared Function ToDictionary(enum__1 As Type) As Dictionary(Of Integer, String)
    '    Dim type = enum__1.[GetType]()
    '    Return [Enum].GetValues(type).Cast(Of Integer)().ToDictionary(Function(e) e, Function(e) [Enum].GetName(type, e))
    'End Function

    Public Shared Function Borough2BoroughName(borough As String) As String
        Dim boroughName = "undefine borough"
        Dim arraryDic As New Dictionary(Of String, String)
        arraryDic.Add("1", "Manhattan")
        arraryDic.Add("2", "Bronx")
        arraryDic.Add("3", "Brooklyn")
        arraryDic.Add("4", "Queens")
        arraryDic.Add("5", "Staten Island")

        If (borough IsNot Nothing AndAlso arraryDic.Item(borough) IsNot Nothing) Then
            Return arraryDic.Item(borough)
        End If
        Return boroughName
    End Function

    Public Shared Function IsAny(Of T)(data As IEnumerable(Of T)) As Boolean
        Return data IsNot Nothing AndAlso data.Any()
    End Function

    Public Shared Function GetLeadsName(leadData As LeadsInfo) As String
        Dim leadsName = ""

        If leadData IsNot Nothing AndAlso Not String.IsNullOrEmpty(leadData.PropertyAddress) Then
            If String.IsNullOrEmpty(leadData.Owner) Then
                Return leadData.PropertyAddress
            End If

            If Not String.IsNullOrEmpty(leadData.UnitNum) Then
                leadsName = String.Format("{0} {1} #{3} - {2}", leadData.Number, leadData.StreetName, leadData.Owner.TrimEnd, leadData.UnitNum)
            Else
                leadsName = String.Format("{0} {1} - {2}", leadData.Number, leadData.StreetName, leadData.Owner.TrimEnd)
            End If

            leadsName = leadsName.TrimStart(" ")

            If Not String.IsNullOrEmpty(leadData.CoOwner) AndAlso leadData.Owner.TrimStart.TrimEnd <> leadData.CoOwner.TrimStart.TrimEnd Then
                leadsName += "; " & leadData.CoOwner.TrimEnd
            End If

        End If
        Return leadsName
    End Function

    Public Shared Function HtmlBlackInfo(leadData As String) As String
        Dim symble = "-"

        Dim strArry As String() = leadData.Split(New Char() {symble})
        If strArry Is Nothing Or strArry.Length < 2 Then
            Return leadData
        End If
        Dim FontStr As String = ""
        Dim EndStr As String = ""

        FontStr = strArry(0)

        EndStr = strArry(1)

        If strArry.Length > 2 Then
            FontStr = FontStr + "-" + EndStr
            EndStr = strArry(2)
        End If

        Return String.Format("<span style=""font-weight: 900;"">{0}</span>-{1}", FontStr, EndStr)
        'Return "<span style=""font-weight: 900;""> 720 QUINCY ST</span> - " & leadData
    End Function



    'Public Shared Function BuildPropertyAddress(num As String, strname As String, borough As String, neighName As String, zip As String) As String
    '    If String.IsNullOrEmpty(num) AndAlso String.IsNullOrEmpty(strname) Then
    '        Return ""
    '    End If

    '    Dim result = String.Format("{0} {1},", num, strname)

    '    If String.IsNullOrEmpty(borough) Then
    '        Return result
    '    End If

    '    If borough = "4" Then
    '        result = result & " " & neighName
    '    Else
    '        If BoroughNames(borough) IsNot Nothing Then
    '            result = result & " " & BoroughNames(borough)
    '        End If
    '    End If

    '    result = result & ",NY " & zip

    '    Return result.TrimStart.TrimEnd
    'End Function

    'Private Shared _boroughNames As Hashtable
    'Private Shared ReadOnly Property BoroughNames As Hashtable
    '    Get
    '        If _boroughNames Is Nothing Then
    '            Dim ht As New Hashtable
    '            ht.Add("1", "Manhattan")
    '            ht.Add("2", "Bronx")
    '            ht.Add("3", "Brooklyn")
    '            ht.Add("5", "Staten Island")

    '            _boroughNames = ht
    '        End If

    '        Return _boroughNames
    '    End Get
    'End Property

    Public Shared Function GetLeadsCount(status As LeadStatus, emp As String) As Integer
        Using context As New Entities
            Return context.Leads.Where(Function(l) l.EmployeeName = emp And l.Status = status).Count
        End Using
    End Function

    Public Shared Function CreateCustomToolbar(ByVal name As String) As HtmlEditorToolbar
        Return New HtmlEditorToolbar(name,
                                     New ToolbarFontSizeEdit(),
                                     New ToolbarJustifyLeftButton(True),
                                     New ToolbarJustifyCenterButton(),
                                     New ToolbarJustifyRightButton(),
                                     New ToolbarJustifyFullButton(),
                                     New ToolbarBoldButton(),
                                     New ToolbarItalicButton(),
                                     New ToolbarUnderlineButton(),
                                     New ToolbarBackColorButton(),
                                     New ToolbarFontColorButton()).CreateDefaultItems()
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

    Public Shared Function GetTeamLeadsCount(status As LeadStatus, teamId As Integer) As Integer
        If status = LeadStatus.InProcess Then
            Return GetMgrLeadsCount(status, Employee.GetTeamUsers(teamId))
        End If

        If status = LeadStatus.ALL Then
            Return GetMgrLeadsCount(status, Employee.GetTeamUsers(teamId))
        End If

        Dim emps = Employee.GetTeamUsers(teamId)
        Return GetMgrLeadsCount(status, emps)
    End Function

    Public Shared Function GetTeamUnAssignedLeadsCount(teamId As Integer) As Integer
        Return Team.GetTeam(teamId).AssignLeadsCount
    End Function

    Public Shared Function GetTeamUnAssignedLeadsCount(teamMgr As String) As Integer
        Using context As New Entities
            Return context.LeadsInfoes.Where(Function(li) li.Lead.EmployeeName = teamMgr And li.Lead.Status = LeadStatus.NewLead).Count
        End Using
    End Function

    Public Shared Function GetUnAssignedLeadsCount(Optional userContext As HttpContext = Nothing) As Integer
        Using context As New Entities
            If userContext Is Nothing AndAlso HttpContext.Current IsNot Nothing Then
                userContext = HttpContext.Current
            End If

            If userContext.User.IsInRole("Admin") Then
                Return context.LeadsInfoes.Where(Function(li) li.Lead Is Nothing).Count
            Else
                If Employee.IsManager(userContext.User.Identity.Name) Then
                    Dim name = userContext.User.Identity.Name
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


    Public Shared Function FormatPhoneNumber(ByVal myNumber As String)
        Dim mynewNumber As String
        mynewNumber = ""
        myNumber = myNumber.Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "")
        If myNumber.Length < 10 Then
            mynewNumber = myNumber
        ElseIf myNumber.Length = 10 Then
            mynewNumber = "(" & myNumber.Substring(0, 3) & ") " &
                    myNumber.Substring(3, 3) & "-" & myNumber.Substring(6, 4)
        ElseIf myNumber.Length > 10 Then
            mynewNumber = "(" & myNumber.Substring(0, 3) & ") " &
                    myNumber.Substring(3, 3) & "-" & myNumber.Substring(6, 4) & " " &
                    myNumber.Substring(10)
        End If
        Return mynewNumber
    End Function

    Public Shared Function RemoveHtmlTags(html As String) As String
        Dim text = Regex.Replace(html, "<[^>]*>", String.Empty)
        Return text
    End Function

    Public Shared Function IsSimilarName(name1 As String, name2 As String) As Boolean
        If String.IsNullOrEmpty(name1) Or String.IsNullOrEmpty(name2) Then
            Return False
        End If

        Dim strCompare = name1.Replace(",", "").Replace("&", " ").Replace("-", " ").Replace("/", "").Split(" ")
        name2 = name2.Replace(",", "").Replace("&", " ").Replace("-", " ").Replace("/", "")
        Dim nameArray = name2.Split(" ")
        Dim result = strCompare.Where(Function(a) nameArray.Contains(a)).Count / strCompare.Length

        Return result > 0.9
    End Function

    Public Shared Function FormatUserName(name As String)
        name = name.Trim
        Return StrConv(name, VbStrConv.ProperCase)
    End Function

    Public Shared Function SaveChangesObj(oldObj As Object, newObj As Object) As Object
        Dim type = oldObj.GetType()

        For Each prop In type.GetProperties
            Dim newValue = prop.GetValue(newObj)
            If newValue IsNot Nothing Then
                Dim oldValue = prop.GetValue(oldObj)
                If Not newValue.Equals(oldValue) Then
                    If prop.CanWrite Then
                        prop.SetValue(oldObj, newValue)
                    End If
                End If
            End If
        Next

        Return oldObj
    End Function

    Public Shared Function IsCompany(name As String) As Boolean
        If (String.IsNullOrEmpty(name)) Then
            Return False
        End If
        'regex match the comany like LLC Crop Etc. regex store in database

        Dim regexStr = Core.PortalSettings.GetValue("CompanyRegex")
        Dim match = Regex.Match(name, regexStr)

        Return match.Success
    End Function

    Public Shared Function GetBoroughByZip(zip As String) As String
        If String.IsNullOrEmpty(zip) Then
            Return ""
        End If

        Select Case zip.Substring(0, 3)
            Case "100"
                Return "1"
            Case "104"
                Return "2"
            Case "112"
                Return "3"
            Case "110", "111", "113", "114", "116"
                Return "4"
            Case "103"
                Return "5"
            Case Else
                Return ""
        End Select
    End Function
End Class
