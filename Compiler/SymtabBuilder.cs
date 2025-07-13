using Syscode.Ast;
using System.Xml.Linq;

namespace Syscode
{
    public class SymtabBuilder
    {
        private Reporter reporter;

        public SymtabBuilder(Reporter reporter)
        {
            this.reporter = reporter;
        }

        public void Generate(Compilation root)
        {
            var declarations = root.Statements.OfType<Declare>(); //.Where(s => s is Declare).Cast<Declare>();
            var procedures = root.Statements.OfType<Procedure>();//.Where(s => s is Procedure).Cast<Procedure>();
            var scopes = root.Statements.OfType<Scope>();//.Where(s => s is Scope).Cast<Scope>();

            root.Symbols.AddRange(declarations.Select(d => CreateSymbol(d))); 
            root.Symbols.AddRange(procedures.Select(d => CreateSymbol(d)));

        }

        public Symbol CreateSymbol(Procedure procedure)
        {
            var sym = new Symbol(procedure);
            procedure.Symbols = procedure.Statements.Where(s => s is Declare).Cast<Declare>().Select(d => CreateSymbol(d)).ToList();
            sym.Invalid = false;
            return sym;

        }
        public Symbol CreateSymbol(Declare declaration)
        {
            ReportDeclaredKeywords(declaration);

            var symbol = new Symbol(declaration);

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

        private void ReportDeclaredKeywords(Declare declaration)
        {
            if (declaration.IsntStructure)
            {
                if (declaration.IsKeyword)
                    reporter.Report(declaration, 1015, declaration.Spelling);
            }
            else
            {
                ReportDeclaredKeywords(declaration.StructBody);
            }
        }

        private void ReportDeclaredKeywords(StructBody Struct)
        {
            if (Struct.IsKeyword)
            {
                reporter.Report(Struct, 1015,Struct.Spelling);
            }

            foreach (var body in Struct.Structs)
            {
                ReportDeclaredKeywords(body);
            }

            // TODO: need to report any fields that are also keywords...
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
                reporter.Report(declaration, 1002, declaration.Spelling);
                return false;
            }

            if (declaration.typeSubscripts.Count > 1)
            {

                reporter.Report(declaration, 1003, declaration.Spelling);
                return false;
            }

            if (IsStringLengthInvalid(declaration))
            {
                reporter.Report(declaration, 1004, declaration.Spelling);
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
                reporter.Report(declaration, 1005, "var");
                return false;

            }

            if (TypeNames.BaseBinaryTypes.Contains(declaration.TypeName))
            {
                if (declaration.typeSubscripts.Any())
                {
                    reporter.Report(declaration, 1006, declaration.Spelling);
                    return false;
                }

                Precision = Convert.ToInt32(declaration.TypeName.Substring(3));

                if (Precision <= 0)
                {
                    reporter.Report(declaration, 1007, declaration.Spelling);
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
                    reporter.Report(declaration, 1006, declaration.Spelling);
                    return false;
                }

                Precision = Convert.ToInt32(declaration.TypeName.Substring(4));
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
                        reporter.Report(declaration, 1007, declaration.Spelling);
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
                        reporter.Report(declaration, 1007, declaration.Spelling);
                        return false;
                    }

                    if (ScaleInvalid)
                    {
                        reporter.Report(declaration, 1008, declaration.Spelling);
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
