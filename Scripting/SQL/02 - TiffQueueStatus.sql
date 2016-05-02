---***************************************************
---*** Shows ETA and Rate                          ***
---***************************************************
Declare @TimePeriod dec(5)


Set @TimePeriod = 15  --Looks at averages for last amount of time specified in whole minutes minutes



select 	count(*) as Remaining, 
	
	dateadd( n, cast( count(*) as real) / (
	select  Count(*) / @TimePeriod 
	from tblWorkQueueLog (nolock) Where 
	ProcessCd = 'tra'
	AND WorkTypeCd = 'WRK'
	AND StartTime between dateadd(n,-@TimePeriod,getdate()) and getdate()),getdate())
	 as [Completion Time], 
	
	(select  Count(*) * (60/@TimePeriod) 
	from tblWorkQueueLog (nolock) Where 
	ProcessCd = 'tra' 
	AND WorkTypeCd = 'WRK'
	AND StartTime between dateadd(n,-@TimePeriod,getdate()) and getdate()) 
	as [Files per hour]
from tblqueueTranslator (Nolock)
/*
---********************************************************
---*** Shows the File Types of what's Next on the Queue ***
---********************************************************
SELECT Top 5000 ft.Description
FROM tblQueueTranslator qh (Nolock)
JOIN tblItem i on i.ItemIdentityID = qh.ItemIdentityID
JOIN tlkpFileType ft on ft.FiosFileTypeID = i.FiosFileTypeID
WHERE qh.ProcessingFlag = 0


---********************************************************
---*** Shows the File Types of what's on the Queue      ***
---********************************************************
SELECT ft.Description, Count(*)
FROM tblQueueTranslator qh (Nolock)
JOIN tblItem i on i.ItemIdentityID = qh.ItemIdentityID
JOIN tlkpFileType ft on ft.FiosFileTypeID = i.FiosFileTypeID
WHERE qh.ProcessingFlag = 0
Group by ft.Description
Order by ft.Description
*/