using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using static SyscodeParser;

namespace Syscode
{
    public class Declare : AstNode, ISpelling
    {
        public string TypeName;
        public string As;
        public CoreType CoreType;
        public List<BoundsPair> Bounds = new();
        private string spelling;
        private StructBody structBody;
        public List<Attribute> Attributes = new();
        public List<Expression> typeSubscripts = new();
        private bool varying;
        public StructBody StructBody 
        { 
            get => structBody; 
            set
            {
                structBody = value;
                TypeName = "structure"; // not possible in grammar but helps by avoiding null typename
            }
        }
        private bool isKeyword;
        public bool Varying { get => varying; internal set => varying = value; }
        public string Spelling { get => spelling; set => spelling = value; }
        public bool IsKeyword { get => isKeyword; set => isKeyword = value; }
        public bool IsStructure { get => structBody != null; }
        public bool IsntStructure { get => !IsStructure; }
        public Declare(DeclareContext context) : base(context)
        {
            if (context.Type != null) 
                CoreType = GetCoreType(context.Type);
        }

        public override string ToString()
        {
            return $"dcl {Spelling} {TypeName}"; 
        }

        public static CoreType GetCoreType(TypeSpecifierContext context)
        {
            if (context.Fix != null) return (CoreType)(context.Fix.Typename.Type);
            if (context.Bit != null) return (CoreType)(context.Bit.Typename.Type);
            if (context.Str != null) return (CoreType)(context.Str.Typename.Type);
            if (context.Ent != null) return (CoreType)(context.Ent.Typename.Type);
            if (context.Lab != null) return (CoreType)(context.Lab.Typename.Type);
            if (context.Ptr != null) return (CoreType)(context.Ptr.Typename.Type);

            return CoreType.UNDEFINED;

        }
    }
}