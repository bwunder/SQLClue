/***********************************************************************************************
 Creates all configuration on a target instance to process SQLClue DDL Event Notifications
 NOTE: Enabling the SERVICE BROKER requires exclusive access to the database

 DO NOT run this script using SSMS, instead check the 'Use Events' 
 check box in the UI to enable SQLClue events 

 The SQLClue Windows service will read from the SQLClueDDLEventsQueue queue
 Both the reader and the monitor may use optional local alerts and will also be monitored 
 by the SQLClue Windows service at each scheduled archive. The Windows service also checks 
 the state of the queue and tries to handle the queue down event. The queue down event handler 
 needs an Operator (see below for details)
  
 SQL Express Edition target work around notes for the queue monitor operator: There is no SQL Agent 
 Service to process alerts in SQL Express. If there is a monitoring dashboard available that can be 
 trained it to watch for the queue down event in the log use it. In this scenario the alert may not 
 be needed. For SQLExpress it is possible to use a statement like
   sp_add_operator @name ='<DB>', @email_address = '<me@mydomain.com>'
 However it is important to realize that there will be no active monitoring for queues in this usage 
 since SQLExpress does not have a SQL Agent Service. The SQLClue scheduler will always check the queue 
 status on each target at each scheduled run time when notifications are enabled serving as a redundantcy
 to this monitor.

 conversation_group_id does not follow QUOTED_IDENTIFIER rules?? - do not put brackets around it 

 msdb is default db for SQLClue DDL event queue but is configurable 
 in user.config as TargetEventNotificationDatabase

Use msdb;
************************************************************************************************/

-- Service broker must be enabled
-- requires exclusive db acess to enable
DECLARE @rc [INT]
 , @db [NVARCHAR](128)
SET @db = db_name()
IF EXISTS (SELECT * FROM [sys].[databases]  
           WHERE [name] = @db 
           AND [is_broker_enabled] = 0)
 BEGIN
  DECLARE @SQLStr AS [NVARCHAR](MAX)
  SET @SQLStr = 'ALTER DATABASE ' + @db +
    ' SET ENABLE_BROKER'
  EXEC @rc = sp_executesql @SQLStr
  IF @@ERROR <> 0 OR @rc <> 0 
   -- abort the script
   RAISERROR('instSQLClueDDLEventNotifications.sql failed to enable Service Broker in DB %s',20,1, @db) WITH LOG
 END

GO

IF  EXISTS (SELECT * FROM [sys].[objects] WHERE [object_id] = object_id(N'[dbo].[pSQLClueGetDDLEvent]') AND [type] in (N'P', N'PC'))
 DROP PROCEDURE [dbo].[pSQLClueGetDDLEvent]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Bill Wunder
-- Create date: April 1, 2007
-- Description:	Get a SQLClue Configuration event
-- Retrieve the next one (1) change from the 
-- notification queue. OUTPUT parm for better 
-- performance from SQLClue Service. Does not 
-- wait once the last queued item is processed
-- to avoid known SB distributed transaction 
-- escalation problem. 
-- =============================================
CREATE PROCEDURE [dbo].[pSQLClueGetDDLEvent]
 ( @EventData as [XML] OUTPUT)
