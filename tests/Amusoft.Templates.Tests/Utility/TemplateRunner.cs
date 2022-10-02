using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Amusoft.Templates.Tests.Toolkit;
using CliWrap;
using CliWrap.Buffered;
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

			var executionTimeout = timeout == default ? TimeSpan.FromSeconds(3) : timeout;
			using var cts = new CancellationTokenSource(executionTimeout);
			var command = await Cli.Wrap("dotnet")
				.WithArguments(processArguments)
				.WithValidation(CommandResultValidation.None)
				.ExecuteBufferedAsync(cts.Token);

			Log.Trace("Output: {Content}", command.StandardOutput);
			Log.Trace("Error: {Content}", command.StandardError);

			ErrorContent = command.StandardError;
			OutputContent = command.StandardOutput;
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
