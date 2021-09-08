using Xunit;
using Xunit.Abstractions;

namespace Amusoft.Templates.Tests.Toolkit
{
	public class TestBase : IClassFixture<GlobalSetupFixture>
	{
		private readonly GlobalSetupFixture _data;

		public TestBase(ITestOutputHelper outputHelper, GlobalSetupFixture data)
		{
			_data = data;
			XUnitOutputTarget.OutputHelper = outputHelper;
		}
	}
}