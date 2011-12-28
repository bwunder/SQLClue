ALTER TABLE [SQLCfg].[tSQLCfg] 
DROP  CONSTRAINT [dft_tSQLCfg_LicenseDate] 
GO
ALTER TABLE [SQLCfg].[tSQLCfg] 
ADD CONSTRAINT [dft_tSQLCfg_LicenseDate] 
DEFAULT (CAST(CAST(CURRENT_TIMESTAMP AS [DATE]) AS [VARCHAR] (30)))
FOR [LicenseDate] 
GO
UPDATE [SQLCfg].[tSQLCfg]
SET [LicenseDate] = CAST(CAST([LicenseDate] AS [DATE]) AS [VARCHAR] (30))
GO
/******************************************************************************
**    Auth: Bill Wunder
**    Date: April 1, 2007
**
**    Note: changes need to be applied to assy refresh too
**
*******************************************************************************
**    Change History
*******************************************************************************
**    Date           Author             Description of Change
**    11/28/2008     bw                 added validation of LicenseDate               
**              
*******************************************************************************/

ALTER TRIGGER [SQLCfg].[trgtSQLCfg_Insert_Update_Delete]
ON [SQLCfg].[tSQLCfg]
FOR INSERT, UPDATE, DELETE
AS
BEGIN

 DECLARE @icount [INT]
  , @dcount [INT]
  , @ucount [INT]
  , @Node  [SQLCfgNode] 
  , @TextData [VARCHAR] (2048);

 SET NOCOUNT ON;

 BEGIN TRY

  SELECT @icount = COUNT(*) FROM [inserted];
  SELECT @dcount = COUNT(*) FROM [deleted];
  SELECT @ucount = COUNT(*) FROM [SQLCfg].[tSQLCfg];

  -- no multi row
  IF @icount > 1 OR @dcount > 1
   BEGIN
    SET @TextData = 'Multi-row [SQLCfg].[tSQLCfg] operations are not permitted';
    RAISERROR (@TextData, 16,1);
   END

  -- only one row in table
  IF @ucount > 1
   BEGIN
    SET @TextData = 'Cannot add multiple [SQLCfg].[tSQLCfg] configurations';
    RAISERROR (@TextData, 16,1);
   END

  -- no delete
  IF @dcount = 1 and @icount = 0 
   BEGIN
    SET @TextData = 'Cannot remove the [SQLCfg].[tSQLCfg] configuration';
    RAISERROR (@TextData, 16,1);
   END

  -- LicenseDate must evaluate to a date
  IF (SELECT ISDATE(CAST([LicenseDate] AS [DATETIME])) FROM inserted) = 0
   BEGIN
    SET @TextData = '[SQLCfg].[tSQLCfg].[LicenseDate] is not a valid date at this locale';
    RAISERROR (@TextData, 16,1);
   END

  SET @Node = 'SQLCfgMetadata|SQLCfg.tSQLCfg';

  -- update
  IF @dcount = 1 and @icount = 1
   BEGIN

    -- add audit info
    UPDATE [SQLCfg].[tSQLCfg]
    SET [RecUpdatedDt] = CURRENT_TIMESTAMP
     , [RecUpdatedUser] = ORIGINAL_LOGIN();

    -- record the update to change
    INSERT [SQLCfg].[tChange]
     ( [Node]  
     , [Version]
     , [Action] 
     , [Definition]
     , [DefinitionDt] )
    SELECT 
       @Node
     , [SQLCfg].[fLastVersion](@Node) + 1
     , 'Modify'
     , 'UPDATE [SQLCfg].[tSQLCfg]
SET [LicensedCompany] = ''' + i.[LicensedCompany] + '''
 , [LicensedUser] = ''' + i.[LicensedUser] + '''
 , [LicenseCode] = ''' + i.[LicenseCode] + '''
 , [LicensedInstanceCount] = ' + CAST(i.[LicensedInstanceCount] AS [VARCHAR] (10)) + '
 , [LicenseDate] = ''' + i.[LicenseDate] + '''
 , [RecCreatedDt] = ''' + CAST(i.[RecCreatedDt] AS [VARCHAR] (20)) + '''
 , [RecCreatedUser] = ''' + i.[RecCreatedUser] + '''
 , [RecUpdatedDt] = ''' + CAST(i.[RecUpdatedDt] AS [VARCHAR] (20)) + '''
 , [RecUpdatedUser] = ''' + i.[RecUpdatedUser] + '''
