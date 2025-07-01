using Syscode.Ast;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syscode
{
    public class SymtabBuilder
    {
        public SymtabBuilder()
        {

        }

        public void Generate(Compilation root)
        {
            var declarations = root.Statements.Where(s => s is Declare).Cast<Declare>();

            root.Symbols = declarations.Select(d => CreateSymbol(d)).ToList();
        }

        public Symbol CreateSymbol(Declare declaration)
        {
            if (declaration.StructBody == null)
            {
                return new Symbol(declaration);
            }

            return null;
        }

    }
}
