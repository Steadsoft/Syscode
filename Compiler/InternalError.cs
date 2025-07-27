namespace Syscode
{
    /// <summary>
    /// Represents a state that is considered impossible. 
    /// </summary>
    [Serializable]
    public class InternalErrorException : Exception
    {
        public InternalErrorException() { }

        public InternalErrorException(string message)
            : base(message) { }

        public InternalErrorException(string message, Exception inner)
            : base(message, inner) { }
    }
}