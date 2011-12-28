-- upgrade the SQLRunbook schema from v1.3.5.4 to v1.3.5.5
USE SQLClueRunbook;
 
IF COLUMNPROPERTY(OBJECT_ID('SQLRunbook.tCategory','U')
                  , 'LastUpdatedDt'
                  , 'ColumnId') IS NULL
 BEGIN
  ALTER TABLE [SQLRunbook].[tCategory] WITH NOCHECK
   ADD [LastUpdatedDt] [DATETIME] NOT NULL  
      CONSTRAINT [dft_tCategory__LastUpdatedDt] 
      DEFAULT (CURRENT_TIMESTAMP)
    , [LastUpdatedUser] [NVARCHAR] (128) NOT NULL 
      CONSTRAINT [dft_tCategory_LastUpdatedUser] 
      DEFAULT (ORIGINAL_LOGIN());
      
  CREATE NONCLUSTERED INDEX [ixn_tCategory__LastUpdatedDt]
  ON [SQLRunbook].[tCategory] (LastUpdatedDt);

  CREATE NONCLUSTERED INDEX [ixn_tCategory__LastUpdatedUser]
  ON [SQLRunbook].[tCategory] (LastUpdatedUser);
 
 END 

GO

IF COLUMNPROPERTY(OBJECT_ID('SQLRunbook.tDocument','U')
                  , 'LastUpdatedDt'
                  , 'ColumnId') IS NULL
 BEGIN

  ALTER TABLE [SQLRunbook].[tDocument] WITH NOCHECK
   ADD [LastUpdatedDt] [DATETIME] NOT NULL  
       CONSTRAINT [dft_tDocument__LastUpdatedDt] 
       DEFAULT (CURRENT_TIMESTAMP)
     , [LastUpdatedUser] [NVARCHAR] (128) NOT NULL 
       CONSTRAINT [dft_tDocument_LastUpdatedUser] 
       DEFAULT (ORIGINAL_LOGIN());

  CREATE NONCLUSTERED INDEX [ixn_tDocument__LastUpdatedDt]
  ON [SQLRunbook].[tDocument] ([LastUpdatedDt]);

  CREATE NONCLUSTERED INDEX [ixn_tDocument__LastUpdatedUser]
  ON [SQLRunbook].[tDocument] ([LastUpdatedUser]);

 END
 
GO
 
IF INDEXPROPERTY(OBJECT_ID('SQLRunbook.tDocument'),'pkc_tDocument__Id','IsClustered') = 1
 BEGIN
 
  IF EXISTS (SELECT * 
           FROM [sys].[fulltext_indexes] 
           WHERE object_name([object_id]) = 'tDocument') 
   DROP FULLTEXT INDEX ON [SQLRunbook].[tDocument];

  ALTER TABLE [SQLRunbook].[tDocumentRating]
  DROP CONSTRAINT [fk_tDocumentRating__DocumentId__TO__tDocument__Id]
   
  ALTER TABLE [SQLRunbook].[tTopicDocument]
  DROP CONSTRAINT [fk_tTopicDocument__DocumentId__TO__tDocument__Id]
  
  ALTER TABLE [SQLRunbook].[tDocument]
    DROP CONSTRAINT [pkc_tDocument__Id];
  
  ALTER TABLE [SQLRunbook].[tDocument]
    ADD CONSTRAINT [pkc_tDocument__Id]
    PRIMARY KEY NONCLUSTERED (Id);

  DROP INDEX [SQLRunbook].[tDocument].[ixn_tDocument__RecCreatedDt];

  CREATE CLUSTERED INDEX [ixn_tDocument__RecCreatedDt]
    ON [SQLRunbook].[tDocument] ([RecCreatedDt]);
  
  ALTER TABLE [SQLRunbook].[tDocumentRating]
    ADD CONSTRAINT [fk_tDocumentRating__DocumentId__TO__tDocument__Id]
    FOREIGN KEY ([DocumentId]) References [SQLRunbook].[tDocument] ([Id])
   
  ALTER TABLE [SQLRunbook].[tTopicDocument]
    ADD CONSTRAINT [fk_tTopicDocument__DocumentId__TO__tDocument__Id]
    FOREIGN KEY ([DocumentId]) References [SQLRunbook].[tDocument] ([Id])
   
  CREATE FULLTEXT INDEX ON [SQLRunbook].[tDocument]
   ([File] LANGUAGE 0X0, [Document] TYPE COLUMN [DocumentType] LANGUAGE 0X0 )
  KEY INDEX [pkc_tDocument__Id] ON [ftSQLRunbookCatalog]
  WITH CHANGE_TRACKING AUTO;                    
  
 END  

GO

