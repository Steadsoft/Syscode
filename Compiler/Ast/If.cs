using Antlr4.Runtime;
using static SyscodeParser;

namespace Syscode
{
    public class If : AstNode
    {
        private GeneralExpression condition;
        private List<AstNode> thenStatements = new List<AstNode>();
        private List<AstNode> elseStatements = new List<AstNode>();
        private List<Elif> elifStatements = new List<Elif>();
        public GeneralExpression Condition { get => condition;}
        public List<AstNode> ThenStatements { get => thenStatements; set => thenStatements = value; }
        public List<AstNode> ElseStatements { get => elseStatements; set => elseStatements = value; }
        public List<Elif> ElifStatements { get => elifStatements; set => elifStatements = value; }
        public If(IfContext context, AstBuilder builder) : base(context)
        {
            condition = builder.CreateExpression(context.ExprThen.Exp);
        }
        public override string ToString()
        {
            return $"{nameof(If)}: ";
        }
    }
}