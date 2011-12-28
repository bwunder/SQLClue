/******************************************************************************
 Create the SQLClue Runbook data store.

 DO NOT run this script using SSMS, use the "Options|SQLClue Runbook|Install' 
 menu item to create the SQLClue Runbook data store

 The data store can be created in any database. SQLRunbook 
 is merely a recommended database name. The name can be changed in the
 installation's SQL connection dialog.  
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
SET NOCOUNT ON

GO

EXEC sp_changedbowner 'sa'

GO

BEGIN TRY
 DECLARE @SET_READ_COMMITED_SNAPSHOT [NVARCHAR](200), @rc [INT]
 SET @SET_READ_COMMITED_SNAPSHOT = 'ALTER DATABASE [' + DB_NAME() + '] SET READ_COMMITTED_SNAPSHOT ON;'
 EXEC @rc = sp_executesql @SET_READ_COMMITED_SNAPSHOT;
END TRY

BEGIN CATCH
  DECLARE @Msg [NVARCHAR](1024);  
  SET @Msg = ERROR_MESSAGE();
  RAISERROR('instSQLClueRunbook.sql failed at SET READ_COMITTED_SNAPSHOT. Error Message: %s @rc:%d',20,1, @Msg, @rc) WITH LOG 
END CATCH

GO

IF NOT EXISTS (SELECT * FROM [sys].[schemas] WHERE [name] = 'SQLRunbook')
 EXEC('CREATE SCHEMA [SQLRunbook] AUTHORIZATION [dbo]')

GO           
            
-- report server access and rate 
IF IS_MEMBER('SQLRunbookUserRole') IS NULL
 CREATE ROLE [SQLRunbookUserRole] AUTHORIZATION [db_owner];

GO 

-- add/modify/remove set relationships between topics, documents and categories
IF IS_MEMBER('SQLRunbookContributorRole') IS NULL
 CREATE ROLE [SQLRunbookContributorRole] AUTHORIZATION [dbo];

EXEC [sys].[sp_addrolemember] 'SQLRunbookUserRole', 'SQLRunbookContributorRole';

GO 

-- service role bypasses document ownership enforcement rules
IF IS_MEMBER('SQLRunbookServiceRole') IS NULL
 CREATE ROLE [SQLRunbookServiceRole] AUTHORIZATION [db_owner];

EXEC [sys].[sp_addrolemember] 'SQLRunbookContributorRole', 'SQLRunbookServiceRole';

GO

-- admin role - bypass all ownership enforcement rules
-- only admin role can change tSQLRunbookOptions and browse the errorlog
IF IS_MEMBER('SQLRunbookAdminRole') IS NULL
 CREATE ROLE [SQLRunbookAdminRole] AUTHORIZATION [db_owner];

EXEC [sys].[sp_addrolemember] 'SQLRunbookServiceRole', 'SQLRunbookAdminRole';

GO 

-- delete ability or category maintenance restricted to owners & admins
-- restricting access to category DDL and topic/document deletes - whatever that means

IF IS_MEMBER('SQLClueAdminRole') IS NULL
 CREATE ROLE [SQLClueAdminRole] AUTHORIZATION [db_owner];
EXEC [sys].[sp_addrolemember] 'SQLRunbookAdminRole', 'SQLClueAdminRole';
EXEC [sys].[sp_addrolemember] 'db_datareader', 'SQLClueAdminRole';

GO
-- enroll current user as uber admin 
DECLARE @Login [NVARCHAR](128)
SET @Login = ORIGINAL_LOGIN()
IF IS_MEMBER('Public')=0
 EXEC [sys].[sp_grantdbaccess] @Login
EXEC [sys].[sp_addrolemember] 'SQLClueAdminRole', @Login;

GO

DECLARE @CountBefore [INT]
SET @CountBefore = (SELECT COUNT(*) FROM [sys].[fulltext_document_types])

-- take all IFilter COM dlls the OS has to offer
EXEC [sp_fulltext_service] 'load_os_resources',1  
PRINT 'Fulltext Search access to locally registered iFilters added. (@action=''load_os_resources'' @value=1)' 

DECLARE @CountAfter [INT]
SET @CountAfter = (SELECT COUNT(*) FROM [sys].[fulltext_document_types])

If @CountAfter > @CountBefore 
 PRINT 'iFilters for ' + CAST(@CountAfter - @CountBefore AS CHAR(10)) + ' new document types added to Fulltext Search.' 

GO

If NOT EXISTS (SELECT * FROM [sys].[fulltext_catalogs] WHERE name = 'ftSQLRunbookCatalog')
 CREATE FULLTEXT CATALOG [ftSQLRunbookCatalog];

GO

EXEC [dbo].[sp_configure] 'show advanced options', 1;

GO

RECONFIGURE;

GO

PRINT 'RECONFIGUREd!'

GO

-- FREETEXT performance helper ???
EXEC [dbo].[sp_configure] 'precompute rank', 1;

GO

-- change noise words to * to avoid runtime error in CONTAINS
EXEC [dbo].[sp_configure] 'transform noise words', 1;

GO

RECONFIGURE;

GO

PRINT 'RECONFIGUREd!'

GO

IF OBJECT_ID('[SQLRunbook].[tOption]','U') IS NULL
 BEGIN

  CREATE TABLE [SQLRunbook].[tOption] (
     EnforceOwnership [BIT] NOT NULL
     CONSTRAINT dft_tOption__EnforceOwnership
     DEFAULT (1)
   , ScanForDocumentChanges [BIT]
     CONSTRAINT dft_tOption__ScanForDocumentChanges
     DEFAULT (1));

   INSERT [SQLRunbook].[tOption] 
    ([EnforceOwnership], [ScanForDocumentChanges])  
   VALUES
    (1,1);

  END;

GO

/*
   Rule is: Verify that the ORIGINAL_LOGON() is a known runbook user for all DML ops. If so then increment A counter. 
   If Not, then add user. How this will work with RS roles/RS security... depends on local configuration.   
*/

IF OBJECT_ID('[SQLRunbook].[tUser]','U') IS NULL
 BEGIN

  CREATE TABLE [SQLRunbook].[tUser] (
     [Id] [INT] IDENTITY (1, 1) NOT NULL 
  ,  [OriginalLogin] [NVARCHAR] (256)
     CONSTRAINT [dft_tUser_Name] 
     DEFAULT (ORIGINAL_LOGIN())
   , [FriendlyName] [NVARCHAR] (128) NOT NULL
   , [Notes] [NVARCHAR] (MAX) 
   , [CategoriesAdded] [INT]
   , [CategoriesUpdated] [INT]
   , [CategoriesDeleted] [INT]
   , [CategoriesRated] [INT]
   , [CategoriesReRated] [INT]
   , [CategoryTopicsAdded] [INT]
   , [CategoryTopicsDeleted] [INT]
   , [TopicsAdded] [INT]
   , [TopicsUpdated] [INT]
   , [TopicsDeleted] [INT]
   , [TopicsRated] [INT]
   , [TopicsReRated] [INT]
   , [TopicDocumentsAdded] [INT]
   , [TopicDocumentsDeleted] [INT]
   , [DocumentsAdded] [INT]
   , [DocumentsUpdated] [INT]
   , [DocumentsDeleted] [INT]
   , [DocumentsRated] [INT]
   , [DocumentsReRated] [INT]
   , [LastContributionDt] [DATETIME] NOT NULL  
     CONSTRAINT [dft_tUser__LastContributionDt] 
     DEFAULT (CURRENT_TIMESTAMP)
   , [RecCreatedDt] [DATETIME] NOT NULL  
     CONSTRAINT [dft_tUser__RecCreatedDt] 
     DEFAULT (CURRENT_TIMESTAMP)
   , [RecCreatedUser] [NVARCHAR] (128) NOT NULL 
     CONSTRAINT [dft_tUser_RecCreatedUser] 
     DEFAULT (ORIGINAL_LOGIN())
   , CONSTRAINT [pkc_tUser__Id] 
     PRIMARY KEY ([Id])
   , CONSTRAINT [ukn_tUser__OriginalLogin] 
     UNIQUE ([OriginalLogin]));

  CREATE NONCLUSTERED INDEX [ixn_tUser__FriendlyName]
  ON [SQLRunbook].[tUser] (FriendlyName);

  CREATE NONCLUSTERED INDEX [ixn_tUser__LastContributionDt]
  ON [SQLRunbook].[tUser] (LastContributionDt);

  CREATE NONCLUSTERED INDEX [ixn_tUser__RecCreatedDt]
  ON [SQLRunbook].[tUser] (RecCreatedDt);

  CREATE FULLTEXT INDEX ON [SQLRunbook].[tUser]
   ( [OriginalLogin] LANGUAGE 0X0, [FriendlyName] LANGUAGE 0X0, [Notes] LANGUAGE 0X0  )
  KEY INDEX [pkc_tUser__Id] ON [ftSQLRunbookCatalog]
  WITH CHANGE_TRACKING AUTO;                    

 END

GO

IF OBJECT_ID('[SQLRunbook].[tCategory]','U') IS NULL
 BEGIN
  CREATE TABLE [SQLRunbook].[tCategory] (
     [Id] [INT] IDENTITY (1, 1) NOT NULL 
   , [Name] [NVARCHAR] (128) NOT NULL
   , [Notes] [NVARCHAR] (MAX) 
   , [RecCreatedDt] [DATETIME] NOT NULL  
     CONSTRAINT [dft_tCategory__RecCreatedDt] 
     DEFAULT (CURRENT_TIMESTAMP)
   , [RecCreatedUser] [NVARCHAR] (128) NOT NULL 
     CONSTRAINT [dft_tCategory_RecCreatedUser] 
     DEFAULT (ORIGINAL_LOGIN())
   , [LastUpdatedDt] [DATETIME] NOT NULL  
     CONSTRAINT [dft_tCategory__LastUpdatedDt] 
     DEFAULT (CURRENT_TIMESTAMP)
   , [LastUpdatedUser] [NVARCHAR] (128) NOT NULL 
     CONSTRAINT [dft_tCategory_LastUpdatedUser] 
     DEFAULT (ORIGINAL_LOGIN())
   , CONSTRAINT [pkc_tCategory__Id] 
     PRIMARY KEY ([Id])
   , CONSTRAINT [ukn_tCategory__Name]
     UNIQUE CLUSTERED([Name]));

  CREATE NONCLUSTERED INDEX [ixn_tCategory__RecCreatedDt]
  ON [SQLRunbook].[tCategory] (RecCreatedDt);

  CREATE NONCLUSTERED INDEX [ixn_tCategory__RecCreatedUser]
  ON [SQLRunbook].[tCategory] (RecCreatedUser);
  
  CREATE NONCLUSTERED INDEX [ixn_tCategory__LastUpdatedDt]
  ON [SQLRunbook].[tCategory] (LastUpdatedDt);

  CREATE NONCLUSTERED INDEX [ixn_tCategory__LastUpdatedUser]
  ON [SQLRunbook].[tCategory] (LastUpdatedUser);
  
  CREATE FULLTEXT INDEX ON [SQLRunbook].[tCategory]
   ( [Name] LANGUAGE 0X0, [Notes] LANGUAGE 0X0 )
  KEY INDEX [pkc_tCategory__Id] ON [ftSQLRunbookCatalog]
  WITH CHANGE_TRACKING AUTO;                    

 END

GO

-- no identity here, want to maintain a 1 based sequence
-- not useful if there are more than 10 rows in table. 6 is better than 10
-- hard to identify granularity necessitates personal interpretations  
-- fere the meaning of each rating needs to be unquestionably clear 
IF OBJECT_ID('[SQLRunbook].[tRating]','U') IS NULL
 BEGIN

  CREATE TABLE [SQLRunbook].[tRating] (
     [Id] [INT] NOT NULL 
   , [Name] [NVARCHAR] (128) NOT NULL
   , [Notes] [NVARCHAR] (MAX) 
   , [RecCreatedDt] [DATETIME] NOT NULL  
     CONSTRAINT [dft_tRating__RecCreatedDt] 
     DEFAULT (CURRENT_TIMESTAMP)
   , [RecCreatedUser] [NVARCHAR] (128) NOT NULL 
     CONSTRAINT [dft_tRating_RecCreatedUser] 
     DEFAULT (ORIGINAL_LOGIN())
   , CONSTRAINT ck_tRating__Id 
     CHECK (ID < 10)  
   , CONSTRAINT [pkn_tRating__Id] 
     PRIMARY KEY NONCLUSTERED ([Id])
   , CONSTRAINT [ukn_tRating__Name]
     UNIQUE ([Name]));

   CREATE NONCLUSTERED INDEX [ixn_tRating__RecCreatedUser]
   ON [SQLRunbook].[tRating] (RecCreatedUser);

   CREATE NONCLUSTERED INDEX [ixn_tRating__RecCreatedDt]
   ON [SQLRunbook].[tRating] (RecCreatedDt);

  CREATE FULLTEXT INDEX ON [SQLRunbook].[tRating]
   ( [Notes] LANGUAGE 0X0 )
  KEY INDEX [pkn_tRating__Id] ON [ftSQLRunbookCatalog]
  WITH CHANGE_TRACKING AUTO;                    

 END

GO

--  , [Category] [NVARCHAR] (128) NOT NULL
IF OBJECT_ID('[SQLRunbook].[tTopic]','U') IS NULL
 BEGIN
  CREATE TABLE [SQLRunbook].[tTopic] (
     [Id] [INT] IDENTITY (1, 1) NOT NULL 
   , [Name] [NVARCHAR] (128) NOT NULL
   , [Notes] [NVARCHAR] (MAX) NULL
   , [Owner] [NVARCHAR] (128) NOT NULL 
     CONSTRAINT [dft_tTopic_Owner] 
     DEFAULT (ORIGINAL_LOGIN())
   , [RecCreatedDt] [DATETIME] NOT NULL  
     CONSTRAINT [dft_tTopic__RecCreatedDt] 
     DEFAULT (CURRENT_TIMESTAMP)
   , [RecCreatedUser] [NVARCHAR] (128) NOT NULL 
     CONSTRAINT [dft_tTopic_RecCreatedUser] 
     DEFAULT (ORIGINAL_LOGIN())
   , [LastUpdatedDt] [DATETIME] NOT NULL  
     CONSTRAINT [dft_tTopic__LastUpdatedDt] 
     DEFAULT (CURRENT_TIMESTAMP)
   , [LastUpdatedUser] [NVARCHAR] (128) NOT NULL 
     CONSTRAINT [dft_tTopic_LastUpdatedUser] 
     DEFAULT (ORIGINAL_LOGIN())
   , CONSTRAINT [qkn_tTopic__Name]
     UNIQUE ([Name]) 
   , CONSTRAINT [pkc_tTopic__Id] 
     PRIMARY KEY ([Id]));

  CREATE NONCLUSTERED INDEX [ixn_tTopic__Owner]
  ON [SQLRunbook].[tTopic] (Owner);

  CREATE NONCLUSTERED INDEX [ixn_tTopic__RecCreatedDt]
  ON [SQLRunbook].[tTopic] (RecCreatedDt);

  CREATE NONCLUSTERED INDEX [ixn_tTopic__RecCreatedUser]
  ON [SQLRunbook].[tTopic] (RecCreatedUser);
 
  CREATE FULLTEXT INDEX ON [SQLRunbook].[tTopic]
   ( [Name] LANGUAGE 0X0, [Notes] LANGUAGE 0X0 )
  KEY INDEX [pkc_tTopic__Id] ON [ftSQLRunbookCatalog]
  WITH CHANGE_TRACKING AUTO;                    

 END

GO

IF OBJECT_ID('[SQLRunbook].[tCategoryTopic]','U') IS NULL

 BEGIN

  CREATE TABLE [SQLRunbook].[tCategoryTopic] (
     [CategoryId] [INT] NOT NULL 
   , [TopicId] [INT] NOT NULL
   , [RecCreatedDt] [DATETIME] NOT NULL  
     CONSTRAINT [dft_tCategoryTopic__RecCreatedDt] 
     DEFAULT (CURRENT_TIMESTAMP)
   , [RecCreatedUser] [NVARCHAR] (128) NOT NULL 
     CONSTRAINT [dft_tCategoryTopic_RecCreatedUser] 
     DEFAULT (ORIGINAL_LOGIN())
   , CONSTRAINT [pkc_tCategoryTopic__CategoryId__TopicId]
     PRIMARY KEY ([CategoryId], [TopicId])
   , CONSTRAINT fk_tCategoryTopic__CategoryId__TO__tCategory__Id 
     FOREIGN KEY ([CategoryId])
     REFERENCES [SQLRunbook].[tCategory]([Id]) 
   , CONSTRAINT fk_tCategoryTopic__TopicId__TO__tTopic__Id 
     FOREIGN KEY ([TopicId])
     REFERENCES [SQLRunbook].[tTopic]([Id]));

  CREATE NONCLUSTERED INDEX [ixn_tCategoryTopic__TopicId]
  ON [SQLRunbook].[tCategoryTopic] (TopicId);

  CREATE NONCLUSTERED INDEX [ixn_tCategoryTopic__RecCreatedDt]
  ON [SQLRunbook].[tCategoryTopic] (RecCreatedDt);

  CREATE NONCLUSTERED INDEX [ixn_tCategoryTopic__RecCreatedUser]
  ON [SQLRunbook].[tCategoryTopic] (RecCreatedUser);  

 END

GO

-- only DocumentTypes in sys.fulltext_document_types will be indexed
IF OBJECT_ID('[SQLRunbook].[tDocument]','U') IS NULL
 
 BEGIN

  CREATE TABLE [SQLRunbook].[tDocument] (
     [Id] [INT] IDENTITY (1, 1) NOT NULL 
   , [File] [NVARCHAR] (450) NULL
   , [Document] [VARBINARY] (MAX) NULL
   , [DocumentType] [NVARCHAR] (8) NULL
   , [LastModifiedDt] [DATETIME] NULL
   , [Owner] [NVARCHAR] (128) NOT NULL 
     CONSTRAINT [dft_tDocument_Owner] 
     DEFAULT (ORIGINAL_LOGIN())
   , [WatchFileForChange] [BIT] NOT NULL
     CONSTRAINT [dft_tDocument_WatchFileForChange] 
     DEFAULT (1)
   , [RecCreatedDt] [DATETIME] NOT NULL  
     CONSTRAINT [dft_tDocument__RecCreatedDt] 
     DEFAULT (CURRENT_TIMESTAMP)
   , [RecCreatedUser] [NVARCHAR] (128) NOT NULL 
     CONSTRAINT [dft_tDocument_RecCreatedUser] 
     DEFAULT (ORIGINAL_LOGIN())
   , [LastUpdatedDt] [DATETIME] NOT NULL  
     CONSTRAINT [dft_tDocument__LastUpdatedDt] 
     DEFAULT (CURRENT_TIMESTAMP)
   , [LastUpdatedUser] [NVARCHAR] (128) NOT NULL 
     CONSTRAINT [dft_tDocument_LastUpdatedUser] 
     DEFAULT (ORIGINAL_LOGIN())
   , CONSTRAINT [qkn_tDocument__File] 
     UNIQUE ([File])
   , CONSTRAINT [pkc_tDocument__Id] 
     PRIMARY KEY NONCLUSTERED ([Id]));

  CREATE NONCLUSTERED INDEX [ixn_tDocument__Owner]
  ON [SQLRunbook].[tDocument] ([Owner]);

  CREATE NONCLUSTERED INDEX [ixn_tDocument__LastModifiedDt]
  ON [SQLRunbook].[tDocument] ([LastModifiedDt]);

  CREATE CLUSTERED INDEX [ixn_tDocument__RecCreatedDt]
  ON [SQLRunbook].[tDocument] ([RecCreatedDt]);

  CREATE NONCLUSTERED INDEX [ixn_tDocument__RecCreatedUser]
  ON [SQLRunbook].[tDocument] ([RecCreatedUser]);

  CREATE NONCLUSTERED INDEX [ixn_tDocument__LastUpdatedDt]
  ON [SQLRunbook].[tDocument] ([LastUpdatedDt]);

  CREATE NONCLUSTERED INDEX [ixn_tDocument__LastUpdatedUser]
  ON [SQLRunbook].[tDocument] ([LastUpdatedUser]);

  CREATE FULLTEXT INDEX ON [SQLRunbook].[tDocument]
   ([File] LANGUAGE 0X0, [Document] TYPE COLUMN [DocumentType] LANGUAGE 0X0 )
  KEY INDEX [pkc_tDocument__Id] ON [ftSQLRunbookCatalog]
  WITH CHANGE_TRACKING AUTO;                    

 END

GO

IF OBJECT_ID('[SQLRunbook].[tTopicDocument]','U') IS NULL

 BEGIN

  CREATE TABLE [SQLRunbook].[tTopicDocument] (
     [TopicId] [INT] NOT NULL 
   , [DocumentId] [INT] NOT NULL
   , [RecCreatedDt] [DATETIME] NOT NULL  
     CONSTRAINT [dft_tTopicDocument__RecCreatedDt] 
     DEFAULT (CURRENT_TIMESTAMP)
   , [RecCreatedUser] [NVARCHAR] (128) NOT NULL 
     CONSTRAINT [dft_tTopicDocument_RecCreatedUser] 
     DEFAULT (ORIGINAL_LOGIN())
   , CONSTRAINT [pkc_tTopicDocument__TopicId__DocumentId]
     PRIMARY KEY ([TopicId], [DocumentId])
   , CONSTRAINT fk_tTopicDocument__DocumentId__TO__tDocument__Id 
     FOREIGN KEY ([DocumentId])
     REFERENCES [SQLRunbook].[tDocument]([Id])
   , CONSTRAINT fk_tTopicDocument__TopicId__TO__tTopic__Id 
     FOREIGN KEY ([TopicId])
     REFERENCES [SQLRunbook].[tTopic]([Id]));

  CREATE NONCLUSTERED INDEX [ixn_tTopicDocument__DocumentId]
  ON [SQLRunbook].[tTopicDocument] (DocumentId);

  CREATE NONCLUSTERED INDEX [ixn_tTopicDocument__RecCreatedDt]
  ON [SQLRunbook].[tTopicDocument] (RecCreatedDt);

  CREATE NONCLUSTERED INDEX [ixn_tTopicDocument__RecCreatedUser]
  ON [SQLRunbook].[tTopicDocument] (RecCreatedUser);  

 END

GO

IF OBJECT_ID('[SQLRunbook].[tCategoryRating]','U') IS NULL

 BEGIN

  CREATE TABLE [SQLRunbook].[tCategoryRating] (
     [Id] [INT] IDENTITY(1,1) NOT NULL
   , [CategoryId] [INT] NOT NULL 
   , [RatingId] [INT] NOT NULL
   , [Notes] [NVARCHAR] (MAX)
   , [RecCreatedDt] [DATETIME] NOT NULL  
     CONSTRAINT [dft_tCategoryRating__RecCreatedDt] 
     DEFAULT (CURRENT_TIMESTAMP)
   , [RecCreatedUser] [NVARCHAR] (128) NOT NULL 
     CONSTRAINT [dft_tCategoryRating_RecCreatedUser] 
     DEFAULT (ORIGINAL_LOGIN())
   , [LastUpdatedDt] [DATETIME] NOT NULL  
     CONSTRAINT [dft_tCategoryRating__LastUpdatedDt] 
     DEFAULT (CURRENT_TIMESTAMP)
   , [LastUpdatedUser] [NVARCHAR] (128) NOT NULL 
     CONSTRAINT [dft_tCategoryRating_LastUpdatedUser] 
     DEFAULT (ORIGINAL_LOGIN())
   , CONSTRAINT [pkn_tCategoryRating__Id]
     PRIMARY KEY NONCLUSTERED ([Id])
   , CONSTRAINT [ukc_tCategoryRating__CategoryId__RecCreatedUser]
     UNIQUE CLUSTERED ([CategoryId], [RecCreatedUser])
   , CONSTRAINT fk_tCategoryRating__RatingId__TO__tRating__Id 
     FOREIGN KEY ([RatingId])
     REFERENCES [SQLRunbook].[tRating]([Id])
   , CONSTRAINT fk_tCategoryRating__CategoryId__TO__tCategory__Id 
     FOREIGN KEY ([CategoryId])
     REFERENCES [SQLRunbook].[tCategory]([Id])
     ON DELETE CASCADE); 

  CREATE NONCLUSTERED INDEX [ixn_tCategoryRating__RecCreatedDt]
  ON [SQLRunbook].[tCategoryRating] (RecCreatedDt);

  CREATE NONCLUSTERED INDEX [ixn_tCategoryRating__RecCreatedUser]
  ON [SQLRunbook].[tCategoryRating] (RecCreatedUser);  

  CREATE FULLTEXT INDEX ON [SQLRunbook].[tCategoryRating]
   ( [Notes] LANGUAGE 0X0 )
  KEY INDEX [pkn_tCategoryRating__Id] ON [ftSQLRunbookCatalog]
  WITH CHANGE_TRACKING AUTO;                    

 END

GO

IF OBJECT_ID('[SQLRunbook].[tTopicRating]','U') IS NULL
 
 BEGIN

  CREATE TABLE [SQLRunbook].[tTopicRating] (
     [Id] [INT] IDENTITY(1,1) NOT NULL
   , [TopicId] [INT] NOT NULL 
   , [RatingId] [INT] NOT NULL
   , [Notes] [NVARCHAR] (MAX)
   , [RecCreatedDt] [DATETIME] NOT NULL  
     CONSTRAINT [dft_tTopicRating__RecCreatedDt] 
     DEFAULT (CURRENT_TIMESTAMP)
   , [RecCreatedUser] [NVARCHAR] (128) NOT NULL 
     CONSTRAINT [dft_tTopicRating_RecCreatedUser] 
     DEFAULT (ORIGINAL_LOGIN())
   , [LastUpdatedDt] [DATETIME] NOT NULL  
     CONSTRAINT [dft_tTopicRating__LastUpdatedDt] 
     DEFAULT (CURRENT_TIMESTAMP)
   , [LastUpdatedUser] [NVARCHAR] (128) NOT NULL 
     CONSTRAINT [dft_tTopicRating_LastUpdatedUser] 
     DEFAULT (ORIGINAL_LOGIN())
   , CONSTRAINT [pkn_tTopicRating__Id]
     PRIMARY KEY NONCLUSTERED ([Id])
   , CONSTRAINT [ukc_tTopicRating__TopicId__RecCreatedUser]
     UNIQUE CLUSTERED ([TopicId], [RecCreatedUser])
   , CONSTRAINT fk_tTopicRating__RatingId__TO__tRating__Id 
     FOREIGN KEY ([RatingId])
     REFERENCES [SQLRunbook].[tRating]([Id])
   , CONSTRAINT fk_tTopicRating__TopicId__TO__tTopic__Id 
     FOREIGN KEY ([TopicId])
     REFERENCES [SQLRunbook].[tTopic]([Id])
     ON DELETE CASCADE);

  CREATE NONCLUSTERED INDEX [ixn_tTopicRating__RecCreatedDt]
  ON [SQLRunbook].[tTopicRating] (RecCreatedDt);

  CREATE NONCLUSTERED INDEX [ixn_tTopicRating__RecCreatedUser]
  ON [SQLRunbook].[tTopicRating] (RecCreatedUser);  

  CREATE FULLTEXT INDEX ON [SQLRunbook].[tTopicRating]
   ( [Notes] LANGUAGE 0X0 )
  KEY INDEX [pkn_tTopicRating__Id] ON [ftSQLRunbookCatalog]
  WITH CHANGE_TRACKING AUTO;                    

 END

