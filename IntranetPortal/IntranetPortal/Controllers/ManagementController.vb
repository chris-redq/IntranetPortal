﻿Imports System.Net
Imports System.Web.Http
Imports System.Web.Http.Description
Imports Newtonsoft.Json.Linq
Imports IntranetPortal.Data
Imports IntranetPortal.Data.RulesEngine
Imports System.IO
Imports Newtonsoft.Json
Imports System.Net.Http
Imports MyIdealProp.Workflow.DBPersistence

Namespace Controllers

    <Authorize(Roles:="Admin")>
    Public Class ManagementController
        Inherits ApiController

        Private ruleEngineName = System.Configuration.ConfigurationManager.AppSettings("RulesEngineServer")

        <ResponseType(GetType(Rule()))>
        <Route("api/Management/RulesEngine")>
        Function GetRulesInEngine() As Rule()
            Using svr As New RulesEngineServices(ruleEngineName)
                Return svr.Rules
            End Using
        End Function

        <ResponseType(GetType(Rule()))>
        <Route("api/Management/RulesEngine/Start/{ruleid}")>
        Function PostStartRule(ruleId As String) As Boolean
            Using svr As New RulesEngineServices(ruleEngineName)
                Return svr.StartRule(ruleId)
            End Using
        End Function

        <Route("api/Management/RulesEngine/Stop/{ruleId}")>
        Function PostStopRule(ruleId As String) As Boolean
            Using svr As New RulesEngineServices(ruleEngineName)
                svr.StopRule(ruleId)
                Return True
            End Using
        End Function

        <Route("api/Management/RulesEngine/Run/{ruleId}")>
        Function PostRunRule(ruleId As String) As Boolean
            Using svr As New RulesEngineServices(ruleEngineName)
                svr.RunRule(ruleId)
                Return True
            End Using
        End Function

        <Route("api/Management/SystemLogs/")>
        Function PostSystemLogs(<FromBody> updateTime As DateTime?) As IHttpActionResult
            Dim logs = Core.SystemLog.GetLatestLogs(updateTime)

            If logs.Count > 0 Then
                updateTime = logs.Last.CreateDate
            End If

            Return Ok(New With {
                      .Logs = logs,
                      .UpdateTime = updateTime
                      })

        End Function

        <Route("api/Management/SystemLogs/{errorId}")>
        Function GetSystemLogs(errorId As Integer) As IHttpActionResult
            Dim log = Core.SystemLog.GetLog(errorId)

            Return Ok(log)
        End Function

        <Route("api/Management/ExpiredLeads/")>
        Function PostExpiredLeads(bbles As String()) As IHttpActionResult
            If bbles Is Nothing Or Not ModelState.IsValid Then
                Return BadRequest("BBLEs can not be empty.")
            End If

            Dim i = 0

            For Each bble In bbles

                If Lead.ExpiredLeadsTask(bble) Then
                    i = i + 1
                End If
            Next

            Return Ok(i)
        End Function

        <Route("api/Management/ExpiredTaskAndWorklist/")>
        Function PostExpiredRequestUpate(taskIds As Integer()) As IHttpActionResult
            For Each taskId In taskIds
                UserTask.ExpiredTaskAndWorklist(taskId)
            Next

            Return Ok()
        End Function

        <ResponseType(GetType(String()))>
        <Route("api/Management/ConvertCSV/")>
        Public Function ConvertCSV() As String
            If HttpContext.Current.Request.Files.Count > 0 Then
                Dim file = HttpContext.Current.Request.Files(0)
                Dim fileReader = New StreamReader(file.InputStream)

                Dim template = Nothing
                Dim templates = New List(Of CSVTemplate)
                Dim line = fileReader.ReadLine()
                While Not String.IsNullOrWhiteSpace(line)
                    Dim tokens = line.Split(",")
                    If Not String.IsNullOrEmpty(tokens(0)) Then
                        If tokens(1).Contains("#") Then
                            If Not template Is Nothing Then
                                templates.Add(template)
                            End If
                            template = New CSVTemplate()
                            template.title = tokens(0).Trim
                            template.items = New List(Of CSVTemplateItem)
                        Else
                            If Not template Is Nothing Then
                                Dim item = New CSVTemplateItem
                                item.label = tokens(0).Trim
                                item.type = tokens(1).Trim
                                item.args = New List(Of String)
                                For i As Integer = 2 To tokens.Length - 1
                                    item.args.Add(tokens(i).Trim)
                                Next
                                template.items.add(item)
                            End If

                        End If
                    End If

                    line = fileReader.ReadLine()
                End While
                If Not template Is Nothing Then
                    templates.Add(template)
                End If
                Return JsonConvert.SerializeObject(templates)
            End If
            Return ""
        End Function

    End Class

    Class CSVTemplate
        Public title As String
        Public items As List(Of CSVTemplateItem)
    End Class

    Class CSVTemplateItem
        Public label As String
        Public type As String
        Public args As List(Of String)
    End Class
End Namespace