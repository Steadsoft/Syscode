using static SyscodeParser;

namespace Syscode
{
    public class REPLACE : AstNode
    {
        public readonly string Name;
        public readonly Expression Expression;
        public REPLACE(Prep_REPLACEContext context, SyscodeAstBuilder builder) : base(context)
        {
            Name = context.Name.GetText();
            Expression = builder.CreateExpression(context.expression());
        }
    }
}
