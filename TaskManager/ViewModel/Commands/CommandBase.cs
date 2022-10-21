using System;
using System.Windows.Input;

namespace TaskManager.ViewModel.Commands;

public abstract class CommandBase : ICommand
{
    protected readonly ProcessListViewModel _processListViewModel;

    public CommandBase(ProcessListViewModel processListViewModel)
    {
        _processListViewModel = processListViewModel;
    }

    public virtual bool CanExecute(object? parameter)
    {
        return true;
    }
    
    public abstract void Execute(object? parameter);

    public event EventHandler? CanExecuteChanged;
}