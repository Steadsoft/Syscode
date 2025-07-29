using Antlr4.Runtime;
using static SyscodeParser;

namespace Syscode
{
    public class While : Loop
    {
        private readonly GeneralExpression whileExp;
        private GeneralExpression? untilExp;   // optional

        public GeneralExpression WhileExp { get => whileExp; }
        public GeneralExpression? UntilExp { get => untilExp; set => untilExp = value; }

        public While(WhileLoopContext context, AstBuilder builder) : base(context)
        {
            this.whileExp = builder.CreateExpression(context.While.Exp);
            this.untilExp = context.Until?.Exp.SafeCreate(builder.CreateExpression);
        }

        public override string ToString()
        {
            if (UntilExp != null)
                return $"while {WhileExp} until {UntilExp}";

            return $"while {WhileExp}";
        }
    }
}
