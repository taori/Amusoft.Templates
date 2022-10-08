using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amusoft.Templates.Tests.Toolkit;
using Amusoft.Templates.Tests.Utility;
using Shouldly;
using VerifyXunit;
using Xunit;
using Xunit.Abstractions;

namespace Amusoft.Templates.Tests.Cases
{
	[UsesVerify]
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

				var expectedLines = $@"File actions would have been taken:
  Create: ./README.md
  Create: ./build/Project.Common.props
  Create: ./build/SourceLink.props
  Create: ./scripts/build.ps1
  Create: ./src/All.sln
  Create: ./src/Directory.Build.props
  Create: ./src/nuget.config
  Create: ./src/packageIcon.png
  Create: ./.github/workflows/CI.yml
  Create: ./src/{sourceName}/{sourceName}.csproj
  Create: ./tests/{sourceName}.IntegrationTests/{sourceName}.IntegrationTests.csproj
  Create: ./tests/{sourceName}.IntegrationTests/UnitTest1.cs
  Create: ./tests/{sourceName}.IntegrationTests/Usings.cs
  Create: ./tests/{sourceName}.UnitTests/{sourceName}.UnitTests.csproj
  Create: ./tests/{sourceName}.UnitTests/nlog.config
  Create: ./tests/{sourceName}.UnitTests/UnitTest1.cs
  Create: ./tests/{sourceName}.UnitTests/Toolkit/EmbeddedResourceReader.cs
  Create: ./tests/{sourceName}.UnitTests/Toolkit/GlobalSetupFixture.cs
  Create: ./tests/{sourceName}.UnitTests/Toolkit/TestBase.cs
".Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries).OrderBy(d => d).ToArray();

				var actualLines = templateRunner.OutputContent.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries).OrderBy(d => d).ToArray();

				actualLines.ShouldAllBe(d => expectedLines.Contains(d));
				actualLines.Length.ShouldBe(expectedLines.Length);
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

				var absoluteFilePath = templateRunner.GetAbsoluteFilePath("./.github/workflows/CI.yml");
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
		[InlineData("GeneratedProject", "Santa Clause", "SampleProject", "SamplePackageId", "SampleProductName", "santa")]
		[InlineData("GeneratedProject2", "Santa Clause2", "SampleProject2", "SamplePackageId2", "SampleProductName2", "santa2")]
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

				var projectFileContent = await ProjectFileContentAsync($"./src/{sourceName}/{sourceName}.csproj");
				await Verifier.Verify(projectFileContent)
					.UseParameters(packageId, "projectFileContent");

				var projectCommonContent = await ProjectFileContentAsync("./build/Project.Common.props");
				await Verifier.Verify(projectCommonContent)
					.UseParameters(packageId, "projectCommonContent");

				async Task<string> ProjectFileContentAsync(string relativeFilePath)
				{
					var absoluteFilePath = templateRunner.GetAbsoluteFilePath(relativeFilePath);
					File.Exists(absoluteFilePath).ShouldBeTrue();
					var projectFileContent = await File.ReadAllTextAsync(absoluteFilePath);
					return projectFileContent;
				}
			}
		}
	}
}