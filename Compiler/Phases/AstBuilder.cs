  using Antlr4.Runtime;
using Antlr4.Runtime.Sharpen;
using Antlr4.Runtime.Tree;
using Syscode.Ast;
using System.Collections.Generic;
using System.Runtime.Versioning;
using System.Linq;
using static Syscode.LexerHelper;
using static Syscode.Attributes;
using static SyscodeParser;
using System.Runtime.InteropServices;

namespace Syscode
{
    /// <summary>
    /// Transforms a Syscode Antlr4 CST into a Syscode AST
    /// </summary>
    public class AstBuilder
    {
        private Reporter reporter;
        private IContainer? currentContainer = null;
        private Compilation compilation = null;
        private Dictionary<string, IConstant> constants;
        private SyscodeParser lexer;
        public AstBuilder(Dictionary<string, IConstant> constants, Reporter reporter, SyscodeParser lexer)
        {
            this.constants = constants;
            this.reporter = reporter;
            this.lexer = lexer;
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
                LoopContext context => CreateLoop(context),
                LeaveContext context => CreateLeave(context),
                _ => new AstNode(rule)
            };
        }
        private Leave CreateLeave(LeaveContext context)
        {
            // TODO: The ref on a leave stmt must be a simple identifier.
            return new Leave(context,this);
        }
        private Loop CreateLoop(LoopContext context)
        {
            if (context == null) throw new ArgumentNullException("context");

            if (context.Loop != null)
            {
                return new Loop(context)
                {
                    Statements = GetStatements(context.Loop).Select(Generate).ToList()
                };
            }
            if (context.For != null)
            {
                return new For(context.For,this)
                {
                    Statements = GetStatements(context.For).Select(Generate).ToList()
                };
            }

            if (context.While != null)
            {
                return new While(context.While, this)
                {
                    Statements = GetStatements(context.While).Select(Generate).ToList()
                };
            }

            if (context.Until != null)
            {
                return new Until(context.Until,this)
                {
                    Statements = GetStatements(context.Until).Select(Generate).ToList()
                };
            }

            throw new InternalErrorException($"Unrecognized loop syntax on line {context.Start.Line}");
        }
        private Goto CreateGoto(GotoContext context)
        {
            return new Goto(context, this);
        }
        private Label CreateLabel(AlabelContext context)
        {
            return new Label(context) { Spelling = context.Name.Spelling.GetText(), Subscript = context.Subscript?.Literal.GetText() };
        }
        private Return CreateReturn(ReturnContext context)
        {
            return new Return(context, this);
        }
        private Call CreateCall(CallContext context)
        {
            return new Call(context, this);
        }
        private Expression FoldIfConstantExpression(Expression expr)
        {
            // TODO: This is fragile just now, we must validate operand types, we cannot assume the expression is valid, only valid syntactically.
            // The expression might need implicit conversions inserting and so on.

            if (expr.Type == ExpressionType.Literal)
                return expr;

            if (expr.Type == ExpressionType.Binary)
            {
                if (expr.Left != null && expr.Left.IsConstant && expr.Right != null && expr.Right.IsConstant)
                {
                    switch (expr.Operator)
                    {
                        case Operator.PLUS:
                            {
                                var result = new Expression(null);

                                if (expr.Left.Literal != null && expr.Left.Literal.Constant.Signed && expr.Right?.Literal != null && expr.Right.Literal.Constant.Signed)
                                {
                                    var sum = expr.Left.Literal.Constant.ValueSigned + expr.Right.Literal.Constant.ValueSigned;
                                    result.Literal = new Literal(LiteralType.Binary) { Value = sum.ToString() };
                                    result.Type = ExpressionType.Literal;
                                }

                                if (expr.Left.Literal.Constant.Signed && expr.Right.Literal.Constant.Unsigned)
                                {
                                    var sum = expr.Left.Literal.Constant.ValueSigned + (long)expr.Right.Literal.Constant.ValueUnsigned;
                                    result.Literal = new Literal(LiteralType.Binary) { Value = sum.ToString() };
                                    result.Type = ExpressionType.Literal;
                                }

                                if (expr.Left.Literal.Constant.Unsigned && expr.Right.Literal.Constant.Signed)
                                {
                                    var sum = (long)expr.Left.Literal.Constant.ValueUnsigned + (long)expr.Right.Literal.Constant.ValueUnsigned;
                                    result.Literal = new Literal(LiteralType.Binary) { Value = sum.ToString() };
                                    result.Type = ExpressionType.Literal;
                                }

                                if (expr.Left.Literal.Constant.Unsigned && expr.Right.Literal.Constant.Unsigned)
                                {
                                    var sum = expr.Left.Literal.Constant.ValueUnsigned + expr.Right.Literal.Constant.ValueUnsigned;
                                    result.Literal = new Literal(LiteralType.Binary) { Value = sum.ToString() };
                                    result.Type = ExpressionType.Literal;
                                }

                                return result;
                            }
                        case Operator.MINUS:
                            {
                                var left = Convert.ToInt32(expr.Left.Literal.Value);
                                var right = Convert.ToInt32(expr.Right.Literal.Value);

                                var sum = left - right;

                                var result = new Expression(null);

                                result.Literal = new Literal(LiteralType.Binary) { Value = sum.ToString() };
                                result.Type = ExpressionType.Literal;
                                return result;
                            }
                        case Operator.TIMES:
                            {
                                var left = Convert.ToInt32(expr.Left.Literal.Value);
                                var right = Convert.ToInt32(expr.Right.Literal.Value);

                                var sum = left * right;

                                var result = new Expression(null);

                                result.Literal = new Literal(LiteralType.Binary) { Value = sum.ToString() };
                                result.Type = ExpressionType.Literal;
                                return result;
                            }
                        case Operator.DIVIDE:
                            {
                                var left = Convert.ToInt32(expr.Left.Literal.Value);
                                var right = Convert.ToInt32(expr.Right.Literal.Value);

                                var sum = left / right;

                                var result = new Expression(null);

                                result.Literal = new Literal(LiteralType.Binary) { Value = sum.ToString() };
                                result.Type = ExpressionType.Literal;
                                return result;
                            }
                    }
                }
            }

            return expr;
        }
        private Operator GetOperator(ExprPrefixedContext context)
        {
            //var operation = context.GetExactNode<PrefixExpressionContext>().GetExactNode<PrefixOperatorContext>();
            var terminal = (TerminalNodeImpl)context.Prefixed.Op.children[0]; ;//  (TerminalNodeImpl)operation.children.Where(c => c is TerminalNodeImpl).Single();
            return (Operator)(terminal.Symbol.Type);
        }
        private Operator GetOperator(ExprBinaryContext context)
        {
            var operation = context.children.Where(c => c is not ExpressionContext).Cast<ParserRuleContext>().Single();
            var terminal = (TerminalNodeImpl)operation.children.Where(c => c is TerminalNodeImpl).Single();

            return (Operator)(terminal.Symbol.Type);
        }
        private Assignment CreateAssignment(AssignmentContext context)
        {
            return new Assignment(context,this);
        }
        internal BasicReference CreateBasicReference(BasicReferenceContext context)
        {
            BasicReference basic = new BasicReference(context);

            if (context.TryGetExactNode<StructureQualificationListContext>(out var qualification))
            {
                var quals = qualification.GetExactNodes<StructureQualificationContext>();

                foreach (var qual in quals)
                {
                    var qualifier = new Qualification(qual);

                    if (qual.TryGetExactNode<ArgumentsContext>(out var arg))
                    {
                        var subs = arg.GetExactNode<SubscriptCommalistContext>();

                        var expressions = subs.GetDerivedNodes<ExpressionContext>().Select(CreateExpression);

                        qualifier.Arguments = new Arguments(subs) { ExpressionList = expressions.ToList() };

                    }

                    basic.Qualifier.Add(qualifier);
                }
            }

            return basic;
        }
        private IEnumerable<ParserRuleContext> GetStatements(ParserRuleContext context)
        {
            return context.GetExactNodes<StatementContext>().Select(s => s.GetDerivedNode<ParserRuleContext>()).ToList();
        }
        private Compilation CreateCompilation(CompilationContext context)
        {
            if (compilation != null)
                throw new InvalidOperationException("Internal error. There can only be a single 'compilation' when compiling a source file.");

            compilation = new Compilation(context) { Statements = GetStatements(context).Select(Generate).ToList() };

            return compilation;
        }
        private Scope CreateScope(ScopeContext context)
        {
            if (context.TryGetExactNode<BlockScopeContext>(out var block))
            {
                return new Scope(block) { Spelling = block.GetExactNode<QualifiedNameContext>().GetText(), Statements = GetStatements(block).Select(Generate).ToList() };
            }

            throw new InvalidOperationException();
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

                #region Repeated data attributes

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
        private StructBody CreateStructure(StructBodyContext context)
        {
            var bounds = new List<BoundsPair>();
            var spelling = context.GetLabelText(nameof(StructBodyContext.Spelling));
            var iskeyword = context.Spelling.children.OfType<KeywordContext>().Any();

            if (context.TryGetExactNode<DimensionSuffixContext>(out var dimensions))
            {
                bounds = CreateBounds(dimensions);
            }

            var structs = context.GetExactNodes<StructBodyContext>().Select(CreateStructure).ToList();
            var fields = context.GetExactNodes<StructFieldContext>().Select(CreateField).ToList(); ;

            return new StructBody(context) { IsKeyword = iskeyword, Spelling = spelling, Bounds = bounds, Structs = structs, Fields = fields };
        }
        private StructField CreateField(StructFieldContext context)
        {
            var bounds = new List<BoundsPair>();

            if (context.TryGetExactNode<SyscodeParser.DimensionSuffixContext>(out var dimensions))
            {
                bounds = CreateBounds(dimensions);
            }

            return new StructField(context) { Bounds = bounds };
        }
        private List<BoundsPair> CreateBounds(DimensionSuffixContext context)
        {
            return context.Pair._BoundPairs.Select(p => new BoundsPair(p,this)).ToList();
        }
        private Procedure CreateProcedure(ProcedureContext context)
        {
            var node = new Procedure(currentContainer, context, this);

            currentContainer = node;
            node.Statements = [.. GetStatements(context).Select(Generate)];
            currentContainer = node.Container;

            return node;
        }
        private Procedure CreateFunction(FunctionContext context)
        {
            var node = new Procedure(currentContainer, context, this); // a func is so similar to a proc we use same class to represent them.

            currentContainer = node;
            node.Statements = [.. GetStatements(context).Select(Generate)];
            currentContainer = node.Container;

            return node;
        }
        private Elif CreateElif(ExprThenBlockContext context)
        {
            return new Elif(context, this) { ThenStatements = GetStatements(context.GetExactNode<ThenBlockContext>()).Select(Generate).ToList() };
        }
        private If CreateIf(IfContext context)
        {
            List<AstNode> else_stmts = new();
            List<Elif> elifs = new();

            var if_then_stmts = GetStatements(context.ExprThen.Then).Select(Generate).ToList();

            if (context.Else != null)
            {
                else_stmts.Load(GetStatements(context.Else.Then).Select(Generate));
            }

            if (context.Elif != null)  // at least one 'elif' is present
            {
                elifs.Load(context.Elif._ExprThen.Select(CreateElif));
            }

            return new If(context, this) { ThenStatements = if_then_stmts, ElseStatements = else_stmts, ElifStatements = elifs };
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
        internal Expression CreateExpression(ExpressionContext context)
        {
            Expression expr = new(context);

            switch (context)
            {
                case ExprPrimitiveContext primitive when primitive.Primitive is RefContext reference:
                    {
                        expr.Reference = CreateReference(reference.Reference);
                        expr.Type = ExpressionType.Primitive;
                        return FoldIfConstantExpression(expr);
                    }
                case ExprPrimitiveContext primitive when primitive.Primitive is LitContext literal:
                    {
                        var txt = literal.Numeric.GetText();
                        expr.Literal = new Literal(literal.Numeric, constants) { Value = txt };
                        expr.Type = ExpressionType.Literal;
                        return FoldIfConstantExpression(expr);
                    }
                case ExprPrimitiveContext primitive when primitive.Primitive is StrContext strng:
                    {
                        var txt = strng.String.GetText();
                        expr.Literal = new Literal(strng.String) { Value = txt };
                        expr.Type = ExpressionType.Literal;
                        return FoldIfConstantExpression(expr);
                    }
                case ExprParenthesizedContext paren:
                    {
                        var result = CreateExpression(paren.Parenthesized.Expr);
                        result.Parenthesized = true;    // NOT we almost certainly don' care about this, it's only relevant to parser. 
                        return FoldIfConstantExpression(result);
                    }
                case ExprPrefixedContext prefixed:
                    {
                        expr.Right = CreateExpression(prefixed.Prefixed.Expr);  //CreateExpression(prefixed.GetExactNode<PrefixExpressionContext>().GetDerivedNode<ExpressionContext>());
                        expr.Operator = GetOperator(prefixed);
                        expr.Type = ExpressionType.Prefix;
                        break;
                    }
                case ExprBinaryContext binary:
                    {
                        expr.Left = CreateExpression(binary.Left);
                        expr.Right = CreateExpression(binary.Rite);
                        expr.Operator = GetOperator(binary);
                        expr.Type = ExpressionType.Binary;
                        return FoldIfConstantExpression(expr);
                    }
                default:  // every other case always contains an operator and a left/right expression. 
                    {
                        throw new InvalidOperationException("Unexpected expression class encountered.");
                    }
            }

            // If the expression is composed wholly of literal constants, we should fold it and make a new literal constant

            return FoldIfConstantExpression(expr);
        }
    }
}