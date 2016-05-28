--This script tells you the rate of Tiffing, and estimated time of completion.  

select count(*) as Remaining, dateadd( n, cast( count(*) as real) / (
select  Count(*) / 15. from tblProcessedItem (nolock) Where ProcessingQueueCd = 'tra'
And endtime between dateadd(n,-15,getdate()) and getdate()),getdate()) as [Completion Time], 
(select  Count(*) * 4 from tblProcessedItem (nolock) Where ProcessingQueueCd = 'tra'
and endtime between dateadd(n,-15,getdate()) and getdate()) as [Files per hour]
from tblqueueTranslator (Nolock)

