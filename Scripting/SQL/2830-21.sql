USE case2830

--make sure the count for all partitions is 22068
SELECT count(itemIdentityID) FROM tblPartitionItemList WHERE PartitionID IN (914, 915, 916, 917, 918, 919, 928, 929, 930, 931, 932, 933)
--22068
--it is so we can continue

 
--create a new partition and processing group
select PartitionID,PartitionName
from tblPartition
where ProcessingGroupID = 191
--937	Job 21 NFE 

--populate the new partition with all items from partitions listed in the PR
DECLARE @PartitionID int
SET @PartitionID = 933
INSERT tblPartitionItem (PartitionID,ItemIdentityID,ExcludeFlag,IncludeChildrenFlag,IncludeParentFlag)
SELECT 937,ItemIdentityID,0,0,0
FROM tblPartitionItemList 
WHERE PartitionID = (@PartitionID) AND ItemIDentityID NOT IN (select ItemIdentityID FROM tblPartitionITem WHERE PartitionID = 937)

--update tblPartition to include children, forgot first go-round

SELECT * from tblPartitionItem  WHERE PartitionID = 937 --17430
UPDATE tblPartitionItem SET IncludeChildrenFlag = 1 WHERE PartitionID = 937 ----17430

--reset partition to be unexploded
SELECT * FROM tblPartition WHERE processingGroupID = 191
UPDATE tblPartition SET PartitionStatusCD = 'NEW' WHERE ProcessingGroupID = 191

procExplodeProcessingGroup 191
--937	17502

