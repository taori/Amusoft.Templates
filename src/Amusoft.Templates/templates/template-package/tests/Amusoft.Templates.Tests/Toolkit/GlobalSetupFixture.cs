using System;
using System.Threading;
using System.Threading.Tasks;
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
			var er = new EmbeddedResourceReader(typeof(GlobalSetupFixture).Assembly);
			var searchDirectoryStart = er.GetContent("Resources.Embedded.solutionFile.txt").TrimEnd();
			_solutionFile = new TemplateSolution(searchDirectoryStart);
			_installation = _solutionFile.InstallTemplatesFromDirectoryAsync("Amusoft.Templates/templates", CancellationToken.None).GetAwaiter().GetResult();
		}

		public void Dispose()
		{
			_installation.Dispose();
		}
	}
}