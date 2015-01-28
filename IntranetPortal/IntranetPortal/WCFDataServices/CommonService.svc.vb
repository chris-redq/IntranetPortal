﻿Imports System.IO

' NOTE: You can use the "Rename" command on the context menu to change the class name "CommonService" in code, svc and config file together.
' NOTE: In order to launch WCF Test Client for testing this service, please select CommonService.svc or CommonService.svc.vb at the Solution Explorer and start debugging.
Public Class CommonService
    Implements ICommonService



    Public Sub SendEmail(userName As String, subject As String, body As String) Implements ICommonService.SendEmail
        Dim emp = Employee.GetInstance(userName)
        IntranetPortal.Core.EmailService.SendMail(emp.Email, "", subject, body, Nothing)
    End Sub

    Public Sub SendMessage(userName As String, title As String, msg As String, bble As String, notifyTime As Date, createBy As String) Implements ICommonService.SendMessage
        UserMessage.AddNewMessage(userName, title, msg, bble, notifyTime, createBy)
    End Sub

    Public Sub SendEmailByTemplate(userName As String, templateName As String, emailData As Dictionary(Of String, String)) Implements ICommonService.SendEmailByTemplate
        Dim emp = Employee.GetInstance(userName)

        If emp IsNot Nothing AndAlso Not String.IsNullOrEmpty(emp.Email) Then
            IntranetPortal.Core.EmailService.SendMail(emp.Email, "", templateName, emailData)
        End If
    End Sub

    Public Sub UpdateLeadsSearchStatus(leadsSearchId As Integer, status As Integer) Implements ICommonService.UpdateLeadsSearchStatus
        Using ctx As New Entities
            Dim ls = ctx.LeadsSearchTasks.Find(leadsSearchId)
            ls.Status = status
            ctx.SaveChanges()
        End Using
    End Sub

    Public Sub SendTaskSummaryEmail(userName As String) Implements ICommonService.SendTaskSummaryEmail
        Dim emp = Employee.GetInstance(userName)
        IntranetPortal.Core.EmailService.SendMail(emp.Email, "", "Task Summary", LoadSummaryEmail(userName), Nothing)
    End Sub

    Private Function LoadSummaryEmail(userName As String) As String
        Dim ts As TaskSummary
        Using Page As New Page
            ts = Page.LoadControl("~/EmailTemplate/TaskSummary.ascx")
            ts.DestinationUser = userName
            ts.BindData()

            Dim sb As New StringBuilder
            Using tw As New StringWriter(sb)
                Using hw As New HtmlTextWriter(tw)
                    ts.RenderControl(hw)
                End Using
            End Using

            Return sb.ToString
        End Using
    End Function
End Class
