// This file is licensed to you under the MIT license.

using MyConsoleApp.Extensions;

namespace MyConsoleApp;

internal class CoreProcedure
{
	public static Task ExecuteAsync(ILogger<CoreProcedure> logger, ActionInputs inputs, CancellationToken cancellationToken)
	{
		Matcher matcher = new();
		matcher.AddIncludePatterns(new[] { "**/*.csproj", "**/*.vbproj" });
		
		logger.LogInformation($"""
		                       Received arguments:
		                       Workspace directory {inputs.WorkspaceDirectory}
		                       """);
		
		

		// https://docs.github.com/actions/reference/workflow-commands-for-github-actions#setting-an-output-parameter
		// ::set-output deprecated as mentioned in https://github.blog/changelog/2022-10-11-github-actions-deprecating-save-state-and-set-output-commands/
		var githubOutputFile = Environment.GetEnvironmentVariable("GITHUB_OUTPUT", EnvironmentVariableTarget.Process);
		if (!string.IsNullOrWhiteSpace(githubOutputFile))
		{
			using (var textWriter = new StreamWriter(githubOutputFile!, true, Encoding.UTF8))
			{
				textWriter.WriteLine($"message=Hello from Github Action {DateTime.Now:F}");
			}
		}
		else
		{
			Console.WriteLine($"::set-output name=message::Hello from Github Action {DateTime.Now:F}");
		}

		ExitEnvironment.WithCode(0);
		return Task.CompletedTask;
	}
}