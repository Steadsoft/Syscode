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
}
