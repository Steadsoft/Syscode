using Antlr4.Runtime;

namespace Syscode
{
    //public class ExpressionBase : AstNode
    //{
    //    public ExpressionBase(ExpressionContext expr) : base(expr)
    //    {

    //    }


    //}
    //public class BinaryExpression : ExpressionBase
    //{
    //    public Expression Left;
    //    public Expression Right;
    //    public Operator Operator;

    //    public BinaryExpression(ExprBinaryContext expr) : base(expr)
    //    {
    //    }
    //}

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

        public bool IsSimpleIdentifier => Reference != null && Reference.IsSimpleIdenitifer;
        public string SimpleIdentifier => Reference.SimpleIdentifer;
        /// <summary>
        /// Examines the expression to ascertain whether it is
        /// an identifier that is referred to in a preprocessor REPLACE statement 
        /// and if so, modifies the token referred to by the expression with its
        /// replace value, it goes recursive if the expression has sub left/right nodes.
        /// </summary>
        /// <param name="tokens"></param>
        /// <param name="replace"></param>

        public void ApplyPreprocessorReplace(List<IToken> tokens, REPLACE replace)
        {
            // Apply to this node
            if (IsSimpleIdentifier && SimpleIdentifier == replace.Name)
            {
                ((CommonToken)tokens[Reference.BasicReference.StartToken]).Text =
                    replace.Expression.ToString().Trim();
            }

            // Recurse
            Left?.ApplyPreprocessorReplace(tokens, replace);
            Right?.ApplyPreprocessorReplace(tokens, replace);
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
                ExpressionType.Binary => $"{Left} {LexerHelper.GetOperatorText(Operator)} {Right}",
                ExpressionType.Literal => Literal?.ToString(),
                ExpressionType.Prefix => $"{LexerHelper.GetOperatorText(Operator)} {Right}",
                ExpressionType.None => "INVALID TYPE!",
                _ => throw new NotImplementedException()
            };

            if (Parenthesized)
                text = $"({text})";

            return text;
        }

    }

    public interface IReplaceCandidate
    {
        void ApplyPreprocessorReplace(List<IToken> tokens, REPLACE replace);
    }
}