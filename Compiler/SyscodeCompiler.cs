using Antlr4.Runtime;
using System.Text;
using System.Text.Json;
using static SyscodeParser;

namespace Syscode
{
    public class SyscodeCompiler
    {
        private SyscodeLexer lexer;
        private AstBuilder builder;
        private SymtabBuilder symtabBuilder;
        private ReferenceResolver resolver;
        public event EventHandler<DiagnosticEvent> diagnostics = delegate { };
        private string errorMesagesPath;
        private ErrorFile messages;
        private Reporter reporter;
        private string file;
        private string fileName;
        private string[] namespaceparts;
        private Dictionary<string, IConstant> constants = new();

        public Reporter Reporter { get => reporter; set => reporter = value; }

        public SyscodeCompiler(string ErrorMessagesPath)
        {
            errorMesagesPath = ErrorMessagesPath;

            string json = File.ReadAllText(errorMesagesPath);

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            messages = JsonSerializer.Deserialize<ErrorFile>(json, options);

            if (messages.Errors.GroupBy(m => m.Number).Where(g => g.Count() > 1).Any())
                throw new InvalidOperationException("The error file contains a duplicate error number.");

            if (messages.Errors.GroupBy(m => m.Message).Where(g => g.Count() > 1).Any())
                throw new InvalidOperationException("The error file contains a duplicate error message.");

        }

        public void Report(AstNode node, int number, string arg)
        {
            var errormsg = messages.Errors.Where(e => e.Number == number).Single();

            var message = errormsg.Message.Replace("{arg}", arg);

            diagnostics(this, new DiagnosticEvent(node, errormsg.Number, errormsg.Severity, message));
        }

        public CompilationContext ParseSourceFile(string SourceFile)
        {
            file = SourceFile;

            fileName = Path.GetFileNameWithoutExtension(file);

            // extract any namespace components from the file's name

            namespaceparts = fileName.Split('.');
            namespaceparts = namespaceparts.Take(namespaceparts.Length - 1).ToArray();

            Reporter = new Reporter(messages, diagnostics);
            var source = new StreamReader(SourceFile);
            var stream = new AntlrInputStream(source);
            lexer = new SyscodeLexer(stream);
            var tokens = new CommonTokenStream(lexer);
            var parser = new SyscodeParser(tokens);
            builder = new AstBuilder(constants, Reporter, parser);
            symtabBuilder = new SymtabBuilder(Reporter);
            resolver = new ReferenceResolver(Reporter);

            return parser.compilation();
        }

        public AstNode GenerateAbstractSyntaxTree(ParserRuleContext context)
        {
            return builder.Generate(context);
        }

        public void ProcessDeclarations(AstNode root)
        {
            symtabBuilder.Generate((Compilation)root);
        }

