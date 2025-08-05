using Antlr4.Runtime;
using static SyscodeParser;

namespace Syscode
{
    public class Assignment : AstNode
    {
        private readonly Reference reference;
        private readonly Expression expression;
        public Assignment(AssignmentContext context, AstBuilder builder) : base(context)
        {
            this.reference = builder.CreateReference(context.Ref);
            this.expression = builder.CreateExpression(context.Exp);
        }

        public Reference Reference { get => reference; }
        public Expression Expression { get => expression;  }

        public override string ToString()
        {
            return $"{Reference} = {Expression}";
        }
    }
}
