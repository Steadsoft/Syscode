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
    }
}
