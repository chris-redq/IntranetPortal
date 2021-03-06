﻿
Imports System.ComponentModel.DataAnnotations

<MetadataType(GetType(ConstructionInitialFormMetaData))>
Partial Public Class ConstructionInitialForm

    Public Shared Function GetAddressByUser(username As String) As List(Of List(Of String))
        Using ctx As New PortalEntities
            Dim result = New List(Of List(Of String))
            Dim pairs = From f In ctx.ConstructionInitialForms
                        Where f.Owner = username
                        Join c In ctx.ConstructionCases
                         On f.BBLE Equals c.BBLE
                        Select f.BBLE, c.CaseName
            For Each p In pairs
                Dim templist = New List(Of String)
                templist.Add(p.BBLE)
                templist.Add(p.CaseName)
                result.Add(templist)
            Next
            Return result
        End Using
    End Function
    Public Shared Function GetForms(BBLE As String) As ConstructionInitialForm
        Using ctx As New PortalEntities
            Return ctx.ConstructionInitialForms.Where(Function(F) F.BBLE = BBLE).FirstOrDefault
        End Using
    End Function

    Public Sub SaveForms(userName As String)
        Using ctx As New PortalEntities
            If ctx.ConstructionInitialForms.Any(Function(t) t.BBLE = BBLE) Then
                Me.UpdateDate = DateTime.Now
                Me.UpdateBy = userName
                ctx.Entry(Me).State = Entity.EntityState.Modified
            Else
                Me.CreateBy = userName
                Me.CreateDate = DateTime.Now
                ctx.ConstructionInitialForms.Add(Me)
            End If

            ctx.SaveChanges()
        End Using
    End Sub

    Public Shared Sub updateOwner(BBLE As String, userName As String, owner As String)
        Using ctx As New PortalEntities
            If ctx.ConstructionInitialForms.Any(Function(t) t.BBLE = BBLE) Then
                Dim form = ctx.ConstructionInitialForms.Where(Function(f) f.BBLE = BBLE).FirstOrDefault
                form.Owner = owner
                form.UpdateDate = DateTime.Now
                form.UpdateBy = userName
                ctx.Entry(form).State = Entity.EntityState.Modified
            Else
                Dim newForm = New ConstructionInitialForm
                newForm.BBLE = BBLE
                newForm.CreateBy = userName
                newForm.CreateDate = DateTime.Now
                newForm.Owner = owner
                ctx.ConstructionInitialForms.Add(newForm)
            End If

            ctx.SaveChanges()
        End Using
    End Sub

End Class

Public Class ConstructionInitialFormMetaData

    <Newtonsoft.Json.JsonConverter(GetType(Core.JsObjectToStringConverter))>
    Public Property Form As String

End Class