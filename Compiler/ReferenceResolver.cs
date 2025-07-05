
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

        }

        public void ResolveReferences(IContainer root, IEnumerable<AstNode> statements)
        {
            statements.OfType<Assignment>().ForEach(a => ResolveAssignment(root, a));
            statements.OfType<Goto>().ForEach(a => ResolveGoto(root, a));

        }

        private void ResolveGoto(IContainer container, Goto statement)
        {
            if (statement.Target.IsntBasic || statement.Target.Basic.IsQualified)  // actually an error, but might be better reported in a distinct phase.
                return;

            if (DeclarationCount(container, statement.Target.Basic.Spelling, out var symbol) == 1)
            {
                statement.Target.Basic.Symbol = symbol;
                statement.Target.Basic.Resolved = true;  // TODO is the resolved declaration a label?
                return;
            }

            if (container is Procedure proc && proc.Container != null)
            {
                ResolveGoto(proc.Container, statement);
            }
        }

        private void ResolveAssignment(IContainer container, Assignment statement) 
        {
            // TODO: include qualified and pointer refs too evemtually

            var reference = statement.Reference;

            if (reference.IsntBasic || reference.Basic.IsQualified)
                return;

            if (DeclarationCount(container, reference.Basic.Spelling, out var symbol) == 1)
            {
                reference.Basic.Resolved = true;
                reference.Basic.Symbol = symbol;
                return;
            }

            if (container is Procedure proc && proc.Container != null)
            {
                ResolveAssignment(proc.Container, statement);
            }
        }

        public void ReportUnresolvedReferences(IContainer container)
        {
            if (container.HasNoStatements)
                return;

            foreach (var statement in container.Statements)
            {
                switch (statement)
                {
                    case Assignment assign:
                        {
                            if (assign.Reference.IsBasic && assign.Reference.Basic.IsQualified == false)
                            {
                                if (assign.Reference.Basic.Resolved == false)
                                {
                                    report(assign, 1000, assign.Reference.Basic.Spelling);
                                }
                            }

                            break;
                        }

                    case Goto gotostmt:
                        {
                            report(gotostmt, 1001, gotostmt.Target.Basic.Spelling);
                            break;
                        }
                    default:
                        break; // TODO: support other statement types
                }
            }

            container.Statements.OfType<Procedure>().ForEach(s => ReportUnresolvedReferences(s));
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
