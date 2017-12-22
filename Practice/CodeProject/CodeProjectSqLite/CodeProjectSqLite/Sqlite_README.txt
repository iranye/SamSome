https://www.codeproject.com/Articles/1210189/Using-SQLite-in-Csharp-VB-Net

CREATE TABLE test (id integer primary key, text varchar(100))
INSERT INTO test (id, text) VALUES (1, 'Test Text 1')
INSERT INTO test (id, text) VALUES (2, 'Test Text 2')

INSERT INTO test (text) VALUES ('Test Text 4')

SELECT * FROM test

DELETE test WHERE id = 2

CREATE TABLE IF NOT EXISTS
  [Mytable] (
  [Id]     INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
  [NAME]   NVARCHAR(2048) NULL)";

* Foreign Keys *
connectString.ForeignKeys = true;

string createQuery =
    @"CREATE TABLE IF NOT EXISTS
        [itemtype] (
        [id]           INTEGER      NOT NULL PRIMARY KEY,
        [name]         VARCHAR(256) NOT NULL
        )";

using (SQLiteCommand cmd = new SQLiteCommand(db.Connection))
{
    cmd.CommandText = createQuery;
    cmd.ExecuteNonQuery();
}

createQuery =
    @"CREATE TABLE IF NOT EXISTS
        [solution] (
        [id]           INTEGER      NOT NULL PRIMARY KEY,
        [parent]       INTEGER      NOT NULL,
        [level]        INTEGER      NOT NULL,
        [name]         VARCHAR(256) NOT NULL,
        [itemtypeid]   INTEGER      NOT NULL,
        FOREIGN KEY (itemtypeid) REFERENCES itemtype(id)
        )";

using (SQLiteCommand cmd = new SQLiteCommand(db.Connection))
{
    cmd.CommandText = createQuery;
    cmd.ExecuteNonQuery();
}