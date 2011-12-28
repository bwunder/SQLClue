/* 
 SQL Configuration Archive db changes for v1.3.5.5
*/  

USE SQLConfiguration;


IF EXISTS (SELECT * 
           FROM [sys].[objects]
           WHERE [name] = 'pkn_tSchedule__Name'
           AND [parent_object_id] = OBJECT_ID('SQLCfg.tConnection'))           
 EXEC sp_rename @objname = 'SQLCfg.tConnection.pkn_tSchedule__Name'  
  , @newname = 'pkn_tConnection__Name' 

GO

/******************************************************************************
**    Auth: Bill Wunder
**    Date: April 1, 2009
**
**    Desc: Fetch the Schedules for a baseline trace plan
**
*******************************************************************************
**    Change History
*******************************************************************************
**    Date           Author             Description of Change
**              
*******************************************************************************/

CREATE PROCEDURE [SQLCfg].[pScheduleSelectByBaselinePlanId] 
 ( @BaselinePlanId [INT])
AS
BEGIN

 SET NOCOUNT ON;

 SELECT [Id] 
  , [InstanceName]
  , [BaselinePlanId]
  , [Interval]
  , [IntervalType]
  , [IntervalBaseDt]
  , [UseEventNotifications]
  , [IsActive]
 FROM [SQLCfg].[tSchedule] 
 WHERE [BaselinePlanId] = @BaselinePlanId;

END

GO

GRANT EXECUTE ON [SQLCfg].[pScheduleSelectByBaselinePlanId] TO [SQLCfgReportingRole] AS [dbo]

GO

