using System.Windows.Input;

public class RelayCommand : ICommand
{
    private readonly Func<Task> _execute;
    private readonly Func<bool> _canExecute;

    public RelayCommand(Func<Task> execute) : this(execute, null) { }

    public RelayCommand(Func<Task> execute, Func<bool> canExecute = null)
    {
        _execute = execute ?? throw new ArgumentNullException(nameof(execute));
        _canExecute = canExecute;
    }

    public event EventHandler CanExecuteChanged
    {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }

    public bool CanExecute(object parameter) => _canExecute?.Invoke() ?? true;

    public async void Execute(object parameter)
    {
        if (_execute != null)
        {
            await _execute();
        }
    }
    public void RaiseCanExecuteChanged() => CommandManager.InvalidateRequerySuggested();
}