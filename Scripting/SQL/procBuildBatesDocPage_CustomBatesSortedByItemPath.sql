SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


ALTER  PROCEDURE dbo.procBuildBatesDocPage_CustomBatesSortedByItemPath
(
    @pi_ProcessingGroupID INT,  -- REQUIRED: Processing group 
    @pi_BatesSetID INT      -- REQUIRED: Bates Set ID to build bates data under
)
AS
/*************************************************
 *                      
 *  Functionality: Creates OutputPageBates records in the Bates Document/Page Style for a Processing Group
 *
 *  What applications call this procedure: None yet, will be DMT - Called by procBuildOutputPageBates
 *
 *  Why is this procedure needed: To be able to publish and tiff
 *
 *  How is this procedure called: By the processing center or PSI staff from Query Analyzer
 *
 *  REVISION:   $Revision: 4 $
 *          $Author: Jfeingold $
 *          $Date: 5/14/03 9:21p $
 *
 ************************************************/
SET NOCOUNT ON



DECLARE @BatesDocNumber INT,
    @PartitionID INT,
    @BatesPrefixID INT,
    @spName varchar(64),
    @ErrorMessage varchar(100),
    @Error INT,
    @RowCount INT


-- Hold all pages from the processing group. Used to compute the new Bates Numbers
Create table #Pages (BatesPrefixID int,
            ItemIdentityID int,
            Page int,
            BatesDocNumber int,
            BatesSuffix int,
            PARTITIONID     INT)        -- ADDED BY JIF 5/12/03

-- Used to assign new DocNumbers to items that don't have a number
Create table #Docs (BatesPrefixID int,
            ItemIdentityID int,
            BatesDocNumber int identity(1,1)
        )

-- Used to keep track of starting batesdocnumbers for each prefix since multiple prefixes can
-- be inserted into #Docs
Create table #PrefixStartDoc(BatesPrefixID int,
            StartBatesDocNumber int)

-- Used to track current maximum doc number used for each prefix.
Create table #PrefixMaxDoc(BatesPrefixID int,
            MaxBatesDocNumber int)

SELECT @spName = object_name(@@PROCID)


-- Check Prefix Errors:
--  A prefix error is when the prefix on tblPartition doesn't match a previously assigned
--  bates prefix for some item in the partition.
IF EXISTS( Select * from tblPartition p 
        JOIN tblPartitionItemList pil on p.PartitionID = pil.PartitionID
        WHERE ProcessingGroupID = @pi_ProcessingGroupID
        and exists( Select *  from  tblOutputPageBates opb
            WHERE opb.ItemIdentityID = pil.ItemIdentityID
            and opb.batessetid = @pi_BatesSetID
            and p.BatesPrefixID <> opb.BatesPrefixID
            and opb.page = 1)
)
BEGIN
    SELECT @ErrorMessage = 'Some Items Already Bates with different prefix'
    RAISERROR(51010, 16, 1, @spName, '', '', @ErrorMessage) WITH LOG, SETERROR
    RETURN(@@Error)
END

-- Get all pages for the processing group, and any currently assigned Bates Numbers
-- and populates #Pages

--  COMMENTED OUT TO SORT BY PARTITIONID (REPLACED BELOW) JIF 5/12/03
--  Insert into #Pages 
--  Select Distinct Coalesce(opb.BatesPrefixID, p.BatesPrefixID),
--      pil.ItemIdentityID, 
--      op.Page,
--      opb.BatesDocNumber,
--      op.Page
--  From tblPartitionItemList pil
--      join tblPartition p on pil.PartitionID = p.PartitionID
--      join tblOutputPage op on op.ItemIdentityID = pil.ItemIdentityID
--      LEFT OUTER JOIN tblOutputPageBates opb on opb.BatesSetID = @pi_BatesSetID
--          and opb.ItemIdentityID = pil.ItemIdentityID
--          and opb.Page = 1
--  Where p.ProcessingGroupID = @pi_ProcessingGroupID


Insert into #Pages 
Select      Coalesce(opb.BatesPrefixID, p.BatesPrefixID),
        pil.ItemIdentityID, 
        op.Page,
        opb.BatesDocNumber,
        op.Page,
        MIN(P.PARTITIONID)
From        tblPartitionItemList pil
        join tblPartition p on pil.PartitionID = p.PartitionID
        join tblOutputPage op on op.ItemIdentityID = pil.ItemIdentityID
LEFT OUTER JOIN tblOutputPageBates opb on opb.BatesSetID = @pi_BatesSetID
        and opb.ItemIdentityID = pil.ItemIdentityID
        and opb.Page = 1
