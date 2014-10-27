﻿Imports IntranetPortal.ShortSale

Public Class ShortSaleManage
    Public Shared Sub MoveLeadsToShortSale(bble As String, createBy As String)
        Dim li = LeadsInfo.GetInstance(bble)

        If li IsNot Nothing Then
            Dim propBase = SaveProp(li, createBy)

            Dim ssCase = New ShortSaleCase(propBase)
            ssCase.BBLE = bble
            ssCase.CaseName = li.LeadsName
            ssCase.Status = ShortSale.CaseStatus.NewFile
            ssCase.Owner = GetIntaker()
            ssCase.CreateBy = createBy
            ssCase.CreateDate = DateTime.Now
            ssCase.Save()
        End If
    End Sub

    Public Shared Function SaveProp(li As LeadsInfo, createBy As String) As IntranetPortal.ShortSale.PropertyBaseInfo
        Dim propBase = IntranetPortal.ShortSale.PropertyBaseInfo.GetInstance(li.BBLE)

        If propBase Is Nothing Then
            propBase = New IntranetPortal.ShortSale.PropertyBaseInfo
            propBase.BBLE = li.BBLE
            propBase.Block = li.Block
            propBase.Lot = li.Lot
            propBase.Number = li.Number
            propBase.StreetName = li.StreetName
            propBase.City = li.NeighName
            propBase.State = li.State
            propBase.Zipcode = li.ZipCode
            propBase.TaxClass = li.TaxClass
            propBase.NumOfStories = li.NumFloors
            propBase.CreateDate = DateTime.Now
            propBase.CreateBy = createBy
            propBase.Save()
        End If

        Return propBase
    End Function

    Private Shared Function GetIntaker() As String
        Return System.Configuration.ConfigurationManager.AppSettings("ShortSaleIntake").ToString
    End Function
End Class
