--	This script resolves common processing errors for: 
--		Inventory, Signature, Unzip, and OfficeMetadataParser
--	Just run this whole script as is, all you need to set are the MediaIDs
declare @LowMediaID int
declare @HighMediaID int
--********************************************************************************************
------------------------------- UPDATE MediaIDs HERE!!!---------------------------------------
set @LowMediaID =  8000033
set @HighMediaID = 8000033
----------------------------------------------------------------------------------------------
--********************************************************************************************
--*************************Do not type below this line**************************************--
----------------------------------------------------------------------------------------------
---------------------------Section 1. MetaData Parser Errors----------------------------------
--********************************************************************************************
--This updates MetaData parser errors
--There will be lots of these. They are common for various reasons.
SET NOCOUNT ON

update ie
set resolution = 'Office Metadata extractor error. No further action needed.'
	, ErrorStatusCd = 'RES'
	, DateUpdated = GetDate()
from 	tblItemError ie
join 	tblItem i on i.ItemIdentityID = ie.ItemIdentityID
where 	ie.FunctionName = 'cOfficeMetaData'
	and ie.ErrorStatusCD = 'UNR'
	and i.MediaID between @LowMediaID and @HighMediaID
Print 'Resolved ' + Cast(@@rowcount as varchar) + ' Office Metadata Parser Errors.'


-- ---------------------------Section 2. Common File/Folder Inventory Errors---------------------
-- --********************************************************************************************
-- --This resolves errors copying and hashing temp internet files.
-- Update	ie
-- Set	Resolution = 'Temporary Internet Files are not processed by Fios Software.'
-- 	, ErrorStatusCD = 'FAT'
-- from 	tblItemError ie
-- join 	tblItem i on i.ItemIdentityID = ie.ItemIdentityID
-- Where	ProcessingQueueCD IN ('INV', 'SIG')
-- 	and ErrorStatusCD = 'UNR'
-- 	and MediaID between @LowMediaID and @HighMediaID
-- 	and len(i.ItemName) > 50
-- 	and (i.Extension = '' 
-- 		or i.ItemName like '%;%' 
-- 		or i.ItemName like '%&%'
-- 		or i.ItemName like '%=%')
-- Print 'Resolved ' + Cast(@@rowcount as varchar) + ' INV errors caused by temporary Internet Files.'
-- 
-- --sets the FiosFileTypeID for temp internet files, if they lack one.
-- Update i
-- set FiosFileTypeID = 373
-- from tblItem i 
-- where 	i.FiosFileTypeID IS NULL
-- 	and i.MediaID between @LowMediaID and @HighMediaID
-- 	and i.ItemIdentityID IN (select ItemIdentityID from tblItemError
-- 				where ProcessingQueueCd = 'SIG' 
-- 				and Resolution = 'Temporary Internet Files are not processed by Fios Software.')
-- 		

---------------------------Section 3. Common Unzip Errors-------------------------------------
--********************************************************************************************
--This resolves Unzip errors for Java .JAR files, which we cannot unzip.
update 	ie
Set	Resolution = '.JAR Files are compressed java.  Item Bypassed.'
	, ErrorStatusCD = 'FAT'
from 	tblItemError ie
join	tblItem i on i.ItemIdentityID = ie.ItemIdentityID
Where	ErrorStatusCD = 'UNR'
	and ie.ProcessingQueueCD = 'ZIP'
	and i.extension = 'jar'
	and i.MediaID between @LowMediaID and @HighMediaID
Print 'Resolved ' + Cast(@@rowcount as varchar) + ' UNZIP Errors for Java .jar files.'

--These are file extensions for stuff we cannot usually unzip
update 	ie
Set	Resolution = 'This type of compression file is not currently handled by Fios Software.  Item Bypassed.'
	, ErrorStatusCD = 'FAT'
from 	tblItemError ie
join	tblItem i on i.ItemIdentityID = ie.ItemIdentityID
Where	ErrorStatusCD = 'UNR'
	and ProcessingQueueCD = 'ZIP'
	and i.MediaID between @LowMediaID and @HighMediaID
	and i.extension IN ('cfg','cob','XPI','HDI','WZ','SAZ','4hd','SPL','bin','wmz','tmp','sac','xa','tar','411','dll','diz','db')
