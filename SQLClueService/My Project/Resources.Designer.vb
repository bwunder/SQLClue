﻿'------------------------------------------------------------------------------
' <auto-generated>
'     This code was generated by a tool.
'     Runtime Version:4.0.30319.468
'
'     Changes to this file may cause incorrect behavior and will be lost if
'     the code is regenerated.
' </auto-generated>
'------------------------------------------------------------------------------

Option Strict On
Option Explicit On

Imports System

Namespace My.Resources
    
    'This class was auto-generated by the StronglyTypedResourceBuilder
    'class via a tool like ResGen or Visual Studio.
    'To add or remove a member, edit your .ResX file then rerun ResGen
    'with the /str option, or rebuild your VS project.
    '''<summary>
    '''  A strongly-typed resource class, for looking up localized strings, etc.
    '''</summary>
    <Global.System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0"),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     Global.System.Runtime.CompilerServices.CompilerGeneratedAttribute(),  _
     Global.Microsoft.VisualBasic.HideModuleNameAttribute()>  _
    Friend Module Resources
        
        Private resourceMan As Global.System.Resources.ResourceManager
        
        Private resourceCulture As Global.System.Globalization.CultureInfo
        
        '''<summary>
        '''  Returns the cached ResourceManager instance used by this class.
        '''</summary>
        <Global.System.ComponentModel.EditorBrowsableAttribute(Global.System.ComponentModel.EditorBrowsableState.Advanced)>  _
        Friend ReadOnly Property ResourceManager() As Global.System.Resources.ResourceManager
            Get
                If Object.ReferenceEquals(resourceMan, Nothing) Then
                    Dim temp As Global.System.Resources.ResourceManager = New Global.System.Resources.ResourceManager("SQLClueSvc.Resources", GetType(Resources).Assembly)
                    resourceMan = temp
                End If
                Return resourceMan
            End Get
        End Property
        
        '''<summary>
        '''  Overrides the current thread's CurrentUICulture property for all
        '''  resource lookups using this strongly typed resource class.
        '''</summary>
        <Global.System.ComponentModel.EditorBrowsableAttribute(Global.System.ComponentModel.EditorBrowsableState.Advanced)>  _
        Friend Property Culture() As Global.System.Globalization.CultureInfo
            Get
                Return resourceCulture
            End Get
            Set
                resourceCulture = value
            End Set
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to SQLClueSvc SQL Configuration Archive Cancelled: Schedule Id {0}.
        '''</summary>
        Friend ReadOnly Property ArchiveCancelled() As String
            Get
                Return ResourceManager.GetString("ArchiveCancelled", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to SQLClueSvc SQL Configuration Archive Complete: Schedule Id {0}.
        '''</summary>
        Friend ReadOnly Property ArchiveComplete() As String
            Get
                Return ResourceManager.GetString("ArchiveComplete", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to SQLClueSvc SQL Configuration Archive Exception: Schedule Id {0}.
        '''</summary>
        Friend ReadOnly Property ArchiveException() As String
            Get
                Return ResourceManager.GetString("ArchiveException", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to SQLClueSvc Scheduling next run date.
        '''</summary>
        Friend ReadOnly Property ArchiveRescheduled() As String
            Get
                Return ResourceManager.GetString("ArchiveRescheduled", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to SQLClueSvc SQL Configuration Archive Starting: Schedule Id {0}.
        '''</summary>
        Friend ReadOnly Property ArchiveStarting() As String
            Get
                Return ResourceManager.GetString("ArchiveStarting", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to {0} {1} 
        '''Additional details of this Archive operation exception may be seen by running Archive ID {2} interactively using the SQLClue User Interface. The complete exception stack trace will be displayed in that scenario..
        '''</summary>
        Friend ReadOnly Property ArchiveWorkerExceptionInfoMsg() As String
            Get
                Return ResourceManager.GetString("ArchiveWorkerExceptionInfoMsg", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to For more information or support for this error see http://www.bwunder.com or email Bill at bwunder@yahoo.com..
        '''</summary>
        Friend ReadOnly Property ExceptionInfoMsg() As String
            Get
                Return ResourceManager.GetString("ExceptionInfoMsg", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to SQLClueSvc Data Center Runbook Automation Exception.
        '''</summary>
        Friend ReadOnly Property RunbookException() As String
            Get
                Return ResourceManager.GetString("RunbookException", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to IsActive And IntervalBaseDt &lt;= &apos;{0}&apos;.
        '''</summary>
        Friend ReadOnly Property ScheduleRowFilter() As String
            Get
                Return ResourceManager.GetString("ScheduleRowFilter", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to IntervalBaseDt, InstanceName, UseEventNotifications DESC.
        '''</summary>
        Friend ReadOnly Property ScheduleSortOrder() As String
            Get
                Return ResourceManager.GetString("ScheduleSortOrder", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to SQLClueSvc Automation Controller Service Exception.
        '''</summary>
        Friend ReadOnly Property ServiceControlException() As String
            Get
                Return ResourceManager.GetString("ServiceControlException", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to SQLClueSvc.
        '''</summary>
        Friend ReadOnly Property ServiceName() As String
            Get
                Return ResourceManager.GetString("ServiceName", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to SQLClueSvc Automation Controller Service Paused.
        '''</summary>
        Friend ReadOnly Property SvcPauseMsg() As String
            Get
                Return ResourceManager.GetString("SvcPauseMsg", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to SQLClueSvc Automation Controller Service Resumed.
        '''</summary>
        Friend ReadOnly Property SvcResumeMsg() As String
            Get
                Return ResourceManager.GetString("SvcResumeMsg", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to SQLClueSvc Automation Controller Service Starting .
        '''</summary>
        Friend ReadOnly Property SvcStartMsg() As String
            Get
                Return ResourceManager.GetString("SvcStartMsg", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to SQLClueSvc Automation Controller Service Stopping.
        '''</summary>
        Friend ReadOnly Property SvcStopMsg() As String
            Get
                Return ResourceManager.GetString("SvcStopMsg", resourceCulture)
            End Get
        End Property
    End Module
End Namespace
