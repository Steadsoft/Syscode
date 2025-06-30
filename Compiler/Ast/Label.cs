using Antlr4.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syscode
{
    public class Label : AstNode
    {
        public string Spelling;
        public string Subscript;
        public Label(ParserRuleContext context) : base(context)
        {
        }

        public override string ToString()
        {
            return Spelling;
        }
    }
}
