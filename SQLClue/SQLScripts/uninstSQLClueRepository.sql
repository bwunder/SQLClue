/******************************************************************************
Notes:
Removes configuration created by instSQLClueRepository.sql script

Do not run this script using SSMS to avoid corrupting the SQLClue install. Use 
the 'Tools|SQL Configuration|Uninstall' menu option only. It will run this 
script and take care of all other necessary SQLClue configuration changes. 
*******************************************************************************
** Change History
*******************************************************************************
** Date           Author             Description of Change
** May 5, 2009    bw                 add this change history block  
**              
**              
**              
**              
*******************************************************************************/

IF  EXISTS (SELECT * FROM [sys].[foreign_keys] 
            WHERE [object_id] = OBJECT_ID(N'[fk_tChangeLabel__ChangeId__to__tChange]') 
            AND [parent_object_id] = OBJECT_ID(N'[SQLCfg].[tChangeLabel]'))
 ALTER TABLE [SQLCfg].[tChangeLabel] DROP CONSTRAINT [fk_tChangeLabel__ChangeId__to__tChange]
GO

IF  EXISTS (SELECT * FROM [sys].[foreign_keys] 
            WHERE [object_id] = OBJECT_ID(N'[SQLCfg].[fk_tChangeLabel__LabelId__to__tLabel]') 
            AND [parent_object_id] = OBJECT_ID(N'[SQLCfg].[tChangeLabel]'))
ALTER TABLE [SQLCfg].[tChangeLabel] DROP CONSTRAINT [fk_tChangeLabel__LabelId__to__tLabel]
GO

IF  EXISTS (SELECT * FROM [sys].[foreign_keys] 
            WHERE [object_id] = OBJECT_ID(N'[SQLCfg].[fk_tDb__Instance__to__tInstance]') 
            AND parent_object_id = OBJECT_ID(N'[SQLCfg].[tDb]'))
ALTER TABLE [SQLCfg].[tDb] DROP CONSTRAINT [fk_tDb__Instance__to__tInstance]
GO

IF  EXISTS (SELECT * FROM [sys].[foreign_keys] 
            WHERE [object_id] = OBJECT_ID(N'[SQLCfg].[fk_tInstance__Name__to__tConnection__InstanceName]') 
            AND [parent_object_id] = OBJECT_ID(N'[SQLCfg].[tInstance]'))
ALTER TABLE [SQLCfg].[tInstance] DROP CONSTRAINT [fk_tInstance__Name__to__tConnection__InstanceName]
GO

IF  EXISTS (SELECT * FROM [sys].[foreign_keys] 
            WHERE [object_id] = OBJECT_ID(N'[SQLCfg].[fk_tJobServer__to__tInstance]') 
            AND [parent_object_id] = OBJECT_ID(N'[SQLCfg].[tJobServer]'))
ALTER TABLE [SQLCfg].[tJobServer] DROP CONSTRAINT [fk_tJobServer__to__tInstance]
GO

IF  EXISTS (SELECT * FROM [sys].[foreign_keys] 
            WHERE [object_id] = OBJECT_ID(N'[SQLCfg].[fk_tServiceBroker__to__tDB__InstanceName__Name]') 
            AND [parent_object_id] = OBJECT_ID(N'[SQLCfg].[tServiceBroker]'))
ALTER TABLE [SQLCfg].[tServiceBroker] DROP CONSTRAINT [fk_tServiceBroker__to__tDB__InstanceName__Name]
GO