GO

IF OBJECT_ID('[SQLRunbook].[tDocumentRating]','U') IS NULL
 
 BEGIN

  CREATE TABLE [SQLRunbook].[tDocumentRating] (
     [Id] [INT] IDENTITY(1,1) NOT NULL
   , [DocumentId] [INT] NOT NULL 
   , [RatingId] [INT] NOT NULL
   , [Notes] [NVARCHAR] (MAX)
   , [RecCreatedDt] [DATETIME] NOT NULL  
     CONSTRAINT [dft_tDocumentRating__RecCreatedDt] 
     DEFAULT (CURRENT_TIMESTAMP)
   , [RecCreatedUser] [NVARCHAR] (128) NOT NULL 
     CONSTRAINT [dft_tDocumentRating_RecCreatedUser] 
     DEFAULT (ORIGINAL_LOGIN())
   , [LastUpdatedDt] [DATETIME] NOT NULL  
     CONSTRAINT [dft_tDocumentRating__LastUpdatedDt] 
     DEFAULT (CURRENT_TIMESTAMP)
   , [LastUpdatedUser] [NVARCHAR] (128) NOT NULL 
     CONSTRAINT [dft_tDocumentRating_LastUpdatedUser] 
     DEFAULT (ORIGINAL_LOGIN())
   , CONSTRAINT [pkn_tDocumentRating__Id]
     PRIMARY KEY NONCLUSTERED ([Id])
   , CONSTRAINT [ukc_tDocumentRating__DocumentId__RecCreatedUser]
     UNIQUE CLUSTERED ([DocumentId], [RecCreatedUser])
   , CONSTRAINT fk_tDocumentRating__RatingId__TO__tRating__Id 
     FOREIGN KEY ([RatingId])
     REFERENCES [SQLRunbook].[tRating]([Id])
   , CONSTRAINT fk_tDocumentRating__DocumentId__TO__tDocument__Id 
     FOREIGN KEY ([DocumentId])
     REFERENCES [SQLRunbook].[tDocument]([Id])
     ON DELETE CASCADE); 

  CREATE NONCLUSTERED INDEX [ixn_tDocumentRating__RecCreatedDt]
  ON [SQLRunbook].[tDocumentRating] (RecCreatedDt);

  CREATE NONCLUSTERED INDEX [ixn_tDocumentRating__RecCreatedUser]
  ON [SQLRunbook].[tDocumentRating] (RecCreatedUser);  

  CREATE FULLTEXT INDEX ON [SQLRunbook].[tDocumentRating]
   ( [Notes] LANGUAGE 0X0 )
  KEY INDEX [pkn_tDocumentRating__Id] ON [ftSQLRunbookCatalog]
  WITH CHANGE_TRACKING AUTO;                    

 END

GO

IF OBJECT_ID('[SQLRunbook].[tSQLErrorLog]','U') IS NULL
 CREATE TABLE [SQLRunbook].[tSQLErrorLog] (
    [Id] [INT] IDENTITY (1, 1) NOT NULL 
  , [UserName] [NVARCHAR] (256) 
  , [DBName] [NVARCHAR] (128) 
  , [ErrorNumber] [INT]
  , [ErrorSeverity] [INT]
  , [ErrorState] [INT] 
  , [ErrorProcedure] [NVARCHAR] (126)
  , [ErrorLine] [INT]
  , [ErrorMessage] [NVARCHAR] (2048)
  , [TextData] [NVARCHAR] (MAX) 
  , [Notes] [NVARCHAR] (MAX) NULL  
  , [RecCreatedDt] [DATETIME] NOT NULL 
    CONSTRAINT [dft_tSQLErrorLog__RecCreatedDt] 
    DEFAULT (CURRENT_TIMESTAMP)
  , CONSTRAINT [pkn_tSQLErrorLog__Id] 
    PRIMARY KEY ([Id]) );

GO 

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLRunbook'
           AND SPECIFIC_NAME = N'fnIsAdmin' )
 DROP FUNCTION [SQLRunbook].[fnIsAdmin];

GO
/******************************************************************************
**	Auth: Bill Wunder
**	Date: April 1, 2008
**
**	Desc: determine if current user is any any flavor of SQLClue admin in the 
**        runbook database. if so, can o'ride row level security restriction 
**
*******************************************************************************
**	Change History
*******************************************************************************
**	Date		   Author		     Description of Change
**	          
*******************************************************************************/
CREATE FUNCTION [SQLRunbook].[fnIsAdmin] ( )
RETURNS [BIT]
WITH EXECUTE AS CALLER
AS
BEGIN
   RETURN(SELECT IS_MEMBER('SQLRunbookAdminRole')|
                 IS_MEMBER('SQLClueAdminRole')|
                 CASE USER_NAME() WHEN 'dbo' THEN 1 ELSE 0 END)
END

GO
  
IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLRunbook'
           AND SPECIFIC_NAME = N'pLogSQLError' )
 DROP PROCEDURE [SQLRunbook].[pLogSQLError];

GO
/******************************************************************************
**	Auth: Bill Wunder
**	Date: April 1, 2007
**
**	Desc: log database errors 
**
*******************************************************************************
**	Change History
*******************************************************************************
**	Date		   Author		     Description of Change
**	          
*******************************************************************************/
CREATE PROCEDURE [SQLRunbook].[pLogSQLError] 
 ( @ParmString [NVARCHAR] (MAX)
 , @ObjectId [INT])
AS
BEGIN
 DECLARE @ObjectName [NVARCHAR] (128)
  , @TextData [NVARCHAR] (MAX)
  , @OriginalLogin [NVARCHAR](256)
  , @ErrorNumber [INT]
  , @ErrorSeverity [INT]
  , @ErrorState [INT]
  , @ErrorProcedure [NVARCHAR] (128)
  , @ErrorLine [INT]
  , @ErrorMessage [NVARCHAR] (2048)
  , @RollBackMsg [NVARCHAR] (256)
  , @SQLErrorLogId [INT];
  
 SET NOCOUNT ON;

 BEGIN TRY
  -- Return if there is no error information to log.
  IF ERROR_NUMBER() IS NULL
   RETURN;

  SET @OriginalLogin = ORIGINAL_LOGIN();
  SET @ErrorNumber = ERROR_NUMBER();
  SET @ErrorSeverity = ERROR_SEVERITY();
  SET @ErrorState = ERROR_STATE();
  SET @ErrorProcedure = ERROR_PROCEDURE();
  SET @ErrorLine = ERROR_LINE();
  SET @ErrorMessage = ERROR_MESSAGE();
  -- IF Output is 0 then nothing happened here
  SET @TextData = ISNULL('[' + OBJECT_SCHEMA_NAME(@ObjectId) + '].', '') 
                + ISNULL('[' + OBJECT_NAME(@ObjectId) + ']', 'Query:') 
                + CHAR(13) + CHAR(10) 
                + ISNULL(@ParmString,'');

  SET @ObjectName = OBJECT_NAME(@ObjectId)

  -- rollback open transaction(s) then log the error
  WHILE @@TRANCOUNT > 0 
   ROLLBACK TRAN;

  INSERT [SQLRunbook].[tSQLErrorLog] 
   ( [UserName] 
   , [ErrorNumber] 
   , [ErrorSeverity]
   , [ErrorState]
   , [ErrorProcedure] 
   , [ErrorLine]
   , [ErrorMessage]
   , [TextData] ) 
  VALUES 
   ( @OriginalLogin
   , @ErrorNumber
   , @ErrorSeverity
   , @ErrorState
   , @ErrorProcedure
   , @ErrorLine
   , @ErrorMessage 
   , @TextData );

  -- fetch the value of the row just inserted
  SELECT @SQLErrorLogID = SCOPE_IDENTITY();
 END TRY

 BEGIN CATCH
  DECLARE @logmsg NVARCHAR(MAX)
   , @Number [INT]
   , @Severity [INT]
   , @State [INT]
   , @Procedure [NVARCHAR] (256)
   , @Line [INT]
   , @Message [NVARCHAR] (2048); 
  SET @Number = ERROR_NUMBER();
  SET @Severity = ERROR_SEVERITY();
  SET @State = ERROR_STATE();
  SET @Procedure = ERROR_PROCEDURE();
  SET @Line = ERROR_LINE();
  SET @Message = ERROR_MESSAGE(); 
  SET @logmsg = 'SQLClue Runbook exception while handling error.' + CHAR(13) + CHAR(10) +
                'Originating error:' + CHAR(13) + CHAR(10) +
                CHAR(9) + 'Original Login  : %s' + CHAR(13) + CHAR(10) +
                CHAR(9) + 'Error Number    : %d' + CHAR(13) + CHAR(10) +
                CHAR(9) + 'Error Severity  : %d' + CHAR(13) + CHAR(10) +
                CHAR(9) + 'Error State     : %d' + CHAR(13) + CHAR(10) +
                CHAR(9) + 'Error Procedure : %s' + CHAR(13) + CHAR(10) +
                CHAR(9) + 'Error Line      : %d' + CHAR(13) + CHAR(10) +
                CHAR(9) + 'Error Message   : %s' + CHAR(13) + CHAR(10) +
                CHAR(9) + 'Call            : %s' + CHAR(13) + CHAR(10) +
                'Handler exception:' + CHAR(13) + CHAR(10) +
                CHAR(9) + 'Error Number    : %d' + CHAR(13) + CHAR(10) +
                CHAR(9) + 'Error Severity  : %d' + CHAR(13) + CHAR(10) +
                CHAR(9) + 'Error State     : %d' + CHAR(13) + CHAR(10) +
                CHAR(9) + 'Error Procedure : %s' + CHAR(13) + CHAR(10) +
                CHAR(9) + 'Error Line      : %d' + CHAR(13) + CHAR(10) +
                CHAR(9) + 'Error Message   : %s'
  RAISERROR(@logmsg
           , 19
           , 1
           , @OriginalLogin
           , @ErrorNumber
           , @ErrorSeverity
           , @ErrorState
           , @ErrorProcedure
           , @ErrorLine
           , @ErrorMessage 
           , @TextData 
           , @Number
           , @Severity
           , @State
           , @Procedure
           , @Line
           , @Message) 
           WITH LOG;
  RETURN -1;
 END CATCH

 -- report an error to the caller but limit details revealed
 RAISERROR ('Database operation failed. See SQLClue Runbook Error Log for additional details. [%d]', 16,1, @SQLErrorLogId);         

END; 

GO

GRANT EXECUTE ON [SQLRunbook].[pLogSQLError] TO [SQLRunbookUserRole];

GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLRunbook'
           AND SPECIFIC_NAME = N'pUserUpsert' )
 DROP PROCEDURE [SQLRunbook].[pUserUpsert];

GO
/******************************************************************************
**	Auth: Bill Wunder
**	Date: April 1, 2007
**
**	Desc: insert else update current Runbook User when a change is made
**        called by all triggers
**
*******************************************************************************
**	Change History
*******************************************************************************
**	Date		   Author		     Description of Change
**	          
*******************************************************************************/
CREATE PROCEDURE [SQLRunbook].[pUserUpsert]  
 (   @OriginalLogin [NVARCHAR] (256)
   , @FriendlyName [NVARCHAR] (128) = null
   , @Notes [NVARCHAR] (MAX) = ''
   , @CategoriesAdded [INT] = 0
   , @CategoriesUpdated [INT] = 0
   , @CategoriesDeleted [INT] = 0
   , @CategoriesRated [INT] = 0
   , @CategoriesReRated [INT] = 0
   , @CategoryTopicsAdded [INT] = 0
   , @CategoryTopicsDeleted [INT] = 0
   , @TopicsAdded [INT] = 0
   , @TopicsUpdated [INT] = 0
   , @TopicsDeleted [INT] = 0
   , @TopicsRated [INT] = 0
   , @TopicsReRated [INT] = 0
   , @TopicDocumentsAdded [INT] = 0
   , @TopicDocumentsDeleted [INT] = 0
   , @DocumentsAdded [INT] = 0
   , @DocumentsUpdated [INT] = 0
   , @DocumentsDeleted [INT] = 0
   , @DocumentsRated [INT] = 0 
   , @DocumentsReRated [INT] = 0 )
AS
BEGIN
 
 BEGIN TRY
  -- no need for explicit txn
  -- will only ever be one op
  -- pretty sure it will always only ever be incremental (+1)
  -- Friendly name can only be set during update 
  SET @Notes = @Notes + CAST(CURRENT_TIMESTAMP AS [NVARCHAR](30)) + ' : ' + ORIGINAL_LOGIN()
             + CASE WHEN @CategoriesAdded > 0 
                    THEN CAST(@CategoriesAdded AS [NVARCHAR](10)) + ' Category(s) added' + CHAR(13) + CHAR(10)
                    ELSE '' END
             + CASE WHEN @CategoriesUpdated > 0 
                    THEN CAST(@CategoriesUpdated AS [NVARCHAR](10)) + ' Category(s) update' + CHAR(13) + CHAR(10)
                    ELSE '' END
             + CASE WHEN @CategoriesDeleted > 0 
                    THEN CAST(@CategoriesDeleted AS [NVARCHAR](10)) + ' Category(s) removed' + CHAR(13) + CHAR(10)
                    ELSE '' END
             + CASE WHEN @CategoriesRated > 0 
                    THEN CAST(@CategoriesRated AS [NVARCHAR](10)) + ' Category(s) rated' + CHAR(13) + CHAR(10)
                    ELSE '' END
             + CASE WHEN @CategoriesReRated > 0 
                    THEN CAST(@CategoriesReRated AS [NVARCHAR](10)) + ' Category(s) re-rated' + CHAR(13) + CHAR(10)
                    ELSE '' END
             + CASE WHEN @CategoryTopicsAdded > 0 
                    THEN CAST(@CategoryTopicsAdded AS [NVARCHAR](10)) + ' Category Topic assignment(s) added' + CHAR(13) + CHAR(10)
                    ELSE '' END
             + CASE WHEN @CategoryTopicsDeleted > 0 
                    THEN CAST(@CategoryTopicsDeleted AS [NVARCHAR](10)) + ' Category Topic assignment(s) removed' + CHAR(13) + CHAR(10)
                    ELSE '' END
             + CASE WHEN @TopicsAdded > 0 
                    THEN CAST(@TopicsAdded AS [NVARCHAR](10)) + ' Topic(s) added' + CHAR(13) + CHAR(10)
                    ELSE '' END
             + CASE WHEN @TopicsUpdated > 0 
                    THEN CAST(@TopicsUpdated AS [NVARCHAR](10)) + ' Topic(s) updated' + CHAR(13) + CHAR(10) 
                    ELSE '' END
             + CASE WHEN @TopicsDeleted > 0 
                    THEN CAST(@TopicsDeleted AS [NVARCHAR](10)) + ' Topic(s) removed' + CHAR(13) + CHAR(10) 
                    ELSE '' END
             + CASE WHEN @TopicsRated > 0 
                    THEN CAST(@TopicsRated AS [NVARCHAR](10)) + ' Topic(s) rated' + CHAR(13) + CHAR(10) 
                    ELSE '' END
             + CASE WHEN @TopicsReRated > 0 
                    THEN CAST(@TopicsReRated AS [NVARCHAR](10)) + ' Topic(s) re-rated' + CHAR(13) + CHAR(10) 
                    ELSE '' END
             + CASE WHEN @TopicDocumentsAdded > 0 
                    THEN CAST(@TopicDocumentsAdded AS [NVARCHAR](10)) + ' Topic Document assignment(s) added' + CHAR(13) + CHAR(10) 
                    ELSE '' END
             + CASE WHEN @TopicDocumentsDeleted > 0 
                    THEN CAST(@TopicDocumentsDeleted AS [NVARCHAR](10)) + ' Topic Document assignment(s) removed' + CHAR(13) + CHAR(10) 
                    ELSE '' END
             + CASE WHEN @DocumentsAdded > 0 
                    THEN CAST(@DocumentsAdded AS [NVARCHAR](10)) + ' Document(s) added' + CHAR(13) + CHAR(10) 
                    ELSE '' END
             + CASE WHEN @DocumentsUpdated > 0 
                    THEN CAST(@DocumentsUpdated AS [NVARCHAR](10)) + ' Document(s) updated' + CHAR(13) + CHAR(10) 
                    ELSE '' END
             + CASE WHEN @DocumentsDeleted > 0 
                    THEN CAST(@DocumentsDeleted AS [NVARCHAR](10)) + ' Document(s) removed' + CHAR(13) + CHAR(10) 
                    ELSE '' END
             + CASE WHEN @DocumentsRated > 0 
                    THEN CAST(@DocumentsRated AS [NVARCHAR](10)) + ' Document(s) rated' + CHAR(13) + CHAR(10) 
                    ELSE '' END
             + CASE WHEN @DocumentsReRated > 0 
                    THEN CAST(@DocumentsReRated AS [NVARCHAR](10)) + ' Document(s) re-rated' + CHAR(13) + CHAR(10) 
                    ELSE '' END
                    
  UPDATE  [SQLRunbook].[tUser]
   SET   [FriendlyName] = COALESCE(@FriendlyName,[FriendlyName],'')
       , [Notes] = ISNULL([Notes],'') 
                 + @Notes
                 + CASE WHEN @FriendlyName <> [FriendlyName] 
                        THEN CAST(CURRENT_TIMESTAMP AS [NVARCHAR](30)) + ' : ' + ORIGINAL_LOGIN() 
                             + CHAR(13) + CHAR(10) 
                             + '[' + @FriendlyName + '] set as FriendlyName'  
                             + CHAR(13) + CHAR(10) 
                        ELSE '' END
       , [CategoriesAdded] = ISNULL([CategoriesAdded],0) + ISNULL(@CategoriesAdded,0)
       , [CategoriesUpdated] = ISNULL([CategoriesUpdated],0) + ISNULL(@CategoriesUpdated,0)
       , [CategoriesDeleted] = ISNULL([CategoriesDeleted],0) + ISNULL(@CategoriesDeleted,0)
       , [CategoriesRated] = ISNULL([CategoriesRated],0) + ISNULL(@CategoriesRated,0)
       , [CategoriesReRated] = ISNULL([CategoriesReRated],0) + ISNULL(@CategoriesReRated,0)
       , [CategoryTopicsAdded] = ISNULL([CategoryTopicsAdded],0) + ISNULL(@CategoryTopicsAdded,0)
       , [CategoryTopicsDeleted] = ISNULL([CategoryTopicsDeleted],0) + ISNULL(@CategoryTopicsDeleted,0)
       , [TopicsAdded] = ISNULL([TopicsAdded],0) + ISNULL(@TopicsAdded,0)
       , [TopicsUpdated] = ISNULL([TopicsUpdated],0) + ISNULL(@TopicsUpdated,0)
       , [TopicsDeleted] = ISNULL([TopicsDeleted],0) + ISNULL(@TopicsDeleted,0)
       , [TopicsRated] = ISNULL([TopicsRated],0) + ISNULL(@TopicsRated,0)
       , [TopicsReRated] = ISNULL([TopicsReRated],0) + ISNULL(@TopicsReRated,0)
       , [TopicDocumentsAdded] = ISNULL([TopicDocumentsAdded],0) + ISNULL(@TopicDocumentsAdded,0)
       , [TopicDocumentsDeleted] = ISNULL([TopicDocumentsDeleted],0) + ISNULL(@TopicDocumentsDeleted,0)
       , [DocumentsAdded] = ISNULL([DocumentsAdded],0) + ISNULL(@DocumentsAdded,0)
       , [DocumentsUpdated] = ISNULL([DocumentsUpdated],0) + ISNULL(@DocumentsUpdated,0)
       , [DocumentsDeleted] = ISNULL([DocumentsDeleted],0) + ISNULL(@DocumentsDeleted,0)
       , [DocumentsRated] = ISNULL([DocumentsRated],0) + ISNULL(@DocumentsRated,0)
       , [DocumentsReRated] = ISNULL([DocumentsReRated],0) + ISNULL(@DocumentsReRated,0)
       , [LastContributionDt] = CURRENT_TIMESTAMP
  WHERE [OriginalLogin] = @OriginalLogin;

  IF @@ROWCOUNT = 0
   INSERT [SQLRunbook].[tUser]
    ( [OriginalLogin]
    , [FriendlyName] 
    , [Notes]
    , [CategoriesAdded]
    , [CategoriesUpdated]
    , [CategoriesDeleted]
    , [CategoriesRated]
    , [CategoriesReRated]
    , [CategoryTopicsAdded]
    , [CategoryTopicsDeleted]
    , [TopicsAdded]
    , [TopicsUpdated]
    , [TopicsDeleted]
    , [TopicsRated]
    , [TopicsReRated]
    , [TopicDocumentsAdded]
    , [TopicDocumentsDeleted]
    , [DocumentsAdded]
    , [DocumentsUpdated]
    , [DocumentsDeleted]
    , [DocumentsRated]
    , [DocumentsReRated]
    , [LastContributionDt] )
   VALUES
    ( @OriginalLogin
    , ''
    , @Notes
    , ISNULL(@CategoriesAdded,0)
    , ISNULL(@CategoriesUpdated,0)
    , ISNULL(@CategoriesDeleted,0)
    , ISNULL(@CategoriesRated,0)
    , ISNULL(@CategoriesReRated,0)
    , ISNULL(@CategoryTopicsAdded,0)
    , ISNULL(@CategoryTopicsDeleted,0)
    , ISNULL(@TopicsAdded,0)
    , ISNULL(@TopicsUpdated,0)
    , ISNULL(@TopicsDeleted,0)
    , ISNULL(@TopicsRated,0)
    , ISNULL(@TopicsReRated,0)
    , ISNULL(@TopicDocumentsAdded,0)
    , ISNULL(@TopicDocumentsDeleted,0)
    , ISNULL(@DocumentsAdded,0)
    , ISNULL(@DocumentsUpdated,0)
    , ISNULL(@DocumentsDeleted,0)
    , ISNULL(@DocumentsRated,0)
    , ISNULL(@DocumentsReRated,0)
    , CURRENT_TIMESTAMP) 

 END TRY
 BEGIN CATCH

  DECLARE @TextData [NVARCHAR] (MAX)
  SET @TextData = '   @OriginalLogin = ' + ISNULL(CHAR(39) + @OriginalLogin + CHAR(39),'NULL') + CHAR(13) + CHAR(10) +
   ' , @FriendlyName = ' + ISNULL(CHAR(39) + @FriendlyName + CHAR(39),'NULL') + CHAR(13) + CHAR(10) +
   ' , @Notes = ' + ISNULL(CHAR(39) + @Notes + CHAR(39),'NULL') + CHAR(13) + CHAR(10) +
   ' , @CategoriesAdded = ' + ISNULL(CAST(@CategoriesAdded AS [NVARCHAR](10)),'NULL') + CHAR(13) + CHAR(10) +
   ' , @CategoriesUpdated = ' + ISNULL(CAST(@CategoriesUpdated AS [NVARCHAR](10)),'NULL') + CHAR(13) + CHAR(10) +
   ' , @CategoriesDeleted = ' + ISNULL(CAST(@CategoriesDeleted AS [NVARCHAR](10)),'NULL') + CHAR(13) + CHAR(10) +
   ' , @CategoriesRated = ' + ISNULL(CAST(@CategoriesRated AS [NVARCHAR](10)),'NULL') + CHAR(13) + CHAR(10) +
   ' , @CategoriesReRated = ' + ISNULL(CAST(@CategoriesReRated AS [NVARCHAR](10)),'NULL') + CHAR(13) + CHAR(10) +
   ' , @CategoryTopicsAdded = ' + ISNULL(CAST(@CategoryTopicsAdded AS [NVARCHAR](10)),'NULL') + CHAR(13) + CHAR(10) +
   ' , @CategoryTopicsDeleted = ' + ISNULL(CAST(@CategoryTopicsDeleted AS [NVARCHAR](10)),'NULL') + CHAR(13) + CHAR(10) +
   ' , @TopicsAdded = ' + ISNULL(CAST(@TopicsAdded AS [NVARCHAR](10)),'NULL') + CHAR(13) + CHAR(10) +
   ' , @TopicsUpdated = ' + ISNULL(CAST(@TopicsUpdated AS [NVARCHAR](10)),'NULL') + CHAR(13) + CHAR(10) +
   ' , @TopicsDeleted = ' + ISNULL(CAST(@TopicsDeleted AS [NVARCHAR](10)),'NULL') + CHAR(13) + CHAR(10) +
   ' , @TopicsRated = ' + ISNULL(CAST(@TopicsRated AS [NVARCHAR](10)),'NULL') + CHAR(13) + CHAR(10) +
   ' , @TopicsReRated = ' + ISNULL(CAST(@TopicsReRated AS [NVARCHAR](10)),'NULL') + CHAR(13) + CHAR(10) +
   ' , @TopicDocumentsAdded = ' + ISNULL(CAST(@TopicDocumentsAdded AS [NVARCHAR](10)),'NULL') + CHAR(13) + CHAR(10) +
   ' , @TopicDocumentsDeleted = ' + ISNULL(CAST(@TopicDocumentsDeleted AS [NVARCHAR](10)),'NULL') + CHAR(13) + CHAR(10) +
   ' , @DocumentsAdded = ' + ISNULL(CAST(@DocumentsAdded AS [NVARCHAR](10)),'NULL') + CHAR(13) + CHAR(10) +
   ' , @DocumentsUpdated = ' + ISNULL(CAST(@DocumentsUpdated AS [NVARCHAR](10)),'NULL') + CHAR(13) + CHAR(10) +
   ' , @DocumentsDeleted = ' + ISNULL(CAST(@DocumentsDeleted AS [NVARCHAR](10)),'NULL') + CHAR(13) + CHAR(10) +
   ' , @DocumentsRated = ' + ISNULL(CAST(@DocumentsRated AS [NVARCHAR](10)),'NULL') + CHAR(13) + CHAR(10) +
   ' , @DocumentsReRated = ' + ISNULL(CAST(@DocumentsReRated AS [NVARCHAR](10)),'NULL'); 

  EXEC [SQLRunbook].[pLogSQLError] @TextData, @@PROCID;

 END CATCH 

END;
GO

GRANT EXECUTE ON [SQLRunbook].[pUserUpsert] TO [Public];

GO

IF OBJECT_ID('SQLRunbook.trgOption_Insert_Update_Delete') IS NOT NULL
 DROP TRIGGER [SQLRunbook].[trgOption_Insert_Update_Delete]

GO
/******************************************************************************
**	Auth: Bill Wunder
**	Date: April 1, 2007
**
*******************************************************************************
**	Change History
*******************************************************************************
**	Date		   Author		     Description of Change
**	          
*******************************************************************************/
CREATE TRIGGER [SQLRunbook].[trgOption_Insert_Update_Delete]
ON [SQLRunbook].[tOption]
FOR INSERT, UPDATE, DELETE
AS
BEGIN
   DECLARE @icount [INT]
  , @dcount [INT]
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
   BEGIN
    RAISERROR ('Multi-row [SQLRunbook].[tOption] operations are not permitted', 16,1);
   END;

  -- no delete
  IF @icount = 0 AND @dcount = 1
   BEGIN
    RAISERROR ('[SQLRunbook].[tOption] delete not permitted', 16,1);
   END;

  -- only one row allowed
  IF @icount = 1 AND @dcount = 0 AND (SELECT COUNT(*) FROM [SQLRunbook].[tOption]) = 1 
   BEGIN
    RAISERROR ('Only one [SQLRunbook].[tOption] row permitted', 16,1);
   END;
  
  -- Row level security always implicitly on for tSQLRunbookOption
  -- see EnforceOwnership comments in the document trigger for details
  IF (Select [SQLRunbook].[fnIsAdmin]()) = 0
   BEGIN
    RAISERROR ('Only SQLClue Runbook Administrator may change [SQLRunbook].[tOption]', 16,1);
   END;

 END TRY 

 BEGIN CATCH

  EXEC [SQLRunbook].[pLogSQLError] NULL, @@PROCID;

 END CATCH 

