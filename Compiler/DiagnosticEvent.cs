﻿namespace Syscode
{
    public class DiagnosticEvent
    {
        public int code;
        public string severity;
        public string message;
        public int line;
        public int column;
        public DiagnosticEvent(AstNode node, int code, string severity, string message)
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