Where       p.ProcessingGroupID = @pi_ProcessingGroupID
GROUP BY    Coalesce(opb.BatesPrefixID, p.BatesPrefixID),
        PIL.ITEMIDENTITYID,
        OPB.BATESDOCNUMBER,
        OP.PAGE

SELECT  @Error = @@ERROR,
    @RowCount = @@ROWCOUNT

IF @Error <> 0 BEGIN 
    SELECT @ErrorMessage = 'ProcessingGroup: ' + COALESCE(cast(@pi_ProcessingGroupID as varchar(30)),'NULL') + '  BatesSetID: ' + COALESCE(cast(@pi_BatesSetID as varchar(30)),'NULL') 
    RAISERROR(51001, 16, 1, @Error, @spName, '#Pages', @ErrorMessage) WITH LOG, SETERROR
    RETURN(@Error)
END

-- Compute suffix using relative page algorithm. For a Doc/Page bates, the suffix is defined to
-- be the relative page number within the document. Since pages can and are removed by QC to handle
-- blanks, we can't depend on the Page field
Update p1
Set BatesSuffix = (Select count(*) from #Pages p2
            where p2.ItemIdentityID = p1.ItemIdentityID
            and p2.page <= p1.page)
from #Pages p1

SELECT  @Error = @@ERROR,
    @RowCount = @@ROWCOUNT

IF @Error <> 0 BEGIN 
    SELECT @ErrorMessage = 'ProcessingGroup: ' + COALESCE(cast(@pi_ProcessingGroupID as varchar(30)),'NULL') + '  BatesSetID: ' + COALESCE(cast(@pi_BatesSetID as varchar(30)),'NULL') 
    RAISERROR(51002, 16, 1, @Error, @spName, '#Pages BatesSuffix', @ErrorMessage) WITH LOG, SETERROR
    RETURN(@Error)
END

-- Check for mis-matched pages:
--    A mis-matched page is a page that has the same relative page number, but a different absolute page number
--    This can happen in re-tiffing, and will need to be handled manually since we don't want to
--    renumber existing pages
IF EXISTS( Select * from #Pages p
        JOIN tblOutputPageBates opb on p.ItemIdentityID = opb.ItemIdentityID
            and opb.BatesSetID = @pi_BatesSetID and p.BatesSuffix = opb.BatesSuffix
            and opb.Page <> p.Page)
BEGIN
    Select * from #Pages p
        JOIN tblOutputPageBates opb on p.ItemIdentityID = opb.ItemIdentityID
            and opb.BatesSetID = @pi_BatesSetID and p.BatesSuffix = opb.BatesSuffix
            and opb.Page <> p.Page
    SELECT @ErrorMessage = 'Some Pages exist that do not have OutputPageBates records'
    RAISERROR(51010, 16, 1, @spName, '', '', @ErrorMessage) WITH LOG, SETERROR
    RETURN(@@Error)
END


-- Insert a list of Items with no BatesDocNumber to allow an identity column to assign the number
Insert into  #Docs (BatesPrefixID,
    ItemIdentityID)
Select BatesPrefixID,
    ItemIdentityID
from #Pages where BatesDocNumber is NULL
and Page = 1
--Order by BatesPrefixID,  PARTITIONID,dbo.fnGetBatesSort(ItemIdentityID)
order by BatesPrefixID,  PARTITIONID,dbo.fnGetItemPathByItemIdentityID(ItemIdentityID), dbo.fnGetBatesSort(ItemIdentityID), page
--new sort order added on 11-01-2004 --BR



SELECT  @Error = @@ERROR,
    @RowCount = @@ROWCOUNT

IF @Error <> 0 BEGIN 
    SELECT @ErrorMessage = 'ProcessingGroup: ' + COALESCE(cast(@pi_ProcessingGroupID as varchar(30)),'NULL') + '  BatesSetID: ' + COALESCE(cast(@pi_BatesSetID as varchar(30)),'NULL') 
    RAISERROR(51001, 16, 1, @Error, @spName, '#Docs', @ErrorMessage) WITH LOG, SETERROR
    RETURN(@Error)
END


-- Find the start BatesDocNumber for each prefix in the Processing Group
Insert into #PrefixStartDoc 
Select BatesPrefixID, min(BatesDocNumber) 
from #Docs
group by BatesPrefixID

SELECT  @Error = @@ERROR,
    @RowCount = @@ROWCOUNT

