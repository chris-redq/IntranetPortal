﻿Imports Newtonsoft.Json
Public Class ZipMap
    Inherits System.Web.UI.Page
    Public Property leadsByZip As String
    Public Property LatLonData As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If (Not IsPostBack) Then
            bindData()
        End If

    End Sub
    Sub bindData()
        Dim s
        Using ctx As New Entities

            s = ctx.LeadsInfoes.GroupBy(Function(l) l.ZipCode).Select(Function(f) New With {.Count = f.Count, .ZipCode = f.Key}).ToList

            ctx.Leads_with_last_log.GroupBy(Function(l) l.ZipCode)

            ''LatLonData = JsonConvert.SerializeObject(ctx.LatLon_View.ToList())
           
        End Using
        leadsByZip = JsonConvert.SerializeObject(s)

    End Sub

    Protected Sub btnExport_Click(sender As Object, e As EventArgs)
        If (gridZipCountInfo.DataSource Is Nothing) Then
            gridZipCountInfo.DataSource = MapDataService.GetAllZipCountInfoList
            gridZipCountInfo.DataBind()
        End If
        gridZipCountExporter.WriteXlsxToResponse()
    End Sub
End Class