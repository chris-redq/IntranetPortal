Imports MyIdealProp.Workflow.Client

Public Class FrmMain
    Private conn As Connection

    Private Sub btnCreate_Click(sender As Object, e As EventArgs) Handles btnCreate.Click
        Dim procFrm As New FrmProcessDetail
        OpenConnection()
        Dim procInst = conn.CreateProcessInstance(cbProcessName.Text)
        procFrm.BindData(procInst)

        If procFrm.ShowDialog = Windows.Forms.DialogResult.OK Then
            procInst.DisplayName = procFrm.DisplayName
            procInst.Priority = [Enum].Parse(GetType(ProcessPriority), procFrm.Priority)

            For Each item In procFrm.DataFields
                procInst.DataFields.Add(item.Key, item.Value)
            Next

            Dim procInstId = conn.StartProcessInstance(procInst)
            MessageBox.Show("Process Instance start success. ProcInstId: " & procInstId)
        End If
    End Sub

    Private Sub OpenConnection()
        conn = New Connection("localhost")
        conn.UserName = cbUser.Text
        conn.Integrated = True
        conn.Open()
    End Sub

    Private Sub FrmMain_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub btnGetAllWorklist_Click(sender As Object, e As EventArgs) Handles btnGetAllWorklist.Click
        OpenConnection()
        gridWorklist.DataSource = conn.OpenMyWorklist()
    End Sub

    Private Sub btnComplete_Click(sender As Object, e As EventArgs) Handles btnComplete.Click
        Dim sn = CType(GridView1.GetRow(GridView1.GetSelectedRows().First), WorklistItem).SeriesNumber
        OpenConnection()
        Dim wli = conn.OpenWorklistItem(sn)
        Dim frm As New FrmProcessDetail
        frm.BindData(wli)
        If frm.ShowDialog = Windows.Forms.DialogResult.OK Then
            For Each item In frm.DataFields
                wli.ProcessInstance.DataFields(item.Key) = item.Value
            Next

            For Each item In frm.ActDataFields
                wli.ActivityDestinationInstance.DataFields(item.Key) = item.Value
            Next

            wli.Finish()
        End If
    End Sub

    Private Sub btnExpired_Click(sender As Object, e As EventArgs) Handles btnExpired.Click
        Try
            For Each row In GridView1.GetSelectedRows()
                Dim sn = CType(GridView1.GetRow(row), WorklistItem).SeriesNumber
                OpenConnection()
                Dim wli = conn.OpenWorklistItem(sn)
                conn.ExpiredProcessInstance(wli.ProcInstId)
            Next

            MessageBox.Show("The process was expired.")
            OpenConnection()
            gridWorklist.DataSource = conn.OpenMyWorklist()
        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
        End Try

    End Sub
End Class
