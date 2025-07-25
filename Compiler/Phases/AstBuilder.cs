using Antlr4.Runtime;
using Antlr4.Runtime.Sharpen;
using Antlr4.Runtime.Tree;
using Syscode.Ast;
using System.Runtime.Versioning;
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
        public AstBuilder(Dictionary<string, IConstant> constants, Reporter reporter)
        {
            this.constants = constants;
            this.reporter = reporter;
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
            int packed = 0;
            int vars = 0;
            int aligns = 0;
            int labels = 0;
            int pointers = 0;
            int integers = 0;
            int entries = 0;
            int bits = 0;
            int strings = 0;
            int ases = 0;

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
            #region Count data attributes
            foreach (var datr in context._Data)
            {
                switch (datr)
                {
                    case PackedContext: packed++; break;
                    case VariableContext: vars++; break;
                    case AlignedContext: aligns++; break;
                    case LabelContext: labels++; break;
                    case BitContext: bits++; break;
                    case PointerContext: pointers++; break;
                    case IntegerContext: integers++; break;
                    case EntryContext: entries++; break;
                    case StringContext: strings++; break;
                    case AsContext: ases++; break;
                }
            }
            #endregion
            #region Report duplications
            bool anyGreaterThanOne = new[] { packed, vars, aligns, labels, bits, pointers, integers, entries, strings, ases }.Any(x => x > 1);

            if (anyGreaterThanOne)
            {
                if (packed > 1)
                    reporter.Report(dcl, 1024, dcl.Spelling, "packed");

                if (vars > 1)
                    reporter.Report(dcl, 1024, dcl.Spelling, "var");

                if (aligns > 1)
                    reporter.Report(dcl, 1024, dcl.Spelling, "aligned");

                if (labels > 1)
                    reporter.Report(dcl, 1024, dcl.Spelling, "label");

                if (bits > 1)
                    reporter.Report(dcl, 1024, dcl.Spelling, "bit");

                if (pointers > 1)
                    reporter.Report(dcl, 1024, dcl.Spelling, "pointer");

                if (integers > 1)
                    reporter.Report(dcl, 1024, dcl.Spelling, "bin or ubin");

                if (entries > 1)
                    reporter.Report(dcl, 1024, dcl.Spelling, "entry");

                if (strings > 1)
                    reporter.Report(dcl, 1024, dcl.Spelling, "string");

                if (ases > 1)
                    reporter.Report(dcl, 1024, dcl.Spelling, "as");

                return dcl;
            }
            #endregion
            #region Report inconsistent attributes
            if ((vars == 1) && (strings == 0 && entries == 0))
            {
                reporter.Report(dcl, 1025, dcl.Spelling);
                return dcl;
            }

            if ((labels + bits + pointers + integers + entries + strings + ases) > 1)
            {
                reporter.Report(dcl, 1024, dcl.Spelling);
                return dcl;
            }
            #endregion
            #region Label
            if (labels == 1)
            {
                var attr = context._Data.OfType<LabelContext>().Single();
                dcl.CoreType = DataType.LABEL;
            }
            #endregion
            #region Bit
            if (bits == 1)
            {
                var attr = context._Data.OfType<BitContext>().Single();
                dcl.CoreType = DataType.BIT;
            }
            #endregion
            #region Pointer
            if (pointers == 1)
            {
                var attr = context._Data.OfType<PointerContext>().Single();
                dcl.CoreType = DataType.POINTER;
            }
            #endregion
            #region Binary
            if (integers == 1)
            {
                int precision = 0;
                int scale = 0;

                dcl.CoreType = DataType.BIN;

                var attr = context._Data.OfType<IntegerContext>().Single();

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

                dcl.Validated = true;

                return dcl;
            }
            #endregion
            #region Entry
            if (entries == 1)
            {
                var attr = context._Data.OfType<EntryContext>().Single();
                dcl.CoreType = DataType.ENTRY;
            }
            #endregion
            #region String
            if (strings == 1)
            {
                var attr = context._Data.OfType<StringContext>().Single();
                dcl.CoreType = DataType.STRING;
            }
            #endregion
            #region As
            if (ases == 1)
            {
                var attr = context._Data.OfType<AsContext>().Single();
                dcl.CoreType = DataType.AS;
            }
            #endregion

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