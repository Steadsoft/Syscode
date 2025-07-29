using Antlr4.Runtime;
using System.Text;
using static SyscodeParser;

namespace Syscode
{
    public class For : Loop
    {
        // Compulsory
        private Reference forRef;
        private GeneralExpression from;
        private GeneralExpression to;
        // Optional
        private GeneralExpression? by;
        private GeneralExpression? untilExp;
        private GeneralExpression? whileExp;

        public Reference ForRef { get => forRef; set => forRef = value; }
        public GeneralExpression From { get => from; set => from = value; }
        public GeneralExpression To { get => to; set => to = value; }
        public GeneralExpression? By { get => by; set => by = value; }
        public GeneralExpression? UntilExp { get => untilExp; set => untilExp = value; }
        public GeneralExpression? WhileExp { get => whileExp; set => whileExp = value; }

        public For(ForLoopContext context, AstBuilder builder) : base(context)
        {
            // Compulsory
            forRef = builder.CreateReference(context.For);
            from = builder.CreateExpression(context.From);
            to = builder.CreateExpression(context.To);

            // Optional
            by = context.By?.SafeCreate(builder.CreateExpression);
            whileExp = context.While?.Exp.SafeCreate(builder.CreateExpression);
            untilExp = context.Until?.Exp.SafeCreate(builder.CreateExpression);
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
