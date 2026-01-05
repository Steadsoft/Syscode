using Antlr4.Runtime;
using static SyscodeParser;

namespace Syscode
{
    public class Elif : AstNode, IReplaceContainer
    {
        public Expression Condition { get; private set; }
        public List<AstNode> Statements { get; set; } = new();
        public Elif(ElifContext context, SyscodeAstBuilder builder) : base(context)
        {
            Condition = builder.CreateExpression(context.block.Condition);
            Statements = builder.GenerateStatements(context.block._Statements);
        }
        public override string ToString()
        {
            return $"{nameof(If)}: ";
        }
        public void ApplyPreprocessorReplace(List<IToken> tokens, REPLACE replace)
        {
            Condition.ApplyPreprocessorReplace(tokens, replace);

            foreach (var statement in Statements.OfType<IReplaceContainer>())
            {
                statement.ApplyPreprocessorReplace(tokens, replace);
            }
        }
    }

    

    public class ELIF : AstNode
    {
        private Expression condition;
        private List<AstNode> statements = new();

        public Expression Condition { get => condition; set => condition = value; }
        public List<AstNode> Statements { get => statements; set => statements = value; }

        public ELIF(Prep_exprThenBlockContext context, SyscodeAstBuilder builder) : base(context)
        {
            condition = builder.CreateExpression(context.Expression);
        }

        public override string ToString()
        {
            return $"{nameof(If)}: ";
        }
    }

}