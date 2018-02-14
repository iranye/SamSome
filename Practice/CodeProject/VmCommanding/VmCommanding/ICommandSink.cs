using System.Windows.Input;

namespace VmCommanding
{
    public interface ICommandSink
    {
        bool CanExecuteCommand(ICommand command, object parameter, out bool handled);
        void ExecuteCommand(ICommand command, object parameter, out bool handled);
    }
}
