using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace Burrich.ConsoleApp.Iterations
{
    public class StackBasedIteration
    {
        private readonly ILogger<StackBasedIteration> _logger;

        public StackBasedIteration(ILogger<StackBasedIteration> logger)
        {
            _logger = logger;
        }

        public void TraverseTree(string root, List<string> excludes)
        {
            _logger.LogInformation($"Starting to traverse tree [RootFolder={root}] [Excludes={string.Join(";", excludes)}]");

            // Data structure to hold names of subfolders to be examined for files.
            var dirs = new Stack<string>(20);

            if (!Directory.Exists(root))
            {
                throw new ArgumentException(nameof(root));
            }

            dirs.Push(root);

            while (dirs.Count > 0)
            {
                var currentDir = dirs.Pop();

                if (excludes.Contains(Path.GetFileName(currentDir)))
                {
                    continue;
                }

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

                string[] files = null;
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
                // Modify this block to perform your required task.
                foreach (var file in files)
                {
                    try
                    {
                        // Perform whatever action is required in your scenario.
                        var fi = new FileInfo(file);
                        Console.WriteLine($"{fi.FullName}: {fi.Length.ToString()}, {fi.CreationTime.ToString("yyyy-MM-dd")}");
                    }
                    catch (FileNotFoundException e)
                    {
                        // If file was deleted by a separate application
                        //  or thread since the call to TraverseTree()
                        // then just continue.
                        Console.WriteLine(e.Message);
                        continue;
                    }
                }

                // Push the subdirectories onto the stack for traversal.
                // This could also be done before handing the files.
                foreach (var str in subDirs)
                {
                    dirs.Push(str);
                }
            }
        }
    }
}
