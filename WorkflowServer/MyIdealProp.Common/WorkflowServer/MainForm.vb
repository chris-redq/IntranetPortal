Imports MyIdealProp.Workflow.Core
Imports MyIdealProp.Workflow.Core.Runtime
Imports MyIdealProp.Workflow.Core.Model

Public Class frmMain
    Dim runTime As Runtime.WorkflowRuntime

    Private Sub btnCreate_Click(sender As Object, e As EventArgs) Handles btnCreate.Click
        Dim frm As New frmNewProcess
        Dim result = frm.ShowDialog()
        If result = Windows.Forms.DialogResult.OK Then
            Dim procInst = frm.ProcessInstance
            runTime.StartProcessInstance(procInst)
        End If
        Return
    End Sub

    Private Sub frmMain_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        runTime = WorkflowServer.GetInstance
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles btnGetAllWorklist.Click
        If String.IsNullOrEmpty(txtLoginUser.Text) Then
            MessageBox.Show("Please input login users")
            Return
        End If

        Dim worklist = runTime.GetWorklist(txtLoginUser.Text)
        gridWorklist.DataSource = worklist
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles btnComplete.Click
        Dim worklistItem = CType(GridView1.GetRow(GridView1.GetSelectedRows().First), WorklistItem)
        runTime.OpenWorklistItem(worklistItem)
        Dim frm As New frmNewProcess
        frm.ProcessInstance = worklistItem.ProcessInstance
        frm.DataBind()
        If frm.ShowDialog() = Windows.Forms.DialogResult.OK Then
            runTime.CompleteWorklist(worklistItem)
        End If
    End Sub

    Private Sub Button1_Click_1(sender As Object, e As EventArgs) Handles btnRefresh.Click
        gridProcHistory.DataSource = MyIdealProp.Workflow.DBPersistence.WorkflowLog.GetAllProcessInstance()
    End Sub

    Private Sub GridView1_RowClick(sender As Object, e As DevExpress.XtraGrid.Views.Grid.RowClickEventArgs) Handles GridView1.RowClick
        Dim item = CType(GridView1.GetRow(e.RowHandle), WorklistItem)
        runTime.OpenWorklistItem(item)
        Dim frm As New frmNewProcess
        frm.ProcessInstance = item.ProcessInstance
        frm.DataBind()
        frm.ShowDialog()
    End Sub

    Private Sub btnAllErrors_Click(sender As Object, e As EventArgs) Handles btnAllErrors.Click
        gridErrors.DataSource = runTime.GetAllErrorLogs()
    End Sub

    Private Sub btnRetry_Click(sender As Object, e As EventArgs) Handles btnRetry.Click

        For Each row In GridView2.GetSelectedRows
            Dim errlog = CType(GridView2.GetRow(row), ErrorLog)
            runTime.RetryError(errlog.LogId)
        Next

        gridErrors.DataSource = runTime.GetAllErrorLogs
    End Sub

    Private Sub btnDeploy_Click(sender As Object, e As EventArgs) Handles btnDeploy.Click
        Dim procId = runTime.DeployProcess(txtNewProcName.Text, txtNewProcDisplayName.Text, txtNewProcDescription.Text, txtNewProcScheme.Text)
        MessageBox.Show("Deploy Successed. Process Id: " & procId)
    End Sub

    Private Sub frmMain_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        runTime.Close()
    End Sub
End Class
