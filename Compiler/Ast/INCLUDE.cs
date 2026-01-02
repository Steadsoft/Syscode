using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SyscodeParser;

namespace Syscode
{
    public class INCLUDE : AstNode
    {
        public  string? Filename;
        public  readonly string? Name;
        public INCLUDE(Prep_INCLUDEContext context, SyscodeAstBuilder builder):base(context)
        {
            Filename = context.File?.Text.Strip('"');
            Name = context.Name?.GetText();
        }
    }
}
