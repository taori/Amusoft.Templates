using System.Reflection;
using Amusoft.XUnit.NLog.Extensions;
using NLog;
using Xunit;
using Xunit.Abstractions;

namespace Amusoft.Templates.Tests.Toolkit
{
	public class TemplateTests : LoggedTestBase, IClassFixture<GlobalSetupFixture>
	{
		public TemplateTests(ITestOutputHelper outputHelper, GlobalSetupFixture data) : base(outputHelper)
		{
		}

		protected EmbeddedResourceReader GetEmbeddedResourceReader() =>
			new EmbeddedResourceReader(Assembly.GetExecutingAssembly());

		protected string GetTemplateRootPath() => GetEmbeddedResourceReader().GetContent("Resources.Embedded.templatesRoot.txt").Trim();
	}
}