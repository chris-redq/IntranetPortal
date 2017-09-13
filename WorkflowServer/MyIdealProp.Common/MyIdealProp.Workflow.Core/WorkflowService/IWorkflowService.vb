Imports System.ServiceModel
Imports MyIdealProp.Workflow.Core.Model
Imports MyIdealProp.Workflow.Core.Runtime

Namespace MyIdealProp.Workflow.Service


    ' NOTE: You can use the "Rename" command on the context menu to change the interface name "IWorkflowService" in both code and config file together.
    <ServiceContract()>
    Public Interface IWorkflowService

        <OperationContract()>
        Function CreateProcessInstance(procName As String) As ProcessInstance

        <OperationContract>
        Function StartProcessInstance(procName As String, DisplayName As String, priority As ProcessInstance.ProcessPriority, dataFields As Dictionary(Of String, Object)) As ProcessInstance

        <OperationContract>
        Sub ExpiredProcessInstance(procInstId As Integer)

        <OperationContract>
        Function GetProcessInstancesByDataFields(procName As String, key As String, value As String) As List(Of Integer)

        <OperationContract()>
        Function GetWorkList() As List(Of WorklistItem)

        <OperationContract()>
        Sub CompleteWorkList(procInstId As Integer, actInstId As Integer, datafields As Dictionary(Of String, Object))

        <OperationContract()>
        Sub CompleteDestInstWorklist(procInstId As Integer, actInstId As Integer, datafields As Dictionary(Of String, Object), actDataFields As Dictionary(Of String, Object))

        '<OperationContract()>
        'Sub CompleteWorkList1(procInstId As Integer, actInstId As Integer, datafields As Dictionary(Of String, Object), actDataFields As Dictionary(Of String, Object))

        <OperationContract()>
        Function OpenWorkListItem(procInstId As Integer, actInstId As Integer) As WorklistItem

        <OperationContract>
        Function UpdateWorklistItem(procInstId As Integer, actInstId As Integer, dataFields As Dictionary(Of String, Object))

        <OperationContract()>
        Sub GotoActivity(procInstId As Integer, actName As String)

    End Interface
End Namespace