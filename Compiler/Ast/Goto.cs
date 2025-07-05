using Antlr4.Runtime;

namespace Syscode
{
    public class Goto : AstNode
    {
        private Reference target;
        public Goto(ParserRuleContext context) : base(context)
        {
        }

        public Reference Target { get => target; internal set => target = value; }

        public override string ToString()
        {
            return $"goto {Target}";
        }
    }
}
