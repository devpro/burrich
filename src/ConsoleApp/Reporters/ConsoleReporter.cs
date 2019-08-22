using System;
using System.IO;

namespace Burrich.ConsoleApp.Reporters
{
    public class ConsoleReporter : IReporter
    {
        public void Init(string machineName)
        {
            Console.WriteLine($"Machine {machineName}");
        }

        public void StartFolder(string name, string gitRemoteOriginUrl = null, bool? hasLocalChanges = null)
        {
            if (string.IsNullOrEmpty(gitRemoteOriginUrl))
            {
                Console.WriteLine($"Folder {name}");
            }
            else if (hasLocalChanges.HasValue && hasLocalChanges.Value)
            {
                Console.WriteLine($"Folder {name} mapped on {gitRemoteOriginUrl} with local changes");
            }
            else
            {
                Console.WriteLine($"Folder {name} mapped on {gitRemoteOriginUrl} with no local changes");
            }
        }

        public void AddFile(FileInfo fi)
        {
            Console.WriteLine($"{fi.FullName}: {fi.Length.ToString()}, {fi.CreationTime.ToString("yyyy-MM-dd")}");
        }

        public void EndFolder()
        {
        }
    }
}
