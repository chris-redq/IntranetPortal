﻿Imports Microsoft.SharePoint.Client
Imports System.Security
Imports Microsoft.SharePoint.Client.Sharing
Imports System.Web

''' <summary>
''' The document service for sharepoint 365
''' </summary>
Public Class DocumentService
    Private Shared ReadOnly userName As String = System.Configuration.ConfigurationManager.AppSettings("Office365UserName")
    Private Shared ReadOnly passwordstr As String = System.Configuration.ConfigurationManager.AppSettings("Office365Password")
    Private Shared ReadOnly serverUrl As String = System.Configuration.ConfigurationManager.AppSettings("Office365ServerUrl")
    Private Shared ReadOnly DocumentLibraryTitle = "Documents"
    Private Shared ReadOnly RootFolderName = "Shared%20Documents"

    ''' <summary>
    ''' get property file list from sharepoint
    ''' </summary>
    ''' <param name="bble">the bble of property</param>
    ''' <returns>the files collection, include a list of category name, file list and sub category</returns>
    Public Shared Function GetFilesByBBLE(bble As String) As Object
        Using ClientContext = GetClientContext()

            If Not IsFolderExsit(ClientContext, bble) Then
                CreateFolder(bble)
            End If

            Dim bbleFolder = ClientContext.Web.GetFolderByServerRelativeUrl("/" & RootFolderName & "/" & bble)
            Dim categories = bbleFolder.Folders
            ClientContext.Load(categories)
            ClientContext.ExecuteQuery()

            Dim ds = (From cate In categories
                     Select New With {
                         .Key = cate.Name,
                         .SubCategory = GetSubCategories(ClientContext, cate),
                         .Group = GetFiles(ClientContext, cate)
                         }).ToList

            Return ds
        End Using
    End Function

    Public Shared Function GetFileByJS(jsFile As String) As Object
        Dim json = Newtonsoft.Json.Linq.JObject.Parse(jsFile)

        If json IsNot Nothing Then
            Dim path = json("path")
            Dim name = json("name")
            If path IsNot Nothing AndAlso Not String.IsNullOrEmpty(path) Then
                Dim file = GetPDFContent(path)

                If file IsNot Nothing AndAlso name IsNot Nothing Then
                    Return New With {.Data = file, .Name = name.ToString}
                End If
            End If
        End If

        Return Nothing
    End Function


    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="bble"></param>
    ''' <param name="folderPath"></param>
    ''' <returns></returns>
    Public Shared Function GetCateByBBLEAndFolder(bble As String, folderPath As String) As Object
        Using ctx = GetClientContext()
            If Not IsFolderExsit(ctx, bble & "/" & folderPath) Then
                CreateFolder(bble & "/" & folderPath)
            End If
            Dim curFolder = ctx.Web.GetFolderByServerRelativeUrl("/" & RootFolderName & "/" & bble & "/" & folderPath)
            Dim folders = getSubFolders(ctx, curFolder)
            Dim files = GetFiles(ctx, curFolder)
            Return New With {
                .path = "/" & bble & "/" & folderPath,
                .folders = folders,
                .files = files
            }
        End Using
    End Function

    Public Shared Function getSubFolders(clientContext As ClientContext, curFolder As Folder) As Object
        Dim folders = curFolder.Folders
        clientContext.Load(folders)
        clientContext.ExecuteQuery()

        Dim ds = (From subFolder In folders
                 Select New With {
                     .name = subFolder.Name,
                     .items = subFolder.ItemCount,
                     .url = subFolder.ServerRelativeUrl,
                     .tag = subFolder.Tag
                    }).ToList
        Return ds
    End Function

    Public Shared Function GetSubCategories(clientContext As ClientContext, cate As Folder) As Object
        Dim categories = cate.Folders
        clientContext.Load(categories)
        clientContext.ExecuteQuery()

        Dim ds = (From childCate In categories
                 Select New With {
                     .Key = childCate.Name,
                    .Group = GetFiles(clientContext, childCate)
                     }).ToList
        Return ds
    End Function

    Public Shared Function GetPDFContent(fileUrl As String) As Byte()
        Using ClientContext = GetClientContext()
            Dim f = File.OpenBinaryDirect(ClientContext, fileUrl)
            'Dim reader = New IO.BinaryReader(f.Stream)
            Using ms As New IO.MemoryStream()
                f.Stream.CopyTo(ms)
                Return ms.ToArray
            End Using
        End Using
    End Function

    Public Shared Function DownLoadFile(uniqueId As String) As Object
        Dim file = DownLoadFileStream(uniqueId)
        If file IsNot Nothing Then
            Return New With {
                .Stream = CType(file.Stream, IO.MemoryStream).ToArray,
                .Name = file.Name.ToString
                }
        End If

        Return Nothing
    End Function

    Public Shared Function DownLoadFileStream(uniqueId As String) As Object
        Using ClientContext = GetClientContext()
            'Dim file = ClientContext.Web.GetFileById(New Guid(uniqueId))
            'If file IsNot Nothing Then
            '    ClientContext.Load(file, Function(f As File) f.ServerRelativeUrl, Function(f As File) f.Name)

            '    Dim ms As New IO.MemoryStream
            '    Dim str = file.OpenBinaryStream()
            '    ClientContext.ExecuteQuery()

            '    str.Value.CopyTo(ms)
            '    ms.Position = 0
            '    Return New With {
            '    .Stream = ms,
            '    .Name = file.Name
            '}

            'End If

            Dim item = GetFileById(uniqueId, ClientContext)
            If item IsNot Nothing Then
                Dim tmpfile = item.File
                ClientContext.Load(tmpfile, Function(f As File) f.ServerRelativeUrl, Function(f As File) f.Name)

                Dim ms As New IO.MemoryStream
                Dim str = tmpfile.OpenBinaryStream()
                ClientContext.ExecuteQuery()

                str.Value.CopyTo(ms)
                ms.Position = 0
                Return New With {
                .Stream = ms,
                .Name = tmpfile.Name
            }
            End If

            Return Nothing
        End Using
    End Function

    Public Shared Function GetPreviewContentLink(uniqueId As String) As String
        Using ClientContext = GetClientContext()

            Dim item = GetFileById(uniqueId, ClientContext)
            If item IsNot Nothing Then
                Dim link = GetAnonymousViewLink(ClientContext, item)
                Dim file = item.File
                ClientContext.Load(file, Function(f As File) f.ServerRelativeUrl, Function(f As File) f.Name)
                ClientContext.ExecuteQuery()
                If String.IsNullOrEmpty(link) Then
                    CreateSharingLink(ClientContext, file.ServerRelativeUrl)
                    link = GetAnonymousViewLink(ClientContext, item)
                End If

                If file.Name.EndsWith("pdf", StringComparison.OrdinalIgnoreCase) Then
                    Dim fileLink = String.Format("/DownloadFile.aspx?pdfUrl={0}", file.ServerRelativeUrl)
                    link = "/pdfViewer/web/viewer.html?file=" + HttpContext.Current.Server.UrlEncode(fileLink)
                End If
                Return link
            End If

            Return ""
        End Using
    End Function

    Public Shared Function GetPreviewContentLinkByFileUrl(fileUrl As String) As String
        Using ClientContext = GetClientContext()

            Dim item = GetFileByUrl(fileUrl, ClientContext)

            If item IsNot Nothing Then
                Dim link = GetAnonymousEditLink(ClientContext, item)
                Dim file = item.File
                ClientContext.Load(file, Function(f As File) f.ServerRelativeUrl, Function(f As File) f.Name)
                ClientContext.ExecuteQuery()
                If String.IsNullOrEmpty(link) Then
                    CreateEditLink(ClientContext, file.ServerRelativeUrl)
                    link = GetAnonymousEditLink(ClientContext, item)
                End If

                Return link
            End If

            Return ""
        End Using
    End Function

    Public Shared Function GetTitleReport(bble As String) As String
        Using ClientContext = GetClientContext()
            Dim titleFolder = bble & "/Short%20Sale/Title%20Report"
            If Not IsFolderExsit(ClientContext, titleFolder) Then
                Return ""
            End If

            Dim folder = ClientContext.Web.GetFolderByServerRelativeUrl("/" & RootFolderName & "/" & titleFolder)
            Dim files = folder.Files
            ClientContext.Load(files)
            ClientContext.ExecuteQuery()

            If files.Count > 0 Then
                Return files(0).UniqueId.ToString
            End If

            Return ""
        End Using
    End Function

    Private Shared Function GetFileById(uniqueId As String, clientContext As ClientContext) As ListItem

        Dim list = clientContext.Web.Lists.GetByTitle(DocumentLibraryTitle)

        Dim f = clientContext.Web.GetFileById(New Guid(uniqueId))

        Return f.ListItemAllFields
    End Function

    Private Shared Function GetFileByUrl(fileUrl As String, clientContext As ClientContext) As ListItem
        Dim file = clientContext.Web.GetFileByServerRelativeUrl(fileUrl)
        clientContext.Load(file.ListItemAllFields)
        clientContext.ExecuteQuery()
        Return file.ListItemAllFields
    End Function

    Public Shared Function UploadFile(folderPath As String, fileBytes As Byte(), fileName As String, uploadBy As String) As String
        CreateFolder(folderPath)
        Dim fileUrl = String.Format("/{2}/{0}/{1}", folderPath, fileName, RootFolderName)

        Dim result = UploadFile(fileUrl, fileBytes, uploadBy)

        Try
            CreateSharingLink(GetClientContext, fileUrl)
        Catch ex As Exception
            Core.SystemLog.LogError("Create Sharing Link", ex, fileUrl, uploadBy, "")
            Return result
        End Try

        Return result
    End Function

    Public Shared Function UploadFile(fileUrl As String, fileBytes As Byte(), uploadBy As String) As String
        Using fs As New IO.MemoryStream(fileBytes)
            Microsoft.SharePoint.Client.File.SaveBinaryDirect(GetClientContext, fileUrl, fs, True)
        End Using

        Using ctx = GetClientContext()
            Dim file = ctx.Web.GetFileByServerRelativeUrl(fileUrl)

            file.ListItemAllFields("UploadBy") = uploadBy
            file.ListItemAllFields.Update()
            ctx.ExecuteQuery()
            Return fileUrl
        End Using
    End Function

    Public Shared Function CreateFolder(rootLibrary As String, folderUrl As String) As Folder
        Using ClientContext = GetClientContext()
            If Not IsFolderExsit(ClientContext, rootLibrary, folderUrl) Then
                Dim list = ClientContext.Web.Lists.GetByTitle(rootLibrary)
                Return EnsureFolder(ClientContext, list.RootFolder, folderUrl)
            End If
        End Using

        Return Nothing
    End Function

    Private Shared Function EnsureFolder(ctx As ClientContext, parentFolder As Folder, folderPath As String) As Folder
        Dim pathElements = folderPath.Split(New Char() {"/"}, StringSplitOptions.RemoveEmptyEntries)
        Dim head = pathElements(0)

        Dim newFolder = parentFolder.Folders.Add(head)
        ctx.Load(newFolder)
        ctx.ExecuteQuery()

        If pathElements.Length > 1 Then
            Dim childrenPath = String.Join("/", pathElements, 1, pathElements.Length - 1)
            Return EnsureFolder(ctx, newFolder, childrenPath)
        End If

        Return newFolder
    End Function

    Private Shared Function CreateFolder(folderUrl As String) As Folder
        ' Dim folders = folderUrl.Split(New Char() {"/"}, StringSplitOptions.RemoveEmptyEntries)

        Using ClientContext = GetClientContext()
            If Not IsFolderExsit(ClientContext, folderUrl) Then
                Dim list = ClientContext.Web.Lists.GetByTitle(DocumentLibraryTitle)
                Return EnsureFolder(ClientContext, list.RootFolder, folderUrl)
            End If
        End Using

        Return Nothing
    End Function

    Private Shared Function IsFolderExsit(ClientContext As ClientContext, folderUrl As String) As Boolean
        Return IsFolderExsit(ClientContext, RootFolderName, folderUrl)
    End Function

    Private Shared Function IsFolderExsit(ClientContext As ClientContext, rootLibrary As String, folderUrl As String) As Boolean
        If Not folderUrl.StartsWith(rootLibrary) Then
            folderUrl = rootLibrary & "/" & folderUrl
        End If

        Dim folder = ClientContext.Web.GetFolderByServerRelativeUrl(folderUrl)

        Try
            ClientContext.Load(folder)
            ClientContext.ExecuteQuery()
            Return True
        Catch ex As Exception
            Return False
        End Try

        Return True
    End Function

    Private Shared Function GetAnonymousEditLink(clientContext As ClientContext, item As ListItem)
        Dim objInfo = ObjectSharingInformation.GetObjectSharingInformation(clientContext, item, True, True, True, True, True, True, True)
        clientContext.Load(objInfo, Function(a As ObjectSharingInformation) a.AnonymousEditLink)
        clientContext.ExecuteQuery()

        Return objInfo.AnonymousEditLink
    End Function

    Private Shared Function GetAnonymousViewLink(clientContext As ClientContext, item As ListItem)
        Dim objInfo = ObjectSharingInformation.GetObjectSharingInformation(clientContext, item, True, True, True, True, True, True, True)
        clientContext.Load(objInfo, Function(a As ObjectSharingInformation) a.AnonymousViewLink)
        clientContext.ExecuteQuery()

        Return objInfo.AnonymousViewLink
    End Function

    Private Shared Sub CreateEditLink(clientContext As ClientContext, fileUrl As String)
        Dim serverurl = clientContext.Url
        Dim userRoleAssignments As New List(Of UserRoleAssignment)
        userRoleAssignments.Add(New UserRoleAssignment With
                                {.UserId = "Everyone",
                                    .Role = Sharing.Role.Edit
                                    })
        Dim returnValue = DocumentSharingManager.UpdateDocumentSharingInfo(clientContext, serverurl & fileUrl, userRoleAssignments, False, True, True, "", True, False)
        clientContext.ExecuteQuery()
    End Sub

    Private Shared Sub CreateSharingLink(clientContext As ClientContext, fileUrl As String)
        Dim serverurl = clientContext.Url
        Dim userRoleAssignments As New List(Of UserRoleAssignment)
        userRoleAssignments.Add(New UserRoleAssignment With
                                {.UserId = "Everyone",
                                    .Role = Sharing.Role.View
                                    })
        Dim returnValue = DocumentSharingManager.UpdateDocumentSharingInfo(clientContext, serverurl & fileUrl, userRoleAssignments, False, True, True, "", True, False)
        clientContext.ExecuteQuery()
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns></returns>
    Private Shared Function GetClientContext() As ClientContext
        Dim password As New SecureString
        Dim clientContext As New ClientContext(serverUrl)
        Array.ForEach(passwordstr.ToCharArray, Sub(c)
                                                   password.AppendChar(c)
                                               End Sub)

        Dim credentials As New SharePointOnlineCredentials(userName, password)
        clientContext.Credentials = credentials
        Return clientContext
    End Function

    Private Shared Function GetFiles(clientContext As ClientContext, cate As Folder) As Object
        Dim files = cate.Files
        clientContext.Load(files)

        Dim result = (From file In files
                      Select New With
                            {
                                .FileID = 1,
                                .Description = file.UniqueId.ToString,
                                .Name = file.Name,
                                .Size = file.Length,
                                .CreateDate = file.TimeCreated,
                                .Createby = GetFileCreateBy(file, clientContext)
                              })

        clientContext.ExecuteQuery()
        Return result.ToList
    End Function

    Private Shared Function GetFileCreateBy(file As File, ctx As ClientContext) As String
        ctx.Load(file, Function(f As File) f.ListItemAllFields.Item("UploadBy"))

        Try
            Return file.ListItemAllFields("UploadBy")
        Catch ex As Exception
            Return ""
        End Try
    End Function
End Class
