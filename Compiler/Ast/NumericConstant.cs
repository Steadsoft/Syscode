using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static SyscodeParser;

namespace Syscode
{
    /// <summary>
    /// Represents a numeric constant
    /// </summary>
    public class NumericConstant : AstNode, IConstant
    {
        private DataType dataType;
        private UInt64 valueUnsigned = 0;
        private Int64 valueSigned = 0;
        private string literalText = string.Empty;
        private bool signed = false;
        private bool negative = false;
        private int scale = 0;
        public static NumericConstant Create(NumericLiteralContext context)
        {
            var constant = new NumericConstant(context);

            if (context.Signed != null)
            {
                constant.signed = true;

                if (context.Signed.Text == "-")
                    constant.negative = true;

            }

            if (context.Bin != null)
            {
                constant.literalText = constant.CleanupString(context.Bin.GetText());
                constant.SetValueFromBase(constant.literalText, 2);
            }

            if (context.Oct != null)
            {
                constant.literalText = constant.CleanupString(context.Oct.GetText());
                constant.SetValueFromBase(constant.literalText, 8);
            }

            if (context.Dec != null)
            {
                constant.literalText = constant.CleanupString(context.Dec.GetText());
                constant.SetValueFromBase(constant.literalText, 10);
            }

            if (context.Hex != null)
            {
                constant.literalText = constant.CleanupString(context.Hex.GetText());
                constant.SetValueFromBase(constant.literalText, 16);
            }

            // The data type we infer here is the most appropriate standard size for the value

            if (constant.signed)
            {
                if (constant.valueSigned >= SByte.MinValue && constant.valueSigned <= SByte.MaxValue)
                {
                    constant.dataType = DataType.BIN;
                }
                else if (constant.valueSigned >= Int16.MinValue && constant.valueSigned <= Int16.MaxValue)
                {
                    constant.dataType = DataType.BIN;
                }
                else if (constant.valueSigned >= Int32.MinValue && constant.valueSigned <= Int32.MaxValue)
                {
                    constant.dataType = DataType.BIN;
                }
                else if (constant.valueSigned >= Int64.MinValue && constant.valueSigned <= Int64.MaxValue)
                {
                    constant.dataType = DataType.BIN;
                }
            }
            else
            {
                if (constant.valueUnsigned >= Byte.MinValue && constant.valueUnsigned <= Byte.MaxValue)
                {
                    constant.dataType = DataType.UBIN;
                }
                else if (constant.valueUnsigned >= UInt16.MinValue && constant.valueUnsigned <= UInt16.MaxValue)
                {
                    constant.dataType = DataType.UBIN;
                }
                else if (constant.valueUnsigned >= UInt32.MinValue && constant.valueUnsigned <= UInt32.MaxValue)
                {
                    constant.dataType = DataType.UBIN;
                }
                else if (constant.valueUnsigned >= UInt64.MinValue && constant.valueUnsigned <= UInt64.MaxValue)
                {
                    constant.dataType = DataType.UBIN;
                }
            }

            return constant;
        }
        private NumericConstant(NumericLiteralContext context) : base(context)
        {
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
        public string LiteralText { get => literalText; set => literalText = value; }

        public string Spelling => literalText;

        public DataType DataType => dataType;

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

    public class EntryConstant : AstNode, IConstant
    {
        private string name;

        public string Spelling => name;

        public DataType DataType => DataType.ENTRY;

        public EntryConstant(ProcedureContext context):base(context)
        {
            name = context.Spelling.GetText(); 
            
        }
    }

    public interface IConstant
    {
        string Spelling { get; }
        DataType DataType { get; }
    }

}