Print 'Resolved ' + Cast(@@rowcount as varchar) + ' UNZIP errors for unprocessable zips based on extension.'

--More compressed files that can't be processed
update 	ie
Set	Resolution = 'File cannot be decoded.  Item Bypassed.'
	, ErrorStatusCD = 'FAT'
from 	tblItemError ie
join	tblItem i on i.ItemIdentityID = ie.ItemIdentityID
Where	ErrorStatusCD = 'UNR'
	and ProcessingQueueCD = 'ZIP'
	and ErrorDescription Like 'File access denied%'
	and i.MediaID between @LowMediaID and @HighMediaID
	and (i.Extension is NULL or i.Extension = '')
Print 'Resolved ' + Cast(@@rowcount as varchar) + ' UNZIP errors for zips that could not be accessed.'
	
--Specific FiosFileTypeIDs that we cannot unzip
update 	ie
Set	Resolution = 'This type of compression file is not currently handled by Fios Software.  Item Bypassed.'
	, ErrorStatusCD = 'FAT'
from 	tblItemError ie
join	tblItem i on i.ItemIdentityID = ie.ItemIdentityID
Where	ErrorStatusCD = 'UNR'
	and ie.ProcessingQueueCD = 'ZIP'
	and i.MediaID between @LowMediaID and @HighMediaID
	and i.FiosFileTypeID NOT BETWEEN 944 and 960
Print 'Resolved ' + Cast(@@rowcount as varchar) + ' UNZIP errors for unprocessable zips based on FiosFileTypeID.'


--Password protected or corrupt zips

update 	ie
Set	Resolution = 'This may be a password-protected compressed file.  Item Bypassed.'
	, ErrorStatusCD = 'FAT'
from 	tblItemError ie
join	tblItem i on i.ItemIdentityID = ie.ItemIdentityID
Where	ErrorStatusCD = 'UNR'
	and ie.ProcessingQueueCD = 'ZIP'
	and i.MediaID between @LowMediaID and @HighMediaID
	and ErrorDescription like 'Unable to complete request%'

Print 'Resolved ' + Cast(@@rowcount as varchar) + ' UNZIP errors for password protected or corrupt zips.'


--Resolves common empty pst error.

update 	ie
Set	Resolution = 'This PST is empty.'
	, ErrorStatusCD = 'FAT'
from 	tblItemError ie
join	tblItem i on i.ItemIdentityID = ie.ItemIdentityID
Where	ErrorStatusCD = 'UNR'
	and ie.ProcessingQueueCD = 'EMA'
	and i.MediaID between @LowMediaID and @HighMediaID
	and ErrorDescription like 'Invalid property value | Unable to complete setup'

Print 'Resolved ' + Cast(@@rowcount as varchar) + ' Empty PST Errors.'

--These next ones resolve common errors for invalid msgs.

update 	ie
Set	Resolution = 'Not a Valid MSG.'
	, ErrorStatusCD = 'FAT'
from 	tblItemError ie
join	tblItem i on i.ItemIdentityID = ie.ItemIdentityID
Where	ErrorStatusCD = 'UNR'
	and ie.ProcessingQueueCD = 'EMA'
	and i.MediaID between @LowMediaID and @HighMediaID
	and ErrorDescription like ' [Collaboration Data Objects - [MAPI_E_NOT_FOUND(8004010F)]]'

Print 'Resolved ' + Cast(@@rowcount as varchar) + ' Invalid MSGs with Mapi.'

update 	ie
Set	Resolution = 'Not a Valid MSG.'
	, ErrorStatusCD = 'FAT'
from 	tblItemError ie
join	tblItem i on i.ItemIdentityID = ie.ItemIdentityID
Where	ErrorStatusCD = 'UNR'
	and ie.ProcessingQueueCD = 'EMA'
	and i.MediaID between @LowMediaID and @HighMediaID
	and ErrorDescription like ' [Collaboration Data Objects - [E_FAIL(80004005)]]'

