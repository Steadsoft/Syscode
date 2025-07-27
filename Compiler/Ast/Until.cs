using Antlr4.Runtime;

namespace Syscode
{
    public class Until : Loop
    {
        private readonly Expression untilExp;
        private Expression? whileExp;   // optional

        public Expression UntilExp { get => untilExp; }
        public Expression? WhileExp { get => whileExp; set => whileExp = value; }

        public Until(ParserRuleContext context, Expression untilexp) : base(context)
        {
            this.untilExp = untilexp;
        }

        public override string ToString()
        {
            if (WhileExp != null) 
               return $"until {UntilExp} while {WhileExp}";

            return $"until {UntilExp}";
        }
    }
}
