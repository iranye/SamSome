declare @root varchar(20)
set @root = 'f:\'


select 'del ' + @root + 'USBAMEX_E001\' + partitionname + '\' + cdname + path
from tblPartitionbundle where itemidentityid in(
select itemidentityid from tblItem where itemid in(
5089588,
5100105,
5293455,
5312729,
5312813,
5347998
))
 and partitionid in(1082, 1101)
union
select 'del ' + @root + 'USBAMEX_E001\' + partitionname + '\' + cdname + replace(path, '.tif', '.txt')
from tblPartitionbundle where itemidentityid in(
select itemidentityid from tblItem where itemid in(
5089588,
5100105,
5293455,
5312729,
5312813,
5347998
))
 and partitionid in(1082, 1101)