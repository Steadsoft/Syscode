using Antlr4.Runtime;

namespace Syscode
{
    public class Loop : AstNode, IStatementContainer
    {
        private List<AstNode> statements;
        private List<Symbol> symbols;

        public Loop(ParserRuleContext context) : base(context)
        {

        }

        public List<AstNode> Statements { get => statements;  set => statements = value; }
        public List<Symbol> Symbols { get => symbols; set => symbols = value; }

        public override string ToString()
        {
            return "do loop";
        }
    }
}