AS
BEGIN

 SET NOCOUNT ON;
 DECLARE @ConversationHandle [UNIQUEIDENTIFIER]
  , @ConversationGroupId [UNIQUEIDENTIFIER]
  , @ConversationGroup [NVARCHAR](50)
  , @ServiceName [NVARCHAR](128)
  , @MessageBody [XML]
  , @MessageTypeName [NVARCHAR](128)
  , @FailedMessage [NVARCHAR](MAX) ;
 
 BEGIN TRY

  -- transaction must be managed by caller          
  If @@TRANCOUNT > 0
   BEGIN

    --WORKAROUND: no WAITFOR to avoid failure at escalation to distributed txn
    GET CONVERSATION GROUP @ConversationGroupId 
    FROM [dbo].[SQLClueDDLEventsQueue];

    IF @ConversationGroupId IS NOT NULL
     BEGIN
      RECEIVE TOP(1)
         @ConversationHandle = [conversation_handle]
       , @ServiceName = [service_name]
       , @MessageTypeName = [message_type_name]
       , @MessageBody = CASE WHEN [validation] = 'X' 
                             THEN CAST([message_body] AS XML)
                             ELSE CAST(N'<none />' AS XML)
                             END
      FROM [dbo].[SQLClueDDLEventsQueue]
      WHERE conversation_group_id = @ConversationGroupId;

      IF @@ROWCOUNT = 1 
       BEGIN  
        -- only want to work with Events
        IF @MessageTypeName = 'http://schemas.microsoft.com/SQL/Notifications/EventNotification'
        AND @ServiceName = 'http://sqlclue.bwunder.com/local/DDLEventsService'
          SELECT @EventData = @MessageBody 
        ELSE
         IF @ServiceName = 'http://sqlclue.bwunder.com/local/DDLEventsService'  
           RAISERROR('Invalid message type "%s" found in the [dbo].[SQLClueDDLEventsQueue]',16,1,@MessageTypeName);
         ELSE
           RAISERROR('Invalid use of [dbo].[SQLClueDDLEventsQueue] by service "%s"',16,1,@ServiceName);

       END -- there is a dialog item to process

      END -- conversation group exists

    END -- caller provided a transaction

   ELSE

     RAISERROR('Invalid transaction state.',16,1);

 END TRY

 BEGIN CATCH

  SET @FailedMessage = '(' + OBJECT_NAME(@@PROCID) + ') failed at line ' 
                     + CAST(ISNULL(ERROR_LINE(),0) AS VARCHAR(10))   
                     + ' on SQL Instance ' + ISNULL(@@SERVERNAME,'?') + CHAR(13) + CHAR(10) 
                     + 'ConversationGroupId ' + ISNULL(CAST(@ConversationGroupId AS NVARCHAR(36)),'?') + CHAR(13) + CHAR(10)
                     + 'ConversationHandle ' + ISNULL(CAST(@ConversationHandle AS NVARCHAR(36)),'?') + CHAR(13) + CHAR(10)
                     + 'Error ' + ISNULL(CAST(ERROR_NUMBER() AS NVARCHAR(10)),'?') 
                     + ', Severity ' + ISNULL(CAST(ERROR_SEVERITY() AS NVARCHAR(10)),'?') 
                     + ', State ' + ISNULL(CAST(ERROR_STATE() AS NVARCHAR(10)),'?') + CHAR(13) + CHAR(10)
                     + '(' + ISNULL(ERROR_MESSAGE(),'error message unknown') + ')';

   RAISERROR (@FailedMessage, 16,1) WITH LOG;

 END CATCH;

