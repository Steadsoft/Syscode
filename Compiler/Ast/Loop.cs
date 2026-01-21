using static SyscodeParser;

namespace Syscode
{
    public class Loop : AstNode
    {
        public Loop(LoopContext context, SyscodeAstBuilder builder) : base(context)
        {
            if (context.Ref is not null)
                Reference = builder.CreateReference(context.Ref);
        }
        public Reference? Reference { get; private set; }
        public override string ToString()
        {
            return $"proceed {Reference}";
        }
    }
}