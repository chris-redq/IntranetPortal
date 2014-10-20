﻿Imports DevExpress.Web.ASPxUploadControl
Imports DevExpress.Web.ASPxEditors
Imports System.Net.Http
Imports System.Web.Services
Imports System.IO
Imports IntranetPortal.Core

Public Class UploadFilePage
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not String.IsNullOrEmpty(Request.QueryString("b")) Then
            hfBBLEData.Value = Request.QueryString("b")
            hfBBLE("BBLE") = Request.QueryString("b")
            'Bindfiles(Request.QueryString("b"))

            If Request.Files IsNot Nothing AndAlso Request.Files.Count > 0 AndAlso Not String.IsNullOrEmpty(Request.QueryString("cate")) Then
                For i = 0 To Request.Files.Count - 1
                    Dim file = Request.Files(i)
                    Dim name = file.FileName
                    Dim ms = New MemoryStream()
                    file.InputStream.CopyTo(ms)

                    Dim bble = Request.QueryString("b")
                    Dim category = Request.QueryString("cate")

                    DocumentService.UploadFile(String.Format("{0}/{1}/", bble, category), ms.ToArray, name, User.Identity.Name)
                Next
            End If
        End If
    End Sub

    Sub Bindfiles(bble As String)
        Using Context As New Entities
            gridFiles.DataSource = Context.FileAttachments.Where(Function(f) f.BBLE = bble).ToList
            gridFiles.DataBind()
        End Using
    End Sub

    Protected Sub uplImage_FileUploadComplete(sender As Object, e As DevExpress.Web.ASPxUploadControl.FileUploadCompleteEventArgs)
        e.CallbackData = SavePostedFile(e.UploadedFile)
    End Sub

    Private Function SavePostedFile(ByVal uploadedFile As UploadedFile) As String
        'Dim bble = Request.QueryString("b").ToString

        If (Not uploadedFile.IsValid) Then
            Return String.Empty
        End If
        Dim fileName As String = uploadedFile.FileName

        Using Context As New Entities
            Dim attach As New FileAttachment
            'attach.BBLE = bble
            attach.Name = uploadedFile.FileName
            attach.ContentType = uploadedFile.ContentType
            attach.Size = uploadedFile.ContentLength
            attach.Data = uploadedFile.FileBytes
            attach.Description = ""
            attach.Createby = Page.User.Identity.Name
            attach.CreateDate = DateTime.Now

            Context.FileAttachments.Add(attach)
            Context.SaveChanges()

            Return attach.FileID
        End Using
    End Function

    Private Sub UploadFileToSharepoint()
        Dim bble = hfBBLEData.Value
        Dim category = cbCategory.Value

        DocumentService.UploadFile(String.Format("{0}/{1}/", bble, category), uplImage.UploadedFiles(0).FileBytes, uplImage.UploadedFiles(0).FileName, User.Identity.Name)
    End Sub

    Protected Sub gridFiles_CustomCallback(sender As Object, e As DevExpress.Web.ASPxGridView.ASPxGridViewCustomCallbackEventArgs)
        If e.Parameters.StartsWith("UpdateCategory") Then

            Dim id = e.Parameters.Split("|")(1)
            Dim category = e.Parameters.Split("|")(2)
            Dim fileBBle = e.Parameters.Split("|")(3)

            Using Context As New Entities
                Dim file = Context.FileAttachments.Where(Function(f) f.FileID = id).SingleOrDefault
                file.Category = category
                Context.SaveChanges()

                gridFiles.DataSource = Context.FileAttachments.Where(Function(f) f.BBLE = fileBBle).ToList
                gridFiles.DataBind()
            End Using

            Return
        End If

        Dim fileId = CInt(e.Parameters.Split("|")(0))
        Dim bble = CStr(e.Parameters.Split("|")(1))

        Using Context As New Entities
            Dim attach = Context.FileAttachments.Where(Function(f) f.FileID = fileId).SingleOrDefault
            attach.BBLE = bble

            Context.SaveChanges()

            gridFiles.DataSource = Context.FileAttachments.Where(Function(f) f.BBLE = bble).ToList
            gridFiles.DataBind()
        End Using
    End Sub

    Protected Sub gridFiles_HtmlRowPrepared(sender As Object, e As DevExpress.Web.ASPxGridView.ASPxGridViewTableRowEventArgs) Handles gridFiles.HtmlRowPrepared
        If Not e.RowType = DevExpress.Web.ASPxGridView.GridViewRowType.Data Then
            Return
        End If

        Dim category = e.GetValue("Category")

        Dim cbCategory = TryCast(gridFiles.FindRowCellTemplateControl(e.VisibleIndex, gridFiles.Columns("Category"), "cbCategory"), ASPxComboBox)
        If Not cbCategory Is Nothing Then
            If Not String.IsNullOrEmpty(category) Then
                For Each item As ListEditItem In cbCategory.Items
                    If item.Value = category Then
                        item.Selected = True
                        Exit For
                    End If
                Next
            End If

            cbCategory.ClientSideEvents.SelectedIndexChanged = String.Format("function(s, e){{UpdateCategory({0}, s);}}", e.KeyValue.ToString)
        End If
    End Sub

    Protected Sub ASPxButton1_Click(sender As Object, e As EventArgs)
        UploadFileToSharepoint()
    End Sub
End Class