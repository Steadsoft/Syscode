using Antlr4.Runtime;
using Syscode.Ast;

namespace Syscode
{
    public class Procedure : Declare, IStatementContainer
    {
        public string Spelling;
        public bool isFunction;
        public List<string> Parameters = new List<string>();
        private List<AstNode> statements = new List<AstNode>();
        public List<Symbol> Symbols = new List<Symbol>();
        public Procedure(ParserRuleContext context) : base(context)
        {
        }

        public List<AstNode> Statements { get => statements; set => statements = value; }

        public override string ToString()
        {
            return $"{nameof(Procedure)}: {Spelling}";
        }
    }
}