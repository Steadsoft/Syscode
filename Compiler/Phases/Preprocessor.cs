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
using static SyscodeParser;

namespace Syscode.Phases
{
    internal class Preprocessor
    {
        private List<IToken> token_list;
        private string folder;
        private Reporter reporter;
        private AstNode root;
        private int total_added_tokens = 0;
        private int initial_token_count = 0;
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

                    ProcessCompilation(token_list, root, folder); 
                    break;
            }

            int index = 0;

            foreach (CommonToken token in token_list)
            {
                token.TokenIndex = index++;
            }

            // Note that token line numbers are now potentially inaccurate.

            return token_list;
        }

        internal static CommonTokenStream GetStreamFromList(List<IToken> list)
        {
            var source = new ListTokenSource(list);
            var stream = new CommonTokenStream(source);
            return stream;
        }


        private void ProcessIncludes(List<IToken> tokens, AstNode context, string folder)
        {
            int prior_tokens = 0;

            foreach (var statement in ((Compilation)context).Statements)
            {
                if (statement is INCLUDE include_statement)
                {
                    ProcessInclude(tokens, include_statement, folder, ref prior_tokens);
                }
            }
        }

        private void ProcessCompilation(List<IToken> tokens, AstNode context, string folder)
        { 
            foreach (var statement in ((Compilation)context).Statements)
            {
                if (statement is IF if_statement)
                {
                    ;
                }

                if (statement is REPLACE replace_statement)
                {
                    ProcessReplace(tokens, replace_statement);
                }
            }
        }

        private void ProcessReplace(List<IToken> tokens, REPLACE include)
        {
            switch (root)
            {
                case Compilation context:
                    {
                        foreach (var statement in ((Compilation)context).Statements)
                        {
                            switch (statement)
                            {
                                case If stmt:
                                    {
                                        var leftref = stmt.Condition.Left.Reference;
                                        var rightref = stmt.Condition.Right.Reference;

                                        if (leftref != null && leftref.IsSimpleIdenitifer)
                                        {
                                            if (leftref.BasicReference.Spelling == include.Name)
                                            {
                                                GetTokenById(tokens, leftref.BasicReference.StartToken).Text = include.Expression.ToString().Trim();
                                            }
                                        }
                                        if (rightref != null && rightref.IsSimpleIdenitifer)
                                        {
                                            if (rightref.BasicReference.Spelling == include.Name)
                                            {
                                                GetTokenById(tokens, rightref.BasicReference.StartToken).Text = include.Expression.ToString().Trim();
                                            }
                                        }

                                        break;
                                    }
                            }
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

            var token_list = LexIncludeFile(Folder + "\\" + include.Filename);
            var token_stream = SyscodeCompiler.GetStreamFromList(token_list);
            var parser = new SyscodeParser(token_stream);
            var cst = parser.compilation();

            Dictionary<string, IConstant> constants = new();

            var builder = new SyscodeAstBuilder(constants,reporter);

            var ast = builder.Generate(cst);

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

    }
}
