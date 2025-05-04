using System.Threading.Tasks;
using MyRoslynPackage.Analyzers;
using MyRoslynPackage.Codefixes;
using MyRoslynPackage.Test.Utils;
using Xunit;

namespace MyRoslynPackage.Test;

public class MyRoslynPackageCodeFixTests : CustomCodeFixTest<MyRoslynPackageAnalyzer, MyRoslynPackageCodeFixProvider>
{
    [Fact]
    public async Task VerifyRewrite()
    {
        /* lang=c#-test */
        const string text = """
        using System;

        namespace S4u.Analyzers.Sample;

        public class [|Logging|]
        {
        }
        public class [|Logging2|]
        {
        }
        """;

        /* lang=c#-test */
        const string fixedText = """
         using System;
         
         namespace S4u.Analyzers.Sample;
         
         public class LOGGING
         {
         }
         public class [|Logging2|]
         {
         }
         """;
        
        /* lang=c#-test */
        const string batchfixedText = """
         using System;
         
         namespace S4u.Analyzers.Sample;
         
         public class LOGGING
         {
         }
         public class LOGGING2
         {
         }
         """;

        
        var test = CreateCodeFix(text, fixedText, batchfixedText, 0);
        test.NumberOfIncrementalIterations = 1;
        test.NumberOfFixAllIterations = 1;
        await test.RunAsync();
    }
}