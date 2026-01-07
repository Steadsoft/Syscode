using static SyscodeParser;

namespace Syscode
{
    public class Always : LoopBase
    {
        public Always(LoopAlwaysContext context, SyscodeAstBuilder builder) : base(context)
        {
            this.Statements = builder.GenerateStatements(context.Always._Statements);
            this.Label = context.Always.Name?.GetText().Replace("@", "");
        }

        public override string ToString()
        {
            return $"do @{Label} loop";
        }
    }
}
