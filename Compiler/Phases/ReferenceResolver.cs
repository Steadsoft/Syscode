namespace Syscode
{
    public class ReferenceResolver
    {
        private Reporter reporter;

        public ReferenceResolver(Reporter Reporter)
        {
            reporter = Reporter;
        }

        public void ResolveContainedReferences(IContainer root)
        {
            if (root is Procedure)
            {
                if (root.Statements.OfType<Scope>().Any())
                {
                    foreach (var scope in root.Statements.OfType<Scope>())
                    {
                        reporter.Report(scope, 1013, "package"); // TODO: rename scope stuff to package
                    }
                }
            }

            if (root is Scope)
            {
                if (root.Statements.Any(s => s is Assignment || s is Goto || s is If || s is Loop || s is Call || s is Return)) 
                {
                    foreach (var stmt in root.Statements.Where(s => s is Assignment || s is Goto || s is If || s is Loop || s is Call || s is Return))
                    {
                        reporter.Report(stmt, 1014, stmt.GetType().Name.ToLower());
                    }
                }
            }


            ResolveStatementReferences(root, root.Statements);

            root.Statements.OfType<Procedure>().ForEach(s => ResolveContainedReferences(s));
            root.Statements.OfType<Scope>().ForEach(s => ResolveContainedReferences(s));

        }
        private void ResolveStatementReferences(IContainer root, IEnumerable<AstNode> statements)
        {
            statements.OfType<Assignment>().ForEach(a => ResolveAssignment(root, a));
            statements.OfType<Goto>().ForEach(a => ResolveGoto(root, a));
            statements.OfType<If>().ForEach(a => ResolveIf(root, a));
            statements.OfType<Loop>().ForEach(l => ResolveLoop(root, l));
            statements.OfType<Call>().ForEach(c => ResolveCall(root, c));
            statements.OfType<Return>().ForEach(r => ResolveReturn(root, r));
        }
        private void ResolveQualifiedReference(Reference reference, Symbol symbol)
        {
            // every Reference contains a single BasicReference and that might or might not be qualified.

            if (symbol.IsntStructure)
                return; // no way can this be resolved!

            var quals = reference.BasicReference.Qualifier.Select(q => q.Spelling).ToList();
            var count = quals.Count;
            var curr_struct = symbol.StructBody;

            for (int X = 1; X < count; X++)
            {
                if (curr_struct.Structs.Where(s => s.Spelling == quals[X]).Any())
                {
                    curr_struct = curr_struct.Structs.Where(s => s.Spelling == quals[X]).Single();
                }
                else
                {
                    // we defer reporting this so that when the errors are later reported they are reported in
                    // line number order. 
                    reference.Report = new Report(reference, 1011, quals[X], quals[X - 1]);
                    return;
                }
            }

            // we now have the innermost qualified struct, does that struct contain the referenced name?

            if (curr_struct.Fields.Where(f => f.Spelling == reference.BasicReference.Spelling).Any())
            {
                reference.IsResolved = true;
                reference.BasicReference.Symbol = symbol;
                return;
            }

            if (curr_struct.Structs.Where(s => s.Spelling == reference.BasicReference.Spelling).Any())
            {
                reference.IsResolved = true;
                reference.BasicReference.Symbol = symbol;
                return;
            }
        }
        private void ResolveReference(IContainer container, Reference reference)
        {
            if (reference.IsntJustBasicReference)
                ResolveReference(container, reference.Pointer);

            if (reference.ArgumentsList.Any())
            {
                // Please refer to grammar to understand how this is structured.

                foreach (Arguments args in reference.ArgumentsList)
                {
                    foreach (Expression e in args.ExpressionList)
                    {
                        ResolveExpression(container, e);
                    }
                }
            }

            if (reference.BasicReference.IsQualified)
            {
                foreach (var qualification in reference.BasicReference.Qualifier)
                {
                    if (qualification.Arguments != null)
                    {
                        foreach (var expr in qualification.Arguments.ExpressionList)
                        {
                            ResolveExpression(container, expr);
                        }
                    }
                }

                if (DeclarationCount(container, reference.BasicReference.Qualifier.First().Spelling, out var sym) == 1)
                {
                    // if not a struct, then exit, because the reference is to a non-struct and can't be qualied, illegal code...

                    if (sym.IsntStructure)
                        return; // no way this can be resolved.

                    ResolveQualifiedReference(reference, sym);
                }
                else
                {
                    // struct name itself not resolved.
                    reference.Report = new Report(reference,1012,reference.BasicReference.Qualifier.First().Spelling);
                    return;
                }
            }
            else
            {
                if (DeclarationCount(container, reference.BasicReference.Spelling, out var symbol) == 1)
                {
                    reference.IsResolved = true;
                    reference.BasicReference.Symbol = symbol;
                    return;
                }
            }

            if (container is Procedure proc && proc.Container != null)
            {
                ResolveReference(proc.Container, reference);
            }
        }
        private void ResolveExpression(IContainer container, Expression expression)
        {
            switch (expression.Type)
            {
                case ExpressionType.Literal:
                    {
                        break;
                    }
                case ExpressionType.Primitive:
                    {
                        ResolveReference(container, expression.Reference);
                        break;
                    }
                case ExpressionType.Prefix:
                    {
                        ResolveExpression(container, expression.Right);
                        break;
                    }
                case ExpressionType.Binary:
                    {
                        ResolveExpression(container, expression.Left);
                        ResolveExpression(container, expression.Right);
                        break;
                    }

            }
        }
        private void ResolveLoop (IContainer container, Loop loop)
        {
            ResolveStatementReferences (container, loop.Statements);
        }
        private void ResolveIf(IContainer container, If ifstmt)
        {
            ResolveExpression(container, ifstmt.Expr);
            ResolveStatementReferences(container, ifstmt.ThenStatements);
            ResolveStatementReferences(container, ifstmt.ElseStatements);
            ResolveStatementReferences(container, ifstmt.ElifStatements.SelectMany(elif => elif.ThenStatements));
            ifstmt.ElifStatements.Select(elif => elif.Expr).ForEach(exp => ResolveExpression (container, exp));
        }
        private void ResolveGoto(IContainer container, Goto statement)
        {
            ResolveReference(container, statement.Reference);
        }
        private void ResolveCall(IContainer container, Call statement)
        {
            ResolveReference(container, statement.Reference);
        }
        private void ResolveReturn(IContainer container, Return statement)
        {
            if (statement.Expression != null) 
                ResolveExpression(container, statement.Expression);
        }
        private void ResolveAssignment(IContainer container, Assignment statement) 
        {
            ResolveReference(container, statement.Reference);
            ResolveExpression(container, statement.Expression);
        }
        public void ReportUnresolvedReference(AstNode node, Reference reference)
        {
            if (reference.ArgumentsList.Any())
            {
                foreach (Arguments args in reference.ArgumentsList)
                {
                    foreach (Expression e in args.ExpressionList)
                    {
                        ReportUnresolvedReferences(node, e);
                    }
                }
            }

            if (reference.BasicReference.IsKeyword)
            {
                if (!(reference.IsResolved &&
                      reference.IsJustBasicReference &&
                      reference.BasicReference.Symbol.CoreType == DataType.BUILTIN))
                {
                    reporter.Report(node, 1015, reference.BasicReference.Spelling); // do not warn about references to keywords when the reference is to a builtin function with the same name as a keyword
                }
            }

            if (reference.BasicReference.IsQualified)
            {
                if (reference.Report != null)
                {
                    // this is a qualification error, report this and be done.
                    reporter.Report(reference.Report);
                }
                if (reference.IsntResolved)
                {
                    reporter.Report(node, 1010, reference.BasicReference.Spelling);
                }

                if (reference.BasicReference.Qualifier != null)
                {
                    foreach (var q in reference.BasicReference.Qualifier)
                    {
                        if (q.Arguments != null)
                        {
                            foreach (var e in q.Arguments.ExpressionList)
                            {
                                ReportUnresolvedReferences(node, e);
                            }
                        }
                    }
                }
            }

            if (reference.BasicReference.IsntQualified)
            {
                if (reference.IsntResolved)
                {
                    reporter.Report(node, 1000, reference.BasicReference.Spelling);
                }
            }

            if (reference.IsntJustBasicReference)
                ReportUnresolvedReference(node, reference.Pointer);

        }
        public void ReportUnresolvedReferences(AstNode node, Expression expression)
        {
            if (expression.Type == ExpressionType.Primitive)
            {
                ReportUnresolvedReference(node, expression.Reference);
            }

            if (expression.Type == ExpressionType.Binary)
            {
                if (expression.Left.Type == ExpressionType.Primitive)
                    ReportUnresolvedReference(node, expression.Left.Reference);
                if (expression.Right.Type == ExpressionType.Primitive)
                    ReportUnresolvedReference(node, expression.Right.Reference);
            }

        }
        public void ReportUnresolvedReferences(IEnumerable<AstNode> statements)
        {
            if (statements.Any() == false)
                return;

            foreach (var statement in statements)
            {
                switch (statement)
                {
                    case Assignment assign: // TODO expand this, for now we're just looking at simple unqualified assignment targets.
                        {
                            ReportUnresolvedReference(assign,assign.Reference);
                            ReportUnresolvedReferences(assign, assign.Expression);
                            break;
                        }
                    case Loop loop: // this statements contains other statements
                        {
                            ReportUnresolvedReferences(loop.Statements);
                            break;
                        }
                    case Goto gotostmt:
                        {
                            ReportUnresolvedReference(gotostmt, gotostmt.Reference);
                            break;
                        }
                    case If ifstmt: // this statements contains other statements
                        {
                            ReportUnresolvedReferences(ifstmt, ifstmt.Expr);
                            ReportUnresolvedReferences(ifstmt.ThenStatements);
                            ReportUnresolvedReferences(ifstmt.ElseStatements);
                            ReportUnresolvedReferences(ifstmt.ElifStatements.SelectMany(elif => elif.ThenStatements));
                            break;
                        }
                    case Call call:
                        {
                            ReportUnresolvedReference(call, call.Reference);
                            break;
                        }
                    case Return ret:
                        {
                            if (ret.Expression != null)
                               ReportUnresolvedReferences(ret, ret.Expression); 
                            break;
                        }
                    default:
                        break; // TODO: support other statement types
                }
            }

            statements.OfType<Procedure>().ForEach(s => ReportUnresolvedReferences(s.Statements));
            statements.OfType<Scope>().ForEach(s => ReportUnresolvedReferences(s.Statements));
        }
        private static int DeclarationCount(IContainer root, string Spelling, out Symbol symbol)
        {
            symbol = null;
            
            var count = root.Symbols.Where(s => s.Spelling == Spelling).Count();

            if (count == 1)
                symbol = root.Symbols.Where(s => s.Spelling == Spelling).Single();

            return count;
        }
    }
    public static class CompilerExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (var item in source)
                action(item);
        }
    }
}
