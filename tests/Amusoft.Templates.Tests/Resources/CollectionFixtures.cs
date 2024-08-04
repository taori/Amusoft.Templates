using System.IO;
using Amusoft.Templates.Tests.Toolkit;
using Amusoft.Templates.Tests.Utility;
using Xunit;

namespace Amusoft.Templates.Tests.Resources;

public class DotnetLibraryRepoSession : TemplateInstallationSession
{
	public DotnetLibraryRepoSession() : base(Path.Combine(TemplatingHelper.GetPathToTemplate(), "dotnet-library-repo") )
	{
	}
}

public class DotnetTemplateRepoSession : TemplateInstallationSession
{
	public DotnetTemplateRepoSession() : base(Path.Combine(TemplatingHelper.GetPathToTemplate(), "dotnet-template"))
	{
	}
}

public class CheatsheetSession : TemplateInstallationSession
{
	public CheatsheetSession() : base(Path.Combine(TemplatingHelper.GetPathToTemplate(), "cheatsheet"))
	{
	}
}