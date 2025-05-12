// This file is licensed to you under the MIT license.

using System;

namespace MyConsoleApp;

internal static class ExitEnvironment
{
	public static EventHandler<int>? OnExit;
	
	public static void WithCode(int code)
	{
		if (Environment.GetEnvironmentVariable("IS_TEST") == "true")
		{
			OnExit?.Invoke(null, code);
			return;
		}
		
		Environment.Exit(code);
	}
}