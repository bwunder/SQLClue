Imports System.Windows.Forms
Public Class frmOptions

    'based on http://www.codeproject.com/vb/net/settingsdialog2005.asp

    Private Settings As New SettingInfoCollection

    Friend Password As New Security.SecureString

    Private Sub frmOptions_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.Text = My.Resources.AdvancedSettings

        Me.Height = CInt(If(Me.Height > My.Forms.Mother.Height, CInt(My.Forms.Mother.Height * 0.8), Me.Height))
        Me.Width = CInt(If(Me.Width > My.Forms.Mother.Width, CInt(My.Forms.Mother.Width * 0.8), Me.Width))
        'Me.Width = CInt(My.Forms.Mother.Width * 0.8)
        'Me.StartPosition = FormStartPosition.CenterParent

        'Load the SaveOnExit value
        chkSaveOnExit.Checked = My.Application.SaveMySettingsOnExit

        'Setup the Setting property object for grabing all the settings
        Dim sp As System.Configuration.SettingsProperty = Nothing

        'Cycle through each setting and add it if appropriate.
        For Each sp In My.Settings.Properties
            ' only the compare settings use underscores in the name so nothing else will get touched here
            ' EXCEPT WHEN IT SAVES using MY.Application.Settings. This could stomp on another tool.
            ' will watch for such a problem and fix if necessary

            'If the name doesn't have an underscore - we can't assign a category so we can't change it.
            'If the setting is ApplicationScoped - we aren't able to change it at runtime.
            'Check also if there is support on this form for the System.Type the setting is.
            If sp.Name.IndexOf("_") <> -1 AndAlso IsUserScope(sp) AndAlso IsAllowedType(sp) Then
                'Passed the tests create a new SettingInfo object
                Dim newSetting As New SettingInfo
                'Load the settings data into the object
                newSetting.LoadData(sp.Name)
                'Add the object to the collection
                Settings.Add(newSetting)
            End If
        Next

        'Sort the settings by category - makes the TreeView look nice
        quickSort(Settings)
        LoadTreeView()
    End Sub

#Region " Sorting Functions "

    ''' <summary>Bootstrap starts the quick sort method of sorting a SettingInfoCollection.</summary>
    ''' <param name="col">The SettingInfoCollection you wish to sort.</param>
    ''' <remarks>Sorts alphabetically by category.</remarks>
    Private Sub quickSort(ByRef col As SettingInfoCollection)
        'Call the actual sorting method
        SortIt(col, 0, col.Count - 1)
    End Sub
    ''' <summary>QuickSort subroutine that does the sorting for a SettingInfoCollection</summary>
    ''' <param name="SortArray">The SettingInfoCollection to sort.</param>
    ''' <param name="First">The starting element of the selection you wish to sort (usually 0).</param>
    ''' <param name="Last">Then final element of the selection you wish to sort (usually the Count - 1 of the collection).</param>
    ''' <remarks>Called by the quickSort subroutine</remarks>
    Private Sub SortIt(ByRef SortArray As SettingInfoCollection, ByVal First As Long, ByVal Last As Long)
        'Copied and modified code to support sorting of the SettingInfoCollection
        'using strings
        Dim Low As Long, High As Long
        Dim Temp As SettingInfo = Nothing
        Dim List_Separator As SettingInfo = Nothing
        Low = First
        High = Last
        List_Separator = SortArray(CInt((First + Last) / 2)).Clone
        Do
            Do While (SortArray(CInt(Low)).Category < List_Separator.Category)
                Low += 1
            Loop
            Do While (SortArray(CInt(High)).Category > List_Separator.Category)
                High -= 1
            Loop
            If (Low <= High) Then
                Temp = SortArray(CInt(Low)).Clone
                SortArray.SetItem(CInt(Low), SortArray(CInt(High)))
                SortArray.SetItem(CInt(High), Temp)
                Low += 1
                High -= 1
            End If
        Loop While (Low <= High)
        If (First < High) Then SortIt(SortArray, First, High)
        If (Low < Last) Then SortIt(SortArray, Low, Last)
    End Sub

#End Region

#Region " Loading Functions "
    ''' <summary>Checks whether the dialog supports a given type for editing</summary>
    ''' <param name="se">A SettingsProperty object as found in the My namespace.</param>
    ''' <returns>Boolean: True editing is supported, false editing is not supported.</returns>
    Private Function IsAllowedType(ByVal se As System.Configuration.SettingsProperty) As Boolean
        'array of System.Types that this form supports.
        Dim allowTypes() As Type = {GetType(Boolean), _
                                    GetType(Integer), _
                                    GetType(String), _
                                    GetType(SqlServerVersion)}
        'If the type of the property is found in the array - the type is supported.
        If Array.IndexOf(allowTypes, se.PropertyType) <> -1 Then Return True
        'The type must not be supported if it got here
        Return False
    End Function
    ''' <summary>Checks whether the Setting is writable or not</summary>
    ''' <param name="sp">A SettingsProperty as specified in the My namespace.</param>
    ''' <returns>Boolean: True setting is writable, false setting is read-only.</returns>
    Private Function IsUserScope(ByVal sp As System.Configuration.SettingsProperty) As Boolean
        'The scope of the setting is stored in the Attributes of the setting
        For Each o As Object In sp.Attributes.Values
            'If we find an ApplicationScopedSettingAttribute the setting isn't UserScoped so return false
            If TypeOf (o) Is System.Configuration.ApplicationScopedSettingAttribute Then Return False
            'If we find a UserScopedSettingAttribute then the setting is UserScoped so return true
            If TypeOf (o) Is System.Configuration.UserScopedSettingAttribute Then Return True
        Next
        'If we didn't find either of them, it isn't UserScoped (and I'm not sure what it is) so return false
        Return False
    End Function

    ''' <summary>Cycles through each setting and makes sure the category is in the TreeView</summary>
    ''' <remarks>If there are no settings - do not try loading the treeview</remarks>
    Private Sub LoadTreeView()
        If Settings.Count = 0 Then Exit Sub
        For Each si As SettingInfo In Settings
            'Add the categories to the root of the TreeView
            AddCat(tvCategories.Nodes, si.Category.Split(CChar(".")), 0)
        Next
    End Sub
    ''' <summary>Adds categories/sub-categories to a TreeView</summary>
    ''' <param name="parent_nodes">The TreeNodeCollection to which you wish to add the category/sub-category</param>
    ''' <param name="fields">An array of categories/sub-categories.  Index 0 is the Category everything else is a sub-category of the index before it.</param>
    ''' <param name="field_num">The index with which to the current TreeNodeCollection level. For a TreeNode.Nodes object this would most likely be 0.</param>
    Private Sub AddCat(ByVal parent_nodes As TreeNodeCollection, ByVal fields() As String, ByVal field_num As Integer)
        'If there were no fields passed or end of the field list then done
        If field_num > fields.GetUpperBound(0) Then Exit Sub

        Dim found_field As Boolean
        'Check each node of the parent to see if it category/sub-category already exists
        For Each child_node As TreeNode In parent_nodes
            If child_node.Text = fields(field_num) Then
                'It exists so check this node for the next sub-category
                AddCat(child_node.Nodes, fields, field_num + 1)
                found_field = True
            End If
        Next child_node

        'If we didn't find the node - then add it
        If Not found_field Then
            Dim new_node As TreeNode = parent_nodes.Add(fields(field_num))
            'Check the next sub-category in the field list
            AddCat(new_node.Nodes, fields, field_num + 1)
        End If
    End Sub

