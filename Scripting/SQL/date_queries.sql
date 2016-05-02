-- The following will query tblitemerror for new entries
declare @DateLookup varchar(33)
-- set @DateLookup = CONVERT(CHAR(8), GETDATE(), 10)
set @DateLookup = DatePart(YYYY, GETDATE()) -- + "-"+ DatePart(mm, GETDATE())

SELECT
TOP 55
@DateLookup today,
substring(CONVERT(CHAR(8), DateAdded, 10), 1, 8) dates,
* 
FROM tblitemerror t (NOLOCK)
--WHERE t.DateAdded LIKE '%today%'
ORDER BY DateAdded

SELECT 
DATEPART(yyyy, DateAdded) yyyy,
DATEPART(mm, DateAdded) mm,
*
FROM tblitemerror (NOLOCK)
WHERE
DATEPART(yyyy, DateAdded) > 2005
AND
DATEPART(mm, DateAdded) > 3
ORDER BY DateAdded

-- Example date formatting functions
PRINT Convert(CHAR(8), GETDATE(),10) gt "Nov 22 2005"

PRINT DATEPART(yyyy, GETDATE())
PRINT '1) HERE IS MON DD YYYY HH:MIAM (OR PM) FORMAT ==>' + 
CONVERT(CHAR(19),GETDATE())  
PRINT '2) HERE IS MM-DD-YY FORMAT ==>' + 
CONVERT(CHAR(8),GETDATE(),10)  
PRINT '3) HERE IS MM-DD-YYYY FORMAT ==>' + 
CONVERT(CHAR(10),GETDATE(),110) 
PRINT '4) HERE IS DD MON YYYY FORMAT ==>' + 
CONVERT(CHAR(11),GETDATE(),106)
PRINT '5) HERE IS DD MON YY FORMAT ==>' + 
CONVERT(CHAR(9),GETDATE(),6) 
PRINT '6) HERE IS DD MON YYYY HH:MM:SS:MMM(24H) FORMAT ==>' + 
CONVERT(CHAR(24),GETDATE(),113)
