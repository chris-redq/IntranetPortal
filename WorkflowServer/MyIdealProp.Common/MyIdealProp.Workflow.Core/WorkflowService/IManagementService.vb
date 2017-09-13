Imports System.ServiceModel

Namespace MyIdealProp.Workflow.Service
    ' NOTE: You can use the "Rename" command on the context menu to change the interface name "IManagementService" in both code and config file together.
    <ServiceContract()>
    Public Interface IManagementService

        <OperationContract()>
        Function GetAllErrorlogs() As List(Of MyIdealProp.Workflow.Core.Model.ErrorLog)

        <OperationContract()>
        Sub RetryError(errorId As Integer)

    End Interface
End Namespace
