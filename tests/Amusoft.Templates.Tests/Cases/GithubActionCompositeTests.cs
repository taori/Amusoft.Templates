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
	public class GithubActionCompositeTests : TemplateTests
	{
		public GithubActionCompositeTests(ITestOutputHelper outputHelper, GlobalSetupFixture data) : base(outputHelper, data)
		{
		}

		[Fact(Timeout = 15_000)]
		public async Task MandatoryParameters()
		{
			using (var loggingScope = new LoggingScope())
			{
				var ex = await Assert.ThrowsAsync<ScaffoldingFailedException>(async() => await Dotnet.Cli.NewAsync("github-action-composite", "", CancellationToken.None));
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
			var args = "-n GeneratedProject --ActionName TheActionName --GitRepository TheGitRepository --GitOwner TheGitOwner";
			using var scaffold = await Dotnet.Cli.NewAsync("github-action-composite", args, CancellationToken.None);

			await Verifier.Verify(await scaffold.GetAllFileContentsAsync().ToListAsync());
		}
	}
}