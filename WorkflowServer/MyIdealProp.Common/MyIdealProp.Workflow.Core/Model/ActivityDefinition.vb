Imports System.Collections.Generic
Imports System

Namespace MyIdealProp.Workflow.Core.Model
    <Serializable>
    Public Class ActivityDefinition
        Inherits BaseDefinition
        Public Property State() As String
        Public Property IsInitial() As Boolean
        Public Property IsFinal() As Boolean
        Public Property IsForSetState() As Boolean
        Public Property IsAutoSchemeUpdate() As Boolean
        Public Property Implementation() As List(Of ActionDefinitionForActivity)
        Public Property PreExecutionImplementation() As List(Of ActionDefinitionForActivity)
        Public Property Destinations As List(Of DestinationDefinition)
        Public Property Escalations As List(Of EscalationDefinition)
        Public Property DataFields() As List(Of DataFieldDefinition)
        Public Property SuccessRule As SuccessRuleDefinition
        Public Property Events As List(Of ActivityEvent)

        Public ReadOnly Property HaveImplementation() As Boolean
            Get
                Return Implementation.Count > 0
            End Get
        End Property

        Public ReadOnly Property HavePreExecutionImplementation() As Boolean
            Get
                Return PreExecutionImplementation.Count > 0
            End Get
        End Property

        Public ReadOnly Property IsState() As Boolean
            Get
                Return Not String.IsNullOrEmpty(Name)
            End Get
        End Property

        Public Shared Function Create(name As String, stateName As String, isInitial As String, isFinal As String, isForSetState As String, isAutoSchemeUpdate As String) As ActivityDefinition
            Return New ActivityDefinition() With { _
                .IsFinal = Not String.IsNullOrEmpty(isFinal) AndAlso Boolean.Parse(isFinal), _
                .IsInitial = Not String.IsNullOrEmpty(isInitial) AndAlso Boolean.Parse(isInitial), _
                .IsForSetState = Not String.IsNullOrEmpty(isForSetState) AndAlso Boolean.Parse(isForSetState), _
                .IsAutoSchemeUpdate = Not String.IsNullOrEmpty(isAutoSchemeUpdate) AndAlso Boolean.Parse(isAutoSchemeUpdate), _
                .Name = name, _
                .State = stateName, _
                .Implementation = New List(Of ActionDefinitionForActivity)(), _
                .PreExecutionImplementation = New List(Of ActionDefinitionForActivity)(),
                .Events = New List(Of ActivityEvent)
            }
        End Function

        Public Sub AddEvent(evet As ActivityEvent)
            Events.Add(evet)
        End Sub

        Public Sub AddAction(action As ActionDefinitionForActivity)
            Implementation.Add(action)
        End Sub

        Public Sub AddPreExecutionAction(action As ActionDefinitionForActivity)
            PreExecutionImplementation.Add(action)
        End Sub
    End Class
End Namespace
