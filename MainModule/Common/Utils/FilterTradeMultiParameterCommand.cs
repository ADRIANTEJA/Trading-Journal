
using System.Windows.Input;

namespace MainModule.Common.Utils;

public class FilterTradeMultiParameterCommand : IMultiParameterCommand
{
    private readonly Action<object, object> _execute;
    private readonly Func<object, object, bool> _canExecute;

    public FilterTradeMultiParameterCommand(Action<object, object> execute) : this(execute, null) { }

    public FilterTradeMultiParameterCommand(Action<object, object> execute, Func<object, object, bool> canExecute)
    {
        _execute = execute;
        _canExecute = canExecute;
    }

    public event EventHandler CanExecuteChanged
    {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }

    public bool CanExecute(object? parameter1, object? parameter2)
    {
        return _canExecute == null ? true : _canExecute(parameter1, parameter2);
    }

    public void Execute(object? parameter1, object? parameter2)
    {
        _execute(parameter1, parameter2);
    }
}
