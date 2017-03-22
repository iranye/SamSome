using System;
using System.Diagnostics;
using System.Windows.Input;

namespace MicroMvvm
{
    public class RelayCommand : ICommand
    {
        readonly Func<Boolean> mCanExecute;
        readonly Action mExecute;

        public RelayCommand(Action execute)
            : this(execute, null)
        {
        }

        public RelayCommand(Action execute, Func<Boolean> canExecute)
        {
            if (execute == null)
            {
                throw new ArgumentNullException("execute");
            }
            mExecute = execute;
            mCanExecute = canExecute;
        }

        #region ICommand Members
        public event EventHandler CanExecuteChanged
        {
            add
            {
                if (mCanExecute != null)
                {
                    CommandManager.RequerySuggested += value;
                }
            }
            remove
            {
                if (mCanExecute != null)
                {
                    CommandManager.RequerySuggested -= value;
                }
            }
        }

        [DebuggerStepThrough]
        public bool CanExecute(object parameter)
        {
            return mCanExecute == null ? true : mCanExecute();
        }

        public void Execute(object parameter)
        {
            mExecute();
        }
        #endregion
    }
}
