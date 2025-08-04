using Antlr4.Runtime;
using static SyscodeParser;

namespace Syscode
{
    public class If : AstNode
    {
        private Expression condition;
        private List<AstNode> thenStatements = new List<AstNode>();
        private List<AstNode> elseStatements = new List<AstNode>();
        private List<Elif> elifStatements = new List<Elif>();
        private string label = string.Empty;
        public string Label => label;
        public Expression Condition { get => condition;}
        public List<AstNode> ThenStatements { get => thenStatements; set => thenStatements = value; }
        public List<AstNode> ElseStatements { get => elseStatements; set => elseStatements = value; }
        public List<Elif> ElifStatements { get => elifStatements; set => elifStatements = value; }
        public If(IfContext context, AstBuilder builder) : base(context)
        {
            condition = builder.CreateExpression(context.ExprThen.Exp);

            if (context.Name != null)
                label = context.Name.GetText();
        }
        public override string ToString()
        {
            return $"{nameof(If)} {label}: ";
        }
    }
}