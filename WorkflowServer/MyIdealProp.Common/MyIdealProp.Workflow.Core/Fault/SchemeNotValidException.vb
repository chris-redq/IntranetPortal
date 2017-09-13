Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System

Namespace MyIdealProp.Workflow.Core.Fault
    Public Class SchemeNotValidException
        Inherits Exception

        Public Sub New(errormsg As String)
        End Sub
    End Class
End Namespace
