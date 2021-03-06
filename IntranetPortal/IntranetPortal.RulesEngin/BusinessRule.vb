﻿Imports System.Threading
Imports MyIdealProp.Workflow.DBPersistence
Imports System.Runtime.Serialization
Imports System.Configuration

''' <summary>
''' The base rule definition
''' </summary>
<KnownType(GetType(LegalFollowUpRule))>
<KnownType(GetType(LeadsAndTaskRule))>
<KnownType(GetType(EmailSummaryRule))>
<KnownType(GetType(AgentActivitySummaryRule))>
<KnownType(GetType(ShortSaleActivityReportRule))>
<KnownType(GetType(LoopServiceRule))>
<KnownType(GetType(CompleteTaskRule))>
<KnownType(GetType(ExpiredAllReminderRule))>
<KnownType(GetType(RecycleProcessRule))>
<KnownType(GetType(PendingAssignRule))>
<KnownType(GetType(AssignLeadsRule))>
<KnownType(GetType(DOBComplaintsCheckingRule))>
<KnownType(GetType(ScanECourtsRule))>
<KnownType(GetType(ConstructionNotifyRule))>
<KnownType(GetType(LegalActivityReportRule))>
<KnownType(GetType(NoticeECourtRule))>
<KnownType(GetType(ShortSaleFollowUpRule))>
<KnownType(GetType(AuctionNotifyRule))>
<KnownType(GetType(NewOfferNotifyRule))>
<KnownType(GetType(EcourtCasesUpdateRule))>
<DataContract>
Public Class BaseRule
    <DataMember>
    Public Property RuleId As Guid
    <DataMember>
    Public Property RuleName As String
    <DataMember>
    Public Property ExecuteOn As TimeSpan
    <DataMember>
    Public Property Period As TimeSpan
    <DataMember>
    Public Property ExecuteNow As Boolean
    <DataMember>
    Public Property Status As RuleStatus = RuleStatus.Stoped

    <DataMember>
    Public Property ExecuteOnWeekend As Boolean

    Public Sub New()
        RuleId = Guid.NewGuid()
    End Sub

    Public Sub New(executeOn As TimeSpan, period As TimeSpan)
        Me.ExecuteOn = executeOn
        Me.Period = period
    End Sub

    Public Overridable Sub Execute()

    End Sub

    Protected Sub Log(msg As String)
        ServiceLog.Log(msg)
    End Sub

    Protected Sub Log(msg As String, ex As Exception)
        ServiceLog.Log(msg, ex)
    End Sub

    Enum RuleStatus
        Active
        InProcess
        Stoped
    End Enum

    Public ReadOnly Property RuleData As Rule
        Get
            Return New Rule With {
                .RuleId = RuleId,
                .RuleName = RuleName,
                .ExecuteNow = ExecuteNow,
                .ExecuteOn = ExecuteOn,
                .ExecuteOnWeekend = ExecuteOnWeekend,
                .Period = Period,
                .Status = Status
                }
        End Get
    End Property
End Class

<DataContract>
Public Class Rule
    <DataMember>
    Public Property RuleId As Guid
    <DataMember>
    Public Property RuleName As String
    <DataMember>
    Public Property ExecuteOn As TimeSpan
    <DataMember>
    Public Property Period As TimeSpan
    <DataMember>
    Public Property ExecuteNow As Boolean
    <DataMember>
    Public Property Status As BaseRule.RuleStatus = BaseRule.RuleStatus.Stoped
    <DataMember>
    Public Property ExecuteOnWeekend As Boolean

End Class

''' <summary>
''' The leads and task related rule
''' Notify user base on the active tasks and leads info
''' </summary>
Public Class LeadsAndTaskRule
    Inherits BaseRule

    Public Overrides Sub Execute()
        ' RunRules()
        RunNewRules()
    End Sub

    Private Sub RunNewRules()
        Dim ruleStartDate = (IntranetPortal.Core.PortalSettings.GetValue("LeadsRuleStartDate"))
        Dim dtStart As DateTime
        If ruleStartDate Is Nothing OrElse Not DateTime.TryParse(ruleStartDate, dtStart) Then
            Throw New Exception("The rule start date is not valid.")
        End If

        Dim lds = Lead.GetAllAgentActiveLeads(dtStart)

        Log("Total Active Leads: " & lds.Count)
        Dim NoRulesUser = IntranetPortal.Core.PortalSettings.GetValue("NoRulesUser")

        'Run Leads Rule
        For Each ld In lds
            Try
                If Not NoRulesUser.Contains(ld.EmployeeName) Then
                    LeadsEscalationRule.Execute(ld)
                End If
            Catch ex As Exception
                Log("Exception when execute Leads Rule. BBLE: " & ld.BBLE & ", Employee: " & ld.EmployeeName & ", Exception: " & ex.Message, ex)
            End Try
        Next
        Log("Leads Rule finished.")
    End Sub

    Private Sub RunRules()
        'Run Task rule
        Dim tasks = UserTask.GetActiveTasks()
        Log("Total Active Tasks: " & tasks.Count)

        For Each t In tasks
            Try
                TaskEscalationRule.Excute(t)
            Catch ex As Exception
                Log("Exception when execute task rule. BBLE: " & t.BBLE & ", Task Id: " & t.TaskID & ", Exception: " & ex.Message, ex)
            End Try
        Next
        Log("Task Rules Finished.")


        Dim lds = Lead.GetAllActiveLeads()
        Log("Total Active Leads: " & lds.Count)
        Dim NoRulesUser = IntranetPortal.Core.PortalSettings.GetValue("NoRulesUser")

        'Run Leads Rule
        For Each ld In lds
            Try
                If Not NoRulesUser.Contains(ld.EmployeeName) Then
                    LeadsEscalationRule.Execute(ld)
                End If
            Catch ex As Exception
                Log("Exception when execute Leads Rule. BBLE: " & ld.BBLE & ", Employee: " & ld.EmployeeName & ", Exception: " & ex.Message, ex)
            End Try
        Next
        Log("Leads Rule finished.")

        'Run deal leads recycle rules
        lds = Lead.GetAllDeadLeads()
        Log("Dead leads recycle rules")

        For Each ld In lds
            Try
                LeadsEscalationRule.Execute(ld)
            Catch ex As Exception
                Log("Exception when execute dead Leads Rule. BBLE: " & ld.BBLE & ", Exception: " & ex.Message, ex)
            End Try
        Next
        Log("Dead leads recycle rules finished.")

        'If WorkingHours.IsWorkingDay(DateTime.Now) Then
        '    Dim rules = AssignRule.GetAllRules()
        '    Log("Assign Rules count: " & rules.Count)
        '    'Run Assign Leads rule
        '    For Each Rule In rules
        '        Try
        '            Log("Assign Rule: " & Rule.ToString)
        '            If Not ServiceLog.Debug Then
        '                Rule.Execute()
        '            End If
        '        Catch ex As Exception
        '            Log("Exception when execute assign Leads Rule. Exception: " & ex.Message, ex)
        '        End Try
        '    Next
        '    Log("Assign Rules Finished")
        'Else
        '    Log("Assgin Rule is cancel, due to non-working day")
        'End If
    End Sub

End Class

''' <summary>
''' The user's daily summary email rule
''' The summary email is send every morning include the
''' follow ups, tasks and appointments
''' </summary>
Public Class EmailSummaryRule
    Inherits BaseRule

    Public Overrides Sub Execute()

        Dim emps = Employee.GetAllActiveEmps()
        Using client As New PortalService.CommonServiceClient
            For Each emp In emps

                Try
                    If HasTask(emp) Then
                        client.SendTaskSummaryEmail(emp)
                        Thread.Sleep(1000)
                    End If

                Catch ex As Exception
                    Log("Send Task Summary Email Error. Employee Name: " & emp, ex)
                End Try
            Next
        End Using
    End Sub

    Private Function HasTask(emp As String) As Boolean
        Dim wls = WorkflowService.GetUserWorklist(emp)
        If wls.Count > 0 Then
            Return True
        End If

        Dim apois = IntranetPortal.UserAppointment.GetMyTodayAppointments(emp)
        If apois.Count > 0 Then
            Return True
        End If

        Dim followUps = IntranetPortal.Lead.GetUserTodayFollowUps(emp)
        If followUps.Count > 0 Then
            Return True
        End If

        Return False
    End Function
