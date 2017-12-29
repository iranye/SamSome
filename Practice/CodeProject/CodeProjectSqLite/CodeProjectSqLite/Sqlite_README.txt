https://www.codeproject.com/Articles/1210189/Using-SQLite-in-Csharp-VB-Net

* Test *
Create new db file where none previously existed.
Create new db file where one previously existed.
Use existing db file.


* Want *
Be able to create or use existing db (i.e., db file)
Be able to create or use existing tables

* Fixes & New Stuff *
Add checkbox to use all lines for sql command (instead of treating each line as a separate command).
Add on-click listener for db-file textbox to change to full-path on click (to allow for corrections)


* CREATE TABLE INSERT TABLE *
CREATE TABLE IF NOT EXISTS Test (Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, NAME NVARCHAR(2048) NULL)
CREATE UNIQUE INDEX index_name ON Test ( name );

INSERT INTO Test (name) VALUES ('Meagen')
INSERT INTO test (text) VALUES ('Test Text 2')

* FOREIGN KEYS *
CREATE TABLE IF NOT EXISTS [itemtype] ([id] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, [name] VARCHAR(256) NOT NULL )
CREATE TABLE IF NOT EXISTS [solution] ( [id] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, [parent] INTEGER NOT NULL, [level] INTEGER NOT NULL, [name] VARCHAR(256) NOT NULL, [itemtypeid] INTEGER NOT NULL, FOREIGN KEY (itemtypeid) REFERENCES itemtype(id) )

INSERT INTO itemtype(name) VALUES ('Protractor')
INSERT INTO solution(parent, level, name, itemtypeid) VALUES (1, 2, 'Gadget', 1)

select * from itemtype
select * from solution

select i.name, s.name
from solution s
join itemtype i on i.id = s.itemtypeid

* TBD *
INSERT INTO test (id, text) VALUES (1, 'Test Text 1')
INSERT INTO test (id, text) VALUES (2, 'Test Text 2')

INSERT INTO test (text) VALUES ('Test Text 4')

SELECT * FROM test

* DELETE & DROP *
DROP TABLE test
DELETE FROM test WHERE id = 2


* Foreign Keys *
connectString.ForeignKeys = true;

string createQuery =
    @"CREATE TABLE IF NOT EXISTS
        itemtype (
        id           INTEGER      NOT NULL PRIMARY KEY,
        name         VARCHAR(256) NOT NULL
        )";

using (SQLiteCommand cmd = new SQLiteCommand(db.Connection))
{
    cmd.CommandText = createQuery;
    cmd.ExecuteNonQuery();
}

createQuery =
    @"CREATE TABLE IF NOT EXISTS
        solution (
        id           INTEGER      NOT NULL PRIMARY KEY,
        parent       INTEGER      NOT NULL,
        level        INTEGER      NOT NULL,
        name         VARCHAR(256) NOT NULL,
        itemtypeid   INTEGER      NOT NULL,
        FOREIGN KEY (itemtypeid) REFERENCES itemtype(id)
        )";

using (SQLiteCommand cmd = new SQLiteCommand(db.Connection))
{
    cmd.CommandText = createQuery;
    cmd.ExecuteNonQuery();
}