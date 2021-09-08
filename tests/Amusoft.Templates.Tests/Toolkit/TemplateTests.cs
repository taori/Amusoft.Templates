using System.Reflection;
using Xunit.Abstractions;

namespace Amusoft.Templates.Tests.Toolkit
{
	public class TemplateTests : TestBase
	{
		public TemplateTests(ITestOutputHelper outputHelper, GlobalSetupFixture data) : base(outputHelper, data)
		{
		}

		protected EmbeddedResourceReader GetEmbeddedResourceReader() =>
			new EmbeddedResourceReader(Assembly.GetExecutingAssembly());

		protected string GetTemplateRootPath() => GetEmbeddedResourceReader().GetContent("Resources.Embedded.templatesRoot.txt").Trim();
	}
}