using Antlr4.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SyscodeParser;

namespace Syscode
{
    public class Type : AstNode
    {
        public string Spelling;
        public StructBody Body;
        public Type(TypeContext context) : base(context)
        {
            Spelling = context.Body.Spelling.GetText();
        }

        public override string ToString()
        {
            return $"type {Spelling}";
        }
    }
}
