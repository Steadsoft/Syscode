using Antlr4.Runtime;
using static SyscodeParser;

namespace Syscode
{
    public class Dcl : AstNode
    {
        public string TypeName;
        public List<BoundsPair> Bounds = new();
        public int Length;
        public string Spelling;
        public Dcl(ParserRuleContext context) : base(context)
        {
            if (context.TryGetExactNode<DimensionSuffixContext>(out var dimensions))
            {
                var commalist = dimensions.GetExactNode<BoundPairCommalistContext>(); ;
                var pairs = commalist.GetExactNodes<BoundPairContext>();

                var lower = pairs.Select(p => p.GetExactNode<LowerBoundContext>().GetExactNode<ExpressionContext>());
                var upper = pairs.Select(p => p.GetExactNode<UpperBoundContext>().GetExactNode<ExpressionContext>());

                pairs.Select(p => new BoundsPair(p) { Lower = null /* lower */ , Upper = null /* upper */});
            }

            Spelling = context.GetLabelText(nameof(DeclareContext.Spelling));
        }
    }
}