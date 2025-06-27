using System.Diagnostics;
using System.Diagnostics.Metrics;

namespace Syscode
{
    public class Syscode
    {
        public static void Main()
        {

            AstNode ast = null;
            var compiler = new SyscodeCompiler();

            var cst = compiler.CompileSourceFile(@"..\..\..\..\TestSource\bulktest.sys");

            //compiler.PrintConcreteSyntaxTree(cst);

            var clock = new Stopwatch();

            clock.Restart();

            for (int I=0; I < 100; I++)
            {
                ast = compiler.GenerateAbstractSyntaxTree(cst);
            }

            clock.Stop();

            compiler.PrintAbstractSyntaxTree(ast);

            var types = compiler.GetLLVMStructTypes(ast);

            Console.WriteLine();
            Console.WriteLine("LLVM TYPES");
            Console.WriteLine();

            foreach (var type in types)
            {
                Console.WriteLine($"{type.Item1} = type {type.Item2}");
            }
        }
    }
}