namespace TaskManager.ViewModel.Commands;

public class RefreshProcessesCommand : CommandBase
{
    public RefreshProcessesCommand(ProcessListViewModel processListViewModel) : base(processListViewModel)
    { }

    public override void Execute(object? parameter)
    {
        _processListViewModel.RefreshProcesses();
    }

}