IF  EXISTS (SELECT * FROM [sys].[objects] 
            WHERE [object_id] = OBJECT_ID(N'[SQLCfg].[fLastVersion]') 
            AND [type] IN (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [SQLCfg].[fLastVersion]
GO

IF  EXISTS (SELECT * FROM [sys].[objects] 
            WHERE [object_id] = OBJECT_ID(N'[SQLCfg].[GetCollection]') 
            AND [type] in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [SQLCfg].[GetCollection]

GO

IF  EXISTS (SELECT * FROM [sys].[objects] 
            WHERE [object_id] = OBJECT_ID(N'[SQLCfg].[GetDatabase]') 
            AND [type] in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [SQLCfg].[GetDatabase]

GO

IF  EXISTS (SELECT * FROM [sys].[objects] 
            WHERE [object_id] = OBJECT_ID(N'[SQLCfg].[GetInstance]') 
            AND [type] in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [SQLCfg].[GetInstance]

GO

IF  EXISTS (SELECT * FROM [sys].[objects] 
            WHERE [object_id] = OBJECT_ID(N'[SQLCfg].[GetItem]') 
            AND [type] in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [SQLCfg].[GetItem]

GO

IF  EXISTS (SELECT * FROM [sys].[objects] 
            WHERE [object_id] = OBJECT_ID(N'[SQLCfg].[GetLength]') 
            AND [type] in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [SQLCfg].[GetLength]

GO

IF  EXISTS (SELECT * FROM [sys].[objects] 
            WHERE [object_id] = OBJECT_ID(N'[SQLCfg].[GetPath]') 
            AND [type] in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [SQLCfg].[GetPath]

GO

IF  EXISTS (SELECT * FROM [sys].[objects] 
            WHERE [object_id] = OBJECT_ID(N'[SQLCfg].[GetSubType]') 
            AND [type] in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [SQLCfg].[GetSubType]

GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLCfg'
           AND SPECIFIC_NAME = N'pSQLCfgUpdate') 
DROP PROCEDURE [SQLCfg].[pSQLCfgUpdate]
GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLCfg'
           AND SPECIFIC_NAME = N'pSQLCfgSelect') 
DROP PROCEDURE [SQLCfg].[pSQLCfgSelect]
GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLCfg'
           AND SPECIFIC_NAME = N'pSQLCfgGet') 
DROP PROCEDURE [SQLCfg].[pSQLCfgGet]
GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLCfg'
           AND SPECIFIC_NAME = N'pInstanceUpdate') 
DROP PROCEDURE [SQLCfg].[pInstanceUpdate]
GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLCfg'
           AND SPECIFIC_NAME = N'pInstanceGet') 
DROP PROCEDURE [SQLCfg].[pInstanceGet]
GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLCfg'
           AND SPECIFIC_NAME = N'pInstanceSelectNameList') 
DROP PROCEDURE [SQLCfg].[pInstanceSelectNameList]
GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLCfg'
           AND SPECIFIC_NAME = N'pInstanceDelete') 
DROP PROCEDURE [SQLCfg].[pInstanceDelete]
GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLCfg'
           AND SPECIFIC_NAME = N'pInstanceSelectAll') 
DROP PROCEDURE [SQLCfg].[pInstanceSelectAll]
GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLCfg'
           AND SPECIFIC_NAME = N'pJobServerUpdate') 
DROP PROCEDURE [SQLCfg].[pJobServerUpdate]
GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLCfg'
           AND SPECIFIC_NAME = N'pJobServerGet') 
DROP PROCEDURE [SQLCfg].[pJobServerGet]
GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLCfg'
           AND SPECIFIC_NAME = N'pJobServerDelete') 
DROP PROCEDURE [SQLCfg].[pJobServerDelete]
GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLCfg'
           AND SPECIFIC_NAME = N'pJobServerSelectAll') 
DROP PROCEDURE [SQLCfg].[pJobServerSelectAll]
GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLCfg'
           AND SPECIFIC_NAME = N'pDbUpdate') 
DROP PROCEDURE [SQLCfg].[pDbUpdate]
GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLCfg'
           AND SPECIFIC_NAME = N'pDbDelete') 
DROP PROCEDURE [SQLCfg].[pDbDelete]
GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLCfg'
           AND SPECIFIC_NAME = N'pDbGet') 
DROP PROCEDURE [SQLCfg].[pDbGet]
GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLCfg'
           AND SPECIFIC_NAME = N'pDbSelectAllForInstance') 
