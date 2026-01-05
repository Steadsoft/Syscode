using Antlr4.Runtime;
using static SyscodeParser;

namespace Syscode
{
    public class While : Loop, IReplaceContainer
    {
        public Expression WhileCondition { get; private set; }
        public Expression? UntilCondition { get; private set; }
        public While(LoopWhileContext context, SyscodeAstBuilder builder) : base(context)
        {
            WhileCondition = builder.CreateExpression(context.While.While.Exp);
            UntilCondition = context.While.Until?.Exp.SafeCreate(builder.CreateExpression);
            Statements = builder.GenerateStatements(context.While._Statements);
            Label = context.While.Name?.GetText().Replace("@", "");
        }
        public override void ApplyPreprocessorReplace(List<IToken> tokens, REPLACE replace)
        {
            WhileCondition.ApplyPreprocessorReplace(tokens, replace);
            UntilCondition?.ApplyPreprocessorReplace(tokens, replace);
            base.ApplyPreprocessorReplace(tokens, replace);
        }
        public override string ToString()
        {
            if (UntilCondition != null)
                return $"while {WhileCondition} until {UntilCondition}";

            return $"while {WhileCondition}";
        }
    }
}