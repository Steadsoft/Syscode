using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SyscodeParser;

namespace Syscode
{
    /// <summary>
    /// Represents a numeric constant
    /// </summary>
    public class NumericConstant : AstNode
    {
        private DataType dataType;
        private UInt64 valueUnsigned = 0;
        private Int64 valueSigned = 0;
        private string literalText = string.Empty;
        private bool signed = false;
        private bool negative = false;
        private int scale = 0;
        public NumericConstant(NumericLiteralContext context) : base(context)
        {
            if (context.Signed != null)
            {
                signed = true;

                if (context.Signed.Text == "-")
                    negative = true;

            }

            if (context.Bin != null)
            {
                literalText = CleanupString(context.Bin.GetText());
                SetValueFromBase(literalText, 2);
            }

            if (context.Oct != null)
            {
                literalText = CleanupString(context.Oct.GetText());
                SetValueFromBase(literalText, 8);
            }

            if (context.Dec != null)
            {
                literalText = CleanupString(context.Dec.GetText());
                SetValueFromBase(literalText, 10);
            }

            if (context.Hex != null)
            {
                literalText = CleanupString(context.Hex.GetText());
                SetValueFromBase(literalText, 16);
            }

            // The data type we infer here is the most appropriate standard size for the value

            if (signed)
            {
                if (valueSigned >= SByte.MinValue && valueSigned <= SByte.MaxValue)
                {
                    dataType = DataType.BIN8;
                }
                else if (valueSigned >= Int16.MinValue && valueSigned <= Int16.MaxValue)
                {
                    dataType = DataType.BIN16;
                }
                else if (valueSigned >= Int32.MinValue && valueSigned <= Int32.MaxValue)
                {
                    dataType = DataType.BIN32;
                }
                else if (valueSigned >= Int64.MinValue && valueSigned <= Int64.MaxValue)
                {
                    dataType = DataType.BIN64;
                }
            }
            else
            {
                if (valueUnsigned >= Byte.MinValue && valueUnsigned <= Byte.MaxValue)
                {
                    dataType = DataType.UBIN8;
                }
                else if (valueUnsigned >= UInt16.MinValue && valueUnsigned <= UInt16.MaxValue)
                {
                    dataType = DataType.UBIN16;
                }
                else if (valueUnsigned >= UInt32.MinValue && valueUnsigned <= UInt32.MaxValue)
                {
                    dataType = DataType.UBIN32;
                }
                else if (valueUnsigned >= UInt64.MinValue && valueUnsigned <= UInt64.MaxValue)
                {
                    dataType = DataType.UBIN64;
                }
            }
        }

        private void SetValueFromBase(string Text, int Base)
        {
            if (signed)
            {
                if (negative)
                    valueSigned = -Convert.ToInt64(Text, Base);
                else
                    valueSigned = Convert.ToInt64(Text, Base);
            }
            else
            {
                valueUnsigned = Convert.ToUInt64(Text, Base);
            }

        }

        public bool Signed { get => signed;  }
        public bool Unsigned { get => !signed; }
        public ulong ValueUnsigned { get => valueUnsigned; set => valueUnsigned = value; }
        public long ValueSigned { get => valueSigned; set => valueSigned = value; }

        /// <summary>
        /// Remioves any trailing chars like ":H" or ":d" etc.
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        private string CleanupString(string Value)
        {
            if (Value.Contains(':'))
                return Value.Substring(0, Value.Length - 2).Replace("_","").Replace(" ","");
            
            return Value.Replace("_", "").Replace(" ", ""); ;
        }
    }
}