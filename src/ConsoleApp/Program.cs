using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Burrich.ConsoleApp.Iterations;
using CommandLine;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Burrich.ConsoleApp
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            Parser.Default.ParseArguments<CommandLineOptions>(args)
                .WithParsed(opts => RunOptionsAndReturnExitCode(opts))
                .WithNotParsed(errs => HandleParseError(errs));
        }

        private static int RunOptionsAndReturnExitCode(CommandLineOptions opts)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location))
                .AddJsonFile($"appsettings.json", true, true)
                .AddEnvironmentVariables()
                .Build();
            // Console.WriteLine(configuration["Logging:LogLevel:Default"]);

            var serviceCollection = new ServiceCollection();
            serviceCollection.AddLogging(builder =>
            {
                builder
                    .AddConfiguration(configuration.GetSection("Logging"))
                    .AddConsole();
            });
            var serviceProvider = serviceCollection.BuildServiceProvider();

            var iteration = new StackBasedIteration(serviceProvider.GetService<ILogger<StackBasedIteration>>());
            iteration.TraverseTree(opts.Directories.First(), configuration.GetSection("Parsing:Excludes").Get<List<string>>());
            return 0;
        }

        private static int HandleParseError(IEnumerable<Error> errors)
        {
            // errors.ToList().ForEach(Console.WriteLine);
            return -2;
        }
    }
}
