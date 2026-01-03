using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SyscodeParser;

namespace Syscode
{
    public class INCLUDE : AstNode
    {
        public  string? Filename;
        public  readonly string? Name;
        public INCLUDE(Prep_INCLUDEContext context, SyscodeAstBuilder builder):base(context)
        {
            Filename = context.File?.Text.Strip('"');
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

            return Directory.GetFiles(Folder,Filename);
        }
    }
}
