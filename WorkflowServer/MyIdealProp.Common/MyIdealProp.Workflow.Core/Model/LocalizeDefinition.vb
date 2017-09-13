Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System

Namespace MyIdealProp.Workflow.Core.Model
    Public Enum LocalizeType
        Command
        State
    End Enum

    <Serializable>
    Public NotInheritable Class LocalizeDefinition
        Public Property Type() As LocalizeType
            Get
                Return m_Type
            End Get
            Friend Set(value As LocalizeType)
                m_Type = value
            End Set
        End Property
        Private m_Type As LocalizeType

        Public Property IsDefault() As Boolean
            Get
                Return m_IsDefault
            End Get
            Friend Set(value As Boolean)
                m_IsDefault = value
            End Set
        End Property
        Private m_IsDefault As Boolean

        Public Property ObjectName() As String
            Get
                Return m_ObjectName
            End Get
            Friend Set(value As String)
                m_ObjectName = value
            End Set
        End Property
        Private m_ObjectName As String

        Public Property Culture() As String
            Get
                Return m_Culture
            End Get
            Friend Set(value As String)
                m_Culture = value
            End Set
        End Property
        Private m_Culture As String

        Public Property Value() As String
            Get
                Return m_Value
            End Get
            Friend Set(value As String)
                m_Value = value
            End Set
        End Property
        Private m_Value As String


        Public Shared Function Create(objectName As String, type As String, culture As String, value As String, isDefault As String) As LocalizeDefinition
            Dim parsedType As LocalizeType
            [Enum].TryParse(type, True, parsedType)

            Return New LocalizeDefinition() With { _
                .Culture = culture, _
                .IsDefault = Not String.IsNullOrEmpty(isDefault) AndAlso Boolean.Parse(isDefault), _
                .ObjectName = objectName, _
                .Type = parsedType, _
                .Value = value _
            }

        End Function
    End Class
End Namespace
