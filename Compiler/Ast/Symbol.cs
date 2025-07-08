using System.Xml.Linq;

namespace Syscode
{
    public class Symbol  
    {
        private bool invalid = true;
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

        public StructBody StructBody { get => declaration.StructBody; }
        public List<BoundsPair> Bounds { get => declaration.Bounds; }
        public string Spelling { get => declaration.Spelling; }
        public bool IsStructure { get => declaration.StructBody != null; }
        public bool IsntStructure { get => !IsStructure; }
        public CoreType CoreType { get => coreType; internal set => coreType = value; }
        public int? Precision { get => precision; internal set => precision = value; }
        public bool Signed { get => signed; internal set => signed = value; }
        public int? Scale { get => scale; internal set => scale = value; }
        public bool Invalid { get => invalid; internal set => invalid = value; }
        public bool Varying { get => varying; internal set => varying = value; }
        public int Length { get => length; internal set => length = value; }

        public override string ToString()
        {
            return declaration.ToString();
        }
    }
}