DROP PROCEDURE [SQLCfg].[pDbSelectAllForInstance]
GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLCfg'
           AND SPECIFIC_NAME = N'pConnectionUpdate') 
DROP PROCEDURE [SQLCfg].[pConnectionUpdate]
GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLCfg'
           AND SPECIFIC_NAME = N'pConnectionGetTargetConnectionString' )
 DROP PROCEDURE [SQLCfg].[pConnectionGetTargetConnectionString];

GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLCfg'
           AND SPECIFIC_NAME = N'pConnectionGet') 
DROP PROCEDURE [SQLCfg].[pConnectionGet]
GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLCfg'
           AND SPECIFIC_NAME = N'pConnectionDelete') 
DROP PROCEDURE [SQLCfg].[pConnectionDelete]
GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLCfg'
           AND SPECIFIC_NAME = N'pConnectionSelectAll') 
DROP PROCEDURE [SQLCfg].[pConnectionSelectAll]
GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLCfg'
           AND SPECIFIC_NAME = N'pConnectionUserGet') 
DROP PROCEDURE [SQLCfg].[pConnectionUserGet]
GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLCfg'
           AND SPECIFIC_NAME = N'pConnectionUserUpdate') 
DROP PROCEDURE [SQLCfg].[pConnectionUserUpdate]
GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLCfg'
           AND SPECIFIC_NAME = N'pDbSelectAll') 
DROP PROCEDURE [SQLCfg].[pDbSelectAll]
GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLCfg'
           AND SPECIFIC_NAME = N'pServiceBrokerUpdate') 
DROP PROCEDURE [SQLCfg].[pServiceBrokerUpdate]
GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLCfg'
           AND SPECIFIC_NAME = N'pServiceBrokerDelete') 
DROP PROCEDURE [SQLCfg].[pServiceBrokerDelete]
GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLCfg'
           AND SPECIFIC_NAME = N'pServiceBrokerGet') 
DROP PROCEDURE [SQLCfg].[pServiceBrokerGet]
GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLCfg'
           AND SPECIFIC_NAME = N'pServiceBrokerSelectAllForInstance') 
DROP PROCEDURE [SQLCfg].[pServiceBrokerSelectAllForInstance]
GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLCfg'
           AND SPECIFIC_NAME = N'pServiceBrokerSelectAll') 
DROP PROCEDURE [SQLCfg].[pServiceBrokerSelectAll]
GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLCfg'
           AND SPECIFIC_NAME = N'pScheduleInsert') 
DROP PROCEDURE [SQLCfg].[pScheduleInsert]
GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLCfg'
           AND SPECIFIC_NAME = N'pScheduleUpdate') 
DROP PROCEDURE [SQLCfg].[pScheduleUpdate]
GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLCfg'
           AND SPECIFIC_NAME = N'pScheduleGet') 
DROP PROCEDURE [SQLCfg].[pScheduleGet]
GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLCfg'
           AND SPECIFIC_NAME = N'pScheduleGetCountForInstance') 
DROP PROCEDURE [SQLCfg].[pScheduleGetCountForInstance];
GO
 
IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLCfg'
           AND SPECIFIC_NAME = N'pScheduleGetInstanceNameById') 
DROP PROCEDURE [SQLCfg].[pScheduleGetInstanceNameById]
GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLCfg'
           AND SPECIFIC_NAME = N'pScheduleDelete') 
DROP PROCEDURE [SQLCfg].[pScheduleDelete]
GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLCfg'
           AND SPECIFIC_NAME = N'pScheduleSelectAll') 
DROP PROCEDURE [SQLCfg].[pScheduleSelectAll]
GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLCfg'
           AND SPECIFIC_NAME = N'pScheduleSelectAllForInstance') 
DROP PROCEDURE [SQLCfg].[pScheduleSelectAllForInstance]
GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLCfg'
           AND SPECIFIC_NAME = N'pScheduleSelectAllWithLastStatus') 
