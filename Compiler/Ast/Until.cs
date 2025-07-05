using Antlr4.Runtime;

namespace Syscode
{
    public class Until : Loop
    {
        private Expression untilExp;
        private Expression whileExp;   // optional

        public Expression UntilExp { get => untilExp; set => untilExp = value; }
        public Expression WhileExp { get => whileExp; set => whileExp = value; }

        public Until(ParserRuleContext context) : base(context)
        {
        }

        public override string ToString()
        {
            if (WhileExp != null) 
               return $"until {UntilExp} while {WhileExp}";

            return $"until {UntilExp}";
        }
    }
}
