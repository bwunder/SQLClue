/******************************************************************************
Removes configuration created by instSQLClueRunbook.sql script

Use the Tools|SQLClue Runbook|Uninstall' menu option. It will run this script. 
Running this script manually will corrupt the SQLClue Runbook install!
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

IF EXISTS (SELECT * 
           FROM [sys].[fulltext_indexes] 
           WHERE object_name([object_id]) = 'tCategory') 
 DROP FULLTEXT INDEX ON [SQLRunbook].[tCategory]

GO

If EXISTS (SELECT * 
           FROM [sys].[fulltext_indexes] 
           WHERE object_name([object_id]) = 'tCategoryRating') 
 DROP FULLTEXT INDEX ON [SQLRunbook].[tCategoryRating];

GO

IF EXISTS (SELECT * 
           FROM [sys].[fulltext_indexes] 
           WHERE object_name([object_id]) = 'tTopic') 
 DROP FULLTEXT INDEX ON [SQLRunbook].[tTopic]

GO

IF EXISTS (SELECT * 
           FROM [sys].[fulltext_indexes] 
           WHERE object_name([object_id]) = 'tTopicRating') 
 DROP FULLTEXT INDEX ON [SQLRunbook].[tTopicRating];

GO

IF EXISTS (SELECT * 
           FROM [sys].[fulltext_indexes] 
           WHERE object_name([object_id]) = 'tDocument') 
 DROP FULLTEXT INDEX ON [SQLRunbook].[tDocument]

GO

IF EXISTS (SELECT * 
           FROM [sys].[fulltext_indexes] 
           WHERE object_name([object_id]) = 'tDocumentRating') 
 DROP FULLTEXT INDEX ON [SQLRunbook].[tDocumentRating];

GO

IF EXISTS (SELECT * 
           FROM [sys].[fulltext_indexes] 
           WHERE object_name([object_id]) = 'tUser') 
 DROP FULLTEXT INDEX ON [SQLRunbook].[tUser];

GO

IF EXISTS (SELECT * 
           FROM [sys].[fulltext_indexes] 
           WHERE object_name([object_id]) = 'tRating') 
 DROP FULLTEXT INDEX ON [SQLRunbook].[tRating];

GO

IF EXISTS (SELECT * 
           FROM [sys].[fulltext_catalogs] 
           WHERE [name] = 'ftSQLRunbookCatalog')
 DROP FULLTEXT CATALOG [ftSQLRunbookCatalog];

GO

IF EXISTS (SELECT * 
           FROM [INFORMATION_SCHEMA].[ROUTINES] 
           WHERE [SPECIFIC_SCHEMA] = N'SQLRunbook'
           AND [SPECIFIC_NAME] = N'fnFormatNote' )
 DROP FUNCTION [SQLRunbook].[fnFormatNote];

GO

IF EXISTS (SELECT * 
           FROM [INFORMATION_SCHEMA].[ROUTINES] 
           WHERE [SPECIFIC_SCHEMA] = N'SQLRunbook'
           AND [SPECIFIC_NAME] = N'fnIsAdmin' )
 DROP FUNCTION [SQLRunbook].[fnIsAdmin];

GO

IF EXISTS (SELECT * 
           FROM [INFORMATION_SCHEMA].[ROUTINES] 
           WHERE [SPECIFIC_SCHEMA] = N'SQLRunbook'
           AND [SPECIFIC_NAME] = N'pOptionGet' )
 DROP PROCEDURE [SQLRunbook].[pOptionGet];

GO

IF EXISTS (SELECT * 
           FROM [INFORMATION_SCHEMA].[ROUTINES] 
           WHERE [SPECIFIC_SCHEMA] = N'SQLRunbook'
           AND [SPECIFIC_NAME] = N'pOptionSet' )
 DROP PROCEDURE [SQLRunbook].[pOptionSet];

GO

IF EXISTS (SELECT * 
           FROM [INFORMATION_SCHEMA].[ROUTINES] 
           WHERE [SPECIFIC_SCHEMA] = N'SQLRunbook'
           AND [SPECIFIC_NAME] = N'pCategoryContains' )
 DROP PROCEDURE [SQLRunbook].[pCategoryContains];

GO

IF EXISTS (SELECT * 
           FROM [INFORMATION_SCHEMA].[ROUTINES] 
           WHERE [SPECIFIC_SCHEMA] = N'SQLRunbook'
           AND [SPECIFIC_NAME] = N'pCategoryDelete' )
 DROP PROCEDURE [SQLRunbook].[pCategoryDelete];

GO

IF EXISTS (SELECT * 
           FROM [INFORMATION_SCHEMA].[ROUTINES] 
           WHERE [SPECIFIC_SCHEMA] = N'SQLRunbook'
           AND [SPECIFIC_NAME] = N'pCategoryInsert' )
 DROP PROCEDURE [SQLRunbook].[pCategoryInsert];

GO

IF EXISTS (SELECT * 
           FROM [INFORMATION_SCHEMA].[ROUTINES] 
           WHERE [SPECIFIC_SCHEMA] = N'SQLRunbook'
           AND [SPECIFIC_NAME] = N'pCategoryRatingUpsert' )
 DROP PROCEDURE [SQLRunbook].[pCategoryRatingUpsert];

GO

IF EXISTS (SELECT * 
           FROM [INFORMATION_SCHEMA].[ROUTINES] 
           WHERE [SPECIFIC_SCHEMA] = N'SQLRunbook'
           AND [SPECIFIC_NAME] = N'pCategoryRatingGetByUser' )
 DROP PROCEDURE [SQLRunbook].[pCategoryRatingGetByUser]

GO

IF EXISTS (SELECT * 
           FROM [INFORMATION_SCHEMA].[ROUTINES] 
           WHERE [SPECIFIC_SCHEMA] = N'SQLRunbook'
           AND [SPECIFIC_NAME] = N'pCategorySelectActive' )
 DROP PROCEDURE [SQLRunbook].[pCategorySelectActive];

GO

IF EXISTS (SELECT * 
           FROM [INFORMATION_SCHEMA].[ROUTINES] 
           WHERE [SPECIFIC_SCHEMA] = N'SQLRunbook'
           AND [SPECIFIC_NAME] = N'pCategorySelectAll' )
 DROP PROCEDURE [SQLRunbook].[pCategorySelectAll];

GO

IF EXISTS (SELECT * 
           FROM [INFORMATION_SCHEMA].[ROUTINES] 
           WHERE [SPECIFIC_SCHEMA] = N'SQLRunbook'
           AND [SPECIFIC_NAME] = N'pCategorySelectByName' )
 DROP PROCEDURE [SQLRunbook].[pCategorySelectByName];

GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLRunbook'
           AND SPECIFIC_NAME = N'pCategorySelectByDateRange' )
 DROP PROCEDURE [SQLRunbook].[pCategorySelectByDateRange];

GO

IF EXISTS (SELECT * 
           FROM [INFORMATION_SCHEMA].[ROUTINES] 
           WHERE [SPECIFIC_SCHEMA] = N'SQLRunbook'
           AND [SPECIFIC_NAME] = N'pCategorySummary' )
 DROP PROCEDURE [SQLRunbook].[pCategorySummary];

GO

IF EXISTS (SELECT * 
           FROM [INFORMATION_SCHEMA].[ROUTINES] 
           WHERE [SPECIFIC_SCHEMA] = N'SQLRunbook'
           AND [SPECIFIC_NAME] = N'pCategoryTopicDelete' )
 DROP PROCEDURE [SQLRunbook].[pCategoryTopicDelete];

GO

IF EXISTS (SELECT * 
           FROM [INFORMATION_SCHEMA].[ROUTINES] 
           WHERE [SPECIFIC_SCHEMA] = N'SQLRunbook'
           AND [SPECIFIC_NAME] = N'pCategoryTopicInsert' )
 DROP PROCEDURE [SQLRunbook].[pCategoryTopicInsert];

GO

IF EXISTS (SELECT * 
           FROM [INFORMATION_SCHEMA].[ROUTINES] 
           WHERE [SPECIFIC_SCHEMA] = N'SQLRunbook'
           AND [SPECIFIC_NAME] = N'pCategoryTopicSelectAll' )
 DROP PROCEDURE [SQLRunbook].[pCategoryTopicSelectAll];

GO

IF EXISTS (SELECT * 
           FROM [INFORMATION_SCHEMA].[ROUTINES] 
           WHERE [SPECIFIC_SCHEMA] = N'SQLRunbook'
           AND [SPECIFIC_NAME] = N'pCategoryTopicSelectByCategoryId' )
 DROP PROCEDURE [SQLRunbook].[pCategoryTopicSelectByCategoryId];

GO

IF EXISTS (SELECT * 
           FROM [INFORMATION_SCHEMA].[ROUTINES] 
           WHERE [SPECIFIC_SCHEMA] = N'SQLRunbook'
           AND [SPECIFIC_NAME] = N'pCategoryTopicSelectByDateRange' )
 DROP PROCEDURE [SQLRunbook].[pCategoryTopicSelectByDateRange];

GO 

IF EXISTS (SELECT * 
           FROM [INFORMATION_SCHEMA].[ROUTINES] 
           WHERE [SPECIFIC_SCHEMA] = N'SQLRunbook'
           AND [SPECIFIC_NAME] = N'pCategoryTopicSelectByTopicId' )
 DROP PROCEDURE [SQLRunbook].[pCategoryTopicSelectByTopicId];

GO 

IF EXISTS (SELECT * 
           FROM [INFORMATION_SCHEMA].[ROUTINES] 
           WHERE [SPECIFIC_SCHEMA] = N'SQLRunbook'
           AND [SPECIFIC_NAME] = N'pCategoryUpdate' )
 DROP PROCEDURE [SQLRunbook].[pCategoryUpdate];

GO

IF EXISTS (SELECT * 
           FROM [INFORMATION_SCHEMA].[ROUTINES] 
           WHERE [SPECIFIC_SCHEMA] = N'SQLRunbook'
           AND [SPECIFIC_NAME] = N'pContributors' )
 DROP PROCEDURE [SQLRunbook].[pContributors];

GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLRunbook'
           AND SPECIFIC_NAME = N'pDocumentSelectChangedByModifiedDateRange' )
 DROP PROCEDURE [SQLRunbook].[pDocumentSelectChangedByModifiedDateRange];

GO

IF EXISTS (SELECT * 
           FROM [INFORMATION_SCHEMA].[ROUTINES] 
           WHERE [SPECIFIC_SCHEMA] = N'SQLRunbook'
           AND [SPECIFIC_NAME] = N'pDocumentDelete' )
 DROP PROCEDURE [SQLRunbook].[pDocumentDelete];

GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLRunbook'
           AND SPECIFIC_NAME = N'pDocumentFormUpdate' )
 DROP PROCEDURE [SQLRunbook].[pDocumentFormUpdate];

GO

IF EXISTS (SELECT * 
           FROM [INFORMATION_SCHEMA].[ROUTINES] 
           WHERE [SPECIFIC_SCHEMA] = N'SQLRunbook'
           AND [SPECIFIC_NAME] = N'pDocumentGet' )
 DROP PROCEDURE [SQLRunbook].[pDocumentGet];

GO

IF EXISTS (SELECT * 
           FROM [INFORMATION_SCHEMA].[ROUTINES] 
           WHERE [SPECIFIC_SCHEMA] = N'SQLRunbook'
           AND [SPECIFIC_NAME] = N'pDocumentGetByFile' )
 DROP PROCEDURE [SQLRunbook].[pDocumentGetByFile];

GO

IF EXISTS (SELECT * 
           FROM [INFORMATION_SCHEMA].[ROUTINES] 
           WHERE [SPECIFIC_SCHEMA] = N'SQLRunbook'
           AND [SPECIFIC_NAME] = N'pDocumentInsert' )
 DROP PROCEDURE [SQLRunbook].[pDocumentInsert];

GO

IF EXISTS (SELECT * 
           FROM [INFORMATION_SCHEMA].[ROUTINES] 
           WHERE [SPECIFIC_SCHEMA] = N'SQLRunbook'
           AND [SPECIFIC_NAME] = N'pDocumentRatingGetByUser' )
 DROP PROCEDURE [SQLRunbook].[pDocumentRatingGetByUser]

GO

IF EXISTS (SELECT * 
           FROM [INFORMATION_SCHEMA].[ROUTINES] 
           WHERE [SPECIFIC_SCHEMA] = N'SQLRunbook'
           AND [SPECIFIC_NAME] = N'pDocumentRatingUpsert' )
 DROP PROCEDURE [SQLRunbook].[pDocumentRatingUpsert];

GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLRunbook'
           AND SPECIFIC_NAME = N'pDocumentRatingSelectByDateRange' )
 DROP PROCEDURE [SQLRunbook].[pDocumentRatingSelectByDateRange];

GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLRunbook'
           AND SPECIFIC_NAME = N'pDocumentRatingSelectByOwner' )
 DROP PROCEDURE [SQLRunbook].[pDocumentRatingSelectByOwner];

GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLRunbook'
           AND SPECIFIC_NAME = N'pDocumentRatingSelectByReviewer' )
 DROP PROCEDURE [SQLRunbook].[pDocumentRatingSelectByReviewer];

GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLRunbook'
           AND SPECIFIC_NAME = N'pDocumentRatingSelectExpiredByReviewer' )
 DROP PROCEDURE [SQLRunbook].[pDocumentRatingSelectExpiredByReviewer];

GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLRunbook'
           AND SPECIFIC_NAME = N'pDocumentRatingSelectByDocumentId' )
 DROP PROCEDURE [SQLRunbook].[pDocumentRatingSelectByDocumentId];

GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLRunbook'
           AND SPECIFIC_NAME = N'pDocumentRatingSelectByDocumentOwner' )
 DROP PROCEDURE [SQLRunbook].[pDocumentRatingSelectByDocumentOwner];

GO

IF EXISTS (SELECT * 
           FROM [INFORMATION_SCHEMA].[ROUTINES] 
           WHERE [SPECIFIC_SCHEMA] = N'SQLRunbook'
           AND [SPECIFIC_NAME] = N'pDocumentSelectAll' )
 DROP PROCEDURE [SQLRunbook].[pDocumentSelectAll];

GO

IF EXISTS (SELECT * 
           FROM [INFORMATION_SCHEMA].[ROUTINES] 
           WHERE [SPECIFIC_SCHEMA] = N'SQLRunbook'
           AND [SPECIFIC_NAME] = N'pDocumentSelectAllFiles' )
 DROP PROCEDURE [SQLRunbook].[pDocumentSelectAllFiles];

GO

IF EXISTS (SELECT * 
           FROM [INFORMATION_SCHEMA].[ROUTINES] 
           WHERE [SPECIFIC_SCHEMA] = N'SQLRunbook'
           AND [SPECIFIC_NAME] = N'pDocumentSelectBLOBById' )
 DROP PROCEDURE [SQLRunbook].[pDocumentSelectBLOBById];

GO

IF EXISTS (SELECT * 
           FROM [INFORMATION_SCHEMA].[ROUTINES] 
           WHERE [SPECIFIC_SCHEMA] = N'SQLRunbook'
           AND [SPECIFIC_NAME] = N'pDocumentSelectByCategoryId' )
 DROP PROCEDURE [SQLRunbook].[pDocumentSelectByCategoryId];

GO

IF EXISTS (SELECT * 
           FROM [INFORMATION_SCHEMA].[ROUTINES] 
           WHERE [SPECIFIC_SCHEMA] = N'SQLRunbook'
           AND [SPECIFIC_NAME] = N'pDocumentSelectByContains' )
 DROP PROCEDURE [SQLRunbook].[pDocumentSelectByContains];

GO

IF EXISTS (SELECT * 
           FROM [INFORMATION_SCHEMA].[ROUTINES] 
           WHERE [SPECIFIC_SCHEMA] = N'SQLRunbook'
           AND [SPECIFIC_NAME] = N'pDocumentSelectByDate' )
 DROP PROCEDURE [SQLRunbook].[pDocumentSelectByDate];

GO

IF EXISTS (SELECT * 
           FROM [INFORMATION_SCHEMA].[ROUTINES] 
           WHERE [SPECIFIC_SCHEMA] = N'SQLRunbook'
           AND [SPECIFIC_NAME] = N'pDocumentSelectByDateRange' )
 DROP PROCEDURE [SQLRunbook].[pDocumentSelectByDateRange];

GO

IF EXISTS (SELECT * 
           FROM [INFORMATION_SCHEMA].[ROUTINES] 
           WHERE [SPECIFIC_SCHEMA] = N'SQLRunbook'
           AND [SPECIFIC_NAME] = N'pDocumentSelectByDocumentId' )
 DROP PROCEDURE [SQLRunbook].[pDocumentSelectByDocumentId];

GO

IF EXISTS (SELECT * 
           FROM [INFORMATION_SCHEMA].[ROUTINES] 
           WHERE [SPECIFIC_SCHEMA] = N'SQLRunbook'
           AND [SPECIFIC_NAME] = N'pDocumentSelectByOwner' )
 DROP PROCEDURE [SQLRunbook].[pDocumentSelectByOwner];

GO

IF EXISTS (SELECT * 
           FROM [INFORMATION_SCHEMA].[ROUTINES] 
           WHERE [SPECIFIC_SCHEMA] = N'SQLRunbook'
           AND [SPECIFIC_NAME] = N'pDocumentSelectByTopicName' )
 DROP PROCEDURE [SQLRunbook].[pDocumentSelectByTopicName];

GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLRunbook'
           AND SPECIFIC_NAME = N'pDocumentSelectChangedByDateRange' )
 DROP PROCEDURE [SQLRunbook].[pDocumentSelectChangedByDateRange];

GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLRunbook'
           AND SPECIFIC_NAME = N'pDocumentSelectForMonitor' )
 DROP PROCEDURE [SQLRunbook].[pDocumentSelectForMonitor];

GO

IF EXISTS (SELECT * 
           FROM [INFORMATION_SCHEMA].[ROUTINES] 
           WHERE [SPECIFIC_SCHEMA] = N'SQLRunbook'
           AND [SPECIFIC_NAME] = N'pDocumentSummary' )
 DROP PROCEDURE [SQLRunbook].[pDocumentSummary];

GO

IF EXISTS (SELECT * 
           FROM [INFORMATION_SCHEMA].[ROUTINES] 
           WHERE [SPECIFIC_SCHEMA] = N'SQLRunbook'
           AND [SPECIFIC_NAME] = N'pDocumentUpdate' )
 DROP PROCEDURE [SQLRunbook].[pDocumentUpdate];

GO

IF EXISTS (SELECT * 
           FROM [INFORMATION_SCHEMA].[ROUTINES] 
           WHERE [SPECIFIC_SCHEMA] = N'SQLRunbook'
           AND [SPECIFIC_NAME] = N'pDocumentUpsert' )
 DROP PROCEDURE [SQLRunbook].[pDocumentUpsert];

GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLRunbook'
           AND SPECIFIC_NAME = N'pFullTextDocumentTypeSelect' )
 DROP PROCEDURE [SQLRunbook].[pFullTextDocumentTypeSelect];

GO

IF EXISTS (SELECT * 
           FROM [INFORMATION_SCHEMA].[ROUTINES] 
           WHERE [SPECIFIC_SCHEMA] = N'SQLRunbook'
           AND [SPECIFIC_NAME] = N'pLogSQLError' )
 DROP PROCEDURE [SQLRunbook].[pLogSQLError];

GO

IF EXISTS (SELECT * 
           FROM [INFORMATION_SCHEMA].[ROUTINES] 
           WHERE [SPECIFIC_SCHEMA] = N'SQLRunbook'
           AND [SPECIFIC_NAME] = N'pMetrics' )
 DROP PROCEDURE [SQLRunbook].[pMetrics];

GO

IF EXISTS (SELECT * 
           FROM [INFORMATION_SCHEMA].[ROUTINES] 
           WHERE [SPECIFIC_SCHEMA] = N'SQLRunbook'
           AND [SPECIFIC_NAME] = N'pRunbookActivity' )
 DROP PROCEDURE [SQLRunbook].[pRunbookActivity];

GO

IF EXISTS (SELECT * 
           FROM [INFORMATION_SCHEMA].[ROUTINES] 
           WHERE [SPECIFIC_SCHEMA] = N'SQLRunbook'
           AND [SPECIFIC_NAME] = N'pRunbookCatalog' )
 DROP PROCEDURE [SQLRunbook].[pRunbookCatalog];

GO

IF EXISTS (SELECT * 
           FROM [INFORMATION_SCHEMA].[ROUTINES] 
           WHERE [SPECIFIC_SCHEMA] = N'SQLRunbook'
           AND [SPECIFIC_NAME] = N'pTopicDelete' )
 DROP PROCEDURE [SQLRunbook].[pTopicDelete];

GO

IF EXISTS (SELECT * 
           FROM [INFORMATION_SCHEMA].[ROUTINES] 
           WHERE [SPECIFIC_SCHEMA] = N'SQLRunbook'
           AND [SPECIFIC_NAME] = N'pTopicDocumentDelete' )
 DROP PROCEDURE [SQLRunbook].[pTopicDocumentDelete];

GO

IF EXISTS (SELECT * 
           FROM [INFORMATION_SCHEMA].[ROUTINES] 
           WHERE [SPECIFIC_SCHEMA] = N'SQLRunbook'
           AND [SPECIFIC_NAME] = N'pTopicDocumentInsert' )
 DROP PROCEDURE [SQLRunbook].[pTopicDocumentInsert];

GO

IF EXISTS (SELECT * 
           FROM [INFORMATION_SCHEMA].[ROUTINES] 
           WHERE [SPECIFIC_SCHEMA] = N'SQLRunbook'
           AND [SPECIFIC_NAME] = N'pTopicDocumentSelectAll' )
 DROP PROCEDURE [SQLRunbook].[pTopicDocumentSelectAll];

GO

IF EXISTS (SELECT * 
           FROM [INFORMATION_SCHEMA].[ROUTINES] 
           WHERE [SPECIFIC_SCHEMA] = N'SQLRunbook'
           AND [SPECIFIC_NAME] = N'pTopicDocumentSelectByDateRange' )
 DROP PROCEDURE [SQLRunbook].[pTopicDocumentSelectByDateRange];

GO

IF EXISTS (SELECT * 
           FROM [INFORMATION_SCHEMA].[ROUTINES] 
           WHERE [SPECIFIC_SCHEMA] = N'SQLRunbook'
           AND [SPECIFIC_NAME] = N'pTopicDocumentSelectByDocumentId' )
 DROP PROCEDURE [SQLRunbook].[pTopicDocumentSelectByDocumentId];

GO

IF EXISTS (SELECT * 
           FROM [INFORMATION_SCHEMA].[ROUTINES] 
           WHERE [SPECIFIC_SCHEMA] = N'SQLRunbook'
           AND [SPECIFIC_NAME] = N'pTopicDocumentSelectByTopicId' )
 DROP PROCEDURE [SQLRunbook].[pTopicDocumentSelectByTopicId];

GO

IF EXISTS (SELECT * 
           FROM [INFORMATION_SCHEMA].[ROUTINES] 
           WHERE [SPECIFIC_SCHEMA] = N'SQLRunbook'
           AND [SPECIFIC_NAME] = N'pTopicGetNotes' )
 DROP PROCEDURE [SQLRunbook].[pTopicGetNotes];

GO

IF EXISTS (SELECT * 
           FROM [INFORMATION_SCHEMA].[ROUTINES] 
           WHERE [SPECIFIC_SCHEMA] = N'SQLRunbook'
           AND [SPECIFIC_NAME] = N'pTopicGetText' )
 DROP PROCEDURE [SQLRunbook].[pTopicGetText];

GO

IF EXISTS (SELECT * 
           FROM [INFORMATION_SCHEMA].[ROUTINES] 
           WHERE [SPECIFIC_SCHEMA] = N'SQLRunbook'
           AND [SPECIFIC_NAME] = N'pTopicInsert' )
 DROP PROCEDURE [SQLRunbook].[pTopicInsert];

GO

IF EXISTS (SELECT * 
           FROM [INFORMATION_SCHEMA].[ROUTINES] 
           WHERE [SPECIFIC_SCHEMA] = N'SQLRunbook'
           AND [SPECIFIC_NAME] = N'pTopicRatingGetByUser' )
 DROP PROCEDURE [SQLRunbook].[pTopicRatingGetByUser]

GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLRunbook'
           AND SPECIFIC_NAME = N'pTopicRatingSelectByDateRange' )
 DROP PROCEDURE [SQLRunbook].[pTopicRatingSelectByDateRange];

GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLRunbook'
           AND SPECIFIC_NAME = N'pTopicRatingSelectByOwner' )
 DROP PROCEDURE [SQLRunbook].[pTopicRatingSelectByOwner];

GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLRunbook'
           AND SPECIFIC_NAME = N'pTopicRatingSelectByReviewer' )
 DROP PROCEDURE [SQLRunbook].[pTopicRatingSelectByReviewer];

GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLRunbook'
           AND SPECIFIC_NAME = N'pTopicRatingSelectExpiredByReviewer' )
 DROP PROCEDURE [SQLRunbook].[pTopicRatingSelectExpiredByReviewer];

GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLRunbook'
           AND SPECIFIC_NAME = N'pTopicRatingSelectByTopicName' )
 DROP PROCEDURE [SQLRunbook].[pTopicRatingSelectByTopicName];

GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLRunbook'
           AND SPECIFIC_NAME = N'pTopicRatingSelectByTopicId' )
 DROP PROCEDURE [SQLRunbook].[pTopicRatingSelectByTopicId];

GO

IF EXISTS (SELECT * 
           FROM [INFORMATION_SCHEMA].[ROUTINES] 
           WHERE [SPECIFIC_SCHEMA] = N'SQLRunbook'
           AND [SPECIFIC_NAME] = N'pTopicRatingUpsert' )
 DROP PROCEDURE [SQLRunbook].[pTopicRatingUpsert];

GO

IF EXISTS (SELECT * 
           FROM [INFORMATION_SCHEMA].[ROUTINES] 
           WHERE [SPECIFIC_SCHEMA] = N'SQLRunbook'
           AND [SPECIFIC_NAME] = N'pTopicSelectAll' )
 DROP PROCEDURE [SQLRunbook].[pTopicSelectAll];

GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLRunbook'
           AND SPECIFIC_NAME = N'pTopicSelectByCategoryId' )
 DROP PROCEDURE [SQLRunbook].[pTopicSelectByCategoryId];

GO

IF EXISTS (SELECT * 
           FROM [INFORMATION_SCHEMA].[ROUTINES] 
           WHERE [SPECIFIC_SCHEMA] = N'SQLRunbook'
           AND [SPECIFIC_NAME] = N'pTopicSelectAllNames' )
 DROP PROCEDURE [SQLRunbook].[pTopicSelectAllNames];

GO

IF EXISTS (SELECT * 
           FROM [INFORMATION_SCHEMA].[ROUTINES] 
           WHERE [SPECIFIC_SCHEMA] = N'SQLRunbook'
           AND [SPECIFIC_NAME] = N'pTopicSelectByAllCriteria' )
 DROP PROCEDURE [SQLRunbook].[pTopicSelectByAllCriteria];

GO 

IF EXISTS (SELECT * 
           FROM [INFORMATION_SCHEMA].[ROUTINES] 
           WHERE [SPECIFIC_SCHEMA] = N'SQLRunbook'
           AND [SPECIFIC_NAME] = N'pTopicSelectByCategory' )
 DROP PROCEDURE [SQLRunbook].[pTopicSelectByCategory];

GO

IF EXISTS (SELECT * 
           FROM [INFORMATION_SCHEMA].[ROUTINES] 
           WHERE [SPECIFIC_SCHEMA] = N'SQLRunbook'
           AND [SPECIFIC_NAME] = N'pTopicSelectByName' )
 DROP PROCEDURE [SQLRunbook].[pTopicSelectByName];

GO

IF EXISTS (SELECT * 
           FROM [INFORMATION_SCHEMA].[ROUTINES] 
           WHERE [SPECIFIC_SCHEMA] = N'SQLRunbook'
           AND [SPECIFIC_NAME] = N'pTopicSelectByContains' )
 DROP PROCEDURE [SQLRunbook].[pTopicSelectByContains];

GO
 
IF EXISTS (SELECT * 
           FROM [INFORMATION_SCHEMA].[ROUTINES] 
           WHERE [SPECIFIC_SCHEMA] = N'SQLRunbook'
           AND [SPECIFIC_NAME] = N'pTopicSelectByCriteria' )
 DROP PROCEDURE [SQLRunbook].[pTopicSelectByCriteria];

GO

IF EXISTS (SELECT * 
           FROM [INFORMATION_SCHEMA].[ROUTINES] 
           WHERE [SPECIFIC_SCHEMA] = N'SQLRunbook'
           AND [SPECIFIC_NAME] = N'pTopicSelectByDate' )
 DROP PROCEDURE [SQLRunbook].[pTopicSelectByDate];

GO

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLRunbook'
           AND SPECIFIC_NAME = N'pTopicSelectByDateRange' )
 DROP PROCEDURE [SQLRunbook].[pTopicSelectByDateRange];

GO

IF EXISTS (SELECT * 
           FROM [INFORMATION_SCHEMA].[ROUTINES] 
           WHERE [SPECIFIC_SCHEMA] = N'SQLRunbook'
           AND [SPECIFIC_NAME] = N'pTopicSelectByDocumentId' )
 DROP PROCEDURE [SQLRunbook].[pTopicSelectByDocumentId];

GO

IF EXISTS (SELECT * 
           FROM [INFORMATION_SCHEMA].[ROUTINES] 
           WHERE [SPECIFIC_SCHEMA] = N'SQLRunbook'
           AND [SPECIFIC_NAME] = N'pTopicSelectByFreetext' )
 DROP PROCEDURE [SQLRunbook].[pTopicSelectByFreetext];

GO

IF EXISTS (SELECT * 
           FROM [INFORMATION_SCHEMA].[ROUTINES] 
           WHERE [SPECIFIC_SCHEMA] = N'SQLRunbook'
           AND [SPECIFIC_NAME] = N'pTopicSelectByOriginalLogin' )
 DROP PROCEDURE [SQLRunbook].[pTopicSelectByOriginalLogin];

GO

IF EXISTS (SELECT * 
           FROM [INFORMATION_SCHEMA].[ROUTINES] 
           WHERE [SPECIFIC_SCHEMA] = N'SQLRunbook'
           AND [SPECIFIC_NAME] = N'pTopicSelectByOwner' )
 DROP PROCEDURE [SQLRunbook].[pTopicSelectByOwner];

GO 

IF EXISTS (SELECT * 
           FROM INFORMATION_SCHEMA.ROUTINES 
           WHERE SPECIFIC_SCHEMA = N'SQLRunbook'
           AND SPECIFIC_NAME = N'pTopicSelectChangedByDateRange' )
 DROP PROCEDURE [SQLRunbook].[pTopicSelectChangedByDateRange];

GO

IF EXISTS (SELECT * 
           FROM [INFORMATION_SCHEMA].[ROUTINES] 
           WHERE [SPECIFIC_SCHEMA] = N'SQLRunbook'
           AND [SPECIFIC_NAME] = N'pTopicSelectNameList' )
 DROP PROCEDURE [SQLRunbook].[pTopicSelectNameList];

GO

IF EXISTS (SELECT * 
           FROM [INFORMATION_SCHEMA].[ROUTINES] 
           WHERE [SPECIFIC_SCHEMA] = N'SQLRunbook'
           AND [SPECIFIC_NAME] = N'pTopicSummary' )
 DROP PROCEDURE [SQLRunbook].[pTopicSummary];

GO

IF EXISTS (SELECT * 
           FROM [INFORMATION_SCHEMA].[ROUTINES] 
           WHERE [SPECIFIC_SCHEMA] = N'SQLRunbook'
           AND [SPECIFIC_NAME] = N'pTopicUpdate' )
 DROP PROCEDURE [SQLRunbook].[pTopicUpdate];

GO

IF EXISTS (SELECT * 
           FROM [INFORMATION_SCHEMA].[ROUTINES] 
           WHERE [SPECIFIC_SCHEMA] = N'SQLRunbook'
           AND [SPECIFIC_NAME] = N'pUserGetDetails' )
 DROP PROCEDURE [SQLRunbook].[pUserGetDetails];

GO

IF EXISTS (SELECT * 
           FROM [INFORMATION_SCHEMA].[ROUTINES] 
           WHERE [SPECIFIC_SCHEMA] = N'SQLRunbook'
           AND [SPECIFIC_NAME] = N'pUserSelect' )
 DROP PROCEDURE [SQLRunbook].[pUserSelect];

GO

IF EXISTS (SELECT * 
           FROM [INFORMATION_SCHEMA].[ROUTINES] 
           WHERE [SPECIFIC_SCHEMA] = N'SQLRunbook'
           AND [SPECIFIC_NAME] = N'pUserSelectAll' )
 DROP PROCEDURE [SQLRunbook].[pUserSelectAll];

GO

IF EXISTS (SELECT * 
           FROM [INFORMATION_SCHEMA].[ROUTINES] 
           WHERE [SPECIFIC_SCHEMA] = N'SQLRunbook'
           AND [SPECIFIC_NAME] = N'pUserSelectAllOriginalLogins' )
 DROP PROCEDURE [SQLRunbook].[pUserSelectAllOriginalLogins];

GO

IF EXISTS (SELECT * 
           FROM [INFORMATION_SCHEMA].[ROUTINES] 
           WHERE [SPECIFIC_SCHEMA] = N'SQLRunbook'
           AND [SPECIFIC_NAME] = N'pUserSelectAllWithContributorScoring' )
 DROP PROCEDURE [SQLRunbook].[pUserSelectAllWithContributorScoring];

GO

IF EXISTS (SELECT * 
           FROM [INFORMATION_SCHEMA].[ROUTINES] 
           WHERE [SPECIFIC_SCHEMA] = N'SQLRunbook'
           AND [SPECIFIC_NAME] = N'pUserSelectDetails' )
 DROP PROCEDURE [SQLRunbook].[pUserSelectDetails];

GO

IF EXISTS (SELECT * 
           FROM [INFORMATION_SCHEMA].[ROUTINES] 
           WHERE [SPECIFIC_SCHEMA] = N'SQLRunbook'
           AND [SPECIFIC_NAME] = N'pUserUpsert' )
 DROP PROCEDURE [SQLRunbook].[pUserUpsert];

GO

IF object_id('[SQLRunbook].[tUser]','U') IS NOT NULL
 DROP TABLE [SQLRunbook].[tUser];

GO

IF object_id('[SQLRunbook].[tTopicRating]','U') IS NOT NULL
 DROP TABLE [SQLRunbook].[tTopicRating];

GO

IF object_id('[SQLRunbook].[tDocumentRating]','U') IS NOT NULL
 DROP TABLE [SQLRunbook].[tDocumentRating];

GO

IF object_id('[SQLRunbook].[tCategoryRating]','U') IS NOT NULL
 DROP TABLE [SQLRunbook].[tCategoryRating];

GO

IF object_id('[SQLRunbook].[tTopicDocument]','U') IS NOT NULL
 DROP TABLE [SQLRunbook].[tTopicDocument];

GO

IF object_id('[SQLRunbook].[tCategoryTopic]','U') IS NOT NULL
 DROP TABLE [SQLRunbook].[tCategoryTopic];

GO

IF object_id('[SQLRunbook].[tCategory]','U') IS NOT NULL
 DROP TABLE [SQLRunbook].[tCategory];

GO

IF object_id('[SQLRunbook].[tRating]','U') IS NOT NULL
 DROP TABLE [SQLRunbook].[tRating];

GO

IF object_id('[SQLRunbook].[tTopic]','U') IS NOT NULL
 DROP TABLE [SQLRunbook].[tTopic];

GO

IF object_id('[SQLRunbook].[tDocument]','U') IS NOT NULL
 DROP TABLE [SQLRunbook].[tDocument];

GO

IF object_id('SQLRunbook.tSQLErrorLog','U') IS NOT NULL
 DROP TABLE [SQLRunbook].[tSQLErrorLog];

GO

IF object_id('SQLRunbook.tOption','U') IS NOT NULL
 DROP TABLE [SQLRunbook].[tOption];

GO

IF EXISTS (SELECT * FROM [INFORMATION_SCHEMA].[SCHEMATA]
           WHERE [SCHEMA_NAME] = 'SQLRunbook')
   DROP SCHEMA [SQLRunbook]          

GO

DECLARE @member [NVARCHAR] (128);
IF IS_MEMBER('SQLRunbookUserRole') IS NOT NULL
 BEGIN
  -- remove all members
  SET @member = (SELECT TOP 1 USER_NAME([member_principal_id]) 
                 FROM [sys].[database_role_members] 
                 WHERE USER_NAME([role_principal_id]) = 'SQLRunbookUserRole');     
  WHILE @member IS NOT NULL
   BEGIN
    EXEC sp_droprolemember 'SQLRunbookUserRole', @member;
    SET @member = (SELECT TOP 1 USER_NAME([member_principal_id]) 
                   FROM [sys].[database_role_members] 
                   WHERE USER_NAME([role_principal_id]) = 'SQLRunbookUserRole');     
   END
   DROP ROLE [SQLRunbookUserRole];
 END 

GO 

-- contributors before reporting users
DECLARE @member [NVARCHAR] (128);
IF IS_MEMBER('SQLRunbookContributorRole') IS NOT NULL
 BEGIN
  -- remove all members
  SET @member = (SELECT TOP 1 USER_NAME([member_principal_id]) 
                 FROM [sys].[database_role_members] 
                 WHERE USER_NAME([role_principal_id]) = 'SQLRunbookContributorRole');     
  WHILE @member IS NOT NULL
   BEGIN
    EXEC sp_droprolemember 'SQLRunbookContributorRole', @member;
    SET @member = (SELECT TOP 1 USER_NAME([member_principal_id]) 
                   FROM [sys].[database_role_members] 
                   WHERE USER_NAME([role_principal_id]) = 'SQLRunbookContributorRole');     
   END
   DROP ROLE SQLRunbookContributorRole;
 END 

GO 

DECLARE @member [NVARCHAR] (128);
IF IS_MEMBER('SQLRunbookServiceRole') IS NOT NULL
 BEGIN
  -- remove all members
  SET @member = (SELECT TOP 1 USER_NAME([member_principal_id]) 
                 FROM [sys].[database_role_members] 
                 WHERE USER_NAME([role_principal_id]) = 'SQLRunbookServiceRole');     
  WHILE @member IS NOT NULL
   BEGIN
    EXEC sp_droprolemember 'SQLRunbookServiceRole', @member;
    SET @member = (SELECT TOP 1 USER_NAME([member_principal_id]) 
                   FROM [sys].[database_role_members] 
                   WHERE USER_NAME([role_principal_id]) = 'SQLRunbookServiceRole');     
   END
   DROP ROLE [SQLRunbookServiceRole];
 END 

GO 

DECLARE @member [NVARCHAR] (128);
IF IS_MEMBER('SQLRunbookAdminRole') IS NOT NULL
 BEGIN
  -- remove all members
  SET @member = (SELECT TOP 1 USER_NAME([member_principal_id]) 
                 FROM [sys].[database_role_members] 
                 WHERE USER_NAME([role_principal_id]) = 'SQLRunbookAdminRole');
  WHILE @member IS NOT NULL
   BEGIN
    EXEC sp_droprolemember 'SQLRunbookAdminRole', @member;
    SET @member = (SELECT TOP 1 USER_NAME([member_principal_id]) 
                   FROM [sys].[database_role_members] 
                   WHERE USER_NAME([role_principal_id]) = 'SQLRunbookAdminRole');     
   END
   DROP ROLE [SQLRunbookAdminRole];
 END 

GO 

DECLARE @member [NVARCHAR] (128)
IF IS_MEMBER('SQLClueAdminRole') IS NOT NULL
AND NOT EXISTS (SELECT * FROM [sys].[database_role_members]
                WHERE USER_NAME([role_principal_id]) LIKE 'SQLCfg%'
                OR USER_NAME([role_principal_id]) LIKE 'QueryBaseline%')   
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

