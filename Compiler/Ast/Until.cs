using Antlr4.Runtime;
using static SyscodeParser;

namespace Syscode
{
    public class Until : Loop
    {
        private readonly Expression untilExp;
        private Expression? whileExp;   // optional

        public Expression UntilExp { get => untilExp; }
        public Expression? WhileExp { get => whileExp; set => whileExp = value; }

        public Until(LoopContext context, AstBuilder builder) : base(context)
        {
            this.untilExp = builder.CreateExpression(context.Until.Until.Exp);
            this.whileExp = context.While.Until?.Exp.SafeCreate(builder.CreateExpression);
        }

        public override string ToString()
        {
            if (WhileExp != null) 
               return $"until {UntilExp} while {WhileExp}";

            return $"until {UntilExp}";
        }
    }
}
