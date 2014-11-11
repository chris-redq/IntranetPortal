﻿Imports System.Text.RegularExpressions

Partial Public Class ShortSaleCase

#Region "Constructor"

    Public Sub New(propBaseinfo As PropertyBaseInfo)
        _propInfo = propBaseinfo
    End Sub

    Public Sub New()

    End Sub

#End Region

#Region "Properties"
    Private _propInfo As PropertyBaseInfo
    Public ReadOnly Property PropertyInfo As PropertyBaseInfo
        Get
            If _propInfo Is Nothing Then
                _propInfo = PropertyBaseInfo.GetInstance(BBLE)
                '_propInfo.StreetName
                If _propInfo Is Nothing Then
                    _propInfo = New PropertyBaseInfo
                End If
            End If

            Return _propInfo
        End Get
    End Property

    Private _judgementInfo As TitleJudgementSearch
    Public ReadOnly Property JudgementInfo
        Get
            If _judgementInfo Is Nothing Then
                _judgementInfo = TitleJudgementSearch.GetInstance(CaseId)

                If _judgementInfo Is Nothing Then
                    _judgementInfo = New TitleJudgementSearch
                    _judgementInfo.CaseId = CaseId
                End If
            End If

            Return _judgementInfo
        End Get
    End Property

    Private _mortgages As List(Of PropertyMortgage)
    Public ReadOnly Property Mortgages As List(Of PropertyMortgage)
        Get
            If _mortgages Is Nothing Then
                Using context As New ShortSaleEntities
                    _mortgages = context.PropertyMortgages.Where(Function(mg) mg.CaseId = CaseId).ToList
                End Using
            End If

            Return _mortgages
        End Get
    End Property

    Private _sellerTitle As PropertyTitle
    Public ReadOnly Property SellerTitle As PropertyTitle
        Get
            If _sellerTitle Is Nothing Then
                _sellerTitle = PropertyTitle.GetTitle(CaseId, PropertyTitle.TitleType.Seller)
            End If

            Return _sellerTitle
        End Get
    End Property

    Private _buyerTitle As PropertyTitle
    Public ReadOnly Property BuyerTitle As PropertyTitle
        Get
            If _buyerTitle Is Nothing Then
                _buyerTitle = PropertyTitle.GetTitle(CaseId, PropertyTitle.TitleType.Buyer)
            End If

            Return _buyerTitle
        End Get
    End Property

    Private _clearences As List(Of TitleClearence)
    Public ReadOnly Property Clearences As List(Of TitleClearence)
        Get
            Return TitleClearence.GetCaseClearences(CaseId)
        End Get
    End Property

    Private _processorContact As PartyContact
    Public ReadOnly Property ProcessorContact As PartyContact
        Get
            If _processorContact Is Nothing AndAlso Processor.HasValue Then
                _processorContact = PartyContact.GetContact(Processor)
            End If

            Return _processorContact
        End Get
    End Property

    Private _comments As List(Of ShortSaleCaseComment)
    Public ReadOnly Property Comments As List(Of ShortSaleCaseComment)
        Get
            If _comments Is Nothing Then
                _comments = ShortSaleCaseComment.GetCaseComments(CaseId)
            End If

            Return _comments
        End Get
    End Property

    Private _upComingDate As DateTime
    Public ReadOnly Property UpComingBPODate As DateTime?
        Get
            If Mortgages IsNot Nothing AndAlso Mortgages.Count > 0 Then
                Return Mortgages(0).UpcomingBPODate
            End If

            Return Nothing
        End Get
    End Property

    Public ReadOnly Property AssignedProcessor As PartyContact
        Get
            If Processor.HasValue Then
                Return PartyContact.GetContact(Processor)
            Else
                Return New PartyContact()
            End If
        End Get
    End Property

    Public ReadOnly Property ReferralContact As PartyContact
        Get
            If Referral.HasValue Then
                Return PartyContact.GetContact(Referral)
            Else
                Return New PartyContact
            End If
        End Get
    End Property

    Public ReadOnly Property ListingAgentContact As PartyContact
        Get
            If ListingAgent.HasValue Then
                Return PartyContact.GetContact(ListingAgent)
            Else
                Return New PartyContact
            End If
        End Get
    End Property

    Public ReadOnly Property Manager As String
        Get
            Return ""
        End Get
    End Property

    Public ReadOnly Property BuyerContact As PartyContact
        Get
            If Buyer.HasValue Then
                Return PartyContact.GetContact(Buyer)
            Else
                Return New PartyContact
            End If
        End Get
    End Property

    Public ReadOnly Property FristMortageProgress As String
        Get
            Return GetMortageStauts(0)

        End Get
    End Property
    Public ReadOnly Property OwnerLastName As String
        Get
            If (Owner IsNot Nothing AndAlso Owner.Length > 0) Then
                Dim mathces = Regex.Matches(Owner, "\s(\w+)$")
                If (mathces.Count > 0) Then
                    Return mathces(0).Value
                End If
            End If
            Return Nothing
        End Get
    End Property
    Function GetMortageStauts(ByVal index As Integer) As String
        If (Mortgages IsNot Nothing AndAlso Mortgages.Count > index) Then

            Return Mortgages(index).Status
        End If
        Return Nothing
    End Function
    Public ReadOnly Property FristMortageLender()
        Get
            Return GetMortageLonder(0)
        End Get
    End Property
    Public ReadOnly Property SencondMortageLender()
        Get
            Return GetMortageLonder(1)
        End Get
    End Property

    Function GetMortageLonder(ByVal index As Integer) As String
        If (Mortgages IsNot Nothing AndAlso Mortgages.Count > index) Then

            Return Mortgages(index).Lender
        End If
        Return Nothing
    End Function
    
    Public ReadOnly Property SencondMortageProgress As String
        Get
            Return GetMortageStauts(1)
        End Get
    End Property

    Public ReadOnly Property SellerAttorneyContact As PartyContact
        Get
            If SellerAttorney.HasValue Then
                Return PartyContact.GetContact(SellerAttorney)
            Else
                Return New PartyContact
            End If
        End Get
    End Property

    Public ReadOnly Property BuyerAttorneyContact As PartyContact
        Get
            If BuyerAttorney.HasValue Then
                Return PartyContact.GetContact(BuyerAttorney)
            Else
                Return New PartyContact
            End If
        End Get
    End Property

    Public ReadOnly Property TitleCompanyContact As PartyContact
        Get
            If TitleCompany.HasValue Then
                Return PartyContact.GetContact(TitleCompany)
            Else
                Return New PartyContact
            End If
        End Get
    End Property

    Private _occupants As List(Of PropertyOccupant)
    Public Property Occupants As List(Of PropertyOccupant)
        Get
            If _occupants Is Nothing Then
                _occupants = PropertyOccupant.GetOccupantsByCase(CaseId)
            End If

            Return _occupants
        End Get
        Set(value As List(Of PropertyOccupant))
            _occupants = value
        End Set
    End Property

    Public ReadOnly Property LastComments As String
        Get
            Return ""
        End Get
    End Property

