using Antlr4.Runtime;
using Syscode.Ast;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SyscodeParser;

namespace Syscode
{
    public class Literal : AstNode
    {
        public string Value = String.Empty;
        private LiteralType literalType;
        public Literal(ParserRuleContext context) : base(context)
        {
            literalType = context switch
            {
                StrLiteralContext => LiteralType.String,
                NumericLiteralContext => LiteralType.Numeric,
                _ => LiteralType.Unknown
            };
        }

        public LiteralType LiteralType { get => literalType;  }

        public override string ToString()
        {
            return Value;
        }
    }
}