END;

GO

IF OBJECT_ID('SQLRunbook.trgDocument_Insert_Update_Delete') IS NOT NULL
 DROP TRIGGER [SQLRunbook].[trgDocument_Insert_Update_Delete]

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
CREATE TRIGGER [SQLRunbook].[trgDocument_Insert_Update_Delete]
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

IF OBJECT_ID('SQLRunbook.trgDocumentRating_Insert_Update_Delete') IS NOT NULL
 DROP TRIGGER [SQLRunbook].[trgDocumentRating_Insert_Update_Delete]

GO
/******************************************************************************
**	Auth: Bill Wunder
**	Date: April 1, 2007
**
*******************************************************************************
**	Change History
*******************************************************************************
**	Date		   Author		     Description of Change
**	          
*******************************************************************************/
CREATE TRIGGER [SQLRunbook].[trgDocumentRating_Insert_Update_Delete]
ON [SQLRunbook].[tDocumentRating]
FOR INSERT, UPDATE, DELETE
AS
BEGIN

 DECLARE @icount [INT]
  , @dcount [INT]
  , @DocumentId [INT]
  , @OriginalLogin NVARCHAR(256);

 SET NOCOUNT ON;

 BEGIN TRY

  SELECT @icount = COUNT(*) FROM [inserted];
  SELECT @dcount = COUNT(*) FROM [deleted];
  SET @OriginalLogin  = ORIGINAL_LOGIN();
  
  -- always maintain the SQLRunbookUser
  
  -- delete
 
  -- update
  IF @dcount > 0 and @icount = @dcount
   BEGIN
    IF EXISTS (SELECT * FROM deleted 
               WHERE [RecCreatedUser] <> @OriginalLogin)               
     RAISERROR ('Invalid Document Rating change. User [%s] cannot update ratings originated by other users.', 16,1);

    UPDATE r
    SET [Notes] = ISNULL(r.[Notes], '')
                 + CAST(CURRENT_TIMESTAMP AS [NVARCHAR](30)) + ' : ' + ORIGINAL_LOGIN() 
                 + CHAR(13) + CHAR(10) +
                 + i.[Notes]
     , [LastUpdatedDt] = CURRENT_TIMESTAMP
     , [LastUpdatedUser] = @OriginalLogin
    FROM [inserted] i
    JOIN [SQLRunbook].[tDocumentRating] r
    ON i.[Id] = r.[Id]

    EXEC [SQLRunbook].[pUserUpsert] @OriginalLogin = @OriginalLogin
                                     , @DocumentsReRated = @icount;   
   END;

  -- insert
  IF @dcount = 0 and @icount = 1
   EXEC [SQLRunbook].[pUserUpsert] @OriginalLogin = @OriginalLogin
                                    , @DocumentsRated = @icount;   

 END TRY 

 BEGIN CATCH

  EXEC [SQLRunbook].[pLogSQLError] NULL, @@PROCID;

 END CATCH 

END;

GO 

IF  OBJECT_ID('[SQLRunbook].[trgTopicDocument_Insert_Update_Delete]','TR') IS NOT NULL
 DROP TRIGGER [SQLRunbook].[trgTopicDocument_Insert_Update_Delete]

GO
/******************************************************************************
**	Auth: Bill Wunder
**	Date: April 1, 2007
**
*******************************************************************************
**	Change History
*******************************************************************************
**	Date		   Author		     Description of Change
**	          
*******************************************************************************/
CREATE TRIGGER [SQLRunbook].[trgTopicDocument_Insert_Update_Delete]
ON [SQLRunbook].[tTopicDocument]
FOR INSERT, UPDATE, DELETE
AS
BEGIN

 DECLARE @icount [INT]
  , @dcount [INT]
  , @DocumentId [INT]
  , @OriginalLogin [NVARCHAR] (128)
  , @Owner [NVARCHAR] (128)
  , @TopicId [INT];

 SET NOCOUNT ON;

 BEGIN TRY

  SELECT @icount = COUNT(*) FROM [inserted];
  SELECT @dcount = COUNT(*) FROM [deleted];
  SET @OriginalLogin = ORIGINAL_LOGIN();

  -- delete
  IF @dcount > 0 and @icount = 0 
   BEGIN
    -- EnforceOwnership - row level security - see comments in document trigger 
    IF (SELECT [SQLRunbook].[fnIsAdmin]()) = 0
    AND (SELECT [EnforceOwnership] FROM [SQLRunbook].[tOption]) = 1
     BEGIN
      SELECT TOP(1) @TopicId = d.[TopicId]
       , @DocumentId = d.[DocumentId]
       , @Owner = t.[Owner] 
      FROM [deleted] d
      JOIN [SQLRunbook].[tTopic] t
      on d.[TopicId] = t.[Id]
      JOIN [SQLRunbook].[tDocument] doc
      on d.[TopicId] = doc.[Id]
      WHERE @OriginalLogin NOT IN (t.[Owner], doc.[Owner]); 
      IF @@ROWCOUNT = 1
       RAISERROR ('User [%s] attempted to delete association of Topic %d owned by user [%s] and Document %d owned by [%s].', 
                  16,1, @OriginalLogin, @TopicId, @Owner, @DocumentId);
     END;
    ELSE
     BEGIN
      EXEC [SQLRunbook].[pUserUpsert] @OriginalLogin = @OriginalLogin
                                       , @TopicDocumentsDeleted = @dcount;     

      -- update the topic text when associated documents are inserted, updated, or deleted
      -- could be multiple rows 
	  UPDATE t  
	  SET [Notes] = ISNULL(t.[Notes], '')
                 + CAST(CURRENT_TIMESTAMP AS [NVARCHAR](30)) + ' : ' + ORIGINAL_LOGIN() 
                  + CHAR(13) + CHAR(10) + 
                  + 'Removed Topic ' + t.[Name] + ' association to Document [' 
                  + rd.[File] + ']. (DocumentId=' + CAST(d.[DocumentId] AS [NVARCHAR](10))+ ')' 
                  + CHAR(13) + CHAR(10)
       , [LastUpdatedDt] = CURRENT_TIMESTAMP
       , [LastUpdatedUser] = @OriginalLogin            
      FROM deleted d
      INNER JOIN [SQLRunbook].[tDocument] rd
      ON d.[DocumentId] = rd.[Id]     
      INNER JOIN [SQLRunbook].[tTopic] t
      ON d.[TopicId] = t.[Id];
    END
   END;

  -- updates
  IF @dcount > 0 and @icount > 0
   BEGIN

    -- topicdocuments cannot be updated 
    RAISERROR ('[SQLRunbook].[tTopicDocument] update operations are not permitted', 16,1);

   END;      

  -- insert
  IF @dcount = 0 AND @icount > 0
   BEGIN

     -- could be multiple rows 
  	 UPDATE t  
	 SET [Notes] = ISNULL(t.[Notes], '')
                 + CAST(CURRENT_TIMESTAMP AS [NVARCHAR](30)) + ' : ' + ORIGINAL_LOGIN() 
                 + CHAR(13) + CHAR(10) +
                 + 'Topic "' + t.[Name] + '" associated to file [' + d.[File] 
                 + ']. (DocumentId=' + CAST(d.Id AS [NVARCHAR](10))+ ')' 
                 + CHAR(13) + CHAR(10)
      , [LastUpdatedDt] = CURRENT_TIMESTAMP
      , [LastUpdatedUser] = @OriginalLogin            
     FROM inserted i
     INNER JOIN [SQLRunbook].[tDocument] d
     ON i.[DocumentId] = d.[Id]     
     INNER JOIN [SQLRunbook].[tTopic] t
     ON i.[TopicId] = t.[Id];

    EXEC [SQLRunbook].[pUserUpsert] @OriginalLogin = @OriginalLogin
                                     , @TopicDocumentsAdded = @icount;   

   END;

 END TRY 

 BEGIN CATCH

  EXEC [SQLRunbook].[pLogSQLError] NULL, @@PROCID;

 END CATCH 

END;

GO 

IF  OBJECT_ID('[SQLRunbook].[trgTopic_Insert_Update_Delete]','TR') IS NOT NULL
 DROP TRIGGER [SQLRunbook].[trgTopic_Insert_Update_Delete]

GO
/******************************************************************************
**	Auth: Bill Wunder
**	Date: April 1, 2007
**
*******************************************************************************
**	Change History
*******************************************************************************
**	Date		   Author		     Description of Change
**	          
*******************************************************************************/
CREATE TRIGGER [SQLRunbook].[trgTopic_Insert_Update_Delete]
ON [SQLRunbook].[tTopic]
FOR INSERT, UPDATE, DELETE
AS
BEGIN

 DECLARE @icount [INT]
  , @dcount [INT]
  , @TopicId [INT]
  , @Op [NVARCHAR] (10)
  , @OriginalLogin [NVARCHAR] (128)
  , @Owner [NVARCHAR] (128);

 SET NOCOUNT ON;

 BEGIN TRY

  SELECT @icount = COUNT(*) FROM [inserted];
  SELECT @dcount = COUNT(*) FROM [deleted];
  SET @OriginalLogin = ORIGINAL_LOGIN()

  -- EnforceOwnership - row level security - see comments in document trigger 
  IF @dcount > 0
   BEGIN
    IF @dcount = 1 
    AND (SELECT [SQLRunbook].[fnIsAdmin]()) = 0
    AND (SELECT [EnforceOwnership] FROM [SQLRunbook].[tOption]) = 1
     BEGIN
      SELECT @TopicId = [Id], @Owner = [Owner] FROM [deleted] WHERE [Owner] <> @OriginalLogin; 
      IF @@ROWCOUNT > 0
       BEGIN
        SET @Op = CASE WHEN @icount > 0 THEN 'delete' ELSE 'update' END;
        RAISERROR ('User [%s] attempted to %s Topic Id %d owned by user [%s]', 16,1, @OriginalLogin, @Op, @TopicId, @Owner);
       END;
     END;
   END;

  -- delete
  IF @dcount > 0 and @icount = 0 
   EXEC [SQLRunbook].[pUserUpsert] @OriginalLogin = @OriginalLogin
                                    , @TopicsDeleted = @dcount;   
  -- updates
  IF @dcount > 0 and @icount > 0
   BEGIN
    EXEC [SQLRunbook].[pUserUpsert] @OriginalLogin = @OriginalLogin
                                     , @TopicsUpdated = @dcount;  
    UPDATE t
    SET [LastUpdatedDt] = CURRENT_TIMESTAMP
     , [LastUpdatedUser] = @OriginalLogin
    FROM inserted i
    JOIN [SQLRunbook].[tTopic] t
    ON i.[Id] = t.[Id];
        
   END

  -- insert
  IF @dcount = 0 AND @icount > 0
   EXEC [SQLRunbook].[pUserUpsert] @OriginalLogin = @OriginalLogin
                                    , @TopicsAdded = @icount;   

 END TRY 

 BEGIN CATCH

  EXEC [SQLRunbook].[pLogSQLError] NULL, @@PROCID;

 END CATCH 

END;

GO 

IF OBJECT_ID('SQLRunbook.trgTopicRating_Insert_Update_Delete') IS NOT NULL
 DROP TRIGGER [SQLRunbook].[trgTopicRating_Insert_Update_Delete]

GO
/******************************************************************************
**	Auth: Bill Wunder
**	Date: April 1, 2007
**
*******************************************************************************
**	Change History
*******************************************************************************
**	Date		   Author		     Description of Change
**	          
*******************************************************************************/
CREATE TRIGGER [SQLRunbook].[trgTopicRating_Insert_Update_Delete]
ON [SQLRunbook].[tTopicRating]
FOR INSERT, UPDATE, DELETE
AS
BEGIN

 DECLARE @icount [INT]
  , @dcount [INT]
  , @DocumentId [INT]
  , @OriginalLogin NVARCHAR(256);

 SET NOCOUNT ON;

 BEGIN TRY

  SELECT @icount = COUNT(*) FROM [inserted];
  SELECT @dcount = COUNT(*) FROM [deleted];
  SET @OriginalLogin  = ORIGINAL_LOGIN();
  
  -- always maintain the SQLRunbookUser
  
  -- delete
  -- noop
 
  -- update
  IF @dcount > 0 and @icount > 0
   BEGIN
    IF EXISTS (SELECT * FROM deleted 
               WHERE [RecCreatedUser] <> @OriginalLogin)               
     RAISERROR ('Invalid Topic Rating change. User [%s] cannot update ratings originated by other users.', 16,1);
        
    UPDATE r
    SET [Notes] = ISNULL(r.[Notes], '')
                 + CAST(CURRENT_TIMESTAMP AS [NVARCHAR](30)) + ' : ' + ORIGINAL_LOGIN() 
                 + CHAR(13) + CHAR(10) +
                 + i.[Notes]
     , [LastUpdatedDt] = CURRENT_TIMESTAMP
     , [LastUpdatedUser] = @OriginalLogin
    FROM [inserted] i
    JOIN [SQLRunbook].[tTopicRating] r
    ON i.[Id] = r.[Id]

    EXEC [SQLRunbook].[pUserUpsert] @OriginalLogin = @OriginalLogin
                                     , @TopicsReRated = @icount;   
   END;

  -- insert
  IF @dcount = 0 and @icount > 0
   EXEC [SQLRunbook].[pUserUpsert] @OriginalLogin = @OriginalLogin
                                    , @TopicsRated = @icount;   

 END TRY 

 BEGIN CATCH

  EXEC [SQLRunbook].[pLogSQLError] NULL, @@PROCID;

 END CATCH 

END;

GO 

IF  OBJECT_ID('[SQLRunbook].[trgCategory_Insert_Update_Delete]','TR') IS NOT NULL
 DROP TRIGGER [SQLRunbook].[trgCategory_Insert_Update_Delete]

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
CREATE TRIGGER [SQLRunbook].[trgCategory_Insert_Update_Delete]
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

IF OBJECT_ID('SQLRunbook.trgCategoryRating_Insert_Update_Delete') IS NOT NULL
 DROP TRIGGER [SQLRunbook].[trgCategoryRating_Insert_Update_Delete]

GO
/******************************************************************************
**	Auth: Bill Wunder
**	Date: April 1, 2007
**
*******************************************************************************
**	Change History
*******************************************************************************
**	Date		   Author		     Description of Change
**	          
*******************************************************************************/
CREATE TRIGGER [SQLRunbook].[trgCategoryRating_Insert_Update_Delete]
ON [SQLRunbook].[tCategoryRating]
FOR INSERT, UPDATE, DELETE
AS
BEGIN

 DECLARE @icount [INT]
  , @dcount [INT]
  , @DocumentId [INT]
  , @OriginalLogin NVARCHAR(256);

 SET NOCOUNT ON;

 BEGIN TRY

  SELECT @icount = COUNT(*) FROM [inserted];
  SELECT @dcount = COUNT(*) FROM [deleted];
  SET @OriginalLogin  = ORIGINAL_LOGIN();
  
  -- always maintain the SQLRunbookUser
  
  -- delete
  -- noop - should only happen through doc trigger
 
  -- update
  IF @dcount > 0 and @icount > 0
   BEGIN
    IF EXISTS (SELECT * FROM deleted 
               WHERE [RecCreatedUser] <> @OriginalLogin)               
     RAISERROR ('Invalid Category Rating change. User [%s] cannot update ratings originated by other users.', 16,1);

    UPDATE r
    SET [Notes] = ISNULL(r.[Notes], '')
                 + CAST(CURRENT_TIMESTAMP AS [NVARCHAR](30)) + ' : ' + ORIGINAL_LOGIN() 
                 + CHAR(13) + CHAR(10) +
                 + i.[Notes]
     , [LastUpdatedDt] = CURRENT_TIMESTAMP
     , [LastUpdatedUser] = @OriginalLogin
    FROM [inserted] i
    JOIN [SQLRunbook].[tCategoryRating] r
    ON i.[Id] = r.[Id]
 
    EXEC [SQLRunbook].[pUserUpsert] @OriginalLogin = @OriginalLogin
                                     , @CategoriesReRated = @icount;   
   END;

  -- insert
  IF @dcount = 0 and @icount > 0
   EXEC [SQLRunbook].[pUserUpsert] @OriginalLogin = @OriginalLogin
                                    , @CategoriesRated = @icount;   

 END TRY 

 BEGIN CATCH

  EXEC [SQLRunbook].[pLogSQLError] NULL, @@PROCID;

 END CATCH 

END;

GO 

IF  OBJECT_ID('[SQLRunbook].[trgCategoryTopic_Insert_Update_Delete]','TR') IS NOT NULL
 DROP TRIGGER [SQLRunbook].[trgCategoryTopic_Insert_Update_Delete]

GO
/******************************************************************************
**	Auth: Bill Wunder
**	Date: April 1, 2007
**
*******************************************************************************
**	Change History
*******************************************************************************
**	Date		   Author		     Description of Change
**	          
*******************************************************************************/
CREATE TRIGGER [SQLRunbook].[trgCategoryTopic_Insert_Update_Delete]
ON [SQLRunbook].[tCategoryTopic]
FOR INSERT, UPDATE, DELETE
AS
BEGIN

 DECLARE @icount [INT]
  , @dcount [INT]
  , @CategoryId [INT]
  , @TopicId [INT]
  , @OriginalLogin [NVARCHAR] (128)
  , @CatOwner [NVARCHAR] (128)
  , @TopOwner [NVARCHAR] (128);

 SET NOCOUNT ON;

 BEGIN TRY

  SELECT @icount = COUNT(*) FROM [inserted];
  SELECT @dcount = COUNT(*) FROM [deleted];
  SET @OriginalLogin = ORIGINAL_LOGIN();

  -- update the topic text when associated documents are inserted, updated, or deleted
  
  -- delete
  IF @dcount > 0 and @icount = 0 
   BEGIN

     -- EnforecOwnership - row level security - see comments in document trigger
    IF (SELECT [SQLRunbook].[fnIsAdmin]()) = 0
    AND (SELECT [EnforceOwnership] FROM [SQLRunbook].[tOption]) = 1
     BEGIN
      SELECT @CategoryId = d.[CategoryId]
           , @TopicId = d.[TopicId]
           , @TopOwner = t.[Owner] 
      FROM deleted d
      JOIN [SQLRunbook].[tCategory] c
      ON d.[CategoryId] = c.[Id]
      JOIN [SQLRunbook].[tTopic] t
      ON d.[TopicId] = t.[Id]
      WHERE @OriginalLogin <> t.[Owner]; 
      IF @@ROWCOUNT = 1
       BEGIN
       
          RAISERROR ('User [%s]. Invalid removal of association between Category %d and Topic %d owned by [%s].', 
                     16,1, @OriginalLogin, @CategoryId, @CatOwner, @TopicId, @TopOwner);
       END              
     END;

    -- could be multiple rows 
  	 UPDATE t  
	 SET [Notes] = ISNULL(t.[Notes], '')
			     + CAST(CURRENT_TIMESTAMP AS VARCHAR(30)) + ' : ' + ORIGINAL_LOGIN() 
			     + CHAR(13) + CHAR(10) +
                 + 'Topic removed from Category ' + c.[Name]
                 + CHAR(13) + CHAR(10)
      , [LastUpdatedDt] = CURRENT_TIMESTAMP
      , [LastUpdatedUser] = @OriginalLogin       
     FROM deleted d
     INNER JOIN [SQLRunbook].[tCategory] c
     ON d.[CategoryId] = c.[Id]     
     INNER JOIN [SQLRunbook].[tTopic] t
     ON d.[TopicId] = t.[Id];

    EXEC [SQLRunbook].[pUserUpsert] @OriginalLogin = @OriginalLogin
                                     , @CategoryTopicsDeleted = @dcount   

   END;

  -- updates
  IF @dcount > 0 and @icount > 0
   BEGIN

    -- topicdocuments cannot be updated 
    RAISERROR ('[SQLRunbook].[tTopicDocument] update operations are not permitted', 16,1);

   END;      

  -- insert
  IF @dcount = 0 AND @icount > 0
   BEGIN
    -- could be multiple rows 

    EXEC [SQLRunbook].[pUserUpsert] @OriginalLogin = @OriginalLogin
                                     , @CategoryTopicsAdded = @icount   

	UPDATE t  
    SET [Notes] = ISNULL(t.[Notes], '')
   		        + CAST(CURRENT_TIMESTAMP AS VARCHAR(30)) + ' : ' + ORIGINAL_LOGIN() 
   		        + CHAR(13) + CHAR(10) +
		        + 'Topic added to category ' + c.[Name]                        
                + CHAR(13) + CHAR(10)
     , [LastUpdatedDt] = CURRENT_TIMESTAMP
     , [LastUpdatedUser] = @OriginalLogin       
    FROM inserted i
    INNER JOIN [SQLRunbook].[tCategory] c
    ON i.[CategoryId] = c.[Id]     
    INNER JOIN [SQLRunbook].[tTopic] t
    ON i.[TopicId] = t.[Id];

   END;

 END TRY 

 BEGIN CATCH

  EXEC [SQLRunbook].[pLogSQLError] NULL, @@PROCID;

 END CATCH 

END;

GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLRunbook'
           AND SPECIFIC_NAME = N'pFullTextDocumentTypeSelect' )
 DROP PROCEDURE [SQLRunbook].[pFullTextDocumentTypeSelect];

GO
/******************************************************************************
**	Auth: Bill Wunder
**	Date: April 1, 2007
**
**	Desc: select  full text ifilters installed on the host   
**
*******************************************************************************
**	Change History
*******************************************************************************
**	Date		   Author		     Description of Change
**	          
*******************************************************************************/
CREATE PROCEDURE [SQLRunbook].[pFullTextDocumentTypeSelect] 
 ( @DocumentType [NVARCHAR] (10))  
AS
BEGIN

 SELECT [document_type]
  , [class_id]
  , [path]
  , [version]
  , [manufacturer]
 FROM [sys].[fulltext_document_types]
 WHERE @DocumentType = 'all' OR [document_type] = @DocumentType  
 
END;

GO

GRANT EXECUTE ON [SQLRunbook].[pFullTextDocumentTypeSelect] TO [SQLRunbookUserRole];

GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLRunbook'
           AND SPECIFIC_NAME = N'pOptionGet' )
 DROP PROCEDURE [SQLRunbook].[pOptionGet];

GO
/******************************************************************************
**	Auth: Bill Wunder
**	Date: April 1, 2007
**
**	Desc: Gets the runbook options   
**
*******************************************************************************
**	Change History
*******************************************************************************
**	Date		   Author		     Description of Change
**	          
*******************************************************************************/
CREATE PROCEDURE [SQLRunbook].[pOptionGet]
 ( @EnforceOwnership [BIT] OUTPUT
 , @ScanForDocumentChanges [BIT] OUTPUT)  
AS
BEGIN

 SELECT @EnforceOwnership = [EnforceOwnership] 
  , @ScanForDocumentChanges = [ScanForDocumentChanges]
 FROM [SQLRunbook].[tOption];

END;

GO

GRANT EXECUTE ON [SQLRunbook].[pOptionGet] TO [SQLRunbookUserRole];

GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLRunbook'
           AND SPECIFIC_NAME = N'pOptionSet' )
 DROP PROCEDURE [SQLRunbook].[pOptionSet];

GO
/******************************************************************************
**	Auth: Bill Wunder
**	Date: April 1, 2007
**
**	Desc: Sets the runbook options   
**
*******************************************************************************
**	Change History
*******************************************************************************
**	Date		   Author		     Description of Change
**	          
*******************************************************************************/
CREATE PROCEDURE [SQLRunbook].[pOptionSet]
 ( @EnforceOwnership [BIT]
 , @ScanForDocumentChanges [BIT])  
AS
BEGIN

 UPDATE [SQLRunbook].[tOption] 
 SET @EnforceOwnership = [EnforceOwnership] 
  , @ScanForDocumentChanges = [ScanForDocumentChanges];

END;

GO

GRANT EXECUTE ON [SQLRunbook].[pOptionSet] TO [SQLRunbookAdminRole];

GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLRunbook'
           AND SPECIFIC_NAME = N'pUserSelectAll' )
 DROP PROCEDURE [SQLRunbook].[pUserSelectAll];

GO
/******************************************************************************
**	Auth: Bill Wunder
**	Date: April 1, 2007
**
**	Desc: Get the list of Runbook Users   
**
*******************************************************************************
**	Change History
*******************************************************************************
**	Date		   Author		     Description of Change
**	          
*******************************************************************************/
CREATE PROCEDURE [SQLRunbook].[pUserSelectAll] 
AS
BEGIN

 SELECT [Id] 
   , [OriginalLogin]
   , [FriendlyName]
   , [Notes]
   , [CategoriesAdded]
   , [CategoriesUpdated]
   , [CategoriesDeleted]
   , [CategoriesRated]
   , [CategoryTopicsAdded]
   , [CategoryTopicsDeleted]
   , [TopicsAdded]
   , [TopicsUpdated]
   , [TopicsDeleted]
   , [TopicsRated]
   , [TopicDocumentsAdded]
   , [TopicDocumentsDeleted]
   , [DocumentsAdded]
   , [DocumentsUpdated]
   , [DocumentsDeleted]
   , [DocumentsRated]
   , [LastContributionDt]
 FROM [SQLRunbook].[tUser]
 ORDER BY [OriginalLogin];

END;

GO

GRANT EXECUTE ON [SQLRunbook].[pUserSelectAll] TO [SQLRunbookUserRole];

GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLRunbook'
           AND SPECIFIC_NAME = N'pUserSelectAllOriginalLogins' )
 DROP PROCEDURE [SQLRunbook].[pUserSelectAllOriginalLogins];

GO
/******************************************************************************
**	Auth: Bill Wunder
**	Date: April 1, 2007
**
**	Desc: Get the list of Runbook Users   
**
*******************************************************************************
**	Change History
*******************************************************************************
**	Date		   Author		     Description of Change
**	          
*******************************************************************************/
CREATE PROCEDURE [SQLRunbook].[pUserSelectAllOriginalLogins] 
AS
BEGIN

 SELECT [Id] 
   , [OriginalLogin]
 FROM [SQLRunbook].[tUser]
 ORDER BY [OriginalLogin];

END;

GO

GRANT EXECUTE ON [SQLRunbook].[pUserSelectAllOriginalLogins] TO [SQLRunbookUserRole];

GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLRunbook'
           AND SPECIFIC_NAME = N'pUserSelect' )
 DROP PROCEDURE [SQLRunbook].[pUserSelect];

GO
/******************************************************************************
**	Auth: Bill Wunder
**	Date: April 1, 2007
**
**	Desc: Get the RunbookUser rows for specified criteria 
**
*******************************************************************************
**	Change History
*******************************************************************************
**	Date		   Author		     Description of Change
**	          
*******************************************************************************/
CREATE PROCEDURE [SQLRunbook].[pUserSelect]
 ( @OriginalLogin [NVARCHAR] (256) )
