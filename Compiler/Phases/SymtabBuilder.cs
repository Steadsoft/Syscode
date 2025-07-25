﻿using Syscode.Ast;

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
            var declarations = root.Statements.OfType<Declare>();
            var procedures = root.Statements.OfType<Procedure>();
            var scopes = root.Statements.OfType<Scope>();

            root.Symbols.AddRange(declarations.Select(CreateSymbol));
            root.Symbols.AddRange(procedures.Select(CreateSymbol));

        }
        public Symbol CreateSymbol(Procedure procedure)
        {
            var sym = new Symbol(procedure);
            procedure.Symbols = procedure.Statements.OfType<Declare>().Select(CreateSymbol).ToList();
            sym.Invalid = false;
            return sym;
        }
        public Symbol CreateSymbol(Declare declaration)
        {
            ReportDeclaredKeywords(declaration);

            ValidateAttributes(declaration);

            if (declaration.IsStructure)
                ValidateStructure(declaration);

            var symbol = new Symbol(declaration);

            if (IsBinaryType(symbol.CoreType))
            {
                if (ValidBinaryDeclaration(declaration, out var precision, out var scale, out var signed, out var bytes))
                {
                    symbol.Precision = precision;
                    symbol.Scale = scale;
                    symbol.Signed = signed;
                    symbol.Invalid = false;
                    symbol.Bytes = bytes;

                    if (declaration.Alignment.AlignmentUnits == AlignmentUnits.Unspecified)
                        declaration.Alignment = GetDefaultAlignment(symbol);

                    ApplyDefaults(symbol);
                    return symbol;
                }
            }

            if (symbol.CoreType == DataType.STRING)
            {
                if (ValidStringDeclaration(declaration, out var length, out var varying))
                {
                    symbol.Varying = varying;
                    symbol.Bytes = length;
                    symbol.Invalid = false;
                    symbol.Varying = declaration.Varying;

                    if (declaration.Alignment.AlignmentUnits == AlignmentUnits.Unspecified)
                        declaration.Alignment = GetDefaultAlignment(symbol);

                    ApplyDefaults(symbol);
                    return symbol;
                }
            }

            ApplyDefaults(symbol);

            return symbol;
        }

        private void ApplyDefaults(Symbol symbol)
        {
            if (symbol.Container == null)  // declared in global namespace
            {
                if (symbol.StorageClass == StorageClass.Unspecified)
                {
                    symbol.StorageClass = StorageClass.Static;
                }
                else
                {
                    if (symbol.StorageClass != StorageClass.Static)
                    {
                        reporter.Report(symbol.Node, 1022);
                    }
                }

                if (symbol.StorageScope == StorageScope.Unspecified)
                {
                    symbol.StorageScope = StorageScope.Internal;
                }
            }
            else
            {
                if (symbol.StorageClass == StorageClass.Unspecified)
                    symbol.StorageClass = StorageClass.Stack;

                if (symbol.StorageScope == StorageScope.Unspecified)
                    symbol.StorageScope = StorageScope.Internal;
            }
        }
        private void ValidateStructure(Declare declaration)
        {
            if (declaration.IsntStructure)
                throw new ArgumentException("The argument must represent a structure.", nameof(declaration));

            ValidateStructure(declaration.StructBody);
        }
        private void ValidateStructure(StructBody Struct)
        {
            var memberNames = Struct.Fields.Select(s => s.Spelling).Concat(Struct.Structs.Select(s => s.Spelling));

            var nameGroups = memberNames.GroupBy(memberNames => memberNames);

            foreach (var nameGroup in nameGroups)
            {
                if (nameGroup.Count() > 1)
                    reporter.Report(Struct, 1019, Struct.Spelling, nameGroup.Key);

            }

            foreach (var structure in Struct.Structs)
            {
                ValidateStructure(structure);
            }
        }
        private void ReportDeclaredKeywords(Declare declaration)
        {
            if (declaration.IsntStructure)
            {
                if (declaration.IsKeyword && declaration.CoreType != DataType.BUILTIN)
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
                reporter.Report(Struct, 1016, Struct.Spelling);
            }

            foreach (var body in Struct.Structs)
            {
                ReportDeclaredKeywords(body);
            }

            // TODO: need to report any fields that are also keywords...
        }
        private bool IsScaleInvalid(Declare declaration)
        {
            return !((declaration.typeSubscripts[1].Type == ExpressionType.Literal) && (declaration.typeSubscripts[1].Literal.LiteralType == LiteralType.Decimal));
        }
        private bool IsPrecisionInvalid(Declare declaration)
        {
            return !((declaration.typeSubscripts[0].Type == ExpressionType.Literal) && (declaration.typeSubscripts[0].Literal.LiteralType == LiteralType.Decimal));
        }
        private bool IsStringLengthInvalid(Declare declaration)
        {
            return !((declaration.typeSubscripts[0].Type == ExpressionType.Literal) && (declaration.typeSubscripts[0].Literal.LiteralType == LiteralType.Decimal));
        }
        private bool ValidStringDeclaration(Declare declaration, out int Length, out bool Varying)
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
        private void ValidateAttributes(Declare declaration)
        {
            var groups = declaration.Attributes.GroupBy(g => g.GetType());
            bool dupes = false;

            foreach (var group in groups)
            {
                if (group.Count() > 1)
                {
                    reporter.Report(declaration, 1018, declaration.Spelling, group.Key.Name.ToLower());
                    dupes = true;
                }
            }

            bool scopeSet = false;
            bool classSet = false;

            foreach (var attribute in declaration.Attributes)
            {
                switch (attribute)
                {
                    case Aligned aligned:
                        if (ValidateAligned(declaration, aligned, out var alignment))
                        {
                            declaration.Alignment = alignment;
                        }
                        break;
                    case External:
                        {
                            if (declaration.StorageScope != StorageScope.Unspecified)
                                reporter.Report(declaration, 1020, declaration.Spelling, "external", declaration.StorageScope.ToString().ToLower());
                            declaration.StorageScope = StorageScope.External;
                            break;
                        }
                    case Internal:
                        {
                            if (declaration.StorageScope != StorageScope.Unspecified)
                                reporter.Report(declaration, 1020, declaration.Spelling, "internal", declaration.StorageScope.ToString().ToLower());
                            declaration.StorageScope = StorageScope.Internal;
                            break;
                        }
                    case Stack:
                        {
                            if (declaration.StorageClass != StorageClass.Unspecified)
                                reporter.Report(declaration, 1021, declaration.Spelling, "stack", declaration.StorageClass.ToString().ToLower());
                            declaration.StorageClass = StorageClass.Stack;
                            break;
                        }
                    case Based:
                        {
                            if (declaration.StorageClass != StorageClass.Unspecified)
                                reporter.Report(declaration, 1021, declaration.Spelling, "based", declaration.StorageClass.ToString().ToLower());
                            declaration.StorageClass = StorageClass.Based;
                            break;
                        }
                    case Static:
                        {
                            if (declaration.StorageClass != StorageClass.Unspecified)
                                reporter.Report(declaration, 1021, declaration.Spelling, "static", declaration.StorageClass.ToString().ToLower());
                            declaration.StorageClass = StorageClass.Static;
                            break;
                        }
                    case Defined:
                        {
                            if (declaration.StorageClass != StorageClass.Unspecified)
                                reporter.Report(declaration, 1021, declaration.Spelling, "defined", declaration.StorageClass.ToString().ToLower());
                            declaration.StorageClass = StorageClass.Defined;
                            break;
                        }

                    default:
                        break;

                }
            }


        }
        private bool ValidateAligned(Declare declaration, Aligned Aligned, out Alignment alignment)
        {
            alignment = null;

            if (Aligned.Alignment.IsConstant && Aligned.Alignment.Literal != null)
            {
                alignment = new Alignment();
                alignment.AlignmentValue = Convert.ToInt32(Aligned.Alignment.Literal.Value);
                alignment.AlignmentUnits = AlignmentUnits.Bytes;
                return true;
            }
            else
            {
                reporter.Report(declaration, 1017, declaration.Spelling);
                return false;
            }
        }
        private bool ValidBinaryDeclaration(Declare declaration, out Int32 Precision, out Int32 Scale, out bool Signed, out int Bytes)
        {
            Precision = 0;
            Scale = 0;
            Signed = false;
            Bytes = 0;

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
                Bytes = GetByteSize(declaration, Precision);
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
                Bytes = GetByteSize(declaration, Precision);
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

                    Bytes = GetByteSize(declaration, Precision);
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
                        Bytes = GetByteSize(declaration, Precision);
                    }
                }
            }

            return true;
        }
        private Alignment GetDefaultAlignment(Symbol symbol)
        {
            int byteLength = (symbol.Precision + 7) / 8;
            int value = 1 << (int)Math.Ceiling(Math.Log2(byteLength));
            return new Alignment() { AlignmentUnits = AlignmentUnits.Bytes, AlignmentValue = value };
        }
        private int GetByteSize(Declare symbol, int Precision)
        {
            if (symbol.Bounds.Count == 0) // not an array
            {
                return symbol.CoreType switch
                {
                    DataType.BIN => (Precision + 7) / 8,
                    DataType.UBIN => (Precision + 7) / 8
                };
            }

            return 0;
        }
        private bool IsBinaryType(DataType type)
        {
            switch (type)
            {
                case DataType.BIN:
                    return true;
                default:
                    return false;

            }
        }
    }
}