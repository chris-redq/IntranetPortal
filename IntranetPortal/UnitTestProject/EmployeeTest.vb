﻿Imports IntranetPortal

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

End Class
