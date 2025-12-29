using static SyscodeParser;

namespace Syscode
{
    public class Elif : AstNode
    {
        private Expression condition;
        private List<AstNode> thenStatements = new();

        public Expression Condition { get => condition; set => condition = value; }
        public List<AstNode> ThenStatements { get => thenStatements; set => thenStatements = value; }

        public Elif(ExprThenBlockContext context, SyscodeAstBuilder builder) : base(context)
        {
            condition = builder.CreateExpression(context.Exp);
        }

        public override string ToString()
        {
            return $"{nameof(If)}: ";
        }
    }

    public class ELIF : AstNode
    {
        private Expression condition;
        private List<AstNode> thenStatements = new();

        public Expression Condition { get => condition; set => condition = value; }
        public List<AstNode> ThenStatements { get => thenStatements; set => thenStatements = value; }

        public ELIF(ExprThenBlockContext context, SyscodeAstBuilder builder) : base(context)
        {
            condition = builder.CreateExpression(context.Exp);
        }

        public override string ToString()
        {
            return $"{nameof(If)}: ";
        }
    }

}