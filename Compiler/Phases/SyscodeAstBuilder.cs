using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using static Syscode.Attributes;
using static SyscodeParser;

namespace Syscode
{
    /// <summary>
    /// Transforms a Syscode Antlr4 CST into a Syscode AST
    /// </summary>
    public class SyscodeAstBuilder
    {
        private readonly Reporter reporter;
        private IContainer? currentContainer = null;
        private readonly Compilation compilation = null;
        public readonly Dictionary<string, IConstant> Constants = new();
        public SyscodeAstBuilder(Reporter reporter)
        {
            this.reporter = reporter;
        }

        public Compilation Generate(CompilationContext rule)
        {
            return (Compilation)Generate((ParserRuleContext)rule);
        }
        public AstNode Generate(ParserRuleContext rule)
        {
            return rule switch
            {
                CompilationContext context => CreateCompilation(context),
                ScopeContext context => CreateScope(context),
                ProcedureContext context => CreateProcedure(context),
                FunctionContext context => CreateFunction(context),
                TypeContext context => CreateType(context),
                IfContext context => CreateIf(context),
                AssignmentContext context => CreateAssignment(context),
                DeclareContext context => CreateDeclaration(context),
                CallContext context => CreateCall(context),
                ReturnContext context => CreateReturn(context),
                AlabelContext context => CreateLabel(context),
                GotoContext context => CreateGoto(context),
                LoopsContext context => CreateLoops(context),
                ExitContext context => CreateLeave(context),
                LoopContext context => CreateLoop(context),
                Prep_IFContext context => CreateIF(context),
                Prep_INCLUDEContext context => CreateINCLUDE(context),
                Prep_REPLACEContext context => CreateREPLACE(context),
                _ => new AstNode(rule)
            };
        }

        private Loop CreateLoop(LoopContext context)
        {
            return new(context, this);
        }
        private Leave CreateLeave(ExitContext context)
        {
            return new (context,this);
        }
        private LoopBase CreateLoops(LoopsContext context)
        {
            if (context == null) throw new ArgumentNullException("context");

            return context switch
            {
                LoopAlwaysContext loop => new Always(loop,this) ,
                LoopForContext loop => new For(loop,this),
                LoopUntilContext loop => new Until(loop,this),
                LoopWhileContext loop => new While(loop,this),
            };

            throw new InternalErrorException($"Unrecognized loop syntax on line {context.Start.Line}");
        }
        private Goto CreateGoto(GotoContext context)
        {
            return new Goto(context, this);
        }
        private static Label CreateLabel(AlabelContext context)
        {
            return new Label(context);
        }
        private Return CreateReturn(ReturnContext context)
        {
            return new Return(context, this);
        }
        private Call CreateCall(CallContext context)
        {
            return new Call(context, this);
        }
        //private Expression FoldIfConstantExpression(Expression expr)
        //{
        //    // TODO: This is fragile just now, we must validate operand types, we cannot assume the expression is valid, only valid syntactically.
        //    // The expression might need implicit conversions inserting and so on.

        //    if (expr.Type == ExpressionType.Literal)
        //        return expr;

        //    if (expr.Type == ExpressionType.Binary)
        //    {
        //        if (expr.Left != null && expr.Left.IsConstant && expr.Right != null && expr.Right.IsConstant)
        //        {
        //            switch (expr.Operator)
        //            {
        //                case Operator.PLUS:
        //                    {
        //                        var result = new Expression(null);

        //                        if (expr.Left.Literal != null && expr.Left.Literal.Constant.Signed && expr.Right?.Literal != null && expr.Right.Literal.Constant.Signed)
        //                        {
        //                            var sum = expr.Left.Literal.Constant.ValueSigned + expr.Right.Literal.Constant.ValueSigned;
        //                            result.Literal = new Literal(LiteralType.Binary) { Value = sum.ToString() };
        //                            result.Type = ExpressionType.Literal;
        //                        }

        //                        if (expr.Left.Literal.Constant.Signed && expr.Right.Literal.Constant.Unsigned)
        //                        {
        //                            var sum = expr.Left.Literal.Constant.ValueSigned + (long)expr.Right.Literal.Constant.ValueUnsigned;
        //                            result.Literal = new Literal(LiteralType.Binary) { Value = sum.ToString() };
        //                            result.Type = ExpressionType.Literal;
        //                        }

        //                        if (expr.Left.Literal.Constant.Unsigned && expr.Right.Literal.Constant.Signed)
        //                        {
        //                            var sum = (long)expr.Left.Literal.Constant.ValueUnsigned + (long)expr.Right.Literal.Constant.ValueUnsigned;
        //                            result.Literal = new Literal(LiteralType.Binary) { Value = sum.ToString() };
        //                            result.Type = ExpressionType.Literal;
        //                        }

        //                        if (expr.Left.Literal.Constant.Unsigned && expr.Right.Literal.Constant.Unsigned)
        //                        {
        //                            var sum = expr.Left.Literal.Constant.ValueUnsigned + expr.Right.Literal.Constant.ValueUnsigned;
        //                            result.Literal = new Literal(LiteralType.Binary) { Value = sum.ToString() };
        //                            result.Type = ExpressionType.Literal;
        //                        }

        //                        return result;
        //                    }
        //                case Operator.MINUS:
        //                    {
        //                        var left = Convert.ToInt32(expr.Left.Literal.Value);
        //                        var right = Convert.ToInt32(expr.Right.Literal.Value);

        //                        var sum = left - right;

        //                        var result = new Expression(null);

