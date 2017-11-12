using System;
using System.Runtime.Serialization;

namespace HD
{
  [Serializable]
  internal class ServerNotAvailableException : Exception
  {
    public ServerNotAvailableException()
    {
    }

    public ServerNotAvailableException(string message) : base(message)
    {
    }

    public ServerNotAvailableException(string message, Exception innerException) : base(message, innerException)
    {
    }

    protected ServerNotAvailableException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
  }
}