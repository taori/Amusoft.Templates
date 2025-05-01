using MyConsoleApp.Extensions;

namespace MyConsoleApp;

class Program
{
	static async Task ExecuteGithubActionAsync(ActionInputs inputs, IHost host)
	{
		using CancellationTokenSource tokenSource = new();

		Console.CancelKeyPress += delegate
		{
			tokenSource.Cancel();
		};
		
		await CoreProcedure.ExecuteAsync(host.GetService<ILogger<CoreProcedure>>(), inputs, tokenSource.Token);
	}


	internal static async Task Main(string[] args)
	{
		using var host = Host.CreateDefaultBuilder(args)
			.ConfigureServices((_, services) =>
			{
			})
			.Build();

		var parser = Default
			.ParseArguments(() => new ActionInputs(), args)
			.WithNotParsed(err => HandleNotParsed(err, host));

		await parser.WithParsedAsync(options => ExecuteGithubActionAsync(options, host));
		await host.RunAsync();
	}

	private static void HandleNotParsed(IEnumerable<Error> errors, IHost host)
	{
		host.GetService<ILoggerFactory>()
			.CreateLogger("MyConsoleApp")
			.LogError(
				string.Join(Environment.NewLine, errors.Select(error => error.ToString()))
			);

		ExitEnvironment.WithCode(2);
	}
}