END; 
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[pSQLClueNotifyQueueDisabled]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[pSQLClueNotifyQueueDisabled];

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Bill Wunder
-- Create date: April 1, 2006
-- Description:	raise error while
-- the primary Queue is disabled 
-- =============================================
CREATE PROCEDURE [dbo].[pSQLClueNotifyQueueDisabled] 
AS
 BEGIN
  SET NOCOUNT ON;
  DECLARE @ConversationHandle [UNIQUEIDENTIFIER]
   , @ConversationGroupId [UNIQUEIDENTIFIER]
   , @ConversationGroup [NVARCHAR](50)
   , @ServiceName [NVARCHAR](128)
   , @MessageBody [XML]
   , @MessageTypeName [NVARCHAR](128)
   , @FailedMessage [NVARCHAR](MAX) ;
  
  BEGIN TRY
   BEGIN TRANSACTION
    BEGIN
     -- Get next event.
     WAITFOR(
      GET CONVERSATION GROUP @ConversationGroupId 
      FROM [dbo].[SQLClueMonitorQueue]),
      TIMEOUT 600 ;

     IF @ConversationGroupId IS NOT NULL
      BEGIN
       -- Process one event per procedure call.
 
       RECEIVE TOP(1)
          @ConversationHandle = [conversation_handle]
        , @ServiceName = [service_name]
        , @MessageTypeName = [message_type_name]
        , @MessageBody = CASE WHEN [validation] = 'X' 
                              THEN CAST([message_body] AS XML)
                              ELSE CAST(N'<none />' AS XML)
                              END
       FROM [dbo].[SQLClueMonitorQueue]
       WHERE conversation_group_id = @ConversationGroupId;

       IF @@ROWCOUNT > 0 
        BEGIN  
         IF @MessageTypeName = 'http://schemas.microsoft.com/SQL/Notifications/EventNotification'
         OR @MessageTypeName = 'http://schemas.microsoft.com/SQL/ServiceBroker/DialogTimer'
          BEGIN
           -- queue coud have been re-enabled since last timer so check the status now  
           IF NOT EXISTS(SELECT * FROM [sys].[service_queues] 
                         WHERE [name] = 'SQLClueDDLEventsQueue'
                         AND [is_receive_enabled] = 1
                         AND [is_enqueue_enabled] = 1) 
            BEGIN
             -- log an error and start a timer to log again in 1 hour
             -- relies upon someone or something to be monitoring the Application Event Log
             If OBJECT_ID('SQLClueDDLEventsQueue') IS NULL
              Raiserror('SQLClue Service Broker Queue [SQLClueDDLEventsQueue] not found!', 16, 1) WITH LOG; 
             ELSE
              Raiserror('SQLClue Service Broker Queue [SQLClueDDLEventsQueue] is disabled!', 16, 1) WITH LOG; 
             BEGIN CONVERSATION TIMER (@ConversationHandle) TimeOut = 3600; 
                      -- 12 hours 43200;
            END
          END
         ELSE
          RAISERROR('Invalid message type "%s" found in the [dbo].[SQLClueMonitorQueue]',16,1,@MessageTypeName);
        END -- dialog item to process
       ELSE
        BEGIN 
         SET @ConversationGroup = CAST(@ConversationGroupId as NVARCHAR(50))
         RAISERROR('No events found in [SQLClueMonitorQueue] for Conversation Group "%s"',16,1, @ConversationGroup);          
        END
      END -- conversation exists
     ELSE
      RAISERROR('No dialogs on [SQLClueMonitorQueue]',16,1);
    END -- call is in a transaction
  COMMIT TRANSACTION  
 END TRY
 BEGIN CATCH
   WHILE @@TRANCOUNT > 0
    ROLLBACK TRANSACTION
   SET @FailedMessage = '(' + OBJECT_NAME(@@PROCID) + ') failed at line ' 
                      + CAST(ISNULL(ERROR_LINE(),0) AS VARCHAR(10))   
                      + ' on SQL Instance ' + ISNULL(@@SERVERNAME,'?') + CHAR(13) + CHAR(10) 
                      + 'ConversationGroupId ' + ISNULL(CAST(@ConversationGroupId AS NVARCHAR(36)),'?') + CHAR(13) + CHAR(10)
                      + 'ConversationHandle ' + ISNULL(CAST(@ConversationHandle AS NVARCHAR(36)),'?') + CHAR(13) + CHAR(10)
                      + 'Error ' + ISNULL(CAST(ERROR_NUMBER() AS NVARCHAR(10)),'?') 
                      + ', Severity ' + ISNULL(CAST(ERROR_SEVERITY() AS NVARCHAR(10)),'?') 
                      + ', State ' + ISNULL(CAST(ERROR_STATE() AS NVARCHAR(10)),'?') + CHAR(13) + CHAR(10)
                      + '(' + ISNULL(ERROR_MESSAGE(),'error message unknown') + ')';
    RAISERROR (@FailedMessage, 16,1) WITH LOG;
  END CATCH;
 END; 
GO

-- an endpoint to forward records to the central server queue

--IF EXISTS (SELECT *
--           FROM [sys].[endpoints] 
--           WHERE [name] = 'SQLClueDDLEndPoint')
-- DROP ENDPOINT SQLClueDDLEndPoint 
 
GO 
-- default Listener_port is 4022
-- RC4 encryption select based on my understanding that it is found more
-- often than other - perhaps better - algorithms
-- change the endpoint as necessary to meet the environmental requirements
--IF NOT EXISTS (SELECT * FROM [sys].[endpoints]
--               WHERE [name] = 'SQLClueDDLEndPoint')
--EXEC sp_executesql N'CREATE ENDPOINT SQLClueDDLEndPoint 
-- STATE = STARTED
-- AS TCP (LISTENER_PORT = 4022, LISTENER_IP = ALL)
-- FOR SERVICE_BROKER 
--   ( AUTHENTICATION = WINDOWS NEGOTIATE  
--   , ENCRYPTION = SUPPORTED ALGORITHM RC4 
--   , MESSAGE_FORWARDING = DISABLED );'

