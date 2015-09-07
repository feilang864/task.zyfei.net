using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FFLTask.UI.PC.CustomException
{
    [Serializable]
    public class NHTransactionException : Exception
    {
        public NHTransactionException() { }

        public NHTransactionException(string message) : base(message) { }
        
        public NHTransactionException(string message, Exception inner) : base(message, inner) { }
        
        protected NHTransactionException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}