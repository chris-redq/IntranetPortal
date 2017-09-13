Imports MyIdealProp.Workflow.Core.Model

Public Class LegalRequestAction

#Region "Line Rules"

    Public Shared Sub DefaultLineRule(ByRef line As LineInstance)
        Console.WriteLine("Line Rule executed.")
        line.LineRuleResult = True
    End Sub

    Public Shared Sub BackToRearchRule(ByRef line As LineInstance)
        Console.WriteLine("Line Rule executed.")
        line.LineRuleResult = line.ProcessInstance.GetDataFieldValue("Result").ToString = "Back"
    End Sub

    Public Shared Sub AssignToAttorneyRule(ByRef line As LineInstance)
        Console.WriteLine("Line Rule executed.")
        line.LineRuleResult = line.ProcessInstance.GetDataFieldValue("Result").ToString <> "Back"
    End Sub

#End Region

#Region "Destination Rule"

    Public Shared Sub ManagerPreviewDestinationRule(ByRef actInst As ActivityInstance)
        Dim mgr = actInst.ProcessInstance.GetDataFieldValue("Mgr").ToString
        If Not String.IsNullOrEmpty(mgr.Trim) Then
            For Each user In mgr.Split(";")
                If Not String.IsNullOrEmpty(user.Trim) Then
                    actInst.DestinationUsers.Add(user.Trim)
                End If
            Next
        End If
    End Sub

    Public Shared Sub LegalResearchDestinationRule(ByRef actInst As ActivityInstance)
        Dim user = actInst.ProcessInstance.GetDataFieldValue("ResearchUser").ToString
        If Not String.IsNullOrEmpty(user.Trim) Then
            actInst.DestinationUsers.Add(user.Trim)
        End If
    End Sub

    Public Shared Sub ManagerAssignDestinationRule(ByRef actInst As ActivityInstance)
        Dim mgr = actInst.ProcessInstance.GetDataFieldValue("Mgr").ToString
        If Not String.IsNullOrEmpty(mgr.Trim) Then
            For Each user In mgr.Split(";")
                If Not String.IsNullOrEmpty(user.Trim) Then
                    actInst.DestinationUsers.Add(user.Trim)
                End If
            Next
        End If
    End Sub

    Public Shared Sub AttorneyHandleDestinationRule(ByRef actInst As ActivityInstance)
        Dim mgr = actInst.ProcessInstance.GetDataFieldValue("Attorney").ToString
        If Not String.IsNullOrEmpty(mgr.Trim) Then
            actInst.DestinationUsers.Add(mgr.Trim)
        End If
    End Sub

#End Region

#Region "Event Rules"

    'Maanger approval client event
    Public Shared Sub ManagerPreviewClientEvent(ByRef evtInst As EventInstance)
        evtInst.ItemData = String.Format("/LegalUI/LegalUI.aspx?MangerPreivew=true&sn={0}", evtInst.ClientSeriesNumber)

        Try
            NotifyUser(evtInst)
        Catch ex As Exception

        End Try
    End Sub

    Public Shared Sub LegalResearchClientEvent(ByRef evtInst As EventInstance)
        evtInst.ItemData = String.Format("/LegalUI/LegalUI.aspx?Agent=true&sn={0}", evtInst.ClientSeriesNumber)

        Try
            NotifyUser(evtInst)
        Catch ex As Exception

        End Try
    End Sub

    Public Shared Sub ManagerAssignClientEvent(ByRef evtInst As EventInstance)
        evtInst.ItemData = String.Format("/LegalUI/LegalUI.aspx?Agent=true&sn={0}", evtInst.ClientSeriesNumber)

        Try
            NotifyUser(evtInst)
        Catch ex As Exception

        End Try
    End Sub

    Public Shared Sub AttorneyHandleClientEvent(ByRef evtInst As EventInstance)
        evtInst.ItemData = String.Format("/LegalUI/LegalUI.aspx?Agent=true&Attorney=true&sn={0}", evtInst.ClientSeriesNumber)

        Try
            NotifyUser(evtInst)
        Catch ex As Exception

        End Try
    End Sub

    Public Shared Sub ClosedEvent(ByRef evtInst As EventInstance)
        Try
            'Send the new task notify
            Using client As New CommonService.CommonServiceClient

                'Dim subject = "You have a New Task"
                'Dim msg = String.Format("{0} just submited a Leads Search Request ({1}). Please review.", evtInst.ProcessInstance.Originator, evtInst.ProcessInstance.GetDataFieldValue("SearchName"))
                'client.SendMessage(user, "New Task", msg, "", DateTime.Now, "System")

                Dim mgr = evtInst.ProcessInstance.GetDataFieldValue("Mgr")

                Dim emailData As New Dictionary(Of String, String)
                emailData.Add("Manager", mgr)
                'emailData.Add("Originator", evtInst.ProcessInstance.Originator)
                emailData.Add("Attorney", evtInst.ProcessInstance.GetDataFieldValue("Attorney"))
                emailData.Add("CaseName", evtInst.ProcessInstance.DisplayName)
                emailData.Add("ViewLink", ProcessConfiguration.Settings("PortalLink") & "/LegalUI/LegalUI.aspx?bble=" & evtInst.ProcessInstance.GetDataFieldValue("BBLE"))
                client.SendEmailByTemplate(mgr, "LegalRequestFinished", emailData)
            End Using
        Catch ex As Exception
            Throw New Exception(ex.Message & ex.StackTrace)
        End Try

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
