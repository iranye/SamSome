using MicroMvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CodeProjectSqLite
{
    class DbViewModel
    {
        public SqliteDb SqliteDb { get; set; }
        public StatusViewModel StatusViewModel { get; set; }

        public DbViewModel()
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

        void ClearSqlCommandBlobExecute()
        {
            SqliteDb.CommandBlob = null;
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
