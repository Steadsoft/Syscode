using Antlr4.Runtime;
using Microsoft.VisualBasic;
using static SyscodeParser;

namespace Syscode
{
    public class Expression : AstNode
    {
        public Literal? Literal;
        public Reference? Reference;
        public Expression? Left;
        public Expression? Right;
        public Operator Operator;
        public ExpressionType Type;
        public bool Parenthesized;
        public Expression(ParserRuleContext context, SyscodeAstBuilder builder) : base(context)
        {
            Literal = null;
            Reference = null;
            Left = null;
            Right = null;
            Operator = Operator.UNDEFINED;
            Type = ExpressionType.None;

            switch (context)
            {
                case ExprPrimitiveContext primitive when primitive.Primitive is RefContext reference:
                    {
                        Reference = builder.CreateReference(reference.Reference);
                        Type = ExpressionType.Primitive;
                        break;
                    }
                case ExprPrimitiveContext primitive when primitive.Primitive is LiteralArithmeticContext literal:
                    {
                        Literal = new Literal(literal.Numeric, Operator.UNDEFINED, builder.Constants);
                        Type = ExpressionType.Literal;
                        break;
                    }
                case ExprPrimitiveContext primitive when primitive.Primitive is LiteralStringContext strng:
                    {
                        Literal = new Literal(strng.String);
                        Type = ExpressionType.Literal;
                        break;
                    }
                case ExprPrefixedContext prefix when prefix.Prefixed.Expr is ExprPrimitiveContext prim && prim.Primitive is LiteralArithmeticContext literal:
                    {
                        Literal = new Literal(literal.Numeric, SyscodeAstBuilder.GetOperator(prefix), builder.Constants);
                        Type = ExpressionType.Literal;
                        break;
                    }
                case ExprPrefixedContext prefixed:
                    {
                        Right = builder.CreateExpression(prefixed.Prefixed.Expr);
                        Operator = SyscodeAstBuilder.GetOperator(prefixed);
                        Type = ExpressionType.Prefix;
                        break;
                    }
                case ExprBinaryContext binary:
                    {
                        Left = builder.CreateExpression(binary.Left);
                        Right = builder.CreateExpression(binary.Rite);
                        Operator = SyscodeAstBuilder.GetOperator(binary);
                        Type = ExpressionType.Binary;
                        break;
                    }
                default:  // every other case always contains an operator and a left/right expression. 
                    {
                        throw new InvalidOperationException("Unexpected expression class encountered.");
                    }
            }


        }

        public bool IsSimpleIdentifier => Reference != null && Reference.IsSimpleIdentifier;
        public string SimpleIdentifier => Reference.SimpleIdentifier;
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

            Reference?.ApplyPreprocessorReplace(tokens, replace);

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
}