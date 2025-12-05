using System;

namespace namasdev.Core.Exceptions
{
    [Serializable]
    public class ExceptionFriendlyMessage : Exception
    {
        public ExceptionFriendlyMessage()
            : base() { }

        public ExceptionFriendlyMessage(string message)
            : this(message, null, null) { }

        public ExceptionFriendlyMessage(string message, string internalMessage)
            : this(message, internalMessage, null) { }

        public ExceptionFriendlyMessage(string message, Exception inner)
            : this(message, null, inner) { }

        public ExceptionFriendlyMessage(string message, string internalMessage, Exception inner)
            : base(message, inner)
        {
            InternalMessage = internalMessage;
        }

        protected ExceptionFriendlyMessage(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }

        public string InternalMessage { get; private set; }

        public bool MustLogError
        {
            get { return !String.IsNullOrWhiteSpace(InternalMessage); }
        }
    }
}
