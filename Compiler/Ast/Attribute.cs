using Antlr4.Runtime;

namespace Syscode
{
    public class Attribute : AstNode
    {
        public Attribute(ParserRuleContext context) : base(context)
        {
        }
    }
    public class Aligned : Attribute
    {
        private Expression alignment = null;
        public Aligned(ParserRuleContext context, Expression alignment) : base(context)
        {
            this.Alignment = alignment;
        }

        public Expression Alignment { get => alignment; set => alignment = value; }
    }
    public class Unaligned : Attribute
    {
        public Unaligned(ParserRuleContext context) : base(context)
        {
        }
    }
    public class Const : Attribute
    {
        public Const(ParserRuleContext context) : base(context)
        {
        }
    }
    public class Static : Attribute
    {
        public Static(ParserRuleContext context) : base(context)
        {
        }
    }
    public class External : Attribute
    {
        public External(ParserRuleContext context) : base(context)
        {
        }
    }
}