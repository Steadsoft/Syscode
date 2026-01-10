using Antlr4.Runtime;
using Antlr4.Runtime.Atn;
using Syscode.Phases;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;
using System.Text.Json;
using static SyscodeParser;

namespace Syscode
{
    public delegate string DiagnosticHandler(SyntaxNode node);


    public class SyntaxNode 
    {
        public RuleContext Rule;
        public SyntaxNode Child;

        public SyntaxNode(RuleContext Context)
        {
            Rule = Context;
        }
    }

public class MyErrorListener : BaseErrorListener
    {
        public override void SyntaxError(
            TextWriter output,
            IRecognizer recognizer,
            IToken offendingSymbol,
            int line,
            int charPositionInLine,
            string msg,
            RecognitionException e)
        {
            
            if (e is NoViableAltException)
            {
                var parts = msg.Split('\'');
                Console.WriteLine($"Line {line} Column {charPositionInLine}. Unrecognized syntax beginning '{parts[1].Strip(@"\n").Strip(@"\r")}'");
                return;
            }
            else
            {
                Console.WriteLine($"Lexical error at {line}:{charPositionInLine}: {msg}");
                return;
            }

            Console.WriteLine($"Syntax error at {line}:{charPositionInLine}: {msg}");
        }
    }




    public class SkipToEndErrorStrategy : DefaultErrorStrategy
    {

        private static readonly Dictionary<System.Type, DiagnosticHandler> _handlers =
            new Dictionary<System.Type, DiagnosticHandler>
            {
                { typeof(IfContext),   DiagnoseIf },
                { typeof(LoopContext),   DiagnoseLoop },
                { typeof(WhileConditionContext),   DiagnoseWhileCondition },
                { typeof(LoopWhileContext),   DiagnoseWhileLoop },
                { typeof(WhileLoopContext),   DiagnoseWhileLoop },
                { typeof(LoopsContext),   DiagnoseLoops },
                { typeof(ElifContext),   DiagnoseElif },
                { typeof(ThenContext), DiagnoseThen },
                { typeof(StatementContext), DiagnoseStatement },
                { typeof(ExprBinaryContext), DiagnoseExprBinary }
            };

        private static string DiagnoseIf(SyntaxNode node)
        {
            return node.Child != null
                ? Diagnose(node.Child)
                : "Bad 'if'";
        }
        private static string DiagnoseLoop(SyntaxNode node)
        {
            return node.Child != null
                ? Diagnose(node.Child)
                : "Bad 'loop'";
        }
        private static string DiagnoseWhileCondition(SyntaxNode node)
        {
            return node.Child != null
                ? Diagnose(node.Child)
                : "Bad 'while condition'";
        }

        private static string DiagnoseWhileLoop(SyntaxNode node)
        {
            return node.Child != null
                ? Diagnose(node.Child)
                : "Bad 'while loop'";
        }


        private static string DiagnoseLoops(SyntaxNode node)
        {
            return node.Child != null
                ? Diagnose(node.Child)
                : "Bad 'loops'";
        }

        private static string DiagnoseThen(SyntaxNode node)
        {
            return node.Child != null
                ? Diagnose(node.Child)
                : "Bad 'then'";
        }
        private static string DiagnoseElif(SyntaxNode node)
        {
            return node.Child != null
                ? Diagnose(node.Child)
                : "Bad 'elif'";
        }

        private static string DiagnoseStatement(SyntaxNode node)
        {
            return node.Child != null
                ? Diagnose(node.Child)
                : "Bad statement";
        }

        private static string DiagnoseExprBinary(SyntaxNode node)
        {
            var context = node.Rule.Parent;

            return (context) switch
            {
                ThenContext when context.Parent is IfContext =>  $"Syntax error: Invalid expression encountered within the 'if' statement on line {Line(context.Parent)}",
                ThenContext when context.Parent is ElifContext => $"Syntax error: Invalid expression encountered within the 'elif' clause on line {Line(context.Parent)}",
                WhileConditionContext when context.Parent is WhileLoopContext => $"Syntax error: Invalid conditional expression encountered within the 'do while' loop on line {Line(context.Parent)}",
                _ => "Undefined Diagnostic Scenario!"
            };
        }

