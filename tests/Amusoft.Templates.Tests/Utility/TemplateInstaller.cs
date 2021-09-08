using System.Diagnostics;
using NLog;

namespace Amusoft.Templates.Tests.Utility
{
	public class TemplateInstaller
	{
		private static readonly Logger Log = LogManager.GetLogger(nameof(TemplateInstaller));

		public string TemplatePath { get; }

		public TemplateInstaller(string templatePath)
		{
			TemplatePath = templatePath;
		}

		public void Install()
		{
			Log.Debug("Installing template {Path}", TemplatePath);
			using var process = new Process();
			process.StartInfo = new ProcessStartInfo("dotnet", $"new -i \"{TemplatePath}\"");
			process.Start();
			process.WaitForExit(10000);
		}

		public void Uninstall()
		{
			Log.Debug("Uninstalling template {Path}", TemplatePath);
			using var process = new Process();
			process.StartInfo = new ProcessStartInfo("dotnet", $"new -u \"{TemplatePath}\"");
			process.Start();
			process.WaitForExit(10000);
		}
	}
}