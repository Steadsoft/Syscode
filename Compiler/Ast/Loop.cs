using Antlr4.Runtime;

namespace Syscode
{
    public class Loop : AstNode, IStatementContainer
    {
        private List<AstNode> statements;
        public Loop(ParserRuleContext context) : base(context)
        {

        }

        public List<AstNode> Statements { get => statements;  set => statements = value; }

        public override string ToString()
        {
            return "do loop";
        }
    }
}