WHERE [LicensedCompany] = ''' + d.[LicensedCompany] + '''
AND [LicensedUser] = ''' + d.[LicensedUser] + '''
AND [LicenseCode] = ''' + d.[LicenseCode] + '''
AND [LicensedInstanceCount] = ' + CAST(d.[LicensedInstanceCount] AS [VARCHAR] (10)) + '
AND [LicenseDate] = ''' + d.[LicenseDate] + '''
AND [RecCreatedDt] = ''' + CAST(d.[RecCreatedDt] AS [VARCHAR] (20)) + '''
AND [RecCreatedUser] = ''' + d.[RecCreatedUser] + '''
AND [RecUpdatedDt] = ''' + CAST(d.[RecUpdatedDt] AS [VARCHAR] (20)) + '''
AND [RecUpdatedUser] = ''' + d.[RecUpdatedUser] + '''
AND ORIGINAL_LOGIN() = ''' + ORIGINAL_LOGIN() + ''''
     , CURRENT_TIMESTAMP
    FROM [inserted] i
    CROSS JOIN [deleted] d;

   END;      

  -- insert
  IF @dcount = 0 and @icount = 1
   BEGIN
    -- log the insert to change
    INSERT [SQLCfg].[tChange]
     ( [Node]  
     , [Version]
     , [Action] 
     , [Definition]
     , [DefinitionDt] )
    SELECT 
       @Node
     , [SQLCfg].[fLastVersion](@Node) + 1
     , 'Include'
     , 'INSERT [SQLCfg].[tSQLCfg]
 ( [LicensedCompany] 
 , [LicensedUser] 
 , [LicenseCode]
 , [LicensedInstanceCount]
 , [LicenseDate] 
 , [RecCreatedDt] 
 , [RecCreatedUser] 
 , [RecUpdatedDt] 
 , [RecUpdatedUser] )
SELECT ''' + i.[LicensedCompany] + '''
 , ''' + i.[LicensedUser] + '''
 , ''' + i.[LicenseCode] + '''
 , ' + CAST(i.[LicensedInstanceCount] AS [VARCHAR] (10)) + '
 , ''' + i.[LicenseDate] + '''
 , ''' + CAST(i.[RecCreatedDt] AS [VARCHAR] (20)) + '''
 , ''' + i.[RecCreatedUser] + '''
 , ''' + CAST(i.[RecUpdatedDt] AS [VARCHAR] (20)) + '''
 , ''' + i.[RecUpdatedUser] + ''''
     , CURRENT_TIMESTAMP
    FROM [inserted] i;

   END;

 END TRY 

 BEGIN CATCH

  EXEC [SQLCfg].[pLogSQLError] @TextData, @@PROCID;

 END CATCH 

END;

GO
/******************************************************************************
**    Auth: Bill Wunder
**    Date: April 1, 2007
**
**    Desc: List items changed for a date
**
*******************************************************************************
**    Change History
*******************************************************************************
**    Date           Author             Description of Change
**    11/28/2009     bw                 remove local specific date conversion          
**
*******************************************************************************/

ALTER Procedure [SQLCfg].[pChangesForDate]
 ( @ChangeDate [DATETIME] = NULL
 , @RootNode [NVARCHAR] (786) = '' )
AS
BEGIN

SET NOCOUNT ON;

SELECT Id 
 , [Node].ToString() AS [Node]
 , [Node].[Database] AS [Database]
 , [Node].[Collection] AS [Collection]
 , [Node].[Item] AS [ChangeItem]
 , [Version]
 , [Action]
 , [RecCreatedDt]
FROM [SQLCfg].[tChange]
WHERE CHARINDEX(@RootNode, Node.ToString()) = 1
AND CAST([RecCreatedDt] AS [DATE]) = CAST(ISNULL(@ChangeDate, CURRENT_TIMESTAMP) AS [DATE])
ORDER BY [Database] DESC, [Collection] DESC, [ChangeItem] DESC; 

END
GO

/******************************************************************************
**    Auth: Bill Wunder
**    Date: April 1, 2007
**
**    Desc: List changes on server for an item, most recent changes first
**       Business rule, ant thing older than @DaystoShow will not be 
**       considred as canididates for the 'revert' status. Instead a 
**       new and possibly duplicated version will be added.
*******************************************************************************
**    Change History
*******************************************************************************
**    Date           Author             Description of Change
**    11/28/2009     bw                 remove local specific date conversion          
**
*******************************************************************************/

ALTER Procedure [SQLCfg].[pChangeHistoryByItem]
 ( @Node [NVARCHAR] (786)
 , @DaysToShow [INT] = 10000 )
AS
BEGIN

SET NOCOUNT ON;

SELECT [Id] 
 , [Version]
 , [Action]
 , CAST([RecCreatedDt] AS [DATE]) AS [Date]
FROM [SQLCfg].[tChange]
WHERE [Node] = @Node
AND [RecCreatedDt] > CAST(CURRENT_TIMESTAMP - @DaysToShow AS [DATE])
ORDER BY [Id] DESC; 

