﻿Public Class BusinessFormDefault
    Inherits System.Web.UI.Page

    Public Property FormData As BusinessForm
    Public Property BusinessList As BusinessListControl

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then
            BusinessList.BindList()

            rptTopmenu.DataSource = FormData.Controls
            rptTopmenu.DataBind()

            rptBusinessControl.DataSource = FormData.Controls
            rptBusinessControl.DataBind()
        End If
    End Sub

    Protected Overrides Sub OnInit(e As EventArgs)
        MyBase.OnInit(e)
        BindControl()
    End Sub

    Private Sub BindControl()
        FormData = BusinessForm.Instance("title")
        BusinessList = Page.LoadControl(FormData.ListControl)
        contentSplitter.GetPaneByName("listPanel").Controls.Add(BusinessList)
    End Sub

    Protected Sub rptBusinessControl_ItemDataBound(sender As Object, e As RepeaterItemEventArgs)
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim ascx As BusinessControl = e.Item.DataItem

            Dim myControl = CType(Page.LoadControl(ascx.AscxFile), UserControl)
            myControl.DataBind()

            Dim lt = CType(e.Item.FindControl("pnlHolder"), Panel)
            lt.Controls.Add(myControl)
        End If
    End Sub
End Class