using Antlr4.Runtime;
using static SyscodeParser;

namespace Syscode
{
    public class Else : AstNode, IReplaceContainer
    {
        public List<AstNode> Statements { get; private set; } = new();
        public Else(ElseContext context, SyscodeAstBuilder builder)
        {
            Statements = builder.GenerateStatements(context._Statements);
        }
        public void ApplyPreprocessorReplace(List<IToken> tokens, REPLACE replace)
        {
            foreach (var statement in Statements.OfType<IReplaceContainer>())
            {
                statement.ApplyPreprocessorReplace(tokens, replace);
            }
        }
    }
}