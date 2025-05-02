using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Amusoft.DotnetNew.Tests.CLI;
using Amusoft.DotnetNew.Tests.Diagnostics;
using Amusoft.DotnetNew.Tests.Scopes;
using Amusoft.Templates.Tests.Toolkit;
using VerifyXunit;
using Xunit;
using Xunit.Abstractions;

namespace Amusoft.Templates.Tests.Cases;

public class TemplateTestScriptTests : TemplateTests
{
	[Theory]
	[InlineData("Some1")]
	[InlineData("Some2")]
	public async Task VerifyDefaultCall(string sourceName)
	{
		var sbArgs = new StringBuilder();
		sbArgs.Append($" -n \"{sourceName}\"");
		
		using var loggingScope = new LoggingScope();
		using var scaffold = await Dotnet.Cli.NewAsync("template-test-script", sbArgs.ToString(), CancellationToken.None);

		var contents = new
		{
			Log = loggingScope.ToFullString(PrintKind.All),
			Files = scaffold.GetRelativeDirectoryPaths(),
			ScriptFile = await scaffold.GetFileContentAsync($"{sourceName}.ps1"),
		};

		await Verifier.Verify(contents)
			.UseParameters(sourceName);
	}
	
	public TemplateTestScriptTests(ITestOutputHelper outputHelper, GlobalSetupFixture data) : base(outputHelper, data)
	{
	}
}