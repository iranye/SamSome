using MicroMvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Text;

namespace CodeProjectSqLite
{
    class SqliteDb : ObservableObject
    {
        private string mStatus = string.Empty;
        const string connectionStringToFile = "Data Source=database.sqlite;Version=3;New=True;Compress=True;";
        const string connectionStringToMemory = "Data Source=:memory:;Version=3;";

        // We use these three SQLite objects:
        SQLiteConnection sqlite_conn;          // Database Connection Object
        SQLiteCommand sqlite_cmd;             // Database Command Object
        SQLiteDataReader sqlite_datareader;  // Data Reader Object

        public string Status
        {
            get { return mStatus; }
            set
            {
                mStatus = value;
                NotifyPropertyChanged("Status");
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
                    Status = string.Format("Switching to {0} DB", mConnectToMemoryDb ? "in-memory" : "file");
                }
            }
        }

        private ObservableCollection<String> mTables = new ObservableCollection<string>();
        public ObservableCollection<String> Tables
        {
            get { return mTables; }
            set
            {
                if (mTables != value)
                {
                    mTables = value;
                    NotifyPropertyChanged("Tables");
                }
            }
        }

        private String mSelectedTable = String.Empty;
        public String SelectedTable
        {
            get { return mSelectedTable; }
            set
            {
                if (mSelectedTable != value)
                {
                    mSelectedTable = value;
                    Status = "Table selected! " + mSelectedTable;
                    SelectAllFromTable(mSelectedTable);
                    NotifyPropertyChanged("SelectedTable");
                }
            }
        }

        private DataTable mTableContents = new DataTable();
        public DataTable TableContents
        {
            get { return mTableContents; }
            set
            {
                if (mTableContents != value)
                {
                    mTableContents = value;
                    NotifyPropertyChanged("TableContents");
                }
            }
        }

        private object mSelectedRow = new object();
        public object SelectedRow
        {
            get { return mSelectedRow; }
            set
            {
                if (mSelectedRow != value)
                {
                    mSelectedRow = value;
                    Status = "Row selected! " + mSelectedRow;
                    NotifyPropertyChanged("SelectedRow");
                }
            }
        }
        
