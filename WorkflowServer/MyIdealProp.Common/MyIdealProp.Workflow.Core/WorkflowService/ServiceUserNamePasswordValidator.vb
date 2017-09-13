Imports System.IdentityModel.Selectors
Imports System.IdentityModel.Tokens

Namespace MyIdealProp.Workflow.Service


    Public Class ServiceUserNamePasswordValidator
        Inherits UserNamePasswordValidator

        Public Overrides Sub Validate(userName As String, password As String)
            If String.IsNullOrEmpty(userName) Or String.IsNullOrEmpty(password) Then
                Throw New SecurityTokenException("Username and password is required")
            End If

            If Not UserSecurity.Validate(userName, password) Then
                Throw New SecurityTokenException("Invalid username or password.")
            End If
        End Sub
    End Class
End Namespace