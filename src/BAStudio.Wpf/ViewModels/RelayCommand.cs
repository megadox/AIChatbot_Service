using System.Windows.Input;

namespace BAStudio.Wpf.ViewModels;

/// <summary>
/// Implements a simple WPF command wrapper for synchronous or asynchronous actions.
/// </summary>
public sealed class RelayCommand : ICommand
{
    private readonly Func<Task>? _executeAsync;
    private readonly Action? _execute;
    private readonly Func<bool>? _canExecute;

    /// <summary>
    /// Creates a command that runs an asynchronous action.
    /// </summary>
    public RelayCommand(Func<Task> executeAsync, Func<bool>? canExecute = null)
    {
        _executeAsync = executeAsync;
        _canExecute = canExecute;
    }

    /// <summary>
    /// Creates a command that runs a synchronous action.
    /// </summary>
    public RelayCommand(Action execute, Func<bool>? canExecute = null)
    {
        _execute = execute;
        _canExecute = canExecute;
    }

    public event EventHandler? CanExecuteChanged;

    /// <summary>
    /// Returns whether the command can currently execute.
    /// </summary>
    public bool CanExecute(object? parameter) => _canExecute?.Invoke() ?? true;

    /// <summary>
    /// Executes the configured action.
    /// </summary>
    public async void Execute(object? parameter)
    {
        if (_executeAsync is not null)
        {
            await _executeAsync();
            return;
        }

        _execute?.Invoke();
    }

    /// <summary>
    /// Notifies WPF that the command enabled state may have changed.
    /// </summary>
    public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
}
