Imports System.Collections.Generic
Imports System.Linq
Imports MyIdealProp.Workflow.Core.Fault
Imports System.Globalization
Imports System.Xml.Linq
Imports System

Namespace MyIdealProp.Workflow.Core.Model
    <Serializable>
    Friend NotInheritable Class ProcessDefinition
        Inherits BaseDefinition

        Public Property Actors() As List(Of DestinationDefinition)
        Public Property DataFields() As List(Of DataFieldDefinition)

        Public Property Commands As List(Of EventDefinition2)
        Public Property Actions As List(Of ActionDefinition)
        Public Property Activities As List(Of ActivityDefinition)
        Friend Property Lines As List(Of LineDefinition)

        Public Property DesignerModel As String

        Public Sub New()
            Actors = New List(Of DestinationDefinition)()
            DataFields = New List(Of DataFieldDefinition)()
            Commands = New List(Of EventDefinition2)()
            Actions = New List(Of ActionDefinition)()
            Activities = New List(Of ActivityDefinition)()
            Lines = New List(Of LineDefinition)()
            DesignerModel = String.Empty
        End Sub

        Public ReadOnly Property InitialActivity() As ActivityDefinition
            Get
                Try
                    Dim initialActivity__1 = Activities.SingleOrDefault(Function(a) a.IsInitial)
                    If initialActivity__1 Is Nothing Then
                        Throw New InitialActivityNotFoundException()
                    End If
                    Return initialActivity__1
                Catch generatedExceptionName As InvalidOperationException
                    Throw
                End Try
            End Get
        End Property

        Public ReadOnly Property ParametersForSerialized() As DataFieldDefinition()
            Get
                If DataFields Is Nothing Then
                    Return Nothing
                End If

                Return DataFields.Where(Function(p) DefaultDefinitions.DefaultParameters.Count(Function(p1) p1.Name = p.Name) = 0).ToArray()
            End Get
        End Property

        Public Function FindActivity(name As String) As ActivityDefinition
            Dim activity = Activities.SingleOrDefault(Function(a) a.Name = name)
            If activity Is Nothing Then
                Throw New ActivityNotFoundException()
            End If
            Return activity
        End Function

        Public Function FindTransition(name As String) As LineDefinition
            Dim transition = Lines.SingleOrDefault(Function(a) a.Name = name)
            If transition Is Nothing Then
                Throw New TransitionNotFoundException()
            End If
            Return transition
        End Function

        'Get line rules
        Public Function GetPossibleTransitionsForActivity(activity As ActivityDefinition) As IEnumerable(Of LineDefinition)
            Return Lines.Where(Function(t) t.From Is activity)
        End Function

        Public Function GetCommandTransitions(activity As ActivityDefinition) As IEnumerable(Of LineDefinition)
            Return Lines.Where(Function(t) t.From Is activity AndAlso t.Trigger.Type = TriggerType.Command)
        End Function

        Public Function GetAllLinesForActivity(activity As ActivityDefinition) As IEnumerable(Of LineDefinition)
            Return Lines.Where(Function(t) t.From Is activity)
        End Function

        Public Function GetCommandTransitionForActivity(activity As ActivityDefinition, commandName As String) As IEnumerable(Of LineDefinition)
            Return Lines.Where(Function(t) t.From Is activity AndAlso t.Trigger.Type = TriggerType.Command AndAlso t.Trigger.Command.Name = commandName)
        End Function

        Public Function GetTimerTransitionForActivity(activity As ActivityDefinition) As IEnumerable(Of LineDefinition)
            Return Lines.Where(Function(t) t.From Is activity AndAlso t.Trigger.Type = TriggerType.Timer)
        End Function

        Public Shared Function Create(name As String, actors As List(Of DestinationDefinition), parameters As List(Of DataFieldDefinition), commands As List(Of EventDefinition2), actions As List(Of ActionDefinition), activities As List(Of ActivityDefinition),
            transitions As List(Of LineDefinition), designerModel As String) As ProcessDefinition
            Return New ProcessDefinition() With {
                .Actions = actions,
                .Activities = activities,
                .Actors = actors,
                .Commands = commands,
                .Name = name,
                .DataFields = parameters,
                .Lines = transitions,
                .DesignerModel = designerModel
            }
        End Function

        Public Function GetParameterDefinition(name As String) As DataFieldDefinition
            Return DataFields.[Single](Function(p) p.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase))
        End Function

        Public Function GetNullableParameterDefinition(name As String) As DataFieldDefinition
            Return DataFields.SingleOrDefault(Function(p) p.Name = name)
        End Function

        Public ReadOnly Property PersistenceParameters() As IEnumerable(Of DataFieldDefinition)
            Get
                Return DataFields.Where(Function(p) p.Scope = DataFieldScope.Persistence)
            End Get
        End Property

    End Class
End Namespace
