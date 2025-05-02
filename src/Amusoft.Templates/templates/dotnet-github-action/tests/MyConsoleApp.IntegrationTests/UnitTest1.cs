using Microsoft.Extensions.Logging.Testing;

using MyConsoleApp.Tests.Shared;

using Shouldly;

using Xunit;

namespace MyConsoleApp.IntegrationTests;

public class UnitTest1
{
	private readonly ITestOutputHelper _outputHelper;

	[Fact]
	public async Task Test1()
	{
		Environment.SetEnvironmentVariable("IS_TEST", "true");
		var exitCode = (int?)null;
		ExitEnvironment.OnExit += (sender, i) => exitCode = i; 
		var w = Path.GetTempPath();
		var logger = new FakeLogger<CoreProcedure>();
		await CoreProcedure.ExecuteAsync(logger, new ActionInputs()
		{
			WorkspaceDirectory = w,
		}, TestContext.Current.CancellationToken);
	    
		logger.Collector.Count.ShouldBeGreaterThan(0);
		exitCode.ShouldNotBeNull();
		exitCode.ShouldBe(0);
	}

	public UnitTest1(ITestOutputHelper outputHelper)
	{
		_outputHelper = outputHelper;
	}
}