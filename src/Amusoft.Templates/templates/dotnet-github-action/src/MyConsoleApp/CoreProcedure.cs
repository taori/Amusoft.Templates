// This file is licensed to you under the MIT license.

using MyConsoleApp.Extensions;

namespace MyConsoleApp;

internal static class CoreProcedure
{
	public static Task ExecuteAsync(IHost host, ActionInputs inputs, CancellationToken cancellationToken)
	{
		Matcher matcher = new();
		matcher.AddIncludePatterns(new[] { "**/*.csproj", "**/*.vbproj" });

		var projects = matcher.GetResultsInFullPath(inputs.Directory);

		var updatedMetrics = false;
		var title = "";
		StringBuilder summary = new();
		if (false)
		{
			var fileName = "CODE_METRICS.md";
			var fullPath = Path.Combine(inputs.Directory, fileName);
			var logger = host.GetService<ILoggerFactory>().CreateLogger(nameof(ExecuteAsync));
			var fileExists = File.Exists(fullPath);

			logger.LogInformation(
				$"{(fileExists ? "Updating" : "Creating")} {fileName} markdown file with latest code metric data."
			);

			// summary.AppendLine(
			// 	title = $"{(fileExists ? "Updated" : "Created")} {fileName} file, analyzed metrics for {metricData.Count} projects."
			// );
			//
			// foreach (var (path, _) in metricData)
			// {
			// 	summary.AppendLine($"- *{path}*");
			// }
			//
			// var contents = metricData.ToMarkDownBody(inputs);
			// await File.WriteAllTextAsync(
			// 	fullPath,
			// 	contents,
			// 	tokenSource.Token
			// );

			updatedMetrics = true;
		}
		else
		{
			summary.Append("No metrics were determined.");
		}

		// https://docs.github.com/actions/reference/workflow-commands-for-github-actions#setting-an-output-parameter
		// ::set-output deprecated as mentioned in https://github.blog/changelog/2022-10-11-github-actions-deprecating-save-state-and-set-output-commands/
		var githubOutputFile = Environment.GetEnvironmentVariable("GITHUB_OUTPUT", EnvironmentVariableTarget.Process);
		if (!string.IsNullOrWhiteSpace(githubOutputFile))
		{
			using (var textWriter = new StreamWriter(githubOutputFile!, true, Encoding.UTF8))
			{
				textWriter.WriteLine($"updated-metrics={updatedMetrics}");
				textWriter.WriteLine($"summary-title={title}");
				textWriter.WriteLine("summary-details<<EOF");
				textWriter.WriteLine(summary);
				textWriter.WriteLine("EOF");
			}
		}
		else
		{
			Console.WriteLine($"::set-output name=updated-metrics::{updatedMetrics}");
			Console.WriteLine($"::set-output name=summary-title::{title}");
			Console.WriteLine($"::set-output name=summary-details::{summary}");
		}

		Environment.Exit(0);
		return Task.CompletedTask;
	}
}