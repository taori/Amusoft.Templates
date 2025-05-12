using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Amusoft.DotnetNew.Tests.CLI;
using Amusoft.DotnetNew.Tests.Diagnostics;
using Amusoft.DotnetNew.Tests.Exceptions;
using Amusoft.DotnetNew.Tests.Scopes;
using Amusoft.Templates.Tests.Toolkit;
using CliWrap;
using CliWrap.Buffered;
using Shouldly;
using VerifyXunit;
using Xunit;
using Xunit.Abstractions;

namespace Amusoft.Templates.Tests.Cases
{
	public class GithubActionDotnetTests : TemplateTests
	{
		public GithubActionDotnetTests(ITestOutputHelper outputHelper, GlobalSetupFixture data) : base(outputHelper, data)
		{
		}

		[Fact(Timeout = 15_000)]
		public async Task MandatoryParameters()
		{
			using (var loggingScope = new LoggingScope())
			{
				var ex = await Assert.ThrowsAsync<ScaffoldingFailedException>(async() => await Dotnet.Cli.NewAsync("github-action-dotnet", "", CancellationToken.None));
				await Verifier.Verify(new
					{
						Output = ex.Output
					}
				);
			}
		}

		[Fact(Timeout = 15_000)]
		public async Task FileStructureTest()
		{
			using var loggingScope = new LoggingScope();
			var args = "-n GeneratedProject --ActionName TheActionName --MaintainerEmail TheMainerMail --GitRepository TheGitRepository --GitOwner TheGitOwner";
			using var scaffold = await Dotnet.Cli.NewAsync("github-action-dotnet", args, CancellationToken.None);

			await Verifier.Verify(scaffold.GetRelativeDirectoryPaths().ToArray());
		}
		
		

		[Fact(Timeout = 60_000)]
		[Trait("Category","SkipInCI")]
		public async Task BuildCheck()
		{
			try
			{
				using var logging = new LoggingScope();
			var args = "-n GeneratedProject --ActionName TheActionName --MaintainerEmail TheMainerMail --GitRepository TheGitRepository --GitOwner TheGitOwner";
				using var scaffold = await Dotnet.Cli.NewAsync("github-action-dotnet", args, CancellationToken.None);

				await scaffold.RestoreAsync($"src/GeneratedProject.slnx", null, CancellationToken.None);
				await scaffold.BuildAsync($"src/GeneratedProject.slnx", "-property:SkipMicrosoftBuildTasksGit=true", CancellationToken.None);
				await scaffold.TestAsync($"src/GeneratedProject.slnx", null, CancellationToken.None);
				await Verifier
					.Verify(logging.ToFullString(PrintKind.All));
			}
			catch (CliException e)
			{
				Assert.Fail(e.Output);
			}
		}

		[Fact(Timeout = 60_000)]
		public async Task VerifyBuildScript()
		{
			try
			{
				using var logging = new LoggingScope();
			var args = "-n GeneratedProject --ActionName TheActionName --MaintainerEmail TheMainerMail --GitRepository TheGitRepository --GitOwner TheGitOwner";
				using var scaffold = await Dotnet.Cli.NewAsync("github-action-dotnet", args, CancellationToken.None);

				var scriptPath = Path.Combine(scaffold.GetAbsoluteRootPath(), $"scripts/build.ps1");
				File.Exists(scriptPath).ShouldBeTrue();
				var scriptExecution = await Cli.Wrap("pwsh")
					.ExecuteBufferedAsync();
				OutputHelper.WriteLine(scriptExecution.StandardOutput);
				
				scriptExecution.ExitCode.ShouldBe(0);
			}
			catch (CliException e)
			{
				Assert.Fail(e.Output);
			}
		}

		[Fact(Timeout = 60_000)]
		[Trait("Category","SkipInCI")]
		public async Task VerifyCoverageScript()
		{
			try
			{
				using var logging = new LoggingScope();
				var args = "-n GeneratedProject --ActionName TheActionName --MaintainerEmail TheMainerMail --GitRepository TheGitRepository --GitOwner TheGitOwner";
				using var scaffold = await Dotnet.Cli.NewAsync("github-action-dotnet", args, CancellationToken.None);

				var scriptPath = Path.Combine(scaffold.GetAbsoluteRootPath(), $"scripts/coverage.ps1");
				File.Exists(scriptPath).ShouldBeTrue();
				var tempFolder = IOHelper.CreateTempDirectory();
				var scriptExecution = await Cli.Wrap("pwsh")
					.WithArguments("-ExecutionPolicy Bypass -File \"" + scriptPath + $"\" -CacheFolder {tempFolder}")
					.ExecuteBufferedAsync();
				OutputHelper.WriteLine(scriptExecution.StandardOutput);
				scriptExecution.ExitCode.ShouldBe(0);
				Directory.EnumerateFiles(tempFolder).Any().ShouldBeTrue();
			}
			catch (CliException e)
			{
				Assert.Fail(e.Output);
			}
		}
	}
}