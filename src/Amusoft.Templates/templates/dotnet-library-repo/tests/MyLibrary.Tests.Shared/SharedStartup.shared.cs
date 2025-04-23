using System.Runtime.CompilerServices;
 
namespace MyLibrary.Tests.Shared;

public class SharedStartup
{
	[ModuleInitializer]
	public static void Initialize()
	{
		VerifyInitializer.Initialize();
	}
}