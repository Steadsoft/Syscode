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
        public readonly string Filename;
        public INCLUDE(Prep_INCLUDEContext context):base(context)
        {
            Filename = context.File.Text.Strip('"');
        }
    }
}
