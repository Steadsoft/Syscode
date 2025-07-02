using Antlr4.Runtime;

namespace Syscode
{
    public class Until : Loop
    {
        public Expression untilExp;
        public Expression whileExp;   // optional
        public Until(ParserRuleContext context) : base(context)
        {
        }

        public override string ToString()
        {
            if (whileExp != null) 
               return $"until {untilExp} while {whileExp}";

            return $"until {untilExp}";
        }
    }
}
