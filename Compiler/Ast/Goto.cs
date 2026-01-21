using Antlr4.Runtime;
using static SyscodeParser;

namespace Syscode
{
    public class Goto : AstNode, IReplaceContainer
    {
        public Reference Reference { get; private set; }
        public Goto(GotoContext context, SyscodeAstBuilder builder) : base(context)
        {
            Reference = builder.CreateReference(context.Ref);
        }
        public override string ToString()
        {
            return $"goto {Reference}";
        }

        public void ApplyPreprocessorReplace(List<IToken> tokens, REPLACE replace)
        {
            Reference.ApplyPreprocessorReplace(tokens, replace);
        }
    }
}