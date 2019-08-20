using System.Collections.Generic;
using CommandLine;

namespace Burrich.ConsoleApp
{
    public class CommandLineOptions
    {
        [Option('d', "directories", Separator = ';', Required = true, HelpText = "Directories to be processed (separated by ;).")]
        public IEnumerable<string> Directories { get; set; }

        [Option('r', "reporter", Required = false, Default = Reporter.Console, HelpText = "Reporter to be used to output data.")]
        public Reporter Reporter { get; set; }

        [Option('o', "output", Required = false, HelpText = "Output path.")]
        public string Output { get; set; }

        [Option('v', "verbose", Required = false, HelpText = "Set output to verbose messages.")]
        public bool Verbose { get; set; }
    }

    public enum Reporter
    {
        PlainText,
        Console
    }
}
