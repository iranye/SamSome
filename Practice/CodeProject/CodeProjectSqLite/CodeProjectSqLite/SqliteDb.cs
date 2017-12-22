using MicroMvvm;
using System;
using System.Data.SQLite;
using System.Text;

namespace CodeProjectSqLite
{
    class SqliteDb : ObservableObject
    {
        private string mStatus = string.Empty;
        public string Status
        {
            get { return mStatus; }
            set
            {
                if (mStatus != value)
                {
                    mStatus = value;
                    NotifyPropertyChanged("Status");
                }
            }
        }

        const int maxCommandLines = 30;
        public string[] CommandLines = null;

        private string mCommandBlob = string.Empty;
        public string CommandBlob
        {
            get { return mCommandBlob; }
            set
            {
                if (mCommandBlob != value)
                {
                    mCommandBlob = value;

                    if (!String.IsNullOrEmpty(mCommandBlob))
                    {
                        CommandLines = mCommandBlob.Split('\n');
                    }
                    NotifyPropertyChanged("CommandBlob");
                }
            }
        }

        private string mSqlQueryCommand = string.Empty;
        public string SqlQueryCommand
        {
            get { return mSqlQueryCommand; }
            set
            {
                if (mSqlQueryCommand != value)
                {
                    mSqlQueryCommand = value;
                    NotifyPropertyChanged("SqlQueryCommand");
                }
            }
        }

        const string connectionStringToFile = "Data Source=database.sqlite;Version=3;New=True;Compress=True;";
        const string connectionStringToMemory = "Data Source=:memory:;Version=3;";

        // We use these three SQLite objects:
        SQLiteConnection sqlite_conn;          // Database Connection Object
        SQLiteCommand sqlite_cmd;             // Database Command Object
        SQLiteDataReader sqlite_datareader;  // Data Reader Object

        private bool mConnectToMemoryDb;
        public bool ConnectToMemoryDb
        {
            get { return mConnectToMemoryDb; }
            set
            {
                if (mConnectToMemoryDb != value)
                {
                    mConnectToMemoryDb = value;
                    NotifyPropertyChanged("ConnectToMemoryDb");
                }
            }
        }

        private SQLiteConnection mSQLiteConnectionToFile = null;
        public SQLiteConnection ConnectionToFile
        {
            get
            {
                if (mSQLiteConnectionToFile == null)
                {
                    mSQLiteConnectionToFile = new SQLiteConnection(connectionStringToFile);
                }
                return mSQLiteConnectionToFile;
            }
        }

        private SQLiteConnection mSQLiteConnectionToMemory = null;
        public SQLiteConnection ConnectionToMemory
        {
            get
            {
                if (mSQLiteConnectionToMemory == null)
                {
                    mSQLiteConnectionToMemory = new SQLiteConnection(connectionStringToMemory);
                }
                return mSQLiteConnectionToMemory;
            }
        }

        public void ExecuteNonQueries()
        {
            if (CommandLines == null || CommandLines.Length == 0)
            {
                throw new ArgumentNullException("CommandLines");
            }

            SQLiteConnection connection = ConnectToMemoryDb ? ConnectionToMemory : ConnectionToFile;
            SQLiteCommand sqliteCommand = null;
            try
            {
                connection.Open();
                sqliteCommand = connection.CreateCommand();
                int rowsAffected = 0;
                foreach (var command in CommandLines)
                {
                    if (!String.IsNullOrWhiteSpace(command))
                    {
                        sqliteCommand.CommandText = command;
                        rowsAffected += sqliteCommand.ExecuteNonQuery();
                    }
                }
                Status = $"{rowsAffected} Row(s) Affected";
            }
            catch (Exception ex)
            {
                Status = ex.Message;
            }
            finally
            {
                connection.Close();
            }
        }

        public void ExecuteReaderQuery()
        {
            if (String.IsNullOrEmpty(SqlQueryCommand))
            {
                throw new ArgumentNullException("SqlQueryCommand");
            }

            SQLiteConnection connection = ConnectToMemoryDb ? ConnectionToMemory : ConnectionToFile;
            SQLiteCommand sqliteCommand = null;
            try
            {
                connection.Open();
                sqliteCommand = connection.CreateCommand();
                sqliteCommand.CommandText = SqlQueryCommand;
                SQLiteDataReader dataReader = sqliteCommand.ExecuteReader();

                StringBuilder sb = new StringBuilder();
                while(dataReader.Read())
                {
                    object idReader = dataReader.GetValue(0);
                    string textReader = dataReader.GetString(1);
                    sb.AppendLine(idReader + " '" + textReader + "'");
                }
                Status = sb.ToString();
            }
            catch (Exception ex)
            {
                Status = ex.Message;
            }
            finally
            {
                connection.Close();
            }
        }        
    }
}
