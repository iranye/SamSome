--This script tells you the rate of Tiffing, and estimated time of completion.  

select count(*) as Remaining, dateadd( n, cast( count(*) as real) / (
select  Count(*) / 15. from tblProcessedItem (nolock) Where ProcessingQueueCd = 'TIF'
And endtime between dateadd(n,-15,getdate()) and getdate()),getdate()) as [Completion Time], 
(select  Count(*) * 4 from tblProcessedItem (nolock) Where ProcessingQueueCd = 'TIF'
and endtime between dateadd(n,-15,getdate()) and getdate()) as [Files per hour]
from tblqueueTiff (Nolock)

