using Antlr4.Runtime;
using System.Text;
using static SyscodeParser;

namespace Syscode
{
    public class BasicReference : AstNode
    {
        public string Spelling;
        private List<Qualification> qualifier = new();
        private bool resolved = false;
        private Symbol symbol;
        public BasicReference(ParserRuleContext context) : base(context)
        {
            Spelling = context.GetLabelText(nameof(BasicReferenceContext.Spelling));
        }

        public bool Resolved { get => resolved; internal set => resolved = value; }
        public bool IsQualified { get => Qualifier.Any(); }
        public bool IsntQualiofied { get => !IsQualified; }
        public Symbol Symbol { get => symbol; internal set => symbol = value; }
        public List<Qualification> Qualifier { get => qualifier; set => qualifier = value; }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            foreach (var qual in Qualifier)
            {
                builder.Append($"{qual.ToString()}.");
            }

            builder.Append(Spelling);

            return builder.ToString();
        }
    }
}