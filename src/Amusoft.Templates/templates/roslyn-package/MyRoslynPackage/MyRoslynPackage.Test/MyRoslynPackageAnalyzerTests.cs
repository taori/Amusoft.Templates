using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using VerifyCS = Microsoft.CodeAnalysis.CSharp.Testing.CSharpCodeFixVerifier<
    MyRoslynPackage.Analyzers.MyRoslynPackageAnalyzer, 
    MyRoslynPackage.Codefixes.MyRoslynPackageCodeFixProvider, 
    Microsoft.CodeAnalysis.Testing.DefaultVerifier>;

namespace MyRoslynPackage.Test
{
    [TestClass]
    public class MyRoslynPackageAnalyzerTests
    {
        //No diagnostics expected to show up
        [TestMethod]
        public async Task VerifyEmptyNoDiagnostics()
        {
            
            var test = @"";

            await VerifyCS.VerifyAnalyzerAsync(test);
        }

        //Diagnostic and CodeFix both triggered and checked for
        [TestMethod]
        public async Task VerifyCodefixSingle()
        {
            /* lang=c#-test */
            var test = @"
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Diagnostics;

    namespace ConsoleApplication1
    {
        class {|#0:TypeName|}
        {   
        }
    }";

            /* lang=c#-test */
            var fixtest = @"
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Diagnostics;

    namespace ConsoleApplication1
    {
        class TYPENAME
        {   
        }
    }";

            var expected = VerifyCS.Diagnostic("MyRoslynPackage").WithLocation(0).WithArguments("TypeName");
            await VerifyCS.VerifyCodeFixAsync(test, expected, fixtest);
        }
    }
}
