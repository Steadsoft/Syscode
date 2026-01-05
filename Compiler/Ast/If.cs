using Antlr4.Runtime;
using System.Runtime.CompilerServices;
using static SyscodeParser;

namespace Syscode
{
    public class If : AstNode, IReplaceContainer
    {
        public string Label { get; private set; }
        public Expression Condition { get; private set; }
        public List<AstNode> Statements { get; private set; } = new();
        public List<Elif> Elifs { get; private set; } = new();
        public Else? Else  {get;private set;}
        public bool HasLabel => Label != null;
        public If(IfContext context, SyscodeAstBuilder builder) : base(context)
        {
            Condition = builder.CreateExpression(context.block.Condition);
            Statements = builder.GenerateStatements(context.block._Statements);

            if (context.else_block != null)
            {
                Else = new Else(context.else_block, builder);
            }

            if (context._elif_blocks.Any())  // at least one 'elif' is present
            {
                Elifs = context._elif_blocks.Select(builder.CreateElif).ToList();
            }

            if (context.Name != null)
            {
                Label = context.Name.Spelling.GetText();
            }
        }
        /// <summary>
        /// Examines the expression in the statement to ascertain whether it is
        /// an identifier that is referred to in a preprocessor REPLACE statement 
        /// and if so, modifies the token referred to by the expression with its
        /// replace value. 
        /// </summary>
        /// <param name="tokens"></param>
        /// <param name="replace"></param>
        public void ApplyPreprocessorReplace(List<IToken> tokens, REPLACE replace)
        {
            Condition.ApplyPreprocessorReplace(tokens, replace);

            foreach (var elif in Elifs)
            {
                elif.ApplyPreprocessorReplace(tokens, replace);
            }

            foreach (var statement in Statements.OfType<IReplaceContainer>())
            {
                statement.ApplyPreprocessorReplace(tokens, replace);
            }

            Else?.ApplyPreprocessorReplace(tokens, replace);
        }
        public override string ToString()
        {
            return $"{nameof(If)} {Label}: ";
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
            preprocessor = true;
            condition = builder.CreateExpression(context.ExprTHEN_block.Expression);
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
            return $"{nameof(IF)} {label}: ";
        }
    }

}