        public void ResolveCompilationReferences(AstNode root)
        {
            resolver.ResolveContainedReferences((Compilation)root);
            resolver.ReportUnresolvedReferences(((Compilation)root).Statements);
        }
        private string RemoveContext(string input)
        {
            return input.Replace("Context", "");
        }
        public string GetText(Antlr4.Runtime.Tree.ITerminalNode Node)
        {
            return Node.GetText();
        }
        public List<(string, string)> GetLLVMStructTypes(AstNode node)
        {
            List<(string, string)> types = new List<(string, string)>();

            switch (node)
            {
                case Procedure Procedure:
                    {
                        foreach (var n in Procedure.Statements)
                        {
                            types.AddRange(GetLLVMStructTypes(n));
                        }
                        break;
                    }
                case Compilation Compilation:
                    {
                        foreach (var n in Compilation.Statements)
                        {
                            types.AddRange(GetLLVMStructTypes(n));
                        }
                        break;
                    }

                case StructBody Structure:
                    {
                        var txt = GetLLVMStructType(Structure);
                        types.Add(($"%{Structure.Spelling}", txt));

                        foreach (var m in Structure.Structs)
                        {
                            if (m is StructBody)
                            {
                                var mmm = GetLLVMStructTypes((StructBody)m);
                                types.AddRange(mmm);
                            }
                        }

                        break;
                    }
                default:
                    break;
            }

            return types;
        }
        public string GetLLVMStructType(StructBody structure)
        {
            StringBuilder sb = new StringBuilder();

            //foreach (var member in structure.Members.OrderBy(m => m.Ordinal))
            //{
            //    switch (member)
            //    {
            //        case StructField f:
            //            {
            //                sb.Append($"{GetLLVMFieldType(f)}, ");
            //                break;
            //            }
            //        case Structure s:
            //            {
            //                sb.Append($"%{s.Spelling}, ");
            //                break;
            //            }
            //        default:
            //            throw new NotImplementedException();
            //    }
            //}

            var txt = $"{{ {sb.ToString().TrimEnd(' ', ',')} }}";

            return txt;

        }
        public string GetLLVMFieldType(StructField type)
        {
            int bytes = 0;


            if (type.TypeName == "bit")
            {
                bytes = (type.Length + 7) / 8;

                return $"[{bytes} x i8]";

            }


            return type.TypeName switch
            {
                "bin" => $"i{type.Length}",
                //"bit" => $"i{type.Length}",
                "string" => $"[{type.Length} x i8]",
                _ => "notyet" //throw new NotImplementedException()
            };
        }
        public void PrintConcreteSyntaxTree(ParserRuleContext context, int depth = 0)
        {
            Console.WriteLine(depth.ToString().PadRight(depth) + " " + RemoveContext(context.GetType().Name));

            var children = context.GetChildren();

            if (children.Any())
            {
                foreach (var child in children)
                {
                    depth++;
                    PrintConcreteSyntaxTree(child, depth);
                    depth--;
                }
            }
        }

