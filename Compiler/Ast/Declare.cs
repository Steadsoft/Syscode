using Antlr4.Runtime;
using static SyscodeParser;

namespace Syscode
{
    public class Declare : AstNode
    {
        public string TypeName;
        public List<BoundsPair> Bounds = new();
        public string Spelling;
        private StructBody structBody;
        public List<Attribute> Attributes = new();
        public List<Expression> typeSubscripts = new();

        public StructBody StructBody 
        { 
            get => structBody; 
            set
            {
                structBody = value;
                TypeName = "structure"; // not possible in grammar but helps by avoiding null typename
            }
        }

        public Declare(ParserRuleContext context) : base(context)
        {
        }

        public override string ToString()
        {
            return $"dcl {Spelling} {TypeName}"; 
        }

    }
}