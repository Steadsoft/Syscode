using Antlr4.Runtime;
using Syscode.Ast;

namespace Syscode
{
    public class Compilation : AstNode , IStatementContainer
    {
        private List<AstNode> statements = new();
        public List<Symbol> Symbols = new();
        public Compilation(ParserRuleContext context) : base(context)
        {
        }

        public List<AstNode> Statements { get => statements; set => statements = value; }
    }
}