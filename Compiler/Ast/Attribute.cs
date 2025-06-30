using Antlr4.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public Aligned(ParserRuleContext context) : base(context)
        {
        }
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
}
