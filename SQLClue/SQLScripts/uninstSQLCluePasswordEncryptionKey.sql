
IF EXISTS (SELECT * FROM sys.symmetric_keys WHERE name = 'PasswordKey')
 DROP SYMMETRIC KEY PasswordKey;

GO

IF EXISTS (SELECT * from sys.certificates where name = 'ArchiveUsers')
 DROP CERTIFICATE ArchiveUsers;

GO

-- Database master key is not dropped