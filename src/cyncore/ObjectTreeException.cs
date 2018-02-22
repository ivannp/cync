using System;

namespace CloudSync.Core
{
    public class ObjectTreeException : Exception
    {
        public ObjectTreeException()
        {
        }

        public ObjectTreeException(string message) : base(message)
        {
        }

        public ObjectTreeException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}