GO

IF EXISTS (SELECT * FROM [sys].[server_event_notifications]
           WHERE [name] = 'SQLClueDDLEventNotification')
 DROP EVENT NOTIFICATION SQLClueDDLEventNotification ON SERVER;

GO 

DECLARE @SQLStr AS nvarchar(max);
SELECT @SQLStr = 'CREATE EVENT NOTIFICATION SQLClueDDLEventNotification ' 
               + 'ON SERVER ' 
               + 'WITH FAN_IN ' 
               + 'FOR DDL_SERVER_LEVEL_EVENTS ' 
               + ', DDL_TABLE_EVENTS ' 
               + ', DDL_VIEW_EVENTS ' 
               + ', DDL_INDEX_EVENTS ' 
               + ', DDL_SYNONYM_EVENTS ' 
               + ', DDL_FUNCTION_EVENTS ' 
               + ', DDL_PROCEDURE_EVENTS ' 
               + ', DDL_TRIGGER_EVENTS ' 
--               + ', DDL_EVENT_NOTIFICATION_EVENTS '  -- caused all notifications to fail
               + ', DDL_ASSEMBLY_EVENTS ' 
               + ', DDL_TYPE_EVENTS ' 

               -- these are in DDL_DATABASE_SECURITY_EVENTS
               + ', DDL_SCHEMA_EVENTS ' 
               + ', DDL_ROLE_EVENTS '
               + ', DDL_CERTIFICATE_EVENTS '
               + ', DDL_AUTHORIZATION_DATABASE_EVENTS '
               + ', DDL_GDR_DATABASE_EVENTS '
               + ', DDL_APPLICATION_ROLE_EVENTS ' 
               + ', DDL_USER_EVENTS '

               + ', DDL_MESSAGE_TYPE_EVENTS ' 
               + ', DDL_CONTRACT_EVENTS ' 
