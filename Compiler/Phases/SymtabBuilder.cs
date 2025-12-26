using Antlr4.Runtime;
using Syscode.Ast;
using System.Data;
using System.Linq;
using static SyscodeParser;

namespace Syscode
{
    // TODO: Rework this, the symtab can and should be built from the CST and then the AST phase can resolve refs to that. The way 
    // it is here is wrong!
    
    public class SymtabBuilder
    {
        private readonly Reporter reporter;
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

        public void Generate(ParserRuleContext rule)
        {
            return rule switch
            {
                //CompilationContext context => CreateCompilation(context),
                //ProcedureContext context => CreateProcedure(context),
                //FunctionContext context => CreateFunction(context),
                //TypeContext context => CreateType(context),
                DeclareContext context => ProcessDeclaration(context),
                AlabelContext context => CreateLabel(context),
                _ => new AstNode(rule)
            };

        }


        public Symbol ProcessDeclaration(DeclareContext context)
        {
            /*
             *  The processing of a Syscode declaration involves the possible presence
             *  of multiple or contradictory attributes. This is because the grammar
             *  is permissive and avoids parse failures for this kind of error, it is
             *  esier to apply these rules after parsing.
             *  
             *  A declaration has two categories of attributes, data-attributes and
             *  attributes.
             */

            var dcl = new Declare(currentContainer, context);

            try
            {

                #region Extract array bounds
                if (context.Bounds != null)
                {
                    dcl.Bounds = context.Bounds.Pair._BoundPairs.Select(p => new BoundsPair(p, this)).ToList();
                }

                if (context.Struct != null)
                {
                    dcl.StructBody = CreateStructure(context.Struct);
                    dcl.Spelling = dcl.StructBody.Spelling;  // copy the spelling for convenience in debugging.
                }
                else
                {
                    dcl.Spelling = context.Spelling.GetText();
                    dcl.IsKeyword = context.Spelling.Key != null;
                }
                #endregion

                var dataAttributesGroups = context._DataAttributes.GroupBy(d => d.GetType()).ToArray();

                #region No data attribues

                if (dataAttributesGroups.Any() == false)
                {
                    reporter.Report(dcl, 1030, dcl.Spelling);
                    return dcl;
                }
                #endregion

                #region Repeated data attributes

                if (dataAttributesGroups.Where(g => g.Count() > 1).Any())
                {
                    var repeaters = dataAttributesGroups.Where(g => g.Count() > 1);

                    foreach (var group in repeaters)
                    {
                        string attrtext = GetKeywordFromAttribute(group.First().GetType());
                        reporter.Report(dcl, 1023, dcl.Spelling, attrtext);
                    }

                    return dcl;
                }
                #endregion

                #region Incompatible attributes

                ReportIncompatibleDataAttributes(dcl, dataAttributesGroups.Select(g => g.Key));

                if (dcl.ReportedError > 0)
                    return dcl;

                #endregion

                #region Apply attributes 

                // At this point there are potentially several attributes present but they are all compatible

                foreach (var attributeGroup in dataAttributesGroups)
                {
                    switch (attributeGroup.Single())  // We know at this point that no attribute occurs more than once, if this throws we have a bug.
                    {
                        case LabelContext attribute:
                            {
                                dcl.CoreType = DataType.LABEL;
                                break;
                            }
                        case BitContext attribute:
                            {
                                dcl.CoreType = DataType.BIT;
                                break;
                            }
                        case PointerContext attribute:
                            {
                                dcl.CoreType = DataType.POINTER;
                                break;
                            }
                        case IntegerContext attribute:
                            {
                                int precision = 0;
                                int scale = 0;

                                dcl.CoreType = DataType.BIN;

                                //var attr = context._DataAttributes.OfType<IntegerContext>().Single();

                                if (attribute.Integer.Args == null) // this is predefined standard type
                                {
                                    dcl.BIN = (attribute.Integer.digits, 0, attribute.Integer.signed);
                                }
                                else
                                {
                                    // extract the details by examining the context further
                                    if (attribute.Integer.Args.List._Exp.Count > 2)
                                    {
                                        reporter.Report(dcl, 1026);
                                        return dcl;
                                    }

                                    var precexp = CreateExpression(attribute.Integer.Args.List._Exp[0]);

                                    if (precexp.IsConstant)
                                        precision = Convert.ToInt32(precexp.Literal.Value);
                                    else
                                    {
                                        reporter.Report(dcl, 1027, "1", "64");
                                        return dcl;
                                    }

                                    if (attribute.Integer.Args.List._Exp.Count == 2) // is there a scale factor?
                                    {
                                        var scaleexp = CreateExpression(attribute.Integer.Args.List._Exp[1]);

                                        if (scaleexp.IsConstant)
                                            scale = Convert.ToInt32(scaleexp.Literal.Value);
                                        else
                                        {
                                            reporter.Report(dcl, 1028, "-60", "64");
                                            return dcl;
                                        }
                                    }

                                    if (precision < 1 || precision > 64)
                                    {
                                        reporter.Report(dcl, 1027, "1", "64");
                                        return dcl;

                                    }
                                    if (scale < -60 || scale > 64)
                                    {
                                        reporter.Report(dcl, 1028, "-60", "64");
                                        return dcl;
                                    }

                                    dcl.BIN = (precision, scale, attribute.Integer.signed);
                                }
                                break;
                            }
                        case EntryContext attribute:
                            {
                                dcl.CoreType = DataType.ENTRY;
                                break;
                            }
                        case StringContext attribute:
                            {
                                dcl.CoreType = DataType.STRING;
                                break;
                            }
                        case AsContext attribute:
                            {
                                dcl.CoreType = DataType.AS;
                                break;
                            }
                        case AlignedContext attribute:
                            {
                                //dcl.CoreType = DataType.AS;
                                break;
                            }
                        case VariableContext attribute:
                            {
                                break;
                            }
                        case BuiltinContext attribute:
                            {
                                dcl.CoreType = DataType.BUILTIN;
                                break;
                            }
                        default:
                            {
                                reporter.Report(dcl, 1032, nameof(CreateDeclaration));
                                throw new InvalidOperationException("Internal error");
                            }
                    }
                }
                #endregion

                // Process remaining non-data attributes

                var attributesGroups = context._Attributes.GroupBy(d => d.GetType()).ToArray();

                #region Repeated attributes

                if (attributesGroups.Where(g => g.Count() > 1).Any())
                {
                    var repeaters = attributesGroups.Where(g => g.Count() > 1);

                    foreach (var group in repeaters)
                    {
                        string attrtext = GetKeywordFromAttribute(group.First().GetType());
                        reporter.Report(dcl, 1029, dcl.Spelling, attrtext);
                    }

                    return dcl;
                }
                #endregion

                #region Incompatible attributes

                //TestCompatibility(dcl, dataAttributesGroups.Select(g => g.Key).OrderBy(g => g.Name));

                //if (dcl.ReportedError > 0)
                //    return dcl;

                #endregion

                dcl.Validated = true;
                return dcl;

            }
            catch (Exception e)
            {
                reporter.Report(dcl, 1032, nameof(CreateDeclaration));
                throw new InternalErrorException($"In '{nameof(CreateDeclaration)}' processing line {dcl.StartLine}.", e);
            }

        }

