using static SyscodeParser;

namespace Syscode
{
    public class Always : Loop
    {
        public Always(LoopAlwaysContext context, AstBuilder builder) : base(context)
        {
            this.Statements = builder.GetStatements(context.Always).Select(builder.Generate).ToList();
            this.Label = context.Always.Name?.GetText().Replace("@", "");
        }

        public override string ToString()
        {
            return $"do @{Label} loop";
        }
    }
}
