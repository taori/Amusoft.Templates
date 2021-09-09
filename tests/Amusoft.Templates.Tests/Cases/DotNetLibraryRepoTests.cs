using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amusoft.Templates.Tests.Toolkit;
using Amusoft.Templates.Tests.Utility;
using Shouldly;
using Xunit;
using Xunit.Abstractions;

namespace Amusoft.Templates.Tests.Cases
{
	public class DotNetLibraryRepoTests : TemplateTests
	{
		public DotNetLibraryRepoTests(ITestOutputHelper outputHelper, GlobalSetupFixture data) : base(outputHelper, data)
		{
		}

		[Theory]
		[InlineData("GeneratedProject")]
		[InlineData("GeneratedProject2")]
		public async Task FileStructureTest(string sourceName)
		{
			using (new TemplateInstallationSession(Path.Combine(GetTemplateRootPath(), "dotnet-library-repo")))
			{
				using var templateRunner = new TemplateRunner("dotnet-library-repo");
				var sbArgs = new StringBuilder();
				sbArgs.Append(" --GitProjectName \"SampleProject\"");
				sbArgs.Append(" --NugetPackageId \"SamplePackageId\"");
				sbArgs.Append(" --ProductName \"SampleProduct\"");
				sbArgs.Append(" --GitUser \"taori\"");
				sbArgs.Append(" --Author \"Santa Clause\"");
				sbArgs.Append($" -n \"{sourceName}\"");
				await templateRunner.ExecuteAsync(sbArgs.ToString());

				templateRunner.OutputContent.ShouldNotBeEmpty();
				templateRunner.ErrorContent.ShouldBeEmpty();
				templateRunner.OutputContent.ShouldEndWith($@"File actions would have been taken:
  Create: ./README.md
  Create: ./build/SourceLink.props
  Create: ./src/All.sln
  Create: ./src/Directory.Build.props
  Create: ./.github/workflows/dotnet.yml
  Create: ./src/{sourceName}/{sourceName}.csproj
  Create: ./tests/{sourceName}.UnitTests/{sourceName}.UnitTests.csproj
  Create: ./tests/{sourceName}.UnitTests/nlog.config
  Create: ./tests/{sourceName}.UnitTests/UnitTest1.cs
  Create: ./tests/{sourceName}.UnitTests/Toolkit/EmbeddedResourceReader.cs
  Create: ./tests/{sourceName}.UnitTests/Toolkit/GlobalSetupFixture.cs
  Create: ./tests/{sourceName}.UnitTests/Toolkit/TestBase.cs
  Create: ./tests/{sourceName}.UnitTests/Toolkit/XUnitOutputTarget.cs
");
			}
		}

		[Theory]
		[InlineData(null, "main")]
		[InlineData("master", "master")]
		public async Task RewriteWorkflowCheck(string rootBranchName, string expectedFileBranch)
		{
			using (new TemplateInstallationSession(Path.Combine(GetTemplateRootPath(), "dotnet-library-repo")))
			{
				using var templateRunner = new TemplateRunner("dotnet-library-repo", false);
				var sbArgs = new StringBuilder();
				sbArgs.Append(" --GitProjectName \"SampleProject\"");
				sbArgs.Append(" --NugetPackageId \"SamplePackageId\"");
				sbArgs.Append(" --ProductName \"SampleProduct\"");
				sbArgs.Append(" --GitUser \"taori\"");
				sbArgs.Append(" --Author \"Santa Clause\"");
				sbArgs.Append(" -n \"GeneratedProject\"");

				if (!string.IsNullOrEmpty(rootBranchName))
					sbArgs.Append($" --RootBranchName \"{rootBranchName}\"");

				await templateRunner.ExecuteAsync(sbArgs.ToString());

				templateRunner.OutputContent.ShouldNotBeEmpty();
				templateRunner.ErrorContent.ShouldBeEmpty();

				var absoluteFilePath = templateRunner.GetAbsoluteFilePath("./.github/workflows/dotnet.yml");
				File.Exists(absoluteFilePath).ShouldBeTrue();
				var content = await File.ReadAllTextAsync(absoluteFilePath);
				content.ShouldContain($@"
  push:
    branches: [ {expectedFileBranch} ]");

				content.ShouldContain($@"
  pull_request:
    branches: [ {expectedFileBranch} ]");

			}
		}


		[Theory]
		[InlineData("GeneratedProject", "Santa Clause", "SampleProject", "SamplePackageId", "SampleProductName", "taori")]
		[InlineData("GeneratedProject2", "Santa Clause2", "SampleProject2", "SamplePackageId2", "SampleProductName2", "taori2")]
		public async Task RewriteProjectFileCheck(string sourceName, string authorName, string projectName, string packageId, string productName, string gitUser)
		{
			using (new TemplateInstallationSession(Path.Combine(GetTemplateRootPath(), "dotnet-library-repo")))
			{
				using var templateRunner = new TemplateRunner("dotnet-library-repo", false);
				var sbArgs = new StringBuilder();
				sbArgs.Append($" --GitProjectName \"{projectName}\"");
				sbArgs.Append($" --NugetPackageId \"{packageId}\"");
				sbArgs.Append($" --ProductName \"{productName}\"");
				sbArgs.Append($" --GitUser \"{gitUser}\"");
				sbArgs.Append($" --RootBranchName \"master\"");
				sbArgs.Append($" --Author \"{authorName}\"");
				sbArgs.Append($" -n \"{sourceName}\"");
				await templateRunner.ExecuteAsync(sbArgs.ToString());

				templateRunner.OutputContent.ShouldNotBeEmpty();
				templateRunner.ErrorContent.ShouldBeEmpty();

				var absoluteFilePath = templateRunner.GetAbsoluteFilePath($"./src/{sourceName}/{sourceName}.csproj");
				File.Exists(absoluteFilePath).ShouldBeTrue();
				var content = await File.ReadAllTextAsync(absoluteFilePath);
				content.ShouldContain($"<Copyright>Copyright © {authorName} {DateTime.Now:yyyy}</Copyright>");
				content.ShouldContain($"<RepositoryUrl>https://github.com/{gitUser}/{projectName}.git</RepositoryUrl>");
				content.ShouldContain($"<PackageProjectUrl>https://github.com/{gitUser}/{projectName}</PackageProjectUrl>");
				content.ShouldContain($"<PackageId>{packageId}</PackageId>");
				content.ShouldContain($"<Authors>{authorName}</Authors>");
				content.ShouldContain($"<Product>{productName}</Product>");
			}
		}
	}
}