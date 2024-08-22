using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Amusoft.DotnetNew.Tests.CLI;
using Amusoft.DotnetNew.Tests.Diagnostics;
using Amusoft.DotnetNew.Tests.Exceptions;
using Amusoft.DotnetNew.Tests.Scopes;
using Amusoft.Templates.Tests.Toolkit;
using NLog.Fluent;
using Shouldly;
using VerifyXunit;
using Xunit;
using Xunit.Abstractions;

namespace Amusoft.Templates.Tests.Cases
{
	public class DotNetLibraryRepoTests : TemplateTests
	{
		public DotNetLibraryRepoTests(ITestOutputHelper outputHelper, GlobalSetupFixture data) : base(outputHelper, data)
		{
		}

		[Theory(Timeout = 15_000)]
		[InlineData("GeneratedProject")]
		[InlineData("GeneratedProject2")]
		public async Task FileStructureTest(string sourceName)
		{
			var sbArgs = new StringBuilder();
			sbArgs.Append(" --GitProjectName \"SampleProject\"");
			sbArgs.Append(" --NugetPackageId \"SamplePackageId\"");
			sbArgs.Append(" --ProductName \"SampleProduct\"");
			sbArgs.Append(" --GitUser \"taori\"");
			sbArgs.Append(" --Author \"Santa Clause\"");
			sbArgs.Append($" -n \"{sourceName}\"");
			
			using var loggingScope = new LoggingScope();
			using var scaffold = await Dotnet.Cli.NewAsync("dotnet-library-repo", sbArgs.ToString(), CancellationToken.None);
			var files = scaffold.GetRelativeDirectoryPaths().ToList();

			await Verifier.Verify(files)
				.UseParameters(sourceName);
		}

		[Theory(Timeout = 15_000)]
		[InlineData(null, "main")]
		[InlineData("master", "master")]
		public async Task RewriteWorkflowCheck(string rootBranchName, string expectedFileBranch)
		{
			var sbArgs = new StringBuilder();
			sbArgs.Append(" --GitProjectName \"SampleProject\"");
			sbArgs.Append(" --NugetPackageId \"SamplePackageId\"");
			sbArgs.Append(" --ProductName \"SampleProduct\"");
			sbArgs.Append(" --GitUser \"taori\"");
			sbArgs.Append(" --Author \"Santa Clause\"");
			sbArgs.Append(" -n \"GeneratedProject\"");

			if (!string.IsNullOrEmpty(rootBranchName))
				sbArgs.Append($" --RootBranchName \"{rootBranchName}\"");
			
			using var loggingScope = new LoggingScope();
			using var scaffold = await Dotnet.Cli.NewAsync("dotnet-library-repo", sbArgs.ToString(), CancellationToken.None);

			var content = await scaffold.GetFileContentAsync(".github/workflows/CI.yml");
			await Verifier.Verify(content)
				.UseParameters(rootBranchName, expectedFileBranch);
		}


		[Theory(Timeout = 15_000)]
		[InlineData("GeneratedProject", "Santa Clause", "SampleProject", "SamplePackageId", "SampleProductName", "santa")]
		[InlineData("GeneratedProject2", "Santa Clause2", "SampleProject2", "SamplePackageId2", "SampleProductName2", "santa2")]
		public async Task RewriteProjectFileCheck(string sourceName, string authorName, string projectName, string packageId, string productName, string gitUser)
		{
			var sbArgs = new StringBuilder();
			sbArgs.Append($" --GitProjectName \"{projectName}\"");
			sbArgs.Append($" --NugetPackageId \"{packageId}\"");
			sbArgs.Append($" --ProductName \"{productName}\"");
			sbArgs.Append($" --GitUser \"{gitUser}\"");
			sbArgs.Append($" --RootBranchName \"master\"");
			sbArgs.Append($" --Author \"{authorName}\"");
			sbArgs.Append($" -n \"{sourceName}\"");

			using var loggingScope = new LoggingScope();
			using var scaffold = await Dotnet.Cli.NewAsync("dotnet-library-repo", sbArgs.ToString(), CancellationToken.None);

			var contents = new
			{
				ProjectFile = await scaffold.GetFileContentAsync($"src/{sourceName}/{sourceName}.csproj"),
				ProjectCommonProps = await scaffold.GetFileContentAsync("build/Project.Common.props"),
			};

			await Verifier.Verify(contents)
				.UseParameters(sourceName);
		}

		[Theory(Timeout = 60_000)]
		[Trait("Category","SkipInCI")]
		[InlineData("GeneratedProject")]
		[InlineData("GeneratedProject2")]
		public async Task BuildCheck(string sourceName)
		{
			try
			{
				var sbArgs = new StringBuilder();
				sbArgs.Append(" --GitProjectName \"SampleProject\"");
				sbArgs.Append(" --NugetPackageId \"SamplePackageId\"");
				sbArgs.Append(" --ProductName \"SampleProduct\"");
				sbArgs.Append(" --GitUser \"taori\"");
				sbArgs.Append(" --Author \"Santa Clause\"");
				sbArgs.Append($" -n \"{sourceName}\"");
			
				using var logging = new LoggingScope();
				using var scaffold = await Dotnet.Cli.NewAsync("dotnet-library-repo", sbArgs.ToString(), CancellationToken.None);

				await scaffold.RestoreAsync($"src/{sourceName}.sln", null, CancellationToken.None);
				await scaffold.BuildAsync($"src/{sourceName}.sln", null, CancellationToken.None);
				await scaffold.TestAsync($"src/{sourceName}.sln", null, CancellationToken.None);
				await Verifier
					.Verify(logging.ToFullString(PrintKind.All))
					.UseParameters(sourceName);
			}
			catch (CliException e)
			{
				Assert.Fail(e.Output);
			}
		}
	}
}