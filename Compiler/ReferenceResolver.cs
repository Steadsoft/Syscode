
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

        public void Resolve(IStatementContainer root)
        {
            var assignments = root.Statements.OfType<Assignment>();

            foreach ( var assignment in assignments )
            {
                if (assignment.Reference.reference == null)
                {
                    if (assignment.Reference.basic.qualifier.Any() == false)
                    {
                        if (root.Symbols.Where(s => s.Spelling == assignment.Reference.basic.Spelling).Any() == false)
                        {
                            diagnostics?.Invoke(this, new DiagnosticEvent(assignment, 1, Severity.Error, $"The reference to '{assignment.Reference.basic.Spelling}' cannot be resolved to a declaration in this or any containing block."));
                        }
                    }
                }
            }

            root.Statements.OfType<Procedure>().ForEach(s => Resolve(s));
            root.Statements.OfType<Scope>().ForEach(s => Resolve(s));
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
