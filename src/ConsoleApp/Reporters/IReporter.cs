using System.IO;

namespace Burrich.ConsoleApp.Reporters
{
    public interface IReporter
    {
        void Init(string machineName);
        void StartFolder(string name, string gitRemoteOriginUrl = null, bool? hasLocalChanges = null);
        void AddFile(FileInfo fi);
        void EndFolder();
    }
}
