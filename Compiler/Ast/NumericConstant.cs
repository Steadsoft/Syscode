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
    public class NumericConstant : IConstant
    {
        private DataType dataType;
        private UInt64 valueUnsigned = 0;
        private Int64 valueSigned = 0;
        private string literalText = string.Empty;
        private bool signed = false;
        private bool negative = false;
        private Base numberbase;
        private Scale scale;
        private int precision = 0;
        private int scalefactor = 0;

        private Operator prefix;
        public NumericConstant(string rawtext, Operator prefix) 
        {
            literalText = rawtext.Trim().Replace(" ","").ToUpper(); 

            SetBase(literalText);
            SetScale(literalText);
            SetPrecision(literalText);

            if (prefix != Operator.UNDEFINED)
                signed = true;
            else
                signed = false;

            if (prefix == Operator.MINUS)
                negative = true;

            // The data type we infer here is the most appropriate standard size for the value

            if (signed)
            {
                if (numberbase == Base.DEC)
                    dataType = DataType.DEC;
                if (numberbase == Base.BIN || numberbase == Base.OCT || numberbase == Base.HEX)
                    dataType = DataType.BIN;
            }
            else
            {
                if (numberbase == Base.DEC)
                    dataType = DataType.UDEC;
                if (numberbase == Base.BIN || numberbase == Base.OCT || numberbase == Base.HEX)
                    dataType = DataType.UBIN;
            }
        }

        private void SetBase(string Text)
        {
            if (Text.EndsWith("[B]"))
            {
                numberbase = Base.BIN;
                return;
            }

            if (Text.EndsWith("[O]"))
            {
                numberbase = Base.OCT;
                return;
            }

            if (Text.EndsWith("[D]"))
            {
                numberbase = Base.DEC;
                return;
            }

            if (Text.EndsWith("[H]") || Text.EndsWith("[HD]") | Text.EndsWith("[HS]") | Text.EndsWith("[DH]") | Text.EndsWith("[SH]"))
            {
                numberbase = Base.HEX;
                return;
            }

            if (Text.Contains('P'))
            {
                numberbase = Base.HEX;
                return;
            }

            numberbase = Base.DEC;
        }
        private void SetPrecision(string Text)
        {
            if (scale == Scale.FLOAT)
            {
                precision = 0;
                scalefactor = 0;
                return;
            }

            switch (numberbase)
            {
                case Base.BIN:
                    {
                        Text = Text.Replace("[", "").Replace("]", "").Replace("B", "");

                        if (Text.Contains('.') == false)
                        {
                            precision = Text.Length;
                            scalefactor = 0;
                            return;
                        }

                        var point = Text.IndexOf('.');
                        var digits = Text.Length - 1;
                        precision = digits;
                        scalefactor = digits - point;
                        return;
                    }

                case Base.DEC:
                    {
                        Text = Text.Replace("[", "").Replace("]", "").Replace("D", "");

                        if (Text.Contains('.') == false)
                        {
                            precision = Text.Length;
                            scalefactor = 0;
                            return;
                        }

                        var point = Text.IndexOf('.');
                        var digits = Text.Length - 1;
                        precision = digits;
                        scalefactor = digits - point;
                        return;
                    }
                case Base.OCT:
                    {
                        Text = Text.Replace("[", "").Replace("]", "").Replace("O", "");

                        // Hex is 1 digit = 4 bits...

                        if (Text.Contains('.') == false)
                        {
                            precision = Text.Length * 3;
                            scalefactor = 0;
                            return;
                        }

                        var point = Text.IndexOf('.');
                        var digits = Text.Length - 1;
                        precision = 3 * digits;
                        scalefactor = 3 * (digits - point);
                        return;

                    }

                case Base.HEX:
                    {
                        Text = Text.Replace("[", "").Replace("]", "").Replace("H", "");

                        // Hex is 1 digit = 4 bits...

                        if (Text.Contains('.') == false)
                        {
                            precision = Text.Length * 4;
                            scalefactor = 0;
                            return;
                        }

                        var point = Text.IndexOf('.');
                        var digits = Text.Length - 1;
                        precision = 4 * digits;
                        scalefactor = 4 * (digits - point);
                        return;

                    }

            }

            return;
        }
        private void  SetScale(string Text)
        {
            if (numberbase == Base.DEC)
            {
                if (Text.Contains('S') || Text.Contains('D') || Text.Contains('E'))
                {
                    scale = Scale.FLOAT;
                    return;
                }

                scale = Scale.FIXED;
                return;
            }

            if (numberbase == Base.HEX)
            {
                if (Text.EndsWith("[HD]") | Text.EndsWith("[HS]") | Text.EndsWith("[DH]") | Text.EndsWith("[SH]") | Text.Contains('P'))
                {
                    scale = Scale.FLOAT;
                    return;
                }

                scale = Scale.FIXED;
                return;
            }

            scale = Scale.FIXED;

        }
        private void SetValueFromBase(string Text, int Base)
        {
            return;

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
        public Base Base => numberbase;
        public Scale Scale => scale;
        public int Scalefactor => scalefactor;
        public int Precision => precision;
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