/******************************************************************************
**	Auth: Bill Wunder
**	Date: April 1, 2007
**
*******************************************************************************
**	Change History
*******************************************************************************
**	Date		   Author		     Description of Change
**	Mar 10, 2009   bw                maintain row update audit columns       
**	          
*******************************************************************************/
ALTER TRIGGER [SQLRunbook].[trgDocument_Insert_Update_Delete]
ON [SQLRunbook].[tDocument]
FOR INSERT, UPDATE, DELETE
AS
BEGIN

 DECLARE @icount [INT]
  , @dcount [INT]
  , @DocumentId [INT]
  , @OriginalLogin NVARCHAR(256)
  , @Owner NVARCHAR(256)
  , @Op NVARCHAR(10);

 SET NOCOUNT ON;

 BEGIN TRY

  SELECT @icount = COUNT(*) FROM [inserted];
  SELECT @dcount = COUNT(*) FROM [deleted];
  SET @OriginalLogin  = ORIGINAL_LOGIN();

  -- no multi row
  IF @icount > 1 OR @dcount > 1
    RAISERROR ('Multi-row operations are not permitted', 16,1);

  -- EnforceOwnership - row level security - all other triggers reference these comments to aid consistency 
  -- only admins, the service or the document owner can update or delete if the host instance is also a 
  -- target instance the service account will be elevated to sysadmin for archive and trace. A sysadmin 
  -- will enter any database as dbo and the token will have no db role information (see sys.user_token)  
  -- Best to assure only members of the SQLRunbookAdminRole and SQLClueAdminRole roles are sa on the host
  IF @dcount = 1 
  AND (SELECT [SQLRunbook].[fnIsAdmin]()) = 0
  AND (SELECT [EnforceOwnership] FROM [SQLRunbook].[tOption]) = 1
   BEGIN
    SELECT @DocumentId = [Id], @Owner = [Owner] FROM [deleted] WHERE [Owner] <> @OriginalLogin; 
    IF @@ROWCOUNT = 1
     BEGIN
      SET @Op = CASE WHEN @icount = 1 AND @dcount = 1 THEN 'update' ELSE 'delete' END;
      RAISERROR ('User [%s] attempted to REMOVE Document %d owned by user [%s]'
                , 16
                , 1
                , @OriginalLogin
                , @DocumentId
                , @Owner);
     END;
   END;

  -- update the topic notes when associated documents are inserted, updated, or deleted
  -- always maintain the SQLRunbookUser
  
  -- delete
  IF @dcount = 1 and @icount = 0 
   EXEC [SQLRunbook].[pUserUpsert] @OriginalLogin = @OriginalLogin
                                    , @DocumentsDeleted = 1;   

  -- update
  IF @dcount = 1 and @icount = 1
   BEGIN
    UPDATE d
    SET [LastUpdatedDt] = CURRENT_TIMESTAMP
     , [LastUpdatedUser] = @OriginalLogin
    FROM [inserted] i
    JOIN [SQLRunbook].[tDocument] d
    ON i.[Id] = d.[Id]

    EXEC [SQLRunbook].[pUserUpsert] @OriginalLogin = @OriginalLogin
                                     , @DocumentsUpdated = 1;   

   END 

  -- insert
  IF @dcount = 0 and @icount = 1
   EXEC [SQLRunbook].[pUserUpsert] @OriginalLogin = @OriginalLogin
                                    , @DocumentsAdded = 1;   

 END TRY 

 BEGIN CATCH

  EXEC [SQLRunbook].[pLogSQLError] NULL, @@PROCID;

 END CATCH 

END;

GO 

/******************************************************************************
**	Auth: Bill Wunder
**	Date: April 1, 2007
**
*******************************************************************************
**	Change History
*******************************************************************************
**	Date		   Author		     Description of Change
**	MAR 10 2009    bw                maintain row updated audit columns
**	          
*******************************************************************************/
ALTER TRIGGER [SQLRunbook].[trgCategory_Insert_Update_Delete]
ON [SQLRunbook].[tCategory]
FOR INSERT, UPDATE, DELETE
AS
BEGIN

 DECLARE @icount [INT]
  , @dcount [INT]
  , @CategoryId [INT]
  , @Op [NVARCHAR] (10)
  , @OriginalLogin [NVARCHAR] (128);

 SET NOCOUNT ON;

 BEGIN TRY

  SELECT @icount = COUNT(*) FROM [inserted];
  SELECT @dcount = COUNT(*) FROM [deleted];
  SET @OriginalLogin = ORIGINAL_LOGIN();
  

  -- application security
  -- EnforceOwnership - only admins can change categories 
  IF @dcount > 0 OR @icount > 0
   BEGIN
    IF (SELECT [SQLRunbook].[fnIsAdmin]()) = 0
     BEGIN
      SET @Op = CASE WHEN @icount > 0 THEN 'delete' ELSE CASE WHEN @dcount > 0 THEN 'update' ELSE 'insert' END END;
      RAISERROR ('Invalid attempted to %s Category by User [%s].', 16,1, @OriginalLogin, @Op, @CategoryId);
     END;
   END;

  -- delete
  IF @dcount > 0 and @icount = 0 
   EXEC [SQLRunbook].[pUserUpsert] @OriginalLogin = @OriginalLogin
                                    , @CategoriesDeleted = @dcount   

  -- updates
  IF @dcount > 0 and @icount > 0
   BEGIN
   
    UPDATE c
    SET [LastUpdatedDt] = CURRENT_TIMESTAMP
     , [LastUpdatedUser] = @OriginalLogin
    FROM [inserted] i
    JOIN [SQLRunbook].[tCategory] c
    ON i.[Id] = c.[Id]
   
    EXEC [SQLRunbook].[pUserUpsert] @OriginalLogin = @OriginalLogin
                                     , @CategoriesUpdated = @dcount   

   END
  -- insert
  IF @dcount = 0 AND @icount > 0

    EXEC [SQLRunbook].[pUserUpsert] @OriginalLogin = @OriginalLogin
                                     , @CategoriesAdded = @icount   

 END TRY 

 BEGIN CATCH

  EXEC [SQLRunbook].[pLogSQLError] NULL, @@PROCID;

 END CATCH 

END;

GO 
/******************************************************************************
**	Auth: Bill Wunder
**	Date: April 1, 2007
**
**	Desc: Get the list of Runbook Categories   
**
*******************************************************************************
**	Change History
*******************************************************************************
**	Date		   Author		     Description of Change
**	MAR 10, 2009   bw                include update audit columns          
**
*******************************************************************************/
ALTER PROCEDURE [SQLRunbook].[pCategorySelectAll] 
AS
BEGIN

 SELECT c.[Id]
  , c.[Name]
  , c.[Notes]
  , COUNT(ct.[TopicId]) AS [TopicCount]
  , ISNULL(cr.[RatingTally],0) AS [RatingTally]
  , ISNULL(cr.[RatingCount],0) AS [RatingCount]
  , c.[RecCreatedDt]
  , c.[RecCreatedUser]
  , c.[LastUpdatedDt]
  , c.[LastUpdatedUser]
 FROM [SQLRunbook].[tCategory] c
 LEFT JOIN (SELECT [CategoryId]
             , SUM([RatingId]) AS [RatingTally]
             , COUNT([RatingId]) AS [RatingCount] 
            FROM [SQLRunbook].[tCategoryRating]
            WHERE [RatingId] < 7 
            GROUP BY [CategoryId]) cr
 ON c.[Id] = cr.[CategoryId]
 LEFT JOIN [SQLRunbook].[tCategoryTopic] ct 
 ON ct.[CategoryId] = c.[Id]  
 GROUP BY c.[Id]
  , c.[Name]
  , c.[Notes]
  , cr.[RatingTally]
  , cr.[RatingCount]
  , c.[RecCreatedDt]
  , c.[RecCreatedUser]
  , c.[LastUpdatedDt]
  , c.[LastUpdatedUser]
 ORDER BY [Name];

