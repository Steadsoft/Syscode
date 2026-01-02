using Antlr4.Runtime;
using static SyscodeParser;

namespace Syscode
{
    public class Until : Loop, IReplaceContainer
    {
        private readonly Expression untilExp;
        private Expression? whileExp;   // optional

        public Expression UntilExp { get => untilExp; }
        public Expression? WhileExp { get => whileExp; set => whileExp = value; }

        public Until(LoopUntilContext context, SyscodeAstBuilder builder) : base(context)
        {
            this.untilExp = builder.CreateExpression(context.Until.Until.Exp);
            this.whileExp = context.Until.While?.Exp.SafeCreate(builder.CreateExpression);
            this.Statements = builder.GenerateStatements(context.Until._Statements);
            this.Label = context.Until.Name?.GetText().Replace("@", "");
            if (context.Until.Name is not null)
                haslabel = true;

        }

        public override string ToString()
        {
            if (WhileExp != null) 
               return $"until {UntilExp} while {WhileExp}";

            return $"until {UntilExp}";
        }

        public void ApplyPreprocessorReplace(List<IToken> tokens, REPLACE replace)
        {
            UntilExp.ApplyPreprocessorReplace(tokens, replace);
            WhileExp?.ApplyPreprocessorReplace(tokens, replace);
            base.ApplyPreprocessorReplace(tokens, replace);
        }
    }
}
