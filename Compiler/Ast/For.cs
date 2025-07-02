using Antlr4.Runtime;

namespace Syscode
{
    public class For : Loop
    {
        public Reference forRef;
        public Expression from;
        public Expression to;
        public Expression by;
        public Expression untilExp;
        public Expression whileExp;
        public For(ParserRuleContext context) : base(context)
        {
        }
    }
}
