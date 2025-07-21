using Syscode.Ast;
using static SyscodeParser;

namespace Syscode
{
    public class Declare : AstNode, ISpelling
    {
        public string TypeName;
        public string As;
        public DataType CoreType;
        private StorageClass storageClass = StorageClass.Unspecified;
        private StorageScope storageScope = StorageScope.Unspecified;
        private List<BoundsPair> bounds = new();
        private string spelling;
        private StructBody structBody;
        public List<Attribute> Attributes = new();
        public List<Expression> typeSubscripts = new();
        private bool varying;
        private bool constantSize = true;
        private Alignment alignment = new Alignment();
        private IContainer container;
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
        private bool isKeyword;
        public bool Varying { get => varying; internal set => varying = value; }
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

        public Declare(IContainer container, DeclareContext context) : base(context)
        {
            if (context.Type != null) 
                CoreType = GetCoreType(context.Type);

            this.container = container;
        }


        public override string ToString()
        {
            return $"dcl {Spelling} {TypeName}"; 
        }

        public static DataType GetCoreType(TypeSpecifierContext context)
        {
            if (context.Fix != null) return (DataType)(context.Fix.Typename.Type);
            if (context.Bit != null) return (DataType)(context.Bit.Typename.Type);
            if (context.Str != null) return (DataType)(context.Str.Typename.Type);
            if (context.Ent != null) return (DataType)(context.Ent.Typename.Type);
            if (context.Lab != null) return (DataType)(context.Lab.Typename.Type);
            if (context.Ptr != null) return (DataType)(context.Ptr.Typename.Type);
            if (context.Builtin != null) return (DataType)(context.Builtin.Typename.Type);

            return DataType.UNDEFINED;

        }
    }
}