        //                        result.Literal = new Literal(LiteralType.Binary) { Value = sum.ToString() };
        //                        result.Type = ExpressionType.Literal;
        //                        return result;
        //                    }
        //                case Operator.TIMES:
        //                    {
        //                        var left = Convert.ToInt32(expr.Left.Literal.Value);
        //                        var right = Convert.ToInt32(expr.Right.Literal.Value);

        //                        var sum = left * right;

        //                        var result = new Expression(null);

        //                        result.Literal = new Literal(LiteralType.Binary) { Value = sum.ToString() };
        //                        result.Type = ExpressionType.Literal;
        //                        return result;
        //                    }
        //                case Operator.DIVIDE:
        //                    {
        //                        var left = Convert.ToInt32(expr.Left.Literal.Value);
        //                        var right = Convert.ToInt32(expr.Right.Literal.Value);

        //                        var sum = left / right;

        //                        var result = new Expression(null);

        //                        result.Literal = new Literal(LiteralType.Binary) { Value = sum.ToString() };
        //                        result.Type = ExpressionType.Literal;
        //                        return result;
        //                    }
        //            }
        //        }
        //    }

        //    return expr;
        //}
        private Assignment CreateAssignment(AssignmentContext context)
        {
            return new Assignment(context,this);
        }
        internal BasicReference CreateBasicReference(BasicReferenceContext context)
        {
            return new(context, this);
        }
        public List<AstNode> GenerateStatements(IList<StatementContext> context)
        {
            return context.Select(s => s.GetDerivedNode<ParserRuleContext>()).Select(Generate).ToList();
        }
        private Compilation CreateCompilation(CompilationContext context)
        {
            return new (context, this);
        }
        private Scope CreateScope(ScopeContext context)
        {
            return new (context) { Spelling = context.GetExactNode<QualifiedNameContext>().GetText(), Statements = GenerateStatements(context._Statements) };
        }
        private Declare CreateDeclaration(DeclareContext context)
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
                    dcl.Bounds = context.Bounds.Pair._BoundPairs.Select(p => new BoundsPair(p,this)).ToList();
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
        private Type CreateType(TypeContext context)
        {
            return new Type(context) { Body = CreateStructure(context.Body) };
        }
        public StructBody CreateStructure(StructBodyContext context)
        {
            return new StructBody(context, this);
        }
        public StructField CreateField(StructFieldContext context)
        {
            return new StructField(context, this);
        }
        public List<BoundsPair> CreateBounds(DimensionSuffixContext context)
        {
            return context.Pair._BoundPairs.Select(p => new BoundsPair(p,this)).ToList();
        }
        private Procedure CreateProcedure(ProcedureContext context)
        {
            return new Procedure(ref currentContainer, context, this);
        }
        private Procedure CreateFunction(FunctionContext context)
        {
            return new Procedure(ref currentContainer, context, this); // a func is so similar to a proc we use same class to represent them.
        }
        public Elif CreateElif(ElifContext context)
        {
            return new Elif(context, this);
        }
        public ELIF CreateELIF(Prep_exprThenBlockContext context)
        {
            return new ELIF(context, this) { Statements = GenerateStatements(context.THEN_block._Statements) };
        }

        private If CreateIf(IfContext context)
        {
            return new If(context, this);
        }
        private IF CreateIF(Prep_IFContext context)
        {
            return new IF(context, this);
        }

        private INCLUDE CreateINCLUDE(Prep_INCLUDEContext context)
        {
            return new INCLUDE(context, this);
        }

        private REPLACE CreateREPLACE(Prep_REPLACEContext context)
        {
            return new REPLACE(context, this);
        }


        internal static Operator GetOperator(ExprPrefixedContext context)
        {
            //var operation = context.GetExactNode<PrefixExpressionContext>().GetExactNode<PrefixOperatorContext>();
            var terminal = (TerminalNodeImpl)context.Prefixed.Op.children[0]; ;//  (TerminalNodeImpl)operation.children.Where(c => c is TerminalNodeImpl).Single();
            return (Operator)(terminal.Symbol.Type);
        }
        internal static Operator GetOperator(ExprBinaryContext context)
        {
            var operation = context.Operator.children.Where(c => c is not ExpressionContext).Cast<ParserRuleContext>().Single();
            var terminal = (TerminalNodeImpl)operation.children.Where(c => c is TerminalNodeImpl).Single();

            return (Operator)(terminal.Symbol.Type);
        }
        /// <summary>
        /// Reports if any of the supplied attributes are incompatible with the others.
        /// </summary>
        /// <param name="dcl"></param>
        /// <param name="classes"></param>
        private void ReportIncompatibleDataAttributes(Declare dcl, IEnumerable<System.Type> classes)
        {
            if (classes.Count() > 1)
            {
                var first = classes.First();
                var rest = classes.Skip(1);

                foreach (var next in rest)
                {
                    if (Attributes.IncompatibleDataAttributes((first, next)))
                    {
                        reporter.Report(dcl, 1031, dcl.Spelling, GetKeywordFromAttribute(next), GetKeywordFromAttribute(first));
                    }
                }

                ReportIncompatibleDataAttributes(dcl, rest);
            }
        }
        internal Reference CreateReference(ReferenceContext context)
        {
            return new Reference(context,this);
        }

        internal Expression CreateExpression(PrimitiveExpressionContext context)
        {
            return new Expression(context, this);
        }
        internal Expression CreateExpression(ExpressionContext context)
        {
            if (context is ExprParenthesizedContext parenthesized)
            {
                var result = CreateExpression(parenthesized.Parenthesized.Expr);
                result.Parenthesized = true;    // NOT we almost certainly don' care about this, it's only relevant to parser. 
                return result;
            }

            Expression expr = new(context, this);

            // If the expression is composed wholly of literal constants, we should fold it and make a new literal constant

            return expr;
        }
    }
}