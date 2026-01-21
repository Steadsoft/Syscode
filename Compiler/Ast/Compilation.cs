using static SyscodeParser;

namespace Syscode
{
    public class Compilation : AstNode , IContainer
    {
        private List<Symbol> symbols = new();
        private IContainer container;
        public Compilation(CompilationContext context, SyscodeAstBuilder builder) : base(context)
        {
            Statements = builder.GenerateStatements(context._Statements);
        }

        public List<AstNode> Statements { get; private set; }
        public List<Symbol> Symbols { get => symbols; set => symbols = value; }
        public IContainer Container { get => container; set => container = value; }
    }
}