AS
BEGIN

 -- not expecting more than a few hundred rows at most
 SELECT [Id] 
   , [OriginalLogin]
   , [FriendlyName]
   , [Notes]
   , [CategoriesAdded]
   , [CategoriesUpdated]
   , [CategoriesDeleted]
   , [CategoriesRated]
   , [CategoryTopicsAdded]
   , [CategoryTopicsDeleted]
   , [TopicsAdded]
   , [TopicsUpdated]
   , [TopicsDeleted]
   , [TopicsRated]
   , [TopicDocumentsAdded]
   , [TopicDocumentsDeleted]
   , [DocumentsAdded]
   , [DocumentsUpdated]
   , [DocumentsDeleted]
   , [DocumentsRated]
   , [LastContributionDt]
 FROM [SQLRunbook].[tUser]
 WHERE [OriginalLogin] = @OriginalLogin;

END;

GO

GRANT EXECUTE ON [SQLRunbook].[pUserSelect] TO [SQLRunbookUserRole];

GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLRunbook'
           AND SPECIFIC_NAME = N'pUserSelectDetails' )
 DROP PROCEDURE [SQLRunbook].[pUserSelectDetails];

GO
/******************************************************************************
**	Auth: Bill Wunder
**	Date: April 1, 2007
**
**	Desc: Get attributes for a Runbook User   
**
*******************************************************************************
**	Change History
*******************************************************************************
**	Date		   Author		     Description of Change
**	          
*******************************************************************************/
CREATE PROCEDURE [SQLRunbook].[pUserSelectDetails] 
 ( @OriginalLogin [NVARCHAR] (256) )

AS
BEGIN

 -- used by ReportViewer, use select
 SELECT u.[Id] 
   , u.[OriginalLogin]
   , u.[FriendlyName]
   , u.[Notes]
   , u.[CategoriesAdded]
   , u.[CategoriesUpdated]
   , u.[CategoriesDeleted]
   , u.[CategoriesRated]
   , u.[CategoryTopicsAdded]
   , u.[CategoryTopicsDeleted]
   , u.[TopicsAdded]
   , u.[TopicsUpdated]
   , u.[TopicsDeleted]
   , u.[TopicsRated]
   , ISNULL(t.[TopicsOwned], 0) AS [TopicsOwned] 
   , u.[TopicDocumentsAdded]
   , u.[TopicDocumentsDeleted]
   , u.[DocumentsAdded]
   , u.[DocumentsUpdated]
   , u.[DocumentsDeleted]
   , u.[DocumentsRated]
   , ISNULL(d.[DocumentsOwned], 0) AS [DocumentsOwned]
   , u.[LastContributionDt]
 FROM [SQLRunbook].[tUser] u
 LEFT JOIN (SELECT [Owner]
             , COUNT(*) AS [TopicsOwned]
            FROM [SQLRunbook].[tTopic]
            GROUP BY [Owner]) t
 ON u.[OriginalLogin] = t.[Owner]      
 LEFT JOIN (SELECT [Owner]
             , COUNT(*) AS [DocumentsOwned]
            FROM [SQLRunbook].[tDocument]
            GROUP BY [Owner]) d
 ON u.[OriginalLogin] = d.[Owner]      
 WHERE u.[OriginalLogin] = @OriginalLogin;

END;

GO

GRANT EXECUTE ON [SQLRunbook].[pUserSelectDetails] TO [SQLRunbookUserRole];

GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLRunbook'
           AND SPECIFIC_NAME = N'pUserSelectAllWithContributorScoring' )
 DROP PROCEDURE [SQLRunbook].[pUserSelectAllWithContributorScoring];

GO
/******************************************************************************
**	Auth: Bill Wunder
**	Date: April 1, 2007
**
**	Desc: Get attributes for a Runbook Contributor  
**
*******************************************************************************
**	Change History
*******************************************************************************
**	Date		   Author		     Description of Change
**	          
*******************************************************************************/
CREATE PROCEDURE [SQLRunbook].[pUserSelectAllWithContributorScoring]  
AS
BEGIN

 SELECT u.[OriginalLogin]
   , u.[FriendlyName]
   , ISNULL(t.[TopicsOwned], 0) AS [TopicsOwned]
   , ISNULL(t.[OwnedTopicPeerRating], 0) AS [OwnedTopicPeerRating] 
   , ISNULL(d.[DocumentsOwned], 0) AS [DocumentsOwned]
   , ISNULL(d.[OwnedDocumentPeerRating], 0) AS [OwnedDocumentPeerRating] 
   , u.[LastContributionDt]
   , IsNull(u.[CategoriesAdded], 0) + ISNULL(u.[CategoriesUpdated],0) + ISNULL(u.[CategoriesDeleted],0)
     + (ISNULL(u.[CategoryTopicsAdded],0) * 2) + ISNULL(u.[CategoryTopicsDeleted],0) 
     + (ISNULL(u.[TopicsAdded],0) * 4) + (ISNULL(u.[TopicsUpdated],0) * 2) + ISNULL(u.[TopicsDeleted],0) + (ISNULL(u.[TopicsRated],0) * 3) + (ISNULL(t.[TopicsOwned], 0) * (6 - ISNULL(t.[OwnedTopicPeerRating], 0)))
     + (ISNULL(u.[TopicDocumentsAdded],0) * 2) +  ISNULL(u.[TopicDocumentsDeleted],0) 
     + (ISNULL(u.[DocumentsAdded],0) * 5) + (ISNULL(u.[DocumentsUpdated],0) * 3) + ISNULL(u.[DocumentsDeleted],0) + (ISNULL(u.[DocumentsRated],0) * 3) + (ISNULL(d.[DocumentsOwned],0) * (6 - ISNULL(d.[OwnedDocumentPeerRating],0)))
     * CASE WHEN u.[LastContributionDt] > CURRENT_TIMESTAMP - 2 THEN 3
            WHEN u.[LastContributionDt] > CURRENT_TIMESTAMP - 4 THEN 2 
            ELSE 1 END AS [ContributorScore]
 FROM [SQLRunbook].[tUser] u
 LEFT JOIN (SELECT t1.[Owner]
            , COUNT(t1.Id) AS [TopicsOwned]
            , CEILING(AVG(tr1.RatingId)) AS [OwnedTopicPeerRating]
           FROM [SQLRunbook].[tTopic] t1
           LEFT JOIN [SQLRunbook].[tTopicRating] tr1
           ON t1.[Id] = tr1.[TopicId]
           AND tr1.[RatingId] BETWEEN 1 AND 6
           GROUP BY [Owner]) t
 ON u.[OriginalLogin] = t.[Owner]      
 LEFT JOIN (SELECT [Owner]
            , COUNT(*) AS [DocumentsOwned]
            , CEILING(AVG(dr1.RatingId)) AS [OwnedDocumentPeerRating] 
           FROM [SQLRunbook].[tDocument] d1
           LEFT JOIN [SQLRunbook].[tDocumentRating] dr1
           ON d1.[Id] = dr1.[DocumentId]
           AND dr1.[RatingId] BETWEEN 1 AND 6
           GROUP BY [Owner]) d
 ON u.[OriginalLogin] = d.[Owner]      
 ORDER BY [ContributorScore] DESC;
 
END

GO

GRANT EXECUTE ON [SQLRunbook].[pUserSelectAllWithContributorScoring] TO [SQLRunbookUserRole];

GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLRunbook'
           AND SPECIFIC_NAME = N'pCategorySelectAll' )
 DROP PROCEDURE [SQLRunbook].[pCategorySelectAll];

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
CREATE PROCEDURE [SQLRunbook].[pCategorySelectAll] 
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

GRANT EXECUTE ON [SQLRunbook].[pCategorySelectAll] TO [SQLRunbookUserRole];

GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLRunbook'
           AND SPECIFIC_NAME = N'pCategorySelectByDateRange' )
 DROP PROCEDURE [SQLRunbook].[pCategorySelectByDateRange];

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
CREATE PROCEDURE [SQLRunbook].[pCategorySelectByDateRange] 
 ( @BeginDt [DATETIME]
 , @EndDt [DATETIME] )
AS
BEGIN

 SELECT c.[Id]
  , c.[Name]
  , c.[Notes]
  , COUNT(ct.[TopicId]) AS [TopicCount]
  , SUM(ISNULL(cr.[RatingId],0)) AS [RatingTally]
  , COUNT(cr.[RatingId]) AS [RatingCount]
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

GRANT EXECUTE ON [SQLRunbook].[pCategorySelectByDateRange] TO [SQLRunbookUserRole];

GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLRunbook'
           AND SPECIFIC_NAME = N'pCategorySelectActive' )
 DROP PROCEDURE [SQLRunbook].[pCategorySelectActive];

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
CREATE PROCEDURE [SQLRunbook].[pCategorySelectActive] 
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

GRANT EXECUTE ON [SQLRunbook].[pCategorySelectActive] TO [SQLRunbookUserRole];

GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLRunbook'
           AND SPECIFIC_NAME = N'pCategorySelectByName' )
 DROP PROCEDURE [SQLRunbook].[pCategorySelectByName];

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
CREATE PROCEDURE [SQLRunbook].[pCategorySelectByName]  
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

GRANT EXECUTE ON [SQLRunbook].[pCategorySelectByName] TO [SQLRunbookUserRole];

GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLRunbook'
           AND SPECIFIC_NAME = N'pCategoryContains' )
 DROP PROCEDURE [SQLRunbook].[pCategoryContains];

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
CREATE PROCEDURE [SQLRunbook].[pCategoryContains] 
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

GRANT EXECUTE ON [SQLRunbook].[pCategoryContains] TO [SQLRunbookUserRole];

GO

    IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLRunbook'
           AND SPECIFIC_NAME = N'pCategoryInsert' )
 DROP PROCEDURE [SQLRunbook].[pCategoryInsert];

GO
/******************************************************************************
**	Auth: Bill Wunder
**	Date: April 1, 2007
**
**	Desc: Add a Runbook Category   
**
*******************************************************************************
**	Change History
*******************************************************************************
**	Date		   Author		     Description of Change
**	          
*******************************************************************************/
CREATE PROCEDURE [SQLRunbook].[pCategoryInsert] 
 ( @Name [NVARCHAR] (128) 
 , @Notes [NVARCHAR] (MAX)) 
AS
BEGIN
DECLARE @TextData [NVARCHAR] (MAX);

 SET XACT_ABORT ON;

 BEGIN TRY
 
  INSERT [SQLRunbook].[tCategory] ([Name], [Notes])
  VALUES (@Name, @Notes);

 END TRY

 BEGIN CATCH

  SET @TextData = '   @Name = ' + ISNULL(CHAR(39) + @Name + CHAR(39), 'NULL') + CHAR(13) + CHAR(10)
                + ' , @Notes = ' + ISNULL(CHAR(39) + @Notes + CHAR(39), 'NULL');  
  EXEC [SQLRunbook].[pLogSQLError] @TextData, @@PROCID;

 END CATCH 

END;

GO

GRANT EXECUTE ON [SQLRunbook].[pCategoryInsert] TO [SQLRunbookContributorRole];

GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLRunbook'
           AND SPECIFIC_NAME = N'pCategoryRatingUpsert' )
 DROP PROCEDURE [SQLRunbook].[pCategoryRatingUpsert];

GO
/******************************************************************************
**	Auth: Bill Wunder
**	Date: April 1, 2007
**
**	Desc: Rate a Runbook Category   
**
*******************************************************************************
**	Change History
*******************************************************************************
**	Date		   Author		     Description of Change
**	          
*******************************************************************************/
CREATE PROCEDURE [SQLRunbook].[pCategoryRatingUpsert] 
 ( @Id [INT]
 , @RatingId [INT]  
 , @Notes [NVARCHAR] (MAX)) 
AS
BEGIN
DECLARE @TextData [NVARCHAR] (MAX);

 SET XACT_ABORT ON;

 BEGIN TRY

  UPDATE [SQLRunbook].[tCategoryRating] 
  SET [RatingId] = @RatingId
    , [Notes] = @Notes
  WHERE [CategoryId] = @Id
  AND RecCreatedUser = ORIGINAL_LOGIN()

  IF @@ROWCOUNT = 0
   INSERT [SQLRunbook].[tCategoryRating] ([CategoryId], [RatingId], [Notes])
   VALUES (@Id, @RatingId, @Notes);

 END TRY

 BEGIN CATCH

  SET @TextData = '   @Id = ' + CONVERT([NVARCHAR](10), @Id) + CHAR(13) + CHAR(10)
                + '   @RatingId = ' + CONVERT([NVARCHAR](10), @RatingId) + CHAR(13) + CHAR(10)
                + ' , @Notes = ' + ISNULL(CHAR(39) + @Notes + CHAR(39), 'NULL');  
  EXEC [SQLRunbook].[pLogSQLError] @TextData, @@PROCID;

 END CATCH 

END;

GO

GRANT EXECUTE ON [SQLRunbook].[pCategoryRatingUpsert] TO [SQLRunbookUserRole];

GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLRunbook'
           AND SPECIFIC_NAME = N'pCategoryUpdate' )
 DROP PROCEDURE [SQLRunbook].[pCategoryUpdate];

GO
/******************************************************************************
**	Auth: Bill Wunder
**	Date: April 1, 2007
**
**	Desc: Modify the name or notes column of a category   
**
*******************************************************************************
**	Change History
*******************************************************************************
**	Date		   Author		     Description of Change
**	          
*******************************************************************************/
CREATE PROCEDURE [SQLRunbook].[pCategoryUpdate] 
 ( @Id [INT]
 , @Name [NVARCHAR] (128) 
 , @Notes [NVARCHAR] (MAX) ) 
AS
BEGIN
DECLARE @TextData [NVARCHAR] (MAX);

 SET XACT_ABORT ON;

 BEGIN TRY

  -- if this is a name change, add a note too
  UPDATE [SQLRunbook].[tCategory] 
  SET [Name] = @Name
    , [Notes] = @Notes
  WHERE [Id] = @Id;

 END TRY

 BEGIN CATCH

  SET @TextData = '   @Id = ' + ISNULL(CONVERT([NVARCHAR] (10), @Id),'NULL') + CHAR(13) + CHAR(10)
                + ' , @Name = ' + ISNULL(CHAR(39) + @Name + CHAR(39), 'NULL') + CHAR(13) + CHAR(10)
                + ' , @Notes = ' + ISNULL(CHAR(39) + @Notes + CHAR(39), 'NULL');  
  EXEC [SQLRunbook].[pLogSQLError] @TextData, @@PROCID;

 END CATCH 

END;

GO

GRANT EXECUTE ON [SQLRunbook].[pCategoryUpdate] TO [SQLRunbookContributorRole];

GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLRunbook'
           AND SPECIFIC_NAME = N'pCategoryDelete' )
 DROP PROCEDURE [SQLRunbook].[pCategoryDelete];

GO
/******************************************************************************
**	Auth: Bill Wunder
**	Date: April 1, 2007
**
**	Desc: delete a runbook category   
**
*******************************************************************************
**	Change History
*******************************************************************************
**	Date		   Author		     Description of Change
**	          
*******************************************************************************/
CREATE PROCEDURE [SQLRunbook].[pCategoryDelete] 
 ( @Id [INT] ) 
AS
BEGIN
DECLARE @TextData [NVARCHAR] (MAX);

 SET XACT_ABORT ON;

 BEGIN TRY

  WHILE EXISTS (SELECT * FROM [SQLRunbook].[tCategoryRating]
                WHERE [CategoryId] = @Id)
   DELETE TOP (1) [SQLRunbook].[tCategoryRating]
   WHERE [CategoryId] = @Id;

  -- this will leave topics that have no category associated
  WHILE EXISTS (SELECT * FROM [SQLRunbook].[tCategoryTopic]
                WHERE [CategoryId] = @Id)   
   DELETE TOP (1) [SQLRunbook].[tCategoryTopic]
   WHERE [CategoryId] = @Id;

  DELETE [SQLRunbook].[tCategory] 
  WHERE [Id] = @Id;
  
 END TRY

 BEGIN CATCH

  SET @TextData ='   @Id = ' + ISNULL(CONVERT([NVARCHAR] (10), @Id),'NULL') + CHAR(13) + CHAR(10)
  EXEC [SQLRunbook].[pLogSQLError] @TextData, @@PROCID;

 END CATCH 

END;

GO

GRANT EXECUTE ON [SQLRunbook].[pCategoryDelete] TO [SQLRunbookContributorRole];

GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLRunbook'
           AND SPECIFIC_NAME = N'pCategoryTopicSelectAll' )
 DROP PROCEDURE [SQLRunbook].[pCategoryTopicSelectAll];

GO
/******************************************************************************
**	Auth: Bill Wunder
**	Date: April 1, 2007
**
**	Desc: get all category/topic xref rows   
**
*******************************************************************************
**	Change History
*******************************************************************************
**	Date		   Author		     Description of Change
**	          
*******************************************************************************/
CREATE PROCEDURE [SQLRunbook].[pCategoryTopicSelectAll] 
AS
BEGIN

 SELECT [CategoryId]
  ,[TopicId]
 FROM [SQLRunbook].[tCategoryTopic];

END;

GO

GRANT EXECUTE ON [SQLRunbook].[pCategoryTopicSelectAll] TO [SQLRunbookUserRole];

GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLRunbook'
           AND SPECIFIC_NAME = N'pCategoryTopicSelectByCategoryId' )
 DROP PROCEDURE [SQLRunbook].[pCategoryTopicSelectByCategoryId];

GO
/******************************************************************************
**	Auth: Bill Wunder
**	Date: April 1, 2007
**
**	Desc: get category/topic xref rows for a category   
**
*******************************************************************************
**	Change History
*******************************************************************************
**	Date		   Author		     Description of Change
**	          
*******************************************************************************/
CREATE PROCEDURE [SQLRunbook].[pCategoryTopicSelectByCategoryId] 
 ( @CategoryId [INT] )
AS
BEGIN

 SELECT [CategoryId]
  ,[TopicId]
 FROM [SQLRunbook].[tCategoryTopic]
 WHERE [CategoryId] = @CategoryId;

END;

GO

GRANT EXECUTE ON [SQLRunbook].[pCategoryTopicSelectByCategoryId] TO [SQLRunbookUserRole];

GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLRunbook'
           AND SPECIFIC_NAME = N'pCategoryTopicSelectByTopicId' )
 DROP PROCEDURE [SQLRunbook].[pCategoryTopicSelectByTopicId];

GO
/******************************************************************************
**	Auth: Bill Wunder
**	Date: April 1, 2007
**
**	Desc: get all category/topic xref rows for a topic  
**
*******************************************************************************
**	Change History
*******************************************************************************
**	Date		   Author		     Description of Change
**	          
*******************************************************************************/
CREATE PROCEDURE [SQLRunbook].[pCategoryTopicSelectByTopicId] 
 ( @TopicId [INT] )
AS
BEGIN

 SELECT [CategoryId]
  ,[TopicId]
 FROM [SQLRunbook].[tCategoryTopic]
 WHERE [TopicId] = @TopicId;

END;

GO

GRANT EXECUTE ON [SQLRunbook].[pCategoryTopicSelectByTopicId] TO [SQLRunbookUserRole];

GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLRunbook'
           AND SPECIFIC_NAME = N'pCategoryTopicSelectByDateRange' )
 DROP PROCEDURE [SQLRunbook].[pCategoryTopicSelectByDateRange];

GO
/******************************************************************************
**	Auth: Bill Wunder
**	Date: April 1, 2007
**
**	Desc: get all category/topic xref rows for a date range with names
**
*******************************************************************************
**	Change History
*******************************************************************************
**	Date		   Author		     Description of Change
**	          
*******************************************************************************/
CREATE PROCEDURE [SQLRunbook].[pCategoryTopicSelectByDateRange] 
 ( @BeginDt [DATETIME]
 , @EndDt [DATETIME] )
AS
BEGIN

 SELECT ct.[CategoryId]
  , ct.[TopicId]
  , c.[Name] AS [CategoryName]
  , t.[Name] AS [TopicName]
  , ct.[RecCreatedDt]
  , ct.[RecCreatedUser]
 FROM [SQLRunbook].[tCategoryTopic] ct
 JOIN [SQLRunbook].[tCategory] c
 ON ct.[CategoryId] = c.[Id]
 JOIN [SQLRunbook].[tTopic] t
 ON ct.[TopicId] = t.[Id]
 WHERE ct.[RecCreatedDt] BETWEEN @BeginDt AND @EndDt
 ORDER BY [RecCreatedDt]
  , [CategoryName]
  , [TopicName];

END;

GO

GRANT EXECUTE ON [SQLRunbook].[pCategoryTopicSelectByDateRange] TO [SQLRunbookUserRole];

GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLRunbook'
           AND SPECIFIC_NAME = N'pCategoryTopicInsert' )
 DROP PROCEDURE [SQLRunbook].[pCategoryTopicInsert];

GO
/******************************************************************************
**	Auth: Bill Wunder
**	Date: April 1, 2007
**
**	Desc: add category/topic xref row at a time   
**
*******************************************************************************
**	Change History
*******************************************************************************
**	Date		   Author		     Description of Change
**	          
*******************************************************************************/
CREATE PROCEDURE [SQLRunbook].[pCategoryTopicInsert] 
 ( @CategoryId [INT]
 , @TopicId [INT] ) 
AS
BEGIN
DECLARE @TextData [NVARCHAR] (MAX);

 SET XACT_ABORT ON;
 
 BEGIN TRY
 
  IF NOT EXISTS(SELECT * FROM [SQLRunbook].[tCategoryTopic]
                WHERE [CategoryId] = @CategoryId
                AND [TopicId] = @TopicId)
   INSERT [SQLRunbook].[tCategoryTopic] ([CategoryId],[TopicId])
   VALUES (@CategoryId, @TopicId); 

 END TRY

 BEGIN CATCH

  SET @TextData = '   @CategoryId = ' + ISNULL(CONVERT([NVARCHAR] (10), @CategoryId),'NULL') + CHAR(13) + CHAR(10)
                + ' , @TopicId = ' +  ISNULL(CONVERT([NVARCHAR] (10),@TopicId),'NULL');
  EXEC [SQLRunbook].[pLogSQLError] @TextData, @@PROCID;

 END CATCH

END;

GO

GRANT EXECUTE ON [SQLRunbook].[pCategoryTopicInsert] TO [SQLRunbookContributorRole];

GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLRunbook'
           AND SPECIFIC_NAME = N'pCategoryTopicDelete' )
 DROP PROCEDURE [SQLRunbook].[pCategoryTopicDelete];

GO
/******************************************************************************
**	Auth: Bill Wunder
**	Date: April 1, 2007
**
**	Desc: delete a runbook category topic xref row  
**
*******************************************************************************
**	Change History
*******************************************************************************
**	Date		   Author		     Description of Change
**	          
*******************************************************************************/
CREATE PROCEDURE [SQLRunbook].[pCategoryTopicDelete] 
 ( @CategoryId [INT]
 , @TopicId [INT] ) 
AS
BEGIN
DECLARE @TextData [NVARCHAR] (MAX);

 SET XACT_ABORT ON;

 BEGIN TRY
  
  DELETE [SQLRunbook].[tCategoryTopic]
  WHERE [CategoryId] = @CategoryId
  AND [TopicId] = @TopicId;

 END TRY

 BEGIN CATCH

  SET @TextData = '   @CategoryId = ' + ISNULL(CONVERT([NVARCHAR] (10), @CategoryId),'NULL') + CHAR(13) + CHAR(10)
                + ' , @TopicId = ' +  ISNULL(CONVERT([NVARCHAR] (10),@TopicId),'NULL');
  EXEC [SQLRunbook].[pLogSQLError] @TextData, @@PROCID;

 END CATCH

END;

GO

GRANT EXECUTE ON [SQLRunbook].[pCategoryTopicDelete] TO [SQLRunbookContributorRole];

GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLRunbook'
           AND SPECIFIC_NAME = N'pTopicRatingSelectByDateRange' )
 DROP PROCEDURE [SQLRunbook].[pTopicRatingSelectByDateRange];

GO
/******************************************************************************
**	Auth: Bill Wunder
**	Date: April 1, 2007
**
**	Desc: all topic ratings in date range
**
*******************************************************************************
**	Change History
*******************************************************************************
**	Date		   Author		     Description of Change
**	          
*******************************************************************************/
CREATE PROCEDURE [SQLRunbook].[pTopicRatingSelectByDateRange]
 ( @BeginDt [DATETIME]
 , @EndDt [DATETIME] )
AS
BEGIN

 SELECT t.[Id] AS [TopicId]
  , t.[Name]
  , t.[Owner]
  , t.[LastUpdatedDt] AS [TopicLastUpdatedDt]
  , t.[LastUpdatedUser] AS [TopicLastUpdatedUser]
  , tr.[Id] AS [TopicRatingId]
  , tr.[RatingId]
  , tr.[Notes]
  , tr.[RecCreatedDt]
  , tr.[RecCreatedUser]
  , tr.[LastUpdatedDt] AS [RatingLastUpdatedDt]
  , tr.[LastUpdatedUser] AS [RatingLastUpdatedUser]
 FROM [SQLRunbook].[tTopicRating] tr
 LEFT JOIN [SQLRunbook].[tTopic] t
 ON tr.[TopicId] = t.[Id]
 WHERE tr.[LastUpdatedDt] BETWEEN @BeginDt AND @EndDT;

END;

GO

GRANT EXECUTE ON [SQLRunbook].[pTopicRatingSelectByDateRange] TO [SQLRunbookUserRole];

GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLRunbook'
           AND SPECIFIC_NAME = N'pTopicRatingSelectByOwner' )
 DROP PROCEDURE [SQLRunbook].[pTopicRatingSelectByOwner];

GO
/******************************************************************************
**	Auth: Bill Wunder
**	Date: April 1, 2007
**
**	Desc: all ratings for topics owned by a user
**
*******************************************************************************
**	Change History
*******************************************************************************
**	Date		   Author		     Description of Change
**	          
*******************************************************************************/
CREATE PROCEDURE [SQLRunbook].[pTopicRatingSelectByOwner]
 ( @OriginalLogin [NVARCHAR] (128) )
AS
BEGIN

 SELECT t.[Id] AS [TopicId]
  , t.[Name]
  , t.[Owner]
  , t.[LastUpdatedDt] AS [TopicLastUpdatedDt]
  , t.[LastUpdatedUser] AS [TopicLastUpdatedUser]
  , tr.[Id] AS [TopicRatingId]
  , tr.[RatingId]
  , tr.[Notes]
  , tr.[RecCreatedDt]
  , tr.[RecCreatedUser]
  , tr.[LastUpdatedDt] AS [RatingLastUpdatedDt]
  , tr.[LastUpdatedUser] AS [RatingLastUpdatedUser]
 FROM [SQLRunbook].[tTopicRating] tr
 LEFT JOIN [SQLRunbook].[tTopic] t
 ON tr.[TopicId] = t.[Id]
 WHERE t.[Owner] = @OriginalLogin;

END;

GO

GRANT EXECUTE ON [SQLRunbook].[pTopicRatingSelectByOwner] TO [SQLRunbookUserRole];

GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLRunbook'
           AND SPECIFIC_NAME = N'pTopicRatingSelectByReviewer' )
 DROP PROCEDURE [SQLRunbook].[pTopicRatingSelectByReviewer];

GO
/******************************************************************************
**	Auth: Bill Wunder
**	Date: April 1, 2007
**
**	Desc: all topic ratings completed by a user
**
*******************************************************************************
**	Change History
*******************************************************************************
**	Date		   Author		     Description of Change
**	          
*******************************************************************************/
CREATE PROCEDURE [SQLRunbook].[pTopicRatingSelectByReviewer]
 ( @OriginalLogin [NVARCHAR] (128) )
