using static SyscodeParser;

namespace Syscode
{
    public class If : AstNode
    {
        private readonly Expression condition;
        private List<AstNode> thenStatements = new();
        private List<AstNode> elseStatements = new();
        private List<Elif> elifStatements = new();
        private readonly string label = string.Empty;
        public string Label => label;
        public Expression Condition { get => condition;}
        public List<AstNode> ThenStatements { get => thenStatements; set => thenStatements = value; }
        public List<AstNode> ElseStatements { get => elseStatements; set => elseStatements = value; }
        public List<Elif> ElifStatements { get => elifStatements; set => elifStatements = value; }
        private bool hasLabel = false;
        public bool HasLabel => hasLabel;
        public If(IfContext context, SyscodeAstBuilder builder) : base(context)
        {
            condition = builder.CreateExpression(context.ExprThen.Exp);
            thenStatements = builder.GenerateStatements(context.ExprThen.then_block._Statements);

            if (context.else_block != null)
            {
                elseStatements = builder.GenerateStatements(context.else_block.then_block._Statements);
            }

            if (context.elif_blocks != null)  // at least one 'elif' is present
            {
                elifStatements = context.elif_blocks._ExprThen.Select(builder.CreateElif).ToList();
            }

            if (context.Name != null)
            {
                label = context.Name.Spelling.GetText();
                 hasLabel = true;
            }
        }
        public override string ToString()
        {
            return $"{nameof(If)} {label}: ";
        }
    }

    public class IF : AstNode
    {
        private readonly Expression condition;
        private List<AstNode> thenStatements = new();
        private List<AstNode> elseStatements = new();
        private List<ELIF> elifStatements = new();
        private readonly string label = string.Empty;
        public string Label => label;
        public Expression Condition { get => condition; }
        public List<AstNode> ThenStatements { get => thenStatements; set => thenStatements = value; }
        public List<AstNode> ElseStatements { get => elseStatements; set => elseStatements = value; }
        public List<ELIF> ElifStatements { get => elifStatements; set => elifStatements = value; }
        private bool hasLabel = false;
        public bool HasLabel => hasLabel;
        public IF(Prep_IFContext context, SyscodeAstBuilder builder) : base(context)
        {
            condition = builder.CreateExpression(context.ExprTHEN_block.Exp);
            thenStatements = builder.GenerateStatements(context.ExprTHEN_block.THEN_block._Statements);

            if (context.ELSE_block != null)
            {
                elseStatements = builder.GenerateStatements(context.ELSE_block.THEN_block._Statements);
            }

            if (context.ELIF_block != null)  // at least one 'elif' is present
            {
                elifStatements = context.ELIF_block._ExprThenBlocks.Select(builder.CreateELIF).ToList();
            }

            if (context.Name != null)
            {
                label = context.Name.Spelling.GetText();
                hasLabel = true;
            }
        }
        public override string ToString()
        {
            return $"{nameof(If)} {label}: ";
        }
    }

}