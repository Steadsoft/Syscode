using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syscode
{
    public class Reporter
    {
        private ErrorFile messages;
        private event EventHandler<DiagnosticEvent> diagnostics = delegate { };
        public Reporter(ErrorFile erfile, EventHandler<DiagnosticEvent> diagnostics)
        {
            messages = erfile;
            this.diagnostics = diagnostics;
        }
        public void Report(AstNode node, int number, params string[] args)
        {
            var errormsg = messages.Errors.Where(e => e.Number == number).Single();
            string message = errormsg.Message;

            int argpos = 0;

            foreach (var arg in args )
            {
                var argid = $"{{arg{argpos}}}";
                message = message.Replace(argid, args[argpos]);
                argpos++;
            }
            

            diagnostics(this, new DiagnosticEvent(node, errormsg.Number, errormsg.Severity, message));
        }

    }
}
