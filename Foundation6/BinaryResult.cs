namespace Foundation
{
    public record struct BinaryResult<T>(BinarySelectionValue Selection, T Value);
}
