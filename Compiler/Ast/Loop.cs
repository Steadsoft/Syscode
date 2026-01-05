using Antlr4.Runtime;
using static SyscodeParser;

namespace Syscode
{
    public abstract class Loop : AstNode, IContainer, IReplaceContainer
    {
        public string? Label;
        public bool HasLabel => Label != null;
        public List<AstNode> Statements { get; protected set; }
        public List<Symbol> Symbols { get; set; }
        public IContainer Container { get; set; }
        public Loop(LoopContext context) : base(context)
        {
        }
        public override string ToString()
        {
            return "do loop";
        }

        public virtual void ApplyPreprocessorReplace(List<IToken> tokens, REPLACE replace)
        {
            foreach (var stmt in Statements.OfType<IReplaceContainer>())
            {
                stmt.ApplyPreprocessorReplace(tokens, replace);
            }
        }
    }
}
