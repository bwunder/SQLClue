Alter table SQLCfg.tChange Disable trigger all
delete SQLCfg.tChange
where Node.SQLInstance = 'BILL_VU\DEV'
Alter table SQLCfg.tChange enable trigger all

Alter table SQLCfg.tServiceBroker Disable trigger all
delete SQLCfg.tServiceBroker
where InstanceName = 'BILL_VU\DEV'
Alter table SQLCfg.tServiceBroker enable trigger all

Alter table SQLCfg.tDB Disable trigger all
delete SQLCfg.tDB 
where InstanceName = 'BILL_VU\DEV'
Alter table SQLCfg.tDB enable trigger all

Alter table SQLCfg.tJobServer Disable trigger all
delete SQLCfg.tJobServer 
where InstanceName = 'BILL_VU\DEV'
Alter table SQLCfg.tJobServer enable trigger all

Alter table SQLCfg.tInstance Disable trigger all
delete SQLCfg.tInstance 
where Name = 'BILL_VU\DEV'
Alter table SQLCfg.tInstance enable trigger all

Alter table SQLCfg.tSchedule Disable trigger all
delete SQLCfg.tSchedule 
where InstanceName = 'BILL_VU\DEV'
Alter table SQLCfg.tSchedule enable trigger all

Alter table SQLCfg.tconnection Disable trigger all
delete SQLCfg.tconnection 
where InstanceName = 'BILL_VU\DEV'
Alter table SQLCfg.tconnection enable trigger all


