using Antlr4.Runtime;

namespace Syscode.Phases
{
    internal class Preprocessor
    {
        // The strategy here is as follows. The AST will contain every preprocessor statement that was in the source file.
        // We walk the AST in two passes, the first is concerned only with INCLUDE statements, these add text to the source.
        // Once all includes are processed we transform the updated token stream to text and then lex and parse that text
        // Then we take that CST and build a fresh AST from it. 
        // At that stage we have a single source text, no INCLUDES and potentially other pre-processing statements. 

        private List<IToken> token_list;
        private string folder;
        private Reporter reporter;
        private Compilation root;
        private int total_added_tokens = 0;
        private int initial_token_count = 0;
        private List<string> included = new();
        private Dictionary<string, string> replacements = new();
        private List<string> included_files = new();
        private int abs_include_file_count = 0;
        public Preprocessor(Compilation root, List<IToken> tokens, string folder, Reporter reporter)
        {
            this.token_list = tokens;
            this.folder = folder;
            this.reporter = reporter;
            this.root = root;
            this.initial_token_count = tokens.Count;
        }

        public List<IToken> Apply()
        {
            int inserted_count = 0;
            StoreReplacements(root);
            ProcessContainedIncludes(token_list, root, folder, ref inserted_count);

            var stream = GetStreamFromList(token_list);

            // Now we convert the stream to text and retokenize that
            // this ensures that the parser now see a set of tokens 
            // that have valid, consistent line number, columns etc.
            // after the stuff we did during preprocessing.

            var src = stream.GetText();

            var char_stream = new AntlrInputStream(src);
            var lexer = new SyscodeLexer(char_stream);
            stream = new CommonTokenStream(lexer);
            stream.Fill();
            token_list = new List<IToken>(stream.GetTokens());

            var parser = new SyscodeParser(stream);
            var cst = parser.compilation();

            var builder = new SyscodeAstBuilder(reporter);

            root = builder.Generate(cst);

            ProcessNonIncludes(token_list, root, folder);

            return new List<IToken>(token_list);
        }

        internal static CommonTokenStream GetStreamFromList(List<IToken> list)
        {
            var source = new ListTokenSource(list);
            var stream = new CommonTokenStream(source);
            return stream;
        }

        private void StoreReplacements(Compilation context)
        {
            foreach (var replace in context.Statements.OfType<REPLACE>())
            {
                if (!replacements.ContainsKey(replace.Name))
                {
                    replacements.Add(replace.Name, replace.Expression.ToString());
                }
            }
        }

        private void ProcessContainedIncludes(List<IToken> tokens, Compilation context, string folder, ref int inserted_count)
        {
            //int inserted_count = 0;

            foreach (var include in context.Statements.OfType<INCLUDE>())
            {
                var insertion_point = include.EndToken + 1 + inserted_count;
                var files = include.GetFiles(replacements, folder);
                var matches = files.Count();
                int rel_include_file_count = 0;

                foreach (var file in files)
                {
                    // We never include a file more than once.

                    if (included_files.Contains(file))
                        continue;

                    included_files.Add(file);
                    
                    abs_include_file_count++;
                    rel_include_file_count++;
                    
                    var included_tokens = TokenizeIncludeFile(include, file, folder, abs_include_file_count, rel_include_file_count, matches);

                    if (included_tokens == null)
                        continue;

                    var token_stream = SyscodeCompiler.GetStreamFromList(included_tokens);
                    var parser = new SyscodeParser(token_stream);
                    var cst = parser.compilation();

                    var builder = new SyscodeAstBuilder(reporter);
                    var ast = builder.Generate(cst);
                    StoreReplacements(ast);
                    ProcessContainedIncludes(included_tokens, ast, folder, ref inserted_count);
                    tokens.InsertRange(insertion_point, included_tokens);

                    inserted_count += included_tokens.Count;
                    insertion_point += included_tokens.Count;
                }
            }
        }

        private void ProcessNonIncludes(List<IToken> tokens, Compilation context, string folder)
        {
            foreach (var statement in context.Statements)
            {
                if (statement is IF if_statement)
                {
                    ;
                }

                if (statement is REPLACE replace)
                {
                    ProcessReplace(tokens, replace);
                }
            }
        }

        private void ProcessReplace(List<IToken> tokens, REPLACE replace)
        {
            switch (root)
            {
                case Compilation context:
                    {
                        foreach (var statement in context.Statements.OfType<IReplaceContainer>())
                        {
                            statement.ApplyPreprocessorReplace(tokens, replace);
                        }

                        break;
                    }
            }
        }

        private static List<IToken> TokenizeIncludeFile(INCLUDE include, string file, string Folder, int abs_include_file_count, int rel_include_file_count, int wildcard_count)
        {
            var file_content = File.ReadAllText(file);
            var line_count = file_content.Split('\n').Length;

            if (include.Wilcard)
                file_content = $"// BEGIN INCLUDE No {abs_include_file_count} ({rel_include_file_count} OF {wildcard_count}) WITH {line_count} LINES FROM{(include.Name is null ? " " : $" '{include.Name}' ")}{file}{Environment.NewLine}{file_content}{Environment.NewLine}// END INCLUDE No {abs_include_file_count}";
            else
                file_content = $"// BEGIN INCLUDE No {abs_include_file_count} WITH {line_count} LINES FROM{(include.Name is null ? " " : $" '{include.Name}' ")}{file}{Environment.NewLine}{file_content}{Environment.NewLine}// END INCLUDE No {abs_include_file_count}";

            // We must ensure a newline is present at the end of the file.
            // If we dont do this then the last character in the file 
            // will appear right before the first character
            // of whatever follows it.

            if (file_content.EndsWith(Environment.NewLine) == false)
                file_content += Environment.NewLine;

            var input = new AntlrInputStream(file_content);
            var lexer = new SyscodeLexer(input);
            var stream = new CommonTokenStream(lexer);
            stream.Fill();

            // Remove EOF so you don't get multiple EOF tokens in the final stream
            return stream.GetTokens().Where(t => t.Type != TokenConstants.EOF).ToList();
        }
    }
}
