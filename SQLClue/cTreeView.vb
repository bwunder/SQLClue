Imports System.Windows.Forms

Public Class cTreeView

    Public Structure structNodeTag
        Public IsCollection As Boolean
        Public HasChildren As Boolean
        Public ChangeId As Integer
        Public Origin As String
        Public obj As Object
        Public Version As Integer
        Public LatestVersion As Integer
    End Structure

    Private Sub RepositoryAddItems(ByRef Node As TreeNode)
        ' this is called when a node that is set to the PlaceHolder is first requested
        Dim CurNode As TreeNode
        ' clean out the placeholder
        '        If Node.Nodes.Count = 1 AndAlso Node.Nodes(0).Text = My.Resources.NodePlaceHolder Then
        Node.Nodes.Clear()
        '        End If
        Dim tbl As DataTable = CompareForm.cCompare.DAL.GetLatestItemsForNode(Node.FullPath)
        For Each r As DataRow In tbl.Rows
            CurNode = CreateNode(String.Format("{0}", r("Item")), r("ChangeId"))
            Node.Nodes.Add(CurNode)
        Next
    End Sub

    Friend Sub FileInitTreeView(ByRef Origin As ComboBox, _
                                ByRef Instance As ComboBox, _
                                ByRef smoTreeView As TreeView)
        Try
            smoTreeView.Nodes.Clear()
            Dim fldr As String() = Instance.Text.Split(CChar("\"))
            Dim CurNode As TreeNode = CreateNode(String.Format( _
                                                               "{0}", _
                                                               fldr(fldr.Length - 1), 0), fldr)
            ' all that matters is that the tag never matches instance or repository tags
            Dim nt As New structNodeTag
            nt.Origin = "Folder"
            CurNode.Tag = nt
            smoTreeView.Nodes.Add(CurNode)
            If smoTreeView.Nodes(0).GetNodeCount(False) = 1 _
            And smoTreeView.Nodes(0).Nodes(0).Text = My.Resources.NodePlaceHolder Then
                smoTreeView.Nodes(0).Nodes(0).Remove()
            End If
            For Each f As String In Directory.GetFiles(Instance.Text, "*.*", SearchOption.TopDirectoryOnly)
                ' need standardized nodes
                smoTreeView.Nodes(0).Nodes.Add(CreateNode(Replace(Replace(f, Instance.Text, ""), "\", ""), f))
            Next
            smoTreeView.Sort()
            smoTreeView.Nodes(0).Expand()
        Catch ex As Exception
            Throw New Exception(String.Format("(cTreeView.FileInitTreeView) Exception."), ex)
        End Try
    End Sub

    Friend Sub InvertNodeColors(ByRef Node As TreeNode)
        Try
            Dim oldBackColor As System.Drawing.Color = Node.BackColor
            Node.BackColor = Node.ForeColor
            Node.ForeColor = oldBackColor
        Catch ex As Exception
            Throw New Exception(String.Format("(cTreeView.InvertNodeColors) Exception."), ex)
        End Try
    End Sub

    Friend Sub LoadTreeFromRepository(ByVal SQLInstance As String, _
                                      ByVal Repository As Server, _
                                      ByRef Tree As TreeView, _
                                      ByVal dtRepositoryInstance As DataTable)


        ' called from compare.TryLoadTreeView
        Try
            'work table of the latest changes for a server
            dtRepositoryInstance = CompareForm.cCompare.DAL.GetHierarchyNodesBySQLInstance(SQLInstance, _
                                                                                           Repository.ConnectionContext.ConnectionString)
            Tree.BeginUpdate()
            ' the root node gets a table of nodes for the SQL Instance
            Tree.Nodes.Add(CreateNode(SQLInstance, dtRepositoryInstance))
            For Each r As DataRow In dtRepositoryInstance.Rows
                Dim curnode As TreeNode = Tree.Nodes(0)
                Dim TreePath() As String = r.Item("TreeViewNodePath").ToString.Split(CType("|", Char))
                If TreePath.Count = 1 Then
                    Dim nt As structNodeTag
                    If curnode.Tag Is Nothing Then
                        nt = New structNodeTag
                    Else
                        nt = CType(curnode.Tag, cTreeView.structNodeTag)
                    End If
                    ' server always has a changeid
                    nt.ChangeId = CInt(r.Item("ChangeId"))
                    nt.Origin = "Repository"
                    nt.Version = CInt(r.Item("Version"))
                    nt.LatestVersion = CInt(r.Item("Version"))
                    nt.obj = dtRepositoryInstance
                    curnode.Tag = nt
                Else
                    '0 Node already has the table in the tag
                    For i As Integer = 1 To TreePath.Count - 1
                        Dim nt As structNodeTag
                        If Not curnode.Nodes.ContainsKey(TreePath(i)) Then
                            ' need key too for ContainsKey matching 
                            curnode.Nodes.Add(TreePath(i), TreePath(i))
                            nt = New structNodeTag
                        Else
                            nt = CType(curnode.Nodes(TreePath(i)).Tag, structNodeTag)
                        End If
                        curnode = curnode.Nodes(TreePath(i))
                        If i < TreePath.Count - 1 Then

                            ' this isn;t right is it?
                            ' subtypes are not collections


                            ' parentlevels are all collections
                            nt.IsCollection = True
                        Else
                            ' leaf always has a changeid
                            nt.ChangeId = CInt(r.Item("ChangeId"))
                            nt.Origin = "Repository"
                            nt.Version = CInt(r.Item("Version"))
                            nt.LatestVersion = CInt(r.Item("Version"))
                        End If
                        curnode.Tag = nt
                    Next
                End If
            Next
            Tree.Sort()
            ' is in the expand event that repository state is added to smo userdata 
            Tree.Nodes(0).Expand()
            Tree.EndUpdate()
        Catch ex As Exception
            Throw New Exception(String.Format("(cTreeView.LoadTreeFromRepository) Exception."), ex)
        End Try
    End Sub

    Friend Sub GetNextTreeLevel(ByVal Origin As String, _
                                ByRef Node As TreeNode, _
                                Optional ByVal dtRepositoryInstance As DataTable = Nothing)
        ' only called by compare form
        Try
            If Origin = My.Resources.OriginSQLInstance Then
                DrillDown(Origin, Node)
            ElseIf Origin = My.Resources.OriginRepository Then
                DrillDown(Origin, Node, dtRepositoryInstance)
            End If
        Catch ex As SmoException
            Mother.HandleException(New ApplicationException("(CTreeView.GetNextTreeLevel) Failed.", ex))
        End Try
    End Sub

    Friend Sub DrillDown(ByVal Origin As String, _
                          ByRef node As TreeNode, _
                          Optional ByVal dtRepositoryInstance As DataTable = Nothing)
        If node.Nodes.Count = 1 AndAlso node.Nodes(0).Text = My.Resources.NodePlaceHolder Then
            node.Nodes.RemoveAt(0)
            If CType(node.Tag, structNodeTag).IsCollection Then
                ' need items for the compare form
                If Not (Origin = My.Resources.OriginArchiveTarget) _
                Or node.Text = "Databases" Then
                    PopulateCollectionItems(Origin, node, dtRepositoryInstance)
                End If
            ElseIf CType(node.Tag, structNodeTag).obj.GetType.Name = "JobServer" _
            Or CType(node.Tag, structNodeTag).obj.GetType.Name = "Server" _
            Or CType(node.Tag, structNodeTag).obj.GetType.Name = "Database" _
            Or CType(node.Tag, structNodeTag).obj.GetType.Name = "ServiceBroker" Then
                PopulateExpandableProperties(node)
            End If
        End If
    End Sub

    Private Overloads Shared Function CreateNode(ByVal smoItem As Object) As TreeNode
        Try
            Dim ItemName As String = Nothing
            Dim SchemaName As String = Nothing
            Dim [property] As PropertyDescriptor
            For Each propertyName As String In New String() {"Schema"}
                [property] = TypeDescriptor.GetProperties(smoItem)(propertyName)
                If Not ([property] Is Nothing) Then
                    SchemaName = [property].GetValue(smoItem).ToString()
                    Exit For
                End If
            Next
            For Each propertyName As String In New String() {"Name"}
                [property] = TypeDescriptor.GetProperties(smoItem)(propertyName)
                If Not ([property] Is Nothing) Then
                    If SchemaName Is Nothing Then
                        ItemName = [property].GetValue(smoItem).ToString()
                    Else
                        ItemName = SchemaName & "." & [property].GetValue(smoItem).ToString()
                    End If
                    Exit For
                End If
            Next
            If ItemName Is Nothing Then
                ItemName = smoItem.ToString()
            End If
            Return CreateNode(ItemName, smoItem)
        Catch ex As Exception
            Throw New Exception("(cTreeView.CreateNode(name)) Exception.", ex)
        End Try
    End Function

    Protected Friend Overloads Shared Function CreateNode(ByVal name As String, ByVal smoItem As Object) As TreeNode
        Try
            ' get a new node with .Text set to the name of node being created
            Dim node As New TreeNode(name)
            Dim nt As New structNodeTag
            ' PopulateExpandableProperties should have identified items not in the target's version and set to nothing
            If smoItem Is Nothing Then
                nt.IsCollection = True
                ' could be moved to presentation, smoItem will stay with node
                node.ForeColor = System.Drawing.SystemColors.GrayText
            Else
                ' assign the object passed to the node tag  
                nt.IsCollection = If(TypeOf smoItem Is ICollection, True, False)
                nt.obj = smoItem
                ' don't add the placeholder if there aren't any members in the collection
                If TypeOf smoItem Is ICollection _
                AndAlso (Not (TypeOf smoItem Is Server) _
                         OrElse Not (UCase(Replace(Replace(smoItem.Parent.ToString, "[", ""), "]", "")) = "TEMPDB")) Then
                    ' DirectCast for performance benefit over CType since smoItem inherits from ICollection
                    If TryCast(smoItem, ICollection).Count > 0 Then
                        node.Nodes.Add(My.Resources.NodePlaceHolder)
                    End If
                ElseIf TypeOf smoItem Is Server _
                Or TypeOf smoItem Is Agent.JobServer _
                Or TypeOf smoItem Is Database _
                Or TypeOf smoItem Is Broker.ServiceBroker Then
                    node.Nodes.Add(My.Resources.NodePlaceHolder)
                End If
            End If
            ' always make the .Text and the .Name the same
            node.Name = name
            node.Tag = nt
            Return node
        Catch ex As Exception
            Throw New Exception(String.Format("({0}.CreateNode(name,item)) Exception.", "cTreeView"), ex)
        End Try
    End Function

    Private Shared Function HasExpandableProperties(ByVal item As Object) As Boolean
        Try
            For Each [property] As PropertyDescriptor In TypeDescriptor.GetProperties(item)
                If IsExpandableProperty([property]) = True Then
                    Return True
                End If
            Next
            Return False
        Catch ex As Exception
            Throw New Exception(String.Format("({0}.HasExpandableProperties) Exception.", "cTreeView"), ex)
        End Try
    End Function

    Private Shared Function IsCollection(ByVal [property] As PropertyDescriptor) As Boolean
        Try
            For Each typ As Type In [property].PropertyType.GetInterfaces()
                If TypeOf typ Is ICollection Then
                    If [property].PropertyType.IsArray Then
                        Exit For
                    End If
                    If [property].Name = "Properties" Then
                        Exit For
                    End If
                    Return True
                End If
            Next
            Return False
        Catch ex As Exception
            Throw New Exception(String.Format("({0}.IsCollection) Exception.", "cTreeView"), ex)
        End Try
    End Function

    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")> _
    Private Sub PopulateExpandableProperties(ByRef node As TreeNode)
        Dim value As Object
        Try
            For Each [property] As PropertyDescriptor In TypeDescriptor.GetProperties(CType(node.Tag, structNodeTag).obj)
                If IsExpandableProperty([property]) Then
                    ' do not include stuff that is static, connection specific, 
                    ' replication, or notification services or already included elsewhere 
                    ' the versions values are in Server Information
                    ' categories scripted with the alerts and jobs
                    ' !!!msx and 2008 equiv servers may be missed
                    If Not (UCase([property].Name) = "ALERTCATEGORIES" Or _
                            UCase([property].Name) = "BUILDCLRVERSION" Or _
                            UCase([property].Name) = "CONNECTIONCONTEXT" Or _
                            UCase([property].Name) = "EVENTS" Or _
                            UCase([property].Name) = "JOBCATEGORIES" Or _
                            UCase([property].Name) = "LANGUAGES" Or _
                            UCase([property].Name) = "NOTIFICATIONSERVICES" Or _
                            UCase([property].Name) = "OPERATORCATEGORIES" Or _
                            UCase([property].Name) = "REPLICATIONSERVER" Or _
                            UCase([property].Name) = "RESOURCEVERSION" Or _
                            UCase([property].Name) = "SHAREDSCHEDULES" Or _
                            UCase([property].Name) = "SYSTEMDATATYPES" Or _
                            UCase([property].Name) = "SYSTEMMESSAGES" Or _
                            UCase([property].Name) = "TARGETSERVERGROUPS" Or _
                            UCase([property].Name) = "USEROPTIONS" Or _
                            UCase([property].Name) = "VERSION") Then
                        ' expecting only items not in the target's version to raise error here
                        Try
                            value = [property].GetValue(CType(node.Tag, structNodeTag).obj)
                        Catch ex As Exception
                            value = Nothing
                        End Try
                        If Not value Is Nothing Then
                            ' add try becasue a sql2000 error still gets through
                            ' seems to barf on an ENUM within the Typeof method for the value passed
                            Try
                                node.Nodes.Add(CreateNode([property].Name, value))
                                ' this was in configform version when your not testing something else in here
                                If node.Parent Is ConfigurationForm.smoTreeView Then
                                    node.LastNode.ContextMenuStrip = ConfigurationForm.ContextMenuStripTreeNode
                                End If
                            Catch ex As Exception
                                value = Nothing
                            End Try
                        End If
                    End If
                End If
            Next
        Catch ex As Exception
            Throw New Exception("(cTreeView.PopulateExpandableProperties) Failed.", ex)
        End Try
    End Sub

    Private Shared Function IsExpandableProperty(ByVal [property] As PropertyDescriptor) As Boolean
        Try
            If Not IsExpandablePropertyType([property].PropertyType) Then
                Return False
            End If
            If [property].PropertyType.IsArray Then
                If Not IsExpandablePropertyType([property].PropertyType.GetElementType()) Then
                    Return False
                End If
            End If
            If UCase([property].Name) = "URN" _
                OrElse UCase([property].Name) = "USERDATA" _
                OrElse UCase([property].Name) = "PROPERTIES" _
                OrElse UCase([property].Name) = "EXTENDEDPROPERTIES" _
                OrElse UCase([property].Name) = "EXTENDEDSTOREDPROCEDURES" _
                OrElse UCase([property].Name) = "FILEGROUPS" _
                OrElse UCase([property].Name) = "LOGFILES" _
                OrElse UCase([property].Name) = "MASTERKEY" _
                OrElse UCase([property].Name) = "OLEDBPROVIDERSETTINGS" _
                OrElse UCase([property].Name) = "SERVICEMASTERKEY" _
                OrElse UCase([property].Name) = "PARENT" Then
                Return False
            End If
            Return True
        Catch ex As Exception
            Throw New Exception(String.Format("({0}.IsExpandableProperty) Exception.", "cTreeView"), ex)
        End Try
    End Function

    Private Shared Function IsExpandablePropertyType(ByVal typ As Type) As Boolean
        Try
            If Type.GetTypeCode(typ) <> TypeCode.Object Then
                Return False
            End If
            If typ Is GetType(Guid) OrElse typ Is GetType(DateTime) Then
                Return False
            End If
            Return True
        Catch ex As Exception
            Throw New Exception(String.Format("({0}.IsExpandablePropertyType) Exception.", "cTreeView"), ex)
        End Try
    End Function

    Private Sub PopulateCollectionItems(ByVal Origin As String, _
                                        ByRef node As TreeNode, _
                                        ByVal dtRepositoryInstance As DataTable)
        ' SQLInstance and repository use this
        For Each item As Object In CType(CType(node.Tag, structNodeTag).obj, ICollection)
            ' if repository, get the changeid for the item else don't add
            If (Origin = My.Resources.OriginRepository) Then
                ' should always only be one or no rows...
                Dim r As DataRow() = dtRepositoryInstance.Select("TreeViewNodePath='" & node.FullPath & "|" & _
                                                                 Replace(Replace(item.ToString, "[", ""), "]", "") & "'", _
                                                                 "ChangeId DESC")
                Dim i As Int32 = 0
                If r.Length > 0 Then
                    i = CInt(r(0)("ChangeId"))
                    dtRepositoryInstance.Rows.Remove(r(0))
                End If
                CType(item, SmoObjectBase).UserData = i
            End If
            'need a little more selective addnode
            'node.Nodes.Add(CreateNode(item))
            ' gray out all system objects 
            Select Case item.GetType.Name
                Case "DatabaseDdlTrigger"
                    If Not CType(item, DatabaseDdlTrigger).IsSystemObject Then
                        node.Nodes.Add(CreateNode(item))
                    End If
                Case "Endpoint"
                    node.Nodes.Add(CreateNode(item))
                    If CType(item, Endpoint).IsSystemObject Then
                        node.LastNode.ForeColor = System.Drawing.SystemColors.GrayText
                        node.LastNode.ToolTipText = "System Object"
                    End If
                Case "Login"
                    node.Nodes.Add(CreateNode(item))
                    If CType(item, Login).IsSystemObject Then
                        node.LastNode.ForeColor = System.Drawing.SystemColors.GrayText
                        node.LastNode.ToolTipText = "System Object"
                    End If
                Case "DatabaseRole"
                    node.Nodes.Add(CreateNode(item))
                    If CType(item, DatabaseRole).IsFixedRole Then
                        node.LastNode.ForeColor = System.Drawing.SystemColors.GrayText
                        node.LastNode.ToolTipText = "System Object"
                    End If
                Case "ServerRole"
                    node.Nodes.Add(CreateNode(item))
                    node.LastNode.ForeColor = System.Drawing.SystemColors.GrayText
                Case "StoredProcedure"
                    If Not CType(item, StoredProcedure).IsSystemObject Then
                        node.Nodes.Add(CreateNode(item))
                    End If
                Case "Table"
                    If Not CType(item, Smo.Table).IsSystemObject Then
                        node.Nodes.Add(CreateNode(item))
                    End If
                Case "View"
                    If Not CType(item, Smo.View).IsSystemObject Then
                        node.Nodes.Add(CreateNode(item))
                    End If
                Case "ServerDdlTrigger"
                    If Not CType(item, ServerDdlTrigger).IsSystemObject Then
                        node.Nodes.Add(CreateNode(item))
                    End If
                Case "User"
                    node.Nodes.Add(CreateNode(item))
                    If CType(item, User).IsSystemObject Then
                        node.LastNode.ForeColor = System.Drawing.SystemColors.GrayText
                        node.LastNode.ToolTipText = "System Object"
                    End If
                Case "UserDefinedFunction"
                    If Not CType(item, UserDefinedFunction).IsSystemObject Then
                        node.Nodes.Add(CreateNode(item))
                    End If
                Case Else
                    node.Nodes.Add(CreateNode(item))
            End Select
        Next
    End Sub

End Class
