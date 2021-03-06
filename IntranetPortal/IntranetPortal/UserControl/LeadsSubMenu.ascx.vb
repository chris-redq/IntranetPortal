﻿Imports DevExpress.Web
Imports System.ComponentModel
Imports IntranetPortal.Data

Public Class LeadsSubMenu
    Inherits System.Web.UI.UserControl

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            MenuControl()


        End If
    End Sub

    Sub MenuControl()
        If Employee.IsManager(Page.User.Identity.Name) Then
            popupMenuLeads.Items.FindByName("Reassign").Visible = True
            popupMenuLeads.Items.FindByName("InProcess").Visible = True
        End If
    End Sub

    Sub BindEmployeeList()
        Using Context As New Entities

            'If Page.User.IsInRole("Admin") Then
            '    listboxEmployee.DataSource = Context.Employees.Where(Function(emp) emp.Active = True Or emp.Name.EndsWith("Office")).ToList.OrderBy(Function(em) em.Name)
            '    listboxEmployee.DataBind()
            '    Return
            'End If

            'Dim mgr = Employee.GetInstance(Page.User.Identity.Name)
            'Dim emps = Employee.GetSubOrdinate(mgr.EmployeeID)
            'emps.Add(mgr)
            'listboxEmployee.DataSource = emps
            listboxEmployee.DataSource = Employee.GetMyEmployees(Page.User.Identity.Name)
            listboxEmployee.DataBind()
        End Using
    End Sub

    Public ReadOnly Property PopupMenu As ASPxPopupMenu
        Get
            Return popupMenuLeads
        End Get
    End Property

    Protected Sub reassignCallback_Callback(source As Object, e As DevExpress.Web.CallbackEventArgs)
        If e.Parameter.Split("|").Length > 0 Then
            Dim bble = e.Parameter.Split("|")(0)
            Dim empId = CInt(e.Parameter.Split("|")(1))
            Dim name = e.Parameter.Split("|")(2)

            Dim ld = Lead.GetInstance(bble)
            If ld IsNot Nothing Then
                ld.ReAssignLeads(name, Page.User.Identity.Name, cbArchived.Checked)
            End If

            'Using Context As New Entities
            '    Dim lead = Context.Leads.Where(Function(ld) ld.BBLE = bble).SingleOrDefault

            '    Dim oldOwner = lead.EmployeeName
            '    lead.EmployeeID = empId
            '    lead.EmployeeName = name
            '    lead.Status = LeadStatus.NewLead
            '    Context.SaveChanges()
            '    LeadsActivityLog.AddActivityLog(DateTime.Now, String.Format("{0} reassign this lead from {1} to {2}.", Page.User.Identity.Name, oldOwner, name), bble, LeadsActivityLog.LogCategory.Status.ToString, LeadsActivityLog.EnumActionType.Reassign)
            'End Using
        End If
    End Sub

    Protected Sub getAddressCallback_Callback(source As Object, e As DevExpress.Web.CallbackEventArgs)
        Using Context As New Entities
            Dim lead = Context.LeadsInfoes.Where(Function(ld) ld.BBLE = e.Parameter).SingleOrDefault
            e.Result = lead.PropertyAddress + "|Block:" + lead.Block + " Lot:" + lead.Lot
        End Using
    End Sub

    Protected Sub leadStatusCallback_Callback(source As Object, e As DevExpress.Web.CallbackEventArgs)
        Try

            If e.Parameter.Length > 0 Then
                Dim AddComment = Nothing
                If e.Parameter.Split("|").Count > 2 Then
                    If e.Parameter.StartsWith("x") Then
                        AddComment = e.Parameter.Split("|").ToList.Item(4)
                    Else

                        AddComment = e.Parameter.Split("|").ToList.Item(2)

                    End If
                End If

                If e.Parameter.StartsWith("Tomorrow") Then
                    If e.Parameter.Contains("|") Then
                        Dim bble = e.Parameter.Split("|")(1)
                        UpdateLeadStatus(bble, LeadStatus.Callback, DateTime.Now.AddDays(1), AddComment)
                    End If
                End If

                If e.Parameter.StartsWith("nextWeek") Then
                    If e.Parameter.Contains("|") Then
                        Dim bble = e.Parameter.Split("|")(1)
                        UpdateLeadStatus(bble, LeadStatus.Callback, DateTime.Now.AddDays(7), AddComment)
                    End If
                End If

                If e.Parameter.StartsWith("thirtyDays") Then
                    If e.Parameter.Contains("|") Then
                        Dim bble = e.Parameter.Split("|")(1)
                        UpdateLeadStatus(bble, LeadStatus.Callback, DateTime.Now.AddDays(30), AddComment)
                    End If
                End If

                If e.Parameter.StartsWith("sixtyDays") Then
                    If e.Parameter.Contains("|") Then
                        Dim bble = e.Parameter.Split("|")(1)
                        UpdateLeadStatus(bble, LeadStatus.Callback, DateTime.Now.AddDays(60), AddComment)
                    End If
                End If

                If e.Parameter.StartsWith("customDays") Then
                    If e.Parameter.Contains("|") Then
                        Dim bble = e.Parameter.Split("|")(1)
                        Dim tmpDate = DateTime.Now

                        If e.Parameter.Split("|").Count > 3 Then
                            If Not DateTime.TryParse(e.Parameter.Split("|")(2), tmpDate) Then

                                Throw New Exception("The input date is not valid. Date: " & e.Parameter.Split("|")(2))
                            End If
                        End If
                        'Dim tmpdate = If(e.Parameter.Split("|").Count > 2, DateTime.Parse(e.Parameter.Split("|")(2)), DateTime.Now)
                        AddComment = e.Parameter.Split("|").ToList.Item(3)
                        UpdateLeadStatus(bble, LeadStatus.Callback, tmpDate, AddComment)
                    End If
                End If

                If e.Parameter.StartsWith(4) Then
                    If e.Parameter.Contains("|") Then
                        Dim bble = e.Parameter.Split("|")(1)
                        UpdateLeadStatus(bble, LeadStatus.DoorKnocks, Nothing, AddComment)
                    End If
                End If

                If e.Parameter.StartsWith(5) Then
                    If e.Parameter.Contains("|") Then
                        Dim bble = e.Parameter.Split("|")(1)
                        UpdateLeadStatus(bble, LeadStatus.Priority, Nothing, AddComment)
                    End If
                End If

                If e.Parameter.StartsWith(6) Then
                    If e.Parameter.Contains("|") Then
                        Dim bble = e.Parameter.Split("|")(1)
                        UpdateLeadStatus(bble, LeadStatus.DeadEnd, Nothing, AddComment)
                    End If
                End If

                If e.Parameter.StartsWith(7) Then
                    If e.Parameter.Contains("|") Then
                        Dim bble = e.Parameter.Split("|")(1)
                        UpdateLeadStatus(bble, LeadStatus.InProcess, Nothing, AddComment)

                        If Not String.IsNullOrEmpty(lbSelectionMode.Value) AndAlso lbSelectionMode.Value = 0 Then
                            'Add leads to short sale section
                            ShortSaleManage.MoveLeadsToShortSale(hfBBLE.Value, Page.User.Identity.Name, Employee.CurrentAppId)
                        End If
                    End If
                End If

                If e.Parameter.StartsWith(8) Then
                    If e.Parameter.Contains("|") Then
                        Dim bble = e.Parameter.Split("|")(1)
                        UpdateLeadStatus(bble, LeadStatus.Closed, Nothing, AddComment)
                    End If
                End If

                'Delete Lead
                If e.Parameter.StartsWith(11) Then
                    If e.Parameter.Contains("|") Then
                        Dim bble = e.Parameter.Split("|")(1)
                        UpdateLeadStatus(bble, LeadStatus.Deleted, Nothing, AddComment)
                    End If
                End If

                ' format will be "x|21|bble" , x indicates this is a special request
                If e.Parameter.StartsWith("x") Then
                    If e.Parameter.Contains("|") Then
                        Dim status = e.Parameter.Split("|")(2)
                        Dim bble = e.Parameter.Split("|")(3)

                        If (e.Parameter.StartsWith("x|LoanMod|20")) Then
                            Dim substatus = e.Parameter.Split("|")(5)
                            UpdateLeadStatus(bble, status, Nothing, AddComment, substatus)
                        Else
                            UpdateLeadStatus(bble, Status, Nothing, AddComment)
                        End If


                    End If
                End If

            End If

        Catch ex As Exception
            Throw New CallbackException(ex.Message, ex)
        End Try
    End Sub

    Sub UpdateLeadStatus(bble As String, status As LeadStatus, callbackDate As DateTime, addCommend As String, Optional subStatus As String = Nothing)
        If Not String.IsNullOrEmpty(subStatus) Then
            Lead.UpdateLeadStatus(bble, status, callbackDate, addCommend, subStatus)
        Else
            Lead.UpdateLeadStatus(bble, status, callbackDate, addCommend)
        End If


    End Sub

    Protected Sub ASPxPopupControl3_WindowCallback(source As Object, e As DevExpress.Web.PopupWindowCallbackArgs)
        PopupContentReAssign.Visible = True
        BindEmployeeList()
    End Sub

    'Dead Leads Popup
    Protected Sub ASPxPopupControl5_WindowCallback(source As Object, e As DevExpress.Web.PopupWindowCallbackArgs)
        popupContentDeadLeads.Visible = True
        If (e.Parameter.StartsWith("Show")) Then
            Dim bble = e.Parameter.Split("|")(1)
            hfBBLE.Value = bble

            Dim ld = Lead.GetInstance(bble)
            If ld.Status = LeadStatus.LoanMod AndAlso ld.SubStatus = LeadSubStatus.LoanModInProcess Then
                Throw New CallbackException("LoanMod In Progress leads can't move to DeadLeads folder.")
            End If

            cbDeadReasons.DataSource = GetType(Lead.DeadReasonEnum).GetEnumValues().OfType(Of Lead.DeadReasonEnum).ToDictionary(
              Function(key)
                  Return CInt(key).ToString
              End Function,
              Function(val)
                  Dim type = val.GetType()
                  Dim fi = type.GetField(val.ToString)
                  Dim attrs = fi.GetCustomAttributes(GetType(DescriptionAttribute), False)
                  If attrs.Length > 0 Then
                      Dim att = CType(attrs(0), DescriptionAttribute)
                      Return att.Description
                  End If
                  Return ""
              End Function
              )

            cbDeadReasons.TextField = "Value"
            cbDeadReasons.ValueField = "Key"
            cbDeadReasons.DataBind()
        End If

        If e.Parameter.StartsWith("Save") Then
            Dim reason = cbDeadReasons.Text
            Dim description = txtDeadLeadDescription.Text
            Dim createby = Page.User.Identity.Name
            Dim emp = Employee.GetInstance(createby)

            Dim comments = String.Format("<table style=""width:100%;line-weight:25px;""> <tr><td style=""width:85px;"">Dead Reason:</td>" &
                                        "<td style=""font-style: italic;font-weight:600;"">{0}</td></tr>" &
                                        "<tr><td>Description:</td><td style=""font-style: italic;font-weight:600;"">{1}</td></tr>" &
                                        "</table>", reason, description)
            If emp.Position = "Finder" AndAlso cbDeadReasons.Value = 6 Then
                Dim taskData = New With {.Reason = cbDeadReasons.Value, .Description = description}
                comments = String.Format("{0} want to dead this leads. Please approval.", createby) + comments
                LeadManage.DeadLeadProcess.ProcessStart(hfBBLE.Value, taskData.ToJsonString, createby, comments, Employee.GetReportToManger(createby).Name)
            Else
                Lead.SetDeadLeadsStatus(hfBBLE.Value, cbDeadReasons.Value, description)
                'UpdateLeadStatus(hfBBLE.Value, LeadStatus.DeadEnd, Nothing)

                LeadsActivityLog.AddActivityLog(DateTime.Now, comments, hfBBLE.Value, LeadsActivityLog.LogCategory.Status.ToString, LeadsActivityLog.EnumActionType.DeadLead)
            End If

            cbDeadReasons.Text = ""
            txtDeadLeadDescription.Text = ""
        End If

        If e.Parameter.StartsWith("DumpDeadLeads") Then
            Dim reason = cbDeadReasons.Text
            Dim description = txtDeadLeadDescription.Text
            Dim createby = Page.User.Identity.Name
            Dim emp = Employee.GetInstance(createby)

            Dim comments = String.Format("<table style=""width:100%;line-weight:25px;""> <tr><td style=""width:85px;"">Dead Reason:</td>" &
                                        "<td style=""font-style: italic;font-weight:600;"">{0}</td></tr>" &
                                        "<tr><td>Description:</td><td style=""font-style: italic;font-weight:600;"">{1}</td></tr>" &
                                        "</table>", reason, description)

            If emp.Position = "Finder" AndAlso cbDeadReasons.Value = 6 Then
                Dim taskData = New With {.Reason = cbDeadReasons.Value, .Description = description}
                comments = String.Format("{0} want to dead this leads. Please approval.", createby) + comments
                LeadManage.DumpLeadProcess.ProcessStart(hfBBLE.Value, taskData.ToJsonString, createby, comments, Employee.GetReportToManger(createby).Name)
            Else
                Dim l = Lead.GetInstance(hfBBLE.Value)
                l.ReAssignLeads("Dead Leads", Page.User.Identity.Name)
                Lead.SetDeadLeadsStatus(l.BBLE, cbDeadReasons.Value, txtDeadLeadDescription.Text, True)
                LeadsActivityLog.AddActivityLog(DateTime.Now, comments, hfBBLE.Value, LeadsActivityLog.LogCategory.Status.ToString, LeadsActivityLog.EnumActionType.DeadLead)
            End If

            cbDeadReasons.Text = ""
            txtDeadLeadDescription.Text = ""
        End If
    End Sub

    'In Process Popup
    Protected Sub ASPxPopupControl4_WindowCallback(source As Object, e As DevExpress.Web.PopupWindowCallbackArgs)
        popupContentInProcess.Visible = True
        If (e.Parameter.StartsWith("Show")) Then
            Dim bble = e.Parameter.Split("|")(1)
            hfInProcessBBLE.Value = bble
            BindEvictionUsers()

            'Bind third part
            cbThirdParty.DataSource = Core.CommonData.GetData("InProcessThirdParty").Select(Function(tp) tp.Name).ToArray
            cbThirdParty.DataBind()
        End If

        If e.Parameter.StartsWith("Save") Then
            Dim bble = hfInProcessBBLE.Value
            UpdateLeadStatus(bble, LeadStatus.InProcess, Nothing, Nothing)

            If Not String.IsNullOrEmpty(lbSelectionMode.Value) Then
                If lbSelectionMode.SelectedValues.Contains("0") Then
                    'Add leads to short sale section
                    If Not ShortSaleManage.IsInProcess(hfInProcessBBLE.Value) Then
                        ShortSaleManage.MoveLeadsToShortSale(hfInProcessBBLE.Value, Page.User.Identity.Name, Employee.CurrentAppId)
                    Else

                    End If
                End If

                If lbSelectionMode.SelectedValues.Contains("1") Then
                    If Not ShortSaleManage.IsInProcess(hfInProcessBBLE.Value) Then
                        ShortSaleManage.MoveLeadsToShortSale(bble, Page.User.Identity.Name, Employee.CurrentAppId)
                    End If

                    Dim name = cbEvictionUsers.Value
                    If String.IsNullOrEmpty(name) Then
                        Throw New Exception("Please select Eviction User")
                    End If
                    EvictionCas.AddEviction(bble, name, Page.User.Identity.Name)
                End If

                If lbSelectionMode.SelectedValues.Contains("4") Then
                    Lead.Publishing(bble)
                End If

                If lbSelectionMode.SelectedValues.Contains("5") Then
                    Dim category = cbThirdParty.Value
                    If String.IsNullOrEmpty(category) Then
                        Throw New Exception("Please Select Third Parties")
                    End If

                    Lead.InThirdParty(bble, category, "", Page.User.Identity.Name)
                End If

                If lbSelectionMode.SelectedValues.Contains("6") Then
                    Lead.InThirdParty(bble, "Straight Sale", "", Page.User.Identity.Name)
                End If

                If lbSelectionMode.SelectedValues.Contains("2") Then
                    Dim ld = Lead.GetInstance(bble)
                    ConstructionManage.StartConstruction(bble, ld.LeadsName, Page.User.Identity.Name)
                    LeadsActivityLog.AddActivityLog(DateTime.Now, "Move to Construction.", bble, LeadsActivityLog.LogCategory.PublicUpdate.ToString)
                End If
            End If
        End If

        If e.Parameter.StartsWith("StartlegalProcess") Then
            Dim bble = hfInProcessBBLE.Value
            UpdateLeadStatus(bble, LeadStatus.InProcess, Nothing, Nothing)

            Dim caseData = e.Parameter.Replace("StartlegalProcess|", "")
            LegalCaseManage.StartLegalRequest(bble, caseData, Page.User.Identity.Name)
        End If
    End Sub
    'Public Function getGenoInfo() As Object
    '    Lead.GetInstance(hfBBLE).toj()
    'End Function
    Private Sub BindEvictionUsers()
        cbEvictionUsers.DataSource = Roles.GetUsersInRole("Eviction-User")
        cbEvictionUsers.DataBind()
    End Sub

    Protected Sub ASPxPopupControl6_WindowCallback(source As Object, e As DevExpress.Web.PopupWindowCallbackArgs)
        If e.Parameter.StartsWith("Show") Then
            Dim bble = e.Parameter.Split("|")(1)
            Dim ctr = CType(LoadControl("~/UserControl/DocumentsUI.ascx"), DocumentsUI)
            ctr.ID = popupWinViewFiles.ID & "FileControls"
            ctr.LeadsName = Lead.GetInstance(bble).LeadsName
            ctr.ViewMode = True
            ctr.BindFilesFromSharepoint(bble)
            e.Window.Controls.Add(ctr)
        End If
    End Sub

    Protected Sub popupRequestUpdate_WindowCallback(source As Object, e As DevExpress.Web.PopupWindowCallbackArgs)
        popContentRequestUpdate.Visible = True
        If e.Parameter = "SendRequest" Then
            Dim bble = hfRequestUpdateBBLE.Value

            If Not String.IsNullOrEmpty(bble) Then
                SendRequest(bble)

                'Cancel Edit, Notify User
                'gridLeads.CancelEdit()
            End If
        Else
            Dim bble = e.Parameter
            hfRequestUpdateBBLE.Value = bble

            Dim ld = Lead.GetInstance(bble)
            TryCast(requestUpdateFormlayout.FindControl("txtRequestUpdateLeadsName"), ASPxTextBox).Text = ld.LeadsName
            TryCast(requestUpdateFormlayout.FindControl("txtRequestUpdateCreateby"), ASPxTextBox).Text = ld.EmployeeName
            TryCast(requestUpdateFormlayout.FindControl("txtRequestUpdateManager"), ASPxTextBox).Text = Employee.GetReportToManger(Page.User.Identity.Name).Name
        End If
    End Sub

    Sub SendRequest(bble)

        Lead.RequestUpdate(bble, txtTaskDes.Text, cbTaskImportant.Text, Page.User.Identity.Name, txtRequestUpdateManager.Text)

        Return



        Dim employees = New List(Of String)
        employees.Add(Page.User.Identity.Name)
        employees.Add(txtRequestUpdateCreateby.Text)
        employees.Add(txtRequestUpdateManager.Text)
        Dim emps = String.Join(";", employees.Distinct().ToArray)

        'Start new task
        Dim actlog As New ActivityLogs
        actlog.SetAsTask(emps, cbTaskImportant.Text, "Request Update", txtTaskDes.Text, bble, Page.User.Identity.Name)
    End Sub

    Protected Sub aspxPopupLegalInfo_WindowCallback(source As Object, e As DevExpress.Web.PopupWindowCallbackArgs)

    End Sub
End Class