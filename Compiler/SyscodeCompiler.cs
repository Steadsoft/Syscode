using Antlr4.Runtime;
using Syscode.Phases;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using static SyscodeParser;

namespace Syscode
{
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
        private readonly Dictionary<string, IConstant> constants = new();
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

        public void CompileSourceFile(string SourceFile)
        {
            compile_id = random.Next().ToString();

            var folder = Path.GetFullPath(Path.GetDirectoryName(SourceFile));

            var list = GetTokenListFromFile(SourceFile);
            var stream = GetStreamFromList(list);
            var parser = new SyscodeParser(stream);
            var cst = parser.compilation();

            builder = new SyscodeAstBuilder(constants, Reporter);

            var ast = builder.Generate(cst);

            preprocessor = new Preprocessor(ast, list, folder, Reporter);

            list = preprocessor.Apply();

            stream = GetStreamFromList(list);

            // Now we convert the stream to text and retokenize that
            // this ensures that the parser now see a set of tokens 
            // that have valid, consistent line number, columns etc.
            // after the stuff we did during preprocessing.

            var src = stream.GetText();

            var char_stream = new AntlrInputStream(src);
            var lexer = new SyscodeLexer(char_stream);
            stream = new CommonTokenStream(lexer);


            parser = new SyscodeParser(stream);
            cst = parser.compilation();

            builder = new SyscodeAstBuilder(constants, Reporter);

            ast = builder.Generate(cst);

            symtabBuilder = new SymtabBuilder(Reporter);

            symtabBuilder.Generate((Compilation)ast);

            resolver = new ReferenceResolver(Reporter);

            resolver.ResolveContainedReferences((Compilation)ast);
            resolver.ReportUnresolvedReferences(((Compilation)ast).Statements);

            this.ast = ast;
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
                case Proceed Proceed:
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

                        foreach (var child in If.ThenStatements)
                        {
                            depth++;
                            GenerateAbstractSyntaxTreeText(child, depth);
                            depth--;
                        }
                        if (If.ElifBlocks.Count != 0)
                        {

                            foreach (var child in If.ElifBlocks)
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
                        if (If.ElseStatements.Count != 0)
                        {
                            astlist.WriteLine($"{LineDepth(depth, node)} Else");

                            foreach (var child in If.ElseStatements)
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
                        astlist.WriteLine($"{LineDepthEnd(depth, Compilation)} End");
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