END;

GO
/******************************************************************************
**	Auth: Bill Wunder
**	Date: April 1, 2007
**
**	Desc: Get the list of Runbook Categories   
**
*******************************************************************************
**	Change History
*******************************************************************************
**	Date		   Author		     Description of Change
**	MAR 10, 2009   bw                include update audit columns          
**	          
*******************************************************************************/
ALTER PROCEDURE [SQLRunbook].[pCategorySelectByDateRange] 
 ( @BeginDt [DATETIME]
 , @EndDt [DATETIME] )
AS
BEGIN

 SELECT c.[Id]
  , c.[Name]
  , c.[Notes]
  , COUNT(ct.[TopicId]) AS [TopicCount]
  , SUM(ISNULL(cr.[RatingId],0)) AS [RatingTally]
  , COUNT(ISNULL(cr.[RatingId],0)) AS [RatingCount]
  , c.[RecCreatedDt]
  , c.[RecCreatedUser]
  , c.[LastUpdatedDt]
  , c.[LastUpdatedUser]
 FROM [SQLRunbook].[tCategory] c
 LEFT JOIN [SQLRunbook].[tCategoryRating] cr
 ON c.[Id] = cr.[CategoryId]
 AND [RatingId] < 7 
 LEFT JOIN [SQLRunbook].[tCategoryTopic] ct 
 ON ct.[CategoryId] = c.[Id] 
 WHERE c.[LastUpdatedDt] BETWEEN @BeginDt AND @EndDt 
 GROUP BY c.[Id]
  , c.[Name]
  , c.[Notes]
  , c.[RecCreatedDt]
  , c.[RecCreatedUser]
  , c.[LastUpdatedDt]
  , c.[LastUpdatedUser]
 ORDER BY [RecCreatedDt], [Name];

END;

GO
/******************************************************************************
**	Auth: Bill Wunder
**	Date: April 1, 2007
**
**	Desc: Get the list of Runbook Categories that have assoc'd topics   
**
*******************************************************************************
**	Change History
*******************************************************************************
**	Date		   Author		     Description of Change
**	MAR 10, 2009   bw                include update audit columns          
**	          
*******************************************************************************/
ALTER PROCEDURE [SQLRunbook].[pCategorySelectActive] 
AS
BEGIN

 SELECT c.[Id]
  , c.[Name]
  , c.[Notes]
  , COUNT(ct.[TopicId]) AS [TopicCount]
  , ISNULL(cr.[RatingTally], 0) AS [RatingTally]
  , ISNULL(cr.[RatingCount], 0) AS [RatingCount]
  , c.[RecCreatedDt]
  , c.[RecCreatedUser]
  , c.[LastUpdatedDt]
  , c.[LastUpdatedUser]
 FROM [SQLRunbook].[tCategory] c
 LEFT JOIN (SELECT [CategoryId]
             , SUM([RatingId]) AS [RatingTally]
             , COUNT([RatingId]) AS [RatingCount] 
            FROM [SQLRunbook].[tCategoryRating]
            WHERE [RatingId] < 7 
            GROUP BY [CategoryId]) cr
 ON c.[Id] = cr.[CategoryId]
 JOIN [SQLRunbook].[tCategoryTopic] ct 
 ON ct.[CategoryId] = c.[Id]  
 GROUP BY c.[Id]
  , c.[Name]
  , c.[Notes]
  , cr.[RatingTally]
  , cr.[RatingCount]
  , c.[RecCreatedDt]
  , c.[RecCreatedUser]
  , c.[LastUpdatedDt]
  , c.[LastUpdatedUser]
 ORDER BY [Name];

END;

GO
/******************************************************************************
**	Auth: Bill Wunder
**	Date: April 1, 2009
**
**	Desc: Select a Runbook Category row for the catalog report  
**
*******************************************************************************
**	Change History
*******************************************************************************
**	Date		   Author		     Description of Change
**	          
*******************************************************************************/
ALTER PROCEDURE [SQLRunbook].[pCategorySelectByName]  
 ( @Name [NVARCHAR] (128) )
AS
BEGIN

 SELECT c.[Id]
  , c.[Name]
  , c.[Notes]
  , COUNT(ct.[TopicId]) AS [TopicCount]
  , ISNULL(cr.[RatingTally], 0) AS [RatingTally]
  , ISNULL(cr.[RatingCount], 0) AS [RatingCount]
  , c.[RecCreatedDt]
  , c.[RecCreatedUser]
  , c.[LastUpdatedDt]
  , c.[LastUpdatedUser]
 FROM [SQLRunbook].[tCategory] c
 LEFT JOIN (SELECT [CategoryId]
             , SUM([RatingId]) AS [RatingTally]
             , COUNT([RatingId]) AS [RatingCount] 
            FROM [SQLRunbook].[tCategoryRating]
            WHERE [RatingId] < 7 
            GROUP BY [CategoryId]) cr
 ON c.[Id] = cr.[CategoryId]
 LEFT JOIN [SQLRunbook].[tCategoryTopic] ct 
 ON ct.[CategoryId] = c.[Id]  
 WHERE [Name] = @Name
 GROUP BY c.[Id]
  , c.[Name]
  , c.[Notes]
  , cr.[RatingTally]
  , cr.[RatingCount]
  , c.[RecCreatedDt]
  , c.[RecCreatedUser]
  , c.[LastUpdatedDt]
  , c.[LastUpdatedUser]
 ORDER BY [Name];

