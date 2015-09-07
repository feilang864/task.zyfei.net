using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FFLTask.UI.PC.CustomException
{
    [Serializable]
    public class UrlParseException : Exception
    {
        public UrlParseException() { }
        public UrlParseException(string message) : base(message) { }
        public UrlParseException(string message, Exception inner) : base(message, inner) { }
        protected UrlParseException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}