End Class

''' <summary>
''' Agent Team Activity Rule, used to send activity rule every night.
''' Also with short sale summary, legal summary and Dead leads report
''' </summary>
Public Class AgentActivitySummaryRule
    Inherits BaseRule

    Public Overrides Sub Execute()
        Dim teams = IntranetPortal.Team.GetActiveTeams()

        Using client As New PortalService.CommonServiceClient
            For Each team In teams
                Try
                    client.SendTeamActivityEmail(team.Name)
                    Thread.Sleep(1000)
                Catch ex As Exception
                    Log("AgentActivitySummaryRule Error. TeamName: " & team.Name, ex)
                End Try
            Next

            Try
                client.SendShortSaleActivityEmail()
                'Disable user summary email
                'client.SendShortSaleUserSummayEmail()
            Catch ex As Exception
                Log("AgentActivitySummaryRule Error. TeamName: ShortSale", ex)
            End Try

            Try
                'client.SendLegalActivityEmail()
                client.SendUserActivitySummayEmail("Legal", DateTime.Today, DateTime.Today.AddDays(1))
            Catch ex As Exception
                Log("AgentActivitySummaryRule Error. TeamName: Legal", ex)
            End Try

            'send deal leads report 
            Try
                client.SendEmailByControl(Employee.CEO.Email, "Deadleads Report - " & DateTime.Today.ToShortDateString, "DeadleadsReport", Nothing)
            Catch ex As Exception
                Log("DeadLeads Report Error.", ex)
            End Try
        End Using
    End Sub
