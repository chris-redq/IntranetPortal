﻿Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports IntranetPortal
Imports IntranetPortal.Data


''' <summary>
''' The corporation entity Unit test
''' </summary>
<TestClass()> Public Class CorpEntityUnitTest

    Dim bble As String = "4089170024 "
    Dim team As String = "RonTeam"

    ''' <summary>
    ''' GetAvailableCorp testing
    ''' </summary>
    <TestMethod()> Public Sub GetAvailableCorp_returnAvailableCorp()
        Dim corp = CorporationEntity.GetAvailableCorp(team, False)

        Assert.AreEqual("Available", corp.Status)
        Assert.AreEqual(team, corp.Office)

    End Sub

    ''' <summary>
    ''' AssignCorp testing
    ''' </summary>
    <TestMethod()> Public Sub AssignCorp_returnAssignedCorp()

        Dim li = LeadsInfo.GetInstance(BBLE)

        Dim corp = CorporationEntity.GetAvailableCorp("RonTeam", False)
        Dim address = "116-55 Queens Blvd"
        corp.AssignCorp(bble, address)

        corp = CorporationEntity.GetEntity(corp.EntityId)

        Assert.AreEqual(corp.BBLE, bble)
        Assert.AreEqual(address, corp.Address)
        Assert.AreEqual("Assigned Out", corp.Status)
    End Sub

End Class