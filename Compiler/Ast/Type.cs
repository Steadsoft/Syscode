using static SyscodeParser;

namespace Syscode
{
    public class Type : AstNode
    {
        public string Spelling;
        private StructBody body;
        public Type(TypeContext context) : base(context)
        {
            Spelling = context.Body.Spelling.GetText();
        }

        public StructBody Body { get => body; set => body = value; }

        public override string ToString()
        {
            return $"type {Spelling}";
        }
    }
}
