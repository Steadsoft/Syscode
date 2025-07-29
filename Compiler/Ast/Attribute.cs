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
        private Expression? alignment = null;
        public Aligned(ParserRuleContext context, Expression alignment) : base(context)
        {
            this.Alignment = alignment;
        }

        public Expression? Alignment { get => alignment; set => alignment = value; }
    }
    public class Packed : Attribute
    {
        public Packed(ParserRuleContext context) : base(context)
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
    public class Internal : Attribute
    {
        public Internal(ParserRuleContext context) : base(context)
        {
        }
    }

    public class Stack : Attribute
    {
        public Stack(ParserRuleContext context) : base(context)
        {
        }
    }

    public class Based : Attribute
    {
        public Based(ParserRuleContext context) : base(context)
        {
        }
    }

    public class Defined : Attribute
    {
        public Defined(ParserRuleContext context) : base(context)
        {
        }
    }

    public class Initial : Attribute
    {
        private Expression expression;
        public Initial(ParserRuleContext context, Expression expression) : base(context)
        {
            this.expression = expression;
        }
    }
}