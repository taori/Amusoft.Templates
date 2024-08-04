using System.Reflection;
using Amusoft.XUnit.NLog.Extensions;
using Xunit;
using Xunit.Abstractions;

namespace Amusoft.Templates.Tests.Toolkit
{
	public partial class TemplateTests : LoggedTestBase, IClassFixture<GlobalSetupFixture>
	{
		public TemplateTests(ITestOutputHelper outputHelper, GlobalSetupFixture data) : base(outputHelper)
		{
		}

		protected EmbeddedResourceReader GetEmbeddedResourceReader() => new(Assembly.GetExecutingAssembly());

		protected string GetTemplateRootPath() => GetEmbeddedResourceReader().GetContent("Resources.Embedded.templatesRoot.txt").Trim();
	}
}