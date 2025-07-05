using Antlr4.Runtime;
using System.Text;
using static SyscodeParser;

namespace Syscode
{
    public class BasicReference : AstNode
    {
        public string Spelling;
        public List<Qualification> qualifier = new();
        private bool resolved = false;
        public BasicReference(ParserRuleContext context) : base(context)
        {
            Spelling = context.GetLabelText(nameof(BasicReferenceContext.Spelling));
        }

        public bool Resolved { get => resolved; internal set => resolved = value; }
        public bool IsQualified { get => qualifier.Any(); }
        public bool IsntQualiofied { get => !IsQualified; }
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            foreach (var qual in qualifier)
            {
                builder.Append($"{qual.ToString()}.");
            }

            builder.Append(Spelling);

            return builder.ToString();
        }
    }
}