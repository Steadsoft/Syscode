using Antlr4.Runtime;
using static SyscodeParser;

namespace Syscode
{
    public class Declare : AstNode
    {
        public string TypeName;
        public List<BoundsPair> Bounds = new();
        public string Spelling;
        public StructBody StructBody;
        public List<Attribute> Attributes = new();
        public Declare(ParserRuleContext context) : base(context)
        {
        }

        public override string ToString()
        {
            return $"dcl {Spelling} {TypeName}"; 
        }

    }
}