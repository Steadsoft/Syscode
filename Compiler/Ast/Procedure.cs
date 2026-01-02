using Antlr4.Runtime;
using static SyscodeParser;

namespace Syscode
{
    public class Procedure : AstNode, IContainer , ISpelling, IReplaceContainer
    {
        private readonly string spelling;
        private readonly bool isFunction;
        private readonly string @as = String.Empty;
        private readonly List<string> parameters = new();
        private List<AstNode> statements = new();
        private List<Symbol> symbols = new();
        private readonly IContainer? container;
        private readonly bool main;
        private StorageClass storageClass = StorageClass.Unspecified;
        private StorageScope storageScope = StorageScope.Unspecified;
        public Procedure(ref IContainer? Container, ProcedureContext context, SyscodeAstBuilder builder) : base(context)
        {
            container = Container;
            spelling = context.Spelling.GetText();
            parameters = context.Params == null? new List<string>() : context.Params._Params.Select(static i => i.GetText()).ToList();
            isFunction = false;

            Container = this;
            statements = builder.GenerateStatements(context._Statements);
            Container = container;

            if (context.Options != null && context.Options.Main != null)
                main = true;
        }

        public Procedure(ref IContainer? Container, FunctionContext context, SyscodeAstBuilder builder) : base(context)
        {
            container = Container;
            spelling = context.Spelling.GetText();
            parameters = context.Params == null ? new List<string>() : context.Params._Params.Select(static i => i.GetText()).ToList();
            isFunction = true;

            Container = this;
            statements = builder.GenerateStatements(context._Statements);
            Container = container;

            if (context.Type != null)
                @as = context.Type.GetText();

            if (context.Options != null && context.Options.Main != null)
                main = true;
        }

        public List<AstNode> Statements { get => statements; set => statements = value; }
        public IContainer Container { get => container; }
        public List<Symbol> Symbols { get => symbols; set => symbols = value; }
        public bool IsFunction { get => isFunction; }
        public string As { get => @as;  }
        public List<string> Parameters { get => parameters; }
        public string Spelling { get => spelling;}
        public bool Main { get => main;  }
        public StorageClass StorageClass { get => storageClass; set => storageClass = value; }
        public StorageScope StorageScope { get => storageScope; set => storageScope = value; }

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