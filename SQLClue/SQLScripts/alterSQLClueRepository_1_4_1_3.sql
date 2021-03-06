USE [SQLClue]
GO
IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLCfg'
           AND SPECIFIC_NAME = N'pChangeSelectLastestBySQLInstance') 
DROP PROCEDURE [SQLCfg].[pChangeSelectLastestBySQLInstance]
GO
/******************************************************************************
**    Auth: Bill Wunder
**    Date: April 1, 2007
**
**    Desc: Get a list of the most recent nodes on a server  
**
*******************************************************************************
**    Change History
*******************************************************************************
**    Date           Author             Description of Change
**    Jan 16, 2010   bw                 sorting done in application       
*******************************************************************************/
ALTER PROCEDURE [SQLCfg].[pChangeGetLatestHierarchyForInstance] 
 ( @SQLInstance [NVARCHAR](128) )
AS
BEGIN

    SELECT c.[Node].ToString() AS [TreeViewNodePath]
     , c.[Id] AS [ChangeId]  
     , latest.[Version]  
    FROM [SQLCfg].[tChange] c
    JOIN (SELECT MAX([Version]) as [Version], [Node]
         FROM [SQLCfg].[tChange]
         WHERE [Node].[SQLInstance] = @SQLInstance 
        AND [Node].[Type] = 'SQLInstance'
        GROUP BY [Node]) AS latest
    ON c.[Node] = latest.[Node]
    AND c.Version = latest.Version
    WHERE c.[Node].[SQLInstance] = @SQLInstance 
    AND c.[Node].[Type] = 'SQLInstance'

END
GO

IF EXISTS (SELECT * 
       FROM INFORMATION_SCHEMA.ROUTINES 
       WHERE SPECIFIC_SCHEMA = N'SQLCfg'
       AND SPECIFIC_NAME = N'pChangePurgeInstance' )
 DROP PROCEDURE [SQLCfg].[pChangePurgeInstance];

GO
/******************************************************************************
**    Auth: Bill Wunder
**    Date: Jan 18, 2010
**
**    Desc: completey remove a targeted SQL Server from the archive 
**          ArchiveLog rows are not removed
**
*******************************************************************************
**    Change History
*******************************************************************************
**    Date           Author             Description of Change
** 
*******************************************************************************/
CREATE PROCEDURE [SQLCfg].[pChangePurgeInstance] 
 ( @InstanceName [NVARCHAR] (128))
