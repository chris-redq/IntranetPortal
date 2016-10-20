﻿Imports System.IO
Imports System.Net
Imports System.Net.Http
Imports System.Net.Http.Headers
Imports System.Web.Http
Imports IntranetPortal.Data
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq

Namespace Controllers
    Public Class UnderwriterController
        Inherits ApiController

        <Route("api/underwriter/{bble}")>
        Public Function getUnderwriter(bble As String) As IHttpActionResult
            'Return Ok()
            Dim uw = UnderwritingManager.getInstance(bble)
            Return Ok(uw)
        End Function

        <Route("api/underwriter")>
        <HttpPost>
        Public Function postUnderwriter(<FromBody> uw As Underwriting) As IHttpActionResult
            If Not ModelState.IsValid Then
                Return BadRequest(ModelState)
            End If

            Try
                Dim u = UnderwritingManager.save(uw, HttpContext.Current.User.Identity.Name)
                Return Ok(u)
            Catch ex As Exception

            End Try

            Return Ok()
        End Function


        <Route("api/underwriter/generatexml/{bble}")>
        <HttpGet>
        Function GenerateExcel(bble As String) As IHttpActionResult



            If Not String.IsNullOrEmpty(bble) Then
                Dim ms As New MemoryStream
                Using fs = File.Open(HttpContext.Current.Server.MapPath("~/App_Data/underwriter.xlsx"), FileMode.OpenOrCreate, FileAccess.ReadWrite)
                    fs.CopyTo(ms)
                End Using

                Dim li As LeadsInfo
                Dim ds As LeadInfoDocumentSearch


                li = LeadsInfo.GetInstance(bble)

                Using ctx As New PortalEntities
                    ds = ctx.LeadInfoDocumentSearches.Find(bble)
                End Using


                If Not ms Is Nothing AndAlso Not li Is Nothing AndAlso Not ds Is Nothing Then

                    Dim excelbytes = ExcelBuilder.fillUpUnderWriterSheet(ms, li, ds)

                    Using tempFile = New FileStream(HttpContext.Current.Server.MapPath("~/TempDataFile/underwriter.xlsx"), FileMode.OpenOrCreate, FileAccess.ReadWrite)
                        tempFile.Write(excelbytes, 0, excelbytes.Length)
                        Return Ok()
                    End Using
                    ms.Close()
                End If
            End If


            Return Ok()
        End Function

        <Route("api/underwriter/getgeneratedxml/{bble}")>
        Function GetGeneratedExcel(bble As String, <FromBody> queryString As JToken) As HttpResponseMessage
            Dim response = New HttpResponseMessage(HttpStatusCode.OK)
            Dim fs = New FileStream(HttpContext.Current.Server.MapPath("~/TempDataFile/underwriter.xlsx"), FileMode.Open)
            Dim bfs = New BinaryReader(fs).ReadBytes(fs.Length)
            response.Content = New ByteArrayContent(bfs)
            response.Content.Headers.Add("Content-Disposition", "inline; filename=underwriter-" & bble & ".xlsx")
            response.Content.Headers.ContentType = New MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            response.Content.Headers.ContentLength = bfs.Length
            Return response
        End Function

    End Class
End Namespace