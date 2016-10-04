
IF OBJECT_ID ('dbo.Heap') IS NOT NULL  -- SELECT OBJECT_ID ('dbo.Heap')
BEGIN
	DROP TABLE #foo
END
create table dbo.Heap
(
	Val varchar(8000) not null
);
;with CTE(ID,Val)
as
(
	select 1, replicate('0', 4098)
	union all
	select ID + 1, Val from CTE WHERE ID < 20
)
insert into dbo.Heap
	select Val from CTE;

SELECT * FROM dbo.Heap

select page_count, avg_record_size_in_bytes, avg_page_space_used_in_percent
from sys.dm_db_index_physical_stats(db_id(),object_id(N'dbo.Heap'),0,null,'DETAILED');


-- WITH Clause
;WITH t100 AS (
 SELECT n=number
 FROM master..spt_values
 WHERE type='P' and number between 1 and 100
)                
SELECT
ISNULL
(
	NULLIF
	(
		CASE WHEN n % 3 = 0 THEN 'Fizz' Else '' END +
		CASE WHEN n % 5 = 0 THEN 'Buzz' Else '' END, ''
	), RIGHT(n,3)
)
FROM t100


-- NULLIF OPERATOR
IF OBJECT_ID ('tempdb..#foo') IS NOT NULL  -- SELECT OBJECT_ID ('tempdb..#foo')
BEGIN
	DROP TABLE #foo
END
GO
CREATE TABLE #foo
(
	id INT NOT NULL IDENTITY,
	flim INT,
	flam INT,
	blip INT,
	blap INT
)

INSERT #foo SELECT 1, 2, 3, NULL
INSERT #foo SELECT  NULL, 2, 3, 4
INSERT #foo SELECT  NULL, NULL, 3, 4
INSERT #foo SELECT 1, 1, 3, NULL
INSERT #foo SELECT 1, NULL, 3, NULL

SELECT * FROM #foo
SELECT COALESCE(flim, flam, blip, blap) FROM #foo


SELECT * FROM #foo
SELECT id, NULLIF(flim, flam) FROM #foo
SELECT ISNULL (flim, NULL) FROM #foo



-- NULLIF OPERATOR (MSDN)
IF OBJECT_ID ('dbo.budgets','U') IS NOT NULL  
   DROP TABLE budgets;  
GO  
SET NOCOUNT ON;  
CREATE TABLE dbo.budgets  
(  
   dept            tinyint   IDENTITY,  
   current_year      decimal   NULL,  
   previous_year   decimal   NULL  
);  
INSERT budgets VALUES(100000, 150000);  
INSERT budgets VALUES(NULL, 300000);  
INSERT budgets VALUES(0, 100000);  
INSERT budgets VALUES(NULL, 150000);  
INSERT budgets VALUES(300000, 250000);  
GO    
SET NOCOUNT OFF;  
SELECT AVG(NULLIF(COALESCE(current_year,  
   previous_year), 0.00)) AS 'Average Budget'  
FROM budgets;  
GO  


*/