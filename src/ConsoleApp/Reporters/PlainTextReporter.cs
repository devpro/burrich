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
            _stringBuffer.AppendLine($"m;{machineName};;;");
            File.AppendAllText(_outputFilepath, _stringBuffer.ToString());
        }

        public void StartFolder(string name)
        {
            _stringBuffer.Clear();
            _stringBuffer.AppendLine($"f;{name};;;");
        }

        public void AddFile(FileInfo fi)
        {
            _stringBuffer.AppendLine($"d;{fi.FullName};{fi.Length.ToString()};{fi.CreationTime.ToString("yyyy-MM-dd")};");
        }

        public void EndFolder()
        {
            File.AppendAllText(_outputFilepath, _stringBuffer.ToString());
        }
    }
}
