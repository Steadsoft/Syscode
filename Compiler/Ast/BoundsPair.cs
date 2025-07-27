using Antlr4.Runtime;
using static SyscodeParser;

namespace Syscode
{
    public class BoundsPair : AstNode
    {
        private Expression upper;
        private Expression? lower;

        public Expression Upper { get => upper; set => upper = value; }
        public Expression? Lower { get => lower; set => lower = value; }

        public BoundsPair(BoundPairContext context, AstBuilder builder) : base(context)
        {
            this.upper = builder.CreateExpression(context.Upper);
            this.lower = context.Lower?.SafeCreate(builder.CreateExpression);
        }
    }
}