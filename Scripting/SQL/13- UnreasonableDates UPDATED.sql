DECLARE @thisDate       DATETIME
DECLARE @DaysInTheFuture    INT
DECLARE @NumberBadDatesI    INT
DECLARE @NumberBadDatesIP   INT
DECLARE @LowMediaID         INT
DECLARE @HighMediaID        INT

--******************** YOU NEED TO SET THE LOW AND HIGH MEDIAID #'s****************************BR: 08-16-2002
SET @LowMediaID =  NLOWMEDIDN
SET @HighMediaID = NHIGHMEDIDN

--*********************************************************************************************

SET @DaysInTheFuture = 365
SET @thisDate        = dateadd(dd,@DaysInTheFuture,GETDATE())

SET NOCOUNT ON


-- ***********************************************************************
-- FILL IN MISSING DATES FROM ORIGINAL PROPERTIES
-- ***********************************************************************

/*
Sample Case (Deannes's):
caseID=2682&job=10&pgrp=64&prtidlow=195&prtidhigh=201&medidlow=4000364&medidhigh=4000364
-- #foo is a temporary table that may be used to hold values that get put into the tblItemProperties table

CREATE TABLE #foo (
    _itemid INT,
    _caseid INT,
    _propertyid INT,
    _propertyvalue VARCHAR(20),
    _dateadded DATETIME,
    _propertyname NVARCHAR(400)
)

-- So far, the following INSERT statement stores the same as what gets stored into tblItemProperties with 
-- the added field _propertyname, which takes the value from tblItemOriginalProperties Propertyname.
DROP TABLE #foo
DELETE #foo
INSERT #foo (_itemid, _caseid, _propertyid, _propertyvalue, _dateadded, _propertyname)
SELECT --TOP 1
i.ItemID,i.CaseID,0, 
MIN(CAST(CAST(REPLACE(LTRIM(RTRIM(CAST(iop.PropertyValue AS VARCHAR(8000)))),'GMT','') AS DATETIME) AS VARCHAR(20))),
GETDATE(), iop.PropertyName
FROM tblItem i (NOLOCK)
JOIN tblItemOriginalProperties iop (NOLOCK) ON iop.ItemID = i.ItemID
WHERE i.MediaID BETWEEN @LowMediaID AND @HighMediaID
AND i.FiosFileTypeID <> 972 -- unknown notes
AND iop.PropertyValue IS NOT NULL
AND iop.PropertyName IN ('PostedDate', 'DeliveredDate', 'Created', 'LastModified' )
GROUP BY i.ItemID,i.CaseID, iop.PropertyName
SELECT * FROM #foo

-- This may allow for a series of UPDATE statements to set _propertyid using the values put into _propertyname:
'PostedDate'   -- 288
'DeliveredDate'-- 272
'Created'      -- 248
'LastModified' -- 259


*/

-- sent
INSERT tblItemProperties 
(ItemID,CaseID,PropertyID,PropertyValue,DateAdded) 
SELECT i.ItemID,i.CaseID,288, 
MIN(CAST(CAST(REPLACE(LTRIM(RTRIM(CAST(iop.PropertyValue AS VARCHAR(8000)))),'GMT','') AS DATETIME) AS VARCHAR(20))),
GETDATE()
FROM tblItem i (NOLOCK)
JOIN tblItemOriginalProperties iop (NOLOCK) ON iop.ItemID = i.ItemID
WHERE i.MediaID BETWEEN @LowMediaID AND @HighMediaID
AND i.FiosFileTypeID <> 972 -- unknown notes
AND iop.PropertyName = 'PostedDate' 
AND iop.PropertyValue IS NOT NULL
AND NOT EXISTS 
( 
    SELECT * 
    FROM tblItemProperties ip (NOLOCK) 
    WHERE ip.ItemID = i.ItemID 
    AND ip.PropertyID = 288
) 
GROUP BY i.ItemID,i.CaseID


-- recd
INSERT tblItemProperties 
(ItemID,CaseID,PropertyID,PropertyValue,DateAdded) 
SELECT i.ItemID,i.CaseID,272, 
MIN(CAST(CAST(REPLACE(LTRIM(RTRIM(CAST(iop.PropertyValue AS VARCHAR(8000)))),'GMT','') AS DATETIME) AS VARCHAR(20))),
GETDATE()
FROM tblItem i (NOLOCK)
JOIN tblItemOriginalProperties iop (NOLOCK) on iop.ItemID = i.ItemID
WHERE i.MediaID BETWEEN @LowMediaID AND @HighMediaID
AND i.FiosFileTypeID <> 972 -- unknown notes
AND iop.PropertyName = 'DeliveredDate'
AND iop.PropertyValue IS NOT NULL
AND NOT EXISTS
( 
    SELECT * 
    FROM tblItemProperties ip (NOLOCK) 
    WHERE ip.ItemID = i.ItemID 
    AND ip.PropertyID = 272
) 
GROUP BY i.ItemID,i.CaseID



-- created
INSERT tblItemProperties 
(ItemID,CaseID,PropertyID,PropertyValue,DateAdded) 
SELECT i.ItemID,i.CaseID,248, 
MIN(CAST(CAST(REPLACE(LTRIM(RTRIM(CAST(iop.PropertyValue AS VARCHAR(8000)))),'GMT','') AS DATETIME) AS VARCHAR(20))),
GETDATE()
FROM tblItem i (NOLOCK)
JOIN tblItemOriginalProperties iop (NOLOCK) on iop.ItemID = i.ItemID
WHERE i.MediaID BETWEEN @LowMediaID AND @HighMediaID
AND i.FiosFileTypeID <> 972 -- unknown notes
AND iop.PropertyName = 'Created'
AND iop.PropertyValue is not null
AND not exists 
( 
    SELECT * 
    FROM tblItemProperties ip (NOLOCK) 
    WHERE ip.ItemID = i.ItemID 
    AND ip.PropertyID = 248
) 
GROUP BY i.ItemID,i.CaseID


