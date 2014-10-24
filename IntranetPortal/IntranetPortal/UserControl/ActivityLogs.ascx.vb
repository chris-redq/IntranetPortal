﻿Imports DevExpress.Web.ASPxEditors
Imports System.Globalization
Imports DevExpress.Web.ASPxCallbackPanel
Imports DevExpress.Web.ASPxPopupControl

Public Class ActivityLogs
    Inherits System.Web.UI.UserControl

    Private Const CommentTypeIconFormat As String = "<div class=""activity_log_item_icon {0}"">" &
                                                        "<i class=""{1}""></i>" &
                                                    "</div>"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'If Page.IsCallback Then
        '    If Not String.IsNullOrEmpty(hfBBLE.Value) Then
        '        BindData(hfBBLE.Value)
        '    End If
        'End If
    End Sub

    Public Sub BindData(bble As String)
        hfBBLE.Value = bble
        Using Context As New Entities
            gridTracking.DataSource = Context.LeadsActivityLogs.Where(Function(log) log.BBLE = bble).OrderByDescending(Function(log) log.ActivityDate).ToList
            gridTracking.DataBind()

            'If Not gridTracking.IsNewRowEditing Then
            '    gridTracking.AddNewRow()
            'End If
        End Using
    End Sub

    Sub BindEmpList()
        Using Context As New Entities
            cbEmps.DataSource = Context.Employees.ToList
            cbEmps.DataBind()

            Dim lbEmps = TryCast(empsDropDownEdit.FindControl("tabPageEmpSelect").FindControl("lbEmps"), ASPxListBox)
            lbEmps.DataSource = Context.Employees.ToList
            lbEmps.DataBind()
        End Using
    End Sub

    Protected Sub gridTracking_RowInserting(sender As Object, e As DevExpress.Web.Data.ASPxDataInsertingEventArgs) Handles gridTracking.RowInserting
        Dim aspxdate = TryCast(gridTracking.FindEditFormTemplateControl("dateActivity"), ASPxDateEdit)
        Dim txtComments = TryCast(gridTracking.FindEditFormTemplateControl("txtComments"), ASPxMemo)
        Dim getdate As DateTime = DateTime.Now

        If (aspxdate IsNot Nothing) Then
            If aspxdate.Date.Date = DateTime.Now.Date Then
                aspxdate.Date = DateTime.Now
            End If

            If aspxdate.Date < DateTime.Now Then
                aspxdate.Date = aspxdate.Date.Date
            End If
            getdate = aspxdate.Date
        End If
       

        If String.IsNullOrEmpty(txtComments.Text) Then
            Throw New Exception("Comments can not be empty.")
        End If

        LeadsActivityLog.AddActivityLog(getdate, txtComments.Text, hfBBLE.Value, LeadsActivityLog.LogCategory.SalesAgent.ToString, LeadsActivityLog.EnumActionType.Comments)
        'AddActivityLog(aspxdate.Date, txtComments.Text, hfBBLE.Value, LeadsActivityLog.LogCategory.SalesAgent.ToString)
        e.Cancel = True

        BindData(hfBBLE.Value)
    End Sub

    Public Function AddActivityLog2(logDate As DateTime, comments As String, bble As String, category As String) As LeadsActivityLog
        Using Context As New Entities
            Dim log As New LeadsActivityLog
            log.BBLE = bble
            log.EmployeeID = CInt(Membership.GetUser(Page.User.Identity.Name).ProviderUserKey)
            log.EmployeeName = Page.User.Identity.Name
            log.Category = category
            log.ActivityDate = logDate
            log.Comments = comments

            Context.LeadsActivityLogs.Add(log)
            Context.SaveChanges()

            Return log
        End Using
    End Function

    Protected Sub gridTracking_CustomCallback(sender As Object, e As DevExpress.Web.ASPxGridView.ASPxGridViewCustomCallbackEventArgs) Handles gridTracking.CustomCallback
        If (e.Parameters = "Task") Then
            SetAsTask()
        End If

        If (e.Parameters.StartsWith("CompleteTask")) Then
            Dim logId = CInt(e.Parameters.Split("|")(1))

            Using Context As New Entities
                Dim task = Context.UserTasks.Where(Function(t) t.LogID = logId).SingleOrDefault

                If task IsNot Nothing Then
                    task.Status = UserTask.TaskStatus.Complete
                    Context.SaveChanges()
                End If

                LeadsActivityLog.AddActivityLog(DateTime.Now, "Task is completed by " & Page.User.Identity.Name, hfBBLE.Value, LeadsActivityLog.LogCategory.Status.ToString, LeadsActivityLog.EnumActionType.SetAsTask)

            End Using

            BindData(hfBBLE.Value)
        End If

        'Approve New Lead
        If (e.Parameters.StartsWith("ApproveNewLead")) Then
            Dim logId = CInt(e.Parameters.Split("|")(1))

            Using Context As New Entities
                Dim log = Context.LeadsActivityLogs.Where(Function(t) t.LogID = logId).SingleOrDefault

                If log IsNot Nothing Then
                    log.Category = LeadsActivityLog.LogCategory.Approved.ToString

                    Dim lead = Context.Leads.Where(Function(ld) ld.BBLE = log.BBLE).SingleOrDefault
                    lead.Status = LeadStatus.NewLead

                    Context.SaveChanges()

                    LeadsActivityLog.AddActivityLog(DateTime.Now, "New Lead is approved by " & Page.User.Identity.Name, hfBBLE.Value, LeadsActivityLog.LogCategory.Status.ToString, LeadsActivityLog.EnumActionType.Approved)

                    'Add Notify Message
                    Dim title = "New lead is Approved"
                    Dim msg = String.Format("New Lead (BBLE: {0}) is Approved by " & Page.User.Identity.Name, lead.BBLE)
                    UserMessage.AddNewMessage(lead.EmployeeName, title, msg, lead.BBLE)
                End If
            End Using

            BindData(hfBBLE.Value)
        End If

        'Decline New Lead
        If (e.Parameters.StartsWith("DeclineNewLead")) Then
            Dim logId = CInt(e.Parameters.Split("|")(1))

            Using Context As New Entities
                Dim log = Context.LeadsActivityLogs.Where(Function(t) t.LogID = logId).SingleOrDefault

                If log IsNot Nothing Then
                    log.Category = LeadsActivityLog.LogCategory.Declined.ToString
                    Dim lead = Context.Leads.Where(Function(ld) ld.BBLE = log.BBLE).SingleOrDefault

                    Context.SaveChanges()

                    LeadsActivityLog.AddActivityLog(DateTime.Now, "New Lead is declined by " & Page.User.Identity.Name, log.BBLE, LeadsActivityLog.LogCategory.Status.ToString, LeadsActivityLog.EnumActionType.Declined)

                    'Add Notify Message
                    Dim title = "Your lead is declined"
                    Dim msg = String.Format("New Lead (BBLE: {0}) is declined by " & Page.User.Identity.Name, log.BBLE)
                    UserMessage.AddNewMessage(lead.EmployeeName, title, msg, log.BBLE)

                    Dim logs = Context.LeadsActivityLogs.Where(Function(l) l.BBLE = log.BBLE)

                    Context.LeadsActivityLogs.RemoveRange(logs)
                    Context.Leads.Remove(lead)
                    Context.SaveChanges()

                End If
            End Using

            BindData(hfBBLE.Value)
        End If

        'AcceptAppointment
        If (e.Parameters.StartsWith("AcceptAppointment")) Then
            Dim logId = CInt(e.Parameters.Split("|")(1))

            UserAppointment.UpdateAppointmentStatus(logId, UserAppointment.AppointmentStatus.Accepted)
            LeadsActivityLog.AddActivityLog(DateTime.Now, "Appointment is accepted by " & Page.User.Identity.Name, hfBBLE.Value, LeadsActivityLog.LogCategory.Status.ToString)

            'Add Notify Message
            Dim userApoint = UserAppointment.GetAppointmentBylogID(logId)
            If userApoint IsNot Nothing Then
                Dim title = "Appointment Accepted"
                Dim msg = userApoint.Subject & " is accepted by " & Page.User.Identity.Name
                UserMessage.AddNewMessage(userApoint.Agent, title, msg, userApoint.BBLE)
            End If

            Lead.UpdateLeadStatus(userApoint.BBLE, LeadStatus.Priority, Nothing)

            BindData(hfBBLE.Value)
        End If

        'Decline Appointment
        If (e.Parameters.StartsWith("DeclineAppointment")) Then
            Dim logId = CInt(e.Parameters.Split("|")(1))

            UserAppointment.UpdateAppointmentStatus(logId, UserAppointment.AppointmentStatus.Declined)
            LeadsActivityLog.AddActivityLog(DateTime.Now, "Appointment is Decline by " & Page.User.Identity.Name, hfBBLE.Value, LeadsActivityLog.LogCategory.Status.ToString)

            'Add Notify Message
            Dim userApoint = UserAppointment.GetAppointmentBylogID(logId)
            If userApoint IsNot Nothing Then
                Dim title = "Appointment Declined"
                Dim msg = userApoint.Subject & " is declined by " & Page.User.Identity.Name
                UserMessage.AddNewMessage(userApoint.Agent, title, msg, userApoint.BBLE)
            End If

            BindData(hfBBLE.Value)
        End If

        'ReSchedule Appointment
        If (e.Parameters.StartsWith("ReScheduleAppointment")) Then
            Dim logId = CInt(e.Parameters.Split("|")(1))

            UserAppointment.UpdateAppointmentStatus(logId, UserAppointment.AppointmentStatus.ReScheduled)
            LeadsActivityLog.AddActivityLog(DateTime.Now, "Appointment is Rescheduled by " & Page.User.Identity.Name, hfBBLE.Value, LeadsActivityLog.LogCategory.Status.ToString)

            'Add Notify Message
            Dim userApoint = UserAppointment.GetAppointmentBylogID(logId)
            If userApoint IsNot Nothing Then
                Dim title = "Appointment ReScheduled"
                Dim msg = userApoint.Subject & " is ReScheduled by " & Page.User.Identity.Name
                UserMessage.AddNewMessage(userApoint.Agent, title, msg, userApoint.BBLE)
            End If

            BindData(hfBBLE.Value)
        End If
    End Sub



    Sub SetAsTask()

        Dim employees = Page.User.Identity.Name & ";" & empsDropDownEdit.Text

        Dim scheduleDate = DateTime.Now
        'If cbTaskSchedule.Text = "1 Day" Then
        '    scheduleDate = scheduleDate.AddDays(1)
        'End If

        'If cbTaskSchedule.Text = "2 Day" Then
        '    scheduleDate = scheduleDate.AddDays(2)
        'End If

        'If cbTaskSchedule.Text = "Custom" Then
        '    scheduleDate = CDate(cbTaskSchedule.Text)
        'End If

        If cbTaskImportant.Text = "Normal" Then
            scheduleDate = scheduleDate.AddDays(3)
        End If

        If cbTaskImportant.Text = "Important" Then
            scheduleDate = scheduleDate.AddDays(1)
        End If

        If cbTaskImportant.Text = "Urgent" Then
            scheduleDate = scheduleDate.AddHours(2)
        End If

        Dim comments = String.Format("<table style=""width:100%;line-weight:25px;""> <tr><td style=""width:100px;"">Employees:</td>" &
                                     "<td>{0}</td></tr>" &
                                     "<tr><td>Action:</td><td>{1}</td></tr>" &
                                     "<tr><td>Important:</td><td>{2}</td></tr>" &
                                   "<tr><td>Description:</td><td>{3}</td></tr>" &
                                   "</table>", employees, cbTaskAction.Text, cbTaskImportant.Text, txtTaskDes.Text)

        Dim log = LeadsActivityLog.AddActivityLog(DateTime.Now, comments, hfBBLE.Value, LeadsActivityLog.LogCategory.Task.ToString, LeadsActivityLog.EnumActionType.SetAsTask)

        Using Context As New Entities

            Context.UserTasks.Add(AddTask(hfBBLE.Value, employees, cbTaskAction.Text, cbTaskImportant.Text, scheduleDate, txtTaskDes.Text, log.LogID))
            Context.SaveChanges()
        End Using

        Dim emps = employees.Split(";").Distinct.ToArray

        Dim ld = LeadsInfo.GetInstance(hfBBLE.Value)
        For i = 0 To emps.Count - 1
            If Not emps(i) = Page.User.Identity.Name Then
                Dim title = String.Format("A New Task has been assigned by {0}  regarding {1} for {2}", Page.User.Identity.Name, cbTaskAction.Text, ld.PropertyAddress)
                UserMessage.AddNewMessage(emps(i), title, comments, hfBBLE.Value)

            End If
        Next

        BindData(hfBBLE.Value)
    End Sub

    Function GetCommentsIconClass(type As String)
        If String.IsNullOrEmpty(type) Then
            Return GetIcon("activity_green_bg", "fa fa-info")
        End If

        Return CommentIconList(type)
    End Function

    Function ShowTaskPanel(cate As String) As Boolean
        Return cate = "Task"
    End Function

    Function AddTask(bble As String, name As String, action As String, important As String, schedule As Date, description As String, logid As Integer) As UserTask

        Dim task As New UserTask
        task.BBLE = bble
        task.EmployeeName = name
        task.Action = action
        task.Important = important
        task.Location = "In Office"
        task.Schedule = schedule
        task.EndDate = schedule.AddHours(2)
        task.Description = description
        task.Status = UserTask.TaskStatus.Active
        task.CreateDate = DateTime.Now
        task.CreateBy = Page.User.Identity.Name
        task.LogID = logid

        Return task
    End Function

    Protected Sub cbCateLog_SelectedIndexChanged(sender As Object, e As EventArgs)

    End Sub

    Protected Sub gridTracking_HtmlRowPrepared(sender As Object, e As DevExpress.Web.ASPxGridView.ASPxGridViewTableRowEventArgs) Handles gridTracking.HtmlRowPrepared
        If Not e.RowType = DevExpress.Web.ASPxGridView.GridViewRowType.Data Then
            Return
        End If

        Dim category = e.GetValue("Category").ToString

        If category = "Approval" Then
            Dim logId = CInt(e.GetValue("LogID"))
            '    Dim chkAccepted = TryCast(gridTracking.FindRowCellTemplateControl(e.VisibleIndex, gridTracking.Columns("Comments"), "chkAccepted"), ASPxCheckBox)
            'Dim chkDeclined = TryCast(gridTracking.FindRowCellTemplateControl(e.VisibleIndex, gridTracking.Columns("Comments"), "chkDeclined"), ASPxCheckBox)
            'chkAccepted.Text = " Approve"
            '    chkAccepted.Visible = True
            '    chkAccepted.ClientSideEvents.CheckedChanged = String.Format("function(s, e){{ApproveNewLead({0});}}", logId)

            '    chkDeclined.Visible = True
            '    chkDeclined.ClientSideEvents.CheckedChanged = String.Format("function(s, e){{DeclineNewLead({0});}}", logId)
        End If


        If category = "Task" Then
            If e.GetValue("LogID") IsNot Nothing Then
                Dim logId = CInt(e.GetValue("LogID"))

                Dim task = UserTask.GetTaskByLogID(logId)
                Dim pnlTask = TryCast(gridTracking.FindRowCellTemplateControl(e.VisibleIndex, gridTracking.Columns("Comments"), "pnlTask"), Panel)
                Dim btnTaskComplete = TryCast(pnlTask.FindControl("btnTaskComplete"), HtmlControl)

                Dim ltTaskResult = TryCast(pnlTask.FindControl("ltTaskResult"), Literal)

                If task Is Nothing Then

                    Dim tblTask = TryCast(pnlTask.FindControl("tblTask"), HtmlControl)
                    tblTask.Visible = False

                    Dim ltTasklogData = TryCast(pnlTask.FindControl("ltTasklogData"), Literal)
                    ltTasklogData.Text = e.GetValue("Comments")
                    Return
                Else
                    'Bind task data
                    Dim ltTaskEmp = TryCast(pnlTask.FindControl("ltTaskEmp"), Literal)
                    ltTaskEmp.Text = task.EmployeeName

                    Dim ltTaskAction = TryCast(pnlTask.FindControl("ltTaskAction"), Literal)
                    ltTaskAction.Text = task.Action

                    Dim ltTaskImpt = TryCast(pnlTask.FindControl("ltTaskImpt"), Literal)
                    ltTaskImpt.Text = task.Important

                    Dim ltTaskComments = TryCast(pnlTask.FindControl("ltTaskComments"), Literal)
                    ltTaskComments.Text = task.Description
                End If

                If task.Status = UserTask.TaskStatus.Active Then
                    If btnTaskComplete IsNot Nothing Then
                        btnTaskComplete.Visible = True
                    End If

                    'e.Row.BackColor = System.Drawing.Color.FromArgb(255, 197, 197)
                    e.Row.CssClass = "activity_log_high_light dxgvDataRow_MetropolisBlue1"

                    If task.Schedule < DateTime.Now Then
                        e.Row.ForeColor = Drawing.Color.Red
                    End If
                Else
                    If task.Status = UserTask.TaskStatus.Complete Then
                        btnTaskComplete.Visible = False

                        If ltTaskResult IsNot Nothing Then
                            ltTaskResult.Text = "Complete"
                        End If

                    Else
                        btnTaskComplete.Visible = False
                    End If

                End If
            End If
        End If


        If category = "Appointment" Then
            Dim logId = CInt(e.GetValue("LogID"))
            Dim userAppoint = UserAppointment.GetAppointmentBylogID(logId)

            Dim pnlAppointment = TryCast(gridTracking.FindRowCellTemplateControl(e.VisibleIndex, gridTracking.Columns("Comments"), "pnlAppointment"), Panel)

            If userAppoint IsNot Nothing Then
                'Bind Data
                Dim lblOwnerName = TryCast(pnlAppointment.FindControl("lblOwnerName"), Label)
                lblOwnerName.Text = LeadsInfo.GetInstance(userAppoint.BBLE).Owner
                Dim ltAptType = TryCast(pnlAppointment.FindControl("ltAptType"), Literal)
                ltAptType.Text = userAppoint.Type
                Dim ltStartTime = TryCast(pnlAppointment.FindControl("ltStartTime"), Literal)
                ltStartTime.Text = userAppoint.ScheduleDate
                Dim ltEndTime = TryCast(pnlAppointment.FindControl("ltEndTime"), Literal)
                ltEndTime.Text = userAppoint.EndDate
                Dim ltAptLocation = TryCast(pnlAppointment.FindControl("ltAptLocation"), Literal)
                ltAptLocation.Text = userAppoint.Location
                Dim ltAptMgr = TryCast(pnlAppointment.FindControl("ltAptMgr"), Literal)
                ltAptMgr.Text = userAppoint.Manager
                Dim ltAptComments = TryCast(pnlAppointment.FindControl("ltAptComments"), Literal)
                ltAptComments.Text = userAppoint.Description

                Dim btnAccept = TryCast(pnlAppointment.FindControl("btnAccept"), HtmlControl)
                Dim btnDecline = TryCast(pnlAppointment.FindControl("btnDecline"), HtmlControl)
                Dim btnReschedule = TryCast(pnlAppointment.FindControl("btnReschedule"), HtmlControl)

                'Dim chkAptAccept = TryCast(pnlAppointment.FindControl("chkAptAccept"), ASPxCheckBox)
                'Dim chkAptDecline = TryCast(pnlAppointment.FindControl("chkAptDecline"), ASPxCheckBox)
                'Dim chkAptReschedule = TryCast(pnlAppointment.FindControl("chkAptReschedule"), ASPxCheckBox)

                Dim ltResult = TryCast(pnlAppointment.FindControl("ltResult"), Literal)


                If userAppoint.Status = UserAppointment.AppointmentStatus.NewAppointment Then
                    'e.Row.BackColor = Drawing.Color.FromArgb(204, 255, 200)
                    e.Row.CssClass = "activity_log_high_light dxgvDataRow_MetropolisBlue1"
                    btnAccept.Visible = True
                    btnDecline.Visible = True
                    btnReschedule.Visible = True
                Else
                    btnAccept.Visible = False
                    btnDecline.Visible = False
                    btnReschedule.Visible = False
                End If

                If userAppoint.Status = UserAppointment.AppointmentStatus.Accepted Then
                    ltResult.Text = "Accepted"
                    'chkAptAccept.Visible = True
                    'chkAptAccept.ReadOnly = True
                    'chkAptAccept.Checked = True
                End If

                If userAppoint.Status = UserAppointment.AppointmentStatus.Declined Then
                    ltResult.Text = "Declined"
                    'chkAptDecline.Visible = True
                    'chkAptDecline.ReadOnly = True
                    'chkAptDecline.Checked = True
                End If

                If userAppoint.Status = UserAppointment.AppointmentStatus.ReScheduled Then
                    ltResult.Text = "ReScheduled"
                    'chkAptReschedule.Visible = True
                    'chkAptReschedule.ReadOnly = True
                    'chkAptReschedule.Checked = True
                End If
            Else
                Dim tblApt = TryCast(pnlAppointment.FindControl("tblApt"), HtmlControl)
                Dim ltApt = TryCast(pnlAppointment.FindControl("ltApt"), Literal)

                tblApt.Visible = False
                ltApt.Text = e.GetValue("Comments")
            End If
        End If
    End Sub

    Protected Sub gridTracking_AfterPerformCallback(sender As Object, e As DevExpress.Web.ASPxGridView.ASPxGridViewAfterPerformCallbackEventArgs)
        If Not String.IsNullOrEmpty(hfBBLE.Value) Then
            BindData(hfBBLE.Value)
        End If
    End Sub

    Public _commentIconList As StringDictionary
    Public ReadOnly Property CommentIconList As StringDictionary
        Get
            If _commentIconList Is Nothing Then
                _commentIconList = New StringDictionary
                _commentIconList.Add(LeadsActivityLog.EnumActionType.CallOwner, GetIcon("activity_green_bg", "fa fa-phone"))
                _commentIconList.Add(LeadsActivityLog.EnumActionType.DoorKnock, GetIcon("activity_red_bg", "fa fa-info"))
                _commentIconList.Add(LeadsActivityLog.EnumActionType.FollowUp, GetIcon("activity_green_bg", "fa fa-info"))
                _commentIconList.Add(LeadsActivityLog.EnumActionType.Appointment, GetIcon("activity_color_clock-o", "fa fa-clock-o"))
                _commentIconList.Add(LeadsActivityLog.EnumActionType.Email, GetIcon("activity_color_envelope", "fa fa-envelope"))
                _commentIconList.Add(LeadsActivityLog.EnumActionType.Comments, GetIcon("activity_color_comment", "fa fa-comment"))
                _commentIconList.Add(LeadsActivityLog.EnumActionType.SetAsTask, GetIcon("activity_color_tasks", "fa fa-tasks"))
                _commentIconList.Add(LeadsActivityLog.EnumActionType.UpdateInfo, GetIcon("activity_color_refresh", "fa fa-refresh"))
                _commentIconList.Add(LeadsActivityLog.EnumActionType.DealClosed, GetIcon("activity_color_check", "fa fa-check"))
                _commentIconList.Add(LeadsActivityLog.EnumActionType.HotLeads, GetIcon("activity_color_info", "fa fa-info"))
                _commentIconList.Add(LeadsActivityLog.EnumActionType.Print, GetIcon("activity_color_print", "fa fa-print"))
            End If

            Return _commentIconList
        End Get
    End Property

    Function GetIcon(bgColor As String, fontClass As String) As String
        Return String.Format(CommentTypeIconFormat, bgColor, fontClass)
    End Function


    Protected Sub addCommentsCallback_Callback(source As Object, e As DevExpress.Web.ASPxCallback.CallbackEventArgs)
        If String.IsNullOrEmpty(e.Parameter) Then
            Return
        End If

        Dim aspxdate = CDate(e.Parameter.Split("|")(0))
        Dim txtComments = e.Parameter.Split("|")(1)

        If aspxdate.Date = DateTime.Now.Date Then
            aspxdate = DateTime.Now
        End If

        If aspxdate < DateTime.Now Then
            aspxdate = aspxdate.Date
        End If

        If String.IsNullOrEmpty(txtComments) Then
            Throw New Exception("Comments can not be empty.")
        End If

        LeadsActivityLog.AddActivityLog(aspxdate, txtComments, hfBBLE.Value, LeadsActivityLog.LogCategory.SalesAgent.ToString, LeadsActivityLog.EnumActionType.Comments)

        'BindData(hfBBLE.Value)
    End Sub

    'Set as task popup call back
    Protected Sub ASPxPopupControl1_WindowCallback(source As Object, e As DevExpress.Web.ASPxPopupControl.PopupWindowCallbackArgs)
        Dim popup = CType(source, ASPxPopupControl)
        PopupContentSetAsTask.Visible = True
        BindEmpList()
    End Sub
End Class