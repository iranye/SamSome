using MicroMvvm;
using System;

namespace CodeProjectSqLite
{
    class StatusViewModel : ObservableObject
    {
        private string mLogMessage = String.Empty;
        public string LogMessage
        {
            get { return mLogMessage; }
            private set
            {
                if (mLogMessage != value)
                {
                    mLogMessage = value;
                    NotifyPropertyChanged("LogMessage");
                }
            }
        }

        public void AddLogMessage(string message)
        {
            var rotateOutAfterSoManyLines = "";
            LogMessage += $"{message} {Environment.NewLine}";
        }

        public void ClearLog()
        {
            LogMessage = String.Empty;
        }
    }
}
