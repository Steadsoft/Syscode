using Antlr4.Runtime;
using Syscode.Ast;

namespace Syscode
{
    public class Procedure : Declare, IStatementContainer
    {
        public string Spelling;
        public bool isFunction;
        public string As;
        public List<string> Parameters = new List<string>();
        private List<AstNode> statements = new List<AstNode>();
        public List<Symbol> Symbols = new List<Symbol>();
        private Procedure container;
        public Procedure(ParserRuleContext context, Procedure Container) : base(context)
        {
            this.Container = Container;
        }

        public List<AstNode> Statements { get => statements; set => statements = value; }
        public Procedure Container { get => container; internal set => container = value; }

        public override string ToString()
        {
            return $"procedure {Spelling}";
        }
    }
}