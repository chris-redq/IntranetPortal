Imports System
Imports System.Runtime.Serialization

Namespace MyIdealProp.Workflow.Core.Model
    <Serializable>
   <DataContract()>
    Public Class DataFieldDefinition
        Inherits BaseDefinition

        <IgnoreDataMember>
        Public Overridable Property Type() As Type
            Get
                Return m_Type
            End Get
            Friend Set(value As Type)
                m_Type = value
            End Set
        End Property
        Private m_Type As Type

        <IgnoreDataMember>
        Public Overridable Property Scope As DataFieldScope
            Get
                Return m_scope
            End Get
            Friend Set(value As DataFieldScope)
                m_scope = value
            End Set
        End Property
        Private m_scope As DataFieldScope

        <IgnoreDataMember>
        Public Overridable Property SerializedDefaultValue() As String
            Get
                Return m_SerializedDefaultValue
            End Get
            Friend Set(value As String)
                m_SerializedDefaultValue = value
            End Set
        End Property
        Private m_SerializedDefaultValue As String

        Public Shared Function Create(name As String, type__1 As String, purpose As String, serializedValue As String) As DataFieldDefinition
            Dim parsedPurpose As DataFieldScope
            [Enum].TryParse(purpose, True, parsedPurpose)
            Return New DataFieldDefinition() With { _
                .Name = name, _
                .Type = Type.[GetType](type__1), _
                .Scope = parsedPurpose, _
                .SerializedDefaultValue = serializedValue _
            }
        End Function

        Public Shared Function Create(name As String, type As String, serializedValue As String) As DataFieldDefinition
            Return Create(name, type, DataFieldScope.Temporary.ToString("G"), serializedValue)
        End Function

        Public Shared Function Create(parameterDefinition As DataFieldDefinition, order As String) As DataFieldDefinitionForAction
            Dim parsedOrder = Integer.Parse(order)
            Return New DataFieldDefinitionForAction() With { _
                .ParameterDefinition = parameterDefinition, _
                .Order = parsedOrder _
            }
        End Function

        Public Shared Function Create(parameterDefinition As DataFieldDefinition, value As Object) As DataFieldDefinitionWithValue
            If value IsNot Nothing AndAlso Not value.[GetType]().Equals(parameterDefinition.Type) AndAlso Not parameterDefinition.Type.IsAssignableFrom(value.[GetType]()) Then
                Throw New InvalidOperationException()
            End If
            Return New DataFieldDefinitionWithValue() With { _
                .ParameterDefinition = parameterDefinition, _
                .Value = value _
            }
        End Function
    End Class

End Namespace
