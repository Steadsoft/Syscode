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
        public INCLUDE(Prep_INCLUDEContext context, SyscodeAstBuilder builder):base(context)
        {
            Filename = context.File.Text.Strip('"');
        }
    }

    public class REPLACE : AstNode
    {
        public readonly string Name;
        public readonly Expression Expression;
        public REPLACE(Prep_REPLACEContext context, SyscodeAstBuilder builder) : base(context)
        {
            Name = context.Name.GetText();
            Expression = builder.CreateExpression(context.expression());
        }
    }
}
