using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Amusoft.DotnetNew.Tests.CLI;
using Amusoft.DotnetNew.Tests.Diagnostics;
using Amusoft.DotnetNew.Tests.Exceptions;
using Amusoft.DotnetNew.Tests.Scopes;
using Amusoft.Templates.Tests.Toolkit;
using VerifyTests;
using VerifyXunit;
using Xunit;
using Xunit.Abstractions;

namespace Amusoft.Templates.Tests.Cases;

public class TemplatePackageTests : TemplateTests
{
	[Theory]
	[InlineData("Some1")]
	[InlineData("Some2")]
	public async Task VerifyDefaultCall(string sourceName)
	{
		var sbArgs = new StringBuilder();
		sbArgs.Append($" -n \"{sourceName}\"");
		sbArgs.Append($" --GitProjectName TheNewProject");
		sbArgs.Append($" --NugetPackageId TheNugetPackageId");
		sbArgs.Append($" --ProductName TheProductName");
		sbArgs.Append($" --GitUser TheUser");
		sbArgs.Append($" --Author TheAuthor");
		
		using var loggingScope = new LoggingScope();
		using var scaffold = await Dotnet.Cli.NewAsync("template-package", sbArgs.ToString(), CancellationToken.None);

		var contents = new
		{
			Log = loggingScope.ToFullString(PrintKind.All),
			Files = scaffold.GetRelativeDirectoryPaths(),
			// ScriptFile = await scaffold.GetFileContentAsync($"{sourceName}.ps1"),
		};

		await Verifier.Verify(contents)
			.UseParameters(sourceName);
	}
	
	[Fact]
	public async Task VerifyMandatoryParameters()
	{
		var sbArgs = new StringBuilder();
		
		var ex = await Assert.ThrowsAsync<ScaffoldingFailedException>(async () => await Dotnet.Cli.NewAsync("template-package", sbArgs.ToString(), CancellationToken.None));
		var regex = new Regex(@"Missing mandatory option\(s\) for the template 'template package'(?<msg>[^""]+)""");
		var match = regex.Match(ex.Output);
		Assert.Matches(regex, ex.Output);
		
		await Verifier.Verify(match.Groups["msg"].Value);
	}
	
	public TemplatePackageTests(ITestOutputHelper outputHelper, GlobalSetupFixture data) : base(outputHelper, data)
	{
	}
}