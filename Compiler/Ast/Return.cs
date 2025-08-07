using static SyscodeParser;

namespace Syscode
{
    public class Return : AstNode
    {
        private Expression? expression;
        public Return(ReturnContext context, AstBuilder builder) : base(context)
        {
            expression = context.Exp?.SafeCreate(builder.CreateExpression);
        }

        public Expression? Expression { get => expression; }

        public override string ToString()
        {
            return $"return ({Expression})";
        }
    }

}
