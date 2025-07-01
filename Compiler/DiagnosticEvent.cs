using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace Syscode
{
    public class DiagnosticEvent
    {
        public int code;
        public Severity severity;
        public string message;
        public int line;
        public int column;
        public DiagnosticEvent(AstNode node, int code, Severity severity, string message)
        {
            this.code = code;
            this.severity = severity;
            this.message = message;
            this.line = node.StartLine;
            this.column = node.StartColumn;
        }
    }

    public enum Severity
    {
        Information,
        Warning,
        Error,
        Fatal
    }
}
