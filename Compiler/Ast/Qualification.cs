using Antlr4.Runtime;
using static SyscodeParser;

namespace Syscode
{
    public class Qualification : AstNode
    {
        public string Spelling;
        public Arguments Arguments;
        public Qualification(ParserRuleContext context) : base(context)
        {
            Spelling = context.GetLabelText(nameof(StructureQualificationContext.Spelling));

        }

        public override string ToString()
        {
            return Spelling + Arguments?.ToString();
        }
    }
}