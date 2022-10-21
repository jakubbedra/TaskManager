using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;

namespace TaskManager.ViewModel.Helper;

public class ProcessProvider
{
    public static List<Process> CreateProcessList()
    {
        var allProcesses = Process.GetProcesses();
        List<Process> processes = new List<Process>();
        foreach (var process in allProcesses)
        {
            processes.Add(process);
        }

        return processes;
    }

    public static List<Process> CreateProcessChildrenList(Process process)
    {
        var query = "Select * From Win32_Process Where ParentProcessId = " + process.Id;
        ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);
        ManagementObjectCollection processList = searcher.Get();

        var result = processList.Cast<ManagementObject>().Select(p =>
            Process.GetProcessById(Convert.ToInt32(p.GetPropertyValue("ProcessId")))
        ).ToList();

        return result;
    }

}
