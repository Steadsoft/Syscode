using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syscode
{
    public enum CoreType
    {
        Binary,
        Decimal,
        String,
        Entry,
        Label
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
