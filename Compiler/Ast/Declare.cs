using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using static SyscodeParser;

namespace Syscode
{
    public class Declare : AstNode
    {
        public string TypeName;
        public CoreType CoreType;
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
            if (context.TryGetExactNode<TypenameContext>(out var tn))
                if (tn.TryGetExactNode<TypeCodeContext>(out var tc))
                {
                    var terminal = (TerminalNodeImpl)tc.children.Where(c => c is TerminalNodeImpl).Single();

                    CoreType = (CoreType)(terminal.Symbol.Type);
                }
        }

        public override string ToString()
        {
            return $"dcl {Spelling} {TypeName}"; 
        }
    }
}