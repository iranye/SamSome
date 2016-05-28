
DROP TABLE #City

Create TABLE #City (
    CityID INT,
    CityName VARCHAR(255),
    StateCd CHAR(2)
    PRIMARY KEY (CityID)
)

INSERT INTO #City (CityID, CityName, StateCd) VALUES (1,'Portland','OR')
INSERT INTO #City (CityID, CityName, StateCd) VALUES (2,'Milwaukie','OR')
INSERT INTO #City (CityID, CityName, StateCd) VALUES (3,'Vancouver','WA')

SELECT * FROM #City
------------------------------------------------------------------------
DROP TABLE #Cat

CREATE TABLE #Cat (
     CatID SMALLINT IDENTITY PRIMARY KEY
    ,CatName VARCHAR(20)
    ,ReportsToCatID INT
    ,FoundInCityID INT
    ,FOREIGN KEY (FoundInCityID) REFERENCES #City
)

INSERT INTO #Cat(CatName, ReportsToCatID, FoundInCityID) VALUES ('Red', NULL, 2)
INSERT INTO #Cat(CatName, ReportsToCatID, FoundInCityID) VALUES ('Sam', 1, 1)
INSERT INTO #Cat(CatName, ReportsToCatID, FoundInCityID) VALUES ('Sheldon', 2, 3)
INSERT INTO #Cat(CatName, ReportsToCatID, FoundInCityID) VALUES ('Vinny', 2, 2)

SELECT * FROM #Cat
------------------------------------------------------------------------
DROP TABLE #CatNap

CREATE TABLE #CatNap (
    CatID SMALLINT,
    StartDtm DATETIME,
    EndDtm DATETIME,
    FOREIGN KEY (CatID) REFERENCES #Cat
)

INSERT INTO #CatNap(CatID, StartDtm, EndDtm) VALUES (1,'2006-05-27 06:00:00','2006-05-27 07:30:00')
INSERT INTO #CatNap(CatID, StartDtm, EndDtm) VALUES (1,'2006-05-27 07:45:00.000','2006-05-27 08:30:00.000')
INSERT INTO #CatNap(CatID, StartDtm, EndDtm) VALUES (1,'2006-05-27 08:45:00.000','2006-05-27 09:30:00.000')
INSERT INTO #CatNap(CatID, StartDtm, EndDtm) VALUES (1,'2006-05-27 09:45:00.000','2006-05-27 10:30:00.000')
INSERT INTO #CatNap(CatID, StartDtm, EndDtm) VALUES (1,'2006-05-28 06:00:00.000','2006-05-28 07:30:00.000')
INSERT INTO #CatNap(CatID, StartDtm, EndDtm) VALUES (3,'2006-05-27 06:00:00.000','2006-05-27 11:00:00.000')
INSERT INTO #CatNap(CatID, StartDtm, EndDtm) VALUES (4,'2006-05-27 06:00:00.000','2006-05-27 09:30:00.000')
INSERT INTO #CatNap(CatID, StartDtm, EndDtm) VALUES (4,'2006-05-27 12:00:00.000','2006-05-27 13:00:00.000')

SELECT * FROM #CatNap
DELETE #CatNap WHERE EndDtm = '2006-05-27 13:00:00.000'
------------------------------------------------------------------------

DECLARE @n int
SET @n = 2

SELECT TOP (@n) *
FROM #Cat
ORDER BY CatName

------------------------------------------------------------------------
-- list all cats found in Oregon that have taken at least two naps
select c.CatName
from #Cat c
join #City l on l.CityID = c.FoundInCityID
where l.StateCd = 'OR'
intersect
select c.CatName
from #Cat c
join #CatNap n on n.CatID = c.CatID
group by c.catName
having count(*) >= 2

-- list all cats, except those found in Washington state
select c.CatName,l.CityName as FoundIn
from #Cat c
join #City l on l.CityID = c.FoundInCityID
except
select c.CatName,l.CityName as FoundIn
from #Cat c
join #City l on l.CityID = c.FoundInCityID
where l.StateCd = 'WA'

declare @CatLog as table (CatID int, CatName varchar(50))
insert #Cat (CatName) 
OUTPUT inserted.CatID,inserted.CatName into @CatLog
values ('Garfield')
select * from @CatLog

declare @CatLog as table (CatID int, CatName_Old varchar(50),CatName_New varchar(50))
update #Cat -- SELECT * FROM #Cat
set CatName = 'Morris'
OUTPUT inserted.CatID,deleted.CatName,inserted.CatName into @CatLog
where CatName = 'Garfield'
select * from @CatLog

