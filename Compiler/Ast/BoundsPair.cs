using Antlr4.Runtime;
using static SyscodeParser;

namespace Syscode
{
    public class BoundsPair : AstNode
    {
        private GeneralExpression upper;
        private GeneralExpression? lower;

        public GeneralExpression Upper { get => upper; set => upper = value; }
        public GeneralExpression? Lower { get => lower; set => lower = value; }

        public BoundsPair(BoundPairContext context, AstBuilder builder) : base(context)
        {
            this.upper = builder.CreateExpression(context.Upper);
            this.lower = context.Lower?.SafeCreate(builder.CreateExpression);
        }
    }
}