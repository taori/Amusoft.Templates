using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Amusoft.DotnetNew.Tests.CLI;
using Amusoft.DotnetNew.Tests.Diagnostics;
using Amusoft.DotnetNew.Tests.Exceptions;
using Amusoft.DotnetNew.Tests.Scopes;
using Amusoft.Templates.Tests.Toolkit;
using VerifyXunit;
using Xunit;
using Xunit.Abstractions;

namespace Amusoft.Templates.Tests.Cases
{
	public class DotNetTemplateTests : TemplateTests
	{
		public DotNetTemplateTests(ITestOutputHelper outputHelper, GlobalSetupFixture data) : base(outputHelper, data)
		{
		}

		[Fact]
		public async Task FileStructureTest()
		{
			using var loggingScope = new LoggingScope();
			using var scaffold = await Dotnet.Cli.NewAsync("dotnet-template", "-au testauthor", CancellationToken.None);

			await Verifier.Verify(new
			{
				Log = loggingScope.ToFullString(PrintKind.All),
				Files = await scaffold.GetAllFileContentsAsync().ToListAsync()
			});
		}

		[Fact]
		public async Task CheckAuthorIsRequired()
		{
			using var loggingScope = new LoggingScope();

			var exception = await Assert.ThrowsAsync<ScaffoldingFailedException>(async() => await Dotnet.Cli.NewAsync("dotnet-template", null, CancellationToken.None));
			await Verifier.Verify(exception.Output);
		}
	}
}