test script
/* reports
EXEC [dbo].[pSQLCfgChangeActionSummaryByNodeForDays] 
EXEC [dbo].[pSQLCfgChangesForDate] 
EXEC [dbo].[pSQLCfgChangeHistoryByItem] 'SQLCfgMetadata|dbo.tSQLCfg'
EXEC [dbo].[pSQLCfgSQLErrorLogSelectMostRecent] 
*/

/* raw data 
select * from [dbo].[tSQLCfg]
select * from [dbo].[tSQLCfgConnection]
select * from [dbo].[tSQLCfgInstance]
Select * from [dbo].[tSQLCfgJobServer]
Select * from [dbo].[tSQLCfgDb]
Select * from [dbo].[tSQLCfgServiceBroker]
Select * from [dbo].[tSQLCfgChange]
select Node.ToString() AS [Node], Version, Node.Type AS [Node Type], Node.Collection AS [Node Collection], Node.Item as [Node Item], Action from [dbo].[tSQLCfgChange] order by id
select * from [dbo].[tSQLCfgSchedule]
select * from [dbo].[tSQLCfgLabel]
select * from [dbo].[tSQLCfgChangeLabel]
select * from [dbo].[tSQLCfgSQLErrorLog]
select * from sys.objects where type <>'S'
*/
/*
--EXEC [dbo].[pSQLCfgChangeApplyLabel] 1,1,'test','just a test',0, 'dbo'
-GO
--exec [dbo].[pSQLCfgDbDelete] 'LiteSpeedLocal', @@SERVERNAME
-GO
--EXEC [dbo].[pSQLCfgChangeApplyLabel] 1,1,'another test','just another test',0, 'dbo'
*/

/*
SELECT * FROM sys.certificates
SELECT * FROM sys.symmetric_keys

DECLARE @Password [NVARCHAR] (128) 
EXEC [dbo].[pSQLCfgSchedulePasswordGet] 1, @password OUTPUT 
SELECT @Password

exec dbo.pSQLCfgSchedulePasswordUpdate @Id=1,@Password=N'TestIng!23'
exec dbo.pSQLCfgSchedulePasswordUpdate @Id=1,@Password=NULL
*/

/* test data
EXEC [dbo].[pSQLCfgConnectionInsert] 'Fred',0,0,'',5,1
DECLARE @id int
--EXEC [dbo].[pSQLCfgScheduleInsert] 'BILL64',1,'day','2007-05-17 08:00:33.167',1, 1, @id OUTPUT
EXEC [dbo].[pSQLCfgScheduleInsert] 'Fred',1,'day','2007-05-17 08:00:33.167',1, 1, @id OUTPUT

EXEC [dbo].[pSQLCfgInstanceInit] 'Fred', 8 , 2, 'dbo'
EXEC [dbo].[pSQLCfgDBInit] 'Fred', 'Wilma', 8, 'dbo'
EXEC [dbo].[pSQLCfgDBInit] 'Fred', 'master', 8, 'dbo'
EXEC [dbo].[pSQLCfgDBInit] 'Fred', 'model', 8, 'dbo'
EXEC [dbo].[pSQLCfgDBInit] 'Fred', 'msdb', 8, 'dbo'

EXEC [dbo].[pSQLCfgConnectionInsert] 'Barney', 0, 0, '', 8, 1
EXEC [dbo].[pSQLCfgScheduleInsert] 'Barney',1,'day','2007-05-17 08:00:33.167',1, 1, @id OUTPUT

EXEC [dbo].[pSQLCfgInstanceInit] 'Barney', 9 , 4, 'dbo'
EXEC [dbo].[pSQLCfgDBInit] 'Barney', 'Betty', 8, 'dbo'
EXEC [dbo].[pSQLCfgDBInit] 'Barney', 'Wilma', 9, 'dbo'
EXEC [dbo].[pSQLCfgDBInit] 'Barney', 'master', 9, 'dbo'
EXEC [dbo].[pSQLCfgDBInit] 'Barney', 'model', 9, 'dbo'
EXEC [dbo].[pSQLCfgDBInit] 'Barney', 'msdb', 9, 'dbo'

*/

/* Notifications
select object_name(queue_id), * from sys.dm_broker_queue_monitors where database_id = db_id('msdb') and object_name(queue_id) like '%SQLClue%'  
select * from sys.server_event_notifications
select * from msdb.sys.event_notifications
Select * from msdb.sys.services where name like '%SQLClue%'
select * from msdb.sys.service_queues where name like '%SQLClue%'
select * from sys.endpoints  where name like '%SQLClue%'
select * from sys.routes  where name like '%SQLClue%'
select cast(message_body as XML), * from SQLClueDDLEventsQueue
select * from SQLClueMonitorQueue
begin tran
exec msdb.dbo.GetSQLClueDDLEvent
commit tran
*/

/*add data to repository that has never been on server
enable trigger trgSQLCfgChange_Insert_Update_Delete on tsqlcfgChange
update tsqlcfgChange
set Definition = Definition + Char(13) + Char(10) + 'repository' 
where node.Type = 'SQLInstance' 
and node.SQLInstance = 'BILL_VU\SQLEXPRESS'
and node.Collection = 'Credentials'
and node.Item = 'FullTextService'

select node.ToString(), Action, Definition from tsqlcfgChange
where node.Type = 'SQLInstance' 
and node.SQLInstance = 'BILL_VU\SQLEXPRESS'
and node.Collection = 'Credentials'
and node.DbName = 'ClueTest1'
and node.Item = 'dbo.p1'

declare @a int, @b int
exec psqlcfgChangeInsert 'BILL_VU\SQLEXPRESS|Credentials|test', 'Add', NULL, 'testing for collection in repos not on server',@a, @b 
select @a, @b
*/

/* fulltextable doctypes

declare @d as varchar(128), @s as varchar(max)
set @d = (select Min(document_type) 
          from sys.fulltext_document_types)
set @s = ''
While @d is not null
 begin
  if @s = '' 
   set @s = @d
  else
   set @s = @s + ', ' + @d
  set @d = (select Min(document_type) 
            from sys.fulltext_document_types 
            where document_type > @d)
 end
select @s
*/