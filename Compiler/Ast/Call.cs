using static SyscodeParser;

namespace Syscode
{
    public class Call : AstNode
    {
        private Reference reference;
        public Call(CallContext context, SyscodeAstBuilder builder) : base(context)
        {
            reference = builder.CreateReference(context.Ref);
        }

        public Reference Reference { get => reference; set => reference = value; }

        public override string ToString()
        {
            return $"call {Reference}";
        }
    }
}