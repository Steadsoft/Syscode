using Syscode;
using System.Diagnostics;

namespace UnitTesting
{
    [TestClass]
    public sealed class BasicTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            var compiler = new SyscodeCompiler(@"..\..\..\..\TestSource\messages.json");
            var cst = compiler.ParseSourceFile(@"..\..\..\Syscode\builtin.sys");
            var ast = compiler.GenerateAbstractSyntaxTree(cst);
            compiler.ProcessDeclarations(ast);
            compiler.ResolveCompilationReferences(ast);
            Assert.AreEqual(1015, compiler.Reporter.Messages.First().code);
        }

        [TestMethod]
        public void TestMethod2()
        {
            var compiler = new SyscodeCompiler(@"..\..\..\..\TestSource\messages.json");
            var cst = compiler.ParseSourceFile(@"..\..\..\Syscode\unresolved.sys");
            var ast = compiler.GenerateAbstractSyntaxTree(cst);
            compiler.ProcessDeclarations(ast);
            compiler.ResolveCompilationReferences(ast);
            Assert.AreEqual(2, compiler.Reporter.Messages.Where(m => m.code == 1000).ToList().Count);
        }

    }
}
