using System.Data.Common;
using Amusoft.XUnit.NLog.Extensions;
using MyLibrary.Tests.Shared;
using MyLibrary.Tests.Shared.Toolkit;
using Xunit;
using Xunit.Abstractions;
using NLog.Fluent;

namespace MyLibrary.IntegrationTests;

public class UnitTest1 : TestBase
{
    [Fact]
    public void Test1()
    {
        Log.Error("test");
    }

    public UnitTest1(ITestOutputHelper outputHelper, AssemblyInitializer data) : base(outputHelper, data)
    {
    }
}