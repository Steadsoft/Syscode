using Antlr4.Runtime;

namespace Syscode
{
    public class Assignment : AstNode
    {
        private Reference reference;
        private Expression expression;
        public Assignment(ParserRuleContext context, Reference reference, Expression expression) : base(context)
        {
            this.reference = reference;
            this.expression = expression;
        }

        public Reference Reference { get => reference; }
        public Expression Expression { get => expression;  }

        public override string ToString()
        {
            return $"{Reference} = {Expression}";
        }
    }
}
