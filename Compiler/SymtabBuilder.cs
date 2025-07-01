using Syscode.Ast;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                symbol.CoreType = CoreType.Binary;
                symbol.Signed = false;

                if (declaration.TypeName == "bin8")
                    symbol.Precision = 8;

                if (declaration.TypeName == "bin16")
                    symbol.Precision = 16;

                if (declaration.TypeName == "bin32")
                    symbol.Precision = 32;

                if (declaration.TypeName == "bin64")
                    symbol.Precision = 64;

                if (declaration.typeSubscripts.Any())
                {
                    if (declaration.typeSubscripts.Count == 1)
                    {
                        bool isPrecisionValid = declaration.typeSubscripts[0].Type == ExpressionType.Literal && declaration.typeSubscripts[0].Literal.LiteralType == LiteralType.Numeric;

                        if (!isPrecisionValid)
                            diagnostics?.Invoke(this, new DiagnosticEvent(declaration, 1, Severity.Error,$"Precision for '{declaration.Spelling}' must be an integer literal"));

                        if (isPrecisionValid)
                            symbol.Precision = Convert.ToInt32(declaration.typeSubscripts[0].Literal.Value);
                    }

                    if (declaration.typeSubscripts.Count == 2)
                    {
                        bool isPrecisionValid = declaration.typeSubscripts[0].Type == ExpressionType.Literal && declaration.typeSubscripts[0].Literal.LiteralType == LiteralType.Numeric;
                        bool isScaleValid = declaration.typeSubscripts[1].Type == ExpressionType.Literal && declaration.typeSubscripts[1].Literal.LiteralType == LiteralType.Numeric;

                        if (!isPrecisionValid)
                            diagnostics?.Invoke(this, new DiagnosticEvent(declaration, 1, Severity.Error, $"Precision for '{declaration.Spelling}' must be an integer literal"));

                        if (!isScaleValid)
                            diagnostics?.Invoke(this, new DiagnosticEvent(declaration, 1, Severity.Error, $"Scale for '{declaration.Spelling}' must be an integer literal"));

                        if (isPrecisionValid && isScaleValid)
                        {
                            symbol.Precision = Convert.ToInt32(declaration.typeSubscripts[0].Literal.Value);
                            symbol.Scale = Convert.ToInt32(declaration.typeSubscripts[1].Literal.Value);
                        }
                    }
                }
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


    }
}
