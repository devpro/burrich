# Burrich

[![Build Status](https://dev.azure.com/devprofr/open-source/_apis/build/status/global-tools/burrich-ci?branchName=master)](https://dev.azure.com/devprofr/open-source/_build/latest?definitionId=39&branchName=master)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=devpro.burrich&metric=alert_status)](https://sonarcloud.io/dashboard?id=devpro.burrich)
[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=devpro.burrich&metric=coverage)](https://sonarcloud.io/dashboard?id=devpro.burrich)
[![Nuget](https://img.shields.io/nuget/v/burrich.svg)](https://www.nuget.org/packages/burrich)

Burrich is a command line tool, written in .NET Core 3.1 / [C# 8.0](https://docs.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-8), that will look at a directory and gather information on files and directories.

> I forgot what was on this external hard drive... And what happens if it gets lost?

Burrich solution has been made to tackle such topics and help organize our digital material, that can be quickly be a mess!

## Build & Run

### Prerequisites

- [.NET Core SDK](https://dot.net)

### Build

- Restore packages: `dotnet restore`
- Build the solution: `dotnet build`

### Run

- Run the project: `dotnet run --project src/ConsoleApp`
- Execute the dll: `dotnet src/ConsoleApp/bin/Debug/netcoreapp2.2/Burrich.ConsoleApp.dll`

## Documentation

### Usage

- `dotnet Burrich.ConsoleApp.dll -d <rootFolder>`

### NuGet packages

- [CommandLineParser](https://github.com/commandlineparser/commandline)

### Applied recipes

- [How to: Iterate Through a Directory Tree (C# Programming Guide)](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/file-system/how-to-iterate-through-a-directory-tree)
- [Logging in .NET Core and ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/logging/)
- [Configuration in ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/)
