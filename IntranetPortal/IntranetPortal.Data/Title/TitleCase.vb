﻿Public Class TitleCase
    Inherits BusinessDataBase

    Public Shared Function GetCase(bble As String) As TitleCase
        Using ctx As New ConstructionEntities
            Return ctx.TitleCases.Find(bble)
        End Using
    End Function

    Public Shared Function GetAllCases(status As DataStatus) As TitleCase()
        Using ctx As New ConstructionEntities
            Return ctx.TitleCases.Where(Function(c) c.Status = status Or status = DataStatus.All).ToArray
        End Using
    End Function

    Public Shared Function Exists(bble As String) As Boolean
        Using ctx As New ConstructionEntities
            Return ctx.TitleCases.Any(Function(c) c.BBLE = bble)
        End Using
    End Function

    Public Shared Function GetCaseStatus(bble As String) As TitleCase.DataStatus
        Using ctx As New ConstructionEntities
            Dim xcase = ctx.TitleCases.Find(bble)
            If xcase IsNot Nothing Then
                Return xcase.Status
            Else
                Return 0
            End If

        End Using

    End Function
    Public Shared Function GetAllCases(userName As String, status As DataStatus) As TitleCase()
        Using ctx As New ConstructionEntities
            Return ctx.TitleCases.Where(Function(c) c.Owner = userName AndAlso (c.Status = status Or status = DataStatus.All)).ToArray
        End Using
    End Function

    Public Overrides Function LoadData(formId As Integer) As BusinessDataBase
        Dim data = MyBase.LoadData(formId)

        Using ctx As New ConstructionEntities
            If ctx.TitleCases.Any(Function(t) t.FormItemId = formId) Then
                data = ctx.TitleCases.Where(Function(t) t.FormItemId = formId).FirstOrDefault
            End If
        End Using

        Return data
    End Function

    Public Sub SaveData(saveBy As String)
        Using ctx As New ConstructionEntities
            If ctx.TitleCases.Any(Function(t) t.BBLE = BBLE) Then
                Me.UpdateDate = DateTime.Now
                Me.UpdateBy = saveBy

                ctx.Entry(Me).State = Entity.EntityState.Modified
            Else
                Me.CreateBy = saveBy
                Me.CreateDate = DateTime.Now

                ctx.TitleCases.Add(Me)
            End If

            ctx.SaveChanges()
        End Using
    End Sub

    Public Overrides Function Save(itemData As FormDataItem) As String
        MyBase.Save(itemData)

        Using ctx As New ConstructionEntities
            If ctx.TitleCases.Any(Function(t) t.FormItemId = itemData.DataId) Then
                UpdateFields(itemData)
                ctx.Entry(Me).State = Entity.EntityState.Modified
            Else
                UpdateFields(itemData, True)
                ctx.TitleCases.Add(Me)
            End If

            ctx.SaveChanges()
            Return BBLE
        End Using
    End Function

    Private Sub UpdateFields(itemData As FormDataItem, Optional newCase As Boolean = False)
        Dim jsonCase = Newtonsoft.Json.Linq.JObject.Parse(itemData.FormData)

        If newCase Then
            FormItemId = itemData.DataId
            BBLE = jsonCase.Item("BBLE")
            CaseName = jsonCase.Item("CaseName")
            Owner = jsonCase.Item("Owner")
            CreateDate = DateTime.Now
            CreateBy = itemData.CreateBy

            Return
        End If

        UpdateBy = itemData.UpdateBy
        UpdateDate = DateTime.Now
    End Sub

    Public Enum DataStatus
        All = -1
        Completed = 1
    End Enum

End Class
