using Antlr4.Runtime;
using Syscode.Ast;

namespace Syscode
{
    public class Procedure : AstNode, IContainer , ISpelling
    {
        private string spelling;
        private bool isFunction;
        private string @as;
        private List<string> parameters = new List<string>();
        private List<AstNode> statements = new List<AstNode>();
        private List<Symbol> symbols = new List<Symbol>();
        private IContainer container;
        private bool main;
        private StorageClass storageClass = StorageClass.Unspecified;
        private StorageScope storageScope = StorageScope.Unspecified;
        public Procedure(ParserRuleContext context, IContainer Container) : base(context)
        {
            this.Container = Container;
        }

        public List<AstNode> Statements { get => statements; set => statements = value; }
        public IContainer Container { get => container;  set => container = value; }
        public List<Symbol> Symbols { get => symbols; set => symbols = value; }
        public bool IsFunction { get => isFunction; set => isFunction = value; }
        public string As { get => @as; set => @as = value; }
        public List<string> Parameters { get => parameters; set => parameters = value; }
        public string Spelling { get => spelling; set => spelling = value; }
        public bool Main { get => main; set => main = value; }
        public StorageClass StorageClass { get => storageClass; set => storageClass = value; }
        public StorageScope StorageScope { get => storageScope; set => storageScope = value; }

        public override string ToString()
        {
            return $"procedure {Spelling}";
        }
    }
}