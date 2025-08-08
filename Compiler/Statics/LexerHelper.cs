namespace Syscode
{
    public static class LexerHelper
    {
        public static class TokenText
        {
            public static readonly string BIT = SyscodeLexer.DefaultVocabulary.GetLiteralName(SyscodeParser.BIT).Replace("'", "");
            public static readonly string BIN = SyscodeLexer.DefaultVocabulary.GetLiteralName(SyscodeParser.BIN).Replace("'", "");
            public static readonly string DEC = SyscodeLexer.DefaultVocabulary.GetLiteralName(SyscodeParser.DEC).Replace("'", "");
            public static readonly string LPAR = SyscodeLexer.DefaultVocabulary.GetLiteralName(SyscodeParser.LPAR).Replace("'", "");
            public static readonly string RPAR = SyscodeLexer.DefaultVocabulary.GetLiteralName(SyscodeParser.RPAR).Replace("'", "");
            public static readonly string STRING = SyscodeLexer.DefaultVocabulary.GetLiteralName(SyscodeParser.STRING).Replace("'", "");
            public static readonly string TIMES = SyscodeLexer.DefaultVocabulary.GetLiteralName(SyscodeParser.TIMES).Replace("'", "");
            public static readonly string PLUS = SyscodeLexer.DefaultVocabulary.GetLiteralName(SyscodeParser.PLUS).Replace("'", "");

            static TokenText()
            {

            }
        }

        public static string KeywordText(int tokenType)
        {
            return SyscodeLexer.DefaultVocabulary.GetSymbolicName(tokenType).Replace("'", "").ToLower();
        }

        public static string GetOperatorText(Operator Op)
        {
            return SyscodeLexer.DefaultVocabulary.GetLiteralName((int)Op).Replace("'", "");
        }
    }
}