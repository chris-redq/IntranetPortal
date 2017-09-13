Imports System.Collections.Generic
Imports System

Namespace MyIdealProp.Workflow.Core.Model
    <Serializable>
    Public Class ActionDefinition
        Inherits BaseDefinition
        Public Overridable Property Type() As Type
            Get
                Return m_Type
            End Get
            Friend Set(value As Type)
                m_Type = value
            End Set
        End Property
        Private m_Type As Type
        Public Overridable Property FullTypeName() As String
            Get
                Return m_FullTypeName
            End Get
            Friend Set(value As String)
                m_FullTypeName = value
            End Set
        End Property
        Private m_FullTypeName As String
        Public Overridable Property MethodName() As String
            Get
                Return m_MethodName
            End Get
            Friend Set(value As String)
                m_MethodName = value
            End Set
        End Property
        Private m_MethodName As String

        Public Overridable ReadOnly Property InputParameters() As IEnumerable(Of DataFieldDefinitionForAction)
            Get
                Return InputParametersList
            End Get
        End Property

        Protected OutputParametersList As List(Of DataFieldDefinitionForAction)

        Public Overridable ReadOnly Property OutputParameters() As IEnumerable(Of DataFieldDefinitionForAction)
            Get
                Return OutputParametersList
            End Get
        End Property

        Protected InputParametersList As List(Of DataFieldDefinitionForAction)

        Public Shared Function Create(name As String, type__1 As String, metodName As String) As ActionDefinition
            Dim t As Type = Nothing
            Try
                t = System.Type.GetType(type__1)
                't = Type.[GetType](type__1)
            Catch ex As Exception

            End Try

            Return New ActionDefinition() With { _
                 .Name = name, _
                 .InputParametersList = New List(Of DataFieldDefinitionForAction)(), _
                 .OutputParametersList = New List(Of DataFieldDefinitionForAction)(), _
                 .Type = t, _
                 .FullTypeName = type__1, _
                 .MethodName = metodName _
            }
        End Function

        Public Shared Function Create(actionDefinition As ActionDefinition, order As String, type As EventDefinitionType) As ActionDefinitionForActivity
            Dim parsedOrder = Integer.Parse(order)
            Return New ActionDefinitionForActivity() With { _
                .ActionDefinition = actionDefinition, _
                .Order = parsedOrder,
                .EventType = type
            }
        End Function

        Public Shared Function Create(actionDefinition As ActionDefinition, order As Integer) As ActionDefinitionForActivity
            Return New ActionDefinitionForActivity() With { _
                .ActionDefinition = actionDefinition, _
                .Order = order _
            }
        End Function

        Public Sub AddInputParameterRef(parameter As DataFieldDefinitionForAction)
            InputParametersList.Add(parameter)
        End Sub

        Public Sub AddOutputParameterRef(parameter As DataFieldDefinitionForAction)
            OutputParametersList.Add(parameter)
        End Sub

        Public Shared NoAction As New ActionDefinition()
    End Class

End Namespace
