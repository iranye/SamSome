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

* MTGO *
DROP TABLE CardType
CREATE TABLE IF NOT EXISTS CardType (Id INTEGER NOT NULL PRIMARY KEY, Name NVARCHAR(256) NOT NULL)
CREATE UNIQUE INDEX index_Name ON CardType ( Name )
INSERT INTO CardType (Id, Name) VALUES (1, 'Artifact')
INSERT INTO CardType (Id, Name) VALUES (2, 'Creature')
INSERT INTO CardType (Id, Name) VALUES (4, 'Enchantment')
INSERT INTO CardType (Id, Name) VALUES (8, 'Instant')
INSERT INTO CardType (Id, Name) VALUES (16, 'Land')

DROP TABLE Expansion
CREATE TABLE IF NOT EXISTS Expansion (Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, Name NVARCHAR(256) NOT NULL)
CREATE UNIQUE INDEX index_Name ON Expansion ( Name )

DROP TABLE Card
DELETE FROM Card WHERE Id=1
CREATE TABLE IF NOT EXISTS Card (Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, Name NVARCHAR(256) NOT NULL, CatID INTEGER NULL, CardTypeId INTEGER NOT NULL, MultiverseId INTEGER NULL, FOREIGN KEY (CardTypeId) REFERENCES CardType(Id))
CREATE UNIQUE INDEX index_Name ON Card ( Name )
INSERT INTO Card (Name, CatID, CardTypeId) VALUES ('Yavimaya Coast', 53352, (SELECT Id FROM CardType WHERE Name = 'Land'))
INSERT INTO Card (Name, CatID, CardTypeId) VALUES ('Abattoir Ghoul', 42418, (SELECT Id FROM CardType WHERE Name = 'Creature'))


<Cards CatID="53352" Quantity="3" Sideboard="false" Name="Yavimaya Coast" />
<Cards CatID="42418" Quantity="4" Sideboard="false" Name="Abattoir Ghoul" />

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

select * from Itemtype
select * from Solution

select i.name, s.name
from solution s
join itemtype i on i.id = s.itemtypeid

* SQL SERVER *
CREATE TABLE dbo.Itemtype (
	Id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	Name VARCHAR(256) NOT NULL
)
CREATE TABLE dbo.Solution (
	Id INTEGER NOT NULL PRIMARY KEY  IDENTITY(1,1),
	Parent INTEGER NOT NULL,
	Level INTEGER NOT NULL,
	Name VARCHAR(256) NOT NULL,
	Itemtypeid INTEGER NOT NULL, FOREIGN KEY (Itemtypeid) REFERENCES Itemtype(Id)
)


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