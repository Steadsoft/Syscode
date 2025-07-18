using System.Xml.Linq;

namespace Syscode
{
    public class Symbol : ISpelling  
    {
        private bool invalid = true;
        private Declare declaration;
        private Procedure procedure;
        private DataType dataType;
        private int precision;
        private int scale;
        private bool signed;
        private bool varying;
        private int bytes;
        public Symbol(Declare declaration) 
        {
            this.Declaration = declaration;
            this.dataType = declaration.CoreType;
        }

        public Symbol(Procedure procedure)
        {
            this.procedure = procedure;
            this.dataType = DataType.ENTRY;
        }

        public StructBody StructBody { get => Declaration?.StructBody; }
        public List<BoundsPair> Bounds { get => Declaration?.Bounds; }
        public string Spelling 
        { 
            get => Declaration != null ? Declaration.Spelling : procedure.Spelling; 
        }
        public AstNode Node
        {
            get
            {
                if (declaration != null)
                    return declaration;
                
                return procedure;
            }
        }
        public bool ConstantSize => declaration.ConstantSize;
        public bool IsStructure { get => Declaration?.StructBody != null; }
        public bool IsntStructure { get => !IsStructure; }
        public DataType CoreType { get => dataType;  }
        public int Precision { get => precision; internal set => precision = value; }
        public bool Signed { get => signed; internal set => signed = value; }
        public int Scale { get => scale; internal set => scale = value; }
        public bool Invalid { get => invalid; internal set => invalid = value; }
        public bool Varying { get => varying; internal set => varying = value; }
        public int Bytes { get => bytes; internal set => bytes = value; }
        /// <summary>
        /// The runtime aligmnet for this datum. 
        /// </summary>
        /// <remarks>
        /// The alignment is zero for next bit alignment, then 1 for next byte and so on
        /// </remarks>
        public int Alignment { get => declaration.Alignment; }
        public Declare Declaration { get => declaration; set => declaration = value; }
        public StorageClass StorageClass { get => declaration != null ? declaration.StorageClass : procedure.StorageClass; }
        public StorageScope StorageScope { get => declaration != null ? declaration.StorageScope : procedure.StorageScope; }

        public override string ToString()
        {
            return Spelling;
        }
    }

    public enum StorageClass
    {
        Unspecified,
        Static,
        Stack,
        Based,
        Defined
    }

    public enum StorageScope
    {
        Unspecified,
        Internal,
        External
    }
}
