using Antlr4.Runtime;

namespace Syscode.Phases
{
    internal class Preprocessor
    {
        // The strategy here is as follows. The AST will contain every preprocessor statement that was in the source file.
        // We walk the AST in two passes, the first is concerned only with INCLUDE statements, these add text to the source.
        // Once all includes are processeed we transform the updated token stream to text and then lex and parse that text
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
        private int include_file_count = 0;
        public Preprocessor(Compilation root, List<IToken> tokens, string folder, Reporter reporter  )
        {
            this.token_list = tokens;
            this.folder = folder;
            this.reporter = reporter;
            this.root = root;
            this.initial_token_count = tokens.Count;
        }

        public List<IToken> Apply()
        {

            switch (root)
            {
                case Compilation context:

                    StoreReplacements(context);
                    ProcessContainedIncludes(token_list, context, folder);

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

                    Dictionary<string, IConstant> constants = new();

                    var builder = new SyscodeAstBuilder(constants, reporter);

                    root = builder.Generate(cst);

                    ProcessNonIncludes(token_list, root, folder); 
                    break;
            }

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
            foreach (var statement in context.Statements)
            {
                if (statement is REPLACE replace && !replacements.ContainsKey(replace.Name))
                {
                    replacements.Add(replace.Name, replace.Expression.ToString());
                }
            }
        }

        private void ProcessContainedIncludes(List<IToken> tokens, Compilation context, string folder)
        {
            int inserted_count = 0;

            foreach (var include in context.Statements.OfType<INCLUDE>())
            {
                // remove the INCLUDE statement's tokens

                var statement_length = (include.EndToken - include.StartToken) + 1;
                var insertion_point = include.StartToken + inserted_count;
                tokens.RemoveRange(insertion_point, statement_length);
                inserted_count -= statement_length;

                foreach (var file in include.GetFiles(replacements, folder))
                {
                    include_file_count++;

                    var token_list = TokenizeIncludeFile(include, file, folder, include_file_count);

                    if (token_list == null)
                        continue;


                    var token_stream = SyscodeCompiler.GetStreamFromList(token_list);
                    var parser = new SyscodeParser(token_stream);
                    var cst = parser.compilation();

                    Dictionary<string, IConstant> constants = new();

                    var builder = new SyscodeAstBuilder(constants, reporter);
                    var ast = builder.Generate(cst);
                    StoreReplacements(ast);
                    ProcessContainedIncludes(token_list, ast, folder);
                    tokens.InsertRange(insertion_point, token_list);

                    inserted_count += token_list.Count;
                    insertion_point += token_list.Count;
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

        private static List<IToken> TokenizeIncludeFile(INCLUDE include, string file, string Folder, int include_file_count)
        {
            var file_content = File.ReadAllText(file);
            var line_count = file_content.Split('\n').Length;

            file_content = $"// BEGIN INCLUDE No {include_file_count} WITH {line_count} LINES FROM{(include.Name is null ? " " : $" '{include.Name}' ")}{file}{Environment.NewLine}{file_content}{Environment.NewLine}// END INCLUDE No {include_file_count}";

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
