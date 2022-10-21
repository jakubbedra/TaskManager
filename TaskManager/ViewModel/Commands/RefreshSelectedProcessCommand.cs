using System.Diagnostics;

namespace TaskManager.ViewModel.Commands;

public class RefreshSelectedProcessCommand : CommandBase
{
    private SelectedProcessViewModel _selectedProcessViewModel;

    public RefreshSelectedProcessCommand(
        ProcessListViewModel processListViewModel,
        SelectedProcessViewModel selectedProcessViewModel
    ) : base(processListViewModel)
    {
        _selectedProcessViewModel = selectedProcessViewModel;
    }

    public override void Execute(object? parameter)
    {
        if (parameter is Process process)
        {
            _selectedProcessViewModel.Parent = process;
        }

        _selectedProcessViewModel.Refresh();
    }
}