        public void PrintDiagnostics()
        {
            Reporter.PrintReport();
        }
        public void PrintAbstractSyntaxTree(AstNode node, int depth = 0, bool Symtab = false)
        {
            if (depth == 0)
            {
                Console.WriteLine($"AST DUMP OF {fileName}.sys");
                Console.WriteLine();
                Console.WriteLine("LINE NEST STATEMENT");
                Console.WriteLine();
            }

            switch (node)
            {
                case Always Always:
                    {
                        Console.WriteLine($"{LineDepth(depth, Always)} {Always.GetType().Name} '{Always.ToString()}'");

                        var children = ((IContainer)(node)).Statements;

                        if (children.Any())
                        {
                            foreach (var child in children)
                            {
                                depth++;
                                PrintAbstractSyntaxTree(child, depth);
                                depth--;
                            }
                        }

                        break;
                    }
                case For For:
                    {
                        Console.WriteLine($"{LineDepth(depth, For)} {For.GetType().Name} '{For.ToString()}'");

                        var children = ((IContainer)(node)).Statements;

                        if (children.Any())
                        {
                            foreach (var child in children)
                            {
                                depth++;
                                PrintAbstractSyntaxTree(child, depth);
                                depth--;
                            }
                        }

                        break;
                    }
                case While While:
                    {
                        Console.WriteLine($"{LineDepth(depth, While)} {While.GetType().Name} '{While.ToString()}'");

                        var children = ((IContainer)(node)).Statements;

                        if (children.Any())
                        {
                            foreach (var child in children)
                            {
                                depth++;
                                PrintAbstractSyntaxTree(child, depth);
                                depth--;
                            }
                        }

                        break;
                    }
                case Until Until:
                    {
                        Console.WriteLine($"{LineDepth(depth, Until)} {Until.GetType().Name} '{Until.ToString()}'");

                        var children = ((IContainer)(node)).Statements;

                        if (children.Any())
                        {
                            foreach (var child in children)
                            {
                                depth++;
                                PrintAbstractSyntaxTree(child, depth);
                                depth--;
                            }
                        }

                        break;
                    }
                case Call Call:
                    {
                        Console.WriteLine($"{LineDepth(depth, Call)} {Call.GetType().Name} '{Call.ToString()}'");
                        break;
                    }
                case Goto Goto:
                    {
                        Console.WriteLine($"{LineDepth(depth, Goto)} {Goto.GetType().Name} '{Goto.ToString()}'");
                        break;
                    }
                case Leave Leave:
                    {
                        Console.WriteLine($"{LineDepth(depth, Leave)} {Leave.GetType().Name} '{Leave.ToString()}'");
                        break;
                    }
                case Return Return:
                    {
                        Console.WriteLine($"{LineDepth(depth, Return)} {Return.GetType().Name} '{Return.Expression?.ToString()}'");
                        break;
                    }
                case Reference Reference:
                    {
                        Console.WriteLine($"{LineDepth(depth, node)} {Reference.ToString()}");
                        break;
                    }
                case Expression Expression:
                    {
                        Console.WriteLine($"{LineDepth(depth, node)} {Expression.ToString()}");
                        break;
                    }
                case Assignment Assignment:
                    {
                        Console.WriteLine($"{LineDepth(depth, node)} {node.GetType().Name}");

                        depth++;
                        Console.WriteLine($"{LineDepth(depth, node)} {Assignment.Reference} = {Assignment.Expression}");
                        depth--;
                        break;
                    }
                case If If:
                    {
                        Console.WriteLine($"{LineDepth(depth, If)} {If.GetType().Name} {If.Condition} '{If.Label}'");

                        foreach (var child in If.ThenStatements)
                        {
                            depth++;
                            PrintAbstractSyntaxTree(child, depth);
                            depth--;
                        }
                        if (If.ElifStatements.Any())
                        {

                            foreach (var child in If.ElifStatements)
                            {
                                Console.WriteLine($"{LineDepth(depth, child)} Elif {child.Condition}");

                                foreach (var elif in child.ThenStatements)
                                {
                                    depth++;
                                    PrintAbstractSyntaxTree(elif, depth);
                                    depth--;
                                }
                            }
                        }
                        if (If.ElseStatements.Any())
                        {
                            Console.WriteLine($"{LineDepth(depth, node)} Else");

                            foreach (var child in If.ElseStatements)
                            {
                                depth++;
                                PrintAbstractSyntaxTree(child, depth);
                                depth--;
                            }
                        }
                        Console.WriteLine($"{LineDepthEnd(depth, If)} End");
                        break;
                    }
                case StructBody Structure:
                    {
                        Console.WriteLine($"{LineDepth(depth, Structure)} {node.GetType().Name} '{Structure.Spelling}'");
                        var children = Structure.Structs;

                        if (children.Any())
                        {
                            foreach (var child in children)
                            {
                                depth++;
                                PrintAbstractSyntaxTree(child, depth);
                                depth--;
                            }
                        }
                        Console.WriteLine($"{LineDepthEnd(depth, Structure)} End");
                        break;
                    }
                case StructField Field:
                    {
                        Console.WriteLine($"{LineDepth(depth, node)} {node.GetType().Name} '{((StructField)(node)).Spelling}' {((StructField)(node)).TypeName} {((StructField)(node)).Length}");
                        break;
                    }
                case Procedure Procedure:
                    {
                        Console.WriteLine($"{LineDepth(depth, Procedure)} {Procedure.GetType().Name} '{Procedure.Spelling}' Main = {Procedure.Main}");

                        if (Symtab)
                            PrintSymbols(Procedure.Symbols, depth, Procedure);

                        var children = ((IContainer)(Procedure)).Statements;

                        if (children.Any())
                        {
                            foreach (var child in children)
                            {
                                depth++;
                                PrintAbstractSyntaxTree(child, depth);
                                depth--;
                            }
                        }
                        Console.WriteLine($"{LineDepthEnd(depth, Procedure)} End");
                        break;

                    }
                case Declare Declare:
                    {
                        Console.WriteLine($"{LineDepth(depth, Declare)} {node.GetType().Name} '{Declare.Spelling}'");
                        break;
                    }
                case Scope Scope:
                    {
                        Console.WriteLine($"{LineDepth(depth, Scope)} {node.GetType().Name} '{Scope.Spelling}'");
                        var children = ((IContainer)(Scope)).Statements;

                        if (children.Any())
                        {
                            foreach (var child in children)
                            {
                                depth++;
                                PrintAbstractSyntaxTree(child, depth);
                                depth--;
                            }
                        }

                        if (Scope.IsBlockScope)
                            Console.WriteLine($"{LineDepthEnd(depth, Scope)} End");
                        break;
                    }
                case Compilation Compilation:
                    {
                        Console.WriteLine($"{LineDepth(depth, Compilation)} {Compilation.GetType().Name}");

                        if (Symtab)
                            PrintSymbols(Compilation.Symbols, depth, Compilation);

                        var children = ((IContainer)(Compilation)).Statements;

                        if (children.Any())
                        {
                            foreach (var child in children)
                            {
                                depth++;
                                PrintAbstractSyntaxTree(child, depth);
                                depth--;
                            }
                        }
                        Console.WriteLine($"{LineDepthEnd(depth, Compilation)} End");
                        break;

                    }
                case IContainer statement:
                    {
                        Console.WriteLine(LineDepth(depth, node) + " " + node.GetType().Name);

                        var children = ((IContainer)(node)).Statements;

                        if (children.Any())
                        {
                            foreach (var child in children)
                            {
                                depth++;
                                PrintAbstractSyntaxTree(child, depth);
                                depth--;
                            }
                        }
                        break;
                    }
            }
        }

