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
using CliWrap;
using CliWrap.Buffered;
using NLog.Fluent;
using Shouldly;
using VerifyXunit;
using Xunit;
using Xunit.Abstractions;

namespace Amusoft.Templates.Tests.Cases
{
	public class RoslynPackageTests : TemplateTests
	{
		public RoslynPackageTests(ITestOutputHelper outputHelper, GlobalSetupFixture data) : base(outputHelper, data)
		{
		}

		[Theory(Timeout = 15_000)]
		[InlineData("GeneratedProject")]
		[InlineData("GeneratedProject2")]
		public async Task FileStructureTest(string sourceName)
		{
			var sbArgs = new StringBuilder();
			sbArgs.Append($" -n \"{sourceName}\"");
			sbArgs.Append($" --GitUser TheGitUser");
			sbArgs.Append($" --PackageName ThePackageName");
			sbArgs.Append($" --GitRepository TheRepository");
			sbArgs.Append($" --Author TheAuthor");
			
			using var loggingScope = new LoggingScope();
			using var scaffold = await Dotnet.Cli.NewAsync("roslyn-package", sbArgs.ToString(), CancellationToken.None);
			var files = scaffold.GetRelativeDirectoryPaths().ToList();

			await Verifier.Verify(files)
				.UseParameters(sourceName);
		}

		[Theory(Timeout = 60_000)]
		[Trait("Category","SkipInCI")]
		[InlineData("GeneratedProject")]
		[InlineData("GeneratedProject2")]
		public async Task VerifyNupkgBuild(string sourceName)
		{
			try
			{
				var sbArgs = new StringBuilder();
				sbArgs.Append($" -n \"{sourceName}\"");
				sbArgs.Append($" --GitUser TheGitUser");
				sbArgs.Append($" --PackageName ThePackageName");
				sbArgs.Append($" --GitRepository TheRepository");
				sbArgs.Append($" --Author TheAuthor");
			
				using var logging = new LoggingScope();
				using var scaffold = await Dotnet.Cli.NewAsync("roslyn-package", sbArgs.ToString(), CancellationToken.None);

				var buildScript = Path.Combine(scaffold.GetAbsoluteRootPath(), $"{sourceName}/scripts/build-nupkg.ps1");
				File.Exists(buildScript).ShouldBeTrue();
				var tempDir = IOHelper.CreateTempDirectory();
				var build = await Cli.Wrap("pwsh")
					.WithArguments("-ExecutionPolicy Bypass -File \"" + buildScript + $"\" -OutDir {tempDir}")
					.ExecuteBufferedAsync();
				OutputHelper.WriteLine(build.StandardOutput);
				
				build.ExitCode.ShouldBe(0);
				Directory.EnumerateFiles(tempDir, "*.nupkg").Count().ShouldBe(1);
				
				Directory.Delete(tempDir, true);
			}
			catch (CliException e)
			{
				Assert.Fail(e.Output);
			}
		}

		[Theory(Timeout = 60_000)]
		[Trait("Category","SkipInCI")]
		[InlineData("GeneratedProject")]
		[InlineData("GeneratedProject2")]
		public async Task VerifyVsixBuild(string sourceName)
		{
			try
			{
				var sbArgs = new StringBuilder();
				sbArgs.Append($" -n \"{sourceName}\"");
				sbArgs.Append($" --GitUser TheGitUser");
				sbArgs.Append($" --PackageName ThePackageName");
				sbArgs.Append($" --GitRepository TheRepository");
				sbArgs.Append($" --Author TheAuthor");
			
				using var logging = new LoggingScope();
				using var scaffold = await Dotnet.Cli.NewAsync("roslyn-package", sbArgs.ToString(), CancellationToken.None);

				var buildScript = Path.Combine(scaffold.GetAbsoluteRootPath(), $"{sourceName}/scripts/build-vsix.ps1");
				File.Exists(buildScript).ShouldBeTrue();
				var tempDir = IOHelper.CreateTempDirectory();
				var build = await Cli.Wrap("pwsh")
					.WithArguments("-ExecutionPolicy Bypass -File \"" + buildScript + $"\" -OutDir {tempDir}")
					.ExecuteBufferedAsync();
				OutputHelper.WriteLine(build.StandardOutput);
				
				build.ExitCode.ShouldBe(0);
				Directory.EnumerateFiles(tempDir, "*.vsix").Count().ShouldBe(1);
				Directory.Delete(tempDir, true);
			}
			catch (CliException e)
			{
				Assert.Fail(e.Output);
			}
		}
	}
}