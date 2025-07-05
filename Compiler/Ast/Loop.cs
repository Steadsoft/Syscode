using Antlr4.Runtime;

namespace Syscode
{
    public class Loop : AstNode, IContainer
    {
        private List<AstNode> statements;
        private List<Symbol> symbols;
        public Loop(ParserRuleContext context) : base(context)
        {

        }

        public List<AstNode> Statements { get => statements;  set => statements = value; }
        public List<Symbol> Symbols { get => symbols; set => symbols = value; }
        public IContainer Container { get => null; set { } }
        public override string ToString()
        {
            return "do loop";
        }
    }
}
