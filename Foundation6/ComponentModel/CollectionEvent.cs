namespace Foundation.ComponentModel;

public record struct CollectionEvent<T>(CollectionActionState State, T? Element);

