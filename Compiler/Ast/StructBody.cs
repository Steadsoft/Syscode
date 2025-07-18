using static SyscodeParser;

namespace Syscode
{
    public class StructBody : AstNode
    {
        public string Spelling;
        private List<BoundsPair> bounds = new();
        private List<StructBody> structs = new List<StructBody>();
        private List<StructField> fields = new List<StructField>();
        private bool isKeyword;
        public List<BoundsPair> Bounds { get => bounds; set => bounds = value; }
        public List<StructBody> Structs { get => structs; set => structs = value; }
        public List<StructField> Fields { get => fields; set => fields = value; }
        public bool IsKeyword { get => isKeyword; set => isKeyword = value; }

        public StructBody(StructBodyContext context) : base(context)
        {
        }

        public override string ToString()
        {
            return $"{nameof(StructBody)}: {Spelling}";
        }

    }
}