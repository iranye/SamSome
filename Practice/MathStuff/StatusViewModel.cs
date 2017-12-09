using System;
using MicroMvvm;

namespace MathStuff
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

        public void AddLogMessage(string message, bool appendNewLine=true)
        {
            var rotateOutAfterSoManyLines = "";
            LogMessage += string.Format("{0}{1}", message, appendNewLine ? Environment.NewLine : "");
        }

        public void ClearLog()
        {
            LogMessage = String.Empty;
        }
    }
}
