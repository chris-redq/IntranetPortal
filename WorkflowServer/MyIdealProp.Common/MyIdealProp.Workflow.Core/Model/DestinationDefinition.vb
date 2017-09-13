Imports System.Collections.Generic
Imports System

Namespace MyIdealProp.Workflow.Core.Model
    <Serializable>
    Public Class DestinationDefinition
        Inherits BaseDefinition

        Public Property DestinationType As String
        Public Property DestinationName As String
        Public Property CodeRule As ActionDefinition


        Public Shared Function CreateDestinationRule(type As String, name As String) As DestinationDefinition
            Return New DestinationDefinition With {
                .DestinationType = type,
                .DestinationName = name,
                .Name = name
                }
        End Function

        Public Shared Function CreateDestinationRule(type As String, name As String, codeRule As ActionDefinition) As DestinationDefinition
            Return New DestinationDefinition With {
                .DestinationType = type,
                .DestinationName = name,
                .CodeRule = codeRule,
                .Name = name
                }
        End Function

        'Public Shared Function CreateRule(name As String, ruleName As String) As DestinationDefinition
        '    Return New ActorDefinitionExecuteRule() With { _
        '        .Name = name, _
        '        .RuleName = ruleName, _
        '        .ParametersDictionary = New Dictionary(Of String, String)() _
        '    }
        'End Function

        'Public Shared Function CreateIsIdentity(name As String, identityId As String) As DestinationDefinition
        '    Return New ActorDefinitionIsIdentity() With { _
        '        .Name = name, _
        '        .IdentityId = New Guid(identityId) _
        '    }
        'End Function

        'Public Shared Function CreateIsInRole(name As String, roleId As String) As DestinationDefinition
        '    Return New ActorDefinitionIsInRole() With { _
        '        .Name = name, _
        '        .RoleId = roleId _
        '    }
        'End Function

        'Public Overridable Sub AddParameter(key As String, value As String)
        'End Sub

    End Class

    Public Class ActorDefinitionExecuteRule
        Inherits DestinationDefinition
        Public Property RuleName() As String

        Public ReadOnly Property Parameters() As IDictionary(Of String, String)
            Get
                Return ParametersDictionary
            End Get
        End Property

        Friend Property ParametersDictionary() As Dictionary(Of String, String)


        Public Sub AddParameter(key As String, value As String)
            ParametersDictionary.Add(key, value)
        End Sub
    End Class

    Public Class ActorDefinitionIsIdentity
        Inherits DestinationDefinition
        Public Property IdentityId() As Guid


    End Class

    Public Class ActorDefinitionIsInRole
        Inherits DestinationDefinition
        Public Property RoleId() As String


    End Class
End Namespace
