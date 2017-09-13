Imports System

Namespace MyIdealProp.Workflow.Core.Model
    Public Class SchemeDefinition(Of T As Class)
        Public Property Scheme() As T
            Get
                Return m_Scheme
            End Get
            Private Set(value As T)
                m_Scheme = value
            End Set
        End Property
        Private m_Scheme As T
        Public Property Id() As Integer
            Get
                Return m_Id
            End Get
            Private Set(value As Integer)
                m_Id = value
            End Set
        End Property
        Private m_Id As Integer
        Public Property IsObsolete() As Boolean
            Get
                Return m_IsObsolete
            End Get
            Private Set(value As Boolean)
                m_IsObsolete = value
            End Set
        End Property
        Private m_IsObsolete As Boolean
        Public Property IsDeterminingParametersChanged() As Boolean
            Get
                Return m_IsDeterminingParametersChanged
            End Get
            Set(value As Boolean)
                m_IsDeterminingParametersChanged = value
            End Set
        End Property
        Private m_IsDeterminingParametersChanged As Boolean

        Public Sub New(id__1 As Integer, scheme__2 As T, isObsolete__3 As Boolean, isDeterminingParametersChanged__4 As Boolean)
            Id = id__1
            Scheme = scheme__2
            IsObsolete = isObsolete__3
            IsDeterminingParametersChanged = isDeterminingParametersChanged__4
        End Sub

        Public Sub New(id As Integer, scheme As T, isObsolete As Boolean)
            Me.New(id, scheme, isObsolete, False)
        End Sub

    End Class
End Namespace
