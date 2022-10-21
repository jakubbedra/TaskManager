namespace TaskManager.ViewModel.Commands;

public class ToggleAutoRefreshCommand : CommandBase
{
    public ToggleAutoRefreshCommand(ProcessListViewModel processListViewModel) : base(processListViewModel)
    { }

    public override void Execute(object? parameter)
    {
        _processListViewModel.ToggleAutoRefresh();
    }
}