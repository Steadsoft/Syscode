using static SyscodeParser;

namespace Syscode
{
    public class Goto : AstNode
    {
        private Reference reference;
        public Goto(GotoContext context, AstBuilder builder) : base(context)
        {
            reference = builder.CreateReference(context.Ref);
        }

        public Reference Reference { get => reference; internal set => reference = value; }

        public override string ToString()
        {
            return $"goto {Reference}";
        }
    }
}