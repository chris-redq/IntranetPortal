﻿Imports Newtonsoft.Json
Imports IntranetPortal.ShortSale
Imports DevExpress.Web.ASPxGridView

Public Class NGShortSalePropertyTab
    Inherits System.Web.UI.UserControl

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load



    End Sub
    Sub initPropertyInfo()
        Using Context As New Entities
            'propertyInfo = Internal.sh .Where(Function(pi) pi.BBLE = "123").FirstOrDefault()
        End Using
    End Sub

    Sub BindData(caseID As Integer)

       

    End Sub

End Class