END;

GO
/******************************************************************************
**	Auth: Bill Wunder
**	Date: April 1, 2007
**
**	Desc: Get all categories with [Notes] containing the literal   
**
*******************************************************************************
**	Change History
*******************************************************************************
**	Date		   Author		     Description of Change
**	MAR 10, 2009   bw                include update audit columns          
**	          
*******************************************************************************/
ALTER PROCEDURE [SQLRunbook].[pCategoryContains] 
 ( @SearchString [NVARCHAR] (128) )
AS
BEGIN

  SELECT c.[Id]
   , c.[Name]
   , c.[Notes]
   , COUNT(ct.[TopicId]) AS [TopicCount]
   , ISNULL(cr.[RatingTally], 0) AS [RatingTally]
   , ISNULL(cr.[RatingCount], 0) AS [RatingCount]
   , c.[RecCreatedDt]
   , c.[RecCreatedUser]
   , c.[LastUpdatedDt]
   , c.[LastUpdatedUser]
  FROM [SQLRunbook].[tCategory] c
  LEFT JOIN (SELECT [CategoryId]
              , SUM([RatingId]) AS [RatingTally]
              , COUNT([RatingId]) AS [RatingCount] 
             FROM [SQLRunbook].[tCategoryRating]
             WHERE [RatingId] < 7 
             GROUP BY [CategoryId]) cr
  ON c.[Id] = cr.[CategoryId]
  LEFT JOIN [SQLRunbook].[tCategoryTopic] ct 
  ON ct.[CategoryId] = c.[Id]  
  WHERE CONTAINS((c.[Name], c.[Notes]), @SearchString)
  OR CHARINDEX(@SearchString, c.[Name]) > 0
  GROUP BY c.[Id]
   , c.[Name]
   , c.[Notes]
   , cr.[RatingTally]
   , cr.[RatingCount]
   , c.[RecCreatedDt]
   , c.[RecCreatedUser]
   , c.[LastUpdatedDt]
   , c.[LastUpdatedUser]
 ORDER BY [Name];

END;

GO
/******************************************************************************
**	Auth: Bill Wunder
**	Date: April 1, 2007
**
**	Desc: all attributes for all Runbook documents (no BLOB column)   
**
*******************************************************************************
**	Change History
*******************************************************************************
**	Date		   Author		     Description of Change
**  MAR 10, 2009   bw                include ow update audit columns
**	          
*******************************************************************************/
ALTER PROCEDURE [SQLRunbook].[pDocumentSelectAll]
AS
BEGIN

 --  this is good for nothing except the dataset definition
 SELECT d.[Id] 
  , d.[File]
  , d.[Document]
  , SUM(CASE WHEN dr.[RatingId] IS NULL THEN 0 ELSE dr.[RatingId] END) AS [RatingTally]
  , SUM(CASE WHEN dr.[RatingId] IS NULL THEN 0 ELSE 1 END) AS [RatingCount]
  , d.[DocumentType]
  , d.[LastModifiedDt]
  , d.[Owner]
  , d.[WatchFileForChange]
  , d.[RecCreatedDt]
  , d.[RecCreatedUser]
  , d.[LastUpdatedDt]
  , d.[LastUpdatedUser]
 FROM [SQLRunbook].[tDocument] d
 LEFT JOIN [SQLRunbook].[tDocumentRating] dr
 ON d.[Id] = dr.[DocumentId]
 GROUP BY d.[Id] 
  , d.[File]
  , d.[Document]
  , d.[DocumentType]
  , d.[LastModifiedDt]
  , d.[Owner]
  , d.[WatchFileForChange]
  , d.[RecCreatedDt]
  , d.[RecCreatedUser]
  , d.[LastUpdatedDt]
  , d.[LastUpdatedUser];

END;

GO
/******************************************************************************
**	Auth: Bill Wunder
**	Date: April 1, 2007
**
**	Desc: all documents owned by a user   
**
*******************************************************************************
**	Change History
*******************************************************************************
**	Date		   Author		     Description of Change
**  MAR 10, 2009   bw                include ow update audit columns
**	          
*******************************************************************************/
ALTER PROCEDURE [SQLRunbook].[pDocumentSelectByOwner]
 ( @OriginalLogin [NVARCHAR] (128) )
AS
BEGIN

 SELECT d.[Id] 
  , d.[File]
  , d.[Document]
  , SUM(CASE WHEN dr.[RatingId] IS NULL THEN 0 ELSE dr.[RatingId] END) AS [RatingTally]
  , SUM(CASE WHEN dr.[RatingId] IS NULL THEN 0 ELSE 1 END) AS [RatingCount]
  , d.[DocumentType]
  , d.[LastModifiedDt]
  , d.[Owner]
  , d.[WatchFileForChange]
  , d.[RecCreatedDt]
  , d.[RecCreatedUser]
  , d.[LastUpdatedDt]
  , d.[LastUpdatedUser]
 FROM [SQLRunbook].[tDocument] d
 LEFT JOIN [SQLRunbook].[tDocumentRating] dr
 ON d.[Id] = dr.[DocumentId]
 WHERE d.[Owner] = @OriginalLogin
 GROUP BY d.[Id] 
  , d.[File]
  , d.[Document]
  , d.[DocumentType]
  , d.[LastModifiedDt]
  , d.[Owner]
  , d.[WatchFileForChange]
  , d.[RecCreatedDt]
  , d.[RecCreatedUser]
  , d.[LastUpdatedDt]
  , d.[LastUpdatedUser];

END;