-- used recursively to display cat hierarchy
with Reports (CatID,CatName,CatLevel) as
(
        -- anchor member
        select CatID,CatName,0 as CatLevel
        from #Cat
        where ReportsToCatID is null

        union all

        -- recursive member
        select c.CatID,c.CatName,r.CatLevel + 1 
        from #Cat c
        join Reports r on r.CatID = c.ReportsToCatID
)
select * from Reports

SELECT * FROM #Cat
ORDER BY ReportsToCatID

-- cat naps per cat per day
select CatID,CatName,[27] May27,[28] May28
from 
(
        select c.CatID,c.CatName,NapDay = day(cn.StartDtm),Duration = datediff(n,cn.StartDtm,cn.EndDtm)
        from #Cat c
        join #CatNap cn on cn.CatID = c.catID
) naps
PIVOT
(
        sum(Duration)
        for NapDay in ([27],[28])
) pvt
order by CatID,CatName

SELECT * FROM #CatNap

-- cat naps per cat per day
select CatID,CatName,Duration
from 
(
        select c.CatID,c.CatName,NapDay = day(cn.StartDtm),Duration = datediff(n,cn.StartDtm,cn.EndDtm)
        from #Cat c
        join #CatNap cn on cn.CatID = c.catID
) naps
PIVOT
(
        sum(Duration)
        for NapDay in ([27],[28])
) pvt
UNPIVOT
(
        Duration for NapDay in ([27],[28])
) upvt
order by CatID,CatName

select ROW_NUMBER() OVER (order by cn.StartDtm) as RowNumber,
c.CatName,cn.StartDtm,cn.EndDtm
from #Cat c
join #CatNap cn on cn.CatID = c.CatID

-- list catnaps in chronological order
-- partitioned by cat
select c.CatName,
ROW_NUMBER() OVER (PARTITION BY c.CatName order By cn.StartDtm),
cn.StartDtm,cn.EndDtm
from #Cat c
join #CatNap cn on cn.CatID = c.CatID

-- cat naps odered by length
select DENSE_RANK() OVER (Order by datediff(n,cn.StartDtm,cn.EndDtm)) DenseRank,
RANK() OVER (Order by datediff(n,cn.StartDtm,cn.EndDtm)) Rank,
c.CatName,cn.StartDtm,cn.EndDtm,
datediff(n,cn.StartDtm,cn.EndDtm) NapLength
from #Cat c
join #CatNap cn on cn.CatID = c.CatID

-- attempt to insert invalid cat nap record
begin try
        insert #CatNap (CatID,StartDtm,EndDtm)
        values (1,'5/28/06','5/27/06')
end try
begin catch
        select error_number() errNumber,
        error_state() errState,
        error_severity() errSeverity,
        error_message() errMessage
end catch

create table #tblXML
(
        RowID int not null primary key,
        xmlColumn xml
)

-- XML
insert #tblXML (RowID,xmlColumn)
values (1,'<cat/>')

SELECT * FROM #tblXML

DROP procedure dbo.procUpdateXmlTable
create procedure dbo.procUpdateXmlTable
(
        @pi_RowID int,
        @pi_xml xml
)

as

update #tblXML
set xmlColumn = @pi_xml
where RowID = @pi_RowID

exec dbo.procUpdateXmlTable
@pi_RowID = 1,
@pi_xml = '<cat><CatID>1</CatID></cat>'

declare @xml xml
set @xml = (select * from #Cat for xml auto)

exec dbo.procUpdateXmlTable
@pi_RowID = 1,
@pi_xml = @xml

select * from #tblXML

insert #tblXML (RowID,xmlColumn)
values (3,
'<Cat> 
<Name value="Red"/> 
<Color value="Red"/>
<Color value="White"/>
</Cat>')

insert tblXML (RowID,xmlColumn)
values (4,
'<Cat> 
<Name value="Red"/> 
<Color value="Red"/>
<Color value="White"/>
<NumMinions value="3"/>
</Cat>')

select xmlColumn.query('Cat/Color')
from #tblXML
where RowID = 3

-- first color
select xmlColumn.value('(Cat/Color/@value)[1]','varchar(50)')
from #tblXML
where RowID = 4

select xmlColumn.exist('Cat/NumMinions[@value=3]')
from #tblXML
where RowID = 4

select * from #tblXML


select cat.color.value('@value','varchar(50)') as Value
from #tblXML x
cross apply x.xmlColumn.nodes('/Cat/Color') as cat(color)
where RowID = 3

select TOP 5 [name],create_date from sys.tables order by [name]
