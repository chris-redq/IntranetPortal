Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System

Namespace MyIdealProp.Workflow.Core.Model
    Public NotInheritable Class TimerDefinition
        Inherits BaseDefinition
        Public Property Type() As TimerType
            Get
                Return m_Type
            End Get
            Friend Set(value As TimerType)
                m_Type = value
            End Set
        End Property
        Private m_Type As TimerType
        Public Property DelayTimeInMilliseconds() As Integer
            Get
                Return m_DelayTimeInMilliseconds
            End Get
            Friend Set(value As Integer)
                m_DelayTimeInMilliseconds = value
            End Set
        End Property
        Private m_DelayTimeInMilliseconds As Integer
        Public Property IntervalTimeInMilliseconds() As Integer
            Get
                Return m_IntervalTimeInMilliseconds
            End Get
            Friend Set(value As Integer)
                m_IntervalTimeInMilliseconds = value
            End Set
        End Property
        Private m_IntervalTimeInMilliseconds As Integer

        Public Shared Function Create(name As String, type As String, delay As String, interval As String) As TimerDefinition
            Dim parsedType As TimerType
            [Enum].TryParse(type, True, parsedType)

            Dim delayTimeInMilliseconds As Integer
            If Not Integer.TryParse(delay, delayTimeInMilliseconds) Then
                Throw New InvalidOperationException()
            End If

            Dim intervalTimeInMilliseconds As Integer
            If Not Integer.TryParse(interval, intervalTimeInMilliseconds) Then
                Throw New InvalidOperationException()
            End If


            Return New TimerDefinition() With { _
                .Name = name, _
                .IntervalTimeInMilliseconds = intervalTimeInMilliseconds, _
                .DelayTimeInMilliseconds = delayTimeInMilliseconds, _
                .Type = parsedType _
            }
        End Function
    End Class
End Namespace
