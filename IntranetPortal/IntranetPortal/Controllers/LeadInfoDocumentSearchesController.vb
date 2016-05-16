﻿Imports System.Data
Imports System.Data.Entity
Imports System.Data.Entity.Infrastructure
Imports System.Linq
Imports System.Net
Imports System.Net.Http
Imports System.Web.Http
Imports System.Web.Http.Description
Imports System.Web.Http.Results
Imports System.Web.Mail
Imports IntranetPortal.Data
Imports IntranetPortal.LeadsActivityLog

Namespace Controllers
    Public Class LeadInfoDocumentSearchesController
        Inherits System.Web.Http.ApiController

        Private db As New PortalEntities

        ' GET: api/LeadInfoDocumentSearches
        Function GetLeadInfoDocumentSearches() As IQueryable(Of LeadInfoDocumentSearch)
            Return db.LeadInfoDocumentSearches
        End Function

        ' GET: api/LeadInfoDocumentSearches/5
        <ResponseType(GetType(LeadInfoDocumentSearch))>
        Function GetLeadInfoDocumentSearch(ByVal id As String) As IHttpActionResult
            Dim leadInfoDocumentSearch As LeadInfoDocumentSearch = db.LeadInfoDocumentSearches.Find(id)
            If IsNothing(leadInfoDocumentSearch) Then
                Throw New Exception("Please make sure you submit a search first!")
            End If

            Return Ok(leadInfoDocumentSearch)
        End Function

        ' PUT: api/LeadInfoDocumentSearches/5
        <ResponseType(GetType(Void))>
        Function PutLeadInfoDocumentSearch(ByVal id As String, ByVal leadInfoDocumentSearch As LeadInfoDocumentSearch) As IHttpActionResult
            If Not ModelState.IsValid Then
                Return BadRequest(ModelState)
            End If

            If Not id.Trim = leadInfoDocumentSearch.BBLE.Trim Then
                Return BadRequest()
            End If

            db.Entry(leadInfoDocumentSearch).State = EntityState.Modified
            leadInfoDocumentSearch.UpdateBy = HttpContext.Current.User.Identity.Name
            leadInfoDocumentSearch.UpdateDate = Date.Now
            'leadInfoDocumentSearch.LeadResearch
            If (leadInfoDocumentSearch.ResutContent IsNot Nothing) Then
                If Not leadInfoDocumentSearch.IsSave Then
                    Dim l = LeadsInfo.GetInstance(leadInfoDocumentSearch.BBLE)
                    Dim maildata As New Dictionary(Of String, String)
                    maildata.Add("Address", l.PropertyAddress)
                    maildata.Add("UserName", leadInfoDocumentSearch.CreateBy)
                    maildata.Add("ResutContent", leadInfoDocumentSearch.ResutContent)

                    If Not String.IsNullOrEmpty(leadInfoDocumentSearch.CreateBy) Then

                        Core.EmailService.SendMail(Employee.GetEmpsEmails(leadInfoDocumentSearch.CreateBy),
                                                   Employee.GetEmpsEmails(leadInfoDocumentSearch.UpdateBy, Employee.CEO.Name),
                                                   "DocSearchCompleted", maildata)
                    End If
                End If
            End If

            Try
                db.SaveChanges()
            Catch ex As DbUpdateConcurrencyException
                If Not (LeadInfoDocumentSearchExists(id)) Then
                    Return NotFound()
                Else
                    Throw
                End If
            End Try

            Return StatusCode(HttpStatusCode.NoContent)
        End Function

        <Route("api/LeadInfoDocumentSearches/{bble}/Completed")>
        <ResponseType(GetType(LeadInfoDocumentSearch))>
        Function PostCompleted(ByVal bble As String, ByVal leadInfoDocumentSearch As LeadInfoDocumentSearch) As IHttpActionResult

            leadInfoDocumentSearch.Status = LeadInfoDocumentSearch.SearchStauts.Completed
            leadInfoDocumentSearch.CompletedBy = HttpContext.Current.User.Identity.Name
            leadInfoDocumentSearch.CompletedOn = Date.Now

            Try
                leadInfoDocumentSearch.Save()
            Catch ex As Exception
                Throw ex
            End Try

            If (Not String.IsNullOrEmpty(leadInfoDocumentSearch.ResutContent)) Then
                SendCompleteNotify(leadInfoDocumentSearch)
            End If

            Return Ok(leadInfoDocumentSearch)
            'PostLeadInfoDocumentSearch(leadInfoDocumentSearch)
        End Function

        Private Sub SendCompleteNotify(leadInfoDocumentSearch As LeadInfoDocumentSearch)
            Dim l = LeadsInfo.GetInstance(leadInfoDocumentSearch.BBLE)
            Dim maildata As New Dictionary(Of String, String)
            If l IsNot Nothing Then
                maildata.Add("Address", l.PropertyAddress)
            Else
                maildata.Add("Address", leadInfoDocumentSearch.BBLE)
            End If

            maildata.Add("UserName", leadInfoDocumentSearch.CreateBy)
            maildata.Add("ResutContent", leadInfoDocumentSearch.ResutContent)

            If Not String.IsNullOrEmpty(leadInfoDocumentSearch.CreateBy) Then
                Dim attachment As Mail.Attachment
                Dim judgeDoc = leadInfoDocumentSearch.LoadJudgesearchDoc
                If judgeDoc IsNot Nothing Then
                    attachment = New Mail.Attachment(New IO.MemoryStream(CType(judgeDoc.Data, Byte())), judgeDoc.Name.ToString)
                    Core.EmailService.SendMail(Employee.GetEmpsEmails(leadInfoDocumentSearch.CreateBy),
                                               Employee.GetEmpsEmails(leadInfoDocumentSearch.UpdateBy, Employee.CEO.Name),
                                               "DocSearchCompleted", maildata, {attachment})
                Else
                    Core.EmailService.SendMail(Employee.GetEmpsEmails(leadInfoDocumentSearch.CreateBy),
                                           Employee.GetEmpsEmails(leadInfoDocumentSearch.UpdateBy,
                                                                  Employee.CEO.Name), "DocSearchCompleted", maildata)
                End If
            End If
        End Sub

        ' POST: api/LeadInfoDocumentSearches
        <ResponseType(GetType(LeadInfoDocumentSearch))>
        Function PostLeadInfoDocumentSearch(ByVal leadInfoDocumentSearch As LeadInfoDocumentSearch) As IHttpActionResult
            If Not ModelState.IsValid Then
                Return BadRequest(ModelState)
            End If
            Dim findSearch = db.LeadInfoDocumentSearches.Find(leadInfoDocumentSearch.BBLE)

            If (findSearch Is Nothing) Then
                db.LeadInfoDocumentSearches.Add(leadInfoDocumentSearch)
                leadInfoDocumentSearch.CreateBy = HttpContext.Current.User.Identity.Name
                leadInfoDocumentSearch.CreateDate = Date.Now
                LeadsActivityLog.AddActivityLog(Date.Now(), "Create a search request to doc Search Agent ", leadInfoDocumentSearch.BBLE, LogCategory.SalesAgent.ToString)
                Dim EntityMananger = Roles.GetUsersInRole("Entity-Manager")(0)

                If (Not String.IsNullOrEmpty(EntityMananger)) Then
                    Dim LeadInfoSearchUser = Employee.GetInstance(EntityMananger) 'Employee.GetInstance("Elizabeth Rodriguez")
                    Dim empl = Employee.GetInstance(HttpContext.Current.User.Identity.Name)
                    Dim mLead = LeadsInfo.GetInstance(leadInfoDocumentSearch.BBLE)

                    If (LeadInfoSearchUser IsNot Nothing AndAlso mLead IsNot Nothing) Then
                        Core.EmailService.SendMail(Employee.GetEmpsEmails(LeadInfoSearchUser), Employee.GetEmpsEmails(empl),
                                               "DocSearchNotify",
                                                New Dictionary(Of String, String) From
                                                {
                                                    {"SubmitUser", empl.Name},
                                                    {"Address", mLead.PropertyAddress},
                                                    {"DocUser", LeadInfoSearchUser.Name},
                                                    {"BBLE", leadInfoDocumentSearch.BBLE},
                                                    {"ExpectedDate", IIf(leadInfoDocumentSearch.ExpectedSigningDate.HasValue, String.Format("{0:d}", leadInfoDocumentSearch.ExpectedSigningDate), "None")}
                                                })
                    End If
                End If

            End If
            leadInfoDocumentSearch.UpdateBy = HttpContext.Current.User.Identity.Name
            leadInfoDocumentSearch.UpdateDate = Date.Now
            Try
                db.SaveChanges()
            Catch ex As DbUpdateException

            End Try

            Return CreatedAtRoute("DefaultApi", New With {.id = leadInfoDocumentSearch.BBLE}, leadInfoDocumentSearch)
        End Function

        ' DELETE: api/LeadInfoDocumentSearches/5
        <ResponseType(GetType(LeadInfoDocumentSearch))>
        Function DeleteLeadInfoDocumentSearch(ByVal id As String) As IHttpActionResult
            Dim leadInfoDocumentSearch As LeadInfoDocumentSearch = db.LeadInfoDocumentSearches.Find(id)
            If IsNothing(leadInfoDocumentSearch) Then
                Return NotFound()
            End If

            db.LeadInfoDocumentSearches.Remove(leadInfoDocumentSearch)
            db.SaveChanges()

            Return Ok(leadInfoDocumentSearch)
        End Function

        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If (disposing) Then
                db.Dispose()
            End If
            MyBase.Dispose(disposing)
        End Sub

        Private Function LeadInfoDocumentSearchExists(ByVal id As String) As Boolean
            Return db.LeadInfoDocumentSearches.Count(Function(e) e.BBLE = id) > 0
        End Function
    End Class
End Namespace