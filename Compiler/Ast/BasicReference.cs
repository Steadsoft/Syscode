using System.Text;
using static SyscodeParser;

namespace Syscode
{
    public class BasicReference : AstNode
    {
        public string Spelling;
        private List<Qualification> qualifierList = new();
        private Symbol? symbol;
        private readonly bool isKeyword;
        public BasicReference(BasicReferenceContext context, SyscodeAstBuilder builder) : base(context)
        {
            Spelling = context.GetLabelText(nameof(BasicReferenceContext.Spelling));
            isKeyword = context.children.OfType<IdentifierContext>().Single().children.OfType<KeywordContext>().Any();

            if (context.Qualification is not null)
            {
                qualifierList = context.Qualification._Qualifiers.Select(q => new Qualification(q, builder)).ToList();
            }
        }

        public bool IsQualified { get => Qualifier.Count != 0; }
        public bool IsntQualified { get => !IsQualified; }
        public Symbol? Symbol { get => symbol; internal set => symbol = value; }
        public List<Qualification> Qualifier { get => qualifierList; set => qualifierList = value; }
        public bool IsKeyword { get => isKeyword;  }
        public bool IsntKeyword { get => !IsKeyword;  }

        public override string ToString()
        {
            StringBuilder builder = new();

            foreach (var qual in Qualifier)
            {
                builder.Append($"{qual.ToString()}.");
            }

            builder.Append(Spelling);

            return builder.ToString();
        }
    }
}