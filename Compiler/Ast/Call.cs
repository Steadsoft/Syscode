using Antlr4.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syscode
{
    public class Call : AstNode
    {
        public Reference Reference;
        public Call(ParserRuleContext context) : base(context)
        {
        }

        public override string ToString()
        {
            return $"call {Reference}";
        }
    }

}
