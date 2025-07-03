using static SyscodeParser;

namespace Syscode
{
    public class Type : AstNode
    {
        public string Spelling;
        public StructBody Body;
        public Type(TypeContext context) : base(context)
        {
            Spelling = context.Body.Spelling.GetText();
        }

        public override string ToString()
        {
            return $"type {Spelling}";
        }
    }
}
