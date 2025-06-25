using Antlr4.Runtime;

namespace Syscode
{
    public class Return : AstNode
    {
        public Expression Expression;
        public Return(ParserRuleContext context) : base(context)
        {
        }
    }

}
