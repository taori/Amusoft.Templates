using System.IO;
using System.Text;
using System.Threading.Tasks;
using Amusoft.Templates.Tests.Toolkit;
using Amusoft.Templates.Tests.Utility;
using Shouldly;
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

				dryRunner.ErrorContent.ShouldBeEmpty();
				dryRunner.OutputContent.ShouldNotBeEmpty();
				dryRunner.OutputContent.ShouldContain("Create: ./cheatsheet.txt");
			}
		}
	}
}