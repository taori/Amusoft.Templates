
using System.IO;
using System.Runtime.CompilerServices;
using VerifyXunit;
using Xunit;

[assembly: CollectionBehavior(DisableTestParallelization = true)]
namespace Amusoft.Templates.Tests.Configuration
{
	public class ProjectSetup
	{
		[ModuleInitializer]
		public static void Initialize()
		{
			Verifier.DerivePathInfo(
				(sourceFile, projectDirectory, type, method) => new(
					directory: Path.Combine(projectDirectory, "Snapshots"),
					typeName: type.Name,
					methodName: method.Name));
		}
	}
}