--               + ', DDL_QUEUE_EVENTS '  -- caused all notifications to fail
--               + ', DDL_SERVICE_EVENTS '  -- caused all notifications to fail
               + ', DDL_ROUTE_EVENTS ' 
               + ', DDL_REMOTE_SERVICE_BINDING_EVENTS ' 
               + ', DDL_XML_SCHEMA_COLLECTION_EVENTS ' 
               + ', DDL_PARTITION_EVENTS ' 
               + 'TO SERVICE ''http://sqlclue.bwunder.com/local/DDLEventsService'', ''' 
               +  CAST([service_broker_guid] AS varchar(50)) + ''';'
FROM [master].[sys].[databases]
WHERE [name] = DB_NAME();
EXEC sp_executesql @SQLStr;

GO

IF EXISTS (SELECT * FROM [sys].[services]
           WHERE [name] = 'http://sqlclue.bwunder.com/local/MonitorService')
 DROP SERVICE [http://sqlclue.bwunder.com/local/MonitorService]

GO

IF OBJECT_ID('dbo.SQLClueMonitorQueue','SQ') IS NOT NULL
 DROP QUEUE [dbo].[SQLClueMonitorQueue]

GO

CREATE QUEUE [dbo].[SQLClueMonitorQueue] 
WITH STATUS = ON
 , ACTIVATION ( STATUS = ON
              , PROCEDURE_NAME = [dbo].[pSQLClueNotifyQueueDisabled]
              , MAX_QUEUE_READERS = 1
              , EXECUTE AS OWNER )       
ON [DEFAULT]; 
 
GO


IF NOT EXISTS (SELECT * FROM [sys].[services]
               WHERE [name] = 'http://sqlclue.bwunder.com/local/MonitorService')
 CREATE SERVICE [http://sqlclue.bwunder.com/local/MonitorService] 
 AUTHORIZATION [dbo] 
 ON QUEUE [dbo].[SQLClueMonitorQueue] ([http://schemas.microsoft.com/SQL/Notifications/PostEventNotification]);

GO


IF EXISTS (SELECT * FROM [sys].[event_notifications]
               WHERE [name] = 'SQLClueQueueDisabledNotification')
 DROP EVENT NOTIFICATION SQLClueQueueDisabledNotification ON QUEUE [dbo].[SQLClueDDLEventsQueue]
GO

-- create the queue and service(no acticvation here, reader is called on a schedule)
-- the contract is built in to Service Broker for Event Notifications

IF EXISTS (SELECT * FROM [sys].[services]
           WHERE [name] ='http://sqlclue.bwunder.com/local/DDLEventsService') 
 DROP SERVICE [http://sqlclue.bwunder.com/local/DDLEventsService] 

GO

-- activation is off; the service uses GetSQLClueDDLEvent instead
If OBJECT_ID('dbo.SQLClueDDLEventsQueue','SQ') IS NULL
 CREATE QUEUE [dbo].[SQLClueDDLEventsQueue] 
 WITH STATUS = ON
 ON [DEFAULT]; 

GO

IF NOT EXISTS(SELECT * FROM [sys].[services]
              WHERE [name] = 'http://sqlclue.bwunder.com/local/DDLEventsService')
 CREATE SERVICE [http://sqlclue.bwunder.com/local/DDLEventsService] 
 AUTHORIZATION [dbo] 
 ON QUEUE [dbo].[SQLClueDDLEventsQueue] ([http://schemas.microsoft.com/SQL/Notifications/PostEventNotification]);

GO

DECLARE @SQLStr AS nvarchar(max);
SELECT @SQLStr = 'CREATE EVENT NOTIFICATION SQLClueQueueDisabledNotification ' + 
                 'ON QUEUE [dbo].[SQLClueDDLEventsQueue] ' + 
                 'FOR BROKER_QUEUE_DISABLED ' +
                 'TO SERVICE ''http://sqlclue.bwunder.com/local/MonitorService'', ''' 
                             + CAST(service_broker_guid AS varchar(50)) + ''';'
FROM [master].[sys].[databases]
WHERE name = DB_NAME();
EXEC sp_executesql @SQLStr; 

GO 

-- the service and the notification are defined in the same db so address is 'Local'
DECLARE @SQLStr AS NVARCHAR(MAX);
IF NOT EXISTS (SELECT * FROM [sys].[routes] 
               WHERE [name] = 'SQLClueRoute')
 BEGIN
  SELECT @SQLStr = 'CREATE ROUTE [SQLClueRoute] ' + 
                   'AUTHORIZATION [dbo] ' + 
                   'WITH SERVICE_NAME = N''http://sqlclue.bwunder.com/local/DDLEventsService'' ' +
                    ', ADDRESS = N''LOCAL'' ' +
                    ', BROKER_INSTANCE = ''' + CAST([service_broker_guid] AS [VARCHAR](50)) + ''';'
  FROM [master].[sys].[databases]
  WHERE [name] = DB_NAME();
  EXEC sp_executesql @SQLStr; 
 END

GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[pSQLClueShowQueue]') AND type in (N'P', N'PC'))
 DROP PROCEDURE [dbo].[pSQLClueShowQueue];

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Bill Wunder
-- Create date: April 1, 2006
-- Description:	preview all changes not yet 
-- processed into the repository
-- =============================================
CREATE PROCEDURE [dbo].[pSQLClueShowQueue]
AS
 BEGIN

  SELECT 
     [QueuingOrder]
   , ISNULL([MessageBody].value('(/EVENT_INSTANCE/EventType)[1]','NVARCHAR(128)'),'') AS [EventType]
   , ISNULL([MessageBody].value('(/EVENT_INSTANCE/DatabaseName)[1]','NVARCHAR(128)'),'') AS [DatabaseName]
   , ISNULL([MessageBody].value('(/EVENT_INSTANCE/SchemaName)[1]','NVARCHAR(128)'),'') AS [SchemaName]
   , ISNULL([MessageBody].value('(/EVENT_INSTANCE/ObjectName)[1]','NVARCHAR(128)'),'') AS [ObjectName]
   , ISNULL([MessageBody].value('(/EVENT_INSTANCE/PostTime)[1]','NVARCHAR(128)'),'')  AS [PostTime]
  FROM (SELECT CASE WHEN [validation] = 'X' 
                    THEN CAST([message_body] AS XML)
                    ELSE CAST(N'<none />' AS XML)
                    END AS [MessageBody]
         , [queuing_order] AS [QueuingOrder]  
        FROM [dbo].[SQLClueDDLEventsQueue]) derived
  ORDER BY [QueuingOrder];

 END

GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[pSQLClueListEvents]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[pSQLClueListEvents];

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Bill Wunder
-- Create date: April 1, 2006
-- Description:	list XML event payload for all  
-- changes not yet processed into the repository
-- =============================================
CREATE PROCEDURE [dbo].[pSQLClueListEvents]
AS
 BEGIN

  SELECT [queuing_order] AS [QueuingOrder]
   , CASE [status] WHEN 0 THEN 'Received message'
                   WHEN 1 THEN 'Ready'
                   WHEN 2 THEN 'Not yet complete'
                   WHEN 3 THEN 'Retained sent message'
                   END AS [MesasageStatus]
   , CASE WHEN [validation] = 'X' 
          THEN CAST([message_body] AS XML)
          ELSE CAST(N'<none />' AS XML)
          END AS [MessageBody]
  FROM [dbo].[SQLClueDDLEventsQueue]
  ORDER BY [queuing_order];  

 END

GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[pSQLClueGetEvent]') AND type in (N'P', N'PC'))
 DROP PROCEDURE [dbo].[pSQLClueGetEvent];

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Bill Wunder
-- Create date: April 1, 2006
-- Description:	get the XML event payload for   
-- a specfied item on the Event Queue
-- =============================================
CREATE PROCEDURE [dbo].[pSQLClueGetEvent]
 (  @QueuingOrder [INT]
  , @MessageBody AS [XML] OUTPUT)
AS
 BEGIN

  SELECT @MessageBody = CASE WHEN [validation] = 'X' 
                             THEN CAST([message_body] AS [XML])
                             ELSE CAST(N'<none />' AS [XML])
                             END
  FROM [dbo].[SQLClueDDLEventsQueue]
  WHERE [queuing_order] = @QueuingOrder;
  
 END

GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[pSQLClueCheckQueueStatus]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[pSQLClueCheckQueueStatus];

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Bill Wunder
-- Create date: April 1, 2006
-- Description:	Get status of target   
-- notification queue
-- =============================================
CREATE PROCEDURE [dbo].[pSQLClueCheckQueueStatus]
AS
 BEGIN

  SELECT [name] AS [Name]
   , CASE [is_receive_enabled] WHEN 1 THEN  'Enabled' ELSE 'Disabled' END AS [ReceiveState] 
   , CASE [is_enqueue_enabled] WHEN 1 THEN  'Enabled' ELSE 'Disabled' END AS [EnqueueState]
   , CASE [is_activation_enabled] WHEN 1 THEN  'Enabled' ELSE 'Disabled' END AS [ActivationState]
   , ISNULL(NULLIF([activation_procedure],''),'none') AS [ActivationProcedure]
  FROM [sys].[service_queues]
  WHERE [name] IN('SQLClueDDLEventsQueue', 'SQLClueMonitorQueue');
 
 END

GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[pSQLClueResetQueueStatus]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[pSQLClueResetQueueStatus];

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Bill Wunder
-- Create date: April 1, 2006
-- Description:	Reset status of target   
-- notification queue
-- =============================================
CREATE PROCEDURE [dbo].[pSQLClueResetQueueStatus]
AS
 BEGIN

  IF EXISTS (SELECT *
            FROM [sys].[service_queues]
            WHERE [name] = 'SQLClueDDLEventsQueue'
            AND (   [is_receive_enabled] = 0
                 OR [is_enqueue_enabled] = 0))
    ALTER QUEUE SQLClueDDLEventsQueue WITH STATUS = ON;

  IF EXISTS (SELECT *
            FROM [sys].[service_queues]
            WHERE [name] = 'SQLClueMonitorQueue'
            AND (   [is_receive_enabled] = 0
                 OR [is_enqueue_enabled] = 0))
    ALTER QUEUE SQLClueMonitorQueue WITH STATUS = ON;

  IF EXISTS (SELECT *
            FROM [sys].[service_queues]
            WHERE [name] = 'SQLClueDDLEventsQueue'
            AND [is_activation_enabled] = 1)
   ALTER QUEUE SQLClueMonitorQueue WITH ACTIVATION(STATUS = OFF);

  IF EXISTS (SELECT *
            FROM [sys].[service_queues]
            WHERE [name] = 'SQLClueMonitorQueue'
            AND [is_activation_enabled] = 0)
   ALTER QUEUE SQLClueMonitorQueue WITH ACTIVATION(STATUS = ON);
  
 END

GO

IF NOT EXISTS (SELECT * FROM [msdb].[dbo].[sysalerts]
               WHERE [name] =N'SQLClue: Event Reader Error')
 EXEC [msdb].[dbo].[sp_add_alert] @name=N'SQLClue: Event Reader Error'
		                              , @message_id=0
		                              , @severity=16
		                              , @enabled=1
		                              , @delay_between_responses=7200
		                              , @include_event_description_in=0 
		                              , @event_description_keyword=N'(SQLClueGetDDLEvent)';

GO

IF NOT EXISTS (SELECT * FROM [msdb].[dbo].[sysalerts]
               WHERE [name] =N'SQLClue: Queue Disabled')
 EXEC [msdb].[dbo].[sp_add_alert] @name=N'SQLClue: Queue Disabled'
     		                         , @message_id=0
		                              , @severity=16
		                              , @enabled=1
 	                              , @delay_between_responses=7200
		                              , @include_event_description_in=0
		                              , @event_description_keyword=N'(SQLClueNotifyQueueDisabled)';
GO

DECLARE @txt as NVARCHAR(1024);
SET @txt = CHAR(13) + CHAR(10) + 'SQLClue DDL Event Notifications are now enabled on SQL Server [' + @@SERVERNAME + '],' + CHAR(13) + CHAR(10)
         + 'however the queue monitoring alert does not have a valid operator assignment. Configure' + CHAR(13) + CHAR(10)
         + 'a "DBA" operator or add an alternate "queue down" monitor if the SQL Agent service or' + CHAR(13) + CHAR(10)
         + 'Database Mail are unavailable.'  + CHAR(13) + CHAR(10) + CHAR(13) + CHAR(10) 
         + 'Only SQLClue''s ability to alert if a notification queue problem occurs is affected. Event ' + CHAR(13) + CHAR(10)
         + 'Notifications will fire without resolving this conflict and the queue can be monitored' + CHAR(13) + CHAR(10)
         + 'manually or in a manner consistent with other organizational Service Broker monitors.' + CHAR(13) + CHAR(10) + CHAR(13) + CHAR(10)
         + 'See notes in script [instDDLEventNotifications.sql] for additional details.';

IF NOT EXISTS (SELECT * FROM [msdb].[dbo].[sysoperators]
               WHERE [name] = 'DBA'
               AND [email_address] IS NOT NULL
               AND [enabled] = 1)
  RAISERROR (@txt,1,1);
ELSE
 BEGIN
  IF NOT EXISTS (SELECT * 
                 FROM [dbo].[sysnotifications] 
                 WHERE [alert_id] = (SELECT [Id] 
                                     FROM [dbo].[sysalerts]  
                                     WHERE [name] = N'SQLClue: Event Reader Error'))
    EXEC [msdb].[dbo].[sp_add_notification] @alert_name=N'SQLClue: Event Reader Error', @operator_name=N'DBA', @notification_method = 1;

  IF NOT EXISTS (SELECT * 
                 FROM [dbo].[sysnotifications] 
                 WHERE [alert_id] = (SELECT [Id] 
                                     FROM [dbo].[sysalerts]  
                                     WHERE [name] = N'SQLClue: Queue Disabled'))
   EXEC [msdb].[dbo].[sp_add_notification] @alert_name=N'SQLClue: Queue Disabled', @operator_name=N'DBA', @notification_method = 1;
 END

GO

