using Antlr4.Runtime;
using Antlr4.Runtime.Sharpen;
using Antlr4.Runtime.Tree;
using Syscode.Ast;
using System.Runtime.Versioning;
using static Syscode.LexerHelper;
using static SyscodeParser;

namespace Syscode
{
    /// <summary>
    /// Transforms a Syscode Antlr4 CST into a Syscode AST
    /// </summary>
    public class AstBuilder
    {
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

                        argsast.ExpressionList.AddRange(expressions); ;
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

                        var expressions = subs.GetDerivedNodes<ExpressionContext>().Select(CreateExpression).ToList();

                        qualifier.Arguments = new Arguments(subs) { ExpressionList = expressions };

                    }

                    basic.Qualifier.Add(qualifier);
                }
            }

            return basic;
        }
        private List<ParserRuleContext> GetStatements(ParserRuleContext context)
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

            // Process the data attributes part of this declaration


            var dataAttributeCounters = new Dictionary<System.Type, (int Count, string Keyword)>
            {
                { typeof(PackedContext),    (0,KeywordText(PACKED)) },
                { typeof(VariableContext),  (0,KeywordText(VARIABLE)) },
                { typeof(AlignedContext),   (0,KeywordText(ALIGNED)) },
                { typeof(LabelContext),     (0,KeywordText(LABEL)) },
                { typeof(BitContext),       (0,KeywordText(BIT)) },
                { typeof(PointerContext),   (0,KeywordText(POINTER)) },
                { typeof(IntegerContext),   (0,$"{KeywordText(BIN)} or {KeywordText(UBIN)}") },
                { typeof(EntryContext),     (0,KeywordText(ENTRY)) },
                { typeof(StringContext),    (0,KeywordText(STRING)) },
                { typeof(AsContext),        (0,KeywordText(AS)) }
            };

            context._DataAttributes.ForEach(d => dataAttributeCounters[d.GetType()] = (dataAttributeCounters[d.GetType()].Count + 1, dataAttributeCounters[d.GetType()].Keyword));


            if (dataAttributeCounters.Values.Any(x => x.Count > 1))
            {
                dataAttributeCounters.Values.Where(v => v.Count > 1).ForEach(v => reporter.Report(dcl, 1029, dcl.Spelling, v.Keyword));
                return dcl;
            }


            #region Report inconsistent attributes
            if ((dataAttributeCounters[typeof(VariableContext)].Count == 1) && (dataAttributeCounters[typeof(StringContext)].Count == 0 && dataAttributeCounters[typeof(EntryContext)].Count == 0))
            {
                reporter.Report(dcl, 1025, dcl.Spelling);
                return dcl;
            }

            if ((dataAttributeCounters[typeof(LabelContext)].Count + dataAttributeCounters[typeof(BitContext)].Count + dataAttributeCounters[typeof(PointerContext)].Count + dataAttributeCounters[typeof(IntegerContext)].Count + dataAttributeCounters[typeof(EntryContext)].Count + dataAttributeCounters[typeof(StringContext)].Count + dataAttributeCounters[typeof(AsContext)].Count) > 1)
            {
                reporter.Report(dcl, 1024, dcl.Spelling);
                return dcl;
            }
            #endregion
            #region Label
            if (dataAttributeCounters[typeof(LabelContext)].Count == 1)
            {
                var attr = context._DataAttributes.OfType<LabelContext>().Single();
                dcl.CoreType = DataType.LABEL;
            }
            #endregion
            #region Bit
            if (dataAttributeCounters[typeof(BitContext)].Count == 1)
            {
                var attr = context._DataAttributes.OfType<BitContext>().Single();
                dcl.CoreType = DataType.BIT;
            }
            #endregion
            #region Pointer
            if (dataAttributeCounters[typeof(PointerContext)].Count == 1)
            {
                var attr = context._DataAttributes.OfType<PointerContext>().Single();
                dcl.CoreType = DataType.POINTER;
            }
            #endregion
            #region Binary
            if (dataAttributeCounters[typeof(IntegerContext)].Count == 1)
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
                        reporter.Report(dcl, 1027,"1","64");
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

            }
            #endregion
            #region Entry
            if (dataAttributeCounters[typeof(EntryContext)].Count == 1)
            {
                var attr = context._DataAttributes.OfType<EntryContext>().Single();
                dcl.CoreType = DataType.ENTRY;
            }
            #endregion
            #region String
            if (dataAttributeCounters[typeof(StringContext)].Count == 1)
            {
                var attr = context._DataAttributes.OfType<StringContext>().Single();
                dcl.CoreType = DataType.STRING;
            }
            #endregion
            #region As
            if (dataAttributeCounters[typeof(AsContext)].Count == 1)
            {
                var attr = context._DataAttributes.OfType<AsContext>().Single();
                dcl.CoreType = DataType.AS;
            }
            #endregion

            // Process remaining non-data attributes

            #region Count attributes
            var attributeCounters = new Dictionary<System.Type, (int Count, string Keyword)>
            {
                { typeof(ConstContext),     (0,KeywordText(CONST)) },
                { typeof(OffsetContext),    (0,KeywordText(OFFSET)) },
                { typeof(ExternalContext),  (0,KeywordText(EXTERNAL)) },
                { typeof(InternalContext),  (0,KeywordText(INTERNAL)) },
                { typeof(StaticContext),    (0,KeywordText(STATIC)) },
                { typeof(BasedContext),     (0,KeywordText(BASED)) },
                { typeof(StackContext),     (0,KeywordText(STACK)) },
                { typeof(InitContext),      (0,KeywordText(INIT)) },
                { typeof(BuiltinContext),   (0,KeywordText(BUILTIN)) },
                { typeof(PadContext),       (0,KeywordText(PAD)) }
            };

            context._Attributes.ForEach(d => attributeCounters[d.GetType()] = (attributeCounters[d.GetType()].Count + 1, attributeCounters[d.GetType()].Keyword));

            #endregion
            #region Report duplications

            if (attributeCounters.Values.Any(x => x.Count > 1))
            {
                attributeCounters.Values.Where(v => v.Count > 1).ForEach(v => reporter.Report(dcl, 1029, dcl.Spelling, v.Keyword));
                return dcl;
            }
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
                else_stmts = GetStatements(context.Else.Then).Select(Generate).ToList();
            }

            if (context.Elif != null)  // at least one 'elif' is present
            {
                elifs = context.Elif._ExprThen.Select(CreateElif).ToList();
            }

            return new If(context) { ThenStatements = if_then_stmts, ElseStatements = else_stmts, ElifStatements = elifs, Condition = condition };
        }
    }
}