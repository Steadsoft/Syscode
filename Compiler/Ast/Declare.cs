using Antlr4.Runtime;
using static SyscodeParser;

namespace Syscode
{
    public class Declare : AstNode
    {
        public string TypeName;
        public List<BoundsPair> Bounds = new();
        public int Length;
        public string Spelling;
        public StructBody StructBody;
        public Declare(ParserRuleContext context) : base(context)
        {

            //Spelling = context.GetLabelText(nameof(DeclareContext.Spelling));
        }
    }
}