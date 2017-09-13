Imports System

Namespace MyIdealProp.Workflow.Core.Model
    <Serializable>
    Public MustInherit Class TriggerDefinition
        Public MustOverride ReadOnly Property Type() As TriggerType


        Public Shared Function Create(type As String) As TriggerDefinition
            Dim parsedType As TriggerType
            [Enum].TryParse(type, True, parsedType)

            Select Case parsedType
                Case TriggerType.Auto
                    Return Auto
                    Exit Select
                Case TriggerType.Command
                    Return New CommandTriggerDefinition()
                Case TriggerType.Timer
                    Return New TimerTriggerDefinition()
            End Select

            Return Nothing
        End Function

        Public ReadOnly Property NameRef() As String
            Get
                Dim res As String
                Select Case Type
                    Case TriggerType.Command
                        res = (If(Command Is Nothing, String.Empty, Command.Name))
                        Exit Select
                    Case TriggerType.Timer
                        res = (If(Timer Is Nothing, String.Empty, Timer.Name))
                        Exit Select
                    Case Else
                        res = String.Empty
                        Exit Select
                End Select
                Return res
            End Get
        End Property

        Public ReadOnly Property Command() As EventDefinition2
            Get
                Return TryCast(Me, CommandTriggerDefinition).Command
            End Get
        End Property

        Public ReadOnly Property Timer() As TimerDefinition
            Get
                Return TryCast(Me, TimerTriggerDefinition).Timer
            End Get
        End Property

        Public Shared ReadOnly Property Auto() As TriggerDefinition
            Get
                Return New AutoTriggerDefinition()
            End Get
        End Property
    End Class

    <Serializable>
    Public Class CommandTriggerDefinition
        Inherits TriggerDefinition
        Public Overrides ReadOnly Property Type() As TriggerType
            Get
                Return TriggerType.Command
            End Get
        End Property

        Public Property Command() As EventDefinition2
            Get
                Return m_Command
            End Get
            Set(value As EventDefinition2)
                m_Command = value
            End Set
        End Property
        Private m_Command As EventDefinition2
    End Class


    Public Class TimerTriggerDefinition
        Inherits TriggerDefinition
        Public Overrides ReadOnly Property Type() As TriggerType
            Get
                Return TriggerType.Timer
            End Get
        End Property

        Public Property Timer() As TimerDefinition
            Get
                Return m_Timer
            End Get
            Set(value As TimerDefinition)
                m_Timer = value
            End Set
        End Property
        Private m_Timer As TimerDefinition
    End Class

    Public Class AutoTriggerDefinition
        Inherits TriggerDefinition
        Public Overrides ReadOnly Property Type() As TriggerType
            Get
                Return TriggerType.Auto
            End Get
        End Property
    End Class



    Public Enum TriggerType
        Command
        Auto
        Timer
    End Enum

    <Serializable>
    Public NotInheritable Class ConditionDefinition
        Private Sub New()
        End Sub

        Public Property Type() As ConditionType
            Get
                Return m_Type
            End Get
            Friend Set(value As ConditionType)
                m_Type = value
            End Set
        End Property
        Private m_Type As ConditionType
        Public Property Action() As ActionDefinition
            Get
                Return m_Action
            End Get
            Friend Set(value As ActionDefinition)
                m_Action = value
            End Set
        End Property
        Private m_Action As ActionDefinition
        Public Property ResultOnPreExecution() As System.Nullable(Of Boolean)
            Get
                Return m_ResultOnPreExecution
            End Get
            Friend Set(value As System.Nullable(Of Boolean))
                m_ResultOnPreExecution = value
            End Set
        End Property
        Private m_ResultOnPreExecution As System.Nullable(Of Boolean)

        Public Shared Function Create(type As String) As ConditionDefinition
            Return Create(type, Nothing, Nothing)
        End Function

        Public Shared Function Create(type As String, resultOnPreExecution As String) As ConditionDefinition
            Return Create(type, Nothing, resultOnPreExecution)
        End Function

        Public Shared Function Create(type As String, action As ActionDefinition, resultOnPreExecution As String) As ConditionDefinition
            Dim parsedType As ConditionType
            [Enum].TryParse(type, True, parsedType)

            Return New ConditionDefinition() With { _
                .Action = action, _
                .Type = parsedType, _
                .ResultOnPreExecution = If(String.IsNullOrEmpty(resultOnPreExecution), CType(Nothing, System.Nullable(Of Boolean)), Boolean.Parse(resultOnPreExecution)) _
            }
        End Function

        Public Shared ReadOnly Property Always() As ConditionDefinition
            Get
                Return New ConditionDefinition() With { _
                    .Type = ConditionType.Always _
                }
            End Get
        End Property

    End Class

    Public Enum ConditionType
        Action
        Always
        Otherwise

    End Enum


End Namespace
