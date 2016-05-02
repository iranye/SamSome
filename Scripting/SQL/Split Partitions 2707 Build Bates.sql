use case2707
go

set nocount on

-- declare variables
declare @ProcessingGroupID 

declare BuildBates cursor local forward_only for
select ProcessingGroupID
from tblProcessingGroup where ProcessingGroupID in (374,409,410,411,412)
order by Notes

open BuildBates 
fetch next from BuildBates into @ProcessingGroupID
while @@fetch_status = 0
begin
	
	exec dbo.procBuildBatesDocPage_CustomBatesSortedByItemPath_Job53
	@pi_ProcessingGroupID = @ProcessingGroupID,
	@pi_BatesSetID = 1

	-- next row
	fetch next from BuildBates into @ProcessingGroupID
end

-- cleanup
close BuildBates 
deallocate BuildBates 

set nocount off