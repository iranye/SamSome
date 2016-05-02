use case2152
go

/*
Job: 919
PE: Kit Gauthier
8/18/06
*/

-- Tiff
/*
-- universe: CMT
-- profile:  PET 
*/


-- **********************************************************************
-- PRELIMINARIES
-- **********************************************************************
-- CaseID	2152
-- CaseName	Chase
-- ClientID	1210
-- ClientName	Merck
-- PCTR_Server	PCTRSQL5
-- PRVL_Server	NULL

-- **********************************************************************
-- CUSTODIAN MAPPING
-- **********************************************************************

-- verify that mapping was finalized in CMT
-- this query should return one or more rows - all "frozen"
select Frozen,MediaID,GroupName,ItemPath from vwCMTCustodianGroupMapping where Job = 919 
--ok

-- verify that job record exists
select * from tblJob where JobNumber = 919 
--919	1251	NULL	NULL	0

-- check for duplicate custodians
select CustodianName from tblCustodian order by 1
--ok

-- **********************************************************************
-- WEB GROUPS
-- **********************************************************************

-- update web groups from CMT mappings for job 919
exec dbo.procCMTExplodeWebGroups @pi_JobNumber = 919
--ok

-- **********************************************************************
-- PARTITON IN PET
-- **********************************************************************

-- results:
exec dbo.procPE_Job_JobStepList 919
-- 1	Custom Custom Cull	1251	177826	1256	177578	Yes	Yes
-- 2	Custom Cull	1256	177578	1258	171104	Yes	Yes
-- 3	Custom Assemble Tiff Set	1258	171104	1259	90505	Yes	Yes

-- **********************************************************************
-- CHECK PROCESS SETTINGS
-- **********************************************************************



