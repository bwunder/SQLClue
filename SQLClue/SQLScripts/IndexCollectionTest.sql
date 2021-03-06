/*
USE [SQLClue]
ALTER TABLE [SQLCfg].[tChange] 
REBUILD PARTITION = ALL
WITH (DATA_COMPRESSION = Page)
*/

/*
Index UDT expressions. You can create indexes on persisted computed columns over UDT expressions. 
The UDT expression can be a field, method, or property of a UDT. The expression must be deterministic 
and must not perform data access.
*/
  DROP TABLE [SQLCfg].[IndexTest1] 

  CREATE TABLE [SQLCfg].[IndexTest1] (
     [Id] [INT] IDENTITY (1, 1) NOT NULL PRIMARY KEY 
   , [Node]  [SQLCfg].[SQLCfgNode] NOT NULL 
   , [Version] [INT]
   , [Collection] AS [SQLCfg].[GetCollection]([Node]) PERSISTED)

insert [SQLCfg].[IndexTest1] (Node, Version)
SELECT Node, version from [SQLCfg].[tChange]

select * from [SQLCfg].[IndexTest1] 
where Collection='EndPoints' 

CREATE INDEX ixn_IndexTest1_Collection
ON [SQLCfg].[IndexTest1]([Collection]) INCLUDE ([Id], [Node], [Version])

select * from [SQLCfg].[IndexTest1] 
where Collection='StoredProcedures' 
