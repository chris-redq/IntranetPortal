﻿Imports DevExpress.Web

Public Class LeadAgent
    Inherits System.Web.UI.Page

    Dim CategoryName As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not String.IsNullOrEmpty(Request.QueryString("c")) Then
            CategoryName = Request.QueryString("c")

            If Not Page.IsPostBack Then
                ASPxSplitter1.ClientVisible = True

                Dim leadPanel = ASPxSplitter1.GetPaneByName("leadPanel")
                leadPanel.Collapsed = False

                If CategoryName = "Search" Then
                    leadPanel.Size = 270
                End If

                LeadsList.BindLeadsList(CategoryName)

                If Not String.IsNullOrEmpty(Request.QueryString("id")) Then

                    If Not Page.ClientScript.IsStartupScriptRegistered("SetleadBBLE") Then
                        Dim cstext1 As String = "<script type=""text/javascript"">" &
                                        String.Format("leadsInfoBBLE = ""{0}"";", Request.QueryString("id")) & "</script>"
                        Page.ClientScript.RegisterStartupScript(Me.GetType, "SetleadBBLE", cstext1)
                    End If
                End If
            End If

            If Page.IsCallback Then
                'LeadsList.BindLeadsList(CategoryName)
            End If
        End If
    End Sub

    Private Sub LeadAgent_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
        If Not Page.IsPostBack Then
            If Not String.IsNullOrEmpty(Request.QueryString("id")) Then
                LeadsList.DisableClientEventOnLoad()
                LeadsInfo.ClientVisible = True
                LeadsInfo.BindData(Request.QueryString("id").ToString)
            End If
        End If
    End Sub
End Class