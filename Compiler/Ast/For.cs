using Antlr4.Runtime;
using System.Text;

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

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            if (by == null) 
                builder.Append($"for {forRef} = {from} to {to}");
            else
                builder.Append($"for {forRef} = {from} to {to} by {by}");

            if (whileExp != null)
                builder.Append($" while {whileExp}");
            
            if (untilExp != null)
                builder.Append($" until {untilExp}");

            return builder.ToString();
        }
    }
}