End Class

''' <summary>
''' Legal user activity report rule
''' The report is send every night.
''' </summary>
Public Class LegalActivityReportRule
    Inherits BaseRule

    Public Overrides Sub Execute()

        Using client As New PortalService.CommonServiceClient
            Try
                client.SendLegalActivityEmail()
                'client.SendUserActivitySummayEmail("Legal", DateTime.Today, DateTime.Today.AddDays(1))
            Catch ex As Exception
                Log("AgentActivitySummaryRule Error. TeamName: Legal", ex)
            End Try
        End Using
    End Sub
End Class

''' <summary>
''' ShortSale user activity Report used for morning and afternoon
''' </summary>
Public Class ShortSaleActivityReportRule
    Inherits BaseRule

    Public Overrides Sub Execute()
        Using client As New PortalService.CommonServiceClient

            Try
                client.SendShortSaleActivityEmail()
            Catch ex As Exception
                Log("ShortSaleActivityReportRule Error. TeamName: ShortSale", ex)
            End Try
        End Using
    End Sub
End Class

''' <summary>
''' The rule to prepare the leads data
''' This rule is running every 5 mins
''' </summary>
Public Class LoopServiceRule
    Inherits BaseRule

    Dim threadPools As New List(Of Thread)
    Dim rules As List(Of Core.DataLoopRule)

    Public Overrides Sub Execute()
        rules = IntranetPortal.Core.DataLoopRule.GetAllActiveRule

        If rules IsNot Nothing AndAlso rules.Count > 0 Then
            CurrentIndex = 0
            For i = 0 To 1
                Log("Thread " & i & " is starting.")
                Dim TestThread As New System.Threading.Thread(New ThreadStart(Sub()
                                                                                  InitialData()
                                                                              End Sub))
                threadPools.Add(TestThread)
                TestThread.Start()
                Log("Thread " & i & " is started.")
            Next

            If threadPools.Count > 0 Then
                For Each th In threadPools
                    th.Join()
                Next
            End If
        End If

        rules = IntranetPortal.Core.DataLoopRule.GetAllActiveRule
        If rules IsNot Nothing And rules.Count > 0 Then
            Execute()
        End If
    End Sub

    Dim CurrentIndex As Integer
    Public Sub InitialData()
        Dim index = 0
        While index < rules.Count
            index = CurrentIndex
            CurrentIndex = index + 1

            If index >= rules.Count Then
                Continue While
            End If

            Dim rule = rules(index)
            Dim attemps = 0
InitialLine:
            attemps += 1
            If attemps > 2 Then
                Continue While
            End If

            Try
                ExecuteDataloopRule(rule)
                'rule.Complete()
            Catch ex As Exception
                Log("Initial Data Error " & rule.BBLE & " Attemps: " & attemps, ex)
                Select Case attemps
                    Case 1
                        Thread.Sleep(30000)
                    Case 2
                        Thread.Sleep(60000)
                    Case 3
                        Thread.Sleep(300000)
                    Case 4
                        NotifyUserDataServiceIsOff()
                        Thread.Sleep(1000000)
                    Case Else
                        Thread.Sleep(1000000)
                End Select

                GoTo InitialLine
            End Try
        End While
    End Sub

    Private Sub ExecuteDataloopRule2(rule As Core.DataLoopRule)

    End Sub

    Private Sub ExecuteDataloopRule(rule As Core.DataLoopRule)
        'check if server is busy
        While DataWCFService.IsServerBusy
            Log("Server is Busy. Try 30s later.")
            Thread.Sleep(30000)
        End While

        If rule.LoopType >= 32 Then

            ExecuteDataloopRule2(rule)
            Return
        End If

        Dim bble = rule.BBLE
        Select Case rule.LoopType
            Case Core.DataLoopRule.DataLoopType.All
                'Dim callback = Sub()
                '                   DataWCFService.UpdateAssessInfo(bble)
                '               End Sub
                'Threading.ThreadPool.QueueUserWorkItem(callback)

                DataWCFService.UpdateAssessInfo(bble)
                rule.Complete(Core.DataLoopRule.DataLoopType.AllHomeOwner)
                Log("General Data is updated. BBLE: " & bble)

            'If DataWCFService.UpdateLeadInfo(bble, True, True, True, True, True, False, True) Then
            '    rule.Complete()
            '    Log("All Data is refreshed. BBLE: " & bble)
            'End If
            Case Core.DataLoopRule.DataLoopType.AllHomeOwner
                Try
                    If (DataWCFService.UpdateLeadInfo(bble, False, False, False, False, False, False, True)) Then
                        Log("Initial Data Message " & bble & String.Format(" Refresh BBLE: {0} homeowner info is finished.", bble))
                    Else
                        Log("Initial Homeowner failed. No homeowner info loaded. BBLE: " & bble)
                    End If

                Catch ex As Exception
                    Log("Initial Homeowner failed. BBLE:" & bble & " Exception: " & ex.Message)
                Finally
                    rule.Complete(Core.DataLoopRule.DataLoopType.AllMortgage)
                End Try
            Case Core.DataLoopRule.DataLoopType.Servicer
                DataWCFService.UpdateServicer(bble)
                DataWCFService.UpdateTaxLiens(bble)
                rule.Complete()
                Log("Initial Data Message " & bble & String.Format(". Servicer info and Taxlien BBLE: {0} data is Update. ", bble))
            Case Core.DataLoopRule.DataLoopType.GeneralData
                DataWCFService.UpdateAssessInfo(bble)
                rule.Complete()
                Log("Initial Data Message " & bble & String.Format(" General info BBLE: {0} data is Update. ", bble))
            Case Core.DataLoopRule.DataLoopType.HomeOwner
                If DataWCFService.UpdateLeadInfo(bble, False, False, False, False, False, False, True) Then
                    rule.Complete()
                    Log("Initial Data Message " & bble & String.Format(" Refresh BBLE: {0} homeowner info is finished.", bble))
                Else
                    rule.Complete()
                    Log("Initial Homeowner failed. No homeowene info loaded. BBLE: " & bble)
                End If
            Case Core.DataLoopRule.DataLoopType.Mortgage, Core.DataLoopRule.DataLoopType.AllMortgage
                If DataWCFService.UpdateLeadInfo(bble, False, True, True, True, True, True, False) Then
                    rule.Complete()
                    Log("Initial Data Message " & bble & String.Format(" BBLE: {0} Morgatage data is loaded. ", bble))
                Else
                    rule.Complete()
                    Log("Failed to Inital Mortgage Data. BBLE: " & bble)
                End If
            Case Core.DataLoopRule.DataLoopType.OnlyMortgage
                If DataWCFService.UpdateLeadInfo(bble, False, True, False, False, False, False, False) Then
                    rule.Complete()
                    Log("Initial Data Message " & bble & String.Format(" BBLE: {0} Morgatage Amount data is loaded.", bble))
                Else
                    rule.Complete()
                    Log("Failed to Inital Mortgage Amount Data. BBLE: " & bble)
                End If
            Case Core.DataLoopRule.DataLoopType.GeneDataAndMortgage
                If DataWCFService.UpdateLeadInfo(bble, True, True, True, True, True, True, False) Then
                    rule.Complete()
                    Log("Initial Data Message " & bble & String.Format(" BBLE: {0} Morgatage Amount data is loaded.", bble))
                Else
                    rule.Complete()
                    Log("Failed to Inital GeneData And Mortgages Amount Data. BBLE: " & bble)
                End If
            Case Else
                Dim lead = LeadsInfo.GetInstance(bble)
                If String.IsNullOrEmpty(lead.Owner) Then
                    If DataWCFService.UpdateLeadInfo(bble, True, True, True, True, True, True, True) Then
                        rule.Complete()
                        Log("All Data is refreshed. BBLE: " & bble)
                    End If
                Else
                    If Not lead.C1stMotgrAmt.HasValue Then
                        If DataWCFService.UpdateLeadInfo(bble, False, True, True, True, True, True, True) Then
                            rule.Complete()
                            Log("Initial Data Message " & bble & String.Format(" BBLE: {0} Morgatage data is loaded. ", bble))
                        End If
                    Else
                        If lead.IsUpdating Then
                            If DataWCFService.UpdateLeadInfo(bble, False, True, True, True, True, True, True) Then
                                rule.Complete()
                                Log("Initial Data Message " & bble & String.Format("Refresh BBLE: {0} data is Finished. ", bble))
                            End If
                        Else
                            If Not lead.HasOwnerInfo Then
                                If DataWCFService.UpdateLeadInfo(bble, False, False, False, False, False, False, True) Then
                                    rule.Complete()
                                    Log("Initial Data Message " & bble & String.Format(" Refresh BBLE: {0} homeowner info is finished.", bble))
                                End If
                            End If
                        End If
                    End If
                End If
        End Select
    End Sub

    Private Sub NotifyUserDataServiceIsOff()
        Using client As New PortalService.CommonServiceClient
            Dim emaildata As New Dictionary(Of String, String)
            emaildata.Add("UserName", "Steven")
            client.SendEmailByTemplate("Steven Wu", "LoopServiceNotAvaiable", emaildata)
        End Using
    End Sub

    'Enum LoopType
    '    GeneralData
    '    HomeOwner
    '    Mortgage
    'End Enum
End Class

''' <summary>
''' The rule is running along with LeadsAndTaskRule to automatically 
''' complete or expire the reminder and related task
''' </summary>
Public Class CompleteTaskRule
    Inherits BaseRule

    Public Overrides Sub Execute()

        Dim remiderInsts = WorkflowService.GetMyOriginated("Portal")

        For Each pInst In remiderInsts.Where(Function(p) p.ProcessName = "ReminderProcess" And p.Status = MyIdealProp.Workflow.DBPersistence.ProcessInstance.ProcessStatus.Active)
            Try
                Dim bble = pInst.GetDataFieldValue("BBLE").ToString
                Dim taskId = pInst.GetDataFieldValue("TaskId").ToString
                If Not (ExpiredReminderTask(bble, taskId, pInst.Id)) Then
                    Log("No Action on pInst: " & pInst.Id & " Process Name: " & pInst.DisplayName)
                End If
            Catch ex As Exception
                Log("Exception in Complete Task Rule. pInst: " & pInst.Id & " Process Name: " & pInst.DisplayName, ex)
            End Try
        Next

    End Sub

    Public Function ExpiredReminderTask(bble As String, taskId As Integer, pInstId As Integer) As Boolean
        Dim tk = UserTask.GetTaskById(taskId)

        If (CheckIfNeedExpired(bble, tk)) Then
            UserTask.ExpiredTask(taskId)
            WorkflowService.ExpiredReminderProcess(pInstId)

            Dim comments = String.Format("{0} is expired by System.", tk.Action)
            LeadsActivityLog.AddActivityLog(DateTime.Now, comments, bble, LeadsActivityLog.LogCategory.Status, Nothing, "Portal", LeadsActivityLog.EnumActionType.SetAsTask)

            Log(String.Format("Task is expired. BBLE: {0}, task Name: {1}, task user: {2}", bble, tk.Action, tk.EmployeeName))
            Return True
        End If

        Return False
    End Function

    Private Function CheckIfNeedExpired(bble As String, tk As UserTask) As Boolean

        If tk.Status = UserTask.TaskStatus.Active Then
            Dim logs = LeadsActivityLog.GetLastAgentAction(bble)

            If logs IsNot Nothing AndAlso logs.LogID > tk.LogID Then
                Return True
            End If

            Dim ld = Lead.GetInstance(bble)
            If ld Is Nothing Then
                Return True
            End If

            If Not tk.EmployeeName.ToLower.Contains(ld.EmployeeName.ToLower) Then
                Return True
            End If
        Else
            Return True
        End If

        Return False
    End Function
End Class

''' <summary>
''' The Rule to expired all the reminder rules
''' </summary>
Public Class ExpiredAllReminderRule
    Inherits BaseRule

    Public Overrides Sub Execute()

        Dim remiderInsts = WorkflowService.GetMyOriginated("Portal")

        For Each pInst In remiderInsts.Where(Function(p) p.ProcessName = "TaskProcess" And p.Status = MyIdealProp.Workflow.DBPersistence.ProcessInstance.ProcessStatus.Active)
            Try
                Dim bble = pInst.GetDataFieldValue("BBLE").ToString
                Dim taskId = pInst.GetDataFieldValue("TaskId").ToString
                If Not (ExpiredReminderTask(bble, taskId, pInst.Id)) Then
                    Log("No Action on pInst: " & pInst.Id & " Process Name: " & pInst.DisplayName)
                End If
            Catch ex As Exception
                Log("Exception in Complete Task Rule. pInst: " & pInst.Id & " Process Name: " & pInst.DisplayName, ex)
            End Try
        Next
    End Sub

    Public Function ExpiredReminderTask(bble As String, taskId As Integer, pInstId As Integer) As Boolean
        Dim tk = UserTask.GetTaskById(taskId)

        If (CheckIfNeedExpired(bble, tk)) Then
            UserTask.ExpiredTask(taskId)
            WorkflowService.ExpiredReminderProcess(pInstId)

            Dim comments = String.Format("{0} is expired by System.", tk.Action)
            LeadsActivityLog.AddActivityLog(DateTime.Now, comments, bble, LeadsActivityLog.LogCategory.SalesAgent.ToString, Nothing, "Portal", LeadsActivityLog.EnumActionType.SetAsTask)

            Log(String.Format("Task is expired. BBLE: {0}, task Name: {1}, task user: {2}", bble, tk.Action, tk.EmployeeName))
            Return True
        End If

        Return False
    End Function

    Private Function CheckIfNeedExpired(bble As String, tk As UserTask) As Boolean
        If tk.Status = UserTask.TaskStatus.Active Then
            Return True
        End If

        Return False
    End Function
