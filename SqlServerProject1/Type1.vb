Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.SqlTypes
Imports Microsoft.SqlServer.Server

<Serializable()> _
<Microsoft.SqlServer.Server.SqlUserDefinedType(Format.Native)> _
Public Structure Type1
    Implements INullable

    Public Overrides Function ToString() As String
        ' Put your code here
        Return ""
    End Function

    Public ReadOnly Property IsNull() As Boolean Implements INullable.IsNull
        Get
            ' Put your code here
            Return m_Null
        End Get
    End Property

    Public Shared ReadOnly Property Null As Type1
        Get
            Dim h As Type1 = New Type1
            h.m_Null = True
            Return h
        End Get
    End Property

    Public Shared Function Parse(ByVal s As SqlString) As Type1
        If s.IsNull Then
            Return Null
        End If

        Dim u As Type1 = New Type1
        ' Put your code here
        Return u
    End Function

    ' This is a place-holder method
    Public Function Method1() As String
        ' Put your code here
        Return "Hello"
    End Function

    ' This is a place-holder static method
    Public Shared Function Method2() As SqlString
        ' Put your code here
        Return New SqlString("Hello")
    End Function

    ' This is a place-holder field member
    Public m_var1 As Integer
    ' Private member
    Private m_Null As Boolean
End Structure

