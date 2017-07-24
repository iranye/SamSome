using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MusicManager
{
    class RelayCommand : ICommand
    {
        readonly Func<Boolean> mCanExecute;
        readonly Action mExecute;

        public RelayCommand(Action execute) : this(execute, null)
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

        public Boolean CanExecute(Object parameter)
        {
            return mCanExecute == null ? true : mCanExecute();
        }

        public void Execute(Object parameter)
        {
            mExecute();
        }
    }
}
