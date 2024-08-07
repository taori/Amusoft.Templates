using System;

namespace Amusoft.Templates.Tests.Utility
{
	public class TemplateInstallationSession : IDisposable
	{
		private readonly TemplateInstaller _installer;

		public TemplateInstallationSession(string templatePath)
		{
			TemplatePath = templatePath;
			_installer = new TemplateInstaller(templatePath);
			_installer.Install();
		}

		public string TemplatePath { get; }

		public void Dispose()
		{
			_installer.Uninstall();
		}
	}
}