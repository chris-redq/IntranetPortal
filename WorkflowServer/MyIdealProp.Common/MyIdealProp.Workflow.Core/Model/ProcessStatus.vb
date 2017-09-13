Imports System.Collections.Generic

Namespace MyIdealProp.Workflow.Core.Model
    <Serializable>
    Public NotInheritable Class ProcessStatus
        Public Property Id() As Integer
            Get
                Return m_Id
            End Get
            Private Set(value As Integer)
                m_Id = value
            End Set
        End Property
        Private m_Id As Integer
        Public Property IsAllowedToChangeStatus() As Boolean
            Get
                Return m_IsAllowedToChangeStatus
            End Get
            Set(value As Boolean)
                m_IsAllowedToChangeStatus = value
            End Set
        End Property
        Private m_IsAllowedToChangeStatus As Boolean
        Public Property IsAllowedToExecuteCommand() As Boolean
            Get
                Return m_IsAllowedToExecuteCommand
            End Get
            Set(value As Boolean)
                m_IsAllowedToExecuteCommand = value
            End Set
        End Property
        Private m_IsAllowedToExecuteCommand As Boolean

        Public Shared ReadOnly NotFound As New ProcessStatus() With { _
            .Id = 255, _
            .IsAllowedToChangeStatus = False, _
            .IsAllowedToExecuteCommand = False _
        }

        Public Shared ReadOnly Unknown As New ProcessStatus() With { _
            .Id = 254, _
            .IsAllowedToChangeStatus = False, _
            .IsAllowedToExecuteCommand = False _
        }

       

        Public Shared ReadOnly Initialized As New ProcessStatus() With { _
            .Id = 0, _
            .IsAllowedToChangeStatus = False, _
            .IsAllowedToExecuteCommand = False _
        }

        Public Shared ReadOnly Running As New ProcessStatus() With { _
            .Id = 1, _
            .IsAllowedToChangeStatus = False, _
            .IsAllowedToExecuteCommand = False _
        }

        Public Shared ReadOnly Active As New ProcessStatus() With { _
            .Id = 2, _
            .IsAllowedToChangeStatus = True, _
            .IsAllowedToExecuteCommand = True _
        }

        Public Shared ReadOnly Completed As New ProcessStatus() With { _
            .Id = 3, _
            .IsAllowedToChangeStatus = True, _
            .IsAllowedToExecuteCommand = False _
        }

        Public Shared ReadOnly Terminated As New ProcessStatus() With { _
            .Id = 4, _
            .IsAllowedToChangeStatus = False, _
            .IsAllowedToExecuteCommand = False _
        }

        Public Shared ReadOnly InError As New ProcessStatus() With {
           .Id = 5,
           .IsAllowedToChangeStatus = True,
           .IsAllowedToExecuteCommand = False
           }

        Public Shared ReadOnly All As IEnumerable(Of ProcessStatus) = New List(Of ProcessStatus)() From { _
            Initialized, _
            Running, _
            Active, _
            Completed, _
            Terminated _
        }

        Public Shared Operator =(cn1 As ProcessStatus, cn2 As ProcessStatus)
            Return cn1.Id = cn2.Id
        End Operator

        Public Shared Operator <>(cn1 As ProcessStatus, cn2 As ProcessStatus)
            Return cn1.Id <> cn2.Id
        End Operator
    End Class


End Namespace
