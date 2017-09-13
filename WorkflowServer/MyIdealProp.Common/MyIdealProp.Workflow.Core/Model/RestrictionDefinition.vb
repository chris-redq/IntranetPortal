Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System

Namespace MyIdealProp.Workflow.Core.Model
    Public Class RestrictionDefinition
        Public Property Type() As RestrictionType
            Get
                Return m_Type
            End Get
            Friend Set(value As RestrictionType)
                m_Type = value
            End Set
        End Property
        Private m_Type As RestrictionType
        Public Property Actor() As DestinationDefinition
            Get
                Return m_Actor
            End Get
            Friend Set(value As DestinationDefinition)
                m_Actor = value
            End Set
        End Property
        Private m_Actor As DestinationDefinition

        Public Shared Function Create(type As String, actor As DestinationDefinition) As RestrictionDefinition
            Dim parsedType As RestrictionType
            [Enum].TryParse(type, True, parsedType)

            Return New RestrictionDefinition() With { _
                .Actor = actor, _
                .Type = parsedType _
            }
        End Function
    End Class

    Public Enum RestrictionType
        Allow
        Restrict
    End Enum
End Namespace
