-- test setup for checking either compare or archive
-- for archive, run in sections and run an archive between each section
/*
use master
if db_id('ClueTest1') is not null
 drop database ClueTest1
if db_id('ClueTest2') is not null
 drop database ClueTest2
*/

create database ClueTest1
go
alter database ClueTest1
SET AUTO_CLOSE OFF
go

alter database ClueTest1
SET TRUSTWORTHY ON
go

create database ClueTest2
go
alter database ClueTest2
SET AUTO_CLOSE OFF
go

use ClueTest1
go
create table t1 (c1 int primary key) 
go
create table t2(c1 int, c2 int)
go
Create proc p1 as select * from t1

go

use ClueTest2
go
create table [dbo].[t1] ([c1] [int] not null) 
go
create table [dbo].[t2] ([c1] [int] not null, [c2] [int] not null) 
go
alter table [dbo].[t1]
ADD CONSTRAINT pk_t1 primary key ([c1])
go
alter table [dbo].[t2]
ADD CONSTRAINT pk_t2 primary key ([c1], [c2])
Go
alter table [dbo].[t2]
ADD CONSTRAINT fk_t2__to__t1__c1 foreign key ([c1])
references [dbo].[t1]([c1])
go

Create proc [dbo].[p1] 
as 
select [c1] from [dbo].[t1]
where [c1] between 3 and 5 
go