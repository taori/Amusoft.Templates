using System;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using DiffEngine;
using VerifyTests;
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
			DiffTools.UseOrder(DiffTool.TortoiseGitMerge, DiffTool.VisualStudioCode, DiffTool.WinMerge);
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
}