select * from tblitemerror (nolock)
order by dateadded desc

select * from tblitemerror (nolock)
where errorstatusCD = 'UNR'

select * from tblitemerror (nolock)
where itemidentityid in (
)

select * from tblitemerror (nolock)
where processingQueueCD = 	'TIF' 
select * from tblitemerror (nolock)
where processingQueueCD = 	'SIG'
select * from tblitemerror (nolock)
where processingQueueCD = 	'EMA'
select * from tblitemerror (nolock)
where processingQueueCD = 	'HTM'
select * from tblitemerror (nolock)
where processingQueueCD = 	'INV'
select * from tblitemerror (nolock)
where processingQueueCD = 	'ZIP'