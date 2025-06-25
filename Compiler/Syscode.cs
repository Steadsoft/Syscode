using System.Diagnostics.Metrics;

namespace Syscode
{
    public class Syscode
    {
        public static void Main()
        {
            var compiler = new SyscodeCompiler();

            var cst = compiler.CompileSourceFile(@"..\..\..\..\TestSource\statements.sys");

            //compiler.PrintConcreteSyntaxTree(cst);

            var ast = compiler.GenerateAbstractSyntaxTree(cst);

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