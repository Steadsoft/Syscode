using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using static SyscodeParser;

namespace Syscode
{
    /// <summary>
    /// Transforms a Syscode Antlr4 CST into a Syscode AST
    /// </summary>
    public class AstBuilder
    {
        private IContainer currentContainer = null;
        public AstBuilder() 
        { 
        
        }

        public AstNode Generate(ParserRuleContext context)
        {
            return context switch
            {
                CompilationContext compilation => CreateCompilation(compilation),
                ScopeContext scope => CreateScope(scope),
                ProcedureContext procedure => CreateProcedure(procedure),
                FunctionContext function => CreateFunction(function),
                TypeContext type => CreateType(type),
                IfContext ifContext => CreateIf(ifContext),
                AssignmentContext assignment => CreateAssignment(assignment),
                DeclareContext declare => CreateDeclaration(declare),
                CallContext call => CreateCall(call),
                ReturnContext ret => CreateReturn(ret),
                LabelContext lab => CreateLabel(lab),
                GotoContext gto => CreateGoto(gto),
                LoopContext loop => CreateLoop(loop),
                _ => new AstNode(context)
            };
        }

        public Loop CreateLoop(LoopContext context)
        {
            if (context == null) throw new ArgumentNullException("context");

            if (context.Loop != null)
            {
                return new Loop(context)
                {
                    Statements = GetStatements(context.Loop).Select(s => Generate(s)).ToList()
                };
            }
            if (context.For != null)
            {
                return new For(context) 
                { 
                    forRef = CreateReference(context.For.For),
                    from = CreateExpression(context.For.From),
                    to = CreateExpression(context.For.To),
                    by = CreateExpression(context.For.By),
                    whileExp = CreateExpression(context.For.While?.Exp),
                    untilExp = CreateExpression(context.For.Until?.Exp),
                    Statements = GetStatements(context.For).Select(s => Generate(s)).ToList()
                };
            }

            if (context.While != null)
            {
                return new While(context)
                {
                   whileExp = CreateExpression(context.While.While.Exp),
                   untilExp = CreateExpression(context.While.Until?.Exp),
                   Statements = GetStatements(context.While).Select(s => Generate(s)).ToList()
                };
            }

            if (context.Until != null)
            {
                return new Until(context)
                {
                    untilExp = CreateExpression(context.Until.Until.Exp) ,
                    whileExp = CreateExpression(context.Until.While?.Exp),
                    Statements = GetStatements(context.Until).Select(s => Generate(s)).ToList()
                };
            }

            throw new InternalErrorException($"Unrecognized loop syntax on line {context.Start.Line}");
        }
        public Goto CreateGoto(GotoContext context)
        {
            return new Goto(context) { Target = CreateReference(context.GetExactNode<ReferenceContext>()) };
        }
        private Label CreateLabel(LabelContext context)
        {
            return new Label(context) { Spelling = context.Spelling.GetText(), Subscript = context.Subscript?.Literal.GetText() };
        }
        private Return CreateReturn(ReturnContext context)
        {
            if (context.TryGetDerivedNode<ExpressionContext>(out var expressionContext))
                return new Return(context) { Expression = CreateExpression(expressionContext) };

            return new Return(context);
        }
        private Call CreateCall(CallContext context)
        {
            return new Call(context) { Reference = CreateReference(context.GetExactNode<ReferenceContext>()) };
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
                            expr.Literal = new Literal(numcontext) { Value = txt };
                            expr.Type = ExpressionType.Literal;

                        }

                        if (prim.TryGetExactNode<StrLiteralContext>(out var strcontext))
                        {
                            var txt = strcontext.GetText();
                            expr.Literal = new Literal(strcontext) { Value = txt };
                            expr.Type = ExpressionType.Literal;

                        }


                        return expr;
                    }
                case ExprParenthesizedContext paren:
                    {
                        var parenctxt = paren.GetExactNode<ParenthesizedExpressionContext>();
                        var expression = parenctxt.GetDerivedNode<ExpressionContext>();
                        var result = CreateExpression(expression);
                        result.Parenthesized = true;
                        return result;
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
                        return expr; ;
                    }
                default:  // every other case always contains an operator and a left/right expression. 
                    {
                        throw new InvalidOperationException("Unexpected expression class encountered.");
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
            var refContext = context.GetExactNode<ReferenceContext>();
            var referenceTask = CreateReference(refContext);
            var exprContext = context.GetDerivedNode<ExpressionContext>();
            var expressionTask = CreateExpression(exprContext);

            return new Assignment(context) { Reference = referenceTask/* .Result */, Expression = expressionTask /* .Result */ };
        }
        private Reference CreateReference(ReferenceContext context)
        {
            Reference reference = new(context);

            // A Reference might contain another Reference...

            if (context.Ref != null)
            //if (context.TryGetExactNode<ReferenceContext>(out var inner))
            {
                reference.reference = CreateReference(context.Ref);
            }

            if (context.ArgsList != null) 
            //if (context.TryGetExactNode<ArgumentsListContext>(out var argslist))
            {
                var argumentsList = context.ArgsList._ArgsSet; /* one or more 'arguments' always present */

                foreach (var arguments in argumentsList)
                {
                    var argsast = new Arguments(arguments);

                    if (arguments.TryGetExactNode<SubscriptCommalistContext>(out var subscriptCommalist))
                    {
                        var expressions = subscriptCommalist.GetDerivedNodes<ExpressionContext>().Select(e => CreateExpression(e)).ToList();

                        argsast.ExpressionList.AddRange(expressions); ;
                    }

                    reference.ArgumentsList.Add(argsast);
                }
            }

            // TODO: process the optional ArgList list..

            reference.Basic = CreateBasicReference(context.Basic);

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

                        var expressions = subs.GetDerivedNodes<ExpressionContext>().Select(e => CreateExpression(e)).ToList();

                        qualifier.Arguments = new Arguments(subs) { ExpressionList = expressions };

                    }
                        
