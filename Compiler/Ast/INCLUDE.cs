using static SyscodeParser;

namespace Syscode
{
    public class INCLUDE : AstNode
    {
        public bool Wilcard = false;
        public  string? Filename;
        public  readonly string? Name;
        public INCLUDE(Prep_INCLUDEContext context, SyscodeAstBuilder builder):base(context)
        {
            Filename = context.File?.Text.Strip('"');

            if (Filename != null)
                if (Filename.Contains('*'))
                    Wilcard = true;

            Name = context.Name?.GetText();
        }

        public string[] GetFiles(Dictionary<string,string> replacements, string Folder)
        {
            if (Name != null)
            {
                if (replacements.ContainsKey(Name))
                {
                    if (replacements[Name].StartsWith('"') && replacements[Name].EndsWith('"'))
                        Filename = replacements[Name].Strip('"');
                    else
                    {
                        ; // error
                        return null;
                    }
                }
                else
                {
                    ; // error
                    return null;
                }
            }

            return Directory.GetFiles(Folder,Filename).OrderBy(f => f).ToArray();
        }
    }
}