DROP PROCEDURE [SQLCfg].[pScheduleSelectAllWithLastStatus]
GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLCfg'
           AND SPECIFIC_NAME = N'pScheduleSelectAllWithLastNbrItemsProcessed') 
DROP PROCEDURE [SQLCfg].[pScheduleSelectAllWithLastNbrItemsProcessed]
GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLCfg'
           AND SPECIFIC_NAME = N'pServiceSettingsSelect') 
DROP PROCEDURE [SQLCfg].[pServiceSettingsSelect]
GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLCfg'
           AND SPECIFIC_NAME = N'pChangeInsert') 
DROP PROCEDURE [SQLCfg].[pChangeInsert]
GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLCfg'
           AND SPECIFIC_NAME = N'pChangeAppendEvent') 
DROP PROCEDURE [SQLCfg].[pChangeAppendEvent]
GO

IF EXISTS (SELECT * 
       FROM INFORMATION_SCHEMA.ROUTINES 
       WHERE SPECIFIC_SCHEMA = N'SQLCfg'
       AND SPECIFIC_NAME = N'pChangePurgeInstance' )
DROP PROCEDURE [SQLCfg].[pChangePurgeInstance];
GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLCfg'
           AND SPECIFIC_NAME = N'pChangeSelectScheduleListForInstance' )
DROP PROCEDURE [SQLCfg].[pChangeSelectScheduleListForInstance];
GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLCfg'
           AND SPECIFIC_NAME = N'pChangeSelectByContains' )
DROP PROCEDURE [SQLCfg].[pChangeSelectByContains];
GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLCfg'
           AND SPECIFIC_NAME = N'pArchiveLogInsert') 
DROP PROCEDURE [SQLCfg].[pArchiveLogInsert]
GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLCfg'
           AND SPECIFIC_NAME = N'pArchiveLogSelectScheduleHistory') 
DROP PROCEDURE [SQLCfg].[pArchiveLogSelectScheduleHistory]
GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLCfg'
           AND SPECIFIC_NAME = N'pArchiveLogSelectScheduleHistoryByScheduleId') 
DROP PROCEDURE [SQLCfg].[pArchiveLogSelectScheduleHistoryByScheduleId]
GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLCfg'
           AND SPECIFIC_NAME = N'pArchiveLogSelectSQLInstanceHistoryByScheduleId') 
DROP PROCEDURE [SQLCfg].[pArchiveLogSelectSQLInstanceHistoryByScheduleId]
GO
IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLCfg'
           AND SPECIFIC_NAME = N'pChangeActionSummaryByNodeForDays') 
DROP PROCEDURE [SQLCfg].[pChangeActionSummaryByNodeForDays]
GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLCfg'
           AND SPECIFIC_NAME = N'pChangeActionByNodeForDays') 
DROP PROCEDURE [SQLCfg].[pChangeActionByNodeForDays]
GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLCfg'
           AND SPECIFIC_NAME = N'pChangeGetAllNodesForInstance') 
DROP PROCEDURE [SQLCfg].[pChangeGetAllNodesForInstance]
GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLCfg'
           AND SPECIFIC_NAME = N'pChangeHistoryByItem') 
DROP PROCEDURE [SQLCfg].[pChangeHistoryByItem]
GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLCfg'
           AND SPECIFIC_NAME = N'pChangeGetLastCountByNodeParent') 
DROP PROCEDURE [SQLCfg].[pChangeGetLastCountByNodeParent]
GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLCfg'
           AND SPECIFIC_NAME = N'pChangeSelectDefinitionByVersion') 
DROP PROCEDURE [SQLCfg].[pChangeSelectDefinitionByVersion]
GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLCfg'
           AND SPECIFIC_NAME = N'pChangeSelectScheduleListForInstance' )
 DROP PROCEDURE [SQLCfg].[pChangeSelectScheduleListForInstance];

GO
IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLCfg'
           AND SPECIFIC_NAME = N'pChangeLatestByItem') 
DROP PROCEDURE [SQLCfg].[pChangeLatestByItem]
GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLCfg'
           AND SPECIFIC_NAME = N'pChangeSelectLastestBySQLInstance') 
