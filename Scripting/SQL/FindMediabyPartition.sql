--This script is used to figure out what media are in a partition.  
--I have used this information to determine if the tiffed items were on a different share.

select * from tblprocessinggroup
select * from tblpartition where processinggroupid = 18

select distinct mediaid from tblpartitionitemlist tpl
join tblitem ti on tpl.itemidentityid = ti.itemidentityid
where partitionid between 242 and 277 

select * from tblpartition where processinggroupid = 263

select * from tblmedia where mediaid in ()


update tblmedia set conversionshare = '\\pctrtitan2\c1\'
 where mediaid in ()
