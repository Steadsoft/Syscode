using Antlr4.Runtime;
using Antlr4.Runtime.Sharpen;
using Antlr4.Runtime.Tree;
using Syscode.Ast;
using System.Collections.Generic;
using System.Runtime.Versioning;
using System.Linq;
using static Syscode.LexerHelper;
using static SyscodeParser;
using System.Runtime.InteropServices;

namespace Syscode
{
    /// <summary>
    /// Transforms a Syscode Antlr4 CST into a Syscode AST
    /// </summary>
    public class AstBuilder
    {
        private static Dictionary<System.Type, string> typeToKeyword = new Dictionary<System.Type, string>
            {
                { typeof(PackedContext),    KeywordText(PACKED) },
                { typeof(VariableContext),  KeywordText(VARIABLE) },
                { typeof(AlignedContext),   KeywordText(ALIGNED) },
                { typeof(LabelContext),     KeywordText(LABEL) },
                { typeof(BitContext),       KeywordText(BIT) },
                { typeof(PointerContext),   KeywordText(POINTER) },
                { typeof(IntegerContext),   $"{KeywordText(BIN)}/{KeywordText(UBIN)}" },
                { typeof(EntryContext),     KeywordText(ENTRY) },
                { typeof(StringContext),    KeywordText(STRING) },
                { typeof(AsContext),        KeywordText(AS) } ,
                { typeof(ConstContext),     KeywordText(CONST) },
                { typeof(OffsetContext),    KeywordText(OFFSET) },
                { typeof(ExternalContext),  KeywordText(EXTERNAL) },
                { typeof(InternalContext),  KeywordText(INTERNAL) },
                { typeof(StaticContext),    KeywordText(STATIC) },
                { typeof(BasedContext),     KeywordText(BASED) },
                { typeof(StackContext),     KeywordText(STACK) },
                { typeof(InitContext),      KeywordText(INIT) },
                { typeof(BuiltinContext),   KeywordText(BUILTIN) },
                { typeof(PadContext),       KeywordText(PAD) }
            };

        private Reporter reporter;
        private IContainer currentContainer = null;
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
        public Leave CreateLeave(LeaveContext context)
        {
            // TODO: The ref on a leave stmt must be a simple identifier.
            return new Leave(context) { Reference = CreateReference(context.Ref) };
        }
        public Loop CreateLoop(LoopContext context)
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
                return new For(context)
                {
                    ForRef = CreateReference(context.For.For),
                    From = CreateExpression(context.For.From),
                    To = CreateExpression(context.For.To),
                    By = CreateExpression(context.For.By),
                    WhileExp = CreateExpression(context.For.While?.Exp),
                    UntilExp = CreateExpression(context.For.Until?.Exp),
                    Statements = GetStatements(context.For).Select(Generate).ToList()
                };
            }

            if (context.While != null)
            {
                return new While(context)
                {
                    WhileExp = CreateExpression(context.While.While.Exp),
                    UntilExp = CreateExpression(context.While.Until?.Exp),
                    Statements = GetStatements(context.While).Select(Generate).ToList()
                };
            }

            if (context.Until != null)
            {
                return new Until(context)
                {
                    UntilExp = CreateExpression(context.Until.Until.Exp),
                    WhileExp = CreateExpression(context.Until.While?.Exp),
                    Statements = GetStatements(context.Until).Select(Generate).ToList()
                };
            }