GO
/******************************************************************************
**	Auth: Bill Wunder
**	Date: April 1, 2007
**
**	Desc: Select list of all Runbook documents for a topic
**
*******************************************************************************
**	Change History
*******************************************************************************
**	Date		   Author		     Description of Change
**  MAR 10, 2009   bw                include row update audit columns
**	          
*******************************************************************************/
ALTER PROCEDURE [SQLRunbook].[pDocumentSelectByTopicId]
 ( @TopicId [Int] )
AS
BEGIN

 SELECT DISTINCT d.[Id] 
  , d.[File]
  , ISNULL(dr.[RatingTally],0) AS [RatingTally]
  , ISNULL(dr.[RatingCount],0) AS [RatingCount]
  , d.[DocumentType]
  , d.[LastModifiedDt]
  , d.[Owner] 
  , d.[WatchFileForChange]
  , d.[RecCreatedDt]
  , d.[RecCreatedUser]
  , d.[LastUpdatedDt]
  , d.[LastUpdatedUser]
 FROM [SQLRunbook].[tDocument] d
 LEFT JOIN (SELECT [DocumentId]
             , SUM([RatingId]) AS [RatingTally]
             , COUNT([RatingId]) AS [RatingCount] 
            FROM [SQLRunbook].[tDocumentRating]
            WHERE [RatingId] < 7 
            GROUP BY [DocumentId]) dr
 ON d.[Id] = dr.[DocumentId]
 JOIN [SQLRunbook].[tTopicDocument] td
 ON d.[Id] = td.[DocumentId]
 WHERE td.[TopicId] = @TopicId
 ORDER BY [File];

END;

GO
/******************************************************************************
**	Auth: Bill Wunder
**	Date: April 1, 2007
**
**	Desc: Select list of all Runbook documents for topics associated with a
**        specified category
**
*******************************************************************************
**	Change History
*******************************************************************************
**	Date		   Author		     Description of Change
**  MAR 10, 2009   bw                include row update audit columns
**	          
*******************************************************************************/
ALTER PROCEDURE [SQLRunbook].[pDocumentSelectByCategoryId]
 ( @CategoryId [Int] )
AS
BEGIN

 SELECT DISTINCT d.[Id] 
  , d.[File]
  , ISNULL(dr.[RatingTally],0) AS [RatingTally]
  , ISNULL(dr.[RatingCount],0) AS [RatingCount]
  , d.[DocumentType]
  , d.[Owner] 
  , d.[WatchFileForChange]
  , d.[RecCreatedDt]
  , d.[RecCreatedUser]
  , d.[LastUpdatedDt]
  , d.[LastUpdatedUser]
 FROM [SQLRunbook].[tDocument] d
 LEFT JOIN (SELECT [DocumentId]
             , SUM([RatingId]) AS [RatingTally]
             , COUNT([RatingId]) AS [RatingCount] 
            FROM [SQLRunbook].[tDocumentRating]
            WHERE [RatingId] < 7 
            GROUP BY [DocumentId]) dr
 ON d.[Id] = dr.[DocumentId]
 JOIN [SQLRunbook].[tTopicDocument] td
 ON d.[Id] = td.[DocumentId]
 JOIN [SQLRunbook].[tCategoryTopic] tc
 ON td.[TopicId] = tc.[TopicId]
 WHERE tc.[CategoryId] = @CategoryId
 ORDER BY [File];

END;

GO
/******************************************************************************
**	Auth: Bill Wunder
**	Date: April 1, 2007
**
**	Desc: list of RunbookDocuments for a date (no BLOB)
**
*******************************************************************************
**	Change History
*******************************************************************************
**	Date		   Author		     Description of Change
**  MAR 10, 2009   bw                include row update audit columns
**	          
*******************************************************************************/
ALTER PROCEDURE [SQLRunbook].[pDocumentSelectByDate]
 ( @Date [DATETIME] )
AS
BEGIN

 SELECT DISTINCT d.[Id] 
  , d.[File]
  , ISNULL(dr.[RatingTally],0) AS [RatingTally]
  , ISNULL(dr.[RatingCount],0) AS [RatingCount]
  , d.[DocumentType]
  , d.[Owner] 
  , d.[WatchFileForChange]
  , d.[RecCreatedDt]
  , d.[RecCreatedUser]
  , d.[LastUpdatedDt]
  , d.[LastUpdatedUser]
 FROM [SQLRunbook].[tDocument] d
 LEFT JOIN (SELECT [DocumentId]
             , SUM([RatingId]) AS [RatingTally]
             , COUNT([RatingId]) AS [RatingCount] 
            FROM [SQLRunbook].[tDocumentRating]
            WHERE [RatingId] < 7 
            GROUP BY [DocumentId]) dr
 ON d.[Id] = dr.[DocumentId]
 WHERE CONVERT([NVARCHAR](10),d.[LastUpdatedDt], 101) = CONVERT([NVARCHAR](10), @Date, 101)
 ORDER BY [File];   
 
END;

GO
/******************************************************************************
**	Auth: Bill Wunder
**	Date: April 1, 2007
**
**	Desc: list of RunbookDocuments for a date range (no BLOB)
**
*******************************************************************************
**	Change History
*******************************************************************************
**	Date		   Author		     Description of Change
**  MAR 10, 2009   bw                include row update audit columns
**	          
*******************************************************************************/
ALTER PROCEDURE [SQLRunbook].[pDocumentSelectByDateRange]
 ( @BeginDt [DATETIME] 
 , @EndDt [DATETIME] )
