namespace Syscode
{
    public enum DataType
    {
        UNDEFINED = 0,
        BIN = SyscodeParser.BIN,
        UBIN = SyscodeParser.UBIN,
        DEC = SyscodeParser.DEC,
        UDEC = SyscodeParser.UDEC,
        //BIN8 = SyscodeParser.BIN8,
        //BIN16 = SyscodeParser.BIN16,
        //BIN32 = SyscodeParser.BIN32,
        //BIN64 = SyscodeParser.BIN64,
        //UBIN8 = SyscodeParser.UBIN8,
        //UBIN16 = SyscodeParser.UBIN16,
        //UBIN32 = SyscodeParser.UBIN32,
        //UBIN64 = SyscodeParser.UBIN64,
        BIT = SyscodeParser.BIT,
        STRING = SyscodeParser.STRING,
        LABEL = SyscodeParser.LABEL,
        ENTRY = SyscodeParser.ENTRY,
        POINTER = SyscodeParser.POINTER,
        AS = SyscodeParser.AS,
        STRUCT = SyscodeParser.STRUCT,
        BUILTIN = SyscodeParser.BUILTIN,
        SINGLE = SyscodeParser.SINGLE,  // implicitly binary float
        DOUBLE = SyscodeParser.DOUBLE,  // implicitoy binary float

    }

    public enum Base
    {
        UNDEFINED = 0,  
        BIN,
        OCT,
        DEC,
        HEX
    }

    public enum Scale
    {
        UNDEFINED = 0,
        FIXED,
        FLOAT
    }

    public static class TypeNames
    {
        public static string[] AllBinaryTypes;
        public static string[] BaseBinaryTypes;
        public static string[] FixedBinaryTypes;
        public static string[] BaseUBinaryTypes;
        public static string[] FixedUBinaryTypes;
        public static string[] BinaryTypes;
        public static string[] UBinaryTypes;

        static TypeNames()
        {
            BaseBinaryTypes = new[]{ "bin8", "bin16", "bin32", "bin64" };
            FixedBinaryTypes = new[]{ "bin" };
            BaseUBinaryTypes = new[]{ "ubin8", "ubin16", "ubin32", "ubin64" };
            FixedUBinaryTypes = new[] { "ubin" };
            
            BinaryTypes = BaseBinaryTypes.Concat(FixedBinaryTypes).ToArray();
            UBinaryTypes = BaseUBinaryTypes.Concat(FixedUBinaryTypes).ToArray();
            AllBinaryTypes = BinaryTypes.Concat(UBinaryTypes).ToArray();
        }


    }
}
