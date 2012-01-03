Reports are developed and maintained as v2 '.rdl' files. The 
'.rdl' files are then copied to this deployment folder and 
renamed with the ReportViewer Client '.rdlc' extension.

To convert the reports for use in a Reporting Services (RS) 
instance or from within the Business Intelligence Developer's 
Studio (BIDS) UI, copy the '.rdlc' files to a new location 
and change the extension of each copied file to '.rdl'.

For SQL Configuration Archive reports, create a Shared 
Datasource named 'SQLConfiguration.rds' that points to the 
SQLClue SQL Configuration Archive database.
 
For Data Center Runbook reports, create a shared datasource 
named 'SQLRunbook.rds' that points to the SQLClue Data Center 
Runbook database.
 
No other changes are required to convert the reports to '.rdl'
files and use them from BIDS UI or deployed to an RS instance.

Note that some functionality available when reports are 
viewed through the ReportViewer control embedded in the SQL 
Clue Administrator's Console does not work when the reports are 
used through BIDS or Reporting Services. On the other hand,
several reports that do not expose parameter selection within
the SQL Clue Administrator's Console expose the report 
parameter selection controls as '.rdl' files. There are 
advantages to both usage scenarios - depending upon the need. 

