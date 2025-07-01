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

        public Symbol CreateSymbol(Declare declaration)
        {
            var symbol = new Symbol(declaration);

            if (declaration.TypeName == "label")
                symbol.CoreType = CoreType.Label;

            if (declaration.TypeName.StartsWith("bin"))
            {
                ValidateBinary(declaration, out var p, out var s, out var signed);
            }

            if (declaration.TypeName.StartsWith("ubin"))
            {
                symbol.CoreType = CoreType.Binary;
                symbol.Signed = true;

                if (declaration.TypeName == "ubin8")
                    symbol.Precision = 8;

                if (declaration.TypeName == "ubin16")
                    symbol.Precision = 16;

                if (declaration.TypeName == "ubin32")
                    symbol.Precision = 32;
                if (declaration.TypeName == "ubin64")
                    symbol.Precision = 64;
            }

            if (declaration.TypeName == "entry")
                symbol.CoreType = CoreType.Entry;

            if (declaration.TypeName == "string")
                symbol.CoreType = CoreType.String;

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

        private bool ValidateBinary(Declare declaration, out Int32? Precision, out Int32? Scale, out bool Signed)
        {
            Precision = null;
            Scale = null;
            Signed = false;

            if (!TypeNames.AllBinaryTypes.Contains(declaration.TypeName))
            {
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
                        diagnostics?.Invoke(this, new DiagnosticEvent(declaration, 1, Severity.Error, $"Precision for '{declaration.Spelling}' must be an integer literal"));
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
                        diagnostics?.Invoke(this, new DiagnosticEvent(declaration, 1, Severity.Error, $"Precision for '{declaration.Spelling}' must be an integer literal"));
                        return false;
                    }

                    if (ScaleInvalid)
                    {
                        diagnostics?.Invoke(this, new DiagnosticEvent(declaration, 1, Severity.Error, $"Scale for '{declaration.Spelling}' must be an integer literal"));
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
    }
}
