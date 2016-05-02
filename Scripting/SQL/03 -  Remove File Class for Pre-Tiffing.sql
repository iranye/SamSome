

delete 
from tblQueueTranslator
where ItemIdentityID in (

select ItemIdentityID 
from tblitem i
join tlkpfiletype ft on ft.FiosFileTypeID = i.FiosFileTypeID
and ft.FileClassID = 8
where mediaid = 127991)
