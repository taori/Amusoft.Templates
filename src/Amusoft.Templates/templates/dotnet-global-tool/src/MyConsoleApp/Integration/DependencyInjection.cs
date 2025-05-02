// This file is licensed to you under the MIT license.

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Spectre.Console.Cli;

namespace MyConsoleApp.Integration;

internal sealed class TypeRegistrar : ITypeRegistrar
{
	private readonly IHostBuilder _builder;

	public TypeRegistrar(IHostBuilder builder)
	{
		_builder = builder;
	}

	public ITypeResolver Build()
	{
		return new TypeResolver(_builder.Build());
	}

	public void Register(Type service, Type implementation)
	{
		_builder.ConfigureServices((_, services) => services.AddSingleton(service, implementation));
	}

	public void RegisterInstance(Type service, object implementation)
	{
		_builder.ConfigureServices((_, services) => services.AddSingleton(service, implementation));
	}

	public void RegisterLazy(Type service, Func<object> func)
	{
		if (func is null) throw new ArgumentNullException(nameof(func));

		_builder.ConfigureServices((_, services) => services.AddSingleton(service, _ => func()));
	}
}

internal sealed class TypeResolver : ITypeResolver, IDisposable
{
	private readonly IHost _host;

	public TypeResolver(IHost provider)
	{
		_host = provider ?? throw new ArgumentNullException(nameof(provider));
	}

	public object? Resolve(Type? type)
	{
		return type != null ? _host.Services.GetService(type) : null;
	}

	public void Dispose()
	{
		_host.Dispose();
	}
}