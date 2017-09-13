Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Xml.Linq
Imports MyIdealProp.Common
Imports MyIdealProp.Workflow.Core.Model
Imports MyIdealProp.Workflow.Core.Fault

Namespace MyIdealProp.Workflow.Core.Parser
    Friend Class XmlWorkflowParser
        Inherits WorkflowParser(Of XElement)


        Public Overrides Function ParseActors(schemeMedium As XElement) As List(Of DestinationDefinition)
            If schemeMedium Is Nothing Then
                Throw New ArgumentNullException("schemeMedium")
            End If

            Dim actorsElement = MyIdealProp.Common.Extensions.SingleOrDefault(schemeMedium, "Actors")

            If actorsElement Is Nothing Then
                Return New List(Of DestinationDefinition)()
            End If

            Dim actors = New List(Of DestinationDefinition)()

            Return actors
        End Function

        Public Overrides Function ParseParameters(schemeMedium As XElement) As List(Of DataFieldDefinition)
            If schemeMedium Is Nothing Then
                Throw New ArgumentNullException("schemeMedium")
            End If
            Dim parametersElement = MyIdealProp.Common.Extensions.SingleOrDefault(schemeMedium, "DataFields")

            Dim parameters = New List(Of DataFieldDefinition)()

            If parametersElement IsNot Nothing Then
                For Each element As XElement In parametersElement.Elements().ToList()
                    Dim parameterDefinition As DataFieldDefinition
                    If element.Attributes("Scope").Count() = 1 Then
                        parameterDefinition = DataFieldDefinition.Create(GetName(element), element.Attributes("Type").[Single]().Value, element.Attributes("Scope").[Single]().Value, GetDefaultValue(element))
                    Else
                        parameterDefinition = DataFieldDefinition.Create(GetName(element), element.Attributes("Type").[Single]().Value, GetDefaultValue(element))
                    End If

                    parameters.Add(parameterDefinition)
                Next
            End If

            'parameters.AddRange(DefaultDefinitions.DefaultParameters)

            Return parameters
        End Function

        Public Overrides Function ParseCommands(schemeMedium As XElement, parameterDefinitions As List(Of DataFieldDefinition)) As List(Of EventDefinition2)
            If schemeMedium Is Nothing Then
                Throw New ArgumentNullException("schemeMedium")
            End If
            If parameterDefinitions Is Nothing Then
                Throw New ArgumentNullException("parameterDefinitions")
            End If
            Dim commandsElement = MyIdealProp.Common.Extensions.SingleOrDefault(schemeMedium, "Commands")


            Dim commands = New List(Of EventDefinition2)()
            If commandsElement Is Nothing Then
                Return commands
            End If

            'Dim parameterDefinitionsList = parameterDefinitions.ToList()

            'For Each element As XElement In commandsElement.Elements().ToList()
            '    Dim command = EventDefinition2.Create(GetName(element))
            '    Dim el = element.Elements("InputParameters").SingleOrDefault()
            '    If el IsNot Nothing Then
            '        For Each xmlInputParameter As XElement In el.Elements()
            '            Dim parameterRef = parameterDefinitionsList.FirstOrDefault(Function(pd) pd.Name = GetNameRef(xmlInputParameter))
            '            If parameterRef Is Nothing Then
            '                Throw New SchemeNotValidException(String.Format("Command {0}: parameter '{1}' not found", command.Name, GetNameRef(xmlInputParameter)))
            '            End If
            '            command.AddParameterRef(GetName(xmlInputParameter), parameterRef)
            '        Next
            '    End If

            '    commands.Add(command)
            'Next

            Return commands
        End Function

        Public Overrides Function ParseActions(schemeMedium As XElement, parameterDefinitions As List(Of DataFieldDefinition)) As List(Of ActionDefinition)
            If schemeMedium Is Nothing Then
                Throw New ArgumentNullException("schemeMedium")
            End If
            If parameterDefinitions Is Nothing Then
                Throw New ArgumentNullException("parameterDefinitions")
            End If
            Dim actionElements = MyIdealProp.Common.Extensions.SingleOrDefault(schemeMedium, "Actions")

            Dim actions = New List(Of ActionDefinition)()
            If actionElements Is Nothing Then
                Return actions
            End If

            Dim parameterDefinitionsList = parameterDefinitions.ToList()



            For Each element As XElement In actionElements.Elements().ToList()
                If element.Elements("ExecuteMethod").Count() > 1 Then
                    Throw New SchemeNotValidException(String.Format("Action {0}: block may contain only one section ExecuteMethod", element.Name))
                End If

                Dim executeMethodElement = element.Elements("ExecuteMethod").[Single]()

                Dim typeName = [GetType](executeMethodElement)

                Dim action = ActionDefinition.Create(GetName(element), typeName, GetMethodName(executeMethodElement))

                actions.Add(action)
            Next

            Return actions
        End Function

        Public Overrides Function ParseActivities(schemeMedium As XElement, actionDefinitions As List(Of ActionDefinition)) As List(Of ActivityDefinition)
            If schemeMedium Is Nothing Then
                Throw New ArgumentNullException("schemeMedium")
            End If
            If actionDefinitions Is Nothing Then
                Throw New ArgumentNullException("actionDefinitions")
            End If
            Dim activitiesElements = MyIdealProp.Common.Extensions.SingleOrDefault(schemeMedium, "Activities")
            If activitiesElements Is Nothing Then
                Throw New ArgumentNullException("")
            End If

            Dim actionDefinitionsList = actionDefinitions.ToList()
            Dim activities = New List(Of ActivityDefinition)()

            For Each element As XElement In activitiesElements.Elements().ToList()
                Dim activity = ActivityDefinition.Create(GetName(element), Nothing, GetIsInitial(element), GetIsFinal(element), Nothing, Nothing)

                'Add Activity Data Fields
                Dim dfs = element.Elements("DataFields").ToList
                If dfs.Count > 0 Then
                    Dim parameters = New List(Of DataFieldDefinition)()

                    For Each ele As XElement In dfs.Elements().ToList()
                        Dim parameterDefinition As DataFieldDefinition
                        If ele.Attributes("Scope").Count() = 1 Then
                            parameterDefinition = DataFieldDefinition.Create(GetName(ele), ele.Attributes("Type").[Single]().Value, ele.Attributes("Scope").[Single]().Value, GetDefaultValue(ele))
                        Else
                            parameterDefinition = DataFieldDefinition.Create(GetName(ele), ele.Attributes("Type").[Single]().Value, GetDefaultValue(ele))
                        End If

                        parameters.Add(parameterDefinition)
                    Next

                    activity.DataFields = parameters
                Else
                    activity.DataFields = New List(Of DataFieldDefinition)
                End If

                'Add activity rules
                Dim rules = element.Elements("Rules").ToList()
                If rules.Count > 0 Then
                    'Add destination rule
                    Dim destinations = rules.Elements("DestinationRule").ToList

                    If destinations Is Nothing Then
                        activity.Destinations = New List(Of DestinationDefinition)()
                    Else
                        Dim actors = New List(Of DestinationDefinition)()

                        For Each ele As XElement In destinations.Elements().ToList()
                            Dim ruleName = GetCodeRule(ele)

                            Dim actor As DestinationDefinition
                            If Not String.IsNullOrEmpty(ruleName) Then
                                Dim codeRule = actionDefinitionsList.Single(Function(ad) ad.Name = ruleName)
                                actor = DestinationDefinition.CreateDestinationRule(GetDestTypeName(ele), GetName(ele), codeRule)
                            Else
                                actor = DestinationDefinition.CreateDestinationRule(GetDestTypeName(ele), GetName(ele))
                            End If

                            If actor IsNot Nothing Then
                                actors.Add(actor)
                            End If
                        Next

                        activity.Destinations = actors
                    End If

                    'Escalation Rule
                    Dim escalations = rules.Elements("EscalationRule").ToList
                    If escalations Is Nothing Then
                        activity.Escalations = New List(Of EscalationDefinition)()
                    Else
                        Dim escals = New List(Of EscalationDefinition)

                        For Each ele As XElement In escalations.Elements.ToList
                            Dim action = actionDefinitionsList.Single(Function(ad) ad.Name = GetSingleValue(ele, "Action"))
                            Dim escal = EscalationDefinition.Create(GetName(ele), GetSingleValue(ele, "DueTime"), action, CInt(GetSingleValue(ele, "Repeat")))
                            escals.Add(escal)
                        Next
                        activity.Escalations = escals
                    End If

                    'Success Rule
                    Dim successRule = rules.Elements("SuccessRule").ToList
                    If successRule Is Nothing Or successRule.Count = 0 Then
                        activity.SuccessRule = Nothing
                    Else
                        Dim ele = successRule.Single()
                        Dim codeRule = actionDefinitionsList.Single(Function(ad) ad.Name = GetCodeRule(ele))
                        activity.SuccessRule = SuccessRuleDefinition.Create(GetName(ele), codeRule)
                    End If
                End If

                Dim Events = element.Elements("Events").ToList()

                If Events.Count() > 0 Then
                    For Each xmlOutputParameter As XElement In Events.[Single]().Elements()
                        Dim nameRef As String = GetEventAction(xmlOutputParameter)
                        Dim evtName = GetName(xmlOutputParameter)
                        Dim actionRef As ActionDefinition = actionDefinitionsList.SingleOrDefault(Function(ad) ad.Name = nameRef)
                        If actionRef Is Nothing Then
                            Throw New KeyNotFoundException(String.Format("Action {0} not found", nameRef))
                        End If
                        activity.AddEvent(ActivityEvent.Create(evtName, actionRef, GetOrder(xmlOutputParameter), GetEventType(xmlOutputParameter)))
                    Next
                End If

                activities.Add(activity)
            Next

            Return activities
        End Function

        Public Overrides Function ParseTransitions(schemeMedium As XElement, actorDefinitions As List(Of DestinationDefinition), commandDefinitions As List(Of EventDefinition2), actionDefinitions As List(Of ActionDefinition), activityDefinitions As List(Of ActivityDefinition)) As List(Of LineDefinition)
            If schemeMedium Is Nothing Then
                Throw New ArgumentNullException("schemeMedium")
            End If
            If commandDefinitions Is Nothing Then
                Throw New ArgumentNullException("commandDefinitions")
            End If
            If actionDefinitions Is Nothing Then
                Throw New ArgumentNullException("actionDefinitions")
            End If
            If activityDefinitions Is Nothing Then
                Throw New ArgumentNullException("activityDefinitions")
            End If
            Dim transitionElements = MyIdealProp.Common.Extensions.SingleOrDefault(schemeMedium, "Lines")

            Dim transitions = New List(Of LineDefinition)()
            If transitionElements Is Nothing Then
                Return transitions
            End If

            Dim actionDefinitionsList = actionDefinitions.ToList()
            Dim activityDefinitionsList = activityDefinitions.ToList()

            For Each transitionElement As XElement In transitionElements.Elements().ToList()
                Dim fromActivity = activityDefinitionsList.[Single](Function(ad) ad.Name = GetFrom(transitionElement))
                Dim toActivity = activityDefinitionsList.[Single](Function(ad) ad.Name = GetTo(transitionElement))
                Dim codeRule = actionDefinitionsList.Single(Function(ad) ad.Name = GetCodeRule(transitionElement))

                Dim transition = LineDefinition.Create(GetName(transitionElement), fromActivity, toActivity, codeRule)
                transitions.Add(transition)
            Next

            Return transitions
        End Function

        Public Overrides Function ParseDesignerModel(schemeMedium As XElement) As String
            Dim item = MyIdealProp.Common.Extensions.SingleOrDefault(schemeMedium, "DesignerModel")
            If item Is Nothing Then
                Return String.Empty
            Else
                Return item.Value
            End If
        End Function

        Public Overrides Function GetProcessName(schemeMedium As XElement) As String
            Return GetName(schemeMedium)
        End Function

        Private Shared Function GetEventType(element As XElement) As String
            Return element.Name.LocalName
        End Function

        Private Shared Function GetName(element As XElement) As String
            Return GetSingleValue(element, "Name")
        End Function

        Private Shared Function GetValue(element As XElement) As String
            Return GetSingleValue(element, "Value")
        End Function

        Private Shared Function GetRuleName(element As XElement) As String
            Return GetSingleValue(element, "RuleName")
        End Function

        Private Shared Function GetOrder(element As XElement) As String
            Return GetSingleValue(element, "Order")
        End Function

        Private Overloads Shared Function [GetType](element As XElement) As String
            Return GetSingleValue(element, "Type")
        End Function

        Private Shared Function GetMethodName(element As XElement) As String
            Return GetSingleValue(element, "MethodName")
        End Function

        Private Shared Function GetTypeName(element As XElement) As String
            Return GetSingleValue(element, "ExceptionType")
        End Function

        Private Shared Function GetDestTypeName(element As XElement) As String
            Return GetSingleValue(element, "Type")
        End Function

        Private Shared Function GetId(element As XElement) As String
            Return GetSingleValue(element, "Id")
        End Function

        Private Shared Function GetEventAction(element As XElement) As String
            Return GetSingleValue(element, "EventAction")
        End Function

        Private Shared Function GetActionName(element As XElement) As String
            Return GetSingleValue(element, "ActionName")
        End Function

        Private Shared Function GetDefaultValue(element As XElement) As String
            Return GetSingleValue(element, "DefaultValue")
        End Function

        Private Shared Function GetPriority(element As XElement) As String
            Return GetSingleValue(element, "Priority")
        End Function

        Private Shared Function GetFrom(element As XElement) As String
            Return GetSingleValue(element, "From")
        End Function

        Private Shared Function GetTo(element As XElement) As String
            Return GetSingleValue(element, "To")
        End Function

        Private Shared Function GetCodeRule(element As XElement) As String
            Return GetSingleValue(element, "CodeRule")
        End Function

        Private Shared Function GetIsFinal(element As XElement) As String
            Return GetSingleValue(element, "IsFinal")
        End Function

        Private Shared Function GetIsInitial(element As XElement) As String
            Return GetSingleValue(element, "IsInitial")
        End Function

        Private Shared Function GetObjectName(element As XElement) As String
            Return GetSingleValue(element, "ObjectName")
        End Function

        Private Shared Function GetIsDefault(element As XElement) As String
            Return GetSingleValue(element, "IsDefault")
        End Function

        Private Shared Function GetIsExecuteImplementation(element As XElement) As String
            Return GetSingleValue(element, "IsExecuteImplementation")
        End Function

        Private Shared Function GetSingleValue(el As XElement, attName As String) As String
            Dim a = el.Attributes(attName).SingleOrDefault()
            Return If(a Is Nothing, Nothing, a.Value)
        End Function
    End Class
End Namespace
