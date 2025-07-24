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

            literalType = context switch
            {
                { Bin: not null } => LiteralType.Binary,
                { Oct: not null } => LiteralType.Octal,
                { Dec: not null } => LiteralType.Decimal,
                { Hex: not null } => LiteralType.Hexadecimal,
                _ => throw new InvalidOperationException("No valid literal found")
            };
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
