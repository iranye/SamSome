using MicroMvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncInCsharp5_0
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
