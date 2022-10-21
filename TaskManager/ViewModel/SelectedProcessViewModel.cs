using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using TaskManager.ViewModel.Helper;

namespace TaskManager.ViewModel;

public class SelectedProcessViewModel : INotifyPropertyChanged
{
    private Process? _parent;

    public Process Parent
    {
        get { return _parent; }
        set
        {
            _parent = value;
            OnPropertyChanged(nameof(Parent));
        }
    }

    private ObservableCollection<Process> _children;

    public ObservableCollection<Process> Children
    {
        get { return _children; }
        set
        {
            _children = value;
            OnPropertyChanged(nameof(Children));
        }
    }

    public SelectedProcessViewModel()
    {
        _parent = null;
        _children = new ObservableCollection<Process>();
    }

    public void Refresh()
    {
        if (_parent != null)
        {
            _parent = Process.GetProcessById(_parent.Id);
            List<Process> childrenList = ProcessProvider.CreateProcessChildrenList(_parent);
            foreach (Process child in childrenList)
            {
                Children.Add(child);
            }
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged(string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}