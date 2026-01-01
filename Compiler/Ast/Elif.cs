using Antlr4.Runtime;
using static SyscodeParser;

namespace Syscode
{
    public class Elif : AstNode, IReplaceCandidate
    {
        private Expression condition;
        private List<AstNode> statements = new();

        public Expression Condition { get => condition; set => condition = value; }
        public List<AstNode> Statements { get => statements; set => statements = value; }

        public Elif(ElifBlockContext context, SyscodeAstBuilder builder) : base(context)
        {
            condition = builder.CreateExpression(context.ConditionalStatements.Condition);
            statements = builder.GenerateStatements(context.ConditionalStatements._Statements);
        }

        public override string ToString()
        {
            return $"{nameof(If)}: ";
        }

        public void ApplyPreprocessorReplace(List<IToken> tokens, REPLACE replace)
        {
            Condition.ApplyPreprocessorReplace(tokens, replace);

            foreach (var stmt in statements.OfType<IReplaceCandidate>())
            {
                stmt.ApplyPreprocessorReplace(tokens, replace);
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