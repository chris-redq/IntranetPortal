Public Class Form1

    Private Sub btnAllErrors_Click(sender As Object, e As EventArgs) Handles btnAllErrors.Click
        Using svr As New MngService.ManagementServiceClient
            GridControl1.DataSource = svr.GetAllErrorlogs()
        End Using
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim errlog = CType(GridView1.GetRow(GridView1.GetSelectedRows().First), MngService.ErrorLog)
        Using svr As New MngService.ManagementServiceClient
            svr.RetryError(errlog.LogId)

        End Using
    End Sub
End Class
