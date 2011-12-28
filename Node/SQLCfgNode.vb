Option Explicit On
Option Strict On
Option Compare Binary

Imports System
Imports System.Data.SqlTypes
Imports Microsoft.SqlServer.Server
Imports System.Text


<System.Serializable(), _
 Microsoft.SqlServer.Server.SqlUserDefinedType( _
        Format.UserDefined, _
        IsByteOrdered:=True, _
        IsFixedLength:=False, _
        MaxByteSize:=768, _
        Name:="SQLCfgNode")> _
Public Structure SQLCfgNode
    Implements INullable, IBinarySerialize

    Private Const _NULL As String = "NULL"
    Private m_Null As Boolean
    Private m_Node As String
    Private m_Strings As String()
    Private m_Type As String
    Private m_SubType As String
    Private m_Length As Int32
    Private m_SQLInstance As String
    Private m_Database As String
    Private m_Collection As String
    Private m_Path As String
    Private m_Item As String

    <Microsoft.SqlServer.Server.SqlMethod(IsDeterministic:=True, _
                                          IsPrecise:=True, _
                                          DataAccess:=Microsoft.SqlServer.Server.DataAccessKind.None, _
                                          SystemDataAccess:=Microsoft.SqlServer.Server.SystemDataAccessKind.None)> _
    Public Overrides Function ToString() As String
        If Me.IsNull Then
            Return _NULL
        Else
            Return Me.m_Node.ToString
        End If
    End Function

    Public ReadOnly Property IsNull() As Boolean Implements INullable.IsNull
        Get
            Return m_Null
        End Get
    End Property

    Public Shared ReadOnly Property Null() As SQLCfgNode
        Get
            Dim h As SQLCfgNode = New SQLCfgNode
            h.m_Null = True
            Return h
        End Get
    End Property

    <Microsoft.SqlServer.Server.SqlMethod(IsDeterministic:=True, _
                                          IsPrecise:=True, _
                                          DataAccess:=Microsoft.SqlServer.Server.DataAccessKind.None, _
                                          SystemDataAccess:=Microsoft.SqlServer.Server.SystemDataAccessKind.None)> _
    Public Shared Function Parse(ByVal s As SqlString) As SQLCfgNode
        If s.IsNull Then
            Return Null
        End If
        Dim u As SQLCfgNode = New SQLCfgNode
        u.m_Node = s.ToString
        u.m_Strings = u.m_Node.Split(CChar("|"))
        u.m_Length = u.m_Strings.Length
        Select Case u.m_Strings(0)
            Case Is = "SQLCfgMetadata"
                ' here the item is the version
                u.m_Type = "Metadata"
                u.m_Collection = u.m_Strings(1)
                Select Case u.m_Collection
                    Case "SQLCfg.tSQLCfg"
                        u.m_SQLInstance = ""
                        u.m_Database = ""
                        u.m_SubType = "License"
                        u.m_Item = "License"
                    Case "SQLCfg.tServiceBroker"
                        u.m_SubType = "ServiceBroker"
                        u.m_SQLInstance = u.m_Strings(2)
                        u.m_Database = u.m_Strings(3)
                        u.m_Item = u.m_Strings(3)
                    Case "SQLCfg.tDb"
                        u.m_SubType = "Database"
                        u.m_SQLInstance = u.m_Strings(2)
                        u.m_Database = u.m_Strings(3)
                        u.m_Item = u.m_Strings(3)
                    Case "SQLCfg.tJobServer"
                        u.m_SubType = "JobServer"
                        u.m_SQLInstance = u.m_Strings(2)
                        u.m_Database = ""
                        u.m_Item = u.m_Strings(2)
                    Case "SQLCfg.tInstance"
                        u.m_SubType = "Server"
                        u.m_SQLInstance = u.m_Strings(2)
                        u.m_Database = ""
                        u.m_Item = u.m_Strings(2)
                    Case "SQLCfg.tConnection"
                        u.m_SubType = "Connection"
                        u.m_SQLInstance = u.m_Strings(2)
                        u.m_Database = ""
                        u.m_Item = u.m_Strings(2)
                    Case "SQLCfg.tSchedule"
                        u.m_SubType = "Schedule"
                        u.m_SQLInstance = u.m_Strings(2)
                        u.m_Database = ""
                        u.m_Item = u.m_Strings(2)
                End Select
            Case Else
                u.m_Type = "SQLInstance"
                u.m_SQLInstance = u.m_Strings(0)
                ' these overwritten below as appropriate
                u.m_SubType = "Server"
                u.m_Database = ""
                u.m_Collection = ""
                u.m_Path = ""
                u.m_Item = ""
                If u.m_Strings.Length = 1 Then
                    u.m_Item = u.m_Strings(0)
                Else
                    ' from here there is always a path|item
                    If u.m_Strings(1) = "ActiveDirectory" _
                    Or u.m_Strings(1) = "Configuration" _
                    Or u.m_Strings(1) = "FullTextService" _
                    Or u.m_Strings(1) = "Information" _
                    Or u.m_Strings(1) = "Mail" _
                    Or u.m_Strings(1) = "ProxyAccount" _
                    Or u.m_Strings(1) = "ResourceGovernor" _
                    Or u.m_Strings(1) = "Settings" _
                    Or u.m_Strings(1) = "TargetServers" Then
                        ' only one possible level
                        u.m_Item = u.m_Strings(1)
                    ElseIf u.m_Strings(1) = "BackupDevices" _
                    Or u.m_Strings(1) = "Audits" _
                    Or u.m_Strings(1) = "Credentials" _
                    Or u.m_Strings(1) = "CryptographicProviders" _
                    Or u.m_Strings(1) = "Endpoints" _
                    Or u.m_Strings(1) = "LinkedServers" _
                    Or u.m_Strings(1) = "Logins" _
                    Or u.m_Strings(1) = "Roles" _
                    Or u.m_Strings(1) = "ServerAuditSpecifications" _
                    Or u.m_Strings(1) = "Triggers" _
                    Or u.m_Strings(1) = "UserDefinedMessages" Then
                        ' with or without an item is still a valid node
                        u.m_Collection = u.m_Strings(1)
                        If u.m_Strings.Length = 3 Then
                            u.m_Item = u.m_Strings(2)
                        End If
                    ElseIf u.m_Strings(1) = "JobServer" Then
                        ' Jobserver is Server type
                        If u.m_Strings.Length = 2 Then
                            u.m_Item = u.m_Strings(1)
                        Else
                            ' enter the jobserver subtype
                            u.m_SubType = "JobServer"
                            If u.m_Strings(2) = "AlertSystem" Then
                                u.m_Item = u.m_Strings(2)
                            Else
                                u.m_Collection = u.m_Strings(2)
                            End If
                            If u.m_Strings.Length = 4 Then
                                u.m_Item = u.m_Strings(3)
                            End If
                        End If
                    ElseIf u.m_Strings(1) = "Databases" Then
                        ' the Databases collection  is Server type
                        If u.m_Strings.Length = 2 Then
                            u.m_Collection = u.m_Strings(1)
                        ElseIf u.m_Strings.Length = 3 Then
                            'a database in the database collection is Server type
                            u.m_Collection = u.m_Strings(1)
                            u.m_Item = u.m_Strings(2)
                        Else
                            ' enter the database subtype
                            u.m_SubType = "Database"
                            u.m_Database = u.m_Strings(2)
                            ' an item just under the db
                            If u.m_Strings(3) = "ActiveDirectory" _
                            Or u.m_Strings(3) = "DatabaseOptions" Then
                                u.m_Item = u.m_Strings(3)
                            ElseIf u.m_Strings(3) = "ServiceBroker" Then
                                ' SQLInstance|Databases|db name|ServiceBroker|Queueus|EventNotifications
                                ' SB has no items but has properties to document
                                If u.m_Strings.Length = 4 Then
                                    u.m_Item = u.m_Strings(3)
                                Else
                                    ' enter subtype servicebroker
                                    u.m_SubType = "ServiceBroker"
                                    u.m_Collection = u.m_Strings(4)
                                    If u.m_Strings.Length = 6 Then
                                        u.m_Item = u.m_Strings(5)
                                    End If
                                End If
                            Else
                                ' still subtype database here
                                u.m_Collection = u.m_Strings(3)
                                If u.m_Strings.Length = 5 Then
                                    u.m_Item = u.m_Strings(4)
                                End If
                            End If
                        End If
                    End If
                End If ' length > 1
        End Select
        u.m_Path = u.m_Node.Replace("|" & u.m_Item, "")
        Return u
    End Function

    Public Sub Read(ByVal r As System.IO.BinaryReader) _
      Implements Microsoft.SqlServer.Server.IBinarySerialize.Read
        m_Node = r.ReadString
    End Sub

    Public Sub Write(ByVal w As System.IO.BinaryWriter) _
     Implements Microsoft.SqlServer.Server.IBinarySerialize.Write
        w.Write(m_Node)
    End Sub

    Public ReadOnly Property Length() As Int32
        Get
            Return (Parse(m_Node).m_Length)
        End Get
    End Property

    Public ReadOnly Property Type() As String
        Get
            Return (Parse(m_Node).m_Type)
        End Get
    End Property

    Public ReadOnly Property SubType() As String
        Get
            'Server, JobServer, Database, ServiceBroker
            Return (Parse(m_Node).m_SubType)
        End Get
    End Property

    Public ReadOnly Property SQLInstance() As String
        Get
            Return (Parse(m_Node).m_SQLInstance)
        End Get
    End Property

    Public ReadOnly Property Database() As String
        Get
            Return (Parse(m_Node).m_Database)
        End Get
    End Property

    Public ReadOnly Property Collection() As String
        Get
            Return (Parse(m_Node).m_Collection)
        End Get
    End Property

    Public ReadOnly Property IsCollection() As Boolean
        Get
            Return If((Not (Parse(m_Node).m_Collection = "") And (Parse(m_Node).m_Item = "")), True, False)
        End Get
    End Property

    Public ReadOnly Property Path() As String
        Get
            Return (Parse(m_Node).m_Path)
        End Get
    End Property

    Public ReadOnly Property Item() As String
        Get
            Return (Parse(m_Node).m_Item)
        End Get
    End Property

End Structure

