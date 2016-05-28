/*********************************************************************************************************
*  QC_MissingHTML
* 
*  Purpose: 
*	Count the number of items with no HTML associated with them.
* 	
*  Inputs:
*	CaseID (int) - the case number in question
*	pi_JobNumber, varchar(50) Comma-delimited list of Job Numbers to get counts from.
*	pi_TargetPGRP, varchar(50), OPTIONAL comma-delimited list of PGRPs that contain the final set of 
*		items to be published in this job. 	
*
*  Outputs:
*	Record set containing the following fields
*	- ProcessingGroupID
*	- MissingHTMLCount
*	
*  Revision History:
*	2006/02/27 - CJM Created
*	2006/02/28 - CJM Ignore containers
*	2006/03/30 - CJM Added Nolock where it was missing and removed duplicate reference to tblHTMLItem.
*	2006/06/28 - QVN/CJM added PGRP lookup ability and size threhold contigency
*	
*********************************************************************************************************/
--Set up Variables
use case<CaseID,int,-1>


declare @pi_TargetPGRP varchar(50)
declare @pi_JobNumber varchar(50)
declare @HTML int
declare @HTMLSize int
select @pi_JobNumber = '<JobNumber,varchar,-1>'
select	@HTML = '<HTML Threshold REQUIRED,varchar,3>'
select	@pi_TargetPGRP = '<TargetPGRP Optional,varchar,>'


IF @HTML = '3'
BEGIN
	set @HTMLSize = 4718592
END
else
BEGIN
	set @HTMLSize = 1572864
end


----------------------------------------------------------------------------------------------------------

CREATE TABLE #JobPgrps
(
JobNumber INT
,ProcessingGroupID INT
)

IF (@pi_TargetPGRP = '')
BEGIN
	INSERT INTO #JobPGRPs
	(JobNumber,ProcessingGroupID)
	select pt.JobNumber, MAX(ptpg.ProcessingGroupID)
	from tblPartitioningTask pt (nolock)
	join tblPartitioningTaskProcessingGroup ptpg (nolock) on pt.PartitioningTaskID = ptpg.PartitioningTaskID
	where pt.PartitioningTaskTypeCd = 'PUB'
        and ProcessingGroupRoleCd = 'RES'
	and pt.JobNumber in (
		SELECT CAST(value AS INT)    
		FROM dbo.fnSplit(@pi_JobNumber ,',')
		)
	group by  pt.JobNumber
END
ELSE
BEGIN
	INSERT INTO #JobPGRPs
	(ProcessingGroupID)
	SELECT CAST(value AS INT)    
	FROM dbo.fnSplit(@pi_TargetPGRP ,',')
END

----------------------------------------------------------------------------------------------------------

-- 
-- SELECT pgrp.ProcessingGroupID
-- 	,dip.itemid--count(*) as MissingHTMLCount
-- FROM tblPartitionItemList pil (NOLOCK)
-- join tblderiveditemproperty dip (nolock) on pil.itemidentityid = dip.itemidentityid
-- JOIN tblPartition p (NOLOCK) ON p.PartitionID = pil.Partitionid
-- JOIN #JobPGRPs pgrp (NOLOCK) ON p.ProcessingGroupID = pgrp.ProcessingGroupID
-- LEFT JOIN tblHTMLItem hi (NOLOCK) ON hi.ItemIdentityID = pil.ItemIdentityID
-- WHERE hi.FilePath IS NULL
-- GROUP BY pgrp.ProcessingGroupID, dip.itemid

-- items in publish set not HTMLd
select p.processinggroupid, i.ItemIdentityID
	,i.ItemID, i.bytecnt
	--,count(*) as MissingHTMLCount
from tblItem i (nolock)
join tblPartitionItemList pil on pil.ItemIdentityID = i.ItemIdentityID
join tblPartition p on p.PartitionID = pil.PartitionID
JOIN #JobPGRPs pgrp (NOLOCK) ON p.ProcessingGroupID = pgrp.ProcessingGroupID
LEFT JOIN tblHTMLItem hi (NOLOCK) ON hi.ItemIdentityID = pil.ItemIdentityID
where
(
	i.ItemTypeID = 4
	or 
	i.FiosFileTypeID in 
	(
		select ft.FiosFileTypeID
		from tlkpFileType ft (nolock)
		join tblFileClassProcessingQueue fc_htm on fc_htm.ClassID = ft.FileClassID
		where fc_htm.ProcessingQueueCd = 'HTM'
	)
)
and not exists
(
	select *
	from tblItemError(nolock)
	where ItemIdentityID = i.ItemIdentityID
	and ProcessingQueueCd = 'HTM'
)
and hi.Itemidentityid is null
-- and not exists
-- (
-- 	select *
-- 	from tblHTMLItem(nolock)
-- 	where ItemIdentityID = i.ItemIdentityID
-- )
--is it a container?
and dbo.fnITM_IsContainer(i.itemidentityid) = 'N'
 --check to see if item is oversized, if so, do not  include in results, PCTR does not attempt HTML
and i.itemidentityid in (select itemidentityid from tblitem where bytecnt < @HTMLSize)
group by  p.processinggroupid, i.ItemIdentityID
	,i.ItemID, i.bytecnt

----------------------------------------------------------------------------------------------------------
--Clean up
DROP TABLE #JobPgrps
