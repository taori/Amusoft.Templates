using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Amusoft.DotnetNew.Tests.Diagnostics;
using Amusoft.DotnetNew.Tests.Scopes;
using Amusoft.DotnetNew.Tests.Templating;
using Xunit;

namespace Amusoft.Templates.Tests.Toolkit
{
	[CollectionDefinition("AssemblyInitializer")]
	public class GlobalSetupFixture : IDisposable
	{
		private readonly TemplateSolution _solutionFile;
		private readonly TemplateInstallationGroup _installation;

		public GlobalSetupFixture()
		{
			using (var scope = new LoggingScope())
			{
				_solutionFile = new TemplateSolution(typeof(GlobalSetupFixture).Assembly.Location, 6, "All.sln");
				_installation = _solutionFile.InstallTemplatesFromDirectoryAsync("Amusoft.Templates/templates", CancellationToken.None).GetAwaiter().GetResult();
				Debug.WriteLine(scope.ToFullString(PrintKind.All));
			}
		}

		public void Dispose()
		{
			_installation.Dispose();
		}
	}
}