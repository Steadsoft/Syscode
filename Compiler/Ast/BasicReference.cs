using Antlr4.Runtime;
using System.Text;
using static SyscodeParser;

namespace Syscode
{
    public class BasicReference : AstNode, IReplaceContainer
    {
        public BasicReference(BasicReferenceContext context, SyscodeAstBuilder builder) : base(context)
        {
            Spelling = context.GetLabelText(nameof(BasicReferenceContext.Spelling));
            IsKeyword = context.children.OfType<IdentifierContext>().Single().children.OfType<KeywordContext>().Any();

            if (context.Qualification is not null)
            {
                Qualifier = context.Qualification._Qualifiers.Select(q => new Qualification(q, builder)).ToList();
            }
        }

        private Symbol? symbol;
        public string Spelling { get; private set; } = String.Empty;
        public bool IsQualified { get => Qualifier.Count != 0; }
        public bool IsntQualified { get => !IsQualified; }
        public Symbol? Symbol { get => symbol; internal set => symbol = value; }
        public List<Qualification> Qualifier { get; private set; } = [];
        public bool IsKeyword { get; private set; }
        public bool IsntKeyword { get => !IsKeyword;  }

        public override string ToString()
        {
            StringBuilder builder = new();

            foreach (var qual in Qualifier)
            {
                builder.Append($"[QUAL<{qual.ToString()}>].");
            }

            builder.Append(Spelling);

            return builder.ToString();
        }

        public void ApplyPreprocessorReplace(List<IToken> tokens, REPLACE replace)
        {
            foreach (var qual in Qualifier)
            {
                qual.ApplyPreprocessorReplace(tokens, replace);
            }
        }
    }
}