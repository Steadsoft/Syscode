using Syscode.Ast;

namespace Syscode
{
    public class Symbol : ISpelling  
    {
        private bool invalid = true;
        private Declare declaration;
        private readonly Procedure procedure;
        private readonly DataType dataType;
        private int precision;
        private int scale;
        private bool signed;
        private bool varying;
        private int bytes;
        private readonly IContainer container;
        private StorageClass storageClass;
        private StorageScope storageScope;
        private bool constantSize;
        private string spelling;
        public Symbol(Declare declaration) 
        {
            this.Declaration = declaration;
            this.dataType = declaration.CoreType;
            this.storageClass = declaration.StorageClass;
            this.storageScope = declaration.StorageScope;
            this.constantSize = declaration.ConstantSize;
            this.spelling = declaration.Spelling;
        }
        public Symbol(Procedure procedure)
        {
            this.procedure = procedure;
            this.dataType = DataType.ENTRY;
            this.container = procedure.Container;
            this.storageClass = procedure.StorageClass;
            this.storageScope = procedure.StorageScope;
            this.constantSize = true;
            this.spelling += procedure.Spelling;
        }
        public Symbol (If If)
        {
            dataType = DataType.LABEL;
            spelling = If.Label;

        }
        public Symbol (Loop Loop)
        {
            dataType = DataType.LABEL;
            spelling = Loop.Label;
        }
        public StructBody StructBody { get => Declaration.StructBody; }
        public List<BoundsPair> Bounds { get => Declaration.Bounds; }
        public string Spelling 
        { 
            get => spelling; 
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
        public bool ConstantSize => constantSize;
        public bool IsStructure { get => Declaration?.StructBody != null; }
        public bool IsntStructure { get => !IsStructure; }
        public DataType DataType { get => dataType;  }
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
        public Alignment Alignment { get => declaration.Alignment; }
        public Declare Declaration { get => declaration; set => declaration = value; }
        public StorageClass StorageClass { get => storageClass; set => storageClass = value; }
        public StorageScope StorageScope { get => storageScope; set => storageScope = value; }
        public IContainer Container { get => declaration != null ? declaration.Container : procedure.Container; }
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
