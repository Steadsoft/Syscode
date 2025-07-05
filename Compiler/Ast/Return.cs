using Antlr4.Runtime;

namespace Syscode
{
    public class Return : AstNode
    {
        private Expression expression;
        public Return(ParserRuleContext context) : base(context)
        {
        }

        public Expression Expression { get => expression; set => expression = value; }

        public override string ToString()
        {
            return $"return ({Expression})";
        }
    }

}
