using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using TaskManager.ViewModel.Commands;
using TaskManager.ViewModel.Helper;

namespace TaskManager.ViewModel;

public class ProcessListViewModel : INotifyPropertyChanged
{
    public SelectedProcessViewModel SelectedProcess { get; set; }

    public ObservableCollection<Process> Processes { get; set; }

    public string ProcessFilter { get; set; }

    private bool _shouldAutoRefresh;

    public bool ShouldAutoRefresh
    {
        get { return _shouldAutoRefresh; }
        set
        {
            _shouldAutoRefresh = value;
            OnPropertyChanged(nameof(ShouldAutoRefresh));
        }
    }

    public int RefreshRateSeconds { get; set; }

    private CollectionView _collectionView;

    public CollectionView CollectionView
    {
        get
        {
            _collectionView = (CollectionView)CollectionViewSource.GetDefaultView(Processes);
            return _collectionView;
        }
    }

    public ICommand SortListViewCommand { get; set; }

    public ICommand RefreshProcessesCommand { get; set; }

    public ICommand ToggleAutoRefreshCommand { get; set; }

    private Dictionary<string, ListSortDirection> _sortDirections;

    public ProcessListViewModel()
    {
        SelectedProcess = new SelectedProcessViewModel();
        ProcessFilter = string.Empty;
        SortListViewCommand = new SortListViewCommand(this);
        RefreshProcessesCommand = new RefreshProcessesCommand(this);
        ToggleAutoRefreshCommand = new ToggleAutoRefreshCommand(this);
        Processes = new ObservableCollection<Process>(ProcessProvider.CreateProcessList());
        CollectionView.Filter = FilterProcesses;
        ShouldAutoRefresh = true;
        RefreshRateSeconds = 1;
        PeriodicRefresh();
    }

    private bool FilterProcesses(object obj)
    {
        if (obj is Process process)
        {
            return process.ProcessName.Contains(ProcessFilter, StringComparison.InvariantCultureIgnoreCase);
        }

        return false;
    }

    private void PeriodicRefresh()
    {
        Action f = async () =>
        {
            while (true)
            {
                if (ShouldAutoRefresh)
                {
                    RefreshProcesses();
                }

                for (int i = 0; i < RefreshRateSeconds; i++)
                    await Task.Delay(1000);
            }
        };
        f();
    }

    public void RefreshProcesses()
    {
        List<Process> processList = ProcessProvider.CreateProcessList();
        Processes.Clear();
        foreach (Process process in processList)
        {
            Processes.Add(process);
        }
        SelectedProcess.Refresh();
    }

    public void SortProcesses(string columnName)
    {
        SortDescription newSortDescription = new SortDescription(columnName, ListSortDirection.Ascending);
        ;
        if (CollectionView.SortDescriptions.Count > 0)
        {
            SortDescription oldSortDescription = CollectionView.SortDescriptions[0];
            if (oldSortDescription.PropertyName.Equals(columnName))
            {
                newSortDescription = new SortDescription(
                    columnName,
                    oldSortDescription.Direction == ListSortDirection.Ascending
                        ? ListSortDirection.Descending
                        : ListSortDirection.Ascending
                );
            }
        }

        CollectionView.SortDescriptions.Clear();
        CollectionView.SortDescriptions.Add(newSortDescription);
    }

    public void ToggleAutoRefresh()
    {
        ShouldAutoRefresh = !ShouldAutoRefresh;
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged(string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}