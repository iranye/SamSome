-- Using temporary tables, it's possible to practice various SQL commands
-- Examples based loosely on those used on w3schools.com: http://www.w3schools.com/sql/sql_join.asp
-- UGLY HACK WARNING: uses data from a pre-existing table to populate the new (volatile) tables

-- Our source table is tblProductionSetOptions, where SettingsID is the PK
SELECT * FROM tblProductionSetOptions 
ORDER BY SettingsID

-- General SELECT and DROP statements for Printing and Deleting tables
SELECT * FROM #emptbl
SELECT * FROM #ordtbl
DROP TABLE #emptbl
DROP TABLE #ordtbl

-- populate #emptbl
SELECT SettingsID Employee_ID, 'Svendson, Stephen' Name
INTO #emptbl
FROM tblProductionSetOptions 
WHERE SettingsID IN (2, 3, 4, 5)

UPDATE #emptbl
SET Name='Svendson, Tove'
WHERE Employee_ID=2

UPDATE #emptbl
SET Name='Pettersen, Kari'
WHERE Employee_ID=4

UPDATE #emptbl
SET Name='Hansen, Ola'
WHERE Employee_ID=5

---------------------------------------
SELECT * FROM tblProductionSetOptions 
ORDER BY SettingsID

SELECT * FROM #emptbl
SELECT * FROM #ordtbl
DROP TABLE #emptbl
DROP TABLE #ordtbl

-- Populate #ordtbl
SELECT ProductionSetID Prod_ID, tblProductionSetOptions.Value Product, ProductionSetID Employee_ID
INTO #ordtbl
FROM tblProductionSetOptions 
WHERE SettingsID IN (3, 11, 4)

--WHERE Value IN ('500', 'FensWithPageNumber', 8)

UPDATE #ordtbl
SET Prod_ID=234, Product='Printer', Employee_ID=5
WHERE Product='500'

UPDATE #ordtbl
SET Prod_ID=657, Product='Table', Employee_ID=3
WHERE Product='8'

UPDATE #ordtbl
SET Prod_ID=865, Product='Chair', Employee_ID=3
WHERE Product='FensWithPageNumber'

SELECT #emptbl.Name
FROM #emptbl, #ordtbl
WHERE #emptbl.Employee_ID=#ordtbl.Employee_ID
AND #ordtbl.Product='Printer' 

SELECT #emptbl.Name, #ordtbl.Product
FROM #emptbl
INNER JOIN #ordtbl
ON #emptbl.Employee_ID=#ordtbl.Employee_ID 

SELECT #emptbl.Name, #ordtbl.Product
FROM #emptbl
LEFT JOIN #ordtbl
ON #emptbl.Employee_ID=#ordtbl.Employee_ID 

SELECT #emptbl.Name
FROM #emptbl
INNER JOIN #ordtbl
ON #emptbl.Employee_ID=#ordtbl.Employee_ID
WHERE #ordtbl.Product = 'Printer'