AS
BEGIN

 SELECT t.[Id] AS [TopicId]
  , t.[Name]
  , t.[Owner]
  , t.[LastUpdatedDt] AS [TopicLastUpdatedDt]
  , t.[LastUpdatedUser] AS [TopicLastUpdatedUser]
  , tr.[Id] AS [TopicRatingId]
  , tr.[RatingId]
  , tr.[Notes]
  , tr.[RecCreatedDt]
  , tr.[RecCreatedUser]
  , tr.[LastUpdatedDt] AS [RatingLastUpdatedDt]
  , tr.[LastUpdatedUser] AS [RatingLastUpdatedUser]
 FROM [SQLRunbook].[tTopicRating] tr
 LEFT JOIN [SQLRunbook].[tTopic] t
 ON tr.[TopicId] = t.[Id]
 WHERE tr.[RecCreatedUser] = @OriginalLogin;

END;

GO

GRANT EXECUTE ON [SQLRunbook].[pTopicRatingSelectByReviewer] TO [SQLRunbookUserRole];

GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLRunbook'
           AND SPECIFIC_NAME = N'pTopicRatingSelectByTopicName' )
 DROP PROCEDURE [SQLRunbook].[pTopicRatingSelectByTopicName];

GO
/******************************************************************************
**	Auth: Bill Wunder
**	Date: April 1, 2007
**
**	Desc: all topic ratings for a topic
**
*******************************************************************************
**	Change History
*******************************************************************************
**	Date		   Author		     Description of Change
**	          
*******************************************************************************/
CREATE PROCEDURE [SQLRunbook].[pTopicRatingSelectByTopicName]
 ( @Name [NVARCHAR] (128) )
AS
BEGIN

 SELECT t.[Id] AS [TopicId]
  , t.[Name]
  , t.[Owner]
  , t.[LastUpdatedDt] AS [TopicLastUpdatedDt]
  , t.[LastUpdatedUser] AS [TopicLastUpdatedUser]
  , tr.[Id] AS [TopicRatingId]
  , tr.[RatingId]
  , tr.[Notes]
  , tr.[RecCreatedDt]
  , tr.[RecCreatedUser]
  , tr.[LastUpdatedDt] AS [RatingLastUpdatedDt]
  , tr.[LastUpdatedUser] AS [RatingLastUpdatedUser]
 FROM [SQLRunbook].[tTopicRating] tr
 LEFT JOIN [SQLRunbook].[tTopic] t
 ON tr.[TopicId] = t.[Id]
 WHERE t.[Name] = @Name;

END;

GO

GRANT EXECUTE ON [SQLRunbook].[pTopicRatingSelectByTopicName] TO [SQLRunbookUserRole];

GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLRunbook'
           AND SPECIFIC_NAME = N'pTopicRatingSelectExpiredByReviewer' )
 DROP PROCEDURE [SQLRunbook].[pTopicRatingSelectExpiredByReviewer];

GO
/******************************************************************************
**	Auth: Bill Wunder
**	Date: April 1, 2007
**
**	Desc: all topic ratings older than the last change of the topic by rater
**
*******************************************************************************
**	Change History
*******************************************************************************
**	Date		   Author		     Description of Change
**	          
*******************************************************************************/
CREATE PROCEDURE [SQLRunbook].[pTopicRatingSelectExpiredByReviewer]
 ( @OriginalLogin [NVARCHAR] (128) )
AS
BEGIN

 SELECT t.[Id] AS [TopicId]
  , t.[Name]
  , t.[Owner]
  , t.[LastUpdatedDt] AS [TopicLastUpdatedDt]
  , t.[LastUpdatedUser] AS [TopicLastUpdatedUser]
  , tr.[Id] AS [TopicRatingId]
  , tr.[RatingId]
  , tr.[Notes]
  , tr.[RecCreatedDt]
  , tr.[RecCreatedUser]
  , tr.[LastUpdatedDt] AS [RatingLastUpdatedDt]
  , tr.[LastUpdatedUser] AS [RatingLastUpdatedUser]
 FROM [SQLRunbook].[tTopicRating] tr
 LEFT JOIN [SQLRunbook].[tTopic] t
 ON tr.[TopicId] = t.[Id]
 WHERE tr.[RecCreatedUser] = @OriginalLogin
 AND tr.[LastUpdatedDt] < t.[LastUpdatedDt]

END;

GO

GRANT EXECUTE ON [SQLRunbook].[pTopicRatingSelectExpiredByReviewer] TO [SQLRunbookUserRole];

GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLRunbook'
           AND SPECIFIC_NAME = N'pTopicRatingUpsert' )
 DROP PROCEDURE [SQLRunbook].[pTopicRatingUpsert];

GO
/******************************************************************************
**	Auth: Bill Wunder
**	Date: April 1, 2007
**
**	Desc: Rate a Runbook Topic   
**
*******************************************************************************
**	Change History
*******************************************************************************
**	Date		   Author		     Description of Change
**	          
*******************************************************************************/
CREATE PROCEDURE [SQLRunbook].[pTopicRatingUpsert] 
 ( @Id [INT]
 , @RatingId [INT]  
 , @Notes [NVARCHAR] (MAX)) 
AS
BEGIN
DECLARE @TextData [NVARCHAR] (MAX);

 SET XACT_ABORT ON;

 BEGIN TRY

  UPDATE [SQLRunbook].[tTopicRating] 
  SET [RatingId] = @RatingId
    , [Notes] = @Notes
  WHERE [TopicId] = @Id
  AND [RecCreatedUser] = ORIGINAL_LOGIN()

  IF @@ROWCOUNT = 0
   INSERT [SQLRunbook].[tTopicRating] ([TopicId], [RatingId], [Notes])
   VALUES (@Id, @RatingId, @Notes);

  END TRY

  BEGIN CATCH

   SET @TextData = '   @Id = ' + CONVERT([NVARCHAR](10), @Id) + CHAR(13) + CHAR(10)
                 + '   @RatingId = ' + CONVERT([NVARCHAR](10), @RatingId) + CHAR(13) + CHAR(10)
                 + ' , @Notes = ' + ISNULL(CHAR(39) + @Notes + CHAR(39), 'NULL');  
   EXEC [SQLRunbook].[pLogSQLError] @TextData, @@PROCID;

  END CATCH 

END;

GO

GRANT EXECUTE ON [SQLRunbook].[pTopicRatingUpsert] TO [SQLRunbookUserRole];

GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLRunbook'
           AND SPECIFIC_NAME = N'pTopicRatingGetByUser' )
 DROP PROCEDURE [SQLRunbook].[pTopicRatingGetByUser];

GO
/******************************************************************************
**	Auth: Bill Wunder
**	Date: April 1, 2007
**
**	Desc: get the users rating of the topic 
**
*******************************************************************************
**	Change History
*******************************************************************************
**	Date		   Author		     Description of Change
**	          
*******************************************************************************/
CREATE PROCEDURE [SQLRunbook].[pTopicRatingGetByUser]
 ( @Id [INT]
 , @User [NVARCHAR](128)
 , @Rating [INT] OUTPUT
 , @Note [NVARCHAR] (MAX) OUTPUT 
 , @AvgRating [INT] OUTPUT
 , @NbrRatings [INT] OUTPUT )
AS
BEGIN

  -- zero if not rated, empty if no topic

 SELECT @Rating = ISNULL(tr.[RatingId],0)
  , @Note = ISNULL(tr.[Notes],'')
  , @AvgRating = ISNULL(tot.[AvgRating],0)
  , @NbrRatings = ISNULL(tot.[NbrRatings],0)
 FROM [SQLRunbook].[tTopic] t
 LEFT JOIN [SQLRunbook].[tTopicRating] tr
 ON t.[Id] = tr.[TopicId]
 AND tr.[RecCreatedUser] = @User
 LEFT JOIN (SELECT [TopicId], AVG(RatingId) AS [AvgRating], COUNT(RatingId) AS [NbrRatings] 
            FROM [SQLRunbook].[tTopicRating] 
            GROUP BY TopicId) tot
 ON t.[Id] = tot.[TopicId]
 WHERE t.[Id] = @Id;

END;

GO

GRANT EXECUTE ON [SQLRunbook].[pTopicRatingGetByUser] TO [SQLRunbookUserRole];

GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLRunbook'
           AND SPECIFIC_NAME = N'pDocumentRatingGetByUser' )
 DROP PROCEDURE [SQLRunbook].[pDocumentRatingGetByUser];

GO
/******************************************************************************
**	Auth: Bill Wunder
**	Date: April 1, 2007
**
**	Desc: get the users rating of the document 
**
*******************************************************************************
**	Change History
*******************************************************************************
**	Date		   Author		     Description of Change
**	          
*******************************************************************************/
CREATE PROCEDURE [SQLRunbook].[pDocumentRatingGetByUser]
 ( @Id [INT]
 , @User [NVARCHAR](128)
 , @Rating [INT] OUTPUT
 , @Note [NVARCHAR] (MAX) OUTPUT 
 , @AvgRating [INT] OUTPUT
 , @NbrRatings [INT] OUTPUT )
AS
BEGIN

  -- zero if not rated, empty if no document

 SELECT @Rating = ISNULL(dr.[RatingId],0)
  , @Note = ISNULL(dr.[Notes],'')
  , @AvgRating = ISNULL(tot.[AvgRating],0)
  , @NbrRatings = ISNULL(tot.[NbrRatings],0)
 FROM [SQLRunbook].[tDocument] d
 LEFT JOIN [SQLRunbook].[tDocumentRating] dr
 ON d.[Id] = dr.[DocumentId]
 AND dr.[RecCreatedUser] = @User
 LEFT JOIN (SELECT [DocumentId], AVG(RatingId) AS [AvgRating], COUNT(RatingId) AS [NbrRatings] 
            FROM [SQLRunbook].[tDocumentRating] 
            GROUP BY DocumentId) tot
 ON d.[Id] = tot.[DocumentId]
 WHERE d.[Id] = @Id;

END;

GO

GRANT EXECUTE ON [SQLRunbook].[pDocumentRatingGetByUser] TO [SQLRunbookUserRole];

GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLRunbook'
           AND SPECIFIC_NAME = N'pDocumentRatingSelectByDateRange' )
 DROP PROCEDURE [SQLRunbook].[pDocumentRatingSelectByDateRange];

GO
/******************************************************************************
**	Auth: Bill Wunder
**	Date: April 1, 2007
**
**	Desc: all user ratings in date range
**
*******************************************************************************
**	Change History
*******************************************************************************
**	Date		   Author		     Description of Change
**	          
*******************************************************************************/
CREATE PROCEDURE [SQLRunbook].[pDocumentRatingSelectByDateRange]
 ( @BeginDt [DATETIME]
 , @EndDt [DATETIME] )
AS
BEGIN

 SELECT d.[Id] AS [DocumentId]
  , d.[File]
  , d.[Owner]
  , d.[LastModifiedDt]
  , dr.[Id] AS [DocumentRatingId]
  , dr.[RatingId]
  , dr.[Notes]
  , dr.[RecCreatedDt]
  , dr.[RecCreatedUser]
  , dr.[LastUpdatedDt]
  , dr.[LastUpdatedUser]
 FROM [SQLRunbook].[tDocumentRating] dr
 LEFT JOIN [SQLRunbook].[tDocument] d
 ON dr.[DocumentId] = d.[Id]
 WHERE dr.[LastUpdatedDt] BETWEEN @BeginDt AND @EndDT;

END;

GO

GRANT EXECUTE ON [SQLRunbook].[pDocumentRatingSelectByDateRange] TO [SQLRunbookUserRole];

GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLRunbook'
           AND SPECIFIC_NAME = N'pDocumentRatingSelectByDocumentOwner' )
 DROP PROCEDURE [SQLRunbook].[pDocumentRatingSelectByDocumentOwner];

GO
/******************************************************************************
**	Auth: Bill Wunder
**	Date: April 1, 2007
**
**	Desc: all document ratings for documents owned by a user
**
*******************************************************************************
**	Change History
*******************************************************************************
**	Date		   Author		     Description of Change
**	          
*******************************************************************************/
CREATE PROCEDURE [SQLRunbook].[pDocumentRatingSelectByDocumentOwner]
 ( @OriginalLogin [NVARCHAR] (128) )
AS
BEGIN

 SELECT d.[Id] AS [DocumentId]
  , d.[File]
  , d.[Owner]
  , d.[LastModifiedDt]
  , dr.[Id] AS [DocumentRatingId]
  , dr.[RatingId]
  , dr.[Notes]
  , dr.[RecCreatedDt]
  , dr.[RecCreatedUser]
  , dr.[LastUpdatedDt]
  , dr.[LastUpdatedUser]
 FROM [SQLRunbook].[tDocumentRating] dr
 LEFT JOIN [SQLRunbook].[tDocument] d
 ON dr.[DocumentId] = d.[Id]
 WHERE d.[Owner] = @OriginalLogin;

END;

GO

GRANT EXECUTE ON [SQLRunbook].[pDocumentRatingSelectByDocumentOwner] TO [SQLRunbookUserRole];

GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLRunbook'
           AND SPECIFIC_NAME = N'pDocumentRatingSelectByReviewer' )
 DROP PROCEDURE [SQLRunbook].[pDocumentRatingSelectByReviewer];

GO
/******************************************************************************
**	Auth: Bill Wunder
**	Date: April 1, 2007
**
**	Desc: all document ratings completed by a user
**
*******************************************************************************
**	Change History
*******************************************************************************
**	Date		   Author		     Description of Change
**	Jan 29, 2010   bw                change SARG to last user from orig user          
**	          
*******************************************************************************/
CREATE PROCEDURE [SQLRunbook].[pDocumentRatingSelectByReviewer]
 ( @OriginalLogin [NVARCHAR] (128) )
AS
BEGIN

 SELECT d.[Id] AS [DocumentId]
  , d.[File]
  , d.[Owner]
  , d.[LastModifiedDt]
  , dr.[Id] AS [DocumentRatingId]
  , dr.[RatingId]
  , dr.[Notes]
  , dr.[RecCreatedDt]
  , dr.[RecCreatedUser]
  , dr.[LastUpdatedDt]
  , dr.[LastUpdatedUser]
 FROM [SQLRunbook].[tDocumentRating] dr
 LEFT JOIN [SQLRunbook].[tDocument] d
 ON dr.[DocumentId] = d.[Id]
 WHERE dr.[RecCreatedUser] = @OriginalLogin;

END;

GO

GRANT EXECUTE ON [SQLRunbook].[pDocumentRatingSelectByReviewer] TO [SQLRunbookUserRole];

GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLRunbook'
           AND SPECIFIC_NAME = N'pDocumentRatingSelectByDocumentId' )
 DROP PROCEDURE [SQLRunbook].[pDocumentRatingSelectByDocumentId];

GO
/******************************************************************************
**	Auth: Bill Wunder
**	Date: April 1, 2007
**
**	Desc: all document ratings completed on a documenmt
**
*******************************************************************************
**	Change History
*******************************************************************************
**	Date		   Author		     Description of Change
**	          
*******************************************************************************/
CREATE PROCEDURE [SQLRunbook].[pDocumentRatingSelectByDocumentId]
 ( @DocumentId [INT] )
AS
BEGIN

 SELECT d.[Id] AS [DocumentId]
  , d.[File]
  , d.[Owner]
  , d.[LastModifiedDt]
  , dr.[Id] AS [DocumentRatingId]
  , dr.[RatingId]
  , dr.[Notes]
  , dr.[RecCreatedDt]
  , dr.[RecCreatedUser]
  , dr.[LastUpdatedDt]
  , dr.[LastUpdatedUser]
 FROM [SQLRunbook].[tDocumentRating] dr
 LEFT JOIN [SQLRunbook].[tDocument] d
 ON dr.[DocumentId] = d.[Id]
 WHERE d.[Id] = @DocumentId;

END;

GO

GRANT EXECUTE ON [SQLRunbook].[pDocumentRatingSelectByDocumentId] TO [SQLRunbookUserRole];

GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLRunbook'
           AND SPECIFIC_NAME = N'pDocumentRatingSelectExpiredByReviewer' )
 DROP PROCEDURE [SQLRunbook].[pDocumentRatingSelectExpiredByReviewer];

GO
/******************************************************************************
**	Auth: Bill Wunder
**	Date: April 1, 2007
**
**	Desc: all document that have changed since rated by a user
**
*******************************************************************************
**	Change History
*******************************************************************************
**	Date		   Author		     Description of Change
**	          
*******************************************************************************/
CREATE PROCEDURE [SQLRunbook].[pDocumentRatingSelectExpiredByReviewer]
 ( @OriginalLogin [NVARCHAR] (128) )
AS
BEGIN

 SELECT d.[Id] AS [DocumentId]
  , d.[File]
  , d.[Owner]
  , d.[LastModifiedDt]
  , dr.[Id] AS [DocumentRatingId]
  , dr.[RatingId]
  , dr.[Notes]
  , dr.[RecCreatedDt]
  , dr.[RecCreatedUser]
  , dr.[LastUpdatedDt]
  , dr.[LastUpdatedUser]
 FROM [SQLRunbook].[tDocumentRating] dr
 LEFT JOIN [SQLRunbook].[tDocument] d
 ON dr.[DocumentId] = d.[Id]
 WHERE dr.[RecCreatedUser] = @OriginalLogin
 AND dr.[LastUpdatedDt] < d.[LastModifiedDt];

END;

GO

GRANT EXECUTE ON [SQLRunbook].[pDocumentRatingSelectExpiredByReviewer] TO [SQLRunbookUserRole];

GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLRunbook'
           AND SPECIFIC_NAME = N'pCategoryRatingGetByUser' )
 DROP PROCEDURE [SQLRunbook].[pCategoryRatingGetByUser];

GO
/******************************************************************************
**	Auth: Bill Wunder
**	Date: April 1, 2007
**
**	Desc: get the users rating of the category 
**
*******************************************************************************
**	Change History
*******************************************************************************
**	Date		   Author		     Description of Change
**	          
*******************************************************************************/
CREATE PROCEDURE [SQLRunbook].[pCategoryRatingGetByUser]
 ( @Id [INT]
 , @User [NVARCHAR](128)
 , @Rating [INT] OUTPUT
 , @Note [NVARCHAR] (MAX) OUTPUT 
 , @AvgRating [INT] OUTPUT
 , @NbrRatings [INT] OUTPUT )
AS
BEGIN

  -- zero if not rated, empty if no category

 SELECT @Rating = ISNULL(cr.[RatingId],0)
  , @Note = ISNULL(cr.[Notes],'')
  , @AvgRating = ISNULL(tot.[AvgRating],0)
  , @NbrRatings = ISNULL(tot.[NbrRatings],0)
 FROM [SQLRunbook].[tCategory] c
 LEFT JOIN [SQLRunbook].[tCategoryRating] cr
 ON c.[Id] = cr.[CategoryId]
 AND cr.[RecCreatedUser] = @User
 LEFT JOIN (SELECT [CategoryId], AVG(RatingId) AS [AvgRating], COUNT(RatingId) AS [NbrRatings] 
            FROM [SQLRunbook].[tCategoryRating] 
            GROUP BY CategoryId) tot
 ON c.[Id] = tot.[CategoryId]
 WHERE c.[Id] = @Id;

END;

GO

GRANT EXECUTE ON [SQLRunbook].[pCategoryRatingGetByUser] TO [SQLRunbookUserRole];

GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLRunbook'
           AND SPECIFIC_NAME = N'pTopicSelectAll' )
 DROP PROCEDURE [SQLRunbook].[pTopicSelectAll];

GO
/******************************************************************************
**	Auth: Bill Wunder
**	Date: April 1, 2007
**
**	Desc: all attributes for all Runbook entries 
**
*******************************************************************************
**	Change History
*******************************************************************************
**	Date		   Author		     Description of Change
**	          
*******************************************************************************/
CREATE PROCEDURE [SQLRunbook].[pTopicSelectAll]
AS
BEGIN

 SELECT t.[Id] 
  , t.[Name]
  , t.[Notes]
  , ISNULL(td.[DocumentCount],0) AS [DocumentCount] 
  , ISNULL(tr.[RatingTally],0) AS [RatingTally]
  , ISNULL(tr.[RatingCount],0) AS [RatingCount]
  , t.[Owner] 
  , t.[RecCreatedDt]
  , t.[RecCreatedUser]
  , t.[LastUpdatedDt]
  , t.[LastUpdatedUser]
 FROM [SQLRunbook].[tTopic] t
 LEFT JOIN (SELECT [TopicId]
             , SUM([RatingId]) AS [RatingTally]
             , COUNT([RatingId]) AS [RatingCount] 
            FROM [SQLRunbook].[tTopicRating]
            WHERE [RatingId] < 7 
            GROUP BY [TopicId]) tr
 ON t.[Id] = tr.[TopicId] 
 LEFT JOIN (SELECT [TopicId]
             , COUNT(*) AS [DocumentCount] 
            FROM [SQLRunbook].[tTopicDocument]
            GROUP BY [TopicId]) td
 ON t.[Id] = td.[TopicId] 
 ORDER BY [Name];


END;

GO

GRANT EXECUTE ON [SQLRunbook].[pTopicSelectAll] TO [SQLRunbookUserRole];

GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLRunbook'
           AND SPECIFIC_NAME = N'pTopicSelectByCategoryId' )
 DROP PROCEDURE [SQLRunbook].[pTopicSelectByCategoryId];

GO
/******************************************************************************
**	Auth: Bill Wunder
**	Date: April 1, 2007
**
**	Desc: Select list of all RunbookCategoryTopics for a category   
**
*******************************************************************************
**	Change History
*******************************************************************************
**	Date		   Author		     Description of Change
**	          
*******************************************************************************/
CREATE PROCEDURE [SQLRunbook].[pTopicSelectByCategoryId]
 ( @CategoryId [Int] )
AS
BEGIN

 SELECT DISTINCT t.[Id] 
  , t.[Name]
  , t.[Notes]
  , ISNULL(td.[DocumentCount],0) AS [DocumentCount] 
  , ISNULL(tr.[RatingTally],0) AS [RatingTally]
  , ISNULL(tr.[RatingCount],0) AS [RatingCount]
  , t.[Owner] 
  , t.[RecCreatedDt]
  , t.[RecCreatedUser]
  , t.[LastUpdatedDt]
  , t.[LastUpdatedUser]
 FROM [SQLRunbook].[tTopic] t
 LEFT JOIN (SELECT [TopicId]
             , SUM([RatingId]) AS [RatingTally]
             , COUNT([RatingId]) AS [RatingCount] 
            FROM [SQLRunbook].[tTopicRating]
            WHERE [RatingId] < 7 
            GROUP BY [TopicId]) tr
 ON t.[Id] = tr.[TopicId] 
 LEFT JOIN (SELECT [TopicId]
             , COUNT(*) AS [DocumentCount] 
            FROM [SQLRunbook].[tTopicDocument]
            GROUP BY [TopicId]) td
 ON t.[Id] = td.[TopicId] 
 JOIN [SQLRunbook].[tCategoryTopic] ct
 ON t.[Id] = ct.[TopicId]
 WHERE ct.[CategoryId] = @CategoryId
 ORDER BY [Name]; 

END;

GO

GRANT EXECUTE ON [SQLRunbook].[pTopicSelectByCategoryId] TO [SQLRunbookUserRole];

GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLRunbook'
           AND SPECIFIC_NAME = N'pTopicSelectByCategory' )
 DROP PROCEDURE [SQLRunbook].[pTopicSelectByCategory];

GO
/******************************************************************************
**	Auth: Bill Wunder
**	Date: April 1, 2007
**
**	Desc: Select list of all RunbookCategoryTopics for a or w/out a category 
**
*******************************************************************************
**	Change History
*******************************************************************************
**	Date		   Author		     Description of Change
**	          
*******************************************************************************/
CREATE PROCEDURE [SQLRunbook].[pTopicSelectByCategory]
 ( @Category [NVARCHAR] (128) )
AS
BEGIN

 SELECT DISTINCT t.[Id] 
  , t.[Name]
  , t.[Notes]
  , ISNULL(td.[DocumentCount],0) AS [DocumentCount] 
  , ISNULL(tr.[RatingTally],0) AS [RatingTally]
  , ISNULL(tr.[RatingCount],0) AS [RatingCount]
  , t.[Owner] 
  , t.[RecCreatedDt]
  , t.[RecCreatedUser]
  , t.[LastUpdatedDt]
  , t.[LastUpdatedUser]
 FROM [SQLRunbook].[tTopic] t
 LEFT JOIN (SELECT [TopicId]
             , SUM([RatingId]) AS [RatingTally]
             , COUNT([RatingId]) AS [RatingCount] 
            FROM [SQLRunbook].[tTopicRating]
            WHERE [RatingId] < 7 
            GROUP BY [TopicId]) tr
 ON t.[Id] = tr.[TopicId] 
 LEFT JOIN (SELECT [TopicId]
             , COUNT(*) AS [DocumentCount] 
            FROM [SQLRunbook].[tTopicDocument]
            GROUP BY [TopicId]) td
 ON t.[Id] = td.[TopicId] 
 LEFT JOIN [SQLRunbook].[tCategoryTopic] ct
 ON t.[Id] = ct.[TopicId]
 LEFT JOIN [SQLRunbook].[tCategory] c
 ON ct.[CategoryId] = c.[Id]
 WHERE c.[Name] = @Category
 OR (ISNULL(@Category, '') = '' AND c.[Name] IS NULL)
 ORDER BY [Name]; 

END;

GO

GRANT EXECUTE ON [SQLRunbook].[pTopicSelectByCategory] TO [SQLRunbookUserRole];

GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLRunbook'
           AND SPECIFIC_NAME = N'pTopicSelectByName' )
 DROP PROCEDURE [SQLRunbook].[pTopicSelectByName];

GO
/******************************************************************************
**	Auth: Bill Wunder
**	Date: April 1, 2007
**
**	Desc: Select a RunbookCategoryTopic by name (for catalog)   
**
*******************************************************************************
**	Change History
*******************************************************************************
**	Date		   Author		     Description of Change
**	          
*******************************************************************************/
CREATE PROCEDURE [SQLRunbook].[pTopicSelectByName]
 ( @Name [NVARCHAR] (128) )
AS
BEGIN

 SELECT DISTINCT t.[Id] 
  , t.[Name]
  , t.[Notes]
  , ISNULL(td.[DocumentCount],0) AS [DocumentCount] 
  , ISNULL(tr.[RatingTally],0) AS [RatingTally]
  , ISNULL(tr.[RatingCount],0) AS [RatingCount]
  , t.[Owner] 
  , t.[RecCreatedDt]
  , t.[RecCreatedUser]
  , t.[LastUpdatedDt]
  , t.[LastUpdatedUser]
 FROM [SQLRunbook].[tTopic] t
 LEFT JOIN (SELECT [TopicId]
             , SUM([RatingId]) AS [RatingTally]
             , COUNT([RatingId]) AS [RatingCount] 
            FROM [SQLRunbook].[tTopicRating]
            WHERE [RatingId] < 7 
            GROUP BY [TopicId]) tr
 ON t.[Id] = tr.[TopicId] 
 LEFT JOIN (SELECT [TopicId]
             , COUNT(*) AS [DocumentCount] 
            FROM [SQLRunbook].[tTopicDocument]
            GROUP BY [TopicId]) td
 ON t.[Id] = td.[TopicId] 
 LEFT JOIN [SQLRunbook].[tCategoryTopic] ct
 ON t.[Id] = ct.[TopicId]
 WHERE t.[Name] = @Name
 ORDER BY [Name]; 

END;

GO

GRANT EXECUTE ON [SQLRunbook].[pTopicSelectByName] TO [SQLRunbookUserRole];

GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLRunbook'
           AND SPECIFIC_NAME = N'pTopicSelectByDate' )
 DROP PROCEDURE [SQLRunbook].[pTopicSelectByDate];

GO
/******************************************************************************
**	Auth: Bill Wunder
**	Date: April 1, 2007
**
**	Desc: Select list of all Runbook topic entries for a date   
**
*******************************************************************************
**	Change History
*******************************************************************************
**	Date		   Author		     Description of Change
**	          
*******************************************************************************/
CREATE PROCEDURE [SQLRunbook].[pTopicSelectByDate]
 ( @Date [DATETIME] )
AS
BEGIN

 SELECT DISTINCT t.[Id] 
  , t.[Name]
  , t.[Notes]
  , ISNULL(td.[DocumentCount],0) AS [DocumentCount] 
  , ISNULL(tr.[RatingTally],0) AS [RatingTally]
  , ISNULL(tr.[RatingCount],0) AS [RatingCount]
  , t.[Owner] 
  , t.[RecCreatedDt]
  , t.[RecCreatedUser]
  , t.[LastUpdatedDt]
  , t.[LastUpdatedUser]
 FROM [SQLRunbook].[tTopic] t
 LEFT JOIN (SELECT [TopicId]
             , SUM([RatingId]) AS [RatingTally]
             , COUNT([RatingId]) AS [RatingCount] 
            FROM [SQLRunbook].[tTopicRating]
            WHERE [RatingId] < 7 
            GROUP BY [TopicId]) tr
 ON t.[Id] = tr.[TopicId] 
 LEFT JOIN (SELECT [TopicId]
             , COUNT(*) AS [DocumentCount] 
            FROM [SQLRunbook].[tTopicDocument]
            GROUP BY [TopicId]) td
 ON t.[Id] = td.[TopicId] 
 WHERE CONVERT([NVARCHAR](10),t.[RecCreatedDt], 101) = CONVERT([NVARCHAR](10), @Date, 101)   
 ORDER BY [Name]; 

END

GO

GRANT EXECUTE ON [SQLRunbook].[pTopicSelectByDate] TO [SQLRunbookUserRole];

GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLRunbook'
           AND SPECIFIC_NAME = N'pTopicSelectByDateRange' )
 DROP PROCEDURE [SQLRunbook].[pTopicSelectByDateRange];

GO
/******************************************************************************
**	Auth: Bill Wunder
**	Date: April 1, 2007
**
**	Desc: Select list of all Runbook topic entries for a date   
**
*******************************************************************************
**	Change History
*******************************************************************************
**	Date		   Author		     Description of Change
**	          
*******************************************************************************/
CREATE PROCEDURE [SQLRunbook].[pTopicSelectByDateRange]
 ( @BeginDt [DATETIME], 
   @EndDt [DATETIME] )
AS
BEGIN

 SELECT DISTINCT t.[Id] 
  , t.[Name]
  , t.[Notes]
  , ISNULL(td.[DocumentCount],0) AS [DocumentCount] 
  , ISNULL(tr.[RatingTally],0) AS [RatingTally]
  , ISNULL(tr.[RatingCount],0) AS [RatingCount]
  , t.[Owner] 
  , t.[RecCreatedDt]
  , t.[RecCreatedUser]
  , t.[LastUpdatedDt]
  , t.[LastUpdatedUser]
 FROM [SQLRunbook].[tTopic] t
 LEFT JOIN (SELECT [TopicId]
             , SUM([RatingId]) AS [RatingTally]
             , COUNT([RatingId]) AS [RatingCount] 
            FROM [SQLRunbook].[tTopicRating]
            WHERE [RatingId] < 7 
            GROUP BY [TopicId]) tr
 ON t.[Id] = tr.[TopicId] 
 LEFT JOIN (SELECT [TopicId]
             , COUNT(*) AS [DocumentCount] 
            FROM [SQLRunbook].[tTopicDocument]
            GROUP BY [TopicId]) td
 ON t.[Id] = td.[TopicId] 
 WHERE t.[RecCreatedDt] BETWEEN @BeginDt AND @EndDt   
 ORDER BY [RecCreatedDt]
  , [Name]; 

END

GO

GRANT EXECUTE ON [SQLRunbook].[pTopicSelectByDateRange] TO [SQLRunbookUserRole];

GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLRunbook'
           AND SPECIFIC_NAME = N'pTopicSelectChangedByDateRange' )
 DROP PROCEDURE [SQLRunbook].[pTopicSelectChangedByDateRange];

GO
/******************************************************************************
**	Auth: Bill Wunder
**	Date: April 1, 2007
**
**	Desc: Select list of all Runbook topic entries changed in the interval   
**
*******************************************************************************
**	Change History
*******************************************************************************
**	Date		   Author		     Description of Change
**	          
*******************************************************************************/
CREATE PROCEDURE [SQLRunbook].[pTopicSelectChangedByDateRange]
 ( @BeginDt [DATETIME],        
   @EndDt [DATETIME] )
AS
BEGIN

 SELECT DISTINCT t.[Id] 
  , t.[Name]
  , t.[Notes]
  , ISNULL(td.[DocumentCount],0) AS [DocumentCount] 
  , ISNULL(tr.[RatingTally],0) AS [RatingTally]
  , ISNULL(tr.[RatingCount],0) AS [RatingCount]
  , t.[Owner] 
  , t.[RecCreatedDt]
  , t.[RecCreatedUser]
  , t.[LastUpdatedDt]
  , t.[LastUpdatedUser]
 FROM [SQLRunbook].[tTopic] t
 LEFT JOIN (SELECT [TopicId]
             , SUM([RatingId]) AS [RatingTally]
             , COUNT([RatingId]) AS [RatingCount] 
            FROM [SQLRunbook].[tTopicRating]
            WHERE [RatingId] < 7 
            GROUP BY [TopicId]) tr
 ON t.[Id] = tr.[TopicId] 
 LEFT JOIN (SELECT [TopicId]
             , COUNT(*) AS [DocumentCount] 
            FROM [SQLRunbook].[tTopicDocument]
            GROUP BY [TopicId]) td
 ON t.[Id] = td.[TopicId] 
 WHERE t.[LastUpdatedDt] BETWEEN @BeginDt AND @EndDt   
 ORDER BY [LastUpdatedDt]
  , [Name]; 

END

GO

GRANT EXECUTE ON [SQLRunbook].[pTopicSelectChangedByDateRange] TO [SQLRunbookUserRole];

GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLRunbook'
           AND SPECIFIC_NAME = N'pTopicSelectByOwner' )
 DROP PROCEDURE [SQLRunbook].[pTopicSelectByOwner];

GO

/******************************************************************************
**	Auth: Bill Wunder
**	Date: April 1, 2007
**
**	Desc: Get all topics owned by this SQLClue Runbook User
**
*******************************************************************************
**	Change History
*******************************************************************************
**	Date		   Author		     Description of Change
**	          
*******************************************************************************/
CREATE PROCEDURE [SQLRunbook].[pTopicSelectByOwner] 
 ( @OriginalLogin [NVARCHAR](256) )
AS
BEGIN

 SELECT DISTINCT t.[Id] 
  , t.[Name]
  , t.[Notes]
  , ISNULL(td.[DocumentCount],0) AS [DocumentCount] 
  , ISNULL(tr.[RatingTally],0) AS [RatingTally]
  , ISNULL(tr.[RatingCount],0) AS [RatingCount]
  , t.[Owner] 
  , t.[RecCreatedDt]
  , t.[RecCreatedUser]
  , t.[LastUpdatedDt]
  , t.[LastUpdatedUser]
 FROM [SQLRunbook].[tTopic] t
 LEFT JOIN (SELECT [TopicId]
             , SUM([RatingId]) AS [RatingTally]
             , COUNT([RatingId]) AS [RatingCount] 
            FROM [SQLRunbook].[tTopicRating]
            WHERE [RatingId] < 7 
            GROUP BY [TopicId]) tr
 ON t.[Id] = tr.[TopicId] 
 LEFT JOIN (SELECT [TopicId]
             , COUNT(*) AS [DocumentCount] 
            FROM [SQLRunbook].[tTopicDocument]
            GROUP BY [TopicId]) td
 ON t.[Id] = td.[TopicId] 
 WHERE t.[Owner] = @OriginalLogin
 ORDER BY [Name]; 

END;
GO

GRANT EXECUTE ON [SQLRunbook].[pTopicSelectByOwner] TO [SQLRunbookUserRole];

GO
 
IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLRunbook'
           AND SPECIFIC_NAME = N'pTopicSelectByDocumentId' )
 DROP PROCEDURE [SQLRunbook].[pTopicSelectByDocumentId];

GO

/******************************************************************************
**	Auth: Bill Wunder
**	Date: April 1, 2007
**
**	Desc: Get all topics referencing a document   
**
*******************************************************************************
**	Change History
*******************************************************************************
**	Date		   Author		     Description of Change
**	          
*******************************************************************************/
CREATE PROCEDURE [SQLRunbook].[pTopicSelectByDocumentId] 
 ( @DocumentId [INT] )
AS
BEGIN

 SELECT DISTINCT t.[Id] 
  , t.[Name]
  , t.[Notes]
  , 1 AS [DocumentCount] 
  , ISNULL(tr.[RatingTally],0) AS [RatingTally]
  , ISNULL(tr.[RatingCount],0) AS [RatingCount]
  , t.[Owner] 
  , t.[RecCreatedDt]
  , t.[RecCreatedUser]
  , t.[LastUpdatedDt]
  , t.[LastUpdatedUser]
 FROM [SQLRunbook].[tTopic] t
 LEFT JOIN (SELECT [TopicId]
             , SUM([RatingId]) AS [RatingTally]
             , COUNT([RatingId]) AS [RatingCount] 
            FROM [SQLRunbook].[tTopicRating]
            WHERE [RatingId] < 7 
            GROUP BY [TopicId]) tr
 ON t.[Id] = tr.[TopicId] 
 JOIN [SQLRunbook].[tTopicDocument] td
 ON t.[Id] = td.[TopicId] 
 WHERE td.[DocumentId] = @DocumentId
 ORDER BY [Name]; 

END;
GO

GRANT EXECUTE ON [SQLRunbook].[pTopicSelectByDocumentId] TO [SQLRunbookUserRole];

GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLRunbook'
           AND SPECIFIC_NAME = N'pTopicSelectByOriginalLogin' )
 DROP PROCEDURE [SQLRunbook].[pTopicSelectByOriginalLogin];

GO

/******************************************************************************
**	Auth: Bill Wunder
**	Date: April 1, 2007
**
**	Desc: Get all topics owned - not all touches - by this SQLClue Runbook User
**
*******************************************************************************
**	Change History
*******************************************************************************
**	Date		   Author		     Description of Change
**	          
*******************************************************************************/
CREATE PROCEDURE [SQLRunbook].[pTopicSelectByOriginalLogin] 
 ( @OriginalLogin [NVARCHAR](256) )
AS
BEGIN

 SELECT DISTINCT t.[Id] 
  , t.[Name]
  , t.[Notes]
  , ISNULL(td.[DocumentCount],0) AS [DocumentCount] 
  , ISNULL(tr.[RatingTally],0) AS [RatingTally]
  , ISNULL(tr.[RatingCount],0) AS [RatingCount]
  , t.[Owner] 
  , t.[RecCreatedDt]
  , t.[RecCreatedUser]
  , t.[LastUpdatedDt]
  , t.[LastUpdatedUser]
 FROM [SQLRunbook].[tTopic] t
 LEFT JOIN (SELECT [TopicId]
             , SUM([RatingId]) AS [RatingTally]
             , COUNT([RatingId]) AS [RatingCount] 
            FROM [SQLRunbook].[tTopicRating]
            WHERE [RatingId] < 7 
            GROUP BY [TopicId]) tr
 ON t.[Id] = tr.[TopicId] 
 LEFT JOIN (SELECT [TopicId]
             , COUNT(*) AS [DocumentCount] 
            FROM [SQLRunbook].[tTopicDocument]
            GROUP BY [TopicId]) td
 ON t.[Id] = td.[TopicId] 
 WHERE t.[RecCreatedUser] = @OriginalLogin
 ORDER BY [Name]; 

END;
GO

GRANT EXECUTE ON [SQLRunbook].[pTopicSelectByOriginalLogin] TO [SQLRunbookUserRole];

GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLRunbook'
           AND SPECIFIC_NAME = N'pTopicSelectByFreeText' )
 DROP PROCEDURE [SQLRunbook].[pTopicSelectByFreeText];

GO

/******************************************************************************
**	Auth: Bill Wunder
**	Date: April 1, 2007
**
**	Desc: Get all topics with notes or indexed documents containing the text   
**
*******************************************************************************
**	Change History
*******************************************************************************
**	Date		   Author		     Description of Change
**	          
*******************************************************************************/
CREATE PROCEDURE [SQLRunbook].[pTopicSelectByFreeText] 
 ( @SearchString [NVARCHAR] (4000) )
AS
BEGIN

 SELECT DISTINCT t.[Id] 
  , t.[Name]
  , t.[Notes]
  , ISNULL(td.[DocumentCount],0) AS [DocumentCount] 
  , ISNULL(tr.[RatingTally],0) AS [RatingTally]
  , ISNULL(tr.[RatingCount],0) AS [RatingCount]
  , t.[Owner] 
  , t.[RecCreatedDt]
  , t.[RecCreatedUser]
  , t.[LastUpdatedDt]
  , t.[LastUpdatedUser]
 FROM [SQLRunbook].[tTopic] t
 LEFT JOIN (SELECT [TopicId]
             , SUM([RatingId]) AS [RatingTally]
             , COUNT([RatingId]) AS [RatingCount] 
            FROM [SQLRunbook].[tTopicRating]
            WHERE [RatingId] < 7 
            GROUP BY [TopicId]) tr
 ON t.[Id] = tr.[TopicId] 
 LEFT JOIN (SELECT [TopicId]
             , COUNT(*) AS [DocumentCount] 
            FROM [SQLRunbook].[tTopicDocument]
            GROUP BY [TopicId]) td
 ON t.[Id] = td.[TopicId] 
 WHERE t.[Id] IN ( SELECT t1.[Id] 
                 FROM [SQLRunbook].[tTopic] t1
                 JOIN [SQLRunbook].[tTopicDocument] td
                 ON t1.[Id] = td.[TopicId] 
                 WHERE FREETEXT((t1.[Name], t1.[Notes]), @SearchString)

                 UNION

                 SELECT t2.[Id] 
                 FROM [SQLRunbook].[tTopic] t2
                 JOIN [SQLRunbook].[tTopicDocument] td
                 ON t2.[Id] = td.[TopicId] 
                 LEFT JOIN [SQLRunbook].[tDocument] d
                 ON td.[DocumentId] = d.[Id]
                 WHERE FREETEXT((d.[File], d.[Document]), @SearchString)

                 UNION

                 SELECT t3.[Id] 
                 FROM [SQLRunbook].[tTopic] t3
                 JOIN [SQLRunbook].[tTopicDocument] td
                 ON t3.[Id] = td.[TopicId] 
                 LEFT JOIN [SQLRunbook].[tDocumentRating] dr
                 ON td.[DocumentId] = dr.[DocumentId]
                 WHERE FREETEXT((dr.[Notes]), @SearchString) )
   ORDER BY [Name];

END;
GO

GRANT EXECUTE ON [SQLRunbook].[pTopicSelectByFreeText] TO [SQLRunbookUserRole]

GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLRunbook'
           AND SPECIFIC_NAME = N'pTopicSelectByContains' )
 DROP PROCEDURE [SQLRunbook].[pTopicSelectByContains];

GO

/******************************************************************************
**	Auth: Bill Wunder
**	Date: April 1, 2007
**
**	Desc: Get all topics with notes or indexed documents containing the literal   
**
*******************************************************************************
**	Change History
*******************************************************************************
**	Date		   Author		     Description of Change
**	          
*******************************************************************************/
CREATE PROCEDURE [SQLRunbook].[pTopicSelectByContains] 
 ( @SearchString [NVARCHAR] (128) )
AS
BEGIN

 SELECT DISTINCT t.[Id] 
  , t.[Name]
  , t.[Notes]
  , ISNULL(td.[DocumentCount],0) AS [DocumentCount] 
  , ISNULL(tr.[RatingTally],0) AS [RatingTally]
  , ISNULL(tr.[RatingCount],0) AS [RatingCount]
  , t.[Owner] 
  , t.[RecCreatedDt]
  , t.[RecCreatedUser]
  , t.[LastUpdatedDt]
  , t.[LastUpdatedUser]
 FROM [SQLRunbook].[tTopic] t
 LEFT JOIN (SELECT [TopicId]
             , SUM([RatingId]) AS [RatingTally]
             , COUNT([RatingId]) AS [RatingCount] 
            FROM [SQLRunbook].[tTopicRating]
            WHERE [RatingId] < 7 
            GROUP BY [TopicId]) tr
 ON t.[Id] = tr.[TopicId] 
 LEFT JOIN (SELECT [TopicId]
             , COUNT(*) AS [DocumentCount] 
            FROM [SQLRunbook].[tTopicDocument]
            GROUP BY [TopicId]) td
 ON t.[Id] = td.[TopicId] 
 WHERE t.[Id] IN ( SELECT t1.[Id] 
                 FROM [SQLRunbook].[tTopic] t1
                 JOIN [SQLRunbook].[tTopicDocument] td
                 ON t1.[Id] = td.[TopicId] 
                 WHERE CONTAINS((t1.[Name], t1.[Notes]), @SearchString)

                 UNION

                 SELECT t2.[Id] 
                 FROM [SQLRunbook].[tTopic] t2
                 JOIN [SQLRunbook].[tTopicDocument] td
                 ON t2.[Id] = td.[TopicId] 
                 LEFT JOIN [SQLRunbook].[tDocument] d
                 ON td.[DocumentId] = d.[Id]
                 WHERE CONTAINS((d.[File], d.[Document]), @SearchString)

                 UNION

                 SELECT t3.[Id] 
                 FROM [SQLRunbook].[tTopic] t3
                 JOIN [SQLRunbook].[tTopicDocument] td
                 ON t3.[Id] = td.[TopicId] 
                 LEFT JOIN [SQLRunbook].[tDocumentRating] dr
                 ON td.[DocumentId] = dr.[DocumentId]
                 WHERE CONTAINS((dr.[Notes]), @SearchString) )
   ORDER BY [Name];

END;
GO

GRANT EXECUTE ON [SQLRunbook].[pTopicSelectByContains] TO [SQLRunbookUserRole]

GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLRunbook'
           AND SPECIFIC_NAME = N'pTopicSelectByCriteria' )
 DROP PROCEDURE [SQLRunbook].[pTopicSelectByCriteria];

GO

/******************************************************************************
**	Auth: Bill Wunder
**	Date: April 1, 2007
**
**	Desc: Get topics matching ALL of date, date range, category, user,
**        a CONTAINS search word or a FREETEXT search string   
**
*******************************************************************************
**	Change History
*******************************************************************************
**	Date		   Author		     Description of Change
**	          
*******************************************************************************/
CREATE PROCEDURE [SQLRunbook].[pTopicSelectByCriteria] 
 ( @BeginDate [DATETIME]
 , @EndDate [DATETIME]
 , @TopicId [INT]
 , @DocumentId [INT]
 , @CategoryId [INT]
 , @OriginalLogin [NVARCHAR] (256) 
 , @SearchCategories [BIT]
 , @SearchTopics [BIT]
 , @SearchDocuments [BIT]
 , @SearchRatings [BIT]
 , @RatingOperator [NCHAR](1)
 , @RatingValue [INT]
 , @Contains [NVARCHAR] (4000))
AS
BEGIN

 -- need a non-blank, non-noiseword to avoid error messages 
 -- when full text predicates are not involved in request
 If NULLIF(@Contains,'') IS NULL 
  SET @Contains = 'ZXYABCUUUUQ' 

 SELECT DISTINCT t.[Id] 
  , t.[Name]
  , t.[Notes]
  , ISNULL(td.[DocumentCount],0) AS [DocumentCount] 
  , ISNULL(tr.[RatingTally],0) AS [RatingTally]
  , ISNULL(tr.[RatingCount],0) AS [RatingCount]
  , t.[Owner] 
  , t.[RecCreatedDt]
  , t.[RecCreatedUser]
  , t.[LastUpdatedDt]
  , t.[LastUpdatedUser]
 FROM [SQLRunbook].[tTopic] t
 LEFT JOIN (SELECT [TopicId]
             , SUM([RatingId]) AS [RatingTally]
             , COUNT([RatingId]) AS [RatingCount] 
            FROM [SQLRunbook].[tTopicRating]
            WHERE [RatingId] < 7 
            GROUP BY [TopicId]) tr
 ON t.[Id] = tr.[TopicId] 
 LEFT JOIN (SELECT [TopicId]
             , COUNT(*) AS [DocumentCount] 
            FROM [SQLRunbook].[tTopicDocument]
            WHERE @DocumentId = 0 
            OR [DocumentId] = @DocumentId        
            GROUP BY [TopicId]) td
 ON t.[Id] = td.[TopicId] 
 LEFT JOIN [SQLRunbook].[tCategoryTopic] ct
 ON t.[Id] = ct.[TopicId]
 LEFT JOIN ( SELECT ct1.[TopicId] AS [TopicId]
				, crank1.[Rank] AS [CatRank]
				, 0 AS [CatRateRank]
				, 0 AS [TopRank]
                , 0 AS [TopRateRank]
				, 0 AS [DocRank]
				, 0 AS [DocRateRank]
             FROM [SQLRunbook].[tCategoryTopic] ct1
             JOIN [SQLRunbook].[tCategory] c1
             ON ct1.[CategoryId] = c1.[Id]
             JOIN CONTAINSTABLE([SQLRunbook].[tCategory],(Name, Notes), @Contains) crank1
             ON c1.[Id] = crank1.[Key]
             WHERE @SearchCategories = 1

             UNION ALL
             
             SELECT ct2.[TopicId] AS [TopicId] 
				, 0 AS [CatRank]
				, crrank2.[Rank] AS [CatRateRank]
				, 0 AS [TopRank]
                , 0 AS [TopRateRank]
				, 0 AS [DocRank]
				, 0 AS [DocRateRank]
             FROM [SQLRunbook].[tCategoryTopic] ct2
             JOIN [SQLRunbook].[tCategoryRating] cr2
             ON ct2.[CategoryId] = cr2.[Id]
             JOIN CONTAINSTABLE([SQLRunbook].[tCategoryRating], Notes, @Contains) crrank2
             ON cr2.[Id] = crrank2.[Key]
             WHERE @SearchCategories = 1 
             AND @SearchRatings = 1 
           
             UNION ALL
           
             SELECT t3.[Id] AS [TopicId]
				, 0 AS [CatRank]
				, 0 AS [CatRateRank]
				, trank3.[Rank] AS [TopRank]
                , 0 AS [TopRateRank]
 				, 0 AS [DocRank]
				, 0 AS [DocRateRank]
             FROM [SQLRunbook].[tTopic] t3
             JOIN CONTAINSTABLE([SQLRunbook].[tTopic], (Name, Notes), @Contains) trank3
             ON t3.[Id] = trank3.[Key]
             WHERE @SearchTopics = 1

             UNION ALL

             SELECT t4.[Id] AS [TopicId] 
				, 0 AS [CatRank]
				, 0 AS [CatRateRank]
				, 0 AS [TopRank]
				, trrank4.[Rank] AS [TopRateRank]
				, 0 AS [DocRank]
				, 0 AS [DocRateRank]
             FROM [SQLRunbook].[tTopic] t4
             LEFT JOIN [SQLRunbook].[tTopicRating] tr4
             ON t4.[Id] = tr4.[TopicId]
             JOIN CONTAINSTABLE([SQLRunbook].[tTopicRating], Notes, @Contains) trrank4
             ON t4.[Id] = trrank4.[Key]
             WHERE @SearchTopics = 1
             AND @SearchRatings = 1

             UNION ALL

             SELECT t5.[Id] AS [TopicId] 
				, 0 AS [CatRank]
				, 0 AS [CatRateRank]
				, 0 AS [TopRank]
                , 0 AS [TopRateRank]
				, drank5.[Rank] AS [DocRank]
				, 0 AS [DocRateRank]
             FROM [SQLRunbook].[tTopic] t5
             JOIN [SQLRunbook].[tTopicDocument] td5
             ON t5.[Id] = td5.[TopicId] 
             LEFT JOIN [SQLRunbook].[tDocument] d5
             ON td5.[DocumentId] = d5.[Id]
             JOIN CONTAINSTABLE([SQLRunbook].[tDocument], ([File], Document), @Contains) drank5
             ON d5.[Id] = drank5.[Key]
             WHERE @SearchDocuments = 1

             UNION ALL

             SELECT t6.[Id] AS [TopicId] 
				, 0 AS [CatRank]
				, 0 AS [CatRateRank]
				, 0 AS [TopRank]
                , 0 AS [TopRateRank]
				, 0 AS [DocRank]
				, drrank6.[Rank] AS [DocRateRank]
             FROM [SQLRunbook].[tTopic] t6
             JOIN [SQLRunbook].[tTopicDocument] td6
             ON t6.[Id] = td6.[TopicId] 
             LEFT JOIN [SQLRunbook].[tDocumentRating] dr6
             ON td6.[DocumentId] = dr6.[DocumentId]
             JOIN CONTAINSTABLE([SQLRunbook].[tDocumentRating], Notes, @Contains) drrank6
             ON dr6.[DocumentId] = drrank6.[KEY]
             WHERE @SearchRatings = 1) tt
 ON t.[Id] = tt.[TopicId]            
 WHERE ((@BeginDate < '1970-04-01'
         OR (@BeginDate = @EndDate 
             AND @BeginDate = CONVERT([NVARCHAR](10),t.[RecCreatedDt], 101) ))  
         OR (@BeginDate = CONVERT([NVARCHAR](10),t.[RecCreatedDt], 101) 
             AND @EndDate < '1970-04-01')
         OR(CONVERT([NVARCHAR](10),t.[RecCreatedDt], 101) 
			BETWEEN CONVERT([NVARCHAR](10), @BeginDate, 101) 
			AND CONVERT([NVARCHAR](10), @EndDate, 101)))  
 OR  (@CategoryId = 0    
      OR ct.[CategoryId] = @CategoryId)
 OR  (@TopicId = 0 
      OR t.[Id] = @TopicId)        
 OR  (@OriginalLogin = '' 
      OR t.[RecCreatedUser] = @OriginalLogin 
      OR t.[Owner] = @OriginalLogin )
END;
GO

GRANT EXECUTE ON [SQLRunbook].[pTopicSelectByCriteria] TO [SQLRunbookUserRole]

GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLRunbook'
           AND SPECIFIC_NAME = N'pTopicSelectByAllCriteria' )
 DROP PROCEDURE [SQLRunbook].[pTopicSelectByAllCriteria];

GO

/******************************************************************************
**	Auth: Bill Wunder
**	Date: April 1, 2007
**
**	Desc: Get topics matching ALL specified criteria   
**
*******************************************************************************
**	Change History
*******************************************************************************
**	Date		   Author		     Description of Change
**	          
*******************************************************************************/
CREATE PROCEDURE [SQLRunbook].[pTopicSelectByAllCriteria] 
 ( @BeginDate [DATETIME]
 , @EndDate [DATETIME]
 , @TopicId [INT]
 , @DocumentId [INT]
 , @CategoryId [INT]
 , @OriginalLogin [NVARCHAR] (256) 
 , @SearchCategories [BIT]
 , @SearchTopics [BIT]
 , @SearchDocuments [BIT]
 , @SearchRatings [BIT]
 , @RatingOperator [NCHAR](1)
 , @RatingValue [INT]
 , @Contains [NVARCHAR] (4000))
