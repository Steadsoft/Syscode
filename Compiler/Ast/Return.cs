using Antlr4.Runtime;
using static SyscodeParser;

namespace Syscode
{
    public class Return : AstNode
    {
        private Expression? expression;
        public Return(ReturnContext context, AstBuilder builder) : base(context)
        {
            context.Exp?.SafeCreate(builder.CreateExpression);
        }

        public Expression? Expression { get => expression; set => expression = value; }

        public override string ToString()
        {
            return $"return ({Expression})";
        }
    }

}
