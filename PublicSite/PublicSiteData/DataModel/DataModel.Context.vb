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

Partial Public Class PublicSiteEntities
    Inherits DbContext

    Public Sub New()
        MyBase.New("name=PublicSiteEntities")
    End Sub

    Protected Overrides Sub OnModelCreating(modelBuilder As DbModelBuilder)
        Throw New UnintentionalCodeFirstException()
    End Sub

    Public Overridable Property Agents() As DbSet(Of Agent)
    Public Overridable Property Neighbors() As DbSet(Of Neighbor)
    Public Overridable Property PropertyImages() As DbSet(Of PropertyImage)
    Public Overridable Property ListProperties() As DbSet(Of ListProperty)
    Public Overridable Property FeatureDatas() As DbSet(Of FeatureData)
    Public Overridable Property PropertyFeatures() As DbSet(Of PropertyFeature)
    Public Overridable Property PortalAgents() As DbSet(Of PortalAgent)

End Class