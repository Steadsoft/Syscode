using Antlr4.Runtime;

namespace Syscode
{
    public class Elif : AstNode
    {
        private Expression condition;
        private List<AstNode> thenStatements = new List<AstNode>();

        public Expression Condition { get => condition; set => condition = value; }
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