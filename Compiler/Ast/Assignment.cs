using Antlr4.Runtime;

namespace Syscode
{
    public class Assignment : AstNode
    {
        public Reference Reference;
        public Expression Expression;
        public Assignment(ParserRuleContext context) : base(context)
        {

        }

        public override string ToString()
        {
            return $"{Reference} = {Expression}";
        }
    }
}
