using System.Windows.Input;

namespace MainModule.Common.Helpers;

public class DelegateCommand : ICommand
{
    private readonly Action<object> _execute;
    private readonly Predicate<object> _canExecute;

    public DelegateCommand(Action<object> execute) : this(execute, null) { }

    public DelegateCommand(Action<object> execute, Predicate<object> canExecute)
    {
        _execute = execute;
        _canExecute = canExecute;
    }

    public event EventHandler CanExecuteChanged
    {
        add => CommandManager.RequerySuggested += value; 
        remove => CommandManager.RequerySuggested -= value;
    }

    public bool CanExecute(object? parameter)
    {
        return _canExecute == null ? true : _canExecute(parameter);
    }

    public void Execute(object? parameter)
    {
        _execute(parameter);
    }
}
