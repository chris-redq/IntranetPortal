Imports MyIdealProp.Workflow.Core.Model

Public Class TaskProcessAction

#Region "Line Rules"

    Public Shared Sub StartToMgrLineRule(ByRef line As LineInstance)
        Console.WriteLine("Line Rule executed.")
        line.LineRuleResult = True
    End Sub

    Public Shared Sub MgrResendLineRule(ByRef line As LineInstance)
        Console.WriteLine("Line Rule executed.")
        Dim result = line.ProcessInstance.GetDataFieldValue("Result").ToString
        line.LineRuleResult = (result = "Resend")
    End Sub

    Public Shared Sub CompletedLineRule(ByRef line As LineInstance)
        Console.WriteLine("Line Rule executed.")
        Dim result = line.ProcessInstance.GetDataFieldValue("Result").ToString
        line.LineRuleResult = (result = "Completed")
    End Sub
#End Region

#Region "Destination Rule"

    Public Shared Sub MgrDestinationRule(ByRef actInst As ActivityInstance)
        Dim mgr = actInst.ProcessInstance.GetDataFieldValue("Mgr").ToString
        If Not String.IsNullOrEmpty(mgr) Then
            For Each user In mgr.Split(";")
                If Not String.IsNullOrEmpty(user.Trim) Then
                    actInst.DestinationUsers.Add(user.Trim)
                End If
            Next
        End If
    End Sub

#End Region

#Region "Event Rules"

    'Maanger approval client event
    Public Shared Sub ManagerApproval(ByRef evtInst As EventInstance)
        Try
            Dim taskUrl = evtInst.ProcessInstance.GetDataFieldValue("TaskUrl").ToString
            evtInst.ItemData = String.Format("{1}?sn={0}", evtInst.ClientSeriesNumber, taskUrl)
        Catch ex As Exception
            evtInst.ItemData = String.Format("{1}?sn={0}", evtInst.ClientSeriesNumber, "/ViewLeadsInfo.aspx")
        End Try

        Try
            NotifyUser(evtInst)
        Catch ex As Exception

        End Try
    End Sub

    Public Shared Sub ProcessApproved(ByRef evtInst As EventInstance)

    End Sub
#End Region


#Region "Private Methods"

    Private Shared Sub NotifyUser(evtInst As EventInstance)
        'Send the new task notify
        Using client As New CommonService.CommonServiceClient
            For Each user In evtInst.ActivityInstance.DestinationUsers
                'Dim subject = "You have a New Task"
                'Dim msg = String.Format("{0} just submited a Leads Search Request ({1}). Please review.", evtInst.ProcessInstance.Originator, evtInst.ProcessInstance.GetDataFieldValue("SearchName"))
                'client.SendMessage(user, "New Task", msg, "", DateTime.Now, "System")

                Dim emailData As New Dictionary(Of String, String)
                emailData.Add("UserName", user)
                emailData.Add("Originator", evtInst.ProcessInstance.Originator)
                emailData.Add("TaskName", evtInst.ProcessInstance.DisplayName)
                emailData.Add("ItemData", evtInst.ItemData)
                emailData.Add("PortalLink", ProcessConfiguration.Settings("PortalLink"))
                client.SendEmailByTemplate(user, "NewTaskNotification", emailData)
            Next
        End Using
    End Sub
#End Region


End Class
