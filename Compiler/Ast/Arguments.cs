using System.Text;
using static SyscodeParser;

namespace Syscode
{
    /// <summary>
    /// Represents a paranthesized commalist of expression which might be array subscripts or func/proc arguments.
    /// </summary>
    public class Arguments : AstNode
    {
        public List<Expression> ExpressionList = new();

        public Arguments(ArgumentsContext context, AstBuilder builder) : base(context)
        {
            ExpressionList = context.List.GetDerivedNodes<ExpressionContext>().Select(builder.CreateExpression).ToList();
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new();

            stringBuilder.Append("(");

            foreach (Expression e in ExpressionList)
            {
                stringBuilder.Append(e.ToString());
                stringBuilder.Append(",");
            }

            stringBuilder.Length = stringBuilder.Length - 1;

            stringBuilder.Append(")");

            return stringBuilder.ToString();
        }
    }
}