AS
BEGIN

 SELECT DISTINCT d.[Id] 
  , d.[File]
  , ISNULL(dr.[RatingTally],0) AS [RatingTally]
  , ISNULL(dr.[RatingCount],0) AS [RatingCount]
  , d.[DocumentType]
  , d.[Owner] 
  , d.[WatchFileForChange]
  , d.[LastModifiedDt]
  , d.[RecCreatedDt]
  , d.[RecCreatedUser]
  , d.[LastUpdatedDt]
  , d.[LastUpdatedUser]
 FROM [SQLRunbook].[tDocument] d
 LEFT JOIN (SELECT [DocumentId]
             , SUM([RatingId]) AS [RatingTally]
             , COUNT([RatingId]) AS [RatingCount] 
            FROM [SQLRunbook].[tDocumentRating]
            WHERE [RatingId] < 7 
            GROUP BY [DocumentId]) dr
 ON d.[Id] = dr.[DocumentId]
 WHERE d.[LastUpdatedDt] BETWEEN @BeginDt AND @EndDt	 
 ORDER BY [LastUpdatedDt]
  , [File];   
 
END;

GO
/******************************************************************************
**	Auth: Bill Wunder
**	Date: April 1, 2007
**
**	Desc: list of RunbookDocuments changed within a date range (no BLOB)
**
*******************************************************************************
**	Change History
*******************************************************************************
**	Date		   Author		     Description of Change
**  MAR 10, 2009   bw                include row update audit columns
**	          
*******************************************************************************/
ALTER PROCEDURE [SQLRunbook].[pDocumentSelectChangedByDateRange]
 ( @BeginDt [DATETIME] 
 , @EndDt [DATETIME] )
AS
BEGIN

 SELECT DISTINCT d.[Id] 
  , d.[File]
  , ISNULL(dr.[RatingTally],0) AS [RatingTally]
  , ISNULL(dr.[RatingCount],0) AS [RatingCount]
  , d.[DocumentType]
  , d.[Owner] 
  , d.[WatchFileForChange]
  , d.[LastModifiedDt]
  , d.[RecCreatedDt]
  , d.[RecCreatedUser]
  , d.[LastUpdatedDt]
  , d.[LastUpdatedUser]
 FROM [SQLRunbook].[tDocument] d
 LEFT JOIN (SELECT [DocumentId]
             , SUM([RatingId]) AS [RatingTally]
             , COUNT([RatingId]) AS [RatingCount] 
            FROM [SQLRunbook].[tDocumentRating]
            WHERE [RatingId] < 7 
            GROUP BY [DocumentId]) dr
 ON d.[Id] = dr.[DocumentId]
 WHERE d.[LastUpdatedDt] BETWEEN @BeginDt AND @EndDt
 ORDER BY [LastUpdatedDt]
  , [File];   
 
END;

GO
/******************************************************************************
**	Auth: Bill Wunder
**	Date: April 1, 2007
**
**	Desc: Get all documents that contain the literal   
**
*******************************************************************************
**	Change History
*******************************************************************************
**	Date		   Author		     Description of Change
**  MAR 10, 2009   bw                include row update audit columns
**	          
*******************************************************************************/
ALTER PROCEDURE [SQLRunbook].[pDocumentSelectByContains] 
 ( @SearchString [NVARCHAR] (128) )
AS
BEGIN

 SELECT DISTINCT d.[Id] 
  , d.[File]
  , ISNULL(dr.[RatingTally],0) AS [RatingTally]
  , ISNULL(dr.[RatingCount],0) AS [RatingCount]
  , d.[DocumentType]
  , d.[Owner] 
  , d.[WatchFileForChange]
  , d.[RecCreatedDt]
  , d.[RecCreatedUser]
  , d.[LastUpdatedDt]
  , d.[LastUpdatedUser]
 FROM [SQLRunbook].[tDocument] d
 LEFT JOIN (SELECT [DocumentId]
             , SUM([RatingId]) AS [RatingTally]
             , COUNT([RatingId]) AS [RatingCount] 
            FROM [SQLRunbook].[tDocumentRating]
            WHERE [RatingId] < 7 
            GROUP BY [DocumentId]) dr
 ON d.[Id] = dr.[DocumentId]
 WHERE CONTAINS((d.[File], d.[Document]), @SearchString) 
 ORDER BY [File];   

END;

GO
/******************************************************************************
**	Auth: Bill Wunder
**	Date: April 1, 2007
**
**	Desc: catalog of all runbook entries  
**
*******************************************************************************
**	Change History
*******************************************************************************
**	Date		   Author		     Description of Change
**  MAR 10, 2009   bw                remove dates
**	          
*******************************************************************************/
ALTER PROCEDURE [SQLRunbook].[pRunbookCatalog] 
AS
BEGIN

 SELECT c.[Name] AS [Category]
    , t.[Id] AS [TopicId]
    , t.[Name] AS [Topic]
    , tr.[RatingId] AS [TopicRating]
    , d.[Id] AS [DocumentId]
    , d.[File] AS [File]
    , LEN(d.[Document])  AS [DocumentBytes]
    , dr.[RatingId] AS [DocumentRating]
    , fdt.[document_type] AS [IFilterType]
 FROM [SQLRunbook].[tCategory] c
 LEFT JOIN [SQLRunbook].[tCategoryTopic] ct
 ON c.[Id] = ct.[CategoryId]
 LEFT JOIN [SQLRunbook].[tTopic] t
 ON ct.[TopicId] = t.[Id]
 LEFT JOIN [SQLRunbook].[tTopicDocument] td
 ON ct.[TopicId] = td.[TopicId]
 LEFT JOIN [SQLRunbook].[tDocument] d
 ON d.[Id] = td.[DocumentId]
 LEFT JOIN [SQLRunbook].[tTopicRating] tr
 ON tr.[TopicId] = t.[Id]
 LEFT JOIN [SQLRunbook].[tDocumentRating] dr
 ON dr.[DocumentId] = d.[Id]
 LEFT JOIN [sys].[fulltext_document_types] fdt
 ON d.[DocumentType] = fdt.[document_type]  
 UNION
 SELECT NULL AS [Category]
    , t.[Id] AS [TopicId]
    , t.[Name] AS [Topic]
    , tr.[RatingId] AS [TopicRating]
    , d.[Id] AS [DocumentId]
    , d.[File] AS [File]
    , LEN(d.[Document])  AS [DocumentBytes]
    , dr.[RatingId] AS [DocumentRating]
    , fdt.[document_type] AS [IFilterType]
 FROM [SQLRunbook].[tTopic] t
 LEFT JOIN [SQLRunbook].[tCategoryTopic] ct
 ON ct.[TopicId] = t.[Id]
 LEFT JOIN [SQLRunbook].[tTopicDocument] td
 ON ct.[TopicId] = td.[TopicId]
 LEFT JOIN [SQLRunbook].[tDocument] d
 ON d.[Id] = td.[DocumentId]
 LEFT JOIN [SQLRunbook].[tTopicRating] tr
 ON tr.[TopicId] = t.[Id]
 LEFT JOIN [SQLRunbook].[tDocumentRating] dr
 ON dr.[DocumentId] = d.[Id]
 LEFT JOIN [sys].[fulltext_document_types] fdt
 ON d.[DocumentType] = fdt.[document_type]
 WHERE ct.[TopicId] IS NULL  
 UNION
 SELECT NULL AS [Category]
    , NULL AS [TopicId]
    , NULL AS [Topic]
    , NULL AS [TopicRating]
    , d.[Id] AS [DocumentId]
    , d.[File] AS [File]
    , LEN(d.[Document])  AS [DocumentBytes]
    , dr.[RatingId] AS [DocumentRating]
    , fdt.[document_type] AS [IFilterType]
 FROM [SQLRunbook].[tDocument] d
 LEFT JOIN [SQLRunbook].[tTopicDocument] td
 ON d.[Id] = td.[DocumentId]
 LEFT JOIN [SQLRunbook].[tDocumentRating] dr
 ON dr.[DocumentId] = d.[Id]
 LEFT JOIN [sys].[fulltext_document_types] fdt
 ON d.[DocumentType] = fdt.[document_type]  
 WHERE td.[DocumentId] Is NULL
 ORDER BY [Category]
  , [Topic]
  , [DocumentId];

