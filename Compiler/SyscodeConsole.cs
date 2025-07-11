using System.ComponentModel;
using System.Diagnostics;

namespace Syscode
{
    public class SyscodeConsole
    {
        static    int errors; static int warns; static int infos;
        
        public static void Main()
        {
            Console.Clear();

            AstNode ast = null;
            var compiler = new SyscodeCompiler(@"..\..\..\..\TestSource\messages.json");

            compiler.diagnostics += onFileFound;

            var cst = compiler.CompileSourceFile(@"..\..\..\..\TestSource\resolve.sys");

            //compiler.PrintConcreteSyntaxTree(cst);

            var clock = new Stopwatch();

            clock.Restart();

            ast = compiler.GenerateAbstractSyntaxTree(cst);

            compiler.ProcessDeclarations(ast);

            compiler.ResolveCompilationReferences(ast);

            compiler.PrintDiagnostics();

            Console.WriteLine();
            Console.WriteLine($"ERRORS {errors}, WARNINGS {warns}, INFOS {infos}");

            clock.Stop();

            //compiler.PrintAbstractSyntaxTree(ast);

            //var types = compiler.GetLLVMStructTypes(ast);

            //Console.WriteLine();
            //Console.WriteLine("LLVM TYPES");
            //Console.WriteLine();

            //foreach (var type in types)
            //{
            //    Console.WriteLine($"{type.Item1} = type {type.Item2}");
            //}
        }

        public static EventHandler<DiagnosticEvent> onFileFound = (sender, eventArgs) =>
        {
            string message = $"{eventArgs.message}";

            if (eventArgs.severity == "Error")
            {
               Console.ForegroundColor = ConsoleColor.Red;
                errors++;
            }


            if (eventArgs.severity == "Warning")
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                warns++;
            }


            if (eventArgs.severity == "Info")
            {
                Console.ForegroundColor = ConsoleColor.Gray;
                infos++;
            }



            Console.Write($"{eventArgs.severity}  {eventArgs.code} on line {eventArgs.line.ToString().PadRight(5)} ");
            Console.ResetColor();
            Console.WriteLine(message);
           
        };
    }
}