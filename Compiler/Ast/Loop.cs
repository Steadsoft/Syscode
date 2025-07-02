using Antlr4.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.Marshalling;
using System.Text;
using System.Threading.Tasks;

namespace Syscode
{
    public class Loop : AstNode, IStatementContainer
    {
        private List<AstNode> statements;
        public Loop(ParserRuleContext context) : base(context)
        {

        }

        public List<AstNode> Statements { get => statements;  set => statements = value; }
    }
}
