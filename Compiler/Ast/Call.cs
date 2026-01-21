using static SyscodeParser;

namespace Syscode
{
    public class Call : AstNode
    {
        public Call(CallContext context, SyscodeAstBuilder builder) : base(context)
        {
            Reference = builder.CreateReference(context.Ref);
        }

        public Reference Reference { get; private set; }

        public override string ToString()
        {
            return $"call {Reference}";
        }
    }
}