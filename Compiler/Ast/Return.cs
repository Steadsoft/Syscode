using Antlr4.Runtime;

namespace Syscode
{
    public class Return : AstNode
    {
        public Expression Expression;
        public Return(ParserRuleContext context) : base(context)
        {
        }

        public override string ToString()
        {
            return $"return ({Expression})";
        }
    }

}
