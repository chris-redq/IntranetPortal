﻿Imports System.Web.Script.Serialization
Imports IntranetPortal.ShortSale
Imports Newtonsoft.Json
Imports System.Web.Services
Imports System.Web.Script.Services

Public Class LegalUI
    Inherits System.Web.UI.Page
    Public SecondaryAction As Boolean = False
    Public Agent As Boolean = False
    Public InWorkflow As Boolean = True

    Public propertyData As String
    Public Property HiddenTab As Boolean = False
    Public Property BBLEStr As String
    Public Property DisplayView As Legal.LegalCaseStatus

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            SecondaryAction = Request.QueryString("Attorney") IsNot Nothing
            Agent = Request.QueryString("Agent") IsNot Nothing

            If Not String.IsNullOrEmpty(Request.QueryString("bble")) Then
                Dim bble = Request.QueryString("bble").ToString
                BindData(bble)
                Return
            End If

            If Not String.IsNullOrEmpty(Request.QueryString("sn")) Then
                ASPxSplitter1.Panes("listPanel").Visible = False
                Dim wli = WorkflowService.LoadTaskProcess(Request.QueryString("sn"))
                If wli IsNot Nothing Then
                    Dim bble = wli.ProcessInstance.DataFields("BBLE").ToString
                    BindData(bble)
                    'ActivityLogs.BindData(bble)

                    'If Not Page.ClientScript.IsStartupScriptRegistered("SetleadBBLE") Then
                    '    Dim cstext1 As String = "<script type=""text/javascript"">" & _
                    '                    String.Format("var leadsInfoBBLE = ""{0}"";", bble) & "</script>"
                    '    Page.ClientScript.RegisterStartupScript(Me.GetType, "SetleadBBLE", cstext1)
                    'End If
                    SetView(wli.ActivityName)

                End If
                Return

            End If

            If Not String.IsNullOrEmpty(Request.QueryString("lc")) Then
                DisplayView = CInt(Request.QueryString("lc"))
                LegalCaseList.BindCaseList(CInt(Request.QueryString("lc")))
                LegalCaseList.AutoLoadCase = True
            End If
        End If

    End Sub

    Sub Page_init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init


        HiddenTab = Not String.IsNullOrEmpty(Request.QueryString("HiddenTab"))
        If (HiddenTab) Then
            ASPxSplitter1.GetPaneByName("LogPanel").Visible = False
        End If

        Dim isInPoupUp = Request.QueryString("InPopUp") IsNot Nothing
        If (isInPoupUp) Then
            SencnedAction.Visible = True
            ASPxSplitter1.Visible = False
        End If


    End Sub

    Private Sub BindData(bble As String)
        ASPxSplitter1.Panes("listPanel").Visible = False
        BBLEStr = bble
        ActivityLogs.BindData(bble)
        If Not Page.ClientScript.IsStartupScriptRegistered("SetleadBBLE") Then
            Dim cstext1 As String = "<script type=""text/javascript"">" & _
                            String.Format("var leadsInfoBBLE = ""{0}"";", bble) & "</script>"
            Page.ClientScript.RegisterStartupScript(Me.GetType, "SetleadBBLE", cstext1)

            If Not Page.ClientScript.IsStartupScriptRegistered("InitLegalData") Then
                cstext1 = "<script type=""text/javascript"">" & _
                                String.Format("setLegalData(""{0}"");", bble) & "</script>"
                Page.ClientScript.RegisterStartupScript(Me.GetType, "InitLegalData", cstext1)
            End If
        End If
    End Sub

    Public Sub SetView(viewName As String)
        If Not ([Enum].TryParse(Of Legal.LegalCaseStatus)(viewName, DisplayView)) Then
            DisplayView = Legal.LegalCaseStatus.Closed
        End If
    End Sub

    Public Function GetAllContact() As String
        Dim json = PartyContact.getAllContact().OrderBy(Function(c) c.Name).ToJsonString
        Return json
    End Function

    Protected Sub cbpLogs_Callback(sender As Object, e As DevExpress.Web.ASPxClasses.CallbackEventArgsBase)
        ActivityLogs.BindData(e.Parameter)
    End Sub

    <WebMethod()> _
    <ScriptMethod>
    Public Shared Sub SaveCase(legalCase As Legal.LegalCase)
        legalCase.SaveData()
    End Sub

    <WebMethod()> _
   <ScriptMethod>
    Public Shared Sub SaveCaseData(caseData As String, bble As String)
        Dim lgCase = Legal.LegalCase.GetCase(bble)
        lgCase.CaseData = caseData
        lgCase.SaveData()
    End Sub

    <WebMethod()> _
   <ScriptMethod>
    Public Shared Sub CompleteResearch(caseData As String, bble As String, sn As String)
        'save data
        SaveCaseData(caseData, bble)

        Dim wli = WorkflowService.GetLegalWorklistItem(sn, bble, Legal.LegalCaseStatus.LegalResearch, HttpContext.Current.User.Identity.Name)

        If wli IsNot Nothing Then
            wli.Finish()
        End If

        'update legal case status
        Legal.LegalCase.UpdateStatus(bble, Legal.LegalCaseStatus.ManagerAssign)

        Dim comments = String.Format("Research is completed. The case is move to manager.")
        LeadsActivityLog.AddActivityLog(DateTime.Now, comments, bble, LeadsActivityLog.LogCategory.Legal.ToString, LeadsActivityLog.EnumActionType.Comments)
    End Sub

    <WebMethod()> _
  <ScriptMethod>
    Public Shared Sub AttorneyComplete(caseData As String, bble As String, sn As String)
        'save data
        SaveCaseData(caseData, bble)

        Dim wli = WorkflowService.GetLegalWorklistItem(sn, bble, Legal.LegalCaseStatus.AttorneyHandle, HttpContext.Current.User.Identity.Name)

        If wli IsNot Nothing Then
            wli.Finish()
        End If

        Legal.LegalCase.UpdateStatus(bble, Legal.LegalCaseStatus.Closed)

        Dim comments = String.Format("Case is closed.")
        LeadsActivityLog.AddActivityLog(DateTime.Now, comments, bble, LeadsActivityLog.LogCategory.Legal.ToString, LeadsActivityLog.EnumActionType.Comments)

    End Sub

    <WebMethod()> _
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Public Shared Function GetCaseData(bble As String) As String
        Dim lcase = Legal.LegalCase.GetCase(bble)
        If lcase IsNot Nothing Then
            Return lcase.CaseData
        End If
        Return "{}"

    End Function

    Protected Sub ASPxPopupControl3_WindowCallback(source As Object, e As DevExpress.Web.ASPxPopupControl.PopupWindowCallbackArgs)
        PopupContentReAssign.Visible = True

        If e.Parameter.StartsWith("type") Then
            Dim type = e.Parameter.Split("|")(1)
            hfUserType.Value = type
            listboxEmployee.DataSource = Roles.GetUsersInRole("Legal-" & type)
            listboxEmployee.DataBind()
        End If
        
        If Not String.IsNullOrEmpty(e.Parameter) AndAlso e.Parameter.StartsWith("Save") Then
            Dim bble = e.Parameter.Split("|")(1)
            Dim user = e.Parameter.Split("|")(2)
            Dim selectType = hfUserType.Value

            Dim sn = ""
            If Not String.IsNullOrEmpty(Request.QueryString("sn")) Then
                sn = Request.QueryString("sn").ToString
            End If
            'Request.QueryString("sn").ToString()

            If selectType = "Research" Then
                LegalCaseManage.AssignToResearch(sn, bble, user, Page.User.Identity.Name)
                'Else
                '    Dim wli = WorkflowService.GetLegalWorklistItem(sn, bble, Legal.LegalCaseStatus.ManagerAssign, HttpContext.Current.User.Identity.Name)

                '    If wli IsNot Nothing Then
                '        wli.ProcessInstance.DataFields("Attorney") = attorney
                '        wli.Finish()
                '    End If

                '    'update legal case status
                '    Dim lc = Legal.LegalCase.GetCase(bble)
                '    lc.Status = Legal.LegalCaseStatus.AttorneyHandle
                '    lc.Attorney = attorney
                '    lc.SaveData()
            End If

            If selectType = "Attorney" Then
                LegalCaseManage.AssignToAttorney(sn, bble, user, Page.User.Identity.Name)
            End If

            Dim comments = String.Format("The case is assign to {0}.", user)
            LeadsActivityLog.AddActivityLog(DateTime.Now, comments, bble, LeadsActivityLog.LogCategory.Legal.ToString, LeadsActivityLog.EnumActionType.Comments)
        End If
    End Sub
    Public Function GetAllRoboSingor() As String
        Return Legal.LegalRoboSignor.GetAllRoboSignor.ToJsonString
    End Function
    Public Function GetAllJudge() As String
        Return Legal.LegalJudge.GetAllJudge.ToJsonString
    End Function
End Class