#End Region

#Region " TreeView Handling "
    ''' <summary>Clears all the setting controls that were created at runtime from the window</summary>
    Private Sub ClearOptions()
        'Cleans up the dynamically added controls for the settings
        Dim ctrl As Control
        'While there are still dynamic controls on the form - remove them
        While tlp.Controls.Count > 0
            'Set an object equal to a reference of the control
            ctrl = tlp.Controls.Item(0)
            'Remove the handlers based on the type of the control
            If TypeOf (ctrl) Is System.Windows.Forms.CheckBox Then
                RemoveHandler ctrl.LostFocus, AddressOf checkbox_handler
            ElseIf TypeOf (ctrl) Is TextBox Then
                RemoveHandler ctrl.LostFocus, AddressOf password_handler
                RemoveHandler ctrl.KeyPress, AddressOf password_keypress_handler
                RemoveHandler ctrl.LostFocus, AddressOf textbox_handler
            ElseIf TypeOf (ctrl) Is ComboBox Then
                RemoveHandler ctrl.LostFocus, AddressOf textbox_handler
            ElseIf TypeOf (ctrl) Is NumericUpDown Then
                RemoveHandler ctrl.LostFocus, AddressOf numericupdown_handler
            End If
            'Remove the control from the form
            ctrl.Dispose()
            ctrl = Nothing
            pgb.Value += 1
        End While
    End Sub
    ''' <summary>Determines whether the type passed would use a textbox or not</summary>
    ''' <param name="typ">A system type that you wish to check.</param>
    ''' <returns>Boolean: True the type does use a textbox, false it doesn't use a textbox.</returns>
    Private Function IsTxtType(ByVal typ As Type) As Boolean
        'If the type passed is one of these we use a textbox to edit it
        If GetType(String) Is typ Then
            Return True
        End If
        'We don't use a textbox on this type.
        Return False
    End Function
    Private Sub tvCategories_AfterSelect(ByVal sender As System.Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles tvCategories.AfterSelect
        'A category was selected - clear the current display

        Try

            'Reset the TabIndex for the dynamic controls
            Dim tbIdx As Integer = 1
            'Get all the settings that match this category
            Dim sets() As SettingInfo = Settings.GetByCategory(tvCategories.SelectedNode.FullPath)
            'If there aren't any we must exit
            If sets Is Nothing Then Exit Sub

            tlp.Hide()
            pgb.Show()
            pgb.Value = 0
            pgb.Minimum = 0
            pgb.Maximum = sets.Length + tlp.Controls.Count
            Me.Refresh()
            tvCategories.HideSelection = False
            ClearOptions()
            tlp.RowStyles.Clear()
            tlp.RowCount = 1
            tlp.Refresh()

            'Some declaration to make things easier in the loop
            Dim lbl As Label = Nothing
            Dim x As New RowStyle
            x.SizeType = SizeType.AutoSize
            x.Height = 10
            'Cycle through this category's settings
            For i As Integer = 0 To UBound(sets)

                Try

                    lbl = New Label
                    lbl.Anchor = CType(AnchorStyles.Left + AnchorStyles.Top, AnchorStyles)
                    x = New RowStyle
                    'If we've reached the end of the settings - don't use the Fill DockStyle.
                    lbl.Dock = DockStyle.Fill
                    lbl.Width = CInt(tlp.Width * 0.6)
                    lbl.TextAlign = ContentAlignment.MiddleLeft
                    If TypeOf (sets(i).Value) Is Boolean Then 'If the setting is a boolean use a checkbox for editing
                        'Setup the checkbox
                        Dim chk As New CheckBox
                        chk.Anchor = CType(AnchorStyles.Left + AnchorStyles.Top, AnchorStyles)
                        chk.Name = sets(i).TrueName
                        chk.Tag = GetTypeAbbr(sets(i).Value.GetType)
                        chk.Checked = CBool(sets(i).Value)
                        chk.TabIndex = tbIdx
                        'Setup the checkbox help string - for after clicking on the question mark button on the form
                        hp1.SetHelpString(chk, String.Format(CultureInfo.CurrentUICulture, My.Resources.BooleanAdvOptionsHelp))
                        hp1.SetShowHelp(chk, True)
                        'Setup the label
                        lbl.Text = sets(i).Name
                        'Add the label
                        tlp.Controls.Add(lbl)
                        tlp.RowStyles.Add(x)
                        'Add the handler for the checkbox, and add it to the form
                        RemoveHandler chk.LostFocus, AddressOf checkbox_handler
                        AddHandler chk.LostFocus, AddressOf checkbox_handler
                        tlp.Controls.Add(chk)
                    ElseIf TypeOf (sets(i).Value) Is Integer Then
                        'If the setting is integer use a numericupdown for editing
                        'Setup the checkbox
                        Dim nupdwn As New NumericUpDown
                        '                chk.CheckAlign = ContentAlignment.MiddleRight
                        nupdwn.Anchor = CType(AnchorStyles.Left + AnchorStyles.Top, AnchorStyles)
                        nupdwn.Name = sets(i).TrueName
                        'nupdwn.Tag = GetTypeAbbr(sets(i).Value.GetType)
                        nupdwn.DecimalPlaces = 0
                        nupdwn.Value = CDec(sets(i).Value)
                        nupdwn.TabIndex = tbIdx
                        'Setup the checkbox help string - for after clicking on the question mark button on the form
                        hp1.SetHelpString(nupdwn, String.Format(CultureInfo.CurrentUICulture, My.Resources.NumericAdvOptionsHelp, sets(i).Name))
                        hp1.SetShowHelp(nupdwn, True)
                        'Setup the label
                        lbl.Text = sets(i).Name
                        'Add the label
                        tlp.Controls.Add(lbl)
                        tlp.RowStyles.Add(x)
                        'Add the handler for the checkbox, and add it to the form
                        RemoveHandler nupdwn.LostFocus, AddressOf numericupdown_handler
                        AddHandler nupdwn.LostFocus, AddressOf numericupdown_handler
                        tlp.Controls.Add(nupdwn)
                    ElseIf TypeOf (sets(i).Value) Is String _
                    And (sets(i).Name.Contains("Pattern") OrElse sets(i).Name.Contains("Replacement")) Then
                        'If the setting is a enum type we fill a combobox
                        'Setup the label
                        lbl.Text = sets(i).Name
                        'Add the label
                        tlp.Controls.Add(lbl)
                        tlp.RowStyles.Add(x)
                        'Setup the ComboBox
                        Dim cbo As New ComboBox
                        cbo.DropDownStyle = ComboBoxStyle.DropDown
                        cbo.FlatStyle = FlatStyle.Standard
                        cbo.AllowDrop = True
                        cbo.DropDownWidth = 128
                        cbo.Name = sets(i).TrueName
                        cbo.Tag = GetTypeAbbr(sets(i).Value.GetType) ' applysettings uses the tag 
                        Select Case sets(i).TrueName
                            Case "Regular__Expressions_NameMatch__Pattern_4"
                                For Each str As String In My.Settings.Regular__Expressions_NameMatch_AutoComplete__Patterns
                                    If Not (cbo.Items.Contains(str)) Then
                                        cbo.Items.Add(str)
                                    End If
                                Next
                            Case "Regular__Expressions_LineReplace__Pattern_1"
                                For Each str As String In My.Settings.Regular__Expressions_LineReplace_AutoComplete__Patterns
                                    If Not (cbo.Items.Contains(str)) Then
                                        cbo.Items.Add(str)
                                    End If
                                Next
                            Case "Regular__Expressions_LineReplace__Replacement_2"
                                For Each str As String In My.Settings.Regular__Expressions_LineReplace_AutoComplete__Replacements
                                    If Not (cbo.Items.Contains(str)) Then
                                        cbo.Items.Add(str)
                                    End If
                                Next
                            Case "Regular__Expressions_LineSplit__Pattern_3"
                                For Each str As String In My.Settings.Regular__Expressions_LineSplit_AutoComplete__Patterns
                                    If Not (cbo.Items.Contains(str)) Then
                                        cbo.Items.Add(str)
                                    End If
                                Next
                        End Select
                        cbo.AutoCompleteSource = AutoCompleteSource.ListItems
                        cbo.AutoCompleteMode = AutoCompleteMode.SuggestAppend
                        cbo.SelectedItem = sets(i).Value
                        cbo.Anchor = CType(AnchorStyles.Left + AnchorStyles.Right + AnchorStyles.Top, AnchorStyles)
                        cbo.TabIndex = tbIdx
                        'Setup the help string - for after clicking on the question mark button on the form
                        hp1.SetHelpString(cbo, String.Format(CultureInfo.CurrentUICulture, My.Resources.StringListAdvOptionsHelp, sets(i).Value.GetType.FullName, sets(i).Name))
                        hp1.SetShowHelp(cbo, True)
                        'Add the handler for the Combobox, and add it to the form
                        RemoveHandler cbo.LostFocus, AddressOf textbox_handler
                        AddHandler cbo.LostFocus, AddressOf textbox_handler
                        tlp.Controls.Add(cbo)
                    ElseIf TypeOf (sets(i).Value) Is String And sets(i).Name = "Encoding" Then
                        'fill a combobox
                        'Setup the label
                        lbl.Text = sets(i).Name
                        'Add the label
                        tlp.Controls.Add(lbl)
                        tlp.RowStyles.Add(x)
                        'Setup the ComboBox
                        Dim cbo As New ComboBox
                        cbo.DropDownStyle = ComboBoxStyle.DropDownList
                        cbo.FlatStyle = FlatStyle.Standard
                        'cbo.Name = sets(i).TrueName
                        'cbo.Tag = GetTypeAbbr(sets(i).Value.GetType) 'not necessary, but everywhere else so...
                        For Each o As EncodingInfo In Encoding.GetEncodings
                            cbo.Items.Add(o.Name)
                        Next
                        cbo.SelectedItem = sets(i).Value
                        cbo.Anchor = CType(AnchorStyles.Left + AnchorStyles.Right + AnchorStyles.Top, AnchorStyles)
                        cbo.TabIndex = tbIdx
                        'Setup the help string - for after clicking on the question mark button on the form
                        hp1.SetHelpString(cbo, String.Format(CultureInfo.CurrentUICulture, My.Resources.StringListAdvOptionsHelp, sets(i).Value.GetType.FullName, sets(i).Name))
                        hp1.SetShowHelp(cbo, True)
                        'Add the handler for the Combobox, and add it to the form
                        RemoveHandler cbo.LostFocus, AddressOf textbox_handler
                        AddHandler cbo.LostFocus, AddressOf textbox_handler
                        tlp.Controls.Add(cbo)
                    ElseIf TypeOf (sets(i).Value) Is String And sets(i).Name = "Network Protocol" Then
                        'fill a combobox
                        'Setup the label
                        lbl.Text = sets(i).Name
                        'Add the label
                        tlp.Controls.Add(lbl)
                        tlp.RowStyles.Add(x)
                        'Setup the ComboBox
                        Dim cbo As New ComboBox
                        cbo.DropDownStyle = ComboBoxStyle.DropDownList
                        cbo.FlatStyle = FlatStyle.Standard
                        'cbo.Name = sets(i).TrueName
                        'cbo.Tag = GetTypeAbbr(sets(i).Value.GetType) 'not necessary, but everywhere else so...
                        For Each p As String In My.Settings.NetworkLibraries
                            cbo.Items.Add(p)
                        Next
                        cbo.SelectedItem = sets(i).Value
                        cbo.Anchor = CType(AnchorStyles.Left + AnchorStyles.Right + AnchorStyles.Top, AnchorStyles)
                        cbo.TabIndex = tbIdx
                        'Setup the help string - for after clicking on the question mark button on the form
                        hp1.SetHelpString(cbo, String.Format(CultureInfo.CurrentUICulture, My.Resources.StringListAdvOptionsHelp, sets(i).Value.GetType.FullName, sets(i).Name))
                        hp1.SetShowHelp(cbo, True)
                        'Add the handler for the Combobox, and add it to the form
                        RemoveHandler cbo.LostFocus, AddressOf textbox_handler
                        AddHandler cbo.LostFocus, AddressOf textbox_handler
                        tlp.Controls.Add(cbo)
                    ElseIf TypeOf (sets(i).Value) Is String And sets(i).TrueName.Contains("Instance__Name") Then
                        'If the setting is a enum type we fill a combobox
                        'Setup the label
                        lbl.Text = sets(i).Name
                        'Add the label
                        tlp.Controls.Add(lbl)
                        tlp.RowStyles.Add(x)
                        'Setup the ComboBox
                        Dim cbo As New ComboBox
                        cbo.DropDownStyle = ComboBoxStyle.DropDown
                        cbo.FlatStyle = FlatStyle.Standard
                        cbo.AllowDrop = True
                        'cbo.Tag = GetTypeAbbr(sets(i).Value.GetType) 'not necessary, but everywhere else so...
                        cbo.Items.AddRange(Mother.InstanceList)
                        cbo.SelectedItem = sets(i).Value
                        cbo.Anchor = CType(AnchorStyles.Left + AnchorStyles.Right + AnchorStyles.Top, AnchorStyles)
                        cbo.TabIndex = tbIdx
                        'Setup the help string - for after clicking on the question mark button on the form
                        hp1.SetHelpString(cbo, String.Format(CultureInfo.CurrentUICulture, My.Resources.StringListAdvOptionsHelp, sets(i).Value.GetType.FullName, sets(i).Name))
                        hp1.SetShowHelp(cbo, True)
                        'Add the handler for the Combobox, and add it to the form
                        RemoveHandler cbo.LostFocus, AddressOf textbox_handler
                        AddHandler cbo.LostFocus, AddressOf textbox_handler
                        tlp.Controls.Add(cbo)
                    ElseIf TypeOf (sets(i).Value) Is String And sets(i).TrueName.Contains("Server__Discovery__Scope") Then
                        'If the setting is a enum type we fill a combobox
                        'Setup the label
                        lbl.Text = sets(i).Name
                        'Add the label
                        tlp.Controls.Add(lbl)
                        tlp.RowStyles.Add(x)
                        'Setup the ComboBox
                        Dim cbo As New ComboBox
                        cbo.DropDownStyle = ComboBoxStyle.DropDownList
                        cbo.FlatStyle = FlatStyle.Standard
                        cbo.SelectedItem = sets(i).Value
                        cbo.Anchor = CType(AnchorStyles.Left + AnchorStyles.Right + AnchorStyles.Top, AnchorStyles)
                        cbo.TabIndex = tbIdx
                        'Setup the help string - for after clicking on the question mark button on the form
                        hp1.SetHelpString(cbo, String.Format(CultureInfo.CurrentUICulture, My.Resources.StringListAdvOptionsHelp, sets(i).Value.GetType.FullName, sets(i).Name))
                        hp1.SetShowHelp(cbo, True)
                        'Add the handler for the Combobox, and add it to the form
                        RemoveHandler cbo.LostFocus, AddressOf textbox_handler
                        AddHandler cbo.LostFocus, AddressOf textbox_handler
                        tlp.Controls.Add(cbo)
                    ElseIf TypeOf (sets(i).Value) Is String And sets(i).TrueName = "Misc_Collection" Then
                        'If the setting is a enum type we fill a combobox
                        'Setup the label
                        lbl.Text = sets(i).Name
                        'Add the label
                        tlp.Controls.Add(lbl)
                        tlp.RowStyles.Add(x)
                        'Setup the ComboBox
                        Dim cbo As New ComboBox
                        cbo.DropDownStyle = ComboBoxStyle.DropDownList
                        'cbo.Name = sets(i).TrueName
                        'cbo.Tag = GetTypeAbbr(sets(i).Value.GetType) 'not necessary, but everywhere else so...
                        cbo.FlatStyle = FlatStyle.Standard
                        cbo.SelectedItem = sets(i).Value
                        cbo.Anchor = CType(AnchorStyles.Left + AnchorStyles.Right + AnchorStyles.Top, AnchorStyles)
                        cbo.TabIndex = tbIdx
                        'Setup the help string - for after clicking on the question mark button on the form
                        hp1.SetHelpString(cbo, String.Format(CultureInfo.CurrentUICulture, My.Resources.StringListAdvOptionsHelp, sets(i).Value.GetType.FullName, sets(i).Name))
                        hp1.SetShowHelp(cbo, True)
                        'Add the handler for the Combobox, and add it to the form
                        RemoveHandler cbo.LostFocus, AddressOf textbox_handler
                        AddHandler cbo.LostFocus, AddressOf textbox_handler
                        tlp.Controls.Add(cbo)
                    ElseIf TypeOf (sets(i).Value) Is SqlServerVersion Or sets(i).Name = "TargetServerVersion" Then
                        'If the setting is a server version enum type we fill a combobox
                        'Setup the label
                        lbl.Text = sets(i).Name
                        'Add the label
                        tlp.Controls.Add(lbl)
                        tlp.RowStyles.Add(x)
                        'Setup the ComboBox
                        Dim cbo As New ComboBox
                        cbo.DropDownStyle = ComboBoxStyle.DropDownList
                        cbo.FlatStyle = FlatStyle.Standard
                        cbo.Name = sets(i).TrueName
                        'smo.SqlServerversions - with an s - has more items (SQL7 & unknown)
                        For Each ver As SqlServerVersion In [Enum].GetValues(GetType(Smo.SqlServerVersion))
                            cbo.Items.Add(ver)
                        Next
                        'Need something besides Nothing else the GetType statements blow up
                        ' then later in the apply setting, need to watch that it dosn't try to stick 0 into the setting else that blows up too

                        Dim MuckedWithIt As Boolean = False
                        If sets(i).Value Is Nothing Then
                            sets(i).Value = 0
                            MuckedWithIt = True
                        End If

                        cbo.Tag = GetTypeAbbr(sets(i).Value.GetType) 'not necessary, but everywhere else so...
                        cbo.SelectedValue = If(MuckedWithIt, Nothing, sets(i).Value)
                        cbo.Anchor = CType(AnchorStyles.Left + AnchorStyles.Right + AnchorStyles.Top, AnchorStyles)
                        cbo.TabIndex = tbIdx
                        'Setup the help string - for after clicking on the question mark button on the form
                        hp1.SetHelpString(cbo, _
                                          String.Format(CultureInfo.CurrentUICulture, _
                                                        My.Resources.StringListAdvOptionsHelp, _
                                                        sets(i).Value.GetType.FullName, _
                                                        sets(i).Name))
                        hp1.SetShowHelp(cbo, True)
                        'Add the handler for the Combobox, and add it to the form
                        RemoveHandler cbo.LostFocus, AddressOf textbox_handler
                        AddHandler cbo.LostFocus, AddressOf textbox_handler
                        tlp.Controls.Add(cbo)

                        If MuckedWithIt Then
                            sets(i).Value = Nothing
                        End If

                    ElseIf TypeOf (sets(i).Value) Is String And sets(i).Name.ToLower.Contains("password") Then
                        'password is never saved
                        'Setup the label
                        lbl.Text = sets(i).Name
                        'Add the label
                        tlp.Controls.Add(lbl)
                        tlp.RowStyles.Add(x)
                        'Setup the textbox
                        Dim txt As New TextBox
                        txt.Name = sets(i).TrueName
                        txt.PasswordChar = Chr(149)
                        'txt.Tag = GetTypeAbbr(sets(i).Value.GetType)
                        txt.Text = CStr(sets(i).Value)
                        txt.Anchor = CType(AnchorStyles.Left + AnchorStyles.Right + AnchorStyles.Top, AnchorStyles)
                        txt.TabIndex = tbIdx
                        'Setup the textbox help string - for after clicking on the question mark button on the form
                        'Uses GetTypeName for type specific messages
                        hp1.SetHelpString(txt, String.Format(CultureInfo.CurrentUICulture, My.Resources.StringAdvOptionsHelp, GetTypeName(sets(i).Value.GetType)))
                        hp1.SetShowHelp(txt, True)
                        'Add the handler for the textbox, and add it to the form
                        RemoveHandler txt.KeyPress, AddressOf password_keypress_handler
                        RemoveHandler txt.LostFocus, AddressOf password_handler
                        AddHandler txt.KeyPress, AddressOf password_keypress_handler
                        AddHandler txt.LostFocus, AddressOf password_handler
                        tlp.Controls.Add(txt)
                    ElseIf IsTxtType(sets(i).Value.GetType) Then 'If the setting is a type we edit with a textbox use a textbox for editing
                        'Setup the label
                        lbl.Text = sets(i).Name
                        'Add the label
                        tlp.Controls.Add(lbl)
                        tlp.RowStyles.Add(x)
                        'Setup the textbox
                        Dim txt As New TextBox
                        txt.Name = sets(i).TrueName
                        'txt.Tag = GetTypeAbbr(sets(i).Value.GetType)
                        txt.Text = CStr(sets(i).Value)
                        txt.Anchor = CType(AnchorStyles.Left + AnchorStyles.Right + AnchorStyles.Top, AnchorStyles)
                        txt.TabIndex = tbIdx
                        'Setup the textbox help string - for after clicking on the question mark button on the form
                        'Uses GetTypeName for type specific messages
                        hp1.SetHelpString(txt, String.Format(CultureInfo.CurrentUICulture, My.Resources.StringAdvOptionsHelp, GetTypeName(sets(i).Value.GetType)))
                        hp1.SetShowHelp(txt, True)
                        'Add the handler for the textbox, and add it to the form
                        RemoveHandler txt.LostFocus, AddressOf textbox_handler
                        AddHandler txt.LostFocus, AddressOf textbox_handler
                        tlp.Controls.Add(txt)
                    End If
                    'Don't keep the label at autosize - remember it's most likely docked
                    lbl.AutoSize = False
                    'Increment the TabIndex
                    tbIdx += 1
                    pgb.Value += 1

                Catch ex As Exception
                    Throw New Exception(String.Format("Unable to load app.setting {1}.", sets(i).TrueName), ex)
                End Try

            Next
            x = New RowStyle
            lbl = New Label
            lbl.Anchor = CType(AnchorStyles.Left + AnchorStyles.Top, AnchorStyles)
            lbl.Text = ""
            x = New RowStyle
            'If we've reached the end of the settings - don't use the Fill DockStyle.
            lbl.Dock = DockStyle.None
            lbl.Width = CInt(tlp.Width / 2)
            tlp.RowStyles.Add(x)
            tlp.Controls.Add(lbl)
            tlp.Controls.Add(lbl)

            'Update the TabIndex of controls already on the form
            chkSaveOnExit.TabIndex = tbIdx + 1
            cmdOK.TabIndex = tbIdx + 2
            cmdCancel.TabIndex = tbIdx + 3
            cmdApply.TabIndex = tbIdx + 4
            pgb.Hide()
            tlp.Show()
            Me.Refresh()

        Catch ex As Exception

            Throw New Exception(String.Format("({0}.AfterSelect) Exception.", Me.Text), ex)

        End Try

    End Sub
    ''' <summary>
    ''' Gets the four letter type abbreviation for the passed type.
    ''' </summary>
    ''' <param name="typ">A system type that you wish to get the four letter type for.</param>
    ''' <returns>String containing the four letter abbrieviation for the type.</returns>
    ''' <remarks>Some are 3 letters with a space in front to make it four characters.</remarks>
    Private Function GetTypeAbbr(ByVal typ As Type) As String
        'Get a four character string to represent the type
        'This goes in the tag of the dynamic control and helps
        'the handlers determine what type to check for in textboxes
        'and buttons.
        If GetType(Boolean) Is typ Then
            Return String.Format(CultureInfo.CurrentUICulture, My.Resources.BooleanAdvOptionsAbbr)
        ElseIf GetType(Integer) Is typ Then
            Return String.Format(CultureInfo.CurrentUICulture, " {0}", My.Resources.NumericAdvOptionsAbbr)
        ElseIf GetType(String) Is typ Then
            Return String.Format(CultureInfo.CurrentUICulture, " {0}", My.Resources.StringAdvOptionsAbbr)
        End If
        Return Nothing
    End Function
    ''' <summary>
    ''' Gets a string containing the type name with a describing article (a, an, the) for display in a MsgBox
    ''' </summary>
    ''' <param name="typ">A system type that you wish to get the name of.</param>
    ''' <returns>String containing the name of the type and an article for describing it.</returns>
    Private Function GetTypeName(ByVal typ As Type) As String
        'Gets a string for the message box error message when trying to determine
        'if a setting was set properly
        If GetType(Boolean) Is typ Then
            Return String.Format(CultureInfo.CurrentUICulture, My.Resources.BooleanAdvOptions)
        ElseIf GetType(Integer) Is typ Then
            Return String.Format(CultureInfo.CurrentUICulture, My.Resources.NumericAdvOptions)
        ElseIf GetType(String) Is typ Then
            Return String.Format(CultureInfo.CurrentUICulture, My.Resources.StringAdvOptions)
        End If
        Return Nothing
    End Function
#End Region

#Region " Property Handlers "
    'checkbox, textbox, combobox
    'Handlers do not apply the values they set - just update the collection
    Private Sub checkbox_handler(ByVal sender As Object, ByVal e As System.EventArgs)
        'Checkboxes can only be boolean - update the setting in the collection
        Settings.SetValueByTrueName(sender.Name.ToString, sender.Checked)
    End Sub
    Private Sub textbox_handler(ByVal sender As Object, ByVal e As System.EventArgs)
        'ServerVersion, byte, char, decimal, double, integer, long, sbyte, short, single, string, uinteger, ulong, ushort
        'is a list of everything that a textbox might contain
        'basically, anything with a TypeAbbr that evaluates to integer
        'check the value in the textbox to make sure it
        'is of the correct type before adding it to the
        'collection - if it's not the correct type tell the
        'user so and select the offending value
        If CType(sender.Tag, String) = GetTypeAbbr(GetType(Integer)) Then
            Dim x As Integer = Nothing
            If Integer.TryParse(sender.Text.ToString, x) Then
                Settings.SetValueByTrueName(sender.Name.ToString, x)
            ElseIf sender.Name.ToString = "Scripting__Options_SMO_TargetServerVersion_530" Then
                For Each ver As SqlServerVersion In [Enum].GetValues(GetType(Smo.SqlServerVersion))
                    If sender.Text.ToString = ver.ToString Then
                        Settings.SetValueByTrueName(sender.Name.ToString, ver)
                    End If
                Next
            Else
                MessageBox.Show("Could not convert " & sender.Text.ToString & " to a Integer.")
                sender.Focus()
                sender.SelectAll()
            End If
        ElseIf CType(sender.Tag, String) = GetTypeAbbr(GetType(String)) Then
            Settings.SetValueByTrueName(sender.Name.ToString, sender.Text)
        End If
    End Sub
    Private Sub password_keypress_handler(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        Try
            If Not ((Control.ModifierKeys And Keys.Control) = Keys.Control) _
                And Not ((Control.ModifierKeys And Keys.Alt) = Keys.Alt) Then
                If CInt(sender.SelectionLength) > 0 Then
                    Password.Clear()
                    sender.Clear()
                    If CInt(sender.SelectionLength) < CInt(sender.TextLength) Then
                        Throw New Exception("For optimal password security, password string edit is not supported. Enter complete password")
                    End If
                Else
                    Select Case Asc(e.KeyChar)

                        Case Keys.Enter

                        Case Keys.Escape

                        Case Keys.Back
                            Try
                                sender.Text.Remove(sender.Text.Length, 1)
                                Password.RemoveAt(CInt(sender.Text.Length))
                            Catch ex As ArgumentOutOfRangeException
                                sender.Clear()
                                Password.Clear()
                            End Try
                        Case Else

                            If Char.IsLetterOrDigit(e.KeyChar) _
                            Or Char.IsPunctuation(e.KeyChar) _
                            Or Char.IsSymbol(e.KeyChar) Then
                                If sender Is Nothing Then
                                    sender.Clear()
                                    Password.Clear()
                                End If
                                Password.AppendChar(e.KeyChar)
                                e.KeyChar = Chr(149)
                            Else
                                Throw New Exception("Invalid character entered")
                            End If
                    End Select
                End If
            Else
                Throw New Exception("Invalid modifier key pressed")
            End If

        Catch ex As ApplicationException
            Dim ExTop As New ApplicationException(My.Resources.InvalidSQLPasswordChar, ex)
            ExTop.Source = Me.Text

            ExTop.Data.Add("AdvancedInformation.ASCIIValueofKeyPressed", Asc(e.KeyChar))

            If ((Control.ModifierKeys And Keys.Control) = Keys.Control) Then
                ExTop.Data.Add("AdvancedInformation.ControlKeyPressed", "Keys.Control")
            End If
            If ((Control.ModifierKeys And Keys.Alt) = Keys.Alt) Then
                ExTop.Data.Add("AdvancedInformation.ControlKeyPressed", "Keys.Alt")
            End If

            Dim emb As New ExceptionMessageBox(ExTop)
            emb.Show(Me)

        End Try

    End Sub

    Private Sub password_handler(ByVal sender As Object, ByVal e As System.EventArgs)
        If sender.Name.ToString = "SQL__Instance__A_Password_5" Then
            'CompareForm.Password = Password
            'CompareForm.Password1Changed = True
        End If
        If sender.Name.ToString = "SQL__Instance__B_Password_5" Then
            'CompareForm.Password2 = Password
            'CompareForm.Password2Changed = True
        End If

    End Sub

    Private Sub numericupdown_handler(ByVal sender As Object, ByVal e As System.EventArgs)
        Settings.SetValueByTrueName(sender.Name.ToString, CInt(sender.Value))
    End Sub

#End Region

#Region " Button Clicks "

    Private Sub cmdApply_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdApply.Click, Button1.Click
        ApplySettings() 'Apply the settings don't save, let the application save when it exits
    End Sub

    Private Sub cmdOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdOK.Click
        'if cmdOK_Click is called by pressing Return/Enter key set the focus to cmdOK, so that LostFocus is called
        ' and the  value is copied to settings
        cmdOK.Focus()

        ApplySettings() 'Apply the settings don't save, let the application save when it exits
        'Tell the dialog it can close and everything is ok
        Me.DialogResult = Windows.Forms.DialogResult.OK
    End Sub
    ''' <summary>
    ''' Applies the settings in the SettingInfoCollection to the actual setting in the My namespace.
    ''' </summary>
    Private Sub ApplySettings()
        'Cycle through each setting that we could edit and
        'update the real setting contained in the My namespace
        Try

            For Each si As SettingInfo In Settings
                Try

                    If (My.Settings.Item(si.TrueName) Is Nothing _
                        AndAlso Not si.Value Is Nothing) _
                    OrElse (Not My.Settings.Item(si.TrueName) Is Nothing _
                            AndAlso Not My.Settings.Item(si.TrueName).Equals(si.Value)) Then

                        'If any connection info is changed, init the compare form panel  
                        If InStr(si.TrueName, "SQL__Instance__A") = 1 Then
                            CompareForm.Panel1ConnectionSettingsChanged = True
                        End If

                        If InStr(si.TrueName, "SQL__Instance__B") = 1 Then
                            CompareForm.Panel2ConnectionSettingsChanged = True
                        End If

                        My.Settings.Item(si.TrueName) = si.Value

                    End If

                Catch ex As Exception

                    Throw New Exception(String.Format("Unable to apply value [{0}] to app.setting {1}.", si.Value, si.TrueName), ex)

                End Try

            Next

            'Update the SaveOnExit value
            My.Application.SaveMySettingsOnExit = chkSaveOnExit.Checked

        Catch ex As Exception

            Throw New Exception(String.Format("({0}.ApplySettings) Exception.", Me.Text), ex)

        End Try

    End Sub

    Private Sub ResetButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ResetButton.Click, Button2.Click

        'Cycle through each setting and clear if a user compare settings
        For Each sp As System.Configuration.SettingsProperty In My.Settings.Properties
            ' only the compare settings use underscores in the name so nothing else will get touched here
            ' EXCEPT WHEN IT SAVES using MY.Application.Settings. This could stomp on another tool.
            ' will watch for such a problem and fix if necessary

            'If the name doesn't have an underscore it must not be changed.
            'If the setting is ApplicationScoped - we aren't able to change it at runtime.
            'Check also if there is support on this form for the System.Type the setting is.
            If sp.Name.IndexOf("_") <> -1 AndAlso IsUserScope(sp) AndAlso IsAllowedType(sp) Then
                'Passed the tests create a new SettingInfo object
                Dim ResetSetting As New SettingInfo
                'Load the settings data into the object
                ResetSetting.LoadData(sp.Name)
                'Add the object to the collection
                ResetSetting.Value = sp.DefaultValue
            End If
        Next
        '       If MessageBox.Show("Resetting the user's settings will reset all SQLClue settings." & vbCrLf & _
        '                           "This includes all connection information to each local SQLClue" & vbCrLf & _
        '                           "data store. Current settings will not be recoverable. Before" & vbCrLf & _
        '                           "selecting [OK], consider creating a backup of the SQLClue " & vbCrLf & _
        '                           "user.config file for user [{0}]." & vbCrLf & vbCrLf & _
        '                           "Reset now?", _
        '                           "Reset to SQLClue application defaults?", _
        '                           MessageBoxButtons.OKCancel, _
        '                           MessageBoxIcon.Warning, _
        '                           MessageBoxDefaultButton.Button2, _
        '                           MessageBoxOptions.ServiceNotification, _
        '                           False) = Windows.Forms.DialogResult.OK Then
        '            My.Settings.Reset()
        Me.Close()
        'End If
    End Sub

    Private Sub ReloadButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ReloadButton.Click, Button3.Click

        If MessageBox.Show("Reloading the user settings will abandon all changes made" & vbCrLf & _
                           "since the Configuration Compare Settings dialog was opened." & vbCrLf & vbCrLf & _
                           "Reload now?", _
                           "Reload Last Saved Compare Settings?", _
                           MessageBoxButtons.OKCancel, _
                           MessageBoxIcon.Warning, _
                           MessageBoxDefaultButton.Button2, _
                           MessageBoxOptions.ServiceNotification, _
                           False) = Windows.Forms.DialogResult.OK Then

            My.Settings.Reload()
            Me.Close()

        End If

    End Sub

    Private Sub cmdCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdCancel.Click

        Me.DialogResult = Windows.Forms.DialogResult.Cancel

    End Sub

#End Region

End Class

''' <summary>The categories for the settings</summary>
''' <remarks></remarks>
Public Class SettingCategory
    Public MainCat As String
    Public SubCats As String
End Class

''' <summary>A collection of SettingCategory object</summary>
''' <remarks>used for returning the Main and SubCategories separately</remarks>
Public Class SettingCategoryCollection
    Inherits CollectionBase

    ''' <summary>
    ''' Adds a Setting
    ''' </summary>
    ''' <param name="item">A SettingCategory Object</param>
    ''' <remarks></remarks>
    Public Sub Add(ByVal item As SettingCategory)
        List.Add(item)
    End Sub
    ''' <summary>
    ''' Returns a specific element of the collection by position. Read-only.
    ''' </summary>
    ''' <param name="index">A numeric expression that specifies the position of an element of the collection. Index must be a number from 0 through the value of the Collection's Count Property.</param>
    ''' <returns>A SettingCategory object from the position specified.</returns>
    Default ReadOnly Property Item(ByVal index As Integer) As SettingCategory
        Get
            Return CType(List.Item(index), SettingCategory)
        End Get
    End Property

    ''' <summary>
    ''' Returns the position of a category and it's subcategory within the collection.
    ''' </summary>
    ''' <param name="name">The name of the main category</param>
    ''' <param name="subc">The subcategory(ies).  If multiple use dot-notation to separate the sub-categories</param>
    ''' <returns>An integer that represent the index of the position of the category/subcategory pair</returns>
    Public Function IndexOf(ByVal name As String, ByVal subc As String) As Integer
        For i As Integer = 0 To Count - 1
            If CType(List.Item(i), SettingCategory).MainCat = name And CType(List.Item(i), SettingCategory).SubCats = subc Then Return i
        Next
        Return -1
    End Function
End Class

''' <summary>
''' An object to hold an indvidual settings data
''' </summary>
Public Class SettingInfo

    Private _Name As String
    ''' <summary>
    ''' The display name of the setting.
    ''' </summary>
    ''' <value>The display name for the setting</value>
    ''' <returns>The display name for the setting</returns>
    Public Property Name() As String
        Get
            Return _Name
        End Get
        Set(ByVal value As String)
            _Name = value
        End Set
    End Property

    Private _Category As String
    ''' <summary>
    ''' The category to which the setting belongs.
    ''' </summary>
    ''' <value>The category to which the setting belongs</value>
    ''' <returns>The dot seperated category/sub-category to which the setting belongs</returns>
    Public ReadOnly Property Category() As String
        Get
            Return _Category
        End Get
    End Property
    ''' <summary>
    ''' Sets the category based on an array of the categories/sub-categories.
    ''' </summary>
    ''' <param name="values">The category/sub-category list.  Index 0 is the category and the sub-categories follow.</param>
    Public Sub SetCategory(ByVal values() As String)
        'The category should contain a dot separated list of values - implode does this nicely
        _Category = implode(".", values)
    End Sub
    ''' <summary>
    ''' Sets the category to the string provided.
    ''' </summary>
    ''' <param name="value">The actual dot separated category/sub-category list. (ex Category1.SubCat1.SubCat2)</param>
    Public Sub SetCategory(ByVal value As String)
        'If the value is already in dot separated format just updated (doesn't check)
        _Category = value
    End Sub
    ''' <summary>
    ''' Takes an array of values and concatenates them separating each item in the array with the value in chr.
    ''' </summary>
    ''' <param name="chr">A character or string which separates the values in "values".</param>
    ''' <param name="values">An array of strings to be concatenated and separated by chr.</param>
    ''' <returns>A string containing all the values in the array provided concatenated and separated by the value in chr.</returns>
    Private Function implode(ByVal chr As String, ByVal values() As String) As String
        'Taken from the php function implode
        'Setup an empty string for building
        Dim tmp As String = Nothing
        'Cycle through the array
        For Each str As String In values
            'Add the current value and the separator to the string
            tmp &= str & chr
        Next
        'The result has an extra delimiter on the end remove it
        tmp = tmp.TrimEnd(CChar(chr))
        'Return the finished result
        Return tmp
    End Function

    ''' <summary>
    ''' Loads a My.Settings setting name into the current SettingInfo object.
    ''' </summary>
    ''' <param name="str">A My.Settings name in the following format (Category_SubCategory1_SubCategory2_etc..._Setting__Name_SortIndex).</param>
    ''' <remarks>A single underscore separates the categories, name, and sort index.  A double underscore signifies a space.  Not providing a sort index, will give the setting a sort index of -1.</remarks>
    Public Sub LoadData(ByVal str As String)
        'Replace the double underscore with a space to allow the window
        'to show multi-word Settings names
        str = str.Replace("__", " ")
        'Check if the last value (separated by underscores) is numeric
        If MyNumeric(str.Substring(str.LastIndexOf("_") + 1)) Then
            'It is numeric - so use it as the SortIndex
            _Sort = Integer.Parse(str.Substring(str.LastIndexOf("_") + 1))
            'Remove it from the string
            str = str.Substring(0, str.LastIndexOf("_"))
        Else
            'Not numeric assign default SortIndex of -1
            _Sort = -1
        End If
        'Assign the name of the setting - should be that last value in the string
        _Name = str.Substring(str.LastIndexOf("_") + 1)
        'Get the category string (don't include the name)
        Dim cat As String = str.Substring(0, str.LastIndexOf("_"))
        'Assign the category (SetCategory takes a string array so use split)
        SetCategory(cat.Split(CChar("_")))
        'Get the value from the My namespace and assign it
        _Value = My.Settings.Item(TrueName)
    End Sub
    ''' <summary>
    ''' Checks if the string is a pure numeric value.
    ''' </summary>
    ''' <param name="val">A string containing the value to be checked.</param>
    ''' <returns>True if the value is numeric, false if the value isn't numeric.</returns>
    ''' <remarks>Cycles through each character in the value passed and checks if it is one of the following: 1234567890.  If it's not the value isn't numeric and it returns false.</remarks>
    Private Function MyNumeric(ByVal val As String) As Boolean
        'Create a string array for valid characters of a numeric string
        Dim chr() As String = {"1", "2", "3", "4", "5", "6", "7", "8", "9", "0"}
        'Check each character in the passed value
        For i As Integer = 0 To val.Length - 1
            If Array.IndexOf(chr, val.Substring(i, 1)) = -1 Then Return False 'Not a number
        Next
        'Successfully made it through the test - it is a number
        Return True
    End Function

    Private _Sort As Integer
    ''' <summary>
    ''' The SortIndex of the setting name.  Determines where the setting will be display on the form once it's category is selected.
    ''' </summary>
    ''' <value>Integer</value>
    ''' <returns>Integer</returns>
    Public Property SortIndex() As Integer
        Get
            Return _Sort
        End Get
        Set(ByVal value As Integer)
            _Sort = value
        End Set
    End Property

    Private _Value As Object
    ''' <summary>
    ''' The value of the setting.  Could be any object type from boolean to font.
    ''' </summary>
    ''' <value>Object containing the settings value.</value>
    ''' <returns>Object containing the settings value.</returns>
    Public Property Value() As Object
        Get
            Return _Value
        End Get
        Set(ByVal value As Object)
            _Value = value
        End Set
    End Property

    ''' <summary>
    ''' The true name of the setting.  Computes the original setting name from the category, name, and sort index.
    ''' </summary>
    ''' <returns>String containing the true setting name.</returns>
    Public ReadOnly Property TrueName() As String
        'Rebuilds the original setting name as specified in the My namespace
        Get
            'Create temporary string and give it the value of the category replacing the periods with underscores
            'Also add the name onto the end of it
            Dim tmp As String = _Category.Replace(".", "_") & "_" & _Name
            'If there is a valid SortIndex - add that to the end as well
            If _Sort <> -1 Then tmp &= "_" & SortIndex
            'Return a value with no spaces (use double underscores instead)
            Return tmp.Replace(" ", "__")
        End Get
    End Property

    ''' <summary>
    ''' Clones the current SettingInfo object to another variable.
    ''' </summary>
    ''' <returns>A copy of the current SettingInfo object.</returns>
    Public Function Clone() As SettingInfo
        'Build a copy of this SettingInfo and return it
        Dim tmp As New SettingInfo
        tmp.Name = _Name
        tmp.SetCategory(_Category)
        tmp.SortIndex = _Sort
        tmp.Value = _Value
        Return tmp
    End Function
