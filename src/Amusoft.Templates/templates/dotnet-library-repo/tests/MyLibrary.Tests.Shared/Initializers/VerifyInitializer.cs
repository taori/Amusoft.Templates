﻿using System.Reflection;

namespace MyLibrary.Tests.Shared.Initializers;

public class VerifyInitializer
{
	public static void Initialize()
	{
		Verifier.DerivePathInfo(PathInfoConfiguration);
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