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
**  JAN 27,2010	   bw                correct rating count  
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
/******************************************************************************
**	Auth: Bill Wunder
**	Date: April 1, 2007
**
**	Desc: all document ratings for documents reviewed by a user
**
*******************************************************************************
**	Change History
*******************************************************************************
**	Date		   Author		     Description of Change
**	Jan 29, 2010   bw                change SARG to last user from orig user          
**	          
*******************************************************************************/
ALTER PROCEDURE [SQLRunbook].[pDocumentRatingSelectByReviewer]
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
 WHERE d.[LastUpdatedUser] = @OriginalLogin;

END;

GO

