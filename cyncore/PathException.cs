using System;

namespace CloudSync.Core
{
    public class PathException : Exception
    {
        public PathException()
        {
        }

        public PathException(string message) : base(message)
        {
        }

        public PathException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
