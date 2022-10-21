namespace TaskManager.ViewModel.Commands;

public class SortListViewCommand : CommandBase
{
    public SortListViewCommand(ProcessListViewModel processListViewModel) : base(processListViewModel)
    { }

    public override void Execute(object? parameter)
    {
        if (parameter is string name)
        {
            _processListViewModel.SortProcesses(name);
        }
    }

}