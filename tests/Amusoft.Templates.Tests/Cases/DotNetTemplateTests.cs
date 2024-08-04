using System.IO;
using System.Threading.Tasks;
using Amusoft.Templates.Tests.Resources;
using Amusoft.Templates.Tests.Toolkit;
using Amusoft.Templates.Tests.Utility;
using VerifyXunit;
using Xunit;
using Xunit.Abstractions;

namespace Amusoft.Templates.Tests.Cases
{
	public class DotNetTemplateTests : TemplateTests, IClassFixture<DotnetTemplateRepoSession>
	{
		private readonly DotnetTemplateRepoSession _session;

		public DotNetTemplateTests(ITestOutputHelper outputHelper, GlobalSetupFixture data, DotnetTemplateRepoSession session) : base(outputHelper, data)
		{
			_session = session;
		}

		[Fact]
		public async Task FileStructureTest()
		{
			using (new TemplateInstallationSession(Path.Combine(GetTemplateRootPath(), "dotnet-template")))
			{
				using var dryRunner = new TemplateRunner("dotnet-template");
				await dryRunner.ExecuteAsync("-au testauthor");

				await Verifier.Verify(new[] { dryRunner.ErrorContent, dryRunner.OutputContent });
			}
		}

		[Fact]
		public async Task CheckAuthorIsRequired()
		{
			using (new TemplateInstallationSession(Path.Combine(GetTemplateRootPath(), "dotnet-template")))
			{
				using var dryRunner = new TemplateRunner("dotnet-template");
				await dryRunner.ExecuteAsync(string.Empty);

				await Verifier.Verify(new[] { dryRunner.ErrorContent, dryRunner.OutputContent });
			}
		}
	}
}