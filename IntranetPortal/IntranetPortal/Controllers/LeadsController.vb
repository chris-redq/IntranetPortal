﻿Imports System.Net
Imports System.Net.Http
Imports System.Web.Http
Imports System.Web.Http.Description
Imports Newtonsoft.Json
Imports Humanizer

Namespace Controllers

    Public Class LeadsController
        Inherits ApiController

        ''' <summary>
        ''' Return vacant leads list
        ''' </summary>
        ''' <returns></returns>
        <Route("api/Leads/VacantLeads")>
        Public Function GetVacantLeads() As IHttpActionResult
            Try
                Dim vacantleads = LeadsInfo.GetLeadsInfoByType(LeadsInfo.LeadsType.VacantLand)
                Dim result = vacantleads.Select(Function(ld) New With {ld.BBLE, ld.PropertyAddress}).ToList
                Return Ok(result)
            Catch ex As Exception
                Throw
            End Try
        End Function

        ''' <summary>
        ''' Return leadsinfo data
        ''' </summary>
        ''' <param name="bble"></param>
        ''' <returns></returns>
        <Route("api/Leads/LeadsInfo/{bble}")>
        Public Function GetLeadsInfo(bble As String) As IHttpActionResult
            Try
                Dim li = LeadsInfo.GetInstance(bble)
                Return Ok(li)
            Catch ex As Exception
                Throw
            End Try
        End Function

        ''' <summary>
        ''' Action to assign leads to user
        ''' </summary>
        ''' <param name="bble"></param>
        ''' <param name="userName"></param>
        ''' <returns></returns>
        <Route("api/Leads/Assign/{bble}")>
        <ResponseType(GetType(Void))>
        Public Function PostAssignLeads(bble As String, <FromBody> userName As String) As IHttpActionResult
            Try
                If String.IsNullOrEmpty(userName) Then
                    Return BadRequest("User name can't be empty.")
                End If

                Lead.AssignLeads(bble, userName, HttpContext.Current.User.Identity.Name)
                Return Ok()
            Catch ex As Exception
                Throw
            End Try
        End Function

        ''' <summary>
        ''' Return agent list managered by current user
        ''' </summary>
        ''' <returns></returns>
        <Route("api/Leads/ManagedAgents")>
        <ResponseType(GetType(String()))>
        Public Function GetManagedAgents() As IHttpActionResult
            Try
                Dim userName = HttpContext.Current.User.Identity.Name
                Dim emps = Employee.GetMyEmployees(userName)
                Return Ok(emps.Select(Function(e) e.Name).ToArray)
            Catch ex As Exception
                Throw
            End Try
        End Function

        <HttpPost>
        <Route("api/Leads/UpdateLeadsSubstatus/{bble}/{newSubStatus}")>
        Public Function PostUpdateLeadsSubstatus(bble As String, newSubStatus As String) As IHttpActionResult

            Try
                If Not String.IsNullOrEmpty(bble) AndAlso Not String.IsNullOrEmpty(newSubStatus) Then
                    Lead.UpdateLeadSubStatus(bble, newSubStatus)
                    Return Ok()
                End If
                Return BadRequest()
            Catch ex As Exception

                Return BadRequest(ex.ToString)
            End Try
        End Function

        <HttpPost>
        <Route("api/Leads/UpdateLeadsType/{bble}/{newType}")>
        Public Function PostUpdateLeadsSubstatus(bble As String, newType As Integer) As IHttpActionResult

            If String.IsNullOrEmpty(bble) Then
                Return BadRequest()
            End If

            Dim userName = HttpContext.Current.User.Identity.Name

            If Not Employee.HasControlLeads(HttpContext.Current.User.Identity.Name, bble) Then
                Return Unauthorized()
            End If

            Try
                Dim leadtype = CType(newType, LeadsInfo.LeadsType)
                LeadsInfo.UpdateType(bble, leadtype, userName)
                LeadsActivityLog.AddActivityLog(DateTime.Now, "Change leads type to " & leadtype.Humanize, bble, LeadsActivityLog.LogCategory.SalesAgent.ToString)
                Return Ok()
            Catch ex As Exception
                Throw ex
            End Try
        End Function

        <HttpPost>
        <Route("api/Leads/{bble}/tags")>
        Public Function PostUpdateLeadsSubstatus(bble As String, <FromBody> tagArray As String()) As IHttpActionResult

            If String.IsNullOrEmpty(bble) Then
                Return BadRequest()
            End If

            Dim userName = HttpContext.Current.User.Identity.Name

            If Not Employee.HasControlLeads(HttpContext.Current.User.Identity.Name, bble) Then
                Return Unauthorized()
            End If

            Try
                Dim tags = String.Join(";", tagArray)
                LeadsInfo.UpdateTags(bble, tags, userName)
                LeadsActivityLog.AddActivityLog(DateTime.Now, "Change leads tags to " & tags, bble, LeadsActivityLog.LogCategory.SalesAgent.ToString)
                Return Ok()
            Catch ex As Exception
                Throw ex
            End Try
        End Function

    End Class
End Namespace