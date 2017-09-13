Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System

Namespace MyIdealProp.Workflow.Core.Runtime
    Public Interface IWorkflowRuleProvider
        Function CheckRule(processId As Guid, identityId As Guid, ruleName As String) As Boolean

        Function CheckRule(processId As Guid, identityId As Guid, ruleName As String, parameters As IDictionary(Of String, String)) As Boolean

        Function GetIdentitiesForRule(processId As Guid, ruleName As String) As IEnumerable(Of Guid)

        Function GetIdentitiesForRule(processId As Guid, ruleName As String, parameters As IDictionary(Of String, String)) As IEnumerable(Of Guid)
    End Interface
End Namespace
