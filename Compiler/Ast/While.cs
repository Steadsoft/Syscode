using Antlr4.Runtime;

namespace Syscode
{
    public class While : Loop
    {
        private readonly Expression whileExp;
        private Expression? untilExp;   // optional

        public Expression WhileExp { get => whileExp; }
        public Expression? UntilExp { get => untilExp; set => untilExp = value; }

        public While(ParserRuleContext context, Expression whileexpr) : base(context)
        {
            this.whileExp = whileexpr;
        }

        public override string ToString()
        {
            if (UntilExp != null)
                return $"while {WhileExp} until {UntilExp}";

            return $"while {WhileExp}";
        }
    }
}
