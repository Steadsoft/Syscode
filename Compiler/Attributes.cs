using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SyscodeParser;
using static Syscode.LexerHelper;

namespace Syscode
{
    public static class Attributes
    {
        private static readonly Dictionary<System.Type, string> typeToKeyword = new()
            {
                { typeof(PackedContext),    KeywordText(PACKED) },
                { typeof(VariableContext),  KeywordText(VARIABLE) },
                { typeof(AlignedContext),   KeywordText(ALIGNED) },
                { typeof(LabelContext),     KeywordText(LABEL) },
                { typeof(BitContext),       KeywordText(BIT) },
                { typeof(PointerContext),   KeywordText(POINTER) },
                { typeof(IntegerContext),   $"{KeywordText(BIN)}/{KeywordText(UBIN)}" },
                { typeof(EntryContext),     KeywordText(ENTRY) },
                { typeof(StringContext),    KeywordText(STRING) },
                { typeof(AsContext),        KeywordText(AS) } ,
                { typeof(ConstContext),     KeywordText(CONST) },
                { typeof(OffsetContext),    KeywordText(OFFSET) },
                { typeof(ExternalContext),  KeywordText(EXTERNAL) },
                { typeof(InternalContext),  KeywordText(INTERNAL) },
                { typeof(StaticContext),    KeywordText(STATIC) },
                { typeof(BasedContext),     KeywordText(BASED) },
                { typeof(StackContext),     KeywordText(STACK) },
                { typeof(InitContext),      KeywordText(INIT) },
                { typeof(BuiltinContext),   KeywordText(BUILTIN) },
                { typeof(PadContext),       KeywordText(PAD) }
            };

        // If we always probe for keys where the two types have names where first
        // is alphabetically before second, then we do not need to code for all 
        // combinations

        private static readonly HashSet<(System.Type, System.Type)> incompatibleDataAttributes = new()
            {
            (typeof(Aligned)        ,  typeof(EntryConstant)),  // aligned and entry are defined here
            (typeof(Aligned)        ,  typeof(LabelContext)) ,
            (typeof(Aligned)        ,  typeof(PackedContext)) ,

            (typeof(AsContext)      ,  typeof(BitContext)) ,
            (typeof(AsContext)      ,  typeof(EntryContext)) ,
            (typeof(AsContext)      ,  typeof(IntegerContext)) ,
            (typeof(AsContext)      ,  typeof(LabelContext)) ,
            (typeof(AsContext)      ,  typeof(PointerContext)) ,
            (typeof(AsContext)      ,  typeof(StringContext)) ,
            (typeof(AsContext)      ,  typeof(VariableContext)) ,

            (typeof(BitContext)     ,  typeof(EntryContext)) ,
            (typeof(BitContext)     ,  typeof(IntegerContext)) ,
            (typeof(BitContext)     ,  typeof(LabelContext)) ,
            (typeof(BitContext)     ,  typeof(PointerContext)) ,
            (typeof(BitContext)     ,  typeof(StringContext)) ,
            (typeof(BitContext)     ,  typeof(VariableContext)) ,

            (typeof(EntryContext)   ,  typeof(IntegerContext)) ,
            (typeof(EntryContext)   ,  typeof(LabelContext)) ,
            (typeof(EntryContext)   ,  typeof(PointerContext)) ,
            (typeof(EntryContext)   ,  typeof(StringContext)) ,  // we don't need entry and aligned here

            (typeof(IntegerContext) ,  typeof(LabelContext)) ,
            (typeof(IntegerContext) ,  typeof(PointerContext)) ,
            (typeof(IntegerContext) ,  typeof(StringContext)) ,

            (typeof(LabelContext)   ,  typeof(PointerContext)) ,
            (typeof(LabelContext)   ,  typeof(StringContext)) ,

            (typeof(PointerContext) ,  typeof(StringContext)) ,
            };
        public static bool IncompatibleDataAttributes((System.Type a, System.Type b) pair)
        {
            // Ensure we order the two types before checking the hash set

            if (String.Compare(pair.a.Name, pair.b.Name, StringComparison.Ordinal) > 0)
                pair = (pair.b, pair.a);
            
            if (incompatibleDataAttributes.Contains(pair))
                return false;

            return true;
        }
        public static string GeyKeywordFromAttribute(System.Type type)
        {
            return typeToKeyword[type];
        }
    }
}