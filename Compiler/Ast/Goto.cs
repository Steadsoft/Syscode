using Antlr4.Runtime;

namespace Syscode
{
    public class Goto : AstNode
    {
        private Reference reference;
        public Goto(ParserRuleContext context) : base(context)
        {
        }

        public Reference Reference { get => reference; internal set => reference = value; }

        public override string ToString()
        {
            return $"goto {Reference}";
        }
    }
}
