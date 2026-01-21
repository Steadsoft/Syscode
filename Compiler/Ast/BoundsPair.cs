using static SyscodeParser;

namespace Syscode
{
    public class BoundsPair : AstNode
    {
        public Expression Upper { get; private set; }
        public Expression? Lower { get; private set; }

        public BoundsPair(BoundPairContext context, SyscodeAstBuilder builder) : base(context)
        {
            Upper = builder.CreateExpression(context.Upper);
            Lower = context.Lower?.SafeCreate(builder.CreateExpression);
        }
    }
}