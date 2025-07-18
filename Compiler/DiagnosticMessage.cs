namespace Syscode
{
    public class DiagnosticMessage
    {
        private int number;
        private string message;
        private string severity;

        public int Number { get => number; set => number = value; }
        public string Message { get => message; set => message = value; }
        public string Severity { get => severity; set => severity = value; }
    }

    public class ErrorFile
    {
        private List<DiagnosticMessage> errors;

        public List<DiagnosticMessage> Errors { get => errors; set => errors = value; }
    }
}
