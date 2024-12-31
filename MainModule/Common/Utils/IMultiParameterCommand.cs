namespace MainModule.Common.Utils;

public interface IMultiParameterCommand
{
    event EventHandler CanExecuteChanged;
    bool CanExecute(object? parameter1, object? parameter2);
    void Execute(object? parameter1, object? parameter2);
}
