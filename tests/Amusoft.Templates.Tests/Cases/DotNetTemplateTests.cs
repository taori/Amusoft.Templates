using System.IO;
using System.Threading.Tasks;
using Amusoft.Templates.Tests.Toolkit;
using Amusoft.Templates.Tests.Utility;
using Shouldly;
using Xunit;
using Xunit.Abstractions;

namespace Amusoft.Templates.Tests.Cases
{
	public class DotNetTemplateTests : TemplateTests
	{
		[Fact]
		public async Task FileStructureTest()
		{
			using (new TemplateInstallationSession(Path.Combine(GetTemplateRootPath(), "dotnet-template")))
			{
				using var dryRunner = new TemplateRunner("dotnet-template");
				await dryRunner.ExecuteAsync("-au testauthor");

				dryRunner.OutputContent.ShouldNotBeEmpty();
				dryRunner.ErrorContent.ShouldBeEmpty();
				dryRunner.OutputContent.ShouldEndWith(@"File actions would have been taken:
  Create: ./.template.config/template.json
");
			}
		}

		[Fact]
		public async Task CheckAuthorIsRequired()
		{
			using (new TemplateInstallationSession(Path.Combine(GetTemplateRootPath(), "dotnet-template")))
			{
				using var dryRunner = new TemplateRunner("dotnet-template");
				await dryRunner.ExecuteAsync(string.Empty);

				dryRunner.OutputContent.ShouldBeEmpty();
				dryRunner.ErrorContent.ShouldNotBeEmpty();
				dryRunner.ErrorContent.ShouldContain("Mandatory option --param:author missing");
			}
		}

		public DotNetTemplateTests(ITestOutputHelper outputHelper, GlobalSetupFixture data) : base(outputHelper, data)
		{
		}
	}
}