DROP PROCEDURE [SQLCfg].[pChangeSelectLastestBySQLInstance]
GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLCfg'
           AND SPECIFIC_NAME = N'pChangeSelectNodesForDateRange') 
DROP PROCEDURE [SQLCfg].[pChangeSelectNodesForDateRange]
GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLCfg'
           AND SPECIFIC_NAME = N'pChangesForArchive') 
DROP PROCEDURE [SQLCfg].[pChangesForArchive]
GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLCfg'
           AND SPECIFIC_NAME = N'pChangesForDate') 
DROP PROCEDURE [SQLCfg].[pChangesForDate]
GO


IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLCfg'
           AND SPECIFIC_NAME = N'pChangeGetLatestHierarchyForInstance') 
DROP PROCEDURE [SQLCfg].[pChangeGetLatestHierarchyForInstance]
GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLCfg'
           AND SPECIFIC_NAME = N'pChangeGetLatestItemsForNode') 
DROP PROCEDURE [SQLCfg].[pChangeGetLatestItemsForNode]
GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLCfg'
           AND SPECIFIC_NAME = N'pChangeSelectLatestByNodeParent') 
DROP PROCEDURE [SQLCfg].[pChangeSelectLatestByNodeParent]
GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLCfg'
           AND SPECIFIC_NAME = N'pConfigurationCatalog') 
DROP PROCEDURE [SQLCfg].[pConfigurationCatalog]
GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLCfg'
           AND SPECIFIC_NAME = N'pChangeSelectDatabaseListForInstance' )
 DROP PROCEDURE [SQLCfg].[pChangeSelectDatabaseListForInstance];
GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLCfg'
           AND SPECIFIC_NAME = N'pSQLErrorLogSelectMostRecent') 
DROP PROCEDURE [SQLCfg].[pSQLErrorLogSelectMostRecent]
GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLCfg'
           AND SPECIFIC_NAME = N'pApplicationErrorLogSelectMostRecent') 
DROP PROCEDURE [SQLCfg].[pApplicationErrorLogSelectMostRecent]
GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLCfg'
           AND SPECIFIC_NAME = N'pRepositoryInit') 
DROP PROCEDURE [SQLCfg].[pRepositoryInit]
GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLCfg'
           AND SPECIFIC_NAME = N'tArchiveLog') 
DROP TABLE [SQLCfg].[tArchiveLog]
GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLCfg'
           AND SPECIFIC_NAME = N'pInstanceInit') 
DROP PROCEDURE [SQLCfg].[pInstanceInit]
GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLCfg'
           AND SPECIFIC_NAME = N'pDbInit') 
DROP PROCEDURE [SQLCfg].[pDbInit]
GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLCfg'
           AND SPECIFIC_NAME = N'pChangeApplyLabel') 
DROP PROCEDURE [SQLCfg].[pChangeApplyLabel]
GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLCfg'
           AND SPECIFIC_NAME = N'pConnectionInsert') 
DROP PROCEDURE [SQLCfg].[pConnectionInsert]
GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLCfg'
           AND SPECIFIC_NAME = N'pSQLCfgInsert') 
DROP PROCEDURE [SQLCfg].[pSQLCfgInsert]
GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLCfg'
           AND SPECIFIC_NAME = N'pJobServerInsert') 
DROP PROCEDURE [SQLCfg].[pJobServerInsert]
GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLCfg'
           AND SPECIFIC_NAME = N'pInstanceInsert')  
DROP PROCEDURE [SQLCfg].[pInstanceInsert]
GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLCfg'
           AND SPECIFIC_NAME = N'pDbInsert')  
DROP PROCEDURE [SQLCfg].[pDbInsert]
GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLCfg'
           AND SPECIFIC_NAME = N'pServiceBrokerInsert')  
DROP PROCEDURE [SQLCfg].[pServiceBrokerInsert]
GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLCfg'
           AND SPECIFIC_NAME = N'pLogSQLError')  
