using Antlr4.Runtime;

namespace Syscode
{
    public class Goto : AstNode
    {
        public Reference target;
        public Goto(ParserRuleContext context) : base(context)
        {
        }

        public override string ToString()
        {
            return $"goto {target}";
        }
    }
}
