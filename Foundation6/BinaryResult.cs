namespace Foundation
{
    public record struct BinaryResult<T>(BinarySelectionValue BinaryDecision, T Value);
}
