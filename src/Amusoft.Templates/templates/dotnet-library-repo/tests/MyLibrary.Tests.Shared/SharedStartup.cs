using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Amusoft.XUnit.NLog.Extensions;
using Shared.TestSdk.Initializers;
using Xunit;
using Xunit.Abstractions;
 
namespace MyLibrary.Tests.Shared
{
	public class SharedStartup
	{
		[ModuleInitializer]
		public static void Initialize()
		{
			VerifyInitializer.Initialize();
		}
	}
	
	[Collection("AssemblyInitializer")]
	public class TestBase : LoggedTestBase
	{
		private readonly AssemblyInitializer _data;

		protected TestBase(ITestOutputHelper outputHelper, AssemblyInitializer data) : base(outputHelper)
		{
			_data = data;
		}
	}
	
	[CollectionDefinition("AssemblyInitializer", DisableParallelization = true)]
	public class AssemblyInitializer: IAsyncLifetime, ICollectionFixture<AssemblyInitializer>
	{
		public Task InitializeAsync()
		{
			return Task.CompletedTask;
		}

		public Task DisposeAsync()
		{
			return Task.CompletedTask;
		}
	}
}