﻿Imports System.Net
Imports System.Web.Http
Imports System.Web.Http.Description
Imports IntranetPortal.Core
Imports Newtonsoft.Json.Linq

Namespace Controllers
    Public Class ReportController
        Inherits ApiController

        <ResponseType(GetType(String))>
        <Route("api/Report/Query")>
        Function PostQuery(<FromBody> queryString As JToken) As IHttpActionResult
            If queryString Is Nothing Then
                Return BadRequest()
            End If

            Try
                Dim qb As New Core.QueryBuilder

                Dim sql = qb.BuildSelectQuery(queryString, "ShortSaleCases")
                Return Ok(sql)
            Catch ex As Exception
                Throw
            End Try
        End Function

        <ResponseType(GetType(DataTable))>
        <Route("api/Report/QueryData")>
        Function PostQueryData(<FromBody> queryString As JToken, baseTable As String, includeAppId As Boolean) As IHttpActionResult
            If queryString Is Nothing Then
                Return BadRequest()
            End If

            Try
                Dim qb As New Core.QueryBuilder

                If includeAppId Then
                    'add appid into sql search
                    Dim jsAppId = <string>
                                   {
                                       "name": "AppId",
                                       "table": "ShortSaleCases",
                                       "column": "AppId",
                                       "type": "number",
                                       "hide":true,
                                        "filters":[
                                                             {
                                                                "criteria": "2",
                                                                "value": "",
                                                                "query": "",
                                                                "$$hashKey": "object:386",
                                                                "WhereTerm": "CreateCompare",
                                                                "CompareOperator": "Equal",
                                                                "value1": ""                                                                
                                                             }
                                                   ]
                                    }
                              </string>

                    Dim jsAppObj = JObject.Parse(jsAppId)
                    jsAppObj("filters")(0)("value1") = Employee.CurrentAppId
                    queryString.Last.AddAfterSelf(jsAppObj)
                End If

                If String.IsNullOrEmpty(baseTable) Then
                    baseTable = "ShortSaleCases"
                End If

                Dim dt = qb.LoadReportData(queryString, baseTable)
                Dim sql = qb.BuildSelectQuery(queryString, baseTable)

                Return Ok({dt, sql})
            Catch ex As Exception
                Throw
            End Try
        End Function

        <ResponseType(GetType(DataTable))>
        <Route("api/Report/QueryReportData")>
        Function PostLoadQueryData(report As CustomReport) As IHttpActionResult
            If Not ModelState.IsValid Then
                Return BadRequest(ModelState)
            End If

            Try
                Dim dt = report.QueryData
                Return Ok(dt)
            Catch ex As Exception
                Throw
            End Try
        End Function

        <ResponseType(GetType(CustomReport()))>
        <Route("api/Report/Load")>
        Function GetMyReports() As IHttpActionResult
            Try
                Dim report = CustomReport.GetReports(CurrentUser)
                If report Is Nothing Then
                    Return NotFound()
                End If

                Return Ok(report)
            Catch ex As Exception
                Throw
            End Try
        End Function

        <ResponseType(GetType(CustomReport))>
        <Route("api/Report/Load/{reportId}")>
        Function GetLoadReport(reportId As Integer) As IHttpActionResult
            Try
                Dim report = CustomReport.Instance(reportId)
                If report Is Nothing Then
                    Return NotFound()
                End If

                Return Ok(report)
            Catch ex As Exception
                Throw
            End Try
        End Function

        <ResponseType(GetType(Void))>
        <Route("api/Report/Save")>
        Function PostSaveReport(report As CustomReport) As IHttpActionResult
            If Not ModelState.IsValid Then
                Return BadRequest(ModelState)
            End If

            Try
                report.Save(CurrentUser)
            Catch ex As Exception
                If Not CustomReport.Exists(report.ReportId) Then
                    Return NotFound()
                Else
                    Throw
                End If
            End Try

            Return StatusCode(HttpStatusCode.NoContent)
        End Function

        <ResponseType(GetType(CustomReport))>
        <Route("api/Report/Delete/{reportId}")>
        Function DeleteReport(reportId As Integer) As IHttpActionResult
            Dim report = CustomReport.Instance(reportId)

            If IsNothing(report) Then
                Return NotFound()
            End If

            report.Delete()

            Return Ok(report)
        End Function

        Private Function CurrentUser() As String
            Return RequestContext.Principal.Identity.Name
        End Function

    End Class
End Namespace