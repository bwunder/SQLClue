I developed and maintain reports in a Visual Studio 2008 project AS '.rdl' files. 

The '.rdl' files are then copied to the ReportViewerReports folder and renamed with 
the ReportViewer Client '.rdlc' extension. 'They' seem to want to keep the control
one version behind Report Server? 


Using the Reports on a Report Server

Once touched by Visual Studio 2010 as '.rdl'files, the report can no longer be so 
easily converted to '.rdlc' for use by the latest available ReportViewer control. 
However there is a report template in Visual Studio 2010 with Juneau to create 
'.rdlc' in the correct format. Look for "2008/01" in the default namespace of the 
report's source file. If you see 2010 the report cannot be renamed for ReportViewer 
use.

To convert the reports for use in a Reporting Services (RS) instance or from within 
the Business Intelligence Developer's Studio 2008 (BIDS) UI, copy the '.rdlc' files 
to a new location and change the extension of each copied file to '.rdl'.

Visual Studio 2010 upgrades the '.rdl' to an un-usable format for the most recent 
Visual Studio ReportViewer control (2008). I have not tried but a reversal of the 
process where '.rdlc' files are designed and then the files copied and renamed to 
'.rdl' may work best in VS2010. Developing reports as '.rdlc' is usually a far less 
friendly development setting. Far easier to use Visual Studio 2008 to develop the 
'.rdl' than having to rely upon application created record sets...  

Note that much functionality available when reports are viewed through the 
ReportViewer 2008 control that should be embedded in the SQL Clue Administrator's 
Console does not work when the reports are used through VS, BIDS or Reporting Services 
as '.rdl'. On the other hand, several reports that do not expose parameter selection 
within the SQL Clue Administrator's Console expose the report parameter selection 
controls as '.rdl' files. There are advantages to both usage scenarios - depending
 upon the need. 


Data Sources 

When moving between '.rdlc' and VS2008 '.rdl' it is helpful to use the same shared 
datasouce names as already contained in the file.
 
The DefaultTrace accepts @@SERVERNAME as a Report Parameter for report the header 
only and uses an embedded datasource that connects to a hard-coded SQL Instance in 
that datasource. Note that the CROSS APPLY to fetch the trace file name fails for SQL 
Server 2005 targets. A TraceFile Report Parameter can be added or all DataSet queries 
made dynamic and the trace file name fetched to build the query string before 
fn_gettracetable executes.  

For all SQL Configuration Archive reports, create a Shared Datasource named 
'SQLConfiguration.rds' that points to the SQLClue SQL Configuration Archive database.
 
For all Data Center Runbook reports, create a shared datasource named 'SQLRunbook.rds' 
that points to the SQLClue Data Center Runbook database.
 
No other changes should be required to convert the reports to '.rdl' files and use 
them from SQL Server 2008 VS/BIDS UI or deployed to a Report Server 2008.


