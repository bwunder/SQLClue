﻿'------------------------------------------------------------------------------
' <auto-generated>
'    This code was generated from a template.
'
'    Manual changes to this file may cause unexpected behavior in your application.
'    Manual changes to this file will be overwritten if the code is regenerated.
' </auto-generated>
'------------------------------------------------------------------------------

Imports System
Imports System.Data.Entity
Imports System.Data.Entity.Infrastructure

Partial Public Class DefaultTraceContainer
    Inherits DbContext

    Public Sub New()
        MyBase.New("name=DefaultTraceContainer")
    End Sub

    Protected Overrides Sub OnModelCreating(modelBuilder As DbModelBuilder)
	    Throw New UnintentionalCodeFirstException()
    End Sub

    Public Property Databases() As DbSet(Of Databases)
    Public Property Logins() As DbSet(Of Logins)
    Public Property Applications() As DbSet(Of Applications)
    Public Property Hosts() As DbSet(Of Hosts)
    Public Property TraceEvents() As DbSet(Of TraceEvents)

End Class