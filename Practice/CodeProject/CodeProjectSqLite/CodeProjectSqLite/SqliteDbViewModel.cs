using MicroMvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Gat.Controls;

namespace CodeProjectSqLite
{
    class SqliteDbViewModel
    {
        public SqliteDb SqliteDb { get; set; }
        public StatusViewModel StatusViewModel { get; set; }

        public SqliteDbViewModel()
        {
            StatusViewModel = new StatusViewModel();
            SqliteDb = new SqliteDb();
            SqliteDb.PropertyChanged += SqliteDb_PropertyChanged;

            Version version = Assembly.GetExecutingAssembly().GetName().Version;
            Version = version.ToString();
        }

        private void SqliteDb_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "Status":
                    StatusViewModel.AddLogMessage($"{SqliteDb.Status}");
                    break;
                default:
                    //StatusViewModel.AddLogMessage($"No handler for 'ListCollection.{e.PropertyName}'");
                    break;
            }
        }

        #region PickFile
        void PickFileExecute()
        {
            ClearLogMessagesExecute();
            if (SqliteDb.ConnectToMemoryDb)
            {
                StatusViewModel.AddLogMessage("ConnectToMemoryDb is set to true");
                return;
            }

            // https://opendialog.codeplex.com/documentation
            var openDialog = new Gat.Controls.OpenDialogView();
            var vm = openDialog.DataContext as OpenDialogViewModel;
            if (vm == null)
            {
                StatusViewModel.AddLogMessage("Failed to initialize open folder dialog");
                return;
            }
            bool? result = vm.Show();
            if (result == true)
            {
                // Get selected path
                if (String.IsNullOrEmpty(vm.SelectedFilePath))
                {
                    StatusViewModel.AddLogMessage("Invalid file path");
                }
                else
                {
                    try
                    {
                        SqliteDb.DbFileName = vm.SelectedFilePath;
                    }
                    catch (Exception ex)
                    {
                        StatusViewModel.AddLogMessage($"Error reading file path '{vm.SelectedFilePath}'{Environment.NewLine}{ex.Message}");
                    }
                }
            }
        }

        bool CanPickFileExecute()
        {
            return true;
        }

        public ICommand PickFile
        {
            get { return new RelayCommand(PickFileExecute, CanPickFileExecute); }
        }
        #endregion

        void DbConnectExecute()
        {
            ClearLogMessagesExecute();
            SqliteDb.OpenDbConnection();
        }

        bool CanDbConnectExecute()
        {
            return true;
        }

        public ICommand DbConnect
        {
            get { return new RelayCommand(DbConnectExecute, CanDbConnectExecute); }
        }

        void DbCloseConnectionExecute()
        {
            ClearLogMessagesExecute();
            SqliteDb.CloseConnection();
        }

        bool CanDbCloseConnectionExecute()
        {
            return true;
        }

        public ICommand DbCloseConnection
        {
            get { return new RelayCommand(DbCloseConnectionExecute, CanDbCloseConnectionExecute); }
        }

        void ClearSqlCommandBlobExecute()
        {
            ClearLogMessagesExecute();
            SqliteDb.CommandBlob = String.Empty;
        }

        bool CanClearSqlCommandBlobExecute()
        {
            return true;
        }

        public ICommand ClearSqlCommandBlob
        {
            get { return new RelayCommand(ClearSqlCommandBlobExecute, CanClearSqlCommandBlobExecute); }
        }
                
        void ExecuteNonQueriesExecute()
        {
            ClearLogMessagesExecute();
            SqliteDb.ExecuteNonQueries();
        }

        bool CanExecuteNonQueriesExecute()
        {
            return true;
        }

        public ICommand ExecuteNonQueries
        {
            get { return new RelayCommand(ExecuteNonQueriesExecute, CanExecuteNonQueriesExecute); }
        }

        void ExecuteReaderQueryExecute()
        {
            ClearLogMessagesExecute();
            SqliteDb.ExecuteReaderQuery();
        }

        bool CanExecuteReaderQueryExecute()
        {
            return true;
        }

        public ICommand ExecuteReaderQuery
        {
            get { return new RelayCommand(ExecuteReaderQueryExecute, CanExecuteReaderQueryExecute); }
        }

        void QueryTablesExecute()
        {
            ClearLogMessagesExecute();
            StatusViewModel.AddLogMessage("Loading Tables datagrid!");
            SqliteDb.QueryForTables();
        }

        bool CanQueryTablesExecute()
        {
            return true;
        }

        public ICommand QueryTables
        {
            get { return new RelayCommand(QueryTablesExecute, CanQueryTablesExecute); }
        }

        void ClearDataSetExecute()
        {
            StatusViewModel.AddLogMessage("Clearing Tables datagrid!");
        }

        bool CanClearDataSetExecute()
        {
            return true;
        }

        public ICommand ClearDataSet
        {
            get { return new RelayCommand(ClearDataSetExecute, CanClearDataSetExecute);}
        }

        #region ClearLogMessages

        void ClearLogMessagesExecute()
        {
            StatusViewModel.ClearLog();
        }

        bool CanClearLogMessagesExecute()
        {
            return true;
        }

        public ICommand ClearLogMessages
        {
            get { return new RelayCommand(ClearLogMessagesExecute, CanClearLogMessagesExecute); }
        }
        #endregion

        public string Version { get; set; }
    }
}
