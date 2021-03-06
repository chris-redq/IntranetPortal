Imports System.Web.SessionState
Imports DevExpress.Web
Imports System.Web.Routing
Imports System.Web.Security
Imports Microsoft.AspNet.SignalR
Imports System.Threading
Imports System.Web.Http

Public Class Global_asax
    Inherits System.Web.HttpApplication

    Sub Application_Start(ByVal sender As Object, ByVal e As EventArgs)
        AddHandler DevExpress.Web.ASPxWebControl.CallbackError, AddressOf Application_Error

        Dim users As New List(Of OnlineUser)
        Application("Users") = users

        WebApiConfig2.Register(GlobalConfiguration.Configuration)
        GlobalConfiguration.Configuration.EnsureInitialized()

        'Core.WorkingLog.CloseAll()
        'Dim json = GlobalConfiguration.Configuration.Formatters.JsonFormatter
        'json.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.Objects
        'json.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
    End Sub

    Sub Session_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when the session is started
        'If Not String.IsNullOrEmpty(User.Identity.Name) Then
        '    Application.Lock()
        '    Dim users = CType(Application("Users"), List(Of OnlineUser))
        '    If users.Where(Function(u) u.UserName = User.Identity.Name).Count = 0 Then


        '        users.Add(User.Identity.Name)
        '        Application("Users") = users
        '    End If
        '    Application.UnLock()
        'End If

        'Dim httpRequest = HttpContext.Current.Request
        'If httpRequest.Browser.IsMobileDevice Then
        '    Dim path = httpRequest.Url.PathAndQuery
        '    Dim isOnMobilePage = path.StartsWith("/Mobile/", StringComparison.OrdinalIgnoreCase)
        '    If Not isOnMobilePage Then
        '        Dim redirectTo = "~/Mobile/default.aspx"
        '        HttpContext.Current.Response.Redirect(redirectTo)
        '    End If
        'End If
    End Sub


    Sub Application_BeginRequest(ByVal sender As Object, ByVal e As EventArgs)
        'Debug.WriteLine("BeginRequest:" & DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff tt"))
    End Sub

    Public Sub RefreshUserTime(sessionId As Integer)
        Application.Lock()
        Dim users = CType(Application("Users"), List(Of OnlineUser))

        If users Is Nothing Then
            Application.Lock()
            Application("Users") = New List(Of OnlineUser)
            Application.UnLock()
        End If

        Dim currentUser = users.Where(Function(u) u.SessionID = sessionId).SingleOrDefault

        If currentUser IsNot Nothing Then
            currentUser.RefreshTime = DateTime.Now
        End If
        Context.Application("Users") = users
        Context.Application.UnLock()
    End Sub

    Sub Application_EndRequest(ByVal sender As Object, ByVal e As EventArgs)
        'Debug.WriteLine("EndRequest:" & DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff tt"))
    End Sub

    Sub Application_AuthenticateRequest(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires upon attempting to authenticate the use
        If Context Is Nothing OrElse Context.User Is Nothing OrElse Context.User.Identity Is Nothing Then
            Return
        End If

        Dim url = Context.Request.RawUrl
        If url.Contains(".aspx") OrElse url.Contains("/api/") OrElse url.Contains(".svc") Then
            If ExcludeUrlFromRefresh.Any(Function(str) url.ToLower.Contains(str.ToLower)) Then
                Return
            End If

            If Context.Request.QueryString("ignore") IsNot Nothing Then
                If CBool(Request.QueryString("ignore")) Then
                    Return
                End If
            End If

            If Not OnlineUser.Refresh(Context) Then
                'LogoutCurrentUser(Context)
            End If
        End If
    End Sub

    Private Sub LogoutCurrentUser(ctx As HttpContext)
        FormsAuthentication.SignOut()
        'ctx.Response.Redirect(FormsAuthentication.LoginUrl)
    End Sub

    Private Shared ExcludeUrlFromRefresh As String() = {"GetModifyUserUrl", "GetCaseLastUpDateTime", "LastLastUpdate", "IsActive"}

    Sub Application_Error(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when an error occurs

        If HttpContext.Current IsNot Nothing AndAlso HttpContext.Current.AllErrors.Length > 0 Then
            Dim ex = HttpContext.Current.AllErrors.Last

            If TypeOf ex Is ThreadAbortException Then
                Return
            End If

            If TypeOf ex Is CallbackException Then
                DevExpress.Web.ASPxWebControl.SetCallbackErrorMessage(ex.Message)
                Return
            End If

            Try
                'UserMessage.AddNewMessage("Portal", "Error in Portal Application", String.Format("Message:{0}, Stack: {1}", ex.Message, ex.StackTrace), "")
                Dim errorId = Core.SystemLog.LogError("PortalError", ex, HttpContext.Current.Request.RawUrl, HttpContext.Current.User.Identity.Name, Nothing)
                'IntranetPortal.Core.SystemLog.Log("", String.Format("Message:{0},Request URL:{2}, Stack: {1}", ex.Message, ex.StackTrace, ), "Error", "", )
                Server.Transfer(String.Format("/PortalError.aspx?errorId={0}", errorId))
            Catch exp As Exception

            End Try
        End If

    End Sub

    Sub Session_End(ByVal sender As Object, ByVal e As EventArgs)



        ' Fires when the session ends
        'Application.Lock()
        'Dim users = CType(Application("Users"), ArrayList)
        'If users.Contains(User.Identity.Name) Then
        '    users.Remove(User.Identity.Name)
        '    Application("Users") = users
        'End If
        'Application.UnLock()
    End Sub

    Sub Application_End(ByVal sender As Object, ByVal e As EventArgs)

        ' Fires when the application ends
    End Sub


End Class
