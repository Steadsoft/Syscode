using Antlr4.Runtime;

namespace Syscode
{
    public class StructBody : AstNode
    {
        public string Spelling;
        private List<BoundsPair> bounds = new();
        private List<StructBody> structs = new List<StructBody>();
        private List<StructField> fields = new List<StructField>();

        public List<BoundsPair> Bounds { get => bounds; set => bounds = value; }
        public List<StructBody> Structs { get => structs; set => structs = value; }
        public List<StructField> Fields { get => fields; set => fields = value; }

        public StructBody(ParserRuleContext context) : base(context)
        {
        }

        public override string ToString()
        {
            return $"{nameof(StructBody)}: {Spelling}";
        }

    }
}