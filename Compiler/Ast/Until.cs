using Antlr4.Runtime;
using static SyscodeParser;

namespace Syscode
{
    public class Until : Loop, IReplaceContainer
    {
        public Expression UntilCondition { get; private set; }
        public Expression? WhileCondition { get; private set; }
        public Until(LoopUntilContext context, SyscodeAstBuilder builder) : base(context)
        {
            UntilCondition = builder.CreateExpression(context.Until.Until.Exp);
            WhileCondition = context.Until.While?.Exp.SafeCreate(builder.CreateExpression);
            Statements = builder.GenerateStatements(context.Until._Statements);
            Label = context.Until.Name?.GetText().Replace("@", "");
        }
        public override void ApplyPreprocessorReplace(List<IToken> tokens, REPLACE replace)
        {
            UntilCondition.ApplyPreprocessorReplace(tokens, replace);
            WhileCondition?.ApplyPreprocessorReplace(tokens, replace);
            base.ApplyPreprocessorReplace(tokens, replace);
        }
        public override string ToString()
        {
            if (WhileCondition != null) 
               return $"until {UntilCondition} while {WhileCondition}";

            return $"until {UntilCondition}";
        }
    }
}