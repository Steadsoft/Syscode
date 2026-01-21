using Antlr4.Runtime;
using static SyscodeParser;

namespace Syscode
{
    public class Assignment : AstNode, IReplaceContainer
    {
        private readonly IToken? mode;
        public Assignment(AssignmentContext context, SyscodeAstBuilder builder) : base(context)
        {
            this.Reference = builder.CreateReference(context.Ref);
            this.Expression = builder.CreateExpression(context.Exp);
            this.mode = context.Mode?.Start;
        }

        public Reference Reference { get; private set; }
        public Expression Expression { get; private set; }

        public void ApplyPreprocessorReplace(List<IToken> tokens, REPLACE replace)
        {
            Reference.ApplyPreprocessorReplace(tokens, replace);
            Expression.ApplyPreprocessorReplace(tokens, replace);
        }

        public override string ToString()
        {
            return $"{Reference} = {Expression}";
        }
    }
}
