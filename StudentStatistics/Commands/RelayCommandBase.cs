using System.Windows.Input;

namespace StudentStatistics.Commands
{
    public abstract class RelayCommandBase : ICommand
    {
        public abstract void Execute(object? parameter);
        public virtual bool CanExecute(object? parameter) => true;

        public event EventHandler? CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
    }
}
