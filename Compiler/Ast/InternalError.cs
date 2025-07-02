using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        protected InternalErrorException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}