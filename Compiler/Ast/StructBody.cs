using static SyscodeParser;

namespace Syscode
{
    public class StructBody : AstNode
    {
        private readonly string spelling;
        private readonly List<BoundsPair> bounds = new();
        private readonly List<StructBody> structs = new();
        private readonly List<StructField> fields = new();
        private readonly bool isKeyword;
        public List<BoundsPair> Bounds { get => bounds; }
        public List<StructBody> Structs { get => structs; }
        public List<StructField> Fields { get => fields; }
        public bool IsKeyword { get => isKeyword;}
        public string Spelling => spelling;

        public StructBody(StructBodyContext context, SyscodeAstBuilder builder) : base(context)
        {
            spelling = context.Spelling.GetText();
            isKeyword = context.Spelling.children.OfType<KeywordContext>().Any();

            if (context.Dims != null)
            {
                bounds = builder.CreateBounds(context.Dims);
            }

            structs = context._Structs.Select(builder.CreateStructure).ToList(); //context.GetExactNodes<StructBodyContext>().Select(CreateStructure).ToList();
            fields = context._Fields.Select(builder.CreateField).ToList();  //context.GetExactNodes<StructFieldContext>().Select(CreateField).ToList(); ;
        }

        public override string ToString()
        {
            return $"{nameof(StructBody)}: {spelling}";
        }
    }
}