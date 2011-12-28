/* IMPORTANT NOTE! BACKUP THE MASTER KEY AND the SERVICE MASTER KEY.

The SQL Clue Archive Planner UI does not use this script; provided for ad hoc use only.

Store the SERVICE MASTER KEY backup, the MASTER KEY backup and the MASTER KEY password 
in a secure location.  Note that the password used to create a backup is not the password 
used to create either key. The backup password(s) must also be securely stored and are 
required should it necessary to restore the key.  Give these details careful thought and 
thorough planning. Inability to produce the passwords or the key backup file when needed 
will render the encryption hierarchy unrecoverable.

The service master key is automatically generated the first time it is needed to encrypt 
a linked server password, credential, or database master key. The service master key is 
ncrypted using the local machine key or the Windows Data Protection API. This API uses a 
key that is derived from the Windows credentials of the SQL Server service account.

The service master key can only be decrypted by the service account under which it was 
created or by a principal that has access to the Windows credentials of that service 
account. Therefore, if you change the Windows account under which the SQL Server service 
runs, you must also enable decryption of the service master key by the new account.

The passwords to create the MASTER KEY and to backup these keys are subject to the 
password policy on Windows Vista, Windows 2003, and Windows 2008. For Windows XP and 
Windows 2000 the "Password Complexity" outlined in the "Password Policy" of SQL Server 
2005 Books Online is recommended. 

The encryption algorithm used to create the symmetric key for column encryption in this 
script will work with Windows XP and Windows 2000. Stronger algorithms such as "AES_256" 
should be used for Windows 2003 and later operating systems.

-- for a default installation, not specifying the path will place the backup in C:\MSSQL\Data
IF EXISTS (SELECT * FROM [sys].[symmetric_keys] WHERE [symmetric_key_id] = 101)
 BACKUP MASTER KEY TO FILE = 'SQLConfigurationRepository_MasterKey.BAK'
 ENCRYPTION BY PASSWORD = ‘<place password here >’

GO
-- for a default installation, not specifying the path will place the backup in C:\MSSQL\Data
BACKUP SERVICE MASTER KEY TO FILE = 'ServiceMasterKey.BAK' 
ENCRYPTION BY PASSWORD = ‘<place password here >’

*/

!!! specify the correct name of the repository database

--USE SQLConfigurationRepository
GO

!!! provide a password 

--If there is no master key, create one now. 
IF NOT EXISTS (SELECT * FROM [sys].[symmetric_keys] WHERE [symmetric_key_id] = 101)
 CREATE MASTER KEY ENCRYPTION 
BY PASSWORD = 'SQLClue';

GO
--always use the name ArchiveUsers for this certificate
IF NOT EXISTS (SELECT * FROM [sys].[certificates] WHERE [name] = 'ArchiveUsers')
 CREATE CERTIFICATE [ArchiveUsers] 
WITH SUBJECT = 'Encryption of SQL Login passwords for target SQL Servers';

GO

--always use the name PasswordKey for this key
IF NOT EXISTS (SELECT * FROM [sys].[symmetric_keys] WHERE [name] = 'PasswordKey')
 CREATE SYMMETRIC KEY [PasswordKey] 
WITH ALGORITHM = DES 
ENCRYPTION BY CERTIFICATE [ArchiveUsers];

GO


