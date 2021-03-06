﻿''' <summary>
''' The base page class for Portal,
''' provide the Username, Employee object
''' </summary>
Public Class PortalPage
    Inherits Page

    Protected Sub Base_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            If Not PortalNavManage.IsViewable(HttpContext.Current) Then
                PageAuthorization = False
                NotAllowdAccess()
            Else
                PageAuthorization = True
            End If
        End If
    End Sub

    Protected Property PageAuthorization As Boolean

    Protected Overridable Sub NotAllowdAccess()
        Server.Transfer("/PortalError.aspx?code=1001")
    End Sub

    Private _userName As String
    Public ReadOnly Property UserName As String
        Get
            If String.IsNullOrEmpty(_userName) Then
                _userName = Page.User.Identity.Name
            End If

            Return _userName
        End Get
    End Property

    Private _currentUser As Employee
    Public ReadOnly Property CurrentUser As Employee
        Get
            If _currentUser Is Nothing Then
                _currentUser = Employee.GetInstance(UserName)
            End If

            Return _currentUser
        End Get
    End Property

    Public ReadOnly Property CurrentAppId As Integer
        Get
            If CurrentUser IsNot Nothing Then
                Return CurrentUser.AppId
            End If

            Return Nothing
        End Get
    End Property

End Class
