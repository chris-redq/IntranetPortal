Imports System


Public Class NotValidationException
    Inherits Exception

    Public Sub New(msg As String)
        MyBase.New(msg)
    End Sub
End Class


Public Class ObjectNotFoundException
    Inherits Exception
    Public Sub New(msg As String)
        MyBase.New(msg)
    End Sub
End Class
