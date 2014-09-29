﻿Imports System.Web.Script.Serialization
Imports IntranetPortal.Messager

Public Class NavMenu
    Inherits System.Web.UI.UserControl

    Private Shared XmlDataFile As String = "~/App_Data/PortalMenu.xml"
    Public Property PortalMenuItems As List(Of PortalNavItem)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        PortalMenuItems = LoadMenuFromXml(HttpContext.Current)
        'InitialMenu()
        'WriteXML()

    End Sub

    Public Sub InitialMenu()
        Dim item As New PortalNavItem
        item.Name = "Manager"
        item.Text = "Manager"
        item.NavigationUrl = "/summary.aspx"

        PortalMenuItems = New List(Of PortalNavItem)
        PortalMenuItems.Add(item)
    End Sub

    Public Sub WriteXML()
        Dim writer As New System.Xml.Serialization.XmlSerializer(GetType(List(Of PortalNavItem)))
        Dim file As New System.IO.StreamWriter(Server.MapPath(XmlDataFile))
        writer.Serialize(file, PortalMenuItems)
        file.Close()
    End Sub

    Public Shared Function LoadMenuFromXml(context As HttpContext) As List(Of PortalNavItem)
        Dim reader As New System.Xml.Serialization.XmlSerializer(GetType(List(Of PortalNavItem)))
        Dim file As New System.IO.StreamReader(context.Server.MapPath(XmlDataFile))
        Dim menuItems = CType(reader.Deserialize(file), List(Of PortalNavItem))
        file.Close()

        Return menuItems
    End Function
End Class

Public Class RefreshLeadsCountHandler
    Implements IHttpAsyncHandler

    Private _Delegate As AsyncProcessorDelegate
    Protected Delegate Sub AsyncProcessorDelegate(context As HttpContext)

    Public Function BeginProcessRequest(context As HttpContext, cb As AsyncCallback, extraData As Object) As IAsyncResult Implements IHttpAsyncHandler.BeginProcessRequest
        _Delegate = New AsyncProcessorDelegate(AddressOf ProcessRequest)
        Return _Delegate.BeginInvoke(context, cb, extraData)
    End Function

    Public Sub EndProcessRequest(result As IAsyncResult) Implements IHttpAsyncHandler.EndProcessRequest

    End Sub

    Public ReadOnly Property IsReusable As Boolean Implements IHttpHandler.IsReusable
        Get

        End Get
    End Property

    Public Sub ProcessRequest(context As HttpContext) Implements IHttpHandler.ProcessRequest
        Dim items = GetAllMenuItems(NavMenu.LoadMenuFromXml(context), context)
        Dim results = From item In items.Where(Function(nav) nav.ShowAmount = True)
                     Select New With {
                         .Name = item.LeadsCountSpanId,
                         .Count = GetLeadsCount(item.Name, item.Text, context.User.Identity.Name, context)
                                }

        Dim json As New JavaScriptSerializer
        Dim jsonString = json.Serialize(results)

        If results IsNot Nothing Then
            context.Response.Clear()
            context.Response.ContentType = "text/html"
            context.Response.Write(jsonString)
            context.Response.End()
        End If
    End Sub

    Public Function GetAllMenuItems(navMenu As List(Of PortalNavItem), userContext As HttpContext, Optional results As List(Of PortalNavItem) = Nothing) As List(Of PortalNavItem)
        If results Is Nothing Then
            results = New List(Of PortalNavItem)
        End If

        For Each item In navMenu
            If item.IsVisible(userContext) Then
                results.Add(item)

                If item.Items IsNot Nothing AndAlso item.Items.Count > 0 Then
                    results = GetAllMenuItems(item.Items, userContext, results)
                End If
            End If
        Next

        Return results
    End Function

    Public Function GetLeadsCount(name As String, itemText As String, userName As String, context As HttpContext) As Integer
        If name.StartsWith("Agent") Then
            Return Utility.GetLeadsCount(Utility.GetLeadStatus(itemText), userName)
        End If

        If name.StartsWith("Mgr") Then
            Dim emps = Employee.GetManagedEmployees(userName)
            Return Utility.GetMgrLeadsCount(Utility.GetLeadStatus(itemText), emps)
        End If

        If name.StartsWith("Task") Then
            Return UserTask.GetTaskCount(userName, context)
        End If

        If name = "AssignLeads" Then
            Return Utility.GetUnAssignedLeadsCount(context)
        End If

        If name.StartsWith("Office") Then
            Return GetOfficeLeadsCount(name, itemText)
        End If

        If name.StartsWith("Team") Then
            Return GetTeamLeadsCount(name, itemText)
        End If

        If name.StartsWith("ShortSale") Then
            Return GetShortSaleCaseCount(name, itemText)
        End If
    End Function

    Function GetTeamLeadsCount(name As String, itemText As String) As Integer
        'Dim mgrId = CInt(name.Split("-")(1))

        Dim tmpStr = name.Split("-")

        If tmpStr.Length > 2 Then
            Dim mgrId = CInt(tmpStr(1))
            Dim type = tmpStr(2)
            Dim mgrName = Employee.GetInstance(mgrId).Name
            Select Case type
                Case "AssignLeads"
                    Return Utility.GetTeamUnAssignedLeadsCount(mgrName)
                Case "Management"
                    Return Utility.GetMgrLeadsCount(LeadStatus.ALL, Employee.GetManagedEmployees(mgrName))
                Case Else
                    Return Utility.GetMgrLeadsCount(Utility.GetLeadStatus(itemText), Employee.GetManagedEmployees(mgrName))
            End Select
        End If

    End Function

    Function GetOfficeLeadsCount(name As String, itemText As String) As Integer
        Dim tmpStr = name.Split("-")

        If tmpStr.Length > 2 Then
            Dim office = tmpStr(1)
            Dim type = tmpStr(2)

            Select Case type
                Case "AssignLeads"
                    Return Utility.GetUnAssignedLeadsCount(office)
                Case "Management"
                    Return Utility.GetOfficeLeadsCount(LeadStatus.ALL, office) + Utility.GetUnAssignedLeadsCount(office)
                Case Else
                    Return Utility.GetOfficeLeadsCount(Utility.GetLeadStatus(itemText), office)
            End Select
        End If

    End Function


    Function GetShortSaleCaseCount(name As String, itemText As String) As Integer
        Dim tmpStr = name.Split("-")

        If tmpStr.Length > 1 Then
            Dim type = tmpStr(1)

            Select Case type
                Case "AssignLeads"
                    Return 0 'Utility.GetUnAssignedLeadsCount(Office)                
                Case Else
                    Dim status As ShortSale.CaseStatus

                    If [Enum].TryParse(Of ShortSale.CaseStatus)(type, status) Then
                        Return ShortSale.ShortSaleCase.GetCaseCount(status)
                    Else
                        Return 0
                    End If
            End Select
        End If
    End Function
End Class