﻿Imports IntranetPortal
Imports System.Web.Security

<TestClass()>
Public Class EmployeeTest

    <TestMethod()>
    Public Sub GetCurrentAppId()
        Dim appId = IntranetPortal.Employee.CurrentAppId
        Assert.AreEqual(Of Integer)(appId, 1)
    End Sub

    <TestMethod>
    Public Sub TestLegalCaseRelateUser()
        Dim el = IntranetPortal.LegalCaseManage.GetCaseRelateUsers("3037220024")
        Assert.IsTrue(el IsNot Nothing)
    End Sub

    <TestMethod>
    Public Sub GetUserRoles_ReturnRolesWithStar()
        Dim roles = System.Web.Security.Roles.GetRolesForUser("Michael Kay")
        Assert.IsTrue(roles.Contains("ShortSale-*"))

        roles = System.Web.Security.Roles.GetRolesForUser("Michael Gali")
        Assert.IsTrue(roles.Contains("OfficeManager-*"))
    End Sub

    <TestMethod>
    Public Sub GetDeptUsersList_ReturnEmployee()
        Dim result = Employee.GetDeptUsersList("GaliTeam", True)
        Assert.IsFalse(result.Any(Function(a) a.Name = "Crystal Dixon"))

        result = Employee.GetDeptUsersList("GaliTeam", False)
        Assert.IsTrue(result.Any(Function(a) a.Name = "Crystal Dixon"))
    End Sub

    <TestMethod>
    Public Sub GetTeamUsersArray_returnUserList()
        Dim result = UserInTeam.GetTeamUsersArray("GaliTeam")
        Assert.IsFalse(result.Contains("Crystal Dixon"))

        For Each user In result
            Assert.IsTrue(Employee.GetEmpTeams(user).Contains("GaliTeam"))
        Next

        result = UserInTeam.GetTeamUsersArray("GaliTeam", True)
        Assert.IsTrue(result.Contains("Crystal Dixon"))
    End Sub

    Dim deptName = "TomTeam"
    <TestMethod>
    Public Sub GetAllDeptUser_returnAllUsers()
        Dim users = Employee.GetDeptUsers(deptName, False)
        Dim users2 = Employee.GetAllDeptUsers(deptName)
        Assert.AreEqual(11, users.Length)
        Assert.AreEqual(11, users2.Length)
    End Sub

    <TestMethod>
    Public Sub GetActiveDeptUser_returnActiveUsers()
        Dim users = Employee.GetDeptUsersList(deptName, False)
        Dim users2 = Employee.GetDeptUsers(deptName)
        Assert.AreEqual(7, users2.Count)
        Assert.AreEqual(7, users.Where(Function(em) em.Active = True).Count)
    End Sub

    <TestMethod>
    Public Sub GetNonActiveDeptUser_returnNonActiveUsers()
        Dim users = Employee.GetDeptUsersList(deptName, False)
        Dim nonActiveUsers = Employee.GetDeptUnActiveUserList(deptName)
        Assert.AreEqual(4, nonActiveUsers.Count)
        Assert.AreEqual(4, users.Where(Function(em) em.Active = False).Count)
    End Sub

    <TestMethod>
    Public Sub GetMyEmployeesByTeam_returnEmployees()
        Dim userName = "Michael Gali"


        Dim users1 = Employee.GetMyEmployeesByTeam(userName).Select(Function(ut) ut.EmployeeName).OrderBy(Function(ut) ut).ToArray
        Dim users2 = UserInTeam.GetTeamUsersArray("GaliTeam").OrderBy(Function(ut) ut).ToArray

        Assert.IsTrue(users1.SequenceEqual(users2))

        Dim ui = New UsersInRole
        ui.Rolename = "OfficeManager-TomTeam"
        ui.Username = userName
        ui.Add()

        users1 = Employee.GetMyEmployeesByTeam(userName).Select(Function(ut) ut.EmployeeName).OrderBy(Function(ut) ut).ToArray
        ui.Delete()
        Dim users2List = users2.ToList
        users2List.AddRange(UserInTeam.GetTeamUsersArray("TomTeam"))
        users2 = users2List.Distinct.OrderBy(Function(ut) ut).ToArray

        Assert.IsTrue(users1.SequenceEqual(users2))

        Assert.IsFalse(ui.IsExsit)
    End Sub

    <TestMethod>
    Public Sub GetEmloyeeEmailTest()
        Dim a As String = ""
        a = Nothing
        Dim emails = Employee.GetEmpsEmails(a)


        Assert.IsTrue(String.IsNullOrEmpty(emails))
        Dim chris = Employee.GetInstance("Chris Yan")
        Dim steven = Employee.GetInstance("xsxxx")
        emails = Employee.GetEmpsEmails(chris, steven)
        Assert.IsFalse(String.IsNullOrEmpty(emails))

        emails = Employee.GetEmpsEmails(steven)
        Assert.IsTrue(String.IsNullOrEmpty(emails))
    End Sub
    <TestMethod>
    Public Sub CryptoPasswordTest()
        Dim e = New Employee()
        Dim testPassword = "Hello wolrd!"
        e.Password = testPassword
        ' not crypto yet the verify password should not equal
        Assert.IsFalse(e.VerifyPassword(testPassword))

        Dim hash = e.CryptoPasswrod(e.Password)
        ' check hello world md5 crypto if you change the password it may not pass this unit test
        ' run CryptoPasswrod function and copy test crypto password to here
        Assert.AreEqual(hash, "52e7a4e9de7dc90f7eb53d67c2d79547")
        e.ChangePassword(testPassword)
        Assert.AreEqual(hash, e.Password)
        ' after change password it should pass verify password
        Assert.IsTrue(e.VerifyPassword(testPassword))
    End Sub
End Class
