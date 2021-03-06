﻿Imports IntranetPortal.Data
Imports ShortSale = IntranetPortal.Data

Public Class ShortSaleSubMenu
    Inherits System.Web.UI.UserControl

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Sub getAddressCallback_Callback(source As Object, e As DevExpress.Web.CallbackEventArgs)
        Dim propInfo = ShortSale.PropertyBaseInfo.GetInstance(e.Parameter)
        e.Result = propInfo.PropertyAddress + "|Block:" + propInfo.Block + " Lot:" + propInfo.Lot
    End Sub

    Protected Sub statusCallback_Callback(source As Object, e As DevExpress.Web.CallbackEventArgs)
        Dim status As ShortSale.CaseStatus
        If [Enum].TryParse(Of ShortSale.CaseStatus)(e.Parameter.Split("|")(0), status) Then
            Dim caseId = CInt(e.Parameter.Split("|")(1))
            Dim ssCase = ShortSaleCase.GetCase(caseId)
            Dim originStatus = CType(ssCase.Status, CaseStatus).ToString

            Dim objData = Nothing
            If e.Parameter.Split("|").Count >= 3 Then
                objData = e.Parameter.Split("|")(2)
            End If

            If (ShortSaleManage.UpdateCaseStatus(caseId, status, Page.User.Identity.Name, objData)) Then
                If status = ShortSale.CaseStatus.FollowUp Then
                    'LeadsActivityLog.AddActivityLog(DateTime.Now, String.Format("{0} set followup on {2}.", Page.User.Identity.Name, objData), ssCase.BBLE, LeadsActivityLog.LogCategory.ShortSale.ToString, LeadsActivityLog.EnumActionType.UpdateInfo)
                Else
                    LeadsActivityLog.AddActivityLog(DateTime.Now, String.Format("{0} change the case from {1} to {2}.", Page.User.Identity.Name, originStatus, status.ToString), ssCase.BBLE, LeadsActivityLog.LogCategory.ShortSale.ToString, LeadsActivityLog.EnumActionType.UpdateInfo)
                End If
            End If

            'If status = CaseStatus.FollowUp Then
            '    Dim dt As DateTime
            '    Dim objData = e.Parameter.Split("|")(2)
            '    Select Case objData
            '        Case "Tomorrow"
            '            dt = DateTime.Now.AddDays(1)
            '        Case "NextWeek"
            '            dt = DateTime.Now.AddDays(7)
            '        Case "ThirtyDays"
            '            dt = DateTime.Now.AddDays(30)
            '        Case "SixtyDays"
            '            dt = DateTime.Now.AddDays(60)
            '        Case Else
            '            If Not DateTime.TryParse(objData, dt) Then
            '                dt = DateTime.Now.AddDays(7)
            '            End If
            '    End Select
            '    ssCase.SaveFollowUp(dt)
            'Else

            'ShortSale.ShortSaleCase.GetCase(caseId).SaveStatus(status)
            'End If

        End If
    End Sub

    Protected Sub ASPxPopupControl3_WindowCallback(source As Object, e As DevExpress.Web.PopupWindowCallbackArgs)
        If String.IsNullOrEmpty(e.Parameter) Then
            PopupContentReAssign.Visible = True
            BindEmployeeList()
            Return
        End If

        If e.Parameter.Split("|").Length > 0 Then
            Dim bble = e.Parameter.Split("|")(0)
            Dim name = e.Parameter.Split("|")(1)

            ShortSaleManage.AssignCaseWithWF(bble, name, Page.User.Identity.Name)

            'ShortSaleCase.ReassignOwner(bble, name)
            'LeadsActivityLog.AddActivityLog(DateTime.Now, String.Format("{0} reassign this case to {1}.", Page.User.Identity.Name, name), bble, LeadsActivityLog.LogCategory.Status.ToString, LeadsActivityLog.EnumActionType.Reassign)
        End If
    End Sub

    Sub BindEmployeeList()
        Using Context As New Entities
            listboxEmployee.DataSource = ShortSaleManage.GetShortSaleUsers 'Employee.GetDeptUsers("Short Sale")
            listboxEmployee.DataBind()
        End Using
    End Sub

    Protected Sub ASPxPopupControl4_WindowCallback(source As Object, e As DevExpress.Web.PopupWindowCallbackArgs)
        If String.IsNullOrEmpty(e.Parameter) Then
            popupCtrEvictionUser.Visible = True
            BindEvictionUser()
            Return
        End If

        If e.Parameter.Split("|").Length > 0 Then
            Dim bble = e.Parameter.Split("|")(0)
            Dim name = e.Parameter.Split("|")(1)
            'ShortSaleCase.ReassignOwner(bble, name)
            ShortSaleCase.GetCaseByBBLE(bble).SaveStatus(CaseStatus.Eviction)
            EvictionCas.AddEviction(bble, name, Page.User.Identity.Name)

            LeadsActivityLog.AddActivityLog(DateTime.Now, String.Format("{0} set to eviction. Eviction User: {1}.", Page.User.Identity.Name, name), bble, LeadsActivityLog.LogCategory.ShortSale.ToString, LeadsActivityLog.EnumActionType.UpdateInfo)
        End If
    End Sub

    Sub BindEvictionUser()
        lbEvictionUsers.DataSource = Roles.GetUsersInRole("Eviction-User")
        lbEvictionUsers.DataBind()
    End Sub

    Protected Sub popupTitleUsers_WindowCallback(source As Object, e As DevExpress.Web.PopupWindowCallbackArgs)
        If String.IsNullOrEmpty(e.Parameter) Then
            popupContentTitleUsers.Visible = True
            listTitleUsers.DataSource = TitleManage.TitleUsers
            listTitleUsers.DataBind()
            Return
        End If

        If e.Parameter.Split("|").Length > 0 Then
            Dim bble = e.Parameter.Split("|")(0)
            Dim name = e.Parameter.Split("|")(1)

            ShortSaleManage.StartTitle(bble, Page.User.Identity.Name, name)

            'ShortSaleCase.ReassignOwner(bble, name)
            'LeadsActivityLog.AddActivityLog(DateTime.Now, String.Format("{0} reassign this case to {1}.", Page.User.Identity.Name, name), bble, LeadsActivityLog.LogCategory.Status.ToString, LeadsActivityLog.EnumActionType.Reassign)
        End If
    End Sub
End Class