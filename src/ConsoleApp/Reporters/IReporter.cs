using System.IO;

namespace Burrich.ConsoleApp.Reporters
{
    public interface IReporter
    {
        void Init();
        void StartFolder(string name);
        void AddFile(FileInfo fi);
        void EndFolder();
    }
}
