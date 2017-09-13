Imports MyIdealProp.Workflow.Core.Model

Public Class ProcessRules
    Public Shared Sub TestAction()
        Console.WriteLine("This is test action from ProcessRules.")
    End Sub

    Public Shared Sub ManagerApproval(ByRef eventInst As EventInstance)
        Console.WriteLine("This is {0} activity.", eventInst.ActivityName)
    End Sub

    Public Shared Sub ManagerApprovalSuccessRule(ByRef actInst As ActivityInstance)
        Dim count = actInst.ActivityDestinationInstances.Where(Function(actDest) actDest.GetDataFieldValue("Result").ToString = "Approve").Count

        If count = 2 Then
            actInst.SuccessRuleResult = True
        Else
            actInst.SuccessRuleResult = False
        End If
    End Sub

    Public Shared Sub MgrDestinationRule(ByRef actInst As ActivityInstance)
        actInst.DestinationUsers.Add("Tester")
    End Sub

    Public Shared Sub LineRule(ByRef line As LineInstance)
        Console.WriteLine("Line Rule executed.")
        line.LineRuleResult = True
    End Sub

    Public Shared Sub ApproveLineRule(ByRef line As LineInstance)
        Console.WriteLine("Approved Result")
        Dim result = line.ProcessInstance.GetDataFieldValue("Result").ToString
        line.LineRuleResult = (result = "Approve")
    End Sub

    Public Shared Sub DeclineLineRule(ByRef line As LineInstance)
        Console.WriteLine("Decline Result")
        Dim result = line.ProcessInstance.GetDataFieldValue("Result").ToString
        line.LineRuleResult = (result <> "Approve")
    End Sub

    Public Shared Sub MgrEscalationRule(ByRef actInst As ActivityInstance)
        Console.WriteLine("Escalation Rule")
        actInst.ProcessInstance.SetDataFieldValue("Result", "send Mail")
    End Sub
End Class