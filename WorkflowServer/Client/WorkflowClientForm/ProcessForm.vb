Imports MyIdealProp.Workflow.Client

Public Class FrmProcessDetail

    Public Property DataFields As New Dictionary(Of String, Object)
    Public Property DisplayName As String
    Public Property Priority As String
    Public Property ActDataFields As New Dictionary(Of String, Object)

    Private Sub ProcessForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub btnCreate_Click(sender As Object, e As EventArgs) Handles btnCreate.Click
        DisplayName = txtProcessName.Text
        Priority = cbPriority.Text
        For Each row As DataGridViewRow In DataGridView1.Rows
            If row IsNot Nothing Then
                If row.Cells(0).Value IsNot Nothing Then
                    DataFields.Add(row.Cells(0).Value, If(row.Cells(1).Value, ""))
                End If
            End If
        Next

        For Each row As DataGridViewRow In gridActDataFields.Rows
            If row IsNot Nothing Then
                If row.Cells(0).Value IsNot Nothing Then
                    ActDataFields.Add(row.Cells(0).Value, If(row.Cells(1).Value, ""))
                End If
            End If
        Next

        DialogResult = Windows.Forms.DialogResult.OK
    End Sub

    Public Sub BindData(procInst As ProcessInstance)
        txtProcessName.Text = procInst.DisplayName
        txtOriginator.Text = procInst.Originator

        For Each df In procInst.DataFields
            DataGridView1.Rows.Add(df.Key, df.Value.ToString)
        Next
        btnCreate.Text = "Finish"
    End Sub

    Public Sub BindData(wli As WorklistItem)
        BindData(wli.ProcessInstance)
        Try
            For Each df In wli.ActivityDestinationInstance.DataFields
                gridActDataFields.Rows.Add(df.Key, df.Value.ToString)
            Next

        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
        End Try
    End Sub
End Class