END;

GO
/******************************************************************************
**	Auth: Bill Wunder
**	Date: April 1, 2007
**
**	Desc: runbook activity   
**
*******************************************************************************
**	Change History
*******************************************************************************
**	Date		   Author		     Description of Change
**	          
*******************************************************************************/
ALTER PROCEDURE [SQLRunbook].[pRunbookActivity]
 ( @DaysToShow [INT]
 , @EndDt [DATETIME])
AS
BEGIN

 -- there could be unassignd categories, topics or documents
 SELECT CONVERT([DATETIME], [ActivityDate]) AS [ActivityDate] 
  , SUM([CategoryCreated]) AS [CategoriesCreated]
  , SUM([CategoryAssigned]) AS [CategoriedAssigned]
  , SUM([TopicCreated]) AS [TopicsCreated]
  , SUM([TopicRated]) AS [TopicsUpdated]
  , SUM([TopicRated]) AS [TopicsRated]
  , SUM([DocumentAssigned]) AS [DocumentsAssigned]
  , SUM([DocumentCreated]) AS [DocumentsCreated]
  , SUM([DocumentAddBytes])/1000 AS [DocumentAddKBytes]
  , SUM([DocumentUpdated]) AS [DocumentsUpdated]
  , SUM([DocumentUpdateBytes])/1000 AS [DocumentUpdateKBytes]
  , SUM([DocumentRated]) AS [DocumentsRated]
  ,(SELECT MAX([Value])
    FROM (SELECT SUM([CategoryCreated]) AS [Value]
          UNION ALL
          SELECT SUM([CategoryAssigned])
          UNION ALL
          SELECT SUM([TopicCreated])
          UNION ALL
          SELECT SUM([TopicUpdated])
          UNION ALL
          SELECT SUM([TopicRated])
          UNION ALL
          SELECT SUM([DocumentAssigned])
          UNION ALL
          SELECT SUM([DocumentCreated])
          UNION ALL
          SELECT SUM([DocumentUpdated])
          UNION ALL
          SELECT SUM([DocumentRated])) as [ColumnValues]) as MaxColumnValue
 FROM (SELECT CONVERT(VARCHAR(12), [RecCreatedDt], 107) AS [ActivityDate]--[CategoryCreateDate]
        , 1 AS [CategoryCreated] 
        , 0 AS [CategoryAssigned]
        , 0 AS [TopicCreated]
        , 0 AS [TopicUpdated]
        , 0 AS [TopicRated]
        , 0 AS [DocumentAssigned] 
        , 0 AS [DocumentCreated]
        , 0 AS [DocumentAddBytes]
        , 0 AS [DocumentUpdated]
        , 0 AS [DocumentUpdateBytes]
        , 0 AS [DocumentRated]
      FROM [SQLRunbook].[tCategory]
      WHERE [RecCreatedDt] BETWEEN @EndDt - @DaysToShow AND @EndDt + 1
      UNION ALL      
      SELECT CONVERT(VARCHAR(12), RecCreatedDt, 107) AS [CategoryAssignDate]
       , 0 AS [CategoryCreated] 
       , 1 AS [CategoryAssigned]
       , 0 AS [TopicCreated]
       , 0 AS [TopicRated]
       , 0 AS [TopicUpdated]
       , 0 AS [DocumentAssigned] 
       , 0 AS [DocumentCreated]
       , 0 AS [DocumentAddBytes]
       , 0 AS [DocumentUpdated]
       , 0 AS [DocumentUpdateBytes]
       , 0 AS [DocumentRated]
      FROM [SQLRunbook].[tCategoryTopic] 
      WHERE [RecCreatedDt] BETWEEN @EndDt - @DaysToShow AND @EndDt + 1
      UNION ALL
      SELECT CONVERT(VARCHAR(12), [RecCreatedDt], 107) AS [TopicAddDate]
       , 0 AS [CategoryCreated] 
       , 0 AS [CategoryAssigned]
       , 1 AS [TopicCreated]
       , 0 AS [TopicUpdated]
       , 0 AS [TopicRated]
       , 0 AS [DocumentAssigned] 
       , 0 AS [DocumentCreated]
       , 0 AS [DocumentAddBytes]
       , 0 AS [DocumentUpdated]
       , 0 AS [DocumentUpdateBytes]
       , 0 AS [DocumentRated]
      FROM [SQLRunbook].[tTopic] 
      WHERE [RecCreatedDt] BETWEEN @EndDt - @DaysToShow AND @EndDt + 1
      UNION ALL
      SELECT CONVERT(VARCHAR(12), [LastUpdatedDt], 107) AS [TopicUpdateDate]
       , 0 AS [CategoryCreated] 
       , 0 AS [CategoryAssigned]
       , 0 AS [TopicCreated]
       , 1 AS [TopicUpdated]
       , 0 AS [TopicRated]
       , 0 AS [DocumentAssigned] 
       , 0 AS [DocumentCreated]
       , 0 AS [DocumentAddBytes]
       , 0 AS [DocumentUpdated]
       , 0 AS [DocumentUpdateBytes]
       , 0 AS [DocumentRated]
      FROM [SQLRunbook].[tTopic] 
      WHERE [LastUpdatedDt] BETWEEN @EndDt - @DaysToShow AND @EndDt + 1
      AND [RecCreatedDt] < [LastUpdatedDt]
      UNION ALL
      SELECT CONVERT(VARCHAR(12), [RecCreatedDt], 107) AS [TopicRateDate]
       , 0 AS [CategoryCreated] 
       , 0 AS [CategoryAssigned]
       , 0 AS [TopicCreated]
       , 0 AS [TopicUpdated]
       , 1 AS [TopicRated]
       , 0 AS [DocumentAssigned] 
       , 0 AS [DocumentCreated]
       , 0 AS [DocumentAddBytes]
       , 0 AS [DocumentUpdated]
       , 0 AS [DocumentUpdateBytes]
       , 0 AS [DocumentRated]
      FROM [SQLRunbook].[tTopicRating]
      WHERE [RecCreatedDt] BETWEEN @EndDt - @DaysToShow AND @EndDt + 1
      UNION ALL
      SELECT CONVERT(VARCHAR(12), [RecCreatedDt], 107) AS [DocumentAssignDate]
       , 0 AS [CategoryCreated] 
       , 0 AS [CategoryAssigned]
       , 0 AS [TopicCreated]
       , 0 AS [TopicUpdated]
       , 0 AS [TopicRated]
       , 1 AS [DocumentAssigned] 
       , 0 AS [DocumentCreated]
       , 0 AS [DocumentAddBytes]
       , 0 AS [DocumentUpdated]
       , 0 AS [DocumentUpdateBytes]
       , 0 AS [DocumentRated]
      FROM [SQLRunbook].[tTopicDocument]
      WHERE [RecCreatedDt] BETWEEN @EndDt - @DaysToShow AND @EndDt + 1
      UNION ALL
      SELECT CONVERT(VARCHAR(12), [RecCreatedDt], 107) AS [DocumentAddDate]
       , 0 AS [CategoryCreated] 
       , 0 AS [CategoryAssigned]
       , 0 AS [TopicCreated]
       , 0 AS [TopicUpdated]
       , 0 AS [TopicRated]
       , 0 AS [DocumentAssigned] 
       , 1 AS [DocumentCreated]
       , ISNULL(LEN([Document]),0) AS [DocumentAddBytes]
       , 0 AS [DocumentUpdated]
       , 0 AS [DocumentUpdateBytes]
       , 0 AS [DocumentRated]
      FROM [SQLRunbook].[tDocument]
      WHERE [RecCreatedDt] BETWEEN @EndDt - @DaysToShow AND @EndDt + 1
      UNION ALL
      SELECT CONVERT(VARCHAR(12), [LastUpdatedDt], 107) AS [DocumentUpdateDate]
       , 0 AS [CategoryCreated] 
       , 0 AS [CategoryAssigned]
       , 0 AS [TopicCreated]
       , 0 AS [TopicUpdated]
       , 0 AS [TopicRated]
       , 0 AS [DocumentAssigned] 
       , 0 AS [DocumentCreated]
       , 0 AS [DocumentAddBytes]
       , 1 AS [DocumentUpdated]
       , CASE WHEN [LastModifiedDt] between [LastUpdatedDt] - 1 and  [LastUpdatedDt]
              THEN ISNULL(LEN([Document]),0) 
              ELSE 0 END AS [DocumentUpdateBytes]
       , 0 AS [DocumentRated]
      FROM [SQLRunbook].[tDocument] 
      WHERE CONVERT(VARCHAR(12), [LastUpdatedDt], 107) BETWEEN @EndDt - @DaysToShow AND @EndDt 
      OR CONVERT(VARCHAR(12), [LastModifiedDt], 107) BETWEEN @EndDt - @DaysToShow AND @EndDt  
      UNION ALL
      SELECT CONVERT(VARCHAR(12), [RecCreatedDt], 107) AS [DocumentRateDate]
       , 0 AS [CategoryCreated] 
       , 0 AS [CategoryAssigned]
       , 0 AS [TopicCreated]
       , 0 AS [TopicUpdated]
       , 0 AS [TopicRated]
	   , 0 AS [DocumentAssigned] 
       , 0 AS [DocumentCreated]
       , 0 AS [DocumentAddBytes]
       , 0 AS [DocumentUpdated]
       , 0 AS [DocumentUpdateBytes]
       , 1 AS [DocumentRated]
      FROM [SQLRunbook].[tDocumentRating] 
      WHERE [LastUpdatedDt] BETWEEN @EndDt - @DaysToShow AND @EndDt + 1) derived
 GROUP BY derived.[ActivityDate]
 ORDER BY [ActivityDate];

END
GO     