AS
BEGIN

 -- need a non-blank, non-noiseword to avoid error messages 
 -- when full text predicates are not involved in request
 If NULLIF(@Contains,'') IS NULL 
  SET @Contains = 'ZXYABCUUUUQ' 

 SELECT DISTINCT t.[Id] 
  , t.[Name]
  , t.[Notes]
  , ISNULL(td.[DocumentCount],0) AS [DocumentCount] 
  , ISNULL(tr.[RatingTally],0) AS [RatingTally]
  , ISNULL(tr.[RatingCount],0) AS [RatingCount]
  , t.[Owner] 
  , t.[RecCreatedDt]
  , t.[RecCreatedUser]
  , t.[LastUpdatedDt]
  , t.[LastUpdatedUser]
 FROM [SQLRunbook].[tTopic] t
 LEFT JOIN (SELECT [TopicId]
             , SUM([RatingId]) AS [RatingTally]
             , COUNT([RatingId]) AS [RatingCount] 
            FROM [SQLRunbook].[tTopicRating]
            WHERE [RatingId] < 7 
            GROUP BY [TopicId]) tr
 ON t.[Id] = tr.[TopicId] 
 LEFT JOIN (SELECT [TopicId]
             , COUNT(*) AS [DocumentCount] 
            FROM [SQLRunbook].[tTopicDocument]
            WHERE [DocumentId] = @DocumentId        
            GROUP BY [TopicId]) td
 ON t.[Id] = td.[TopicId] 
 LEFT JOIN [SQLRunbook].[tCategoryTopic] ct
 ON t.[Id] = ct.[TopicId]
 LEFT JOIN ( SELECT ct1.[TopicId] AS [TopicId]
				, crank1.[Rank] AS [CatRank]
				, 0 AS [CatTopRank]
				, 0 AS [TopRank]
                , 0 AS [TopRatRank]
				, 0 AS [TopDocRank]
				, 0 AS [DocRank]
				, 0 AS [DocRatRank]
             FROM [SQLRunbook].[tCategoryTopic] ct1
             JOIN [SQLRunbook].[tCategory] c1
             ON ct1.[CategoryId] = c1.[Id]
             JOIN CONTAINSTABLE([SQLRunbook].[tCategory],(Name, Notes), @Contains) crank1
             ON c1.[Id] = crank1.[Key]
             WHERE @SearchCategories = 1

             UNION ALL
             
             SELECT ct2.[TopicId] AS [TopicId] 
				, 0 AS [CatRank]
				, ctrank2.[Rank] AS [CatTopRank]
				, 0 AS [TopRank]
                , 0 AS [TopRatRank]
				, 0 AS [TopDocRank]
				, 0 AS [DocRank]
				, 0 AS [DocRatRank]
             FROM [SQLRunbook].[tCategoryTopic] ct2
             JOIN [SQLRunbook].[tCategoryRating] cr2
             ON ct2.[CategoryId] = cr2.[Id]
             JOIN CONTAINSTABLE([SQLRunbook].[tCategoryRating], Notes, @Contains) ctrank2
             ON cr2.[Id] = ctrank2.[Key]
             WHERE @SearchCategories = 1 
             AND @SearchRatings = 1             
           
             UNION ALL
           
             SELECT t3.[Id] AS [TopicId]
				, 0 AS [CatRank]
				, 0 AS [CatTopRank]
				, trank3.[Rank] AS [TopRank]
                , 0 AS [TopRatRank]
 				, 0 AS [TopDocRank]
				, 0 AS [DocRank]
				, 0 AS [DocRatRank]
             FROM [SQLRunbook].[tTopic] t3
             JOIN CONTAINSTABLE([SQLRunbook].[tTopic], (Name, Notes), @Contains) trank3
             ON t3.[Id] = trank3.[Key]
             WHERE @SearchTopics = 1

             UNION ALL

             SELECT t4.[Id] AS [TopicId] 
				, 0 AS [CatRank]
				, 0 AS [CatTopRank]
				, 0 AS [TopRank]
				, trrank4.[Rank] AS [TopRatRank]
				, 0 AS [TopDocRank]
				, 0 AS [DocRank]
				, 0 AS [DocRatRank]
             FROM [SQLRunbook].[tTopic] t4
             LEFT JOIN [SQLRunbook].[tTopicRating] tr4
             ON t4.[Id] = tr4.[TopicId]
             JOIN CONTAINSTABLE([SQLRunbook].[tTopicRating], Notes, @Contains) trrank4
             ON t4.[Id] = trrank4.[Key]
             AND @SearchTopics = 1
             AND @SearchRatings = 1

             UNION ALL

             SELECT t5.[Id] AS [TopicId] 
				, 0 AS [CatRank]
				, 0 AS [CatTopRank]
				, 0 AS [TopRank]
                , 0 AS [TopRatRank]
				, 0 AS [TopDocRank]
				, drank5.[Rank] AS [DocRank]
				, 0 AS [DocRatRank]
             FROM [SQLRunbook].[tTopic] t5
             JOIN [SQLRunbook].[tTopicDocument] td5
             ON t5.[Id] = td5.[TopicId] 
             LEFT JOIN [SQLRunbook].[tDocument] d5
             ON td5.[DocumentId] = d5.[Id]
             JOIN CONTAINSTABLE([SQLRunbook].[tDocument], ([File], Document), @Contains) drank5
             ON d5.[Id] = drank5.[Key]
             AND @SearchDocuments = 1

             UNION ALL

             SELECT t6.[Id] AS [TopicId] 
				, 0 AS [CatRank]
				, 0 AS [CatTopRank]
				, 0 AS [TopRank]
                , 0 AS [TopRatRank]
				, 0 AS [TopDocRank]
				, 0 AS [DocRank]
				, drrank6.[Rank] AS [DocRatRank]
             FROM [SQLRunbook].[tTopic] t6
             JOIN [SQLRunbook].[tTopicDocument] td6
             ON t6.[Id] = td6.[TopicId] 
             LEFT JOIN [SQLRunbook].[tDocumentRating] dr6
             ON td6.[DocumentId] = dr6.[DocumentId]
             JOIN CONTAINSTABLE([SQLRunbook].[tDocumentRating], Notes, @Contains) drrank6
             ON dr6.[DocumentId] = drrank6.[KEY]
             And @SearchRatings = 1) tt
 ON t.[Id] = tt.[TopicId]            
 WHERE ((@BeginDate < '1970-04-01'
         OR (@BeginDate = @EndDate 
             AND @BeginDate = CONVERT([NVARCHAR](10),t.[RecCreatedDt], 101) ))  
         OR (@BeginDate = CONVERT([NVARCHAR](10),t.[RecCreatedDt], 101) 
             AND @EndDate < '1970-04-01')
         OR(CONVERT([NVARCHAR](10),t.[RecCreatedDt], 101) 
			BETWEEN CONVERT([NVARCHAR](10), @BeginDate, 101) 
			AND CONVERT([NVARCHAR](10), @EndDate, 101)))  
 AND  (@CategoryId = 0    
       OR ct.[CategoryId] = @CategoryId)
 AND  (@TopicId = 0 
       OR t.[Id] = @TopicId)        
 AND  (@OriginalLogin = '' 
       OR t.[RecCreatedUser] = @OriginalLogin 
       OR t.[Owner] = @OriginalLogin )
END;
GO

GRANT EXECUTE ON [SQLRunbook].[pTopicSelectByAllCriteria] TO [SQLRunbookUserRole]

GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLRunbook'
           AND SPECIFIC_NAME = N'pTopicSelectAllNames' )
 DROP PROCEDURE [SQLRunbook].[pTopicSelectAllNames];

GO
/******************************************************************************
**	Auth: Bill Wunder
**	Date: April 1, 2007
**
**	Desc: unique list of topic names and Ids for all Runbook entries 
**
*******************************************************************************
**	Change History
*******************************************************************************
**	Date		   Author		     Description of Change
**	          
*******************************************************************************/
CREATE PROCEDURE [SQLRunbook].[pTopicSelectAllNames]
AS
BEGIN

 SELECT [Id], [Name]
 FROM [SQLRunbook].[tTopic]
 ORDER BY [Name];

END;

GO

GRANT EXECUTE ON [SQLRunbook].[pTopicSelectAllNames] TO [SQLRunbookUserRole];

GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLRunbook'
           AND SPECIFIC_NAME = N'pTopicInsert' )
 DROP PROCEDURE [SQLRunbook].[pTopicInsert];

GO
/******************************************************************************
**	Auth: Bill Wunder
**	Date: April 1, 2007
**
**	Desc: Add a Runbook Topic   
**
*******************************************************************************
**	Change History
*******************************************************************************
**	Date		   Author		     Description of Change
**	          
*******************************************************************************/
CREATE PROCEDURE [SQLRunbook].[pTopicInsert] 
 ( @Name [NVARCHAR] (128) 
 , @Notes [NVARCHAR] (MAX) 
 , @Owner [NVARCHAR] (128) ) 
AS
BEGIN
DECLARE @TextData [NVARCHAR] (MAX);

 SET XACT_ABORT ON;

 BEGIN TRY

  INSERT [SQLRunbook].[tTopic] ([Name], [Notes], [Owner])
  VALUES (@Name, @Notes, @Owner);

 END TRY

 BEGIN CATCH

  SET @TextData = '   @Name = ' + ISNULL(CHAR(39) + @Name + CHAR(39), 'NULL') + CHAR(13) + CHAR(10)
                + ' , @Notes = ' + ISNULL(CHAR(39) + @Notes + CHAR(39), 'NULL') + CHAR(13) + CHAR(10)
			    + ' , @Owner = ' + ISNULL(CHAR(39) + @Owner + CHAR(39), 'NULL');  
  EXEC [SQLRunbook].[pLogSQLError] @TextData, @@PROCID;

 END CATCH

END;

GO

GRANT EXECUTE ON [SQLRunbook].[pTopicInsert] TO [SQLRunbookContributorRole];

GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLRunbook'
           AND SPECIFIC_NAME = N'pTopicUpdate' )
 DROP PROCEDURE [SQLRunbook].[pTopicUpdate];

GO
/******************************************************************************
**	Auth: Bill Wunder
**	Date: April 1, 2007
**
**	Desc: Modify a topic   
**
*******************************************************************************
**	Change History
*******************************************************************************
**	Date		   Author		     Description of Change
**	          
*******************************************************************************/
CREATE PROCEDURE [SQLRunbook].[pTopicUpdate] 
 ( @Id [INT]
 , @Name [NVARCHAR] (128) 
 , @Notes [NVARCHAR] (MAX) 
 , @Owner [NVARCHAR] (128) ) 
AS
BEGIN
DECLARE @TextData [NVARCHAR] (MAX);

 SET XACT_ABORT ON;

 BEGIN TRY

  -- if this is a name change, add a note too
  UPDATE [SQLRunbook].[tTopic] 
  SET [Name] = @Name
    , [Notes] = @Notes 
    , [Owner] = ISNULL(NULLIF(@OWNER,''), ORIGINAL_LOGIN())
  WHERE [Id] = @Id;

 END TRY

 BEGIN CATCH

  SET @TextData = '   @Id = ' + ISNULL(CONVERT([NVARCHAR] (10), @Id),'NULL') + CHAR(13) + CHAR(10)
                + ' , @Name = ' + ISNULL(CHAR(39) + @Name + CHAR(39), 'NULL') + CHAR(13) + CHAR(10)
                + ' , @Notes = ' + ISNULL(CHAR(39) + @Notes + CHAR(39), 'NULL')+ CHAR(13) + CHAR(10)
                + ' , @Owner = ' + ISNULL(CHAR(39) + @Name + CHAR(39), 'NULL');  
  EXEC [SQLRunbook].[pLogSQLError] @TextData, @@PROCID;

 END CATCH

END;

GO

GRANT EXECUTE ON [SQLRunbook].[pTopicUpdate] TO [SQLRunbookContributorRole];

GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLRunbook'
           AND SPECIFIC_NAME = N'pTopicDelete' )
 DROP PROCEDURE [SQLRunbook].[pTopicDelete];

GO
/******************************************************************************
**	Auth: Bill Wunder
**	Date: April 1, 2007
**
**	Desc: delete a runbook topic   
**
*******************************************************************************
**	Change History
*******************************************************************************
**	Date		   Author		     Description of Change
**	          
*******************************************************************************/
CREATE PROCEDURE [SQLRunbook].[pTopicDelete] 
 ( @Id [Int]) 
AS
BEGIN
 DECLARE @docs TABLE(DocumentId Int PRIMARY KEY);
 DECLARE @rc [INT]
  , @TextData [NVARCHAR] (MAX);

   SET XACT_ABORT ON;

   BEGIN TRY

   -- txn must always end in this proc to avoid the non-fatal 3903 warning 
    BEGIN TRANSACTION
    
    -- xref tables gotta go
    DELETE [SQLRunbook].[tTopicDocument]      
	WHERE [TopicId] = @Id;

    DELETE [SQLRunbook].[tCategoryTopic]      
	WHERE [TopicId] = @Id;

    -- check each doc for other topics, if not then remove docs when removing topic
    INSERT @docs ([DocumentId])
    SELECT d.[Id]
    FROM [SQLRunbook].[tDocument] d
    JOIN [SQLRunbook].[tTopicDocument] td
    ON d.[Id]  = td.[DocumentId]
    WHERE td.[TopicId] = @Id
    AND NOT EXISTS (SELECT * 
                    FROM [SQLRunbook].[tTopicDocument]
                    WHERE [TopicId] <> td.[TopicId]
                    AND [DocumentId] = td.[DocumentId]); 

    IF @@ROWCOUNT > 0
     
     BEGIN  
      -- order to avoiding fkey violations    
      DELETE [SQLRunbook].[tDocument]      
      WHERE [ID] IN (SELECT [DocumentId] FROM @docs);
     	 
     END

	DELETE [SQLRunbook].[tTopic]
	WHERE [Id] = @Id;
  
   COMMIT TRANSACTION

  END TRY

  BEGIN CATCH 
  
   -- txn must always end in this proc to avoid the non-fatal 3903 warning 
   -- the error handler tries to end any open txn befor logging
   ROLLBACK TRANSACTION

   SET @TextData = '   @Id = ' + ISNULL(CONVERT([NVARCHAR] (10), @Id),'NULL');
   EXEC [SQLRunbook].[pLogSQLError] @TextData, @@PROCID;

  END CATCH

END;

GO

GRANT EXECUTE ON [SQLRunbook].[pTopicDelete] TO [SQLRunbookContributorRole];

GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLRunbook'
           AND SPECIFIC_NAME = N'pTopicGetNotes' )
 DROP PROCEDURE [SQLRunbook].[pTopicGetNotes];

GO
/******************************************************************************
**	Auth: Bill Wunder
**	Date: April 1, 2007
**
**	Desc: get the notes for a Runbook Topic   
**
*******************************************************************************
**	Change History
*******************************************************************************
**	Date		   Author		     Description of Change
**	          
*******************************************************************************/
CREATE PROCEDURE [SQLRunbook].[pTopicGetNotes]
 ( @Id [INT]
 , @Notes [NVARCHAR](MAX) OUTPUT ) 
AS
BEGIN

 SELECT @Notes = [Notes]
 FROM [SQLRunbook].[tTopic]
 WHERE [Id] = @Id;

END;

GO

GRANT EXECUTE ON [SQLRunbook].[pTopicGetNotes] TO [SQLRunbookUserRole];

GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLRunbook'
           AND SPECIFIC_NAME = N'pTopicDocumentSelectAll' )
 DROP PROCEDURE [SQLRunbook].[pTopicDocumentSelectAll];

GO
/******************************************************************************
**	Auth: Bill Wunder
**	Date: April 1, 2007
**
**	Desc: get all topic/document xref rows 
**
*******************************************************************************
**	Change History
*******************************************************************************
**	Date		   Author		     Description of Change
**	          
*******************************************************************************/
CREATE PROCEDURE [SQLRunbook].[pTopicDocumentSelectAll] 
AS
BEGIN

 SELECT [TopicId]
  , [DocumentId]
 FROM [SQLRunbook].[tTopicDocument];

END;

GO

--GRANT EXECUTE ON [SQLRunbook].[pTopicDocumentSelectAll] TO [SQLClueAdmin];

GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLRunbook'
           AND SPECIFIC_NAME = N'pTopicDocumentSelectByTopicId' )
 DROP PROCEDURE [SQLRunbook].[pTopicDocumentSelectByTopicId];

GO
/******************************************************************************
**	Auth: Bill Wunder
**	Date: April 1, 2007
**
**	Desc: get all topic/document xref rows for a topic   
**
*******************************************************************************
**	Change History
*******************************************************************************
**	Date		   Author		     Description of Change
**	          
*******************************************************************************/
CREATE PROCEDURE [SQLRunbook].[pTopicDocumentSelectByTopicId] 
 ( @TopicId [INT] )
AS
BEGIN

 SELECT [TopicId]
  , [DocumentId]
 FROM [SQLRunbook].[tTopicDocument]
 WHERE [TopicId] = @TopicId;
  
END;

GO

GRANT EXECUTE ON [SQLRunbook].[pTopicDocumentSelectByTopicId] TO [SQLRunbookUserRole];

GO

GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLRunbook'
           AND SPECIFIC_NAME = N'pTopicDocumentSelectByDocumentId' )
 DROP PROCEDURE [SQLRunbook].[pTopicDocumentSelectByDocumentId];

GO
/******************************************************************************
**	Auth: Bill Wunder
**	Date: April 1, 2007
**
**	Desc: get all topic/document xref rows for a document
**
*******************************************************************************
**	Change History
*******************************************************************************
**	Date		   Author		     Description of Change
**	          
*******************************************************************************/
CREATE PROCEDURE [SQLRunbook].[pTopicDocumentSelectByDocumentId] 
 ( @DocumentId [INT] )
AS
BEGIN

 SELECT [TopicId]
  , [DocumentId]
 FROM [SQLRunbook].[tTopicDocument]
 WHERE [DocumentId] = @DocumentId;

END;

GO

GRANT EXECUTE ON [SQLRunbook].[pTopicDocumentSelectByDocumentId] TO [SQLRunbookUserRole];

GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLRunbook'
           AND SPECIFIC_NAME = N'pTopicDocumentSelectByDateRange' )
 DROP PROCEDURE [SQLRunbook].[pTopicDocumentSelectByDateRange];

GO
/******************************************************************************
**	Auth: Bill Wunder
**	Date: April 1, 2007
**
**	Desc: get all topic/document xref rows for a date range with names
**
*******************************************************************************
**	Change History
*******************************************************************************
**	Date		   Author		     Description of Change
**	          
*******************************************************************************/
CREATE PROCEDURE [SQLRunbook].[pTopicDocumentSelectByDateRange] 
 ( @BeginDt [DATETIME]
 , @EndDt [DATETIME] )
AS
BEGIN

 SELECT td.[TopicId]
  , td.[DocumentId]
  , t.[Name] AS [TopicName]
  , d.[File]
  , td.[RecCreatedDt]
  , td.[RecCreatedUser]
 FROM [SQLRunbook].[tTopicDocument] td
 JOIN [SQLRunbook].[tTopic] t
 ON td.[TopicId] = t.[Id]
 JOIN [SQLRunbook].[tDocument] d
 ON td.[DocumentId] = d.[Id]
 WHERE td.[RecCreatedDt] BETWEEN @BeginDt AND @EndDt
 ORDER BY [RecCreatedDt]
  , [TopicName]
  , [File];

END;

GO

GRANT EXECUTE ON [SQLRunbook].[pTopicDocumentSelectByDateRange] TO [SQLRunbookUserRole];

GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLRunbook'
           AND SPECIFIC_NAME = N'pTopicDocumentInsert' )
 DROP PROCEDURE [SQLRunbook].[pTopicDocumentInsert];

GO
/******************************************************************************
**	Auth: Bill Wunder
**	Date: April 1, 2007
**
**	Desc: add a topic/document xref row   
**
*******************************************************************************
**	Change History
*******************************************************************************
**	Date		   Author		     Description of Change
**	          
*******************************************************************************/
CREATE PROCEDURE [SQLRunbook].[pTopicDocumentInsert] 
 ( @TopicId [INT]
 , @DocumentId [Int] ) 
AS
BEGIN
DECLARE @TextData [NVARCHAR] (MAX);

 SET XACT_ABORT ON;

 BEGIN TRY

  IF NOT EXISTS (SELECT * FROM [SQLRunbook].[tTopicDocument]
                 WHERE [TopicId] = @TopicId AND [DocumentId] = @DocumentId) 
   INSERT [SQLRunbook].[tTopicDocument] ([TopicId], [DocumentId])
   VALUES (@TopicId, @DocumentId); 

 END TRY

 BEGIN CATCH

  SET @TextData = '   @TopicId = ' + ISNULL(CONVERT([NVARCHAR] (10), @TopicId),'NULL') + CHAR(13) + CHAR(10)
                + ' , @DocumentId = ' + ISNULL(CONVERT([NVARCHAR] (10),@DocumentId),'NULL');
  EXEC [SQLRunbook].[pLogSQLError] @TextData, @@PROCID;

 END CATCH

END;

GO

GRANT EXECUTE ON [SQLRunbook].[pTopicDocumentInsert] TO [SQLRunbookContributorRole];

GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLRunbook'
           AND SPECIFIC_NAME = N'pTopicDocumentDelete' )
 DROP PROCEDURE [SQLRunbook].[pTopicDocumentDelete];

GO
/******************************************************************************
**	Auth: Bill Wunder
**	Date: April 1, 2007
**
**	Desc: delete a runbook topic document xref row  
**
*******************************************************************************
**	Change History
*******************************************************************************
**	Date		   Author		     Description of Change
**	          
*******************************************************************************/
CREATE PROCEDURE [SQLRunbook].[pTopicDocumentDelete] 
 ( @TopicId [INT]
 , @DocumentId [Int] ) 
AS
BEGIN
DECLARE @TextData [NVARCHAR] (MAX);

 SET XACT_ABORT ON;

 BEGIN TRY

  DELETE [SQLRunbook].[tTopicDocument]
  WHERE [TopicId] = @TopicId
  AND [DocumentID] = @DocumentId;

 END TRY

 BEGIN CATCH

  SET @TextData = '   @TopicId = ' + ISNULL(CONVERT([NVARCHAR] (10), @TopicId),'NULL') + CHAR(13) + CHAR(10)
                + ' , @DocumentId = ' +  ISNULL(CONVERT([NVARCHAR] (10),@DocumentId),'NULL');
  EXEC [SQLRunbook].[pLogSQLError] @TextData, @@PROCID;

 END CATCH;

END;

GO

GRANT EXECUTE ON [SQLRunbook].[pTopicDocumentDelete] TO [SQLRunbookContributorRole];

GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLRunbook'
           AND SPECIFIC_NAME = N'pDocumentRatingUpsert' )
 DROP PROCEDURE [SQLRunbook].[pDocumentRatingUpsert];

GO
/******************************************************************************
**	Auth: Bill Wunder
**	Date: April 1, 2007
**
**	Desc: Rate a Runbook Document   
**
*******************************************************************************
**	Change History
*******************************************************************************
**	Date		   Author		     Description of Change
**	          
*******************************************************************************/
CREATE PROCEDURE [SQLRunbook].[pDocumentRatingUpsert] 
 ( @Id [INT]
 , @RatingId [INT]  
 , @Notes [NVARCHAR] (MAX)) 
AS
BEGIN
DECLARE @TextData [NVARCHAR] (MAX);

 SET XACT_ABORT ON;

 BEGIN TRY

  UPDATE [SQLRunbook].[tDocumentRating] 
  SET [RatingId] = @RatingId
    , [Notes] = @Notes
  WHERE [DocumentId] = @Id
  AND [RecCreatedUser] = ORIGINAL_LOGIN()

  IF @@ROWCOUNT = 0
   INSERT [SQLRunbook].[tDocumentRating] ([DocumentId], [RatingId], [Notes])
   VALUES (@Id, @RatingId, @Notes);

 END TRY

 BEGIN CATCH

  SET @TextData = '   @Id = ' + CONVERT([NVARCHAR](10), @Id) + CHAR(13) + CHAR(10)
                + '   @RatingId = ' + CONVERT([NVARCHAR](10), @RatingId) + CHAR(13) + CHAR(10)
                + ' , @Notes = ' + ISNULL(CHAR(39) + @Notes + CHAR(39), 'NULL');  
  EXEC [SQLRunbook].[pLogSQLError] @TextData, @@PROCID;

 END CATCH 

END;

GO

GRANT EXECUTE ON [SQLRunbook].[pDocumentRatingUpsert] TO [SQLRunbookUserRole];

GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLRunbook'
           AND SPECIFIC_NAME = N'pDocumentSelectAll' )
 DROP PROCEDURE [SQLRunbook].[pDocumentSelectAll];

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
CREATE PROCEDURE [SQLRunbook].[pDocumentSelectAll]
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

GRANT EXECUTE ON [SQLRunbook].[pDocumentSelectAll] TO [SQLRunbookUserRole];

GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLRunbook'
           AND SPECIFIC_NAME = N'pDocumentSelectByOwner' )
 DROP PROCEDURE [SQLRunbook].[pDocumentSelectByOwner];

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
CREATE PROCEDURE [SQLRunbook].[pDocumentSelectByOwner]
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

GRANT EXECUTE ON [SQLRunbook].[pDocumentSelectByOwner] TO [SQLRunbookUserRole];

GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLRunbook'
           AND SPECIFIC_NAME = N'pDocumentSelectAllFiles' )
 DROP PROCEDURE [SQLRunbook].[pDocumentSelectAllFiles];

GO
/******************************************************************************
**	Auth: Bill Wunder
**	Date: April 1, 2007
**
**	Desc: Get the list of all document files   
**
*******************************************************************************
**	Change History
*******************************************************************************
**	Date		   Author		     Description of Change
**	          
*******************************************************************************/
CREATE PROCEDURE [SQLRunbook].[pDocumentSelectAllFiles] 
AS
BEGIN

-- drop down list for reports
 SELECT [Id] 
   , [File]
 FROM [SQLRunbook].[tDocument];

END;

GO

GRANT EXECUTE ON [SQLRunbook].[pDocumentSelectAllFiles] TO [SQLRunbookUserRole];

GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLRunbook'
           AND SPECIFIC_NAME = N'pDocumentInsert' )
 DROP PROCEDURE [SQLRunbook].[pDocumentInsert];

GO
/******************************************************************************
**	Auth: Bill Wunder
**	Date: April 1, 2007
**
**	Desc: Add a Runbook DOcument   
**
*******************************************************************************
**	Change History
*******************************************************************************
**	Date		   Author		     Description of Change
**	          
*******************************************************************************/
CREATE PROCEDURE [SQLRunbook].[pDocumentInsert] 
 ( @File [NVARCHAR] (450)
 , @Document [VARBINARY] (MAX) 
 , @DocumentType [NVARCHAR] (8) 
 , @LastModifiedDt [DATETIME]
 , @Owner [NVARCHAR] (128)
 , @WatchFileForChange [BIT]) 
