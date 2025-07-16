using System.Xml.Linq;

namespace Syscode
{
    public class Symbol : ISpelling  
    {
        private bool invalid = true;
        private Declare declaration;
        private Procedure procedure;
        private CoreType coreType;
        private int precision;
        private int scale;
        private bool signed;
        private bool varying;
        private int length;
        private int alignment;
        public Symbol(Declare declaration) 
        {
            this.Declaration = declaration;
        }

        public Symbol(Procedure procedure)
        {
            this.procedure = procedure;
        }

        public StructBody StructBody { get => Declaration?.StructBody; }
        public List<BoundsPair> Bounds { get => Declaration?.Bounds; }
        public string Spelling 
        { 
            get => Declaration != null ? Declaration.Spelling : procedure.Spelling; 
        }
        public bool IsStructure { get => Declaration?.StructBody != null; }
        public bool IsntStructure { get => !IsStructure; }
        public CoreType CoreType { get => coreType; internal set => coreType = value; }
        public int Precision { get => precision; internal set => precision = value; }
        public bool Signed { get => signed; internal set => signed = value; }
        public int Scale { get => scale; internal set => scale = value; }
        public bool Invalid { get => invalid; internal set => invalid = value; }
        public bool Varying { get => varying; internal set => varying = value; }
        public int Length { get => length; internal set => length = value; }
        /// <summary>
        /// The runtime aligmnet for this datum. 
        /// </summary>
        /// <remarks>
        /// The alignment is zero for next bit alignment, then 1 for next byte and so on
        /// </remarks>
        public int Alignment { get => alignment; set => alignment = value; }
        public Declare Declaration { get => declaration; set => declaration = value; }

        public override string ToString()
        {
            return Spelling;
        }
    }
}
