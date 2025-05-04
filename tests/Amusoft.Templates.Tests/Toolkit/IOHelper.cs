using System.IO;

namespace Amusoft.Templates.Tests.Toolkit;

public static class IOHelper
{
	public static string CreateTempDirectory()
	{
		var path = Path.GetTempFileName();
		File.Delete(path);
		Directory.CreateDirectory(path);
		return path;
	}
}