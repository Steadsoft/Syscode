using Antlr4.Runtime;
using System.Text;

namespace Syscode
{
    public class For : Loop
    {
        private Reference forRef;
        private Expression from;
        private Expression to;
        private Expression? by;
        private Expression? untilExp;
        private Expression? whileExp;

        public Reference ForRef { get => forRef; set => forRef = value; }
        public Expression From { get => from; set => from = value; }
        public Expression To { get => to; set => to = value; }
        public Expression? By { get => by; set => by = value; }
        public Expression? UntilExp { get => untilExp; set => untilExp = value; }
        public Expression? WhileExp { get => whileExp; set => whileExp = value; }

        public For(ParserRuleContext context) : base(context)
        {
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            if (By == null) 
                builder.Append($"for {ForRef} = {From} to {To}");
            else
                builder.Append($"for {ForRef} = {From} to {To} by {By}");

            if (WhileExp != null)
                builder.Append($" while {WhileExp}");
            
            if (UntilExp != null)
                builder.Append($" until {UntilExp}");

            return builder.ToString();
        }
    }
}