END
GO
/******************************************************************************
**    Auth: Bill Wunder
**    Date: April 1, 2007
**
**    Desc: Get the SQL errors for the the last n days  
**
*******************************************************************************
**    Change History
*******************************************************************************
**    Date           Author             Description of Change
**    11/28/2009     bw                 add date conversion          
**              
*******************************************************************************/

ALTER PROCEDURE [SQLCfg].[pSQLErrorLogSelectMostRecent] 
 ( @DaysToGet [INT] = 30 )
AS
BEGIN

 SELECT [UserName]
  , [DBName]
  , [ErrorNumber]
  , [ErrorSeverity]
  , [ErrorState]
  , [ErrorProcedure]
  , [ErrorLine]
  , [ErrorMessage]
  , [TextData]
  , [Notes]
  , [RecCreatedDt] 
 FROM [SQLCfg].[tSQLErrorLog]  
 WHERE [RecCreatedDt] > CAST(CURRENT_TIMESTAMP - @DaysToGet AS DATE);
 
END
GO

/******************************************************************************
**    Auth: Bill Wunder
**    Date: April 1, 2007
**
**    Desc: Get the number of changes for the last @DaysToShow days
**       optionally filter by Instance, Database, Schema, ItemType and/or Name
*******************************************************************************
**    Change History
*******************************************************************************
**    Date           Author             Description of Change
**    11/28/2009     bw                 remove local specific date conversion          
**              
*******************************************************************************/

ALTER Procedure [SQLCfg].[pChangeActionSummaryByNodeForDays]
 ( @DaysToShow [INT] = 30 
 , @Node [NVARCHAR] (786) = 'All' )
AS
BEGIN

SET NOCOUNT ON;

SELECT [Node].[SQLInstance] As [SQLInstance] 
 , [Node].[Type] As [Type]
 , CAST([RecCreatedDt] AS [DATE]) AS [Date]
 , CASE WHEN @Node = 'All' THEN 'All Nodes' ELSE @Node END AS [Node]
    , Count(*) AS [TotalChangeCount] 
 , SUM(CASE WHEN Action = 'Include' THEN 1 ELSE 0 END) AS [MetadataRowInserts] 
 , SUM(CASE WHEN Action = 'Modify' THEN 1 ELSE 0 END) AS [MetadataRowUpdates] 
 , SUM(CASE WHEN Action = 'Remove' THEN 1 ELSE 0 END) AS [MetadataRowDeletes] 
 , SUM(CASE WHEN Action = 'Add' THEN 1 ELSE 0 END) AS [SQLConfigurationAdded] 
 , SUM(CASE WHEN Action = 'Change' THEN 1 ELSE 0 END) AS [SQLConfigurationChanged] 
 , SUM(CASE WHEN Action = 'Delete' THEN 1 ELSE 0 END) AS [SQLConfigurationRemoved] 
FROM [SQLCfg].[tChange]
WHERE [RecCreatedDt] > CAST(CURRENT_TIMESTAMP - @DaysToShow AS [DATE])
AND [Node] = CASE WHEN @Node = 'All' THEN [Node] ELSE @Node END
GROUP BY [Node].[SQLInstance], [Node].[Type]
 , CAST([RecCreatedDt] AS [DATE])
ORDER BY [Date]
 , [SQLInstance]
 , [Type];

END
GO
/******************************************************************************
**    Auth: Bill Wunder
**    Date: April 1, 2007
**
**    Desc: Get the list of changes for the last @DaysToShow days
**       optionally filter by Instance, Database, Schema, ItemType and/or Name
*******************************************************************************
**    Change History
*******************************************************************************
**    Date           Author             Description of Change
**    11/28/2009     bw                 remove local specific date conversion          
**              
*******************************************************************************/

ALTER Procedure [SQLCfg].[pChangeActionByNodeForDays]
 ( @DaysToShow [INT] = 30 
 , @Node [NVARCHAR] (786) = 'All' )
AS
BEGIN

SET NOCOUNT ON;

SELECT [Node].[SQLInstance] As [SQLInstance] 
 , [Node].[Type] As [Type]
 , CAST([RecCreatedDt] AS [DATE]) AS [Date]
 , CASE WHEN @Node = 'All' THEN 'All Nodes' ELSE @Node END AS [Node]
 , Action 
FROM [SQLCfg].[tChange]
WHERE [RecCreatedDt] > CAST(CURRENT_TIMESTAMP - @DaysToShow AS [DATE])
AND [Node] = CASE WHEN @Node = 'All' THEN [Node] ELSE @Node END
ORDER BY [Date], [SQLInstance], [Type];

END
GO

