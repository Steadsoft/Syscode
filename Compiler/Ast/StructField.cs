using static Syscode.LexerHelper.TokenText;

namespace Syscode
{
    public class StructField   : AstNode
    {
        public string Spelling;
        public string TypeName;
        public List<BoundsPair> Bounds = new();
        public int Length;
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
        }

        public override string ToString()
        {
            return $"{nameof(StructField)}: {Spelling}";
        }
    }
}