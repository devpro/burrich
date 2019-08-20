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

        public void StartFolder(string name)
        {
            Console.WriteLine($"Folder {name}");
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
