using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Amusoft.DotnetNew.Tests.CLI;
using Amusoft.DotnetNew.Tests.Diagnostics;
using Amusoft.DotnetNew.Tests.Exceptions;
using Amusoft.DotnetNew.Tests.Scopes;
using Amusoft.Templates.Tests.Toolkit;
using NLog.Fluent;
using Shouldly;
using VerifyXunit;
using Xunit;
using Xunit.Abstractions;

namespace Amusoft.Templates.Tests.Cases
{
	public class RoslynRepoWorkflowTests : TemplateTests
	{
		public RoslynRepoWorkflowTests(ITestOutputHelper outputHelper, GlobalSetupFixture data) : base(outputHelper, data)
		{
		}

		[Theory(Timeout = 15_000)]
		[InlineData("GeneratedProject")]
		[InlineData("GeneratedProject2")]
		public async Task FileStructureTest(string sourceName)
		{
			var sbArgs = new StringBuilder();
			sbArgs.Append($" -n \"{sourceName}\"");
			
			using var loggingScope = new LoggingScope();
			using var scaffold = await Dotnet.Cli.NewAsync("roslyn-repository-workflow", sbArgs.ToString(), CancellationToken.None);
			var files = scaffold.GetRelativeDirectoryPaths().ToList();

			await Verifier.Verify(files)
				.UseParameters(sourceName);
		}

		[Fact]
		public async Task ContentRewriteCheck()
		{
			var sbArgs = new StringBuilder();
			sbArgs.Append($" -n GeneratedProject");
			
			using var loggingScope = new LoggingScope();
			using var scaffold = await Dotnet.Cli.NewAsync("roslyn-repository-workflow", sbArgs.ToString(), CancellationToken.None);
			var contents = await scaffold.GetAllFileContentsAsync().ToListAsync();
			var contentsByName = contents.ToDictionary(d => d.RelativePath, d => d.Content);

			await Verifier.Verify(contentsByName);
		}
	}
}