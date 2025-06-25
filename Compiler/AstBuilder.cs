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
                StructContext structure => CreateStructure(structure),
                IfContext ifContext => CreateIf(ifContext),
                AssignmentContext assignment => CreateAssignment(assignment),
                DeclareContext declare => new Dcl(declare),
                CallContext call => CreateCall(call),
                ReturnContext ret => CreateReturn(ret),
                _ => new AstNode(context)
            };
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
                        var expression = parenctxt.GetExactNode<ExpressionContext>();
                        var result = CreateExpression(expression);
                        result.Type = ExpressionType.Parenthesized;
                        return result;
                    }
                case ExprPrefixedContext prefixed:
                    {
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
        private Operator GetOperator(ExprBinaryContext context)
        {
            var operation = context.children.Where(c => c is not ExpressionContext).Cast<ParserRuleContext>().Single();
            var terminal = (TerminalNodeImpl)operation.children.Where(c => c is TerminalNodeImpl).Single();

            return (Operator)(terminal.Symbol.Type);
        }
        private Assignment CreateAssignment(AssignmentContext context)
        {
            var refContext = context.GetExactNode<ReferenceContext>();
            var reference = CreateReference(refContext);
            var exprContext = context.GetDerivedNode<ExpressionContext>();
            var expression = CreateExpression(exprContext);

            return new Assignment(context) { Referenece = reference, Expression = expression };
        }
        private Reference CreateReference(ReferenceContext context)
        {
            Reference reference = new(context);

            // A Reference might contain another Reference...

            if (context.TryGetExactNode<ReferenceContext>(out var inner))
            {
                reference.reference = CreateReference(inner);
            }

            if (context.TryGetExactNode<ArgumentsListContext>(out var argslist))
            {
                var argumentsList = argslist.GetExactNodes<ArgumentsContext>().ToList(); /* one or more 'arguments' always present */

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

            reference.basic = CreateBasicReference(context.GetExactNode<BasicReferenceContext>());

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

                        basic.qualifier.Add(qualifier);
                    }

                }
                //basic.qualifier = qualification.GetExactNodes<StructureQualificationContext>().Select(q => new Qualification(q)).ToList();
                //var argumentsList = qualification.GetExactNodes<ArgumentsContext>().ToList(); /* one or more 'arguments' always present */
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
        private Structure CreateStructure(StructContext context)
        {
            return CreateStructure(context.GetExactNode<StructDefinitionContext>());
        }
        private Structure CreateStructure(StructDefinitionContext context)
        {
            var elements = new List<StructureMember>();
            var bounds = new List<BoundsPair>();
            var struct_name = context.GetExactNode<StructNameContext>();
            var spelling = struct_name.GetLabelText(nameof(StructNameContext.Spelling));

            if (context.TryGetExactNode<DimensionSuffixContext>(out var dimensions))
            {
                bounds = CreateBounds(dimensions);
            }

            var members = context.GetExactNode<StructMembersContext>();
            var fields = members.GetExactNodes<StructMemberContext>().SelectMany(m => m.GetExactNodes<StructFieldContext>()).Select(d => CreateField(d)).ToList();
            var structs = members.GetExactNodes<StructMemberContext>().SelectMany(m => m.GetExactNodes<StructDefinitionContext>()).Select(s => CreateStructure(s)).ToList();

            elements.AddRange(fields);
            elements.AddRange(structs);

            return new Structure(context) { Spelling = spelling, Bounds = bounds, Members = elements };
        }
        private Field CreateField(StructFieldContext context)
        {
            var bounds = new List<BoundsPair>();

            if (context.TryGetExactNode<SyscodeParser.DimensionSuffixContext>(out var dimensions))
            {
                bounds = CreateBounds(dimensions);
            }

            return new Field(context) { Bounds = bounds };
        }
        private List<BoundsPair> CreateBounds(DimensionSuffixContext context)
        {
            var bounds = new List<BoundsPair>();
            var commalist = context.GetExactNode<BoundPairCommalistContext>(); ;
            var pairs = commalist.GetExactNodes<BoundPairContext>();

            var lower = pairs.Select(p => p.GetExactNode<LowerBoundContext>().GetExactNode<ExpressionContext>());
            var upper = pairs.Select(p => p.GetExactNode<UpperBoundContext>().GetExactNode<ExpressionContext>());

            bounds = pairs.Select(p => new BoundsPair(p) { Lower = null /* lower */ , Upper = null /* upper */}).ToList();

            return bounds;
        }
        private Procedure CreateProcedure(ProcedureContext context)
        {
            var node = new Procedure(context);

            node.Spelling = context.GetLabelText(nameof(ProcedureContext.Spelling));

            if (context.TryGetExactNode<ParamListContext>(out var parameters))
            {
                node.Parameters = [.. parameters.GetExactNodes<IdentifierContext>().Select(i => i.GetText())];
            }

            node.Statements = [.. GetStatements(context).Select(s => Generate(s))];

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