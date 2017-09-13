Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System
Imports System.Runtime.Serialization

Namespace MyIdealProp.Workflow.Core.Model
    <Serializable>
    <DataContract()>
    Public NotInheritable Class DataFieldDefinitionWithValue
        Inherits DataFieldDefinition

        <DataMember>
        Public Overrides Property Name() As String
            Get
                Return ParameterDefinition.Name
            End Get
            Set(value As String)
                ParameterDefinition.Name = value
            End Set
        End Property

        <IgnoreDataMember>
        Public Overrides Property Scope() As DataFieldScope
            Get
                Return ParameterDefinition.Scope
            End Get
            Friend Set(value As DataFieldScope)
                ParameterDefinition.Scope = value
            End Set
        End Property

        <IgnoreDataMember>
        Public Overrides Property Type() As Type
            Get
                Return ParameterDefinition.Type
            End Get
            Friend Set(value As Type)
                ParameterDefinition.Type = value
            End Set
        End Property

        <IgnoreDataMember>
        Friend Property ParameterDefinition() As DataFieldDefinition
            Private Get
                Return m_ParameterDefinition
            End Get
            Set(value As DataFieldDefinition)
                m_ParameterDefinition = value
            End Set
        End Property
        Private m_ParameterDefinition As DataFieldDefinition

        <DataMember>
        Public Property Value() As Object
            Get
                Return m_Value
            End Get
            Set(value As Object)
                m_Value = value
            End Set
        End Property
        Private m_Value As Object

        <IgnoreDataMember>
        Public Property DisplayValue As String
            Get
                Return Value.ToString
            End Get
            Set(value As String)
                m_Value = value
            End Set
        End Property

        Public Shared Function CreateWithDefaultValue(dfDefinition As DataFieldDefinition) As DataFieldDefinitionWithValue
            Dim result = New DataFieldDefinitionWithValue
            result.ParameterDefinition = dfDefinition
            result.Value = CTypeDynamic(dfDefinition.SerializedDefaultValue, dfDefinition.Type)
            Return result
        End Function
    End Class
End Namespace
