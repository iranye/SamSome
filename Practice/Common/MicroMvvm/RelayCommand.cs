using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MicroMvvm
{
    public class RelayCommand : ICommand
    {
        private readonly Action mExecute;
        private Func<bool> mCanExecute;

        public RelayCommand(Action execute) : this(execute, null)
        {}

        public RelayCommand(Action execute, Func<bool> canExecute)
        {
            if (execute == null)
            {
                throw new ArgumentNullException("execute");
            }
            mExecute = execute;
            mCanExecute = canExecute;
        }

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

        public bool CanExecute(object parameter)
        {
            return mCanExecute == null ? true : mCanExecute();
        }

        public void Execute(object parameter)
        {
            mExecute();
        }

    }
}
