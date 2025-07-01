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
        private bool structure = false;
        private List<BoundsPair> bounds = new();
        private Declare declaration;
        private CoreType coreType;
        private int precision;
        private int scale;
        private bool signed;
        public Symbol(Declare declaration) 
        {
            this.declaration = declaration;

            if (declaration.TypeName == "label")
                CoreType = CoreType.Label;

            if (declaration.TypeName.StartsWith("bin"))
            {
                CoreType = CoreType.Binary;
                Signed = false;

                if (declaration.TypeName == "bin8")
                    precision = 8;

                if (declaration.TypeName == "bin16")
                    precision = 16;

                if (declaration.TypeName == "bin32")
                    precision = 32;

                if (declaration.TypeName == "bin64")
                    precision = 64;

                if (declaration.typeSubscripts.Any())
                {
                    if (declaration.typeSubscripts.Count == 1)
                    {
                        if (declaration.typeSubscripts[0].Type != ExpressionType.Literal)
                            ;

                        precision = Convert.ToInt32(declaration.typeSubscripts[0].Literal.Value);
                    }

                    if (declaration.typeSubscripts.Count == 2)
                    {
                        if (declaration.typeSubscripts[0].Type != ExpressionType.Literal)
                            ;
                        if (declaration.typeSubscripts[1].Type != ExpressionType.Literal)
                            ;

                        precision = Convert.ToInt32(declaration.typeSubscripts[0].Literal.Value);
                        scale = Convert.ToInt32(declaration.typeSubscripts[1].Literal.Value);
                    }

                }

            }

            if (declaration.TypeName.StartsWith("ubin"))
            {
                CoreType = CoreType.Binary;
                Signed = true;

                if (declaration.TypeName == "ubin8")
                    precision = 8;

                if (declaration.TypeName == "ubin16")
                    precision = 16;

                if (declaration.TypeName == "ubin32")
                    precision = 32;
                if (declaration.TypeName == "ubin64")
                    precision = 64;
            }


            if (declaration.TypeName == "entry")
                CoreType = CoreType.Entry;

            if (declaration.TypeName == "string")
                CoreType = CoreType.String;


        }

        public List<BoundsPair> Bounds { get => declaration.Bounds; }
        public string Spelling { get => declaration.Spelling; }
        public bool Structure { get => declaration.StructBody != null; }
        public CoreType CoreType { get => coreType; private set => coreType = value; }
        public int Precision { get => precision; private set => precision = value; }
        public bool Signed { get => signed; private set => signed = value; }
    }
}
