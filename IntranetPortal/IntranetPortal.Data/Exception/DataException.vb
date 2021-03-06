﻿Public Class DataException
    Inherits Exception

    Public Property Code As String

    ''' <summary>
    ''' </summary>
    Public Sub New()
        MyBase.New()
    End Sub

    ''' <summary>
    ''' with the specified error message.
    ''' </summary>
    ''' <param name="message">The message that describes the error.</param>
    Public Sub New(ByVal message As String)
        MyBase.New(message)
    End Sub

    ''' <summary>
    ''' with the specified error message and a reference to the inner
    ''' exception that is the cause of this exception.
    ''' </summary>
    ''' <param name="message">The message that describes the error.</param>
    ''' <param name="innerException">The exception that is the cause of the
    ''' current exception, or a null reference if no inner exception is
    ''' specified</param>
    Public Sub New(ByVal message As String, ByVal innerException As System.Exception)
        MyBase.New(message, innerException)
    End Sub

    Public Sub New(code As String, message As String)
        MyBase.New(message)
        Me.Code = code
    End Sub
End Class
