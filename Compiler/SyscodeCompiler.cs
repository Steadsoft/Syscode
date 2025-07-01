using Antlr4.Runtime;
using System.Text;
using static SyscodeParser;

namespace Syscode
{
    public class SyscodeCompiler
    {
        private SyscodeLexer lexer;
        private AstBuilder builder;
        private SymtabBuilder symtabBuilder;
        public SyscodeCompiler()
        {
        }

        public CompilationContext CompileSourceFile(string File)
        {
            var source = new StreamReader(File);
            var stream = new AntlrInputStream(source);
            lexer = new SyscodeLexer(stream);
            var tokens = new CommonTokenStream(lexer);
            var parser = new SyscodeParser(tokens);
            builder = new AstBuilder();
            symtabBuilder = new SymtabBuilder();

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
                case Procedure proc:
                    {
                        foreach (var n in proc.Statements)
                        {
                            types.AddRange(GetLLVMStructTypes(n));
                        }
                        break;
                    }
                case Compilation prog:
                    {
                        foreach (var n in prog.Statements)
                        {
                            types.AddRange(GetLLVMStructTypes(n));
                        }
                        break;
                    }

                case StructBody structure:
                    {
                        var txt = GetLLVMStructType(structure);
                        types.Add(($"%{structure.Spelling}", txt));

                        foreach (var m in structure.Structs)
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
        public void PrintAbstractSyntaxTree(AstNode node, int depth = 0)
        {
            if (depth == 0)
            {
                Console.WriteLine("AST DUMP");
                Console.WriteLine();
                Console.WriteLine("LINE NEST STATEMENT");
                Console.WriteLine();
            }

            switch (node)
            {
                case Call call :
                    {
                        Console.WriteLine($"{LineDepth(depth, call)} {call.GetType().Name} '{call.ToString()}'");
                        break;
                    }
                case Return ret:
                    {
                        Console.WriteLine($"{LineDepth(depth, ret)} {ret.GetType().Name} '{ret.Expression?.ToString()}'");
                        break;
                    }
                case Reference reference:
                    {
                        Console.WriteLine($"{LineDepth(depth, node)} {reference.ToString()}");
                        break;
                    }
                case Expression expression:
                    {
                        Console.WriteLine($"{LineDepth(depth, node)} {expression.ToString()}");
                        break;
                    }
                case Assignment assign:
                    {
                        Console.WriteLine($"{LineDepth(depth, node)} {node.GetType().Name}");

                        depth++;
                        Console.WriteLine($"{LineDepth(depth, node)} {assign.Reference} = {assign.Expression}");
                        //PrintAbstractSyntaxTree(assign.Referenece, depth);
                        //PrintAbstractSyntaxTree(assign.Expression, depth);
                        depth--;
                        break;
                    }
                case If ifstmt:
                    {
                        Console.WriteLine($"{LineDepth(depth, node)} {node.GetType().Name}");

                        foreach (var child in ifstmt.ThenStatements)
                        {
                            depth++;
                            PrintAbstractSyntaxTree(child, depth);
                            depth--;
                        }
                        if (ifstmt.ElifStatements.Any())
                        {

                            foreach (var child in ifstmt.ElifStatements)
                            {
                                Console.WriteLine($"{LineDepth(depth, node)} Elif");

                                foreach (var elif in child.ThenStatements)
                                {
                                    depth++;
                                    PrintAbstractSyntaxTree(elif, depth);
                                    depth--;
                                }
                            }
                        }
                        if (ifstmt.ElseStatements.Any())
                        {
                            Console.WriteLine($"{LineDepth(depth, node)} Else");

                            foreach (var child in ifstmt.ElseStatements)
                            {
                                depth++;
                                PrintAbstractSyntaxTree(child, depth);
                                depth--;
                            }
                        }
                        Console.WriteLine($"{LineDepthEnd(depth, ifstmt)} End");
                        break;
                    }
                case StructBody structure:
                    {
                        Console.WriteLine($"{LineDepth(depth, structure)} {node.GetType().Name} '{structure.Spelling}'");
                        var children = structure.Structs;

                        if (children.Any())
                        {
                            foreach (var child in children)
                            {
                                depth++;
                                PrintAbstractSyntaxTree(child, depth);
                                depth--;
                            }
                        }
                        Console.WriteLine($"{LineDepthEnd(depth, structure)} End");
                        break;
                    }
                case StructField field:
                    {
                        Console.WriteLine($"{LineDepth(depth, node)} {node.GetType().Name} '{((StructField)(node)).Spelling}' {((StructField)(node)).TypeName} {((StructField)(node)).Length}");
                        break;
                    }

                case Procedure proc:
                    {
                        Console.WriteLine($"{LineDepth(depth, proc)} {proc.GetType().Name} '{proc.Spelling}'");

                        var children = ((IStatementContainer)(proc)).Statements;

                        if (children.Any())
                        {
                            foreach (var child in children)
                            {
                                depth++;
                                PrintAbstractSyntaxTree(child, depth);
                                depth--;
                            }
                        }
                        Console.WriteLine($"{LineDepthEnd(depth, proc)} End");
                        break;

                    }
                case Declare dcl:
                    {
                        Console.WriteLine($"{LineDepth(depth, dcl)} {node.GetType().Name} '{dcl.Spelling}'");
                        break;
                    }

                case Scope scope:
                    {
                        Console.WriteLine($"{LineDepth(depth, scope)} {node.GetType().Name} '{scope.Spelling}'");
                        var children = ((IStatementContainer)(scope)).Statements;

                        if (children.Any())
                        {
                            foreach (var child in children)
                            {
                                depth++;
                                PrintAbstractSyntaxTree(child, depth);
                                depth--;
                            }
                        }
                        
                        if (scope.IsBlockScope)
                            Console.WriteLine($"{LineDepthEnd(depth, scope)} End");
                        break;
                    }
                case IStatementContainer statement:
                    {
                        Console.WriteLine(LineDepth(depth, node) + " " + node.GetType().Name);

                        var children = ((IStatementContainer)(node)).Statements;

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