﻿Imports System.Threading
Imports DevExpress.Web
Imports Newtonsoft.Json.Linq

Public Class ImportAgentData
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        BindAgents()
        BindGrid()
    End Sub

    Sub BindAgents()
        Using Context As New Entities
            cbAgents.DataSource = Context.Agent_Properties.Where(Function(a) a.Active = True).Select(Function(a) a.Agent_Name).Distinct.OrderBy(Function(em) em.ToString).ToList
            cbAgents.DataBind()

            cbImportAgent.DataSource = Context.Employees.Where(Function(emp) emp.Active = True Or emp.Name.EndsWith("Office")).ToList.OrderBy(Function(em) em.Name)
            cbImportAgent.DataBind()
            cbImportAgent.Items.Insert(0, New ListEditItem())

            cbEmpFrom.DataSource = Context.Employees.ToList.OrderBy(Function(em) em.Name)
            cbEmpFrom.DataBind()
            cbEmpFrom.Items.Insert(0, New ListEditItem())

            cbEmpTo.DataSource = Context.Employees.ToList.OrderBy(Function(em) em.Name)
            cbEmpTo.DataBind()
            cbEmpTo.Items.Insert(0, New ListEditItem())



            Dim LeadsStatus = System.Enum.GetNames(GetType(LeadStatus)).ToList
            LeadsStatus.Insert(0, "")
            cbStatusToChange.DataSource = LeadsStatus
            cbStatusToChange.DataBind()
            cbStatusFrom.DataSource = LeadsStatus
            cbStatusFrom.DataBind()

        End Using
    End Sub

    Protected Sub btnLoad_Click(sender As Object, e As EventArgs)
        BindGrid()
    End Sub

    Sub BindGrid()
        Using Context As New Entities
            Dim agent = cbAgents.Text
            If Not String.IsNullOrEmpty(agent) Then
                gridLead.DataSource = Context.Agent_Properties.Where(Function(ap) ap.Agent_Name = agent And ap.BBLE IsNot Nothing And (ap.Active = True Or Not ap.Active.HasValue)).ToList
                gridLead.DataBind()
            End If
        End Using

    End Sub

    Protected Sub btnLoad2_Click(sender As Object, e As EventArgs)

        Dim agent = cbEmpFrom.Text
        Dim LoadLeads = New List(Of Lead)
        If Not String.IsNullOrEmpty(agent) Then
            Dim empId = CInt(cbEmpFrom.Value)

            If (String.IsNullOrEmpty(cbStatusFrom.Text)) Then
                LoadLeads = LoadAgentLeads(agent, empId)
            Else

                LoadLeads = LoadAgentLeads(agent, empId, ConvertLeadStatus(cbStatusFrom.Text))
            End If

        End If

        If (Not String.IsNullOrEmpty(BBLEList.Text)) Then
            LoadLeads = LoadBBLEsLeads(BBLEList.Text)

        End If

        If (cbLeadsCropOwned.Checked) Then
            LoadLeads = LoadAgentLeadsWithCorpOwned(LoadLeads)
        End If
        gridAgentLeads.DataSource = LoadLeads
        gridAgentLeads.DataBind()
    End Sub
    Function LoadBBLEsLeads(json As String) As List(Of Lead)
        Dim BBLes = JArray.Parse(json).ToArray
        Dim BBLEsStr As List(Of String) = New List(Of String)
        For Each b In BBLes
            Dim bStr As String = CStr(b).Trim
            BBLEsStr.Add(bStr)
        Next
        Using Context As New Entities
            Return Context.Leads.Where(Function(ap) BBLEsStr.Contains(ap.BBLE)).ToList

        End Using
    End Function
    Protected Function ConvertLeadStatus(status As String) As Integer
        Dim iStatus = CInt([Enum].Parse(GetType(LeadStatus), status))
        Return iStatus
    End Function

    Private Function LoadAgentLeads(name As String, empId As Integer) As List(Of Lead)
        Using Context As New Entities
            Return Context.Leads.Where(Function(ap) ap.EmployeeName = name And ap.EmployeeID = empId).ToList
        End Using
    End Function


    Function LoadAgentLeads(name As String, empId As Integer, Stauts As LeadStatus) As List(Of Lead)
        Return LoadAgentLeads(name, empId).Where(Function(l) l.Status = Stauts).ToList
    End Function

    Function LoadAgentLeadsWithCorpOwned(ls As List(Of Lead)) As List(Of Lead)
        Dim rls As List(Of Lead) = New List(Of Lead)
        If (ls IsNot Nothing) Then
            For Each l In ls
                Dim li = LeadsInfo.GetInstance(l.BBLE)

                If (Utility.IsCompany(li.Owner) Or Utility.IsCompany(li.CoOwner)) Then
                    rls.Add(l)
                End If

            Next
        End If
        Return rls

    End Function
    Protected Sub btnTransfer_Click(sender As Object, e As EventArgs)
        Using ctx As New Entities
            Dim agent = cbEmpFrom.Text
            Dim lList = New List(Of Lead)
            If (Not String.IsNullOrEmpty(BBLEList.Text)) Then
                lList = LoadBBLEsLeads(BBLEList.Text)
            End If

            If (lList.Count = 0) Then
                If String.IsNullOrEmpty(agent) Then
                    ASPxLabel2.Text = "Please select agent for transfer from."
                    Return
                End If

            End If

            If String.IsNullOrEmpty(cbEmpTo.Text) Then
                ASPxLabel2.Text = "Please select agent for transfer to."
                Return
            End If

            If Not String.IsNullOrEmpty(cbStatusToChange.Text) AndAlso ConvertLeadStatus(cbStatusToChange.Text) = LeadStatus.Callback Then
                If (deCallBackTime.Value Is Nothing Or deCallBackTime.Value <= Date.Now) Then
                    ASPxLabel2.Text = "When Transfer to call back need select call back date geater than now"
                    Return
                End If
            End If
            Dim keepStatus = CBool(cbKeepStatus.Value)


            Dim empId = CInt(cbEmpFrom.Value)

            If (lList.Count = 0) Then
                If (String.IsNullOrEmpty(cbStatusFrom.Text)) Then
                    lList = ctx.Leads.Where(Function(ap) ap.EmployeeName = agent AndAlso ap.EmployeeID = empId).ToList
                Else
                    Dim iStutsFrom = ConvertLeadStatus(cbStatusFrom.Text)
                    lList = ctx.Leads.Where(Function(le) le.EmployeeID = empId AndAlso le.Status = iStutsFrom).ToList()
                End If

                If Not String.IsNullOrEmpty(txtLeadsAmount.Text) Then
                    Dim count = CInt(txtLeadsAmount.Text)
                    lList = lList.Take(count).ToList
                End If
            Else
                Dim BBles = lList.Select(Function(b) b.BBLE).ToList
                lList = ctx.Leads.Where(Function(b) BBles.Contains(b.BBLE)).ToList
            End If

            If (cbLeadsCropOwned.Checked) Then
                lList = LoadAgentLeadsWithCorpOwned(lList)
            End If

            For Each ld In lList
                ld.EmployeeID = CInt(cbEmpTo.Value)
                ld.EmployeeName = cbEmpTo.Text

                If Not keepStatus Then

                    If (String.IsNullOrEmpty(cbStatusToChange.Text)) Then
                        ld.Status = LeadStatus.NewLead
                    Else
                        ld.Status = ConvertLeadStatus(cbStatusToChange.Text)
                    End If
                End If
                If (ld.Status = LeadStatus.NewLead) Then
                    ld.AssignDate = Date.Now
                End If

                If (ld.Status = LeadStatus.Callback AndAlso deCallBackTime.Value IsNot Nothing) Then
                    ld.CallbackDate = deCallBackTime.Value
                End If
                Dim strtoStatus = If(String.IsNullOrEmpty(cbStatusToChange.Text), "", "in status " + cbStatusToChange.Text)
                Dim strFormStatus = If(String.IsNullOrEmpty(cbStatusFrom.Text), "", "in status " + cbStatusFrom.Text)
                LeadsActivityLog.AddActivityLog(DateTime.Now, "Transfer form: " & cbEmpFrom.Text & " " & strFormStatus & " to: " & cbEmpTo.Text & strtoStatus, ld.BBLE, LeadsActivityLog.LogCategory.Status.ToString, Nothing, Page.User.Identity.Name, LeadsActivityLog.EnumActionType.Reassign)

                If ld.Status = LeadStatus.NewLead Then
                    LeadsStatusLog.AddNewEntity(ld.BBLE, LeadsStatusLog.LogType.NewLeads, ld.EmployeeName, Page.User.Identity.Name, agent, ctx)
                End If

            Next

            ctx.SaveChanges()
        End Using

        ASPxLabel2.Text = "Transfer finished."
    End Sub

    Protected Sub btnImport_Click(sender As Object, e As EventArgs) Handles btnImport.Click
        Try
            Dim agent = cbAgents.Text
            Using Context As New Entities
                Dim bbles As New List(Of String)

                For Each prop In Context.Agent_Properties.Where(Function(ap) ap.Agent_Name = agent And ap.BBLE IsNot Nothing And (ap.Active = True Or Not ap.Active.HasValue))
                    Dim li = Context.LeadsInfoes.Where(Function(l) l.BBLE = prop.BBLE).SingleOrDefault
                    If li Is Nothing Then
                        li = New LeadsInfo
                        li.PropertyAddress = prop.Property_Address
                        li.BBLE = prop.BBLE
                        li.StreetName = prop.Out_Address
                        li.NeighName = prop.Out_City
                        li.State = prop.Out_State
                        li.ZipCode = prop.Out_Zipcode
                        li.CreateBy = Page.User.Identity.Name
                        li.CreateDate = DateTime.Now

                        If Not String.IsNullOrEmpty(prop.Type) Then
                            li.Type = li.GetLeadsType(prop.Type)
                        End If

                        If Context.LeadsInfoes.Local.Where(Function(tmp) tmp.BBLE = li.BBLE).Count = 0 Then
                            Context.LeadsInfoes.Add(li)
                            bbles.Add(prop.BBLE)
                        End If
                    Else
                        'Dim lead = Context.Leads.Where(Function(l) l.BBLE = prop.BBLE).SingleOrDefault
                        'If lead IsNot Nothing Then
                        '    Context.Leads.Remove(lead)
                        'End If
                    End If

                    prop.Active = False
                    Dim replaceExsited = chkReplace.Checked

                    If Not String.IsNullOrEmpty(cbImportAgent.Value) Then
                        Dim bble = prop.BBLE
                        Dim newlead = Context.Leads.Where(Function(ld) ld.BBLE = bble).SingleOrDefault
                        If newlead Is Nothing Then
                            newlead = New Lead() With {
                                              .BBLE = bble,
                                              .LeadsName = li.LeadsName,
                                              .Neighborhood = li.NeighName,
                                              .EmployeeID = CInt(cbImportAgent.SelectedItem.Value),
                                              .EmployeeName = cbImportAgent.SelectedItem.Text,
                                              .Status = LeadStatus.NewLead,
                                              .AssignDate = DateTime.Now,
                                              .AssignBy = User.Identity.Name
                                              }

                            If Context.Leads.Local.Where(Function(tmp) tmp.BBLE = bble).Count = 0 Then
                                Context.Leads.Add(newlead)
                            End If
                        Else
                            If replaceExsited Then
                                newlead.LeadsName = li.LeadsName
                                newlead.Neighborhood = li.NeighName
                                newlead.EmployeeID = CInt(cbImportAgent.SelectedItem.Value)
                                newlead.EmployeeName = cbImportAgent.SelectedItem.Text
                                newlead.Status = LeadStatus.NewLead
                                newlead.AssignDate = DateTime.Now
                                newlead.AssignBy = User.Identity.Name
                            End If
                        End If
                    End If
                Next

                Context.SaveChanges()
                lblMsg.Text = "Import Complete!"
            End Using
        Catch ex As Exception

            Dim msg = "Error: " & ex.Message
            If ex.InnerException IsNot Nothing Then
                msg += msg & ex.InnerException.Message

                If ex.InnerException.InnerException IsNot Nothing Then
                    msg += msg & ex.InnerException.InnerException.Message
                End If
            End If

            lblMsg.Text = msg
        End Try
    End Sub

    Protected Sub ASPxButton2_Click(sender As Object, e As EventArgs) Handles ASPxButton2.Click
        Dim bbles = LeadsData(cbLeadsType.Value).Select(Function(b) b.BBLE).ToArray

        If Not CBool(Application("InLoop")) Then

            Dim ctx = HttpContext.Current
            ctx.Application.Lock()
            ctx.Application("TotalCount") = bbles.Count
            ctx.Application("Processed") = 0
            ctx.Application("InLoop") = True
            ctx.Application.UnLock()

            For i = 0 To 1
                Dim TestThread As New System.Threading.Thread(New ThreadStart(Sub()
                                                                                  HttpContext.Current = ctx
                                                                                  InitialData(bbles, ctx.Application)
                                                                              End Sub))
                TestThread.Start()
            Next
        End If

        Dim script = "<script type=""text/javascript"">"
        script += "RefreshProgress(0);"
        script += "</script>"

        If Not Page.ClientScript.IsStartupScriptRegistered("Refresh") Then
            Page.ClientScript.RegisterStartupScript(Me.GetType, "Refresh", script)
        End If
    End Sub

    Public Shared Sub InitialData(bbles As String(), appState As HttpApplicationState)
        Dim count = 0

        While count < bbles.Length
            appState.Lock()
            count = CInt(appState("Processed"))
            appState("Processed") = count + 1
            appState.UnLock()

            If count >= bbles.Length Then
                Continue While
            End If

            Dim bble = bbles(count)
            Dim attemps = 0
