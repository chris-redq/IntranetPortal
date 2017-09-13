Imports System.Collections.Generic
Imports MyIdealProp.Workflow.Core.Cache
Imports MyIdealProp.Workflow.Core.Model
Imports System

Namespace MyIdealProp.Workflow.Core.Builder
    Friend Interface IWorkflowBuilder
        Function CreateNewProcess(processId As Integer, processName As String, parameters As IDictionary(Of String, IEnumerable(Of Object))) As ProcessInstance

        Function CreateNewProcessScheme(processId As Integer, processName As String, parameters As IDictionary(Of String, IEnumerable(Of Object))) As ProcessInstance

        Function GetProcessInstance(processId As Integer) As ProcessInstance

        Sub SetCache(cache As IParsedProcessCache)

        Sub RemoveCache()

        Function GetProcessScheme(processName As String) As ProcessDefinition

        Function GetProcessScheme(processName As String, parameters As IDictionary(Of String, IEnumerable(Of Object))) As ProcessDefinition

        Function GetAllProcessScheme() As List(Of ProcessDefinition)
    End Interface

End Namespace
