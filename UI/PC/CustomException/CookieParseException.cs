using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FFLTask.UI.PC.CustomException
{
    [Serializable]
    public class CookieParseException : Exception
    {
        public CookieParseException() { }

        public CookieParseException(string message) : base(message) { }
        
        public CookieParseException(string message, Exception inner) : base(message, inner) { }
        
        protected CookieParseException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}