        private static int Line(RuleContext rule)
        {
            return ((ParserRuleContext)(rule)).Start.Line;
        }


        public static string Diagnose(SyntaxNode node)
        {
            var type = node.Rule.GetType();

            if (_handlers.TryGetValue(type, out var handler))
                return handler(node);

            return "Unknown construct";
        }
        public override void Recover(Parser recognizer, RecognitionException e)
        {
            recognizer.Context.exception = e;

            var chain = GetParseChain(recognizer.Context);

            var m = Diagnose(chain);

            var tokens = recognizer.InputStream;
            int ttype = tokens.LA(1);

            if (recognizer.Context is IfContext || recognizer.Context is LoopsContext)
            {
                while (ttype != SyscodeParser.End && ttype != SyscodeParser.Eof)
                {
                    recognizer.Consume();
                    ttype = tokens.LA(1);
                }

                if (ttype == SyscodeParser.End || ttype == SyscodeParser.Eof)
                    recognizer.Consume();

            }
            else
            {
                while (ttype != SyscodeParser.SEMICOLON && ttype != SyscodeParser.NEWLINE)
                {
                    recognizer.Consume();
                    ttype = tokens.LA(1);
                }

            if (ttype == SyscodeParser.SEMICOLON || ttype == SyscodeParser.NEWLINE)
                recognizer.Consume();

            }
        }

        public override void Sync(Parser recognizer)
        {
            ;// Disable default sync behavior
        }

        private SyntaxNode GetParseChain(ParserRuleContext Node)
        {
            var stack = new Stack<RuleContext>();

            stack.Push(Node);

            while (stack.Peek() is not StatementContext)
                stack.Push(stack.Peek().Parent);

            var list = stack.ToList();

            SyntaxNode root = new SyntaxNode(list[0]);

            var curr = root;

            for (int I = 1; I < list.Count; I++)
            {
                var next = new SyntaxNode(list[I]);
                curr.Child = next;
                curr = next;
            }

            return root;

        }

    }




    public class SyscodeCompiler
    {
        // SEE: https://www.ibm.com/docs/en/SSY2V3_6.2/pdf/pl_i_zos_6_2_language_reference.pdf
        private SyscodeAstBuilder builder;
        private SymtabBuilder symtabBuilder;
        private ReferenceResolver resolver;
        private Preprocessor preprocessor;
        private List<IToken> original_tokens;
        public event EventHandler<DiagnosticEvent> diagnostics = delegate { };
        private readonly string errorMesagesPath;
        private readonly ErrorFile? messages;
        private Reporter reporter;
        private string file;
        private string fileName;
        private string[] namespaceparts;
        private AstListing astlist = new();
        private AstNode ast;
        private Random random = new(12345);
        private string compile_id;
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

            if (messages == null)
                throw new ArgumentNullException(nameof(messages));

            if (messages.Errors.GroupBy(m => m.Number).Where(g => g.Count() > 1).Any())
                throw new InvalidOperationException("The error file contains a duplicate error number.");

            if (messages.Errors.GroupBy(m => m.Message).Where(g => g.Count() > 1).Any())
                throw new InvalidOperationException("The error file contains a duplicate error message.");

        }

        //public void Report(AstNode node, int number, string arg)
        //{
        //    var errormsg = messages.Errors.Where(e => e.Number == number).Single();

        //    var message = errormsg.Message.Replace("{arg}", arg);

        //    diagnostics(this, new DiagnosticEvent(node, errormsg.Number, errormsg.Severity, message));
        //}

