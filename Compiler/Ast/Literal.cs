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
        private string value = String.Empty;
        private LiteralType literalType;
        private NumericConstant constant;
        private Operator prefixop;
        public Literal(NumericLiteralContext context, Operator prefixop, Dictionary<string, IConstant> constants) : base(context)
        {
            constant = new NumericConstant(context.GetText(),prefixop);

            constants.TryAdd(constant.Spelling, constant);

            this.prefixop = prefixop;

            literalType = context switch
            {
                { Bin: not null } => LiteralType.Binary,
                { Oct: not null } => LiteralType.Octal,
                { Dec: not null } => LiteralType.Decimal,
                { Hex: not null } => LiteralType.Hexadecimal,
                _ => throw new InvalidOperationException("No valid literal found")
            };

            value = context.GetText();

            switch (literalType)
            {
                case LiteralType.Binary:
                    {
                        break;
                    }

                case LiteralType.Octal:
                    {
                        break;
                    }
                case LiteralType.Decimal:
                    {
                        break;
                    }
                case LiteralType.Hexadecimal:
                    {
                        break;
                    }

            }
        }
        public Operator PrefixOp => prefixop;
        public bool Signed => prefixop == Operator.UNDEFINED ? false : true;

        public string Value => value;

        public NumericConstant Constant
        {
            get { return constant; }
        }

        public Literal(StringLiteralContext context) :base(context) 
        {
            value = context.Text.Text;
            literalType = LiteralType.String;
        }

        public Literal(LiteralType literalType) : base(null)
        {
            this.literalType = literalType;
        }

        public LiteralType LiteralType { get => literalType;  }

        public override string ToString()
        {
            return value;
        }
    }
}
