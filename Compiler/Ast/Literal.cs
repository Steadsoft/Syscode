using Antlr4.Runtime;
using Syscode.Ast;
using System.Globalization;
using System.Reflection.Metadata;
using static SyscodeParser;

namespace Syscode
{
    public class Literal : AstNode
    {
        // Numeric literals are for now, represented by the actual number of bits in the literal
        // but also the numeric value is stored in a 64 bit int, signed or unsigned
        public string Value = String.Empty;
        private LiteralType literalType;
        private int bitlength; // the actual number of bits implied by the literal
        private NumericConstant constant;
        public Literal(NumericLiteralContext context) : base(context)
        {
            constant = new NumericConstant(context);

            if (context.Bin != null)
            {
                literalType = LiteralType.Binary;
            }

            if (context.Oct != null)
            {
                literalType = LiteralType.Octal;
            }

            if (context.Dec != null)
            {
                literalType = LiteralType.Decimal;
            }

            if (context.Hex != null)
            {
                literalType = LiteralType.Hexadecimal;
            }
        }

        public NumericConstant Constant
        {
            get { return constant; }
        }

        public Literal(StringLiteralContext context) :base(context) 
        {
            Value = context.Text.Text;
            literalType = LiteralType.String;
        }

        public Literal(LiteralType literalType) : base(null)
        {
            this.literalType = literalType;
        }

        public LiteralType LiteralType { get => literalType;  }

        public override string ToString()
        {
            return Value;
        }
    }
}
