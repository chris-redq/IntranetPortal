'------------------------------------------------------------------------------
' <auto-generated>
'     This code was generated from a template.
'
'     Manual changes to this file may cause unexpected behavior in your application.
'     Manual changes to this file will be overwritten if the code is regenerated.
' </auto-generated>
'------------------------------------------------------------------------------

Imports System
Imports System.Collections.Generic

Partial Public Class LeadsActivityLog
    Public Property LogID As Integer
    Public Property BBLE As String
    Public Property EmployeeID As Integer
    Public Property Category As String
    Public Property EmployeeName As String
    Public Property ActivityDate As Nullable(Of Date)
    Public Property Comments As String
    Public Property ActionType As Nullable(Of Integer)

    Public Overridable Property Lead As Lead
    Public Overridable Property Employee As Employee

End Class
