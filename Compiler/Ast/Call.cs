using Antlr4.Runtime;

namespace Syscode
{
    public class Call : AstNode
    {
        private Reference reference;
        public Call(ParserRuleContext context) : base(context)
        {
        }

        public Reference Reference { get => reference; set => reference = value; }

        public override string ToString()
        {
            return $"call {Reference}";
        }
    }

}
