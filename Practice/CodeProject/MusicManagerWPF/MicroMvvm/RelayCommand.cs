using System;
using System.Windows.Input;

namespace MicroMvvm
{
    /// <summary>
    /// A command whose sole purpose is to relay its functionality to other objects by invoking delegates. The default return value for the CanExecute method is 'true'.
    /// </summary>
    public class RelayCommand : ICommand
    {
        Action mExecute;
        Func<Boolean> mCanExecute;

        public RelayCommand(Action execute)
            : this(execute, null)
        {}
        
        public event EventHandler CanExecuteChanged
        {
            add
            {
                if (mCanExecute != null)
                    CommandManager.RequerySuggested += value;
            }
            remove
            {
                if (mCanExecute != null)
                    CommandManager.RequerySuggested -= value;
            }
        }

        public RelayCommand(Action execute, Func<Boolean> canExecute)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");
            mExecute = execute;
            mCanExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return mCanExecute == null || mCanExecute();
        }

        public void Execute(object parameter)
        {
            mExecute();
        }
    }
}