End Class

Public Class CreateReminderBaseOnErrorProcess
    Inherits BaseRule

    Public Overrides Sub Execute()

        For Each pId In {4329, 4332, 4333}
            Dim procInstId = pId
            Dim procInst = WorkflowService.LoadProcInstById(procInstId)
            Dim bble = procInst.GetDataFieldValue("BBLE").ToString
            Dim taskId = procInst.GetDataFieldValue("TaskId")
            Dim mgr = procInst.GetDataFieldValue("Mgr").ToString
            Dim displayName = procInst.DisplayName
            WorkflowService.StartTaskProcess("ReminderProcess", displayName, taskId, bble, mgr, "Important")
            Log("Reminder Process is created. pId: " & pId)
        Next
    End Sub
End Class

''' <summary>
''' The rule to recycle the leads
''' </summary>
Public Class RecycleProcessRule
    Inherits BaseRule

    Public Overrides Sub Execute()
        Dim rleads = Core.RecycleLead.GetActiveRecycleLeads

        For Each rld In rleads
            ExecuteRecycle(rld)
        Next
    End Sub

    Public Sub ExecuteRecycle(rld As Core.RecycleLead)
        Try
            Dim ld = Lead.GetInstance(rld.BBLE)

            If ld.IsValidToRecycle Then
                'for now don not do real recycle.
                ld.Recycle("RecycleRule")

                rld.Recycle()
                Log("Leads is Recycled. BBLE: " & ld.BBLE)

                WorkflowService.ExpiredRecycleProcess(rld.BBLE)
            Else
                'Since user did action against this leads, the Recycle action expired
                WorkflowService.ExpiredRecycleProcess(rld.BBLE)
                rld.Expire()
                Log("Lead recycle is expired. BBLE: " & ld.BBLE)
            End If
        Catch ex As Exception
            Log("Error in recycle leads, BBLE: " & rld.BBLE, ex)
        End Try
    End Sub
End Class

Public Class RefreshDataRule
    Inherits BaseRule

    Dim rules As List(Of Core.DataLoopRule)
    Public Overrides Sub Execute()
        rules = IntranetPortal.Core.DataLoopRule.GetAllActiveRule

    End Sub

End Class

''' <summary>
''' The rule will prepare the leads data and assign to agents base on the configuration
''' This rule will running every 5 mins
''' </summary>
Public Class PendingAssignRule
    Inherits BaseRule

    Public Overrides Sub Execute()
        AssignLeads()
        MoveToDataLoop()
    End Sub

    Public Sub AssignLeads()
        Dim lds = PendingAssignLead.GetInLoopLeads()

        For Each ld In lds
            Dim li = LeadsInfo.GetInstance(ld.BBLE)
            If li IsNot Nothing Then
                If Not li.IsUpdating Then
                    Try
                        li.AssignTo(ld.EmployeeName, ld.CreateBy, False)

                        ld.Finish()
                    Catch ex As Exception
                        Log("Pending Assign leads error " & li.BBLE, ex)
                    End Try
                End If
            End If
        Next
    End Sub

    Public Sub MoveToDataLoop()
        Dim lds = PendingAssignLead.GetAllPendingLeads

        For Each ld In lds
            Try
                LeadsInfo.CreateLeadsInfo(ld.BBLE, ld.Type, ld.CreateBy)
            Catch ex As Exception
                Log("Create leads Info error " & ld.BBLE, ex)
            End Try

            Core.DataLoopRule.AddRules(ld.BBLE, Core.DataLoopRule.DataLoopType.All, "PendingAssignRule")
            ld.Update(PendingAssignLead.PendingStatus.InLoop)
        Next
    End Sub
End Class

''' <summary>
''' The rule to check properties' DOB complaints 
''' The rule will run 4 times a day to monitor the DOB complaints
''' Once the complaints was found, notify the property owner and related users
''' </summary>
Public Class DOBComplaintsCheckingRule
    Inherits BaseRule

    Public Property SendingNotifyEmail As Boolean
    Public Property IsTesting As Boolean

    Public Overrides Sub Execute()

        Dim props = Data.CheckingComplain.GetAllComplains

        Dim names As New List(Of String)
        Dim complaintServer = ConfigurationManager.AppSettings("DOBComplaintServer").ToString

        For Each prop In props

            While DataWCFService.IsServerBusy(complaintServer)
                Log("DOB Complaints Refresh: the server is busy. Will try 120s later.")
                Thread.Sleep(120000)
            End While

            If Not IsTesting Then
                prop.RefreshComplains("RuleEngine", complaintServer)
            End If

            If Not String.IsNullOrEmpty(prop.NotifyUsers) Then
                names.AddRange(prop.NotifyUsers.Split(New Char() {";"}, StringSplitOptions.RemoveEmptyEntries))
            End If

            Thread.Sleep(1000)
        Next

        If SendingNotifyEmail Then
            For Each name In names.Distinct.ToArray

                Try
                    Using client As New PortalService.CommonServiceClient


                        Dim mailData As New Dictionary(Of String, String)
                        mailData.Add("UserName", name)
                        client.SendEmailByTemplate(name, "ComplaintsRefreshed", mailData)
                        Thread.Sleep(1000)
                    End Using
                Catch ex As Exception
                    Log("Sending Notify Email " & name, ex)
                End Try
            Next
        End If
    End Sub
