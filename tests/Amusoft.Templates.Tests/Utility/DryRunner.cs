using System;
using System.Threading.Tasks;
using Amusoft.Templates.Tests.Toolkit;
using NLog;

namespace Amusoft.Templates.Tests.Utility
{
	public class DryRunner
	{
		private static readonly Logger Log = LogManager.GetLogger(nameof(DryRunner));

		public string TemplateName { get; }

		public string OutputContent { get; set; }

		public string ErrorContent { get; set; }

		public DryRunner(string templateName)
		{
			TemplateName = templateName;
		}

		public async Task ExecuteAsync(string arguments, TimeSpan timeout = default)
		{
			var processArguments = $"new {TemplateName} {arguments} --dry-run -o C:\\tmp";
			Log.Debug("Executing dotnet {Arguments}", processArguments);

			var processRunner = new SimpleProcessRunner("dotnet", processArguments);
			var executionTimeout = timeout == default ? TimeSpan.FromSeconds(3) : timeout;
			await processRunner.ExecuteAsync(executionTimeout);

			Log.Trace("Output: {Content}", processRunner.OutputContent);
			Log.Trace("Error: {Content}", processRunner.ErrorContent);

			ErrorContent = processRunner.ErrorContent;
			OutputContent = processRunner.OutputContent;
		}
	}
}