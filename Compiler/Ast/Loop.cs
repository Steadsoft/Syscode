using Antlr4.Runtime;
using static SyscodeParser;

namespace Syscode
{
    public abstract class Loop : AstNode, IContainer
    {
        private List<AstNode> statements;
        private List<Symbol> symbols;
        private string? label;
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
    }
}