DROP PROCEDURE [SQLCfg].[pLogSQLError]
GO

IF  EXISTS (SELECT * FROM [sys].[objects]  
            WHERE [object_id] = OBJECT_ID(N'[SQLCfg].[vArchive]') 
            AND [type] IN (N'V'))
DROP VIEW [SQLCfg].[vArchive]
GO

IF  EXISTS (SELECT * FROM [sys].[objects]  
            WHERE [object_id] = OBJECT_ID(N'[SQLCfg].[tChangeLabel]') 
            AND [type] IN (N'U'))
DROP TABLE [SQLCfg].[tChangeLabel]
GO

IF  EXISTS (SELECT * FROM [sys].[objects]  
            WHERE [object_id] = OBJECT_ID(N'[SQLCfg].[tLabel]') 
            AND [type] IN (N'U'))
DROP TABLE [SQLCfg].[tLabel]
GO

IF  EXISTS (SELECT * FROM [sys].[objects]  
            WHERE [object_id] = OBJECT_ID(N'[SQLCfg].[tChange]') 
            AND [type] IN (N'U'))
DROP TABLE [SQLCfg].[tChange]
GO

IF  EXISTS (SELECT * FROM [sys].[objects]  
            WHERE [object_id] = OBJECT_ID(N'[SQLCfg].[tServiceBroker]')  
            AND [type] IN (N'U'))
DROP TABLE [SQLCfg].[tServiceBroker]
GO

IF  EXISTS (SELECT * FROM [sys].[objects]  
            WHERE [object_id] = OBJECT_ID(N'[SQLCfg].[tDb]')  
            AND [type] IN (N'U'))
DROP TABLE [SQLCfg].[tDb]
GO

IF  EXISTS (SELECT * FROM [sys].[objects]  
            WHERE [object_id] = OBJECT_ID(N'[SQLCfg].[tJobServer]')  
            AND [type] IN (N'U'))
DROP TABLE [SQLCfg].[tJobServer]
GO

IF  EXISTS (SELECT * FROM [sys].[objects]  
            WHERE [object_id] = OBJECT_ID(N'[SQLCfg].[tInstance]')  
            AND [type] IN (N'U'))
DROP TABLE [SQLCfg].[tInstance]
GO

IF  EXISTS (SELECT * FROM [sys].[objects]  
            WHERE [object_id] = OBJECT_ID(N'[SQLCfg].[tSchedule]')  
            AND [type] IN (N'U'))
DROP TABLE [SQLCfg].[tSchedule]
GO

IF  EXISTS (SELECT * FROM [sys].[objects]  
            WHERE [object_id] = OBJECT_ID(N'[SQLCfg].[tConnection]') 
            AND [type] IN (N'U'))
DROP TABLE [SQLCfg].[tConnection]
GO

IF  EXISTS (SELECT * FROM [sys].[objects]  
            WHERE [object_id] = OBJECT_ID(N'[SQLCfg].[tSQLErrorLog]') 
            AND [type] IN (N'U'))
DROP TABLE [SQLCfg].[tSQLErrorLog]
GO

IF  EXISTS (SELECT * FROM [sys].[objects]  
            WHERE [object_id] = OBJECT_ID(N'[SQLCfg].[tServiceSettings]') 
            AND [type] IN (N'U'))
DROP TABLE [SQLCfg].[tServiceSettings]
GO

IF  EXISTS (SELECT * FROM [sys].[objects]  
            WHERE [object_id] = OBJECT_ID(N'[SQLCfg].[tArchiveLog]') 
            AND [type] IN (N'U'))
DROP TABLE [SQLCfg].[tArchiveLog]
GO

IF  EXISTS (SELECT * FROM [sys].[objects]  
            WHERE [object_id] = OBJECT_ID(N'[SQLCfg].[tSQLCfg]') 
            AND [type] IN (N'U'))
DROP TABLE [SQLCfg].[tSQLCfg]
GO

IF EXISTS (SELECT * FROM [sys].[fulltext_catalogs]
           WHERE [name] = 'ftSQLConfigurationRepositoryCatalog')
