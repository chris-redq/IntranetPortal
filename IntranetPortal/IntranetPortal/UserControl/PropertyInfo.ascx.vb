﻿Imports DevExpress.Web
Imports Humanizer

Public Class PropertyInfo
    Inherits UserControl
    Implements ICallbackEventHandler

    Public Property LeadsInfoData As LeadsInfo = New LeadsInfo
    Public Property LeadsData As Lead

    Public Property callbackResult As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Dim cm As ClientScriptManager = Page.ClientScript
            Dim cbReference As String
            cbReference = cm.GetCallbackEventReference(Me, "arg",
                "ReceiveServerData", "")
            Dim callbackScript As String = ""
            callbackScript &= "function CallServer(arg, context)" &
                "{" & cbReference & "; }"
            cm.RegisterClientScriptBlock(Me.GetType(),
                "CallServer", callbackScript, True)
        End If
    End Sub

    Public Function BindData() As Boolean
        'If LeadsInfoData.IsApartment Then
        '    LeadsInfoData = ""
        'End If

        UpatingPanel.Visible = LeadsInfoData.IsUpdating
        hfBBLE.Value = LeadsInfoData.BBLE
        PreAssignPopup.BBLE = LeadsInfoData.BBLE
        Me.DataBind()
        Dim lisPens = LeadsInfoData.LisPens
        If lisPens IsNot Nothing Then
            gridLiens.DataSource = lisPens
            gridLiens.DataBind()
        End If
    End Function

    Public Function LinesDefendantAndIndex() As String
        Dim add_info = ""
        If LeadsInfoData.LisPens IsNot Nothing Then
            Dim index = 1
            For Each lens As PortalLisPen In LeadsInfoData.LisPens
                add_info += "Defendant Name: " + lens.Defendant + " Index: " + lens.Index + "<br>"
                index += 1
            Next
        End If
        Return add_info
    End Function

    Protected Sub leadsCommentsCallbackPanel_Callback(sender As Object, e As DevExpress.Web.CallbackEventArgsBase)
        If LeadsInfoData.BBLE Is Nothing AndAlso hfBBLE.Value IsNot Nothing Then
            LeadsInfoData = LeadsInfo.GetInstance(hfBBLE.Value)
        End If

        If e.Parameter.StartsWith("Add") Then
            Using Context As New Entities
                Dim lc As New LeadsComment
                lc.Comments = e.Parameter.Split("|")(1)
                lc.CreateBy = Page.User.Identity.Name
                lc.CreateTime = DateTime.Now
                lc.BBLE = LeadsInfoData.BBLE

                Context.LeadsComments.Add(lc)
                Context.SaveChanges()
            End Using
        End If

        If e.Parameter.StartsWith("Delete") Then
            Using Context As New Entities
                Dim commentId = CInt(e.Parameter.Split("|")(1))
                Dim lc = Context.LeadsComments.Find(commentId)
                If lc IsNot Nothing Then
                    Context.LeadsComments.Remove(lc)
                    Context.SaveChanges()
                End If
            End Using
        End If
    End Sub

    Protected Sub callPanelReferrel_Callback(sender As Object, e As DevExpress.Web.CallbackEventArgsBase)
        If e.Parameter = "Save" Then
            Dim bble = hfBBLE.Value

            Using Context As New Entities
                Dim prop = Context.PropertyReferrels.Find(bble)

                If prop Is Nothing Then
                    prop = New PropertyReferrel
                    prop.BBLE = bble
                    prop.ReferrelName = txtReferrelName.Value
                    prop.PhoneNo = txtReferrelPhone.Value
                    prop.Email = txtReferrelEmail.Value

                    Context.PropertyReferrels.Add(prop)
                Else
                    prop.ReferrelName = txtReferrelName.Value
                    prop.PhoneNo = txtReferrelPhone.Value
                    prop.Email = txtReferrelEmail.Value
                End If

                Context.SaveChanges()
            End Using
        End If
    End Sub

    Protected Sub cbpMortgageData_Callback(sender As Object, e As DevExpress.Web.CallbackEventArgsBase)
        If e.Parameter = "Save" Then
            Dim bble = hfBBLE.Value

            Using Context As New Entities
                Dim mort = Context.LeadsMortgageDatas.Find(bble)

                If mort Is Nothing Then
                    mort = New LeadsMortgageData
                    mort.BBLE = bble
                    mort.C1stFannie = cb1stFannie.Checked
                    mort.C1stFHA = cb1stFHA.Checked
                    mort.C1stServicer = txt1stServicer.Value

                    mort.C2ndFannie = cb2ndFannie.Checked
                    mort.C2ndFHA = cb2ndFHA.Checked
                    mort.C2ndServicer = txt2ndServicer.Value

                    mort.C3rdFannie = cb3rdFannie.Checked
                    mort.C3rdFHA = cb3rdFHA.Checked
                    mort.C3rdServicer = txt3rdServicer.Value

                    mort.CreateBy = Page.User.Identity.Name
                    mort.CreateDate = DateTime.Now

                    Context.LeadsMortgageDatas.Add(mort)
                Else
                    mort.C1stFannie = cb1stFannie.Checked
                    mort.C1stFHA = cb1stFHA.Checked
                    mort.C1stServicer = txt1stServicer.Value

                    mort.C2ndFannie = cb2ndFannie.Checked
                    mort.C2ndFHA = cb2ndFHA.Checked
                    mort.C2ndServicer = txt2ndServicer.Value

                    mort.C3rdFannie = cb3rdFannie.Checked
                    mort.C3rdFHA = cb3rdFHA.Checked
                    mort.C3rdServicer = txt3rdServicer.Value
                End If

                Dim li = Context.LeadsInfoes.Find(bble)
                If li IsNot Nothing Then
                    li.C1stMotgrAmt = ParseDec(txtC1stMotgr.Text)
                    li.C2ndMotgrAmt = ParseDec(txtC2ndMotgr.Text)
                    li.C3rdMortgrAmt = ParseDec(txtC3rdMotgr.Text)
                    li.TaxesAmt = ParseDec(txtTaxesAmt.Text)
                    li.WaterAmt = ParseDec(txtWaterAmt.Text)
                    li.DOBViolationsAmt = ParseDec(txtViolationAmt.Text)
                End If

                Context.SaveChanges()
            End Using

            If LeadsInfoData.BBLE Is Nothing AndAlso hfBBLE.Value IsNot Nothing Then
                LeadsInfoData = LeadsInfo.GetInstance(hfBBLE.Value)
                cbpMortgageData.DataBind()
            End If
        End If
    End Sub

    Function ParseDec(num As String) As Decimal
        Dim result As Decimal

        If Decimal.TryParse(num, result) Then
            Return result
        End If

        Return 0
    End Function

    Public Function HumanizeTime(ts As DateTime?) As String
        If ts.HasValue Then
            Return ts.Value.Humanize()
        End If

        Return Nothing
    End Function

    Public Sub RaiseCallbackEvent(eventArgument As String) Implements ICallbackEventHandler.RaiseCallbackEvent
        If String.IsNullOrEmpty(eventArgument) Then
            callbackResult = "-1"

        ElseIf eventArgument.StartsWith("getLeadsStatus") Then

            Dim bble = eventArgument.Split("|")(1)
            LeadsData = Lead.GetInstance(bble)
            If LeadsData IsNot Nothing Then
                callbackResult = "getLeadsStatusResult|" & LeadsData.Status & "|" & LeadsData.SubStatus
            Else
                callbackResult = "-1"
            End If
        Else
            callbackResult = "-1"
        End If
    End Sub

    Public Function GetCallbackResult() As String Implements ICallbackEventHandler.GetCallbackResult
        Return callbackResult
    End Function

    Protected Sub ASPxGridView1_CustomCallback(sender As Object, e As DevExpress.Web.ASPxGridViewCustomCallbackEventArgs)
        If e.Parameters.StartsWith("load") Then
            Dim bble = e.Parameters.Split("|")(1)
            Dim allLiens = DataWCFService.GetAllLiens(bble)

            Dim grid = CType(sender, ASPxGridView)
            grid.DataSource = allLiens?.localLienData?.nYCTaxLiensSold
            grid.DataBind()
        End If
    End Sub
End Class