using Antlr4.Runtime;
using static SyscodeParser;

namespace Syscode
{
    public class Compilation : AstNode , IContainer
    {
        private List<AstNode> statements = new();
        private List<Symbol> symbols = new();
        private IContainer container;
        public Compilation(CompilationContext context, AstBuilder builder) : base(context)
        {
            statements = builder.GenerateStatements(context._Statements);
        }

        public List<AstNode> Statements { get => statements; set => statements = value; }
        public List<Symbol> Symbols { get => symbols; set => symbols = value; }
        public IContainer Container { get => container; set => container = value; }
    }
}