namespace Foundation
{
    public record struct BinaryResult<T>(BinarySelection Selection, T Value);
}
