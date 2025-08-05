namespace Syscode
{
    public class Reporter
    {
        private readonly ErrorFile file;
        private event EventHandler<DiagnosticEvent> diagnostics = delegate { };
        private List<DiagnosticEvent> messages = new();

        public List<DiagnosticEvent> Messages { get => messages.OrderBy(e => e.line).ToList(); set => messages = value; }

        public Reporter(ErrorFile erfile, EventHandler<DiagnosticEvent> diagnostics)
        {
            file = erfile;
            this.diagnostics = diagnostics;
        }
        public void Report(Report report)
        {
            Report(report.Node, report.Code, report.Args.ToArray());
        }
        public void Report(AstNode node, int number, params string[] args)
        {
            node.ReportedError = number;
            var errormsg = file.Errors.Where(e => e.Number == number).Single();
            string message = errormsg.Message;

            int argpos = 0;

            foreach (var arg in args )
            {
                var argid = $"{{arg{argpos}}}";
                message = message.Replace(argid, args[argpos]);
                argpos++;
            }

            messages.Add(new DiagnosticEvent(node, errormsg.Number, errormsg.Severity, message));
        }


        public void PrintReport()
        {
            Messages.ForEach(r =>  diagnostics(this, r));
        }

    }
}
