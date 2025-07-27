using Syscode.Ast;
using static SyscodeParser;

namespace Syscode
{
    
    public class Declare : AstNode, ISpelling
    {
        private (int precision, int scale, bool signed) BinDesc;  // represents the details of bin and ubin types.
        private (int length, bool varying) StringDesc;
        
        public string TypeName;
        public bool packed = false;
        public bool varAttributePresent = false; // for entry variables or varying strings
        public string As;
        public DataType CoreType = DataType.UNDEFINED;
        private StorageClass storageClass = StorageClass.Unspecified;
        private StorageScope storageScope = StorageScope.Unspecified;
        private List<BoundsPair> bounds = new();
        private string spelling;
        private StructBody structBody;
        public List<Attribute> Attributes = new();
        public List<Expression> typeSubscripts = new();
        private bool constantSize = true;
        private Alignment alignment = new Alignment();
        private IContainer container;
        private bool isKeyword;
        private bool validated = false;

        public StructBody StructBody 
        { 
            get => structBody; 
            set
            {
                structBody = value;
                TypeName = "structure"; // not possible in grammar but helps by avoiding null typename
                CoreType = DataType.STRUCT;
            }
        }
        public bool IsArray { get => Bounds.Any(); }
        public bool ConstantSize { get => constantSize; }
        public bool IsntArray { get => !IsArray; }
        public bool Varying 
        { 
            get
            {
                if (CoreType == DataType.STRING && varAttributePresent)
                   return true;
                return false;
            }
        }
        public bool Variable
        {
            get
            {
                if (CoreType == DataType.ENTRY && varAttributePresent)
                    return true;
                return false;
            }
        }

        public string Spelling { get => spelling; set => spelling = value; }
        public bool IsKeyword { get => isKeyword; set => isKeyword = value; }
        public bool IsStructure { get => structBody != null; }
        public bool IsntStructure { get => !IsStructure; }
        public List<BoundsPair> Bounds 
        { 
            get => bounds; 
            set
            {
                bounds = value;

                foreach (var pair in bounds)
                {
                    if (!pair.Upper.IsConstant)
                    {
                        constantSize = false;
                        return;
                    }

                    if (pair.Lower != null)
                        if (!pair.Lower.IsConstant)
                        {
                            constantSize = false;
                            return;
                        }
                }
            }
        }
        public Alignment Alignment { get => alignment; internal set => alignment = value; }
        public StorageClass StorageClass { get => storageClass; set => storageClass = value; }
        public StorageScope StorageScope { get => storageScope; set => storageScope = value; }
        public IContainer Container { get => container;  }
        public (int precision, int scale, bool signed) BIN { get => BinDesc; set => BinDesc = value; }
        public (int length, bool varying) STRING { get => StringDesc; set => StringDesc = value; }
        /// <summary>
        /// Indicates that the declaration contains no errors or inconsistencies
        /// </summary>
        public bool Validated { get => validated; set => validated = value; }
        public Declare(IContainer container, DeclareContext context) : base(context)
        {
            this.container = container;
        }
        public override string ToString()
        {
            return $"dcl {Spelling} {TypeName}"; 
        }
    }
}