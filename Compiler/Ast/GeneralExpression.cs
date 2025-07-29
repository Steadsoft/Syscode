using Antlr4.Runtime;
using static SyscodeParser;

namespace Syscode
{
    public class Expression : AstNode
    {
        public Expression(ExpressionContext expr) : base(expr)
        {

        }


    }
    public class BinaryExpression : Expression
    {
        public GeneralExpression Left;
        public GeneralExpression Right;
        public Operator Operator;

        public BinaryExpression(ExprBinaryContext expr) : base(expr)
        {
        }
    }

    public class GeneralExpression : AstNode
    {
        public Literal? Literal;
        public Reference? Reference;
        public GeneralExpression? Left;
        public GeneralExpression? Right;
        public Operator Operator;
        public ExpressionType Type;
        public bool Parenthesized;
        public GeneralExpression(ParserRuleContext context) : base(context)
        {
            Literal = null;
            Reference = null;
            Left = null;
            Right = null;
            Operator = Operator.UNDEFINED;
            Type = ExpressionType.None;
        }

        public bool IsConstant
        {
            get
            {
                if (Literal != null)
                    return true;

                if (Left != null && Right != null)
                    return Left.IsConstant & Right.IsConstant;

                return false;
            }
        }
        public bool IsntConstant => !IsConstant;
        public override string ToString()
        {
            var text = Type switch
            {
                ExpressionType.Primitive => Reference?.ToString(),
                ExpressionType.Binary => $"{Left} {LexerHelper.GetOperatorText(Operator)} {Right}" ,
                ExpressionType.Literal => Literal?.ToString(),
                ExpressionType.Prefix => $"{LexerHelper.GetOperatorText(Operator)} {Right}"
            };

            if (Parenthesized)
                text = $"({text})";

            return text;
        }

    }
}