        private void SelectAllFromTable(string tableName)
        {
            if (DbConnectionState != ConnectionState.Open)
            {
                Status = "Connection is not open.";
                return;
            }
            
            SqlQueryCommand = $"Select * from {tableName}";
            DataSet ds = new DataSet();

            SQLiteConnection connection = ConnectToMemoryDb ? ConnectionToMemory : ConnectionToFile;
            try
            {
                SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter(SqlQueryCommand, connection);
                dataAdapter.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    TableContents = ds.Tables[0];
                }
            }
            catch (Exception e)
            {
                Status = e.Message;
            }
        }

        private SQLiteConnection mSQLiteConnectionToFile = null;
        public SQLiteConnection ConnectionToFile
        {
            get
            {
                return mSQLiteConnectionToFile;
            }
            private set
            {
                if (mSQLiteConnectionToFile != value)
                {
                    mSQLiteConnectionToFile = value;
                    NotifyPropertyChanged("ConnectionToFile");
                }
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

        //private SQLiteConnection mCurrentDbConnection;
        //public SQLiteConnection CurrentDbConnection
        //{
        //    get { return ConnectToMemoryDb ? ConnectionToMemory : ConnectionToFile; }
        //    set
        //    {
        //        if (mCurrentDbConnection != value)
        //        {
        //            mCurrentDbConnection = value;
        //            NotifyPropertyChanged("CurrentDbConnection");
        //        }
        //    }
        //}
        
        public void OpenDbConnection()
        {
            try
            {
                if (ConnectToMemoryDb)
                {
                    if (ConnectionToMemory.State != System.Data.ConnectionState.Open)
                    {
                        ConnectionToMemory.Open();
                    }
                    Status = "Connection to in-memory DB is Open";
                    return;
                }
                if (ConnectionToFile != null && ConnectionToFile.State == ConnectionState.Open)
                {
                    Status = "Connection to file DB is already Open";
                    return;
                }
                string dbFileFullPath = DbFileFullPath;
                if (String.IsNullOrWhiteSpace(dbFileFullPath))
                {
                    Status = "ERROR: No DB File has been specified.";
                    return;
                }
                if (!Directory.Exists(DbFileInfo.Directory.FullName))
                {
                    Status = $"Invalid directory path '{DbFileInfo.Directory.FullName}'";
                    return;
                }

                SQLiteConnectionStringBuilder connectionStringBuilder = new SQLiteConnectionStringBuilder
                {
                    DataSource = dbFileFullPath,
                    ForeignKeys = true
                };

                Status = $"Connection-to-file DB string:{Environment.NewLine}{connectionStringBuilder}";
                mSQLiteConnectionToFile = new SQLiteConnection(connectionStringBuilder.ToString());

                if (OverwriteDbFile || !File.Exists(dbFileFullPath))
                {
                    // Overwrites a file if it is already there
                    SQLiteConnection.CreateFile(dbFileFullPath);
                }
                else
                {
                    Status = "Using existing DB";
                }

                mSQLiteConnectionToFile.Open();
                Status = "Connection to file DB has been Opened";
                NotifyPropertyChanged("DbConnectionStatus");
            }
            catch (Exception e)
            {
                Status = $"Failed to open connection to db:{Environment.NewLine}{e.Message}";
            }
        }

        public ConnectionState DbConnectionState
        {
            get
            {
                SQLiteConnection connection = ConnectToMemoryDb ? ConnectionToMemory : ConnectionToFile;
                return connection == null ? ConnectionState.Closed : connection.State;
            }
        }

        public string DbConnectionStatus
        {
            get
            {
                SQLiteConnection connection = ConnectToMemoryDb ? ConnectionToMemory : ConnectionToFile;
                return connection == null ? "Closed" : connection.State.ToString();
            }
        }

        public void CloseConnection()
        {
            SQLiteConnection connection = ConnectToMemoryDb ? ConnectionToMemory : ConnectionToFile;
            if (connection == null)
            {
                Status = "Invalid Connection";
                return;
            }
            try
            {
                connection.Close();
            }
            catch (Exception e)
            {
                Status = $"Failed to close connection to db:{Environment.NewLine}{e.Message}";
            }
            finally
            {
                NotifyPropertyChanged("DbConnectionStatus");
            }
        }

        // TBD
        public void ExecuteNonQueries()
        {
            if (CommandLines == null || CommandLines.Length == 0)
            {
                Status ="ERROR: No CommandLine(s)";
                return;
            }

            if (DbConnectionState != ConnectionState.Open)
            {
                Status = "Connection is not yet open.";
                return;
            }

            SQLiteConnection connection = ConnectToMemoryDb ? ConnectionToMemory : ConnectionToFile;
            SQLiteCommand sqliteCommand = null;
            try
            {
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
        }

        public void ExecuteReaderQuery()
        {
            if (String.IsNullOrEmpty(SqlQueryCommand))
            {
                Status = "ERROR: No Query(s)";
            }

            if (DbConnectionState != ConnectionState.Open)
            {
                Status = "Connection is not yet open.";
                return;
            }

            SQLiteConnection connection = ConnectToMemoryDb ? ConnectionToMemory : ConnectionToFile;
            SQLiteCommand sqliteCommand = null;
            try
            {
                sqliteCommand = connection.CreateCommand();
                sqliteCommand.CommandText = SqlQueryCommand;
                SQLiteDataReader dataReader = sqliteCommand.ExecuteReader();

                StringBuilder sb = new StringBuilder();
                while (dataReader.Read())
                {
                    for (int i = 0; i < dataReader.FieldCount; i++)
                    {
                        sb.Append($"'{dataReader.GetValue(i)}', ");
                    }

                    sb.AppendLine("");
                    //object idReader = dataReader.GetValue(0);
                    //string textReader = dataReader.GetString(1);
                    //sb.AppendLine(idReader + " '" + textReader + "'");
                }
                Status = sb.ToString();
            }
            catch (Exception ex)
            {
                Status = ex.Message;
            }
        }
        
        public void QueryForTables()
        {
            if (DbConnectionState != ConnectionState.Open)
            {
                Status = "Connection is not yet open.";
                return;
            }

            DataSet ds = new DataSet();
            SqlQueryCommand = "Select * from sqlite_master;";

            SQLiteConnection connection = ConnectToMemoryDb ? ConnectionToMemory : ConnectionToFile;
            try
            {
                SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter(SqlQueryCommand, connection);
                dataAdapter.Fill(ds);
                DataRowCollection dataRowCollection = ds.Tables[0].Rows;

                foreach (DataRow dr in dataRowCollection)
                {
                    Tables.Add(dr["name"].ToString());
                }
            }
            catch (Exception e)
            {
                Status = e.Message;
            }
        }

        #region DB File
        private FileInfo mDbFileInfo = null;
        public FileInfo DbFileInfo
        {
            get
            {
                return mDbFileInfo;
            }
            set
            {
                if (mDbFileInfo != value)
                {
                    mDbFileInfo = value;
                    NotifyPropertyChanged("DbFileName");
                    NotifyPropertyChanged("DbFileFullPath");
                }
            }
        }

        private bool mOverwriteDbFile;
        public bool OverwriteDbFile
        {
            get { return mOverwriteDbFile; }
            set
            {
                if (mOverwriteDbFile != value)
                {
                    mOverwriteDbFile = value;
                    NotifyPropertyChanged("OverwriteDbFile");
                }
            }
        }

        public string DbFileFullPath => mDbFileInfo?.FullName ?? String.Empty;

        public string DbFileName
        {
            get { return mDbFileInfo?.Name ?? String.Empty; }
            set
            {
                if (!String.IsNullOrWhiteSpace(value) && DbFileFullPath != value)
                {
                    if (DbConnectionState != ConnectionState.Closed)
                    {
                        CloseConnection();
                        ConnectionToFile = null;
                    }
                    DbFileInfo = new FileInfo(value);
                    if (!Directory.Exists(DbFileInfo.Directory.FullName))
                    {
                        Status = $"Invalid directory path '{DbFileInfo.Directory.FullName}'";
                    }
                }
            }
        }
        #endregion
    }
}
