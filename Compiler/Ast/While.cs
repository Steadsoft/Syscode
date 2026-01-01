using Antlr4.Runtime;
using static SyscodeParser;

namespace Syscode
{
    public class While : Loop, IReplaceCandidate
    {
        private readonly Expression whileExp;
        private Expression? untilExp;   // optional

        public Expression WhileExp { get => whileExp; }
        public Expression? UntilExp { get => untilExp; set => untilExp = value; }

        public While(LoopWhileContext context, SyscodeAstBuilder builder) : base(context)
        {
            this.whileExp = builder.CreateExpression(context.While.While.Exp);
            this.untilExp = context.While.Until?.Exp.SafeCreate(builder.CreateExpression);
            this.Statements = builder.GenerateStatements(context.While._Statements);
            this.Label = context.While.Name?.GetText().Replace("@", "");

            if (context.While.Name is not null)
                haslabel    = true;
        }

        public void ApplyPreprocessorReplace(List<IToken> tokens, REPLACE replace)
        {
            WhileExp.ApplyPreprocessorReplace(tokens, replace);
            UntilExp?.ApplyPreprocessorReplace(tokens, replace);
        }

        public override string ToString()
        {
            if (UntilExp != null)
                return $"while {WhileExp} until {UntilExp}";

            return $"while {WhileExp}";
        }
    }
}
