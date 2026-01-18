using Antlr4.Runtime;
using static SyscodeParser;

namespace Syscode
{
    public class Procedure : AstNode, IContainer , ISpelling, IReplaceContainer
    {
        private readonly IContainer? container;
        public List<AstNode> Statements { get; private set; }
        public IContainer? Container { get => container; }
        public List<Symbol> Symbols { get; set ; } =  new List<Symbol>();
        public bool IsFunction { get; private set; }
        public string As { get; private set; } = string.Empty;
        public List<string> Parameters { get; private set; }
        public string Spelling { get; private set; }
        public bool Main { get; private set; }
        public StorageClass StorageClass { get; set; }
        public StorageScope StorageScope { get; set; }
        
        public Procedure(ref IContainer? Container, ProcedureContext context, SyscodeAstBuilder builder) : base(context)
        {
            container = Container;
            Spelling = context.Spelling.GetText();
            Parameters = context.Params == null? new List<string>() : context.Params._Params.Select(static i => i.GetText()).ToList();
            IsFunction = false;

            Container = this;
            Statements = builder.GenerateStatements(context._Statements);
            Container = container;

            if (context.Options != null && context.Options.Main != null)
                Main = true;
        }
        public Procedure(ref IContainer? Container, FunctionContext context, SyscodeAstBuilder builder) : base(context)
        {
            container = Container;
            Spelling = context.Spelling.GetText();
            Parameters = context.Params == null ? new List<string>() : context.Params._Params.Select(static i => i.GetText()).ToList();
            IsFunction = true;

            Container = this;
            Statements = builder.GenerateStatements(context._Statements);
            Container = container;

            if (context.Type != null)
                As = context.Type.GetText();

            if (context.Options != null && context.Options.Main != null)
                Main = true;
        }
        public override string ToString()
        {
            return $"procedure {Spelling}";
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