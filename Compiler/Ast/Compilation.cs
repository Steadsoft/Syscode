using Antlr4.Runtime;
using Syscode.Ast;

namespace Syscode
{
    public class Compilation : AstNode , IStatementContainer
    {
        private List<AstNode> statements = new();
        private List<Symbol> symbols = new();
        public Compilation(ParserRuleContext context) : base(context)
        {
        }

        public List<AstNode> Statements { get => statements; set => statements = value; }
        public List<Symbol> Symbols { get => symbols; set => symbols = value; }

    }
}