        public StructBody CreateStructure(StructBodyContext context)
        {
            return new StructBody(context, this);
        }


        public Symbol CreateSymbol(Procedure procedure)
        {
            var sym = new Symbol(procedure);
            procedure.Symbols.Load(procedure.Statements.OfType<Declare>().Select(CreateSymbol));
            procedure.Symbols.AddRange(procedure.Statements.OfType<If>().Where(s => s.HasLabel).Select(CreateSymbol));
            procedure.Symbols.AddRange(procedure.Statements.OfType<Loop>().Where(s => s.HasLabel).Select(CreateSymbol));
            procedure.Symbols.AddRange(procedure.Statements.OfType<Label>().Select(CreateSymbol));
            sym.Invalid = false;
            return sym;
        }
        public Symbol CreateSymbol (If If)
        {
            return new Symbol(If);
        }
        public Symbol CreateSymbol(Loop Loop)
        {
            return new Symbol(Loop);
        }

        public Symbol CreateSymbol(Label Label)
        {
            return new Symbol(Label);
        }

        public Symbol CreateSymbol(Declare declaration)
        {
            ReportDeclaredKeywords(declaration);

            ValidateAttributes(declaration);

            if (declaration.IsStructure)
                ValidateStructure(declaration);

            var symbol = new Symbol(declaration,this);

            if (symbol.DataType == DataType.STRING)
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

        public void ApplyDefaults(Symbol symbol)
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
            var memberNames = Struct.Fields.Where(f => f.Pad == false).Select(s => s.Spelling).Concat(Struct.Structs.Select(s => s.Spelling));

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
        private static bool IsScaleInvalid(Declare declaration)
        {
            return !((declaration.typeSubscripts[1].Type == ExpressionType.Literal) && (declaration.typeSubscripts[1].Literal?.LiteralType == LiteralType.Decimal));
        }
        private static bool IsPrecisionInvalid(Declare declaration)
        {
            return !((declaration.typeSubscripts[0].Type == ExpressionType.Literal) && (declaration.typeSubscripts[0].Literal?.LiteralType == LiteralType.Decimal));
        }
        private static bool IsStringLengthInvalid(Declare declaration)
        {
            return !((declaration.typeSubscripts[0].Type == ExpressionType.Literal) && (declaration.typeSubscripts[0].Literal?.LiteralType == LiteralType.Decimal));
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

            Length = Convert.ToInt32(declaration.typeSubscripts[0].Literal?.Value);

            return true;

        }
        private void ValidateAttributes(Declare declaration)
        {
            var groups = declaration.Attributes.GroupBy(g => g.GetType());

            foreach (var group in groups)
            {
                if (group.Count() > 1)
                {
                    reporter.Report(declaration, 1018, declaration.Spelling, group.Key.Name.ToLower());
                }
            }


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

            if (Aligned.Alignment.IsConstant && Aligned.Alignment?.Literal != null)
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
                if (declaration.typeSubscripts.Count != 0)
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
                if (declaration.typeSubscripts.Count != 0)
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


            if (declaration.typeSubscripts.Count != 0)
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
                        Precision = Convert.ToInt32(declaration.typeSubscripts[0].Literal?.Value);

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
                        Precision = Convert.ToInt32(declaration.typeSubscripts[0].Literal?.Value);
                        Scale = Convert.ToInt32(declaration.typeSubscripts[1].Literal?.Value);
                        Bytes = GetByteSize(declaration, Precision);
                    }
                }
            }

            return true;
        }
        public  Alignment GetDefaultAlignment(Symbol symbol)
        {
            int byteLength = (symbol.Precision + 7) / 8;
            int value = 1 << (int)Math.Ceiling(Math.Log2(byteLength));
            return new Alignment() { AlignmentUnits = AlignmentUnits.Bytes, AlignmentValue = value };
        }
        private static int GetByteSize(Declare symbol, int Precision)
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
        private static bool IsBinaryType(DataType type)
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