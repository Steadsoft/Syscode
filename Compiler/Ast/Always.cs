using static SyscodeParser;

namespace Syscode
{
    public class Always : Loop
    {
        public Always(LoopAlwaysContext context, AstBuilder builder) : base(context)
        {
            this.Statements = context.Always._Statements.Select(builder.Generate).ToList();
            this.Label = context.Always.Name?.GetText().Replace("@", "");
        }
    }
}
