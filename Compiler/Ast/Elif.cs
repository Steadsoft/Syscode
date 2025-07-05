using Antlr4.Runtime;

namespace Syscode
{
    public class Elif : AstNode
    {
        private AstNode expr;
        private List<AstNode> thenStatements = new List<AstNode>();

        public AstNode Expr { get => expr; set => expr = value; }
        public List<AstNode> ThenStatements { get => thenStatements; set => thenStatements = value; }

        public Elif(ParserRuleContext context) : base(context)
        {
        }

        public override string ToString()
        {
            return $"{nameof(If)}: ";
        }
    }
}