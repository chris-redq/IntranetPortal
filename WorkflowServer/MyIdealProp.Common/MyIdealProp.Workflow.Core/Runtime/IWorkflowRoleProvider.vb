Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System

Namespace MyIdealProp.Workflow.Core.Runtime
    Public Interface IWorkflowRoleProvider
        Function IsInRole(identityId As Guid, roleId As String) As Boolean
        Function GetAllInRole(roleId As String) As IEnumerable(Of Guid)
    End Interface
End Namespace
