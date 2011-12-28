/************************************************************************
Removes configuration created by instSQLClueDDLEventNotifications
NOTE: DOES NOT DISABLE SERVICE BROKER to avoid the single user moment

DO NOT run this script using SSMS, instead uncheck the 'Use Events' 
check box in the UI to disable events 

msdb is default db for SQLClue DDL event queue but is configurable 
in user.config as TargetEventNotificationDatabase

Use msdb;
***************************************************************************/

IF  EXISTS (SELECT * FROM [sys].[objects] 
            WHERE [object_id] = OBJECT_ID(N'[dbo].[pSQLClueGetDDLEvent]') 
            AND [type] IN (N'P', N'PC'))
 DROP PROCEDURE [dbo].[pSQLClueGetDDLEvent];
GO

IF  EXISTS (SELECT * FROM [sys].[objects] 
            WHERE [object_id] = OBJECT_ID(N'[dbo].[pSQLClueNotifyQueueDisabled]') 
            AND [type] IN (N'P', N'PC'))
 DROP PROCEDURE [dbo].[pSQLClueNotifyQueueDisabled];
GO

IF EXISTS (SELECT * 
           FROM [sys].[endpoints] 
           WHERE [name] = 'SQLClueDDLEndPoint')
 DROP ENDPOINT SQLClueDDLEndPoint; 
GO 

IF EXISTS (SELECT * FROM [sys].[server_event_notifications]
           WHERE [name] = 'SQLClueDDLEventNotification')
 DROP EVENT NOTIFICATION SQLClueDDLEventNotification ON SERVER;
GO

IF EXISTS (SELECT * FROM [sys].[services]
           WHERE name = 'http://sqlclue.bwunder.com/local/MonitorService')
 DROP SERVICE [http://sqlclue.bwunder.com/local/MonitorService]; 
GO

-- Only use this notification to alert if the queue has become disabled
IF OBJECT_ID('dbo.SQLClueMonitorQueue') IS NOT NULL
 DROP QUEUE [dbo].[SQLClueMonitorQueue];
GO

IF EXISTS (SELECT * FROM [sys].[event_notifications]
               WHERE [name] = 'SQLClueQueueDisabledNotification')
 DROP EVENT NOTIFICATION SQLClueQueueDisabledNotification ON QUEUE [dbo].[SQLClueDDLEventsQueue];
GO 

IF EXISTS (SELECT * from [sys].[services]
           WHERE [name] ='http://sqlclue.bwunder.com/local/DDLEventsService') 
 DROP SERVICE [http://sqlclue.bwunder.com/local/DDLEventsService]; 
GO

If OBJECT_ID('dbo.SQLClueDDLEventsQueue','SQ') IS NOT NULL
 DROP QUEUE [dbo].[SQLClueDDLEventsQueue]; 
GO

IF EXISTS (SELECT * FROM [sys].[routes] 
           WHERE [name] = 'SQLClueRoute')
 DROP ROUTE [SQLClueRoute]; 
 GO

IF EXISTS (SELECT * FROM [msdb].[dbo].[sysalerts]
           WHERE NAME =N'SQLClue: Event Reader Error')
 EXEC [msdb].[dbo].[sp_delete_alert] @name=N'SQLClue: Event Reader Error';
GO

IF EXISTS (SELECT * FROM [msdb].[dbo].[sysalerts]
           WHERE [name] =N'SQLClue: Queue Disabled')
 EXEC [msdb].[dbo].[sp_delete_alert] @name=N'SQLClue: Queue Disabled';
GO

IF  EXISTS (SELECT * FROM sys.objects 
            WHERE object_id = OBJECT_ID(N'[dbo].[pSQLClueShowQueue]') 
            AND type in (N'P', N'PC'))
 DROP PROCEDURE [dbo].[pSQLClueShowQueue];
GO

IF  EXISTS (SELECT * FROM sys.objects 
            WHERE object_id = OBJECT_ID(N'[dbo].[pSQLClueListEvents]') 
            AND type in (N'P', N'PC'))
 DROP PROCEDURE [dbo].[pSQLClueListEvents];
GO

IF  EXISTS (SELECT * FROM sys.objects 
            WHERE object_id = OBJECT_ID(N'[dbo].[pSQLClueGetEvent]') 
            AND type in (N'P', N'PC'))
 DROP PROCEDURE [dbo].[pSQLClueGetEvent];
GO

IF  EXISTS (SELECT * FROM sys.objects 
            WHERE object_id = OBJECT_ID(N'[dbo].[pSQLClueCheckQueueStatus]') 
            AND type in (N'P', N'PC'))
 DROP PROCEDURE [dbo].[pSQLClueCheckQueueStatus];
GO

IF  EXISTS (SELECT * FROM sys.objects 
            WHERE object_id = OBJECT_ID(N'[dbo].[pSQLClueResetQueueStatus]') 
            AND type in (N'P', N'PC'))
 DROP PROCEDURE [dbo].[pSQLClueResetQueueStatus];
GO