        public static SyscodeParser CreateParser(CommonTokenStream Stream)
        {
            var parser = new SyscodeParser(Stream);
            parser.RemoveErrorListeners();
            parser.AddErrorListener(new MyErrorListener());
            parser.ErrorHandler = new SkipToEndErrorStrategy();
            //parser.AddParseListener(new ParserTraceListener());
            parser.Interpreter.PredictionMode = PredictionMode.LL_EXACT_AMBIG_DETECTION;
            parser.Trace = true;


            return parser;
        }

        public void CompileSourceFile(string SourceFile, bool print_listing = true)
        {
            compile_id = random.Next().ToString();

            SourceFile = Path.GetFullPath(SourceFile);

            var folder = Path.GetFullPath(Path.GetDirectoryName(SourceFile));

            var list = GetTokenListFromFile(SourceFile);
            var stream = GetStreamFromList(list);
            var parser = SyscodeCompiler.CreateParser(stream);

            var cst = parser.compilation();

            PrintConcreteSyntaxTree(cst);

            builder = new SyscodeAstBuilder(Reporter);

            var ast = builder.Generate(cst);

            preprocessor = new Preprocessor(ast, list, folder, Reporter);

            list = preprocessor.Apply();

            stream = GetStreamFromList(list);

            // Now we convert the stream to text and retokenize that
            // this ensures that the parser now see a set of tokens 
            // that have valid, consistent line number, columns etc.
            // after the stuff we did during preprocessing.

            var src = stream.GetText();

            if (print_listing)
            {
                PrintListing(src, SourceFile);
            }

            var char_stream = new AntlrInputStream(src);
            var lexer = new SyscodeLexer(char_stream);
            stream = new CommonTokenStream(lexer);


            parser = SyscodeCompiler.CreateParser(stream);
            cst = parser.compilation();

            builder = new SyscodeAstBuilder(Reporter);

            ast = builder.Generate(cst);

            symtabBuilder = new SymtabBuilder(Reporter);

            symtabBuilder.Generate(ast);

            resolver = new ReferenceResolver(Reporter);

            resolver.ResolveContainedReferences(ast);
            resolver.ReportUnresolvedReferences(ast.Statements);

            this.ast = ast;
        }

        private void PrintListing(string text, string path)
        {
            Console.ForegroundColor = ConsoleColor.White;

            var lines = text.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);

            int real_line = 0;

            // Stack of "current file line numbers"
            Stack<int> fileLines = new Stack<int>();
            fileLines.Push(0);   // main file starts at line 0

            Console.WriteLine($"SYSCODE COMPILER v0.6.2 PREPROCESSOR LISTING FOR SOURCE FILE {path}");
            Console.WriteLine("REAL FILE SOURCE TEXT");

