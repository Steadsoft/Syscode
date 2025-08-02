using static Syscode.LexerHelper.TokenText;
//using static SyscodeParser;

namespace Syscode
{
    public class StructField   : AstNode
    {
        public string Spelling;
        private string typeName;
        private List<BoundsPair> bounds = new();
        private int length;
        private bool pad;
        public string TypeName { get => typeName; set => typeName = value; }
        public List<BoundsPair> Bounds { get => bounds; set => bounds = value; }
        public int Length { get => length; set => length = value; }
        public bool Pad => pad;
        public StructField(SyscodeParser.StructFieldContext context) : base(context)
        {
            var bounds = new List<BoundsPair>();

            Spelling = context.GetLabelText(nameof(SyscodeParser.StructFieldContext.Spelling));
            var type = context.GetLabelText(nameof(SyscodeParser.StructFieldContext.Type));

            if (type.Contains(',') ==  false )
            {
                if (type.Contains(LPAR))
                {
                    int lp = type.IndexOf(LPAR);
                    int rp = type.IndexOf(RPAR);
                    TypeName = type.Substring(0, lp);
                    Length = Convert.ToInt32(type.Substring(lp + 1, rp - (lp + 1)));
                }
                else
                {
                    if (type.StartsWith(BIT))
                    {
                        TypeName = BIT;
                        Length = Convert.ToInt32(type.Substring(3));
                    }
                    if (type.StartsWith(DEC))  // BCD
                    {
                        TypeName = DEC;
                        Length = Convert.ToInt32(type.Substring(3));
                    }
                    if (type.StartsWith(BIN))
                    {
                        TypeName = BIN;
                        Length = Convert.ToInt32(type.Substring(3));
                    }
                    if (type.StartsWith(STRING))
                    {
                        TypeName = STRING;
                        Length = Convert.ToInt32(type.Substring(6));
                    }
                }
            }

            if (context._Attr.Any())
            {
                if (context._Attr.OfType<SyscodeParser.PadContext>().Any())
                    pad = true;
            }
        }

        public override string ToString()
        {
            return $"{nameof(StructField)}: {Spelling}";
        }
    }
}