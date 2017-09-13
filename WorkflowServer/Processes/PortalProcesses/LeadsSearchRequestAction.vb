Imports MyIdealProp.Workflow.Core.Model

Public Class LeadsSearchRequestAction

#Region "Line Rules"

    Public Shared Sub StartToMgrLineRule(ByRef line As LineInstance)
        Console.WriteLine("Line Rule executed.")
        line.LineRuleResult = True

    End Sub

    Public Shared Sub ApproveLineRule(ByRef line As LineInstance)
        Console.WriteLine("Line Rule executed.")
        Dim result = line.ProcessInstance.GetDataFieldValue("Result").ToString
        line.LineRuleResult = (result = "Approve")
    End Sub

    Public Shared Sub DeclineLineRule(ByRef line As LineInstance)
        Console.WriteLine("Line Rule executed.")
        Dim result = line.ProcessInstance.GetDataFieldValue("Result").ToString
        line.LineRuleResult = (result = "Decline")
    End Sub

#End Region

#Region "Destination Rule"

    'Add manager
    Public Shared Sub MgrDestinationRule(ByRef actInst As ActivityInstance)
        'Dim mgr = actInst.ProcessInstance.GetDataFieldValue("Mgr").ToString
        'If Not String.IsNullOrEmpty(mgr) Then
        '    actInst.DestinationUsers.Add(mgr)
        'End If
    End Sub

    'Add data upload
    Public Shared Sub UploadDataDestinationRule(ByRef actInst As ActivityInstance)
        'Dim mgr = actInst.ProcessInstance.GetDataFieldValue("Mgr").ToString
        'If Not String.IsNullOrEmpty(mgr) Then
        '    actInst.DestinationUsers.Add(mgr)
        'End If
    End Sub
#End Region

#Region "Event Rules"
    'Maanger approval client event
    Public Shared Sub ManagerApproval(ByRef evtInst As EventInstance)
        evtInst.ItemData = String.Format("/LeadsGenerator/LeadsSearchApproval.aspx?sn={0}", evtInst.ClientSeriesNumber)
        NotifyUser(evtInst)
    End Sub

    Public Shared Sub UploadDataAction(ByRef evtInst As EventInstance)
        evtInst.ItemData = String.Format("/LeadsGenerator/LeadsSearchUploadData.aspx?sn={0}", evtInst.ClientSeriesNumber)
        NotifyUser(evtInst)
    End Sub

    Public Shared Sub ProcessApproved(ByRef evtInst As EventInstance)
        Using client As New CommonService.CommonServiceClient
            Dim searchId = evtInst.ProcessInstance.GetDataFieldValue("SearchId")
            If searchId IsNot Nothing Then
                client.UpdateLeadsSearchStatus(CInt(searchId), 1)
            End If

            Dim title = String.Format("Leads Search Request of {0} is approved.", evtInst.ProcessInstance.GetDataFieldValue("SearchName"))
            Dim description = String.Format("Your {0} Please review.", title)
            client.SendMessage(evtInst.ProcessInstance.Originator, title, description, "", DateTime.Now, "System")

            Dim emailData As New Dictionary(Of String, String)
            emailData.Add("Originator", evtInst.ProcessInstance.Originator)
            emailData.Add("SearchName", evtInst.ProcessInstance.GetDataFieldValue("SearchName"))
            emailData.Add("ViewLink", ProcessConfiguration.Settings("PortalLink") & "/LeadsGenerator/LeadsGenerator.aspx?n=" & evtInst.ProcessInstance.GetDataFieldValue("SearchName"))

            Dim names As New List(Of String)
            names.Add(evtInst.ProcessInstance.Originator)
            names.Add("Isaac Aronov")
            names.Add("Jamie Ventura")
            names.Add("Ron Borovinsky")

            client.SendEmailByTemplate(String.Join(";", names.ToArray), "LeadsSearchApproval", emailData)
        End Using
    End Sub

    Public Shared Sub ProcessDeclined(ByRef evtInst As EventInstance)
        Using client As New CommonService.CommonServiceClient
            Dim searchId = evtInst.ProcessInstance.GetDataFieldValue("SearchId")
            If searchId IsNot Nothing Then
                client.UpdateLeadsSearchStatus(CInt(searchId), 2)
            End If

            Dim title = String.Format("Leads Search Request of {0} is Declined.", evtInst.ProcessInstance.GetDataFieldValue("SearchName"))
            Dim description = String.Format("Your {0} Please review.", title)
            client.SendMessage(evtInst.ProcessInstance.Originator, title, description, "", DateTime.Now, "System")

            Dim emailData As New Dictionary(Of String, String)
            emailData.Add("Originator", evtInst.ProcessInstance.Originator)
            emailData.Add("SearchName", evtInst.ProcessInstance.GetDataFieldValue("SearchName"))
            client.SendEmailByTemplate(evtInst.ProcessInstance.Originator, "LeadsSearchDecline", emailData)
        End Using
    End Sub

#End Region

#Region "Private Methods"

    Private Shared Sub NotifyUser(evtInst As EventInstance)
        'Send the new task notify
        Using client As New CommonService.CommonServiceClient
            For Each user In evtInst.ActivityInstance.DestinationUsers
                Dim subject = "New Task for Leads Search"
                Dim msg = String.Format("{0} just submited a Leads Search Request ({1}). Please review.", evtInst.ProcessInstance.Originator, evtInst.ProcessInstance.GetDataFieldValue("SearchName"))
                client.SendMessage(user, "New Task", msg, "", DateTime.Now, "System")

                Dim emailData As New Dictionary(Of String, String)
                emailData.Add("UserName", user)
                emailData.Add("Originator", evtInst.ProcessInstance.Originator)
                emailData.Add("SearchName", evtInst.ProcessInstance.GetDataFieldValue("SearchName"))
                emailData.Add("ItemData", evtInst.ItemData)
                emailData.Add("PortalLink", ProcessConfiguration.Settings("PortalLink"))
                client.SendEmailByTemplate(user, "LeadsSearchNotification", emailData)
            Next
        End Using
    End Sub
#End Region

End Class
