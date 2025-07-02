using Antlr4.Runtime;

namespace Syscode
{
    public class While : Loop
    {
        public Expression whileExp;
        public Expression untilExp;   // optional
        public While(ParserRuleContext context) : base(context)
        {
        }

        public override string ToString()
        {
            if (untilExp != null)
                return $"while {whileExp} until {untilExp}";

            return $"while {whileExp}";
        }
    }
}
