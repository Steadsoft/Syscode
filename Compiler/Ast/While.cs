using Antlr4.Runtime;
using static SyscodeParser;

namespace Syscode
{
    public class While : Loop
    {
        private readonly Expression whileExp;
        private Expression? untilExp;   // optional

        public Expression WhileExp { get => whileExp; }
        public Expression? UntilExp { get => untilExp; set => untilExp = value; }

        public While(LoopWhileContext context, AstBuilder builder) : base(context)
        {
            this.whileExp = builder.CreateExpression(context.While.While.Exp);
            this.untilExp = context.While.Until?.Exp.SafeCreate(builder.CreateExpression);
            this.Statements = context.While._Statements.Select(builder.Generate).ToList();
        }

        public override string ToString()
        {
            if (UntilExp != null)
                return $"while {WhileExp} until {UntilExp}";

            return $"while {WhileExp}";
        }
    }
}
