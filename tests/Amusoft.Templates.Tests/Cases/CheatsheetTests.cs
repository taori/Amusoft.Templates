using System.IO;
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
	public class CheatsheetTests : TemplateTests, IClassFixture<CheatsheetSession>
	{
		private readonly CheatsheetSession _session;

		public CheatsheetTests(ITestOutputHelper outputHelper, GlobalSetupFixture data, CheatsheetSession session) : base(outputHelper, data)
		{
			_session = session;
		}

		[Fact]
		public async Task FileStructureTest()
		{
			using var dryRunner = new TemplateRunner("amusoft-cheatsheet");
			var sb = new StringBuilder();
			sb.Append(" --Author testUser --ProjectId TestId");
			await dryRunner.ExecuteAsync(sb.ToString());

			await Verifier.Verify(new[] { dryRunner.OutputContent, dryRunner.ErrorContent });
		}
	}
}