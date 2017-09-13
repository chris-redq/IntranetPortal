Imports MyIdealProp.Workflow.Core.Model

Public Class NewLeadsRequestAction

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

    Public Shared Sub MgrDestinationRule(ByRef actInst As ActivityInstance)
        Dim mgr = actInst.ProcessInstance.GetDataFieldValue("Mgr").ToString
        If Not String.IsNullOrEmpty(mgr.Trim) Then
            actInst.DestinationUsers.Add(mgr.Trim)
        End If
    End Sub

#End Region

#Region "Event Rules"

    'Maanger approval client event
    Public Shared Sub ManagerApproval(ByRef evtInst As EventInstance)
        evtInst.ItemData = String.Format("/ViewLeadsinfo.aspx?sn={0}", evtInst.ClientSeriesNumber)
    End Sub

    Public Shared Sub ProcessApproved(ByRef evtInst As EventInstance)

    End Sub

    Public Shared Sub ProcessDeclined(ByRef evtInst As EventInstance)

    End Sub

#End Region

End Class
