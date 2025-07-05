
using System.Text;
using System.Xml.Linq;

namespace Syscode
{
    public class ReferenceResolver
    {
        private event EventHandler<DiagnosticEvent>? diagnostics;

        public ReferenceResolver(EventHandler<DiagnosticEvent>? diagnostics)
        {
            this.diagnostics = diagnostics;
        }

        public void ResolveReferences(IStatementContainer root)
        {
            ResolveReferences(root, root.Statements);

            root.Statements.OfType<Procedure>().ForEach(s => ResolveReferences(s));

        }

        public void ResolveReferences(IStatementContainer root, IEnumerable<AstNode> statements)
        {
            statements.OfType<Assignment>().ForEach(a => ResolveAssignment(root, a));

            statements.OfType<Goto>().ForEach(a => ResolveGoto(root, a));

        }

        private void ResolveGoto(IStatementContainer container, Goto statement)
        {
            if (statement.Target.IsntBasic || statement.Target.Basic.IsQualified)  // actually an error, but might be better reported in a distinct phase.
                return;

            if (DeclarationCount(container, statement.Target.Basic.Spelling) == 1)
            {
                statement.Target.Basic.Resolved = true;  // TODO is the resolved declaration a label?
                return;
            }

            if (container is Procedure proc && proc.Container != null)
            {
                ResolveGoto(proc.Container, statement);
            }

        }

        private void ResolveAssignment(IStatementContainer container, Assignment statement) 
        {
            // TODO: include qualified and pointer refs too evemtually

            var reference = statement.Reference;

            if (reference.IsntBasic || reference.Basic.IsQualified)
                return;

            if (DeclarationCount(container, reference.Basic.Spelling) == 1)
            {
                reference.Basic.Resolved = true;
                return;
            }

            if (container is Procedure proc && proc.Container != null)
            {
                ResolveAssignment(proc.Container, statement);
            }
        }

        public void ReportUnresolvedReferences(IStatementContainer container)
        {
            if (container.Empty)
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
                                    Report(assign, $"The assignment target '{assign.Reference.Basic.Spelling}' could not be resolved to a declaration in this or any containing scope.");
                                }
                            }

                            break;
                        }

                    case Goto gotostmt:
                        {
                            Report(gotostmt, $"The goto target '{gotostmt.Target.Basic.Spelling}' could not be resolved to a label in this or any containing scope.");
                            break;
                        }
                    default:
                        break; // TODO: support other statement types
                }
            }

            container.Statements.OfType<Procedure>().ForEach(s => ReportUnresolvedReferences(s));
        }

        public static int DeclarationCount(IStatementContainer root, string Spelling)
        {
            return root.Symbols.Where(s => s.Spelling == Spelling).Count();
        }

        public void Report(AstNode node, string Message)
        {
            diagnostics?.Invoke(this, new DiagnosticEvent(node, 1, Severity.Error, Message));
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
