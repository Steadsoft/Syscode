using Antlr4.Runtime;

namespace Syscode
{
    public class Expression : AstNode
    {
        public Literal Literal;
        public Reference Reference;
        public Expression Left;
        public Expression Right;
        public Operator Operator;
        public ExpressionType Type;
        public Expression(ParserRuleContext context) : base(context)
        {
            Literal = null;
            Reference = null;
            Left = null;
            Right = null;
            Operator = Operator.UNDEFINED;
            Type = ExpressionType.None;
        }

        public override string ToString()
        {
            return Type switch
            {
                ExpressionType.Primitive => Reference.ToString(),
                ExpressionType.Parenthesized => $"({Reference.ToString()})",
                ExpressionType.Binary => $"{Left} {LexerHelper.GetOperatorText(Operator)} {Right}" ,
                ExpressionType.Literal => Literal.ToString(),
                ExpressionType.Prefix => $"{LexerHelper.GetOperatorText(Operator)} {Right}"
            };
        }

    }
}