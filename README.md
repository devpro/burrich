# Burrich

This is the codebase of Burrich, a command line tool, written in .NET Core 2.2, that will look at a directory and gather intel on files and directories.

> I forgot what was on this external hard drive... And what happens if it gets lost?

Burrich solution has been made to tackle such topics and help organize our digital elements, that can be quickly be a mess!

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
