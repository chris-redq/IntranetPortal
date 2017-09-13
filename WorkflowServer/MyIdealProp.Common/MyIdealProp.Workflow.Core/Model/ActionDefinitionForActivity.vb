Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System

Namespace MyIdealProp.Workflow.Core.Model
    <Serializable>
    Public Class ActionDefinitionForActivity
        Inherits ActionDefinition

        Public Overrides Property Name() As String
            Get
                Return ActionDefinition.Name
            End Get
            Set(value As String)
                ActionDefinition.Name = value
            End Set
        End Property

        Public Overrides ReadOnly Property InputParameters() As IEnumerable(Of DataFieldDefinitionForAction)
            Get
                Return ActionDefinition.InputParameters
            End Get
        End Property

        Public Overrides ReadOnly Property OutputParameters() As IEnumerable(Of DataFieldDefinitionForAction)
            Get
                Return ActionDefinition.OutputParameters
            End Get
        End Property

        Public Overrides Property MethodName() As String
            Get
                Return ActionDefinition.MethodName
            End Get
            Friend Set(value As String)
                ActionDefinition.MethodName = value
            End Set
        End Property

        Public Overrides Property Type() As Type
            Get
                Return ActionDefinition.Type
            End Get
            Friend Set(value As Type)
                ActionDefinition.Type = value
            End Set
        End Property

        Public Property ActionDefinition() As ActionDefinition
        Public Property Order() As Integer
        Public Property EventType As EventDefinitionType
    End Class

    Public Enum EventDefinitionType
        Client
        Server
    End Enum
End Namespace
