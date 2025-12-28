using static SyscodeParser;

namespace Syscode
{
    public class Qualification : AstNode
    {
        public string Spelling;
        public Arguments Arguments;
        private readonly bool isKeyword;

        public Qualification(StructureQualificationContext context, SyscodeAstBuilder builder) : base(context)
        {
            Spelling = context.Spelling.GetText();
            isKeyword = context.children.OfType<IdentifierContext>().Single().children.OfType<KeywordContext>().Any();

            if (context.Args is not null)
            {
                Arguments = new Arguments(context.Args, builder);
            }
        }

        public override string ToString()
        {
            return Spelling + Arguments?.ToString();
        }
    }
}