        public void PrintSymbols(IEnumerable<Symbol> symbols, int depth, AstNode node)
        {
            foreach (var symbol in symbols)
            {
                if (symbol.IsntStructure)
                {
                    StringBuilder builder = new StringBuilder();

                    if (symbol.CoreType != DataType.ENTRY)
                    {
                        builder.Append($"{LineDepth(depth, symbol.Node)}  ");
                        builder.Append($"SYMBOL: '{symbol.ToString()}'");
                        builder.Append($", TYPE: {symbol.CoreType}");
                        builder.Append($"({symbol.Precision},{symbol.Scale})");
                        builder.Append($", BYTES: {symbol.Bytes}");
                        builder.Append($", ALIGN: {symbol.Alignment.AlignmentValue} {symbol.Alignment.AlignmentUnits}");
                        builder.Append($", IS FIXED SIZE: {symbol.ConstantSize}");
                        builder.Append($", IS ARRAY: {symbol.Declaration.IsArray}");
                        builder.Append($", CLASS: {symbol.StorageClass}");
                        builder.Append($", SCOPE: {symbol.StorageScope}");
                        Console.WriteLine(builder.ToString());

                    }
                    else
                    {
                        builder.Append($"{LineDepth(depth, symbol.Node)}  ");
                        builder.Append($"SYMBOL: '{symbol.ToString()}'");
                        builder.Append($", TYPE: {symbol.CoreType}");
                        builder.Append($", CLASS: {symbol.StorageClass}");
                        builder.Append($", SCOPE: {symbol.StorageScope}");
                        Console.WriteLine(builder.ToString());
                    }

                }
                else
                    Console.WriteLine($"{LineDepth(depth, symbol.Declaration)}  SYMBOL: '{symbol.ToString()}' STRUCT");

            }
        }
        public string LineDepth(int depth, AstNode node)
        {
            return $"{node.StartLine.ToString().PadRight(4)} {depth.ToString().PadRight(4 + depth)}";
        }
        public string LineDepthEnd(int depth, AstNode node)
        {
            return $"{node.StopLine.ToString().PadRight(4)} {depth.ToString().PadRight(4 + depth)}";
        }
    }
}