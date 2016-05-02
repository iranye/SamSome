/*********************************************************************************************************
*  QC_ItemLevelStamping  
* 
*  Purpose: 
*	Returns the number of items with each stamping template set up with a total count for the pgrp.
* 	
*  Inputs:
*	CaseID
*	TargetPGRP
* 	
*  Outputs:
*	BatesStampTemplate (VARCHAR) - From tblProductionSetOptionsPartitionItemStamp.BatesSTampTemplate
*	ItemCount (INT) - How many items are set up with this stamping template
*	
*  Revision History:
*	03/21/05 - HWG Created
*	07/28/05 - CJM Templatized and added totals.
*	05/29/2006 - QVN made PGRP input optional, now we can use Job number if its a standard Tiff job
*
*  To Do:
*	This needs optimization. Since we utilize both tblItem and tblPartilItemList, we should find
*		a way to increase the performance.
*	
*********************************************************************************************************/
use case<CaseID,int,>

declare @TargetPGRP VARCHAR(50)
DECLARE @pi_JobNumber int

SET @pi_JobNumber = '<JobNumber,int,-1>'
set	@TargetPGRP = '<TargetPGRP Optional,VARCHAR,>'

IF (@TargetPGRP = '')
BEGIN
SET @TargetPGRP = (select top 1 ptpg.ProcessingGroupID
                      FROM tblPartitioningTask pt (NOLOCK)
                      JOIN tblPartitioningTaskProcessingGroup ptpg  (NOLOCK)
                        ON pt.PartitioningTaskID = ptpg.PartitioningTaskID
                      WHERE pt.PartitioningTaskTypeCd = 'TIF'
                      AND pt.JobNumber = @pi_JobNumber
                      GROUP BY  ptpg.ProcessingGroupID
                      ORDER BY ptpg.ProcessingGroupID DESC
                      )
END

----------------------------------------------------------------------------------------------------------

select		psopis.BatesStampTemplate, count (pil.ItemIdentityID) ItemCount
from		tblPartitionItemList pil (nolock)
join		tblPartition p (nolock)
on		pil.PartitionID = p.PartitionID
join		tblProductionSetOptionsPartitionItemStamp psopis (nolock)
on		p.PartitionID = psopis.PartitionID
and		pil.ItemIdentityID = psopis.ItemIdentityID
where		p.ProcessingGroupID in (
					SELECT CAST(value AS INT)    
				       	FROM dbo.fnSplit(@TargetPGRP ,',')
					) 
Group by 	psopis.BatesStampTemplate

Union

select		'zzzTotal' as BatesStampTemplate, count(*) TotalItemCount
from		tblPartitionItemList pil (nolock)
join		tblPartition p (nolock)
on		pil.PartitionID = p.PartitionID
where		p.ProcessingGroupID in (
					SELECT CAST(value AS INT)    
				       	FROM dbo.fnSplit(@TargetPGRP ,',')
					)


--Item Level Stamping Detail
select		p.PartitionName,p.PartitionID,pil.ItemIdentityID,psopis.BatesStampTemplate
from		tblPartitionItemList pil (nolock)
join		tblPartition p (nolock)
on		pil.PartitionID = p.PartitionID
join		tblProductionSetOptionsPartitionItemStamp psopis
on		p.PartitionID = psopis.PartitionID
and		pil.ItemIdentityID = psopis.ItemIdentityID
where		p.ProcessingGroupID in (
					SELECT CAST(value AS INT)    
				       	FROM dbo.fnSplit(@TargetPGRP ,',')
					) 

