Imports MyIdealProp.Workflow.Core.Model
Imports System

Namespace MyIdealProp.Workflow.Core.Cache
    Friend Interface IParsedProcessCache
        Sub Clear()

        Function GetProcessDefinitionBySchemeId(schemeId As Integer) As ProcessDefinition

        Sub AddProcessDefinition(schemeId As Integer, processDefinition As ProcessDefinition)
    End Interface
End Namespace
