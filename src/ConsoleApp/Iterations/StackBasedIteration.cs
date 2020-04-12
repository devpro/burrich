using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Burrich.ConsoleApp.Reporters;
using Microsoft.Extensions.Logging;

namespace Burrich.ConsoleApp.Iterations
{
    public class StackBasedIteration
    {
        private readonly ILogger<StackBasedIteration> _logger;

        private readonly IReporter _reporter;

        private readonly List<string> _excludes;

        public StackBasedIteration(ILogger<StackBasedIteration> logger, IReporter reporter, List<string> excludes)
        {
            _logger = logger;
            _reporter = reporter;
            _excludes = excludes;
        }

        public void TraverseTree(string root)
        {
            if (string.IsNullOrEmpty(root) || !Directory.Exists(root))
            {
                throw new ArgumentException($"Root folder \"{root}\" doesn't exist", nameof(root));
            }

            _logger.LogInformation($"Starting to traverse tree [RootFolder={root}] [Excludes={string.Join(";", _excludes)}]");

            _reporter.Init(Environment.MachineName);

            // will hold names of subfolders to be examined for files
            var dirs = new Stack<string>(20);

            dirs.Push(root);

            while (dirs.Count > 0)
            {
                var currentDir = dirs.Pop();

                if (_excludes.Contains(Path.GetFileName(currentDir)))
                {
                    continue;
                }

                ProcessDirectories(currentDir, ref dirs);
            }
        }

        private string ExecuteCommandLine(string workingDirectory, string executable, string arguments)
        {
            using var p = new Process();
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.FileName = executable;
            p.StartInfo.Arguments = arguments;
            p.StartInfo.WorkingDirectory = workingDirectory;
            p.Start();
            var output = p.StandardOutput.ReadToEnd();
            p.WaitForExit();
            return output;
        }

        private bool GetDirectories(string currentDir, out string[] subDirs)
        {
            try
            {
                subDirs = Directory.GetDirectories(currentDir);
                return true;
            }
            catch (UnauthorizedAccessException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (DirectoryNotFoundException e)
            {
                Console.WriteLine(e.Message);
            }

            subDirs = null;
            return false;
        }

        private bool GetFiles(string currentDir, out string[] files)
        {
            try
            {
                files = Directory.GetFiles(currentDir);
                return true;
            }
            catch (UnauthorizedAccessException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (DirectoryNotFoundException e)
            {
                Console.WriteLine(e.Message);
            }

            files = null;
            return false;
        }

        private void ProcessDirectories(string currentDir, ref Stack<string> dirs)
        {
            if (!GetDirectories(currentDir, out var subDirs))
            {
                return;
            }

            // check git repository
            if (subDirs.Contains(Path.Combine(currentDir, ".git")))
            {
                var gitStatus = ExecuteCommandLine(currentDir, "git", "status -s");
                if (!string.IsNullOrEmpty(gitStatus))
                {
                    _logger.LogWarning($"Directory \"{currentDir}\" has uncommitted changes");
                    _logger.LogDebug(gitStatus);
                }
                var gitRemoteOriginUrl = ExecuteCommandLine(currentDir, "git", "remote get-url origin")
                    .Replace("\t", "")
                    .Replace("\n", "")
                    .Trim();
                _reporter.StartFolder(currentDir, gitRemoteOriginUrl, !string.IsNullOrEmpty(gitStatus));
                _reporter.EndFolder();
                return;
            }

            foreach (var str in subDirs)
            {
                dirs.Push(str);
            }

            _reporter.StartFolder(currentDir);

            ProcessFiles(currentDir);

            _reporter.EndFolder();
        }

        private void ProcessFiles(string currentDir)
        {
            if (!GetFiles(currentDir, out var files))
            {
                return;
            }

            // performs the required action on each file
            FileInfo fileInfo;
            foreach (var file in files)
            {
                try
                {
                    fileInfo = new FileInfo(file);
                    if (_excludes.Contains(fileInfo.Name))
                    {
                        continue;
                    }

                    _reporter.AddFile(fileInfo);
                }
                catch (FileNotFoundException e)
                {
                    // if the file was deleted by a separate application or thread since the call to TraverseTree() then just continue.
                    Console.WriteLine(e.Message);
                }
            }
        }
    }
}