DROP FULLTEXT CATALOG [ftSQLConfigurationRepositoryCatalog]
GO

IF  EXISTS (SELECT * FROM [sys].[types]  
            WHERE [name] = 'SQLCfgNode'
            AND SCHEMA_NAME([schema_id]) = 'SQLCfg')
DROP TYPE [SQLCfg].[SQLCfgNode]
GO

IF  EXISTS (SELECT * FROM [sys].[assemblies] asms  
            WHERE asms.name = N'SQLClueCLR')
DROP ASSEMBLY [SQLClueCLR]
GO

DECLARE @member [NVARCHAR] (128)
IF IS_MEMBER('SQLCfgReportingRole') IS NOT NULL
 BEGIN
  -- remove all members
  SET @member = (SELECT TOP 1 USER_NAME([member_principal_id]) 
                 FROM [sys].[database_role_members] 
                 WHERE USER_NAME([role_principal_id]) = 'SQLCfgReportingRole')     
  WHILE @member IS NOT NULL
   BEGIN
    EXEC sp_droprolemember 'SQLCfgReportingRole', @member
    SET @member = (SELECT TOP 1 USER_NAME([member_principal_id]) 
                   FROM [sys].[database_role_members] 
                   WHERE USER_NAME([role_principal_id]) = 'SQLCfgReportingRole')     
   END
   DROP ROLE [SQLCfgReportingRole]
 END 

GO 

DECLARE @member [NVARCHAR] (128)
IF IS_MEMBER('SQLCfgServiceRole') IS NOT NULL
 BEGIN
  -- remove all members
  SET @member = (SELECT TOP 1 USER_NAME([member_principal_id]) 
                 FROM [sys].[database_role_members] 
                 WHERE USER_NAME([role_principal_id]) = 'SQLCfgServiceRole')     
  WHILE @member IS NOT NULL
   BEGIN
    EXEC sp_droprolemember 'SQLCfgServiceRole', @member
    SET @member = (SELECT TOP 1 USER_NAME([member_principal_id]) 
                   FROM [sys].[database_role_members] 
                   WHERE USER_NAME([role_principal_id]) = 'SQLCfgServiceRole')     
   END
   DROP ROLE [SQLCfgServiceRole]
 END 

GO 

DECLARE @member [NVARCHAR] (128)
IF IS_MEMBER('SQLCfgAdminRole') IS NOT NULL
 BEGIN
  -- remove all members
  SET @member = (SELECT TOP 1 USER_NAME([member_principal_id]) 
                 FROM [sys].[database_role_members] 
                 WHERE USER_NAME([role_principal_id]) = 'SQLCfgAdminRole')     
  WHILE @member IS NOT NULL
   BEGIN
    EXEC sp_droprolemember 'SQLCfgAdminRole', @member
    SET @member = (SELECT TOP 1 USER_NAME([member_principal_id]) 
                   FROM [sys].[database_role_members] 
                   WHERE USER_NAME([role_principal_id]) = 'SQLCfgAdminRole')     
   END
   DROP ROLE [SQLCfgAdminRole]
 END 

GO 

DECLARE @member [NVARCHAR] (128)
IF IS_MEMBER('SQLClueAdminRole') IS NOT NULL
 BEGIN
  -- remove all members
  SET @member = (SELECT TOP 1 USER_NAME([member_principal_id]) 
                 FROM [sys].[database_role_members] 
                 WHERE USER_NAME([role_principal_id]) = 'SQLClueAdminRole')     
  WHILE @member IS NOT NULL
   BEGIN
    EXEC sp_droprolemember 'SQLClueAdminRole', @member
    SET @member = (SELECT TOP 1 USER_NAME([member_principal_id]) 
                   FROM [sys].[database_role_members] 
                   WHERE USER_NAME([role_principal_id]) = 'SQLClueAdminRole')     
   END
   DROP ROLE [SQLClueAdminRole]
 END 

GO 

