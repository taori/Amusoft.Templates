using System.Runtime.CompilerServices;
 
namespace MyConsoleApp.Tests.Shared;

public class SharedStartup
{
	[ModuleInitializer]
	public static void Initialize()
	{
		VerifyInitializer.Initialize();
	}
}