CREATE TABLE #foo (foo_field int)

INSERT INTO #foo (foo_field) VALUES (99)

SELECT * FROM #foo

DROP TABLE #foo