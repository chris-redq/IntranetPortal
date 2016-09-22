﻿'------------------------------------------------------------------------------
' <auto-generated>
'     This code was generated from a template.
'
'     Manual changes to this file may cause unexpected behavior in your application.
'     Manual changes to this file will be overwritten if the code is regenerated.
' </auto-generated>
'------------------------------------------------------------------------------

Imports System
Imports System.Data.Entity
Imports System.Data.Entity.Infrastructure

Partial Public Class PortalEntities
    Inherits DbContext

    Public Sub New()
        MyBase.New("name=PortalEntities")
    End Sub

    Protected Overrides Sub OnModelCreating(modelBuilder As DbModelBuilder)
        Throw New UnintentionalCodeFirstException()
    End Sub

    Public Overridable Property CleareneceNotes() As DbSet(Of CleareneceNote)
    Public Overridable Property PartyContacts() As DbSet(Of PartyContact)
    Public Overridable Property PropertyFloors() As DbSet(Of PropertyFloor)
    Public Overridable Property PropertyTitles() As DbSet(Of PropertyTitle)
    Public Overridable Property ShortSaleCases() As DbSet(Of ShortSaleCase)
    Public Overridable Property TitleClearences() As DbSet(Of TitleClearence)
    Public Overridable Property PropertyBaseInfoes() As DbSet(Of PropertyBaseInfo)
    Public Overridable Property ShortSaleCaseComments() As DbSet(Of ShortSaleCaseComment)
    Public Overridable Property PropertyOccupants() As DbSet(Of PropertyOccupant)
    Public Overridable Property TitleJudgementSearches() As DbSet(Of TitleJudgementSearch)
    Public Overridable Property EvictionCases() As DbSet(Of EvictionCas)
    Public Overridable Property Employees() As DbSet(Of Employee)
    Public Overridable Property ShortSaleActivityLogs() As DbSet(Of ShortSaleActivityLog)
    Public Overridable Property MortgageStatusDatas() As DbSet(Of MortgageStatusData)
    Public Overridable Property GroupTypes() As DbSet(Of GroupType)
    Public Overridable Property PropertyValueInfoes() As DbSet(Of PropertyValueInfo)
    Public Overridable Property PropertyMortgages() As DbSet(Of PropertyMortgage)
    Public Overridable Property PropertyOwners() As DbSet(Of PropertyOwner)
    Public Overridable Property ShortSaleOverviews() As DbSet(Of ShortSaleOverview)
    Public Overridable Property ShortSaleBuyers() As DbSet(Of ShortSaleBuyer)
    Public Overridable Property ShortSaleOffers() As DbSet(Of ShortSaleOffer)
    Public Overridable Property CorporationEntities() As DbSet(Of CorporationEntity)
    Public Overridable Property ShortSaleCheckLists() As DbSet(Of ShortSaleCheckList)
    Public Overridable Property LegalCases() As DbSet(Of LegalCase)
    Public Overridable Property LegalECourts() As DbSet(Of LegalECourt)
    Public Overridable Property LegalJudges() As DbSet(Of LegalJudge)
    Public Overridable Property LegalRoboSignors() As DbSet(Of LegalRoboSignor)
    Public Overridable Property LawReferences() As DbSet(Of LawReference)
    Public Overridable Property LeadInfoDocumentSearches() As DbSet(Of LeadInfoDocumentSearch)
    Public Overridable Property CheckingComplains() As DbSet(Of CheckingComplain)
    Public Overridable Property ConstructionBudgets() As DbSet(Of ConstructionBudget)
    Public Overridable Property ConstructionCases() As DbSet(Of ConstructionCase)
    Public Overridable Property ConstructionInitialForms() As DbSet(Of ConstructionInitialForm)
    Public Overridable Property ConstructionSpotChecks() As DbSet(Of ConstructionSpotCheck)
    Public Overridable Property ConstructionViolations() As DbSet(Of ConstructionViolation)
    Public Overridable Property FormDataItems() As DbSet(Of FormDataItem)
    Public Overridable Property TitleCases() As DbSet(Of TitleCase)
    Public Overridable Property UserFollowUps() As DbSet(Of UserFollowUp)
    Public Overridable Property DataStatus() As DbSet(Of DataStatu)
    Public Overridable Property LegalECourtsAlls() As DbSet(Of LegalECourtsAll)
    Public Overridable Property AuctionProperties() As DbSet(Of AuctionProperty)
    Public Overridable Property AuctionPropertyViews() As DbSet(Of AuctionPropertyView)
    Public Overridable Property LegalCaseReports() As DbSet(Of LegalCaseReport)
    Public Overridable Property LegalManagerReports() As DbSet(Of LegalManagerReport)
    Public Overridable Property SSFirstMortgages() As DbSet(Of SSFirstMortgage)
    Public Overridable Property BusinessChecks() As DbSet(Of BusinessCheck)
    Public Overridable Property CheckRequests() As DbSet(Of CheckRequest)
    Public Overridable Property PreSignRecords() As DbSet(Of PreSignRecord)
    Public Overridable Property DeedCorps() As DbSet(Of DeedCorp)
    Public Overridable Property DeedCorpProperties() As DbSet(Of DeedCorpProperty)
    Public Overridable Property ShortSaleLeadsInfoes() As DbSet(Of ShortSaleLeadsInfo)
    Public Overridable Property PropertyOffers() As DbSet(Of PropertyOffer)
    Public Overridable Property AuditLogs() As DbSet(Of AuditLog)
    Public Overridable Property SSLeads() As DbSet(Of SSLead)
    Public Overridable Property SSLeadsStatusLogs() As DbSet(Of SSLeadsStatusLog)
    Public Overridable Property UnderwritingRequests() As DbSet(Of UnderwritingRequest)

End Class
