Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System

Namespace MyIdealProp.Workflow.Core.Model
    <Serializable>
    Public NotInheritable Class DataFieldDefinitionForAction
        Inherits DataFieldDefinition
        Public Overrides Property Name() As String
            Get
                Return ParameterDefinition.Name
            End Get
            Set(value As String)
                ParameterDefinition.Name = value
            End Set
        End Property

        Public Overrides Property Scope() As DataFieldScope
            Get
                Return ParameterDefinition.Scope
            End Get
            Friend Set(value As DataFieldScope)
                ParameterDefinition.Scope = value
            End Set
        End Property

        Public Overrides Property Type() As Type
            Get
                Return ParameterDefinition.Type
            End Get
            Friend Set(value As Type)
                ParameterDefinition.Type = value
            End Set
        End Property

        Public Overrides Property SerializedDefaultValue() As String
            Get
                Return ParameterDefinition.SerializedDefaultValue
            End Get
            Friend Set(value As String)
                ParameterDefinition.SerializedDefaultValue = value
            End Set
        End Property

        Public Property ParameterDefinition() As DataFieldDefinition
            Get
                Return m_ParameterDefinition
            End Get
            Friend Set(value As DataFieldDefinition)
                m_ParameterDefinition = value
            End Set
        End Property
        Private m_ParameterDefinition As DataFieldDefinition
        Public Property Order() As Integer
            Get
                Return m_Order
            End Get
            Friend Set(value As Integer)
                m_Order = value
            End Set
        End Property
        Private m_Order As Integer
    End Class
End Namespace
