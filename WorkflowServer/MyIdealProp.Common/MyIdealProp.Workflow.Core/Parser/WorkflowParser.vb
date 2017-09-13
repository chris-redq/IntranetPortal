Imports System.Collections.Generic
Imports System.Linq
Imports MyIdealProp.Workflow.Core.Model

Namespace MyIdealProp.Workflow.Core.Parser
    Friend MustInherit Class WorkflowParser(Of TSchemeMedium As Class)
        Implements IWorkflowParser(Of TSchemeMedium)

        Public MustOverride Function ParseActors(schemeMedium As TSchemeMedium) As List(Of DestinationDefinition)

        Public MustOverride Function ParseParameters(schemeMedium As TSchemeMedium) As List(Of DataFieldDefinition)

        Public MustOverride Function ParseCommands(schemeMedium As TSchemeMedium, parameterDefinitions As List(Of DataFieldDefinition)) As List(Of EventDefinition2)

        Public MustOverride Function ParseActions(schemeMedium As TSchemeMedium, parameterDefinitions As List(Of DataFieldDefinition)) As List(Of ActionDefinition)

        Public MustOverride Function ParseActivities(schemeMedium As TSchemeMedium, actionDefinitions As List(Of ActionDefinition)) As List(Of ActivityDefinition)

        Public MustOverride Function ParseTransitions(schemeMedium As TSchemeMedium, actorDefinitions As List(Of DestinationDefinition), commandDefinitions As List(Of EventDefinition2), actionDefinitions As List(Of ActionDefinition), activityDefinitions As List(Of ActivityDefinition)) As List(Of LineDefinition)

        Public MustOverride Function ParseDesignerModel(schemeMedium As TSchemeMedium) As String

        Public MustOverride Function GetProcessName(schemeMedium As TSchemeMedium) As String

        Public Function Parse(schemeMedium As TSchemeMedium) As ProcessDefinition Implements IWorkflowParser(Of TSchemeMedium).Parse
            Dim actors = ParseActors(schemeMedium).ToList()
            Dim parameters = ParseParameters(schemeMedium).ToList()
            Dim commands = ParseCommands(schemeMedium, parameters).ToList()
            Dim actions = ParseActions(schemeMedium, parameters).ToList()
            Dim activities = ParseActivities(schemeMedium, actions).ToList()
            Dim transitions = ParseTransitions(schemeMedium, actors, commands, actions, activities).ToList()
            Dim designerModel = ParseDesignerModel(schemeMedium)

            Return ProcessDefinition.Create(GetProcessName(schemeMedium), actors, parameters, commands, actions, activities,
                transitions, designerModel)
        End Function
    End Class
End Namespace
