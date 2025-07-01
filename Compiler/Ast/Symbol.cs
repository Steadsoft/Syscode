using Antlr4.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Syscode.Ast
{
    public class Symbol  
    {
        private bool invalid = true;
        private bool structure = false;
        private List<BoundsPair> bounds = new();
        private Declare declaration;
        private CoreType coreType;
        private int? precision;
        private int? scale;
        private bool signed;
        private bool varying;
        private int length;
        public Symbol(Declare declaration) 
        {
            this.declaration = declaration;

        }

        public List<BoundsPair> Bounds { get => declaration.Bounds; }
        public string Spelling { get => declaration.Spelling; }
        public bool Structure { get => declaration.StructBody != null; }
        public CoreType CoreType { get => coreType; internal set => coreType = value; }
        public int? Precision { get => precision; internal set => precision = value; }
        public bool Signed { get => Signed1; internal set => Signed1 = value; }
        public int? Scale { get => scale; internal set => scale = value; }
        public bool Signed1 { get => signed; internal set => signed = value; }
        public bool Invalid { get => invalid; internal set => invalid = value; }
        public bool Varying { get => varying; internal set => varying = value; }
        public int Length { get => length; internal set => length = value; }
    }
}
