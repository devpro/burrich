using System;
using System.Collections.Generic;
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
            _logger.LogInformation($"Starting to traverse tree [RootFolder={root}] [Excludes={string.Join(";", _excludes)}]");

            // Data structure to hold names of subfolders to be examined for files.
            var dirs = new Stack<string>(20);

            if (!Directory.Exists(root))
            {
                throw new Exception($"Root folder {root} doesn't exist");
            }

            _reporter.Init(Environment.MachineName);

            dirs.Push(root);

            while (dirs.Count > 0)
            {
                var currentDir = dirs.Pop();

                if (_excludes.Contains(Path.GetFileName(currentDir)))
                {
                    continue;
                }

                _reporter.StartFolder(currentDir);

                string[] subDirs;
                try
                {
                    subDirs = Directory.GetDirectories(currentDir);
                }
                // An UnauthorizedAccessException exception will be thrown if we do not have
                // discovery permission on a folder or file. It may or may not be acceptable
                // to ignore the exception and continue enumerating the remaining files and
                // folders. It is also possible (but unlikely) that a DirectoryNotFound exception
                // will be raised. This will happen if currentDir has been deleted by
                // another application or thread after our call to Directory.Exists. The
                // choice of which exceptions to catch depends entirely on the specific task
                // you are intending to perform and also on how much you know with certainty
                // about the systems on which this code will run.
                catch (UnauthorizedAccessException e)
                {
                    Console.WriteLine(e.Message);
                    continue;
                }
                catch (DirectoryNotFoundException e)
                {
                    Console.WriteLine(e.Message);
                    continue;
                }

                if (subDirs.Contains(Path.Combine(currentDir, ".git")))
                {
                    _reporter.EndFolder();
                    continue;
                }

                string[] files;
                try
                {
                    files = Directory.GetFiles(currentDir);
                }
                catch (UnauthorizedAccessException e)
                {
                    Console.WriteLine(e.Message);
                    continue;
                }
                catch (DirectoryNotFoundException e)
                {
                    Console.WriteLine(e.Message);
                    continue;
                }

                // Perform the required action on each file here.
                foreach (var file in files)
                {
                    try
                    {

                        // Perform whatever action is required in your scenario.
                        var fi = new FileInfo(file);
                        if (_excludes.Contains(fi.Name))
                        {
                            continue;
                        }

                        _reporter.AddFile(fi);
                    }
                    catch (FileNotFoundException e)
                    {
                        // If file was deleted by a separate application or thread since the call to TraverseTree() then just continue.
                        Console.WriteLine(e.Message);
                    }
                }

                // Push the subdirectories onto the stack for traversal.
                // This could also be done before handing the files.
                foreach (var str in subDirs)
                {
                    dirs.Push(str);
                }

                _reporter.EndFolder();
            }
        }
    }
}
