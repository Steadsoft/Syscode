using Antlr4.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syscode
{
    public class Goto : AstNode
    {
        public Reference target;
        public Goto(ParserRuleContext context) : base(context)
        {
        }

        public override string ToString()
        {
            return $"goto {target}";
        }
    }
}
