﻿Public Class ViewLeadsInfo
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not String.IsNullOrEmpty(Request.QueryString("id")) Then
            Dim bble = Request.QueryString("id").ToString
            LoadData(bble)
            Return
        End If

        If Not String.IsNullOrEmpty(Request.QueryString("b")) Then
            Dim bble = Request.QueryString("b").ToString
            LeadsInfo.ShowLogPanel = False
            LoadData(bble)
            Return
        End If

        If Not String.IsNullOrEmpty(Request.QueryString("sn")) AndAlso Not Page.IsPostBack Then
            Dim sn = Request.QueryString("sn").ToString
            Dim wli = WorkflowService.LoadTaskProcess(sn)
            If wli IsNot Nothing Then
                Dim bble = wli.ProcessInstance.DataFields("BBLE").ToString
                LoadData(bble)
            End If
            Return
        End If

        If Not String.IsNullOrEmpty(Request.QueryString("ProcInstId")) Then
            Dim procInst = WorkflowService.LoadProcInstById(CInt(Request.QueryString("ProcInstId")))

            If procInst IsNot Nothing Then
                Dim bble = procInst.GetDataFieldValue("BBLE")
                If bble IsNot Nothing AndAlso Not String.IsNullOrEmpty(bble) Then
                    If Not Employee.HasControlLeads(User.Identity.Name, bble) Then
                        Server.Transfer("/PortalError.aspx?code=1001")
                    End If

                    If procInst.ProcessName = "NewLeadsRequest" Then
                        Dim result = procInst.GetDataFieldValue("Result")

                        If result = "Decline" Then
                            Response.Clear()
                            Response.Write("This new leads is declined.")
                            Response.End()
                            Return
                        End If
                    End If

                    If Not String.IsNullOrEmpty(bble) Then
                        'check if this leads is waiting manager approval
                        Dim ld = IntranetPortal.Lead.GetInstance(bble)
                        If ld IsNot Nothing AndAlso ld.Status = IntranetPortal.LeadStatus.MgrApprovalInWf Then
                            Response.Clear()
                            Response.Write("This leads is pending for manager approval. Please wait.")
                            Response.End()
                            Return
                        End If

                        If Not Employee.HasControlLeads(User.Identity.Name, bble) Then
                            Server.Transfer("/PortalError.aspx?code=1001")
                        End If

                        LoadData(bble)
                    End If
                End If

            End If
        End If
    End Sub

    Dim leadsBBLE As String

    Private Sub LoadData(bble As String)
        If Not Page.IsPostBack Then

            leadsBBLE = bble

            If Not Page.ClientScript.IsStartupScriptRegistered("SetleadBBLE") Then
                Dim cstext1 As String = "<script type=""text/javascript"">" &
                                String.Format("var leadsInfoBBLE = ""{0}"";", bble) & "</script>"
                Page.ClientScript.RegisterStartupScript(Me.GetType, "SetleadBBLE", cstext1)
            End If

            If Not Page.ClientScript.IsClientScriptBlockRegistered("OnEndCallback") Then
                Dim js As String = "<script type=""text/javascript"">" &
                                   "function OnEndCallback() {}" &
                                   "</script>"
                Page.ClientScript.RegisterClientScriptBlock(Me.GetType, "OnEndCallback", js)
            End If

        End If
        'If ShortSaleManage.IsInShortSale(bble) Then
        '    If Not Page.ClientScript.IsStartupScriptRegistered("ReloadHomebreakCtr") Then
        '        Dim js As String = "<script type=""text/javascript"">" & _
        '                           "$(function () {setTimeout(reloadHomeBreakCtrl(leadsInfoBBLE),1000)});" & _
        '                           "</script>"
        '        Page.ClientScript.RegisterStartupScript(Me.GetType, "ReloadHomebreakCtr", js)
        '    End If
        'End If
    End Sub

    Protected Sub onPreReander() Handles Me.PreRender
        If Not String.IsNullOrEmpty(leadsBBLE) Then
            LeadsInfo.BindData(leadsBBLE)
        End If
    End Sub
End Class