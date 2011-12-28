Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.SqlTypes
Imports Microsoft.SqlServer.Server

Partial Public Class UserDefinedFunctions
    '<Microsoft.SqlServer.Server.SqlFunction()> _
    'Public Shared Function SQLCfgNodeFunctions() As SqlString
    '    ' Add your code here
    '    Return New SqlString("Hello")
    'End Function

    <Microsoft.SqlServer.Server.SqlFunction(DataAccess:=DataAccessKind.None, _
                                            IsDeterministic:=True, _
                                            IsPrecise:=True, _
                                            Name:="GetCollection")> _
    Public Shared Function GetCollection(ByVal Node As SQLCfgNode) As SqlString
        ' Add your code here
        Return Node.Collection
    End Function

    <Microsoft.SqlServer.Server.SqlFunction(DataAccess:=DataAccessKind.None, _
                                            IsDeterministic:=True, _
                                            IsPrecise:=True, _
                                            Name:="GetDatabase")> _
    Public Shared Function GetDatabase(ByVal Node As SQLCfgNode) As SqlString
        ' Add your code here
        Return Node.Database
    End Function

    <Microsoft.SqlServer.Server.SqlFunction(DataAccess:=DataAccessKind.None, _
                                            IsDeterministic:=True, _
                                            IsPrecise:=True, _
                                            Name:="GetItem")> _
    Public Shared Function GetItem(ByVal Node As SQLCfgNode) As SqlString
        ' Add your code here
        Return Node.Item
    End Function

    <Microsoft.SqlServer.Server.SqlFunction(DataAccess:=DataAccessKind.None, _
                                            IsDeterministic:=True, _
                                            IsPrecise:=True, _
                                            Name:="GetPath")> _
    Public Shared Function GetPath(ByVal Node As SQLCfgNode) As SqlString
        ' Add your code here
        Return Node.Path
    End Function

    <Microsoft.SqlServer.Server.SqlFunction(DataAccess:=DataAccessKind.None, _
                                            IsDeterministic:=True, _
                                            IsPrecise:=True, _
                                            Name:="GetInstance")> _
    Public Shared Function GetInstance(ByVal Node As SQLCfgNode) As SqlString
        ' Add your code here
        Return Node.SQLInstance
    End Function

    <Microsoft.SqlServer.Server.SqlFunction(DataAccess:=DataAccessKind.None, _
                                            IsDeterministic:=True, _
                                            IsPrecise:=True, _
                                            Name:="GetSubType")> _
    Public Shared Function GetSubType(ByVal Node As SQLCfgNode) As SqlString
        ' Add your code here
        Return Node.SubType
    End Function

    <Microsoft.SqlServer.Server.SqlFunction(DataAccess:=DataAccessKind.None, _
                                            IsDeterministic:=True, _
                                            IsPrecise:=True, _
                                            Name:="GetLength")> _
    Public Shared Function GetLength(ByVal Node As SQLCfgNode) As SqlInt32
        ' Add your code here
        Return Node.Length
    End Function

End Class
