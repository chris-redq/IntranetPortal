Imports System.Collections.Generic
Imports System.Xml.Linq
Imports MyIdealProp.Workflow.Core.Model

Namespace MyIdealProp.Workflow.Core.Parser
    Friend Interface IWorkflowParser(Of In TSchemeMedium As Class)
        Function Parse(schemeMedium As TSchemeMedium) As ProcessDefinition

    End Interface


End Namespace
