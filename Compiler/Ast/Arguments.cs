using Antlr4.Runtime;
using System.Text;

namespace Syscode
{
    /// <summary>
    /// Represents a paranthesized commalist of expression which might be array subscripts or func/proc arguments.
    /// </summary>
    public class Arguments : AstNode
    {
        public List<Expression> ExpressionList = new();

        public Arguments(ParserRuleContext context) : base(context)
        {
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append("(");

            foreach (Expression e in ExpressionList)
            {
                stringBuilder.Append(e.ToString());
                stringBuilder.Append(", ");
            }

            stringBuilder.Length = stringBuilder.Length - 2;

            stringBuilder.Append(")");

            return stringBuilder.ToString();
        }
    }
}