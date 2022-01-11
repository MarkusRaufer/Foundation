namespace Foundation;

using System.Runtime.Serialization;

/// <summary>
/// Should be thrown on unexpected return values.
/// </summary>
[Serializable]
public class ReturnValueException : Exception
{
    public ReturnValueException() { }
    public ReturnValueException(string? message) : base(message) { }
    public ReturnValueException(string? message, Exception? inner) : base(message, inner) { }
    protected ReturnValueException(SerializationInfo info, StreamingContext context) : base(info, context) { }
}

