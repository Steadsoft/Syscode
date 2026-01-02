using Antlr4.Runtime;
using static SyscodeParser;

namespace Syscode
{
    public abstract class Loop : AstNode, IContainer, IReplaceContainer
    {
        private List<AstNode> statements;
        private List<Symbol> symbols;
        private string? label;
        protected bool haslabel = false;
        public bool HasLabel { get { return haslabel; } }
        public Loop(LoopContext context) : base(context)
        {
        }
        public string? Label { get => label; protected set => label = value; }
        public List<AstNode> Statements { get => statements; protected set => statements = value; }
        public List<Symbol> Symbols { get => symbols; set => symbols = value; }
        public IContainer Container { get => null; set { } }
        public override string ToString()
        {
            return "do loop";
        }

        public void ApplyPreprocessorReplace(List<IToken> tokens, REPLACE replace)
        {
            foreach (var stmt in Statements.OfType<IReplaceContainer>())
            {
                stmt.ApplyPreprocessorReplace(tokens, replace);
            }
        }
    }
}
