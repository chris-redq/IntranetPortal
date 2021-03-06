﻿Imports DevExpress.Web.ASPxHtmlEditor
Imports IntranetPortal.Data

Public Class ShortSaleFileOverview
    Inherits System.Web.UI.UserControl

    Public Property Category As LeadsActivityLog.LogCategory

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Public Sub BindData(bble As String)

        hfBBLE.Value = bble

        gridTracking.DataSource = ShortSaleOverview.LoadOverview(bble, Category.ToString)
        gridTracking.DataBind()

    End Sub

    Protected Sub gridTracking_CustomCallback(sender As Object, e As DevExpress.Web.ASPxGridViewCustomCallbackEventArgs)

        If e.Parameters.StartsWith("Add") Then

            Dim comments = e.Parameters.Replace("Add|", "")

            Dim log As New ShortSaleOverview
            log.ActivityDate = DateTime.Now
            log.UserName = Page.User.Identity.Name
            log.Comments = comments
            log.BBLE = hfBBLE.Value
            log.Category = Category.ToString
            log.Save()

            BindData(hfBBLE.Value)
        End If
    End Sub

    Protected Sub txtComments_Load(sender As Object, e As EventArgs)
        If Not Page.IsPostBack Then
            Dim htmlEditor = CType(sender, ASPxHtmlEditor)
            If htmlEditor.Toolbars.Count = 0 Then
                htmlEditor.Toolbars.Add(Utility.CreateCustomToolbar("Custom"))
            End If
        End If
    End Sub

    Protected Sub gridTracking_DataBinding(sender As Object, e As EventArgs)
        If gridTracking.DataSource Is Nothing Then
            BindData(hfBBLE.Value)
        End If
    End Sub
End Class