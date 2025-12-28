using static SyscodeParser;

namespace Syscode
{
    public class Leave : AstNode
    {
        private Reference? reference = null;
        public Leave(ExitContext context, SyscodeAstBuilder builder) : base(context)
        {
            if (context.Ref is not null)
                reference = builder.CreateReference(context.Ref);
        }
        public Reference? Reference { get => reference; }
        public override string ToString()
        {
            return $"leave {Reference}";
        }
    }
}