End Class

''' <summary>
''' A collection of SettingInfo objects
''' </summary>
Public Class SettingInfoCollection
    Inherits CollectionBase

    ''' <summary>
    ''' Adds a SettingInfo object to the collection.
    ''' </summary>
    ''' <param name="item">A SettingInfo object</param>
    Public Sub Add(ByVal item As SettingInfo)
        'Add the item to the list
        List.Add(item)
    End Sub

    ''' <summary>
    ''' Returns a specific element of the collection by position. Read-only.
    ''' </summary>
    ''' <param name="index">A numeric expression that specifies the position of an element of the collection. Index must be a number from 0 through the value of the Collection's Count Property.</param>
    ''' <returns>A SettingInfo object from the position specified.</returns>
    Default Public ReadOnly Property Item(ByVal index As Integer) As SettingInfo
        Get
            'Return the selected item (make sure it is the correct type)
            Return CType(List.Item(index), SettingInfo)
        End Get
    End Property

    ''' <summary>
    ''' Sets the values of a SettingInfo object already in the collection.
    ''' </summary>
    ''' <param name="index">A numeric expression that specifies the position of an element you wish to change in the collection. Index must be a number from 0 through the value of the Collection's Count Property.</param>
    ''' <param name="value">A SettingInfo object that contains the values you wish to set to the element specified by Index.</param>
    Public Sub SetItem(ByVal index As Integer, ByVal value As SettingInfo)
        'Update an item already in the collection, must do it one value at a time
        'otherwise we just get a reference
        CType(List.Item(index), SettingInfo).SetCategory(value.Category)
        CType(List.Item(index), SettingInfo).Name = value.Name
        CType(List.Item(index), SettingInfo).Value = value.Value
        CType(List.Item(index), SettingInfo).SortIndex = value.SortIndex
    End Sub

    ''' <summary>
    ''' Get's all the SettingInfo objects in the collection that have the same category as the one specified.
    ''' </summary>
    ''' <param name="category">A string expression that specifies the category of the elements to retrieve.  Category must be in the dot separated format used by the SettingInfo objects (ex. Category.SubCategory.SubCat2)</param>
    ''' <returns>An array of SettingInfo objects that match the category specified.</returns>
    Public Function GetByCategory(ByVal category As String) As SettingInfo()
        'Create and empty array used to return the found data
        Dim tmp() As SettingInfo = Nothing
        Dim iT As Integer = 0
        Dim sortIt As Boolean = False
        'Cycle through all the settings
        For si As Integer = 0 To Count - 1
            'Does the category match?
            If CType(List.Item(si), SettingInfo).Category = category Then
                'Yes - Initialize current array itemj
                ReDim Preserve tmp(iT)
                'If the SortIndex isn't default (-1) then we need to sort the entire array when done
                If CType(List.Item(si), SettingInfo).SortIndex <> -1 Then sortIt = True
                'Copy the item to the array
                tmp(iT) = CType(List.Item(si), SettingInfo)
                'Increment the index
                iT += 1
            End If
        Next
        'Do we need to sort it?
        If sortIt Then
            'Yes - call the quicksort subroutine for this array
            Me.SortIt(tmp, 0, UBound(tmp))
        End If
        'Return the sorted (if necessary) array
        Return tmp
    End Function

    Public Function GetCategories() As SettingCategoryCollection
        Dim tmp As New SettingCategoryCollection
        Dim add As SettingCategory
        Dim name As String
        Dim subn As String
        For Each si As SettingInfo In List
            name = Nothing
            subn = Nothing
            If si.Category.IndexOf(".") = -1 Then name = si.Category Else name = si.Category.Substring(0, si.Category.IndexOf("."))
            If si.Category.IndexOf(".") = -1 Then subn = Nothing Else subn = si.Category.Substring(si.Category.IndexOf("."))
            If tmp.IndexOf(name, subn) = -1 Then
                add = New SettingCategory
                add.MainCat = name
                add.SubCats = subn
                tmp.Add(add)
            End If
        Next
        Return tmp
    End Function

    ''' <summary>
    ''' Sorts an array of SettingInfo objects by their SortIndex.
    ''' </summary>
    ''' <param name="SortArray">A reference to the array you wish to sort.</param>
    ''' <param name="First">The starting element of the selection you wish to sort (usually 0).</param>
    ''' <param name="Last">Then final element of the selection you wish to sort (usually the upper bound of the array).</param>
    ''' <remarks>Called from GetByCategory</remarks>
    Private Sub SortIt(ByRef SortArray() As SettingInfo, ByVal First As Long, ByVal Last As Long)
        'Copied and modified from the SortIt method in the form
        Dim Low As Long, High As Long
        Dim Temp As SettingInfo = Nothing
        Dim List_Separator As SettingInfo = Nothing
        Low = First
        High = Last
        List_Separator = SortArray(CInt((First + Last) / 2)).Clone
        Do
            Do While (SortArray(CInt(Low)).SortIndex < List_Separator.SortIndex)
                Low += 1
            Loop
            Do While (SortArray(CInt(High)).SortIndex > List_Separator.SortIndex)
                High -= 1
            Loop
            If (Low <= High) Then
                Temp = SortArray(CInt(Low)).Clone
                SortArray(CInt(Low)) = SortArray(CInt(High)).Clone
                SortArray(CInt(High)) = Temp.Clone
                Low += 1
                High -= 1
            End If
        Loop While (Low <= High)
        If (First < High) Then SortIt(SortArray, First, High)
        If (Low < Last) Then SortIt(SortArray, Low, Last)
    End Sub

    ''' <summary>
    ''' Sets the value of an SettingInfo object already in the collection by it's true name.
    ''' </summary>
    ''' <param name="name">The name of the setting as specified in the My.Settings object.</param>
    ''' <param name="value">The value you wish to set to the specified setting.</param>
    ''' <remarks>Called by the control handlers when the controls lose focus.</remarks>
    Public Sub SetValueByTrueName(ByVal name As String, ByVal value As Object)
        'Loop through all items in the list
        For i As Integer = 0 To Count - 1
            'Does the item's TrueName match the name passed?
            If CType(List(i), SettingInfo).TrueName = name Then
                'Yes - then update the value and exit
                CType(List(i), SettingInfo).Value = value
                Exit Sub
            End If
        Next
    End Sub

End Class
