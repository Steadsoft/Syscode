using Antlr4.Runtime;
using static SyscodeParser;

namespace Syscode
{
    public class Return : AstNode, IReplaceContainer
    {
        private Expression? expression;
        public Return(ReturnContext context, SyscodeAstBuilder builder) : base(context)
        {
            expression = context.Exp?.SafeCreate(builder.CreateExpression);
        }

        public Expression? Expression { get => expression; }

        public void ApplyPreprocessorReplace(List<IToken> tokens, REPLACE replace)
        {
            expression?.ApplyPreprocessorReplace(tokens, replace);
        }

        public override string ToString()
        {
            return $"return ({Expression})";
        }
    }

}
