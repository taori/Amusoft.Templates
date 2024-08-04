using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amusoft.Templates.Tests.Resources;
using Amusoft.Templates.Tests.Toolkit;
using Amusoft.Templates.Tests.Utility;
using Shouldly;
using VerifyXunit;
using Xunit;
using Xunit.Abstractions;

namespace Amusoft.Templates.Tests.Cases
{
	public class DotNetLibraryRepoTests : TemplateTests, IClassFixture<DotnetLibraryRepoSession>
	{
		private readonly DotnetLibraryRepoSession _session;

		public DotNetLibraryRepoTests(ITestOutputHelper outputHelper, GlobalSetupFixture data, DotnetLibraryRepoSession session) : base(outputHelper, data)
		{
			_session = session;
		}

		[Theory]
		[InlineData("GeneratedProject")]
		[InlineData("GeneratedProject2")]
		public async Task FileStructureTest(string sourceName)
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

			await Verifier.Verify(templateRunner.OutputContent)
				.UseParameters(sourceName);
		}

		[Theory]
		[InlineData(null, "main")]
		[InlineData("master", "master")]
		public async Task RewriteWorkflowCheck(string rootBranchName, string expectedFileBranch)
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
			await Verifier.Verify(content)
				.UseParameters(rootBranchName, expectedFileBranch);
		}


		[Theory]
		[InlineData("GeneratedProject", "Santa Clause", "SampleProject", "SamplePackageId", "SampleProductName", "santa")]
		[InlineData("GeneratedProject2", "Santa Clause2", "SampleProject2", "SamplePackageId2", "SampleProductName2", "santa2")]
		public async Task RewriteProjectFileCheck(string sourceName, string authorName, string projectName, string packageId, string productName, string gitUser)
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