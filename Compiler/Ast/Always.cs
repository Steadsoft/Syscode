using static SyscodeParser;

namespace Syscode
{
    public class Always : Loop
    {
        public Always(LoopAlwaysContext context, AstBuilder builder) : base(context)
        {
            Statements = context.Always._Statements.Select(builder.Generate).ToList();
        }
    }
}
