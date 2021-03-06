﻿Imports System.Threading
Imports IntranetPortal.Core
Imports System.ServiceModel
Imports System.ServiceModel.Description

''' <summary>
''' The Rule Engine Service
''' </summary>
Public Class RulesService
    Private Shared ServiceInstance As RulesService
    Private StateObjs As New List(Of StateObjClass)
    Public Delegate Sub StatusChange(sta As ServiceStatus)
    Public Event OnStatusChange As StatusChange
    Private serviceMode As RunningMode
    Public _rulesProvider As IRulesProvider

    Private _svrStatus As ServiceStatus
    Public Property Status As ServiceStatus
        Get
            Return _svrStatus
        End Get
        Set(value As ServiceStatus)
            _svrStatus = value
            RaiseEvent OnStatusChange(_svrStatus)
        End Set
    End Property

    ''' <summary>
    ''' Get Rule Engine singleton instance
    ''' </summary>
    ''' <returns>The Rule Service Object</returns>
    Public Shared Function GetInstance() As RulesService
        If ServiceInstance Is Nothing Then
            ServiceInstance = New RulesService

            ' load the rules settings
            If RulesEngineSection.Settings IsNot Nothing AndAlso RulesEngineSection.Settings.RulesProvider IsNot Nothing Then
                Dim rp = RulesEngineSection.Settings.RulesProvider
                If rp IsNot Nothing Then
                    Dim t = Type.GetType(rp.Type)
                    If t IsNot Nothing Then
                        ServiceInstance._rulesProvider = CType(Activator.CreateInstance(t), IRulesProvider)
                    End If
                End If
            End If
        End If

        Return ServiceInstance
    End Function

    ''' <summary>
    ''' Get Rule engine running mode
    ''' The running mode include: debug, release and trial.
    ''' </summary>
    ''' <returns></returns>
    Public Shared ReadOnly Property Mode As RunningMode
        Get
            Return GetInstance.serviceMode
        End Get
    End Property

    ''' <summary>
    ''' The State entry of Business Rule engine
    ''' </summary>
    Public Sub Start()
        If Status <> ServiceStatus.Running Then
            Status = ServiceStatus.Running

            ' setup service
            InitServiceMode()

            ' initial all business rules
            InitRules()

            ' host api service
            HostService()

            Log("Service is running")
            Log("Service Running Mode: " + Mode.ToString)

            ' start the rules
            For Each Rule In Rules
                Log("Initial Rule: " & Rule.RuleName)
                RunTimer(Rule)
                Log("Initial Rule (" & Rule.RuleName & ") Finished. ")
            Next

            Log("Service is Start")
        End If
    End Sub

    Public Property Rules As List(Of BaseRule)

    Protected Overridable Sub InitRules()
        If _rulesProvider IsNot Nothing Then
            Rules = _rulesProvider.GetRules()
            Return
        Else
            Rules = New List(Of BaseRule)
        End If

        ' test leads assign rules
        'Rules.Add(New LoopServiceRule() With {.ExecuteOn = TimeSpan.Parse("00:00:00"), .Period = TimeSpan.Parse("00:05:00"), .RuleName = "Data Loop Rule", .ExecuteNow = True, .ExecuteOnWeekend = True})
        'Rules.Add(New PendingAssignRule With {.ExecuteOn = TimeSpan.Parse("00:00:00"), .Period = TimeSpan.Parse("00:05:00"), .RuleName = "Import Pending Assign Rule", .ExecuteNow = True, .ExecuteOnWeekend = True})

        'Rules.Add(New RecycleProcessRule() With {.ExecuteOn = TimeSpan.Parse("19:00:00"), .Period = TimeSpan.Parse("1.0:0:0"), .RuleName = "Recycle Leads"})
        'Rules.Add(New CompleteTaskRule() With {.ExecuteOn = TimeSpan.Parse("20:00:00"), .Period = TimeSpan.Parse("1.0:0:0"), .RuleName = "Complete Leads Task"})
        Rules.Add(New LeadsAndTaskRule() With {.ExecuteOn = TimeSpan.Parse("19:30:00"), .Period = TimeSpan.Parse("1.0:0:0"), .RuleName = "Leads business rule"})
        Rules.Add(New AgentActivitySummaryRule() With {.ExecuteOn = TimeSpan.Parse("21:30:00"), .Period = TimeSpan.Parse("1.0:0:0"), .RuleName = "Team Activity Email Rule"})

        Rules.Add(New EmailSummaryRule() With {.ExecuteOn = TimeSpan.Parse("06:30:00"), .Period = TimeSpan.Parse("1.0:0:0"), .RuleName = "User Task Summary Rule"})
        Rules.Add(New LoopServiceRule() With {.ExecuteOn = TimeSpan.Parse("00:00:00"), .Period = TimeSpan.Parse("00:05:00"), .RuleName = "Data Loop Rule", .ExecuteNow = True, .ExecuteOnWeekend = True})
        Rules.Add(New PendingAssignRule With {.ExecuteOn = TimeSpan.Parse("00:00:00"), .Period = TimeSpan.Parse("00:05:00"), .RuleName = "Import Pending Assign Rule", .ExecuteNow = True, .ExecuteOnWeekend = True})

        'Rules.Add(New DOBComplaintsCheckingRule With {.ExecuteOn = TimeSpan.Parse("7:00:00"), .Period = TimeSpan.Parse("1.00:00:00"), .RuleName = "DOB Complaints refresh rule Morning", .ExecuteNow = False, .ExecuteOnWeekend = True, .SendingNotifyEmail = True})
        'Rules.Add(New DOBComplaintsCheckingRule With {.ExecuteOn = TimeSpan.Parse("19:00:00"), .Period = TimeSpan.Parse("1.00:00:00"), .RuleName = "DOB Complaints refresh rule at Evening", .ExecuteNow = False, .ExecuteOnWeekend = True})
        'Rules.Add(New DOBComplaintsCheckingRule With {.ExecuteOn = TimeSpan.Parse("13:00:00"), .Period = TimeSpan.Parse("1.00:00:00"), .RuleName = "DOB Complaints refresh rule at Noon", .ExecuteNow = False, .ExecuteOnWeekend = True})
        'Rules.Add(New DOBComplaintsCheckingRule With {.ExecuteOn = TimeSpan.Parse("23:00:00"), .Period = TimeSpan.Parse("1.00:00:00"), .RuleName = "DOB Complaints refresh rule at Night", .ExecuteNow = False, .ExecuteOnWeekend = True})

        'Legal
        'Rules.Add(New LegalFollowUpRule() With {.ExecuteOn = TimeSpan.Parse("07:00:00"), .Period = TimeSpan.Parse("1.0:0:0"), .RuleName = "Legal Follow up Rule"})

        'ShortSale
        Rules.Add(New ShortSaleActivityReportRule() With {.ExecuteOn = TimeSpan.Parse("10:00:00"), .Period = TimeSpan.Parse("1.0:0:0"), .RuleName = "ShortSale Activity Email Rule on 10am"})
        Rules.Add(New ShortSaleActivityReportRule() With {.ExecuteOn = TimeSpan.Parse("12:01:00"), .Period = TimeSpan.Parse("1.0:0:0"), .RuleName = "ShortSale Activity Email Rule on 12pm"})
        Rules.Add(New ShortSaleActivityReportRule() With {.ExecuteOn = TimeSpan.Parse("15:01:00"), .Period = TimeSpan.Parse("1.0:0:0"), .RuleName = "ShortSale Activity Email Rule on 3pm"})
        Rules.Add(New ShortSaleFollowUpRule() With {.ExecuteOn = TimeSpan.Parse("07:30:00"), .Period = TimeSpan.Parse("1.0:0:0"), .RuleName = "ShortSale Follow up Rule"})

        'Auction
        'Rules.Add(New AuctionNotifyRule() With {.ExecuteOn = TimeSpan.Parse("08:00:00"), .Period = TimeSpan.Parse("1.0:0:0"), .RuleName = "Auction Properties Daily Notify Rule"})

        Dim days = 7 - DateTime.Today.DayOfWeek
        'Rules.Add(New AuctionNotifyRule() With {.ExecuteOn = TimeSpan.Parse(String.Format("{0}.08:00:00", days)), .Period = TimeSpan.Parse("7.0:0:0"), .RuleName = "Auction Properties Weekly Notify Rule", .ExecuteOnWeekend = True, .IsWeekly = True})

        ' Auto Assign
        ' Rules.Add(New AutoAssignRule With {.ExecuteOn = TimeSpan.Parse(String.Format("16:00:00", days)), .Period = TimeSpan.Parse("1.0:0:0"), .RuleName = "Auto Sign Leads", .ExecuteNow = False})

        'New Offer
        Rules.Add(New NewOfferNotifyRule() With {.ExecuteOn = TimeSpan.Parse(String.Format("{0}.08:00:00", (days + 1) Mod 7)), .Period = TimeSpan.Parse("7.0:0:0"), .RuleName = "New Offer Accepted Weekly Notify Rule", .ExecuteOnWeekend = True})
        Rules.Add(New NewOfferNotifyRule() With {.ExecuteOn = TimeSpan.Parse("08:10:00"), .Period = TimeSpan.Parse("1.0:0:0"), .RuleName = "New Offer Accepted Weekly Notify Rule", .ExecuteOnWeekend = False, .IsWeekly = False})

        'Ecourt data update
        Rules.Add(New EcourtCasesUpdateRule With {.ExecuteOn = TimeSpan.Parse("12:00:00"), .Period = TimeSpan.Parse("1.00:00:00"), .RuleName = "Ecourt Data Syncing Rule", .ExecuteNow = False, .ExecuteOnWeekend = True})

        ' Dialer integration service
        'Rules.Add(New DialerIntegrationRule With {.ExecuteOn = TimeSpan.Parse("20:50:00"), .Period = TimeSpan.Parse("1.00:00:00"), .RuleName = "Dialer integration Rule", .ExecuteNow = False, .ExecuteOnWeekend = True})
    End Sub

    ''' <summary>
    ''' Stop the service
    ''' </summary>
    Public Sub StopService()
        ' Request Dispose of the timer object.
        If StateObjs.Count > 0 Then
            For Each obj In StateObjs
                obj.TimerCanceled = True
                obj.TimerReference.Dispose()
            Next
        End If
        host.Close()
    End Sub

    Private Sub RunTimer(rule As BaseRule, Optional stateObj As StateObjClass = Nothing)
        rule.Status = BaseRule.RuleStatus.Active

        If stateObj Is Nothing Then
            stateObj = New StateObjClass
            stateObj.Rule = rule
            StateObjs.Add(stateObj)
        End If

        stateObj.TimerCanceled = False

        Dim TimerDelegate As New System.Threading.TimerCallback(AddressOf TimerTask)

        Dim dueTime = DateTime.Today.Add(rule.ExecuteOn) - DateTime.Now
        If dueTime.TotalSeconds < 0 Then
            dueTime = DateTime.Today.AddDays(rule.Period.TotalDays).Add(rule.ExecuteOn) - DateTime.Now
        End If

        If rule.ExecuteNow Then
            dueTime = TimeSpan.Parse("00:00:01")
        End If

        Log("Due Time: " & dueTime.ToString & " Period: " & rule.Period.ToString)
        Dim timerItem = New System.Threading.Timer(TimerDelegate, stateObj, dueTime, rule.Period)

        ' Save a reference for Dispose.
        stateObj.TimerReference = timerItem
    End Sub

    Private Sub TimerTask(ByVal StateObj As Object)
        Dim State As StateObjClass = CType(StateObj, StateObjClass)

        If State.Rule.Status = BaseRule.RuleStatus.InProcess Then
            Log("Timer is cancel")
            Return
        End If

        If State.Rule Is Nothing Then
            Log("Business Rule is nothing. Please check.")
            Return
        End If

        Log("Launched rule Task: " & State.Rule.RuleName)
        State.Rule.Status = BaseRule.RuleStatus.InProcess

        Try
            If WorkingHours.IsWorkingDay(DateTime.Now) Then
                'Run Rules
                State.Rule.Execute()
            Else
                If State.Rule.ExecuteOnWeekend Then
                    State.Rule.Execute()
                End If
            End If
        Catch ex As Exception
            Log("Exception in Run Rules", ex)
        End Try

        Log("Rule Task " & State.Rule.RuleName & " is complete")

        'Dim dueTime = DateTime.Today.Add(State.Rule.ExecuteOn) - DateTime.Now
        'If dueTime.TotalSeconds < 0 Then
        '    dueTime = DateTime.Today.AddDays(1).Add(State.Rule.ExecuteOn) - DateTime.Now
        'End If

        'State.TimerReference.Change(dueTime, State.Rule.Period)

        State.Rule.Status = BaseRule.RuleStatus.Active

        If State.TimerCanceled Then
            ' Dispose Requested.
            DisposeTimer(State)
        End If
    End Sub

    ''' <summary>
    ''' Execute the specific rule
    ''' </summary>
    ''' <param name="ruleId">The Rule Id</param>
    Public Sub ExecuteRule(ruleId As String)
        Dim stateObj = StateObjs.SingleOrDefault(Function(s) s.Rule.RuleId.ToString = ruleId)
        TimerTask(stateObj)
    End Sub

    ''' <summary>
    ''' Start the business rule
    ''' </summary>
    ''' <param name="ruleId">The Rule Id</param>
    Public Sub StartRule(ruleId As String)
        Dim stateObj = StateObjs.SingleOrDefault(Function(s) s.Rule.RuleId.ToString = ruleId)
        If stateObj IsNot Nothing Then
            DisposeTimer(stateObj)
            RunTimer(stateObj.Rule, stateObj)
        End If
    End Sub

    ''' <summary>
    ''' Stop the given rule
    ''' </summary>
    ''' <param name="ruleId"></param>
    Public Sub StopRule(ruleId As String)
        Dim stateObj = StateObjs.SingleOrDefault(Function(s) s.Rule.RuleId.ToString = ruleId)

        If stateObj IsNot Nothing Then
            stateObj.Rule.Status = BaseRule.RuleStatus.Stoped
            stateObj.TimerCanceled = True

            If stateObj.TimerReference IsNot Nothing Then
                stateObj.TimerReference.Dispose()
            End If
        End If
    End Sub

    Private Sub DisposeTimer(stateObj As StateObjClass)
        If stateObj.TimerReference IsNot Nothing Then
            stateObj.TimerReference.Dispose()
            Log("Rule " & stateObj.Rule.RuleName & " is stoped.")
        End If
    End Sub

    <Obsolete>
    Private Sub RunRules()
        'Run Task rule
        Dim tasks = UserTask.GetActiveTasks()
        Log("Total Active Tasks: " & tasks.Count)

        'execute the rules
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

        'Run Leads Rule
        For Each ld In lds
            Try
                LeadsEscalationRule.Execute(ld)
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

        If WorkingHours.IsWorkingDay(DateTime.Now) Then
            Dim rules = AssignRule.GetAllRules()
            Log("Assign Rules count: " & rules.Count)
            'Run Assign Leads rule
            For Each Rule In rules
                Try
                    Log("Assign Rule: " & Rule.ToString)
                    If Not ServiceLog.Debug Then
                        Rule.Execute()
                    End If
                Catch ex As Exception
                    Log("Exception when execute assign Leads Rule. Exception: " & ex.Message, ex)
                End Try
            Next
            Log("Assign Rules Finished")
        Else
            Log("Assgin Rule is cancel, due to non-working day")
        End If

        'Log("Run Data loop service")
        'Try
        '    IntranetPortal.LeadsDataManage.LeadsDataService.GetInstance.DataLoop("New")
        'Catch ex As Exception
        '    Log("Exception when Data loop. Exception: " & ex.Message, ex)
        'End Try
        'Log("Data Loop service Started.")
    End Sub

    Private Sub Log(msg As String)
        ServiceLog.Log(msg)
    End Sub

    Private Sub Log(msg As String, ex As Exception)
        ServiceLog.Log(msg, ex)
    End Sub

    Public Sub InitServiceMode()
        Dim mode = System.Configuration.ConfigurationManager.AppSettings("ServiceMode")
        If Not String.IsNullOrEmpty(mode) Then
            serviceMode = [Enum].Parse(GetType(RunningMode), mode)
        End If
    End Sub
    Private host As ServiceHost
    Private Sub HostService()

        ' create the api host service
        host = New ServiceHost(GetType(RulesEngineServices))

        host = New ServiceHost(GetType(RulesEngineServices), New Uri("net.tcp://localhost:8001/RulesEngineService"))
        Dim smb = host.Description.Behaviors.Find(Of ServiceMetadataBehavior)()
        If smb Is Nothing Then
            smb = New ServiceMetadataBehavior
        End If

        smb.MetadataExporter.PolicyVersion = PolicyVersion.Policy15
        host.Description.Behaviors.Add(smb)

        Dim netTcp = New NetTcpBinding(SecurityMode.None, False)
        netTcp.CloseTimeout = TimeSpan.Parse("00:10:00")
        netTcp.OpenTimeout = TimeSpan.Parse("00:10:00")
        netTcp.SendTimeout = TimeSpan.Parse("00:10:00")
        netTcp.MaxBufferPoolSize = 2147483647
        netTcp.MaxBufferSize = 2147483647
        netTcp.MaxReceivedMessageSize = 2147483647
        netTcp.ReaderQuotas.MaxArrayLength = 2147483647
        netTcp.ReaderQuotas.MaxNameTableCharCount = 2147483647
        netTcp.ReaderQuotas.MaxStringContentLength = 2147483647
        netTcp.ReaderQuotas.MaxDepth = 2147483647
        netTcp.ReaderQuotas.MaxBytesPerRead = 2147483647

        Dim svrEndpoint As ServiceEndpoint = host.AddServiceEndpoint(GetType(IRulesEngineServices), netTcp, "")
        host.AddServiceEndpoint(ServiceMetadataBehavior.MexContractName, MetadataExchangeBindings.CreateMexTcpBinding(), "mex")

        host.Open()

        Log("Client Service Host is started.")
    End Sub

    ''' <summary>
    ''' The state object class used to exchange data between threads of timer task
    ''' </summary>
    Private Class StateObjClass
        ' Used to hold parameters for calls to TimerTask. 
        Public SomeValue As Integer
        Public TimerReference As System.Threading.Timer
        Public TimerCanceled As Boolean
        Public InProcess As Boolean
        Public Rule As BaseRule

    End Class

    Enum ServiceStatus
        Stopped
        Sleep
        Running
    End Enum

    Enum RunningMode
        Debug
        Trial
        Release
    End Enum
End Class

Public Interface IRulesProvider
    Function GetRules() As List(Of BaseRule)
End Interface
