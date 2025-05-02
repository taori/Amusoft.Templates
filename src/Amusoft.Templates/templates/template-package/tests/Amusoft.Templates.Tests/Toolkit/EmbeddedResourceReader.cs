using System;
using System.IO;
using System.Reflection;

namespace Amusoft.Templates.Tests.Toolkit;

public class EmbeddedResourceReader
{
	private readonly Assembly _assembly;

	public EmbeddedResourceReader(Assembly assembly)
	{
		_assembly = assembly;
	}

	public string GetContent(string accessPath)
	{
		try
		{
			using var reader = GetStream(accessPath);
			if (reader is null)
				throw new Exception($"{accessPath} not found");
			
			using var streamReader = new StreamReader(reader);
			return streamReader.ReadToEnd();
		}
		catch (Exception e)
		{
			throw new Exception($"Failed to get content for accessPath {accessPath}", e);
		}
	}

	private Stream? GetStream(string accessPath)
	{
		var fullPath = _assembly.GetName().Name + "." + accessPath;
		return _assembly.GetManifestResourceStream(fullPath);
	}
}