﻿Public Class LeadDocSearchList
    Inherits System.Web.UI.UserControl

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If (Not IsPostBack) Then
            BindCase()
            gridDocSearch.GroupBy(gridDocSearch.Columns("Status"))
        End If

    End Sub

    Sub BindCase()
        If (gridDocSearch.DataSource Is Nothing) Then
            Using ctx As New Entities
                Dim Searches = Data.LeadInfoDocumentSearch.GetAllSearches
                Dim BBlEs = Searches.Select(Function(s) s.BBLE).ToList
                Dim leads = ctx.Leads.Where(Function(l) BBlEs.Contains(l.BBLE)).ToList
                Dim datasource = (From s In Searches
                                  Join l In leads On s.BBLE Equals l.BBLE
                                  Select New With
                                                {.BBLE = s.BBLE,
                                                 .Name = l.LeadsName,
                                                 .Status = s.Status,
                                                 .UpdateDate = s.UpdateDate,
                                                 .ExpectedSigningDate = s.ExpectedSigningDate})
                If HttpContext.Current.User.IsInRole("DocSearch-Outside") Then
                    datasource = datasource.AsEnumerable.Where(Function(c)
                                                                   Return c.Status = Data.LeadInfoDocumentSearch.SearchStatus.NewSearch
                                                               End Function)
                End If
                gridDocSearch.DataSource = datasource

                gridDocSearch.DataBind()

            End Using

        End If
    End Sub

    Protected Sub gridDocSearch_DataBinding(sender As Object, e As EventArgs)
        BindCase()
    End Sub
End Class