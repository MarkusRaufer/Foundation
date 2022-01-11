namespace Foundation;

using System.Runtime.Serialization;

[Serializable]
public class InvalidArgumentException : Exception
{
    public InvalidArgumentException()
    {
    }

    public InvalidArgumentException(string message) : base(message)
    {
    }

    public InvalidArgumentException(string message, Exception inner) : base(message, inner)
    {
    }

    protected InvalidArgumentException(
        SerializationInfo info,
        StreamingContext context) : base(info, context)
    {
    }
}

