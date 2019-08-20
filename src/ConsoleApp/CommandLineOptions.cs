using System.Collections.Generic;
using CommandLine;

namespace Burrich.ConsoleApp
{
    public class CommandLineOptions
    {
        [Option('d', "directories", Required = true, HelpText = "Directories to be processed.")]
        public IEnumerable<string> Directories { get; set; }

        [Option('v', "verbose", Required = false, HelpText = "Set output to verbose messages.")]
        public bool Verbose { get; set; }
    }
}
