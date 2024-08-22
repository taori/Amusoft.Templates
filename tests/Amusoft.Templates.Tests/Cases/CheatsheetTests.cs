using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Amusoft.DotnetNew.Tests.CLI;
using Amusoft.DotnetNew.Tests.Diagnostics;
using Amusoft.DotnetNew.Tests.Scopes;
using Amusoft.Templates.Tests.Toolkit;
using Shouldly;
using VerifyXunit;
using Xunit;
using Xunit.Abstractions;

namespace Amusoft.Templates.Tests.Cases
{
	public class CheatsheetTests : TemplateTests
	{
		public CheatsheetTests(ITestOutputHelper outputHelper, GlobalSetupFixture data) : base(outputHelper, data)
		{
		}

		[Fact(Timeout = 15_000)]
		public async Task FileStructureTest()
		{
			using var loggingScope = new LoggingScope();
			using var scaffold = await Dotnet.Cli.NewAsync("amusoft-cheatsheet", "--Author testUser --ProjectId TestId", CancellationToken.None);

			await Verifier.Verify(new
			{
				Log = loggingScope.ToFullString(PrintKind.All),
				Files = await scaffold.GetAllFileContentsAsync().ToListAsync()
			});
		}
	}
}