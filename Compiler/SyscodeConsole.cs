using System.ComponentModel;
using System.Diagnostics;

namespace Syscode
{
    public class SyscodeConsole
    {
        public static void Main()
        {

            AstNode ast = null;
            var compiler = new SyscodeCompiler(@"..\..\..\..\TestSource\messages.json");

            compiler.diagnostics += onFileFound;

            var cst = compiler.CompileSourceFile(@"..\..\..\..\TestSource\statements.sys");

            //compiler.PrintConcreteSyntaxTree(cst);

            var clock = new Stopwatch();

            clock.Restart();

            ast = compiler.GenerateAbstractSyntaxTree(cst);

            compiler.ProcessDeclarations(ast);

            compiler.ResolveReferences(ast);

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
               Console.ForegroundColor = ConsoleColor.Red;
            if (eventArgs.severity == "Warning")
                Console.ForegroundColor = ConsoleColor.Yellow;
            if (eventArgs.severity == "Info")
                Console.ForegroundColor = ConsoleColor.Gray;

            Console.Write($"{eventArgs.severity}  {eventArgs.code} on line {eventArgs.line.ToString().PadRight(5)} ");
            Console.ResetColor();
            Console.WriteLine(message);
        };
    }
}