IF @Error <> 0 BEGIN 
    SELECT @ErrorMessage = 'ProcessingGroup: ' + COALESCE(cast(@pi_ProcessingGroupID as varchar(30)),'NULL') + '  BatesSetID: ' + COALESCE(cast(@pi_BatesSetID as varchar(30)),'NULL') 
    RAISERROR(51001, 16, 1, @Error, @spName, '#PrefixStartDoc', @ErrorMessage) WITH LOG, SETERROR
    RETURN(@Error)
END

-- Find the Maximum current BatesDocNumber for each prefix that will receive new docs
Insert into #PrefixMaxDoc
Select psd.BatesPrefixID,
    Max(BatesDocNumber)
from #PrefixStartDoc psd
    Left Outer JOIN tblOutputPageBates opb
    on opb.BatesSetID = @pi_BatesSetID
    and opb.BatesPrefixID = psd.BatesPrefixID
Group by psd.BatesPrefixID

SELECT  @Error = @@ERROR,
    @RowCount = @@ROWCOUNT

IF @Error <> 0 BEGIN 
    SELECT @ErrorMessage = 'ProcessingGroup: ' + COALESCE(cast(@pi_ProcessingGroupID as varchar(30)),'NULL') + '  BatesSetID: ' + COALESCE(cast(@pi_BatesSetID as varchar(30)),'NULL') 
    RAISERROR(51001, 16, 1, @Error, @spName, '#PrefixMaxDoc', @ErrorMessage) WITH LOG, SETERROR
    RETURN(@Error)
END

-- If the MaxDoc is null, set it to the StartingBatesNumber - 1 as last used doc number
Update pmd
    Set MaxBatesDocNumber = bp.StartingBatesNumber - 1
From #PrefixMaxDoc pmd
    JOIN tblBatesPrefix bp on pmd.BatesPrefixID = bp.BatesPrefixID
Where pmd.MaxBatesDocNumber IS NULL

SELECT  @Error = @@ERROR,
    @RowCount = @@ROWCOUNT

IF @Error <> 0 BEGIN 
    SELECT @ErrorMessage = 'ProcessingGroup: ' + COALESCE(cast(@pi_ProcessingGroupID as varchar(30)),'NULL') + '  BatesSetID: ' + COALESCE(cast(@pi_BatesSetID as varchar(30)),'NULL') 
    RAISERROR(51002, 16, 1, @Error, @spName, '#PrefixMaxDoc', @ErrorMessage) WITH LOG, SETERROR
    RETURN(@Error)
END


-- Place BatesDocNumber back page table (Note: The suffix is already there)
Update p
Set BatesDocNumber =  d.BatesDocNumber - psd.StartBatesDocNumber + pmd.MaxBatesDocNumber + 1
from #PrefixStartDoc psd
JOIN #PrefixMaxDoc pmd on psd.batesprefixid = pmd.batesprefixid
JOIN #Docs d on d.BatesPrefixID = psd.BatesPrefixID
JOIN #Pages p on p.BatesPrefixID = d.BatesPrefixID
    AND p.ItemIdentityID = d.ItemIdentityID

SELECT  @Error = @@ERROR,
    @RowCount = @@ROWCOUNT
    
IF @Error <> 0 BEGIN 
    SELECT @ErrorMessage = 'ProcessingGroup: ' + COALESCE(cast(@pi_ProcessingGroupID as varchar(30)),'NULL') + '  BatesSetID: ' + COALESCE(cast(@pi_BatesSetID as varchar(30)),'NULL') 
    RAISERROR(51002, 16, 1, @Error, @spName, '#Pages BatesDocNumber', @ErrorMessage) WITH LOG, SETERROR
    RETURN(@Error)
END


BEGIN TRAN
    -- Insert new rows into tblOutputPageBates
    Insert into tblOutputPageBates 
    Select @pi_BatesSetID, p.ItemIdentityID, p.Page, p.BatesPrefixID, p.BatesDocNumber, p.BatesSuffix
    from #Pages p
    where not exists(
        Select * from tblOutputPageBates opb
            where opb.BatesSetID = @pi_BatesSetID
            and opb.ItemIdentityID = p.ItemIdentityID
            and opb.Page = p.Page )

    IF @Error <> 0 BEGIN 
        ROLLBACK TRAN
        SELECT @ErrorMessage = 'ProcessingGroup: ' + COALESCE(cast(@pi_ProcessingGroupID as varchar(30)),'NULL') + '  BatesSetID: ' + COALESCE(cast(@pi_BatesSetID as varchar(30)),'NULL') 
        RAISERROR(51001, 16, 1, @Error, @spName, 'tblOutputPageBates', @ErrorMessage) WITH LOG, SETERROR
        RETURN(@Error)
    END

COMMIT TRAN

SET NOCOUNT OFF

Return 0



GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

