--exec SQLCfg.pChangeInsert @Node=N'BILL_VU\EEEVAL|Logins|BILL_VU\bill',@Action=N'Add',@EventData=NULL,@Definition=N'test1',@ChangeId=0,@Version=0 
--exec SQLCfg.pChangeInsert @Node=N'BILL_VU\EEEVAL|JobServer|AlertSystem',@Action=N'Add',@EventData=NULL,@Definition=N'test2',@ChangeId=0,@Version=0
--exec SQLCfg.pChangeInsert @Node=N'BILL_VU\EEEVAL|Databases|SQLClue3.5.1|Tables',@Action=N'Add',@EventData=NULL,@Definition=N'test3',@ChangeId=0,@Version=0
--exec SQLCfg.pChangeInsert @Node=N'BILL_VU\EEEVAL|Databases|SQLClue3.5.1|ServiceBroker|Queues',@Action=N'Add',@EventData=NULL,@Definition=N'test4',@ChangeId=0,@Version=0


DECLARE @node SQLCfg.SQLCfgNode
--SET @n = N'BILL_VU\EEEVAL|Logins|BILL_VU\bill'
SET @node = N'BILL_VU\EEEVAL|Databases|SQLClue3.5.1|Tables|SQLCfg.SQLCfg'
SELECT @node.ToString() AS [n.ToString()]
SELECT @node.[Length] AS [n.Length]
SELECT @node.[Type] AS [n.Type]
SELECT @node.[SubType] AS [n.SubType]
SELECT @node.[Database] AS [n.Database]
SELECT @node.[Collection] AS [n.Collection]
SELECT @node.[Item] AS [n.Item]