            foreach (var line in lines)
            {
                real_line++;

                if (line.StartsWith("// BEGIN "))
                {
                    // Start a new include file → push a fresh counter
                    fileLines.Push(0);
                    Console.Write($"{real_line,-5}     ");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"{line}");
                    Console.ForegroundColor = ConsoleColor.White;
                }
                else if (line.StartsWith("// END "))
                {
                    // End include → pop back to parent file
                    fileLines.Pop();
                    Console.Write($"{real_line,-5}     ");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"{line}");
                    Console.ForegroundColor = ConsoleColor.White;
                }
                else
                {
                    // Increment the current file's line number
                    int newFileLine = fileLines.Pop() + 1;
                    fileLines.Push(newFileLine);
                    Console.WriteLine($"{real_line,-5}{newFileLine,-5}{line}");
                }
            }
        }
        internal static CommonTokenStream GetStreamFromList(List<IToken> list)  
        {
            var source = new ListTokenSource(list);
            var stream = new CommonTokenStream(source);
            return stream;
        }

        private CommonTokenStream GetTokenStreamFromList(List<IToken> list)
        {
            var tokenSource = new ListTokenSource(list);
            var tokenStream = new CommonTokenStream(tokenSource);
            return tokenStream;
        }

        private List<IToken> GetTokenListFromFile(string file)
        {
            fileName = Path.GetFileNameWithoutExtension(file);
            // extract any namespace components from the file's name

            namespaceparts = fileName.Split('.');
            namespaceparts = namespaceparts.Take(namespaceparts.Length - 1).ToArray();

            Reporter = new Reporter(messages, diagnostics);

            var source = new StreamReader(file);
            var char_stream = new AntlrInputStream(source);
            var lexer = new SyscodeLexer(char_stream);
            var stream = new CommonTokenStream(lexer); 
            stream.Fill();
            return new List<IToken>(stream.GetTokens());
        }

        private List<IToken> ProcessPreprocessorDirectives(SyscodeLexer Lexer, string Folder)
        {
            var tokens = new CommonTokenStream(Lexer);

            tokens.Fill();

            var token_list = new List<IToken>(tokens.GetTokens());

            for (int T = 0; T < token_list.Count; T++)
            {
                switch (token_list[T].Type)
                {
                    case SyscodeLexer.INCLUDE:
                        {
                            if (token_list[T + 1].Type == SyscodeLexer.STR_LITERAL)
                            {
                                var include_file_name = token_list[T + 1].Text.Replace("\"", "");
                                var inctokens = LexIncludeFile(Folder + "\\" + include_file_name);
                                token_list.RemoveRange(T, 2);
                                token_list.InsertRange(T, inctokens);
                                T--;
                                break;
                            }

                            // preprocerssor sytax error
                            AstNode fake = new AstNode(token_list[T]);
                            reporter.Report(fake, 1033);
                            break;
                        }
                    case SyscodeLexer.IF:
                        {
                            break; 
                        }
                }
            }

            return token_list;
        }

        private List<IToken> LexIncludeFile(string path)
        {
            var text = File.ReadAllText(path);
            var input = new AntlrInputStream(text);
            var lexer = new SyscodeLexer(input);
            var stream = new CommonTokenStream(lexer);
            stream.Fill();

            // Remove EOF so you don't get multiple EOF tokens in the final stream
            return stream.GetTokens()
                         .Where(t => t.Type != TokenConstants.EOF)
                         .ToList();
        }


        //public AstNode GenerateAbstractSyntaxTree(ParserRuleContext context)
        //{
        //    return builder.Generate(context);
        //}

        //public List<IToken> ApplyPreprocessing(AstNode root)
        //{
        //    return preprocessor.Apply(root);
        //}

        //public void ProcessDeclarations(AstNode root)
        //{
        //    symtabBuilder.Generate((Compilation)root);
        //}

        //public void ResolveReferences(AstNode root)
        //{
        //    resolver.ResolveContainedReferences((Compilation)root);
        //    resolver.ReportUnresolvedReferences(((Compilation)root).Statements);
        //}
        private static string RemoveContext(string input)
        {
            return input.Replace("Context", "");
        }
        public static string GetText(Antlr4.Runtime.Tree.ITerminalNode Node)
        {
            return Node.GetText();
        }
        public List<(string, string)> GetLLVMStructTypes(AstNode node)
        {
            List<(string, string)> types = new();

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
                            if (m is StructBody body)
                            {
                                var mmm = GetLLVMStructTypes(body);
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
        public static string GetLLVMStructType(StructBody structure)
        {
            StringBuilder sb = new();

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
        public static string GetLLVMFieldType(StructField type)
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

            if (children.Count != 0)
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

        public void PrintAbstractSyntaxTree(int depth = 0, bool Symtab = false)
        {
            GenerateAbstractSyntaxTreeText(ast, depth, Symtab);

            foreach (var line in astlist.list.Take(4))
            {
                Console.WriteLine(line);
            }

            foreach (var line in astlist.list.Skip(4).OrderBy(l => Convert.ToInt16(l.Substring(0, 4))))
            {
                Console.WriteLine(line);
            }

        }
        public void GenerateAbstractSyntaxTreeText(AstNode node, int depth = 0, bool Symtab = false)
        {
            if (depth == 0)
            {
                astlist.WriteLine($"AST DUMP OF {fileName}.sys");
                astlist.WriteLine();
                astlist.WriteLine("LINE NEST STATEMENT");
                astlist.WriteLine();
            }

            switch (node)
            {
                case Always Always:
                    {
                        astlist.WriteLine($"{LineDepth(depth, Always)} {Always.GetType().Name} '{Always}'");

                        var children = ((IContainer)(node)).Statements;

                        if (children.Count != 0)
                        {
                            foreach (var child in children)
                            {
                                depth++;
                                GenerateAbstractSyntaxTreeText(child, depth);
                                depth--;
                            }
                        }

                        break;
                    }
                case For For:
                    {
                        astlist.WriteLine($"{LineDepth(depth, For)} {For.GetType().Name} '{For}'");

                        var children = ((IContainer)(node)).Statements;

                        if (children.Count != 0)
                        {
                            foreach (var child in children)
                            {
                                depth++;
                                GenerateAbstractSyntaxTreeText(child, depth);
                                depth--;
                            }
                        }

                        break;
                    }
                case While While:
                    {
                        astlist.WriteLine($"{LineDepth(depth, While)} {While.GetType().Name} '{While}'");

                        var children = ((IContainer)(node)).Statements;

                        if (children.Count != 0)
                        {
                            foreach (var child in children)
                            {
                                depth++;
                                GenerateAbstractSyntaxTreeText(child, depth);
                                depth--;
                            }
                        }

                        break;
                    }
                case Until Until:
                    {
                        astlist.WriteLine($"{LineDepth(depth, Until)} {Until.GetType().Name} '{Until}'");

                        var children = ((IContainer)(node)).Statements;

                        if (children.Count != 0)
                        {
                            foreach (var child in children)
                            {
                                depth++;
                                GenerateAbstractSyntaxTreeText(child, depth);
                                depth--;
                            }
                        }

                        break;
                    }
                case Call Call:
                    {
                        astlist.WriteLine($"{LineDepth(depth, Call)} {Call.GetType().Name} '{Call}'");
                        break;
                    }
                case Goto Goto:
                    {
                        astlist.WriteLine($"{LineDepth(depth, Goto)} {Goto.GetType().Name} '{Goto}'");
                        break;
                    }
                case Leave Leave:
                    {
                        astlist.WriteLine($"{LineDepth(depth, Leave)} {Leave.GetType().Name} '{Leave}'");
                        break;
                    }
                case Loop Proceed:
                    {
                        astlist.WriteLine($"{LineDepth(depth, Proceed)} {Proceed.GetType().Name} '{Proceed}'");
                        break;
                    }
                case Return Return:
                    {
                        astlist.WriteLine($"{LineDepth(depth, Return)} {Return.GetType().Name} '{Return.Expression?.ToString()}'");
                        break;
                    }
                case Reference Reference:
                    {
                        astlist.WriteLine($"{LineDepth(depth, node)} {Reference}");
                        break;
                    }
                case Expression Expression:
                    {
                        astlist.WriteLine($"{LineDepth(depth, node)} {Expression}");
                        break;
                    }
                case Assignment Assignment:
                    {
                        astlist.WriteLine($"{LineDepth(depth, node)} {node.GetType().Name}");

                        depth++;
                        astlist.WriteLine($"{LineDepth(depth, node)} {Assignment.Reference} = {Assignment.Expression}");
                        depth--;
                        break;
                    }
                case If If:
                    {
                        astlist.WriteLine($"{LineDepth(depth, If)} {If.GetType().Name} {If.Condition} {(!String.IsNullOrEmpty(If.Label) ? $" @{If.Label}" : "")}");

                        foreach (var child in If.Statements)
                        {
                            depth++;
                            GenerateAbstractSyntaxTreeText(child, depth);
                            depth--;
                        }
                        if (If.Elifs.Count != 0)
                        {

                            foreach (var child in If.Elifs)
                            {
                                astlist.WriteLine($"{LineDepth(depth, child)} Elif {child.Condition}");

                                foreach (var elif in child.Statements)
                                {
                                    depth++;
                                    GenerateAbstractSyntaxTreeText(elif, depth);
                                    depth--;
                                }
                            }
                        }
                        if (If.Else?.Statements.Count != 0)
                        {
                            astlist.WriteLine($"{LineDepth(depth, node)} Else");

                            foreach (var child in If.Statements)
                            {
                                depth++;
                                GenerateAbstractSyntaxTreeText(child, depth);
                                depth--;
                            }
                        }
                        astlist.WriteLine($"{LineDepthEnd(depth, If)} End");
                        break;
                    }
                case IF IF:
                    {
                        astlist.WriteLine($"{LineDepth(depth, IF)} {IF.GetType().Name} {IF.Condition} {(!String.IsNullOrEmpty(IF.Label) ? $" @{IF.Label}" : "")}");

                        foreach (var child in IF.ThenStatements)
                        {
                            depth++;
                            GenerateAbstractSyntaxTreeText(child, depth);
                            depth--;
                        }
                        if (IF.ElifStatements.Count != 0)
                        {

                            foreach (var child in IF.ElifStatements)
                            {
                                astlist.WriteLine($"{LineDepth(depth, child)} ELIF {child.Condition}");

                                foreach (var elif in child.Statements)
                                {
                                    depth++;
                                    GenerateAbstractSyntaxTreeText(elif, depth);
                                    depth--;
                                }
                            }
                        }
                        if (IF.ElseStatements.Count != 0)
                        {
                            astlist.WriteLine($"{LineDepth(depth, node)} ELSE");

                            foreach (var child in IF.ElseStatements)
                            {
                                depth++;
                                GenerateAbstractSyntaxTreeText(child, depth);
                                depth--;
                            }
                        }
                        astlist.WriteLine($"{LineDepthEnd(depth, IF)} END");
                        break;
                    }
                case StructBody Structure:
                    {
                        astlist.WriteLine($"{LineDepth(depth, Structure)} {node.GetType().Name} '{Structure.Spelling}'");
                        var children = Structure.Structs;

                        if (children.Count != 0)
                        {
                            foreach (var child in children)
                            {
                                depth++;
                                GenerateAbstractSyntaxTreeText(child, depth);
                                depth--;
                            }
                        }
                        astlist.WriteLine($"{LineDepthEnd(depth, Structure)} End");
                        break;
                    }
                case StructField Field:
                    {
                        astlist.WriteLine($"{LineDepth(depth, node)} {node.GetType().Name} '{((StructField)(node)).Spelling}' {((StructField)(node)).TypeName} {((StructField)(node)).Length}");
                        break;
                    }
                case Procedure Procedure:
                    {
                        astlist.WriteLine($"{LineDepth(depth, Procedure)} {Procedure.GetType().Name} '{Procedure.Spelling}' Main = {Procedure.Main}");

                        if (Symtab)
                            PrintSymbols(Procedure.Symbols, depth, Procedure);

                        var children = ((IContainer)(Procedure)).Statements;

                        if (children.Count != 0)
                        {
                            foreach (var child in children)
                            {
                                depth++;
                                GenerateAbstractSyntaxTreeText(child, depth, Symtab);
                                depth--;
                            }
                        }
                        astlist.WriteLine($"{LineDepthEnd(depth, Procedure)} End");
                        break;

                    }
                case Declare Declare:
                    {
                        astlist.WriteLine($"{LineDepth(depth, Declare)} {node.GetType().Name} '{Declare.Spelling}'");
                        break;
                    }
                case Scope Scope:
                    {
                        astlist.WriteLine($"{LineDepth(depth, Scope)} {node.GetType().Name} '{Scope.Spelling}'");
                        var children = ((IContainer)(Scope)).Statements;

                        if (children.Count != 0)
                        {
                            foreach (var child in children)
                            {
                                depth++;
                                GenerateAbstractSyntaxTreeText(child, depth, Symtab);
                                depth--;
                            }
                        }

                        if (Scope.IsBlockScope)
                            astlist.WriteLine($"{LineDepthEnd(depth, Scope)} End");
                        break;
                    }
                case Compilation Compilation:
                    {
                        astlist.WriteLine($"{LineDepth(depth, Compilation)} {Compilation.GetType().Name}");

                        if (Symtab)
                            PrintSymbols(Compilation.Symbols, depth, Compilation);

                        var children = ((IContainer)(Compilation)).Statements;

                        if (children.Count != 0)
                        {
                            foreach (var child in children)
                            {
                                depth++;
                                GenerateAbstractSyntaxTreeText(child, depth, Symtab);
                                depth--;
                            }
                        }
                        astlist.WriteLine($"{LineDepthEnd(depth, Compilation)} End Compilation");
                        break;

                    }
                case IContainer statement:
                    {
                        astlist.WriteLine(LineDepth(depth, node) + " " + node.GetType().Name);

                        var children = ((IContainer)(node)).Statements;

                        if (children.Count != 0)
                        {
                            foreach (var child in children)
                            {
                                depth++;
                                GenerateAbstractSyntaxTreeText(child, depth, Symtab);
                                depth--;
                            }
                        }
                        break;
                    }
            }
        }

        public void PrintSymbols(IEnumerable<Symbol> symbols, int depth, AstNode node)
        {
            astlist.WriteLine($"{LineDepth(depth, node)} SYMBOLS = {symbols.Count()}");

            foreach (var symbol in symbols.OrderBy(s => s.Node.StartLine) )
            {
                if (symbol.IsntStructure)
                {
                    StringBuilder builder = new();

                    if (symbol.DataType != DataType.ENTRY)
                    {
                        builder.Append($"{LineDepth(depth, symbol.Node)}  ");
                        builder.Append($"{symbol.DataType}");
                        builder.Append($", NAME: '{symbol.Spelling}'");
                        builder.Append($"({symbol.Precision},{symbol.Scale})");
                        builder.Append($", BYTES: {symbol.Bytes}");
                        builder.Append($", ALIGN: {symbol.Alignment.AlignmentValue} {symbol.Alignment.AlignmentUnits}");
                        builder.Append($", IS FIXED SIZE: {symbol.ConstantSize}");
                        builder.Append($", IS ARRAY: {symbol.Declaration?.IsArray}");
                        builder.Append($", CLASS: {symbol.StorageClass}");
                        builder.Append($", SCOPE: {symbol.StorageScope}");
                        astlist.WriteLine(builder.ToString());

                    }
                    else
                    {
                        builder.Append($"{LineDepth(depth, symbol.Node)}  ");
                        builder.Append($"{symbol.DataType}");
                        builder.Append($", NAME: '{symbol.Spelling}'");
                        builder.Append($", TYPE: {symbol.DataType}");
                        builder.Append($", CLASS: {symbol.StorageClass}");
                        builder.Append($", SCOPE: {symbol.StorageScope}");
                        astlist.WriteLine(builder.ToString());
                    }

                }
                else
                    astlist.WriteLine($"{LineDepth(depth, symbol.Declaration)}  {symbol.DataType}, NAME: {symbol.Spelling}");

            }
        }
        public static string LineDepth(int depth, AstNode node)
        {
            return $"{node.StartLine,-4} {depth.ToString().PadRight(4 + depth)}";
        }
        public static string LineDepthEnd(int depth, AstNode node)
        {
            return $"{node.StopLine,-4} {depth.ToString().PadRight(4 + depth)}";
        }
    }

    public class AstListing
    {
        public List<string> list = new List<string>();

        public void WriteLine(string text)
        {
            list.Add(text);
        }
        public void WriteLine()
        {
            list.Add("");
        }
    }
}