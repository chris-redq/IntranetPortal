Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System

Namespace MyIdealProp.Workflow.Core.Model
    Public MustInherit Class OnErrorDefinition
        Inherits BaseDefinition
        Public Property ActionType() As OnErrorActionType
            Get
                Return m_ActionType
            End Get
            Friend Set(value As OnErrorActionType)
                m_ActionType = value
            End Set
        End Property
        Private m_ActionType As OnErrorActionType
        Public Property Priority() As Integer
            Get
                Return m_Priority
            End Get
            Friend Set(value As Integer)
                m_Priority = value
            End Set
        End Property
        Private m_Priority As Integer
        ' public bool IsRethrow { get; internal set; }
        Public Property ExceptionType() As Type
            Get
                Return m_ExceptionType
            End Get
            Friend Set(value As Type)
                m_ExceptionType = value
            End Set
        End Property
        Private m_ExceptionType As Type

        Public Shared Function CreateSetActivityOnError(name As String, nameRef As String, priority As String, typeName As String) As SetActivityOnErrorDefinition
            'string isExecuteImplementation, string isRethrow
            'IsExecuteImplementation =
            '    !string.IsNullOrEmpty(isExecuteImplementation) && bool.Parse(isExecuteImplementation),
            'IsRethrow =
            '   !string.IsNullOrEmpty(isRethrow) && bool.Parse(isRethrow),
            Return New SetActivityOnErrorDefinition() With { _
                .ActionType = OnErrorActionType.SetActivity, _
                .NameRef = nameRef, _
                .Name = name, _
                .Priority = If(Not String.IsNullOrEmpty(priority), Integer.Parse(priority), Integer.MaxValue), _
                .ExceptionType = Type.[GetType](typeName) _
            }
        End Function
    End Class

    Public NotInheritable Class SetActivityOnErrorDefinition
        Inherits OnErrorDefinition
        Public Property NameRef() As String
            Get
                Return m_NameRef
            End Get
            Friend Set(value As String)
                m_NameRef = value
            End Set
        End Property
        Private m_NameRef As String
        'public bool IsExecuteImplementation { get; internal set; }
    End Class
End Namespace
