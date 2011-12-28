 DECLARE @BeginDate [DATETIME]
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
 , @Contains [NVARCHAR] (4000)

SELECT @BeginDate = GETDATE() - 10
 , @EndDate = GETDATE() 
 , @TopicId = 0
 , @DocumentId = 0 
 , @CategoryId = 0
 , @OriginalLogin = ''
 , @SearchCategories = 1
 , @SearchTopics = 1 
 , @SearchDocuments = 1
 , @SearchRatings = 1
 , @RatingOperator = '>'
 , @RatingValue = -1
 , @Contains = 'SQLClue'
 
exec [SQLRunbook].[pTopicSelectByCriteria] 
   @BeginDate = @BeginDate
 , @EndDate = @EndDate 
 , @TopicId = @TopicId
 , @DocumentId = @DocumentId 
 , @CategoryId = @CategoryId
 , @OriginalLogin = @OriginalLogin
 , @SearchCategories = @SearchCategories
 , @SearchTopics = @SearchTopics 
 , @SearchDocuments = @SearchDocuments
 , @SearchRatings = @SearchRatings
 , @RatingOperator = @RatingOperator
 , @RatingValue = @RatingValue
 , @Contains = @Contains
 
exec [SQLRunbook].[pTopicSelectByAllCriteria] 
   @BeginDate = @BeginDate
 , @EndDate = @EndDate 
 , @TopicId = @TopicId
 , @DocumentId = @DocumentId 
 , @CategoryId = @CategoryId
 , @OriginalLogin = @OriginalLogin
 , @SearchCategories = @SearchCategories
 , @SearchTopics = @SearchTopics 
 , @SearchDocuments = @SearchDocuments
 , @SearchRatings = @SearchRatings
 , @RatingOperator = @RatingOperator
 , @RatingValue = @RatingValue
 , @Contains = @Contains



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
