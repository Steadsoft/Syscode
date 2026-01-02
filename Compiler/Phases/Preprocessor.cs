using Antlr4.Runtime;
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using static SyscodeParser;

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
        private AstNode root;
        private int total_added_tokens = 0;
        private int initial_token_count = 0;
        private List<string> included = new();
        private Dictionary<string, string> replacements = new();
        private int include_file_count = 0;
        public Preprocessor(AstNode root, List<IToken> tokens, string folder, Reporter reporter  )
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
                    ProcessIncludes(token_list, context, folder);

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

        private void StoreReplacements(AstNode context)
        {
            foreach (var statement in ((Compilation)context).Statements)
            {
                if (statement is REPLACE replace && !replacements.ContainsKey(replace.Name))
                {
                    replacements.Add(replace.Name, replace.Expression.ToString());
                }
            }
        }


        private void ProcessIncludes(List<IToken> tokens, AstNode context, string folder)
        {
            int prior_tokens = 0;

            foreach (var statement in ((Compilation)context).Statements)
            {
                if (statement is INCLUDE include_statement && !included.Contains(include_statement.Filename))
                {
                    ProcessInclude(tokens, include_statement, folder, ref prior_tokens);
                    included.Add(include_statement.Filename);
                }
            }
        }

        private void ProcessNonIncludes(List<IToken> tokens, AstNode context, string folder)
        { 
            foreach (var statement in ((Compilation)context).Statements)
            {
                if (statement is IF if_statement)
                {
                    ;
                }

                if (statement is REPLACE replace)
                {
                    if (replacements.ContainsKey(replace.Name))
                    {
                        if (replacements[replace.Name] == replace.Expression.ToString())
                        {
                            ;// warning multiple replaces for 'Name' 
                        }
                        else
                        {
                            ;// error conflicting replaces
                        }

                        continue;
                    }

                    ProcessReplace(tokens, replace);
                    replacements.Add(replace.Name, replace.Expression.ToString());
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

        private static CommonToken GetTokenById(List<IToken> tokens, int id)
        {
            return (CommonToken)tokens.Where(t => t.TokenIndex == id).Single();
        }

        private void ProcessInclude(List<IToken> tokens, INCLUDE include, string Folder, ref int PriorTokens)
        {
            include.StartToken += PriorTokens;
            include.EndToken += PriorTokens;

            var token_list = LexIncludeFile(include, Folder);

            if (token_list == null)
                return;

            var token_stream = SyscodeCompiler.GetStreamFromList(token_list);
            var parser = new SyscodeParser(token_stream);
            var cst = parser.compilation();

            Dictionary<string, IConstant> constants = new();

            var builder = new SyscodeAstBuilder(constants,reporter);

            var ast = builder.Generate(cst);

            StoreReplacements(ast);

            ProcessIncludes(token_list, ast, folder);

            int remove_count = (include.EndToken - include.StartToken) + 1;
            int insert_count = token_list.Count;

            tokens.RemoveRange(include.StartToken, remove_count);
            tokens.InsertRange(include.StartToken, token_list);

            int added_tokens = insert_count - remove_count;
            int added_lines = (token_list.Last().Line - token_list.First().Line) + 1;

            total_added_tokens += added_tokens;

            PriorTokens += added_tokens;
        }
        private List<IToken> LexIncludeFile(INCLUDE include, string Folder)
        {
            include_file_count++;

            if (include.Name != null)
            {
                if (replacements.ContainsKey(include.Name))
                {
                    if (replacements[include.Name].StartsWith('"') && replacements[include.Name].EndsWith('"'))
                        include.Filename = replacements[include.Name].Strip('"');
                    else
                    {
                        ; // error
                        return null;
                    }
                }
                else
                {
                    ; // error
                    return null;
                }
            }

            var path = Folder + "\\" + include.Filename;

            var text = File.ReadAllText(path);

            var line_count = text.Split('\n').Length;

            text = $"// BEGIN INCLUDE No {include_file_count} WITH {line_count} LINES FROM{(include.Name is null ? " " : $" '{include.Name}' ")}{path}" + Environment.NewLine + text + Environment.NewLine + $"// END INCLUDE {include_file_count}";

            // We must ensure a newline is present at the end of the file.
            // If we dont do this then the last character in the file 
            // will appear right before the first character
            // of whatever follows it.

            if (text.EndsWith(Environment.NewLine) == false)
                text += Environment.NewLine;

            var input = new AntlrInputStream(text);
            var lexer = new SyscodeLexer(input);
            var stream = new CommonTokenStream(lexer);
            stream.Fill();

            // Remove EOF so you don't get multiple EOF tokens in the final stream
            return stream.GetTokens().Where(t => t.Type != TokenConstants.EOF).ToList();
        }

    }
}
