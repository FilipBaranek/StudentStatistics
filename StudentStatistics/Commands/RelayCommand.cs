namespace StudentStatistics.Commands
{
    public class RelayCommand : RelayCommandBase
    {
        private readonly Action _execute;

        public RelayCommand(Action execute)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
        }

        public override void Execute(object? parameter) => _execute();

    }

    public class RelayCommand<T> : RelayCommandBase
    {
        private readonly Action<T> _execute;

        public RelayCommand(Action<T> execute)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
        }

        public override void Execute(object? parameter)
        {
            if (parameter is T castedParam)
            {
                _execute(castedParam);
            }
            else
            {
                throw new ArgumentException("Invalid parameter type");
            }
        }
    }
}
