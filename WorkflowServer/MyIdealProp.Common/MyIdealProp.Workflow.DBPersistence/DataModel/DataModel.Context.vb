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

Partial Public Class DataModelContainer
    Inherits DbContext

    Public Sub New()
        MyBase.New("name=DataModelContainer")
    End Sub

    Protected Overrides Sub OnModelCreating(modelBuilder As DbModelBuilder)
        Throw New UnintentionalCodeFirstException()
    End Sub

    Public Overridable Property ProcessSchemes() As DbSet(Of ProcessScheme)
    Public Overridable Property ActivityInstances() As DbSet(Of ActivityInstance)
    Public Overridable Property DataFields() As DbSet(Of DataField)
    Public Overridable Property EventInstances() As DbSet(Of EventInstance)
    Public Overridable Property LineInstances() As DbSet(Of LineInstance)
    Public Overridable Property Worklists() As DbSet(Of Worklist)
    Public Overridable Property EscalationInstances() As DbSet(Of EscalationInstance)
    Public Overridable Property ProcessInstances() As DbSet(Of ProcessInstance)

End Class
