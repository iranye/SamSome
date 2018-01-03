https://www.codeproject.com/Articles/1158937/SQLite-with-Csharp-Net-and-Entity-Framework

To Run, copy SQLite.Interop.dll from lib directory to debug.

CREATE TABLE IF NOT EXISTS EmployeeMaster (Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, EmpName NVARCHAR NOT NULL, Salary DOUBLE NOT NULL, Designation VARCHAR NOT NULL)
INSERT INTO EmployeeMaster (EmpName, Salary, Designation) VALUES ('Ira', 123.34, 'Rocker')