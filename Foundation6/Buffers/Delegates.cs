namespace Foundation.Buffers;

public delegate ReadOnlySpan<T> TransformSpan<T>(ReadOnlySpan<T> span);