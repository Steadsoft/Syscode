using Syscode.Ast;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Formats.Asn1.AsnWriter;

namespace Syscode
{
    public class SymtabBuilder
    {
        private event EventHandler<DiagnosticEvent>? diagnostics;
        public SymtabBuilder(EventHandler<DiagnosticEvent>? diagnostics)
        {
            this.diagnostics = diagnostics;
        }

        public void Generate(Compilation root)
        {
            var declarations = root.Statements.Where(s => s is Declare).Cast<Declare>();

            root.Symbols = declarations.Select(d => CreateSymbol(d)).ToList();
        }

        //public Symbol CreateSymbol (Procedure proc)
        //{
        //    var symbol = new Symbol(proc);

        //    return symbol;
        //}

        public Symbol CreateSymbol(Declare declaration)
        {
            var symbol = new Symbol(declaration);

            if (declaration is Procedure proc)
            {
                var sym = new Symbol(declaration);
                proc.Symbols = proc.Statements.Where(s => s is Declare).Cast<Declare>().Select(d => CreateSymbol(d)).ToList();
                sym.Invalid = false;
                return sym;
            }

            symbol.CoreType = declaration.CoreType;

            if (IsBinary(symbol.CoreType) && ValidateBinary(declaration, out var precision, out var scale, out var signed))
            {
                symbol.Precision = precision;
                symbol.Scale = scale;
                symbol.Signed = signed;
                symbol.Invalid = false;
                return symbol;
            }

            if (symbol.CoreType == CoreType.STRING)
            {
                if (ValidateString(declaration, out var length, out var varying))
                {
                    symbol.Varying = varying;
                    symbol.Length = length;
                    symbol.Invalid = false;
                    symbol.Varying = declaration.Varying;
                    return symbol;
                }
            }

            return symbol;
        }

        private bool IsScaleInvalid(Declare declaration)
        {
            return !((declaration.typeSubscripts[1].Type == ExpressionType.Literal) && (declaration.typeSubscripts[1].Literal.LiteralType == LiteralType.Numeric));
        }

        private bool IsPrecisionInvalid(Declare declaration)
        {
            return !((declaration.typeSubscripts[0].Type == ExpressionType.Literal) && (declaration.typeSubscripts[0].Literal.LiteralType == LiteralType.Numeric));
        }

        private bool IsStringLengthInvalid(Declare declaration)
        {
            return !((declaration.typeSubscripts[0].Type == ExpressionType.Literal) && (declaration.typeSubscripts[0].Literal.LiteralType == LiteralType.Numeric));
        }


        private bool ValidateString (Declare declaration, out int Length, out bool Varying)
        {
            Length = 0;
            Varying = false;

            if (declaration.typeSubscripts.Count == 0)
            {
                diagnostics?.Invoke(this, new DiagnosticEvent(declaration, 1, Severity.Error, $"The string variable '{declaration.Spelling}' must have a length specifier."));
                return false;
            }

            if (declaration.typeSubscripts.Count > 1)
            {
                diagnostics?.Invoke(this, new DiagnosticEvent(declaration, 1, Severity.Error, $"The string variable '{declaration.Spelling}' must have a single valued length specifier."));
                return false;
            }

            if (IsStringLengthInvalid(declaration))
            {
                diagnostics?.Invoke(this, new DiagnosticEvent(declaration, 1, Severity.Error, $"The length for string '{declaration.Spelling}' must be a positive non-zero integer literal"));
                return false;
            }

            Length = Convert.ToInt32(declaration.typeSubscripts[0].Literal.Value);

            return true;

        }

        private bool ValidateBinary(Declare declaration, out Int32? Precision, out Int32? Scale, out bool Signed)
        {
            Precision = null;
            Scale = null;
            Signed = false;

            if (!TypeNames.AllBinaryTypes.Contains(declaration.TypeName))
            {
                return false;
            }

            if (declaration.Varying)
            {
                diagnostics?.Invoke(this, new DiagnosticEvent(declaration, 1, Severity.Error, $"The option 'var' can only be applied to string declarations."));
                return false;

            }

            if (TypeNames.BaseBinaryTypes.Contains(declaration.TypeName))
            {
                if (declaration.typeSubscripts.Any())
                {
                    diagnostics?.Invoke(this, new DiagnosticEvent(declaration, 1, Severity.Error, $"The type of '{declaration.Spelling}' does not support range attributes."));
                    return false;
                }

                Precision = Convert.ToInt32(declaration.TypeName.Substring(3));

                if (Precision <= 0)
                {
                    diagnostics?.Invoke(this, new DiagnosticEvent(declaration, 1, Severity.Error, $"The precision for the variable '{declaration.Spelling}' must be a positive non-zero integer literal"));
                    return false;
                }

                Scale = 0;
                Signed = true;
                return true;
            }

            if (TypeNames.BaseUBinaryTypes.Contains(declaration.TypeName))
            {
                if (declaration.typeSubscripts.Any())
                {
                    diagnostics?.Invoke(this, new DiagnosticEvent(declaration, 1, Severity.Error, $"The type of '{declaration.Spelling}' does not support range attributes."));
                    return false;
                }

                Precision = Convert.ToInt32(declaration.TypeName.Substring(3));
                Scale = 0;
                Signed = false;
                return true;
            }

            if (declaration.TypeName == "bin")
                Signed = true;
            else
                Signed = false;


            if (declaration.typeSubscripts.Any())
            {
                if (declaration.typeSubscripts.Count == 1)
                {
                    bool PrecisionInvalid = IsPrecisionInvalid(declaration);

                    if (PrecisionInvalid)
                    {
                        diagnostics?.Invoke(this, new DiagnosticEvent(declaration, 1, Severity.Error, $"The precision for the variable '{declaration.Spelling}' must be a positive non-zero integer literal"));
                        return true;
                    }

                    if (!PrecisionInvalid)
                        Precision = Convert.ToInt32(declaration.typeSubscripts[0].Literal.Value);

                    return true;
                }

                if (declaration.typeSubscripts.Count == 2)
                {
                    bool PrecisionInvalid = IsPrecisionInvalid(declaration);
                    bool ScaleInvalid = IsScaleInvalid(declaration);

                    if (PrecisionInvalid)
                    {
                        diagnostics?.Invoke(this, new DiagnosticEvent(declaration, 1, Severity.Error, $"The precision for the variable '{declaration.Spelling}' must be a positive non-zero integer literal"));
                        return false;
                    }

                    if (ScaleInvalid)
                    {
                        diagnostics?.Invoke(this, new DiagnosticEvent(declaration, 1, Severity.Error, $"The scale for the variable '{declaration.Spelling}' must be an integer literal"));
                        return false;
                    }

                    if (!PrecisionInvalid && !ScaleInvalid)
                    {
                        Precision = Convert.ToInt32(declaration.typeSubscripts[0].Literal.Value);
                        Scale = Convert.ToInt32(declaration.typeSubscripts[1].Literal.Value);
                    }
                }
            }

            return true;
        }

        private bool IsBinary(CoreType type)
        {
            switch (type)
            {
                case CoreType.BIN:
                case CoreType.BIN8:
                case CoreType.BIN16:
                case CoreType.BIN32:
                case CoreType.BIN64:
                case CoreType.UBIN:
                case CoreType.UBIN8:
                case CoreType.UBIN16:
                case CoreType.UBIN32:
                case CoreType.UBIN64:
                    return true;
                    default:
                    return false;

            }
        }
    }
}
