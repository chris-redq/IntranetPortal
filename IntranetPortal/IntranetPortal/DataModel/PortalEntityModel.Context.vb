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
Imports System.Data.Entity.Core.Objects
Imports System.Linq

Partial Public Class Entities
    Inherits DbContext

    Public Sub New()
        MyBase.New("name=Entities")
    End Sub

    Protected Overrides Sub OnModelCreating(modelBuilder As DbModelBuilder)
        Throw New UnintentionalCodeFirstException()
    End Sub

    Public Overridable Property Employees() As DbSet(Of Employee)
    Public Overridable Property LeadsActivityLogs() As DbSet(Of LeadsActivityLog)
    Public Overridable Property Leads() As DbSet(Of Lead)
    Public Overridable Property LeadsInfoes() As DbSet(Of LeadsInfo)
    Public Overridable Property Roles() As DbSet(Of Role)
    Public Overridable Property UsersInRoles() As DbSet(Of UsersInRole)
    Public Overridable Property FileAttachments() As DbSet(Of FileAttachment)
    Public Overridable Property UserTasks() As DbSet(Of UserTask)
    Public Overridable Property UserAppointments() As DbSet(Of UserAppointment)
    Public Overridable Property OrderResponses() As DbSet(Of OrderRespons)
    Public Overridable Property APIOrders() As DbSet(Of APIOrder)
    Public Overridable Property UserMessages() As DbSet(Of UserMessage)
    Public Overridable Property NYC_St_Names() As DbSet(Of NYC_St_Names)
    Public Overridable Property PortalQuotes() As DbSet(Of PortalQuote)
    Public Overridable Property OwnerContacts() As DbSet(Of OwnerContact)
    Public Overridable Property Agent_Properties() As DbSet(Of Agent_Properties)
    Public Overridable Property HomeOwners() As DbSet(Of HomeOwner)
    Public Overridable Property LoginLogs() As DbSet(Of LoginLog)
    Public Overridable Property SharedLeads() As DbSet(Of SharedLead)
    Public Overridable Property UserProfileDatas() As DbSet(Of UserProfileData)
    Public Overridable Property BuildingCodes() As DbSet(Of BuildingCode)
    Public Overridable Property Offices() As DbSet(Of Office)
    Public Overridable Property LeadsComments() As DbSet(Of LeadsComment)
    Public Overridable Property PortalNotes() As DbSet(Of PortalNote)
    Public Overridable Property PropertyReferrels() As DbSet(Of PropertyReferrel)
    Public Overridable Property LeadsMortgageDatas() As DbSet(Of LeadsMortgageData)
    Public Overridable Property HomeOwnerAddresses() As DbSet(Of HomeOwnerAddress)
    Public Overridable Property AssginRulesLogs() As DbSet(Of AssginRulesLog)
    Public Overridable Property AssignRules() As DbSet(Of AssignRule)
    Public Overridable Property Teams() As DbSet(Of Team)
    Public Overridable Property UserInTeams() As DbSet(Of UserInTeam)
    Public Overridable Property LeadsTaxLiens() As DbSet(Of LeadsTaxLien)
    Public Overridable Property HomeOwnerEmails() As DbSet(Of HomeOwnerEmail)
    Public Overridable Property NYC_DATA_COMMENT() As DbSet(Of NYC_DATA_COMMENT)
    Public Overridable Property LeadsSearchTasks() As DbSet(Of LeadsSearchTask)
    Public Overridable Property SearchResults() As DbSet(Of SearchResult)
    Public Overridable Property ApartmentBuildings() As DbSet(Of ApartmentBuilding)
    Public Overridable Property Apartments() As DbSet(Of Apartment)
    Public Overridable Property LatLon_View() As DbSet(Of LatLon_View)
    Public Overridable Property LeadsAssignViews() As DbSet(Of LeadsAssignView)
    Public Overridable Property LeadsAssignView2() As DbSet(Of LeadsAssignView2)
    Public Overridable Property MapDataSets() As DbSet(Of MapDataSet)
    Public Overridable Property LeadsStatusLogs() As DbSet(Of LeadsStatusLog)
    Public Overridable Property TeamLeadsInZips() As DbSet(Of TeamLeadsInZip)
    Public Overridable Property PendingAssignLeads() As DbSet(Of PendingAssignLead)
    Public Overridable Property Leads_with_last_log() As DbSet(Of Leads_with_last_log)
    Public Overridable Property LeadsInThirdParties() As DbSet(Of LeadsInThirdParty)
    Public Overridable Property LeadsActivityLogArchiveds() As DbSet(Of LeadsActivityLogArchived)
    Public Overridable Property NYC_Scan_TaxLiens_Per_Year() As DbSet(Of NYC_Scan_TaxLiens_Per_Year)
    Public Overridable Property InProcessBBLEs() As DbSet(Of InProcessBBLE)
    Public Overridable Property TLODatas() As DbSet(Of TLOData)
    Public Overridable Property HomeOwnerPhones() As DbSet(Of HomeOwnerPhone)
    Public Overridable Property PortalLisPens() As DbSet(Of PortalLisPen)
    Public Overridable Property PortalLeadsViews() As DbSet(Of PortalLeadsView)

    Public Overridable Function UpdateEmployeeName(oldName As String, newName As String) As Integer
        Dim oldNameParameter As ObjectParameter = If(oldName IsNot Nothing, New ObjectParameter("OldName", oldName), New ObjectParameter("OldName", GetType(String)))

        Dim newNameParameter As ObjectParameter = If(newName IsNot Nothing, New ObjectParameter("NewName", newName), New ObjectParameter("NewName", GetType(String)))

        Return DirectCast(Me, IObjectContextAdapter).ObjectContext.ExecuteFunction("UpdateEmployeeName", oldNameParameter, newNameParameter)
    End Function

    Public Overridable Function UploadSearchInfo2Portal() As Integer
        Return DirectCast(Me, IObjectContextAdapter).ObjectContext.ExecuteFunction("UploadSearchInfo2Portal")
    End Function

    Public Overridable Function RemoteLeadsQuery(query As String) As ObjectResult(Of String)
        Dim queryParameter As ObjectParameter = If(query IsNot Nothing, New ObjectParameter("query", query), New ObjectParameter("query", GetType(String)))

        Return DirectCast(Me, IObjectContextAdapter).ObjectContext.ExecuteFunction(Of String)("RemoteLeadsQuery", queryParameter)
    End Function

End Class