-- modified
INSERT tblItemProperties 
(ItemID,CaseID,PropertyID,PropertyValue,DateAdded) 
SELECT i.ItemID,i.CaseID,259, 
MIN(CAST(CAST(REPLACE(LTRIM(RTRIM(CAST(iop.PropertyValue AS VARCHAR(8000)))),'GMT','') AS DATETIME) AS VARCHAR(20))),
GETDATE()
FROM tblItem i (NOLOCK)
JOIN tblItemOriginalProperties iop (NOLOCK) on iop.ItemID = i.ItemID
WHERE i.MediaID BETWEEN @LowMediaID AND @HighMediaID
AND i.FiosFileTypeID <> 972 -- unknown notes
AND iop.PropertyName = 'LastModified'
AND iop.PropertyValue is not null
AND not exists 
( 
    SELECT * 
    FROM tblItemProperties ip (NOLOCK) 
    WHERE ip.ItemID = i.ItemID 
    AND ip.PropertyID = 259
) 
GROUP BY i.ItemID,i.CaseID


-- ****************************************************************************************
-- BAD DATES
-- ****************************************************************************************


SELECT  @NumberBadDatesI = Count(distinct ItemIdentityID)
FROM    tblItem
WHERE   (MediaID BETWEEN @LowMediaID AND @HighMediaID)
    AND (
    (DateCreated < '1960' or DateCreated > @thisDate)
    Or (DateLastModified < '1960' or DateLastModified  > @thisDate)
    Or (DateLastAccessed < '1960' or DateLastAccessed  > @thisDate)
    )

SELECT  @NumberBadDatesIP = count(Distinct ip.ItemID)
FROM    tblItemProperties ip
JOIN    tblItem i on i.ItemID = ip.ItemID
WHERE   (MediaID BETWEEN @LowMediaID AND @HighMediaID)
    AND (
    PropertyID in (248,259,272,288,301)
    AND (left(cast(propertyValue as varchar(5)),4) < '1960' or
    --Left(cast(propertyValue as varchar(5)),4) > @thisDate)
    CAST(CAST(propertyValue AS VARCHAR(100))AS DATETIME) > @thisDate)
    )

if @@ERROR = 0 AND @NumberBadDatesI = 0 AND  @NumberBadDatesIP = 0
Begin
    Print 'This case does not have unreasonable dates.'
End
Else
Begin
    IF @@ERROR <> 0
        Begin
        GOTO Errorhandler
        End
    Else
        BEGIN  

        PRINT 'Unreasonable Dates found.  Refer to PCTR QC Engineer.'
    
        IF @NumberBadDatesIP <> 0
            Begin
            PRINT 'Dates Found in Item Properties:'
    
            SELECT CAST(ip.ItemID AS VARCHAR(100)) + ',' AS "ip.ItemID",
                ip.PropertyID, ap.Shortname, ip.propertyValue
            FROM    tblItemProperties ip (NOLOCK)
                JOIN tblItem i (NOLOCK) ON i.ItemID = ip.ItemID 
                JOIN tlkpApplicationProperties ap (NOLOCK)
                    ON ip.PropertyID = ap.PropertyID
            WHERE   (MediaID BETWEEN @LowMediaID AND @HighMediaID)
                AND (
                ip.PropertyID IN (248,259,272,288,301)
                AND (LEFT(CAST(propertyValue AS varchar(5)),4) < '1960' OR
                --LEFT(CAST(propertyValue AS varchar(5)),4) > @thisDate)
                CAST(CAST(propertyValue AS VARCHAR(100))AS DATETIME) > @thisDate)
                )
            End

        IF @NumberBadDatesI <> 0
            Begin   

            Print 'Dates Found in Item:'
        
            SELECT  CAST(ItemIdentityID AS VARCHAR(100)) + ',' AS "ItemIdentityID", 
                ItemName, DateCreated, DateLastModified, DateLastAccessed
            FROM    tblItem
            WHERE   (MediaID BETWEEN @LowMediaID AND @HighMediaID)
                AND (
                (DateCreated < '1960' or DateCreated > @thisDate)
                OR (DateLastModified < '1960' OR DateLastModified  > @thisDate)
                OR (DateLastAccessed < '1960' OR DateLastAccessed  > @thisDate)
                )
            End
        End
    
End

Return


ErrorHandler:

Print   'The Code F-ed up.'
Return


/* 

--For Loose Documents
UPDATE  tblitem 
SET dateCreated = NULL 
WHERE   itemidentityid IN (
155576,
173227
)


--For Email messages (REMEMBER TO USE ITEMID NOT ITEMIDENTITYIID.  DON'T FORGET THE PROPERTYID OR YOU WILL CHANGE ALL THE 
--PROPERTIES FOR THAT ITEM.
UPDATE tblitemproperties SET propertyvalue = NULL
-- SELECT * FROM tblitemproperties (NOLOCK)
WHERE itemid IN (
201813,
217685,
193967,
216599
)
AND propertyid = 272

caseID=2763&job=&pgrp=&prtidlow=&prtidhigh=&medidlow=4000355&medidhigh=4000355
*/