using Antlr4.Runtime;

namespace Syscode
{
    public class While : Loop
    {
        private Expression whileExp;
        private Expression untilExp;   // optional

        public Expression WhileExp { get => whileExp; set => whileExp = value; }
        public Expression UntilExp { get => untilExp; set => untilExp = value; }

        public While(ParserRuleContext context) : base(context)
        {
        }

        public override string ToString()
        {
            if (UntilExp != null)
                return $"while {WhileExp} until {UntilExp}";

            return $"while {WhileExp}";
        }
    }
}