AS
BEGIN
DECLARE @TextData [NVARCHAR] (MAX);

 SET XACT_ABORT ON;

 BEGIN TRY

  IF @LastModifiedDt = NULL
   SET @LastModifiedDt = CURRENT_TIMESTAMP

  INSERT [SQLRunbook].[tDocument] ([File], [Document], [DocumentType], [LastModifiedDt], [Owner], [WatchFileForChange])
  VALUES (@File, @Document, @DocumentType, @LastModifiedDt, @Owner, @WatchFileForChange);

 END TRY

 BEGIN CATCH

  SET @TextData = '   @File = ' + ISNULL(CHAR(39) + @File + CHAR(39), 'NULL') + CHAR(13) + CHAR(10)
                + ' , @Document = <binary image - ' + CAST(ISNULL(LEN(@Document),0) AS [NVARCHAR](10)) + 'Bytes>' + CHAR(13) + CHAR(10)
                + ' , @DocumentType = ' + ISNULL(CHAR(39) + @DocumentType + CHAR(39), 'NULL') + CHAR(13) + CHAR(10)
                + ' , @LastModifiedDt = ' + ISNULL(CHAR(39) + CAST(@LastModifiedDt AS [NVARCHAR](30)) + CHAR(39), 'NULL') + CHAR(13) + CHAR(10) 
                + ' , @WatchFileForChange = ' + CAST(ISNULL(@WatchFileForChange, 'NULL') AS [NVARCHAR] (4)) + CHAR(13) + CHAR(10)
                + ' , @Owner = ' + ISNULL(CHAR(39) + @Owner + CHAR(39), 'NULL');

  EXEC [SQLRunbook].[pLogSQLError] @TextData, @@PROCID;

 END CATCH

END;

GO

GRANT EXECUTE ON [SQLRunbook].[pDocumentInsert] TO [SQLRunbookContributorRole];

GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLRunbook'
           AND SPECIFIC_NAME = N'pDocumentUpsert' )
 DROP PROCEDURE [SQLRunbook].[pDocumentUpsert];

GO
/******************************************************************************
**	Auth: Bill Wunder
**	Date: April 1, 2007
**
**	Desc: Update else add a Runbook Document   
**
*******************************************************************************
**	Change History
*******************************************************************************
**	Date		   Author		     Description of Change
**	          
*******************************************************************************/
CREATE PROCEDURE [SQLRunbook].[pDocumentUpsert] 
 ( @File [NVARCHAR] (450)
 , @Document [VARBINARY] (MAX) 
 , @DocumentType [NVARCHAR] (8) 
 , @LastModifiedDt [DATETIME]
 , @Owner [NVARCHAR] (128)
 , @WatchFileForChange [BIT]
 , @DocumentId [INT] OUTPUT) 
AS
BEGIN
DECLARE @TextData [NVARCHAR] (MAX);

 SET XACT_ABORT ON;

 BEGIN TRY

  IF @LastModifiedDt = NULL
   SET @LastModifiedDt = CURRENT_TIMESTAMP

  UPDATE [SQLRunbook].[tDocument]
  SET [Document] = @Document
   , [DocumentType] = @DocumentType
   , [LastModifiedDt] = @LastModifiedDt
   , [Owner] = @Owner 
   , [WatchFileForChange] = @WatchFileForChange
   , @DocumentId = Id
  WHERE [File] = @File

  IF @@ROWCOUNT = 0
   BEGIN
    INSERT [SQLRunbook].[tDocument] ([File], [Document], [DocumentType], [LastModifiedDt], [Owner], [WatchFileForChange])
    VALUES (@File, @Document, @DocumentType, @LastModifiedDt, @Owner, @WatchFileForChange);
 
    SET @DocumentId = SCOPE_IDENTITY()
   END  

 END TRY


 BEGIN CATCH

  SET @TextData = '   @File = ' + ISNULL(CHAR(39) + @File + CHAR(39), 'NULL') + CHAR(13) + CHAR(10)
                + ' , @Document = <binary image - ' + CAST(ISNULL(LEN(@Document),0) AS [NVARCHAR](10)) + 'Bytes>' + CHAR(13) + CHAR(10)
                + ' , @DocumentType = ' + ISNULL(CHAR(39) + @DocumentType + CHAR(39), 'NULL') + CHAR(13) + CHAR(10)
                + ' , @LastModifiedDt = ' + ISNULL(CHAR(39) + CAST(@LastModifiedDt AS [NVARCHAR](30)) + CHAR(39), 'NULL') + CHAR(13) + CHAR(10) 
                + ' , @WatchFileForChange = ' + CAST(ISNULL(@WatchFileForChange, 'NULL') AS [NVARCHAR] (4)) + CHAR(13) + CHAR(10)
                + ' , @Owner = ' + ISNULL(CHAR(39) + @Owner + CHAR(39), 'NULL');
  EXEC [SQLRunbook].[pLogSQLError] @TextData, @@PROCID;

 END CATCH

END;

GO

GRANT EXECUTE ON [SQLRunbook].[pDocumentUpsert] TO [SQLRunbookContributorRole];

GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLRunbook'
           AND SPECIFIC_NAME = N'pDocumentUpdate' )
 DROP PROCEDURE [SQLRunbook].[pDocumentUpdate];

GO
/******************************************************************************
**	Auth: Bill Wunder
**	Date: April 1, 2007
**
**	Desc: Modify a topic   
**
*******************************************************************************
**	Change History
*******************************************************************************
**	Date		   Author		     Description of Change
**	          
*******************************************************************************/
CREATE PROCEDURE [SQLRunbook].[pDocumentUpdate] 
 ( @Id [INT]
 , @File [NVARCHAR] (450)
 , @Document [VARBINARY] (MAX) 
 , @DocumentType [NVARCHAR] (8) 
 , @LastModifiedDt [DATETIME] 
 , @Owner [NVARCHAR] (128)
 , @WatchFileForChange [BIT]) 
AS
BEGIN
DECLARE @TextData [NVARCHAR] (MAX);

 SET XACT_ABORT ON;

 BEGIN TRY

  IF @LastModifiedDt = NULL
   SET @LastModifiedDt = CURRENT_TIMESTAMP

  UPDATE [SQLRunbook].[tDocument] 
  SET [File] = @File
   , [Document] = @Document
   , [DocumentType] = @DocumentType
   , [LastModifiedDt] = @LastModifiedDt
   , [Owner] = @Owner
   , [WatchFileForChange] = @WatchFileForChange
  WHERE [Id] = @Id;

 END TRY

 BEGIN CATCH

  SET @TextData = '   @Id = ' + ISNULL(CONVERT([NVARCHAR] (10), @Id),'NULL') + CHAR(13) + CHAR(10)
                + ' , @File = ' + ISNULL(CHAR(39) + @File + CHAR(39), 'NULL') + CHAR(13) + CHAR(10)
                + ' , @Document = <binary image - ' + CAST(ISNULL(LEN(@Document),0) AS [NVARCHAR](10)) + 'Bytes>' + CHAR(13) + CHAR(10)
                + ' , @DocumentType = ' + ISNULL(CHAR(39) + @DocumentType + CHAR(39), 'NULL') + CHAR(13) + CHAR(10)
                + ' , @LastModifiedDt = ' + ISNULL(CHAR(39) + CAST(@LastModifiedDt AS [NVARCHAR](30)) + CHAR(39), 'NULL') + CHAR(13) + CHAR(10) 
                + ' , @WatchFileForChange = ' + CAST(ISNULL(@WatchFileForChange, 'NULL') AS [NVARCHAR] (4)) + CHAR(13) + CHAR(10)
                + ' , @Owner = ' + ISNULL(CHAR(39) + @Owner + CHAR(39), 'NULL');
  EXEC [SQLRunbook].[pLogSQLError] @TextData, @@PROCID;

 END CATCH 

END;

GO

GRANT EXECUTE ON [SQLRunbook].[pDocumentUpdate] TO [SQLRunbookContributorRole];

GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLRunbook'
           AND SPECIFIC_NAME = N'pDocumentFormUpdate' )
 DROP PROCEDURE [SQLRunbook].[pDocumentFormUpdate];

GO
/******************************************************************************
**	Auth: Bill Wunder
**	Date: April 1, 2007
**
**	Desc: Modify a topic   
**
*******************************************************************************
**	Change History
*******************************************************************************
**	Date		   Author		     Description of Change
**	          
*******************************************************************************/
CREATE PROCEDURE [SQLRunbook].[pDocumentFormUpdate] 
 ( @Id [INT]
 , @DocumentType [NVARCHAR] (8) 
 , @Owner [NVARCHAR] (128)
 , @WatchFileForChange [BIT]) 
AS
BEGIN
DECLARE @TextData [NVARCHAR] (MAX);

 SET XACT_ABORT ON;

 BEGIN TRY

  UPDATE [SQLRunbook].[tDocument] 
  SET[DocumentType] = @DocumentType
   , [Owner] = @Owner
   , [WatchFileForChange] = @WatchFileForChange
  WHERE [Id] = @Id
  AND (   [DocumentType] <> @DocumentType
       OR [Owner] <> @Owner
       OR [WatchFileForChange] <> @WatchFileForChange);

 END TRY

 BEGIN CATCH

  SET @TextData = '   @Id = ' + ISNULL(CONVERT([NVARCHAR] (10), @Id),'NULL') + CHAR(13) + CHAR(10)
                + ' , @DocumentType = ' + ISNULL(CHAR(39) + @DocumentType + CHAR(39), 'NULL') + CHAR(13) + CHAR(10) 
                + ' , @WatchFileForChange = ' + CAST(ISNULL(@WatchFileForChange, 'NULL') AS [NVARCHAR] (4)) + CHAR(13) + CHAR(10)
                + ' , @Owner = ' + ISNULL(CHAR(39) + @Owner + CHAR(39), 'NULL');
  EXEC [SQLRunbook].[pLogSQLError] @TextData, @@PROCID;

 END CATCH 

END;

GO

GRANT EXECUTE ON [SQLRunbook].[pDocumentFormUpdate] TO [SQLRunbookContributorRole];

GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLRunbook'
           AND SPECIFIC_NAME = N'pDocumentDelete' )
 DROP PROCEDURE [SQLRunbook].[pDocumentDelete];

GO
/******************************************************************************
**	Auth: Bill Wunder
**	Date: April 1, 2007
**
**	Desc: delete a runbook document  
**
*******************************************************************************
**	Change History
*******************************************************************************
**	Date		   Author		     Description of Change
**	          
*******************************************************************************/
CREATE PROCEDURE [SQLRunbook].[pDocumentDelete] 
 ( @DocumentId [Int] ) 
AS
BEGIN
DECLARE @TextData [NVARCHAR] (MAX);

 SET XACT_ABORT ON;

 BEGIN TRY 
  
  WHILE EXISTS (SELECT * FROM [SQLRunbook].[tTopicDocument]
                WHERE [DocumentId] = @DocumentId)
   DELETE TOP (1) [SQLRunbook].[tTopicDocument]
   WHERE [DocumentId] = @DocumentId;

  WHILE EXISTS (SELECT * FROM [SQLRunbook].[tDocumentRating]
                WHERE [DocumentId] = @DocumentId)
   DELETE TOP (1) [SQLRunbook].[tDocumentRating]
   WHERE [DOcumentId] = @DocumentId;

  DELETE [SQLRunbook].[tTopicDocument]
  WHERE [DocumentId] = @DocumentId;

 END TRY

 BEGIN CATCH

  SET @TextData = '   @DocumentId = ' + ISNULL(CONVERT([NVARCHAR] (10), @DocumentId),'NULL');
  EXEC [SQLRunbook].[pLogSQLError] @TextData, @@PROCID;

 END CATCH 

END;

GO

GRANT EXECUTE ON [SQLRunbook].[pDocumentDelete] TO [SQLRunbookContributorRole];

GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLRunbook'
           AND SPECIFIC_NAME = N'pDocumentGet' )
 DROP PROCEDURE [SQLRunbook].[pDocumentGet];

GO
/******************************************************************************
**	Auth: Bill Wunder
**	Date: April 1, 2007
**
**	Desc: Get the document for a Document row   
**
*******************************************************************************
**	Change History
*******************************************************************************
**	Date		   Author		     Description of Change
**	          
*******************************************************************************/
CREATE PROCEDURE [SQLRunbook].[pDocumentGet]
 ( @Id [INT]
 , @Document [VARBINARY](MAX) OUTPUT ) 
AS
BEGIN

 SELECT @Document = [Document]
 FROM [SQLRunbook].[tDocument]
 WHERE [Id] = @Id;

END;

GO

GRANT EXECUTE ON [SQLRunbook].[pDocumentGet] TO [SQLRunbookUserRole];

GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLRunbook'
           AND SPECIFIC_NAME = N'pDocumentSelectBLOBById' )
 DROP PROCEDURE [SQLRunbook].[pDocumentSelectBLOBById];

GO
/******************************************************************************
**	Auth: Bill Wunder
**	Date: April 1, 2007
**
**	Desc: Select the document from a Document row   
**        
*******************************************************************************
**	Change History
*******************************************************************************
**	Date		   Author		     Description of Change
**	          
*******************************************************************************/
CREATE PROCEDURE [SQLRunbook].[pDocumentSelectBLOBById]
 ( @Id [INT] ) 
AS
BEGIN

 SELECT [Document] 
  , [DocumentType]
 FROM [SQLRunbook].[tDocument]
 WHERE [Id] = @Id;

END;

GO

GRANT EXECUTE ON [SQLRunbook].[pDocumentSelectBLOBById] TO [SQLRunbookUserRole];

GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLRunbook'
           AND SPECIFIC_NAME = N'pDocumentGetByFile' )
 DROP PROCEDURE [SQLRunbook].[pDocumentGetByFile];

GO
/******************************************************************************
**	Auth: Bill Wunder
**	Date: April 1, 2007
**
**	Desc: Get all attribs in db for a document by the file name   
**
*******************************************************************************
**	Change History
*******************************************************************************
**	Date		   Author		     Description of Change
**	          
*******************************************************************************/
CREATE PROCEDURE [SQLRunbook].[pDocumentGetByFile]
 ( @File NVARCHAR(450)
 , @Id [INT] OUTPUT
 , @DocumentLength [Integer] OUTPUT
 , @DocumentType [NVARCHAR](8) OUTPUT
 , @LastModifiedDt [DATETIME] OUTPUT
 , @Owner [NVARCHAR](128) OUTPUT
 , @WatchFileForChange [BIT] OUTPUT
 , @IsAdmin [BIT] OUTPUT ) 
AS
BEGIN

 SELECT @Id = [Id] 
  , @DocumentLength = Len([Document])
  , @DocumentType = [DocumentType]
  , @LastModifiedDt = [LastModifiedDt]
  , @Owner = [Owner]
  , @WatchFileForChange = [WatchFileForChange]
 FROM [SQLRunbook].[tDocument]
 WHERE [File] = @File;

 SET @IsAdmin = (SELECT [SQLRunbook].[fnIsAdmin]())

END;

GO

GRANT EXECUTE ON [SQLRunbook].[pDocumentGetByFile] TO [SQLRunbookUserRole];

GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLRunbook'
           AND SPECIFIC_NAME = N'pDocumentSelectForMonitor' )
 DROP PROCEDURE [SQLRunbook].[pDocumentSelectForMonitor];

GO
/******************************************************************************
**	Auth: Bill Wunder
**	Date: April 1, 2007
**
**	Desc: Get attribs for all documents to be monitored   
**  a few hundred would be exceptionally large (but nice to have)
**
*******************************************************************************
**	Change History
*******************************************************************************
**	Date		   Author		     Description of Change
**	          
*******************************************************************************/
CREATE PROCEDURE [SQLRunbook].[pDocumentSelectForMonitor]
AS
BEGIN

 SELECT [File]
  , [LastModifiedDt]
  , [Owner]
 FROM [SQLRunbook].[tDocument]
 WHERE [WatchFileForChange] = 1
 ORDER BY [File];

END;

GO

GRANT EXECUTE ON [SQLRunbook].[pDocumentSelectForMonitor] TO [SQLRunbookUserRole];

GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLRunbook'
           AND SPECIFIC_NAME = N'pDocumentSelectByTopicName' )
 DROP PROCEDURE [SQLRunbook].[pDocumentSelectByTopicName];

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
CREATE PROCEDURE [SQLRunbook].[pDocumentSelectByTopicName]
 ( @TopicName [NVARCHAR] (128) )
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
 JOIN [SQLRunbook].[tTopic] t
 ON t.[Id] = td.[TopicId]
 WHERE t.[Name] = @TopicName
 ORDER BY [File];

END;

GO

GRANT EXECUTE ON [SQLRunbook].[pDocumentSelectByTopicName] TO [SQLRunbookUserRole];

GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLRunbook'
           AND SPECIFIC_NAME = N'pDocumentSelectByCategoryId' )
 DROP PROCEDURE [SQLRunbook].[pDocumentSelectByCategoryId];

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
CREATE PROCEDURE [SQLRunbook].[pDocumentSelectByCategoryId]
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

GRANT EXECUTE ON [SQLRunbook].[pDocumentSelectByCategoryId] TO [SQLRunbookUserRole];

GO


IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLRunbook'
           AND SPECIFIC_NAME = N'pDocumentSelectByDocumentId' )
 DROP PROCEDURE [SQLRunbook].[pDocumentSelectByDocumentId];

GO
/******************************************************************************
**	Auth: Bill Wunder
**	Date: March 13, 2007
**
**	Desc: Select a Runbook documents by if (for catalog details)
**        specified category
**
*******************************************************************************
**	Change History
*******************************************************************************
**	Date		   Author		     Description of Change
**	          
*******************************************************************************/
CREATE PROCEDURE [SQLRunbook].[pDocumentSelectByDocumentId]
 ( @DocumentId [Int] )
AS
BEGIN

 SELECT DISTINCT d.[Id] 
  , d.[File]
  , d.[LastModifiedDt]
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
 WHERE d.[Id] = @DocumentId;

END;

GO

GRANT EXECUTE ON [SQLRunbook].[pDocumentSelectByDocumentId] TO [SQLRunbookUserRole];

GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLRunbook'
           AND SPECIFIC_NAME = N'pDocumentSelectByDate' )
 DROP PROCEDURE [SQLRunbook].[pDocumentSelectByDate];

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
CREATE PROCEDURE [SQLRunbook].[pDocumentSelectByDate]
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

GRANT EXECUTE ON [SQLRunbook].[pDocumentSelectByDate] TO [SQLRunbookUserRole];

GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLRunbook'
           AND SPECIFIC_NAME = N'pDocumentSelectByDateRange' )
 DROP PROCEDURE [SQLRunbook].[pDocumentSelectByDateRange];

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
CREATE PROCEDURE [SQLRunbook].[pDocumentSelectByDateRange]
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

GRANT EXECUTE ON [SQLRunbook].[pDocumentSelectByDateRange] TO [SQLRunbookUserRole];

GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLRunbook'
           AND SPECIFIC_NAME = N'pDocumentSelectChangedByDateRange' )
 DROP PROCEDURE [SQLRunbook].[pDocumentSelectChangedByDateRange];

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
CREATE PROCEDURE [SQLRunbook].[pDocumentSelectChangedByDateRange]
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

GRANT EXECUTE ON [SQLRunbook].[pDocumentSelectChangedByDateRange] TO [SQLRunbookUserRole];

GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLRunbook'
           AND SPECIFIC_NAME = N'pDocumentSelectByContains' )
 DROP PROCEDURE [SQLRunbook].[pDocumentSelectByContains];

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
CREATE PROCEDURE [SQLRunbook].[pDocumentSelectByContains] 
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

GRANT EXECUTE ON [SQLRunbook].[pDocumentSelectByContains] TO [SQLRunbookUserRole];

GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLRunbook'
           AND SPECIFIC_NAME = N'pRunbookCatalog' )
 DROP PROCEDURE [SQLRunbook].[pRunbookCatalog];

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
CREATE PROCEDURE [SQLRunbook].[pRunbookCatalog] 
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

GRANT EXECUTE ON [SQLRunbook].[pRunbookCatalog] TO [SQLRunbookUserRole];

GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLRunbook'
           AND SPECIFIC_NAME = N'pRunbookActivity' )
 DROP PROCEDURE [SQLRunbook].[pRunbookActivity];

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
CREATE PROCEDURE [SQLRunbook].[pRunbookActivity]
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

GRANT EXECUTE ON [SQLRunbook].[pRunbookActivity] TO [SQLRunbookUserRole];

GO

-- add categories
-- avoid all escaped inline quotes, do not use inline double quotes 
IF NOT EXISTS (SELECT * FROM [SQLRunbook].[tCategory])
 BEGIN
   EXEC [SQLRunbook].[pCategoryInsert] 'Alerts','event based, conditional and automated system monitoring notifications';
   EXEC [SQLRunbook].[pCategoryInsert] 'Application','application specific data layer considerations & requirements';
   EXEC [SQLRunbook].[pCategoryInsert] 'Architecture','software, systems, & infrastructure design fundamentals';
   EXEC [SQLRunbook].[pCategoryInsert] 'Backups','all system and database backup and restore activity';
   EXEC [SQLRunbook].[pCategoryInsert] 'Batch Window','Window of least application activity or otherwise designated as preferred time for system maintenance ';
   EXEC [SQLRunbook].[pCategoryInsert] 'Best Practice','routine work practices that produce the most desirable results';
   EXEC [SQLRunbook].[pCategoryInsert] 'Business Continuity','information necessary to sustain the ongoing business concern';
   EXEC [SQLRunbook].[pCategoryInsert] 'Business Rule','rule to avoid adverse business consequences for a conduct, action, practice, or procedure';
   EXEC [SQLRunbook].[pCategoryInsert] 'Change Management','patterns & practices to application software changes';
   EXEC [SQLRunbook].[pCategoryInsert] 'Coding Standards','any well known internal definition of the minimally acceptable';
   EXEC [SQLRunbook].[pCategoryInsert] 'Configuration','the required state for a platform, SQL Server or database property or attribute';
   EXEC [SQLRunbook].[pCategoryInsert] 'Contact','person to contact including how to contact and the circumstances under which the contact would be appropriate';
   EXEC [SQLRunbook].[pCategoryInsert] 'Data Lifecycle','Delete/Archive, ETL, synchronization and other activities related to the temporal value of data';
   EXEC [SQLRunbook].[pCategoryInsert] 'Data Model','the logical abstraction of the application requirements into the theoretical relational database model';
   EXEC [SQLRunbook].[pCategoryInsert] 'Database Maintenance','proactive measures intended to assure the health of a database';
   EXEC [SQLRunbook].[pCategoryInsert] 'Database Design','work practices for producing & refining the Data Model';
   EXEC [SQLRunbook].[pCategoryInsert] 'Database Development','the physical implementation of the Data Model';
   EXEC [SQLRunbook].[pCategoryInsert] 'Encryption','server master keys, database master keys, symmetric keys, asymmetric keys, certificates, implenentation details';
   EXEC [SQLRunbook].[pCategoryInsert] 'Forward Recovery','contingencies for all identified restart/recovery scenarios';
   EXEC [SQLRunbook].[pCategoryInsert] 'Fulltext Search','search of unstructured data columns';
   EXEC [SQLRunbook].[pCategoryInsert] 'Installation','datastore instance and application installation requirements, settings, and work practices';
   EXEC [SQLRunbook].[pCategoryInsert] 'Hardware','requirements, migrations, tests, evaluations, recommendations, etc.';
   EXEC [SQLRunbook].[pCategoryInsert] 'High Availability','measures intentionally undertaken to meet business data availablilty expectation';
   EXEC [SQLRunbook].[pCategoryInsert] 'Methodology','routines for repetitive or mundane activities requiring everyones participation';
   EXEC [SQLRunbook].[pCategoryInsert] 'Monitoring','measures to verify and validate how effectively the database meets identified business requirements';
   EXEC [SQLRunbook].[pCategoryInsert] 'Notifications','strategies, recipient maintenance details, listings, etc.';
   EXEC [SQLRunbook].[pCategoryInsert] 'Optimizations','improvements to data layer usability, performance, or efficiency';
   EXEC [SQLRunbook].[pCategoryInsert] 'Performance','objective and systematic performance measurement, analysis and tuning';
   EXEC [SQLRunbook].[pCategoryInsert] 'Support','chronicle of communications with any database related software vendor support';
   EXEC [SQLRunbook].[pCategoryInsert] 'Research','proof-of-concept information and findings';
   EXEC [SQLRunbook].[pCategoryInsert] 'Reverts','rollbacks and backouts of newly implemented changes';
   EXEC [SQLRunbook].[pCategoryInsert] 'Security','internal measures of security and models for security';
   EXEC [SQLRunbook].[pCategoryInsert] 'Scalability', 'ability to absorb and adapt to increased activity by increasing capacity or resources';
   EXEC [SQLRunbook].[pCategoryInsert] 'Scheduled Tasks', 'SQL Agent and other non-application scheduled events that touch the database';
   EXEC [SQLRunbook].[pCategoryInsert] 'Software Lifecycle','the internal development/test/runtime inter-environmental considerations';
   EXEC [SQLRunbook].[pCategoryInsert] 'Start-up','starting or restarting SQL Instances, applications, and systems';
   EXEC [SQLRunbook].[pCategoryInsert] 'Templates','a shell, outline or generalize description';
   EXEC [SQLRunbook].[pCategoryInsert] 'Testing','activities to verify a course of action before the course of action is actually undertaken';
   EXEC [SQLRunbook].[pCategoryInsert] 'MemoToSelf','needs to be addressed - when unclear of category or uncertain if useful or as a reminder for follow-up.';
   EXEC [SQLRunbook].[pCategoryInsert] 'Troubleshooting','problem resolution tactics, hueristics and recipes';
 END;  

GO

-- Ratings
IF NOT EXISTS (SELECT * FROM [SQLRunbook].[tRating])
 BEGIN

  -- hard coded into administrators console...
  -- lower rating id is better
  -- do NOT include stand-aside in scoring -document score of (1+2+1+2+1 + 3+4+4 + 5+6)/10=2.9 
  -- DO include in tally - document ratings of 5-accepted, 3-conditional, 2 unacceptable, 1 stand-aside - no idea what this means
  INSERT [SQLRunbook].[tRating] ([Id], [Name], [Notes]) 
  VALUES (1, 'helpful', 'Accurate. Contains accurate information used to successfully complete the noted work task(s).');

  INSERT [SQLRunbook].[tRating] ([Id], [Name], [Notes]) 
  VALUES (2, 'reviewed', 'Accurate. Reviewed for complete and accurate information with regard to the noted areas of consideration.');

  INSERT [SQLRunbook].[tRating] ([Id], [Name], [Notes]) 
  VALUES (3, 'useful', 'Adequate. Fundamentally correct with unclear details as noted. Author is encouraged to revise and resubmit.');

  INSERT [SQLRunbook].[tRating] ([Id], [Name], [Notes]) 
  VALUES (4, 'reservations', 'Adequate. Area(s) of missing or inaccurate detail as noted. Recommend revision and resubmittal.');

  INSERT [SQLRunbook].[tRating] ([Id], [Name], [Notes]) 
  VALUES (5, 'incorrect', 'Unacceptable. Contains errors, omissions or mistakes that produce incorrect results. The noted corrections and/or recovery actions are necessary. Must be revised.');

  INSERT [SQLRunbook].[tRating] ([Id], [Name], [Notes]) 
  VALUES (6, 'block', 'Unacceptable. Do not use this information for the reason(s) noted. Correct results are not possible using this information.');

  INSERT [SQLRunbook].[tRating] ([Id], [Name], [Notes]) 
  VALUES (7, 'stand-aside', 'Not rated for reason(s) noted. Will support as determined appropriate by group.');

 END;

GO

