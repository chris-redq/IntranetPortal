Public Class Root
    Inherits System.Web.UI.MasterPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'MainSplitter.GetPaneByName("HeaderPane").Size = If(DevExpress.Web.ASPxClasses.ASPxWebControl.GlobalTheme = "Moderno", 101, 87)
        lblVersion.Text = String.Format("Application Beta Version: {0},{1}", System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString, Server.MachineName)

        If Page.IsCallback Then
            'BindSearchGrid("")
        End If
    End Sub

    Protected Sub gridSearch_CustomCallback(sender As Object, e As DevExpress.Web.ASPxGridView.ASPxGridViewCustomCallbackEventArgs)
        'Dim key = e.Parameters
        'BindSearchGrid(key)
    End Sub

    Private _empUnderMgred As String()
    Public ReadOnly Property EmpUnderManaged As String()
        Get
            If _empUnderMgred Is Nothing Then

                If Page.User.IsInRole("Admin") Or Page.User.IsInRole("Title-Users") Then
                    _empUnderMgred = Employee.GetAllEmps()
                Else
                    Dim emps = Employee.GetManagedEmployees(Page.User.Identity.Name).ToList
                    emps.AddRange(Employee.GetControledDeptEmployees(Page.User.Identity.Name).ToList)
                    _empUnderMgred = emps.ToArray
                End If
            End If
            Return _empUnderMgred
        End Get
    End Property

    Private _sharedBBLEs As String()
    Public ReadOnly Property SharedBBLEs As String()
        Get
            Using Context As New Entities
                If _sharedBBLEs Is Nothing Then
                    _sharedBBLEs = Context.SharedLeads.Where(Function(sl) sl.UserName = Page.User.Identity.Name).Select(Function(sl) sl.BBLE).ToArray
                End If

                Return _sharedBBLEs
            End Using
        End Get
    End Property


    Public Function HasControl(bble As String)
        Return Employee.HasControlLeads(Page.User.Identity.Name, bble)
    End Function

    Sub BindSearchGrid(key As String)
        key = txtSearchKey.Text
        If String.IsNullOrEmpty(key) Then
            Return
        End If

        If IsPhoneNo(key) Then
            Dim phoneNo = key.Replace("(", "").Replace(")", "").Replace("-", "")

            Using Context As New Entities
                Dim results = (From lead In Context.Leads
                                        Join field In Context.HomeOwnerPhones On lead.BBLE Equals field.BBLE
                                        Where field.Phone = phoneNo Or lead.BBLE.StartsWith(phoneNo)
                                        Select lead).Union(Context.Leads.Where(Function(ld) ld.BBLE.StartsWith(phoneNo))).Distinct.ToList

                gridSearch.DataSource = results
                gridSearch.DataBind()
            End Using
        Else
            Using Context As New Entities
                gridSearch.DataSource = Context.Leads.Where(Function(ld) ld.LeadsName.Contains(key) Or ld.BBLE.StartsWith(key)).ToList
                gridSearch.DataBind()
            End Using
        End If
    End Sub

    Protected Sub HeadLoginStatus_LoggingOut(sender As Object, e As LoginCancelEventArgs)
        OnlineUser.LogoutUser(HttpContext.Current.User.Identity.Name)
    End Sub

    Function IsPhoneNo(key As String) As Boolean
        Dim phoneNo = key.Replace("(", "").Replace(")", "").Replace("-", "")

        Dim result As Int64 = 0
        If phoneNo.Length = 10 AndAlso Int64.TryParse(phoneNo, result) Then
            Return True
        End If

        Return False
    End Function

    Protected Sub gridSearch_AfterPerformCallback(sender As Object, e As DevExpress.Web.ASPxGridView.ASPxGridViewAfterPerformCallbackEventArgs) Handles gridSearch.AfterPerformCallback
        BindSearchGrid("")
    End Sub

    Protected Sub btnLogOut_Click(sender As Object, e As EventArgs)
        FormsAuthentication.SignOut()
        FormsAuthentication.RedirectToLoginPage()
    End Sub

    Protected Sub pcMain_WindowCallback(source As Object, e As DevExpress.Web.ASPxPopupControl.PopupWindowCallbackArgs)
        popupContentSearchPanel.Visible = True
        BindSearchGrid(e.Parameter)
    End Sub
End Class