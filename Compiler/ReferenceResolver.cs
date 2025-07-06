
using System.ComponentModel;
using System.Text;
using System.Xml.Linq;

namespace Syscode
{
    public class ReferenceResolver
    {
        private Action<AstNode, int, string> report;

        public ReferenceResolver(Action<AstNode, int, string> Reporter)
        {
            report = Reporter;
        }


        public void ResolveReferences(IContainer root)
        {
            ResolveReferences(root, root.Statements);

            root.Statements.OfType<Procedure>().ForEach(s => ResolveReferences(s));
            root.Statements.OfType<Scope>().ForEach(s => ResolveReferences(s));

        }

        private void ResolveReferences(IContainer root, IEnumerable<AstNode> statements)
        {
            statements.OfType<Assignment>().ForEach(a => ResolveAssignment(root, a));
            statements.OfType<Goto>().ForEach(a => ResolveGoto(root, a));
            statements.OfType<If>().ForEach(a => ResolveIf(root, a));
            statements.OfType<Loop>().ForEach(l => ResolveLoop(root, l));
        }

        private void ResolveReference(IContainer container, Reference reference)
        {
            if (reference.IsntBasic)// || reference.Basic.IsQualified)
                return;

            if (reference.Basic.IsQualified)
            {
                if (DeclarationCount(container, reference.Basic.Qualifier.First().Spelling, out var sym) == 1)
                {
                    reference.Basic.IsResolved = true;
                    reference.Basic.Symbol = sym;
                }
            }
            else
            {
                if (DeclarationCount(container, reference.Basic.Spelling, out var symbol) == 1)
                {
                    reference.Basic.IsResolved = true;
                    reference.Basic.Symbol = symbol;
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

        public void ResolveLoop (IContainer container, Loop loop)
        {
            ResolveReferences (container, loop.Statements);
        }

        private void ResolveIf(IContainer container, If ifstmt)
        {
            ResolveReferences(container, ifstmt.ThenStatements);
            ResolveReferences(container, ifstmt.ElseStatements);
            ResolveReferences(container, ifstmt.ElifStatements.SelectMany(elif => elif.ThenStatements));
        }

        private void ResolveGoto(IContainer container, Goto statement)
        {
            ResolveReference(container, statement.Target);
        }

        private void ResolveAssignment(IContainer container, Assignment statement) 
        {
            ResolveReference(container, statement.Reference);
            ResolveExpression(container, statement.Expression);
        }

        public void ReportUnresolvedReference(AstNode node, Reference reference)
        {
            if (reference.IsBasic && reference.Basic.IsQualified)
            {
                if (reference.Basic.IsntResolved)
                {
                    report(node, 1010, reference.Basic.ToString());
                }
            }

            if (reference.IsBasic && reference.Basic.IsntQualified)
            {
                if (reference.Basic.IsntResolved)
                {
                    report(node, 1000, reference.Basic.Spelling);
                }
            }

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
                            ReportUnresolvedReference(gotostmt, gotostmt.Target);
                            break;
                        }
                    case If ifstmt: // this statements contains other statements
                        {
                            ReportUnresolvedReferences(ifstmt.ThenStatements);
                            ReportUnresolvedReferences(ifstmt.ElseStatements);
                            ReportUnresolvedReferences(ifstmt.ElifStatements.SelectMany(elif => elif.ThenStatements));
                            break;
                        }
                    default:
                        break; // TODO: support other statement types
                }
            }

            statements.OfType<Procedure>().ForEach(s => ReportUnresolvedReferences(s.Statements));
            statements.OfType<Scope>().ForEach(s => ReportUnresolvedReferences(s.Statements));
        }

        public static int DeclarationCount(IContainer root, string Spelling, out Symbol symbol)
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
