namespace Foundation;

using System.Runtime.Serialization;

/// <summary>
/// Should be thrown when return value is not within the expected range.
/// </summary>
[Serializable]
public class ReturnValueOutOfRangeException : ReturnValueException
{
    public ReturnValueOutOfRangeException() : this("Return value was out of the range of valid values.")
    {
    }

    public ReturnValueOutOfRangeException(string? message) : base(message) { }
    public ReturnValueOutOfRangeException(string? message, Exception? inner) : base(message, inner) { }
    protected ReturnValueOutOfRangeException(SerializationInfo info, StreamingContext context) : base(info, context) { }
}