                    basic.qualifier.Add(qualifier);
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
            return new Compilation(context) { Statements = GetStatements(context).Select(s => Generate(s)).ToList() };
        }
        private Scope CreateScope(ScopeContext context)
        {
            if (context.TryGetExactNode<BlockScopeContext>(out var block))
            {
                return new Scope(block) { Spelling = block.GetExactNode<QualifiedNameContext>().GetText(), Statements = GetStatements(block).Select(s => Generate(s)).ToList() };
            }

            throw new InvalidOperationException();
        }
        private Declare CreateDeclaration(DeclareContext context)
        {
            var dcl = new Declare(context);

            if (context.Bounds !=  null)
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
                dcl.TypeName = context.GetLabelText(nameof(DeclareContext.Type));
            }

            var attribs = context.GetDerivedNodes<MemberAttributesContext>();

            foreach ( var attrib in attribs)
            {
                switch (attrib)
                {
                    case (AttribAlignedContext):
                        dcl.Attributes.Add(new Aligned(attrib));
                        break;
                    case (AttribUnalignedContext):
                        dcl.Attributes.Add(new Unaligned(attrib));
                        break;
                    case (AttribConstContext):
                        dcl.Attributes.Add(new Const(attrib));
                        break;
                    default:
                        throw new InvalidOperationException();
                }
            }

            if (context.Type != null)
            {
                if (context.Type.As != null)
                    dcl.As = context.Type.As.GetText();

                if (context.Type.Code != null)
                {
                    dcl.TypeName = context.Type.Code.GetText();
                }

                if (context.Type.Args != null)
                {
                    var expressions = context.Type.Args.GetExactNode<SubscriptCommalistContext>().GetDerivedNodes<ExpressionContext>().Select(e => CreateExpression(e)).ToList();
                    dcl.typeSubscripts = expressions;
                }
            }

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

            if (context.TryGetExactNode<DimensionSuffixContext>(out var dimensions))
            {
                bounds = CreateBounds(dimensions);
            }

            var structs = context.GetExactNodes<StructBodyContext>().Select(s => CreateStructure(s)).ToList();
            var fields = context.GetExactNodes<StructFieldContext>().Select(f => CreateField(f)).ToList(); ;

            return new StructBody(context) { Spelling = spelling, Bounds = bounds, Structs = structs, Fields = fields };
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

            var bounds = context.Pair._BoundPairs.Select(p => new BoundsPair(p) { Lower = CreateExpression(p.Lower), Upper = CreateExpression(p.Upper)}).ToList();

            return bounds;
        }
        private Procedure CreateProcedure(ProcedureContext context)
        {
            var node = new Procedure(context, currentContainer);

            currentContainer = node; 

            node.Spelling = context.Spelling.GetText();

            if (context.Params != null)
            {
                node.Parameters = context.Params._Params.Select(i => i.GetText()).ToList();
            }

            node.Statements = [.. GetStatements(context).Select(s => Generate(s))];

            currentContainer = node.Container;

            return node;
        }

        private Procedure CreateFunction(FunctionContext context)
        {
            var node = new Procedure(context, currentContainer); // a func is so similar to a proc we use same class to represent them.

            currentContainer = node;

            node.Spelling = context.Spelling.GetText(); 

            node.As = context.Type.GetText();

            if (context.TryGetExactNode<ParamListContext>(out var parameters))
            {
                node.Parameters = [.. parameters.GetExactNodes<IdentifierContext>().Select(i => i.GetText())];
            }

            node.Statements = [.. GetStatements(context).Select(s => Generate(s))];
            node.isFunction = true;

            currentContainer = node.Container;

            return node;
        }

        private Elif CreateElif(ExprThenBlockContext context)
        {
            return new Elif(context) { Expr = null, ThenStatements = GetStatements(context.GetExactNode<ThenBlockContext>()).Select(s => Generate(s)).ToList() };
        }
        private If CreateIf(IfContext context)
        {
            List<AstNode> else_stmts = new();
            List<Elif> elifs = new();

            var if_then_block = context.GetExactNode<ExprThenBlockContext>().GetExactNode<ThenBlockContext>();
            var if_then_stmts = GetStatements(if_then_block).Select(s => Generate(s)).ToList();

            if (context.TryGetExactNode<ElseBlockContext>(out var else_block))
            {
                var then_block = else_block.GetExactNode<ThenBlockContext>();
                else_stmts = GetStatements(then_block).Select(s => Generate(s)).ToList();
            }

            if (context.TryGetExactNode<ElifBlockContext>(out var elif_block))
            {
                var then_blocks = elif_block.GetExactNodes<ExprThenBlockContext>();
                elifs = then_blocks.Select(etb => CreateElif(etb)).ToList();
            }

            return new If(context) { ThenStatements = if_then_stmts, ElseStatements = else_stmts, ElifStatements = elifs };
        }
    }
}