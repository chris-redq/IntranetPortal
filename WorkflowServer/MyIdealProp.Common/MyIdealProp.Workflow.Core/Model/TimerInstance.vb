Imports System.Collections.Generic
Imports System

Namespace MyIdealProp.Workflow.Core.Model
    Public Class TimerInstance
        Public Property EscalationId As Integer
        Public Property ProcInstId As Integer
        Public Property ActInstId As Integer
        Public Property DueTime As DateTime
        Public Property ActionName As String
        Public Property TimerId As Integer
    End Class
End Namespace

