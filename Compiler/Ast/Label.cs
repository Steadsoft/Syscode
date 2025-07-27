using Antlr4.Runtime;

namespace Syscode
{
    public class Label : AstNode
    {
        public string Spelling;
        public string? Subscript;
        public Label(ParserRuleContext context) : base(context)
        {
        }

        public override string ToString()
        {
            return "@" + Spelling;
        }
    }
}