            throw new InternalErrorException($"Unrecognized loop syntax on line {context.Start.Line}");
        }
        public Goto CreateGoto(GotoContext context)
        {
            return new Goto(context) { Reference = CreateReference(context.Ref) };
        }
        private Label CreateLabel(AlabelContext context)
        {
            return new Label(context) { Spelling = context.Name.Spelling.GetText(), Subscript = context.Subscript?.Literal.GetText() };
        }
        private Return CreateReturn(ReturnContext context)
        {
            return new Return(context) { Expression = CreateExpression(context.Exp) };
        }
        private Call CreateCall(CallContext context)
        {
            return new Call(context) { Reference = CreateReference(context.Ref) };
        }
        private Expression CreateExpression(ExpressionContext context)
        {
            if (context == null) // allows this to be called for an  optional expression and not fault.
                return null;

            Expression expr = new(context);

            switch (context)
            {
                case ExprPrimitiveContext primContext:
                    {
                        var prim = primContext.GetExactNode<PrimitiveExpressionContext>();

                        if (prim.TryGetExactNode<ReferenceContext>(out var refcontext))
                        {
                            expr.Reference = CreateReference(refcontext);
                            expr.Type = ExpressionType.Primitive;
                        }

                        if (prim.TryGetExactNode<NumericLiteralContext>(out var numcontext))
                        {
                            var txt = numcontext.GetText();
                            expr.Literal = new Literal(numcontext, constants) { Value = txt };
                            expr.Type = ExpressionType.Literal;

                        }

                        if (prim.TryGetExactNode<StringLiteralContext>(out var strcontext))
                        {
                            var txt = strcontext.GetText();
                            expr.Literal = new Literal(strcontext) { Value = txt };
                            expr.Type = ExpressionType.Literal;

                        }

                        return FoldConstantExpression(expr); ;
                    }
                case ExprParenthesizedContext paren:
                    {
                        var parenctxt = paren.GetExactNode<ParenthesizedExpressionContext>();
                        var expression = parenctxt.GetDerivedNode<ExpressionContext>();
                        var result = CreateExpression(expression);
                        result.Parenthesized = true;
                        return FoldConstantExpression(result); ;
                    }
                case ExprPrefixedContext prefixed:
                    {
                        expr.Right = CreateExpression(prefixed.GetExactNode<PrefixExpressionContext>().GetDerivedNode<ExpressionContext>());
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
                        return FoldConstantExpression(expr);
                    }
                default:  // every other case always contains an operator and a left/right expression. 
                    {
                        throw new InvalidOperationException("Unexpected expression class encountered.");
                    }
            }

            // If the expression is composed wholly of literal constants, we should fold it and make a new literal constant


            return FoldConstantExpression(expr);

        }
        private Expression FoldConstantExpression(Expression expr)
        {
            // TODO: This is fragile just now, we must validate operand types, we cannot assume the expression is valid, only valid syntactically.
            // The expression might need implicit conversions inserting and so on.

            if (expr.Type == ExpressionType.Literal)
                return expr;

            if (expr.Type == ExpressionType.Binary)
            {
                if (expr.Left.IsConstant && expr.Right.IsConstant)
                {
                    switch (expr.Operator)
                    {
                        case Operator.PLUS:
                            {
                                var result = new Expression(null);

                                if (expr.Left.Literal.Constant.Signed && expr.Right.Literal.Constant.Signed)
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
            var operation = context.GetExactNode<PrefixExpressionContext>().GetExactNode<PrefixOperatorContext>();
            var terminal = (TerminalNodeImpl)operation.children.Where(c => c is TerminalNodeImpl).Single();
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
            var reference = CreateReference(context.Ref);
            var expression = CreateExpression(context.Exp);

            return new Assignment(context) { Reference = reference, Expression = expression };
        }
        private Reference CreateReference(ReferenceContext context)
        {
            Reference reference = new(context);

            // A Reference might contain another Reference...

            if (context.Pointer != null)
            {
                reference.Pointer = CreateReference(context.Pointer);
            }

            if (context.ArgsList != null)
            {
                var argumentsList = context.ArgsList._ArgsSet; /* one or more 'arguments' always present */

                foreach (var arguments in argumentsList)
                {
                    var argsast = new Arguments(arguments);

                    if (arguments.TryGetExactNode<SubscriptCommalistContext>(out var subscriptCommalist))
                    {
                        var expressions = subscriptCommalist.GetDerivedNodes<ExpressionContext>().Select(CreateExpression).ToList();

                        argsast.ExpressionList.AddRange(expressions);
                    }

                    reference.ArgumentsList.Add(argsast);
                }
            }

            // TODO: process the optional ArgList list..

            reference.BasicReference = CreateBasicReference(context.Basic);

            return reference;
        }
        private BasicReference CreateBasicReference(BasicReferenceContext context)
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

            #region Extract array bounds
            if (context.Bounds != null)
            {
                dcl.Bounds = context.Bounds.Pair._BoundPairs.Select(p => new BoundsPair(p) { Lower = CreateExpression(p.Lower), Upper = CreateExpression(p.Upper) }).ToList();
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
                    string attrtext = typeToKeyword[group.First().GetType()];
                    reporter.Report(dcl, 1023, dcl.Spelling, attrtext);
                }

                return dcl;
            }
            #endregion

            #region Incompatible attributes

            TestCompatibility(dcl, dataAttributesGroups.Select(g => g.Key).OrderBy(g => g.Name));

            if (dcl.ReportedError > 0)
                return dcl;

            #endregion

            #region Apply attributes 

            // At this point there are potentially several attributes present but they are all compatible

            foreach (var attributeGroup in dataAttributesGroups)
            {
                switch (attributeGroup.First())  // We know at this point that no attribute occurs more than once.
                {
                    case LabelContext lblCtx:
                        {
                            dcl.CoreType = DataType.LABEL;
                            break;
                        }
                    case BitContext bitCtx:
                        {
                            dcl.CoreType = DataType.BIT;
                            break;
                        }
                    case PointerContext pointerCtx:
                        {
                            dcl.CoreType = DataType.POINTER;
                            break;
                        }
                    case IntegerContext integerCtx:
                        {
                            int precision = 0;
                            int scale = 0;

                            dcl.CoreType = DataType.BIN;

                            var attr = context._DataAttributes.OfType<IntegerContext>().Single();

                            if (attr.Integer.Args == null) // this is predefined standard type
                            {
                                dcl.BIN = (attr.Integer.digits, 0, attr.Integer.signed);
                            }
                            else
                            {
                                // extract the details by examining the context further
                                if (attr.Integer.Args.List._Exp.Count > 2)
                                {
                                    reporter.Report(dcl, 1026);
                                    return dcl;
                                }

                                var precexp = CreateExpression(attr.Integer.Args.List._Exp[0]);

                                if (precexp.IsConstant)
                                    precision = Convert.ToInt32(precexp.Literal.Value);
                                else
                                {
                                    reporter.Report(dcl, 1027, "1", "64");
                                    return dcl;
                                }

                                if (attr.Integer.Args.List._Exp.Count == 2) // is there a scale factor?
                                {
                                    var scaleexp = CreateExpression(attr.Integer.Args.List._Exp[1]);

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

                                dcl.BIN = (precision, scale, attr.Integer.signed);
                            }
                            break;
                        }
                    case EntryContext entryCtx:
                        {
                            dcl.CoreType = DataType.ENTRY;
                            break;
                        }
                    case StringContext stringCtx:
                        {
                            dcl.CoreType = DataType.STRING;
                            break;
                        }
                    case AsContext asCtx:
                        {
                            dcl.CoreType = DataType.AS;
                            break;
                        }
                    case AlignedContext alCtx:
                        {
                            dcl.CoreType = DataType.AS;
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

            #region Count attributes

            //context._Attributes.ForEach(d => counters[d.GetType()] = (counters[d.GetType()].Count + 1, counters[d.GetType()].Keyword));

            //#endregion
            //#region Report duplications
            //counters.Values.Where(v => v.Count > 1).ForEach(v => reporter.Report(dcl, 1029, dcl.Spelling, v.Keyword));
            #endregion

            dcl.Validated = true;

            return dcl;
        }

        public Type CreateType(TypeContext context)
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

            var bounds = context.Pair._BoundPairs.Select(p => new BoundsPair(p) { Lower = CreateExpression(p.Lower), Upper = CreateExpression(p.Upper) }).ToList();

            return bounds;
        }
        private Procedure CreateProcedure(ProcedureContext context)
        {
            var node = new Procedure(currentContainer, context);

            currentContainer = node;

            node.Spelling = context.Spelling.GetText();

            if (context.Params != null)
            {
                node.Parameters = context.Params._Params.Select(i => i.GetText()).ToList();
            }

            if (context.Options != null)
            {
                if (context.Options.Main != null)
                    node.Main = true;
            }

            node.Statements = [.. GetStatements(context).Select(Generate)];

            currentContainer = node.Container;

            return node;
        }

        private Procedure CreateFunction(FunctionContext context)
        {
            var node = new Procedure(currentContainer, context); // a func is so similar to a proc we use same class to represent them.

            currentContainer = node;

            node.Spelling = context.Spelling.GetText();

            node.As = context.Type.GetText();

            if (context.Params != null)
            {
                node.Parameters = context.Params._Params.Select(i => i.GetText()).ToList();
            }

            node.Statements = [.. GetStatements(context).Select(Generate)];
            node.IsFunction = true;

            currentContainer = node.Container;

            return node;
        }

        private Elif CreateElif(ExprThenBlockContext context)
        {
            var condition = CreateExpression(context.Exp);
            return new Elif(context) { Condition = condition, ThenStatements = GetStatements(context.GetExactNode<ThenBlockContext>()).Select(Generate).ToList() };
        }
        private If CreateIf(IfContext context)
        {
            List<AstNode> else_stmts = new();
            List<Elif> elifs = new();

            var if_then_stmts = GetStatements(context.ExprThen.Then).Select(Generate).ToList();
            var condition = CreateExpression(context.ExprThen.Exp);

            if (context.Else != null)
            {
                else_stmts.Load(GetStatements(context.Else.Then).Select(Generate));
            }

            if (context.Elif != null)  // at least one 'elif' is present
            {
                elifs.Load(context.Elif._ExprThen.Select(CreateElif));
            }

            return new If(context) { ThenStatements = if_then_stmts, ElseStatements = else_stmts, ElifStatements = elifs, Condition = condition };
        }

        private void TestCompatibility(Declare dcl, IEnumerable<System.Type> types)
        {
            if (types.Count() > 1)
            {
                var first = types.First();
                var rest = types.Skip(1);

                foreach (var attr in rest)
                {
                    if (!CompatibleAttributes(first, attr))
                    {
                        reporter.Report(dcl, 1031, dcl.Spelling, typeToKeyword[attr], typeToKeyword[first]);
                    }
                }

                TestCompatibility(dcl, rest);
            }
        }

        private bool CompatibleAttributes(System.Type a, System.Type b)
        {
            if (a == typeof(Aligned) && b == typeof(EntryConstant)) return false;
            if (a == typeof(Aligned) && b == typeof(LabelContext)) return false;
            if (a == typeof(Aligned) && b == typeof(PackedContext)) return false;

            if (a == typeof(AsContext) && b == typeof(BitContext)) return false;
            if (a == typeof(AsContext) && b == typeof(EntryContext)) return false;
            if (a == typeof(AsContext) && b == typeof(IntegerContext)) return false;
            if (a == typeof(AsContext) && b == typeof(LabelContext)) return false;
            if (a == typeof(AsContext) && b == typeof(PointerContext)) return false;
            if (a == typeof(AsContext) && b == typeof(StringContext)) return false;
            if (a == typeof(AsContext) && b == typeof(VariableContext)) return false;

            if (a == typeof(BitContext) && b == typeof(EntryContext)) return false;
            if (a == typeof(BitContext) && b == typeof(IntegerContext)) return false;
            if (a == typeof(BitContext) && b == typeof(LabelContext)) return false;
            if (a == typeof(BitContext) && b == typeof(PointerContext)) return false;
            if (a == typeof(BitContext) && b == typeof(StringContext)) return false;
            if (a == typeof(BitContext) && b == typeof(VariableContext)) return false;

            if (a == typeof(EntryContext) && b == typeof(IntegerContext)) return false;
            if (a == typeof(EntryContext) && b == typeof(LabelContext)) return false;
            if (a == typeof(EntryContext) && b == typeof(PointerContext)) return false;
            if (a == typeof(EntryContext) && b == typeof(StringContext)) return false;

            if (a == typeof(IntegerContext) && b == typeof(LabelContext)) return false;
            if (a == typeof(IntegerContext) && b == typeof(PointerContext)) return false;
            if (a == typeof(IntegerContext) && b == typeof(StringContext)) return false;

            if (a == typeof(LabelContext) && b == typeof(PointerContext)) return false;
            if (a == typeof(LabelContext) && b == typeof(StringContext)) return false;

            if (a == typeof(PointerContext) && b == typeof(StringContext)) return false;

            return true;

        }

    }
}