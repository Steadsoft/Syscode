using Antlr4.Runtime;
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        public Preprocessor(List<IToken> tokens, string folder, Reporter reporter  )
        {
            this.token_list = tokens;
            this.folder = folder;
            this.reporter = reporter;
        }

        public List<IToken> Apply(AstNode root)
        {

            switch (root)
            {
                case Compilation context:
                    ProcessCompilation(token_list, context, folder); 
                    break;
            }

            int index = 0;

            foreach (CommonToken token in token_list)
            {
                token.TokenIndex = index++;
            }

            return token_list;
        }

        private void ProcessCompilation(List<IToken> tokens, AstNode context, string folder)
        {
            int prior_lines = 0;

            foreach (var statement in ((Compilation)context).Statements)
            {
                if (statement is IF if_statement)
                {
                    ;
                }

                if (statement is INCLUDE include_statement)
                {
                    ProcessInclude(tokens, include_statement, folder, ref prior_lines);
                }

                if (statement is REPLACE replace_statement)
                {
                    ;//ProcessInclude(tokens, include_statement, folder);
                }

            }
        }
        private void ProcessInclude(List<IToken> tokens, INCLUDE include, string Folder, ref int PriorLines)
        {
            include.StartToken += PriorLines;
            include.EndToken += PriorLines; 

            var token_list = LexIncludeFile(Folder + "\\" + include.Filename);
            var token_stream = SyscodeCompiler.GetStreamFromList(token_list);
            var parser = new SyscodeParser(token_stream);
            var cst = parser.compilation();

            Dictionary<string, IConstant> constants = new();

            var builder = new SyscodeAstBuilder(constants,reporter);

            var ast = builder.Generate(cst);

            ProcessCompilation(token_list, ast, folder);

            int remove_count = (include.EndToken - include.StartToken) + 1;
            int insert_count = token_list.Count;

            tokens.RemoveRange(include.StartToken, remove_count);
            tokens.InsertRange(include.StartToken, token_list);

            PriorLines += (insert_count - remove_count);
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
