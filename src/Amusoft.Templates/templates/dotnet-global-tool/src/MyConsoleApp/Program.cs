using Microsoft.Extensions.Hosting;

using Spectre.Console.Cli;

using MyConsoleApp.Integration;

namespace MyConsoleApp;

class Program
{
	static async Task<int> Main(string[] args)
	{
#if DEBUG
		while (Console.ReadLine() is {Length: > 0} line)
		{
			var r = await RunApplication(line.Split(' '));
			Console.WriteLine($"ExitCode: {r}");
		}

		return 0;
#else
		return await RunApplication(args);
#endif
	}

	private static Task<int> RunApplication(string[] args)
	{
		var host = CreateHostBuilder(args);
		var registrar = new TypeRegistrar(host);
		var app = new CommandApp(registrar);
		app.Configure(configurator =>
			{
				configurator.AddBranch(
					"project", projectCommand =>
					{
						projectCommand.AddCommand<AddCommand>("add");
						projectCommand.AddCommand<RemoveCommand>("remove");
					}
				);
			}
		);
	    
		return app.RunAsync(args);
	}

	internal static IHostBuilder CreateHostBuilder(string[] args) =>
		Host
			.CreateDefaultBuilder(args)
			.ConfigureServices((hostContext, services) =>
			{
				// add service registrations here
			});
    
	internal class AddCommand : Command<AddCommand.AddCommandSettings>
	{
		public class AddCommandSettings : CommandSettings
		{
		}
	    
		public override int Execute(CommandContext context, AddCommandSettings settings)
		{
			throw new NotImplementedException();
		}
	}
    
	internal class RemoveCommand : Command<RemoveCommand.RemoveCommandSettings>
	{
		public class RemoveCommandSettings : CommandSettings
		{
		}
	    
		public override int Execute(CommandContext context, RemoveCommandSettings settings)
		{
			throw new NotImplementedException();
		}
	}
}
