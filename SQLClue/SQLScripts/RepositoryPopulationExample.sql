/*

 Before running the script, the default values of the Include bit columns can be 
 modified in SQLCfg.pInstanceInit and SQLCfg.pDB if desired.  
  
 Any table that provides the required server properties - Name, EngineEdition 
 and VersionMajor - can be used in place of [dbo].[ServerList]. [dbo].[ServerList] 
 is used here to illustrate a script based initialization for SQLClue SQL Configuration.
 
 Each IntervalBaseDt can have a unique value or they may be grouped as desired. 
 By default, all are set to start the archive at midnight. Archive Schedules with the 
 same IntervalBaseDt are executed serially by the Automation Controller.

-- clean-up
DROP TABLE [dbo].[ServerList] 
DELETE [SQLCfg].[tServiceBroker] 
DELETE [SQLCfg].[tDb]
DELETE [SQLCfg].[tJobServer]
DELETE [SQLCfg].[tInstance]
DELETE [SQLCfg].[tSchedule]
DELETE [SQLCfg].[tConnection]

-- verify
SELECT * FROM [SQLCfg].[tServiceBroker] 
SELECT * FROM [SQLCfg].[tDb]
SELECT * FROM [SQLCfg].[tJobServer]
SELECT * FROM [SQLCfg].[tInstance]
SELECT * FROM [SQLCfg].[tConnection]
SELECT * FROM [SQLCfg].[tSchedule]

select * from SQLCfg.tSQLErrorLog
*/

CREATE TABLE [dbo].[ServerList] 
( [Id] [INT] IDENTITY(1,1) primary key
, [Name] [NVARCHAR](128)  
, [EngineEdition] [INT]   -- 2 (Standard and Workgroup),3 (Enterprise, Enterprise Evaluation, and Developer),4 (Express)
, [VersionMajor] [INT] )  -- 8,9 or 10
GO

-- this populates a row for the SQLClue host - always included the host instance
-- add any other instances using your script - the EngineEdition and VersionMajor must always be provided
INSERT [dbo].[ServerList] ([Name], [EngineEdition], [VersionMajor]) 
SELECT InstanceName = @@SERVERNAME
 , EngineEdition = CONVERT([INT], SERVERPROPERTY('EngineEdition'))
 , VersionMajor = PARSENAME(CONVERT([NVARCHAR](128), SERVERPROPERTY('ProductVersion')),4)
GO

DECLARE @Id [INT]
 , @EngineEdition [INT]
 , @VersionMajor [INT]
 , @InstanceName [NVARCHAR](128)
 , @IntravalBaseDt [DATETIME]
 , @ScheduleId [INT];
 
SET @IntravalBaseDt = CONVERT([DATETIME], CONVERT([DATE], CURRENT_TIMESTAMP));

SELECT @Id = MIN([Id]) 
FROM [dbo].[ServerList];

WHILE @Id IS NOT NULL
 BEGIN

  SELECT @EngineEdition = [EngineEdition]
   , @VersionMajor = [VersionMajor]
   , @InstanceName = [Name]
  FROM [dbo].[ServerList]
  WHERE [Id] = @Id;
  
  EXEC [SQLCfg].[pConnectionInsert] 
     @InstanceName = @InstanceName
   , @EncryptedConnection = 0
   , @TrustServerCertificate = 0
   , @NetworkProtocol = ''
   , @ConnectionTimeout = 60
   , @LoginSecure = 1;

   EXEC [SQLCfg].[pInstanceInit] 
     @InstanceName = @InstanceName
   , @VersionMajor = @VersionMajor
   , @EngineEdition = @EngineEdition;

   EXEC [SQLCfg].[pScheduleInsert]
      @InstanceName = @InstanceName
    , @BaselinePlanId = 0
    , @Interval = 1
    , @IntervalType = 'day'
    , @IntervalBaseDt = @IntravalBaseDt  
    , @UseEventNotifications = 0 
    , @IsActive = 1
    , @Id = @ScheduleId;

  SELECT @Id = MIN([Id]) 
  FROM [dbo].[ServerList]
  WHERE [Id] > @Id;

 END

