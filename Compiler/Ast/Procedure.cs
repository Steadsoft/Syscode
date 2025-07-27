using Antlr4.Runtime;
using System.Xml.Linq;
using static SyscodeParser;

namespace Syscode
{
    public class Procedure : AstNode, IContainer , ISpelling
    {
        private string spelling;
        private bool isFunction;
        private string @as = String.Empty;
        private List<string> parameters = new List<string>();
        private List<AstNode> statements = new List<AstNode>();
        private List<Symbol> symbols = new List<Symbol>();
        private IContainer container;
        private bool main;
        private StorageClass storageClass = StorageClass.Unspecified;
        private StorageScope storageScope = StorageScope.Unspecified;
        public Procedure(IContainer Container, ProcedureContext context, AstBuilder builder) : base(context)
        {
            container = Container;
            spelling = context.Spelling.GetText();
            parameters = context.Params == null? new List<string>() : context.Params._Params.Select(static i => i.GetText()).ToList();
            isFunction = false;

            if (context.Options != null && context.Options.Main != null)
                main = true;
        }

        public Procedure(IContainer Container, FunctionContext context, AstBuilder builder) : base(context)
        {
            container = Container;
            spelling = context.Spelling.GetText();
            parameters = context.Params == null ? new List<string>() : context.Params._Params.Select(static i => i.GetText()).ToList();
            isFunction = true;

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
    }
}