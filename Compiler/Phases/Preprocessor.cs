using Antlr4.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SyscodeParser;

namespace Syscode.Phases
{
    internal class Preprocessor
    {
        private List<IToken> tokens;
        private string folder;
        public Preprocessor(List<IToken> tokens, string folder  )
        {
            this.tokens = tokens;
            this.folder = folder;
        }

        public List<IToken> Apply(AstNode root)
        {

            switch (root)
            {
                case Compilation context:
                    ProcessCompilation(context); 
                    break;
            }

            return tokens;
        }

        private void ProcessCompilation(Compilation context)
        {
            foreach (var stmt in context.Statements)
            {
                if (stmt is IF)
                {
                    ;
                }

                if (stmt is INCLUDE inc)
                {
                    ProcessInclude(inc);
                }
            }
        }

        private void ProcessInclude(INCLUDE stmt)
        {
            ProcessPreprocessorDirectives(stmt, folder);
        }

        private void ProcessPreprocessorDirectives(INCLUDE stmt, string Folder)
        {
            var inctokens = LexIncludeFile(Folder + "\\" + stmt.Filename);
            tokens.RemoveRange(stmt.StartToken, stmt.EndToken - stmt.StartToken);
            tokens.InsertRange(stmt.StartToken, inctokens);
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
