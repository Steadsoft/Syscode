using Antlr4.Runtime;
using static SyscodeParser;

namespace Syscode
{
    public class Scope : AstNode, IContainer
    {
        public string Spelling;
        public bool IsBlockScope = false;
        private List<AstNode> statements = new();
        private List<Symbol> symbols = new();
        private IContainer container;
        public Scope(ParserRuleContext context) : base(context)
        {
            if (context is BlockScopeContext)
            {
                IsBlockScope = true;
                Spelling = context.GetLabelText(nameof(BlockScopeContext.Name));
            }

        }
        public IContainer Container { get { return container; } set { container = value; } }
        public List<AstNode> Statements { get => statements; set => statements = value; }
        public List<Symbol> Symbols { get => symbols; set => symbols = value; }
    }
}