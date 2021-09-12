using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amusoft.Templates.Tests.Toolkit;
using NLog;

namespace Amusoft.Templates.Tests.Utility
{
	public class TemplateRunner : IDisposable
	{
		private static readonly Logger Log = LogManager.GetLogger(nameof(TemplateRunner));

		private readonly bool _dryRun;

		public string TemplateName { get; }

		public string OutputContent { get; set; }

		public string ErrorContent { get; set; }

		public TemplateRunner(string templateName, bool dryRun = true)
		{
			_dryRun = dryRun;
			TemporaryDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N")).TrimEnd(Path.DirectorySeparatorChar);

			TemplateName = templateName;
		}

		public string TemporaryDirectory { get; }

		public async Task ExecuteAsync(string arguments, TimeSpan timeout = default)
		{
			var argumentBuilder = new StringBuilder();
			argumentBuilder.Append($"new {TemplateName} {arguments}");
			ApplyConfiguration(argumentBuilder);

			var processArguments = argumentBuilder.ToString();

			Log.Debug("Executing dotnet {Arguments}", processArguments);

			var processRunner = new SimpleProcessRunner("dotnet", processArguments);
			var executionTimeout = timeout == default ? TimeSpan.FromSeconds(3) : timeout;
			await processRunner.ExecuteAsync(executionTimeout);

			Log.Trace("Output: {Content}", processRunner.OutputContent);
			Log.Trace("Error: {Content}", processRunner.ErrorContent);

			ErrorContent = processRunner.ErrorContent;
			OutputContent = processRunner.OutputContent;
		}

		private void ApplyConfiguration(StringBuilder argumentBuilder)
		{
			if (_dryRun)
			{
				argumentBuilder.Append(" --dry-run -o C:\\tmp");
			}
			else
			{
				argumentBuilder.Append($" -o \"{TemporaryDirectory}\"");
			}
		}

		public void Dispose()
		{
			if (!_dryRun)
			{
				if (Directory.Exists(TemporaryDirectory))
					Directory.Delete(TemporaryDirectory, true);
			}
		}

		public string GetAbsoluteFilePath(string relativePath)
		{
			var absoluteUri = new Uri(new Uri(TemporaryDirectory + Path.DirectorySeparatorChar, UriKind.Absolute), new Uri(relativePath, UriKind.Relative));
			return absoluteUri.AbsolutePath;
		}
	}
}
