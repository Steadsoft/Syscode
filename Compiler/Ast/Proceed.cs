using static SyscodeParser;

namespace Syscode
{
    public class Proceed : AstNode
    {
        private Reference? reference = null;
        public Proceed(ProceedContext context, AstBuilder builder) : base(context)
        {
            if (context.Ref is not null)
                reference = builder.CreateReference(context.Ref);
        }
        public Reference? Reference { get => reference; }
        public override string ToString()
        {
            return $"proceed {Reference}";
        }
    }
}