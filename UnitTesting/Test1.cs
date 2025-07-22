using Syscode;
using System.Diagnostics;

namespace UnitTesting
{
    [TestClass]
    public sealed class Test1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var compiler = new SyscodeCompiler(@"..\..\..\..\TestSource\messages.json");
            var cst = compiler.CompileSourceFile(@"..\..\..\Syscode\builtin.sys");
            var ast = compiler.GenerateAbstractSyntaxTree(cst);
            compiler.ProcessDeclarations(ast);
            compiler.ResolveCompilationReferences(ast);
            Assert.AreEqual(1015, compiler.Reporter.Messages.First().code);
        }
    }
}
