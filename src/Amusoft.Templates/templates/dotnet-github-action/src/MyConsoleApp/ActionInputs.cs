using System;

using CommandLine;

namespace MyConsoleApp;

internal class ActionInputs
{
	public ActionInputs()
	{
		if (Environment.GetEnvironmentVariable("GREETINGS") is { Length: > 0 } greetings)
		{
			Console.WriteLine(greetings);
		}
	}

	[Option('w', "workspace",
		Required = true,
		HelpText = "The workspace directory, or repository root directory.")]
	public string WorkspaceDirectory { get; set; } = null!;
}