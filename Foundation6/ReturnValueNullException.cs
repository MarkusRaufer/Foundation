namespace Foundation;

using System.Runtime.Serialization;

/// <summary>
/// Should be thrown when the expected return value must not be null.
/// </summary>
[Serializable]
public class ReturnValueNullException : ReturnValueException
{
    public ReturnValueNullException() : this("Return value cannot be null")
    {
    }
    public ReturnValueNullException(string? message) : base(message) { }
    public ReturnValueNullException(string? message, Exception? inner) : base(message, inner) { }
    protected ReturnValueNullException(SerializationInfo info, StreamingContext context) : base(info, context) { }
}