Print 'Resolved ' + Cast(@@rowcount as varchar) + ' Invalid MSGs with E_Fail.'

update 	ie
Set	Resolution = 'Not a Valid MSG.'
	, ErrorStatusCD = 'FAT'
from 	tblItemError ie
join	tblItem i on i.ItemIdentityID = ie.ItemIdentityID
Where	ErrorStatusCD = 'UNR'
	and ie.ProcessingQueueCD = 'EMA'
	and i.MediaID between @LowMediaID and @HighMediaID
	and ErrorDescription like 'Item is not an Outlook.MailItem'

Print 'Resolved ' + Cast(@@rowcount as varchar) + ' Non Outlook Items.'


update 	ie
Set	Resolution = 'Not a Valid MSG.'
	, ErrorStatusCD = 'FAT'
from 	tblItemError ie
join	tblItem i on i.ItemIdentityID = ie.ItemIdentityID
Where	ErrorStatusCD = 'UNR'
	and ie.ProcessingQueueCD = 'EMA'
	and i.MediaID between @LowMediaID and @HighMediaID
	and ErrorDescription like 'Item was not openable by Outlook (ActiveInspector Is Nothing)'

Print 'Resolved ' + Cast(@@rowcount as varchar) + ' Unopenable Outlook Items.'

--Sometimes the email app throws this error.  It is bogus.
update 	ie
Set	Resolution = 'Bogus Error.'
	, ErrorStatusCD = 'RES'
from 	tblItemError ie
join	tblItem i on i.ItemIdentityID = ie.ItemIdentityID
Where	ErrorStatusCD = 'UNR'
	and ie.ProcessingQueueCD = 'EMA'
	and i.MediaID between @LowMediaID and @HighMediaID
	and ErrorDescription like 'Cannot delete local pst file'

Print 'Resolved ' + Cast(@@rowcount as varchar) + ' Bogus pst error'

--Correctly signatures Project Files, that we're identified as Biff files
update	tblItem
set	FiosFileTypeID = 166
where	Extension = 'mpp'
	and FiosFileTypeID not in (166, 721)
	and MediaID between @LowMediaID and @HighMediaID

Print 'Identified & Updated ' + Cast(@@rowcount as varchar) + ' as Incorrectly Signatured Project Files'


--Resolves Recall Messages
update ie 
set	resolution = 'Recall Messages Not Supported'
	,errorstatuscd = 'FAT'
from	tblitemerror ie
join	tblItem i on i.ItemIdentityID = ie.ItemIdentityID
where	ie.ErrorStatusCd = 'UNR'
	and errordescription LIKE 'Recall message encountered%'
	and i.MediaID between @LowMediaID and @HighMediaID

Print 'Resolved ' + Cast(@@rowcount as varchar) + ' Recalled Messages' 

--Resolves incorrectly formed msg
update ie
set 	Resolution = 'This is not a correctly formed MSG file'
	, ErrorStatusCd = 'RES'
from 	tblItemError ie
join	tblItem i on i.ItemIdentityID = ie.ItemIdentityID
Where	ErrorStatusCD = 'UNR'
	and i.MediaID between @LowMediaID and @HighMediaID
	and ie.ProcessingQueueCD = 'EMA'
	and i.extension = 'MSG'
	and i.FiosFIleTypeID <> 962
	and ie.ErrorDescription like '%The file may not exist, you may not have permission to open it, or it may be open in another program. Right-click the folder that contains the file, and then click Properties to check your permissions for the folder. | Item is not openable by Outlook'

Print 'Resolved ' + Cast(@@rowcount as varchar) + ' EMA errors for MSGs.'


--resolves errors for Emails with OLE objects that cannot create RTF files
update ie
set 	Resolution = 'Could not create RTF for email with embedded OLE objects'
	, ErrorStatusCd = 'FAT'
