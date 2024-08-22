using System.Reflection;
using Amusoft.DotnetNew.Tests.Diagnostics;
using Amusoft.DotnetNew.Tests.Exceptions;

namespace MyLibrary.Tests.Shared;

public class VerifyInitializer
{
	public static void Initialize()
	{
		Verifier.DerivePathInfo(PathInfoConfiguration);
		VerifierSettings.ScrubMember<Exception>(nameof(Exception.StackTrace));
		VerifierSettings.ScrubMember<CommandResult>(nameof(CommandResult.Runtime));
	}

	private static PathInfo PathInfoConfiguration(string sourcefile, string projectdirectory, Type type, MethodInfo method)
	{
		return new PathInfo(
			directory: Path.Combine(projectdirectory, "Snapshots"),
			typeName: type.Name,
			methodName: method.Name
		);
	}
}