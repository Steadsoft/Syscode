using Antlr4.Runtime;

namespace Syscode
{
    public class StructBody : AstNode
    {
        public string Spelling;
        public List<BoundsPair> Bounds = new();
        public List<StructBody> Structs = new List<StructBody>();
        public List<StructField> Fields = new List<StructField>();
        public StructBody(ParserRuleContext context) : base(context)
        {
        }

        public override string ToString()
        {
            return $"{nameof(StructBody)}: {Spelling}";
        }

    }
}