Imports MyIdealProp.Workflow.Core
Imports MyIdealProp.Workflow.Core.Runtime
Imports MyIdealProp.Workflow.Core.Model
Imports MyIdealProp.Workflow.DBPersistence

Public Class frmNewProcess

    Private runTime As WorkflowRuntime
    Private procInst As Model.ProcessInstance
    Private IsCreateNew As Boolean = True
    Private Sub frmNewProcess_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If IsCreateNew Then
            runTime = WorkflowServer.GetInstance
            'cbProcessList.DataSource = runTime.GetAllProcessScheme().Select(Function(fc) fc.Name).ToArray
        End If
    End Sub

    Public Property ProcessInstance As Model.ProcessInstance
        Get
            Return procInst
        End Get
        Set(value As Model.ProcessInstance)
            procInst = value
        End Set
    End Property

    Public Sub DataBind()
        txtProcessName.Text = procInst.DisplayName
        txtOriginator.Text = procInst.Originator
        GridControl1.DataSource = procInst.DataFields
        IsCreateNew = False
        'GridView1.OptionsBehavior.Editable = False
        'btnCreate.Visible = False
    End Sub

    Private Sub cbProcessList_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbProcessList.SelectedIndexChanged
        procInst = runTime.CreateProcessInstance(cbProcessList.Text)
        GridControl1.DataSource = procInst.DataFields
    End Sub

    Private Sub GridView1_CellValueChanging(sender As Object, e As DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs) Handles GridView1.CellValueChanged
        If e.Column.FieldName = "DisplayValue" Then
            Dim datafield = CType(GridView1.GetRow(e.RowHandle), DataFieldDefinitionWithValue)
            datafield.Value = e.Value
        End If
    End Sub

    Private Sub btnCreate_Click(sender As Object, e As EventArgs) Handles btnCreate.Click
        procInst.DisplayName = txtProcessName.Text
        procInst.Originator = txtOriginator.Text
        Me.DialogResult = Windows.Forms.DialogResult.OK
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
    End Sub
End Class