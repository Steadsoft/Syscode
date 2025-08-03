using Antlr4.Runtime;
using static SyscodeParser;

namespace Syscode
{
    public class Label : AstNode
    {
        public string spelling;
        public string? subscript;
        public Label(AlabelContext context) : base(context)
        {
            spelling = context.Name.Spelling.GetText();
            subscript = context.Subscript?.Literal.GetText();
        }
        public string Spelling => spelling;
        public string? Subscript => subscript;

        public override string ToString()
        {
            return "@" + spelling;
        }
    }
}
