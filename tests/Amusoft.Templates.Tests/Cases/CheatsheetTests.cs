using System.IO;
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
	public class CheatsheetTests : TemplateTests
	{
		public CheatsheetTests(ITestOutputHelper outputHelper, GlobalSetupFixture data) : base(outputHelper, data)
		{
		}

		[Fact]
		public async Task FileStructureTest()
		{
			using (new TemplateInstallationSession(Path.Combine(GetTemplateRootPath(), "cheatsheet")))
			{
				using var dryRunner = new TemplateRunner("amusoft-cheatsheet");
				var sb = new StringBuilder();
				sb.Append(" --Author testUser --ProjectId TestId");
				await dryRunner.ExecuteAsync(sb.ToString());

				await Verifier.Verify(new[] { dryRunner.OutputContent, dryRunner.ErrorContent });
			}
		}
	}
}