InitialLine:
            attemps += 1
            Try

                'UpdatePropertyAddress(bble)
                Dim lead = LeadsInfo.GetInstance(bble)

                'If String.IsNullOrEmpty(lead.Owner) Then
                '    DataWCFService.UpdateAssessInfo(bble)
                'End If

                If String.IsNullOrEmpty(lead.Owner) Then
                    If DataWCFService.UpdateLeadInfo(bble, True, True, True, True, True, False, True) Then
                        UserMessage.AddNewMessage("Service Message", "Initial Data Message " & bble, String.Format("All BBLE: {0} data is loaded. ", bble), bble, DateTime.Now, "Initial Data")
                    End If
                Else
                    If Not lead.C1stMotgrAmt.HasValue Then
                        If DataWCFService.UpdateLeadInfo(bble, False, True, True, True, True, False, True) Then
                            UserMessage.AddNewMessage("Service Message", "Initial Data Message " & bble, String.Format("BBLE: {0} Morgatage data is loaded. ", bble), bble, DateTime.Now, "Initial Data")
                        End If
                    Else
                        If lead.IsUpdating Then
                            If DataWCFService.UpdateLeadInfo(bble, False, True, True, True, True, False, True) Then
                                UserMessage.AddNewMessage("Service Message", "Initial Data Message " & bble, String.Format("Refresh BBLE: {0} data is Finished. ", bble), bble, DateTime.Now, "Initial Data")
                            End If
                        Else
                            If Not lead.HasOwnerInfo Then
                                If DataWCFService.UpdateLeadInfo(bble, False, False, False, False, False, False, True) Then
                                    UserMessage.AddNewMessage("Service Message", "Initial Data Message " & bble, String.Format("Refresh BBLE: {0} homeowner info is finished.", bble), bble, DateTime.Now, "Initial Data")
                                End If
                            End If
                        End If
                    End If
                End If
                'Thread.Sleep(1000)
            Catch ex As Exception
                UserMessage.AddNewMessage("Service Error", "Initial Data Error " & bble & " Attemps: " & attemps, "Error: " & ex.Message & " StackTrace: " & ex.StackTrace, bble, DateTime.Now, "Initial Data")
                Select Case attemps
                    Case 1
                        Thread.Sleep(30000)
                    Case 2
                        Thread.Sleep(60000)
                    Case 3
                        Thread.Sleep(300000)
                    Case Else
                        Thread.Sleep(1000000)
                End Select

                GoTo InitialLine
            End Try
        End While

        appState.Lock()
        appState("InLoop") = False
        appState.UnLock()
    End Sub

    Public Shared Sub UpdatePropertyAddress(bble As String)
        Using Context As New Entities
            Dim ld = Context.LeadsInfoes.Find(bble)
            If ld IsNot Nothing Then
                ld.PropertyAddress = IntranetPortal.Core.PropertyHelper.BuildPropertyAddress(ld.Number, ld.StreetName, ld.Borough, ld.NeighName, ld.ZipCode)
                Context.SaveChanges()
            End If
        End Using
    End Sub

    Protected Sub ASPxButton1_Click(sender As Object, e As EventArgs) Handles ASPxButton1.Click
        gridNewLeads.DataBind()
    End Sub

    Public Function LeadsData(type As String) As List(Of LeadsInfo)
        Dim newLeads As New List(Of LeadsInfo)
        Using Context As New Entities

            Select Case type
                Case ""
                    newLeads = Context.LeadsInfoes.ToList
                Case "Unassign"
                    newLeads = Context.LeadsInfoes.Where(Function(ld) ld.Lead Is Nothing).ToList
                Case "New"
                    newLeads = Context.LeadsInfoes.Where(Function(ld) String.IsNullOrEmpty(ld.PropertyAddress)).ToList
                Case "HomeOwner"
                    newLeads = Context.LeadsInfoes.Where(Function(ld) String.IsNullOrEmpty(ld.Owner)).ToList
                Case "MotgrAmt"
                    newLeads = Context.LeadsInfoes.Where(Function(ld) ld.C1stMotgrAmt Is Nothing).OrderByDescending(Function(ld) ld.CreateDate).ToList
                Case "Existed"
                    newLeads = Context.LeadsInfoes.Where(Function(ld) Not String.IsNullOrEmpty(ld.Owner)).ToList
            End Select
        End Using
        Return newLeads
    End Function

    Protected Sub gridNewLeads_DataBinding(sender As Object, e As EventArgs)
        gridNewLeads.DataSource = LeadsData(cbLeadsType.Value)
    End Sub

    Protected Sub checkProgress_Callback(source As Object, e As DevExpress.Web.CallbackEventArgs)
        Application.Lock()
        Dim total = Application("TotalCount")
        Dim count = Application("Processed")
        Application.UnLock()

        e.Result = CDbl(count / total)
    End Sub








    Protected Sub exportLastLog_Click(sender As Object, e As EventArgs)
        BindLastLog()
        ASPxLoadLastLogExporter.WriteXlsxToResponse()
    End Sub

    Protected Sub LoadLastLog_Click(sender As Object, e As EventArgs)

        BindLastLog()
    End Sub
    Protected Sub BindLastLog()
        Using ctx As New Entities
            Dim logs = ctx.Leads_with_last_log.ToList()
            For Each log In logs

                If (Not String.IsNullOrEmpty(log.LastUpdateComments)) Then
                    log.LastUpdateComments = Regex.Replace(log.LastUpdateComments, "<[^>]*(>|$)", String.Empty)
                End If

            Next
            gridLastLogView.DataSource = logs
            gridLastLogView.DataBind()
        End Using
    End Sub

    Protected Sub LoadImportJson_Click(sender As Object, e As EventArgs)
        BindPaddingAssginLead()
    End Sub
    Sub BindPaddingAssginLead()
        Dim paddingAssginLeads = GetPaddingAssignLeads()
        If (paddingAssginLeads Is Nothing) Then
            ImportStauts.Text = "Please fill padding assgin Josn"
            Return
        End If
        Import2PaddingAssginGrid.DataSource = paddingAssginLeads.ToList().Select(Function(l) New With {.BBLE = l.Item("BBLE").ToString, .EmployeeName = l.Item("EmployeeName").ToString})
        Import2PaddingAssginGrid.DataBind()
    End Sub
    Protected Sub Import2PaddingBtn_Click(sender As Object, e As EventArgs)
        Dim paddingLeads = GetPaddingAssignLeads().Select(Function(l) New With {.BBLE = l.Item("BBLE").ToString, .EmployeeName = l.Item("EmployeeName").ToString}).Distinct()
        If (paddingLeads Is Nothing) Then
            ImportStauts.Text = "Please fill padding assgin Josn"
            Return
        End If
        Dim listNeedAdd = New List(Of PendingAssignLead)
        ImportStauts.Text = "Start transfter"

        Using ctx As New Entities

            For Each p In paddingLeads

                Dim l = New PendingAssignLead
                l.BBLE = p.BBLE
                If (Not Lead.InSystem(l.BBLE)) Then
                    l.EmployeeName = p.EmployeeName
                    Dim es = Employee.GetInstance(l.EmployeeName)
                    If (es Is Nothing) Then
                        Throw New Exception("Can't find emloyee " & l.EmployeeName & " please check !")
                    End If
                    l.CreateBy = "System Import"
                    l.CreateDate = DateTime.Now
                    l.Status = 0
                    'do not add duplicate
                    If (listNeedAdd.Where(Function(t) t.BBLE = l.BBLE)).FirstOrDefault Is Nothing Then
                        listNeedAdd.Add(l)
                    End If



                End If
               

            Next
            If (listNeedAdd.Count > 0) Then
                ctx.PendingAssignLeads.AddRange(listNeedAdd)
                ctx.SaveChanges()
            End If

        End Using

        ImportStauts.Text = "Transfer finished total " & listNeedAdd.Count & " Leads"
    End Sub
    Function GetPaddingAssignLeads() As JArray
        If (Not String.IsNullOrEmpty(ImportJson.Text)) Then
            Dim needImport = JArray.Parse(ImportJson.Text)
            If (cbNotShowExist.Checked) Then
                needImport = needImport.Values().Select(Function(l) Not Lead.InSystem(l.Item("BBLE")))
            End If
            Return needImport
        End If

        Return Nothing
    End Function

    Protected Sub cbNotShowExist_CheckedChanged(sender As Object, e As EventArgs)
        BindPaddingAssginLead()
    End Sub
End Class