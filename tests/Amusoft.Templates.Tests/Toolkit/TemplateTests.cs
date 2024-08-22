using System.Reflection;
using Amusoft.XUnit.NLog.Extensions;
using Xunit;
using Xunit.Abstractions;

namespace Amusoft.Templates.Tests.Toolkit
{
	[Collection("AssemblyInitializer")]
	public partial class TemplateTests : LoggedTestBase, IClassFixture<GlobalSetupFixture>
	{
		public TemplateTests(ITestOutputHelper outputHelper, GlobalSetupFixture data) : base(outputHelper)
		{
			OutputHelper = outputHelper;
		}

		public ITestOutputHelper OutputHelper { get; }
	}
}