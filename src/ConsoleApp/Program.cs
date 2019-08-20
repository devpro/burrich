using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Burrich.ConsoleApp.Iterations;
using Burrich.ConsoleApp.Reporters;
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
            var parser = new Parser(cfg => cfg.CaseInsensitiveEnumValues = true);
            parser.ParseArguments<CommandLineOptions>(args)
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

            IReporter reporter;
            switch (opts.Reporter)
            {
                case Reporter.PlainText:
                    reporter = new PlainTextReporter(opts.Output ?? Path.Combine(Directory.GetCurrentDirectory(), $"report-{DateTime.Now.ToString("yyyyMMddHHmmss")}.txt"));
                    break;
                default:
                    reporter = new ConsoleReporter();
                    break;
            }

            var iteration = new StackBasedIteration(serviceProvider.GetService<ILogger<StackBasedIteration>>(), reporter,
                configuration.GetSection("Parsing:Excludes").Get<List<string>>());
            opts.Directories.ToList().ForEach(x => iteration.TraverseTree(x));
            return 0;
        }

        private static int HandleParseError(IEnumerable<Error> errors)
        {
            errors.ToList().ForEach(Console.WriteLine);
            return -2;
        }
    }
}
