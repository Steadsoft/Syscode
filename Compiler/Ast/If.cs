using Antlr4.Runtime;

namespace Syscode
{
    public class If : AstNode
    {
        private Expression expr;
        private List<AstNode> thenStatements = new List<AstNode>();
        private List<AstNode> elseStatements = new List<AstNode>();
        private List<Elif> elifStatements = new List<Elif>();

        public Expression Expr { get => expr; set => expr = value; }
        public List<AstNode> ThenStatements { get => thenStatements; set => thenStatements = value; }
        public List<AstNode> ElseStatements { get => elseStatements; set => elseStatements = value; }
        public List<Elif> ElifStatements { get => elifStatements; set => elifStatements = value; }

        public If(ParserRuleContext context) : base(context)
        {
        }

        public override string ToString()
        {
            return $"{nameof(If)}: ";
        }
    }
}