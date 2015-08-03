﻿Imports System.ServiceModel
Imports System.ServiceModel.Activation
Imports System.ServiceModel.Web
Imports System.Web.Script.Services
Imports IntranetPortal.ShortSale
Imports System.IO

<ServiceContract(Namespace:="")>
<AspNetCompatibilityRequirements(RequirementsMode:=AspNetCompatibilityRequirementsMode.Allowed)>
Public Class ContactService

    <OperationContract()>
    <WebGet()>
    Public Function GetContacts(args As String) As Channels.Message
        Dim p = PartyContact.SearchContacts(args)
        Return p.ToJson()
    End Function

    <OperationContract()>
   <WebGet(ResponseFormat:=WebMessageFormat.Json)>
    Public Function GetBankList() As Channels.Message
        Dim gp = GroupType.GetGroup(5)
        If gp IsNot Nothing Then
            Return gp.Contacts.Where(Function(a) (a.Disable Is Nothing Or a.Disable = False)).OrderBy(Function(p) p.Name).ToList.ToJson
        End If
        Return Nothing
    End Function

    <OperationContract()>
    <WebGet()>
    Public Function LoadContacts() As Channels.Message ' As List(Of PartyContact)
        Return PartyContact.getAllContact().ToJson()

    End Function

    <OperationContract()>
    <WebGet()>
    Public Function GetAllContacts(id As Integer) As Channels.Message ' As List(Of PartyContact)
        If (id = 0) Then
            Return PartyContact.getAllContact().ToJson()
        End If
        Dim p = PartyContact.getAllContact().Where(Function(ps) ps.ContactId = id)

        Return p.ToJson()
        ' Add your operation implementation here
    End Function

    <OperationContract()>
    Public Function CheckInShortSale(BBLE As String) As Channels.Message ' As List(Of PartyContact)

        Return ShortSaleCase.InShortSale(BBLE).ToJson
        ' Add your operation implementation here
    End Function

    <OperationContract()>
   <WebGet()>
    Public Function GetAllBuyerEntities() As Channels.Message
        Return ShortSale.CorporationEntity.GetAllEntities().OrderBy(Function(c) c.CorpName).ToJson
    End Function

    <OperationContract()>
    <WebInvoke(RequestFormat:=WebMessageFormat.Json, ResponseFormat:=WebMessageFormat.Json, BodyStyle:=WebMessageBodyStyle.WrappedRequest)>
    Public Function SaveCorpEntitiy(c As String) As Channels.Message
        If (Not String.IsNullOrEmpty(c)) Then
            Dim entity = Newtonsoft.Json.JsonConvert.DeserializeObject(Of ShortSale.CorporationEntity)(c)
            If (entity.EntityId = 0 AndAlso CorporationEntity.GetEntityByCorpName(entity.CorpName) IsNot Nothing) Then
                Return Nothing
            End If
            entity.Save()
            Return entity.ToJson
        End If
        Return Nothing
    End Function
    ' Add more operations here and mark them with <OperationContract()>

    <OperationContract()>
    <WebInvoke(Method:="POST", ResponseFormat:=WebMessageFormat.Json)>
    Public Function UploadFile(stream As Stream) As String

        Dim ms As New MemoryStream
        stream.CopyTo(ms)
        ms.Position = 0

        Dim id = HttpContext.Current.Request.QueryString("id")
        Dim fileName = HttpContext.Current.Request.QueryString("type")

        Return ShortSale.CorporationEntity.UploadFile(id, fileName & ".pdf", ms.ToArray, HttpContext.Current.User.Identity.Name)
    End Function
End Class