using Antlr4.Runtime;

namespace Syscode
{
    public class BoundsPair : AstNode
    {
        private Expression upper;
        private Expression lower;

        public Expression Upper { get => upper; set => upper = value; }
        public Expression Lower { get => lower; set => lower = value; }

        public BoundsPair(ParserRuleContext context) : base(context)
        {
        }
    }
}