#End Region

#Region "Methods"

    Public Sub Save()
        Using context As New ShortSaleEntities
            If CaseId = 0 Then
                If Not String.IsNullOrEmpty(BBLE) Then
                    Dim tmpCase = context.ShortSaleCases.Where(Function(ss) ss.BBLE = BBLE).FirstOrDefault
                    If tmpCase IsNot Nothing Then
                        CaseId = tmpCase.CaseId
                    End If
                End If
            End If

            If CaseId = 0 Then
                CreateDate = DateTime.Now
                context.Entry(Me).State = Entity.EntityState.Added
            Else
                Dim obj = context.ShortSaleCases.Find(CaseId)
                obj = ShortSaleUtility.SaveChangesObj(obj, Me)
                context.SaveChanges()
            End If

            context.SaveChanges()

            If _mortgages IsNot Nothing Then
                For Each mg In _mortgages
                    If mg.CaseId = 0 Then
                        mg.CaseId = CaseId
                    End If

                    mg.Save()
                Next
            End If

            If _propInfo IsNot Nothing Then
                _propInfo.Save()
            End If

            If _judgementInfo IsNot Nothing Then
                _judgementInfo.Save()
            End If

            If _sellerTitle IsNot Nothing Then
                _sellerTitle.CaseId = CaseId
                _sellerTitle.Type = PropertyTitle.TitleType.Seller
                _sellerTitle.Save()
            End If

            If _buyerTitle IsNot Nothing Then
                _buyerTitle.CaseId = CaseId
                _buyerTitle.Type = PropertyTitle.TitleType.Buyer
                _buyerTitle.Save()
            End If

            If _occupants IsNot Nothing Then
                For Each opt In _occupants
                    If Not opt.CaseId.HasValue Then
                        opt.CaseId = CaseId
                    End If
                    opt.Save()

                Next
            End If

        End Using
    End Sub

    Public Sub SaveStatus(status As CaseStatus)
        Me.Status = status
        Save()
    End Sub

    Public Sub SaveFollowUp(dt As DateTime)
        Me.Status = CaseStatus.FollowUp
        Me.CallbackDate = dt
        Save()
    End Sub

    Public Sub SaveChanges()
        Save()
        'If CaseId > 0 Then
        '    Using context As New ShortSaleEntities
        '        Dim origCase = context.ShortSaleCases.Find(CaseId)
        '        origCase = Utility.SaveChangesObj(origCase, Me)
        '        context.SaveChanges()
        '    End Using
        'End If
    End Sub


    Public Shared Sub ReassignOwner(caseId As Integer, owner As String)
        Dim ssCase = GetCase(caseId)
        If ssCase Is Nothing Then
            Throw New Exception("Can't find short sale case. Case Id is " & caseId)
        End If

        ssCase.ReassignOwner(owner)
    End Sub

    Public Shared Sub ReassignOwner(bble As String, owner As String)
        Dim ssCase = GetCaseByBBLE(bble)
        If ssCase Is Nothing Then
            Throw New Exception("Can't find short sale case. BBLE is " & bble)
        End If

        ssCase.ReassignOwner(owner)
    End Sub

    Private Sub ReAssignOwner(owner As String)
        Me.Owner = owner
        Me.Status = CaseStatus.NewFile
        Save()
    End Sub

    Public Shared Function SaveCase(ssCase As ShortSaleCase) As Boolean
        Try
            ssCase.Save()
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Shared Function DeleteCase(caseId As Integer) As Boolean
        Return True
    End Function

    Public Shared Function GetCase(caseId As Integer) As ShortSaleCase
        Using context As New ShortSaleEntities
            Return context.ShortSaleCases.Find(caseId)
        End Using
    End Function

    Public Shared Function GetCaseByBBLE(bble As String) As ShortSaleCase
        Using context As New ShortSaleEntities
            Return context.ShortSaleCases.Where(Function(ss) ss.BBLE = bble).SingleOrDefault
        End Using
    End Function

    Public Shared Function GetAllCase() As List(Of ShortSaleCase)
        Using context As New ShortSaleEntities
            Return context.ShortSaleCases.ToList
        End Using
    End Function

    Public Shared Function GetCaseByStatus(status As CaseStatus) As List(Of ShortSaleCase)
        Using context As New ShortSaleEntities
            Return context.ShortSaleCases.Where(Function(ss) ss.Status = status).ToList
        End Using
    End Function

    Public Shared Function GetCaseByStatus(status As CaseStatus, owner As String) As List(Of ShortSaleCase)
        Using context As New ShortSaleEntities
            Return context.ShortSaleCases.Where(Function(ss) ss.Status = status AndAlso ss.Owner = owner).ToList
        End Using
    End Function

    Public Shared Function GetCaseCount(status As CaseStatus, owner As String) As Integer
        Return GetCaseByStatus(status, owner).Count
    End Function

    Public Shared Function GetCaseCount(status As CaseStatus) As Integer
        Return GetCaseByStatus(status).Count
    End Function
#End Region

End Class

Public Enum CaseStatus
    NewFile = 0
    FollowUp = 1
    Active = 2
    Eviction = 3
    OnHold = 4
    Closed = 5
End Enum

Public Enum ModelStatus
    Original = 0
    Added = 1
    Modified = 2
    Deleted = 3
End Enum
