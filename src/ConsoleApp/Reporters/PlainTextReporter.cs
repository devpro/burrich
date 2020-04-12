using System.IO;
using System.Text;

namespace Burrich.ConsoleApp.Reporters
{
    public class PlainTextReporter : IReporter
    {
        private readonly string _outputFilepath;

        private readonly StringBuilder _stringBuffer;

        public PlainTextReporter(string outputFilepath)
        {
            _outputFilepath = outputFilepath;
            _stringBuffer = new StringBuilder();
        }

        public void Init(string machineName)
        {
            File.Create(_outputFilepath).Dispose();
            _stringBuffer.Clear();
            _stringBuffer.AppendLine($"TYPE;NAME;LENGTH;CREATEDAT;GITREMOTEURL;HASLOCALCHANGES;");
            _stringBuffer.AppendLine($"m;{machineName};;;;;");
            File.AppendAllText(_outputFilepath, _stringBuffer.ToString());
        }

        public void StartFolder(string name, string gitRemoteOriginUrl = null, bool? hasLocalChanges = null)
        {
            _stringBuffer.Clear();
            _stringBuffer.AppendLine($"f;{name};;;{gitRemoteOriginUrl ?? ""};{(hasLocalChanges.HasValue ? hasLocalChanges.Value.ToString() : "")};");
        }

        public void AddFile(FileInfo fileInfo)
        {
            _stringBuffer.AppendLine($"d;{fileInfo.FullName};{fileInfo.Length};{fileInfo.CreationTime:yyyy-MM-dd};;;");
        }

        public void EndFolder()
        {
            File.AppendAllText(_outputFilepath, _stringBuffer.ToString());
        }
    }
}
