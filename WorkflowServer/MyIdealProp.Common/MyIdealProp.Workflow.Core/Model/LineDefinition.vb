Imports System.Collections.Generic
Imports System

Namespace MyIdealProp.Workflow.Core.Model
    <Serializable>
    Friend Class LineDefinition
        Inherits BaseDefinition
        Public Property From() As ActivityDefinition
        Public Property [To]() As ActivityDefinition
        Public Property Classifier() As TransitionClassifier
        Public Property Trigger() As TriggerDefinition
        Public Property Condition() As ConditionDefinition
        Public Property CodeRule As ActionDefinition

        Public ReadOnly Property Restrictions() As IEnumerable(Of RestrictionDefinition)
            Get
                Return RestrictionsList
            End Get
        End Property

        Protected RestrictionsList As List(Of RestrictionDefinition)

        Public ReadOnly Property OnErrors() As IEnumerable(Of OnErrorDefinition)
            Get
                Return OnErrorsList
            End Get
        End Property

        Protected OnErrorsList As List(Of OnErrorDefinition)

        Friend Shared Function Create(name As String, from As ActivityDefinition, [to] As ActivityDefinition, codeRule As ActionDefinition) As LineDefinition

            Return New LineDefinition() With { _
                .Name = name, _
                .[To] = [to], _
                .From = from,
                .CodeRule = codeRule
            }
        End Function

        Public Shared Function Create(from As ActivityDefinition, [to] As ActivityDefinition) As LineDefinition
            Return New LineDefinition() With { _
                .Name = "Undefined", _
                .[To] = [to], _
                .From = from, _
                .Classifier = TransitionClassifier.NotSpecified, _
                .Condition = ConditionDefinition.Always, _
                .Trigger = TriggerDefinition.Auto, _
                .RestrictionsList = New List(Of RestrictionDefinition)(), _
                .OnErrorsList = New List(Of OnErrorDefinition)() _
            }
        End Function

        Public Sub AddRestriction(restriction As RestrictionDefinition)
            RestrictionsList.Add(restriction)
        End Sub

        Public Sub AddOnError(onError As OnErrorDefinition)
            OnErrorsList.Add(onError)
        End Sub
    End Class
End Namespace