from 	tblItemError ie
join	tblItem i on i.ItemIdentityID = ie.ItemIdentityID
Where	ErrorStatusCD = 'UNR'
	and i.MediaID between @LowMediaID and @HighMediaID
	and ie.ProcessingQueueCD = 'EMA'
	and i.FiosFileTypeID = 962
	and ie.FunctionName = 'modPSTProcessing::SaveAsRTF'

Print 'Resolved ' + Cast(@@rowcount as varchar) + ' EMA errors for RTF files for emails with OLE objects.'


--resolves errors for PSTs that are too small to actually have email in them
update ie
set 	Resolution = 'File size < 32,768 Bytes indicates this file is not a correctly formed PST File.'
	, ErrorStatusCd = 'RES'
from 	tblItemError ie
join	tblItem i on i.ItemIdentityID = ie.ItemIdentityID
Where	ErrorStatusCD = 'UNR'
	and i.MediaID between @LowMediaID and @HighMediaID
	and ie.ProcessingQueueCD = 'EMA'
	and i.extension = 'pst'
	and i.ByteCnt < 32768
Print 'Resolved ' + Cast(@@rowcount as varchar) + ' EMA errors for PSTs < 32,768 bytes.'

--PSTs that errored but have children, so it's OK.
update 	ie
set 	Resolution = 'This PST errored out but it DOES have children.'
	, ErrorStatusCd = 'RES'
from 	tblItemError ie
join	tblItem i on i.ItemIdentityID = ie.ItemIdentityID
Where	ErrorStatusCD = 'UNR'
	and ie.ProcessingQueueCD = 'EMA'
	and i.MediaID between @LowMediaID and @HighMediaID
	and i.extension = 'pst'
	and dbo.fnGetChildCount(ie.ItemIdentityID) > 0
	and (ie.ErrorDescription like '%Unable to Attach PST file%' 
		or 
	     ie.ErrorDescription like '%Outlook failed to add the personal store to this session%'
		or
	     ie.ErrorDescription like '%Unable to get stats for attached PST file%'
		or 
	     ie.ErrorDescription like ' | Unable to copy PST file to local disk%')
Print 'Resolved ' + Cast(@@rowcount as varchar) + ' EMA errors for PSTs that sucessfully had children.'


update ie
set 	Resolution = 'Cannot save the RTF.'
	, ErrorStatusCd = 'INF'
from 	tblItemError ie
join	tblItem i on i.ItemIdentityID = ie.ItemIdentityID
Where	ErrorStatusCD = 'UNR'
	and i.MediaID between @LowMediaID and @HighMediaID
	and ie.ProcessingQueueCD = 'EMA'
	AND ie.errordescription = 'Unable to create RTF file for message'
Print 'Resolved ' + Cast(@@rowcount as varchar) + ' RTF Errors.'



update ie
set 	Resolution = 'For Custom Properties, not needed.'
	, ErrorStatusCd = 'INF'
from 	tblItemError ie
join	tblItem i on i.ItemIdentityID = ie.ItemIdentityID
Where	ErrorStatusCD = 'UNR'
	and i.MediaID between @LowMediaID and @HighMediaID
	and errordescription like 'Method% of object% failed'
	and  functionname = 'AddCustomProperties'
Print 'Resolved ' + Cast(@@rowcount as varchar) + ' Custom Property Errors.'


update ie
set 	Resolution = 'Cannot Save Attachment'
	, ErrorStatusCd = 'INF'
from 	tblItemError ie
join	tblItem i on i.ItemIdentityID = ie.ItemIdentityID
Where	ErrorStatusCD = 'UNR'
	and i.MediaID between @LowMediaID and @HighMediaID
	and errordescription like 'Cannot save the attachment. The operation failed. An object could not be found.'
Print 'Resolved ' + Cast(@@rowcount as varchar) + ' attachment errors'




--update 'pagefile.sys' to be an OS file type
update i
set FiosFileTypeID = 723
from tblItem i (nolock)
where i.MediaID between @LowMediaID and @HighMediaID
and ItemName = 'pagefile.sys'
Print 'Set ' + Cast(@@rowcount as varchar) + ' pagefile.sys files to FiosFileTypeID 723 for Windows OS swap files.'
