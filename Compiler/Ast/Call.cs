using Antlr4.Runtime;

namespace Syscode
{
    public class Call : AstNode
    {
        public Reference Reference;
        public Call(ParserRuleContext context) : base(context)
        {
        }

        public override string ToString()
        {
            return $"call {Reference}";
        }
    }

}
