using System;
using System.Diagnostics;

namespace Amusoft.Templates.Tests.Toolkit
{
	public class ObservableProcessFactory
	{
		public static Process Create(string fileName, string arguments)
		{
			var process = new Process();
			process.StartInfo.FileName = fileName;
			process.StartInfo.Arguments = arguments;
			process.StartInfo.RedirectStandardError = true;
			process.StartInfo.RedirectStandardOutput = true;
			process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
			process.StartInfo.CreateNoWindow = true;
			process.StartInfo.UseShellExecute = false;
			process.EnableRaisingEvents = false;

			return process;
		}
	}
}