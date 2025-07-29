using Antlr4.Runtime;
using static SyscodeParser;

namespace Syscode
{
    public class ExpressionBase : AstNode
    {
        public ExpressionBase(ExpressionContext expr) : base(expr)
        {

        }


    }
    public class BinaryExpression : ExpressionBase
    {
        public Expression Left;
        public Expression Right;
        public Operator Operator;

        public BinaryExpression(ExprBinaryContext expr) : base(expr)
        {
        }
    }

    public class Expression : AstNode
    {
        public Literal? Literal;
        public Reference? Reference;
        public Expression? Left;
        public Expression? Right;
        public Operator Operator;
        public ExpressionType Type;
        public bool Parenthesized;
        public Expression(ParserRuleContext context) : base(context)
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