End Class

<DataContract>
Public Class LegalFollowUpRule
    Inherits BaseRule

    Public Overrides Sub Execute()

        Using client As New PortalService.CommonServiceClient

            Try
                client.LegalFollowUp()

            Catch ex As Exception
                Log("Legal Followup Rule Error", ex)
            End Try
        End Using
    End Sub
End Class

<DataContract>
Public Class ShortSaleFollowUpRule
    Inherits BaseRule

    Public Overrides Sub Execute()

        Using client As New PortalService.CommonServiceClient
            Try
                Dim users = ShortSaleManage.GetShortSaleUsers
                For Each ssUser In users
                    SendEmail(ssUser, PortalReport.CaseActivityData.ActivityType.ShortSale)

                    Thread.Sleep(1000)
                Next

                users = TitleManage.TitleUsers
                For Each tUsers In users
                    SendEmail(tUsers, PortalReport.CaseActivityData.ActivityType.Title)

                    Thread.Sleep(1000)
                Next
            Catch ex As Exception
                Log("ShortSale user followup rule error", ex)
            End Try
        End Using
    End Sub

    Private Sub SendEmail(userName As String, type As PortalReport.CaseActivityData.ActivityType)
        Dim emp = Employee.GetInstance(userName)
        If emp IsNot Nothing Then
            Dim params As New Dictionary(Of String, String)
            params.Add("Name", userName)
            params.Add("Type", type)
            Try
                Using client As New PortalService.CommonServiceClient
                    client.SendEmailByControl(emp.Email, "FollowUps Reminder - " & DateTime.Today.ToShortDateString, "ShortSaleUserFollowUp", params)
                End Using
            Catch ex As Exception
                Log("Send Followup email error in ShortSaleFollowUpRule", ex)
            End Try
        End If
    End Sub
End Class

