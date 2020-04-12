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
                Console.WriteLine($"{name}");
            }
            else if (hasLocalChanges.HasValue && hasLocalChanges.Value)
            {
                Console.WriteLine($"{name} -> git {gitRemoteOriginUrl} with local changes");
            }
            else
            {
                Console.WriteLine($"{name} -> git {gitRemoteOriginUrl}");
            }
        }

        public void AddFile(FileInfo fi)
        {
            Console.WriteLine($"- {fi.FullName}: {fi.Length}, {fi.CreationTime:yyyy-MM-dd}");
        }

        public void EndFolder()
        {
            // nothing to do here
        }
    }
}
