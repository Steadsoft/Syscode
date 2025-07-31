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

    [TestClass]
    public sealed class LiteralTests
    {
        [TestMethod]
        public void Test1()
        {
            var constant = new NumericConstant("123", Operator.UNDEFINED);

            Assert.AreEqual(Base.DEC, constant.Base);
            Assert.AreEqual(DataType.UDEC, constant.DataType);
            Assert.AreEqual(Scale.FIXED, constant.Scale);
            Assert.AreEqual(3, constant.Precision);
            Assert.AreEqual(0, constant.Scalefactor);
        }

        [TestMethod]
        public void Test2()
        {
            var constant = new NumericConstant("123.456", Operator.UNDEFINED);

            Assert.AreEqual(Base.DEC, constant.Base);
            Assert.AreEqual(DataType.UDEC, constant.DataType);
            Assert.AreEqual(Scale.FIXED, constant.Scale);
            Assert.AreEqual(6, constant.Precision);
            Assert.AreEqual(3, constant.Scalefactor);
        }

        [TestMethod]
        public void Test3()
        {
            var constant = new NumericConstant("123.456e+2", Operator.UNDEFINED);

            Assert.AreEqual(Base.DEC, constant.Base);
            Assert.AreEqual(DataType.UDEC, constant.DataType);
            Assert.AreEqual(Scale.FLOAT, constant.Scale);
            Assert.AreEqual(0, constant.Precision);
            Assert.AreEqual(0, constant.Scalefactor);
        }

        [TestMethod]
        public void Test4()
        {
            var constant = new NumericConstant("1101.1110 [b]", Operator.UNDEFINED);

            Assert.AreEqual(Base.BIN, constant.Base);
            Assert.AreEqual(DataType.UBIN, constant.DataType);
            Assert.AreEqual(Scale.FIXED, constant.Scale);
            Assert.AreEqual(8, constant.Precision);
            Assert.AreEqual(4, constant.Scalefactor);
        }

        [TestMethod]
        public void Test5()
        {
            var constant = new NumericConstant("23.7665 [h]", Operator.UNDEFINED);

            Assert.AreEqual(Base.HEX, constant.Base);
            Assert.AreEqual(DataType.UBIN, constant.DataType);
            Assert.AreEqual(Scale.FIXED, constant.Scale);
            Assert.AreEqual(24, constant.Precision);
            Assert.AreEqual(16, constant.Scalefactor);
        }

        [TestMethod]
        public void Test6()
        {
            var constant = new NumericConstant("23.7665 [o]", Operator.PLUS);

            Assert.AreEqual(Base.OCT, constant.Base);
            Assert.AreEqual(DataType.BIN, constant.DataType);
            Assert.AreEqual(Scale.FIXED, constant.Scale);
            Assert.AreEqual(18, constant.Precision);
            Assert.AreEqual(12, constant.Scalefactor);
        }

        [TestMethod]
        public void Test7()
        {
            var constant = new NumericConstant("1.00101010p+3", Operator.PLUS);

            Assert.AreEqual(Base.HEX, constant.Base);
            Assert.AreEqual(DataType.BIN, constant.DataType);
            Assert.AreEqual(Scale.FLOAT, constant.Scale);
            Assert.AreEqual(0, constant.Precision);
            Assert.AreEqual(0, constant.Scalefactor);
        }


    }
}
