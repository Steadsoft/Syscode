using static SyscodeParser;

namespace Syscode
{
    public class Leave : AstNode
    {
        public Leave(ExitContext context, SyscodeAstBuilder builder) : base(context)
        {
            if (context.Ref is not null)
                Reference = builder.CreateReference(context.Ref);
        }
        public Reference? Reference { get; private set; }
        public override string ToString()
        {
            return $"leave {Reference}";
        }
    }
}