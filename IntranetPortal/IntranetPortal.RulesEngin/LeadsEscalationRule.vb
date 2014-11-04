﻿

Public Class LeadsEscalationRule
    Public Sub Execute(ld As Lead)
        For Each Rule In GetRule(ld)
            If Rule.IsDateDue(ld.LastUpdate2, ld) Then
                Rule.Execute(ld)
                Return
            End If
        Next
    End Sub

    Private Function GetRule(ld As Lead) As List(Of EscalationRule)
        Return TaskRules.Where(Function(r) r.Name = CType(ld.Status, LeadStatus).ToString).OrderByDescending(Function(r) r.Sequence).ToList
    End Function

    Private Shared Function TaskRules() As List(Of EscalationRule)
        Dim rules As New List(Of EscalationRule)
        rules.Add(New EscalationRule("NewLeads", "3.00:00:00",
                                     Sub(leads)
                                         Dim ld = CType(leads, Lead)
                                         UserMessage.AddNewMessage(ld.EmployeeName, "New Leads Handler Warning", String.Format("The new leads BBLE:{0} will be recycled.", ld.BBLE), ld.BBLE)
                                     End Sub,
                                     Function(leads)
                                         Dim ld = CType(leads, Lead)
                                         Return ld.LastUpdate2 <= ld.AssignDate
                                     End Function,
                                     1))

        rules.Add(New EscalationRule("NewLeads", "4.00:00:00",
                                     Sub(leads)
                                         Dim ld = CType(leads, Lead)
                                         'insert to assign leads folder of this team
                                         ld.ReAssignLeads(ld.Employee.Department & " Office")
                                     End Sub,
                                     Function(leads)
                                         Dim ld = CType(leads, Lead)
                                         Return ld.LastUpdate2 <= ld.AssignDate
                                     End Function, 2))

        rules.Add(New EscalationRule("CallBack", "24:00:00",
                                     Sub(leads)
                                         Dim ld = CType(leads, Lead)
                                         UserMessage.AddNewMessage(ld.EmployeeName, "You missed a Callback", String.Format("The Call back of Leads ({0}) need to hanlder. BBLE:{1}", ld.LeadsName, ld.BBLE), ld.BBLE)
                                     End Sub,
                                     Function(leads)
                                         Dim ld = CType(leads, Lead)
                                         Return ld.LastUpdate2 <= ld.CallbackDate
                                     End Function, 1,
                                     Function(leads)
                                         Dim ld = CType(leads, Lead)
                                         Return ld.CallbackDate
                                     End Function))

        rules.Add(New EscalationRule("CallBack", "2.00:00:00",
                                Sub(leads)
                                    Dim ld = CType(leads, Lead)
                                    'generate Urgent Task and include Manager and Agent
                                    Dim emps = ld.EmployeeName & ";" & ld.Employee.Manager
                                    Dim log As New ActivityLogs
                                    log.SetAsTask(emps, "Urgent", "Callback Due", String.Format("The leads {0} need callback.", ld.LeadsName), ld.BBLE, "System")
                                End Sub,
                                Function(leads)
                                    Dim ld = CType(leads, Lead)
                                    Return ld.LastUpdate2 <= ld.CallbackDate
                                End Function, 2,
                                Function(leads)
                                    Dim ld = CType(leads, Lead)
                                    Return ld.CallbackDate
                                End Function))

        rules.Add(New EscalationRule("CallBack", "3.00:00:00",
                                Sub(leads)
                                    Dim ld = CType(leads, Lead)
                                    'generate Urgent Task and include Manager and Agent
                                    ld.ReAssignLeads(ld.Employee.Department & " Office")
                                End Sub,
                                Function(leads)
                                    Dim ld = CType(leads, Lead)
                                    Return ld.LastUpdate2 <= ld.CallbackDate
                                End Function, 2,
                                Function(leads)
                                    Dim ld = CType(leads, Lead)
                                    Return ld.CallbackDate
                                End Function))

        rules.Add(New EscalationRule("Doorknock", "7.00:00:00",
                             Sub(leads)
                                 Dim ld = CType(leads, Lead)
                                 Dim emps = ld.EmployeeName & ";" & ld.Employee.Manager
                                 Dim log As New ActivityLogs
                                 log.SetAsTask(emps, "Urgent", "Doorknock Due", String.Format("The leads {0} need doorknock.", ld.LeadsName), ld.BBLE, "System")
                             End Sub, Function(leads)
                                          Return True
                                      End Function, 1))

        rules.Add(New EscalationRule("Doorknock", "10.00:00:00",
                          Sub(leads)
                              Dim ld = CType(leads, Lead)
                              ld.ReAssignLeads(ld.Employee.Department & " Office")
                          End Sub, Function(leads)
                                       Return True
                                   End Function, 2))

        'rules.Add(New EscalationRule("HotLeads", "2.00:00:00",
        '                           Sub(leads)
        '                               Dim ld = CType(leads, Lead)
        '                               Dim emps = ld.EmployeeName & ";" & ld.Employee.Manager
        '                               Dim log As New ActivityLogs
        '                               log.SetAsTask(emps, "Urgent", "Doorknock Due", String.Format("The leads {0} need doorknock.", ld.LeadsName), ld.BBLE, "System")
        '                           End Sub,
        '                           Function(leads)
        '                               Dim ld = CType(leads, Lead)
        '                               Return ld.Task Is Nothing And ld.Appointment Is Nothing
        '                           End Function, 2))

        rules.Add(New EscalationRule("HotLeads", "2.00:00:00",
                                   Sub(leads)
                                       Dim ld = CType(leads, Lead)
                                       Dim emps = ld.EmployeeName & ";" & ld.Employee.Manager
                                       Dim log As New ActivityLogs
                                       log.SetAsTask(emps, "Urgent", "HotLeads Due", String.Format("The hot leads {0} need take care.", ld.LeadsName), ld.BBLE, "System")
                                   End Sub,
                                   Function(leads)
                                       Dim ld = CType(leads, Lead)
                                       Return ld.Task Is Nothing And ld.Appointment Is Nothing
                                   End Function, 1))

        rules.Add(New EscalationRule("HotLeads", "4.00:00:00",
                               Sub(leads)
                                   Dim ld = CType(leads, Lead)
                                   ld.ReAssignLeads(ld.Employee.Department & " Office")
                               End Sub,
                               Function(leads)
                                   Dim ld = CType(leads, Lead)
                                   Return ld.Task Is Nothing And ld.Appointment Is Nothing
                               End Function, 2))

        Return rules
    End Function

    
End Class