WITH EXECUTE AS OWNER -- needed to disable triggers 
AS
BEGIN
 DECLARE @TextData [NVARCHAR] (MAX);

 SET XACT_ABORT ON;
 SET NOCOUNT ON;

 BEGIN TRY

   BEGIN TRAN
    
    DELETE [SQLCfg].[tChangeLabel]
    WHERE [ChangeId] IN (SELECT [ChangeId]
                         FROM [SQLCfg].[tChange] 
                         WHERE [Node].[SQLInstance] = @InstanceName) 
       
    -- do not log anything, it would be deleted else inaccessible anyway
    ALTER TABLE [SQLCfg].[tServiceBroker]
    DISABLE TRIGGER ALL

     DELETE [SQLCfg].[tServiceBroker] 
     WHERE [InstanceName] = @InstanceName
    
    ALTER TABLE [SQLCfg].[tServiceBroker]
    ENABLE TRIGGER ALL

    ALTER TABLE [SQLCfg].[tDb]
    DISABLE TRIGGER ALL

     DELETE [SQLCfg].[tDb] 
     WHERE [InstanceName] = @InstanceName

    ALTER TABLE [SQLCfg].[tDb]
    ENABLE TRIGGER ALL

    ALTER TABLE [SQLCfg].[tJobServer]
    DISABLE TRIGGER ALL
 
     DELETE [SQLCfg].[tJobServer] 
     WHERE [InstanceName] = @InstanceName

    ALTER TABLE [SQLCfg].[tJobServer]
    ENABLE TRIGGER ALL

    ALTER TABLE [SQLCfg].[tInstance]
    DISABLE TRIGGER ALL
 
     DELETE [SQLCfg].[tInstance] 
     WHERE [Name] = @InstanceName

    ALTER TABLE [SQLCfg].[tInstance]
    ENABLE TRIGGER ALL

    ALTER TABLE [SQLCfg].[tChange]
    DISABLE TRIGGER ALL

     DELETE [SQLCfg].[tChange] 
     WHERE [Node].[SQLInstance] = @InstanceName
         
    ALTER TABLE [SQLCfg].[tChange]
    ENABLE TRIGGER ALL

    IF EXISTS (SELECT * 
               FROM [SQLCfg].[tSchedule]
               WHERE [InstanceName] = @InstanceName
               AND [UseEventNotifications] = 1)
     RAISERROR ('Important! Run script [uninstSQLClueDDLEventNotifications.sql] from the "SQLClue Application Resources" 
script library in the SQLClue Notification db (msdb is the default) on SQL Server %s to remove Event 
Notification configuration. ', 1, 1, @InstanceName) 

    ALTER TABLE [SQLCfg].[tSchedule]
    DISABLE TRIGGER ALL

     DELETE [SQLCfg].[tSchedule]
     WHERE [InstanceName] = @InstanceName 

    ALTER TABLE [SQLCfg].[tSchedule]
    ENABLE TRIGGER ALL

    ALTER TABLE [SQLCfg].[tConnection]
    DISABLE TRIGGER ALL
     
     DELETE [SQLCfg].[tConnection]
     WHERE [InstanceName] = @InstanceName
     
    ALTER TABLE [SQLCfg].[tConnection]
    ENABLE TRIGGER ALL

    -- decriment the licensed instance count
    UPDATE [SQLCfg].[tSQLCfg]
    SET [LicensedInstanceCount] = [LicensedInstanceCount] - 1 

   COMMIT TRAN;

  END TRY
   
  BEGIN CATCH

   SET @TextData = '  @InstanceName = ' + ISNULL(CHAR(39) + CAST(@InstanceName AS VARCHAR(128)) + CHAR(39),'NULL');
   EXEC [SQLCfg].[pLogSQLError] @TextData, @@PROCID;

 END CATCH;

END
GO

GRANT EXECUTE ON [SQLCfg].[pChangePurgeInstance] TO [SQLCfgAdminRole];

GO

/******************************************************************************
**    Auth: Bill Wunder
**    Date: April 1, 2007
**
*******************************************************************************
**    Change History
*******************************************************************************
**    Date           Author      Description of Change
**    11/10/2009     bw          remove QueryBaseline references 
**    Jan 21, 2010   bw          bugfix- stop writing reschueduled to tChange          
**              
*******************************************************************************/

ALTER TRIGGER [SQLCfg].[trgtSchedule_Insert_Update_Delete]
ON [SQLCfg].[tSchedule]
FOR INSERT, UPDATE, Delete
AS
BEGIN
 DECLARE @icount [INT]
  , @dcount [INT] 
  , @NodeBase [NVARCHAR] (50) 
  , @TextData [VARCHAR] (2047)
  , @InstanceName [NVARCHAR] (128);

 SET NOCOUNT ON;

 BEGIN TRY
  SELECT @icount = COUNT(*) FROM [inserted];
  SELECT @dcount = COUNT(*) FROM [deleted];

  SET @NodeBase = 'SQLCfgMetadata|SQLCfg.tSchedule|';

  -- no multi-row
  IF @icount > 1 OR @dcount > 1
   BEGIN
    SET @TextData = 'Multi-row [SQLCfg].[tSchedule] operations are not permitted';
    RAISERROR (@TextData,16,1);
   END

  -- inserts
  IF @icount = 1 AND @dcount = 0
   BEGIN
  
    UPDATE s
    SET [InstanceName] = s.[InstanceName] 
    FROM [SQLCfg].[tSchedule] s
    JOIN [inserted] i 
    ON s.[InstanceName] = i.[InstanceName];

    INSERT [SQLCfg].[tChange]
     ( [Node]
     , [Version]
     , [Action] 
     , [Definition]
     , [DefinitionDt] )
    SELECT
       @NodeBase + [InstanceName] + '|ScheduleId=' + CAST(Id as [VARCHAR] (10))
     , [SQLCfg].[fLastVersion](@NodeBase + [InstanceName] + '|ScheduleId=' + CAST(Id as [VARCHAR] (10))) + 1
     , 'Include'
     , 'INSERT [SQLCfg].[tSchedule]
 ( [InstanceName]
 , [Interval]
 , [IntervalType]
 , [IntervalBaseDt]
 , [UseEventNotifications]
 , [IsActive]
 , [RecCreatedDt] 
 , [RecCreatedUser]
 , [RecUpdatedDt]
 , [RecUpdatedUser] )
SELECT [InstanceName] = ''' + [InstanceName] + '''
AND [Interval] = ' + CAST([Interval] AS [VARCHAR] (10)) + ' 
AND [IntervalType] = ''' + [IntervalType] + ''' 
AND [IntervalBaseDt] = ''' + CAST([IntervalBaseDt] AS [VARCHAR] (20)) + ''' 
AND [UseEventNotifications] = ' + CAST([UseEventNotifications] AS [CHAR] (1)) + ' 
AND [IsActive] = ' + CAST([IsActive] AS [CHAR] (1)) + ' 
AND [RecCreatedDt] = ''' + CAST([RecCreatedDt] AS [VARCHAR] (20)) + '''
AND [RecCreatedUser] = ''' + [RecCreatedUser] + '''
AND [RecUpdatedDt] = ''' + CAST([RecUpdatedDt] AS [VARCHAR] (20)) + '''
AND [RecUpdatedUser] = ''' + [RecUpdatedUser] + ''''
     , CURRENT_TIMESTAMP
    FROM [inserted] 
    WHERE [InstanceName] NOT IN (SELECT [InstanceName] FROM [deleted]);
   END

  -- updates
  IF @icount = 1 AND @dcount = 1
   BEGIN    

    UPDATE s
    SET [RecUpdatedDt] = CURRENT_TIMESTAMP
     , [RecUpdatedUser] = ORIGINAL_LOGIN()
     , [InstanceName] = s.[InstanceName]
    FROM [SQLCfg].[tSchedule] s
    JOIN [inserted] i 
    ON s.[Id] = i.[Id]
    JOIN [deleted] d
    ON i.[Id] = d.[Id];
    
    -- if only IntervalBaseDate changes do not write a change record
    If EXISTs ( SELECT * 
                FROM inserted i
                JOIN deleted d
                ON i.[Id] = d.[Id]
                WHERE i.[InstanceName] <> d.[InstanceName]
                OR i.[Interval] <> d.[Interval]
                OR i.[IntervalType] <> d.[IntervalType]
                OR i.[UseEventNotifications] <> d.[UseEventNotifications]
                OR i.[IsActive] <> d.[IsActive])
     BEGIN
      INSERT [SQLCfg].[tChange]
       ( [Node]
       , [Version]
       , [Action] 
       , [Definition]
       , [DefinitionDt] )
      SELECT
         @NodeBase + i.[InstanceName] + '|ScheduleId=' + CAST(i.Id as [VARCHAR] (10))
       , [SQLCfg].[fLastVersion](@NodeBase + i.[InstanceName] + '|ScheduleId=' + CAST(i.Id as [VARCHAR] (10))) + 1
       , 'Modify'
       , 'UPDATE [SQLCfg].[tSchedule]
SET [InstanceName] = ''' + i.[InstanceName] + '''
 , [Interval] = ' + CAST(i.[Interval] AS [VARCHAR] (10)) + ' 
 , [IntervalType] = ''' + i.[IntervalType] + ''' 
 , [IntervalBaseDt] = ''' + CAST(i.[IntervalBaseDt] AS [VARCHAR] (20)) + ''' 
 , [UseEventNotifications] = ' + CAST(i.[UseEventNotifications] AS [CHAR] (1)) + ' 
 , [IsActive] = ' + CAST(i.[IsActive] AS [CHAR] (1)) + ' 
 , [RecCreatedDt] = ''' + CAST(i.[RecCreatedDt] AS [VARCHAR] (20)) + '''
 , [RecCreatedUser] = ''' + i.[RecCreatedUser] + '''
 , [RecUpdatedDt] = ''' + CAST(i.[RecUpdatedDt] AS [VARCHAR] (20)) + '''
 , [RecUpdatedUser] = ''' + i.[RecUpdatedUser] + '''
WHERE [InstanceName] = ''' + d.[InstanceName] + '''
AND [Interval] = ' + CAST(d.[Interval] AS [VARCHAR] (10)) + ' 
AND [IntervalType] = ''' + d.[IntervalType] + ''' 
AND [IntervalBaseDt] = ''' + CAST(d.[IntervalBaseDt] AS [VARCHAR] (20)) + ''' 
AND [UseEventNotifications] = ' + CAST(d.[UseEventNotifications] AS [CHAR] (1)) + ' 
AND [IsActive] = ' + CAST(d.[IsActive] AS [CHAR] (1)) + ' 
AND [RecCreatedDt] = ''' + CAST(d.[RecCreatedDt] AS [VARCHAR] (20)) + '''
AND [RecCreatedUser] = ''' + d.[RecCreatedUser] + '''
AND [RecUpdatedDt] = ''' + CAST(d.[RecUpdatedDt] AS [VARCHAR] (20)) + '''
AND [RecUpdatedUser] = ''' + d.[RecUpdatedUser] + ''''
       , CURRENT_TIMESTAMP
      FROM [inserted] i 
      JOIN [deleted] d
      ON i.[InstanceName] = d.[InstanceName];
     END
   END

  -- deletes
  IF @icount = 0 AND @dcount = 1
   BEGIN

    -- active schedule with notifications enabled means there is still
    -- notification metadata deployed to the instance
    SELECT @InstanceName = [InstanceName] FROM [deleted]
    WHERE [UseEventNotifications] = 1
    AND [IsActive] = 1;

    IF @@ROWCOUNT > 0
      RAISERROR ('Remove event notifications configuration on Instance [%s] before deleting the Schedule',16,1, @InstanceName);
  
    INSERT [SQLCfg].[tChange]
     ( [Node]
     , [Version]
     , [Action] 
     , [Definition]
     , [DefinitionDt] )
    SELECT
       @NodeBase + [InstanceName] + '|ScheduleId=' + CAST(Id as [VARCHAR] (10))
     , SQLCfg.[fLastVersion](@NodeBase + [InstanceName] + '|ScheduleId=' + CAST(Id as [VARCHAR] (10))) + 1
     , 'Remove'
     , 'DELETE [SQLCfg].[tSchedule]
WHERE [InstanceName] = ''' + [InstanceName] + '''
AND [Interval] = ' + CAST([Interval] AS [VARCHAR] (10)) + ' 
AND [IntervalType] = ''' + [IntervalType] + ''' 
AND [IntervalBaseDt] = ''' + CAST([IntervalBaseDt] AS [VARCHAR] (20)) + ''' 
AND [UseEventNotifications] = ' + CAST([UseEventNotifications] AS [CHAR] (1)) + ' 
AND [IsActive] = ' + CAST([IsActive] AS [CHAR] (1)) + ' 
AND [RecCreatedDt] = ''' + CAST([RecCreatedDt] AS [VARCHAR] (20)) + '''
AND [RecCreatedUser] = ''' + [RecCreatedUser] + '''
AND [RecUpdatedDt] = ''' + CAST([RecUpdatedDt] AS [VARCHAR] (20)) + '''
AND [RecUpdatedUser] = ''' + [RecUpdatedUser] + ''''
     , CURRENT_TIMESTAMP
    FROM [deleted] 
    WHERE [InstanceName] NOT IN (SELECT [InstanceName] FROM [inserted]);
   
  END

 END TRY

 BEGIN CATCH

  EXEC [SQLCfg].[pLogSQLError] @TextData, @@PROCID;

 END CATCH 

END;

GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLCfg'
           AND SPECIFIC_NAME = N'pChangeSelectScheduleListForInstance' )
 DROP PROCEDURE [SQLCfg].[pChangeSelectScheduleListForInstance];

GO

/******************************************************************************
**    Auth: Bill Wunder
**    Date: January 21, 2010
**
**    Desc: Node path for all schedules that belong to a SQL Server instance    
**       
*******************************************************************************
**    Change History
*******************************************************************************
**    Date           Author             Description of Change
**              
*******************************************************************************/
CREATE PROCEDURE [SQLCfg].[pChangeSelectScheduleListForInstance] 
 ( @SQLInstance [NVARCHAR] (128) )
AS
BEGIN

    SELECT Distinct [Node].[Path] AS [CfgCollection]
    FROM SQLCfg.tChange 
    WHERE [Node].[Type]  = 'metadata' 
    AND [Node].[SubType] = 'Schedule'
    AND [Node].[Item] = @SQLInstance
    ORDER BY [CfgCollection]

END
GO

GRANT EXECUTE ON [SQLCfg].[pChangeSelectScheduleListForInstance] TO [SQLCfgReportingRole];

GO

